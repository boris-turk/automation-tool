using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Standard;

namespace BTurk.Automation.DependencyResolution
{
    public class RootMenuRequest : Request
    {
        public RootMenuRequest(string text) : base(text)
        {
        }

        public override IEnumerable<Request> ChildRequests()
        {
            yield return new CommitRepositoryRequest();
            yield return new OpenSolutionRequest();
        }
    }
}