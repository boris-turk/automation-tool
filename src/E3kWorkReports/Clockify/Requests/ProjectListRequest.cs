using System.Collections.Generic;
using AutomationEngine.RestApi;
using E3kWorkReports.Clockify.DomainModel;

namespace E3kWorkReports.Clockify.Requests
{
    public class ProjectListRequest : IGetRequest<List<Project>>
    {
        public ProjectListRequest(string workspaceId)
        {
            WorkspaceId = workspaceId;
        }

        public string WorkspaceId { get; }
    }
}