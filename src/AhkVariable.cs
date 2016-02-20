using System;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class AhkVariable
    {
        [XmlText]
        public string Value { get; set; }
    }
}