using AutomationEngine;
using AutomationEngine.RestApi;
using E3kWorkReports.Clockify.Requests;

namespace E3kWorkReports.Clockify
{
    public class ClockifyRestApiConfiguration : RestApiConfiguration
    {
        public ClockifyRestApiConfiguration()
        {
            Timeout = 10000;
            ContentType = "application/json";
            Headers.Add("X-Api-Key", ApiKey);
        }

        public string ApiKey { get; } = Encryption.Decrypt("VZUhNEXUoW08Yf0c+SEc23bdoyYV2vEx");

        public override string GetServerAddress<TRequest>(TRequest request)
        {
            var isReportRequest = typeof(TRequest).InheritsFrom(typeof(IReportRequest<>)); 
            return isReportRequest ? "https://reports.api.clockify.me/v1" : "https://api.clockify.me/api/v1";
        }
    }
}