using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class PatternCollection
    {
        public PatternCollection()
        {
            LeadingParts = new List<PatternPart>();
        }

        [XmlElement(typeof(Word))]
        [XmlElement(typeof(Context))]
        public List<PatternPart> LeadingParts { get; set; }

        public RegularExpression ArgumentsRegex { get; set; }

        public bool ArgumentsRegexSpecified
        {
            get { return ArgumentsRegex != null; }
        }

        public void SetContextReplacement(string replacement)
        {
            foreach (Context context in LeadingParts.OfType<Context>())
            {
                if (context.Value == null)
                {
                    context.Replacement = replacement;
                }
            }
        }
    }
}