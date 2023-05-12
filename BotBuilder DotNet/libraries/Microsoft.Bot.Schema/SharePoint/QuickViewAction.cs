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
    /// SharePoint show location action
    /// </summary>
    public class QuickViewAction: IAction, IOnCardSelectionAction
    {
        private string type = "QuickView";
        /// <summary>
        /// Initializes a new instance of the <see cref="QuickViewAction"/> class.
        /// </summary>
        public QuickViewAction()
        {
            // Do nothing
        }
        
        /// <summary>
        /// Gets or Sets the action parameters of type <see cref="QuickViewActionParameters"/>.
        /// </summary>
        /// <value>This value is the parameters of the action.</value>
        [JsonProperty(PropertyName = "parameters")]
        public QuickViewActionParameters Parameters { get; set; }
    }
}
