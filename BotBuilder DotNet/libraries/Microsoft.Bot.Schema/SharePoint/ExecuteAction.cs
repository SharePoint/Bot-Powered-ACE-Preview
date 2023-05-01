// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Microsoft.Bot.Schema.SharePoint
{
    /// <summary>
    /// Action.Execute
    /// </summary>
    public class ExecuteAction: Action
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteAction"/> class.
        /// </summary>
        public ExecuteAction()
        {
            // Do nothing
        }
    
        /// <summary>
        /// Gets or Sets the verb associated with this action of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the verb associated with the action</value>
        [JsonProperty(PropertyName = "verb")]
        public string Verb { get; set; }
    }
}
