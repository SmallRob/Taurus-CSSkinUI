
namespace Com_CSSkin.SkinControl
{
    #region Directives
    using System;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Drawing;
    using System.Reflection;
    using System.ComponentModel;
    using Com_CSSkin.SkinClass;
    using Com_CSSkin.Imaging;
    #endregion

    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class cScrollBar : NativeWindow, IDisposable
    {
        #region Constants
        // showwindow
        private const int SW_HIDE = 0x0;
        private const int SW_NORMAL = 0x1;
        // window styles
        private const int GWL_STYLE = (-16);
        private const int GWL_EXSTYLE = (-20);
        private const int WS_EX_TOPMOST = 0x8;
        private const int WS_EX_TOOLWINDOW = 0x80;
        private const int WS_CHILD = 0x40000000;
        private const int WS_OVERLAPPED = 0x0;
        private const int WS_CLIPCHILDREN = 0x2000000;
        private const int WS_CLIPSIBLINGS = 0x4000000;
        private const int WS_VISIBLE = 0x10000000;
        private const int SS_OWNERDRAW = 0xD;
        // size/move
        private const uint SWP_NOSIZE = 0x0001;
        private const uint SWP_NOMOVE = 0x0002;
        private const uint SWP_NOZORDER = 0x0004;
        private const uint SWP_NOREDRAW = 0x0008;
        private const uint SWP_NOACTIVATE = 0x0010;
        private const uint SWP_FRAMECHANGED = 0x0020;
        private const uint SWP_SHOWWINDOW = 0x0040;
        private const uint SWP_HIDEWINDOW = 0x0080;
        private const uint SWP_NOCOPYBITS = 0x0100;
        private const uint SWP_NOOWNERZORDER = 0x0200;
        private const uint SWP_NOSENDCHANGING = 0x0400;
        // setwindowpos
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        // scroll messages
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;
        private const int SB_LINEUP = 0;
        private const int SB_LINEDOWN = 1;
        private const int SB_LINELEFT = 0;
        private const int SB_LINERIGHT = 1;
        private const int SB_PAGEUP = 2;
        private const int SB_PAGEDOWN = 3;
        private const int SB_PAGELEFT = 2;
        private const int SB_PAGERIGHT = 3;
        // mouse buttons
        private const int VK_LBUTTON = 0x1;
        private const int VK_RBUTTON = 0x2;
        // redraw
        private const int RDW_INVALIDATE = 0x0001;
        private const int RDW_INTERNALPAINT = 0x0002;
        private const int RDW_ERASE = 0x0004;
        private const int RDW_VALIDATE = 0x0008;
        private const int RDW_NOINTERNALPAINT = 0x0010;
        private const int RDW_NOERASE = 0x0020;
        private const int RDW_NOCHILDREN = 0x0040;
        private const int RDW_ALLCHILDREN = 0x0080;
        private const int RDW_UPDATENOW = 0x0100;
        private const int RDW_ERASENOW = 0x0200;
        private const int RDW_FRAME = 0x0400;
        private const int RDW_NOFRAME = 0x0800;
        // scroll bar messages
        private const int SB_HORZ = 0x0;
        private const int SB_VERT = 0x1;
        private const int SBM_SETPOS = 0x00E0;
        private const int SBM_GETPOS = 0x00E1;
        private const int SBM_SETRANGE = 0x00E2;
        private const int SBM_SETRANGEREDRAW = 0x00E6;
        private const int SBM_GETRANGE = 0x00E3;
        private const int SBM_ENABLE_ARROWS = 0x00E4;
        private const int SBM_SETSCROLLINFO = 0x00E9;
        private const int SBM_GETSCROLLINFO = 0x00EA;
        private const int SBM_GETSCROLLBARINFO = 0x00EB;
        private const int SIF_RANGE = 0x0001;
        private const int SIF_PAGE = 0x0002;
        private const int SIF_POS = 0x0004;
        private const int SIF_DISABLENOSCROLL = 0x0008;
        private const int SIF_TRACKPOS = 0x0010;
        private const int SIF_ALL = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS);
        // scrollbar states
        private const int STATE_SYSTEM_INVISIBLE = 0x00008000;
        private const int STATE_SYSTEM_OFFSCREEN = 0x00010000;
        private const int STATE_SYSTEM_PRESSED = 0x00000008;
        private const int STATE_SYSTEM_UNAVAILABLE = 0x00000001;
        private const uint OBJID_HSCROLL = 0xFFFFFFFA;
        private const uint OBJID_VSCROLL = 0xFFFFFFFB;
        private const uint OBJID_CLIENT = 0xFFFFFFFC;
        // window messages
        private const int WM_PAINT = 0xF;
        private const int WM_NCPAINT = 0x85;//scm
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_MOUSELEAVE = 0x2A3;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_MBUTTONDBLCLK = 0x209;
        private const int WM_MOUSEWHEEL = 0x20A;
        private const int WM_SIZE = 0x5;
        private const int WM_MOVE = 0x3;
        private const int WM_STYLECHANGED = 0x007D;//scm
        private const int WM_WINDOWPOSCHANGED = 0x0047;

        // message handler
        private static IntPtr MSG_HANDLED = new IntPtr(1);
        #endregion

        #region Enums
        private enum SB_HITEST : int
        {
            offControl = 0,
            topArrow,
            bottomArrow,
            leftArrow,
            rightArrow,
            button,
            track
        }

