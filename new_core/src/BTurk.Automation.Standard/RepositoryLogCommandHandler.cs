using BTurk.Automation.Core;
using BTurk.Automation.Core.Annotations;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

[IgnoreUnusedTypeWarning<RepositoryLogCommandHandler>]
public class RepositoryLogCommandHandler : RepositoryCommand<RepositoryLogCommand>
{
    public RepositoryLogCommandHandler(IProcessStarter processStarter,
        IRequestsProvider<Repository> repositoriesProvider)
        : base(repositoriesProvider)
    {
        ProcessStarter = processStarter;
    }

    private IProcessStarter ProcessStarter { get; }

    public override void Handle(RepositoryLogCommand command)
    {
        var programPath = GetProgramPath(command.Path);
        var arguments = GetArguments("log", command.Path);
        ProcessStarter.Start(programPath, arguments);
    }
}