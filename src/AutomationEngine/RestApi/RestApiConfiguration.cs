using System;
using System.Collections.Generic;

namespace AutomationEngine.RestApi
{
    public abstract class RestApiConfiguration
    {
        public const string GetMethod = "GET";
        public const string PostMethod = "POST";

        protected RestApiConfiguration()
        {
            Headers = new Dictionary<string, string>();
        }

        public int Timeout { get; set; }

        public string ContentType { get; set; }

        public Dictionary<string, string> Headers { get; }

        public abstract string GetServerAddress<TRequest>(TRequest request) where TRequest : IRequest;

        public string GetEndPointMethod<TRequest>() where TRequest : IRequest
        {
            if (typeof(TRequest).InheritsFrom(typeof(IGetRequest<>)))
                return GetMethod;

            if (typeof(TRequest).InheritsFrom(typeof(IPostRequest<>)))
                return PostMethod;

            throw new NotSupportedException($"Could not determine method type for request {typeof(TRequest).Name}");
        }
    }
}