// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

require('dotenv').config();
const {  SharePointActivityHandler, CardFactory, TeamsInfo, MessageFactory } = require('botbuilder');
const { 
    GetCardViewResponse, 
    AceData,
    ActionButton, 
    SharepointAction, 
    GetQuickViewResponse, 
    PropertyPanePageHeader, 
    PropertyPaneDropDownProperties, 
    PropertyPaneDropDownOption, 
    PropertyPaneLabelProperties, 
    PropertyPaneChoiceGroupProperties, 
    PropertyPaneChoiceGroupOption, 
    PropertyPaneChoiceGroupIconProperties,
    GetPropertyPaneConfigurationResponse,
    PropertyPanePage,
    PropertyPaneGroup,
    PropertyPaneGroupField,
    PropertyPaneTextFieldProperties,
    PropertyPaneToggleProperties,
    PropertyPaneSliderProperties,
    PropertyPaneLinkProperties,
    PropertyPaneLinkPopupWindowProperties,
    BasicCardParameters,
    QuickViewParameters,
    PrimaryTextCardParameters,
    ImageCardParameters,
    SignInCardParameters,
    HandleActionReponse,
    SetPropertyPaneConfigurationResponse
} = require('botframework-schema');
const AdaptiveCards = require("adaptivecards");
const baseurl = process.env.BaseUrl;

class SharepointMessagingExtensionsActionBot extends SharePointActivityHandler {
    
	constructor() {
        super();
        this.cardViewsCreated = false;
        this.quickViewsCreated = false;
        this.cardViewMap = new Map();
        this.quickViewMap = new Map();
        this.currentView = "";
        this.updatedView = null;
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
        if (!this.cardViewsCreated){
            this.createCardViews();
        }
        if (this.updatedView){
            return this.updatedView;
        }
        this.currentView = 'PRIMARY_TEXT_CARD_VIEW';
        return this.cardViewMap.get('PRIMARY_TEXT_CARD_VIEW');
    }

    /**
     * Override this in a derived class to provide logic for when a quick view is fetched
     * 
     * @param context - A strongly-typed context object for this turn
     * @param taskModuleRequest - The task module invoke request value payload
     * @returns A task module response for the request
     */
    async OnSharePointTaskGetQuickViewAsync(context, taskModuleRequest){
        if (!this.quickViewsCreated){
            this.createQuickViews();
        }
        let quickViewId;
        if (this.currentView.includes("CARD")){
            quickViewId = this.cardViewMap.get(this.currentView).OnCardSelection.Parameters.View;
        }
        return this.quickViewMap.get(quickViewId);
    }

