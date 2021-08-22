using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.E3k
{
    public class OpenWindowRequest : Request, ICollectionRequest
    {
        public OpenWindowRequest() : base("window")
        {
        }

        public IEnumerable<IRequest> GetRequests(EnvironmentContext context)
        {
            yield return new SelectionRequest<Module>()
            {
                //Selected = solution => solution.Open()
            };
        }
    }
}