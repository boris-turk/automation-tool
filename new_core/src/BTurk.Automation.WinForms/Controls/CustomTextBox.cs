using System;
using System.Windows.Forms;

namespace BTurk.Automation.WinForms.Controls;

public class CustomTextBox : TextBox
{
    public event EventHandler<KeyEventArgs> InterceptKeyPress;

    protected override bool ProcessCmdKey(ref Message m, Keys keyData)
    {
        InterceptKeyPress?.Invoke(this, new KeyEventArgs(keyData));
        return base.ProcessCmdKey(ref m, keyData);
    }
}