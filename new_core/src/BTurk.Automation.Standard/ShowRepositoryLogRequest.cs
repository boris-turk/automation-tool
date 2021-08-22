using System.Collections.Generic;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class ShowRepositoryLogRequest : Request, ICollectionRequest<Repository>
    {
        public ShowRepositoryLogRequest() : base("log")
        {
        }

        public IEnumerable<Repository> GetRequests(IRequestsProvider<Repository> provider)
        {
            foreach (var request in provider.GetRequests())
            {
                request.Command = new ShowRepositoryLogCommand(request);
                yield return request;
            }
        }
    }
}