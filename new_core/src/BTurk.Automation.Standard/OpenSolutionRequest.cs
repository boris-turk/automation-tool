using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

public class OpenSolutionRequest : Request
{
    public OpenSolutionRequest()
    {
        Configure()
            .SetText("solution")
            .AddChildRequestsProvider<Solution>();
            //.SetCommand(r => new OpenWithDefaultProgramCommand(r));
    }
}