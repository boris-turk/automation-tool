using System.Collections.Generic;

// ReSharper disable TypeParameterCanBeVariant

namespace BTurk.Automation.Core.Requests
{
    public interface IRequestsConsumer<TRequest> where TRequest : Request
    {
        void Add(IEnumerable<TRequest> requests);
    }
}