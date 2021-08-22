using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Standard
{
    public abstract class RepositoryCommand<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
    {
        public abstract void Handle(TCommand command);

        protected string GetProgramPath(Repository repository)
        {
            return repository.Type == RepositoryType.Git
                ? @"C:\Program Files\TortoiseGit\bin\TortoiseGitProc.exe"
                : @"c:\Program Files\TortoiseSVN\bin\TortoiseProc.exe";
        }

        protected string GetArguments(string commandName, string path)
        {
            return $"/command:{commandName} /path:{path}";
        }
    }
}