﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Adapters;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Builder.Dialogs.Prompts;
using Microsoft.Bot.Schema;
using Microsoft.Recognizers.Text;
using Xunit;
using static Microsoft.Bot.Builder.Dialogs.Prompts.PromptCultureModels;

namespace Microsoft.Bot.Builder.Dialogs.Tests
{
    public class ConfirmPromptLocTests
    {
        /// <summary>
        /// Generates an Enumerable of variations on all supported locales.
        /// </summary>
        /// <returns>An iterable collection of objects.</returns>
        public static IEnumerable<object[]> GetLocaleVariationTest()
        {
            var testLocales = new TestLocale[]
            {
                new TestLocale(Bulgarian, "(1) да или (2) Не", "да", "Не"),
                new TestLocale(Chinese, "(1) 是的 要么 (2) 不", "是的", "不"),
                new TestLocale(Dutch, "(1) Ja of (2) Nee", "Ja", "Nee"),
                new TestLocale(English, "(1) Yes or (2) No", "Yes", "No"),
                new TestLocale(French, "(1) Oui ou (2) Non", "Oui", "Non"),
                new TestLocale(German, "(1) Ja oder (2) Nein", "Ja", "Nein"),
                new TestLocale(Hindi, "(1) हां या (2) नहीं", "हां", "नहीं"),
                new TestLocale(Italian, "(1) Si o (2) No", "Si", "No"),
                new TestLocale(Japanese, "(1) はい または (2) いいえ", "はい", "いいえ"),
                new TestLocale(Korean, "(1) 예 또는 (2) 아니", "예", "아니"),
                new TestLocale(Portuguese, "(1) Sim ou (2) Não", "Sim", "Não"),
                new TestLocale(Spanish, "(1) Sí o (2) No", "Sí", "No"),
                new TestLocale(Swedish, "(1) Ja eller (2) Nej", "Ja", "Nej"),
                new TestLocale(Turkish, "(1) Evet veya (2) Hayır", "Evet", "Hayır")
            };

            foreach (var locale in testLocales)
            {
                yield return new object[] { locale.ValidLocale, locale.ValidLocale, locale.ExpectedPrompt, locale.InputThatResultsInOne, "1" };
                yield return new object[] { locale.ValidLocale, locale.ValidLocale, locale.ExpectedPrompt, locale.InputThatResultsInZero, "0" };

                yield return new object[] { locale.CapEnding, locale.CapEnding, locale.ExpectedPrompt, locale.InputThatResultsInOne, "1" };
                yield return new object[] { locale.CapEnding, locale.CapEnding, locale.ExpectedPrompt, locale.InputThatResultsInZero, "0" };

                yield return new object[] { locale.TitleEnding, locale.TitleEnding, locale.ExpectedPrompt, locale.InputThatResultsInOne, "1" };
                yield return new object[] { locale.TitleEnding, locale.TitleEnding, locale.ExpectedPrompt, locale.InputThatResultsInZero, "0" };

                yield return new object[] { locale.CapTwoLetter, locale.CapTwoLetter, locale.ExpectedPrompt, locale.InputThatResultsInOne, "1" };
                yield return new object[] { locale.CapTwoLetter, locale.CapTwoLetter, locale.ExpectedPrompt, locale.InputThatResultsInZero, "0" };

                yield return new object[] { locale.LowerTwoLetter, locale.LowerTwoLetter, locale.ExpectedPrompt, locale.InputThatResultsInOne, "1" };
                yield return new object[] { locale.LowerTwoLetter, locale.LowerTwoLetter, locale.ExpectedPrompt, locale.InputThatResultsInZero, "0" };
            }
        }

