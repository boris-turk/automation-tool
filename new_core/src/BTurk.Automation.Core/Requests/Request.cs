using System;

namespace BTurk.Automation.Core.Requests
{
    public class Request : IRequest
    {
        public bool CanMoveNext { get; set; }

        public Func<string, string> FilterProvider { get; set; }
    }
}