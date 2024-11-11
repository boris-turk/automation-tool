using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Standard;

public class RepositoryCommitCommand : ICommand
{
    public string Path { get; }

    public RepositoryCommitCommand(string path)
    {
        Path = path;
    }
}