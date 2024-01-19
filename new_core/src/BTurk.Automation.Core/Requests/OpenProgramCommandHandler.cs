using System.Diagnostics;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.Configuration;

namespace BTurk.Automation.Core.Requests;

public class OpenProgramCommandHandler : ICommandHandler<OpenProgramRequest>
{
    public OpenProgramCommandHandler(IConfigurationProvider configurationProvider)
    {
        ConfigurationProvider = configurationProvider;
    }

    private IConfigurationProvider ConfigurationProvider { get; }

    public void Handle(OpenProgramRequest command)
    {
        var path = ConfigurationProvider.Configuration.GetProgramPath(command.ProgramName);
        Process.Start(path);
    }
}