using System.Xml.Serialization;

namespace AutomationEngine
{
    public abstract class AbstractValue
    {
        [XmlText]
        public string Value { get; set; }

        public abstract string InteropValue { get; }
    }
}