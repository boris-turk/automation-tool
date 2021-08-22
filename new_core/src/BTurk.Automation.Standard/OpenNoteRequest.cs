using System.Collections.Generic;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class OpenNoteRequest : Request, ISelectionRequest<Note>
    {
        public OpenNoteRequest() : base("note")
        {
        }

        public IEnumerable<Note> GetRequests(IRequestsProvider<Note> provider)
        {
            foreach (var request in provider.GetRequests())
            {
                request.Command = null; //request.Open();
                yield return request;
            }
        }
    }
}