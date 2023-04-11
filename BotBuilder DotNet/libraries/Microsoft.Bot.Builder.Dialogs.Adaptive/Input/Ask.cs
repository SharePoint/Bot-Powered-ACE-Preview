﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveExpressions.Properties;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Templates;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Dialogs.Adaptive.Actions
{
    /// <summary>
    /// Ask for an open-ended response.
    /// </summary>
    /// <remarks>
    /// This sends an activity and then terminates the turn with <see cref="DialogTurnStatus.CompleteAndWait"/>.
    /// The next activity from the user will then be handled by the parent adaptive dialog.
    /// 
    /// It also builds in a model of the properties that are expected in response through <see cref="DialogPath.ExpectedProperties"/>.
    /// <see cref="DialogPath.Retries"/> is updated as the same question is asked multiple times.
    /// </remarks>
    public class Ask : SendActivity
    {
        /// <summary>
        /// Class identifier.
        /// </summary>
        [JsonProperty("$kind")]
        public new const string Kind = "Microsoft.Ask";

        /// <summary>
        /// Initializes a new instance of the <see cref="Ask"/> class.
        /// </summary>
        /// <param name="text">Optional, text value.</param>
        /// <param name="expectedProperties">Optional, expected properties values.</param>
        /// <param name="callerPath">Optional, source file full path.</param>
        /// <param name="callerLine">Optional, line number in source file.</param>
        [JsonConstructor]
        public Ask(
            string text = null,
            ArrayExpression<string> expectedProperties = null,
            [CallerFilePath] string callerPath = "",
            [CallerLineNumber] int callerLine = 0)
        : base(text, callerPath, callerLine)
        {
            this.Activity = new ActivityTemplate(text ?? string.Empty);
            this.ExpectedProperties = expectedProperties;
        }

        /// <summary>
        /// Gets or sets properties expected to be filled by response.
        /// </summary>
        /// <value>
        /// String array or expression which evaluates to string array.
        /// </value>
        [JsonProperty("expectedProperties")]
        public ArrayExpression<string> ExpectedProperties { get; set; }

        /// <summary>
        /// Gets or sets the default operation that will be used when no operation is recognized.
        /// </summary>
        /// <remarks>
        /// When this Ask is executed, the defaultOperation will define the operation to use to assign an 
        /// identified entity to a property if there is no operation entity recognized in the input.
        /// </remarks>
        /// <value>String or expression evaluates to a string.</value>
        [JsonProperty("defaultOperation")]
        public StringExpression DefaultOperation { get; set; }

        /// <summary>
        /// Called when the dialog is started and pushed onto the dialog stack.
        /// </summary>
        /// <param name="dc">The <see cref="DialogContext"/> for the current turn of conversation.</param>
        /// <param name="options">Optional, initial information to pass to the dialog.</param>
        /// <param name="cancellationToken">Optional, a <see cref="CancellationToken"/> that can be used by other objects
        /// or threads to receive notice of cancellation.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public override async Task<DialogTurnResult> BeginDialogAsync(DialogContext dc, object options = null, CancellationToken cancellationToken = default)
        {
            //get number of retries from memory
            if (!dc.State.TryGetValue(DialogPath.Retries, out int retries))
            {
                retries = 0;
            }

            dc.State.TryGetValue(TurnPath.DialogEvent, out DialogEvent trigger);

            var expected = this.ExpectedProperties?.GetValue(dc.State);
            if (expected != null
                && dc.State.TryGetValue(DialogPath.ExpectedProperties, out List<string> lastExpectedProperties)
                && !expected.Any(prop => !lastExpectedProperties.Contains(prop))
                && !lastExpectedProperties.Any(prop => !expected.Contains(prop))
                && dc.State.TryGetValue(DialogPath.LastTriggerEvent, out DialogEvent lastTrigger)
                && lastTrigger.Name.Equals(trigger.Name, StringComparison.Ordinal))
            {
                retries++;
            }
            else
            {
                retries = 0;
            }

            dc.State.SetValue(DialogPath.Retries, retries);
            dc.State.SetValue(DialogPath.LastTriggerEvent, trigger);
            dc.State.SetValue(DialogPath.ExpectedProperties, expected);
            var result = await base.BeginDialogAsync(dc, options, cancellationToken).ConfigureAwait(false);
            result.Status = DialogTurnStatus.CompleteAndWait;
            return result;
        }
    }
}
