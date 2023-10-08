using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

public class ShowRepositoryLogRequest : CollectionRequest<Repository>
{
    public ShowRepositoryLogRequest() : base("log")
    {
    }

    protected override void OnRequestLoaded(Repository repository)
    {
        repository.Command = new ShowRepositoryLogCommand(repository);
    }
}