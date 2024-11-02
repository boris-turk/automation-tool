using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard;

public class VisualStudioRequest : Request
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

    private bool IsVisualStudioContext(EnvironmentContext context)
    {
        return context.WindowTitle.Contains("Visual Studio");
    }
}