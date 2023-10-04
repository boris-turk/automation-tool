using System.Collections.Generic;
using BTurk.Automation.Core.Commands;

namespace BTurk.Automation.Standard;

public class ConnectWithAlwaysOnCommand : ICommand
{
    public const string BorisUserName = @"\\balx\boris_turk";

    public ConnectWithAlwaysOnCommand(string computerName)
        : this(computerName, BorisUserName)
    {
    }

    public ConnectWithAlwaysOnCommand(string computerName, string userName)
    {
        ComputerName = computerName;
        UserName = userName;
    }

    public string ComputerName { get; }

    public string UserName { get; }
}