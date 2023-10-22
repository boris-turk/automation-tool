using System.Windows.Forms;
using BTurk.Automation.Core.Views;
using BTurk.Automation.Host.AssemblyLoading;
using BTurk.Automation.WinForms;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.DependencyResolution;

public class GuestProcess : IGuestProcess
{
    private MainForm _mainForm;
    private GlobalShortcuts _globalShortcuts;

    private Form ActiveForm => _mainForm ?? Form.ActiveForm;

    public void Start()
    {
        try
        {
            if (!AskForCredentials())
                return;

            _globalShortcuts = Container.GetInstance<GlobalShortcuts>();
            _mainForm = Container.GetInstance<MainForm>();
            _mainForm.Load += (_, _) => _globalShortcuts.Install();

            Application.Run(_mainForm);
        }
        finally
        {
            _globalShortcuts?.Uninstall();
            _globalShortcuts = null;
        }
    }

    private bool AskForCredentials()
    {
        string password = null;

        var viewBuilder = Container.GetInstance<IViewProvider>().Builder();

        viewBuilder.ModalDialogStyle();

        viewBuilder.CancelQuestion("Exit?");

        viewBuilder
            .AddField<string>()
            .LabelText("Master password:")
            .PasswordInputStyle()
            .BindSetter(v => password = v);

        viewBuilder.CreateAndShow();

        return password != null;
    }

    public void Dispose()
    {
        ActiveForm?.Invoke(DisposeResources);
    }

    private void DisposeResources()
    {
        _globalShortcuts?.Uninstall();
        ActiveForm?.Dispose();
    }
}