using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class Repository : SearchItem
    {
        public Repository(string text, RepositoryType type, string absolutePath) : base(text)
        {
            Type = type;
            AbsolutePath = absolutePath;
        }

        public RepositoryType Type { get; }

        public string AbsolutePath { get; }
    }
}