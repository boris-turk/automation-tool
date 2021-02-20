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
            if (!context.WindowTitle.Contains("Visual Studio"))
                yield break;

            yield return new AhkSendRequest
            {
                Text = "close all tabs but current",
                Keys = "!^c"
            };
        }
    }
}