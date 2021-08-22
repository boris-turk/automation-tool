using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.Standard;

namespace BTurk.Automation.DependencyResolution
{
    public class RootMenuRequest : Request, ISelectionRequest
    {
        public RootMenuRequest() : base("Root")
        {
        }

        public IEnumerable<Request> GetRequests(EnvironmentContext context)
        {
            yield return new MainMenuRequest();
            yield return new VisualStudioRequest();
        }
    }
}