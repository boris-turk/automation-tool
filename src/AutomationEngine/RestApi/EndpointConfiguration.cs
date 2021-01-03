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
        private readonly Func<TRequest, string> _pathProvider;

        public EndpointConfiguration(Func<TRequest, string> pathProvider)
            : base(DetermineMethod())
        {
            _pathProvider = pathProvider ?? throw new ArgumentNullException(nameof(pathProvider));
        }

        public string GetPath(TRequest request) => _pathProvider.Invoke(request);

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