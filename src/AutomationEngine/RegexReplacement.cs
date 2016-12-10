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

        public bool SearchRegexSpecified => !string.IsNullOrWhiteSpace(SearchRegex);

        public string Replacement { get; set; }

        public bool ReplacementSpecified => !string.IsNullOrWhiteSpace(Replacement);
    }
}