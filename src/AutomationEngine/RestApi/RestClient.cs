using System.IO;
using System.Net;
using System.Text;

namespace AutomationEngine.RestApi
{
    public class RestClient : IRestClient
    {
        private readonly RestApiConfiguration _configuration;

        public RestClient(RestApiConfiguration configuration)
        {
            _configuration = configuration;
        }

        public TResult SendRequest<TRequest, TResult>(TRequest request) where TRequest : IRequest<TResult>
        {
            var expect100Continue = ServicePointManager.Expect100Continue;
            var securityProtocol = ServicePointManager.SecurityProtocol;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = (_, __, ___, ____) => true;

                return SendRequestInternal<TRequest, TResult>(request);
            }
            finally
            {
                ServicePointManager.Expect100Continue = expect100Continue;
                ServicePointManager.SecurityProtocol = securityProtocol;
                ServicePointManager.ServerCertificateValidationCallback = null;
            }
        }

        private TResult SendRequestInternal<TRequest, TResult>(TRequest request) where TRequest : IRequest<TResult>
        {
            var requestParameters = GetParameters(request);

            var webRequest = (HttpWebRequest)WebRequest.Create(requestParameters.Url);
            webRequest.Method = requestParameters.RequestMethod;
            webRequest.ContentType = requestParameters.ContentType;
            webRequest.Timeout = requestParameters.RequestTimeout;

            foreach (var header in requestParameters.Headers)
                webRequest.Headers.Add(header.Key, header.Value);

            if (requestParameters.IsPostRequest)
            {
                var contentAsString = JsonConverter.ToJsonString(request);
                var contentAsByteArray = Encoding.UTF8.GetBytes(contentAsString);
                webRequest.ContentLength = contentAsByteArray.Length;

                using (var dataStream = webRequest.GetRequestStream())
                    dataStream.Write(contentAsByteArray, 0, contentAsByteArray.Length);
            }

            try
            {
                using (var response = webRequest.GetResponse() as HttpWebResponse)
                using (var responseStream = response?.GetResponseStream())
                {
                    if (responseStream == null)
                    {
                        throw CreateNullResponseException(request, requestParameters);
                    }

                    var result = JsonConverter.DeserializeJson<TResult>(responseStream);

                    return result;
                }
            }
            catch (WebException e)
            {
                OnException(e, request, requestParameters);
                throw;
            }
        }

        private WebException CreateNullResponseException<TRequest>(TRequest request, RequestParameters parameters)
        {
            var exception = new WebException("Null response");
            exception.Data.Add("requestParameters", parameters);

            if (parameters.IsPostRequest)
            {
                var contentAsString = JsonConverter.ToJsonString(request);
                exception.Data.Add("content", contentAsString);
            }

            return exception;
        }

        private void OnException<TRequest>(WebException e, TRequest request, RequestParameters parameters)
        {
            var responseString = GetResponseString(e.Response);

            e.Data.Add("response", responseString);
            e.Data.Add("request_uri", parameters.Url);
            e.Data.Add("http_method", parameters.RequestMethod);
            e.Data.Add("content_type", parameters.ContentType);

            if (parameters.IsPostRequest)
            {
                var requestContent = JsonConverter.ToJsonString(request);
                e.Data.Add("request_body", requestContent);
            }
        }

        private string GetResponseString(WebResponse response)
        {
            if (response == null)
            {
                return "";
            }

            using (var stream = response.GetResponseStream())
            {
                if (stream == null)
                    return "";

                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public RequestParameters GetParameters<TRequest>(TRequest request) where TRequest : IRequest
        {
            var parameters = new RequestParameters
            {
                Url = GetUrl(request),
                ContentType = _configuration.ContentType,
                RequestMethod = _configuration.GetEndPointMethod<TRequest>(),
                RequestTimeout = GetTimeout()
            };

            foreach (var header in _configuration.Headers)
                parameters.Headers.Add(header.Key, header.Value);

            return parameters;
        }

        private string GetUrl<TRequest>(TRequest request) where TRequest : IRequest
        {
            var url = $"{_configuration.ServerAddress}/{request.EndPointPath}";
            return url;
        }

        private int GetTimeout()
        {
            return _configuration.Timeout <= 0 ? 3000 : _configuration.Timeout;
        }
    }
}