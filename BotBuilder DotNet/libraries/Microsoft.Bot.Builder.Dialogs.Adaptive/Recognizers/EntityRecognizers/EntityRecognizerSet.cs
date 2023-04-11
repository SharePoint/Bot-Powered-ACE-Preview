﻿// Licensed under the MIT License.
// Copyright (c) Microsoft Corporation. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Dialogs.Adaptive.Recognizers
{
    /// <summary>
    /// EntityRecognizerSet - Implements a workflow against a pool of IEntityRecognizer instances, iterating until nobody has anything new to add.
    /// </summary>
    public class EntityRecognizerSet : List<EntityRecognizer>
    {
        /// <summary>
        /// Class identifier.
        /// </summary>
        [JsonProperty("$kind")]
        public const string Kind = "Microsoft.EntityRecognizerSet";

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityRecognizerSet"/> class.
        /// </summary>
        [JsonConstructor]
        public EntityRecognizerSet()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityRecognizerSet"/> class.
        /// </summary>
        /// <param name="recognizers"><see cref="EntityRecognizer"/> instances pool.</param>
        public EntityRecognizerSet(IEnumerable<EntityRecognizer> recognizers)
            : base(recognizers)
        {
        }

        /// <summary>
        /// Implement RecognizeEntities by iterating against the Recognizer pool.
        /// </summary>
        /// <param name="dialogContext">Context for the current turn of conversation.</param>
        /// <param name="entities">if no entities are passed in, it will generate a <see cref="TextEntity"/> for turnContext.Activity.Text and then generate entities off of that.</param>
        /// <returns><see cref="Entity"/> list.</returns>
        public virtual Task<IList<Entity>> RecognizeEntitiesAsync(DialogContext dialogContext, IEnumerable<Entity> entities = null)
        {
            return this.RecognizeEntitiesAsync(dialogContext, dialogContext.Context.Activity, entities);
        }

        /// <summary>
        /// Implement RecognizeEntities by iterating against the Recognizer pool.
        /// </summary>
        /// <param name="dialogContext">Context for the current turn of conversation.</param>
        /// <param name="activity">activity to recognize against.</param>
        /// <param name="entities">if no entities are passed in, it will generate a <see cref="TextEntity"/> for turnContext.Activity.Text and then generate entities off of that.</param>
        /// <returns><see cref="Entity"/> list.</returns>
        public virtual async Task<IList<Entity>> RecognizeEntitiesAsync(DialogContext dialogContext, Activity activity, IEnumerable<Entity> entities = null)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                return await this.RecognizeEntitiesAsync(dialogContext, activity.Text, activity.Locale, entities).ConfigureAwait(false);
            }

            return new List<Entity>();
        }

        /// <summary>
        /// Implement RecognizeEntities by iterating against the Recognizer pool.
        /// </summary>
        /// <param name="dialogContext">Context for the current turn of conversation.</param>
        /// <param name="text">text to recognize.</param>
        /// <param name="locale">locale to use.</param>
        /// <param name="entities">if no entities are passed in, it will generate a <see cref="TextEntity"/> for turnContext.Activity.Text and then generate entities off of that.</param>
        /// <returns><see cref="Entity"/> list.</returns>
        public virtual async Task<IList<Entity>> RecognizeEntitiesAsync(DialogContext dialogContext, string text, string locale, IEnumerable<Entity> entities = null)
        {
            List<Entity> allNewEntities = new List<Entity>();
            List<Entity> entitiesToProcess = new List<Entity>(entities ?? Array.Empty<Entity>());

            if (entitiesToProcess.Count == 0)
            {
                var textEntity = new TextEntity(text);
                textEntity.Properties["start"] = 0;
                textEntity.Properties["end"] = text.Length;
                textEntity.Properties["score"] = 1.0;
                allNewEntities.Add(textEntity);
                entitiesToProcess.Add(textEntity);
            }

            do
            {
                List<Entity> newEntitiesToProcess = new List<Entity>();

                foreach (var recognizer in this)
                {
                    try
                    {
                        // get new entities
                        var newEntities = await recognizer.RecognizeEntitiesAsync(dialogContext, text, locale, entitiesToProcess).ConfigureAwait(false);

                        foreach (var newEntity in newEntities)
                        {
                            // if unique
                            if (!allNewEntities.Any(entity => entity.Equals(newEntity)))
                            {
                                // add to all results
                                allNewEntities.Add(newEntity);

                                // add to list to be processed more
                                newEntitiesToProcess.Add(newEntity);
                            }
                        }
                    }
#pragma warning disable CA1031 // Do not catch general exception types (trace the exception and continue).
                    catch (Exception err)
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        System.Diagnostics.Trace.TraceWarning(err.Message);
                    }
                }

                // switch to next pool of new entities to process
                entitiesToProcess = newEntitiesToProcess;
            }
            while (entitiesToProcess.Count > 0);

            return allNewEntities;
        }
    }
}
