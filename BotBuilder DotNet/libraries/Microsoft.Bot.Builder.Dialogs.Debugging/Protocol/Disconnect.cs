﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

namespace Microsoft.Bot.Builder.Dialogs.Debugging.Protocol
{
    internal class Disconnect
    {
        public bool Restart { get; set; }

        public bool TerminateDebuggee { get; set; }
    }
}