        [Theory]
        [InlineData(null, Culture.Bulgarian, "(1) да или (2) Не", "да", "1")]
        [InlineData(null, Culture.Bulgarian, "(1) да или (2) Не", "Не", "0")]
        [InlineData(null, Culture.Chinese, "(1) 是的 要么 (2) 不", "是的", "1")]
        [InlineData(null, Culture.Chinese, "(1) 是的 要么 (2) 不", "不", "0")]
        [InlineData(null, Culture.Dutch, "(1) Ja of (2) Nee", "Ja", "1")]
        [InlineData(null, Culture.Dutch, "(1) Ja of (2) Nee", "Nee", "0")]
        [InlineData(null, Culture.English, "(1) Yes or (2) No", "Yes", "1")]
        [InlineData(null, Culture.English, "(1) Yes or (2) No", "No", "0")]
        [InlineData(null, Culture.French, "(1) Oui ou (2) Non", "Oui", "1")]
        [InlineData(null, Culture.French, "(1) Oui ou (2) Non", "Non", "0")]
        [InlineData(null, Culture.German, "(1) Ja oder (2) Nein", "Ja", "1")]
        [InlineData(null, Culture.German, "(1) Ja oder (2) Nein", "Nein", "0")]
        [InlineData(null, Culture.Hindi, "(1) हां या (2) नहीं", "हां", "1")]
        [InlineData(null, Culture.Hindi, "(1) हां या (2) नहीं", "नहीं", "0")]
        [InlineData(null, Culture.Italian, "(1) Si o (2) No", "Si", "1")]
        [InlineData(null, Culture.Italian, "(1) Si o (2) No", "No", "0")]
        [InlineData(null, Culture.Japanese, "(1) はい または (2) いいえ", "はい", "1")]
        [InlineData(null, Culture.Japanese, "(1) はい または (2) いいえ", "いいえ", "0")]
        [InlineData(null, Culture.Korean, "(1) 예 또는 (2) 아니", "예", "1")]
        [InlineData(null, Culture.Korean, "(1) 예 또는 (2) 아니", "아니", "0")]
        [InlineData(null, Culture.Portuguese, "(1) Sim ou (2) Não", "Sim", "1")]
        [InlineData(null, Culture.Portuguese, "(1) Sim ou (2) Não", "Não", "0")]
        [InlineData(null, Culture.Spanish, "(1) Sí o (2) No", "Sí", "1")]
        [InlineData(null, Culture.Spanish, "(1) Sí o (2) No", "No", "0")]
        [InlineData(null, Culture.Swedish, "(1) Ja eller (2) Nej", "Ja", "1")]
        [InlineData(null, Culture.Swedish, "(1) Ja eller (2) Nej", "Nej", "0")]
        [InlineData(null, Culture.Turkish, "(1) Evet veya (2) Hayır", "Evet", "1")]
        [InlineData(null, Culture.Turkish, "(1) Evet veya (2) Hayır", "Hayır", "0")]
        public async Task ConfirmPrompt_Activity_Locale_Default(string activityLocale, string defaultLocale, string prompt, string utterance, string expectedResponse)
        {
            await ConfirmPrompt_Locale_Impl(activityLocale, defaultLocale, prompt, utterance, expectedResponse);
        }

        [Theory]
        [InlineData(null, null, "(1) Yes or (2) No", "Yes", "1")]
        [InlineData(null, "", "(1) Yes or (2) No", "Yes", "1")]
        [InlineData(null, "not-supported", "(1) Yes or (2) No", "Yes", "1")]
        public async Task ConfirmPrompt_Activity_Locale_Illegal_Default(string activityLocale, string defaultLocale, string prompt, string utterance, string expectedResponse)
        {
            await ConfirmPrompt_Locale_Impl(activityLocale, defaultLocale, prompt, utterance, expectedResponse);
        }

