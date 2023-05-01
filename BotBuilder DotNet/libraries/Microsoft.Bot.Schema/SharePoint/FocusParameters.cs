// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Microsoft.Bot.Schema.SharePoint
{
    /// <summary>
    /// SharePoint focus parameters.
    /// </summary>
    public class FocusParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FocusParameters"/> class.
        /// </summary>
        public FocusParameters()
        {
            // Do nothing
        }

        public enum AriaLive
        {
            /// <summary>
            /// Polite
            /// </summary>
            Polite = "polite",

            /// <summary>
            /// Assertive
            /// </summary>
            Assertive = "assertive",

            /// <summary>
            /// Off
            /// </summary>
            Off = "off"
        }


        /// <summary>
        /// Gets or Sets the focus target of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the focus target.</value>
        [JsonProperty(PropertyName = "focusTarget")]
        public string FocusTarget { get; set; }

        /// <summary>
        /// Gets or Sets the aria live property of type <see cref="AriaLive"/>.
        /// </summary>
        /// <value>This value sets the accessibility reading of the contents within the focus target.</value>
        [JsonProperty(PropertyName = "ariaLive")]
        public AriaLive AriaLive { get; set; }
    }
}
