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
        AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;

        if (!CreateMutex())
            return;

        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        Environment.CurrentDirectory = GetExecutingAssemblyDirectory();

        StartupProcess startupProcess = null;
        try
        {
            startupProcess = StartupProcess.Instance;
            startupProcess?.Run();
        }
        catch (Exception e)
        {
            ReportError(e);
        }
        finally
        {
            startupProcess?.Dispose();
            AppDomain.CurrentDomain.UnhandledException -= OnAppDomainUnhandledException;
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
        _mutex = new Mutex(true, mutexId, out var result);

        if (result)
            return true;

        _mutex.Close();
        _mutex = null;
        return false;
    }

    private static void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
    {
        ReleaseMutex();

        if (e == null)
        {
            ReportError(null);
            return;
        }

        if (e.ExceptionObject is Exception exception)
            ReportError(exception);
        else
            ReportError(null);
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

    private static void ReportError(Exception exception)
    {
        var message = exception == null
            ? "Unknown error occurred."
            : $"{exception.Message}{Environment.NewLine}{exception.StackTrace}";

        string errorFilePath = Path.Combine(StartupProcess.CurrentAssemblyDirectory, @"ERROR_REPORT.txt");

        File.AppendAllText(errorFilePath, message);
    }
}