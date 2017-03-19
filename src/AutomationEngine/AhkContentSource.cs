using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public abstract class AhkContentSource
    {
        protected AhkContentSource()
        {
            NameRegex = new RegexReplacement();
            ReturnValueRegex = new RegexReplacement();
        }

        public abstract string ReturnType { get; }

        public virtual string Function { get; set; }

        public bool FunctionSpecified => !string.IsNullOrWhiteSpace(Function);

        public RegexReplacement NameRegex { get; set; }

        public bool ReturnsFilePaths { get; set; }

        public bool NameRegexSpecified =>
            NameRegex.SearchRegexSpecified ||
            NameRegex.ReplacementSpecified;

        public RegexReplacement ReturnValueRegex { get; set; }

        public bool ReturnValueRegexSpecified =>
            ReturnValueRegex.SearchRegexSpecified ||
            ReturnValueRegex.ReplacementSpecified;

        [XmlIgnore]
        public virtual List<AbstractValue> InteropArguments => new List<AbstractValue>
        {
            new StringValue { Value = NameRegex.SearchRegex },
            new StringValue { Value = NameRegex.Replacement },
            new StringValue { Value = ReturnValueRegex.SearchRegex },
            new StringValue { Value = ReturnValueRegex.Replacement },
        };
    }
}