﻿// Licensed under the MIT License.
// Copyright (c) Microsoft Corporation. All rights reserved.

using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveExpressions.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Microsoft.Bot.Builder.Dialogs.Adaptive.Actions
{
    /// <summary>
    /// Lets you modify an array in memory.
    /// </summary>
    public class EditArray : Dialog
    {
        /// <summary>
        /// Class identifier.
        /// </summary>
        [JsonProperty("$kind")]
        public const string Kind = "Microsoft.EditArray";

        /// <summary>
        /// Initializes a new instance of the <see cref="EditArray"/> class.
        /// </summary>
        /// <param name="changeType">change type.</param>
        /// <param name="arrayProperty">array property (optional).</param>
        /// <param name="value">value to insert.</param>
        /// <param name="resultProperty">output property to put Pop/Take into.</param>
        public EditArray(ArrayChangeType changeType, string arrayProperty = null, object value = null, string resultProperty = null)
            : base()
        {
            this.ChangeType = new EnumExpression<ArrayChangeType>(changeType);

            if (!string.IsNullOrEmpty(arrayProperty))
            {
                this.ItemsProperty = arrayProperty;
            }

            switch (changeType)
            {
                case ArrayChangeType.Clear:
                case ArrayChangeType.Pop:
                case ArrayChangeType.Take:
                    if (ResultProperty != null)
                    {
                        this.ResultProperty = resultProperty;
                    }

                    break;
                case ArrayChangeType.Push:
                case ArrayChangeType.Remove:
                    if (value != null)
                    {
                        this.Value = new ValueExpression(value);
                    }

                    break;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EditArray"/> class.
        /// </summary>
        /// <param name="callerPath">Optional, source file full path.</param>
        /// <param name="callerLine">Optional, line number in source file.</param>
        [JsonConstructor]
        public EditArray([CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
            : base()
        {
            this.RegisterSourceLocation(callerPath, callerLine);
        }

        /// <summary>
        /// Possibles array change actions type.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter), /*camelCase*/ true)]
        public enum ArrayChangeType
        {
            /// <summary>
            /// Push item onto the end of the array
            /// </summary>
            Push,

            /// <summary>
            /// Pop the item off the end of the array
            /// </summary>
            Pop,

            /// <summary>
            /// Take an item from the front of the array
            /// </summary>
            Take,

            /// <summary>
            /// Remove the item from the array, regardless of it's location. Searches starting at the front of the array and stops when the first matching item is found.
            /// </summary>
            Remove,

            /// <summary>
            /// Clear the contents of the array
            /// </summary>
            Clear
        }

        /// <summary>
        /// Gets or sets type of change being applied.
        /// </summary>
        /// <value>
        /// Type of change being applied.
        /// </value>
        [JsonProperty("changeType")]
        public EnumExpression<ArrayChangeType> ChangeType { get; set; } = new EnumExpression<ArrayChangeType>(default(ArrayChangeType));

        /// <summary>
        /// Gets or sets an optional expression which if is true will disable this action.
        /// </summary>
        /// <example>
        /// "user.age > 18".
        /// </example>
        /// <value>
        /// A boolean expression. 
        /// </value>
        [JsonProperty("disabled")]
        public BoolExpression Disabled { get; set; }

        /// <summary>
        /// Gets or sets property path expression to the collection of items.
        /// </summary>
        /// <value>
        /// Property path expression to the collection of items.
        /// </value>
        [JsonProperty("itemsProperty")]
        public StringExpression ItemsProperty { get; set; }

        /// <summary>
        /// Gets or sets the path expression to store the result of the action.
        /// </summary>
        /// <value>
        /// The path expression to store the result of the action.
        /// </value>
        [JsonProperty("resultProperty")]
        public StringExpression ResultProperty { get; set; }

        /// <summary>
        /// Gets or sets the expression of the value to put onto the array.
        /// </summary>
        /// <value>
        /// The expression of the value to put onto the array.
        /// </value>
        [JsonProperty("value")]
        public ValueExpression Value { get; set; } = new ValueExpression();

        /// <summary>
        /// Called when the dialog is started and pushed onto the dialog stack.
        /// </summary>
        /// <param name="dc">The <see cref="DialogContext"/> for the current turn of conversation.</param>
        /// <param name="options">Optional, initial information to pass to the dialog.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (options is CancellationToken)
            {
                throw new ArgumentException($"{nameof(options)} cannot be a cancellation token");
            }

            if (Disabled != null && Disabled.GetValue(dc.State))
            {
                return await dc.EndDialogAsync(cancellationToken: cancellationToken).ConfigureAwait(false);
            }

            var changeType = ChangeType.GetValue(dc.State);

            if (this.ItemsProperty == null)
            {
                throw new InvalidOperationException($"EditArray: \"{changeType}\" operation couldn't be performed because the {nameof(ItemsProperty)} wasn't specified.");
            }

            var property = this.ItemsProperty.GetValue(dc.State);
            var array = dc.State.GetValue<JArray>(property, () => new JArray());

            object item = null;
            object result = null;

            switch (changeType)
            {
                case ArrayChangeType.Pop:
                    item = array[array.Count - 1];
                    array.RemoveAt(array.Count - 1);
                    result = item;
                    break;

                case ArrayChangeType.Push:
                    EnsureValue();
                    var (itemResult, error) = this.Value.TryGetValue(dc.State);
                    if (error == null && itemResult != null)
                    {
                        array.Add(itemResult);
                    }

                    break;

                case ArrayChangeType.Take:
                    if (array.Count == 0)
                    {
                        break;
                    }

                    item = array[0];
                    array.RemoveAt(0);
                    result = item;
                    break;

                case ArrayChangeType.Remove:
                    EnsureValue();
                    (itemResult, error) = this.Value.TryGetValue(dc.State);
                    if (error == null && itemResult != null)
                    {
                        result = false;
                        for (var i = 0; i < array.Count; ++i)
                        {
                            if (array[i].ToString() == itemResult.ToString() || JToken.DeepEquals(array[i], JToken.FromObject(itemResult)))
                            {
                                result = true;
                                array.RemoveAt(i);
                                break;
                            }
                        }
                    }

                    break;

                case ArrayChangeType.Clear:
                    result = array.Count > 0;
                    array.Clear();
                    break;
            }

            dc.State.SetValue(property, array);

            if (ResultProperty != null)
            {
                dc.State.SetValue(this.ResultProperty.GetValue(dc.State), result);
            }

            return await dc.EndDialogAsync(result, cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        protected override string OnComputeId()
        {
            return $"{GetType().Name}[{ChangeType?.ToString() + ": " + ItemsProperty?.ToString()}]";
        }

        private void EnsureValue()
        {
            if (Value == null)
            {
                throw new InvalidOperationException($"EditArray: \"{ChangeType}\" operation couldn't be performed for array \"{ItemsProperty}\" because a value wasn't specified.");
            }
        }
    }
}
