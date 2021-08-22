using BTurk.Automation.Core;

namespace BTurk.Automation.Standard
{
    public class ShowRepositoryLogCommandHandler : RepositoryCommand<ShowRepositoryLogCommand>
    {
        private readonly IProcessStarter _processStarter;

        public ShowRepositoryLogCommandHandler(IProcessStarter processStarter)
        {
            _processStarter = processStarter;
        }

        public override void Handle(ShowRepositoryLogCommand command)
        {
            var programPath = GetProgramPath(command.Repository);
            var arguments = GetArguments("log", command.Repository.Path);
            _processStarter.Start(programPath, arguments);
        }
    }
}