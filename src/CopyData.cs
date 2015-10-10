using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Ahk.Messages;

namespace Ahk
{
    public static class MessageHelper
    {
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        private delegate bool EnumThreadDelegate(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumThreadWindows(uint dwThreadId, EnumThreadDelegate lpfn, IntPtr lParam);

        [StructLayout(LayoutKind.Sequential)]
        private struct CopyDataStructure
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
        private struct DataStructure
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 11)]
            public string Text;
        }

        // Allocate a pointer to an arbitrary structure on the global heap.
        private static IntPtr IntPtrAlloc<T>(T param)
        {
            IntPtr retval = Marshal.AllocHGlobal(Marshal.SizeOf(param));
            Marshal.StructureToPtr(param, retval, false);
            return retval;
        }

        // Free a pointer to an arbitrary structure from the global heap.
        private static void IntPtrFree(ref IntPtr preAllocated)
        {
            if (IntPtr.Zero == preAllocated)
                throw (new NullReferenceException("Go Home"));
            Marshal.FreeHGlobal(preAllocated);
            preAllocated = IntPtr.Zero;
        }

        public static void SendMessage(Process process, string message)
        {
            var dataStructure = new DataStructure
            {
                Text = message
            };

            IntPtr hWnd = WindowHandles(process).FirstOrDefault();

            IntPtr buffer = IntPtrAlloc(dataStructure);
            CopyDataStructure copyData = new CopyDataStructure
            {
                dwData = IntPtr.Zero,
                lpData = buffer,
                cbData = Marshal.SizeOf(dataStructure)
            };

            IntPtr copyDataBuff = IntPtrAlloc(copyData);
            SendMessage(hWnd, WindowMessages.WmCopydata, IntPtr.Zero, copyDataBuff);
            IntPtrFree(ref copyDataBuff);
            IntPtrFree(ref buffer);
        }

        private static IEnumerable<IntPtr> WindowHandles(Process process)
        {
            var handles = new List<IntPtr>();
            foreach (ProcessThread thread in process.Threads)
                EnumThreadWindows((uint)thread.Id, (hWnd, lParam) => { handles.Add(hWnd); return true; }, IntPtr.Zero);
            return handles;
        }
    }
}