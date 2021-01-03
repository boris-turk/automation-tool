using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class AutomaticLock
    {
        [DataMember(Name = "changeDay")]
        public string ChangeDay { get; set; }

        [DataMember(Name = "dayOfMonth")]
        public long DayOfMonth { get; set; }

        [DataMember(Name = "firstDay")]
        public string FirstDay { get; set; }

        [DataMember(Name = "olderThanPeriod")]
        public string OlderThanPeriod { get; set; }

        [DataMember(Name = "olderThanValue")]
        public long OlderThanValue { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
}