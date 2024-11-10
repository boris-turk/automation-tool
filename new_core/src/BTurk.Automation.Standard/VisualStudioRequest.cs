using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

public class VisualStudioRequest : Request
{
    public VisualStudioRequest()
    {
        Configure()
            .ProcessCondition(c => c.IsVisualStudio())
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
}
