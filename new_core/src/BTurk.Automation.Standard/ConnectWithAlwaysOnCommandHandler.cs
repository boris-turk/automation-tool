using BTurk.Automation.Core;
using BTurk.Automation.Core.Annotations;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.Configuration;
using BTurk.Automation.Core.Queries;
using BTurk.Automation.Standard.SecurityServices;

namespace BTurk.Automation.Standard;

[IgnoreUnusedTypeWarning<ConnectWithAlwaysOnCommandHandler>]
public class ConnectWithAlwaysOnCommandHandler : ICommandHandler<ConnectWithAlwaysOnCommand>
{
    public ConnectWithAlwaysOnCommandHandler(IProcessStarter processStarter, IQueryProcessor queryProcessor,
        IConfigurationProvider configurationProvider)
    {
        ProcessStarter = processStarter;
        ConfigurationProvider = configurationProvider;
        QueryProcessor = queryProcessor;
    }

    private IProcessStarter ProcessStarter { get; }

    private IQueryProcessor QueryProcessor { get; }

    private IConfigurationProvider ConfigurationProvider { get; }

    public void Handle(ConnectWithAlwaysOnCommand command)
    {
        var credentials = QueryProcessor.Process(new UserCredentialsQuery("Xlab", "ISL Pronto"));

        var password = credentials.Password;
        var userName = credentials.UserName;
        var computerName = command.ComputerName;

        var arguments = $"--connect-search \"{computerName}\" --username {userName} --password {password}";

        var programPath = ConfigurationProvider.Configuration.GetIslProgramPath();

        ProcessStarter.Start(programPath, arguments);
    }
}