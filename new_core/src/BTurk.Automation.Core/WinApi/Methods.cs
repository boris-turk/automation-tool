using System;
using System.Runtime.InteropServices;

// ReSharper disable IdentifierTypo

namespace BTurk.Automation.Core.WinApi
{
    public static class Methods
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);
    }
}