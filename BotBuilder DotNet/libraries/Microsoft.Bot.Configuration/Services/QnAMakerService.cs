﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Microsoft.Bot.Configuration.Encryption;
using Newtonsoft.Json;

namespace Microsoft.Bot.Configuration
{
    /// <summary>
    /// Configuration properties for a connected LUIS service.
    /// </summary>
    [Obsolete("This class is deprecated.  See https://aka.ms/bot-file-basics for more information.", false)]
    public class QnAMakerService : ConnectedService
    {
        private string _hostname;

        /// <summary>
        /// Initializes a new instance of the <see cref="QnAMakerService"/> class.
        /// </summary>
        public QnAMakerService()
            : base(ServiceTypes.QnA)
        {
        }

        /// <summary>
        /// Gets or sets kbId.
        /// </summary>
        /// <value>The Knowledge Base Id.</value>
        [JsonProperty("kbId")]
        public string KbId { get; set; }

        /// <summary>
        /// Gets or sets subscriptionKey.
        /// </summary>
        /// <value>The subscription key.</value>
        [JsonProperty("subscriptionKey")]
        public string SubscriptionKey { get; set; }

        /// <summary>
        /// Gets or sets url for the deployed qnaMaker instance.
        /// </summary>
        /// <value>The Host name.</value>
        [JsonProperty("hostname")]
        public string Hostname { get => _hostname; set => _hostname = new Uri(new Uri(value), "/qnamaker").AbsoluteUri; }

        /// <summary>
        /// Gets or sets endpointKey.
        /// </summary>
        /// <value>The endpoint.</value>
        [JsonProperty("endpointKey")]
        public string EndpointKey { get; set; }

        /// <inheritdoc/>
        public override void Encrypt(string secret)
        {
            base.Encrypt(secret);

            if (!string.IsNullOrEmpty(this.EndpointKey))
            {
                this.EndpointKey = this.EndpointKey.Encrypt(secret);
            }

            if (!string.IsNullOrEmpty(this.SubscriptionKey))
            {
                this.SubscriptionKey = this.SubscriptionKey.Encrypt(secret);
            }
        }

        /// <inheritdoc/>
        public override void Decrypt(string secret)
        {
            base.Decrypt(secret);

            if (!string.IsNullOrEmpty(this.EndpointKey))
            {
                this.EndpointKey = this.EndpointKey.Decrypt(secret);
            }

            if (!string.IsNullOrEmpty(this.SubscriptionKey))
            {
                this.SubscriptionKey = this.SubscriptionKey.Decrypt(secret);
            }
        }
    }
}
