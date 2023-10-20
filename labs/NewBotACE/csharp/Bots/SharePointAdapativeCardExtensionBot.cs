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
using System.Diagnostics;
using System.Linq;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class SharePointAdapativeCardExtensionBot : SharePointActivityHandler
    {
        public readonly string baseUrl;
        private static Dictionary<string, CardViewResponse> cardViewDict;
        private static Dictionary<string, QuickViewResponse> quickViewDict;
        public bool cardViewsCreated = false;
        public bool quickViewsCreate = false;
        public string currentView = "";

        public SharePointAdapativeCardExtensionBot(IConfiguration configuration) : base()
        {
            this.baseUrl = configuration["BaseUrl"];
            SharePointAdapativeCardExtensionBot.cardViewDict = new Dictionary<string, CardViewResponse>();
            SharePointAdapativeCardExtensionBot.quickViewDict = new Dictionary<string, QuickViewResponse>();

            if (!SharePointAdapativeCardExtensionBot.cardViewDict.ContainsKey("PRIMARY_TEXT_CARD_VIEW"))
            {
                var aceData = new AceData()
                {
                    Title = "Bot Ace Demo",
                    CardSize = AceData.AceCardSize.Large,
                    DataVersion = "1.0",
                    Id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f"
                };

                Trace.Write("\n\n\nStarting to get card view.\n\n\n");
                // PRIMARY
                CardViewResponse primaryTextCard = new CardViewResponse();
                primaryTextCard.AceData = aceData;
                primaryTextCard.CardViewParameters = CardViewParameters.PrimaryTextCardViewParameters(
                    new CardBarComponent()
                    {
                        Id = "test"
                    },
                    new CardTextComponent()
                    {
                        Text = "My Bot"
                    },
                    new CardTextComponent()
                    {
                        Text = "This is the description of a bot"
                    },
                    new List<BaseCardComponent>()
                    {
                        new CardButtonComponent()
                        {
                            Title = "Basic view",
                            Style = CardButtonStyle.Positive,
                            Action = new SubmitAction()
                            {
                                Parameters = new Dictionary<string, object>()
                                {
                                    {"viewToNavigateTo", "BASIC_CARD_VIEW"}
                                }
                            }
                        },
                        new CardButtonComponent()
                        {
                            Title = "Primary input view",
                            Action = new SubmitAction()
                            {
                                Parameters = new Dictionary<string, object>()
                                {
                                    {"viewToNavigateTo", "PRIMARY_TEXT_CARD_VIEW_INPUT"}
                                }
                            }
                        }
                    });

                primaryTextCard.ViewId = "PRIMARY_TEXT_CARD_VIEW";

                primaryTextCard.OnCardSelection = new QuickViewAction()
                {
                    Parameters = new QuickViewActionParameters()
                    {
                        View = "PRIMARY_TEXT_QUICK_VIEW"
                    }
                };


                SharePointAdapativeCardExtensionBot.cardViewDict.Add(primaryTextCard.ViewId, primaryTextCard);

                // PRIMARY WITH INPUT
                CardViewResponse primaryTextInputCard = new CardViewResponse();
                primaryTextInputCard.AceData = aceData;
                primaryTextInputCard.CardViewParameters = CardViewParameters.PrimaryTextCardViewParameters(
                    new CardBarComponent(),
                    new CardTextComponent()
                    {
                        Text = "My Bot"
                    },
                    new CardTextComponent()
                    {
                        Text = "This is the description of a bot"
                    },
                    new List<BaseCardComponent>()
                    {
                        new CardTextInputComponent()
                        {
                            Placeholder = "placeholder",
                            IconBefore = new Bot.Schema.SharePoint.CardImage()
                            {
                                Image = "Send"
                            },
                            Button = new CardTextInputTitleButton()
                            {
                                Title = "Search",
                                Action = new SubmitAction()
                                {
                                    Parameters = new Dictionary<string, object>()
                                    {
                                        {"viewToNavigateTo", "BASIC_CARD_VIEW"}
                                    }
                                }
                            }
                        }
                    });

                primaryTextInputCard.ViewId = "PRIMARY_TEXT_CARD_VIEW_INPUT";

                primaryTextInputCard.OnCardSelection = new QuickViewAction()
                {
                    Parameters = new QuickViewActionParameters()
                    {
                        View = "PRIMARY_TEXT_QUICK_VIEW"
                    }
                };


                SharePointAdapativeCardExtensionBot.cardViewDict.Add(primaryTextInputCard.ViewId, primaryTextInputCard);

                // BASIC
                CardViewResponse basicCard = new CardViewResponse();
                basicCard.AceData = aceData;
                basicCard.CardViewParameters = CardViewParameters.BasicCardViewParameters(
                    new CardBarComponent(),
                    new CardTextComponent()
                    {
                        Text = "Basic Card"
                    },
                    new List<BaseCardComponent>()
                    {
                        new CardButtonComponent()
                        {
                            Title = "Image view",
                            Action = new SubmitAction()
                            {
                                Parameters = new Dictionary<string, object>()
                                {
                                    {"viewToNavigateTo", "IMAGE_CARD_VIEW"}
                                }
                            }
                        },
                        new CardButtonComponent()
                        {
                            Title = "Get media",
                            Action = new SelectMediaAction()
                            {
                                Parameters = new SelectMediaActionParameters()
                                {
                                    MediaType = SelectMediaActionParameters.MediaTypeOption.Audio
                                }
                            }
                        }
                    });

                basicCard.ViewId = "BASIC_CARD_VIEW";

                basicCard.OnCardSelection = new QuickViewAction()
                {
                    Parameters = new QuickViewActionParameters()
                    {
                        View = "BASIC_QUICK_VIEW"
                    }
                };
                SharePointAdapativeCardExtensionBot.cardViewDict.Add(basicCard.ViewId, basicCard);


                // IMAGE
                CardViewResponse imageCard = new CardViewResponse();
                imageCard.AceData = aceData;
                imageCard.CardViewParameters = CardViewParameters.ImageCardViewParameters(
                    new CardBarComponent(),
                    new CardTextComponent()
                    {
                        Text = "My bot's image card"
                    },
                    new List<BaseCardComponent>()
                    {
                        new CardButtonComponent()
                        {
                            Title = "Input view",
                            Action = new SubmitAction()
                            {
                                Parameters = new Dictionary<string, object>()
                                {
                                    {"viewToNavigateTo", "INPUT_CARD_VIEW"}
                                }
                            }
                        },
                        new CardButtonComponent()
                        {
                            Title = "Show location",
                            Action = new GetLocationAction()
                            {
                                Parameters = new GetLocationActionParameters()
                                {
                                    ChooseLocationOnMap = true
                                }
                            }
                        }
                    },
                    new Bot.Schema.SharePoint.CardImage()
                    {
                        Image = "https://download.logo.wine/logo/SharePoint/SharePoint-Logo.wine.png",
                        AltText = "SharePoint Logo"
                    });
                imageCard.ViewId = "IMAGE_CARD_VIEW";

                imageCard.OnCardSelection = new QuickViewAction()
                {
                    Parameters = new QuickViewActionParameters()
                    {
                        View = "IMAGE_QUICK_VIEW"
                    }
                };

                SharePointAdapativeCardExtensionBot.cardViewDict.Add(imageCard.ViewId, imageCard);

                // Input
                CardViewResponse inputCard = new CardViewResponse();
                inputCard.AceData = aceData;
                inputCard.CardViewParameters = CardViewParameters.TextInputCardViewParameters(
                    new CardBarComponent(),
                    new CardTextComponent()
                    {
                        Text = "My bot's input card"
                    },
                    new CardTextInputComponent()
                    {
                        DefaultValue = "Default"
                    },
                    new List<CardButtonComponent>()
                    {
                        new CardButtonComponent()
                        {
                            Title = "Sign in view",
                            Action = new SubmitAction()
                            {
                                Parameters = new Dictionary<string, object>()
                                {
                                    {"viewToNavigateTo", "SIGN_IN_CARD_VIEW"}
                                }
                            }
                        }
                    },
                    new Bot.Schema.SharePoint.CardImage()
                    {
                        Image = "https://download.logo.wine/logo/SharePoint/SharePoint-Logo.wine.png",
                        AltText = "SharePoint Logo"
                    });
                inputCard.ViewId = "INPUT_CARD_VIEW";

                inputCard.OnCardSelection = new QuickViewAction()
                {
                    Parameters = new QuickViewActionParameters()
                    {
                        View = "IMAGE_QUICK_VIEW"
                    }
                };

                SharePointAdapativeCardExtensionBot.cardViewDict.Add(inputCard.ViewId, inputCard);

                // Sign In
                CardViewResponse signInCard = new CardViewResponse();
                signInCard.AceData = aceData;
                dynamic props = new JObject();
                props.uri = "placeholder";
                props.connectionName = "placeholder";
                props.signInButtonText = "Sign in";
                signInCard.AceData.Properties = props;

                signInCard.CardViewParameters = CardViewParameters.SignInCardViewParameters(
                    new CardBarComponent(),
                    new CardTextComponent()
                    {
                        Text = "My bot's sign in card"
                    },
                    new CardTextComponent()
                    {
                        Text = "This is a sign in card template!"
                    },
                    new CardButtonComponent()
                    {
                        Title = "Primary text view",
                        Action = new SubmitAction()
                        {
                            Parameters = new Dictionary<string, object>(){
                                {"viewToNavigateTo", "PRIMARY_TEXT_CARD_VIEW"}
                            }
                        }
                    });

                signInCard.ViewId = "SIGN_IN_CARD_VIEW";

                signInCard.OnCardSelection = new QuickViewAction()
                {
                    Parameters = new QuickViewActionParameters()
                    {
                        View = "SIGN_IN_QUICK_VIEW"
                    }
                };

                SharePointAdapativeCardExtensionBot.cardViewDict.Add(signInCard.ViewId, signInCard);
                Trace.Write("\n\n\nCard views created!\n\n\n");
            }

        }

        protected override Task<CardViewResponse> OnSharePointTaskGetCardViewAsync(ITurnContext<IInvokeActivity> turnContext, AceRequest aceRequest, CancellationToken cancellationToken)
        {
            this.currentView = "PRIMARY_TEXT_CARD_VIEW";
            // Access the instanceId of your ACE here
            Trace.Write("\n\n\nHere is your ACE's instanceId! " + turnContext.Activity.Value + "\n\n\n");

            return Task.FromResult(SharePointAdapativeCardExtensionBot.cardViewDict["SIGN_IN_CARD_VIEW"]);
        }

        protected override Task<QuickViewResponse> OnSharePointTaskGetQuickViewAsync(ITurnContext<IInvokeActivity> turnContext, AceRequest aceRequest, CancellationToken cancellationToken)
        {
            Trace.Write("\n\n\nStarting to get quick view.\n\n\n");
            QuickViewResponse response = new QuickViewResponse();
            response.Title = "Primary Text quick view";
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
            Trace.Write("\n\n\nQuick View created.\n\n\n");
            return Task.FromResult(response);
        }

        protected override Task<GetPropertyPaneConfigurationResponse> OnSharePointTaskGetPropertyPaneConfigurationAsync(ITurnContext<IInvokeActivity> turnContext, AceRequest aceRequest, CancellationToken cancellationToken)
        {
            // note that the majority of the following code is currently not used by the card either in rendering the card view nor the quick view.
            // this is an example of the syntaxt that needs to be used to surface controls in the property pane.
            // However, setting the title, primary text, and description text fields will provide a sneak peek of applying property pane changes. 
            Trace.Write("\n\n\nStarting to create the Property Pane Configuration.\n\n\n");
            GetPropertyPaneConfigurationResponse response = new GetPropertyPaneConfigurationResponse();
            PropertyPanePage page = new PropertyPanePage();
            page.Header = new PropertyPanePageHeader();
            page.Header.Description = "Property pane for control";

            PropertyPaneGroup group = new PropertyPaneGroup();
            PropertyPaneGroupField titleText = new PropertyPaneGroupField();
            titleText.TargetProperty = "title";
            titleText.Type = PropertyPaneGroupField.FieldType.TextField;
            PropertyPaneTextFieldProperties titleTextProperties = new PropertyPaneTextFieldProperties();
            titleTextProperties.Label = "Title";
            titleTextProperties.Disabled = false;
            titleTextProperties.MaxLength = 255;
            titleText.Properties = titleTextProperties;

            PropertyPaneGroupField primaryText = new PropertyPaneGroupField();
            primaryText.TargetProperty = "primaryText";
            primaryText.Type = PropertyPaneGroupField.FieldType.TextField;
            PropertyPaneTextFieldProperties primaryTextProperties = new PropertyPaneTextFieldProperties();
            primaryTextProperties.Label = "Primary Text";
            primaryTextProperties.MaxLength = 255;
            primaryText.Properties = primaryTextProperties;

            PropertyPaneGroupField descriptionText = new PropertyPaneGroupField();
            descriptionText.TargetProperty = "description";
            descriptionText.Type = PropertyPaneGroupField.FieldType.TextField;
            PropertyPaneTextFieldProperties descriptionTextProperties = new PropertyPaneTextFieldProperties();
            descriptionTextProperties.Label = "Description Text";
            descriptionTextProperties.MaxLength = 255;
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

            Trace.Write("\n\n\nProperty Pane Configuration created.\n\n\n");
            return Task.FromResult(response);
        }

        protected override Task<BaseHandleActionResponse> OnSharePointTaskSetPropertyPaneConfigurationAsync(ITurnContext<IInvokeActivity> turnContext, AceRequest aceRequest, CancellationToken cancellationToken)
        {
            Trace.Write("\n\n\nStarting to set the Property Pane Configuration.\n\n\n");
            CardViewResponse primaryTextCardView = SharePointAdapativeCardExtensionBot.cardViewDict["PRIMARY_TEXT_CARD_VIEW"];

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
                        ((primaryTextCardView.CardViewParameters.Header.ToList())[0] as CardTextComponent).Text = aceProperties[property.Key];
                        break;
                    case "description":
                        ((primaryTextCardView.CardViewParameters.Body.ToList())[0] as CardTextComponent).Text = aceProperties[property.Key];
                        break;
                    default:
                        break;
                }
            }

            CardViewHandleActionResponse response = new CardViewHandleActionResponse();

            response.RenderArguments = primaryTextCardView;
            Trace.Write("\n\n\nFinished setting the Property Pane Configuration.\n\n\n");
            return Task.FromResult<BaseHandleActionResponse>(response);
        }

        protected override Task<BaseHandleActionResponse> OnSharePointTaskHandleActionAsync(ITurnContext<IInvokeActivity> turnContext, AceRequest aceRequest, CancellationToken cancellationToken)
        {
            if (turnContext != null)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            Trace.Write("\n\n\nStarted to handle action.\n\n\n");
            JObject actionParameters = (JObject)((JObject)turnContext.Activity.Value).Property("data").Value;

            if (actionParameters["type"].ToString().Equals("Submit"))
            {
                string viewToNavigateTo = actionParameters["data"]["viewToNavigateTo"].ToString();
                CardViewHandleActionResponse response = new CardViewHandleActionResponse();


                response.RenderArguments = SharePointAdapativeCardExtensionBot.cardViewDict[viewToNavigateTo];

                Trace.Write("\n\n\nFinished handling action.\n\n\n");
                return Task.FromResult<BaseHandleActionResponse>(response);
            }

            Trace.Write("\n\n\nFinished handling action.\n\n\n");
            return Task.FromResult<BaseHandleActionResponse>(new NoOpHandleActionResponse());
        }
    }
}

