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
    /// Handle Action Response.
    /// </summary>
    public class HandleActionResponse 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HandleActionResponse"/> class.
        /// </summary>
        public HandleActionResponse()
        {
            // Do nothing
        }

        /// <summary>
        /// This enum contains the different types of responses possible after handling an action.
        /// </summary>
        public enum ResponseTypeOption
        {
            /// <summary>
            /// CardView
            /// </summary>
            [EnumMember(Value = "Card")]
            CardView,

            /// <summary>
            /// QuickView
            /// </summary>
            [EnumMember(Value = "QuickView")]
            QuickView,

            /// <summary>
            /// NoOp
            /// </summary>
            [EnumMember(Value = "NoOp")]
            NoOp
        }

        /// <summary>
        /// Gets or Sets ViewType for return handle action view.
        /// </summary>
        /// <value>This value is the view type of the handle action response.</value>
        [JsonProperty(PropertyName = "responseType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ResponseTypeOption ResponseType { get; set; }

        /// <summary>
        /// Gets or Sets the render arguments.
        /// </summary>
        /// <value>This value is the render arguments of the handle action response.</value>
        [JsonProperty(PropertyName = "renderArguments")]
        public ISharePointViewResponse RenderArguments { get; set; }
    }
}
