using System.Diagnostics;
using BTurk.Automation.Core;

namespace BTurk.Automation.DependencyResolution
{
    public class ProcessStarter : IProcessStarter
    {
        public void Start(string fileName, string arguments)
        {
            Process.Start(fileName, arguments);
        }
    }
}