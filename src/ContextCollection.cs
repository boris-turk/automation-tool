using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [XmlRoot("Contexts")]
    public class ContextCollection : FileStorage<ContextCollection>
    {
        public ContextCollection()
        {
            Contexts = new List<string>();
        }

        public override string StorageFileName
        {
            get { return "contexts.xml"; }
        }

        [XmlElement("Context")]
        public List<string> Contexts { get; set; }
    }
}