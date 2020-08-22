using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BTurk.Automation.Host.AssemblyLoading
{
    public class StartupProcess : IDisposable
    {
		internal static readonly string CurrentAssemblyDirectory =
	        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private readonly object _lockObject = new object();
		private AssemblyManager _assemblyManager;
        private FileSystemWatcher _fileSystemWatcher;
        private bool _reloadAssemblies;
        private readonly bool _exit = false;

        public void Run()
		{
			_assemblyManager = new AssemblyManager();
            _assemblyManager.Load();
            _fileSystemWatcher = CreateFileSystemWatcher();

            Loop();
        }

        private void Loop()
        {
            do
            {
                Monitor.Enter(_lockObject);
                Monitor.Wait(_lockObject);

                if (_reloadAssemblies)
                {
                    _assemblyManager.Load();
                    _reloadAssemblies = false;
                }
            } while (!_exit);
            Monitor.Exit(_lockObject);
        }

        private FileSystemWatcher CreateFileSystemWatcher()
        {
            var fileSystemWatcher = new FileSystemWatcher
            {
                Filter = "*.dll",
                Path = CurrentAssemblyDirectory
            };

            fileSystemWatcher.Changed += FileSystemWatcherChanged;
            fileSystemWatcher.Created += FileSystemWatcherChanged;
            fileSystemWatcher.Deleted += FileSystemWatcherChanged;

            fileSystemWatcher.EnableRaisingEvents = true;

            return fileSystemWatcher;
        }

        private async void FileSystemWatcherChanged(object sender, FileSystemEventArgs e)
        {
            Monitor.Enter(_lockObject);

            if (_reloadAssemblies)
            {
                Monitor.Exit(_lockObject);
                return;
            }

            _reloadAssemblies = true;

            Monitor.Exit(_lockObject);

            // wait a bit, maybe file system changes are still being applied
            await Task.Delay(TimeSpan.FromSeconds(2));

            Monitor.Enter(_lockObject);
            Monitor.Pulse(_lockObject);
            Monitor.Exit(_lockObject);
        }

        public void Dispose()
        {
            _assemblyManager?.Unload();
            _fileSystemWatcher?.Dispose();
        }
    }
}
