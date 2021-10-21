using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.E3k
{
    public class BuildCleanupCommand : IAsyncCommand
    {
        public BuildCleanupCommand(string rootPath)
        {
            RootPath = rootPath;
        }

        public string RootPath { get; }
    }
}