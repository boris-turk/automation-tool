using System.Text.RegularExpressions;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class CommitRequestHandler : RootRequestHandler
    {
        public CommitRequestHandler(IRequestHandler<CompositeRequest> requestHandler) :
            base(requestHandler)
        {
            var repositoryRequest = new SelectionRequest<Repository>(OnRepositorySelected)
            {
                FilterProvider = GetRepositorySearchText
            };

            AddRequest(repositoryRequest);
        }

        protected override string CommandName => "commit";

        private string GetRepositorySearchText(string text)
        {
            return Regex.Replace(text, @"^\S*", "").Trim();
        }

        private void OnRepositorySelected(Repository repository)
        {
        }
    }
}
