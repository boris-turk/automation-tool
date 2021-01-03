using System.Collections.Generic;

namespace AutomationEngine.RestApi
{
    public class RequestParameters
    {
        public RequestParameters()
        {
            Headers = new Dictionary<string, string>();
        }

        public string Url { get; set; }

        public string ContentType { get; set; }

        public int RequestTimeout { get; set; }

        public string RequestMethod { get; set; }

        public Dictionary<string, string> Headers { get; }

        public bool IsPostRequest => RequestMethod == EndpointConfiguration.PostMethod;

        public override string ToString()
        {
            return $"Ulr = {Url}, " +
                   $"RequestMethod = {RequestMethod}, " +
                   $"ContentType = {ContentType}, " +
                   $"RequestTimeout = {RequestTimeout}";
        }
    }
}