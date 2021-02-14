using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

// ReSharper disable IdentifierTypo

namespace BTurk.Automation.Core.WinApi
{
    public static class Methods
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("ole32.dll")]
        public static extern int CreateBindCtx(uint reserved, out IBindCtx ppbc);

        [DllImport("user32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        public static int GetActiveProcessId()
        {
            var windowHandle = GetActiveWindow();
            GetWindowThreadProcessId(windowHandle, out var processId);
            return (int)processId;
        }

        public static IntPtr GetActiveWindow()
        {
            var foregroundWindow = GetForegroundWindow();
            var parentWindow = GetParent(foregroundWindow);
            return parentWindow != IntPtr.Zero ? parentWindow : foregroundWindow;
        }

        public static string GetWindowText(IntPtr hWnd)
        {
            int size = GetWindowTextLength(hWnd);

            if (size <= 0)
                return "";

            var builder = new StringBuilder(size + 1);
            GetWindowText(hWnd, builder, builder.Capacity);

            return builder.ToString();
        }

        public static string GetClassName(IntPtr handle)
        {
            const int maxChars = 256;
            var className = new StringBuilder(maxChars);

            if (GetClassName(handle.ToInt32(), className, maxChars) > 0)
                return className.ToString();

            return "";
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int GetClassName(int hWnd, StringBuilder lpClassName, int nMaxCount);
    }
}