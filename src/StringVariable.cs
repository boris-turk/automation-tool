using System;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class StringValue
    {
        [XmlText]
        public string Value { get; set; }
    }
}
