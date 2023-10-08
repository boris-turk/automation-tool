using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard;

public class GitConsoleRequest : CollectionRequest<Repository>
{
    public GitConsoleRequest() : base("git")
    {
    }

    protected override void OnRequestLoaded(Repository repository)
    {
        repository.Command = new OpenGitConsoleCommand(repository.Path);
    }

    protected override bool CanLoadRequest(Repository repository, EnvironmentContext context)
    {
        return repository.Type == RepositoryType.Git;
    }
}