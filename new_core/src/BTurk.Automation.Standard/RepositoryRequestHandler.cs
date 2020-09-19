using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Standard
{
    public class RepositoryRequestHandler : IRequestHandler<SelectionRequest<Repository>>
    {
        private readonly ISearchEngine _searchEngine;

        public RepositoryRequestHandler(ISearchEngine searchEngine)
        {
            _searchEngine = searchEngine;
        }

        public void Handle(SelectionRequest<Repository> request)
        {
            _searchEngine.AddItems(GetRepositories());
            _searchEngine.FilterText = request.FilterTextProvider?.Invoke(_searchEngine.SearchText);
        }

        public IEnumerable<Repository> GetRepositories()
        {
            var projectsDirectory = @"C:\work\projects";

            foreach (var directory in Directory.GetDirectories(projectsDirectory))
            {
                var repository = ToRepository(directory);

                if (repository != null)
                    yield return repository;
            }
        }

        private Repository ToRepository(string directory)
        {
            var candidate = Path.GetFileName(directory);

            if (Ignored.Contains(candidate))
                return null;

            if (candidate == "V50")
                return new Repository("trunk", RepositoryType.Git, directory);

            var match = Regex.Match(candidate, @"V50Rev0*(\d+)");

            if (match.Success)
            {
                var name = $"r{match.Groups[1].Value}";
                return new Repository(name, RepositoryType.Svn, directory);
            }

            match = Regex.Match(candidate, @"V50(.*)");

            if (match.Success)
            {
                var name = $"{match.Groups[1].Value.ToLower()}";
                return new Repository(name, RepositoryType.Svn, directory);
            }

            return null;
        }

        private string[] Ignored => new[]
        {
            "V50MicSvn",
            "V50NewCore",
        };
    }
}