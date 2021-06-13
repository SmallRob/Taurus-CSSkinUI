using System;
using System.Runtime.InteropServices;

namespace Com_CSSkin.Win32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct TT_HITTESTINFO
    {
        public IntPtr hwnd;
        public POINT pt;
        public TOOLINFO ti;
    }
}
