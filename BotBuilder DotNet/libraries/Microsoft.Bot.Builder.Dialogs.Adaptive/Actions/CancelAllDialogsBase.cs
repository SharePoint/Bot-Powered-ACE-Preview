﻿// Licensed under the MIT License.
// Copyright (c) Microsoft Corporation. All rights reserved.

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveExpressions.Properties;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Dialogs.Adaptive.Actions
{
    /// <summary>
    /// Base class for CancelAllDialogs api.
    /// </summary>
    public class CancelAllDialogsBase : Dialog
    {
        private bool cancelAll;

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelAllDialogsBase"/> class.
        /// </summary>
        /// <param name="cancelAll">Set to <c>true</c> to cancel all dialogs; <c>false</c> otherwise.</param>
        [JsonConstructor]
        public CancelAllDialogsBase(bool cancelAll)
        {
            this.cancelAll = cancelAll;
        }

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
        /// Gets or sets a value indicating whether to have the calling dialog should process the activity.
        /// </summary>
        /// <value>The default for this will be true, which means the calling dialog should not look at the activity.  You can set this to false to dispatch the activity to the parent dialog.</value>
        [DefaultValue(true)]
        [JsonProperty("activityProcessed")]
        public BoolExpression ActivityProcessed { get; set; }

        /// <summary>
        /// Gets or sets event name. 
        /// </summary>
        /// <value>
        /// Event name. 
        /// </value>
        [JsonProperty("eventName")]
        public StringExpression EventName { get; set; }

        /// <summary>
        /// Gets or sets value expression for EventValue.
        /// </summary>
        /// <value>
        /// Value expression for EventValue.
        /// </value>
        [JsonProperty("eventValue")]
        public ValueExpression EventValue { get; set; }

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

            var eventName = this.EventName?.GetValue(dc.State);
            var eventValue = this.EventValue?.GetValue(dc.State);

            if (this.ActivityProcessed != null && this.ActivityProcessed.GetValue(dc.State) == false)
            {
                // mark that this hasn't been recognized
                dc.State.SetValue(TurnPath.ActivityProcessed, false);
                
                // emit ActivityReceived, which will tell parent it's supposed to handle recognition.
                eventName = DialogEvents.ActivityReceived;
                eventValue = dc.Context.Activity;
            }

            if (dc.Parent == null)
            {
                return await dc.CancelAllDialogsAsync(cancelAll, eventName, eventValue, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                var turnResult = await dc.Parent.CancelAllDialogsAsync(cancelAll, eventName, eventValue, cancellationToken).ConfigureAwait(false);
                turnResult.ParentEnded = true;
                return turnResult;
            }
        }
    }
}
