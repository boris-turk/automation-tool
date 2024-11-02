using System.Collections.Generic;
using System.Linq;

namespace BTurk.Automation.Core.Requests;

public class EmptyRequestProvider<TRequest> : IRequestsProvider<TRequest> where TRequest : IRequestV2
{
    public IEnumerable<TRequest> GetRequests()
    {
        return Enumerable.Empty<TRequest>();
    }
}