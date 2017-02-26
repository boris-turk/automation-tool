using System;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public abstract class AbstractValue
    {
        [XmlAttribute]
        public ValueType Type { get; set; }

        public bool TypeSpecified => Type != ValueType.None;

        [XmlText]
        public string Value { get; set; }

        public abstract string InteropValue { get; }

        public abstract bool IsEmpty { get; }
    }
}