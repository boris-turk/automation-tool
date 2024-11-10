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
                AhkRequest("close all tabs but current", "^!c"),
				AhkRequest("file folder", "^!t")
            );
    }

    private AhkSendRequest AhkRequest(string text, string keys)
    {
        var request = new AhkSendRequest { Text = text, Keys = keys };
        return request;
    }

    private bool IsVisualStudioContext(EnvironmentContext context)
    {
        return context.WindowTitle.Contains("Visual Studio");
    }
}
