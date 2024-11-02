using System.Collections.Generic;

namespace BTurk.Automation.Core.Requests;

public interface IChildRequestsProviderV2
{
    IEnumerable<TRequest> LoadChildren<TRequest>() where TRequest : IRequestV2;
}