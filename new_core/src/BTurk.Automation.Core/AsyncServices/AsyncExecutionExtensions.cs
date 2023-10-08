using BTurk.Automation.Core.AsyncServices;

// ReSharper disable CheckNamespace
// ReSharper disable UnusedMember.Global

namespace BTurk.Automation;

public static class AsyncExecutionExtensions
{
    public static void SetProgress(this IAsyncExecution asyncExecution, int current, int total)
    {
        asyncExecution.SetProgressData(new ProgressData(current, total));
    }

    public static void SetProgress(this IAsyncExecution asyncExecution, int percent)
    {
        asyncExecution.SetProgressData(new ProgressData(percent));
    }

    public static void SetProgressText(this IAsyncExecution asyncExecution, string text)
    {
        asyncExecution.SetProgressData(new ProgressData(text));
    }

    public static void SetProgressTextArguments(this IAsyncExecution asyncExecution, params object[] arguments)
    {
        asyncExecution.SetProgressData(new ProgressData(arguments));
    }
}