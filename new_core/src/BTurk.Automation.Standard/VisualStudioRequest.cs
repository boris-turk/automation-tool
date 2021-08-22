using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class VisualStudioRequest : Request, ICollectionRequest
    {
        public VisualStudioRequest() : base("Visual studio")
        {
        }

        public IEnumerable<IRequest> GetRequests(EnvironmentContext context)
        {
            if (!IsVisualStudioContext(context))
                yield break;

            yield return new AhkSendRequest
            {
                Text = "close all tabs but current",
                Keys = "!^c"
            };
        }

        protected override bool CanAccept(DispatchPredicateContext context)
        {
            return IsVisualStudioContext(context.EnvironmentContext);
        }

        private bool IsVisualStudioContext(EnvironmentContext context)
        {
            return context.WindowTitle.Contains("Visual Studio");
        }
    }
}