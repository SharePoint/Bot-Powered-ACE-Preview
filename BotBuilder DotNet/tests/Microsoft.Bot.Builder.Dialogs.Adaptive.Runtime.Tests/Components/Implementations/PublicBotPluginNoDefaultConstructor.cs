﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.Bot.Builder.Runtime.Tests.Components.Implementations
{
    public class PublicBotPluginNoDefaultConstructor : BotComponent
    {
        public PublicBotPluginNoDefaultConstructor(bool foo)
        {
        }

        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
        }
    }
}
