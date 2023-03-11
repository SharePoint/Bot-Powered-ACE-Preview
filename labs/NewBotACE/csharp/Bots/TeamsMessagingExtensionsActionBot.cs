// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AdaptiveCards;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Teams;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.BotBuilderSamples.Helpers;
using Microsoft.BotBuilderSamples.Models;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class TeamsMessagingExtensionsActionBot : TeamsActivityHandler
    {
        public readonly string baseUrl;

        private static string cardView = @"{
                        ""aceData"" : {
                            ""cardSize"": ""Medium"",
                            ""dataVersion"": ""1.0"",
                            ""id"": ""a1de36bb-9e9e-4b8e-81f8-853c3bba483f"",
                            ""description"": ""This card is rendered from a bot"",
                            ""iconProperty"": ""SharePointLogo"",
                            ""instanceId"": """",
                            ""properties"": {},
                            ""title"": ""Bot Ace Demo""
                        },
                        ""templateType"": ""PrimaryTextCardView"",
                        ""data"": {
                          ""actionButtons"": [
                            {
                              ""title"": ""Details"",
                              ""action"": {
                                ""type"": ""QuickView"",
                                ""parameters"": {
                                    ""view"": ""a1de36bb-9e9e-4b8e-81f8-853c3bba483f_QUICK_VIEW""
                                }
                              }
                            }
                          ],
                          ""primaryText"": ""My Bot""
                        }
         }";

        public TeamsMessagingExtensionsActionBot(IConfiguration configuration) : base()
        {
            this.baseUrl = configuration["BaseUrl"];
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

            var attachments = new List<MessagingExtensionAttachment>();
            attachments.Add(new MessagingExtensionAttachment
            {
                Content = card,
                ContentType = HeroCard.ContentType,
                Preview = card.ToAttachment(),
            });

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

            var attachments = new List<MessagingExtensionAttachment>();
            attachments.Add(new MessagingExtensionAttachment
            {
                Content = card,
                ContentType = HeroCard.ContentType,
                Preview = card.ToAttachment(),
            });

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
            var imgUrl = baseUrl + "/profile-image.png";

            var card = new ThumbnailCard
            {
                Title = "ID: " + cardData.EmpId,
                Subtitle = "Name: " + cardData.EmpName,
                Text = "E-Mail: " + cardData.EmpEmail,
                Images = new List<CardImage> { new CardImage { Url = imgUrl } },
            };

            var attachments = new List<MessagingExtensionAttachment>();
            attachments.Add(new MessagingExtensionAttachment
            {
                Content = card,
                ContentType = ThumbnailCard.ContentType,
                Preview = card.ToAttachment(),
            });

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
                        Url = baseUrl + "/Home/RazorView",
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
                        Url = baseUrl + "/Home/HtmlPage",
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
                        Url = baseUrl + "/Home/CustomForm",
                    },
                },
            };
            return response;
        }

        private static Attachment GetAdaptiveCardAttachmentFromFile(string fileName)
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
        }
        protected override Task<TaskModuleResponse> OnTeamsTaskModuleFetchAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            JObject activityObject = turnContext.Activity.Value as JObject;
            if (activityObject != null)
            {
                string activityValue = (string)((JValue)activityObject.Property("activity").Value).Value;
                if (activityValue == "cardView")
                {
                    return Task.FromResult(GetCardView());
                }
                else if (activityValue == "quickView")
                {
                    return Task.FromResult(GetQuickView());
                }
                else if (activityValue == "propertyPaneConfiguration")
                {
                    return Task.FromResult(GetPropertyPaneConfiguration());
                }
                else if (activityValue == "setAceProperties")
                {
                    JObject aceProperties = (JObject)activityObject.Property("data").Value;
                    return Task.FromResult(SetAceProperties(aceProperties));
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

        private TaskModuleResponse GetCardView()
        {
            return new TaskModuleResponse
            {
                Task = new TaskModuleMessageResponse
                {
                   Type = "result",
                    Value = TeamsMessagingExtensionsActionBot.cardView
                },
            };
        }

        private TaskModuleResponse GetQuickView()
        {
            return new TaskModuleResponse
            {
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
        }

        private TaskModuleResponse GetPropertyPaneConfiguration()
        {
            return new TaskModuleResponse
            {
                Task = new TaskModuleMessageResponse
                {
                    Type = "result",
                    Value = @"{
                      ""pages"": [
                        {
                            ""header"": {
                                ""description"": ""Property Pane for My Bot""
                            },
                            ""groups"": [
                                {
                                    ""groupFields"": [
                                        {
                                            ""type"": 3,
                                            ""targetProperty"": ""title"",
                                            ""properties"": {
                                                ""label"": ""Title"",
                                                ""value"": ""Bot Ace Demo""
                                            }
                                        },
                                        {
                                            ""type"": 3,
                                            ""targetProperty"": ""description"",
                                            ""properties"": {
                                                ""label"": ""Description"",
                                                ""value"": ""This card is rendered from a bot""
                                            }
                                        }
                                    ]
                                }
                            ]
                        }
                      ]
                    }"
                },
            };
        }

        private TaskModuleResponse SetAceProperties(JObject test)
        {
            dynamic json = JsonConvert.DeserializeObject(TeamsMessagingExtensionsActionBot.cardView);
            foreach (dynamic property in test)
            {
                if (property.Key.Equals("title") || property.Key.Equals("description"))
                {
                    json.aceData[property.Key] = test[property.Key];
                }
                else
                {
                    json.data[property.Key] = test[property.Key];
                }
            }
            TeamsMessagingExtensionsActionBot.cardView = JsonConvert.SerializeObject(json);
            return new TaskModuleResponse
            {
                Task = new TaskModuleMessageResponse
                {
                    Type = "result",
                    Value = TeamsMessagingExtensionsActionBot.cardView
                },
            };
        }

    }
}