        [Theory]
        [InlineData(null, Culture.Bulgarian, "(1) да или (2) Не", "1", "1")]
        [InlineData(null, Culture.Bulgarian, "(1) да или (2) Не", "2", "0")]
        [InlineData(null, Culture.Chinese, "(1) 是的 要么 (2) 不", "1", "1")]
        [InlineData(null, Culture.Chinese, "(1) 是的 要么 (2) 不", "2", "0")]
        [InlineData(null, Culture.Dutch, "(1) Ja of (2) Nee", "1", "1")]
        [InlineData(null, Culture.Dutch, "(1) Ja of (2) Nee", "2", "0")]
        [InlineData(null, Culture.English, "(1) Yes or (2) No", "1", "1")]
        [InlineData(null, Culture.English, "(1) Yes or (2) No", "2", "0")]
        [InlineData(null, Culture.French, "(1) Oui ou (2) Non", "1", "1")]
        [InlineData(null, Culture.French, "(1) Oui ou (2) Non", "2", "0")]
        [InlineData(null, Culture.German, "(1) Ja oder (2) Nein", "1", "1")]
        [InlineData(null, Culture.German, "(1) Ja oder (2) Nein", "2", "0")]
        [InlineData(null, Culture.Hindi, "(1) हां या (2) नहीं", "1", "1")]
        [InlineData(null, Culture.Hindi, "(1) हां या (2) नहीं", "2", "0")]
        [InlineData(null, Culture.Italian, "(1) Si o (2) No", "1", "1")]
        [InlineData(null, Culture.Italian, "(1) Si o (2) No", "2", "0")]
        [InlineData(null, Culture.Japanese, "(1) はい または (2) いいえ", "1", "1")]
        [InlineData(null, Culture.Japanese, "(1) はい または (2) いいえ", "2", "0")]
        [InlineData(null, Culture.Korean, "(1) 예 또는 (2) 아니", "1", "1")]
        [InlineData(null, Culture.Korean, "(1) 예 또는 (2) 아니", "2", "0")]
        [InlineData(null, Culture.Portuguese, "(1) Sim ou (2) Não", "1", "1")]
        [InlineData(null, Culture.Portuguese, "(1) Sim ou (2) Não", "2", "0")]
        [InlineData(null, Culture.Spanish, "(1) Sí o (2) No", "1", "1")]
        [InlineData(null, Culture.Spanish, "(1) Sí o (2) No", "2", "0")]
        [InlineData(null, Culture.Swedish, "(1) Ja eller (2) Nej", "1", "1")]
        [InlineData(null, Culture.Swedish, "(1) Ja eller (2) Nej", "2", "0")]
        [InlineData(null, Culture.Turkish, "(1) Evet veya (2) Hayır", "1", "1")]
        [InlineData(null, Culture.Turkish, "(1) Evet veya (2) Hayır", "2", "0")]
        public async Task ConfirmPrompt_Activity_Locale_Default_Number(string activityLocale, string defaultLocale, string prompt, string utterance, string expectedResponse)
        {
            await ConfirmPrompt_Locale_Impl(activityLocale, defaultLocale, prompt, utterance, expectedResponse);
        }

        [Theory]
        [InlineData(null, null, "(1) Yes or (2) No", "1", "1")]
        [InlineData(null, "", "(1) Yes or (2) No", "1", "1")]
        [InlineData(null, "not-supported", "(1) Yes or (2) No", "1", "1")]
        public async Task ConfirmPrompt_Activity_Locale_Illegal_Default_Number(string activityLocale, string defaultLocale, string prompt, string utterance, string expectedResponse)
        {
            await ConfirmPrompt_Locale_Impl(activityLocale, defaultLocale, prompt, utterance, expectedResponse);
        }

