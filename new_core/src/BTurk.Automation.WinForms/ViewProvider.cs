using System.Drawing;
using System.Windows.Forms;
using BTurk.Automation.Core.Views;

namespace BTurk.Automation.WinForms;

public class ViewProvider : IViewProvider
{
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
        };

        form.Layout += (_, _) => OnFormLayout(form);

        foreach (var fieldConfiguration in configuration.Fields)
            CreateAndAddControl(form, fieldConfiguration);

        return form;
    }

    private void CreateAndAddControl(CustomForm form, IControlConfiguration configuration)
    {
        var control = _controlProvider.Create(configuration);
        form.Controls.Add(control);
    }

    private void OnFormLayout(CustomForm form)
    {
        const int verticalFormPadding = 5;
        const int horizontalFormPadding = 5;
        const int verticalPaddingBetweenControls = 5;

        var topOffset = verticalFormPadding;
        var leftOffset = horizontalFormPadding;

        var formClientHeight = topOffset;
        var formClientWidth = leftOffset;

        foreach (Control control in form.Controls)
        {
            control.Location = new Point(leftOffset, topOffset);
            topOffset += control.Height + verticalPaddingBetweenControls;
            formClientWidth = control.Right + horizontalFormPadding;
            formClientHeight = control.Bottom + verticalFormPadding;
        }

        if (formClientHeight < 50)
            formClientHeight = 50;

        if (formClientWidth < 100)
            formClientWidth = 100;

        form.ClientSize = new Size(formClientWidth, formClientHeight);
    }
}