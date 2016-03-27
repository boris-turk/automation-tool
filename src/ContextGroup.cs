using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class ContextGroup
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlElement("Context")]
        public List<string> Contexts { get; set; }
    }
}