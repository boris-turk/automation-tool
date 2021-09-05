using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.E3k
{
    public class BuildCleanupCommand : ICommand
    {
        public BuildCleanupCommand(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }
    }
}