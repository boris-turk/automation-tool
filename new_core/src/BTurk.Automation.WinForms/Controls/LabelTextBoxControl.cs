using System.Drawing;
using System.Windows.Forms;
using BTurk.Automation.Core;
using BTurk.Automation.WinForms.Views;

namespace BTurk.Automation.WinForms.Controls;

public class LabelTextBoxControl : UserControl, IBindableControl, IFocusableControl
{
    private bool _isPasswordMode;
    public const int DefaultTextBoxWidth = 150;

    private Button _actionButton;

    public Label Label { get; }

    public CustomTextBox TextBox { get; }

    public FieldValueAccessor ValueAccessor { get; set; }

    public LabelTextBoxControl()
    {
        Label = new Label { AutoSize = true };
        TextBox = new CustomTextBox { Width = DefaultTextBoxWidth };

        Controls.Add(Label);
        Controls.Add(TextBox);
    }

    protected override void OnLayout(LayoutEventArgs e)
    {
        const int paddingBetweenLabelAndTextBox = 5;
        const int paddingBetweenTextBoxAndActionButton = 2;

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

        if (_actionButton != null)
        {
            _actionButton.Height = height;
            _actionButton.Left = TextBox.Right + paddingBetweenTextBoxAndActionButton;
            width = _actionButton.Right;
        }

        Size = new Size(width, height);

        Label.Top = (Size.Height - Label.Height) / 2;
        TextBox.Top = (Size.Height - TextBox.Height) / 2;

        base.OnLayout(e);
    }

    bool IFocusableControl.AcceptsFocus => ValueAccessor.CanSetValue();

    public bool IsPasswordMode
    {
        get => _isPasswordMode;
        set
        {
            _isPasswordMode = value;
            OnPasswordModeChanged();
        }
    }

    private void OnPasswordModeChanged()
    {
        AddTogglePasswordVisibilityButtonIfNecessary();

        if (!IsPasswordMode && _actionButton != null)
            _actionButton.Visible = false;
    }

    private void AddTogglePasswordVisibilityButtonIfNecessary()
    {
        if (!IsPasswordMode || _actionButton is TogglePasswordVisibilityButton)
            return;

        _actionButton = new TogglePasswordVisibilityButton
        {
            AssociatedTextBox = TextBox
        };

        Controls.Add(_actionButton);

        PerformLayout();
    }

    void IFocusableControl.Focus()
    {
        TextBox.Focus();
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