        [Theory]
        [InlineData(Culture.Bulgarian, null, "(1) да или (2) Не", "да", "1")]
        [InlineData(Culture.Bulgarian, null, "(1) да или (2) Не", "Не", "0")]
        [InlineData(Culture.Chinese, null, "(1) 是的 要么 (2) 不", "是的", "1")]
        [InlineData(Culture.Chinese, null, "(1) 是的 要么 (2) 不", "不", "0")]
        [InlineData(Culture.Dutch, null, "(1) Ja of (2) Nee", "Ja", "1")]
        [InlineData(Culture.Dutch, null, "(1) Ja of (2) Nee", "Nee", "0")]
        [InlineData(Culture.English, null, "(1) Yes or (2) No", "Yes", "1")]
        [InlineData(Culture.English, null, "(1) Yes or (2) No", "No", "0")]
        [InlineData(Culture.French, null, "(1) Oui ou (2) Non", "Oui", "1")]
        [InlineData(Culture.French, null, "(1) Oui ou (2) Non", "Non", "0")]
        [InlineData(Culture.German, null, "(1) Ja oder (2) Nein", "Ja", "1")]
        [InlineData(Culture.German, null, "(1) Ja oder (2) Nein", "Nein", "0")]
        [InlineData(Culture.Hindi, null, "(1) हां या (2) नहीं", "हां", "1")]
        [InlineData(Culture.Hindi, null, "(1) हां या (2) नहीं", "नहीं", "0")]
        [InlineData(Culture.Italian, null, "(1) Si o (2) No", "Si", "1")]
        [InlineData(Culture.Italian, null, "(1) Si o (2) No", "No", "0")]
        [InlineData(Culture.Japanese, null, "(1) はい または (2) いいえ", "はい", "1")]
        [InlineData(Culture.Japanese, null, "(1) はい または (2) いいえ", "いいえ", "0")]
        [InlineData(Culture.Korean, null, "(1) 예 또는 (2) 아니", "예", "1")]
        [InlineData(Culture.Korean, null, "(1) 예 또는 (2) 아니", "아니", "0")]
        [InlineData(Culture.Portuguese, null, "(1) Sim ou (2) Não", "Sim", "1")]
        [InlineData(Culture.Portuguese, null, "(1) Sim ou (2) Não", "Não", "0")]
        [InlineData(Culture.Spanish, null, "(1) Sí o (2) No", "Sí", "1")]
        [InlineData(Culture.Spanish, null, "(1) Sí o (2) No", "No", "0")]
        [InlineData(Culture.Swedish, null, "(1) Ja eller (2) Nej", "Ja", "1")]
        [InlineData(Culture.Swedish, null, "(1) Ja eller (2) Nej", "Nej", "0")]
        [InlineData(Culture.Turkish, null, "(1) Evet veya (2) Hayır", "Evet", "1")]
        [InlineData(Culture.Turkish, null, "(1) Evet veya (2) Hayır", "Hayır", "0")]
        public async Task ConfirmPrompt_Activity_Locale_Activity(string activityLocale, string defaultLocale, string prompt, string utterance, string expectedResponse)
        {
            await ConfirmPrompt_Locale_Impl(activityLocale, defaultLocale, prompt, utterance, expectedResponse);
        }

        [Theory]
        [InlineData(null, null, "(1) Yes or (2) No", "Yes", "1")]
        [InlineData("", null, "(1) Yes or (2) No", "Yes", "1")]
        [InlineData("not-supported", null, "(1) Yes or (2) No", "Yes", "1")]
        public async Task ConfirmPrompt_Activity_Locale_Illegal_Activity(string activityLocale, string defaultLocale, string prompt, string utterance, string expectedResponse)
        {
            await ConfirmPrompt_Locale_Impl(activityLocale, defaultLocale, prompt, utterance, expectedResponse);
        }

        [Theory]
        [MemberData(nameof(GetLocaleVariationTest), DisableDiscoveryEnumeration = true)]
        public async Task ConfirmPrompt_Locale_Variations(string activityLocale, string defaultLocale, string prompt, string utterance, string expectedResponse)
        {
            await ConfirmPrompt_Locale_Impl(activityLocale, defaultLocale, prompt, utterance, expectedResponse);
        }

