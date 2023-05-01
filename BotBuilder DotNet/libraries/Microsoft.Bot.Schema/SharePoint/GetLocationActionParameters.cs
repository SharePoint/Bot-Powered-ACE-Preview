// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Microsoft.Bot.Schema.SharePoint
{
    /// <summary>
    /// SharePoint parameters for a Get Location action.
    /// </summary>
    public class GetLocationActionParameters: ICardActionParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLocationActionParameters"/> class.
        /// </summary>
        public GetLocationActionParameters()
        {
            // Do nothing
        }

        /// <summary>
        /// Gets or Sets the choose location on map of type <see cref="bool"/>.
        /// </summary>
        /// <value>This value indicates whether a location on the map can be chosen.</value>
        [JsonProperty(PropertyName = "ChooseLocationOnMap")]
        public bool ChooseLocationOnMap { get; set; }
    }
}
