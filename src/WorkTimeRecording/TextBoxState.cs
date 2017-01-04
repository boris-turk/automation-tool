using System.Windows.Forms;

namespace WorkTimeRecording
{
    public class TextBoxState
    {
        private readonly TextBox _textBox;

        private string _text;
        private int _selectionStart;
        private int _selectionLength;

        public TextBoxState(TextBox textBox)
        {
            _textBox = textBox;
        }

        public void Save()
        {
            _text = _textBox.Text;
            _selectionStart = _textBox.SelectionStart;
            _selectionLength = _textBox.SelectionLength;
        }

        public void Restore()
        {
            _textBox.Text = _text;
            _textBox.SelectionStart = _selectionStart;
            _textBox.SelectionLength = _selectionLength;
        }
    }
}