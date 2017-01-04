using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AutomationEngine;

namespace WorkTimeRecording
{
    public partial class WorkingTimeInput : AutomationEngineForm
    {
        private readonly TextBoxState _durationTextBoxState;
        private bool _ignoreDurationChange;

        public WorkingTimeInput()
        {
            InitializeComponent();

            _durationTextBoxState = new TextBoxState(_duration);
            _durationTextBoxState.Save();

            _duration.TextChanged += (sender, args) => OnDurationChanged();
            _duration.LostFocus += (sender, args) => OnDurationLostFocus();

            StartPosition = FormStartPosition.CenterScreen;
        }

        private void OnDurationLostFocus()
        {
            _duration.Text = GetDuration().ToTimeSpanString();
        }

        private void OnDurationChanged()
        {
            if (_ignoreDurationChange)
            {
                _ignoreDurationChange = false;
                return;
            }
            if (!IsValidDuration() && _duration.Text.Length > 0)
            {
                _ignoreDurationChange = true;
                _durationTextBoxState.Restore();
            }
            else
            {
                _durationTextBoxState.Save();
            }
        }

        public string Project
        {
            get { return _project.Text; }
            set { _project.Text = value; }
        }

        public string Task
        {
            get { return _task.Text; }
            set { _task.Text = value; }
        }

        public string Description
        {
            get { return _description.Text; }
            set { _description.Text = value; }
        }

        private string DurationText
        {
            get
            {
                string text = (_duration.Text ?? string.Empty).Trim();
                if (!text.Contains(":"))
                {
                    text += ":";
                }
                if (text.EndsWith(":"))
                {
                    text += "00";
                }
                if (Regex.IsMatch(text, @":\d$"))
                {
                    text += "0";
                }
                return text;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                ActiveControl = _duration;
                SetDefaultValues();
            }
            base.OnVisibleChanged(e);
        }

        private void SetDefaultValues()
        {
            _date.Value = DateTime.Now.Date;
            _duration.Text = string.Empty;
        }

        protected override void OnExecute()
        {
            var workingTimeEntry = new WorkingTimeEntry
            {
                Date = _date.Value.Date,
                Description = _description.Text,
                Duration = GetDuration(),
                Project = Project,
                Task = Task
            };

            WorkingTimeStorage.Instance.Entries.Add(workingTimeEntry);
            WorkingTimeStorage.Instance.Save();

            ResetContents();

            Hide();
        }

        private void ResetContents()
        {
            _duration.Text = string.Empty;
            _description.Text = string.Empty;
        }

        private TimeSpan GetDuration()
        {
            return DurationText.FromTimeSpanString();
        }

        private bool IsValidDuration()
        {
            TimeSpan result;
            return TimeSpan.TryParseExact(DurationText, @"h\:mm", CultureInfo.InvariantCulture, out result);
        }

        protected override string ValidateInput()
        {
            if (!IsValidDuration() || GetDuration().Ticks <= 0)
            {
                return "Invalid duration.";
            }
            if (string.IsNullOrWhiteSpace(Description))
            {
                return "Description cannot be empty.";
            }
            return null;
        }
    }
}
