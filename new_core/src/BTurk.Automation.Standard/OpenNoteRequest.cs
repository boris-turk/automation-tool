using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class OpenNoteRequest : Request, ICollectionRequest<Note>
    {
        public OpenNoteRequest() : base("note")
        {
        }

        void ICollectionRequest<Note>.OnLoaded(Note note)
        {
            note.Command = null; //request.Open();
        }
    }
}