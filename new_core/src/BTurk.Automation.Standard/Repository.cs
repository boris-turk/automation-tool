using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class Repository : Request
    {
        public Repository(string text)
            : base(text)
        {
        }

        public RepositoryType Type { get; set; }

        public string AbsolutePath { get; set; }
    }
}