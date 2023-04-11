﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveExpressions;
using AdaptiveExpressions.Properties;
using Microsoft.Recognizers.Text.DateTime;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Dialogs.Adaptive.Input
{
    /// <summary>
    /// Input dialog to collect a datetime from the user.
    /// </summary>
    /// <remarks>
    /// The value that is output from a DateTimeInput is an array of DateTimeResolutions, or the output of OutputFormat.</remarks>
    public class DateTimeInput : InputDialog
    {
        /// <summary>
        /// Class identifier.
        /// </summary>
        [JsonProperty("$kind")]
        public const string Kind = "Microsoft.DateTimeInput";

        /// <summary>
        /// Initializes a new instance of the <see cref="DateTimeInput"/> class.
        /// </summary>
        /// <param name="callerPath">Optional, source file full path.</param>
        /// <param name="callerLine">Optional, line number in source file.</param>
        public DateTimeInput([CallerFilePath] string callerPath = "", [CallerLineNumber] int callerLine = 0)
        {
            this.RegisterSourceLocation(callerPath, callerLine);
        }

        /// <summary>
        /// Gets or sets the DefaultLocale to use to parse confirmation choices if there is not one passed by the caller.
        /// </summary>
        /// <value>
        /// string or expression which evaluates to a string with locale.
        /// </value>
        [JsonProperty("defaultLocale")]
        public StringExpression DefaultLocale { get; set; } = null;

        /// <summary>
        /// Gets or sets the expression to use to format the result.
        /// </summary>
        /// <remarks>The default output is an array of DateTimeResolutions, this property is an expression which is evaluated to determine the output of the dialog.</remarks>
        /// <value>an expression.</value>
        [JsonProperty("outputFormat")]
        public Expression OutputFormat { get; set; }

        /// <summary>
        /// Called when input has been received.
        /// </summary>
        /// <param name="dc">The <see cref="DialogContext"/> for the current turn of conversation.</param>
        /// <param name="cancellationToken">Optional, the <see cref="CancellationToken"/> that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>InputState which reflects whether input was recognized as valid or not.</returns>
        protected override Task<InputState> OnRecognizeInputAsync(DialogContext dc, CancellationToken cancellationToken = default(CancellationToken))
        {
            var input = dc.State.GetValue<object>(VALUE_PROPERTY);
            var culture = GetCulture(dc);
            var refTime = dc.Context.Activity.LocalTimestamp?.LocalDateTime;
            var results = DateTimeRecognizer.RecognizeDateTime(input.ToString(), culture, refTime: refTime);
            if (results.Count > 0)
            {
                // Return list of resolutions from first match
                var result = new List<DateTimeResolution>();
                var values = (List<Dictionary<string, string>>)results[0].Resolution["values"];
                foreach (var value in values)
                {
                    result.Add(ReadResolution(value));
                }

                dc.State.SetValue(VALUE_PROPERTY, result);

                if (OutputFormat != null)
                {
                    var (outputValue, error) = this.OutputFormat.TryEvaluate(dc.State);
                    if (error == null)
                    {
                        dc.State.SetValue(VALUE_PROPERTY, outputValue);
                    }
                    else
                    {
                        throw new InvalidOperationException($"OutputFormat Expression evaluation resulted in an error. Expression: {this.OutputFormat}. Error: {error}");
                    }
                }
            }
            else
            {
                return Task.FromResult(InputState.Unrecognized);
            }

            return Task.FromResult(InputState.Valid);
        }

        private DateTimeResolution ReadResolution(IDictionary<string, string> resolution)
        {
            var result = new DateTimeResolution();

            if (resolution.TryGetValue("timex", out var timex))
            {
                result.Timex = timex;
            }

            if (resolution.TryGetValue("value", out var value))
            {
                result.Value = value;
            }

            if (resolution.TryGetValue("start", out var start))
            {
                result.Start = start;
            }

            if (resolution.TryGetValue("end", out var end))
            {
                result.End = end;
            }

            return result;
        }

        private string GetCulture(DialogContext dc)
        {
            // Note: Default locale will be considered for deprecation as part of 4.13.
            return dc.GetLocale() ?? this.DefaultLocale?.GetValue(dc.State);
        }
    }
}
