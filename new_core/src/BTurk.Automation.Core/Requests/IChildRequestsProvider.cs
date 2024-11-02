using System.Collections.Generic;

namespace BTurk.Automation.Core.Requests;

public interface IChildRequestsProvider
{
    IEnumerable<TRequest> LoadChildren<TRequest>() where TRequest : IRequest;
}