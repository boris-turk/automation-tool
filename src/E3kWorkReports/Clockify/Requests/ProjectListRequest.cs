using System.Collections.Generic;
using System.Runtime.Serialization;
using AutomationEngine.RestApi;
using E3kWorkReports.Clockify.DomainModel;

namespace E3kWorkReports.Clockify.Requests
{
    [DataContract]
    public class ProjectListRequest : IGetRequest<List<Project>>
    {
        public ProjectListRequest(string workspaceId)
        {
            WorkspaceId = workspaceId;
        }

        public string WorkspaceId { get; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "page-size")]
        public int PageSize { get; } = int.MaxValue;

        string IRequest.EndPointPath => $"/workspaces/{WorkspaceId}/projects";
    }
}