using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [XmlRoot("ExecutableItems")]
    public class ExecutableItemsCollection
    {
        [XmlElement("Item", typeof(ExecutableItem))]
        [XmlElement("FileItem", typeof(FileItem))]
        public List<ExecutableItem> Items { get; set; }

        public string Group { get; set; }

        public bool GroupSpecified
        {
            get { return !string.IsNullOrWhiteSpace(Group); }
        }

        public ExecutableItemsCollection()
        {
            Items = new List<ExecutableItem>();
        }
    }
}
