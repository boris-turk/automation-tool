using System.Collections.Generic;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.UnitTests;

public class FakeChildRequestsProvider : IChildRequestsProviderV2
{
    public IEnumerable<IRequestV2> LoadChildren<TRequest>() where TRequest : IRequestV2
    {
        return [];
    }
}
