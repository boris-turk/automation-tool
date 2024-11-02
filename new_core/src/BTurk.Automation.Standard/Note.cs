using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

public class Note : Request, IFileRequest
{
    public Note()
    {
        Configure().SetCommand(new OpenWithDefaultProgramCommand(this));
    }

    public string Path { get; set; }
}