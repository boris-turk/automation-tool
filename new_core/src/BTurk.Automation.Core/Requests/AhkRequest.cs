using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core.Requests
{
    [DataContract]
    public class AhkRequest : Request
    {
        public string Command { get; set; }
    }
}