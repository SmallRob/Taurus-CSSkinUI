
using System;

namespace Com_CSSkin.Win32.Const
{
    public class SIF
    {
        public const int SIF_RANGE = 0x1;
        public const int SIF_PAGE = 0x2;
        public const int SIF_POS = 0x4;
        public const int SIF_DISABLENOSCROLL = 0x8;
        public const int SIF_TRACKPOS = 0x10;
        public const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;
    }
}
