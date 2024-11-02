using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

public class CommitRepositoryRequest : Request
{
    public CommitRepositoryRequest()
    {
        Configure()
            .SetText("commit")
            .AddChildRequestsProvider<Repository>()
            .SetCommand(r => new CommitRepositoryCommand(r));
    }
}