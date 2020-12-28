using System.Collections.Generic;
using System.Text.RegularExpressions;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class CommitRequestHandler : NamedCommand
    {
        protected override string CommandName => "commit";

        protected override IEnumerable<Request> CreateRequests()
        {
            yield return new SelectionRequest<Repository>(OnRepositorySelected)
            {
                FilterTextProvider = GetRepositorySearchText
            };
        }

        private string GetRepositorySearchText(string text)
        {
            return Regex.Replace(text, @"^\S*", "").Trim();
        }

        private void OnRepositorySelected(Repository repository)
        {
            repository.Commit();
        }
    }
}
