// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

require("dotenv").config();
const {
  TeamsInfo,
  MessageFactory,
  SharePointActivityHandler,
} = require("botbuilder");
const {
  BasicCardView,
  PrimaryTextCardView,
  ImageCardView,
  SignInCardView,
  FieldType,
} = require("botframework-schema");
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
      case "createCard":
        return createCardCommand(context, action);
      case "shareMessage":
        return shareMessageCommand(context, action);
      case "webView":
        return await webViewResponse(action);
    }
  }

  async handleTeamsMessagingExtensionFetchTask(context, action) {
    switch (action.commandId) {
      case "webView":
        return empDetails();
      case "Static HTML":
        return dateTimeInfo();
      default:
        try {
          const member = await this.getSingleMember(context);
          return {
            task: {
              type: "continue",
              value: {
                card: GetAdaptiveCardAttachment(),
                height: 400,
                title: `Hello ${member}`,
                width: 300,
              },
            },
          };
        } catch (e) {
          if (e.code === "BotNotInConversationRoster") {
            return {
              task: {
                type: "continue",
                value: {
                  card: GetJustInTimeCardAttachment(),
                  height: 400,
                  title: "Adaptive Card - App Installation",
                  width: 300,
                },
              },
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
      if (e.code === "MemberNotFoundInConversation") {
        context.sendActivity(MessageFactory.text("Member not found."));
        return e.code;
      }
      throw e;
    }
  }

  /**
   * Override this in a derived class to provide logic for when a card view is fetched
   *
   * @param context - A strongly-typed context object for this turn
   * @param aceRequest - The Ace invoke request value payload
   * @returns A Card View Response for the request
   */
  async onSharePointTaskGetCardViewAsync(context, aceRequest) {
    console.log("Starting to get card view");
    //Instance id of your bot ACE will be accessible via the value below whenever a request is sent!
    console.log(context.activity.value.data);
    if (!this.cardViewsCreated) {
      this.createCardViews();
    }
    if (this.updatedView) {
      return this.updatedView;
    }
    this.currentView = "PRIMARY_TEXT_CARD_VIEW";
    console.log("Card view created!");
    return this.cardViewMap.get("PRIMARY_TEXT_CARD_VIEW");
  }

  /**
   * Override this in a derived class to provide logic for when a quick view is fetched
   *
   * @param context - A strongly-typed context object for this turn
   * @param aceRequest - The Ace invoke request value payload
   * @returns A Quick View Response for the request
   */
  async onSharePointTaskGetQuickViewAsync(context, aceRequest) {
    console.log("Starting to get quick view");
    if (!this.quickViewsCreated) {
      this.createQuickViews();
    }
    let quickViewId;
    if (this.currentView.includes("CARD")) {
      quickViewId = this.cardViewMap.get(this.currentView).onCardSelection
        .parameters.view;
    }
    console.log("Quick view created!");
    return this.quickViewMap.get(quickViewId);
  }

  /**
   * Override this in a derived class to provide logic for getting configuration pane properties.
   *
   * @param context - A strongly-typed context object for this turn
   * @param aceRequest - The Ace invoke request value payload
   * @returns A Property Pane Configuration Response for the request
   */
  async onSharePointTaskGetPropertyPaneConfigurationAsync(context, aceRequest) {
    console.log("Starting to create a Property Pane Configuration!");
    return {
      pages: [
        {
          header: {
            description: "Property pane for control",
          },
          groups: [
            {
              groupName: "Configurable Properties",
              groupFields: [
                {
                  type: FieldType.TextField,
                  targetProperty: "title",
                  properties: {
                    label: "Title",
                  },
                },
                {
                  type: FieldType.TextField,
                  targetProperty: "primaryText",
                  properties: {
                    label: "Primary Text",
                  },
                },
                {
                  type: FieldType.TextField,
                  targetProperty: "description",
                  properties: {
                    label: "Description",
                  },
                },
              ],
            },
            {
              // To make these properties 'configurable', edit the logic in OnSharePointTaskSetPropertyPaneConfigurationAsync
              groupName: "Nonconfigurable Props (see code!)",
              groupFields: [
                {
                  type: FieldType.Toggle,
                  targetProperty: "toggle",
                  properties: {
                    label: "Turn this feature on?",
                    key: "uniqueKey",
                  },
                },
                {
                  type: FieldType.Dropdown,
                  targetProperty: "dropdown",
                  properties: {
                    label: "Country",
                    options: [
                      {
                        type: "Header",
                        text: "Country",
                      },
                      {
                        type: "Divider",
                      },
                      {
                        type: "Normal",
                        text: "Canada",
                        key: "can",
                      },
                      {
                        type: "Normal",
                        text: "USA",
                        key: "US",
                      },
                      {
                        type: "Normal",
                        text: "Mexico",
                        key: "mex",
                      },
                    ],
                    selectedKey: "can",
                  },
                },
                {
                  type: FieldType.Label,
                  targetProperty: "label",
                  properties: {
                    text: "LABEL ONLY! (required)",
                    required: true,
                  },
                },
                {
                  type: FieldType.Slider,
                  targetProperty: "slider",
                  properties: {
                    label: "Opacity:",
                    min: 0,
                    max: 100,
                  },
                },
                {
                  type: FieldType.ChoiceGroup,
                  targetProperty: "choice",
                  properties: {
                    label: "Icon selection:",
                    options: [
                      {
                        iconProps: {
                          officeFabricIconFontName: "Sunny",
                        },
                        text: "Sun",
                        key: "sun",
                      },
                      {
                        iconProps: {
                          officeFabricIconFontName: "Airplane",
                        },
                        text: "plane",
                        key: "AirPlane",
                      },
                    ],
                  },
                },
                {
                  type: FieldType.HorizontalRule,
                },
                {
                  type: FieldType.Link,
                  properties: {
                    href: "https://www.bing.com",
                    text: "Bing",
                    popupWindowProps: {
                      width: 250,
                      height: 250,
                      title: "BING POPUP",
                      positionWindowPosition: "Center",
                    },
                  },
                },
              ],
            },
          ],
        },
      ],
    };
  }

  /**
   * Override this in a derived class to provide logic for setting configuration pane properties.
   * The bot will send back the properties that were changed in the property pane with
   * the key being the property name and the value being the new value of the property.
   *
   * To access the properties that were changed use: context.activity.value.data.data
   *
   * @param context - A strongly-typed context object for this turn
   * @param aceRequest - The Ace invoke request value payload
   * @returns A Card view or no-op action response
   */
  async onSharePointTaskSetPropertyPaneConfigurationAsync(context, aceRequest) {
    try {
      console.log("Starting to set properties!");
      const primaryTextCardView = this.cardViewMap.get(
        "PRIMARY_TEXT_CARD_VIEW"
      );
      const changedProperties = context.activity.value.data;
      for (const property in changedProperties) {
        if (Object.prototype.hasOwnProperty.call(changedProperties, property)) {
          switch (property) {
            case "title":
              primaryTextCardView.aceData.title = changedProperties[property];
              break;
            case "primaryText":
              primaryTextCardView.cardViewParameters.header[0].text =
                changedProperties[property];
              break;
            case "description":
              primaryTextCardView.cardViewParameters.body[0].text =
                changedProperties[property];
              break;
            default:
              break;
          }
        }
      }
      this.cardViewMap.set(primaryTextCardView.viewId, primaryTextCardView);
      console.log("Properties updated!");
      return {
        responseType: "Card",
        renderArguments: primaryTextCardView,
      };
    } catch (error) {
      console.log(error);
    }
  }

  /**
   * Override this in a derived class to provide logic for setting configuration pane properties.
   *
   * @param context - A strongly-typed context object for this turn
   * @param aceRequest - The Ace invoke request value payload
   * @returns A handle action response
   */
  async onSharePointTaskHandleActionAsync(context, aceRequest) {
    console.log("Starting to handle an action!");
    const viewToNavigateTo = context.activity.value.data.data.viewToNavigateTo;
    if (viewToNavigateTo.includes("CARD")) {
      console.log("Action handled!");
      return {
        responseType: "Card",
        renderArguments: this.cardViewMap.get(viewToNavigateTo),
      };
    } else if (viewToNavigateTo.includes("QUICK")) {
      console.log("Action handled!");
      return {
        responseType: "QuickView",
        renderArguments: this.quickViewMap.get(viewToNavigateTo),
      };
    }
  }

  async createCardViews() {
    const aceData = {
      cardSize: "Large",
      title: "BOT DRIVEN ACE",
      description: "bot description",
      dataVersion: "1.0",
      id: "a1de36bb-9e9e-4b8e-81f8-853c3bba483f",
    };

    try {
      const basicCardView = BasicCardView(
        {
          componentName: "cardBar",
        },
        {
          componentName: "text",
          text: "My bot's basic card",
        },
        [
          {
            componentName: "cardButton",
            title: "Image View",
            action: {
              type: "Execute",
              parameters: {
                viewToNavigateTo: "IMAGE_CARD_VIEW",
              },
            },
          },
        ]
      );

      // Card View Response
      const cardViewResponse = {
        viewId: "BASIC_CARD_VIEW",
        cardViewParameters: basicCardView,
        aceData: aceData,
        onCardSelection: {
          type: "QuickView",
          parameters: {
            view: "BASIC_QUICK_VIEW",
          },
        },
      };

      this.cardViewMap.set(cardViewResponse.viewId, cardViewResponse);
    } catch (error) {
      console.log(error);
    }

    try {
      const primaryTextCardViewResponse = {
        viewId: "PRIMARY_TEXT_CARD_VIEW",
        cardViewParameters: PrimaryTextCardView(
          {
            componentName: "cardBar",
          },
          {
            componentName: "text",
            text: "My bot's primary text card",
          },
          {
            componentName: "text",
            text: "A nice description",
          },
          [
            {
              componentName: "cardButton",
              title: "Basic View",
              action: {
                type: "Execute",
                parameters: {
                  viewToNavigateTo: "BASIC_CARD_VIEW",
                },
              },
            },
          ]
        ),
        aceData: aceData,
        onCardSelection: {
          type: "QuickView",
          parameters: {
            view: "PRIMARY_TEXT_QUICK_VIEW",
          },
        },
      };

      this.cardViewMap.set(
        primaryTextCardViewResponse.viewId,
        primaryTextCardViewResponse
      );
    } catch (error) {
      console.log(error);
    }
    try {
      const imageCardViewResponse = {
        viewId: "IMAGE_CARD_VIEW",
        cardViewParameters: ImageCardView(
          {
            componentName: "cardBar",
          },
          {
            componentName: "text",
            text: "My bot's image card",
          },
          {
            url: "https://download.logo.wine/logo/SharePoint/SharePoint-Logo.wine.png",
            altText: "Sharepoint logo",
          },
          [
            {
              componentName: "cardButton",
              title: "Sign In View",
              action: {
                type: "Execute",
                parameters: {
                  viewToNavigateTo: "SIGN_IN_CARD_VIEW",
                },
              },
            },
          ]
        ),
        aceData: aceData,
        onCardSelection: {
          type: "QuickView",
          parameters: {
            view: "IMAGE_QUICK_VIEW",
          },
        },
      };

      this.cardViewMap.set(imageCardViewResponse.viewId, imageCardViewResponse);
    } catch (error) {
      console.log(error);
    }
    try {
      const signInCardViewResponse = {
        viewId: "SIGN_IN_CARD_VIEW",
        cardViewParameters: SignInCardView(
          {
            componentName: "cardBar",
          },
          {
            componentName: "text",
            text: "My bot's sign in card",
          },
          {
            componentName: "text",
            text: "Use this card to sign in",
          },
          {
            componentName: "cardButton",
            title: "Primary Text View",
            action: {
              type: "Execute",
              parameters: {
                viewToNavigateTo: "PRIMARY_TEXT_QUICK_VIEW",
              },
            },
          }
        ),
        aceData: {
          ...aceData,
          properties: {
            uri: "placeholder",
            connectionName: "placeholder",
            signInButtonText: "Sign In",
          },
        },
        onCardSelection: {
          type: "QuickView",
          parameters: {
            view: "SIGN_IN_QUICK_VIEW",
          },
        },
      };

      this.cardViewMap.set(
        signInCardViewResponse.viewId,
        signInCardViewResponse
      );
    } catch (error) {
      console.log(error);
    }
  }

  async createQuickViews() {
    try {
      const basicQuickViewResponse = {
        viewId: "BASIC_QUICK_VIEW",
        data: {},
        title: "Basic Quick View",
      };

      const template = new AdaptiveCards.AdaptiveCard();

      const container = new AdaptiveCards.Container();
      container.separator = true;
      container.selectAction = new AdaptiveCards.SubmitAction();
      container.selectAction.data = {
        viewToNavigateTo: "IMAGE_QUICK_VIEW",
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
      basicQuickViewResponse.template = template;

      this.quickViewMap.set(
        basicQuickViewResponse.viewId,
        basicQuickViewResponse
      );
    } catch (error) {
      console.log(error);
    }
    try {
      const primaryTextQuickViewResponse = {
        viewId: "PRIMARY_TEXT_QUICK_VIEW",
        data: {},
        title: "Primary Text Quick View",
      };

      const template = new AdaptiveCards.AdaptiveCard();

      const container = new AdaptiveCards.Container();
      container.separator = true;
      container.selectAction = new AdaptiveCards.SubmitAction();
      container.selectAction.data = {
        viewToNavigateTo: "BASIC_QUICK_VIEW",
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
      descriptionText.text =
        "When a Bot powers an Ace it allows you to customize the content of an Ace without deploying a new package, learning about the SPFX toolchain, or having to deploy updates to your customer sites.";
      descriptionText.color = AdaptiveCards.TextColor.Dark;
      descriptionText.size = AdaptiveCards.TextSize.Medium;
      descriptionText.wrap = true;
      descriptionText.maxLines = 6;
      descriptionText.spacing = AdaptiveCards.Spacing.None;
      container.addItem(descriptionText);

      template.addItem(container);
      primaryTextQuickViewResponse.template = template;

      this.quickViewMap.set(
        primaryTextQuickViewResponse.viewId,
        primaryTextQuickViewResponse
      );
    } catch (error) {
      console.log(error);
    }
    try {
      const imageQuickViewResponse = {
        viewId: "IMAGE_QUICK_VIEW",
        data: {},
        title: "Image Quick View",
      };

      const template = new AdaptiveCards.AdaptiveCard();

      const container = new AdaptiveCards.Container();
      container.separator = true;
      container.selectAction = new AdaptiveCards.SubmitAction();
      container.selectAction.data = {
        viewToNavigateTo: "SIGN_IN_QUICK_VIEW",
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
      imageQuickViewResponse.template = template;

      this.quickViewMap.set(
        imageQuickViewResponse.viewId,
        imageQuickViewResponse
      );
    } catch (error) {
      console.log(error);
    }
    try {
      const signInQuickViewResponse = {
        viewId: "SIGN_IN_QUICK_VIEW",
        data: {},
        title: "Sign In Quick View",
      };

      const template = new AdaptiveCards.AdaptiveCard();

      const container = new AdaptiveCards.Container();
      container.separator = true;
      container.selectAction = new AdaptiveCards.SubmitAction();
      container.selectAction.data = {
        viewToNavigateTo: "PRIMARY_TEXT_QUICK_VIEW",
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
      signInQuickViewResponse.template = template;

      this.quickViewMap.set(
        signInQuickViewResponse.viewId,
        signInQuickViewResponse
      );
    } catch (error) {
      console.log(error);
    }
  }
}

module.exports.SharepointMessagingExtensionsActionBot =
  SharepointMessagingExtensionsActionBot;
