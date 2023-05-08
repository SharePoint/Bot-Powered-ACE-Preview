// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Microsoft.Bot.Schema.SharePoint
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SharepointAction"/> class.
    /// </summary>
    public class SharepointAction
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SharepointAction"/> class.
        /// </summary>
        public SharepointAction()
        {
            // Do nothing
        }

        /// <summary>
        /// This enum contains the different types of actions available in the SPFx framework.
        /// </summary>
        public enum ActionType
        {
            /// <summary>
            /// QuickView
            /// </summary>
            QuickView,

            /// <summary>
            /// Submit
            /// </summary>
            Submit, 

            /// <summary>
            /// ExternalLink
            /// </summary>
            ExternalLink,

            /// <summary>
            /// SelectMedia
            /// </summary>
            [EnumMember(Value = "VivaAction.SelectMedia")]
            SelectMedia, 

            /// <summary>
            /// GetLocation
            /// </summary>
            [EnumMember(Value = "VivaAction.GetLocation")]
            GetLocation,

            /// <summary>
            /// ShowLocation
            /// </summary>
            [EnumMember(Value = "VivaAction.ShowLocation")]
            ShowLocation, 

            /// <summary>
            /// Execute
            /// </summary>
            Execute 
        }

        /// <summary>
        /// Gets or Sets the type of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the type of the action.</value>
        [JsonProperty(PropertyName = "type")]
        public ActionType Type { get; set; }

        /// <summary>
        /// Gets or Sets the action parameters of type <see cref="ICardActionParameters"/>.
        /// </summary>
        /// <value>This value is the parameters of the action.</value>
        [JsonProperty(PropertyName = "parameters")]
        public ICardActionParameters Parameters { get; set; }
    }
}
