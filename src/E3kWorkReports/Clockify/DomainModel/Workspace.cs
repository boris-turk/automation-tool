using System;
using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class Workspace
    {
        [DataMember(Name = "hourlyRate")]
        public HourlyRate HourlyRate { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "imageUrl")]
        public Uri ImageUrl { get; set; }

        [DataMember(Name = "memberships")]
        public Membership[] Memberships { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "workspaceSettings")]
        public WorkspaceSettings WorkspaceSettings { get; set; }
    }
}