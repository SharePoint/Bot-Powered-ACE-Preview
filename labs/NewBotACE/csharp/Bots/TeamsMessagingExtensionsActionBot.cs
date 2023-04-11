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
using System.Runtime;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Http;
using Microsoft.Graph;
using Attachment = Microsoft.Bot.Schema.Attachment;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Graph.ExternalConnectors;
using static System.Collections.Specialized.BitVector32;
using static Microsoft.Graph.Constants;
using Microsoft.Graph.CallRecords;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

namespace Microsoft.BotBuilderSamples.Bots
{
    public class ACEQuickViewTemplate
    {
        [JsonProperty("$schema")]
        public string schema { get; set; }

        [JsonProperty("type")]
        public string type { get; set; }

        [JsonProperty("version")]
        public string version { get; set; }

        [JsonProperty("body")]
        public object body { get; set; }
    }

    public class TeamsMessagingExtensionsActionBot : TeamsActivityHandler
    {
        public readonly string baseUrl;
        public readonly string connectionName;
        private readonly string signInQuickViewId = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f_COMPLETESIGNIN_QUICK_VIEW";

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
            this.connectionName = configuration["ConnectionName"];
        }

        #region Message Extensions
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
            var adaptiveCardJson = System.IO.File.ReadAllText(Path.Combine(paths));

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };
            return adaptiveCardAttachment;
        }

        #endregion

        protected override Task<TaskModuleResponse> OnTeamsTaskModuleFetchAsync(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            JObject activityObject = turnContext.Activity.Value as JObject;
            if (activityObject != null)
            {
                string activityValue = (string)((JValue)activityObject.Property("activity").Value).Value;
                if (activityValue == "cardView")
                {
                    return GetCardView(turnContext, taskModuleRequest, cancellationToken);
                }
                else if (activityValue == "quickView")
                {
                    return Task.FromResult(GenerateSignInQuickView());
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

        #region Card Views

        private async Task<TaskModuleResponse> GetCardView(ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            // var token = await TryToGetUserToken(null, turnContext, taskModuleRequest, cancellationToken);
            // var signInUser = null; // TryGetUser(token);
            // string cardView = signInUser == null ? GenerateSignInCardView(turnContext, cancellationToken) : GenerateCardView();
            return new TaskModuleResponse
            {
                Task = new TaskModuleMessageResponse
                {
                   Type = "result",
                   Value = GenerateSignInCardView(turnContext, cancellationToken)
                },
            };
        }

        private string GenerateSignInCardView(ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            // var signInLink = TryGetSignInLink(turnContext, cancellationToken);

            object aceData = new
            {
                cardSize = "Large",
                dataVersion = "1.0",
                id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f",
                iconProperty = "SharePointLogo",
                instanceId = "",
                properties = new { },
                title = "3P IDP Test",
                primaryText = "Please Sign In",
                description = "Testing sign in through sign in template for bots",
                signInButtonText = "Sign In",
                uri = "https://login.microsoft.com",
                connectionName
            };
            
            object quickViewButton = new
            {
                title = "Details",
                action = new
                {
                    type = "QuickView",
                    parameters = new
                    {
                        view = signInQuickViewId
                    }
                }
            };
            List<object> actionButtonsWrapper = new List<object> { quickViewButton };
            
            return JsonConvert.SerializeObject(new
            {
                aceData,
                templateType = "SignIn",
                data = new
                {
                    actionButtons = actionButtonsWrapper
                },
                viewId = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f"
            });
        }

        private string GenerateCardView()
        {
            object aceData = new
            {
                cardSize = "Medium",
                dataVersion = "1.0",
                id = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f",
                description = "This card is rendered from a bot",
                iconProperty = "SharePointLogo",
                instanceId = "",
                properties = new {},
                title = "Bot Ace Demo"
            };

            object quickViewButton = new
            {
                title = "Details",
                action = new
                {
                    type = "QuickView",
                    parameters = new
                    {
                        view = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f_QUICK_VIEW"
                    }
                }
            };
            List<object> actionButtonsWrapper = new List<object> { quickViewButton };

            return JsonConvert.SerializeObject(new
            {
                aceData,
                templateType = "PrimaryTextCardView",
                data = new
                {
                    actionButtons = actionButtonsWrapper
                },
                primaryText = "My Bot",
                viewId = "a1de36bb-9e9e-4b8e-81f8-853c3bba483f"
            });
        }

        #endregion

        #region Quick View
        private TaskModuleResponse GenerateSignInQuickView()
        {
            return new TaskModuleResponse
            {
                Task = new TaskModuleMessageResponse
                {
                    Type = "result",
                    Value = GenerateSignInQuickViewValue()
                }
            };
        }

        private string GenerateSignInQuickViewValue()
        {
            object title = new {
                type = "TextBlock",
                text = "Complete Sign In",
                color = "dark",
                weight = "Bolder",
                size = "large",
                wrap = true,
                maxLines = 1,
                spacing = "None"
            };
            object description = new
            {
                type = "TextBlock",
                text = "Input the magic code from signing into Azure Active Directory in order to continue.",
                color = "dark",
                size = "medium",
                wrap = true,
                maxLines = 6,
                spacing = "None"
            };
            object magicCodeInput = new
            {
                type = "Input.Number",
                placeholder = "Enter Magic Code",
                id = "magicCode",
                isRequired = true,
            };
            object actions = new
            {
                type = "ActionSet",
                actions = new List<object> {
                    new {
                        type = "Action.Submit",
                        title = "Complete Sign In"
                    }
                }
            };
            List<object> itemsWrapper = new List<object> { title, description, magicCodeInput, actions };
            
            object body = new
            {
                type = "Container",
                separator = true,
                items = itemsWrapper
            };
            List<object> bodyWrapper = new List<object> { body };

            return JsonConvert.SerializeObject(new
            {
                viewType = "QuickView",
                renderArguments = new
                {
                    viewId = signInQuickViewId,
                    data = new
                    {
                        title = "Title",
                        description = "description"
                    },
                    template = new ACEQuickViewTemplate
                    {
                        schema = "http://adaptivecards.io/schemas/adaptive-card.json",
                        type = "AdaptiveCard",
                        version = "1.2",
                        body = bodyWrapper
                    },
                }
            });
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
        #endregion

        #region Authentication
        private Task<TokenResponse> TryToGetUserToken(string magicCode, ITurnContext<IInvokeActivity> turnContext, TaskModuleRequest taskModuleRequest, CancellationToken cancellationToken)
        {
            var userTokenClient = turnContext.TurnState.Get<UserTokenClient>();
            return userTokenClient.GetUserTokenAsync(turnContext.Activity.From.Id, connectionName, turnContext.Activity.ChannelId, magicCode, cancellationToken);
        }

        private async Task<Graph.User> TryGetUser(TokenResponse response)
        {
            if (response != null && !string.IsNullOrEmpty(response.Token))
            {
                var client = new SimpleGraphClient(response.Token);
                return await client.GetMeAsync().ConfigureAwait(false);
            }

            return null;
        }

        private async Task<string> TryGetSignInLink(ITurnContext<IInvokeActivity> turnContext, CancellationToken cancellationToken)
        {
            var userTokenClient = turnContext.TurnState.Get<UserTokenClient>();
            var signInResource = await userTokenClient.GetSignInResourceAsync(connectionName, (Activity)turnContext.Activity, null, cancellationToken).ConfigureAwait(false);
            return signInResource.SignInLink;
        }

        #endregion

        #region Property Pane
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

        private TaskModuleResponse SetAceProperties(JObject aceProperties)
        {
            dynamic json = JsonConvert.DeserializeObject(TeamsMessagingExtensionsActionBot.cardView);
            foreach (dynamic property in aceProperties)
            {
                if (property.Key.Equals("title") || property.Key.Equals("description" ))
                {
                    json.aceData[property.Key] = aceProperties[property.Key];
                }
                else
                {
                    json.data[property.Key] = aceProperties[property.Key];
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
        #endregion
    }
}

