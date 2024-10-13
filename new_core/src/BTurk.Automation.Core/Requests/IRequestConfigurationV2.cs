using System.Collections.Generic;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public interface IRequestConfigurationV2
{
    string Text { get; }
    bool ScanChildrenIfUnmatched { get; }
    bool CanHaveChildren { get; }
    bool CanProcess(EnvironmentContext environmentContext);
    IEnumerable<IRequestV2> GetChildren(IChildRequestsProviderV2 childRequestsProvider);
}