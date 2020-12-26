using System.Threading;
using System.Windows.Forms;
using BTurk.Automation.Core.WinApi;
using BTurk.Automation.Host.AssemblyLoading;

// ReSharper disable UnusedMember.Global

namespace BTurk.Automation.DependencyResolution
{
    public class GuestProcess : IGuestProcess
    {
        private MainForm _mainForm;

        public void Start()
        {
            GlobalKeyboardHook keyboardHook = null;

            try
            {
                keyboardHook = new GlobalKeyboardHook();
                keyboardHook.KeyboardPressed += OnKeyPressed;

                _mainForm = Container.GetInstance<MainForm>();
                Application.Run(_mainForm);
            }
            catch (ThreadAbortException)
            {
                // most probably the domain is being unloaded, it is safe to ignore this exception
            }
            finally
            {
                keyboardHook?.Dispose();
            }
        }

        private void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (IsToggleAutomationWindowEvent(e))
            {
                e.Handled = true;
                _mainForm.ToggleVisibility();
            }
        }

        private bool IsToggleAutomationWindowEvent(GlobalKeyboardHookEventArgs e)
        {
            return
                e.KeyboardState == KeyboardState.SysKeyDown &&
                e.KeyboardData.Flags == Constants.LlkhfAltdown &&
                e.KeyboardData.VirtualCode == Constants.VK_SPACE;
        }

        public void Unload()
        {
        }
    }
}