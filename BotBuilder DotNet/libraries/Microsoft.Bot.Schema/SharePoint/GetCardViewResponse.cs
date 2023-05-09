// Copyright (c) Microsoft Corporation. All rights reserved.
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
    /// SharePoint GetCardView response object.
    /// </summary>
    public class GetCardViewResponse : ISharePointViewResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetCardViewResponse"/> class.
        /// </summary>
        /// <param name="templateType">Template type of the card view.</param>
        public GetCardViewResponse(CardViewTemplateType templateType)
        {
            this.TemplateType = templateType;
        }

        /// <summary>
        /// This enum contains the different types of card templates available in the SPFx framework.
        /// </summary>
        public enum CardViewTemplateType
        {
            /// <summary>
            /// Primary text card view
            /// </summary>
            [EnumMember(Value = "Basic")]
            BasicCardView,

            /// <summary>
            /// Image card view
            /// </summary>
            [EnumMember(Value = "Image")]
            ImageCardView,

            /// <summary>
            /// Primary Text card view
            /// </summary>
            [EnumMember(Value = "PrimaryText")]
            PrimaryTextCardView,

            /// <summary>
            /// Sign In card view
            /// </summary>
            [EnumMember(Value = "SignIn")]
            SignInCardView
        }

        /// <summary>
        /// Gets or Sets the template type of the card view of type <see cref="CardViewTemplateType"/> enum.
        /// </summary>
        /// <value>This value is the template type of the card view response.</value>
        [JsonProperty(PropertyName = "templateType")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CardViewTemplateType TemplateType { get; set; }

        /// <summary>
        /// Gets or Sets AceData for the card view of type <see cref="AceData"/>.
        /// </summary>
        /// <value>This value is the ace data of the card view response.</value>
        [JsonProperty(PropertyName = "aceData")]
        public AceData AceData { get; set; }

        /// <summary>
        /// Gets or Sets data associated with the card view of type <see cref="ICardParameters"/>.
        /// </summary>
        /// <value>This value is the data of the card view response.</value>
        [JsonProperty(PropertyName = "data")]
        public ICardParameters Data { get; set; }

        /// <summary>
        /// Gets or Sets action to be performed when card is selected of type <see cref="SharepointAction"/>.
        /// </summary>
        /// <value>This value is the action performed when card is clicked.</value>
        [JsonProperty(PropertyName = "onCardSelection")]
        public SharepointAction OnCardSelection { get; set; }

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
