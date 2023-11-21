using System.Diagnostics;
using BTurk.Automation.Core.Configuration;
using BTurk.Automation.Core.Requests;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.Standard;

public static class Extensions
{
    public static void Open(this IFileRequest request)
    {
        Process.Start(request.Path);
    }

    public static string GetIslProgramPath(this SystemConfiguration configuration)
    {
        return configuration.GetProgramPath("Isl");
    }

    public static string GetLocalAutomationConfigurationDirectory(this SystemConfiguration configuration)
    {
        return configuration.GetProgramPath("LocalAutomationConfiguration");
    }
}