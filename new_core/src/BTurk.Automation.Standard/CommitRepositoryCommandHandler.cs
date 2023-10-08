using BTurk.Automation.Core;

namespace BTurk.Automation.Standard;

public class CommitRepositoryCommandHandler : RepositoryCommand<CommitRepositoryCommand>
{
    private readonly IProcessStarter _processStarter;

    public CommitRepositoryCommandHandler(IProcessStarter processStarter)
    {
        _processStarter = processStarter;
    }

    public override void Handle(CommitRepositoryCommand command)
    {
        var programPath = GetProgramPath(command.Repository);
        var arguments = GetArguments("commit", command.Repository.Path);
        _processStarter.Start(programPath, arguments);
    }
}