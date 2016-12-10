using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    [XmlRoot("ContextGroups")]
    public class ContextGroupCollection : FileStorage<ContextGroupCollection>
    {
        public ContextGroupCollection()
        {
            Groups = new List<ContextGroup>();
        }

        public override string StorageFileName => "context_groups.xml";

        [XmlElement("Group")]
        public List<ContextGroup> Groups { get; set; }
    }
}