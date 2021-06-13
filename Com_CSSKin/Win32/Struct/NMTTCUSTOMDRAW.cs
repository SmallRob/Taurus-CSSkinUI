using System;
using System.Runtime.InteropServices;

namespace Com_CSSkin.Win32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NMTTCUSTOMDRAW
    {
        public NMCUSTOMDRAW nmcd;
        public uint uDrawFlags;
    }
}
