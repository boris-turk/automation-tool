using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class ShowRepositoryLogRequest : Request
    {
        public ShowRepositoryLogRequest() : base("log")
        {
        }

        protected override IEnumerable<Request> ChildRequests(EnvironmentContext context)
        {
            yield return new SelectionRequest<Repository>
            {
                Selected = repository => repository.Log()
            };
        }
    }
}