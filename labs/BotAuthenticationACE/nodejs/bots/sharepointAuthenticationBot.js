// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

require('dotenv').config();
const { SharePointActivityHandler, CardFactory, TeamsInfo, MessageFactory, CloudAdapterBase, TurnContext } = require('botbuilder');
import { CloudAdapterBase, TurnContext } from 'botbuilder-core';
const { 
    GetCardViewResponse, 
    AceData, 
    ActionButton, 
    ActionParameters, 
    GetQuickViewResponse, 
} = require('botframework-schema');
const AdaptiveCards = require("adaptivecards");
const baseurl = process.env.BaseUrl;
const connectionName = process.env.ConnectionName;
const appTitle = process.env.AppTitle;

class SharepointAuthenticationBot extends SharePointActivityHandler {
    
	constructor() {
        super();
        this.cardViewResponse = undefined;
	}
	
    async handleTeamsMessagingExtensionSubmitAction(context, action) {
        switch (action.commandId) {
        case 'createCard':
            return createCardCommand(context, action);
        case 'shareMessage':
            return shareMessageCommand(context, action);
        case 'webView':
            return await webViewResponse(action);
        }
    }

    async handleTeamsMessagingExtensionFetchTask(context, action) {
        switch (action.commandId) {
        case 'webView':
            return empDetails();
        case 'Static HTML':
            return dateTimeInfo();
        default:
            try {
                const member = await this.getSingleMember(context);
                return {
                    task: {
                        type: 'continue',
                        value: {
                            card: GetAdaptiveCardAttachment(),
                            height: 400,
                            title: `Hello ${ member }`,
                            width: 300
                        }
                    }
                };
            } catch (e) {
                if (e.code === 'BotNotInConversationRoster') {
                    return {
                        task: {
                            type: 'continue',
                            value: {
                                card: GetJustInTimeCardAttachment(),
                                height: 400,
                                title: 'Adaptive Card - App Installation',
                                width: 300
                            }
                        }
                    };
                }
                throw e;
            }
        }
    }

    async getSingleMember(context) {
        try {
            const member = await TeamsInfo.getMember(
                context,
                context.activity.from.id
            );
            return member.name;
        } catch (e) {
            if (e.code === 'MemberNotFoundInConversation') {
                context.sendActivity(MessageFactory.text('Member not found.'));
                return e.code;
            }
            throw e;
        }
    }

     /**
     * Override this in a derived class to provide logic for when a card view is fetched
     * 
     * @param context - A strongly-typed context object for this turn
     * @param taskModuleRequest - The task module invoke request value payload
     * @returns A task module response for the request
     */
    async OnSharePointTaskGetCardViewAsync(context, taskModuleRequest){
        try {
            if (!this.cardViewResponse) {
                // check to see if the user has already signed in
                const user = await TryGetAuthenticatedUser(null, turnContext);
                if (user != null)
                {
                    return GenerateCardView(user);
                } else {
                    return await GenerateSignInCardView(turnContext, cancellationToken);
                }
            } else {
                return this.cardViewResponse;
            }
            
        } catch(error) {
            console.log(error);
        }
    }

    /**
     * Override this in a derived class to provide logic for when a quick view is fetched
     * 
     * @param context - A strongly-typed context object for this turn
     * @param taskModuleRequest - The task module invoke request value payload
     * @returns A task module response for the request
     */
    async OnSharePointTaskGetQuickViewAsync(context, taskModuleRequest) {
        try {
            return GenerateSignInQuickView();
        } catch(error) {
            console.log(error);
        }
    }

    async OnSharePointTaskHandleActionAsync(turnContext, taskModuleRequest) {
        const magicCode = (taskModuleRequest?.Data)?.GetValue("data")?.SelectToken("magicCode")?.ToString();
        const user = await TryGetAuthenticatedUser(magicCode, turnContext);
        const displayText = `Hello, ${user?.DisplayName}! You're signed in.`;

        const response = new HandleActionResponse();
        response.ResponseType = HandleActionResponseType.Card;
        response.RenderArguments = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.PrimaryText)

        response.RenderArguments.AceData = new AceData();
        response.RenderArguments.AceData.DataVersion = "1.0";
        response.RenderArguments.AceData.Id = "SignedInView";
        response.RenderArguments.AceData.CardSize = AceData.AceCardSize.Large;
        response.RenderArguments.AceData.Title = appTitle

        response.RenderArguments.Data = {};
        response.RenderArguments.Data.PrimaryText = "Signed In";
        response.RenderArguments.Data.Description = displayText;
        
        response.RenderArguments.ViewId = "SignedInViewId";

