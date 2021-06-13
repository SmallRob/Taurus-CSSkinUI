
using System;
using System.Collections.Generic;
using System.Text;

namespace Com_CSSkin.Win32.Const
{
    public static class CS
    {
        public const int CS_VREDRAW = 0x0001;
        public const int CS_HREDRAW = 0x0002;
        public const int CS_DBLCLKS = 0x0008;
        public const int CS_OWNDC = 0x0020;
        public const int CS_CLASSDC = 0x0040;
        public const int CS_PARENTDC = 0x0080;
        public const int CS_NOCLOSE = 0x0200;
        public const int CS_SAVEBITS = 0x0800;
        public const int CS_BYTEALIGNCLIENT = 0x1000;
        public const int CS_BYTEALIGNWINDOW = 0x2000;
        public const int CS_GLOBALCLASS = 0x4000;

        public const int CS_IME = 0x00010000;
        //#if(_WIN32_WINNT >= 0x0501)
        public const int CS_DROPSHADOW = 0x00020000;
    }
}
