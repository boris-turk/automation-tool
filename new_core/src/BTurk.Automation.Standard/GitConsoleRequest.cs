using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class GitConsoleRequest : Request, ICollectionRequest<Repository>
    {
        public GitConsoleRequest() : base("git")
        {
        }

        void ICollectionRequest<Repository>.OnLoaded(Repository repository)
        {
        }
    }
}