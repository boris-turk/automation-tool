using System.Diagnostics;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Configuration;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

// ReSharper disable StringLiteralTypo
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

    public static bool IsVisualStudio(this EnvironmentContext context)
    {
        var isProperContext = context.WindowClass.ContainsIgnoreCase("HwndWrapper") &&
                              context.WindowTitle.ContainsIgnoreCase("Microsoft Visual Studio");

        return isProperContext;
    }
}