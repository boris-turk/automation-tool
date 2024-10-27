using System.Collections.Generic;

namespace BTurk.Automation.Core.Requests;

public interface IChildRequestsProviderV2
{
    IEnumerable<IRequestV2> LoadChildren<TRequest>() where TRequest : IRequestV2;
}