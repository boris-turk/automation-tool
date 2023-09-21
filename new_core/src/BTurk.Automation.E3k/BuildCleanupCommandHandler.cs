using System.Collections.Generic;
using System.IO;
using BTurk.Automation.Core;
using BTurk.Automation.Core.AsyncServices;
using BTurk.Automation.Core.Commands;
using BTurk.Automation.Core.Helpers;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.E3k
{
    public class BuildCleanupCommandHandler : ICommandHandler<BuildCleanupCommand>
    {
        private string _rootPath;

        public BuildCleanupCommandHandler(IProcessStarter processStarter, IAsyncExecution asyncExecution)
        {
            ProcessStarter = processStarter;
            AsyncExecution = asyncExecution;
        }

        public IProcessStarter ProcessStarter { get; }

        public IAsyncExecution AsyncExecution { get; }

        public void Handle(BuildCleanupCommand command)
        {
            _rootPath = command.RootPath;

            foreach (var directory in GetDirectories(command.RootPath))
            {
                DeleteDirectoryContents(directory);

                if (AsyncExecution.IsCanceled)
                    return;
            }
        }

        private void DeleteDirectoryContents(string directory)
        {
            var directoryInfo = new DirectoryInfo(directory);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();

                if (AsyncExecution.IsCanceled)
                    return;
            }

            foreach (DirectoryInfo subDirectory in directoryInfo.GetDirectories())
            {
                subDirectory.Delete(recursive: true);

                var relativeSubDirectoryPath = GetRelativeSubDirectoryPath(subDirectory);
                AsyncExecution.SetProgressText($"Deleting directory {relativeSubDirectoryPath}");

                if (AsyncExecution.IsCanceled)
                    return;
            }
        }

        private string GetRelativeSubDirectoryPath(DirectoryInfo subDirectory)
        {
            var relativeSubDirectory = subDirectory.FullName.Substring(_rootPath.Length);
            relativeSubDirectory = relativeSubDirectory.TrimEnd(Path.DirectorySeparatorChar);
            return relativeSubDirectory;
        }

        private IEnumerable<string> GetDirectories(string rootPath)
        {
            yield return Path.Combine(rootPath, "bin");

            var srcDirectory = Path.Combine(rootPath, "src");

            var iterator = new DirectoryIterator(srcDirectory)
            {
                CanVisit = d => !d.EndsWith(@"src\packages") && !d.IsHiddenDirectory()
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