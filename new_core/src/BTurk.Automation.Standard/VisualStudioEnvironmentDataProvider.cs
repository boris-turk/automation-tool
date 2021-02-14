using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class VisualStudioEnvironmentDataProvider : IAdditionalEnvironmentDataProvider
    {
        public void Process(EnvironmentContext context)
        {
            using var provider = DTEInstanceProvider.GetActiveInstance();
            context.Paths.Add(provider.Instance.Solution.FullName);
            context.Paths.Add(provider.Instance.Solution.DTE.Documents.DTE.ActiveDocument.FullName);
        }
    }
}