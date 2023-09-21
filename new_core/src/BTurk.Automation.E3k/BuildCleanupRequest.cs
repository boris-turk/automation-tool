using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.Standard;

namespace BTurk.Automation.E3k
{
    public class BuildCleanupRequest : CollectionRequest<Repository>
    {
        public BuildCleanupRequest() : base("cleanup")
        {
        }

        protected override void OnRequestLoaded(Repository repository)
        {
            repository.Command = new BuildCleanupCommand(repository.Path);
        }

        protected override bool CanLoadRequest(Repository repository, EnvironmentContext context)
        {
            return repository.IsTrunk() || repository.IsClean() || repository.Revision() >= 8;
        }
    }
}