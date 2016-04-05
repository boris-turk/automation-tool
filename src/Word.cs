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

        public override bool IsMatch(string text)
        {
            if (string.IsNullOrWhiteSpace(Value))
            {
                return false;
            }

            return Value.StartsPartiallyWith(text);
        }
    }
}