﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Microsoft.Bot.Builder.Dialogs.Memory.PathResolvers
{
    /// <summary>
    /// Maps @ => turn.recognized.entitites.xxx[0].
    /// </summary>
    public class AtPathResolver : AliasPathResolver
    {
        private const string Prefix = "turn.recognized.entities.";

        private static readonly char[] _delims = { '.', '[' };

        /// <summary>
        /// Initializes a new instance of the <see cref="AtPathResolver"/> class.
        /// </summary>
        public AtPathResolver()
            : base("@", string.Empty)
        {
        }

        /// <summary>
        /// Transforms the path.
        /// </summary>
        /// <param name="path">Path to inspect.</param>
        /// <returns>Transformed path.</returns>
        public override string TransformPath(string path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            path = path.Trim();
            if (path.StartsWith("@", StringComparison.Ordinal) && path.Length > 1 && IsPathChar(path[1]))
            {
                var end = path.IndexOfAny(_delims);
                if (end == -1)
                {
                    end = path.Length;
                }

                var property = path.Substring(1, end - 1);
                var suffix = path.Substring(end);
                path = $"{Prefix}{property}.first(){suffix}";
            }

            return path;
        }
    }
}
