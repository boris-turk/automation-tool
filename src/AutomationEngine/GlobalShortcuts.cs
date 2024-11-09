using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
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

                using (var fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var reader = new StreamReader(fileStream))
                {
                    if (!reader.EndOfStream)
                        return reader.ReadLine();
                }

                return string.Empty;
            }
            catch
            {
                Thread.Sleep(50);
                return ReadWithRetry(count - 1);
            }
        }

        public static string GetActiveApplicationContext()
        {
            var currentWindow = GlobalShortcuts.GetForegroundWindow();
            var currentWindowText = GlobalShortcuts.GetWindowText(currentWindow);
            var className = GlobalShortcuts.GetClassName(currentWindow);
            var text = $"{currentWindowText} ahk_class {className}";
            return text;
        }

        private static string GetWindowText(IntPtr hWnd)
        {
            int size = GetWindowTextLength(hWnd);
            if (size > 0)
            {
                var builder = new StringBuilder(size + 1);
                GetWindowText(hWnd, builder, builder.Capacity);
                return builder.ToString();
            }

            return "";
        }

        private static string GetClassName(IntPtr handle)
        {
            const int maxChars = 256;
            var className = new StringBuilder(maxChars);

            if (GetClassName(handle.ToInt32(), className, maxChars) > 0)
                return className.ToString();

            return "";
        }

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetClassName(int hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);
    }
}