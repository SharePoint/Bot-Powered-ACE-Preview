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
    SignInCardParameters
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
        return this.cardViewMap.get('PRIMARY_TEXT_CARD_VIEW');
        try {
            if(!this.cardViewResponse){
                this.cardViewResponse = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.PrimaryTextCardView);
                this.cardViewResponse.TemplateType = GetCardViewResponse.CardViewTemplateType.PrimaryTextCardView;
                this.cardViewResponse.ViewId = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f_CARD_VIEW"
                this.cardViewResponse.AceData = new AceData();
                this.cardViewResponse.AceData.CardSize = AceData.AceCardSize.Medium;
                this.cardViewResponse.AceData.Title = "BOT DRIVEN ACE";
                this.cardViewResponse.AceData.DataVersion = "1.0";
                this.cardViewResponse.AceData.Id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f";

                this.cardViewResponse.Data = new CardViewData();
                this.cardViewResponse.Data.PrimaryText = "My Bot!";
                const button = new ActionButton();
                button.Title = "DETAILS";
                button.Action = new SharepointAction();
                button.Action.Type = "QuickView";
                button.Action.Parameters = new ActionParameters();
                button.Action.Parameters.View =  "a1de36bb-9e9e-4b8e-81f8-853c3bba483f_QUICK_VIEW";

                const actionButtons = new Array();

                actionButtons[0] = button;

                this.cardViewResponse.Data.ActionButtons = actionButtons;

                return this.cardViewResponse;
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
    async OnSharePointTaskGetQuickViewAsync(context, taskModuleRequest){
        if (!this.quickViewsCreated){
            this.createQuickViews();
        }
        return this.quickViewMap.get('BASIC_QUICK_VIEW');
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

        const group = new PropertyPaneGroup();
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


        const fields = [
            text,
            primaryText,
            descriptionText,
            toggle,
            dropDown,
            label,
            slider,
            choiceGroup,
            horizontalRule,
            link
        ];

        group.GroupFields = fields;

        const groups = [group];
        page.Groups = groups;

        const pages = [page];
        response.Pages = pages;

        return JSON.stringify(response); 
    }

    /**
     * Override this in a derived class to provide logic for setting configuration pane properties.
     * 
     * @param context - A strongly-typed context object for this turn
     * @param taskModuleRequest - The task module invoke request value payload
     * @returns A task module response for the request
     */
    async OnSharePointTaskSetPropertyPaneConfigurationAsync(context, taskModuleRequest){
        try {
            const primaryTextCardView = this.cardViewMap.get("PRIMARY_TEXT_CARD_VIEW");
            const changedProperties = context.activity.value.data;
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
            return primaryTextCardView;
        } catch (error){
            console.log(error);
        }
        
        // dynamic json = JsonConvert.DeserializeObject(TeamsMessagingExtensionsActionBot.cardView);
        // foreach (dynamic property in aceProperties)
        // {
        //     if (property.Key.Equals("title") || property.Key.Equals("description" ))
        //     {
        //         json.aceData[property.Key] = aceProperties[property.Key];
        //     }
        //     else
        //     {
        //         json.data[property.Key] = aceProperties[property.Key];
        //     }
        return '';
    }
    
    async createCardViews(){
        try {
            const basicCardView = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.BasicCardView);
            basicCardView.TemplateType = GetCardViewResponse.CardViewTemplateType.BasicCardView;
            basicCardView.ViewId = "BASIC_CARD_VIEW"

            const aceData = new AceData();
            aceData.CardSize = AceData.AceCardSize.Medium;
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
            const action = new SharepointAction();
            action.Type = SharepointAction.ActionType.QuickView;
            action.Parameters = quickViewActionParameters;
            const button = new ActionButton();
            button.Title = "Quick View";
            button.Action = action;

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
            aceData.CardSize = AceData.AceCardSize.Medium;
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
            const action = new SharepointAction();
            action.Type = SharepointAction.ActionType.QuickView;
            action.Parameters = quickViewActionParameters;
            const button = new ActionButton();
            button.Title = "Quick View";
            button.Action = action;

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
            aceData.CardSize = AceData.AceCardSize.Medium;
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
            const action = new SharepointAction();
            action.Type = SharepointAction.ActionType.QuickView;
            action.Parameters = quickViewActionParameters;
            const button = new ActionButton();
            button.Title = "Quick View";
            button.Action = action;

            const cardButtons = new Array();

            cardButtons[0] = button;

            imageCard.CardButtons = cardButtons;
            imageCard.OnCardSelection = action;

            this.cardViewMap.set(imageCard.ViewId, imageCard);
        } catch(error) {
            console.log(error);
        }
        try {
            const signInCard = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.SignInCardView);
            signInCard.TemplateType = GetCardViewResponse.CardViewTemplateType.SignInCardView;
            signInCard.ViewId = "SIGN_IN_CARD_VIEW"

            const aceData = new AceData();
            aceData.CardSize = AceData.AceCardSize.Medium;
            aceData.Title = "BOT DRIVEN ACE";
            aceData.Description= "bot description";
            aceData.DataVersion = "1.0";
            aceData.Id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f";
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
            const action = new SharepointAction();
            action.Type = SharepointAction.ActionType.QuickView;
            action.Parameters = quickViewActionParameters;
            const button = new ActionButton();
            button.Title = "Quick View";
            button.Action = action;

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
            basicQuickView.StackSize = 1;
            basicQuickView.ViewId = "BASIC_QUICK_VIEW";
            basicQuickView.Data = {};

            const template = new AdaptiveCards.AdaptiveCard();

            const container = new AdaptiveCards.Container();
            container.separator = true;
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
            basicQuickView.Template = template;
            basicQuickView.Title = "Basic Quick View"

            this.quickViewMap.set(basicQuickView.ViewId, basicQuickView)
        } catch(error) {
            console.log(error);
        }
    }
}



module.exports.SharepointMessagingExtensionsActionBot = SharepointMessagingExtensionsActionBot;
