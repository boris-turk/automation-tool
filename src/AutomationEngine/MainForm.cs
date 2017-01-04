using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace AutomationEngine
{
    public partial class MainForm : AutomationEngineForm
    {
        private const int OutOfScreenOffset = -20000;

        public event Action AhkFunctionResultReported;
        public event Action Execute;
        public event Action<ActionType> ShortcutPressed;

        public MainForm()
        {
            InitializeComponent();
            TopMost = true;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(OutOfScreenOffset, OutOfScreenOffset);
            Closing += (sender, args) =>
            {
                args.Cancel = true;
                Visible = false;
            };
            _listBox.SelectedIndexChanged += (o, a) => MoveFocusToTextBox();
        }

        private void MoveFocusToTextBox()
        {
            if (!_textBox.Focused)
            {
                _textBox.Focus();
            }
        }

        protected override void OnExecute()
        {
            Execute?.Invoke();
        }

        public Label StackLabel => _stackLabel;

        public Label StateLabel => _stateLabel;

        public TextBox TextBox => _textBox;

        public ListBox ListBox => _listBox;

        public bool WorkInProgressVisible
        {
            get { return _workInProgressPictureBox.Visible; }
            set
            {
                _workInProgressPictureBox.Visible = value;
                if (value)
                {
                    _workInProgressPictureBox.BringToFront();
                }
                else
                {
                    
                    _workInProgressPictureBox.SendToBack();
                }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            List<AutomationAction> actions = Configuration.Instance.Actions;

            AutomationAction shortcut = actions.FirstOrDefault(x =>
                x.Shortcut.Alt == e.Alt &&
                x.Shortcut.Control == e.Control &&
                (x.Shortcut.Key == e.KeyCode || x.Shortcut.Character == e.KeyCode.ToCharacter()));

            if (shortcut != null)
            {
                ShortcutPressed?.Invoke(shortcut.ActionType);
                e.Handled = true;
            }

            base.OnKeyDown(e);
        }

        protected override void OnShown(EventArgs e)
        {
            Visible = false;
            base.OnShown(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ReloadGuard.Start(this);
            LoadMenuEngine();
        }

        public void LoadMenuEngine()
        {
            var rootMenu = AutomationEngine.Menu.LoadFromFile<Menu>(Configuration.MenusFileName);
            MenuEngine.Start(this, rootMenu);
        }

        public void ToggleVisibility()
        {
            Visible = !Visible;
            if (Visible)
            {
                if (Location.X == OutOfScreenOffset)
                {
                    CenterToScreen();
                }
                MenuEngine.Instance.ResetMenuEngine();
                TopMost = true;
                Activate();
            }
        }

        public void RaiseAhkFunctionResultReportedEvent()
        {
            AhkFunctionResultReported?.Invoke();
        }
    }
}
