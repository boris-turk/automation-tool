using System.Windows.Forms;
using BTurk.Automation.Core.Views;

namespace BTurk.Automation.WinForms;

public class FieldControlProvider : IControlProvider<FieldConfiguration<string>>
{
    public Control CreateInstance(FieldConfiguration<string> configuration)
    {
        var labelTextBox = new LabelTextBoxControl();

        labelTextBox.Label.Text = configuration.LabelText;

        if (configuration.InputStyle == FieldInputStyle.Password)
            labelTextBox.TextBox.PasswordChar = '*';

        return labelTextBox;
    }
}