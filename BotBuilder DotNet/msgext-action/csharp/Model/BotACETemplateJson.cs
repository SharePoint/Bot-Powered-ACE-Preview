// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Newtonsoft.Json;

namespace TeamsMessagingExtensionsAction.Model
{
    public class BotACETemplateJson
    {
        [JsonProperty("$schema")]
        public string Schema { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("body")]
        public object Body { get; set; }
    }
}
