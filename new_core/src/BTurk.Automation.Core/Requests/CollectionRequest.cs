using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public abstract class CollectionRequest<TRequest> : Request, ICollectionRequest<TRequest> where TRequest : IRequest
{
    protected CollectionRequest()
    {
    }

    protected CollectionRequest(string text) : base(text)
    {
    }

    protected virtual IEnumerable<IRequest> GetRequests()
    {
        return [];
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
        var allRequests = context.SuppliedRequests.Cast<IRequest>().Union(GetRequests());

        foreach (var request in allRequests)
        {
            if (request is TRequest properRequest)
            {
                if (!CanLoadRequest(properRequest, context.EnvironmentContext))
                    continue;

                OnRequestLoaded(properRequest);
            }

            yield return request;
        }
    }
}
