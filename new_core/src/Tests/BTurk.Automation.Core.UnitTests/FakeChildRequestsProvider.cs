using System.Collections.Generic;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.UnitTests;

public class FakeChildRequestsProvider : IChildRequestsProvider
{
    public IEnumerable<TRequest> LoadChildren<TRequest>() where TRequest : IRequest
    {
        return [];
    }
}
