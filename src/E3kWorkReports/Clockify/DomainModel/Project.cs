using System.Runtime.Serialization;

namespace E3kWorkReports.Clockify.DomainModel
{
    [DataContract]
    public class Project
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "hourlyRate")]
        public object HourlyRate { get; set; }

        [DataMember(Name = "clientId")]
        public string ClientId { get; set; }

        [DataMember(Name = "workspaceId")]
        public string WorkspaceId { get; set; }

        [DataMember(Name = "billable")]
        public bool Billable { get; set; }

        [DataMember(Name = "memberships")]
        public Membership[] Memberships { get; set; }

        [DataMember(Name = "color")]
        public string Color { get; set; }

        [DataMember(Name = "estimate")]
        public Estimate Estimate { get; set; }

        [DataMember(Name = "archived")]
        public bool Archived { get; set; }

        [DataMember(Name = "duration")]
        public string Duration { get; set; }

        [DataMember(Name = "clientName")]
        public string ClientName { get; set; }

        [DataMember(Name = "note")]
        public string Note { get; set; }

        [DataMember(Name = "template")]
        public bool Template { get; set; }

        [DataMember(Name = "public")]
        public bool Public { get; set; }

        [DataMember(Name = "costRate")]
        public object CostRate { get; set; }

        [DataMember(Name = "budgetEstimate")]
        public object BudgetEstimate { get; set; }

        [DataMember(Name = "timeEstimate")]
        public TimeEstimate TimeEstimate { get; set; }
    }
}