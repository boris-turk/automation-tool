using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.Standard;

namespace BTurk.Automation.DependencyResolution
{
    public class MainMenuRequest : SelectionRequest<Request>
    {
        public MainMenuRequest() : base("Main menu")
        {
        }

        protected override IEnumerable<Request> ChildRequests(EnvironmentContext context)
        {
            if (context != EnvironmentContext.Empty)
                yield break;

            yield return new CommitRepositoryRequest();
            yield return new ShowRepositoryLogRequest();
            yield return new OpenSolutionRequest();
            yield return new OpenNoteRequest();
        }

        protected override bool CanVisit(VisitPredicateContext context)
        {
            return context.EnvironmentContext == EnvironmentContext.Empty;
        }
    }
}