﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Schema
{
    /// <summary>
    /// Conversation is ending, or a request to end the conversation.
    /// </summary>
    public interface IEndOfConversationActivity : IActivity
    {
        /// <summary>
        /// Gets or Sets Code indicating why the conversation has ended.
        /// </summary>
        /// <value>Code.</value>
        string Code { get; set; }

        /// <summary>
        /// Gets or Sets Content to display when ending the conversation.
        /// </summary>
        /// <value>Text.</value>
        string Text { get; set; }
    }
}
