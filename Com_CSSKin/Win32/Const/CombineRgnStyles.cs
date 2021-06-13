
using System;

namespace Com_CSSkin.Win32.Const
{
    public class CombineRgnStyles
    {
        private CombineRgnStyles() { }

        public const int RGN_AND = 1;
        public const int RGN_OR = 2;
        public const int RGN_XOR = 3;
        public const int RGN_DIFF = 4;
        public const int RGN_COPY = 5;
        public const int RGN_MIN = RGN_AND;
        public const int RGN_MAX = RGN_COPY;
    }
}
