namespace AutomationEngine
{
    public class ContentSource
    {
        public ContentSource()
        {
            NameRegex = new RegexReplacement();
            ReturnValueRegex = new RegexReplacement();
        }

        public RegexReplacement NameRegex { get; set; }

        public bool NameRegexSpecified
        {
            get
            {
                return NameRegex.SearchRegexSpecified ||
                    NameRegex.ReplacementSpecified;
            }
        }

        public RegexReplacement ReturnValueRegex { get; set; }

        public bool ReturnValueRegexSpecified
        {
            get
            {
                return ReturnValueRegex.SearchRegexSpecified ||
                    ReturnValueRegex.ReplacementSpecified;
            }
        }
    }
}