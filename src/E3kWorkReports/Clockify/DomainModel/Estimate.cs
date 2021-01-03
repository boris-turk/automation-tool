using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class Estimate
    {
        [DataMember(Name = "estimate")]
        public string EstimateEstimate { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}