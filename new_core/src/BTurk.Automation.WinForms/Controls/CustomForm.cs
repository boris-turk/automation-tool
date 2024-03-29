﻿using System;
using System.Windows.Forms;
using BTurk.Automation.Core.Views;

// ReSharper disable VirtualMemberCallInConstructor

namespace BTurk.Automation.WinForms.Controls;

public class CustomForm : Form, IView
{
    public event EventHandler<KeyEventArgs> CmdKeyPress;

    public ViewConfiguration Configuration { get; set; }

    public bool Confirmed { get; private set; }

    public CustomForm()
    {
        _ = new CloseFormDecorator(this);
        Font = MainForm.PreferredFont;
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

    protected override bool ProcessCmdKey(ref Message m, Keys keyData)
    {
        var keyEventArgs = new KeyEventArgs(keyData);

        CmdKeyPress?.Invoke(this, keyEventArgs);

        if (keyEventArgs.Handled)
            return true;

        return base.ProcessCmdKey(ref m, keyData);
    }

    public void CloseAsConfirmed()
    {
        Confirmed = true;
        Close();
    }
}