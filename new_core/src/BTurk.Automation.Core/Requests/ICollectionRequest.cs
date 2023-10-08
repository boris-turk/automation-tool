using System.Collections.Generic;

namespace BTurk.Automation.Core.Requests;

public interface ICollectionRequest<TRequest> where TRequest : IRequest
{
    IEnumerable<IRequest> GetRequests(RequestLoadContext<TRequest> context);
}