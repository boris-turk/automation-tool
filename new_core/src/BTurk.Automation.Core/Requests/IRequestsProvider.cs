using System.Collections.Generic;

// ReSharper disable TypeParameterCanBeVariant

namespace BTurk.Automation.Core.Requests;

public interface IRequestsProvider<TRequest> where TRequest : IRequestV2
{
    IEnumerable<TRequest> GetRequests();
}