        private enum SYSTEM_METRICS : int
        {
            SM_CXSCREEN = 0,
            SM_CYSCREEN = 1,
            SM_CXVSCROLL = 2,
            SM_CYHSCROLL = 3,
            SM_CYCAPTION = 4,
            SM_CXBORDER = 5,
            SM_CYBORDER = 6,
            SM_CYVTHUMB = 9,
            SM_CXHTHUMB = 10,
            SM_CXICON = 11,
            SM_CYICON = 12,
            SM_CXCURSOR = 13,
            SM_CYCURSOR = 14,
            SM_CYMENU = 15,
            SM_CXFULLSCREEN = 16,
            SM_CYFULLSCREEN = 17,
            SM_CYKANJIWINDOW = 18,
            SM_MOUSEPRESENT = 19,
            SM_CYVSCROLL = 20,
            SM_CXHSCROLL = 21,
            SM_SWAPBUTTON = 23,
            SM_CXMIN = 28,
            SM_CYMIN = 29,
            SM_CXSIZE = 30,
            SM_CYSIZE = 31,
            SM_CXFRAME = 32,
            SM_CYFRAME = 33,
            SM_CXMINTRACK = 34,
            SM_CYMINTRACK = 35,
            SM_CYSMCAPTION = 51,
            SM_CXMINIMIZED = 57,
            SM_CYMINIMIZED = 58,
            SM_CXMAXTRACK = 59,
            SM_CYMAXTRACK = 60,
            SM_CXMAXIMIZED = 61,
            SM_CYMAXIMIZED = 62
        }
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Sequential)]
        private struct PAINTSTRUCT
        {
            internal IntPtr hdc;
            internal int fErase;
            internal RECT rcPaint;
            internal int fRestore;
            internal int fIncUpdate;
            internal int Reserved1;
            internal int Reserved2;
            internal int Reserved3;
            internal int Reserved4;
            internal int Reserved5;
            internal int Reserved6;
            internal int Reserved7;
            internal int Reserved8;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            internal RECT(int X, int Y, int Width, int Height) {
                this.Left = X;
                this.Top = Y;
                this.Right = Width;
                this.Bottom = Height;
            }
            internal int Left;
            internal int Top;
            internal int Right;
            internal int Bottom;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SCROLLINFO
        {
            internal uint cbSize;
            internal uint fMask;
            internal int nMin;
            internal int nMax;
            internal uint nPage;
            internal int nPos;
            internal int nTrackPos;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct SCROLLBARINFO
        {
            internal int cbSize;
            internal RECT rcScrollBar;
            internal int dxyLineButton;
            internal int xyThumbTop;
            internal int xyThumbBottom;
            internal int reserved;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            internal int[] rgstate;
        }

        #endregion

        #region API
        [DllImport("user32.dll")]
        private static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool StretchBlt(IntPtr hDest, int X, int Y, int nWidth, int nHeight, IntPtr hdcSrc,
        int sX, int sY, int nWidthSrc, int nHeightSrc, int dwRop);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr handle);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr handle, IntPtr hdc);

        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, ref SCROLLBARINFO lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        private extern static int OffsetRect(ref RECT lpRect, int x, int y);

        [DllImport("user32.dll")]
        private static extern bool ValidateRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(SYSTEM_METRICS smIndex);

        [DllImport("uxtheme.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private extern static bool IsAppThemed();

        [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, String pszSubAppName, String pszSubIdList);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PtInRect([In] ref RECT lprc, Point pt);

        [DllImport("user32.dll")]
        private static extern int ScreenToClient(IntPtr hwnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        private static extern short GetKeyState(int nVirtKey);

        [DllImport("user32.dll")]
        private static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        [DllImport("user32.dll")]
        private static extern int GetScrollPos(IntPtr hWnd, int nBar);

        [DllImport("user32.dll")]
        private static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CreateWindowEx(int exstyle, string lpClassName, string lpWindowName, int dwStyle,
            int x, int y, int nWidth, int nHeight, IntPtr hwndParent, IntPtr Menu, IntPtr hInstance, IntPtr lpParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DestroyWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndAfter, int x, int y, int cx, int cy, uint flags);

        [DllImport("user32.dll")]
        static extern bool EqualRect([In] ref RECT lprc1, [In] ref RECT lprc2);

        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        #endregion

        #region Fields
        private bool _bPainting = false;
        //private bool _bMoved = false;
        //private bool _bFading = false;
        private IntPtr _hMaskWnd = IntPtr.Zero;
        private int _iArrowCx = 14;
        private int _iArrowCy = 14;
        private Orientation _eScrollbarOrientation;
        private IntPtr _hScrollBarWnd = IntPtr.Zero;
        private cStoreDc _cArrowDc = new cStoreDc();
        private cStoreDc _cThumbDc = new cStoreDc();
        private cStoreDc _cTrackDc = new cStoreDc();
        private Bitmap _oArrowBitmap;
        private Bitmap _oThumbBitmap;
        private Bitmap _oTrackBitmap;
        private Bitmap _oMask;
        #endregion

        #region Constructor
        public cScrollBar(IntPtr hWnd, Orientation orientation, Bitmap thumb, Bitmap track, Bitmap arrow, Bitmap fader) {
            if (hWnd == IntPtr.Zero)
                throw new Exception("The scrollbar handle is invalid.");
            ArrowGraphic = arrow;
            ThumbGraphic = thumb;
            TrackGraphic = track;
            _hScrollBarWnd = hWnd;
            Direction = orientation;
            scrollbarMetrics();
            if (Environment.OSVersion.Version.Major > 5) {
                if (IsAppThemed())
                    SetWindowTheme(_hScrollBarWnd, "", "");
            }
            createScrollBarMask();
            //ScrollBar sc = (ScrollBar)Control.FromHandle(_hScrollBarWnd);//scm
            //sc.Scroll += new ScrollEventHandler(sc_Scroll);//scm
            //sc.MouseEnter += new EventHandler(sc_MouseEnter);//scm
            //Control ct = Control.FromHandle(GetParent(_hScrollBarWnd));//scm
            //ct.Paint += new PaintEventHandler(ct_Paint);//scm
            this.AssignHandle(hWnd);
            if (fader != null)
                TransitionGraphic = fader;
            checkBarState();//scm
        }

        public cScrollBar(IntPtr hWnd, Orientation orientation) {
            if (hWnd == IntPtr.Zero)
                throw new Exception("The scrollbar handle is invalid.");
            _hScrollBarWnd = hWnd;
            Direction = orientation;
            scrollbarMetrics();
            if (Environment.OSVersion.Version.Major > 5) {
                if (IsAppThemed())
                    SetWindowTheme(_hScrollBarWnd, "", "");
            }
            createScrollBarMask();
            //ScrollBar sc = (ScrollBar)Control.FromHandle(_hScrollBarWnd);//scm
            //sc.Scroll += new ScrollEventHandler(sc_Scroll);//scm
            //sc.MouseEnter += new EventHandler(sc_MouseEnter);//scm
            //Control ct = Control.FromHandle(GetParent(_hScrollBarWnd));//scm
            //ct.Paint += new PaintEventHandler(ct_Paint);//scm
            this.AssignHandle(hWnd);
            checkBarState();//scm
        }
        void sc_MouseEnter(object sender, EventArgs e) {
            scrollFader();
        }

        public void Dispose() {
            try {
                this.ReleaseHandle();
                if (_oArrowBitmap != null) _oArrowBitmap.Dispose();
                if (_cArrowDc != null) _cArrowDc.Dispose();
                if (_oThumbBitmap != null) _oThumbBitmap.Dispose();
                if (_cThumbDc != null) _cThumbDc.Dispose();
                if (_oTrackBitmap != null) _oTrackBitmap.Dispose();
                if (_cTrackDc != null) _cTrackDc.Dispose();
                if (_hMaskWnd != IntPtr.Zero) DestroyWindow(_hMaskWnd);
                //if (_oMask != null) _oMask.Dispose();
            } catch { }
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Events
        private void ct_Paint(object sender, PaintEventArgs e) {
            invalidateWindow(false);
        }

        private void sc_Scroll(object sender, ScrollEventArgs e) {
            invalidateWindow(false);
        }
        #endregion

        #region Properties
        private Bitmap ArrowGraphic {
            get { return _oArrowBitmap; }
            set {
                _oArrowBitmap = value;
                if (_cArrowDc.Hdc != IntPtr.Zero) {
                    _cArrowDc.Dispose();
                    _cArrowDc = new cStoreDc();
                }
                _cArrowDc.Width = _oArrowBitmap.Width;
                _cArrowDc.Height = _oArrowBitmap.Height;
                SelectObject(_cArrowDc.Hdc, _oArrowBitmap.GetHbitmap());

            }
        }

        private Bitmap ThumbGraphic {
            get { return _oThumbBitmap; }
            set {
                _oThumbBitmap = value;
                if (_cThumbDc.Hdc != IntPtr.Zero) {
                    _cThumbDc.Dispose();
                    _cThumbDc = new cStoreDc();
                }
                _cThumbDc.Width = _oThumbBitmap.Width;
                _cThumbDc.Height = _oThumbBitmap.Height;
                SelectObject(_cThumbDc.Hdc, _oThumbBitmap.GetHbitmap());

            }
        }

        private Bitmap TrackGraphic {
            get { return _oTrackBitmap; }
            set {
                _oTrackBitmap = value;
                if (_cTrackDc.Hdc != IntPtr.Zero) {
                    _cTrackDc.Dispose();
                    _cTrackDc = new cStoreDc();
                }
                _cTrackDc.Width = _oTrackBitmap.Width;
                _cTrackDc.Height = _oTrackBitmap.Height;
                SelectObject(_cTrackDc.Hdc, _oTrackBitmap.GetHbitmap());

            }
        }

        private Bitmap TransitionGraphic {
            get { return _oMask; }
            set { _oMask = value; }
        }

        private Orientation Direction {
            get { return _eScrollbarOrientation; }
            set { _eScrollbarOrientation = value; }
        }

        private int HScrollPos {
            get { return GetScrollPos((IntPtr)this.Handle, SB_HORZ); }
            set { SetScrollPos((IntPtr)this.Handle, SB_HORZ, value, true); }
        }

        private int VScrollPos {
            get { return GetScrollPos((IntPtr)this.Handle, SB_VERT); }
            set { SetScrollPos((IntPtr)this.Handle, SB_VERT, value, true); }
        }
        #endregion

        #region Methods
        private void checkBarState() {
            if ((GetWindowLong(_hScrollBarWnd, GWL_STYLE) & WS_VISIBLE) == WS_VISIBLE)
                ShowWindow(_hMaskWnd, SW_NORMAL);
            else
                ShowWindow(_hMaskWnd, SW_HIDE);
        }

        private void createScrollBarMask() {
            Type t = typeof(cScrollBar);
            Module m = t.Module;
            IntPtr hInstance = Marshal.GetHINSTANCE(m);
            IntPtr hParent = GetParent(_hScrollBarWnd);
            RECT tr = new RECT();
            Point pt = new Point();

            GetWindowRect(_hScrollBarWnd, ref tr);
            pt.X = tr.Left;
            pt.Y = tr.Top;
            ScreenToClient(hParent, ref pt);

            _hMaskWnd = CreateWindowEx(WS_EX_TOPMOST | WS_EX_TOOLWINDOW,
                "STATIC", "",
                SS_OWNERDRAW | WS_CHILD | WS_CLIPSIBLINGS | WS_OVERLAPPED | WS_VISIBLE,
                pt.X, pt.Y,
                (tr.Right - tr.Left), (tr.Bottom - tr.Top),
                hParent,
                IntPtr.Zero, hInstance, IntPtr.Zero);

            // set z-order
            SetWindowPos(_hMaskWnd, HWND_TOP,
                0, 0,
                0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_NOOWNERZORDER);
        }
        //#region scm

        private void drawScrollBar() {
            if (ThumbGraphic == null || TrackGraphic == null || ArrowGraphic == null) {
                //drawScrollBar_Graphics();
            } else {
                drawScrollBar_Image();
            }
        }

        private void drawScrollBar_Image() {

            SCROLLBARINFO sbi = new SCROLLBARINFO();
            RECT tr = new RECT();
            cStoreDc tempDc = new cStoreDc();
            int offset = 0;
            int width = 0;
            int section = 0;

            GetWindowRect(_hScrollBarWnd, ref tr);
            OffsetRect(ref tr, -tr.Left, -tr.Top);
            tempDc.Width = tr.Right;
            tempDc.Height = tr.Bottom;
            SB_HITEST hitTest = scrollbarHitTest();

            sbi.cbSize = Marshal.SizeOf(sbi);
            SendMessage(_hScrollBarWnd, SBM_GETSCROLLBARINFO, 0, ref sbi);

            if (Direction == Orientation.Horizontal) {
                // draw the track
                using (StretchImage si = new StretchImage(_cTrackDc.Hdc, tempDc.Hdc,
                    new Rectangle(0, 0, _cTrackDc.Width, _cTrackDc.Height),
                    new Rectangle(_iArrowCx, 0, tr.Right - (2 * _iArrowCx), tr.Bottom), 2, StretchModeEnum.STRETCH_HALFTONE)) { }
                // draw the arrows
                section = 7;
                width = _cArrowDc.Width / section;
                // left arrow
                if (hitTest == SB_HITEST.leftArrow) {
                    if (leftKeyPressed())
                        offset = 2;
                    else
                        offset = 1;
                } else {
                    offset = 0;
                }
                using (StretchImage si = new StretchImage(_cArrowDc.Hdc, tempDc.Hdc,
                    new Rectangle(offset * width, 0, width, _cArrowDc.Height),
                    new Rectangle(0, 0, _iArrowCx, tr.Bottom), 2, StretchModeEnum.STRETCH_HALFTONE)) { }

                // right arrow
                if (hitTest == SB_HITEST.rightArrow) {
                    if (leftKeyPressed())
                        offset = 5;
                    else
                        offset = 4;
                } else {
                    offset = 3;
                }
                using (StretchImage si = new StretchImage(_cArrowDc.Hdc, tempDc.Hdc,
                    new Rectangle(offset * width, 0, width, _cArrowDc.Height),
                    new Rectangle(tr.Right - _iArrowCx, 0, _iArrowCx, tr.Bottom), 2, StretchModeEnum.STRETCH_HALFTONE)) { }

                // draw the thumb
                section = 3;
                width = _cThumbDc.Width / section;
                if (hitTest == SB_HITEST.button) {
                    if (leftKeyPressed())
                        offset = 2;
                    else
                        offset = 1;
                } else {
                    offset = 0;
                }
                Point pst = getScrollBarThumb();
                using (StretchImage si = new StretchImage(_cThumbDc.Hdc, tempDc.Hdc,
                    new Rectangle(offset * width, 0, width, _cThumbDc.Height),
                    new Rectangle(pst.X, 0, pst.Y - pst.X, tr.Bottom), 2, StretchModeEnum.STRETCH_HALFTONE)) { }
            } else {


                // draw the track
                using (StretchImage si = new StretchImage(_cTrackDc.Hdc, tempDc.Hdc,
                    new Rectangle(0, 0, _cTrackDc.Width, _cTrackDc.Height),
                    new Rectangle(0, _iArrowCy, tr.Right, tr.Bottom - (2 * _iArrowCy)), 2, StretchModeEnum.STRETCH_HALFTONE)) { }
                section = 6;
                width = _cArrowDc.Width / section;

                // top arrow
                if (hitTest == SB_HITEST.topArrow) {
                    if (leftKeyPressed())
                        offset = 2;
                    else
                        offset = 1;
                } else {
                    offset = 0;
                }
                using (StretchImage si = new StretchImage(_cArrowDc.Hdc, tempDc.Hdc,
                    new Rectangle(offset * width, 0, width, _cArrowDc.Height),
                    new Rectangle(0, 0, tr.Right, _iArrowCy), 2, StretchModeEnum.STRETCH_HALFTONE)) { }

                // bottom arrow
                if (hitTest == SB_HITEST.bottomArrow) {
                    if (leftKeyPressed())
                        offset = 5;
                    else
                        offset = 4;
                } else {
                    offset = 3;
                }
                using (StretchImage si = new StretchImage(_cArrowDc.Hdc, tempDc.Hdc,
                    new Rectangle(offset * width, 0, width, _cArrowDc.Height),
                    new Rectangle(0, tr.Bottom - _iArrowCy, tr.Right, _iArrowCy), 2, StretchModeEnum.STRETCH_HALFTONE)) { }

                // draw the thumb
                section = 3;
                width = _cThumbDc.Width / section;
                if (hitTest == SB_HITEST.button) {
                    if (leftKeyPressed())
                        offset = 2;
                    else
                        offset = 1;
                } else {
                    offset = 0;
                }
                Point pst = getScrollBarThumb();
                using (StretchImage si = new StretchImage(_cThumbDc.Hdc, tempDc.Hdc,
                    new Rectangle(offset * width, 0, width, _cThumbDc.Height),
                    new Rectangle(0, pst.X, _iArrowCx, pst.Y - pst.X), 2, StretchModeEnum.STRETCH_HALFTONE)) { }
            }
            IntPtr hdc = GetDC(_hMaskWnd);
            BitBlt(hdc, 0, 0, tr.Right, tr.Bottom, tempDc.Hdc, 0, 0, 0xCC0020);
            ReleaseDC(_hMaskWnd, hdc);
            tempDc.Dispose();
        }

        ////scm
        //private void drawScrollBar_Graphics() {
        //    SCROLLBARINFO sbi = new SCROLLBARINFO();
        //    RECT tr = new RECT();
        //    cStoreDc tempDc = new cStoreDc();

        //    GetWindowRect(_hScrollBarWnd, ref tr);
        //    OffsetRect(ref tr, -tr.Left, -tr.Top);
        //    tempDc.Width = tr.Right;
        //    tempDc.Height = tr.Bottom;
        //    SB_HITEST hitTest = scrollbarHitTest();

        //    sbi.cbSize = Marshal.SizeOf(sbi);
        //    SendMessage(_hScrollBarWnd, SBM_GETSCROLLBARINFO, 0, ref sbi);

        //    if (Direction == Orientation.Horizontal) {
        //        using (Graphics g = Graphics.FromHdc(tempDc.Hdc)) {
        //            // draw the track
        //            using (PaintScrollBarTrackEventArgs te =
        //                new PaintScrollBarTrackEventArgs(
        //                g,
        //                new Rectangle(_iArrowCx, 0, tr.Right - (2 * _iArrowCx), tr.Bottom),//trackRect
        //                Direction,
        //                true)) {
        //                PaintScrollBarTrack(te);
        //            }
        //            // draw the arrows
        //            // left arrow
        //            ControlState topLeftArrowState = ControlState.Normal;
        //            if (hitTest == SB_HITEST.leftArrow) {
        //                if (leftKeyPressed())
        //                    topLeftArrowState = ControlState.Pressed;
        //                else
        //                    topLeftArrowState = ControlState.Hover;
        //            }
        //            ArrowDirection arrowDirection = Direction == Orientation.Horizontal ? ArrowDirection.Left : ArrowDirection.Up;
        //            using (PaintScrollBarArrowEventArgs te =
        //                new PaintScrollBarArrowEventArgs(
        //                g,
        //                new Rectangle(0, 0, _iArrowCx, tr.Bottom),
        //                topLeftArrowState,
        //                arrowDirection,
        //                Direction,
        //                true)) {
        //                PaintScrollBarArrow(te);
        //            }
        //            // right arrow
        //            ControlState bottomRightArrowState = ControlState.Normal;
        //            if (hitTest == SB_HITEST.rightArrow) {
        //                if (leftKeyPressed())
        //                    bottomRightArrowState = ControlState.Pressed;
        //                else
        //                    bottomRightArrowState = ControlState.Hover;
        //            }
        //            arrowDirection = Direction == Orientation.Horizontal ? ArrowDirection.Right : ArrowDirection.Down;

        //            using (PaintScrollBarArrowEventArgs te =
        //                new PaintScrollBarArrowEventArgs(
        //                g,
        //                new Rectangle(tr.Right - _iArrowCx, 0, _iArrowCx, tr.Bottom),
        //                bottomRightArrowState,
        //                arrowDirection,
        //                Direction,
        //                true)) {
        //                PaintScrollBarArrow(te);
        //            }
        //            // draw the thumb
        //            ControlState thumbState = ControlState.Normal;
        //            if (hitTest == SB_HITEST.button) {
        //                if (leftKeyPressed())
        //                    thumbState = ControlState.Pressed;
        //                else
        //                    thumbState = ControlState.Hover;
        //            }
        //            Point pst = getScrollBarThumb();
        //            using (PaintScrollBarThumbEventArgs te =
        //                new PaintScrollBarThumbEventArgs(
        //                g,
        //                new Rectangle(pst.X, 0, pst.Y - pst.X, tr.Bottom),
        //                thumbState,
        //                Direction,
        //                true)) {
        //                PaintScrollBarThumb(te);
        //            }
        //        }
        //    } else {
        //        using (Graphics g = Graphics.FromHdc(tempDc.Hdc)) {
        //            // draw the track
        //            using (PaintScrollBarTrackEventArgs te =
        //                new PaintScrollBarTrackEventArgs(
        //                g,
        //                new Rectangle(0, _iArrowCy, tr.Right, tr.Bottom - (2 * _iArrowCy)),//trackRect
        //                Direction,
        //                true)) {
        //                PaintScrollBarTrack(te);
        //            }
        //            // draw the arrows
        //            // top arrow
        //            ControlState topLeftArrowState = ControlState.Normal;
        //            if (hitTest == SB_HITEST.topArrow) {
        //                if (leftKeyPressed())
        //                    topLeftArrowState = ControlState.Pressed;
        //                else
        //                    topLeftArrowState = ControlState.Hover;
        //            }
        //            ArrowDirection arrowDirection = Direction == Orientation.Horizontal ? ArrowDirection.Left : ArrowDirection.Up;
        //            using (PaintScrollBarArrowEventArgs te =
        //                new PaintScrollBarArrowEventArgs(
        //                g,
        //                new Rectangle(0, 0, tr.Right, _iArrowCy),
        //                topLeftArrowState,
        //                arrowDirection,
        //                Direction,
        //                true)) {
        //                PaintScrollBarArrow(te);
        //            }
        //            // bottom arrow
        //            ControlState bottomRightArrowState = ControlState.Normal;
        //            if (hitTest == SB_HITEST.bottomArrow) {
        //                if (leftKeyPressed())
        //                    bottomRightArrowState = ControlState.Pressed;
        //                else
        //                    bottomRightArrowState = ControlState.Hover;
        //            }
        //            arrowDirection = Direction == Orientation.Horizontal ? ArrowDirection.Right : ArrowDirection.Down;

        //            using (PaintScrollBarArrowEventArgs te =
        //                new PaintScrollBarArrowEventArgs(
        //                g,
        //                new Rectangle(0, tr.Bottom - _iArrowCy, tr.Right, _iArrowCy),
        //                bottomRightArrowState,
        //                arrowDirection,
        //                Direction,
        //                true)) {
        //                PaintScrollBarArrow(te);
        //            }
        //            // draw the thumb
        //            ControlState thumbState = ControlState.Normal;
        //            if (hitTest == SB_HITEST.button) {
        //                if (leftKeyPressed())
        //                    thumbState = ControlState.Pressed;
        //                else
        //                    thumbState = ControlState.Hover;
        //            }
        //            Point pst = getScrollBarThumb();
        //            using (PaintScrollBarThumbEventArgs te =
        //                new PaintScrollBarThumbEventArgs(
        //                g,
        //                new Rectangle(0, pst.X, _iArrowCx, pst.Y - pst.X),
        //                thumbState,
        //                Direction,
        //                true)) {
        //                PaintScrollBarThumb(te);
        //            }
        //        }
        //    }
        //    IntPtr hdc = GetDC(_hMaskWnd);
        //    BitBlt(hdc, 0, 0, tr.Right, tr.Bottom, tempDc.Hdc, 0, 0, 0xCC0020);
        //    ReleaseDC(_hMaskWnd, hdc);
        //    tempDc.Dispose();
        //}

        //private ScrollBarColorTable _colorTable;
        //[Browsable(false)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        //public ScrollBarColorTable ColorTable {
        //    get {
        //        if (_colorTable == null) {
        //            _colorTable = new ScrollBarColorTable();
        //        }
        //        return _colorTable;
        //    }
        //    set {
        //        _colorTable = value;
        //    }
        //}
        //private Color GetGray(Color color) {
        //    return ColorConverterEx.RgbToGray(
        //        new RGB(color)).Color;
        //}

        //private void PaintScrollBarTrack(PaintScrollBarTrackEventArgs e) {
        //    Graphics g = e.Graphics;
        //    Rectangle rect = e.TrackRectangle;

        //    Color baseColor = GetGray(ColorTable.Base);

        //    ControlPaintEx.DrawScrollBarTrack(
        //        g, rect, baseColor, Color.White, e.Orientation);
        //}
        //private void PaintScrollBarArrow(PaintScrollBarArrowEventArgs e) {
        //    Graphics g = e.Graphics;
        //    Rectangle rect = e.ArrowRectangle;
        //    ControlState controlState = e.ControlState;
        //    ArrowDirection direction = e.ArrowDirection;
        //    bool bHorizontal = e.Orientation == Orientation.Horizontal;
        //    bool bEnabled = e.Enabled;

        //    Color backColor = ColorTable.BackNormal;
        //    Color baseColor = ColorTable.Base;
        //    Color borderColor = ColorTable.Border;
        //    Color innerBorderColor = ColorTable.InnerBorder;
        //    Color foreColor = ColorTable.Fore;

        //    bool changeColor = false;

        //    if (bEnabled) {
        //        switch (controlState) {
        //            case ControlState.Hover:
        //                baseColor = ColorTable.BackHover;
        //                break;
        //            case ControlState.Pressed:
        //                baseColor = ColorTable.BackPressed;
        //                changeColor = true;
        //                break;
        //            default:
        //                baseColor = ColorTable.Base;
        //                break;
        //        }
        //    } else {
        //        backColor = GetGray(backColor);
        //        baseColor = GetGray(ColorTable.Base);
        //        borderColor = GetGray(borderColor);
        //        foreColor = GetGray(foreColor);
        //    }

        //    using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g)) {
        //        ControlPaintEx.DrawScrollBarArraw(
        //            g,
        //            rect,
        //            baseColor,
        //            backColor,
        //            borderColor,
        //            innerBorderColor,
        //            foreColor,
        //            e.Orientation,
        //            direction,
        //            changeColor);
        //    }
        //}
        //private void PaintScrollBarThumb(PaintScrollBarThumbEventArgs e) {
        //    bool bEnabled = e.Enabled;
        //    if (!bEnabled) {
        //        return;
        //    }

        //    Graphics g = e.Graphics;
        //    Rectangle rect = e.ThumbRectangle;
        //    ControlState controlState = e.ControlState;

        //    Color backColor = ColorTable.BackNormal;
        //    Color baseColor = ColorTable.Base;
        //    Color borderColor = ColorTable.Border;
        //    Color innerBorderColor = ColorTable.InnerBorder;

        //    bool changeColor = false;

        //    switch (controlState) {
        //        case ControlState.Hover:
        //            baseColor = ColorTable.BackHover;
        //            break;
        //        case ControlState.Pressed:
        //            baseColor = ColorTable.BackPressed;
        //            changeColor = true;
        //            break;
        //        default:
        //            baseColor = ColorTable.Base;
        //            break;
        //    }

        //    using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g)) {
        //        ControlPaintEx.DrawScrollBarThumb(
        //            g,
        //            rect,
        //            baseColor,
        //            backColor,
        //            borderColor,
        //            innerBorderColor,
        //            e.Orientation,
        //            changeColor);
        //    }
        //}
        ////scm end
        //#endregion scm end

        private RECT focusedPartSize() {
            RECT tr = new RECT();
            GetWindowRect(_hScrollBarWnd, ref tr);
            OffsetRect(ref tr, -tr.Left, -tr.Top);

            switch (scrollbarHitTest()) {
                case SB_HITEST.leftArrow:
                    tr = new RECT(0, 0, _iArrowCx, tr.Bottom);
                    break;

                case SB_HITEST.rightArrow:
                    tr = new RECT(tr.Right - _iArrowCx, 0, tr.Right, tr.Bottom);
                    break;

                case SB_HITEST.topArrow:
                    tr = new RECT(0, 0, tr.Right, _iArrowCy);
                    break;

                case SB_HITEST.bottomArrow:
                    tr = new RECT(0, tr.Bottom - _iArrowCy, tr.Right, _iArrowCy);
                    break;

                case SB_HITEST.button:
                    Point pst = getScrollBarThumb();
                    if (Direction == Orientation.Horizontal)
                        tr = new RECT(pst.X, 2, pst.Y - pst.X, tr.Bottom);
                    else
                        tr = new RECT(0, pst.X, _iArrowCx, pst.Y - pst.X);
                    break;

            }
            return tr;
        }

        private RECT getScrollBarRect() {
            SCROLLBARINFO sbi = new SCROLLBARINFO();
            sbi.cbSize = Marshal.SizeOf(sbi);
            SendMessage(_hScrollBarWnd, SBM_GETSCROLLBARINFO, 0, ref sbi);
            return sbi.rcScrollBar;
        }
        //scm
        private void sizeCheck() {
            //RECT tr = new RECT();
            //RECT tw = new RECT();
            //GetWindowRect(_hMaskWnd, ref tw);
            //GetWindowRect(_hScrollBarWnd, ref tr);
            //if (!EqualRect(ref tr, ref tw))
            //    SetWindowPos(_hMaskWnd, IntPtr.Zero, tr.Left, tr.Top, tr.Right - tr.Left, tr.Bottom - tr.Top, SWP_NOACTIVATE | SWP_NOOWNERZORDER | SWP_NOZORDER);


            RECT controlRect = new RECT();
            RECT maskRect = new RECT();

            GetWindowRect(_hMaskWnd, ref maskRect);
            GetWindowRect(_hScrollBarWnd, ref controlRect);

            uint uFlag = SWP_NOACTIVATE | SWP_NOOWNERZORDER | SWP_NOZORDER;

            if (!EqualRect(ref controlRect, ref maskRect)) {
                Point point = new Point(controlRect.Left, controlRect.Top);
                IntPtr hParent = GetParent(_hMaskWnd);
                ScreenToClient(hParent, ref point);

                SetWindowPos(
                    _hMaskWnd,
                    IntPtr.Zero,
                    point.X,
                    point.Y,
                    controlRect.Right - controlRect.Left,
                    controlRect.Bottom - controlRect.Top,
                    uFlag);
            }
        }
        //scm
        private Point getScrollBarThumb() {
            RECT rect1 = new RECT();
            GetWindowRect(_hScrollBarWnd, ref rect1);
            OffsetRect(ref rect1, -rect1.Left, -rect1.Top);

            Rectangle rect = new Rectangle(
                rect1.Left,
                rect1.Top,
                rect1.Right - rect1.Left,
                rect1.Bottom - rect1.Top);

            int arrowWidth = _iArrowCx;
            bool bHorizontal = Direction == Orientation.Horizontal ? true : false;
            ScrollBar scrollBar = (ScrollBar)Control.FromHandle(_hScrollBarWnd);
            int width;
            Point point = new Point();

            if (bHorizontal) {
                width = rect.Width - arrowWidth * 2;
            } else {
                width = rect.Height - arrowWidth * 2;
            }

            int value = scrollBar.Maximum - scrollBar.Minimum - scrollBar.LargeChange + 1;
            float thumbWidth = (float)width / ((float)value / scrollBar.LargeChange + 1);

            if (thumbWidth < 8) {
                thumbWidth = 8f;
            }

            if (value != 0) {
                int curValue = scrollBar.Value - scrollBar.Minimum;
                if (curValue > value) {
                    curValue = value;
                }
                point.X = (int)(curValue * ((float)(width - thumbWidth) / value));
            }
            point.X += arrowWidth;
            point.Y = point.X + (int)Math.Ceiling(thumbWidth);

            if (bHorizontal && scrollBar.RightToLeft == RightToLeft.Yes) {
                point.X = scrollBar.Width - point.X;
                point.Y = scrollBar.Width - point.Y;
            }

            return point;
        }
        //private Point getScrollBarThumb()
        //{
        //    Point pt = new Point();
        //    ScrollBar sc = (ScrollBar)Control.FromHandle(_hScrollBarWnd);
        //    float incr = 0;

        //    if (sc.ClientRectangle.Width > sc.ClientRectangle.Height)
        //    {
        //        incr = ((float)(sc.ClientRectangle.Width - (_iArrowCx * 2)) / sc.Maximum);
        //        pt.X = (int)(incr * sc.Value) + _iArrowCx;
        //        pt.Y = pt.X + (int)(incr * sc.LargeChange);
        //    }
        //    else
        //    {
        //        incr = ((float)(sc.ClientRectangle.Height - (_iArrowCy * 2)) / sc.Maximum);
        //        pt.X = (int)(incr * sc.Value) + _iArrowCy;
        //        pt.Y = pt.X + (int)(incr * sc.LargeChange);
        //    }
        //    // fix for ?bug? in scrollbar
        //    if (sc.Value < sc.Maximum)
        //    {
        //        sc.Value++;
        //        sc.Value--;
        //    }
        //    else
        //    {
        //        sc.Value--;
        //        sc.Value++;
        //    }
        //    invalidateWindow(true);
        //    return pt;
        //}

        private void invalidateWindow(bool messaged) {
            if (messaged)
                RedrawWindow(_hScrollBarWnd, IntPtr.Zero, IntPtr.Zero, RDW_INTERNALPAINT);
            else
                RedrawWindow(_hScrollBarWnd, IntPtr.Zero, IntPtr.Zero, RDW_INVALIDATE | RDW_UPDATENOW);
        }

        private bool leftKeyPressed() {
            if (mouseButtonsSwitched())
                return (GetKeyState(VK_RBUTTON) < 0);
            else
                return (GetKeyState(VK_LBUTTON) < 0);
        }

        private bool mouseButtonsSwitched() {
            return (GetSystemMetrics(SYSTEM_METRICS.SM_SWAPBUTTON) != 0);
        }

        private SB_HITEST scrollbarHitTest() {
            Point pt = new Point();
            RECT tr = new RECT();

            GetWindowRect(_hScrollBarWnd, ref tr);
            OffsetRect(ref tr, -tr.Left, -tr.Top);

            RECT tp = tr;
            GetCursorPos(ref pt);
            ScreenToClient(_hScrollBarWnd, ref pt);

            if (Direction == Orientation.Horizontal) {
                if (PtInRect(ref tr, pt)) {
                    // left arrow
                    tp.Right = _iArrowCx;
                    if (PtInRect(ref tp, pt))
                        return SB_HITEST.leftArrow;
                    // right arrow
                    tp.Left = tr.Right - _iArrowCx;
                    tp.Right = tr.Right;
                    if (PtInRect(ref tp, pt))
                        return SB_HITEST.rightArrow;
                    // button
                    Point pb = getScrollBarThumb();
                    tp.Left = pb.X;
                    tp.Right = pb.Y;
                    if (PtInRect(ref tp, pt))
                        return SB_HITEST.button;
                    // track
                    return SB_HITEST.track;
                }
            } else {
                if (PtInRect(ref tr, pt)) {
                    // top arrow
                    tp.Bottom = _iArrowCy;
                    if (PtInRect(ref tp, pt))
                        return SB_HITEST.topArrow;
                    // bottom arrow
                    tp.Top = tr.Bottom - _iArrowCy;
                    tp.Bottom = tr.Bottom;
                    if (PtInRect(ref tp, pt))
                        return SB_HITEST.bottomArrow;
                    // button
                    Point pb = getScrollBarThumb();
                    tp.Top = pb.X;
                    tp.Bottom = pb.Y;
                    if (PtInRect(ref tp, pt))
                        return SB_HITEST.button;
                    // track
                    return SB_HITEST.track;
                }
            }
            return SB_HITEST.offControl;
        }

        private void scrollbarMetrics() {
            if (Direction == Orientation.Horizontal) {
                _iArrowCx = GetSystemMetrics(SYSTEM_METRICS.SM_CXHSCROLL);
                _iArrowCy = GetSystemMetrics(SYSTEM_METRICS.SM_CYHSCROLL);
            } else {
                _iArrowCx = GetSystemMetrics(SYSTEM_METRICS.SM_CXVSCROLL);
                _iArrowCy = GetSystemMetrics(SYSTEM_METRICS.SM_CYVSCROLL);
            }
        }

        private void scrollHorizontal(bool Right) {
            if (Right)
                SendMessage(_hScrollBarWnd, WM_HSCROLL, SB_LINERIGHT, 0);
            else
                SendMessage(_hScrollBarWnd, WM_HSCROLL, SB_LINELEFT, 0);

        }

        private void scrollVertical(bool Down) {
            if (Down)
                SendMessage(_hScrollBarWnd, WM_VSCROLL, SB_LINEDOWN, 0);
            else
                SendMessage(_hScrollBarWnd, WM_VSCROLL, SB_LINEUP, 0);
        }

        private void scrollFader() {
            SB_HITEST hitTest = scrollbarHitTest();
            if (hitTest == SB_HITEST.button) {
                Point pst = getScrollBarThumb();
                if (TransitionGraphic != null) {
                    cTransition ts;
                    RECT tr = new RECT();
                    if (Direction == Orientation.Horizontal) {
                        GetWindowRect(_hScrollBarWnd, ref tr);
                        ts = new cTransition(_hMaskWnd, _hScrollBarWnd, TransitionGraphic, new Rectangle(pst.X, 0, pst.Y - pst.X, tr.Bottom));
                    } else {
                        ts = new cTransition(_hMaskWnd, _hScrollBarWnd, TransitionGraphic, new Rectangle(0, pst.X, _iArrowCx, pst.Y - pst.X));
                    }
                }
            }
        }
        #endregion

        #region WndProc

        //private SB_HITEST _lastMouseDownHistTest;//scm
        protected override void WndProc(ref Message m) //scm
        {


            //PAINTSTRUCT ps = new PAINTSTRUCT();

            //switch (m.Msg)
            //{
            //    case WM_PAINT:
            //        if (!_bPainting)
            //        {
            //            _bPainting = true;
            //            // start painting engine
            //            BeginPaint(m.HWnd, ref ps);
            //            drawScrollBar();
            //            ValidateRect(m.HWnd, ref ps.rcPaint);
            //            // done
            //            EndPaint(m.HWnd, ref ps);
            //            _bPainting = false;
            //            m.Result = MSG_HANDLED;
            //        }
            //        else
            //        {
            //            base.WndProc(ref m);
            //        }
            //        break;

            //    case WM_SIZE:
            //    case WM_MOVE:
            //        sizeCheck();
            //        base.WndProc(ref m);
            //        break;

            //    default:
            //        base.WndProc(ref m);
            //        break;
            //}

            try {
                switch (m.Msg) {
                    case WM_PAINT:
                        if (!_bPainting) {
                            PAINTSTRUCT ps = new PAINTSTRUCT();

                            _bPainting = true;
                            BeginPaint(m.HWnd, ref ps);
                            drawScrollBar();
                            ValidateRect(m.HWnd, ref ps.rcPaint);

                            EndPaint(m.HWnd, ref ps);
                            _bPainting = false;

                            m.Result = MSG_HANDLED;
                        } else {
                            base.WndProc(ref m);
                        }
                        break;
                    case SBM_SETSCROLLINFO:
                        drawScrollBar();

                        base.WndProc(ref m);
                        break;
                    case WM_STYLECHANGED:
                        drawScrollBar();
                        base.WndProc(ref m);
                        break;
                    case WM_LBUTTONDOWN:
                        //_lastMouseDownHistTest = scrollbarHitTest();
                        drawScrollBar();
                        base.WndProc(ref m);
                        break;
                    case WM_LBUTTONUP:
                    case WM_MOUSEMOVE:
                        Console.WriteLine(m.Msg.ToString());
                        drawScrollBar();
                        base.WndProc(ref m);
                        break;
                    case WM_MOUSELEAVE:
                        Console.WriteLine("WM_MOUSELEAVE");
                        drawScrollBar();
                        base.WndProc(ref m);
                        break;
                    case WM_WINDOWPOSCHANGED:
                        checkBarState();
                        sizeCheck();
                        base.WndProc(ref m);
                        break;
                    default:
                        base.WndProc(ref m);
                        break;
                }
            } catch {
            }
        }
        #endregion
    }
}