    /**
     * Override this in a derived class to provide logic for getting configuration pane properties.
     * 
     * @param context - A strongly-typed context object for this turn
     * @param taskModuleRequest - The task module invoke request value payload
     * @returns A task module response for the request
     */
    async OnSharePointTaskGetPropertyPaneConfigurationAsync(context, taskModuleRequest){
        const response = new GetPropertyPaneConfigurationResponse();
        const page = new PropertyPanePage();
        page.Header = new PropertyPanePageHeader();
        page.Header.Description = "Property pane for control";

        const configurableGroup = new PropertyPaneGroup();
        configurableGroup.GroupName = 'Configurable Properties';
        

        const text = new PropertyPaneGroupField();
        text.TargetProperty = "title";
        text.Type = PropertyPaneGroupField.FieldType.TextField;
        const textProperties = new PropertyPaneTextFieldProperties();
        textProperties.Value = "Bot Ace Demo";
        textProperties.Label = "Title";
        text.Properties = textProperties;

        const primaryText = new PropertyPaneGroupField();
        primaryText.TargetProperty = "primaryText";
        primaryText.Type = PropertyPaneGroupField.FieldType.TextField;
        const primaryTextProperties = new PropertyPaneTextFieldProperties();
        primaryTextProperties.Value = "My Bot!";
        primaryTextProperties.Label = "Primary Text";
        primaryText.Properties = primaryTextProperties;

        const descriptionText = new PropertyPaneGroupField();
        descriptionText.TargetProperty = "description";
        descriptionText.Type = PropertyPaneGroupField.FieldType.TextField;
        const descriptionTextProperties = new PropertyPaneTextFieldProperties();
        descriptionTextProperties.Value = "";
        descriptionTextProperties.Label = "Description";
        descriptionText.Properties = descriptionTextProperties;

        // To make these properties "configurable", edit the logic in OnSharePointTaskSetPropertyPaneConfigurationAsync
        const dummyGroup = new PropertyPaneGroup();
        dummyGroup.GroupName = 'Nonconfigurable Props (see code!)';

        const toggle = new PropertyPaneGroupField();

        toggle.TargetProperty = "toggle";
        toggle.Type = PropertyPaneGroupField.FieldType.Toggle;
        const toggleProperties = new PropertyPaneToggleProperties();
        toggleProperties.Label = "Turn this feature on?";
        toggleProperties.Key = "uniqueKey";
        toggle.Properties = toggleProperties;

        const dropDown = new PropertyPaneGroupField();
        dropDown.TargetProperty = "dropdown";
        dropDown.Type = PropertyPaneGroupField.FieldType.Dropdown;
        const dropDownProperties = new PropertyPaneDropDownProperties();
        const options = [];
        const countryHeader = new PropertyPaneDropDownOption();
        countryHeader.Type = PropertyPaneDropDownOption.DropDownOptionType.Header;
        countryHeader.Text = "Country";

        const divider = new PropertyPaneDropDownOption();
        divider.Type = PropertyPaneDropDownOption.DropDownOptionType.Divider;

        const canada = new PropertyPaneDropDownOption();
        canada.Type = PropertyPaneDropDownOption.DropDownOptionType.Normal;
        canada.Text = "Canada";
        canada.Key = "can";

        const usa = new PropertyPaneDropDownOption();
        usa.Text = "USA";
        usa.Key = "US";

        const mexico = new PropertyPaneDropDownOption();
        mexico.Type = PropertyPaneDropDownOption.DropDownOptionType.Normal;
        mexico.Text = "Mexico";
        mexico.Key = "mex";

        options.push(countryHeader);
        options.push(divider);
        options.push(canada);
        options.push(usa);
        options.push(mexico);
        dropDownProperties.Options = options;
        dropDownProperties.SelectedKey = "can";
        dropDown.Properties = dropDownProperties;

        const label = new PropertyPaneGroupField();
        label.TargetProperty = "label";
        label.Type = PropertyPaneGroupField.FieldType.Label;
        const labelProperties = new PropertyPaneLabelProperties();
        labelProperties.Text = "LABEL ONLY! (required)";
        labelProperties.Required = true;
        label.Properties = labelProperties;

        const slider = new PropertyPaneGroupField();
        slider.TargetProperty = "slider";
        slider.Type = PropertyPaneGroupField.FieldType.Slider;
        const sliderProperties = new PropertyPaneSliderProperties();
        sliderProperties.Label = "Opacity:";
        sliderProperties.Min = 0;
        sliderProperties.Max = 100;
        slider.Properties = sliderProperties;

        const choiceGroup = new PropertyPaneGroupField();
        choiceGroup.TargetProperty = "choice";
        choiceGroup.Type = PropertyPaneGroupField.FieldType.ChoiceGroup;
        const choiceGroupproperties = new PropertyPaneChoiceGroupProperties();
        choiceGroupproperties.Label = "Icon selection:";
        const choiceGroupOptions =  [];

        const sunny = new PropertyPaneChoiceGroupOption();
        sunny.IconProps = new PropertyPaneChoiceGroupIconProperties();
        sunny.IconProps.OfficeFabricIconFontName = "Sunny";
        sunny.Text = "Sun";
        sunny.Key = "sun";

        const plane = new PropertyPaneChoiceGroupOption();
        plane.IconProps = new PropertyPaneChoiceGroupIconProperties();
        plane.IconProps.OfficeFabricIconFontName = "Airplane";
        plane.Text = "plane";
        plane.Key = "AirPlane";

        choiceGroupOptions.push(sunny);
        choiceGroupOptions.push(plane);
        choiceGroupproperties.Options = choiceGroupOptions;
        choiceGroup.Properties = choiceGroupproperties;

        const horizontalRule = new PropertyPaneGroupField();
        horizontalRule.Type = PropertyPaneGroupField.FieldType.HorizontalRule;

        const link = new PropertyPaneGroupField();
        link.Type = PropertyPaneGroupField.FieldType.Link;
        const linkProperties = new PropertyPaneLinkProperties();
        linkProperties.Href = "https://www.bing.com";
        linkProperties.Text = "Bing";

        const popupProps = new PropertyPaneLinkPopupWindowProperties();
        popupProps.Width = 250;
        popupProps.Height = 250;
        popupProps.Title = "BING POPUP";
        popupProps.PositionWindowPosition = PropertyPaneLinkPopupWindowProperties.PopupWindowPosition.Center;

        linkProperties.PopupWindowProps = popupProps;
        link.Properties = linkProperties;


        const configurableFields = [
            text,
            primaryText,
            descriptionText,
        ];
        const dummyFields = [
            toggle,
            dropDown,
            label,
            slider,
            choiceGroup,
            horizontalRule,
            link
        ];

        configurableGroup.GroupFields = configurableFields;
        dummyGroup.GroupFields = dummyFields;

        const groups = [configurableGroup, dummyGroup];
        page.Groups = groups;

        const pages = [page];
        response.Pages = pages;

        return response; 
    }

