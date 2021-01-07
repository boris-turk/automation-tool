using System.Collections.Generic;
using System.Runtime.Serialization;
using AutomationEngine.RestApi;
using E3kWorkReports.Clockify.DomainModel;

namespace E3kWorkReports.Clockify.Requests
{
    [DataContract]
    public class WorkspaceListRequest : IGetRequest<List<Workspace>>
    {
        string IRequest.EndPointPath => "/workspaces";
    }
}