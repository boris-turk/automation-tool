using System.Collections.Generic;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Standard;

public class ConnectWithAlwaysOnCommandHandler : ICommandHandler<ConnectWithAlwaysOnCommand>
{
    private readonly IProcessStarter _processStarter;
    private readonly IConfigurationProvider _configurationProvider;

    public ConnectWithAlwaysOnCommandHandler(IProcessStarter processStarter,
        IConfigurationProvider configurationProvider)
    {
        _processStarter = processStarter;
        _configurationProvider = configurationProvider;
    }

    public void Handle(ConnectWithAlwaysOnCommand command)
    {
        var password = "";
        var userName = command.UserName;
        var computerName = command.ComputerName;

        var arguments = $"--connect-search \"{computerName}\" --username {userName} --password {password}";

        var programPath = _configurationProvider.Configuration.GetIslProgramPath();

        _processStarter.Start(programPath, arguments);
    }

    private Dictionary<string, string> Passwords => new()
    {
        {ConnectWithAlwaysOnCommand.BorisUserName, ""}
    };
}