using System;
using System.Runtime.InteropServices;

namespace Com_CSSkin.Win32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NCCALCSIZE_PARAMS
    {
        public RECT rgrc0;     //Proposed New Window Coordinates
        public RECT rgrc1;     //Original Window Coordinates (before resize/move)
        public RECT rgrc2;     //Original Client Area (before resize/move)
        public IntPtr lppos;
    }
}