        return response;
    }

    GenerateCardView(user) {
        const displayText = `Hello, ${user?.DisplayName}! You're signed in.`;

        const response = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.PrimaryText);
        response.AceData = new AceData();
        response.AceData.DataVersion = "1.0";
        response.AceData.Id = "SignedInView";
        response.AceData.CardSize = AceData.AceCardSize.Large;
        response.AceData.Title = appTitle;
        response.ViewId = "SignedInView";

        response.Data = {};
        return response;
    }

    async GenerateSignInCardView(turnContext) {
        var signInResource = await TryGetSignInResource(turnContext);
        var signInLink = signInResource != null ? new Uri(signInResource.SignInLink) : new Uri(string.Empty);

        var aceData = new AceData
        {
            DataVersion = "1.0",
            Id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f",

            CardSize = AceData.AceCardSize.Large,
            Title = appTitle
        };
        aceData.Properties.SignInUri = signInLink;
        aceData.Properties.ConnectionName = connectionName;
        
        var data = {};
        data.PrimaryText = "Please Sign In";
        data.Description = "Testing sign in through sign in template for bots"
        data.SignInButtonText = "Sign In"

        const completeSignInButton = new ActionButton
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

        const actionButtons = new List<ActionButton>
        {
            completeSignInButton
        };

        const response = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.SignIn);
        response.AceData = aceData;
        response.CardButtons = actionButtons;
        response.Data = data;
        response.ViewId = "signInCard";

        return response;
    }


    GenerateSignInQuickView() {
        const titleText = new AdaptiveTextBlock
        {
            Text = "Complete Sign In",
            Color = AdaptiveTextColor.Dark,
            Weight = AdaptiveTextWeight.Bolder,
            Size = AdaptiveTextSize.Medium,
            Wrap = true,
            MaxLines = 1,
            Spacing = AdaptiveSpacing.None
        };
        const descriptionText = new AdaptiveTextBlock
        {
            Text = "Input the magic code from signing into Azure Active Directory in order to continue.",
            Color = AdaptiveTextColor.Dark,
            Size = AdaptiveTextSize.Default,
            Wrap = true,
            MaxLines = 6,
            Spacing = AdaptiveSpacing.None
        };
        const magicCodeInputField = new AdaptiveNumberInput
        {
            Placeholder = "Enter Magic Code",
            Id = "magicCode",
            IsRequired = true
        };
        const submitAction = new AdaptiveSubmitAction
        {
            Title = "Submit",
            Id = "SubmitMagicCode"
        };
        const container = new AdaptiveContainer
        {
            Separator = true,
            Items = new List<AdaptiveElement>
            {
                titleText, descriptionText, magicCodeInputField
            }
        };

        const ace = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0));
        ace.Body = new List<AdaptiveElement> { container };
        ace.Actions = new List<AdaptiveAction> { submitAction };
        const response = new GetQuickViewResponse
        {
            Template = ace,
            ViewId = _signInQuickViewId,
            StackSize = 1
        };
        response.Data.Title = "Complete Sign In";
        response.Data.Description = "Complete signing into a third party identity provider.";

        return response;
    }

    /**
     * Override this in a derived class to provide logic for setting configuration pane properties.
     * 
     * @param magicCode - a six digit magic code from bot framework 
     * @param context - A strongly-typed context object for this turn
     * @param taskModuleRequest - The task module invoke request value payload
     * @returns A task module response for the request
     */
    async TryToGetUserToken(magicCode, turnContext)
    {
        const userTokenClient = turnContext.turnState.get<UserTokenClient>(context.adapter.UserTokenClientKey);
        if (userTokenClient) {
            return userTokenClient.getUserToken(
                turnContext.activity?.from?.id,
                connectionName,
                turnContext.activity?.channelId,
                magicCode);
        } else {
            throw new Error('userTokenClient is null');
        }
    }

    /**
     * Override this in a derived class to provide logic for setting configuration pane properties.
     * 
     * @param magicCode - a six digit magic code from bot framework 
     * @param context - A strongly-typed context object for this turn
     * @returns A Microsoft.Graph.User user
     */
    async TryGetAuthenticatedUser(magicCode, turnContext)
    {
        const response = await TryToGetUserToken(magicCode, turnContext);
        if (response != null && !string.IsNullOrEmpty(response.Token))
        {
            const client = new SimpleGraphClient(response.token);
            return await client.getMe();
        } else {
            return null;
        }
    }

    /**
     * Override this in a derived class to provide logic for setting configuration pane properties.
     * 
     * @param context - A strongly-typed context object for this turn
     * @returns A SignInResource
     */
    async TryGetSignInResource(turnContext)
    {
        const userTokenClient = turnContext.turnState.get < UserTokenClient > (context.adapter.UserTokenClientKey);
        return userTokenClient.getSignInResource(connectionName, turnContext.activity, undefined);
    }
}

module.exports.SharepointMessagingExtensionsActionBot = SharepointMessagingExtensionsActionBot;
