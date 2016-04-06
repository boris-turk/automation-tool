using System;

namespace AutomationEngine
{
    [Serializable]
    public class Word : PatternPart
    {
        public override string DisplayValue
        {
            get { return Value; }
        }

        public bool IsContext { get; set; }

        public override bool IsMatch(string text)
        {
            if (string.IsNullOrWhiteSpace(Value))
            {
                return false;
            }

            if (IsContext)
            {
                return Value.PartiallyContains(text);
            }
            else
            {
                return Value.StartsPartiallyWith(text);
            }
        }
    }
}