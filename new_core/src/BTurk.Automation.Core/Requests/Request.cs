using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    [DataContract]
    public class Request
    {
        public Request()
        {
        }

        public Request(string text)
        {
            Text = text;
        }

        [DataMember(Name = "Text")]
        public string Text { get; set; }

        public virtual IEnumerable<Request> ChildRequests(EnvironmentContext context)
        {
            return Enumerable.Empty<Request>();
        }

        public override string ToString() => Text;
    }
}