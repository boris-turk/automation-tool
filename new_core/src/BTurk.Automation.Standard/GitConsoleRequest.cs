using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class GitConsoleRequest : Request, ICollectionRequestFilter<Repository>
    {
        public GitConsoleRequest() : base("git")
        {
        }

        void ICollectionRequest<Repository>.OnLoaded(Repository repository)
        {
        }

        bool ICollectionRequestFilter<Repository>.CanLoad(Repository request)
        {
            return request.Type == RepositoryType.Git;
        }
    }
}