using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    public class PatternCollection
    {
        public PatternCollection()
        {
            Values = new List<ValueItem>();
        }

        [XmlElement(typeof(Word))]
        [XmlElement(typeof(Context))]
        [XmlElement(typeof(RegularExpression))]
        public List<ValueItem> Values { get; set; }
    }
}