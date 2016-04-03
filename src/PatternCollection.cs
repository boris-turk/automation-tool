using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class PatternCollection
    {
        public PatternCollection()
        {
            Values = new List<PatternPart>();
        }

        [XmlElement(typeof(Word))]
        [XmlElement(typeof(Context))]
        [XmlElement(typeof(RegularExpression))]
        public List<PatternPart> Values { get; set; }
    }
}