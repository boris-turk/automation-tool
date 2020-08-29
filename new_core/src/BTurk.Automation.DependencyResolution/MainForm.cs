using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
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
            Closing += (sender, args) =>
            {
                args.Cancel = true;
                Visible = false;
            };
            _listBox.SelectedIndexChanged += (o, a) => MoveFocusToTextBox();
        }

        public List<SearchItem> Items { get; }

        public ISearchHandler<Request> SearchHandler { get; set; }

        private void MoveFocusToTextBox()
        {
            if (!_textBox.Focused)
            {
                _textBox.Focus();
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
            ActionType = actionType;

            SearchHandler.Handle(new Request());

            var filterText = GetFilterText();

            ListBox.Items.Clear();

            foreach (var item in Items)
                ListBox.Items.Add(item.Text);

            SelectItem(0);
        }

        private void SelectItem(int itemIndex)
        {
            if (itemIndex < 0)
            {
                return;
            }

            if (ListBox.Items.Count > itemIndex)
            {
                ListBox.SelectedIndex = itemIndex;
            }
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

        private string GetFilterText()
        {
            var selection = FilterSelection;

            if (FilterSelection == null)
                selection = new Selection(0, TextBox.Text.Length);

            if (selection.Length <= 0)
                return "";

            return TextBox.Text.Substring(selection.Start, selection.Length);
        }

        public string SearchText
        {
            get => TextBox.Text;
            set => TextBox.Text = value;
        }

        public Selection FilterSelection { get; set; }

        public Selection TextSelection
        {
            get => new Selection(TextBox.SelectionStart, TextBox.SelectionLength);
            set
            {
                TextBox.SelectionStart = value.Start;
                TextBox.SelectionLength = value.Length;
            }
        }

        public int SelectedItemIndex => ListBox.SelectedIndex;

        public ActionType ActionType { get; private set; }

        public void AddItems(IEnumerable<SearchItem> items)
        {
            Items.AddRange(items);
        }
    }
}
