
using System;
using System.Runtime.InteropServices;

namespace Com_CSSkin.Win32.Com
{
    [ComVisible(true), StructLayout(LayoutKind.Sequential)]
    public struct tagOLECMD
    {
        [MarshalAs(UnmanagedType.U4)]
        public uint cmdID;
        [MarshalAs(UnmanagedType.U4)]
        public uint cmdf;
    }
}
