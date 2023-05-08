// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Bot.Schema.SharePoint
{
    /// <summary>
    /// Set property pane configuration Response.
    /// </summary>
    public class SetPropertyPaneConfigurationResponse 
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="SetPropertyPaneConfigurationResponse"/> class.
        /// </summary>
        public SetPropertyPaneConfigurationResponse()
        {
            // Do nothing
        }

        /// <summary>
        /// This enum contains the different types of responses possible after setting properties.
        /// </summary>
        public enum ResponseTypeOption
        {
            /// <summary>
            /// CardView
            /// </summary>
            [EnumMember(Value = "Card")]
            CardView,

            /// <summary>
            /// NoOp
            /// </summary>
            [EnumMember(Value = "NoOp")]
            NoOp
        }

        /// <summary>
        /// Gets or Sets ViewType for return set property pane configuration view.
        /// </summary>
        /// <value>This value is the view type of the set property pane configuration response.</value>
        [JsonProperty(PropertyName = "responseType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseTypeOption ResponseType { get; set; }

        /// <summary>
        /// Gets or Sets the render arguments.
        /// </summary>
        /// <value>This value is the render arguments of the set property pane configuration response.</value>
        [JsonProperty(PropertyName = "renderArguments")]
        public ISharePointViewResponse RenderArguments { get; set; }
    }
}
