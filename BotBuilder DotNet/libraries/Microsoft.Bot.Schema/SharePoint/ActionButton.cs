// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Microsoft.Bot.Schema.SharePoint
{
    /// <summary>
    /// SharePoint action button.
    /// </summary>
    public class ActionButton
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionButton"/> class.
        /// </summary>
        public ActionButton()
        {
            // Do nothing
        }

        /// <summary>
        /// This enum contains the different types of action styles available in the SPFx framework.
        /// </summary>
        public enum ActionStyle
        {
            /// <summary>
            /// Default
            /// </summary>
            [EnumMember(Value = "default")]
            Default,

            /// <summary>
            /// Positive
            /// </summary>
            [EnumMember(Value = "positive")]
            Positive,

            /// <summary>
            /// Destructive
            /// </summary>
            [EnumMember(Value = "destructive")]
            Destructive
        }

        /// <summary>
        /// Gets or Sets the title of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the title of the action button.</value>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets the action of type <see cref="SharepointAction"/>.
        /// </summary>
        /// <value>This value is the action of the action button.</value>
        [JsonProperty(PropertyName = "action")]
        public SharepointAction Action { get; set; }

        /// <summary>
        /// Gets or Sets the id of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the id of the action button.</value>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets the style of type <see cref="ActionStyle"/>.
        /// </summary>
        /// <value>This value is the style of the action button.</value>
        [JsonProperty(PropertyName = "style")]
        public ActionStyle Style { get; set; }
    }
}
