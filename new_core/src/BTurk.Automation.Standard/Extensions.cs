using System.Diagnostics;
using BTurk.Automation.Core.Requests;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.Standard
{
    public static class Extensions
    {
        public static void Log(this Repository repository)
        {
            repository.ExecuteCommand("log");
        }

        public static void Commit(this Repository repository)
        {
            repository.ExecuteCommand("commit");
        }

        public static void Open(this IFileRequest request)
        {
            Process.Start(request.Path);
        }

        private static void ExecuteCommand(this Repository repository, string command)
        {
            var path = repository.Path;
            var programPath = repository.GetProgramPath();
            Process.Start(programPath, $"/command:{command} /path:{path}");
        }

        private static string GetProgramPath(this Repository repository)
        {
            return repository.Type == RepositoryType.Git
                ? @"C:\Program Files\TortoiseGit\bin\TortoiseGitProc.exe"
                : @"c:\Program Files\TortoiseSVN\bin\TortoiseProc.exe";
        }
    }
}