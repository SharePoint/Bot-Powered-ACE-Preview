﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Microsoft.Bot.Builder.LanguageGeneration
{
    /// <summary>
    /// Class which represents a single template which can be evaluated.
    /// </summary>
    /// <remarks>
    /// Defines a data model that can easily understand and use the context for all kinds of visitors,
    /// whether it's an evaluator, static checker, analyzer, and so on.
    /// </remarks>
    public class Template
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Template"/> class.
        /// </summary>
        /// <param name="templateName">Template name without parameters.</param>
        /// <param name="parameters">Parameter list.</param>
        /// <param name="templateBody">Template content.</param>
        /// <param name="sourceRange">Source range of template.</param>
        internal Template(
            string templateName,
            List<string> parameters,
            string templateBody,
            SourceRange sourceRange)
        {
            this.Name = templateName ?? string.Empty;
            this.Parameters = parameters ?? new List<string>();
            this.Body = templateBody ?? string.Empty;
            this.SourceRange = sourceRange;
            Expressions = new List<ExpressionRef>();
        }

        /// <summary>
        /// Gets expression reference list.
        /// </summary>
        /// <value>
        /// Expression reference list.
        /// </value>
        public IList<ExpressionRef> Expressions { get; }

        /// <summary>
        /// Gets or sets source range.
        /// </summary>
        /// <value>
        /// Start line of the template in LG file.
        /// </value>
        public SourceRange SourceRange { get; set; }

        /// <summary>
        /// Gets or sets name of the template, which follows '#' in a LG file.
        /// </summary>
        /// <value>
        /// Name of the template, which follows '#' in a LG file.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets parameter list of this template.
        /// </summary>
        /// <value>
        /// Parameter list of this template.
        /// </value>
#pragma warning disable CA2227 // Collection properties should be read only (we can't remove the setter without breaking binary compat)
        public List<string> Parameters { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

        /// <summary>
        /// Gets or sets text format of Body of this template. All content except Name and Parameters.
        /// </summary>
        /// <value>
        /// Text format of Body of this template. All content except Name and Parameters.
        /// </value>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the parse tree of this template.
        /// </summary>
        /// <value>
        /// The parse tree of this template.
        /// </value>
        public LGTemplateParser.BodyContext TemplateBodyParseTree { get; set; }

        /// <summary>
        /// Gets or sets properties that are not otherwise defined by the <see cref="Template"/> core type.
        /// </summary>
        /// <value>The extended properties for the object.</value>
#pragma warning disable CA2227 // Collection properties should be read only
        public JObject Properties { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

        /// <inheritdoc/>
        public override string ToString() => $"[{Name}({string.Join(", ", Parameters)})]\"{Body}\"";
    }
}
