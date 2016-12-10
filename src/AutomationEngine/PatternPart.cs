using System;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public abstract class PatternPart
    {
        [XmlAttribute]
        public string Value { get; set; }

        public bool ValueSpecified => !string.IsNullOrWhiteSpace(Value);

        public abstract string DisplayValue { get; }
    }
}