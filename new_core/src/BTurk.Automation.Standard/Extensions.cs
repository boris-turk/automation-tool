using System.Diagnostics;

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

        public static void Open(this Solution repository)
        {
            Process.Start(repository.AbsolutePath);
        }

        private static void ExecuteCommand(this Repository repository, string command)
        {
            var path = repository.AbsolutePath;
            var programPath = repository.GetProgramPath();
            Process.Start(programPath, $"/command:{command} /path:{path}");
        }
    }
}