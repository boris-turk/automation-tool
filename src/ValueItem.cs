using System.Xml.Serialization;

namespace AutomationEngine
{
    public class ValueItem
    {
        [XmlAttribute]
        public string Value { get; set; }

        public bool ValueSpecified
        {
            get { return !string.IsNullOrWhiteSpace(Value); }
        }
    }
}