    /**
     * Override this in a derived class to provide logic for setting configuration pane properties.
     * The bot will send back the properties that were changed in the property pane with
     * the key being the property name and the value being the new value of the property.
     * 
     * To access the properties that were changed use: context.activity.value.data.data
     * 
     * @param context - A strongly-typed context object for this turn
     * @param taskModuleRequest - The task module invoke request value payload
     * @returns A task module response for the request
     */
    async OnSharePointTaskSetPropertyPaneConfigurationAsync(context, taskModuleRequest){
        try {
            
            const primaryTextCardView = this.cardViewMap.get("PRIMARY_TEXT_CARD_VIEW");
            const changedProperties = context.activity.value.data;
            console.log(changedProperties);
            for (const property in changedProperties) {
                if (Object.prototype.hasOwnProperty.call(changedProperties, property)) {
                    switch (property){
                        
                        case "title":
                            primaryTextCardView.AceData.Title = changedProperties[property];
                            break;
                        case "primaryText":
                            primaryTextCardView.Data.PrimaryText = changedProperties[property];
                            break;
                        case "description":
                            primaryTextCardView.Data.Description = changedProperties[property];
                            break
                        default:
                            break;
                    }
                }
            }
            this.cardViewMap.set(primaryTextCardView.ViewId, primaryTextCardView);
            const response = new SetPropertyPaneConfigurationResponse();
            response.ReponseType = SetPropertyPaneConfigurationResponse.ResponseTypeOption.CardView;
            response.RenderArguments = primaryTextCardView;
            this.updatedView = response.RenderArguments;
            return response;
        } catch (error){
            console.log(error);
        }
    }

    async OnSharePointTaskHandleActionAsync(context, taskModuleRequest){
        const viewToNavigateTo = context.activity.value.data.data.viewToNavigateTo;
        if (viewToNavigateTo.includes('CARD')){
            const response = new HandleActionReponse();
            this.currentView = viewToNavigateTo;
            response.ReponseType = HandleActionReponse.ResponseTypeOption.CardView;
            response.RenderArguments = this.cardViewMap.get(viewToNavigateTo)
            return response;
        } else if (viewToNavigateTo.includes('QUICK')){
            const response = new HandleActionReponse();
            response.ReponseType = HandleActionReponse.ResponseTypeOption.QuickView;
            response.RenderArguments = this.quickViewMap.get(viewToNavigateTo)
            return response;
        }
    } 
    
    async createCardViews(){
        try {
            const basicCardView = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.BasicCardView);
            basicCardView.TemplateType = GetCardViewResponse.CardViewTemplateType.BasicCardView;
            basicCardView.ViewId = "BASIC_CARD_VIEW"

            const aceData = new AceData();
            aceData.CardSize = AceData.AceCardSize.Large;
            aceData.Title = "BOT DRIVEN ACE";
            aceData.Description= "bot description";
            aceData.DataVersion = "1.0";
            aceData.Id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f";
            basicCardView.AceData = aceData;

            const basicCardParameters = new BasicCardParameters();
            basicCardParameters.PrimaryText = "My bot's basic card";
            basicCardView.Data = basicCardParameters;

            const quickViewActionParameters = new QuickViewParameters();
            quickViewActionParameters.View = "BASIC_QUICK_VIEW";
            const quickViewAction = new SharepointAction();
            quickViewAction.Type = SharepointAction.ActionTypeOption.QuickView;
            quickViewAction.Parameters = quickViewActionParameters;
            basicCardView.OnCardSelection = quickViewAction;

            const viewNavAction = new SharepointAction();
            viewNavAction.Type = SharepointAction.ActionTypeOption.Execute;
            viewNavAction.Parameters = {
                "viewToNavigateTo": "IMAGE_CARD_VIEW"
            };

            const button = new ActionButton();
            button.Title = "Image View";
            button.Action = viewNavAction;

            const cardButtons = new Array();

            cardButtons[0] = button;

            basicCardView.CardButtons = cardButtons;

            this.cardViewMap.set(basicCardView.ViewId, basicCardView);
        } catch(error) {
            console.log(error);
        }
        try {
            const primaryTextCard = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.PrimaryTextCardView);
            primaryTextCard.TemplateType = GetCardViewResponse.CardViewTemplateType.PrimaryTextCardView;
            primaryTextCard.ViewId = "PRIMARY_TEXT_CARD_VIEW"

