using BTurk.Automation.Core.Annotations;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard;

[IgnoreUnusedTypeWarning<VisualStudioEnvironmentDataProvider>]
public class VisualStudioEnvironmentDataProvider : IAdditionalEnvironmentDataProvider
{
    public void Process(EnvironmentContext context)
    {
        if (!context.IsVisualStudio())
            return;

        using var provider = DTEInstanceProvider.GetActiveInstance();
        var solution = provider.Instance.Solution;
        context.Path = solution.DTE.Documents.DTE.ActiveDocument?.FullName ?? solution.FullName;
    }
}