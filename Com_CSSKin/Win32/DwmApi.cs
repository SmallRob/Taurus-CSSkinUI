
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Com_CSSkin.Win32
{
    public class DwmApi
    {
        /// <summary>
        /// 允许在指定的窗口模糊效果
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="pBlurBehind"></param>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmEnableBlurBehindWindow(IntPtr hWnd, DWM_BLURBEHIND pBlurBehind);

        /// <summary>
        /// 扩展到客户端区域的窗框
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="pMargins"></param>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmExtendFrameIntoClientArea(IntPtr hWnd, MARGINS pMargins);

        /// <summary>
        /// 获取一个值，指示是否启用DWM的组成。 应用程序可以收听组成状态变化的处理
        /// </summary>
        /// <returns></returns>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern bool DwmIsCompositionEnabled();

        /// <summary>
        /// 检索DWM的玻璃成分中使用当前的颜色。.应用程序可以听颜色的变化， 处理 WM_DWMCOLORIZATIONCOLORCHANGED通知
        /// </summary>
        /// <param name="pcrColorization"></param>
        /// <param name="pfOpaqueBlend"></param>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmGetColorizationColor(out int pcrColorization, [MarshalAs(UnmanagedType.Bool)]out bool pfOpaqueBlend);

        /// <summary>
        /// 启用或禁用DWM的组成
        /// </summary>
        /// <param name="bEnable"></param>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmEnableComposition(bool bEnable);

        /// <summary>
        /// 创建目标和源窗口之间的DWM缩略图关系
        /// </summary>
        /// <param name="dest"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern IntPtr DwmRegisterThumbnail(IntPtr dest, IntPtr source);

        /// <summary>
        /// 移除DWM缩略图关系DwmRegisterThumbnail功能
        /// </summary>
        /// <param name="hThumbnail"></param>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmUnregisterThumbnail(IntPtr hThumbnail);

        /// <summary>
        /// 更新的属性为DWM缩略图。
        /// </summary>
        /// <param name="hThumbnail"></param>
        /// <param name="props"></param>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmUpdateThumbnailProperties(IntPtr hThumbnail, DWM_THUMBNAIL_PROPERTIES props);

        /// <summary>
        /// 检索源DWM缩略图的大小
        /// </summary>
        /// <param name="hThumbnail"></param>
        /// <param name="size"></param>
        [DllImport("dwmapi.dll", PreserveSig = false)]
        public static extern void DwmQueryThumbnailSourceSize(IntPtr hThumbnail, out Size size);

        [StructLayout(LayoutKind.Sequential)]
        public class DWM_THUMBNAIL_PROPERTIES
        {
            public uint dwFlags;
            public RECT rcDestination;
            public RECT rcSource;
            public byte opacity;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fVisible;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fSourceClientAreaOnly;

            public const uint DWM_TNP_RECTDESTINATION = 0x00000001;
            public const uint DWM_TNP_RECTSOURCE = 0x00000002;
            public const uint DWM_TNP_OPACITY = 0x00000001;
            public const uint DWM_TNP_VISIBLE = 0x00000008;
            public const uint DWM_TNP_SOURCECLIENTAREAONLY = 0x00000010;
        }

        [StructLayout(LayoutKind.Sequential)]
        public class MARGINS
        {
            public int cxLeftWidth, cxRightWidth, cyTopHeight, cyBottomHeight;

            public MARGINS(int left, int top, int right, int bottom)
            {
                cxLeftWidth = left; cyTopHeight = top;
                cxRightWidth = right; cyBottomHeight = bottom;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public class DWM_BLURBEHIND
        {
            public uint dwFlags;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fEnable;
            public IntPtr hRegionBlur;
            [MarshalAs(UnmanagedType.Bool)]
            public bool fTransitionOnMaximized;

            public const uint DWM_BB_ENABLE = 0x00000001;
            public const uint DWM_BB_BLURREGION = 0x00000002;
            public const uint DWM_BB_TRANSITIONONMAXIMIZED = 0x00000004;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int left, top, right, bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left; this.top = top; this.right = right; this.bottom = bottom;
            }
        }
    }
}
