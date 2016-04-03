using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [XmlRoot("Contexts")]
    public class ContextCollection : FileStorage<ContextCollection>
    {
        public ContextCollection()
        {
            Entries = new List<Context>();
        }

        public override string StorageFileName
        {
            get { return "contexts.xml"; }
        }

        [XmlElement("Context")]
        public List<Context> Entries { get; set; }

        [XmlIgnore]
        public string Current { get; set; }
    }
}