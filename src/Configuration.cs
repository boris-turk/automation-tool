using System.IO;

namespace Ahk
{
    public static class Configuration
    {
        public static string RootDirectory = @"C:\Users\Boris\Dropbox\Automation";

        public static string FilesPath => Path.Combine(RootDirectory, "files.txt");

        public static string UrlsPath => Path.Combine(RootDirectory, "urls.txt");

        public static string SolutionsPath => Path.Combine(RootDirectory, "solutions.txt");

        public static string RepositoriesPath => Path.Combine(RootDirectory, "repositories.txt");

        public static string ProgramsPath => Path.Combine(RootDirectory, "programs.txt");
    }
}