using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace AutomationEngine
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            int currentProcessPid = Process.GetCurrentProcess().Id;

            Process.GetProcessesByName("AutomationEngine")
                .Where(x => x.Id != currentProcessPid)
                .ToList()
                .ForEach(x => x.Kill());

            SetCurrentDirectory();

            Application.ThreadException += OnApplicationThreadException;
            AppDomain.CurrentDomain.UnhandledException += OnAppDomainUnhandledException;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(FormFactory.Instance<MainForm>());
        }

        private static void SetCurrentDirectory()
        {
            if (Debugger.IsAttached)
            {
                Environment.CurrentDirectory = @"C:\Users\Boris\Dropbox\Automation";
                return;
            }

            string fileLocation = Assembly.GetExecutingAssembly().Location;
            string directory = Path.GetDirectoryName(fileLocation);

            if (directory == null)
            {
                MessageBox.Show("Unable to determine root directory");
                return;
            }

            Environment.CurrentDirectory = directory;
        }

        private static void OnAppDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e == null)
            {
                MessageBox.Show("Unhandled AppDomain exception ");
                return;
            }

            var exception = e.ExceptionObject as Exception;
            if (exception != null)
            {
                MessageBox.Show(exception.ToString());
            }
            else
            {
                MessageBox.Show("Unhandled AppDomain exception ");
            }
        }

        private static void OnApplicationThreadException(object sender, ThreadExceptionEventArgs exception)
        {
            if (exception == null || exception.Exception == null)
            {
                MessageBox.Show("Unhandled application thread exception ");
            }
            else
            {
                MessageBox.Show(exception.Exception.ToString());
            }
        }
    }
}
