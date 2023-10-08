using System.Threading;
using System.Windows.Forms;
using BTurk.Automation.Host.AssemblyLoading;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.DependencyResolution;

public class GuestProcess : IGuestProcess
{
    private MainForm _mainForm;
    private GlobalShortcuts _globalShortcuts;

    public void Start()
    {
        try
        {
            _globalShortcuts = new GlobalShortcuts();

            _mainForm = Container.GetInstance<MainForm>();
            _mainForm.Load += (_, __) => _globalShortcuts.Install();

            Application.Run(_mainForm);
        }
        catch (ThreadAbortException)
        {
            // most probably the domain is being unloaded, it is safe to ignore this exception
        }
        finally
        {
            _globalShortcuts.Uninstall();
        }
    }

    public void Unload()
    {
    }
}