using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class Repository : Request
    {
        public RepositoryType Type { get; set; }

        public string AbsolutePath { get; set; }
    }
}