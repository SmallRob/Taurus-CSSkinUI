
using System;

namespace Com_CSSkin.Win32.Const
{
    /// <summary>
    /// Use for setwindowpos.
    /// </summary>
    public static class HWND
    {
        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        public static readonly IntPtr HWND_TOP = new IntPtr(0);
        public static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
    }
}
