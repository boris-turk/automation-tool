using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using BTurk.Automation.Core.WinApi;
using EnvDTE;
using Process = System.Diagnostics.Process;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace BTurk.Automation.Standard
{
    public class DTEInstanceProvider
    {
        private const string VisualStudio2019Version = "16.0";
        private const string VisualStudioProcessName = "devenv";

        public static DTEInstance GetActiveInstance()
        {
            var windowHandle = Methods.GetActiveWindow();
            var process = GetVisualStudioProcess(windowHandle);
            var instance = GetDTE(process.Id);

            if (instance == null)
                throw new InvalidOperationException("Cannot obtain Visual Studio handle.");

            return new DTEInstance(instance);
        }

        private static DTE GetDTE(int processId)
        {
            var programId = $"!VisualStudio.DTE.{VisualStudio2019Version}:{processId}";

            object runningObject = null;
            IBindCtx bindContext = null;
            IRunningObjectTable runningObjectTable = null;
            IEnumMoniker enumMonikers = null;

            try
            {
                Marshal.ThrowExceptionForHR(Methods.CreateBindCtx(reserved: 0, out bindContext));

                bindContext.GetRunningObjectTable(out runningObjectTable);
                runningObjectTable.EnumRunning(out enumMonikers);

                IMoniker[] moniker = new IMoniker[1];
                IntPtr numberFetched = IntPtr.Zero;

                while (enumMonikers.Next(1, moniker, numberFetched) == 0)
                {
                    IMoniker runningObjectMoniker = moniker[0];

                    string name = null;

                    try
                    {
                        runningObjectMoniker?.GetDisplayName(bindContext, null, out name);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        // Do nothing, there is something in the ROT that we do not have access to.
                    }

                    if (string.Equals(name, programId, StringComparison.Ordinal))
                    {
                        var errorCode = runningObjectTable.GetObject(runningObjectMoniker, out runningObject);
                        Marshal.ThrowExceptionForHR(errorCode);
                        break;
                    }
                }
            }
            finally
            {
                if (enumMonikers != null)
                    Marshal.ReleaseComObject(enumMonikers);

                if (runningObjectTable != null)
                    Marshal.ReleaseComObject(runningObjectTable);

                if (bindContext != null)
                    Marshal.ReleaseComObject(bindContext);
            }

            return (DTE)runningObject;
        }

        private static Process GetVisualStudioProcess(IntPtr windowHandle)
        {
            var processes = Process.GetProcesses()
                .Where(x => x.ProcessName == VisualStudioProcessName)
                .ToList();

            if (processes.Count == 0)
                throw new InvalidOperationException("No visual studio instance running.");

            if (processes.Count > 1)
            {
                processes = processes
                    .Where(x => x.MainWindowHandle == windowHandle)
                    .ToList();
            }

            var process = processes[0];
            return process;
        }

        public class DTEInstance : IDisposable
        {
            public DTEInstance(DTE instance)
            {
                Instance = instance;
            }

            public DTE Instance { get; }

            public void Dispose()
            {
                Marshal.ReleaseComObject(Instance);
            }
        }
    }
}