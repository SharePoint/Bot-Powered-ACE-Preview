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
            var magicCode = (taskModuleRequest?.Data as JObject)?.GetValue("data")?.SelectToken("magicCode")?.ToString();
            var user = await TryGetAuthenticatedUser(magicCode, turnContext, cancellationToken);
            if (magicCode != null && user != null)
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

            var response = new HandleActionResponse();
            response.ResponseType = HandleActionResponse.ResponseTypeOption.CardView;

            var renderArguments = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.PrimaryTextCardView);
            renderArguments.AceData = new AceData();
            renderArguments.AceData.Title = _appTitle;
            renderArguments.AceData.DataVersion = "1.0";
            renderArguments.AceData.Id = "SignedInView";
            renderArguments.AceData.CardSize = AceData.AceCardSize.Large;

            var param = new PrimaryTextCardParameters();
            param.PrimaryText = "Signed In";
            param.Description = $"Hello, {user?.DisplayName}! You're signed in.";
            renderArguments.Data = param;
            renderArguments.ViewId = "SignedInViewId";
            response.RenderArguments = renderArguments;

            return response;
        }

        protected override Task<SetPropertyPaneConfigurationResponse> OnSharePointTaskSetPropertyPaneConfigurationAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            if (turnContext != null)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }

            return Task.FromResult(new SetPropertyPaneConfigurationResponse());
        }

        private async Task<GetCardViewResponse> GenerateSignInCardView(ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            var signInResource = await TryGetSignInResource(turnContext, cancellationToken, null);
            var signInLink = signInResource != null ? new Uri(signInResource.SignInLink) : new Uri(string.Empty);

            GetCardViewResponse response = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.SignInCardView);
            response.AceData = new AceData();
            response.AceData.CardSize = AceData.AceCardSize.Large;
            response.AceData.DataVersion = "1.0";
            response.AceData.Id = "a1de36bb - 9e9e - 4b8e - 81f8 - 853c3bba483f";
            response.AceData.Title = _appTitle;
            var props = JsonConvert.SerializeObject(new { signInUri = signInLink, _connectionName = _connectionName });
            response.AceData.Properties = (JObject)JsonConvert.DeserializeObject(props);

            SignInCardParameters param = new SignInCardParameters();
            param.PrimaryText = "Please Sign In";
            param.Description = "Testing sign in through sign in template for bots";
            param.SignInButtonText = "Sign In";
            response.Data = param;

            SharepointAction buttonAction = new SharepointAction();
            buttonAction.Type = SharepointAction.ActionType.QuickView;
            QuickViewParameters buttonParams = new QuickViewParameters();
            buttonParams.View = _signInQuickViewId;
            buttonAction.Parameters = buttonParams;

            ActionButton completeSignInButton = new ActionButton();
            completeSignInButton.Title = "Complete Sign In";
            completeSignInButton.Action = buttonAction;

            List<ActionButton> actionButtons = new List<ActionButton>
            {
                completeSignInButton
            };

            response.CardButtons = actionButtons;
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
            GetQuickViewResponse response = new GetQuickViewResponse();
            response.Title = "Complete Sign In";
            response.Template = ace;
            response.ViewId = _signInQuickViewId;

            return response;
        }

        private GetCardViewResponse GenerateCardView(Graph.User user, ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            var displayText = $"Hello, {user?.DisplayName}! You're signed in.";

            AceData aceData = new AceData();
            aceData.DataVersion = "1.0";
            aceData.Id = "SignedInView";
            aceData.CardSize = AceData.AceCardSize.Large;
            aceData.Title = _appTitle;

            GetCardViewResponse response = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.PrimaryTextCardView);
            response.AceData = aceData;
            response.ViewId = "SignedInView";
            response.Data = new PrimaryTextCardParameters
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
            var response = await TryToGetUserToken(magicCode, turnContext, cancellationToken).ConfigureAwait(false);
            if (response != null && !string.IsNullOrEmpty(response.Token))
            {
                var client = new SimpleGraphClient(response.Token);
                return await client.GetMeAsync().ConfigureAwait(false);
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
