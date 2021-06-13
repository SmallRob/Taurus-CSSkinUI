
using System;
using System.Runtime.InteropServices;
using System.Drawing;
using Com_CSSkin.Win32.Struct;
using Com_CSSkin.Win32.Callback;
using Com_CSSkin.SkinControl;
using System.Windows.Forms;
using System.Diagnostics.CodeAnalysis;

namespace Com_CSSkin.Win32
{
    public class NativeMethods
    {
        private NativeMethods() {
        }
        #region 属性变换
        //Listview universal constants
        public const int LVM_FIRST = 0x1000;
        //Listview messages
        public const int LVM_SETEXTENDEDLISTVIEWSTYLE = LVM_FIRST + 54;
        public const int LVS_EX_FULLROWSELECT = 0x00000020;
        //Listview extended styles
        public const int LVS_EX_DOUBLEBUFFER = 0x00010000;
        public struct PCURSORINFO
        {
            public int cbSize;
            public int flag;
            public IntPtr hCursor;
            public NativeMethods.Point ptScreenPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Size
        {
            public Int32 cx;
            public Int32 cy;

            public Size(Int32 x, Int32 y) {
                cx = x;
                cy = y;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public Int32 x;
            public Int32 y;

            public Point(Int32 x, Int32 y) {
                this.x = x;
                this.y = y;
            }
        }

        public const Int32 ULW_ALPHA = 2;
        public const int WM_USER = 0x0400;
        public const int EM_GETOLEINTERFACE = WM_USER + 60;
        #endregion

        #region 枚举
        [Flags]
        public enum ImageListDrawFlags : int
        {
            ILD_NORMAL = 0x00000000,
            ILD_TRANSPARENT = 0x00000001,
            ILD_BLEND25 = 0x00000002,
            ILD_FOCUS = 0x00000002,
            ILD_BLEND50 = 0x00000004,
            ILD_SELECTED = 0x00000004,
            ILD_BLEND = 0x00000004,
            ILD_MASK = 0x00000010,
            ILD_IMAGE = 0x00000020,
            ILD_ROP = 0x00000040,
            ILD_OVERLAYMASK = 0x00000F00,
            ILD_PRESERVEALPHA = 0x00001000,
            ILD_SCALE = 0x00002000,
            ILD_DPISCALE = 0x00004000,
            ILD_ASYNC = 0x00008000
        }
        #endregion

        #region USER32.DLL
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern int SetLayeredWindowAttributes(IntPtr Handle, int crKey, byte bAlpha, int dwFlags);
        public static IntPtr SetWindowLong(IntPtr hWnd, short nIndex, IntPtr dwNewLong) {
            if (IntPtr.Size == 4) {
                return SetWindowLongPtr32(hWnd, nIndex, dwNewLong);
            }
            return SetWindowLongPtr64(hWnd, (int)nIndex, dwNewLong);
        }
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable")]
        [DllImport("user32", EntryPoint = "SetWindowLong")]
        public static extern IntPtr SetWindowLongPtr32(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist")]
        [DllImport("user32", EntryPoint = "SetWindowLongPtr")]
        public static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
        /// <summary>
        /// 热键定义
        /// </summary>
        /// <param name="hWnd">要定义热键的窗口的句柄 </param>
        /// <param name="id">定义热键ID（不能与其它ID重复）</param>
        /// <param name="fsModifiers">标识热键是否在按Alt、Ctrl、Shift、Windows等键时才会生效 </param>
        /// <param name="vk">定义热键的内容 </param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vk);
        /// <summary>
        /// 取消热键定义
        /// </summary>
        /// <param name="hWnd">要取消热键的窗口的句柄</param>
        /// <param name="id">要取消热键的ID</param>
        /// <returns></returns>
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(
            IntPtr hwnd, int msg, int wParam, ref HDITEM lParam);

        [DllImport("user32.dll")]
        public static extern bool GetComboBoxInfo(
            IntPtr hwndCombo, ref ComboBoxInfo info);

        [DllImport("user32.DLL", EntryPoint = "GetCaretBlinkTime")]
        public static extern uint GetCaretBlinkTime();

        [DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string fileName);

        [DllImport("user32.dll")]//在桌面找寻子窗体
        public static extern IntPtr ChildWindowFromPointEx(IntPtr pHwnd, NativeMethods.Point pt, uint uFlgs);
        public const int CWP_SKIPDISABLED = 0x2;   //忽略不可用窗体
        public const int CWP_SKIPINVISIBL = 0x1;   //忽略隐藏的窗体
        public const int CWP_All = 0x0;            //一个都不忽略

        [DllImport("user32.dll")]
        public static extern bool GetCursorInfo(out PCURSORINFO pci);

        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        /// <summary>
        /// 执行动画
        /// </summary>
        /// <param name="whnd">控件句柄</param>
        /// <param name="dwtime">动画时间</param>
        /// <param name="dwflag">动画组合名称</param>
        /// <returns>bool值，动画是否成功</returns>
        [DllImport("user32.dll")]
        public static extern bool AnimateWindow(IntPtr whnd, int dwtime, int dwflag);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll")]
        public static extern bool LockWindowUpdate(IntPtr hWndLock);

        [DllImport("user32.dll")]
        public static extern bool MessageBeep(int uType);

        [DllImport("user32.dll")]
        public static extern IntPtr BeginPaint(
            IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("user32.dll")]
        public static extern bool EndPaint(
            IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("user32.dll")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool TrackPopupMenuEx(
            IntPtr hMenu, uint uFlags, int x, int y, IntPtr hWnd, IntPtr tpmParams);

        [DllImport("user32.dll")]
        public static extern IntPtr TrackPopupMenu(
            IntPtr hMenu, int uFlags, int x, int y, int nReserved, IntPtr hWnd, IntPtr par);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(
            IntPtr hWnd, ref RECT rectUpdate, IntPtr hrgnUpdate, int flags);

        [DllImport("user32.dll")]
        public static extern bool RedrawWindow(
            IntPtr hWnd, IntPtr rectUpdate, IntPtr hrgnUpdate, int flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool AdjustWindowRectEx(
            ref RECT lpRect, int dwStyle, bool bMenu, int dwExStyle);

        [DllImport("user32.dll")]
        public static extern int GetSystemMetrics(int nIndex);

        [DllImport("user32.dll")]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        [DllImport("user32.dll")]
        public static extern void DisableProcessWindowsGhosting();

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(
            IntPtr hwndParent,
            IntPtr hwndChildAfter,
            string lpszClass,
            string lpszWindow);

        [DllImport("User32.dll", EntryPoint = "FlashWindow")]
        public static extern bool FlashWindow(
            IntPtr hWnd, bool bInvert);

        [DllImport("user32.dll")]
        public static extern IntPtr DefWindowProc(
            IntPtr hWnd, int uMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true,
            CharSet = CharSet.Unicode, BestFitMapping = false)]
        public static extern IntPtr CreateWindowEx(
            int exstyle,
            string lpClassName,
            string lpWindowName,
            int dwStyle,
            int x,
            int y,
            int nWidth,
            int nHeight,
            IntPtr hwndParent,
            IntPtr Menu,
            IntPtr hInstance,
            IntPtr lpParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern IntPtr LoadIcon(
            IntPtr hInstance, int lpIconName);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsZoomed(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(
            IntPtr hWnd,
            IntPtr hWndAfter,
            int x,
            int y,
            int cx,
            int cy,
            uint flags);

        [DllImport("gdi32.dll")]
        public static extern int CreateRoundRectRgn(
            int x1, int y1, int x2, int y2, int x3, int y3);

        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(
            IntPtr hWnd, IntPtr hRgn, bool bRedraw);

        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(
            IntPtr hwnd, int hRgn, Boolean bRedraw);

        [DllImport("user32.dll",
            CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool InvalidateRect(
            IntPtr hWnd, ref RECT rect, bool erase);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern bool GetClientRect(
            IntPtr hWnd, ref RECT r);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowRect(
            IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        public extern static int OffsetRect(
            ref RECT lpRect, int x, int y);

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(
            IntPtr hwnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLongPtr(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(
            IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
        public static extern int GetWindowLong(HandleRef hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        public static extern IntPtr SetWindowLongPtr(
            IntPtr hwnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern bool ScreenToClient(IntPtr hWnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr handle);

        [DllImport("USER32.dll")]
        public static extern IntPtr GetDCEx(IntPtr hWnd, IntPtr hrgnClip, int flags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr handle);

        [DllImport("user32.dll")]
        public static extern int ReleaseDC(IntPtr handle, IntPtr hdc);

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern bool TrackMouseEvent(
            ref TRACKMOUSEEVENT lpEventTrack);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PtInRect(ref RECT lprc, Point pt);

        [DllImport("user32.dll")]
        public static extern bool EqualRect(
            [In] ref RECT lprc1, [In] ref RECT lprc2);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr SetTimer(
            IntPtr hWnd,
            int nIDEvent,
            uint uElapse,
            IntPtr lpTimerFunc);

        [DllImport("user32.dll", ExactSpelling = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool KillTimer(
            IntPtr hWnd, uint uIDEvent);

        [DllImport("user32.dll")]
        public static extern int SetFocus(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool PostMessage(
            IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, int wParam, int lParam);

        //[DllImport("User32.dll",
        //    CharSet = CharSet.Auto,
        //    PreserveSig = false)]
        //public static extern IRichEditOle SendMessage(
        //    IntPtr hWnd, int message, int wParam);

        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, int wParam, ref TOOLINFO lParam);

        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, int wParam, ref RECT lParam);

        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd,
            int msg,
            IntPtr wParam,
            [MarshalAs(UnmanagedType.LPTStr)]string lParam);

        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, IntPtr wParam, ref NMHDR lParam);

        [DllImport("user32.dll")]
        public extern static int SendMessage(
            IntPtr hWnd, int msg, IntPtr wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(
            IntPtr hWnd, int msg, int wParam, ref SCROLLBARINFO lParam);

        [DllImport("user32.dll")]
        public static extern short GetKeyState(int nVirtKey);

        [DllImport("user32.dll")]
        public static extern bool ValidateRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        public static extern int GetScrollBarInfo(
            IntPtr hWnd, uint idObject, ref SCROLLBARINFO psbi);

        [DllImport("user32.dll")]
        public static extern bool GetScrollInfo(
            IntPtr hwnd, int fnBar, ref SCROLLINFO scrollInfo);

        [DllImport("user32.dll")]
        public static extern bool EnableScrollBar(
            IntPtr hWnd, int wSBflags, int wArrows);

        public static IntPtr GetClassLongPtr(IntPtr hWnd, int nIndex) {
            if (IntPtr.Size == 8) {
                return GetClassLongPtr64(hWnd, nIndex);
            } else {
                return GetClassLongPtr32(hWnd, nIndex);
            }
        }

        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        private static extern IntPtr GetClassLongPtr32(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        private static extern IntPtr GetClassLongPtr64(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DrawIconEx(IntPtr hdc, int xLeft, int yTop, IntPtr hIcon,
           int cxWidth, int cyHeight, int istepIfAniCur, IntPtr hbrFlickerFreeDraw,
           int diFlags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern bool UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst,
            ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pprSrc,
            Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("USER32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SetWindowsHookEx(
            int idHook, HookProc lpfn, int hMod, int dwThreadId);

        [DllImport("USER32.dll", CharSet = CharSet.Auto)]
        public static extern int CallNextHookEx(
            IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("USER32.dll", CharSet = CharSet.Auto)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        #endregion

        #region GDI32.DLL
        /// <summary>  
        /// 在指定的设备场景中取得一个像素的RGB值  
        /// </summary>  
        /// <param name="hdc">一个设备场景的句柄</param>  
        /// <param name="nXPos">逻辑坐标中要检查的横坐标</param>  
        /// <param name="nYPos">逻辑坐标中要检查的纵坐标</param>  
        /// <returns>指定点的颜色</returns>  
        [DllImport("gdi32.dll")]
        public extern static uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("gdi32.dll")]
        public extern static int ExcludeClipRect(
            IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        [DllImport("gdi32.dll", EntryPoint = "GdiAlphaBlend")]
        public static extern bool AlphaBlend(
            IntPtr hdcDest,
            int nXOriginDest,
            int nYOriginDest,
            int nWidthDest,
            int nHeightDest,
            IntPtr hdcSrc,
            int nXOriginSrc,
            int nYOriginSrc,
            int nWidthSrc,
            int nHeightSrc,
            BLENDFUNCTION blendFunction);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool StretchBlt(
            IntPtr hDest,
            int X,
            int Y,
            int nWidth,
            int nHeight,
            IntPtr hdcSrc,
            int sX,
            int sY,
            int nWidthSrc,
            int nHeightSrc,
            int dwRop);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BitBlt(
            IntPtr hdc,
            int nXDest,
            int nYDest,
            int nWidth,
            int nHeight,
            IntPtr hdcSrc,
            int nXSrc,
            int nYSrc,
            int dwRop);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDCA(
            [MarshalAs(UnmanagedType.LPStr)]string lpszDriver,
            [MarshalAs(UnmanagedType.LPStr)]string lpszDevice,
            [MarshalAs(UnmanagedType.LPStr)]string lpszOutput,
            int lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDCW(
            [MarshalAs(UnmanagedType.LPWStr)]string lpszDriver,
            [MarshalAs(UnmanagedType.LPWStr)]string lpszDevice,
            [MarshalAs(UnmanagedType.LPWStr)]string lpszOutput,
            int lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateDC(
            string lpszDriver,
            string lpszDevice,
            string lpszOutput,
            int lpInitData);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(
            IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true)]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject(IntPtr hObject);

        [DllImport("gdi32.dll")]
        public static extern uint SetPixel(IntPtr hdc, int X, int Y, int crColor);

        [DllImport("gdi32.dll",
           CharSet = CharSet.Auto,
           SetLastError = true,
           ExactSpelling = true)]
        public static extern IntPtr CreateRectRgn(int x1, int y1, int x2, int y2);

        [DllImport("gdi32.dll",
            SetLastError = true,
            ExactSpelling = true)]
        public static extern int CombineRgn(
            IntPtr hRgn, IntPtr hRgn1, IntPtr hRgn2, int nCombineMode);

        #endregion

        #region comctl32.dll
        [DllImport("comctl32.dll", SetLastError = true)]
        public static extern IntPtr ImageList_GetIcon(
            IntPtr himl, int i, int flags);

        [StructLayout(LayoutKind.Sequential)]
        public struct HDITEM
        {
            public int mask;
            public int cxy;
            public string pszText;
            public IntPtr hbm;
            public int cchTextMax;
            public int fmt;
            public IntPtr lParam;
            public int iImage;
            public int iOrder;
            public uint type;
            public IntPtr pvFilter;
        }

        [DllImport("comctl32.dll",
            CallingConvention = CallingConvention.StdCall)]
        public static extern bool InitCommonControlsEx(
            ref INITCOMMONCONTROLSEX iccex);
        #endregion

        #region kernel32.dll
        [DllImport("kernel32.dll", ExactSpelling = true, CharSet = CharSet.Auto)]
        public static extern int GetCurrentThreadId();

        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            ref NMHDR destination, IntPtr source, int length);

        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            ref NMTTDISPINFO destination, IntPtr source, int length);

        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            IntPtr destination, ref NMTTDISPINFO Source, int length);

        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            ref POINT destination, ref RECT Source, int length);

        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            ref NMTTCUSTOMDRAW destination, IntPtr Source, int length);

        [DllImport("kernel32.dll")]
        public extern static int RtlMoveMemory(
            ref NMCUSTOMDRAW destination, IntPtr Source, int length);

        [DllImport("kernel32.dll")]
        public static extern int MulDiv(int nNumber, int nNumerator, int nDenominator);

        #endregion

        #region uxtheme.dll

        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(
            IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        [DllImport("uxtheme.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool IsAppThemed();

        #endregion

        #region shell32.dll

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int FindExecutable(string filename, string directory, ref string result);

        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern int ShellExecute(IntPtr hwnd, string lpOperation, string lpFile,
            string lpParameters, string lpDirectory, int nShowCmd);

        [DllImport("kernel32.dll")]
        public static extern int WinExec(string lpCmdLine, int nCmdShow);

        #endregion

        #region ole32.dll
        //[DllImport("ole32.dll")]
        //public static extern int CreateILockBytesOnHGlobal(
        //    IntPtr hGlobal, bool fDeleteOnRelease, out ILockBytes ppLkbyt);

        //[DllImport("ole32.dll")]
        //public static extern int StgCreateDocfileOnILockBytes(
        //    ILockBytes plkbyt, uint grfMode, uint reserved, out IStorage ppstgOpen);

        //[DllImport("ole32.dll")]
        //public static extern int OleCreateFromFile([In] ref Guid rclsid,
        //    [MarshalAs(UnmanagedType.LPWStr)] string lpszFileName,
        //    [In] ref Guid riid,
        //    uint renderopt,
        //    ref FORMATETC pFormatEtc,
        //    IOleClientSite pClientSite,
        //    IStorage pStg,
        //    [MarshalAs(UnmanagedType.IUnknown)] out object ppvObj);

        [DllImport("ole32.dll")]
        public static extern int OleSetContainedObject(
            [MarshalAs(UnmanagedType.IUnknown)] object pUnk, bool fContained);
        #endregion

        #region ComboBoxButtonState
        public enum ComboBoxButtonState
        {
            STATE_SYSTEM_NONE = 0,
            STATE_SYSTEM_INVISIBLE = 0x00008000,
            STATE_SYSTEM_PRESSED = 0x00000008
        }
        #endregion

        #region ComboBoxInfo Struct
        [StructLayout(LayoutKind.Sequential)]
        public struct ComboBoxInfo
        {
            public int cbSize;
            public RECT rcItem;
            public RECT rcButton;
            public ComboBoxButtonState stateButton;
            public IntPtr hwndCombo;
            public IntPtr hwndEdit;
            public IntPtr hwndList;
        }
        #endregion
    }
}
