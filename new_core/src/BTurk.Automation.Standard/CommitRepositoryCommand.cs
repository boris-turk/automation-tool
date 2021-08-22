using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Standard
{
    public class CommitRepositoryCommand : ICommand
    {
        public Repository Repository { get; }

        public CommitRepositoryCommand(Repository repository)
        {
            Repository = repository;
        }
    }
}