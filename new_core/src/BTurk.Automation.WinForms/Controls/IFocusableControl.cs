namespace BTurk.Automation.WinForms.Controls;

public interface IFocusableControl
{
    int TabIndex { get; }
    bool AcceptsFocus { get; }
    void Focus();
}