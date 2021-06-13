using System;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Com_CSSkin.Win32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WINDOWPLACEMENT
    {
        public int length;
        public int flags;
        public int showCmd;
        public Point ptMinPosition;
        public Point ptMaxPosition;
        public RECT rcNormalPosition;
        public static WINDOWPLACEMENT Default
        {
            get
            {
                WINDOWPLACEMENT structure = new WINDOWPLACEMENT();
                structure.length = Marshal.SizeOf(structure);
                return structure;
            }
        }
    }
}
