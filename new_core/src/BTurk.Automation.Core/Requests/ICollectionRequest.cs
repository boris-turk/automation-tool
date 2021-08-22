using System.Collections.Generic;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public interface ICollectionRequest
    {
        IEnumerable<IRequest> GetRequests(EnvironmentContext context);
    }

    public interface ICollectionRequest<TRequest> where TRequest : IRequest
    {
        IEnumerable<TRequest> GetRequests(IRequestsProvider<TRequest> provider);
    }
}