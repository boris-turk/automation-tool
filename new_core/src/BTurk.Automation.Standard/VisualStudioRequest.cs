using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class VisualStudioRequest : SelectionRequest<Request>
    {
        public VisualStudioRequest() : base("Visual studio")
        {
        }

        public override IEnumerable<Request> ChildRequests(EnvironmentContext context)
        {
            if (!IsVisualStudioContext(context))
                yield break;

            yield return new AhkSendRequest
            {
                Text = "close all tabs but current",
                Keys = "!^c"
            };
        }

        public override bool CanVisit(VisitPredicateContext context)
        {
            return IsVisualStudioContext(context.EnvironmentContext);
        }

        private bool IsVisualStudioContext(EnvironmentContext context)
        {
            return context.WindowTitle.Contains("Visual Studio");
        }
    }
}