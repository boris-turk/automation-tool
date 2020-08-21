using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using BTurk.Automation.Core.AssemblyLoading;

namespace BTurk.Automation.Core
{
    public static class Program
    {
		private static Mutex _mutex;
		private static StartupProcess _startupProcess;

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

			try
            {
                var mainForm = new MainForm();
                mainForm.Load += (_, __) => OnMainFormLoaded();
                Application.Run(mainForm);
			}
			catch (Exception e)
			{
				ReportError(e);
			}
			finally
			{
                _startupProcess.Dispose();
				AppDomain.CurrentDomain.UnhandledException -= OnAppDomainUnhandledException;
				ReleaseMutex();
            }
        }

        private static void OnMainFormLoaded()
        {
            _startupProcess = new StartupProcess();
            _startupProcess.Run();
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
}
