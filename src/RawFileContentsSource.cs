using System;

namespace AutomationEngine
{
    [Serializable]
    public class RawFileContentsSource
    {
        public string Path { get; set; }
        public RegexReplacement NameRegex { get; set; }
        public RegexReplacement ReturnValueRegex { get; set; }
    }
}