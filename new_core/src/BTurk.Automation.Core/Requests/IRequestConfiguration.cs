using System.Collections.Generic;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests;

public interface IRequestConfiguration
{
    string Text { get; }
    bool ScanChildrenIfUnmatched { get; }
    bool CanHaveChildren { get; }
    bool CanProcess(IRequest childRequest, EnvironmentContext environmentContext);
    bool ExecuteCommand(ICommandProcessor commandProcessor, IRequest childRequest);
    IEnumerable<IRequest> GetChildren(IChildRequestsProvider childRequestsProvider);
}