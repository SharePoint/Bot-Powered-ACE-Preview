﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Bot.Builder.AI.QnA;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Adaptive;
using Microsoft.Bot.Builder.Dialogs.Declarative;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.Bot.Builder.Dialogs.Adaptive.Runtime.Component
{
    /// <summary>
    /// Retrieve the built-in enumeration of <see cref="BotComponent"/> instances.
    /// </summary>
    internal static class BuiltInBotComponents
    {
        private static readonly List<BotComponent> _components = new List<BotComponent>()
        {
            new DialogsBotComponent(),
            new DeclarativeBotComponent(),
            new AdaptiveBotComponent(),
            new LanguageGenerationBotComponent(),
            new QnAMakerBotComponent(),
            new LuisBotComponent(),
        };

        internal static void LoadAll(IServiceCollection services, IConfiguration configuration)
        {
            foreach (var component in _components)
            {
                component.ConfigureServices(services, configuration);
            }
        }
    }
}
