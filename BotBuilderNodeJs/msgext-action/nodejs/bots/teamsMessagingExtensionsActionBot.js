// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

require('dotenv').config();
const {  SharePointActivityHandler, CardFactory, TeamsInfo, MessageFactory } = require('botbuilder');
const { 
    GetCardViewResponse, 
    AceData, 
    CardViewData,
    ActionButton, 
    ActionParameters, 
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
    PropertyPaneLinkPopupWindowProperties
} = require('botframework-schema');
const AdaptiveCards = require("adaptivecards");
const baseurl = process.env.BaseUrl;

class TeamsMessagingExtensionsActionBot extends SharePointActivityHandler {
    
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
        try {
            const response = new GetQuickViewResponse();
            response.Data = {}
            response.Data.title = "BOT QUICK VIEW";
            response.Data.description = "BOT DESCRIPTION";
            response.Template = new AdaptiveCards.AdaptiveCard();

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

            response.Template.addItem(container); 

            response.ViewId = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f_QUICK_VIEW";
            response.StackSize = 1;

            return response;
        } catch(error) {
            console.log(error);
        }
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
            const changedProperties = context.activity.value.data;
            for (const property in changedProperties) {
                if (Object.prototype.hasOwnProperty.call(changedProperties, property)) {
                    switch (property){
                        
                        case "title":
                            this.cardViewResponse.AceData.Title = changedProperties[property];
                            break;
                        case "primaryText":
                            this.cardViewResponse.Data.PrimaryText = changedProperties[property];
                            break;
                        case "description":
                            this.cardViewResponse.Data.Description = changedProperties[property];
                            break
                        default:
                            break;
                    }
                }
            }

            return {
                viewType: 'Card',
                renderArguments: this.cardViewResponse
            };
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
}

// function GetJustInTimeCardAttachment() {
//     return CardFactory.adaptiveCard({
//         actions: [
//             {
//                 type: 'Action.Submit',
//                 title: 'Continue',
//                 data: { msteams: { justInTimeInstall: true } }
//             }
//         ],
//         body: [
//             {
//                 text: 'Looks like you have not used Action Messaging Extension app in this team/chat. Please click **Continue** to add this app.',
//                 type: 'TextBlock',
//                 wrap: true
//             }
//         ],
//         type: 'AdaptiveCard',
//         version: '1.0'
//     });
// }

// function GetAdaptiveCardAttachment() {
//     return CardFactory.adaptiveCard({
//         actions: [{ type: 'Action.Submit', title: 'Close' }],
//         body: [
//             {
//                 text: 'This app is installed in this conversation. You can now use it to do some great stuff!!!',
//                 type: 'TextBlock',
//                 isSubtle: false,
//                 wrap: true
//             }
//         ],
//         type: 'AdaptiveCard',
//         version: '1.0'
//     });
// }

// function createCardCommand(context, action) {
//     // The user has chosen to create a card by choosing the 'Create Card' context menu command.
//     const data = action.data;
//     const heroCard = CardFactory.heroCard(data.title, data.text);
//     heroCard.content.subtitle = data.subTitle;
//     const attachment = { contentType: heroCard.contentType, content: heroCard.content, preview: heroCard };

//     return {
//         composeExtension: {
//             type: 'result',
//             attachmentLayout: 'list',
//             attachments: [
//                 attachment
//             ]
//         }
//     };
// }

// function shareMessageCommand(context, action) {
//     // The user has chosen to share a message by choosing the 'Share Message' context menu command.
//     let userName = 'unknown';
//     if (action.messagePayload.from &&
//             action.messagePayload.from.user &&
//             action.messagePayload.from.user.displayName) {
//         userName = action.messagePayload.from.user.displayName;
//     }

//     // This Messaging Extension example allows the user to check a box to include an image with the
//     // shared message.  This demonstrates sending custom parameters along with the message payload.
//     let images = [];
//     const includeImage = action.data.includeImage;
//     if (includeImage === 'true') {
//         images = ['https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQtB3AwMUeNoq4gUBGe6Ocj8kyh3bXa9ZbV7u1fVKQoyKFHdkqU'];
//     }
//     const heroCard = CardFactory.heroCard(`${ userName } originally sent this message:`,
//         action.messagePayload.body.content,
//         images);

//     if (action.messagePayload.attachments && action.messagePayload.attachments.length > 0) {
//         // This sample does not add the MessagePayload Attachments.  This is left as an
//         // exercise for the user.
//         heroCard.content.subtitle = `(${ action.messagePayload.attachments.length } Attachments not included)`;
//     }

//     const attachment = { contentType: heroCard.contentType, content: heroCard.content, preview: heroCard };

//     return {
//         composeExtension: {
//             type: 'result',
//             attachmentLayout: 'list',
//             attachments: [
//                 attachment
//             ]
//         }
//     };
// }

// function empDetails() {
// 	console.log(baseurl);
//     return {
//         task: {
//             type: 'continue',
//             value: {
//                 width: 350,
//                 height: 300,
//                 title: 'Task module WebView',
//                 url: `${ baseurl }/customForm`
//             }
//         }
//     };
// }

// function dateTimeInfo() {
//     return {
//         task: {
//             type: 'continue',
//             value: {
//                 width: 450,
//                 height: 125,
//                 title: 'Task module Static HTML',
//                 url: `${ baseurl }/staticPage`
//             }
//         }
//     };
// }

// async function webViewResponse(action) {
//     // The user has chosen to create a card by choosing the 'Create Card' context menu command.
//     const data = await action.data;
//     const heroCard = CardFactory.heroCard(`ID: ${ data.EmpId }`, `E-Mail: ${ data.EmpEmail }`);
//     heroCard.content.subtitle = `Name: ${ data.EmpName }`;
//     const attachment = { contentType: heroCard.contentType, content: heroCard.content, preview: heroCard };
//     return {
//         composeExtension: {
//             type: 'result',
//             attachmentLayout: 'list',
//             attachments: [
//                 attachment
//             ]
//         }
//     };
// }





module.exports.TeamsMessagingExtensionsActionBot = TeamsMessagingExtensionsActionBot;
