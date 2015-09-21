using System;
using System.Runtime.InteropServices;

namespace Ahk.Messages
{
    public struct CopyDataStruct
    {
        public IntPtr DwData;

        public int CbData;

        [MarshalAs(UnmanagedType.LPStr)]
        public string LpData;
    }
}