using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using AutomationEngine.Messages;

namespace AutomationEngine
{
    public partial class MainForm : AutomationEngineForm
    {
        private const int OutOfScreenOffset = -20000;
        public event Action AhkFunctionResultReported;
        public event Action Execute;

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
            if (Execute != null)
            {
                Execute();
            }
        }

        public Label StackLabel
        {
            get { return _stackLabel; }
        }

        public TextBox TextBox
        {
            get { return _textBox; }
        }

        public ListBox ListBox
        {
            get { return _listBox; }
        }

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
            Menu rootMenu = AutomationEngine.Menu.LoadFromFile(Configuration.MenusFileName);
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
                MenuEngine.Instance.ClearSearchBar();
                TopMost = true;
                Activate();
            }
        }

        public void RaiseAhkFunctionResultReportedEvent()
        {
            if (AhkFunctionResultReported != null)
            {
                AhkFunctionResultReported();
            }
        }
    }
}
