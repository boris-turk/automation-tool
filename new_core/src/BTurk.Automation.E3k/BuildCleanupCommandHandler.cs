using System.Collections.Generic;
using System.IO;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.Helpers;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.E3k
{
    public class BuildCleanupCommandHandler : ICommandHandler<BuildCleanupCommand>
    {
        public BuildCleanupCommandHandler(IProcessStarter processStarter)
        {
            ProcessStarter = processStarter;
        }

        public IProcessStarter ProcessStarter { get; }

        public void Handle(BuildCleanupCommand command)
        {
            foreach (var directory in GetDirectories(command.RootPath))
                DeleteDirectoryContents(directory);
        }

        private void DeleteDirectoryContents(string directory)
        {
            var directoryInfo = new DirectoryInfo(directory);

            foreach (FileInfo file in directoryInfo.GetFiles())
                file.Delete();

            foreach (DirectoryInfo subDirectory in directoryInfo.GetDirectories())
                subDirectory.Delete(true);
        }

        private IEnumerable<string> GetDirectories(string rootPath)
        {
            yield return Path.Combine(rootPath, "bin");

            var srcDirectory = Path.Combine(rootPath, "src");

            var iterator = new DirectoryIterator(srcDirectory)
            {
                CanVisit = _ => !_.EndsWith(@"src\packages") && !_.IsHiddenDirectory()
            };

            foreach (var directoryScope in iterator)
            {
                if (IsBuildOutputDirectory(directoryScope))
                    yield return directoryScope.Directory;
            }
        }

        private bool IsBuildOutputDirectory(DirectoryScope directoryScope)
        {
            return directoryScope.EndsWith(@"\bin") || directoryScope.EndsWith(@"\obj");
        }
    }
}