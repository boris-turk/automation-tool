using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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

            Items = new List<Request>();

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

            _listBox.SelectedIndexChanged += (_, __) => MoveFocusToTextBox();

            TextBox.KeyDown += (_, args) => OnTextBoxKeyDown(args);

            VisibleChanged += (_, __) => { if (Visible) OnBecomingVisible(); };
        }

        public List<Request> Items { get; }

        public RequestDispatcher RequestDispatcher { get; set; }

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

            OnBecomingVisible();

            base.OnShown(e);
        }

        private void OnBecomingVisible()
        {
            MessagePublisher.Publish(ShowingAutomationWindowMessage.MainMenu);

            if (string.IsNullOrWhiteSpace(TextBox.Text))
                TriggerAction(ActionType.MoveNext);
            else
                TextBox.Text = "";
        }

        protected override void OnLoad(EventArgs e)
        {
            TextBox.TextChanged += OnSearchTextChanged;
            base.OnLoad(e);
        }

        private void OnSearchTextChanged(object sender, EventArgs e)
        {
            var actionType = _currentText.Length > TextBox.Text.Length
                ? ActionType.MovePrevious
                : ActionType.MoveNext;

            _currentText = TextBox.Text;

            TriggerAction(actionType);
        }

        public void TriggerAction(ActionType actionType)
        {
            ActionType = actionType;

            RequestDispatcher.Dispatch();

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

        public string SearchText
        {
            get => TextBox.Text;
            set => TextBox.Text = value;
        }

        public Selection TextSelection
        {
            get => new Selection
            {
                Start = TextBox.SelectionStart,
                Length = TextBox.SelectionLength
            };
            set
            {
                TextBox.SelectionStart = value.Start;
                TextBox.SelectionLength = value.Length;
            }
        }

        public Request SelectedItem => (Request)ListBox.SelectedItem;

        public ActionType ActionType { get; private set; }

        public void AddItems(IEnumerable<Request> items)
        {
            Items.AddRange(items);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
                OnGlobalShortcutKeyPressed(m.WParam.ToInt32());

            base.WndProc(ref m);
        }

        private void OnGlobalShortcutKeyPressed(int shortcutId)
        {
            if (!Visible)
                RequestDispatcher.Reset();

            var message = shortcutId == GlobalShortcuts.OpenMainWindowShortcutId
                ? ShowingAutomationWindowMessage.MainMenu
                : ShowingAutomationWindowMessage.ApplicationMenu;

            MessagePublisher.Publish(message);

            ToggleVisibility();
        }
    }
}
