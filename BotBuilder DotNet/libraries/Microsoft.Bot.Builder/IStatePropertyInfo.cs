﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.

namespace Microsoft.Bot.Builder
{
    /// <summary>
    /// Metadata about a property, including policy info.
    /// </summary>
    public interface IStatePropertyInfo
    {
        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        string Name { get; }
    }
}
