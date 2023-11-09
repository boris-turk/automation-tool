using System.Windows.Forms;

namespace BTurk.Automation.WinForms.Controls;

public class TogglePasswordVisibilityButton : Button
{
    private bool _passwordVisible;
    private CustomTextBox _associatedTextBox;

    public TogglePasswordVisibilityButton()
    {
        ApplyButtonAttributes();
        Click += (_, _) => OnTogglePasswordVisibility();
    }

    public CustomTextBox AssociatedTextBox
    {
        get => _associatedTextBox;
        set
        {
            _associatedTextBox = value;
            ApplyButtonAttributes();
        }
    }

    public bool PasswordVisible
    {
        get => _passwordVisible;
        set
        {
            _passwordVisible = value;
            ApplyButtonAttributes();
        }
    }

    private void ApplyButtonAttributes()
    {
        Text = PasswordVisible ? "\u25ce" : "\u25cf";
        SetupMaskedTextBoxCharacter();
    }

    private void OnTogglePasswordVisibility()
    {
        var activeControl = FindForm()?.ActiveControl as IFocusableControl;
        PasswordVisible = !PasswordVisible;
        SetupMaskedTextBoxCharacter();
        activeControl?.Focus();
    }

    private void SetupMaskedTextBoxCharacter()
    {
        if (AssociatedTextBox != null)
            AssociatedTextBox.PasswordChar = PasswordVisible ? '\0' : '*';
    }

    protected override void OnLayout(LayoutEventArgs levent)
    {
        Width = TextRenderer.MeasureText(Text, Font).Width + 4;
        base.OnLayout(levent);
    }
}