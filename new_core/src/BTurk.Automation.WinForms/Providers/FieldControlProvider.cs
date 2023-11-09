using System.Windows.Forms;
using BTurk.Automation.Core.Converters;
using BTurk.Automation.Core.Views;
using BTurk.Automation.WinForms.Controls;
using BTurk.Automation.WinForms.Views;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.WinForms.Providers;

public class FieldControlProvider : IControlProvider<FieldConfiguration<string>>
{
    public FieldControlProvider(IGuiValueConverter converter)
    {
        Converter = converter;
    }

    private IGuiValueConverter Converter { get; }

    public Control CreateInstance(FieldConfiguration<string> configuration)
    {
        var labelTextBox = new LabelTextBoxControl
        {
            ValueAccessor = new FieldValueAccessor(Converter, configuration)
        };

        labelTextBox.Label.Text = configuration.LabelText;

        if (configuration.InputStyle == FieldInputStyle.Password)
            labelTextBox.IsPasswordMode = true;

        return labelTextBox;
    }
}