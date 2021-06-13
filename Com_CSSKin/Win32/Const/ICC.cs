
using System;

namespace Com_CSSkin.Win32.Const
{
    /// <summary>
    /// INITCOMMONCONTROLSEX 结构的 dwICC 字段使用的常量。
    /// </summary>
    public static class ICC
    {
        /// <summary>
        /// listview, header
        /// </summary>
        public const int ICC_LISTVIEW_CLASSES = 0x00000001; 

        /// <summary>
        /// treeview, tooltips
        /// </summary>
        public const int ICC_TREEVIEW_CLASSES = 0x00000002;

        /// <summary>
        /// 注册工具栏、状态栏、Trackbar和Tooltip类。
        /// </summary>
        public const int ICC_BAR_CLASSES = 0x00000004; 
        
        /// <summary>
        /// tab, tooltips
        /// </summary>
        public const int ICC_TAB_CLASSES = 0x00000008;

        /// <summary>
        /// updown
        /// </summary>
        public const int ICC_UPDOWN_CLASS = 0x00000010; 

        /// <summary>
        /// progress
        /// </summary>
        public const int ICC_PROGRESS_CLASS = 0x00000020;

        /// <summary>
        /// hotkey
        /// </summary>
        public const int ICC_HOTKEY_CLASS = 0x00000040; 

        /// <summary>
        /// animate
        /// </summary>
        public const int ICC_ANIMATE_CLASS = 0x00000080; 
 
        public const int ICC_WIN95_CLASSES = 0x000000FF;

        /// <summary>
        /// month picker, date picker, time picker, updown
        /// </summary>
        public const int ICC_DATE_CLASSES = 0x00000100;

        /// <summary>
        /// comboex
        /// </summary>
        public const int ICC_USEREX_CLASSES = 0x00000200;

        /// <summary>
        ///注册Rebar类。
        /// </summary>
        public const int ICC_COOL_CLASSES = 0x00000400;

#if (_WIN32_IE0400) //>= 0x0400)
        public const int ICC_INTERNET_CLASSES = 0x00000800;

        /// <summary>
        /// page scroller
        /// </summary>
        public const int ICC_PAGESCROLLER_CLASS = 0x00001000;

        /// <summary>
        /// native font control
        /// </summary>
        public const int ICC_NATIVEFNTCTL_CLASS = 0x00002000;
#endif

#if (_WIN32_WINNT501)  //>= 0x501
        public const int ICC_STANDARD_CLASSES = 0x00004000;
        public const int ICC_LINK_CLASS = 0x00008000;
#endif
    }
}
