using System;
using System.Linq;
using System.Xml.Serialization;

namespace AutomationEngine
{
    [Serializable]
    public class Context : PatternPart
    {
        [XmlIgnore]
        public string Replacement { get; set; }

        public override string DisplayValue
        {
            get
            {
                if (ValueSpecified)
                {
                    return Value;
                }
                return Replacement;
            }
        }

        public override bool IsMatch(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            string context;

            if (ValueSpecified)
            {
                context = Value;
            }
            else
            {
                context = Replacement;
            }

            return context.PartiallyContains(text);
        }
    }
}