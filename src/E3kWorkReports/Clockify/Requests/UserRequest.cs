using AutomationEngine.RestApi;
using E3kWorkReports.Clockify.DomainModel;

namespace E3kWorkReports.Clockify.Requests
{
    public class UserRequest : IGetRequest<User>
    {
        string IRequest.EndPointPath => "/user";
    }
}