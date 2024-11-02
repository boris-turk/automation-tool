using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.E3k;

public class OpenWindowRequest : Request
{
    public OpenWindowRequest()
    {
        Configure().SetText("window");
    }
}