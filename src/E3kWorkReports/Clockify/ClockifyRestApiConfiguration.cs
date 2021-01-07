using System.Collections.Generic;
using AutomationEngine;
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

        public string ApiKey { get; } = Encryption.Decrypt("VZUhNEXUoW08Yf0c+SEc23bdoyYV2vEx");

        private static List<EndpointConfiguration> GetSupportedEndPoints()
        {
            return new List<EndpointConfiguration>
            {
                new EndpointConfiguration<UsersRequest>(),
                new EndpointConfiguration<WorkspaceListRequest>(),
                new EndpointConfiguration<ProjectListRequest>(),
                new EndpointConfiguration<TaskListRequest>(),
                new EndpointConfiguration<TimeEntriesRequest>()
            };
        }
    }
}