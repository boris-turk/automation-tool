using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class UrlCollectionRequest : Request, ICollectionRequest<UrlRequest>
    {
        public UrlCollectionRequest() : base("url")
        {
        }

        void ICollectionRequest<UrlRequest>.OnLoaded(UrlRequest urlRequest)
        {
            urlRequest.Command = new OpenWithDefaultProgramCommand(urlRequest);
        }
    }
}