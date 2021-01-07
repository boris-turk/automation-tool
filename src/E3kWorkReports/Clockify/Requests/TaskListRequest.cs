using System.Collections.Generic;
using System.Runtime.Serialization;
using AutomationEngine.RestApi;
using E3kWorkReports.Clockify.DomainModel;

namespace E3kWorkReports.Clockify.Requests
{
    [DataContract]
    public class TaskListRequest : IGetRequest<List<Task>>
    {
        public TaskListRequest(string workspaceId, string projectId)
        {
            WorkspaceId = workspaceId;
            ProjectId = projectId;
        }

        public string WorkspaceId { get; }

        public string ProjectId { get; }

        [DataMember(Name = "page-size")]
        public int PageSize { get; } = int.MaxValue;

        string IRequest.EndPointPath => $"/workspaces/{WorkspaceId}/projects/{ProjectId}/tasks";
    }
}