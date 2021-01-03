using System;
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

        public EndPointType RequestType { get; set; }

        public string ContentType { get; set; }

        public int RequestTimeout { get; set; }

        public string RequestMethod
        {
            get
            {
                switch (RequestType)
                {
                    case EndPointType.Get:
                        return "GET";
                    case EndPointType.Post:
                        return "POST";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public Dictionary<string, string> Headers { get; }

        public override string ToString()
        {
            return $"Ulr = {Url}, " +
                   $"RequestMethod = {RequestType}, " +
                   $"ContentType = {ContentType}, " +
                   $"RequestTimeout = {RequestTimeout}";
        }
    }
}