// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Microsoft.Bot.Schema.SharePoint
{
    /// <summary>
    /// SharePoint parameters for a select media action.
    /// </summary>
    public class SelectMediaActionParameters: ICardActionParameters, IOnCardSelectionActionParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectMediaActionParameters"/> class.
        /// </summary>
        public SelectMediaActionParameters()
        {
            // Do nothing
        }

        public enum MediaType
        {
            /// <summary>
            /// Image
            /// </summary>
            Image = 1,

            /// <summary>
            /// Audio
            /// </summary>
            Audio = 4, 

            /// <summary>
            /// Document
            /// </summary>
            Document = 8
        }

        /// <summary>
        /// Gets or Sets type of media to be selected of type <see cref="MediaType"/>.
        /// </summary>
        /// <value>This value is the type of media to be selected.</value>
        [JsonProperty(PropertyName = "mediaType")]
        public MediaType MediaType { get; set; }

        /// <summary>
        /// Gets or Sets the allow multiple capture property of type <see cref="bool"/>.
        /// </summary>
        /// <value>This value indicates whether multiple files can be selected.</value>
        [JsonProperty(PropertyName = "allowMultipleCapture")]
        public bool AllowMultipleCapture { get; set; }

        /// <summary>
        /// Gets or Sets the max size per file selected of type <see cref="int"/>.
        /// </summary>
        /// <value>This value is the max size per file selected.</value>
        [JsonProperty(PropertyName = "maxSizePerFile")]
        public int MaxSizePerFile { get; set; }

        /// <summary>
        /// Gets or Sets the supported file formats of select media action of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the supported file formats of select media action.</value>
        [JsonProperty(PropertyName = "supportedFileFormats")]
        public IEnumerable<string> SupportedFileFormats { get; set; }
    }
}
