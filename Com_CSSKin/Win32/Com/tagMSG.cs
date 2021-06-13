
using System;
using System.Runtime.InteropServices;

namespace Com_CSSkin.Win32.Com
{
    [ComVisible(true), StructLayout(LayoutKind.Sequential)]
    public struct tagMSG
    {
        public IntPtr hwnd;
        [MarshalAs(UnmanagedType.I4)]
        public int message;
        public IntPtr wParam;
        public IntPtr lParam;
        [MarshalAs(UnmanagedType.I4)]
        public int time;
        // pt was a by-value POINT structure
        [MarshalAs(UnmanagedType.I4)]
        public int pt_x;
        [MarshalAs(UnmanagedType.I4)]
        public int pt_y;
        //public tagPOINT pt;
    }
}