            const aceData = new AceData();
            aceData.CardSize = AceData.AceCardSize.Large;
            aceData.Title = "BOT DRIVEN ACE";
            aceData.Description= "bot description";
            aceData.DataVersion = "1.0";
            aceData.Id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f";
            primaryTextCard.AceData = aceData;

            const primaryTextCardParameters = new PrimaryTextCardParameters();
            primaryTextCardParameters.PrimaryText = "My bot's primary text card";
            primaryTextCardParameters.Description = "A nice description"
            primaryTextCard.Data = primaryTextCardParameters;

            const quickViewActionParameters = new QuickViewParameters();
            quickViewActionParameters.View = "PRIMARY_TEXT_QUICK_VIEW";
            const quickViewAction = new SharepointAction();
            quickViewAction.Type = SharepointAction.ActionTypeOption.QuickView;
            quickViewAction.Parameters = quickViewActionParameters;
            primaryTextCard.OnCardSelection = quickViewAction;

            const viewNavAction = new SharepointAction();
            viewNavAction.Type = SharepointAction.ActionTypeOption.Execute;
            viewNavAction.Parameters = {
                "viewToNavigateTo": "BASIC_CARD_VIEW"
            };
            
            const button = new ActionButton();
            button.Title = "Basic View";
            button.Action = viewNavAction;

            const cardButtons = new Array();

            cardButtons[0] = button;

            primaryTextCard.CardButtons = cardButtons;

