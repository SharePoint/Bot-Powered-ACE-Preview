// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.SharePoint;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.SharePoint;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Bot.Schema.Teams;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class SharePointAdapativeCardExtensionBot : SharePointActivityHandler
    {
        public readonly string baseUrl;
        private static Dictionary<string, ICardViewResponse> cardViewDict;
        private static Dictionary<string, GetQuickViewResponse> quickViewDict;
        public bool cardViewsCreated = false;
        public bool quickViewsCreate = false;
        public string currentView = "";

        public SharePointAdapativeCardExtensionBot(IConfiguration configuration) : base()
        {
            this.baseUrl = configuration["BaseUrl"];
            SharePointAdapativeCardExtensionBot.cardViewDict = new Dictionary<string, ICardViewResponse>();
            SharePointAdapativeCardExtensionBot.quickViewDict = new Dictionary<string, GetQuickViewResponse>();

            if (!SharePointAdapativeCardExtensionBot.cardViewDict.ContainsKey("PRIMARY_TEXT_CARD_VIEW"))
            {
                PrimaryTextCardViewResponse primaryTextCard = new PrimaryTextCardViewResponse();
                primaryTextCard.AceData = new AceData();
                primaryTextCard.AceData.CardSize = AceData.AceCardSize.Large;
                primaryTextCard.AceData.Title = "Bot Ace Demo";
                primaryTextCard.AceData.DataVersion = "1.0";
                primaryTextCard.AceData.Id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f";
                primaryTextCard.Data = new PrimaryTextCardParameters()
                {
                    PrimaryText = "My Bot",
                    Description = "This is the description of a bot"
                };
                primaryTextCard.ViewId = "PRIMARY_TEXT_CARD_VIEW";

                primaryTextCard.OnCardSelection = new QuickViewAction()
                {
                    Parameters = new QuickViewActionParameters()
                    {
                        View = "PRIMARY_TEXT_QUICK_VIEW"
                    }
                };

                ActionButton primaryTextButton = new ActionButton();
                primaryTextButton.Title = "Basic View";
                SubmitAction primaryTextSubmitAction = new SubmitAction();
                primaryTextSubmitAction.Parameters = new Dictionary<string, object>(){
                    {"viewToNavigateTo", "BASIC_CARD_VIEW"}
                };
                primaryTextButton.Action = primaryTextSubmitAction;

                ActionButton primaryTextButton2= new ActionButton();
                primaryTextButton2.Title="sic View";
                primaryTextButton2.Action = primaryTextSubmitAction;

                ActionButton primaryTextButton3=new ActionButton();
                primaryTextButton3.Title = "Basic View";
                primaryTextButton3.Action = primaryTextSubmitAction;

                List<ActionButton> actionButtons = new List<ActionButton>
                {
                    primaryTextButton, 
                    primaryTextButton2,
                    primaryTextButton3
                };

                primaryTextCard.CardButtons = actionButtons;
                SharePointAdapativeCardExtensionBot.cardViewDict.Add(primaryTextCard.ViewId, primaryTextCard);

                // BASIC
                BasicCardViewResponse basicCard = new BasicCardViewResponse();
                basicCard.AceData = new AceData();
                basicCard.AceData.CardSize = AceData.AceCardSize.Large;
                basicCard.AceData.Title = "BOT ACE DEMO";
                basicCard.AceData.Description = "BOT ACE DESCRIPTION";
                basicCard.AceData.DataVersion = "1.0";
                basicCard.AceData.Id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f";
                basicCard.Data = new BasicCardParameters()
                {
                    PrimaryText = "Basic Card",
                };

                basicCard.ViewId = "BASIC_CARD_VIEW";

                basicCard.OnCardSelection = new QuickViewAction()
                {
                    Parameters = new QuickViewActionParameters()
                    {
                        View = "BASIC_QUICK_VIEW"
                    }
                };

                ActionButton basicButton = new ActionButton();
                basicButton.Title = "Image View";
                SubmitAction basicSubmitAction = new SubmitAction();
                basicSubmitAction.Parameters = new Dictionary<string, object>(){
                    {"viewToNavigateTo", "IMAGE_CARD_VIEW"}
                };
                basicButton.Action = basicSubmitAction;

                List<ActionButton> basicActionButtons = new List<ActionButton>
                {
                    basicButton
                };

                basicCard.CardButtons = basicActionButtons;
                SharePointAdapativeCardExtensionBot.cardViewDict.Add(basicCard.ViewId, basicCard);

                ImageCardViewResponse imageCard = new ImageCardViewResponse();
                imageCard.AceData = new AceData();
                imageCard.AceData.CardSize = AceData.AceCardSize.Large;
                imageCard.AceData.Title = "BOT ACE DEMO";
                imageCard.AceData.Description = "BOT ACE DESCRIPTION";
                imageCard.AceData.DataVersion = "1.0";
                imageCard.AceData.Id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f";
                imageCard.Data = new ImageCardParameters()
                {
                    PrimaryText = "My bot's image card",
                    ImageUrl = "https://download.logo.wine/logo/SharePoint/SharePoint-Logo.wine.png",
                    ImageAltText = "Sharepoint logo"
                };

                imageCard.ViewId = "IMAGE_CARD_VIEW";

                imageCard.OnCardSelection = new QuickViewAction()
                {
                    Parameters = new QuickViewActionParameters()
                    {
                        View = "IMAGE_QUICK_VIEW"
                    }
                };

                ActionButton imageButton = new ActionButton();
                imageButton.Title = "Sign In View";
                SubmitAction imageSubmitAction = new SubmitAction();
                imageSubmitAction.Parameters = new Dictionary<string, object>(){
                    {"viewToNavigateTo", "SIGN_IN_CARD_VIEW"}
                };
                imageButton.Action = imageSubmitAction;

                List<ActionButton> imageActionButtons = new List<ActionButton>
                {
                    imageButton
                };

                imageCard.CardButtons = imageActionButtons;
                SharePointAdapativeCardExtensionBot.cardViewDict.Add(imageCard.ViewId, imageCard);

                // Sign In
                SignInCardViewResponse signInCard = new SignInCardViewResponse();
                signInCard.AceData = new AceData();
                signInCard.AceData.CardSize = AceData.AceCardSize.Large;
                signInCard.AceData.Title = "BOT ACE DEMO";
                signInCard.AceData.Description = "BOT ACE DESCRIPTION";
                signInCard.AceData.DataVersion = "1.0";
                signInCard.AceData.Id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f";
                dynamic props = new JObject();
                props.uri = "placeholder";
                props.connectionName = "placeholder";
                signInCard.AceData.Properties = props;

                signInCard.Data = new SignInCardParameters()
                {
                    PrimaryText = "My bot's sign in card",
                    SignInButtonText = "Sign in",
                    Description = "This is a sign in card template!"
                };

                signInCard.ViewId = "SIGN_IN_CARD_VIEW";

                signInCard.OnCardSelection = new QuickViewAction()
                {
                    Parameters = new QuickViewActionParameters()
                    {
                        View = "SIGN_IN_QUICK_VIEW"
                    }
                };

                ActionButton signInButton = new ActionButton();
                signInButton.Title = "Primary Text View";
                SubmitAction signInSubmitAction = new SubmitAction();
                signInSubmitAction.Parameters = new Dictionary<string, object>(){
                    {"viewToNavigateTo", "PRIMARY_TEXT_CARD_VIEW"}
                };
                signInButton.Action = signInSubmitAction;

                List<ActionButton> signInActionButtons = new List<ActionButton>
                {
                    signInButton
                };

                signInCard.CardButtons = signInActionButtons;
                SharePointAdapativeCardExtensionBot.cardViewDict.Add(signInCard.ViewId, signInCard);
            }

        }

        protected override Task<ICardViewResponse> OnSharePointTaskGetCardViewAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            this.currentView = "PRIMARY_TEXT_CARD_VIEW";

            return Task.FromResult(SharePointAdapativeCardExtensionBot.cardViewDict["PRIMARY_TEXT_CARD_VIEW"]);
        }

        protected override Task<GetQuickViewResponse> OnSharePointTaskGetQuickViewAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            GetQuickViewResponse response = new GetQuickViewResponse();
            response.Title =  "Primary Text quick view";
            response.Template = new AdaptiveCard();

            AdaptiveContainer container = new AdaptiveContainer();
            container.Separator = true;
            AdaptiveTextBlock titleText = new AdaptiveTextBlock();
            titleText.Text = "Benefits of Bot Aces";
            titleText.Color = AdaptiveTextColor.Dark;
            titleText.Weight = AdaptiveTextWeight.Bolder;
            titleText.Size = AdaptiveTextSize.Large;
            titleText.Wrap = true;
            titleText.MaxLines = 1;
            titleText.Spacing = AdaptiveSpacing.None;
            container.Items.Add(titleText);

            AdaptiveTextBlock descriptionText = new AdaptiveTextBlock();
            descriptionText.Text = "When a Bot powers an Ace it allows you to customize the content of an Ace without deploying a new package, learning about the SPFX toolchain, or having to deploy updates to your customer sites.";
            descriptionText.Color = AdaptiveTextColor.Dark;
            descriptionText.Size = AdaptiveTextSize.Medium;
            descriptionText.Wrap = true;
            descriptionText.MaxLines = 6;
            descriptionText.Spacing = AdaptiveSpacing.None;
            container.Items.Add(descriptionText);

            response.Template.Body.Add(container);

            response.ViewId = "PRIMARY_TEXT_QUICK_VIEW";
            return Task.FromResult(response);
        }

        protected override Task<GetPropertyPaneConfigurationResponse> OnSharePointTaskGetPropertyPaneConfigurationAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
        // note that the majority of the following code is currently not used by the card either in rendering the card view nor the quick view.
        // this is an example of the syntaxt that needs to be used to surface controls in the property pane.
        // However, setting the title, primary text, and description text fields will provide a sneak peek of applying property pane changes. 
            GetPropertyPaneConfigurationResponse response = new GetPropertyPaneConfigurationResponse();
            PropertyPanePage page = new PropertyPanePage();
            page.Header = new PropertyPanePageHeader();
            page.Header.Description = "Property pane for control";

            PropertyPaneGroup group = new PropertyPaneGroup();
            PropertyPaneGroupField titleText = new PropertyPaneGroupField();
            titleText.TargetProperty = "title";
            titleText.Type = PropertyPaneGroupField.FieldType.TextField;
            PropertyPaneTextFieldProperties titleTextProperties = new PropertyPaneTextFieldProperties();
            titleTextProperties.Value = "Bot Ace Demo";
            titleTextProperties.Label = "Title";
            titleTextProperties.Disabled = false;
            titleTextProperties.MaxLength = 10;
            titleText.Properties = titleTextProperties;

            PropertyPaneGroupField primaryText = new PropertyPaneGroupField();
            primaryText.TargetProperty = "primaryText";
            primaryText.Type = PropertyPaneGroupField.FieldType.TextField;
            PropertyPaneTextFieldProperties primaryTextProperties = new PropertyPaneTextFieldProperties();
            primaryTextProperties.Value = "My Bot's primary text";
            primaryTextProperties.Label = "Primary Text";
            primaryTextProperties.MaxLength = 10;
            primaryText.Properties = primaryTextProperties;

            PropertyPaneGroupField descriptionText = new PropertyPaneGroupField();
            descriptionText.TargetProperty = "description";
            descriptionText.Type = PropertyPaneGroupField.FieldType.TextField;
            PropertyPaneTextFieldProperties descriptionTextProperties = new PropertyPaneTextFieldProperties();
            descriptionTextProperties.Value = "My Bot's description";
            descriptionTextProperties.Label = "Description Text";
            descriptionTextProperties.MaxLength = 10;
            descriptionText.Properties = descriptionTextProperties;

            PropertyPaneGroupField toggle = new PropertyPaneGroupField();
            toggle.TargetProperty = "toggle";
            toggle.Type = PropertyPaneGroupField.FieldType.Toggle;
            PropertyPaneToggleProperties toggleProperties = new PropertyPaneToggleProperties();
            toggleProperties.Label = "Turn this feature on?";
            toggleProperties.Key = "uniqueKey";
            toggle.Properties = toggleProperties;

            PropertyPaneGroupField dropDown = new PropertyPaneGroupField();
            dropDown.TargetProperty = "dropdown";
            dropDown.Type = PropertyPaneGroupField.FieldType.Dropdown;
            PropertyPaneDropDownProperties dropDownProperties = new PropertyPaneDropDownProperties();
            List<PropertyPaneDropDownOption> options = new List<PropertyPaneDropDownOption>();
            PropertyPaneDropDownOption countryHeader = new PropertyPaneDropDownOption();
            countryHeader.Type = PropertyPaneDropDownOption.DropDownOptionType.Header;
            countryHeader.Text = "Country";

            PropertyPaneDropDownOption divider = new PropertyPaneDropDownOption();
            divider.Type = PropertyPaneDropDownOption.DropDownOptionType.Divider;

            PropertyPaneDropDownOption canada = new PropertyPaneDropDownOption();
            canada.Type = PropertyPaneDropDownOption.DropDownOptionType.Normal;
            canada.Text = "Canada";
            canada.Key = "can";

            PropertyPaneDropDownOption usa = new PropertyPaneDropDownOption();
            usa.Text = "USA";
            usa.Key = "US";

            PropertyPaneDropDownOption mexico = new PropertyPaneDropDownOption();
            mexico.Type = PropertyPaneDropDownOption.DropDownOptionType.Normal;
            mexico.Text = "Mexico";
            mexico.Key = "mex";

            options.Add(countryHeader);
            options.Add(divider);
            options.Add(canada);
            options.Add(usa);
            options.Add(mexico);
            dropDownProperties.Options = options;
            dropDownProperties.SelectedKey = "can";
            dropDown.Properties = dropDownProperties;

            PropertyPaneGroupField label = new PropertyPaneGroupField();
            label.TargetProperty = "label";
            label.Type = PropertyPaneGroupField.FieldType.Label;
            PropertyPaneLabelProperties labelProperties = new PropertyPaneLabelProperties();
            labelProperties.Text = "LABEL ONLY! (required)";
            labelProperties.Required = true;
            label.Properties = labelProperties;

            PropertyPaneGroupField slider = new PropertyPaneGroupField();
            slider.TargetProperty = "slider";
            slider.Type = PropertyPaneGroupField.FieldType.Slider;
            PropertyPaneSliderProperties sliderProperties = new PropertyPaneSliderProperties();
            sliderProperties.Label = "Opacity:";
            sliderProperties.Min = 0;
            sliderProperties.Max = 100;
            slider.Properties = sliderProperties;

            PropertyPaneGroupField choiceGroup = new PropertyPaneGroupField();
            choiceGroup.TargetProperty = "choice";
            choiceGroup.Type = PropertyPaneGroupField.FieldType.ChoiceGroup;
            PropertyPaneChoiceGroupProperties choiceGroupproperties = new PropertyPaneChoiceGroupProperties();
            choiceGroupproperties.Label = "Icon selection:";
            List<PropertyPaneChoiceGroupOption> choiceGroupOptions = new List<PropertyPaneChoiceGroupOption>();

            PropertyPaneChoiceGroupOption sunny = new PropertyPaneChoiceGroupOption();
            sunny.IconProps = new PropertyPaneChoiceGroupIconProperties();
            sunny.IconProps.OfficeFabricIconFontName = "Sunny";
            sunny.Text = "Sun";
            sunny.Key = "sun";

            PropertyPaneChoiceGroupOption plane = new PropertyPaneChoiceGroupOption();
            plane.IconProps = new PropertyPaneChoiceGroupIconProperties();
            plane.IconProps.OfficeFabricIconFontName = "Airplane";
            plane.Text = "plane";
            plane.Key = "AirPlane";

            choiceGroupOptions.Add(sunny);
            choiceGroupOptions.Add(plane);
            choiceGroupproperties.Options = choiceGroupOptions;
            choiceGroup.Properties = choiceGroupproperties;

            PropertyPaneGroupField horizontalRule = new PropertyPaneGroupField();
            horizontalRule.Type = PropertyPaneGroupField.FieldType.HorizontalRule;

            PropertyPaneGroupField link = new PropertyPaneGroupField();
            link.Type = PropertyPaneGroupField.FieldType.Link;
            PropertyPaneLinkProperties linkProperties = new PropertyPaneLinkProperties();
            linkProperties.Href = "https://www.bing.com";
            linkProperties.Text = "Bing";

            PropertyPaneLinkPopupWindowProperties popupProps = new PropertyPaneLinkPopupWindowProperties();
            popupProps.Width = 250;
            popupProps.Height = 250;
            popupProps.Title = "BING POPUP";
            popupProps.PositionWindowPosition = PropertyPaneLinkPopupWindowProperties.PopupWindowPosition.Center;

            linkProperties.PopupWindowProps = popupProps;
            link.Properties = linkProperties;


            List<PropertyPaneGroupField> fields = new List<PropertyPaneGroupField>()
            {
                titleText,
                primaryText,
                descriptionText,
                toggle,
                dropDown,
                label,
                slider,
                choiceGroup,
                horizontalRule,
                link
            };

            group.GroupFields = fields;

            List<PropertyPaneGroup> groups = new List<PropertyPaneGroup>()
            {
                group
            };
            page.Groups = groups;

            List<PropertyPanePage> pages = new List<PropertyPanePage>
            {
                page
            };
            response.Pages = pages;

            return Task.FromResult(response);
        }

        protected override Task<SetPropertyPaneConfigurationResponse> OnSharePointTaskSetPropertyPaneConfigurationAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            PrimaryTextCardViewResponse primaryTextCardView = SharePointAdapativeCardExtensionBot.cardViewDict["PRIMARY_TEXT_CARD_VIEW"] as PrimaryTextCardViewResponse;

            JObject activityObject = turnContext.Activity.Value as JObject;
            JObject aceProperties = (JObject)activityObject.Property("data").Value;

            foreach (dynamic property in aceProperties)
            {
                switch (property.Key)
                {
                    case "title":
                        primaryTextCardView.AceData.Title = aceProperties[property.Key];
                        break;
                    case "primaryText":
                        (primaryTextCardView.Data as PrimaryTextCardParameters).PrimaryText = aceProperties[property.Key];
                        break;
                    case "description":
                        (primaryTextCardView.Data as PrimaryTextCardParameters).Description = aceProperties[property.Key];
                        break;
                    default:
                        break;
                }
            }

            SetPropertyPaneConfigurationResponse response = new SetPropertyPaneConfigurationResponse();
            response.ResponseType = SetPropertyPaneConfigurationResponse.ResponseTypeOption.CardView;
            response.RenderArguments = primaryTextCardView;
            return Task.FromResult(response);
        }

        protected override Task<HandleActionResponse> OnSharePointTaskHandleActionAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            if (turnContext != null)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            JObject actionParameters = (JObject)((JObject)turnContext.Activity.Value).Property("data").Value;

            if (actionParameters["type"].ToString().Equals("Submit"))
            {
                string viewToNavigateTo = actionParameters["data"]["viewToNavigateTo"].ToString();
                HandleActionResponse response = new HandleActionResponse();
                response.ResponseType = HandleActionResponse.ResponseTypeOption.CardView;

                
                response.RenderArguments = SharePointAdapativeCardExtensionBot.cardViewDict[viewToNavigateTo];

                return Task.FromResult(response);
            }

            return Task.FromResult(new HandleActionResponse());
        }
    }
}

