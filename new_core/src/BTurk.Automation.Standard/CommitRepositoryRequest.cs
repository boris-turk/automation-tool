using System.Collections.Generic;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class CommitRepositoryRequest : Request, ICollectionRequest<Repository>
    {
        public CommitRepositoryRequest() : base("commit")
        {
        }

        public IEnumerable<Repository> GetRequests(IRequestsProvider<Repository> provider)
        {
            foreach (var repository in provider.GetRequests())
            {
                repository.Command = null;
                yield return repository;
            }
        }
    }
}
