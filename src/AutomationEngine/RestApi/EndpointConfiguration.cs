using System;

namespace AutomationEngine.RestApi
{
    public class EndpointConfiguration
    {
        public const string GetMethod = "GET";
        public const string PostMethod = "POST";

        protected EndpointConfiguration(string method)
        {
            Method = method;
        }

        public string Method { get; }
    }

    public class EndpointConfiguration<TRequest> : EndpointConfiguration where TRequest : IRequest
    {
        public EndpointConfiguration()
            : base(DetermineMethod())
        {
        }

        public string GetPath(TRequest request) => request.EndPointPath;

        public static string DetermineMethod()
        {
            if (typeof(TRequest).InheritsFrom(typeof(IGetRequest<>)))
                return GetMethod;

            if (typeof(TRequest).InheritsFrom(typeof(IPostRequest<>)))
                return PostMethod;

            throw new NotSupportedException($"Could not determine method type for request {typeof(TRequest).Name}");
        }
    }
}