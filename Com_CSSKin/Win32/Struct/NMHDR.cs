using System;
using System.Runtime.InteropServices;

namespace Com_CSSkin.Win32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct NMHDR
    {
        public NMHDR(int flag)
        {
            this.hwndFrom = IntPtr.Zero;
            this.idFrom = 0;
            this.code = 0;
        }

        public IntPtr hwndFrom;
        public int idFrom;
        public int code;
    }
}
