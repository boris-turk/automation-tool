using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class Total
    {
        [DataMember(Name = "totalTime")]
        public int TotalTime { get; set; }

        [DataMember(Name = "totalBillableTime")]
        public int TotalBillableTime { get; set; }

        [DataMember(Name = "entriesCount")]
        public int EntriesCount { get; set; }

        [DataMember(Name = "totalAmount")]
        public int TotalAmount { get; set; }
    }
}