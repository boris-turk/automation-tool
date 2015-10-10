using System.IO;
using System.Threading;

namespace Ahk
{
    public class ReloadGuard
    {
        private static ReloadGuard _guard;

        private readonly MainForm _mainForm;

        public static void Start(MainForm mainForm)
        {
            if (_guard == null)
            {
                _guard = new ReloadGuard(mainForm);
            }
        }

        public ReloadGuard(MainForm mainForm)
        {
            _mainForm = mainForm;

            var watcher = new FileSystemWatcher
            {
                Path = Configuration.RootDirectory,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName
            };

            watcher.Changed += (sender, args) => OnFileSystemChange();
            watcher.Created += (sender, args) => OnFileSystemChange();
            watcher.Deleted += (sender, args) => OnFileSystemChange();
            watcher.Renamed += (sender, args) => OnFileSystemChange();

            watcher.EnableRaisingEvents = true;
        }

        private void OnFileSystemChange()
        {
            Thread.Sleep(1000);
            _mainForm.InvokeCommand(() => _mainForm.LoadMenuEngine());
        }
    }
}