using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class Round
    {
        [DataMember(Name = "minutes")]
        public long Minutes { get; set; }

        [DataMember(Name = "round")]
        public string RoundRound { get; set; }
    }
}