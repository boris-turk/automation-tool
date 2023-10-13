using System.Threading;
using System.Windows.Forms;
using BTurk.Automation.Core.Views;
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
            if (!AskForCredentials())
                return;

            _globalShortcuts = new GlobalShortcuts();

            _mainForm = Container.GetInstance<MainForm>();
            _mainForm.Load += (_, _) => _globalShortcuts.Install();

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

    private bool AskForCredentials()
    {
        string password = null;

        var viewBuilder = Container.GetInstance<IViewProvider>().Builder();

        viewBuilder.ModalDialogStyle();

        viewBuilder.CancelQuestion("Exit?");

        var field = viewBuilder
            .AddField<string>()
            .PasswordInputStyle()
            .BindSetter(v => password = v);

        var view = viewBuilder.CreateAndShow();

        return password != null;
    }

    public void Unload()
    {
    }
}