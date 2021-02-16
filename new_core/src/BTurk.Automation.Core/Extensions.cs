namespace BTurk.Automation.Core
{
    public static class Extensions
    {
        public static bool StartsPartiallyWith(this string testedText, string value)
        {
            return ContainsPartially(testedText, value, true);
        }

        public static bool ContainsPartially(this string testedText, string value)
        {
            return ContainsPartially(testedText, value, false);
        }

        private static bool ContainsPartially(string testedText, string value, bool mustMatchAtStart)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            int j = 0;
            for (int i = 0; i < testedText.Length; i++)
            {
                if (j >= value.Length)
                {
                    break;
                }
                if (char.ToUpperInvariant(value[j]) == char.ToUpperInvariant(testedText[i]))
                {
                    j++;
                }
                else if (j == 0 && mustMatchAtStart)
                {
                    return false;
                }
            }

            return j == value.Length;
        }
    }
}