using BTurk.Automation.Core.Requests;
using BTurk.Automation.Standard;

namespace BTurk.Automation.E3k
{
    public class BuildCleanupRequest : Request, ICollectionRequestFilter<Repository>
    {
        public BuildCleanupRequest() : base("cleanup")
        {
        }

        void ICollectionRequest<Repository>.OnLoaded(Repository repository)
        {
            repository.Command = new BuildCleanupCommand(repository.Path);
        }

        bool ICollectionRequestFilter<Repository>.CanLoad(Repository repository)
        {
            return repository.IsTrunk() || repository.IsClean() || repository.Revision() >= 8;
        }
    }
}