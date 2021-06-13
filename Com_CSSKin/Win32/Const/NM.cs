
using System;

namespace Com_CSSkin.Win32.Const
{
    /// <summary>
    /// Generic WM_NOTIFY notification codes 
    /// </summary>
    public static class NM
    {
        public const int NM_FIRST = 0;
        public const int NM_OUTOFMEMORY = (NM_FIRST - 1);
        public const int NM_CLICK = (NM_FIRST - 2);    // uses NMCLICK struct
        public const int NM_DBLCLK = (NM_FIRST - 3);
        public const int NM_RETURN = (NM_FIRST - 4);
        public const int NM_RCLICK = (NM_FIRST - 5);    // uses NMCLICK struct
        public const int NM_RDBLCLK = (NM_FIRST - 6);
        public const int NM_SETFOCUS = (NM_FIRST - 7);
        public const int NM_KILLFOCUS = (NM_FIRST - 8);

#if (_WIN32_IE0300) //>= 0x0300
        public const int NM_CUSTOMDRAW = (NM_FIRST-12);
        public const int NM_HOVER = (NM_FIRST-13);
#endif

#if (_WIN32_IE0400) //>= 0x0400
        public const int NM_NCHITTEST = (NM_FIRST-14);   // uses NMMOUSE struct
        public const int NM_KEYDOWN  = (NM_FIRST-15);   // uses NMKEY struct
        public const int NM_RELEASEDCAPTURE = (NM_FIRST-16);
        public const int NM_SETCURSOR = (NM_FIRST-17);   // uses NMMOUSE struct
        public const int NM_CHAR = (NM_FIRST-18);   // uses NMCHAR struct
#endif

#if (_WIN32_IE0401) //>= 0x0401
        public const int NM_TOOLTIPSCREATED = (NM_FIRST-19);   // notify of when the tooltips window is create
#endif

#if (_WIN32_IE0500) //>= 0x0500
        public const int NM_LDOWN = (NM_FIRST-20);
        public const int NM_RDOWN = (NM_FIRST-21);
        public const int NM_THEMECHANGED = (NM_FIRST-22);
#endif
    }
}
