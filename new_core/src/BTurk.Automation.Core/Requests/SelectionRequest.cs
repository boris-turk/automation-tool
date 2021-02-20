using System;

namespace BTurk.Automation.Core.Requests
{
    public class SelectionRequest<TRequest> : Request, ISelectionRequest where TRequest : Request
    {
        public SelectionRequest()
        {
        }

        public SelectionRequest(string text)
        {
            Text = text;
        }

        public Action<TRequest> Selected { get; set; }
    }
}