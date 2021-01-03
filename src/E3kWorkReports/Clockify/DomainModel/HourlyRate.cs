using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class HourlyRate
    {
        [DataMember(Name = "amount")]
        public long Amount { get; set; }

        [DataMember(Name = "currency")]
        public string Currency { get; set; }
    }
}
