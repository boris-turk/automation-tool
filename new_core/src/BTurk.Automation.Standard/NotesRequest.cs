using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class NotesRequest : Request, IRequestConsumer<Note>
    {
        public NotesRequest() : base("note")
        {
        }

        public void Execute(Note request)
        {
            request.Open();
        }
    }
}