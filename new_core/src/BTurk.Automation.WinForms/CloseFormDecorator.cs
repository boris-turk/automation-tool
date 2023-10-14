using System.Windows.Forms;
using BTurk.Automation.Core;

namespace BTurk.Automation.WinForms;

public class CloseFormDecorator
{
    private readonly CustomForm _form;
    private bool _isClosingConfirmed;

    public CloseFormDecorator(CustomForm form)
    {
        _form = form;
        _form.FormClosing += (_, e) => OnFormClosing(e);
        _form.KeyPress += (_, e) => OnKeyPress(e);
    }

    private string CancelQuestion => _form.Configuration.CancelQuestion;

    protected void OnFormClosing(FormClosingEventArgs e)
    {
        if (e.CloseReason == CloseReason.UserClosing && !_isClosingConfirmed)
        {
            var result = ProcessCancelQuestion();
            e.Cancel = result != DialogResult.Yes;
        }
    }

    private void OnKeyPress(KeyPressEventArgs e)
    {
        if (e.KeyChar != (char) 27)
            return;

        var result = ProcessCancelQuestion();

        if (result != DialogResult.Yes)
            return;

        e.Handled = true;
        _isClosingConfirmed = true;
        _form.Close();
    }

    private DialogResult ProcessCancelQuestion()
    {
        if (!CancelQuestion.HasLength())
            return DialogResult.Yes;

        var result = MessageBox.Show(CancelQuestion, "", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

        return result;
    }
}