using System.Runtime.Serialization;

namespace BTurk.Automation.Core.Requests
{
    [DataContract]
    public class AhkSendRequest : Request
    {
        public string Keys { get; set; }
    }
}