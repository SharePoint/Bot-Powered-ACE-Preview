﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Builder.Dialogs.Debugging.Protocol
{
    internal class SetExceptionBreakpoints
    {
#pragma warning disable CA1819 // Properties should not return arrays (we can't change this without breaking binary compat)
        public string[] Filters { get; set; }
#pragma warning restore CA1819 // Properties should not return arrays
    }
}
