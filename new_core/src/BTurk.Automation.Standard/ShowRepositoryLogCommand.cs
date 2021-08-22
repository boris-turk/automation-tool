using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Standard
{
    public class ShowRepositoryLogCommand : ICommand
    {
        public Repository Repository { get; }

        public ShowRepositoryLogCommand(Repository repository)
        {
            Repository = repository;
        }
    }
}