using System.Collections.Generic;
using System.Text.RegularExpressions;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class SolutionRequestHandler : Command
    {
        protected override string CommandName => "solution";

        protected override IEnumerable<Request> CreateRequests()
        {
            yield return new SelectionRequest<Solution>(OnSolutionSelected)
            {
                FilterTextProvider = GetRepositorySearchText
            };
        }

        private string GetRepositorySearchText(string text)
        {
            return Regex.Replace(text, @"^\S*", "").Trim();
        }

        private void OnSolutionSelected(Solution solution)
        {
            solution.Open();
        }
    }
}