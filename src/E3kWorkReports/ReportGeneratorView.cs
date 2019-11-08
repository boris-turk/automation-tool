using System;
using System.IO;
using AutomationEngine;
using E3kWorkReports.Properties;

// ReSharper disable IdentifierTypo

namespace E3kWorkReports
{
    public partial class ReportGeneratorView : AutomationEngineForm
    {
        public event Action Execute;

        public string DirectoryPath => Directory.Text;

        public string BorisFileName => BorisFile.Text;

        public string AndrejFileName => AndrejFile.Text;

        public string BorutFileName => BorutFile.Text;

        public ReportGeneratorView()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            var month = $"{DateTime.Now.Year}-{DateTime.Now.Month:00}";
            Directory.Text = Path.Combine(Settings.Default.Directory, month);
            base.OnLoad(e);
        }

        private void OnGenerateReportClick(object sender, EventArgs e) => Execute?.Invoke();
    }
}
