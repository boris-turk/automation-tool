using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class SummaryReportSettings
    {
        [DataMember(Name = "group")]
        public string Group { get; set; }

        [DataMember(Name = "subgroup")]
        public string Subgroup { get; set; }
    }
}