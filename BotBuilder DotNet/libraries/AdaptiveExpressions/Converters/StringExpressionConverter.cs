﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using AdaptiveExpressions.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdaptiveExpressions.Converters
{
    /// <summary>
    /// Converter which allows json to be expression to object or static object.
    /// </summary>
    public class StringExpressionConverter : JsonConverter<StringExpression>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringExpressionConverter"/> class.
        /// </summary>
        public StringExpressionConverter()
        {
        }

        /// <summary>
        /// Reads the JSON representation of the object.
        /// </summary>
        /// <param name="reader">The Newtonsoft.Json.JsonReader to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read.</param>
        /// <param name="hasExistingValue">A boolean value indicating whether there is an existing value of object to be read.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>A StringExpression instance.</returns>
        public override StringExpression ReadJson(JsonReader reader, Type objectType, StringExpression existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(string))
            {
                return new StringExpression((string)reader.Value);
            }
            else
            {
                return new StringExpression(JToken.Load(reader));
            }
        }

        /// <summary>
        /// Writes the JSON representation of the object.
        /// </summary>
        /// <param name="writer">The Newtonsoft.Json.JsonWriter to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, StringExpression value, JsonSerializer serializer)
        {
            if (value.ExpressionText != null)
            {
                serializer.Serialize(writer, value.ToString());
            }
            else
            {
                serializer.Serialize(writer, value.Value);
            }
        }
    }
}
