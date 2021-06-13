using System;
using System.Runtime.InteropServices;

namespace Com_CSSkin.Win32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct POINT
    {
        public int X;
        public int Y;

        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
