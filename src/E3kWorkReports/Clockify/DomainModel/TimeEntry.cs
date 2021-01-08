using System.Collections.Generic;
using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class TimeEntry
    {
        [DataMember(Name = "_id")]
        public string Id { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "userId")]
        public string UserId { get; set; }

        [DataMember(Name = "billable")]
        public bool Billable { get; set; }

        [DataMember(Name = "taskId")]
        public object TaskId { get; set; }

        [DataMember(Name = "projectId")]
        public object ProjectId { get; set; }

        [DataMember(Name = "timeInterval")]
        public TimeInterval TimeInterval { get; set; }

        [DataMember(Name = "approvalRequestId")]
        public string ApprovalRequestId { get; set; }

        [DataMember(Name = "taskName")]
        public string TaskName { get; set; }

        [DataMember(Name = "tags")]
        public List<object> Tags { get; set; }

        [DataMember(Name = "isLocked")]
        public bool IsLocked { get; set; }

        [DataMember(Name = "customFields")]
        public List<CustomField> CustomFields { get; set; }

        [DataMember(Name = "invoicingInfo")]
        public InvoicingInfo InvoicingInfo { get; set; }

        [DataMember(Name = "amount")]
        public long Amount { get; set; }

        [DataMember(Name = "rate")]
        public long Rate { get; set; }

        [DataMember(Name = "userName")]
        public string UserName { get; set; }

        [DataMember(Name = "userEmail")]
        public string UserEmail { get; set; }

        [DataMember(Name = "projectName")]
        public string ProjectName { get; set; }

        [DataMember(Name = "projectColor")]
        public string ProjectColor { get; set; }

        [DataMember(Name = "clientName")]
        public string ClientName { get; set; }
    }
}