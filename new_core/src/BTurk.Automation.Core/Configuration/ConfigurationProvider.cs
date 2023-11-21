// ReSharper disable UnusedMember.Global

using BTurk.Automation.Core.DataPersistence;
using BTurk.Automation.Core.FileSystem;

namespace BTurk.Automation.Core.Configuration;

public class ConfigurationProvider : IConfigurationProvider
{
    private SystemConfiguration _configuration;

    private readonly IResourceProvider _resourceProvider;

    public ConfigurationProvider(IResourceProvider resourceProvider)
    {
        _resourceProvider = resourceProvider;
    }

    public SystemConfiguration Configuration
    {
        get
        {
            return _configuration ??= _resourceProvider.Load<SystemConfiguration>(
                new FileParameters(DirectoryParameters.Configuration, "configuration.json")
            );
        }
    }
}