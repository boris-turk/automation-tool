using BTurk.Automation.Core;
using BTurk.Automation.Core.Commands;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace BTurk.Automation.Standard;

public class OpenGitConsoleCommandHandler : ICommandHandler<OpenGitConsoleCommand>
{
    private readonly IProcessStarter _processStarter;

    public OpenGitConsoleCommandHandler(IProcessStarter processStarter)
    {
        _processStarter = processStarter;
    }

    public void Handle(OpenGitConsoleCommand command)
    {
        var minttyExe = @"C:\Program Files\Git\usr\bin\mintty.exe";
        var gitBash = @"C:\Program Files\Git\git-bash.exe";

        var gitBashOptions =
            $"--dir \"{command.Directory}\" -o AppID=GitForWindows.Bash -o AppLaunchCmd=\"{gitBash}\" " +
            $"-o AppName=\"Git Bash\" -i \"{gitBash}\" --store-taskbar-properties -- /usr/bin/bash --login -i";

        var argument = string.Join(" ", gitBashOptions);

        _processStarter.Start(minttyExe, argument);
    }
}