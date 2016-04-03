using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class ContextGroup
    {
        public ContextGroup()
        {
            Contexts = new List<Context>();
        }

        [XmlAttribute]
        public string Id { get; set; }

        [XmlElement("Context")]
        public List<Context> Contexts { get; set; }
    }
}