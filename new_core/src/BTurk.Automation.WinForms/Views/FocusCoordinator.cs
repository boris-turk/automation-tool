using System;
using System.Windows.Forms;
using BTurk.Automation.WinForms.Controls;

namespace BTurk.Automation.WinForms.Views;

public class FocusCoordinator
{
    private readonly CustomForm _form;

    public FocusCoordinator(CustomForm form)
    {
        _form = form;
        _form.Load += OnLoaded;
        _form.KeyDown += OnKeyDown;
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
    }

    private void OnLoaded(object sender, EventArgs e)
    {
    }

    private void OnKeyPress(object sender, KeyPressEventArgs e)
    {
    }
}