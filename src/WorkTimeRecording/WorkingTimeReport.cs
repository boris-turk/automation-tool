using System.Collections.Generic;
using System.Collections.Specialized;
using AutomationEngine;

namespace WorkTimeRecording
{
    public class WorkingTimeReport : IPluginExecutor
    {
        public string Id => "report_working_time";

        public void Execute(params string[] arguments)
        {
            var mainForm = FormFactory.Instance<WorkingTimeInput>();
            mainForm.Project = arguments[0];
            mainForm.Task = arguments[1];

            mainForm.Description = string.Empty;
            if (arguments.Length > 2)
            {
                mainForm.Description = arguments[2];
            }

            mainForm.Show();
        }
    }
}