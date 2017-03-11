using System;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class VisibilityCondition
    {
        [XmlAttribute]
        public VisibilityConditionType Type { get; set; }

        [XmlAttribute]
        public string Value { get; set; }
    }
}