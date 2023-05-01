// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Microsoft.Bot.Schema.SharePoint
{
    /// <summary>
    /// SharePoint base card parameters.
    /// </summary>
    public class BaseCardParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseCardParameters"/> class.
        /// </summary>
        public BaseCardParameters()
        {
            // Do nothing
        }

        /// <summary>
        /// Gets or Sets the icon property of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the url of the icon to display</value>
        [JsonProperty(PropertyName = "iconProperty")]
        public string IconProperty { get; set; }

        /// <summary>
        /// Gets or Sets the icon alt text of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the  icon alt text</value>
        [JsonProperty(PropertyName = "iconAltText")]
        public string IconAltText { get; set; }

        /// <summary>
        /// Gets or Sets the title of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the title to display</value>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }
}
