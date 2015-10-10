using System;
using System.Runtime.InteropServices;

namespace AutomationEngine.Messages
{
    public struct CopyDataStruct
    {
        public IntPtr DwData;

        public int CbData;

        [MarshalAs(UnmanagedType.LPStr)]
        public string LpData;
    }
}
