using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BTurk.Automation.Core.AssemblyLoading
{
    public class StartupProcess : IDisposable
    {
		internal static readonly string CurrentAssemblyDirectory =
	        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private readonly object _lockObject = new object();
		private AssemblyManager _assemblyManager;
        private FileSystemWatcher _fileSystemWatcher;
        private bool _assembliesBeingLoaded;

        public void Run()
		{
			_assemblyManager = new AssemblyManager();
            LoadAssemblies();
            _fileSystemWatcher = CreateFileSystemWatcher();
		}

        private static string[] GetAssemblyPaths()
        {
            return Directory.GetFiles(CurrentAssemblyDirectory, "*.dll");
        }

        private void LoadAssemblies()
        {
            var paths = GetAssemblyPaths();
            _assemblyManager.LoadFrom(paths);
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

            if (_assembliesBeingLoaded)
            {
                Monitor.Exit(_lockObject);
                return;
            }

            _assembliesBeingLoaded = true;

            Monitor.Exit(_lockObject);

            // wait a bit, maybe file system changes are still being applied
            await Task.Delay(TimeSpan.FromSeconds(2));

            Monitor.Enter(_lockObject);

            try
            {
                _assembliesBeingLoaded = true;
                LoadAssemblies();
            }
            finally
            {
                _assembliesBeingLoaded = false;
                Monitor.Exit(_lockObject);
            }
        }

        public void Dispose() => _fileSystemWatcher?.Dispose();
    }
}