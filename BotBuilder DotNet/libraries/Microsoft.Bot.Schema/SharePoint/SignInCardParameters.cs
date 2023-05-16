// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Microsoft.Bot.Schema.SharePoint
{
    /// <summary>
    /// SharePoint sign in card parameters.
    /// </summary>
    public class SignInCardParameters : BaseCardParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SignInCardParameters"/> class.
        /// </summary>
        public SignInCardParameters()
        {
            // Do nothing
        }

        /// <summary>
        /// Gets or Sets the primary text of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the primary text to display.</value>
        [JsonProperty(PropertyName = "primaryText")]
        public string PrimaryText { get; set; }

        /// <summary>
        /// Gets or Sets the description of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the description of the card view.</value>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets the sign in button text of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the sign in button text of the card view.</value>
        [JsonProperty(PropertyName = "signInButtonText")]
        public string SignInButtonText { get; set; }
    }
}
