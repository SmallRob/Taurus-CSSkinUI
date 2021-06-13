
using System;

namespace Com_CSSkin.Win32.Const
{
    /// <summary>
    /// SetWindowPos Flags.
    /// </summary>
    public static class SWP
    {
        public const uint SWP_NOSIZE = 0x0001;
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOZORDER = 0x0004;
        public const uint SWP_NOREDRAW = 0x0008;
        public const uint SWP_NOACTIVATE = 0x0010;
        public const uint SWP_FRAMECHANGED = 0x0020;//The frame changed: send WM_NCCALCSIZE
        public const uint SWP_SHOWWINDOW = 0x0040;
        public const uint SWP_HIDEWINDOW = 0x0080;
        public const uint SWP_NOCOPYBITS = 0x0100;
        public const uint SWP_NOOWNERZORDER = 0x0200;//Don't do owner Z ordering
        public const uint SWP_NOSENDCHANGING = 0x0400;//Don't send WM_WINDOWPOSCHANGING

        public const uint SWP_DRAWFRAME = SWP_FRAMECHANGED;
        public const uint SWP_NOREPOSITION = SWP_NOOWNERZORDER;

#if(WINVER0400) //>= 0x0400
        public const uint SWP_DEFERERASE = 0x2000;
        public const uint SWP_ASYNCWINDOWPOS = 0x4000;
#endif
    }
}
