using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

public class CommitRepositoryRequest : CollectionRequest<Repository>
{
    public CommitRepositoryRequest()
    {
        Configure()
            .SetText("commit")
            .AddChildRequestsProvider<Repository>();
    }

    protected override void OnRequestLoaded(Repository repository)
    {
        repository.Command = new CommitRepositoryCommand(repository);
    }
}