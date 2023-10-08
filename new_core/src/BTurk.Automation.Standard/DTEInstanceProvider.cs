using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;
using BTurk.Automation.Core.WinApi;
using EnvDTE;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace BTurk.Automation.Standard;

public class DTEInstanceProvider
{
    public static DTEInstance GetActiveInstance()
    {
        var processId = Methods.GetActiveProcessId();
        var instance = GetDTE(processId);

        if (instance == null)
            throw new InvalidOperationException("Cannot obtain Visual Studio handle.");

        return new DTEInstance(instance);
    }

    private static DTE GetDTE(int processId)
    {
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

                if (name == null)
                    continue;

                if (Regex.IsMatch(name, @$"!VisualStudio.DTE.(\d|.)*:{processId}", RegexOptions.IgnoreCase))
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