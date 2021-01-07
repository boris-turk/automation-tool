using System.Collections.Generic;
using AutomationEngine.RestApi;
using E3kWorkReports.Clockify.DomainModel;

namespace E3kWorkReports.Clockify.Requests
{
    public class WorkspaceListRequest : IGetRequest<List<Workspace>>
    {
        string IRequest.EndPointPath => "/workspaces";
    }
}