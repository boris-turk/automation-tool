using AutomationEngine;

namespace WorkTimeRecording
{
    public class WorkingTimeReport : IPluginExecutor
    {
        public string Id => "report_working_time";

        public void Execute(params string[] arguments)
        {
            var mainForm = FormFactory.Instance<WorkingTimeInput>();
            mainForm.Description = string.Empty;
            mainForm.Show();
        }
    }
}