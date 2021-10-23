using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class VisualStudioEnvironmentDataProvider : IAdditionalEnvironmentDataProvider
    {
        public void Process(EnvironmentContext context)
        {
            using var provider = DTEInstanceProvider.GetActiveInstance();
            var solution = provider.Instance.Solution;
            context.Path = solution.DTE.Documents.DTE.ActiveDocument?.FullName ?? solution.FullName;
        }
    }
}