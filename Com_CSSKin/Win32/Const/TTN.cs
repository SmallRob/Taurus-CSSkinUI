
using System;
using System.Runtime.InteropServices;

namespace Com_CSSkin.Win32.Const
{
    public static class TTN
    {
        // ownerdraw
        public const int TTN_FIRST = (-520);

        public const int TTN_GETDISPINFOA = (TTN_FIRST - 0);
        public const int TTN_GETDISPINFOW = (TTN_FIRST - 10);
        public const int TTN_SHOW = (TTN_FIRST - 1);
        public const int TTN_POP = (TTN_FIRST - 2);
        public const int TTN_LINKCLICK = (TTN_FIRST - 3);

        public const int TTN_NEEDTEXTA = TTN_GETDISPINFOA;
        public const int TTN_NEEDTEXTW = TTN_GETDISPINFOW;

        public const int TTN_LAST = (-549);

        public readonly static int TTN_GETDISPINFO;
        public readonly static int TTN_NEEDTEXT;

        static TTN()
        {
            bool unicode = Marshal.SystemDefaultCharSize != 1;
            if (unicode)
            {
                TTN_GETDISPINFO = TTN_GETDISPINFOW;
                TTN_NEEDTEXT = TTN_NEEDTEXTW;
            }
            else
            {
                TTN_GETDISPINFO = TTN_GETDISPINFOA;
                TTN_NEEDTEXT = TTN_NEEDTEXTA;
            }
        }
    }
}
