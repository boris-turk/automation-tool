using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
