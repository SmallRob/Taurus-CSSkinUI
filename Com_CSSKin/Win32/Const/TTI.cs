
using System;

namespace Com_CSSkin.Win32.Const
{
    /// <summary>
    /// ToolTip Icons possible wParam values for TTM_SETTITLE message.
    /// </summary>
    public static class TTI
    {
        public const int TTI_NONE = 0;
        public const int TTI_INFO = 1; //(32512)
        public const int TTI_WARNING = 2;
        public const int TTI_ERROR = 3;

        //// values larger thant TTI_ERROR are assumed to be an HICON value
        public const int TTI_INFO_LARGE = 4;
        public const int TTI_WARNING_LARGE = 5;
        public const int TTI_ERROR_LARGE = 6;
    }
}
