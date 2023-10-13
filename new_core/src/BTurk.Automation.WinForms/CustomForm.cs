using System.Windows.Forms;
using BTurk.Automation.Core.Views;

namespace BTurk.Automation.WinForms;

public class CustomForm : Form, IView
{
    public ViewConfiguration Configuration { get; set; }

    public CustomForm()
    {
        _ = new CloseFormDecorator(this);
    }

    public void Execute<TAction>(TAction command) where TAction : IViewAction
    {
        if (command is ShowViewAction)
            Show(Configuration?.ShowAsModalDialog ?? false);
    }

    private void Show(bool showAsModalDialog)
    {
        if (showAsModalDialog)
            ShowDialog();
        else
            Show();
    }
}