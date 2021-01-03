using System;
using System.Collections.Generic;
using System.Linq;

namespace AutomationEngine.RestApi
{
    public class RestApiConfiguration
    {
        public RestApiConfiguration(List<EndpointConfiguration> endPoints)
        {
            Headers = new Dictionary<string, string>();
            EndPoints = endPoints;
        }

        public List<EndpointConfiguration> EndPoints { get; }

        public int Timeout { get; set; }

        public string ServerAddress { get; set; }

        public string ContentType { get; set; }

        public Dictionary<string, string> Headers { get; set; }

        public string GetEndPointMethod<TRequest>() where TRequest : IRequest
        {
            var endPoint = GetEndPoint<TRequest>();
            return endPoint.Method;
        }

        public string GetEndPointPath<TRequest>(TRequest request) where TRequest : IRequest
        {
            var endPoint = GetEndPoint<TRequest>();
            return endPoint.GetPath(request);
        }

        private EndpointConfiguration<TRequest> GetEndPoint<TRequest>() where TRequest : IRequest
        {
            var candidates = EndPoints.OfType<EndpointConfiguration<TRequest>>().ToList();

            if (candidates.Count == 0)
                throw new Exception($"Unknown request type {typeof(TRequest).FullName}");

            if (candidates.Count > 1)
                throw new Exception($"Multiple endpoints registered for request type {typeof(TRequest).FullName}");

            return candidates.Single();
        }
    }
}