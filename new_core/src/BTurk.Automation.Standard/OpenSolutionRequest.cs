using System.Collections.Generic;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class OpenSolutionRequest : Request, ICollectionRequest<Solution>
    {
        public OpenSolutionRequest() : base("solution")
        {
        }

        public IEnumerable<Solution> GetRequests(IRequestsProvider<Solution> provider)
        {
            foreach (var request in provider.GetRequests())
            {
                request.Command = null; //request.Open();
                yield return request;
            }
        }
    }
}