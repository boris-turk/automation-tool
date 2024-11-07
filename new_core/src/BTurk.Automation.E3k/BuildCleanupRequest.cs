using BTurk.Automation.Core.Requests;
using BTurk.Automation.Standard;

namespace BTurk.Automation.E3k;

public class BuildCleanupRequest : Request
{
    public BuildCleanupRequest()
    {
        Configure()
            .SetText("cleanup")
            .AddChildRequestsProvider<Repository>()
            .SetCommand(r => new BuildCleanupCommand(r.Path))
            .ProcessCondition(r => r.IsTrunk() || r.IsClean() || r.Revision() >= 8);
    }
}