        [Theory]
        [InlineData("custom-custom", "(1) customYes customOr (2) customNo", "customYes", "1")]
        [InlineData("custom-custom", "(1) customYes customOr (2) customNo", "customNo", "0")]
        public async Task ConfirmPrompt_Locale_Override_ChoiceDefaults(string defaultLocale, string prompt, string utterance, string expectedResponse)
        {
            var convoState = new ConversationState(new MemoryStorage());
            var dialogState = convoState.CreateProperty<DialogState>("dialogState");

            var adapter = new TestAdapter()
                .Use(new AutoSaveStateMiddleware(convoState));

            // Create new DialogSet.
            var dialogs = new DialogSet(dialogState);

            var culture = new PromptCultureModel()
            {
                InlineOr = " customOr ",
                InlineOrMore = " customOrMore ",
                Locale = "custom-custom",
                Separator = "customSeparator",
                NoInLanguage = "customNo",
                YesInLanguage = "customYes",
            };

            var customDict = new Dictionary<string, (Choice, Choice, ChoiceFactoryOptions)>()
            {
                { culture.Locale, (new Choice(culture.YesInLanguage), new Choice(culture.NoInLanguage), new ChoiceFactoryOptions(culture.Separator, culture.InlineOr, culture.InlineOrMore, true)) },
            };

            // Prompt should default to English if locale is a non-supported value
            dialogs.Add(new ConfirmPrompt("ConfirmPrompt", customDict, null, defaultLocale));

            await new TestFlow(adapter, async (turnContext, cancellationToken) =>
            {
                turnContext.Activity.Locale = culture.Locale;

                var dc = await dialogs.CreateContextAsync(turnContext, cancellationToken);

                var results = await dc.ContinueDialogAsync(cancellationToken);
                if (results.Status == DialogTurnStatus.Empty)
                {
                    await dc.PromptAsync("ConfirmPrompt", new PromptOptions { Prompt = new Activity { Type = ActivityTypes.Message, Text = "Prompt." } }, cancellationToken);
                }
                else if (results.Status == DialogTurnStatus.Complete)
                {
                    if ((bool)results.Result)
                    {
                        await turnContext.SendActivityAsync(MessageFactory.Text("1"), cancellationToken);
                    }
                    else
                    {
                        await turnContext.SendActivityAsync(MessageFactory.Text("0"), cancellationToken);
                    }
                }
            })
            .Send("hello")
            .AssertReply("Prompt. " + prompt)
            .Send(utterance)
            .AssertReply(expectedResponse)
            .StartTestAsync();
        }

        private async Task ConfirmPrompt_Locale_Impl(string activityLocale, string defaultLocale, string prompt, string utterance, string expectedResponse)
        {
            var convoState = new ConversationState(new MemoryStorage());
            var dialogState = convoState.CreateProperty<DialogState>("dialogState");

            var adapter = new TestAdapter(TestAdapter.CreateConversation(nameof(ConfirmPrompt_Locale_Impl)))
                .Use(new AutoSaveStateMiddleware(convoState));

            // Create new DialogSet.
            var dialogs = new DialogSet(dialogState);

            // Prompt should default to English if locale is a non-supported value
            dialogs.Add(new ConfirmPrompt("ConfirmPrompt", defaultLocale: defaultLocale));

            await new TestFlow(adapter, async (turnContext, cancellationToken) =>
            {
                turnContext.Activity.Locale = activityLocale;

                var dc = await dialogs.CreateContextAsync(turnContext, cancellationToken);

                var results = await dc.ContinueDialogAsync(cancellationToken);
                if (results.Status == DialogTurnStatus.Empty)
                {
                    await dc.PromptAsync("ConfirmPrompt", new PromptOptions { Prompt = new Activity { Type = ActivityTypes.Message, Text = "Prompt." } }, cancellationToken);
                }
                else if (results.Status == DialogTurnStatus.Complete)
                {
                    if ((bool)results.Result)
                    {
                        await turnContext.SendActivityAsync(MessageFactory.Text("1"), cancellationToken);
                    }
                    else
                    {
                        await turnContext.SendActivityAsync(MessageFactory.Text("0"), cancellationToken);
                    }
                }
            })
            .Send("hello")
            .AssertReply("Prompt. " + prompt)
            .Send(utterance)
            .AssertReply(expectedResponse)
            .StartTestAsync();
        }
    }
}
