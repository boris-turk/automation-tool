using System.Collections.Generic;

// ReSharper disable TypeParameterCanBeVariant

namespace BTurk.Automation.Core.Requests
{
    public interface IRequestsProvider<TRequest> where TRequest : Request
    {
        IEnumerable<TRequest> GetRequests();
    }
}