using System;
using System.Collections.Generic;
using AutomationEngine.RestApi;
using E3kWorkReports.Clockify.Requests;

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

        private static Dictionary<Type, EndpointConfiguration> GetSupportedEndPoints()
        {
            return new Dictionary<Type, EndpointConfiguration>
            {
                {typeof(UserRequest), new EndpointConfiguration("user", EndPointType.Get)}
            };
        }
    }
}