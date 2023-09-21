using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public abstract class CollectionRequest : CollectionRequest<IRequest>
{
    protected CollectionRequest()
    {
    }

    protected CollectionRequest(string text) : base(text)
    {
    }
}

public abstract class CollectionRequest<TRequest> : Request, ICollectionRequest<TRequest> where TRequest : IRequest
{
    protected CollectionRequest()
    {
    }

    protected CollectionRequest(string text) : base(text)
    {
    }

    protected virtual IEnumerable<TRequest> GetRequests()
    {
        return Enumerable.Empty<TRequest>();
    }

    protected virtual void OnRequestLoaded(TRequest request)
    {
    }

    protected virtual bool CanLoadRequest(TRequest request, EnvironmentContext context)
    {
        return true;
    }

    IEnumerable<IRequest> ICollectionRequest<TRequest>.GetRequests(RequestLoadContext<TRequest> context)
    {
        var allRequests = context.SuppliedRequests.Union(GetRequests());

        foreach (var request in allRequests)
        {
            if (!CanLoadRequest(request, context.EnvironmentContext))
                continue;

            OnRequestLoaded(request);

            yield return request;
        }
    }
}
