using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard;

public class VisualStudioRequest : CollectionRequest<IRequest>
{
    public VisualStudioRequest()
    {
        Configure()
            .ProcessCondition(IsVisualStudioContext)
            .AddChildRequests(
                new AhkSendRequest
                {
                    Text = "close all tabs but current",
                    Keys = "!^c"
                }
            );
    }

    protected override IEnumerable<IRequest> GetRequests()
    {
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