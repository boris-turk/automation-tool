using System;
using System.Collections.Generic;
using System.Linq;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    public class Request
    {
        public Request()
        {
        }

        public Request(string text)
        {
            Text = text;
        }

        public string Text { get; set; }

        public Action Action { get; set; }

        public virtual IEnumerable<Request> ChildRequests(EnvironmentContext context)
        {
            return Enumerable.Empty<Request>();
        }

        public override string ToString() => Text;
    }
}