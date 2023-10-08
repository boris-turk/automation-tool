using BTurk.Automation.Core;
using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Standard;

public class OpenWithDefaultProgramCommandHandler : ICommandHandler<OpenWithDefaultProgramCommand>
{
    private readonly IProcessStarter _processStarter;

    public OpenWithDefaultProgramCommandHandler(IProcessStarter processStarter)
    {
        _processStarter = processStarter;
    }

    public void Handle(OpenWithDefaultProgramCommand command)
    {
        _processStarter.Start(command.FileRequest.Path);
    }
}