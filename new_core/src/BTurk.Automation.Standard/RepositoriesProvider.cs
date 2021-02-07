using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public class RepositoriesProvider : RequestsProvider<Repository>
    {
        private string[] Ignored => new[]
        {
            "V50MicSvn",
            "V50NewCore",
        };

        protected override IEnumerable<Repository> Load()
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
            {
                return new Repository("trunk")
                {
                    Type = RepositoryType.Git,
                    AbsolutePath = directory
                };
            }

            var match = Regex.Match(candidate, @"V50Rev0*(\d+)");

            if (match.Success)
            {
                var name = $"r{match.Groups[1].Value}";
                return new Repository(name)
                {
                    Type = RepositoryType.Svn,
                    AbsolutePath = directory
                };
            }

            match = Regex.Match(candidate, @"V50(.*)");

            if (match.Success)
            {
                var name = $"{match.Groups[1].Value.ToLower()}";

                if (name == "mic")
                {
                    return new Repository(name)
                    {
                        Type = RepositoryType.Git,
                        AbsolutePath = directory
                    };
                }

                return new Repository(name)
                {
                    Type = RepositoryType.Svn,
                    AbsolutePath = directory
                };
            }

            return null;
        }

    }
}