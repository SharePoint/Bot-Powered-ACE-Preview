﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Bot.Builder.Dialogs
{
    /// <summary>
    /// Defines path passed to the active dialog.
    /// </summary>
#pragma warning disable CA1052 // Static holder types should be Static or NotInheritable (we can't change this without breaking binary compat)
    public class ThisPath
#pragma warning restore CA1052 // Static holder types should be Static or NotInheritable
    {
        /// <summary>
        /// The options that were passed to the active dialog via options argument of BeginDialog.
        /// </summary>
        public const string Options = "this.options";

        /// <summary>
        /// The options that were passed to the active dialog via options argument of BeginDialog.
        /// </summary>
        /// <remarks>This property is deprecated, use ThisPath.Options instead.</remarks>
        [Obsolete("This property is deprecated, use ThisPath.Options instead.")]
        public const string OPTIONS = "this.options";
    }
}
