// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Microsoft.Bot.Schema.SharePoint
{
    /// <summary>
    /// SharePoint image card parameters.
    /// </summary>
    public class ImageCardParameters : BaseCardParameters, ICardParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCardParameters"/> class.
        /// </summary>
        public ImageCardParameters()
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
        /// Gets or Sets the image url of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the image url of the card view.</value>
        [JsonProperty(PropertyName = "imageUrl")]
        public string ImageUrl { get; set; }

        /// <summary>
        /// Gets or Sets the image alt text of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the image alt text of the card view.</value>
        [JsonProperty(PropertyName = "imageAltText")]
        public string ImageAltText { get; set; }
    }
}
