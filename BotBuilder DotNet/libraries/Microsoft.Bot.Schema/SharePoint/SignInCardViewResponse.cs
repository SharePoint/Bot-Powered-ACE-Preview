﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.Bot.Schema.Teams;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Microsoft.Bot.Schema.SharePoint
{
    /// <summary>
    /// SharePoint Primary Card View response object.
    /// </summary>
    public class SignInCardViewResponse : ICardViewResponse
    {
#pragma warning disable CA1823 // Avoid unused private fields
#pragma warning disable CS0414 // The field 'SignInCardViewResponse.templateType' is assigned but its value is never used
        private string templateType = "SignIn";
#pragma warning restore CS0414 // The field 'SignInCardViewResponse.templateType' is assigned but its value is never used
#pragma warning restore CA1823 // Avoid unused private fields

        /// <summary>
        /// Initializes a new instance of the <see cref="SignInCardViewResponse"/> class.
        /// </summary>
        public SignInCardViewResponse()
        {
        }

        /// <summary>
        /// Gets or Sets AceData for the card view of type <see cref="AceData"/>.
        /// </summary>
        /// <value>This value is the ace data of the card view response.</value>
        [JsonProperty(PropertyName = "aceData")]
        public AceData AceData { get; set; }

        /// <summary>
        /// Gets or Sets data associated with the card view of type <see cref="SignInCardParameters"/>.
        /// </summary>
        /// <value>This value is the data of the card view response.</value>
        [JsonProperty(PropertyName = "data")]
        public SignInCardParameters Data { get; set; }

        /// <summary>
        /// Gets or Sets action to be performed when card is selected of type <see cref="IOnCardSelectionAction"/>.
        /// </summary>
        /// <value>This value is the action performed when card is clicked.</value>
        [JsonProperty(PropertyName = "onCardSelection")]
        public IOnCardSelectionAction OnCardSelection { get; set; }

        /// <summary>
        /// Gets or Sets button(s) on the card view of type <see cref="ActionButton"/>.
        /// </summary>
        /// <value>This value is the button(s) associated with the card view.</value>
        [JsonProperty(PropertyName = "cardButtons")]
        public IEnumerable<ActionButton> CardButtons { get; set; }

        /// <summary>
        /// Gets or Sets the view Id of type <see cref="string"/>.
        /// </summary>
        /// <value>This value is the view id of the card view.</value>
        [JsonProperty(PropertyName = "viewId")]
        public string ViewId { get; set; }
    }
}
