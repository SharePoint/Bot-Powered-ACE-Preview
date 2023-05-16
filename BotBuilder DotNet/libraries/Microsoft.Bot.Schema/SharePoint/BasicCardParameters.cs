// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Microsoft.Bot.Schema.SharePoint
{
    /// <summary>
    /// SharePoint basic card parameters.
    /// </summary>
    public class BasicCardParameters : BaseCardParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicCardParameters"/> class.
        /// </summary>
        public BasicCardParameters()
        {
            // Do nothing
        }

        /// <summary>
        /// Gets or Sets the primary text of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the primary text to display.</value>
        [JsonProperty(PropertyName = "primaryText")]
        public string PrimaryText { get; set; }
    }
}
