﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Bot.Builder.Dialogs.Adaptive.Templates;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Dialogs.Adaptive
{
    /// <summary>
    /// JsonConverter to load ITemplate&lt;Activity&gt;.
    /// </summary>
    [SuppressMessage("Performance", "CA1812", Justification = "The class gets instantiated by dependency injection.")]
    internal class ActivityTemplateConverter : JsonConverter<ActivityTemplate>
    {
        public override bool CanRead => false;

        public override bool CanWrite => true;

        public override ActivityTemplate ReadJson(JsonReader reader, Type objectType, ActivityTemplate existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer, ActivityTemplate value, JsonSerializer serializer)
        {
            // save template as string
            serializer.Serialize(writer, value.Template);
        }
    }
}
