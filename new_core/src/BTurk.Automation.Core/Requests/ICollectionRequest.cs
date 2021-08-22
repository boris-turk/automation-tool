using System.Collections.Generic;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public interface ICollectionRequest
    {
        IEnumerable<IRequest> GetRequests(EnvironmentContext context);
    }

    public interface ICollectionRequest<in TRequest> where TRequest : IRequest
    {
        void OnLoaded(TRequest childRequest);
    }
}