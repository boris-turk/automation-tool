using System;
using System.Collections.Generic;

namespace AutomationEngine.RestApi
{
    public class RestApiConfiguration
    {
        public RestApiConfiguration(Dictionary<Type, EndpointConfiguration> endPoints)
        {
            Headers = new Dictionary<string, string>();
            EndPoints = endPoints;
        }

        public Dictionary<Type, EndpointConfiguration> EndPoints { get; }

        public int Timeout { get; set; }

        public string ServerAddress { get; set; }

        public string ContentType { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public EndPointType GetEndPointType<TRequest>()
        {
            var endPoint = GetEndPoint<TRequest>();
            return endPoint.Type;
        }

        public string GetEndPointName<TRequest>()
        {
            var endPoint = GetEndPoint<TRequest>();
            return endPoint.Name;
        }

        private EndpointConfiguration GetEndPoint<TRequest>()
        {
            if (EndPoints.TryGetValue(typeof(TRequest), out var endPoint))
                return endPoint;

            throw new Exception($"Unknown request type {typeof(TRequest).FullName}");
        }
    }
}