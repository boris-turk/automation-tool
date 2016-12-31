using System.Linq;
using System.Text.RegularExpressions;
using AutomationEngine;

namespace WorkTimeRecording
{
    public class WorkingTimeReport : IPlugin
    {
        public string Id => "report_working_time";

        public void Execute(params string[] arguments)
        {
            var mainForm = FormFactory.Instance<WorkingTimeInput>();
            mainForm.Project = GetProject(arguments);
            mainForm.Task = GetTask(arguments);
            mainForm.Show();
        }

        private string GetProject(string[] arguments)
        {
            string text = (arguments.FirstOrDefault() ?? string.Empty).Trim();
            return Regex.Replace(text, @"\s.*", "");
        }

        private string GetTask(string[] arguments)
        {
            string text = (arguments.FirstOrDefault() ?? string.Empty).Trim();
            return Regex.Replace(text, @"\S+\s+", "");
        }
    }
}