using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.E3k;
using BTurk.Automation.Standard;

namespace BTurk.Automation.DependencyResolution
{
    public class MainMenuRequest : Request, ISelectionRequest
    {
        public MainMenuRequest() : base("Main menu")
        {
        }

        public IEnumerable<Request> GetRequests(EnvironmentContext context)
        {
            if (context != EnvironmentContext.Empty)
                yield break;

            yield return new CommitRepositoryRequest();
            yield return new ShowRepositoryLogRequest();
            yield return new OpenSolutionRequest();
            yield return new OpenNoteRequest();
            yield return new OpenWindowRequest();
        }

        protected override bool CanAccept(DispatchPredicateContext context)
        {
            return context.EnvironmentContext == EnvironmentContext.Empty;
        }
    }
}