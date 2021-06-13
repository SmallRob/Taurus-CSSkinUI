
using System;

namespace Com_CSSkin.Win32.Const
{
    public class ESB
    {
        private ESB() { }

        public const int ESB_ENABLE_BOTH = 0x0000;
        public const int ESB_DISABLE_BOTH = 0x0003;

        public const int ESB_DISABLE_LEFT = 0x0001;
        public const int ESB_DISABLE_RIGHT = 0x0002;

        public const int ESB_DISABLE_UP = 0x0001;
        public const int ESB_DISABLE_DOWN = 0x0002;

        public const int ESB_DISABLE_LTUP = ESB_DISABLE_LEFT;
        public const int ESB_DISABLE_RTDN = ESB_DISABLE_RIGHT;
    }
}
