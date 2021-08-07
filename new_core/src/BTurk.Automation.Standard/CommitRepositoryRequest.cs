using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class CommitRepositoryRequest : Request
    {
        public CommitRepositoryRequest() : base("commit")
        {
        }

        protected override IEnumerable<Request> ChildRequests(EnvironmentContext context)
        {
            yield return new SelectionRequest<Repository>
            {
                Selected = repository => repository.Commit()
            };
        }
    }
}
