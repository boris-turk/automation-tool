using BTurk.Automation.Core.Requests;
using BTurk.Automation.Standard;

namespace BTurk.Automation.DependencyResolution;

public class RootMenuRequest : Request
{
    public RootMenuRequest()
    {
        Configure()
            .AddChildRequests(
                new MainMenuRequest(),
                new VisualStudioRequest()
            );
    }
}