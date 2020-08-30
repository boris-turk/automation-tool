using System;

namespace BTurk.Automation.Core.SearchEngine
{
    public class SequentialRequest : Request
    {
        public bool CanMoveNext { get; set; }

        public Func<string, string> FilterProvider { get; set; }
    }
}