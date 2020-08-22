using System.Threading;
using System.Windows.Forms;
using BTurk.Automation.Host.AssemblyLoading;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.DependencyResolution
{
    public class GuestProcess : IGuestProcess
    {
        private MainForm _mainForm;

        public void Start()
        {
            try
            {
                _mainForm = Container.GetInstance<MainForm>();
                Application.Run(_mainForm);
            }
            catch (ThreadAbortException)
            {
                // most probably the domain is being unloaded, it is safe to ignore this exception
            }
        }

        public void Unload()
        {
        }
    }
}