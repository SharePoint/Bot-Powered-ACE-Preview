// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using AdaptiveCards;
using Azure;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.SharePoint;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.SharePoint;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Bot.Streaming.Payloads;
using Microsoft.BotBuilderSamples.Helpers;
using Microsoft.BotBuilderSamples.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Graph;
using Microsoft.Identity.Core.Cache;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class SharePointAdaptiveCardExtensionAuthBot : SharePointActivityHandler
    {
        private readonly string _appTitle;
        private readonly string _connectionName;
        private readonly string _signInQuickViewId = "botACE3PIDP_QUICKVIEW_COMPLETESIGNIN";

        public SharePointAdaptiveCardExtensionAuthBot(IConfiguration configuration)
            : base()
        {
            this._appTitle = configuration["AppTitle"];
            this._connectionName = configuration["ConnectionName"];
        }

        protected override async Task<GetCardViewResponse> OnSharePointTaskGetCardViewAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            // check to see if the user has already signed in
            var user = await TryGetAuthenticatedUser(null, turnContext, cancellationToken);
            if (user != null)
            {
                return GenerateCardView(user, turnContext, cancellationToken);
            }

            return await GenerateSignInCardView(turnContext, cancellationToken);
        }

        protected override Task<GetQuickViewResponse> OnSharePointTaskGetQuickViewAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            return Task.FromResult(GenerateSignInQuickView());
        }

        protected override async Task<HandleActionResponse> OnSharePointTaskHandleActionAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            var magicCode = (taskModuleRequest?.Data as JObject)?.GetValue("data")?.SelectToken("magicCode")?.ToString();
            var user = await TryGetAuthenticatedUser(magicCode, turnContext, cancellationToken);
            var displayText = $"Hello, {user?.DisplayName}! You're signed in.";

            HandleActionResponse response = new HandleActionResponse
            {
                ViewType = HandleActionResponse.HandleActionResponseType.Card,
                RenderArguments = new RenderArgumentsBody
                {
                    AceData = new AceData
                    {
                        DataVersion = "1.0",
                        Id = "SignedInView",

                        CardSize = AceData.AceCardSize.Large,
                        Title = _appTitle,
                        PrimaryText = displayText
                    }
                }
            };

            return response;
        }

        protected override Task OnSharePointTaskSetPropertyPaneConfigurationAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            if (turnContext != null)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }

            return Task.CompletedTask;
        }

        private async Task<GetCardViewResponse> GenerateSignInCardView(ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            var signInResource = await TryGetSignInResource(turnContext, cancellationToken, null);
            var signInLink = signInResource != null ? new Uri(signInResource.SignInLink) : new Uri(string.Empty);

            var aceData = new AceData
            {
                DataVersion = "1.0",
                Id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f",

                CardSize = AceData.AceCardSize.Large,
                Title = _appTitle,

                PrimaryText = "Please Sign In",
                Description = "Testing sign in through sign in template for bots",

                SignInButtonText = "Sign In",
                SignInUri = signInLink,
                ConnectionName = _connectionName
            };
            ActionButton completeSignInButton = new ActionButton
            {
                Title = "Complete Sign In",
                Action = new Microsoft.Bot.Schema.SharePoint.Action
                {
                    Type = "QuickView",
                    Parameters = new ActionParameters
                    {
                        View = _signInQuickViewId
                    }
                }
            };

            List<ActionButton> actionButtons = new List<ActionButton>
            {
                completeSignInButton
            };

            GetCardViewResponse response = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.SignIn);
            response.AceData = aceData;
            response.Data = new CardViewData
            {
                ActionButtons = actionButtons
            };
            response.ViewId = "signInCard";

            return response;
        }

        private GetQuickViewResponse GenerateSignInQuickView()
        {
            AdaptiveTextBlock titleText = new AdaptiveTextBlock
            {
                Text = "Complete Sign In",
                Color = AdaptiveTextColor.Dark,
                Weight = AdaptiveTextWeight.Bolder,
                Size = AdaptiveTextSize.Medium,
                Wrap = true,
                MaxLines = 1,
                Spacing = AdaptiveSpacing.None
            };
            AdaptiveTextBlock descriptionText = new AdaptiveTextBlock
            {
                Text = "Input the magic code from signing into Azure Active Directory in order to continue.",
                Color = AdaptiveTextColor.Dark,
                Size = AdaptiveTextSize.Default,
                Wrap = true,
                MaxLines = 6,
                Spacing = AdaptiveSpacing.None
            };
            AdaptiveNumberInput magicCodeInputField = new AdaptiveNumberInput
            {
                Placeholder = "Enter Magic Code",
                Id = "magicCode",
                IsRequired = true
            };
            AdaptiveSubmitAction submitAction = new AdaptiveSubmitAction
            {
                Title = "Submit",
                Id = "SubmitMagicCode"
            };
            AdaptiveContainer container = new AdaptiveContainer
            {
                Separator = true,
                Items = new List<AdaptiveElement>
                {
                    titleText, descriptionText, magicCodeInputField
                }
            };

            AdaptiveCard ace = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));
            ace.Body = new List<AdaptiveElement> { container };
            ace.Actions = new List<AdaptiveAction> { submitAction };
            GetQuickViewResponse response = new GetQuickViewResponse
            {
                Data = new QuickViewData
                {
                    Title = "Complete Sign In",
                    Description = "Complete signing into a third party identity provider."
                },
                Template = ace,
                ViewId = _signInQuickViewId,
                StackSize = 1
            };

            return response;
        }

        private GetCardViewResponse GenerateCardView(Graph.User user, ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            var displayText = $"Hello, {user?.DisplayName}! You're signed in.";

            var aceData = new AceData
            {
                DataVersion = "1.0",
                Id = "SignedInView",

                CardSize = AceData.AceCardSize.Large,
                Title = _appTitle,
            };

            GetCardViewResponse response = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.PrimaryText);
            response.AceData = aceData;
            response.ViewId = "SignedInView";
            response.Data = new CardViewData
            {
                PrimaryText = displayText
            };

            return response;
        }

        private async Task<TokenResponse> TryToGetUserToken(string magicCode, ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            var userTokenClient = turnContext.TurnState.Get<UserTokenClient>();
            return await userTokenClient.GetUserTokenAsync(turnContext.Activity.From.Id, _connectionName, turnContext.Activity.ChannelId, magicCode, cancellationToken).ConfigureAwait(false);
        }

        private async Task<Graph.User> TryGetAuthenticatedUser(string magicCode, ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            var response = await TryToGetUserToken(magicCode, turnContext, cancellationToken);
            if (response != null && !string.IsNullOrEmpty(response.Token))
            {
                var client = new SimpleGraphClient(response.Token);
                return await client.GetMeAsync();
            }

            return null;
        }

        private async Task<SignInResource> TryGetSignInResource(ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken, string magicCode)
        {
            var userTokenClient = turnContext.TurnState.Get<UserTokenClient>();
            var signInResource = await userTokenClient.GetSignInResourceAsync(_connectionName, (Activity)turnContext.Activity, magicCode, cancellationToken).ConfigureAwait(false);
            return signInResource;
        }
    }
}
