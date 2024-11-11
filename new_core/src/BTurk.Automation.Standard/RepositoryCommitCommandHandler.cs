using System.Linq;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Annotations;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

[IgnoreUnusedTypeWarning<RepositoryCommitCommandHandler>]
public class RepositoryCommitCommandHandler : RepositoryCommand<RepositoryCommitCommand>
{
    public RepositoryCommitCommandHandler(IProcessStarter processStarter,
        IRequestsProvider<Repository> repositoriesProvider)
        : base(repositoriesProvider)
    {
        ProcessStarter = processStarter;
    }

    private IProcessStarter ProcessStarter { get; }

    public override void Handle(RepositoryCommitCommand command)
    {
        var programPath = GetProgramPath(command.Path);
        var arguments = GetArguments("commit", command.Path);
        ProcessStarter.Start(programPath, arguments);
    }
}