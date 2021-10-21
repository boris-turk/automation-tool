using System.Collections.Generic;
using System.Runtime.Serialization;
using AutomationEngine.RestApi;
using E3kWorkReports.Clockify.DomainModel;

namespace E3kWorkReports.Clockify.Requests
{
    [DataContract]
    public class ProjectListRequest : IGetRequest<List<Project>>
    {
        private const int MaxPageSize = 5000; // upper limit enforced by clockify

        public ProjectListRequest(string workspaceId)
        {
            WorkspaceId = workspaceId;
        }

        public string WorkspaceId { get; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "page-size")]
        public int PageSize { get; } = MaxPageSize;

        string IRequest.EndPointPath => $"/workspaces/{WorkspaceId}/projects";
    }
}