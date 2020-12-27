using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

namespace AutomationEngine
{
    public class GlobalShortcuts
    {
        public const int OpenMainWindowShortcutId = 1;
        public const int OpenAppContextWindowShortcutId = 2;

        private const uint MOD_ALT = 0x0001;
        private const int VK_SPACE = 0x20;
        private const int VK_OEM_1 = 0xBA; // semicolon

        private readonly string _filePath = Path.Combine(Path.GetTempPath(), "new_automation_activity.txt");

        private Timer _timer;
        private MainForm _form;
        private bool _shortcutsInstalled;

        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public void Install()
        {
            _form = FormFactory.Instance<MainForm>();

            _timer = new Timer(OnTimerElapsed, null, Timeout.Infinite, Timeout.Infinite);
            _timer.Change(0, Timeout.Infinite);
        }
        
        private void OnTimerElapsed(object state)
        {
            var newProcessRunning = IsNewProcessRunning();

            if (newProcessRunning && _shortcutsInstalled)
                _form.Invoke((Action)UnRegisterHotKeys);
            else if (!newProcessRunning && !_shortcutsInstalled)
                _form.Invoke((Action)RegisterHotKeys);

            _timer.Change(500, Timeout.Infinite);
        }

        private void RegisterHotKeys()
        {
            _shortcutsInstalled = RegisterHotKey(_form.Handle, OpenMainWindowShortcutId, MOD_ALT, VK_SPACE);
            _shortcutsInstalled = RegisterHotKey(_form.Handle, OpenAppContextWindowShortcutId, MOD_ALT, VK_OEM_1);
        }

        public void UnRegisterHotKeys()
        {
            if (!_shortcutsInstalled)
                return;

            _shortcutsInstalled = false;

            UnregisterHotKey(_form.Handle, OpenMainWindowShortcutId);
            UnregisterHotKey(_form.Handle, OpenAppContextWindowShortcutId);
        }

        private bool IsNewProcessRunning()
        {
            try
            {
                var text = ReadWithRetry(3);

                if (string.IsNullOrWhiteSpace(text))
                    return false;

                var success = DateTime.TryParseExact(text, "MM/dd/yyyy hh:mm:ss.fff tt", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out var result);

                if (!success)
                    return false;

                var isRunning = DateTime.Now - result < TimeSpan.FromMilliseconds(1000);

                return isRunning;
            }
            catch
            {
                return false;
            }
        }

        private string ReadWithRetry(int count)
        {
            try
            {
                if (count == 0 || !File.Exists(_filePath))
                    return null;

                return File.ReadAllLines(_filePath).FirstOrDefault();
            }
            catch
            {
                Thread.Sleep(50);
                return ReadWithRetry(count - 1);
            }
        }
    }
}