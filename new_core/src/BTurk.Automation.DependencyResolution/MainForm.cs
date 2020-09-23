using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using BTurk.Automation.Core.Requests;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.DependencyResolution
{
    internal partial class MainForm : Form, ISearchEngine, ISearchItemsProvider
    {
        private const int OutOfScreenOffset = -20000;

        public MainForm()
        {
            Items = new List<SearchItem>();

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
        }

        public List<SearchItem> Items { get; }

        public RootRequestHandler RootRequestHandler { get; set; }

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
            if (e.KeyCode == Keys.Escape)
            {
                if (!Debugger.IsAttached)
                    Hide();

                e.SuppressKeyPress = true;
                e.Handled = true;
            }
            else if (!e.Alt && !e.Control && !e.Shift && e.KeyCode == Keys.Enter)
            {
                TriggerAction(ActionType.Execution);
                e.SuppressKeyPress = true;
                e.Handled = true;
            }

            base.OnKeyDown(e);
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

            if (string.IsNullOrWhiteSpace(TextBox.Text))
                TriggerAction(ActionType.TextChanged);
            else
                TextBox.Text = "";

            base.OnShown(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            TextBox.TextChanged += (_, __) => TriggerAction(ActionType.TextChanged);
            base.OnLoad(e);
        }

        public void TriggerAction(ActionType actionType)
        {
            FilterText = null;
            ActionType = actionType;

            RootRequestHandler.Handle();

            var selectedIndex = ListBox.SelectedIndex;

            ListBox.Items.Clear();

            foreach (var item in new FilterAlgorithm(FilterText).Filter(Items))
                ListBox.Items.Add(item);

            if (actionType != ActionType.Execution)
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

        public string FilterText { get; set; }

        public Selection TextSelection
        {
            get => new Selection(TextBox.SelectionStart, TextBox.SelectionLength);
            set
            {
                TextBox.SelectionStart = value.Start;
                TextBox.SelectionLength = value.Length;
            }
        }

        public SearchItem SelectedItem => (SearchItem)ListBox.SelectedItem;

        public ActionType ActionType { get; private set; }

        public void AddItems(IEnumerable<SearchItem> items)
        {
            Items.AddRange(items);
        }
    }
}
