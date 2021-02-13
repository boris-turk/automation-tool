using System.Diagnostics;
using BTurk.Automation.Core.Requests;

namespace BTurk.Automation.Standard
{
    public static class Extensions
    {
        public static string GetProgramPath(this Repository repository)
        {
            if (repository.Type == RepositoryType.Git)
                return @"C:\Program Files\TortoiseGit\bin\TortoiseGitProc.exe";

            return @"c:\Program Files\TortoiseSVN\bin\TortoiseProc.exe";
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
    }
}