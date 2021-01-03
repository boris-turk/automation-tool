using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class Membership
    {
        [DataMember(Name = "hourlyRate")]
        public HourlyRate HourlyRate { get; set; }

        [DataMember(Name = "membershipStatus")]
        public string MembershipStatus { get; set; }

        [DataMember(Name = "membershipType")]
        public string MembershipType { get; set; }

        [DataMember(Name = "targetId")]
        public string TargetId { get; set; }

        [DataMember(Name = "userId")]
        public string UserId { get; set; }
    }
}
