using System;
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

    protected override void OnLoad(EventArgs e)
    {
        StartPosition = FormStartPosition.CenterScreen;
        base.OnLoad(e);
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