// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
using Microsoft.AspNetCore.Components;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.SharePoint;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.SharePoint;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Bot.Streaming.Payloads;
using Microsoft.BotBuilderSamples.Helpers;
using Microsoft.BotBuilderSamples.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class SharePointBotDrivenAceActionBot : SharePointActivityHandler
    {
        private static int index = 0;
        private readonly string _baseUrl;

        public SharePointBotDrivenAceActionBot(IConfiguration configuration) 
            : base()
        {
            this._baseUrl = configuration["BaseUrl"];
        }

        protected override Task<GetQuickViewResponse> OnSharePointTaskGetQuickViewAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            GetQuickViewResponse response = new GetQuickViewResponse();
            response.Title = "BOT QUICK VIEW";
            response.Template = new AdaptiveCard();

            AdaptiveContainer container = new AdaptiveContainer();
            container.Separator = true;
            AdaptiveTextBlock titleText = new AdaptiveTextBlock();
            titleText.Text = "BENEFITS OF BOT ACES";
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
            return Task.FromResult(response);
        }

        /*
                Task = new TaskModuleMessageResponse
                {
                    Type = "result",
                    Value = @"{
  ""data"": {
    ""title"": ""Bot quick view"",
    ""description"": ""Bot description""
  },
  ""template"": {
    ""$schema"": ""http://adaptivecards.io/schemas/adaptive-card.json"",
    ""type"": ""AdaptiveCard"",
    ""version"": ""1.2"",
    ""body"": [
      {
        ""type"": ""Container"",
        ""separator"": true,
        ""items"": [
          {
            ""type"": ""TextBlock"",
            ""text"": ""Benefits of Bot Aces"",
            ""color"": ""dark"",
            ""weight"": ""Bolder"",
            ""size"": ""large"",
            ""wrap"": true,
            ""maxLines"": 1,
            ""spacing"": ""None""
          },
          {
            ""type"": ""TextBlock"",
            ""text"": ""When a Bot powers an Ace it allows you to customize the content of an Ace without deploying a new package, learning about the SPFX toolchain, or having to deploy updates to your customer sites."",
            ""color"": ""dark"",
            ""wrap"": true,
            ""size"": ""medium"",
            ""maxLines"": 6,
            ""spacing"": ""None""
          }
        ]
      }
    ]
  },
  ""viewId"": """",
  ""viewStackSize"": 1
}"
                },
            };
        }*/

        protected override Task<ICardViewResponse> OnSharePointTaskGetCardViewAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            PrimaryTextCardViewResponse response = new PrimaryTextCardViewResponse();
            response.AceData = new AceData();
            response.AceData.CardSize = AceData.AceCardSize.Medium;
            response.AceData.Title = "BOT DRIVEN ACE";
            response.AceData.DataVersion = "1.0";
            response.AceData.Id = "<App ID>";
            response.Data = new PrimaryTextCardParameters()
            {
                PrimaryText = "MY BOT " + SharePointBotDrivenAceActionBot.index++.ToString()
            };
            response.ViewId = "view1";

            ActionButton button = new ActionButton();
            button.Title = "DETAILS";
            button.Action = new QuickViewAction()
            {
                Parameters = new QuickViewActionParameters() { View = "appid_QUICK_VIEW" }
            };

            List<ActionButton> actionButtons = new List<ActionButton>
            {
                button
            };

            response.CardButtons = actionButtons;

            return Task.FromResult(response as ICardViewResponse);
        }

        /*
        protected override Task<TaskModuleResponse> OnTeamsTaskModuleFetchAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            JObject activityObject = turnContext.Activity.Value as JObject;
            if (activityObject != null)
            {
                string activityValue = (string)((JValue)activityObject.Property("activity").Value).Value;
                if (activityValue == "cardView")
                {
                    TaskModuleResponse resp = GetCardView();
                    return Task.FromResult(resp);
                }
                else if (activityValue == "quickView")
                {
                    return Task.FromResult(GetQuickView());
                }
            }

            // Return empty for now;
            return Task.FromResult(new TaskModuleResponse
            {
                Task = new TaskModuleMessageResponse
                {
                    Type = "result",
                    Value = "{}"
                }
            });
        }

        protected override async Task<MessagingExtensionActionResponse> OnTeamsMessagingExtensionSubmitActionAsync(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action, CancellationToken cancellationToken)
        {
            switch (action.CommandId)
            {
                case "createCard":
                    return CreateCardCommand(turnContext, action);
                case "shareMessage":
                    return ShareMessageCommand(turnContext, action);
                case "webView":
                    return WebViewResponse(turnContext, action);
                case "createAdaptiveCard":
                    return CreateAdaptiveCardResponse(turnContext, action);
                case "razorView":
                    return RazorViewResponse(turnContext, action);
                case "HTML":
                    return ShareHTMLCard(turnContext, action);
            }
            return await Task.FromResult(new MessagingExtensionActionResponse());
        }*/

        protected override Task<GetPropertyPaneConfigurationResponse> OnSharePointTaskGetPropertyPaneConfigurationAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
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

            List<PropertyPanePage> pages = new List<PropertyPanePage>
            {
                page
            };
            response.Pages = pages;

            return Task.FromResult(response); 
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

        private MessagingExtensionActionResponse RazorViewResponse(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action)
        {
            // The user has chosen to create a card by choosing the 'Create Card' context menu command.
            RazorViewResponse cardData = JsonConvert.DeserializeObject<RazorViewResponse>(action.Data.ToString());
            var card = new HeroCard
            {
                Title = "Requested User: " + turnContext.Activity.From.Name,
                Text = cardData.DisplayData,
            };

            var attachments = new List<MessagingExtensionAttachment>
            {
                new MessagingExtensionAttachment
                {
                    Content = card,
                    ContentType = HeroCard.ContentType,
                    Preview = card.ToAttachment(),
                }
            };

            return new MessagingExtensionActionResponse
            {
                ComposeExtension = new MessagingExtensionResult
                {
                    AttachmentLayout = "list",
                    Type = "result",
                    Attachments = attachments,
                },
            };
        }

        private MessagingExtensionActionResponse CreateCardCommand(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action)
        {
            // The user has chosen to create a card by choosing the 'Create Card' context menu command.
            var createCardData = ((JObject)action.Data).ToObject<CardResponse>();

            var card = new HeroCard
            {
                Title = createCardData.Title,
                Subtitle = createCardData.Subtitle,
                Text = createCardData.Text,
            };

            var attachments = new List<MessagingExtensionAttachment>
            {
                new MessagingExtensionAttachment
                {
                    Content = card,
                    ContentType = HeroCard.ContentType,
                    Preview = card.ToAttachment(),
                }
            };

            return new MessagingExtensionActionResponse
            {
                ComposeExtension = new MessagingExtensionResult
                {
                    AttachmentLayout = "list",
                    Type = "result",
                    Attachments = attachments,
                },
            };
        }

        private MessagingExtensionActionResponse ShareMessageCommand(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action)
        {
            // The user has chosen to share a message by choosing the 'Share Message' context menu command.
            var heroCard = new HeroCard
            {
                Title = $"{action.MessagePayload.From?.User?.DisplayName} orignally sent this message:",
                Text = action.MessagePayload.Body.Content,
            };

            if (action.MessagePayload.Attachments != null && action.MessagePayload.Attachments.Count > 0)
            {
                // This sample does not add the MessagePayload Attachments.  This is left as an
                // exercise for the user.
                heroCard.Subtitle = $"({action.MessagePayload.Attachments.Count} Attachments not included)";
            }

            // This Messaging Extension example allows the user to check a box to include an image with the
            // shared message.  This demonstrates sending custom parameters along with the message payload.
            var includeImage = ((JObject)action.Data)["includeImage"]?.ToString();
            if (string.Equals(includeImage, bool.TrueString, StringComparison.OrdinalIgnoreCase))
            {
                heroCard.Images = new List<CardImage>
                {
                    new CardImage { Url = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQtB3AwMUeNoq4gUBGe6Ocj8kyh3bXa9ZbV7u1fVKQoyKFHdkqU" },
                };
            }

            return new MessagingExtensionActionResponse
            {
                ComposeExtension = new MessagingExtensionResult
                {
                    Type = "result",
                    AttachmentLayout = "list",
                    Attachments = new List<MessagingExtensionAttachment>()
                    {
                        new MessagingExtensionAttachment
                        {
                            Content = heroCard,
                            ContentType = HeroCard.ContentType,
                            Preview = heroCard.ToAttachment(),
                        },
                    },
                },
            };
        }

        private MessagingExtensionActionResponse WebViewResponse(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action)
        {
            // The user has chosen to create a card by choosing the 'Web View' context menu command.
            CustomFormResponse cardData = JsonConvert.DeserializeObject<CustomFormResponse>(action.Data.ToString());
            var imgUrl = _baseUrl + "/profile-image.png";

            var card = new ThumbnailCard
            {
                Title = "ID: " + cardData.EmpId,
                Subtitle = "Name: " + cardData.EmpName,
                Text = "E-Mail: " + cardData.EmpEmail,
                Images = new List<CardImage> { new CardImage { Url = imgUrl } },
            };

            var attachments = new List<MessagingExtensionAttachment>
            {
                new MessagingExtensionAttachment
                {
                    Content = card,
                    ContentType = ThumbnailCard.ContentType,
                    Preview = card.ToAttachment(),
                }
            };

            return new MessagingExtensionActionResponse
            {
                ComposeExtension = new MessagingExtensionResult
                {
                    AttachmentLayout = "list",
                    Type = "result",
                    Attachments = attachments,
                },
            };
        }

        private MessagingExtensionActionResponse CreateAdaptiveCardResponse(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action)
        {
            var createCardResponse = ((JObject)action.Data).ToObject<CardResponse>();
            var attachments = CardHelper.CreateAdaptiveCardAttachment(action, createCardResponse);

            return new MessagingExtensionActionResponse
            {
                ComposeExtension = new MessagingExtensionResult
                {
                    AttachmentLayout = "list",
                    Type = "result",
                    Attachments = attachments,
                },
            };
        }

        private MessagingExtensionActionResponse ShareHTMLCard(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action)
        {
            var createCardResponse = ((JObject)action.Data).ToObject<CardResponse>();
            var attachments = CardHelper.CreateAdaptiveCardAttachmentForHTML(action, createCardResponse);

            return new MessagingExtensionActionResponse
            {
                ComposeExtension = new MessagingExtensionResult
                {
                    AttachmentLayout = "list",
                    Type = "result",
                    Attachments = attachments,
                },
            };
        }

        /*
        protected override async Task<MessagingExtensionActionResponse> OnTeamsMessagingExtensionFetchTaskAsync(
            ITurnContext<IInvokeActivity> turnContext, 
            MessagingExtensionAction action, 
            CancellationToken cancellationToken)
        {
            switch (action.CommandId)
            {
                case "webView":
                    return EmpDetails(turnContext, action);
                case "HTML":
                    return TaskModuleHTMLPage(turnContext, action);
                case "razorView":
                    return DateDayInfo(turnContext, action);
                default:
                    // we are handling two cases within try/catch block 
                    //if the bot is installed it will create adaptive card attachment and show card with input fields
                    string memberName;
                    try
                    {
                        // Check if your app is installed by fetching member information.
                        var member = await TeamsInfo.GetMemberAsync(turnContext, turnContext.Activity.From.Id, cancellationToken);
                        memberName = member.Name;
                    }
                    catch (ErrorResponseException ex)
                    {
                        if (ex.Body.Error.Code == "BotNotInConversationRoster")
                        {
                            return new MessagingExtensionActionResponse
                            {
                                Task = new TaskModuleContinueResponse
                                {
                                    Value = new TaskModuleTaskInfo
                                    {
                                        Card = GetAdaptiveCardAttachmentFromFile("justintimeinstallation.json"),
                                        Height = 200,
                                        Width = 400,
                                        Title = "Adaptive Card - App Installation",
                                    },
                                },
                            };
                        }
                        throw; // It's a different error.
                    }

                    return new MessagingExtensionActionResponse
                    {
                        Task = new TaskModuleContinueResponse
                        {
                            Value = new TaskModuleTaskInfo
                            {
                                Card = GetAdaptiveCardAttachmentFromFile("adaptiveCard.json"),
                                Height = 200,
                                Width = 400,
                                Title = $"Welcome {memberName}",
                            },
                        },
                    };
            }
        }
        */

        private MessagingExtensionActionResponse DateDayInfo(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action)
        {
            var response = new MessagingExtensionActionResponse()
            {
                Task = new TaskModuleContinueResponse()
                {
                    Value = new TaskModuleTaskInfo()
                    {
                        Height = 175,
                        Width = 300,
                        Title = "Task Module Razor View",
                        Url = _baseUrl + "/Home/RazorView",
                    },
                },
            };
            return response;
        }

        private MessagingExtensionActionResponse TaskModuleHTMLPage(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action)
        {
            var response = new MessagingExtensionActionResponse()
            {
                Task = new TaskModuleContinueResponse()
                {
                    Value = new TaskModuleTaskInfo()
                    {
                        Height = 200,
                        Width = 400,
                        Title = "Task Module HTML Page",
                        Url = _baseUrl + "/Home/HtmlPage",
                    },
                },
            };
            return response;
        }

        private MessagingExtensionActionResponse EmpDetails(ITurnContext<IInvokeActivity> turnContext, MessagingExtensionAction action)
        {
            var response = new MessagingExtensionActionResponse()
            {
                Task = new TaskModuleContinueResponse()
                {
                    Value = new TaskModuleTaskInfo()
                    {
                        Height = 300,
                        Width = 450,
                        Title = "Task Module WebView",
                        Url = _baseUrl + "/Home/CustomForm",
                    },
                },
            };
            return response;
        }

        /*private static Attachment GetAdaptiveCardAttachmentFromFile(string fileName)
        {
            //Read the card json and create attachment.
            string[] paths = { ".", "Resources", fileName };
            var adaptiveCardJson = File.ReadAllText(Path.Combine(paths));

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };
            return adaptiveCardAttachment;
        }*/
    }
}
