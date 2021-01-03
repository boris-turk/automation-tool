using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class TimeEstimate
    {
        [DataMember(Name = "estimate")]
        public string Estimate { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "resetOption")]
        public object ResetOption { get; set; }

        [DataMember(Name = "active")]
        public bool Active { get; set; }
    }
}