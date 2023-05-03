// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Microsoft.Bot.Schema.SharePoint
{
    /// <summary>
    /// Action.Submit
    /// </summary>
    public class SubmitAction: Action
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubmitAction"/> class.
        /// </summary>
        public SubmitAction()
        {
            // Do nothing
        }

        /// <summary>
        /// Gets or Sets the action parameters of type <see cref="IDictionary<string, Object>"/>.
        /// </summary>
        /// <value>This value is the parameters of the action.</value>
        [JsonProperty(PropertyName = "parameters")]
        public IDictionary<string, Object> Parameters { get; set; }
    
        /// <summary>
        /// Gets or Sets confirmation dialog associated with this action of type <see cref="ConfirmationDialog"/>.
        /// </summary>
        /// <value>This value is the confirmation dialog associated with this action</value>
        [JsonProperty(PropertyName = "confirmationDialog")]
        public ConfirmationDialog ConfirmationDialog { get; set; }
    }
}
