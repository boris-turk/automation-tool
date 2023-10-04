using BTurk.Automation.Core;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.Standard;

public class ConfigurationProvider : IConfigurationProvider
{
    private Configuration _configuration;

    private readonly IResourceProvider _resourceProvider;

    public ConfigurationProvider(IResourceProvider resourceProvider)
    {
        _resourceProvider = resourceProvider;
    }

    public Configuration Configuration => _configuration ??= _resourceProvider.Load<Configuration>("configuration");
}