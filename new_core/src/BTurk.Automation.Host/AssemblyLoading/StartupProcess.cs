using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BTurk.Automation.Host.AssemblyLoading;

public class StartupProcess : IDisposable
{
    public const string WaitHandleName = "StartupProcessWaitHandle";

    public static StartupProcess Instance { get; } = new();

    private StartupProcess()
    {
    }

    internal static readonly string CurrentAssemblyDirectory =
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

    private AssemblyManager _assemblyManager;
    private FileSystemWatcher _fileSystemWatcher;
    private bool _reloadAssemblies;

    private readonly EventWaitHandle _waitHandle = new(false, EventResetMode.ManualReset, WaitHandleName);

    public void Run()
    {
        _assemblyManager = new AssemblyManager();
        _assemblyManager.Load();
        _fileSystemWatcher = CreateFileSystemWatcher();

        Loop();
    }

    public void OnGuestProcessFinished()
    {
        var waitHandle = EventWaitHandle.OpenExisting(WaitHandleName);
        waitHandle.Set();
    }

    private void Loop()
    {
        do
        {
            _reloadAssemblies = false;

            _waitHandle.Reset();
            _waitHandle.WaitOne();

            if (_reloadAssemblies)
                _assemblyManager.Load();

        } while (_reloadAssemblies);
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
        if (_reloadAssemblies)
            return;

        _reloadAssemblies = true;

        // wait a bit, maybe file system changes are still being applied
        await Task.Delay(TimeSpan.FromSeconds(2));

        _waitHandle.Set();
    }

    public void Dispose()
    {
        _waitHandle?.Dispose();
        _fileSystemWatcher?.Dispose();
        _assemblyManager?.Dispose();
    }
}