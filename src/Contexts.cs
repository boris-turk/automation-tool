using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    public class Contexts : FileStorage<Contexts>
    {
        public Contexts()
        {
            Entries = new List<string>();
        }

        public override string StorageFileName
        {
            get { return "contexts.xml"; }
        }

        [XmlElement("Context")]
        public List<string> Entries { get; set; }

        [XmlIgnore]
        public string Current { get; set; }
    }
}