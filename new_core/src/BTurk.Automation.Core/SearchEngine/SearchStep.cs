using System.Collections.Generic;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Core.SearchEngine
{
    public class SearchStep
    {
        public SearchStep(Request request)
        {
            Request = request;
            Children = new List<Request>();
        }

        public Request Request { get; }

        public List<Request> Children { get; }

        public string Text { get; set; }
    }
}