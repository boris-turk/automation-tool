using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Standard
{
    public class OpenGitConsoleCommand : ICommand
    {
        public OpenGitConsoleCommand(string directory)
        {
            Directory = directory;
        }

        public string Directory { get; set; }
    }
}