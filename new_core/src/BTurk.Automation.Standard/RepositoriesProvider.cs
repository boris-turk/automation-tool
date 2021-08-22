using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Requests;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.Standard
{
    public class RepositoriesProvider : IRequestsProvider<Repository>
    {
        private readonly IResourceProvider _resourceProvider;

        public RepositoriesProvider(IResourceProvider resourceProvider)
        {
            _resourceProvider = resourceProvider;
        }

        private string[] Ignored => new[]
        {
            "V50MicSvn",
            "V50NewCore",
        };

        public IEnumerable<Repository> GetRequests()
        {
            var projectsDirectory = @"C:\work\projects";

            foreach (var directory in Directory.GetDirectories(projectsDirectory))
            {
                var repository = ToRepository(directory);

                if (repository != null)
                    yield return repository;
            }

            foreach (var repository in _resourceProvider.Load<List<Repository>>("repositories"))
                yield return repository;
        }

        private Repository ToRepository(string directory)
        {
            var candidate = Path.GetFileName(directory);

            if (Ignored.Contains(candidate))
                return null;

            if (candidate == "V50")
            {
                return new Repository
                {
                    Text = "trunk",
                    Type = RepositoryType.Git,
                    Path = directory
                };
            }

            var match = Regex.Match(candidate, @"V50Rev0*(\d+)");

            if (match.Success)
            {
                var name = $"r{match.Groups[1].Value}";
                return new Repository
                {
                    Text = name,
                    Type = RepositoryType.Svn,
                    Path = directory
                };
            }

            match = Regex.Match(candidate, @"V50(.*)");

            if (match.Success)
            {
                var name = $"{match.Groups[1].Value.ToLower()}";

                if (name == "mic")
                {
                    return new Repository
                    {
                        Text = name,
                        Type = RepositoryType.Git,
                        Path = directory
                    };
                }

                return new Repository
                {
                    Text = name,
                    Type = RepositoryType.Svn,
                    Path = directory
                };
            }

            return null;
        }
    }
}