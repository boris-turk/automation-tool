using System;
using System.IO;
using System.Threading;

namespace AutomationEngine
{
    public class ReloadGuard
    {
        private static ReloadGuard _guard;

        private readonly MainForm _mainForm;
        private bool _enabled;

        public static bool Enabled
        {
            get { return _guard._enabled; }
            set { _guard._enabled = value; }
        }

        public static void Start(MainForm mainForm)
        {
            if (_guard == null)
            {
                _guard = new ReloadGuard(mainForm);
            }
        }

        public ReloadGuard(MainForm mainForm)
        {
            _enabled = true;
            _mainForm = mainForm;

            var watcher = new FileSystemWatcher
            {
                Path = ".",
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
            if (!_enabled)
            {
                return;
            }
            _enabled = false;
            ThreadPool.QueueUserWorkItem(x => ReloadMenuEngine());
        }

        private void ReloadMenuEngine()
        {
            Thread.Sleep(500);
            _mainForm.InvokeCommand(() => _mainForm.LoadMenuEngine());
            _enabled = true;
        }
    }
}
