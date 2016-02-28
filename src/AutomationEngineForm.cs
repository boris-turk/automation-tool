using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AutomationEngine
{
    public class AutomationEngineForm : Form
    {
        private const string AutomationEngineWindowTitle = "Automation engine";

        public AutomationEngineForm()
        {
            StartPosition = FormStartPosition.CenterScreen;
            Load += (sender, args) => Initialize();
        }

        protected virtual string WindowName
        {
            get { return string.Empty; }
        }

        protected List<Control> ChildControls
        {
            get { return Controls.OfType<Control>().ToList(); }
        }

        protected List<Control> FocusableControls
        {
            get
            {
                return ChildControls
                    .Where(x => x.TabStop)
                    .OrderBy(x => x.TabIndex)
                    .ToList();
            }
        }

        public Control FocusedControl
        {
            get { return ChildControls.FirstOrDefault(x => x.Focused); }
        }

        public bool IsMainWindow
        {
            get { return GetType() == typeof(MainForm); }
        }

        private void Initialize()
        {
            SetWindowTitle();
            ChildControls.ForEach(x => x.KeyDown += (s, a) => OnControlKeyDown(a));
        }

        private void SetWindowTitle()
        {
            if (string.IsNullOrWhiteSpace(WindowName))
            {
                Text = AutomationEngineWindowTitle;
            }
            else
            {
                Text = AutomationEngineWindowTitle + " - " + WindowName;
            }
        }

        private void OnControlKeyDown(KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.KeyCode == Keys.Escape)
            {
                OnCloseRequested();
            }
            if (keyEventArgs.KeyCode == Keys.Enter)
            {
                OnEnterKeyPressed();
            }
        }

        private void OnEnterKeyPressed()
        {
            if (FocusedControl == FocusableControls.Last())
            {
                OnExecute();
            }
        }

        protected virtual void OnExecute()
        {
        }

        private void OnCloseRequested()
        {
            if (IsMainWindow)
            {
                Hide();
            }
            else
            {
                Close();
            }
        }
    }
}