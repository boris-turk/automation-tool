using System.Collections.Generic;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.UnitTests;

public class FakeChildRequestsProvider : IChildRequestsProviderV2
{
    IEnumerable<IRequestV2> IChildRequestsProviderV2.LoadChildren(IRequestV2 request)
    {
        return [];
    }
}
