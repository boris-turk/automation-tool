﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BTurk.Automation.Core.SearchEngine;

namespace BTurk.Automation.Core
{
    internal partial class MainForm : Form
    {
        private const int OutOfScreenOffset = -20000;

        public event Action AhkFunctionResultReported;

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

        public ISearchHandler SearchHandler { get; set; }

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

        protected override void OnShown(EventArgs e)
        {
            if (Debugger.IsAttached)
                CenterToScreen();
            else
                Visible = false;

            if (string.IsNullOrWhiteSpace(TextBox.Text))
                ReloadSearchResults();
            else
                TextBox.Text = "";

            base.OnShown(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            ReloadSearchResults();
            TextBox.TextChanged += (_, __) => ReloadSearchResults();
            base.OnLoad(e);
        }

        public void ReloadSearchResults()
        {
            var result = SearchHandler.Handle(TextBox.Text);

            ListBox.Items.Clear();

            foreach (var item in result.Items.Where(_ => !_.IsFilteredOut))
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
                //_selectedIndexChangedEventDeactivated = true;
                ListBox.SelectedIndex = itemIndex;
                //_selectedIndexChangedEventDeactivated = false;
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

        public void RaiseAhkFunctionResultReportedEvent()
        {
            AhkFunctionResultReported?.Invoke();
        }
    }
}
