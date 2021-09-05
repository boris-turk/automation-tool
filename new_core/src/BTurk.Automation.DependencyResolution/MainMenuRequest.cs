using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.E3k;
using BTurk.Automation.Standard;

namespace BTurk.Automation.DependencyResolution
{
    public class MainMenuRequest : Request, ICollectionRequest
    {
        public MainMenuRequest() : base("Main menu")
        {
        }

        public IEnumerable<IRequest> GetRequests(EnvironmentContext context)
        {
            if (context != EnvironmentContext.Empty)
                yield break;

            yield return new CommitRepositoryRequest();
            yield return new ShowRepositoryLogRequest();
            yield return new OpenSolutionRequest();
            yield return new OpenNoteRequest();
            yield return new OpenWindowRequest();
            yield return new UrlCollectionRequest();
            yield return new GitConsoleRequest();
            yield return new BuildCleanupRequest();
        }

        protected override bool CanAccept(DispatchPredicateContext context)
        {
            return context.EnvironmentContext == EnvironmentContext.Empty;
        }
    }
}