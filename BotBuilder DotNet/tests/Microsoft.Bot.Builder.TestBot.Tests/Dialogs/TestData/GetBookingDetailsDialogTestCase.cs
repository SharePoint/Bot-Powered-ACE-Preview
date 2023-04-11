﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Bot.Builder.TestBot.Shared;

namespace Microsoft.BotBuilderSamples.Tests.Dialogs.TestData
{
    public class GetBookingDetailsDialogTestCase
    {
        /// <summary>
        /// Gets or sets the name for the test case.
        /// </summary>
        /// <value>The test case name.</value>
        public string Name { get; set; }

        public BookingDetails InitialBookingDetails { get; set; }

        public string[,] UtterancesAndReplies { get; set; }

        public BookingDetails ExpectedBookingDetails { get; set; }
    }
}
