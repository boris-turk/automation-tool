using System;
using System.Text.RegularExpressions;

namespace AutomationEngine
{
    [Serializable]
    public class RegularExpression : PatternPart
    {
        public override string DisplayValue
        {
            get { return string.Empty; }
        }

        public override bool IsMatch(string text)
        {
            if (string.IsNullOrWhiteSpace(Value))
            {
                return false;
            }
            return Regex.IsMatch(text, Value);
        }
    }
}