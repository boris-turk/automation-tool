using System;
using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class TimeInterval
    {
        [DataMember(Name = "end")]
        public DateTime End { get; set; }

        [DataMember(Name = "start")]
        public DateTime Start { get; set; }
    }
}