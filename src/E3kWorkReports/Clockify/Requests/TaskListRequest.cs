using System.Collections.Generic;
using AutomationEngine.RestApi;
using E3kWorkReports.Clockify.DomainModel;

namespace E3kWorkReports.Clockify.Requests
{
    public class TaskListRequest : IGetRequest<List<Task>>
    {
        public TaskListRequest(string workspaceId, string projectId)
        {
            WorkspaceId = workspaceId;
            ProjectId = projectId;
        }

        public string WorkspaceId { get; }

        public string ProjectId { get; }

        string IRequest.EndPointPath => $"/workspaces/{WorkspaceId}/projects/{ProjectId}/tasks";
    }
}