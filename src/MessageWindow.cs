using System.Windows.Forms;

namespace AutomationEngine
{
    public class MessageWindow
    {
        public string Message { get; set; }

        public string Caption { get; set; }

        public MessageBoxButtons Type { get; set; }

        public DialogResult Result { get; private set; }

        public void Show()
        {
            Result = MessageBox.Show(Message, Caption, Type, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
        }
    }
}