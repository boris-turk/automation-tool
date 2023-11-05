using System;
using System.Drawing;
using System.Windows.Forms;
using BTurk.Automation.Core;
using BTurk.Automation.Core.Views;
using BTurk.Automation.WinForms.Controls;
using BTurk.Automation.WinForms.Views;

namespace BTurk.Automation.WinForms.Providers;

public class ViewProvider : IViewProvider
{
    private const int VerticalPaddingBetweenControls = 5;

    private readonly IControlProvider _controlProvider;

    public ViewProvider(IControlProvider controlProvider)
    {
        _controlProvider = controlProvider;
    }

    public IView Create(ViewConfiguration configuration)
    {
        var form = new CustomForm
        {
            KeyPreview = true,
            Configuration = configuration,
            Padding = new Padding
            {
                Left = 10,
                Right = 10,
                Top = 5,
                Bottom = 15
            }
        };

        _ = new FocusCoordinator(form);

        form.Layout += (_, _) => OnFormLayout(form);

        foreach (var fieldConfiguration in configuration.Fields)
            CreateAndAddControl(form, fieldConfiguration);

        return form;
    }

    private void CreateAndAddControl(CustomForm form, IControlConfiguration configuration)
    {
        var control = _controlProvider.Create(configuration);
        OnControlCreated(control, form);
        form.Controls.Add(control);
    }

    private void OnControlCreated(Control control, CustomForm form)
    {
        if (control is IFocusableControl)
            control.TabIndex = form.GetAllChildControls<IFocusableControl>().MaxOrDefault(c => c.TabIndex) + 1;
    }

    private void OnFormLayout(CustomForm form)
    {
        var topOffset = form.Padding.Top;
        var leftOffset = form.Padding.Left;

        var formClientHeight = topOffset;
        var formClientWidth = leftOffset;

        foreach (Control control in form.Controls)
        {
            control.Location = new Point(leftOffset, topOffset);
            topOffset += control.Height + VerticalPaddingBetweenControls;
            formClientWidth = Math.Max(formClientWidth, control.Right + form.Padding.Right);
            formClientHeight = Math.Max(formClientHeight, control.Bottom + form.Padding.Bottom);
        }

        if (formClientHeight < 50)
            formClientHeight = 50;

        if (formClientWidth < 100)
            formClientWidth = 100;

        form.ClientSize = new Size(formClientWidth, formClientHeight);
    }
}