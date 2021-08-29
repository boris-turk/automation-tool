using System.Collections.Generic;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class UrlsProvider : IRequestsProvider<UrlRequest>
    {
        private readonly IResourceProvider _resourceProvider;

        public UrlsProvider(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
        }

        public virtual IEnumerable<UrlRequest> GetRequests()
        {
            return _resourceProvider.Load<List<UrlRequest>>("urls");
        }
    }
}