using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class OpenSolutionRequest : Request
    {
        public OpenSolutionRequest() : base("solution")
        {
        }

        protected override IEnumerable<Request> ChildRequests(EnvironmentContext context)
        {
            yield return new SelectionRequest<Solution>
            {
                Selected = solution => solution.Open()
            };
        }
    }
}