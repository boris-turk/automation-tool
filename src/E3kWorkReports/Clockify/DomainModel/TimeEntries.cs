using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class TimeEntries
    {
        [DataMember(Name = "billable")]
        public bool Billable { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "isLocked")]
        public bool IsLocked { get; set; }

        [DataMember(Name = "projectId")]
        public string ProjectId { get; set; }

        [DataMember(Name = "tagIds")]
        public string[] TagIds { get; set; }

        [DataMember(Name = "taskId")]
        public string TaskId { get; set; }

        [DataMember(Name = "timeInterval")]
        public TimeInterval TimeInterval { get; set; }

        [DataMember(Name = "userId")]
        public string UserId { get; set; }

        [DataMember(Name = "workspaceId")]
        public string WorkspaceId { get; set; }
    }
}