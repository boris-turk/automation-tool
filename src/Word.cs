using System;

namespace AutomationEngine
{
    [Serializable]
    public class Word : PatternPart
    {
        public override string DisplayValue => Value;

        public bool IsContext { get; set; }

        public int MatchScore(string text)
        {
            if (Value.StartsWith(text, StringComparison.InvariantCultureIgnoreCase))
            {
                return 1000;
            }
            if (Value.StartsPartiallyWith(text))
            {
                return 100;
            }
            if (Value.ContainsPartially(text))
            {
                return 1;
            }
            return 0;
        }
    }
}