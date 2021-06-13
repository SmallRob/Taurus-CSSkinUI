
using System;

namespace Com_CSSkin.Win32.Const
{
    public static class TTF
    {
        public const int TTF_IDISHWND = 0x0001;
        // Use this to center around trackpoint in trackmode
        // -OR- to center around tool in normal mode.
        // Use TTF_ABSOLUTE to place the tip exactly at the track coords when
        // in tracking mode.  TTF_ABSOLUTE can be used in conjunction with TTF_CENTERTIP
        // to center the tip absolutely about the track point.
        public const int TTF_CENTERTIP = 0x0002;
        public const int TTF_RTLREADING = 0x0004;
        public const int TTF_SUBCLASS = 0x0010;
        public const int TTF_TRACK = 0x0020;
        public const int TTF_ABSOLUTE = 0x0080;
        public const int TTF_TRANSPARENT = 0x0100;
        public const int TTF_PARSELINKS = 0x1000;
        public const int TTF_DI_SETITEM = 0x8000;// valid only on the TTN_NEEDTEXT callback
    }
}
