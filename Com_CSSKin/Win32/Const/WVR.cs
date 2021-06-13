
using System;

namespace Com_CSSkin.Win32.Const
{
    /// <summary>
    /// WM_NCCALCSIZE "window valid rect" return values.
    /// </summary>
    public class WVR
    {
        public const int WVR_ALIGNTOP = 0x0010;
        public const int WVR_ALIGNLEFT = 0x0020;
        public const int WVR_ALIGNBOTTOM = 0x0040;
        public const int WVR_ALIGNRIGHT = 0x0080;
        public const int WVR_HREDRAW = 0x0100;
        public const int WVR_VREDRAW = 0x0200;
        public const int WVR_REDRAW = (WVR_HREDRAW | WVR_VREDRAW);
        public const int WVR_VALIDRECTS = 0x0400;
    }
}