            this.cardViewMap.set(primaryTextCard.ViewId, primaryTextCard);
        } catch(error) {
            console.log(error);
        }
        try {
            const imageCard = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.ImageCardView);
            imageCard.TemplateType = GetCardViewResponse.CardViewTemplateType.ImageCardView;
            imageCard.ViewId = "IMAGE_CARD_VIEW"

            const aceData = new AceData();
            aceData.CardSize = AceData.AceCardSize.Large;
            aceData.Title = "BOT DRIVEN ACE";
            aceData.Description= "bot description";
            aceData.DataVersion = "1.0";
            aceData.Id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f";
            imageCard.AceData = aceData;

            const imageCardParameters = new ImageCardParameters();
            imageCardParameters.ImageUrl = "https://download.logo.wine/logo/SharePoint/SharePoint-Logo.wine.png";
            imageCardParameters.ImageAltText = "Sharepoint logo";
            imageCardParameters.PrimaryText = "My bot's image card";
            imageCard.Data = imageCardParameters;

            const quickViewActionParameters = new QuickViewParameters();
            quickViewActionParameters.View = "IMAGE_QUICK_VIEW";
            const quickViewAction = new SharepointAction();
            quickViewAction.Type = SharepointAction.ActionTypeOption.QuickView;
            quickViewAction.Parameters = quickViewActionParameters
            imageCard.OnCardSelection = quickViewAction;

            const viewNavAction = new SharepointAction();
            viewNavAction.Type = SharepointAction.ActionTypeOption.Execute;
            viewNavAction.Parameters = {
                "viewToNavigateTo": "SIGN_IN_CARD_VIEW"
            };
            
            const button = new ActionButton();
            button.Title = "Sign In View";
            button.Action = viewNavAction;

            const cardButtons = new Array();

            cardButtons[0] = button;

            imageCard.CardButtons = cardButtons;

            this.cardViewMap.set(imageCard.ViewId, imageCard);
        } catch(error) {
            console.log(error);
        }
        try {
            const signInCard = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.SignInCardView);
            signInCard.TemplateType = GetCardViewResponse.CardViewTemplateType.SignInCardView;
            signInCard.ViewId = "SIGN_IN_CARD_VIEW"

            const aceData = new AceData();
            aceData.CardSize = AceData.AceCardSize.Large;
            aceData.Title = "BOT DRIVEN ACE";
            aceData.Description= "bot description";
            aceData.DataVersion = "1.0";
            aceData.Id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f";
            aceData.Properties = {
                uri: "placeholder",
                connectionName: "placeholder"
            }
            signInCard.AceData = aceData;

            const signInCardParameters = new SignInCardParameters();
            signInCardParameters.PrimaryText = "My bot's sign in card";
            signInCardParameters.Description = "Use this card to sign in";
            signInCardParameters.SignInButtonText = "Sign In";
            signInCardParameters.uri = "....";
            signInCardParameters.ConnectionName = "...";
            signInCard.Data = signInCardParameters;

            const quickViewActionParameters = new QuickViewParameters();
            quickViewActionParameters.View = "SIGN_IN_QUICK_VIEW";
            const quickViewAction = new SharepointAction();
            quickViewAction.Type = SharepointAction.ActionTypeOption.QuickView;
            quickViewAction.Parameters = quickViewActionParameters;
            signInCard.OnCardSelection = quickViewAction;

            const viewNavAction = new SharepointAction();
            viewNavAction.Type = SharepointAction.ActionTypeOption.Execute;
            viewNavAction.Parameters = {
                "viewToNavigateTo": "PRIMARY_TEXT_CARD_VIEW"
            };
            
            const button = new ActionButton();
            button.Title = "Primary Text View";
            button.Action = viewNavAction;

            const cardButtons = new Array();

            cardButtons[0] = button;

            signInCard.CardButtons = cardButtons;

            this.cardViewMap.set(signInCard.ViewId, signInCard);
        } catch(error) {
            console.log(error);
        }
    }

    async createQuickViews(){
        try {
            const basicQuickView = new GetQuickViewResponse();
            basicQuickView.ViewId = "BASIC_QUICK_VIEW";
            basicQuickView.Data = {};

            const template = new AdaptiveCards.AdaptiveCard();

            const container = new AdaptiveCards.Container();
            container.separator = true;
            container.selectAction = new AdaptiveCards.SubmitAction();
            container.selectAction.data = {
                viewToNavigateTo: "IMAGE_QUICK_VIEW"
            };
            const titleText = new AdaptiveCards.TextBlock();
            titleText.text = "BASIC CARD QUICK VIEW";
            titleText.color = AdaptiveCards.TextColor.Dark;
            titleText.weight = AdaptiveCards.TextWeight.Bolder;
            titleText.size = AdaptiveCards.TextSize.Large;
            titleText.wrap = true;
            titleText.maxLines = 1;
            titleText.spacing = AdaptiveCards.Spacing.None;
            container.addItem(titleText);

            const descriptionText = new AdaptiveCards.TextBlock();
            descriptionText.text = "This is the quick view for the basic card.";
            descriptionText.color = AdaptiveCards.TextColor.Dark;
            descriptionText.size = AdaptiveCards.TextSize.Medium;
            descriptionText.wrap = true;
            descriptionText.maxLines = 6;
            descriptionText.spacing = AdaptiveCards.Spacing.None;
            container.addItem(descriptionText);

            template.addItem(container); 
            basicQuickView.Template = template;
            basicQuickView.Title = "Basic Quick View"

            this.quickViewMap.set(basicQuickView.ViewId, basicQuickView)
        } catch(error) {
            console.log(error);
        }
        try {
            const primaryTextQuickView = new GetQuickViewResponse();
            primaryTextQuickView.ViewId = "PRIMARY_TEXT_QUICK_VIEW";
            primaryTextQuickView.Data = {};

            const template = new AdaptiveCards.AdaptiveCard();

            const container = new AdaptiveCards.Container();
            container.separator = true;
            container.selectAction = new AdaptiveCards.SubmitAction();
            container.selectAction.data = {
                viewToNavigateTo: "BASIC_QUICK_VIEW"
            };
            const titleText = new AdaptiveCards.TextBlock();
            titleText.text = "BENEFITS OF BOT ACES";
            titleText.color = AdaptiveCards.TextColor.Dark;
            titleText.weight = AdaptiveCards.TextWeight.Bolder;
            titleText.size = AdaptiveCards.TextSize.Large;
            titleText.wrap = true;
            titleText.maxLines = 1;
            titleText.spacing = AdaptiveCards.Spacing.None;
            container.addItem(titleText);

            const descriptionText = new AdaptiveCards.TextBlock();
            descriptionText.text = "When a Bot powers an Ace it allows you to customize the content of an Ace without deploying a new package, learning about the SPFX toolchain, or having to deploy updates to your customer sites.";
            descriptionText.color = AdaptiveCards.TextColor.Dark;
            descriptionText.size = AdaptiveCards.TextSize.Medium;
            descriptionText.wrap = true;
            descriptionText.maxLines = 6;
            descriptionText.spacing = AdaptiveCards.Spacing.None;
            container.addItem(descriptionText);

            template.addItem(container); 
            primaryTextQuickView.Template = template;
            primaryTextQuickView.Title = "Primary Text Quick View"

            this.quickViewMap.set(primaryTextQuickView.ViewId, primaryTextQuickView)
        } catch(error) {
            console.log(error);
        }
        try {
            const imageQuickView = new GetQuickViewResponse();
            imageQuickView.ViewId = "IMAGE_QUICK_VIEW";
            imageQuickView.Data = {};

            const template = new AdaptiveCards.AdaptiveCard();

            const container = new AdaptiveCards.Container();
            container.separator = true;
            container.selectAction = new AdaptiveCards.SubmitAction();
            container.selectAction.data = {
                viewToNavigateTo: "SIGN_IN_QUICK_VIEW"
            };
            const titleText = new AdaptiveCards.TextBlock();
            titleText.text = "IMAGE QUICK VIEW";
            titleText.color = AdaptiveCards.TextColor.Dark;
            titleText.weight = AdaptiveCards.TextWeight.Bolder;
            titleText.size = AdaptiveCards.TextSize.Large;
            titleText.wrap = true;
            titleText.maxLines = 1;
            titleText.spacing = AdaptiveCards.Spacing.None;
            container.addItem(titleText);

            const descriptionText = new AdaptiveCards.TextBlock();
            descriptionText.text = "This is the quick view for the image card.";
            descriptionText.color = AdaptiveCards.TextColor.Dark;
            descriptionText.size = AdaptiveCards.TextSize.Medium;
            descriptionText.wrap = true;
            descriptionText.maxLines = 6;
            descriptionText.spacing = AdaptiveCards.Spacing.None;
            container.addItem(descriptionText);

            template.addItem(container); 
            imageQuickView.Template = template;
            imageQuickView.Title = "Image Quick View"

            this.quickViewMap.set(imageQuickView.ViewId, imageQuickView)
        } catch(error) {
            console.log(error);
        }
        try {
            const signInQuickView = new GetQuickViewResponse();
            signInQuickView.ViewId = "SIGN_IN_QUICK_VIEW";
            signInQuickView.Data = {};

            const template = new AdaptiveCards.AdaptiveCard();

            const container = new AdaptiveCards.Container();
            container.separator = true;
            container.selectAction = new AdaptiveCards.SubmitAction();
            container.selectAction.data = {
                viewToNavigateTo: "PRIMARY_TEXT_QUICK_VIEW"
            };
            const titleText = new AdaptiveCards.TextBlock();
            titleText.text = "SIGN IN QUICK VIEW";
            titleText.color = AdaptiveCards.TextColor.Dark;
            titleText.weight = AdaptiveCards.TextWeight.Bolder;
            titleText.size = AdaptiveCards.TextSize.Large;
            titleText.wrap = true;
            titleText.maxLines = 1;
            titleText.spacing = AdaptiveCards.Spacing.None;
            container.addItem(titleText);

            const descriptionText = new AdaptiveCards.TextBlock();
            descriptionText.text = "This is the quick view for the sign in card.";
            descriptionText.color = AdaptiveCards.TextColor.Dark;
            descriptionText.size = AdaptiveCards.TextSize.Medium;
            descriptionText.wrap = true;
            descriptionText.maxLines = 6;
            descriptionText.spacing = AdaptiveCards.Spacing.None;
            container.addItem(descriptionText);

            template.addItem(container); 
            signInQuickView.Template = template;
            signInQuickView.Title = "Sign In Quick View"

            this.quickViewMap.set(signInQuickView.ViewId, signInQuickView)
        } catch(error) {
            console.log(error);
        }
    }
}



module.exports.SharepointMessagingExtensionsActionBot = SharepointMessagingExtensionsActionBot;
