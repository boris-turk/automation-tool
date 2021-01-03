using System.Runtime.Serialization;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class Task
    {
        [DataMember(Name = "assigneeIds")]
        public string[] AssigneeIds { get; set; }

        [DataMember(Name = "estimate")]
        public string Estimate { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "projectId")]
        public string ProjectId { get; set; }

        [DataMember(Name = "status")]
        public string Status { get; set; }
    }
}