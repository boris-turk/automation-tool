using System;
using System.Runtime.Serialization;
using AutomationEngine.RestApi;
using E3kWorkReports.Clockify.DomainModel;

namespace E3kWorkReports.Clockify.Requests
{
    [DataContract]
    public class DetailedReportRequest : IReportRequest<DetailedReport>
    {
        public DetailedReportRequest(string workspaceId)
        {
            WorkspaceId = workspaceId;
        }

        public string WorkspaceId { get; }

        [DataMember(Name = "dateRangeStart")]
        public DateTimeOffset Start { get; set; }

        [DataMember(Name = "dateRangeEnd")]
        public DateTimeOffset End { get; set; }

        [DataMember(Name = "detailedFilter")]
        public DetailedFilter DetailedFilter { get; } = new DetailedFilter();

        [DataMember(Name = "sortOrder")]
        public string SortOrder { get; } = "ASCENDING";

        [DataMember(Name = "exportType")]
        public string ExportType { get; } = "JSON";

        [DataMember(Name = "amountShown")]
        public string AmountShown { get; } = "HIDE_AMOUNT";

        [DataMember(Name = "users")]
        public EntityFilter Users { get; set; }

        [DataMember(Name = "projects")]
        public EntityFilter Projects { get; set; }

        [DataMember(Name = "tasks")]
        public EntityFilter Tasks { get; set; }

        string IRequest.EndPointPath => $"/workspaces/{WorkspaceId}/reports/detailed";
    }
}