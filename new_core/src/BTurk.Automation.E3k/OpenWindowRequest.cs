using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.E3k;

public class OpenWindowRequest : CollectionRequest
{
    public OpenWindowRequest() : base("window")
    {
    }

    protected override IEnumerable<IRequest> GetRequests()
    {
        yield return new SelectionRequest<Module>
        {
            //Selected = solution => solution.Open()
        };
    }
}