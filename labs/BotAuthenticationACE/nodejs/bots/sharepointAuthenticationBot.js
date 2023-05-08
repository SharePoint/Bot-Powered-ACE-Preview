// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

require('dotenv').config();
const { SharePointActivityHandler, CardFactory, TeamsInfo, MessageFactory} = require('botbuilder');
const { CloudAdapterBase, TurnContext } = require('botbuilder-core');
const { SimpleGraphClient } = require('../simple-graph-client');
const { 
    GetCardViewResponse, 
    AceData, 
    ActionButton, 
    GetQuickViewResponse,
    HandleActionReponse,
    PrimaryTextCardParameters,
    QuickViewParameters,
    SignInCardParameters,
    SharepointAction, 
} = require('botframework-schema');
const AdaptiveCards = require("adaptivecards");
const baseurl = process.env.BaseUrl;
const connectionName = process.env.ConnectionName;
const appTitle = "Sign In Bot";

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
                const user = await this.TryGetAuthenticatedUser(null, context);
                if (user != null)
                {
                    return this.GenerateCardView(user);
                } else {
                    return await this.GenerateSignInCardView(context);
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
            return this.GenerateSignInQuickView();
        } catch(error) {
            console.log(error);
        }
    }

    async OnSharePointTaskHandleActionAsync(turnContext, taskModuleRequest) {
        const magicCode = turnContext.activity.value.data.data.magicCode;
        const user = await this.TryGetAuthenticatedUser(magicCode, turnContext);
        const displayText = `Hello, ${user?.DisplayName}! You're signed in.`;

        const response = new HandleActionReponse();
        response.ResponseType = HandleActionReponse.ResponseType.CardView;
        const cardView = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.PrimaryTextCardView);
        cardView.TemplateType = GetCardViewResponse.CardViewTemplateType.PrimaryTextCardView;

        cardView.AceData = new AceData();
        cardView.AceData.DataVersion = "1.0";
        cardView.AceData.Id = "SignedInView";
        cardView.AceData.CardSize = AceData.AceCardSize.Large;
        cardView.AceData.Title = appTitle

        const params = new PrimaryTextCardParameters();
        params.PrimaryText = "Signed In";
        params.Description = displayText;
        cardView.Data = params;
        
        cardView.ViewId = "SignedInViewId";

        response.RenderArguments = cardView;

        return response;
    }

    GenerateCardView(user) {
        const displayText = `Hello, ${user.displayName}! You're signed in.`;

        const response = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.PrimaryTextCardView);
        response.TemplateType = GetCardViewResponse.CardViewTemplateType.PrimaryTextCardView;
        response.AceData = new AceData();
        response.AceData.DataVersion = "1.0";
        response.AceData.Id = "SignedInView";
        response.AceData.CardSize = AceData.AceCardSize.Large;
        response.AceData.Title = appTitle;
        response.ViewId = "SignedInView";

        const params = new PrimaryTextCardParameters();
        params.PrimaryText = "Signed In";
        params.Description = displayText;
        response.Data = params;
        return response;
    }

    async GenerateSignInCardView(turnContext) {
        var signInResource = await this.TryGetSignInResource(turnContext);
        var signInLink = signInResource != null ? signInResource.signInLink : string.Empty;
        var aceData = new AceData();
        aceData.DataVersion = "1.0";
        aceData.Id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f";
        aceData.CardSize = AceData.AceCardSize.Large;
        aceData.Title = appTitle;

        aceData.Properties = {
            "uri": signInLink,
            connectionName: connectionName
        };
        
        var data = new SignInCardParameters();
        data.PrimaryText = "Please Sign In";
        data.Description = "Testing sign in through sign in template for bots"
        data.SignInButtonText = "Sign In"

        const completeSignInButton = new ActionButton();
        completeSignInButton.Title = "Complete Sign In";
        const buttonAction = new SharepointAction();
        buttonAction.Type = SharepointAction.ActionType.QuickView;
        const params =  new QuickViewParameters();
        params.View = "signInQuickView";
        buttonAction.Parameters = params;
        completeSignInButton.Action = buttonAction;

        const actionButtons = [];
        actionButtons[0] = completeSignInButton;

        const response = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.SignInCardView);
        response.TemplateType = GetCardViewResponse.CardViewTemplateType.SignInCardView;
        response.AceData = aceData;
        response.CardButtons = actionButtons;
        response.Data = data;
        response.ViewId = "signInCard";

        return response;
    }


    GenerateSignInQuickView() {
        const titleText = new AdaptiveCards.TextBlock();
        titleText.text = "Complete Sign In";
        titleText.color = AdaptiveCards.TextColor.Dark;
        titleText.weight = AdaptiveCards.TextWeight.Bolder;
        titleText.size = AdaptiveCards.TextSize.Medium;
        titleText.wrap = true;
        titleText.maxLines = 1;
        titleText.spacing = AdaptiveCards.Spacing.None;

        const descriptionText = new AdaptiveCards.TextBlock();
        descriptionText.text = "Input the magic code from signing into Azure Active Directory in order to continue.";
        descriptionText.color = AdaptiveCards.TextColor.Dark;
        descriptionText.size = AdaptiveCards.TextSize.Default;
        descriptionText.wrap = true;
        descriptionText.maxLines = 6;
        descriptionText.spacing = AdaptiveCards.Spacing.None;
        
        const magicCodeInputField = new AdaptiveCards.NumberInput();
        magicCodeInputField.placeholder = "Enter Magic Code";
        magicCodeInputField.id = "magicCode";
        

        const submitAction = new AdaptiveCards.SubmitAction();
        submitAction.title = "Submit";
        submitAction.id = "SubmitMagicCode";

        const container =  new AdaptiveCards.Container();
        container.separator = true;
        container.addItem(titleText);
        container.addItem(descriptionText);
        container.addItem(magicCodeInputField);

        const template = new AdaptiveCards.AdaptiveCard();
        template.addItem(container);
        template.addAction(submitAction);
        const response = new GetQuickViewResponse();
        response.Template = template;
        response.ViewId = "signInQuickView"
        response.Title = "Complete Sign In"

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
        // const userTokenClient = turnContext.turnState.get(turnContext.adapter.UserTokenClientKey);
        const userTokenClient = turnContext.adapter;
        if (userTokenClient) {
            return userTokenClient.getUserToken(
                turnContext,
                connectionName,
                magicCode,
            );
            
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
        const response = await this.TryToGetUserToken(magicCode, turnContext);
        if (response != null && response.token)
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
        const userTokenClient = turnContext.adapter;
        return userTokenClient.getSignInResource(turnContext, connectionName, turnContext.activity.from.id, 'https://login.microsoftonline.com', undefined);
    }
}

module.exports.SharepointAuthenticationBot = SharepointAuthenticationBot;
