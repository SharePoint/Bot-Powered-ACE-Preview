﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;

namespace Microsoft.Bot.Builder.Dialogs
{
    /// <summary>
    /// Defines Dialog Dependencies interface for enumerating child dialogs.
    /// </summary>
    public interface IDialogDependencies
    {
        /// <summary>
        /// Enumerate child dialog dependencies so they can be added to the containers dialogset.
        /// </summary>
        /// <returns>dialog enumeration.</returns>
        IEnumerable<Dialog> GetDependencies();
    }
}
