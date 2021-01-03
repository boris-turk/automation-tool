using System;
using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "activeWorkspace")]
        public string ActiveWorkspace { get; set; }

        [DataMember(Name = "defaultWorkspace")]
        public string DefaultWorkspace { get; set; }

        [DataMember(Name = "email")]
        public string Email { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "memberships")]
        public Membership[] Memberships { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "profilePicture")]
        public Uri ProfilePicture { get; set; }

        [DataMember(Name = "settings")]
        public Settings Settings { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }
    }
}
