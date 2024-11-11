using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Standard;

public class RepositoryLogCommand : ICommand
{
    public string Path { get; }

    public RepositoryLogCommand(string path)
    {
        Path = path;
    }
}