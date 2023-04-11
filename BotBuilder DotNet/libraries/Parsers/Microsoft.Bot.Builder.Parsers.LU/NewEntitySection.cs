﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Microsoft.Bot.Builder.Parsers.LU
{
    /// <summary>
    /// Class for NewEntitySection sections.
    /// </summary>
    public class NewEntitySection : SectionEntity
    {
        // TODO: pass this constant to a helper class.
        private char[] _invalidCharsInIntentOrEntityName = { '<', '>', '*', '%', '&', ':', '\\', '$' };

        /// <summary>
        /// Initializes a new instance of the <see cref="NewEntitySection"/> class.
        /// </summary>
        /// <param name="parseTree">The new entity context from the parse tree.</param>
        public NewEntitySection(LUFileParser.NewEntitySectionContext parseTree)
        {
            if (parseTree == null)
            {
                throw new ArgumentNullException(nameof(parseTree));
            }

            SectionType = SectionType.NewEntitySection;
            Errors = new List<Error>();
            Name = ExtractName(parseTree);
            Type = ExtractType(parseTree);
            Roles = ExtractRoles(parseTree);
            Features = ExtractFeatures(parseTree);
            CompositeDefinition = ExtractCompositeDefinition(parseTree);
            RegexDefinition = ExtractRegexDefinition(parseTree);
            if (string.Equals(Type, "list", StringComparison.Ordinal))
            {
                SynonymsList = ExtractSynonyms(parseTree);
            }
            else
            {
                ListBody = ExtractPhraseList(parseTree);
            }

            string secTypeStr = $"{SectionType}";
            Id = $"{char.ToLower(secTypeStr[0], System.Globalization.CultureInfo.InvariantCulture) + secTypeStr.Substring(1)}_{Name}";
            var startPosition = new Position { Line = parseTree.Start.Line, Character = parseTree.Start.Column };
            var stopPosition = new Position { Line = parseTree.Stop.Line, Character = parseTree.Stop.Column + parseTree.Stop.Text.Length };
            Range = new Range { Start = startPosition, End = stopPosition };
        }

        public string ExtractName(LUFileParser.NewEntitySectionContext parseTree)
        {
            var entityName = string.Empty;
            if (parseTree.newEntityDefinition().newEntityLine().newEntityName() != null)
            {
                entityName = parseTree.newEntityDefinition().newEntityLine().newEntityName().GetText().Trim();
            }
            else if (parseTree.newEntityDefinition().newEntityLine().newEntityNameWithWS() != null)
            {
                entityName = parseTree.newEntityDefinition().newEntityLine().newEntityNameWithWS().GetText().Trim();
            }
            else
            {
                Errors.Add(
                    Diagnostic.BuildDiagnostic(
                        message: "Invalid entity line, did you miss entity name after @",
                        context: parseTree.newEntityDefinition().newEntityLine()));
            }

            if (!string.IsNullOrEmpty(entityName) && entityName.IndexOfAny(_invalidCharsInIntentOrEntityName) >= 0)
            {
                Errors.Add(
                    Diagnostic.BuildDiagnostic(
                        message: $"Invalid entity line, entity name {entityName} cannot contain any of the following characters: [<, >, *, %, &, :, \\, $]",
                        context: parseTree.newEntityDefinition().newEntityLine()));
                return null;
            }
            else
            {
                return entityName;
            }
        }

        public string ExtractType(LUFileParser.NewEntitySectionContext parseTree)
        {
            if (parseTree.newEntityDefinition().newEntityLine().newEntityType() != null)
            {
                return parseTree.newEntityDefinition().newEntityLine().newEntityType().GetText().Trim();
            }

            return null;
        }

        public string ExtractRoles(LUFileParser.NewEntitySectionContext parseTree)
        {
            if (parseTree.newEntityDefinition().newEntityLine().newEntityRoles() != null)
            {
                return parseTree.newEntityDefinition().newEntityLine().newEntityRoles().newEntityRoleOrFeatures().GetText().Trim();
            }

            return null;
        }

        public string ExtractFeatures(LUFileParser.NewEntitySectionContext parseTree)
        {
            if (parseTree.newEntityDefinition().newEntityLine().newEntityUsesFeatures() != null)
            {
                return parseTree.newEntityDefinition().newEntityLine().newEntityUsesFeatures().newEntityRoleOrFeatures().GetText().Trim();
            }

            return null;
        }

        public string ExtractCompositeDefinition(LUFileParser.NewEntitySectionContext parseTree)
        {
            if (parseTree.newEntityDefinition().newEntityLine().newCompositeDefinition() != null)
            {
                return parseTree.newEntityDefinition().newEntityLine().newCompositeDefinition().GetText().Trim();
            }

            return null;
        }

        public string ExtractRegexDefinition(LUFileParser.NewEntitySectionContext parseTree)
        {
            if (parseTree.newEntityDefinition().newEntityLine().newRegexDefinition() != null)
            {
                return parseTree.newEntityDefinition().newEntityLine().newRegexDefinition().GetText().Trim();
            }

            return null;
        }

        public List<SynonymElement> ExtractSynonyms(LUFileParser.NewEntitySectionContext parseTree)
        {
            var synonymsOrPhraseList = new List<SynonymElement>();
            if (parseTree.newEntityDefinition().newEntityListbody() != null)
            {
                foreach (var errorItemStr in parseTree.newEntityDefinition().newEntityListbody().errorString())
                {
                    if (!string.IsNullOrEmpty(errorItemStr.GetText().Trim()))
                    {
                        Errors.Add(
                            Diagnostic.BuildDiagnostic(
                                message: "Invalid list entity line, did you miss '-' at line begin?",
                                context: errorItemStr));
                    }
                }

                var bodyElement = new SynonymElement();
                foreach (var normalItemStr in parseTree.newEntityDefinition().newEntityListbody().normalItemString())
                {
                    var trimedItemStr = normalItemStr.GetText().Trim();
                    var normalizedValueMatch = Regex.Match(trimedItemStr, @"(?: |\t)*-(?: |\t)*(.*)(?: |\t)*:$");
                    if (normalizedValueMatch.Success)
                    {
                        if (bodyElement.NormalizedValue != null)
                        {
                            // This is not the first value in the list
                            synonymsOrPhraseList.Add(bodyElement);
                            bodyElement = new SynonymElement();
                        }

                        bodyElement.NormalizedValue = normalizedValueMatch.Groups[1].Value.Trim();
                    }
                    else
                    {
                        var index = trimedItemStr.IndexOf('-');
                        var synonym = trimedItemStr.Remove(index, 1);
                        bodyElement.Synonyms.Add(synonym.Trim());

                        if (bodyElement.NormalizedValue == null) 
                        {
                            bodyElement.NormalizedValue = synonym.Trim();
                        }
                    }
                }

                if (bodyElement.NormalizedValue != null)
                {
                    // There was at least one
                    synonymsOrPhraseList.Add(bodyElement);
                }
            }

            if (!string.IsNullOrEmpty(Type) && Type.IndexOf('=') > -1 && synonymsOrPhraseList.Count == 0)
            {
                var errorMsg = $"no synonyms list found for list entity definition: \"{parseTree.newEntityDefinition().newEntityLine().GetText()}\"";
                var error = Diagnostic.BuildDiagnostic(
                    message: errorMsg,
                    context: parseTree.newEntityDefinition().newEntityLine(),
                    severity: Diagnostic.WARN);
                Errors.Add(error);
            }

            return synonymsOrPhraseList;
        }

        public List<string> ExtractPhraseList(LUFileParser.NewEntitySectionContext parseTree)
        {
            var synonymsOrPhraseList = new List<string>();
            if (parseTree.newEntityDefinition().newEntityListbody() != null)
            {
                foreach (var errorItemStr in parseTree.newEntityDefinition().newEntityListbody().errorString())
                {
                    if (!string.IsNullOrEmpty(errorItemStr.GetText().Trim()))
                    {
                        Errors.Add(
                            Diagnostic.BuildDiagnostic(
                                message: "Invalid list entity line, did you miss '-' at line begin?",
                                context: errorItemStr));
                    }
                }

                foreach (var normalItemStr in parseTree.newEntityDefinition().newEntityListbody().normalItemString())
                {
                    synonymsOrPhraseList.Add(normalItemStr.GetText());
                }
            }

            if (!string.IsNullOrEmpty(Type) && Type.IndexOf('=') > -1 && synonymsOrPhraseList.Count == 0)
            {
                var errorMsg = $"no synonyms list found for list entity definition: \"{parseTree.newEntityDefinition().newEntityLine().GetText()}\"";
                var error = Diagnostic.BuildDiagnostic(
                    message: errorMsg,
                    context: parseTree.newEntityDefinition().newEntityLine(),
                    severity: Diagnostic.WARN);
                Errors.Add(error);
            }

            return synonymsOrPhraseList;
        }
    }
}
