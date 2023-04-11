﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;

namespace Microsoft.Bot.Builder
{
    /// <summary>
    /// Values persisted for the lifetime of the turn as part of the <see cref="ITurnContext"/>.
    /// </summary>
    /// <remarks>
    /// Typical values which are stored here are objects which are needed for the lifetime of a turn, such
    /// as IStorage, BotState, ConversationState, ILanguageGenerator, ResourceExplorer etc.
    /// </remarks>
    public class TurnContextStateCollection : Dictionary<string, object>, IDisposable
    {
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="TurnContextStateCollection"/> class.
        /// </summary>
        public TurnContextStateCollection()
            : base(StringComparer.InvariantCultureIgnoreCase)
        {
        }

        /// <summary>
        /// Gets a cached value by name from the turn's context.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="key">The name of the object.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> is null.</exception>
        /// <returns>The object; or null if no service is registered by the key, or
        /// the retrieved object does not match the object type.</returns>
        public T Get<T>(string key)
            where T : class
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Get));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (TryGetValue(key, out var service))
            {
                if (service is T result)
                {
                    return result;
                }
            }

            // return null if either the key or type don't match
            return null;
        }

        /// <summary>
        /// Gets the default value by type from the turn's context.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <returns>The object; or null if no default service of the type is registered.</returns>
        /// <remarks>The default service key is the <see cref="Type.FullName"/> of the object type.</remarks>
        public T Get<T>()
            where T : class
        {
            return Get<T>(typeof(T).FullName);
        }

        /// <summary>
        /// Adds a value to the turn's context.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="key">The name of the object.</param>
        /// <param name="value">The value to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> or <paramref name="value"/>is null.</exception>
        public void Add<T>(string key, T value)
            where T : class
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Add));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            // note this can throw if the key is already present
            base.Add(key, value);
        }

        /// <summary>
        /// Adds a value to the turn's context.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="value">The object to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/>is null.</exception>
        /// <remarks>The default service key is the <see cref="Type.FullName"/> of the object type.</remarks>
        public void Add<T>(T value)
            where T : class
        {
            Add(typeof(T).FullName, value);
        }

        /// <summary>
        /// Set a value to the turn's context.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="key">The name of the object.</param>
        /// <param name="value">The value to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key"/> or <paramref name="value"/>is null.</exception>
        public void Set<T>(string key, T value)
            where T : class
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(Set));
            }

            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            this[key] = value;
        }

        /// <summary>
        /// Set a value to the turn's context.
        /// </summary>
        /// <typeparam name="T">The type of the object.</typeparam>
        /// <param name="value">The value to add.</param>
        /// <exception cref="ArgumentNullException"><paramref name="value"/>is null.</exception>
        public void Set<T>(T value)
            where T : class
        {
            Set(typeof(T).FullName, value);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Frees resources if the disposing parameter is set to true.
        /// </summary>
        /// <param name="disposing">Boolean value that indicates if freeing resources should be performed.</param>
        protected virtual void Dispose(bool disposing)
        {
            _disposed = true;
        }
    }
}
