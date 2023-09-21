using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class UrlCollectionRequest : CollectionRequest<UrlRequest>
    {
        public UrlCollectionRequest() : base("url")
        {
        }

        protected override void OnRequestLoaded(UrlRequest urlRequest)
        {
            urlRequest.Command = new OpenWithDefaultProgramCommand(urlRequest);
        }
    }
}