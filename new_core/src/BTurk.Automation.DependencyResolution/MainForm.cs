using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BTurk.Automation.Core.Messages;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.DependencyResolution
{
    internal partial class MainForm : Form, ISearchEngine
    {
        private string _currentText;

        private const int OutOfScreenOffset = -20000;

        public MainForm()
        {
            _currentText = "";

            Items = new List<IRequest>();

            InitializeComponent();
            TopMost = false;
            KeyPreview = true;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Location = new Point(OutOfScreenOffset, OutOfScreenOffset);

            Closing += (_, args) =>
            {
                args.Cancel = true;
                Visible = false;
            };

            _listBox.SelectedIndexChanged += (_, _) => MoveFocusToTextBox();

            TextBox.KeyDown += (_, args) => OnTextBoxKeyDown(args);

            CreateInitialStep();
        }

        public List<IRequest> Items { get; }

        public IRequestActionDispatcher Dispatcher { get; set; }

        public EnvironmentContext Context => EnvironmentContextProvider.Context;

        public IEnvironmentContextProvider EnvironmentContextProvider { get; set; }

        public IMessagePublisher MessagePublisher { get; set; }

        private void MoveFocusToTextBox()
        {
            if (!TextBox.Focused)
            {
                TextBox.Focus();
            }
        }

        public Label StackLabel => _stackLabel;

        public Label StateLabel => _stateLabel;

        public TextBox TextBox => _textBox;

        public ListBox ListBox => _listBox;

        public bool WorkInProgressVisible
        {
            get => _workInProgressPictureBox.Visible;
            set
            {
                _workInProgressPictureBox.Visible = value;

                if (value)
                    _workInProgressPictureBox.BringToFront();
                else
                    _workInProgressPictureBox.SendToBack();
            }
        }

        private void CreateInitialStep()
        {
            Steps = new List<SearchStep> { new(new RootMenuRequest()) };
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            var handled = ProcessKeyDown(e);

            if (handled)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
            }

            base.OnKeyDown(e);
        }

        private bool ProcessKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    Hide();
                    return true;

                case Keys.Enter when !e.Alt && !e.Control && !e.Shift:
                    TriggerAction(ActionType.Execute);
                    return true;

                default:
                    return false;
            }
        }

        private void OnTextBoxKeyDown(KeyEventArgs args)
        {
            var handled = false;

            if (args.KeyCode == Keys.Down)
            {
                SelectItem(ListBox.SelectedIndex + 1);
                handled = true;
            }

            if (args.KeyCode == Keys.Up)
            {
                SelectItem(ListBox.SelectedIndex - 1);
                handled = true;
            }

            if (args.KeyData == (Keys.Control | Keys.Back))
            {
                SendKeys.SendWait("^+{LEFT}{BACKSPACE}");
                handled = true;
            }

            if (handled)
            {
                args.SuppressKeyPress = true;
                args.Handled = true;
            }
        }

        protected override void OnShown(EventArgs e)
        {
            if (Debugger.IsAttached)
                CenterToScreen();
            else
                Visible = false;

            base.OnShown(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
                TriggerAction(ActionType.MoveNext);
            else
                TextBox.Text = "";
        }

        protected override void OnLoad(EventArgs e)
        {
            TextBox.TextChanged += OnSearchTextChanged;
            MessagePublisher.Publish(ShowingAutomationWindowMessage.MainMenu);
            base.OnLoad(e);
        }

        private void OnSearchTextChanged(object sender, EventArgs e)
        {
            if (!Visible)
            {
                _currentText = TextBox.Text;
                return;
            }

            var actionType = _currentText.Length > TextBox.Text.Length
                ? ActionType.MovePrevious
                : ActionType.MoveNext;

            _currentText = TextBox.Text;

            TriggerAction(actionType);
        }

        public void TriggerAction(ActionType actionType)
        {
            ActionType = actionType;

            if (actionType == ActionType.MoveNext && SearchText.Length > 0)
                Steps.Last().Text += SearchText.Last();

            if (actionType == ActionType.MoveNext && !Items.Any() && SearchText.Trim().Length > 0)
                return;

            Dispatcher.Dispatch(Steps.Last().Request, actionType);

            var selectedIndex = ListBox.SelectedIndex;

            ListBox.BeginUpdate();

            ListBox.Items.Clear();

            foreach (var item in Items)
                ListBox.Items.Add(item);

            ListBox.EndUpdate();

            if (actionType != ActionType.Execute)
                selectedIndex = 0;

            SelectItem(selectedIndex);
        }

        private void SelectItem(int itemIndex)
        {
            if (itemIndex < 0)
                return;

            if (ListBox.Items.Count > itemIndex)
                ListBox.SelectedIndex = itemIndex;
        }

        public void ToggleVisibility()
        {
            Visible = !Visible;

            if (!Visible)
                return;

            if (Location.X == OutOfScreenOffset)
                CenterToScreen();

            TopMost = true;
            Activate();
        }

        public List<SearchStep> Steps { get; private set; }

        public string SearchText
        {
            get => TextBox.Text;
            set => TextBox.Text = value;
        }

        public Request SelectedItem => (Request)ListBox.SelectedItem;

        public ActionType ActionType { get; private set; }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
                OnGlobalShortcutKeyPressed(m.WParam.ToInt32());

            base.WndProc(ref m);
        }

        private void OnGlobalShortcutKeyPressed(int shortcutId)
        {
            if (!Visible)
                CreateInitialStep();

            var message = shortcutId == GlobalShortcuts.OpenMainWindowShortcutId
                ? ShowingAutomationWindowMessage.MainMenu
                : ShowingAutomationWindowMessage.ApplicationMenu;

            MessagePublisher.Publish(message);

            ToggleVisibility();
        }
    }
}
