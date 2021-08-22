using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;
using BTurk.Automation.Standard;

namespace BTurk.Automation.DependencyResolution
{
    public class RootMenuRequest : Request
    {
        public RootMenuRequest() : base("Root")
        {
        }

        protected override IEnumerable<Request> ChildRequests(EnvironmentContext context)
        {
            yield return new MainMenuRequest();
            yield return new VisualStudioRequest();
        }
    }
}