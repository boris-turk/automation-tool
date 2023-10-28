using System.Drawing;
using System.Windows.Forms;
using BTurk.Automation.Core;
using BTurk.Automation.WinForms.Views;

namespace BTurk.Automation.WinForms.Controls;

public class LabelTextBoxControl : UserControl, IBindableControl
{
    public const int DefaultTextBoxWidth = 150;

    public Label Label { get; }

    public TextBox TextBox { get; }

    public FieldValueAccessor ValueAccessor { get; set; }

    public LabelTextBoxControl()
    {
        Label = new Label { AutoSize = true };
        TextBox = new TextBox { Width = DefaultTextBoxWidth };

        Controls.Add(Label);
        Controls.Add(TextBox);
    }

    protected override void OnLayout(LayoutEventArgs e)
    {
        const int paddingBetweenLabelAndTextBox = 5;

        if (!Label.Text.HasLength())
        {
            Label.Visible = false;
            TextBox.Location = Point.Empty;
        }
        else
        {
            Label.Visible = true;
            Label.Location = Point.Empty;
            TextBox.Left = Label.Right + paddingBetweenLabelAndTextBox;
        }

        var width = TextBox.Right;
        var height = TextBox.Bottom;

        if (Label.Visible && Label.Bottom > height)
            height = Label.Bottom;

        Size = new Size(width, height);

        Label.Top = (Size.Height - Label.Height) / 2;
        TextBox.Top = (Size.Height - TextBox.Height) / 2;

        base.OnLayout(e);
    }

    void IBindableControl.Bind(BindingType bindingType)
    {
        switch (bindingType)
        {
            case BindingType.Get:
                TextBox.Text = ValueAccessor.GetValue<string>();
                break;
            case BindingType.Set:
                ValueAccessor.SetValue(TextBox.Text);
                break;
        }
    }
}