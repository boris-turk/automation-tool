using System;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public abstract class AbstractValue
    {
        [XmlText]
        public string Value { get; set; }

        public abstract string InteropValue { get; }
    }
}