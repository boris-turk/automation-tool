using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AutomationEngine;

namespace WorkTimeRecording
{
    public partial class WorkingTimeInput : AutomationEngineForm
    {
        public WorkingTimeInput()
        {
            InitializeComponent();

            StartPosition = FormStartPosition.CenterScreen;
        }

        private static string FilePath => @"xlab\work.txt";

        public string Description
        {
            get => _description.Text;
            set => _description.Text = value;
        }

        private string DateText => _date.Value.Date.ToString("d.M.yyyy");

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                ActiveControl = _description;
                SetDefaultValues();
            }
            base.OnVisibleChanged(e);
        }

        private void SetDefaultValues()
        {
            _date.Value = DateTime.Now.Date;
        }

        protected override void OnExecute()
        {
            List<string> contents = new List<string>();
            if (File.Exists(FilePath))
            {
                contents = File.ReadAllLines(FilePath).ToList();
            }
            AppendDateEntryIfNecessary(contents);

            int index = contents.FindIndex(x => x.Trim() == DateText);

            var existingEntriesCount = contents
                .Skip(index)
                .TakeWhile(x => x.Trim() != "")
                .Count();

            contents.Insert(index + existingEntriesCount, _description.Text);

            File.WriteAllLines(FilePath, contents);
            ResetContents();

            Hide();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if ((e.Alt || e.Control) && e.KeyCode == Keys.E)
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                Hide();
                Process.Start(FilePath);
            }

            base.OnKeyDown(e);
        }

        private void AppendDateEntryIfNecessary(List<string> contents)
        {
            if (contents.All(x => x.Trim() != DateText))
            {
                contents.AddRange(new[] {"", DateText});
            }
        }

        private void ResetContents()
        {
            _description.Text = string.Empty;
        }

        protected override string ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(Description))
            {
                return "Description cannot be empty.";
            }
            return null;
        }
    }
}
