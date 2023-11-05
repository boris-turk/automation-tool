using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using BTurk.Automation.WinForms.Controls;

namespace BTurk.Automation.WinForms.Views;

public class FocusCoordinator
{
    private readonly CustomForm _form;
    private IFocusableControl _focusedControl;

    public FocusCoordinator(CustomForm form)
    {
        _form = form;
        _form.Load += OnLoaded;
        _form.CmdKeyPress += OnCmdKeyPress;
    }

    private void OnLoaded(object sender, EventArgs e)
    {
        foreach (var control in _form.GetAllChildControls<IBindableControl>())
            control.Bind(BindingType.Get);

        FocusFirst();
    }

    private void OnCmdKeyPress(object sender, KeyEventArgs e)
    {
        var keyData = e.KeyData & ~(Keys.Shift | Keys.Control | Keys.Alt);

        if (keyData == Keys.Enter)
            FocusNextOrConfirmWindow();
        else if (keyData == Keys.Tab && !e.Shift)
            FocusNext();
        else if (keyData == Keys.Tab && e.Shift)
            FocusPrevious();
    }

    private void FocusFirst()
    {
        _focusedControl = _form
            .GetAllChildControls<IFocusableControl>()
            .Where(c => c.AcceptsFocus)
            .OrderBy(c => c.TabIndex)
            .FirstOrDefault();

        _focusedControl?.Focus();
    }

    private void FocusNext()
    {
        var control = GetFocusableControl(ListSortDirection.Ascending);

        if (control != null)
            FocusControl(control);
    }

    private void FocusPrevious()
    {
        var control = GetFocusableControl(ListSortDirection.Descending);

        if (control != null)
            FocusControl(control);
    }

    private void FocusNextOrConfirmWindow()
    {
        var control = GetFocusableControl(ListSortDirection.Ascending);

        if (control == null)
            _form.CloseAsConfirmed();
        else
            FocusControl(control);
    }

    private IFocusableControl GetFocusableControl(ListSortDirection direction)
    {
        if (_focusedControl == null)
            return null;

        var controls = _form.GetAllChildControls<IFocusableControl>();

        controls = direction == ListSortDirection.Ascending
            ? controls.OrderBy(c => c.TabIndex).SkipWhile(c => c.TabIndex <= _focusedControl.TabIndex)
            : controls.OrderByDescending(c => c.TabIndex).SkipWhile(c => c.TabIndex >= _focusedControl.TabIndex);

        var control = controls.FirstOrDefault();

        return control;
    }

    private void FocusControl(IFocusableControl control)
    {
        OnConfirmingFocusedFieldContents();
        _focusedControl = control;
        _focusedControl.Focus();
    }

    public void OnConfirmingFocusedFieldContents()
    {
        (_focusedControl as IBindableControl)?.Bind(BindingType.Set);
    }

}