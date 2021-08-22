using System.Collections.Generic;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public interface ISelectionRequest
    {
        IEnumerable<Request> GetRequests(EnvironmentContext context);
    }

    public interface ISelectionRequest<TRequest> where TRequest : Request
    {
        IEnumerable<TRequest> GetRequests(IRequestsProvider<TRequest> provider);
    }
}