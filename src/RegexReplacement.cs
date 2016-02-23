namespace AutomationEngine
{
    public class RegexReplacement
    {
        public RegexReplacement()
        {
            SearchRegex = string.Empty;
            Replacement = string.Empty;
        }

        public string SearchRegex { get; set; }

        public bool SearchRegexSpecified
        {
            get { return !string.IsNullOrWhiteSpace(SearchRegex);}
        }

        public string Replacement { get; set; }

        public bool ReplacementSpecified
        {
            get { return !string.IsNullOrWhiteSpace(Replacement);}
        }
    }
}