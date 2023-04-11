﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationInsights;
using Microsoft.Bot.Builder.ApplicationInsights;
using Microsoft.Bot.Builder.Dialogs.Declarative.Resources;
using Microsoft.Bot.Builder.Dialogs.Memory;
using Microsoft.Bot.Builder.Dialogs.Memory.Scopes;
using Microsoft.Bot.Builder.Skills;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Microsoft.Bot.Builder.Dialogs.Adaptive.Runtime
{
    internal class ConfigurationAdaptiveDialogBot : AdaptiveDialogBot
    {
        private const string DefaultLanguageGeneratorId = "main.lg";

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationAdaptiveDialogBot"/> class using <see cref="IConfiguration"/>.
        /// </summary>
        /// <param name="configuration">An <see cref="IConfiguration"/> instance.</param>
        /// <param name="resourceExplorer">The Bot Builder <see cref="ResourceExplorer"/> to load the <see cref="AdaptiveDialog"/> from.</param>
        /// <param name="conversationState">The <see cref="ConversationState"/> implementation to use for this <see cref="AdaptiveDialog"/>.</param>
        /// <param name="userState">The <see cref="UserState"/> implementation to use for this <see cref="AdaptiveDialog"/>.</param>
        /// <param name="skillConversationIdFactoryBase">The <see cref="SkillConversationIdFactoryBase"/> implementation to use for this <see cref="AdaptiveDialog"/>.</param>
        /// <param name="languagePolicy">The <see cref="LanguagePolicy"/> implementation to use for this <see cref="AdaptiveDialog"/>.</param>
        /// <param name="botFrameworkAuthentication">A <see cref="BotFrameworkAuthentication"/> for making calls to Bot Builder Skills.</param>
        /// <param name="telemetryClient">A <see cref="IBotTelemetryClient"/> for logging bot telemetry events.</param>
        /// <param name="scopes">A set of <see cref="MemoryScope"/> that will be added to the <see cref="ITurnContext"/>.</param>
        /// <param name="pathResolvers">A set of <see cref="IPathResolver"/> that will be added to the <see cref="ITurnContext"/>.</param>
        /// <param name="dialogs">Custom <see cref="Dialog"/> that will be added to the root DialogSet.</param>
        /// <param name="logger">An <see cref="ILogger"/> instance.</param>
        public ConfigurationAdaptiveDialogBot(
            IConfiguration configuration,
            ResourceExplorer resourceExplorer,
            ConversationState conversationState,
            UserState userState,
            SkillConversationIdFactoryBase skillConversationIdFactoryBase,
            LanguagePolicy languagePolicy,
            BotFrameworkAuthentication botFrameworkAuthentication = null,
            IBotTelemetryClient telemetryClient = null,
            IEnumerable<MemoryScope> scopes = default,
            IEnumerable<IPathResolver> pathResolvers = default,
            IEnumerable<Dialog> dialogs = default,
            ILogger logger = null)
            : base(
                configuration.GetSection(ConfigurationConstants.RootDialogKey).Value,
                configuration.GetSection(ConfigurationConstants.LanguageGeneratorKey).Value ?? DefaultLanguageGeneratorId,
                resourceExplorer,
                conversationState,
                userState,
                skillConversationIdFactoryBase,
                languagePolicy,
                botFrameworkAuthentication ?? BotFrameworkAuthenticationFactory.Create(),
                telemetryClient ?? new NullBotTelemetryClient(),
                scopes ?? Enumerable.Empty<MemoryScope>(),
                pathResolvers ?? Enumerable.Empty<IPathResolver>(),
                dialogs ?? Enumerable.Empty<Dialog>(),
                logger: logger ?? NullLogger<AdaptiveDialogBot>.Instance)
        {
        }
    }
}
