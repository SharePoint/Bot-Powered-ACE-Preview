// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Bot.Schema.SharePoint
{
    /// <summary>
    /// Handle Action Response.
    /// </summary>
    /// <typeparam name="T">The first generic type parameter.</typeparam>
    public class HandleActionResponse<T>
        where T : SharePointViewResponse
    {
        /// <summary>
        /// Gets or Sets ViewType for return handle action view.
        /// </summary>
        /// <value>This value is the view type of the handle action response.</value>
        [JsonProperty(PropertyName = "responseType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public HandleActionResponseType ResponseType { get; set; }

        /// <summary>
        /// Gets or Sets the render arguments.
        /// </summary>
        /// <value>This value is the render arguments of the handle action response.</value>
        [JsonProperty(PropertyName = "renderArguments")]
        public T RenderArguments { get; set; }
    }
}
