using System.Runtime.Serialization;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    [DataContract]
    public class Repository : Request
    {
        [DataMember(Name = "Type")]
        public RepositoryType Type { get; set; }

        [DataMember(Name = "Path")]
        public string Path { get; set; }
    }
}