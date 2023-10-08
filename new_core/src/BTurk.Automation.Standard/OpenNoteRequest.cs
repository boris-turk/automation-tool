using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard;

public class OpenNoteRequest : CollectionRequest<Note>
{
    public OpenNoteRequest() : base("note")
    {
    }

    protected override void OnRequestLoaded(Note note)
    {
        note.Command = new OpenWithDefaultProgramCommand(note);
    }
}