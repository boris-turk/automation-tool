using System.Collections.Generic;
using System.Runtime.Serialization;
using AutomationEngine.RestApi;
using E3kWorkReports.Clockify.DomainModel;

namespace E3kWorkReports.Clockify.Requests
{
    [DataContract]
    public class UsersRequest : IGetRequest<List<User>>
    {
        public UsersRequest(string workspaceId)
        {
            WorkspaceId = workspaceId;
        }

        [IgnoreDataMember]
        public string WorkspaceId { get; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        string IRequest.EndPointPath => $"/workspaces/{WorkspaceId}/users";
    }
}