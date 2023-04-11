﻿// Licensed under the MIT License.
// Copyright (c) Microsoft Corporation. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using AdaptiveExpressions;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Recognizers;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Dialogs.Adaptive.Conditions
{
    /// <summary>
    /// Actions triggered when an Intent of "ChooseIntent" has been emitted by a recognizer.
    /// </summary>
    /// <remarks>
    /// This trigger is run when the utterance has triggered ambiguity between intents from multiple recognizers in a CrossTrainedRecognizerSet.
    /// </remarks>
    public class OnChooseIntent : OnIntent
    {
        /// <summary>
        /// Class identifier.
        /// </summary>
        [JsonProperty("$kind")]
        public new const string Kind = "Microsoft.OnChooseIntent";

        /// <summary>
        /// Initializes a new instance of the <see cref="OnChooseIntent"/> class.
        /// </summary>
        /// <param name="actions">Optional, actions to add to the plan when the rule constraints are met.</param>
        /// <param name="condition">Optional, condition which needs to be met for the actions to be executed.</param>
        /// <param name="callerPath">Optional, source file full path.</param>
        /// <param name="callerLine">Optional, line number in source file.</param>
        [JsonConstructor]
        public OnChooseIntent(List<Dialog> actions = null, string condition = null, [CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
            : base(CrossTrainedRecognizerSet.ChooseIntent, actions: actions, condition: condition, callerPath: callerPath, callerLine: callerLine)
        {
        }

        /// <summary>
        /// Gets or sets the list of intent names that must be in the chooseIntent to match.
        /// </summary>
        /// <value>List of intent names that must be in the chooseIntent to match.</value>
        [JsonProperty("intents")]
#pragma warning disable CA2227 // Collection properties should be read only
        public List<string> Intents { get; set; } = new List<string>();
#pragma warning restore CA2227 // Collection properties should be read only

        /// <inheritdoc/>
        protected override Expression CreateExpression()
        {
            // add constraints for the intents property if set
            if (this.Intents?.Any() == true)
            {
                var constraints = this.Intents.Select(subIntent => Expression.Parse($"contains(jPath({TurnPath.Recognized}, '$.candidates[*].intent'), '{subIntent}')"));
                return Expression.AndExpression(base.CreateExpression(), Expression.AndExpression(constraints.ToArray()));
            }

            return base.CreateExpression();
        }
    }
}
