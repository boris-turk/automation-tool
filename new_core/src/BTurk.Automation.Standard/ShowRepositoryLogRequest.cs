using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

public class ShowRepositoryLogRequest : Request
{
    public ShowRepositoryLogRequest()
    {
        Configure()
            .SetText("log")
            .AddChildRequestsProvider<Repository>()
            .SetCommand(r => new ShowRepositoryLogCommand(r));
    }
}