using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

public class OpenNoteRequest : Request
{
    public OpenNoteRequest()
    {
        Configure()
            .SetText("note")
            .ScanChildrenIfUnmatched()
            .AddChildRequestsProvider<Note>()
            .SetCommand(r => new OpenWithDefaultProgramCommand(r));
    }
}