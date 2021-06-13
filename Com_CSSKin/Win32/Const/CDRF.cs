
using System;

namespace Com_CSSkin.Win32.Const
{
    /// <summary>
    /// custom draw return flags
    /// values under 0x00010000 are reserved for global custom draw values.
    /// above that are for specific controls
    /// </summary>
    public static class CDRF
    {
        public const int CDRF_DODEFAULT = 0x0;
        public const int CDRF_NEWFONT = 0x2;
        public const int CDRF_SKIPDEFAULT = 0x4;
        public const int CDRF_NOTIFYPOSTPAINT = 0x10;
        public const int CDRF_NOTIFYITEMDRAW = 0x20;

#if (_WIN32_IE0400) //>= 0x0400
        public const int CDRF_NOTIFYSUBITEMDRAW = 0x20; // flags are the same, we can distinguish by context
#endif
        public const int CDRF_NOTIFYPOSTERASE = 0x40;
        public const int CDRF_NOTIFYITEMERASE = 0x80;
    }
}
