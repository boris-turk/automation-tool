using System;
using System.Collections.Generic;
using System.Linq;

namespace BTurk.Automation.Core.Requests
{
    public class Request
    {
        public Request(string text)
        {
            Text = text;
        }

        public string Text { get; }

        public Action Action { get; set; }

        public virtual IEnumerable<Request> ChildRequests() => Enumerable.Empty<Request>();

        public override string ToString() => Text;
    }
}