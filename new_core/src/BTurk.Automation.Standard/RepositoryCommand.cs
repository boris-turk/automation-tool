using System.Linq;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

public abstract class RepositoryCommand<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    protected RepositoryCommand(IRequestsProvider<Repository> repositoriesProvider)
    {
        RepositoriesProvider = repositoriesProvider;
    }

    private IRequestsProvider<Repository> RepositoriesProvider { get; }

    public abstract void Handle(TCommand command);

    private RepositoryType GetRepositoryType(string path)
    {
        var type = (
            from repository in RepositoriesProvider.GetRequests()
            where repository.Path.StartsWithIgnoreCase(path)
            select repository.Type
            ).FirstOrDefault();

        return type;
    }

    protected string GetProgramPath(string path)
    {
        var repositoryType = GetRepositoryType(path);

        return repositoryType == RepositoryType.Git
            ? @"C:\Program Files\TortoiseGit\bin\TortoiseGitProc.exe"
            : @"c:\Program Files\TortoiseSVN\bin\TortoiseProc.exe";
    }

    protected string GetArguments(string commandName, string path)
    {
        return $"/command:{commandName} /path:{path}";
    }
}