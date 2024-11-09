using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using BTurk.Automation.Host.AssemblyLoading;

namespace BTurk.Automation.Host;

public static class Program
{
    private static Mutex _mutex;

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        if (!CreateMutex())
            return;

        StartupProcess startupProcess = null;

        try
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Environment.CurrentDirectory = GetExecutingAssemblyDirectory();

            startupProcess = StartupProcess.Instance;
            startupProcess?.Run();
        }
        catch (Exception e)
        {
            var errorMessage = StartupProcess.GetErrorMessage(e);
            StartupProcess.LogErrorMessage(errorMessage);
        }
        finally
        {
            startupProcess?.Dispose();
            ReleaseMutex();
        }
    }

    private static string GetExecutingAssemblyDirectory()
    {
        var fileLocation = Assembly.GetExecutingAssembly().Location;
        return Path.GetDirectoryName(fileLocation) ?? Environment.CurrentDirectory;
    }

    private static bool CreateMutex()
    {
        var mutexId = GetMutexId(Environment.CurrentDirectory);
        _mutex = new Mutex(true, mutexId, out var acquiredOwnership);

        if (acquiredOwnership)
            return true;

        _mutex.Close();
        _mutex = null;

        return false;
    }

    private static string GetMutexId(string directory)
    {
        string id = directory.Replace(":", "_").Replace(@"\", "_");
        return $"MicServiceMutex_{id}";
    }

    private static void ReleaseMutex()
    {
        try
        {
            _mutex?.ReleaseMutex();
            _mutex?.Close();
            _mutex = null;
        }
        catch (Exception)
        {
            // ignore possible exceptions
        }
    }
}