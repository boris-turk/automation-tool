using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using AutomationEngine.RestApi;
using E3kWorkReports.Clockify.DomainModel;

namespace E3kWorkReports.Clockify.Requests
{
    [DataContract]
    public class TimeEntriesRequest : IGetRequest<List<TimeEntries>>
    {
        public TimeEntriesRequest(string workspaceId, string userId)
        {
            WorkspaceId = workspaceId;
            UserId = userId;
        }

        public string UserId { get; }

        public string WorkspaceId { get; }

        [DataMember(Name = "start")]
        public DateTime? Start { get; set; }

        [DataMember(Name = "end")]
        public DateTime? End { get; set; }

        [DataMember(Name = "project")]
        public string Project { get; set; }

        [DataMember(Name = "page-size")]
        public int PageSize { get; } = int.MaxValue;

        string IRequest.EndPointPath => $"/workspaces/{WorkspaceId}/user/{UserId}/time-entries";
    }
}