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
using Microsoft.BotBuilderSamples.Helpers;
using Microsoft.BotBuilderSamples.Models;
using Microsoft.Bot.Schema.Teams;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class SharePointAdapativeCardExtensionBot : SharePointActivityHandler
    {
        public readonly string baseUrl;

        public SharePointAdapativeCardExtensionBot(IConfiguration configuration) : base()
        {
            this.baseUrl = configuration["BaseUrl"];
        }

        protected override Task<GetQuickViewResponse> OnSharePointTaskGetQuickViewAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            GetQuickViewResponse response = new GetQuickViewResponse();
            response.Data = new QuickViewData();
            response.Data.Title = "Bot quick view";
            response.Data.Description = "Bot description";
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

            response.ViewId = "qv1";
            response.StackSize = 1;
            return Task.FromResult(response);
        }

        protected override Task<GetCardViewResponse> OnSharePointTaskGetCardViewAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            GetCardViewResponse response = new GetCardViewResponse(GetCardViewResponse.CardViewTemplateType.PrimaryText);
            response.AceData = new AceData();
            response.AceData.CardSize = AceData.AceCardSize.Medium;
            response.AceData.Title = "Bot Ace Demo";
            response.AceData.DataVersion = "1.0";
            response.AceData.Id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f";
            response.Data = new CardViewData();
            response.Data.PrimaryText = "My Bot";
            response.ViewId = "view1";

            ActionButton button = new ActionButton();
            button.Title = "Details";
            button.Action = new Microsoft.Bot.Schema.SharePoint.Action();
            button.Action.Type = "QuickView";
            button.Action.Parameters = new ActionParameters();
            button.Action.Parameters.View = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f_QUICK_VIEW";

            List<ActionButton> actionButtons = new List<ActionButton>();
            actionButtons.Add(button);

            response.Data.ActionButtons = actionButtons;

            return Task.FromResult(response);
        }

        protected override Task<GetPropertyPaneConfigurationResponse> OnSharePointTaskGetPropertyPaneConfigurationAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
        // note that the following code is currently not used by the card either in rendering the card view nor the quick view.
        //however this is an example of the syntaxt that needs to be used to surface controls in the property pane.
            GetPropertyPaneConfigurationResponse response = new GetPropertyPaneConfigurationResponse();
            PropertyPanePage page = new PropertyPanePage();
            page.Header = new PropertyPanePageHeader();
            page.Header.Description = "Property pane for control";

            PropertyPaneGroup group = new PropertyPaneGroup();
            PropertyPaneGroupField text = new PropertyPaneGroupField();
            text.TargetProperty = "title";
            text.Type = PropertyPaneGroupField.FieldType.TextField;
            PropertyPaneTextFieldProperties textProperties = new PropertyPaneTextFieldProperties();
            textProperties.Value = "Bot Ace Demo";
            textProperties.Label = "Title";
            text.Properties = textProperties;

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
                text,
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

            List<PropertyPanePage> pages = new List<PropertyPanePage>();
            pages.Add(page);
            response.Pages = pages;

            return Task.FromResult(response);
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
    }
}

