using System;
using System.Linq;

namespace AutomationEngine
{
    [Serializable]
    public class Context : PatternPart
    {
        public string ReplacedValue { get; set; }

        public override string DisplayValue
        {
            get
            {
                if (ValueSpecified)
                {
                    return Value;
                }
                return ReplacedValue;
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
                context = ReplacedValue;
            }

            if (string.IsNullOrEmpty(context))
            {
                return false;
            }

            int j = 0;
            for (int i = 0; i < context.Length; i++)
            {
                if (j >= text.Length)
                {
                    break;
                }
                if (context[i] == text[j])
                {
                    j++;
                }
            }

            return j == text.Length;
        }
    }
}