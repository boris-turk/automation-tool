using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.Standard;

namespace BTurk.Automation.DependencyResolution
{
    public class MainMenuRequest : Request
    {
        public MainMenuRequest() : base("main menu")
        {
        }

        public override IEnumerable<Request> ChildRequests(EnvironmentContext context)
        {
            if (context != EnvironmentContext.Empty)
                yield break;

            yield return new CommitRepositoryRequest();
            yield return new OpenSolutionRequest();
            yield return new NotesRequest();
        }
    }
}