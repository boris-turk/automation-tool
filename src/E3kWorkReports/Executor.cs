using AutomationEngine;

namespace E3kWorkReports
{
    public class Executor : IPluginExecutor
    {
        public string Id => "E3kWorkReportGenerator";

        public void Execute(params string[] arguments)
        {
            var mainForm = FormFactory.Instance<ReportGeneratorView>();
            mainForm.Execute += () => GenerateReport(mainForm);
            mainForm.Show();
        }

        private void GenerateReport(ReportGeneratorView form)
        {
            var generator = new ReportGenerator(form.DirectoryPath, form.BorutFileName);
            generator.Execute();
        }
    }
}
