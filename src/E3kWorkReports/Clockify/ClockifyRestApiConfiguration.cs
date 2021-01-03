using System.Collections.Generic;
using AutomationEngine.RestApi;
using E3kWorkReports.Clockify.Requests;

// ReSharper disable StringLiteralTypo

namespace E3kWorkReports.Clockify
{
    public class ClockifyRestApiConfiguration : RestApiConfiguration
    {
        public ClockifyRestApiConfiguration() :
            base(GetSupportedEndPoints())
        {
            Timeout = 10000;
            ContentType = "application/json";
            ServerAddress = "https://api.clockify.me/api/v1";

            Headers = new Dictionary<string, string>
            {
                {"X-Api-Key", ApiKey}
            };
        }

        public string ApiKey => "X/Bhfe0+VhbBLcyd";

        private static List<EndpointConfiguration> GetSupportedEndPoints()
        {
            return new List<EndpointConfiguration>
            {
                new EndpointConfiguration<UserRequest>(_ => "user"),
                new EndpointConfiguration<WorkspaceListRequest>(_ => "workspaces"),
                new EndpointConfiguration<ProjectListRequest>(_ => $"/workspaces/{_.WorkspaceId}/projects")
            };
        }
    }
}