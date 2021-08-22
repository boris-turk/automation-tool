using System.Collections.Generic;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class OpenNoteRequest : Request
    {
        public OpenNoteRequest() : base("note")
        {
        }

        protected override IEnumerable<Request> ChildRequests(EnvironmentContext context)
        {
            yield return new SelectionRequest<Note>
            {
                ChildExecuted = note => note.Open()
            };
        }
    }
}