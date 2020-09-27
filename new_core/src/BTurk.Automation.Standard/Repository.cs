using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class Repository : SearchItem
    {
        public RepositoryType Type { get; set; }

        public string AbsolutePath { get; set; }
    }
}