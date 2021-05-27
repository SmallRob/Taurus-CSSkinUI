
namespace Com_CSSkin.SkinControl
{
    #region Directives
    using System;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Drawing;
    using System.Text;
    using System.Reflection;
    #endregion

    public class ScrollBarHelper : NativeWindow, IDisposable
    {
        #region Constants
        // style
        /// <summary>
        /// 获得风格
        /// </summary>
        private const int GWL_STYLE = (-16);
        /// <summary>
        /// 获得扩展风格
        /// </summary>
        private const int GWL_EXSTYLE = (-20);
        private const int WS_EX_TOPMOST = 0x8;
        private const int WS_EX_TOOLWINDOW = 0x80;
        private const int WS_CHILD = 0x40000000;
        private const int WS_OVERLAPPED = 0x0;
        private const int WS_CLIPSIBLINGS = 0x4000000;
        private const int WS_VISIBLE = 0x10000000;
        private const int WS_HSCROLL = 0x100000;
        private const int WS_VSCROLL = 0x200000;
        private const int SS_OWNERDRAW = 0xD;

        // showwindow
        private const int SW_HIDE = 0x0;//False
        private const int SW_NORMAL = 0x1;//True

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
        /// <summary>
        /// 用于重画窗口的用户区
        /// </summary>
        private const int WM_PAINT = 0xF;
        /// <summary>
        /// 用于重画窗口的非用户区，如标题，边框和滚动杆
        /// </summary>
        private const int WM_NCPAINT = 0x85;
        private const int WM_NCMOUSEMOVE = 0xA0;
        /// <summary>
        /// 消息在鼠标移动时被发送至已获焦点的窗口
        /// </summary>
        private const int WM_MOUSEMOVE = 0x200;
        /// <summary>
        /// WM_MOUSELEAVE 是鼠标离开窗口时发出的消息，但是这个消息与普通的鼠标消息不同，要收到WM_MOUSELEAVE消息必须先调用TrackMouseEvent，
        /// 并且每调用一次TrackMouseEvent 窗口只能收到一次WM_MOUSELEAVE，也就说如果要获得WM_MOUSELEAVE消息的话，当鼠标重新进入窗口时必须调
        /// 用一次TrackMouseEvent。
        /// </summary>
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
        private const int WM_STYLECHANGED = 0x7D;
        private const int WM_SIZE = 0x5;
        private const int WM_MOVE = 0x3;
        public const int WM_NCMOUSELEAVE = 0x02A2;// non client mouse scm
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

        /// <summary>
        /// 来存储一个矩形框的左上角坐标、宽度和高度
        /// </summary>
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

        /// <summary>
        /// 一个SCROLLBARINFO结构，以获得的信息。在调用GetScrollBarInfo，设置cbSize成员为sizeof（SCROLLBARINFO）。
        /// </summary>
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

        /// <summary>
        /// OffsetRect函数将指定的矩形移动到指定的位置 
        /// </summary>
        /// <param name="lpRect">[输入输出]指向一个RECT结构，其中包含了被移动矩形的逻辑坐标</param>
        /// <param name="x">[输入]指定的矩形左右移动的量。当向左移动的时候，这个参数必须是一个负值</param>
        /// <param name="y">[输出]指定的矩形上下移动的量。当想上移动的时候，这个参数应该是一个负值。</param>
        /// <returns>如果函数成功，返回非0，否则返回0。 </returns>
        [DllImport("user32.dll")]
        private extern static int OffsetRect(ref RECT lpRect, int x, int y);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ValidateRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        private static extern int GetSystemMetrics(SYSTEM_METRICS smIndex);

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
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EqualRect([In] ref RECT lprc1, [In] ref RECT lprc2);

        /// <summary>
        /// GetWindowLong是一个函数。该函数获得有关指定窗口的信息，函数也获得在额外窗口内存中指定偏移位地址的32位度整型值。
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        /// <summary>
        /// 函数功能：该函数设置指定窗口的显示状态
        /// </summary>
        /// <param name="hWnd">指窗口句柄</param>
        /// <param name="nCmdShow">指定窗口如何显示</param>
        /// <returns>如果窗口当前可见，则返回值为非零。如果窗口当前被隐藏，则返回值为零</returns>
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        /// <summary>
        ///  检索有关指定滚动条信息。
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="idObject"></param>
        /// <param name="psbi"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern int GetScrollBarInfo(IntPtr hWnd, uint idObject, ref SCROLLBARINFO psbi);
        #endregion

        #region Fields
        private bool _bTrackingMouse = false;//scm
        private int _iArrowCx = 0;
        private int _iArrowCy = 0;
        private IntPtr _hVerticalMaskWnd = IntPtr.Zero;
        private IntPtr _hHorizontalMaskWnd = IntPtr.Zero;
        /// <summary>
        /// 当前窗口句柄
        /// </summary>
        private IntPtr _hSizerMaskWnd = IntPtr.Zero;
        private IntPtr _hControlWnd = IntPtr.Zero;
        private cStoreDc _cHorizontalArrowDc = new cStoreDc();
        private cStoreDc _cHorizontalThumbDc = new cStoreDc();
        private cStoreDc _cHorizontalTrackDc = new cStoreDc();
        private cStoreDc _cVerticalArrowDc = new cStoreDc();
        private cStoreDc _cVerticalThumbDc = new cStoreDc();
        private cStoreDc _cVerticalTrackDc = new cStoreDc();
        private Bitmap _oHorizontalArrowBitmap;
        private Bitmap _oHorizontalThumbBitmap;
        private Bitmap _oHorizontalTrackBitmap;
        private Bitmap _oVerticalArrowBitmap;
        private Bitmap _oVerticalThumbBitmap;
        private Bitmap _oVerticalTrackBitmap;
        private Bitmap _oMask;
        #endregion

        #region Constructor
        public ScrollBarHelper(IntPtr hWnd, Bitmap hztrack, Bitmap hzarrow, Bitmap hzthumb, Bitmap vttrack, Bitmap vtarrow, Bitmap vtthumb, Bitmap fader) {
            if (hWnd == IntPtr.Zero)
                throw new Exception("控制手柄是无效的。");
            if (hztrack == null)
                throw new Exception("水平轨道图像是无效的。");
            if (hzarrow == null)
                throw new Exception("水平箭头图像是无效的。");
            if (hzthumb == null)
                throw new Exception("水平拇指形象是无效的。");
            if (vttrack == null)
                throw new Exception("垂直轨迹图像是无效的。");
            if (vtarrow == null)
                throw new Exception("垂直箭头图像是无效的。");
            if (vtthumb == null)
                throw new Exception("竖拇指图像是无效的。");
            HorizontalArrowGraphic = (Bitmap)hzarrow.Clone();
            HorizontalThumbGraphic = (Bitmap)hzthumb.Clone();
            HorizontalTrackGraphic = (Bitmap)hztrack.Clone();
            VerticalArrowGraphic = (Bitmap)vtarrow.Clone();
            VerticalThumbGraphic = (Bitmap)vtthumb.Clone();
            VerticalTrackGraphic = (Bitmap)vttrack.Clone();
            //这个类的推子将需要一些额外的代码
            if (fader != null)
                TransitionGraphic = fader;
            scrollbarMetrics();
            _hControlWnd = hWnd;
            createScrollBarMask();
            this.AssignHandle(_hControlWnd);
        }
        #endregion

        #region Properties

        public void refin() {
            this.invalidateWindow(false);

        }
        private Bitmap HorizontalArrowGraphic {
            get { return _oHorizontalArrowBitmap; }
            set {
                _oHorizontalArrowBitmap = value;
                if (_cHorizontalArrowDc.Hdc != IntPtr.Zero) {
                    _cHorizontalArrowDc.Dispose();
                    _cHorizontalArrowDc = new cStoreDc();
                }
                _cHorizontalArrowDc.Width = _oHorizontalArrowBitmap.Width;
                _cHorizontalArrowDc.Height = _oHorizontalArrowBitmap.Height;
                SelectObject(_cHorizontalArrowDc.Hdc, _oHorizontalArrowBitmap.GetHbitmap());

            }
        }

        private Bitmap HorizontalThumbGraphic {
            get { return _oHorizontalThumbBitmap; }
            set {
                _oHorizontalThumbBitmap = value;
                if (_cHorizontalThumbDc.Hdc != IntPtr.Zero) {
                    _cHorizontalThumbDc.Dispose();
                    _cHorizontalThumbDc = new cStoreDc();
                }
                _cHorizontalThumbDc.Width = _oHorizontalThumbBitmap.Width;
                _cHorizontalThumbDc.Height = _oHorizontalThumbBitmap.Height;
                SelectObject(_cHorizontalThumbDc.Hdc, _oHorizontalThumbBitmap.GetHbitmap());

            }
        }

        private Bitmap HorizontalTrackGraphic {
            get { return _oHorizontalTrackBitmap; }
            set {
                _oHorizontalTrackBitmap = value;
                if (_cHorizontalTrackDc.Hdc != IntPtr.Zero) {
                    _cHorizontalTrackDc.Dispose();
                    _cHorizontalTrackDc = new cStoreDc();
                }
                _cHorizontalTrackDc.Width = _oHorizontalTrackBitmap.Width;
                _cHorizontalTrackDc.Height = _oHorizontalTrackBitmap.Height;
                SelectObject(_cHorizontalTrackDc.Hdc, _oHorizontalTrackBitmap.GetHbitmap());

            }
        }

        private Bitmap VerticalArrowGraphic {
            get { return _oVerticalArrowBitmap; }
            set {
                _oVerticalArrowBitmap = value;
                if (_cVerticalArrowDc.Hdc != IntPtr.Zero) {
                    _cVerticalArrowDc.Dispose();
                    _cVerticalArrowDc = new cStoreDc();
                }
                _cVerticalArrowDc.Width = _oVerticalArrowBitmap.Width;
                _cVerticalArrowDc.Height = _oVerticalArrowBitmap.Height;
                SelectObject(_cVerticalArrowDc.Hdc, _oVerticalArrowBitmap.GetHbitmap());

            }
        }

        private Bitmap VerticalThumbGraphic {
            get { return _oVerticalThumbBitmap; }
            set {
                _oVerticalThumbBitmap = value;
                if (_cVerticalThumbDc.Hdc != IntPtr.Zero) {
                    _cVerticalThumbDc.Dispose();
                    _cVerticalThumbDc = new cStoreDc();
                }
                _cVerticalThumbDc.Width = _oVerticalThumbBitmap.Width;
                _cVerticalThumbDc.Height = _oVerticalThumbBitmap.Height;
                SelectObject(_cVerticalThumbDc.Hdc, _oVerticalThumbBitmap.GetHbitmap());

            }
        }

        private Bitmap VerticalTrackGraphic {
            get { return _oVerticalTrackBitmap; }
            set {
                _oVerticalTrackBitmap = value;
                if (_cVerticalTrackDc.Hdc != IntPtr.Zero) {
                    _cVerticalTrackDc.Dispose();
                    _cVerticalTrackDc = new cStoreDc();
                }
                _cVerticalTrackDc.Width = _oVerticalTrackBitmap.Width;
                _cVerticalTrackDc.Height = _oVerticalTrackBitmap.Height;
                SelectObject(_cVerticalTrackDc.Hdc, _oVerticalTrackBitmap.GetHbitmap());

            }
        }

        private Bitmap TransitionGraphic {
            get { return _oMask; }
            set { _oMask = value; }
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
        /// <summary>
        /// 滚动条状态
        /// </summary>
        private void checkBarState() {
            if ((GetWindowLong(_hControlWnd, GWL_STYLE) & WS_VISIBLE) == WS_VISIBLE) {
                if (hasHorizontal())
                    ShowWindow(_hHorizontalMaskWnd, SW_NORMAL);
                else
                    ShowWindow(_hHorizontalMaskWnd, SW_HIDE);

                if (hasVertical())
                    ShowWindow(_hVerticalMaskWnd, SW_NORMAL);
                else
                    ShowWindow(_hVerticalMaskWnd, SW_HIDE);

                if (hasSizer())
                    ShowWindow(_hSizerMaskWnd, SW_NORMAL);
                else
                    ShowWindow(_hSizerMaskWnd, SW_HIDE);
            } else {
                ShowWindow(_hHorizontalMaskWnd, SW_HIDE);
                ShowWindow(_hVerticalMaskWnd, SW_HIDE);
                ShowWindow(_hSizerMaskWnd, SW_HIDE);
            }
        }

        private void createScrollBarMask() {
            Type t = typeof(cScrollBar);
            Module m = t.Module;
            IntPtr hInstance = Marshal.GetHINSTANCE(m);
            IntPtr hParent = GetParent(_hControlWnd);
            RECT tr = new RECT();
            Point pt = new Point();
            SCROLLBARINFO sb = new SCROLLBARINFO();
            sb.cbSize = Marshal.SizeOf(sb);

            // 垂直滚动条
            // 得到的大小和位置
            GetScrollBarInfo(_hControlWnd, OBJID_VSCROLL, ref sb);
            tr = sb.rcScrollBar;
            pt.X = tr.Left;
            pt.Y = tr.Top;
            ScreenToClient(hParent, ref pt);

            // 创建窗口
            _hVerticalMaskWnd = CreateWindowEx(WS_EX_TOPMOST | WS_EX_TOOLWINDOW,
                "STATIC", "",
                SS_OWNERDRAW | WS_CHILD | WS_CLIPSIBLINGS | WS_OVERLAPPED | WS_VISIBLE,
                pt.X, pt.Y,
                (tr.Right - tr.Left), (tr.Bottom - tr.Top),
                hParent,
                IntPtr.Zero, hInstance, IntPtr.Zero);

            // 设置Z -阶
            SetWindowPos(_hVerticalMaskWnd, HWND_TOP,
                0, 0,
                0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_NOOWNERZORDER);

            // 水平滚动条
            GetScrollBarInfo(_hControlWnd, OBJID_HSCROLL, ref sb);
            tr = sb.rcScrollBar;
            pt.X = tr.Left;
            pt.Y = tr.Top;
            ScreenToClient(hParent, ref pt);

            _hHorizontalMaskWnd = CreateWindowEx(WS_EX_TOPMOST | WS_EX_TOOLWINDOW,
                "STATIC", "",
                SS_OWNERDRAW | WS_CHILD | WS_CLIPSIBLINGS | WS_OVERLAPPED | WS_VISIBLE,
                pt.X, pt.Y,
                (tr.Right - tr.Left), (tr.Bottom - tr.Top),
                hParent,
                IntPtr.Zero, hInstance, IntPtr.Zero);

            SetWindowPos(_hHorizontalMaskWnd, HWND_TOP,
                0, 0,
                0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_NOOWNERZORDER);

            // sizer
            _hSizerMaskWnd = CreateWindowEx(WS_EX_TOPMOST | WS_EX_TOOLWINDOW,
                "STATIC", "",
                SS_OWNERDRAW | WS_CHILD | WS_CLIPSIBLINGS | WS_OVERLAPPED | WS_VISIBLE,
                pt.X + (tr.Right - tr.Left), pt.Y,
                _iArrowCx, _iArrowCy,
                hParent,
                IntPtr.Zero, hInstance, IntPtr.Zero);

            SetWindowPos(_hSizerMaskWnd, HWND_TOP,
                0, 0,
                0, 0,
                SWP_NOMOVE | SWP_NOSIZE | SWP_NOACTIVATE | SWP_NOOWNERZORDER);
            reSizeMask();
        }

        /// <summary>
        /// 画滚动条
        /// </summary>
        public void drawScrollBar() {
            RECT tr = new RECT();
            Point pst = new Point();
            cStoreDc tempDc = new cStoreDc();
            IntPtr hdc = IntPtr.Zero;
            int offset = 0;
            int width = 0;
            int section = 0;

            SCROLLBARINFO sb = new SCROLLBARINFO();
            sb.cbSize = Marshal.SizeOf(sb);

            #region 是否存在水平滚动条
            if (hasHorizontal()) {
                GetScrollBarInfo(_hControlWnd, OBJID_HSCROLL, ref sb);
                tr = sb.rcScrollBar;
                OffsetRect(ref tr, -tr.Left, -tr.Top);
                tempDc.Width = tr.Right;
                tempDc.Height = tr.Bottom;
                SB_HITEST hitTest = scrollbarHitTest(Orientation.Horizontal);

                // 绘制轨道
                using (StretchImage si = new StretchImage(_cHorizontalTrackDc.Hdc, tempDc.Hdc, new Rectangle(0, 0, _cHorizontalTrackDc.Width, _cHorizontalTrackDc.Height), new Rectangle(_iArrowCx, 0, tr.Right - (2 * _iArrowCx), tr.Bottom), 2, StretchModeEnum.STRETCH_HALFTONE)) { }
                // 绘制箭头
                section = 7;
                width = _cHorizontalArrowDc.Width / section;
                // 左箭头
                if (hitTest == SB_HITEST.leftArrow) {
                    if (leftKeyPressed())
                        offset = 2;
                    else
                        offset = 1;
                } else {
                    offset = 0;
                }
                using (StretchImage si = new StretchImage(_cHorizontalArrowDc.Hdc, tempDc.Hdc, new Rectangle(offset * width, 0, width, _cHorizontalArrowDc.Height), new Rectangle(0, 0, _iArrowCx, tr.Bottom), 2, StretchModeEnum.STRETCH_HALFTONE)) { }

                // 右箭头
                if (hitTest == SB_HITEST.rightArrow) {
                    if (leftKeyPressed())
                        offset = 5;
                    else
                        offset = 4;

                } else {
                    offset = 3;
                }
                using (StretchImage si = new StretchImage(_cHorizontalArrowDc.Hdc, tempDc.Hdc, new Rectangle(offset * width, 0, width, _cHorizontalArrowDc.Height), new Rectangle(tr.Right - _iArrowCx, 0, _iArrowCx, tr.Bottom), 2, StretchModeEnum.STRETCH_HALFTONE)) { }

                // 绘制拇指
                section = 3;
                width = _cHorizontalThumbDc.Width / section;
                if (hitTest == SB_HITEST.button) {
                    if (leftKeyPressed())
                        offset = 2;
                    else
                        offset = 1;

                } else {
                    offset = 0;
                }
                pst.X = sb.xyThumbTop;
                pst.Y = sb.xyThumbBottom;
                using (StretchImage si = new StretchImage(_cHorizontalThumbDc.Hdc, tempDc.Hdc, new Rectangle(offset * width, 0, width, _cHorizontalThumbDc.Height), new Rectangle(pst.X, 1, pst.Y - pst.X, tr.Bottom), 2, StretchModeEnum.STRETCH_HALFTONE)) { }

                hdc = GetDC(_hHorizontalMaskWnd);
                BitBlt(hdc, 0, 0, tr.Right, tr.Bottom, tempDc.Hdc, 0, 0, 0xCC0020);
                ReleaseDC(_hHorizontalMaskWnd, hdc);
            }
            #endregion

            #region 是否存在水平/垂直滚动条 - 右下小模块
            if (hasSizer()) {
                tempDc.Width = _iArrowCx;
                tempDc.Height = _iArrowCy;
                offset = 6;
                section = 7;
                width = _cHorizontalArrowDc.Width / section;

                using (StretchImage si = new StretchImage(_cHorizontalArrowDc.Hdc, tempDc.Hdc, new Rectangle(offset * width, 0, width, _cHorizontalArrowDc.Height), new Rectangle(0, 0, _iArrowCx, _iArrowCy), 0, StretchModeEnum.STRETCH_HALFTONE)) { }
                hdc = GetDC(_hSizerMaskWnd);
                BitBlt(hdc, 0, 0, _iArrowCx, _iArrowCy, tempDc.Hdc, 0, 0, 0xCC0020);
                ReleaseDC(_hSizerMaskWnd, hdc);
            }
            #endregion

            #region 是否存在垂直滚动条
            if (hasVertical()) {
                GetScrollBarInfo(_hControlWnd, OBJID_VSCROLL, ref sb);
                tr = sb.rcScrollBar;
                OffsetRect(ref tr, -tr.Left, -tr.Top);
                tempDc.Width = tr.Right;
                tempDc.Height = tr.Bottom;
                SB_HITEST hitTest = scrollbarHitTest(Orientation.Vertical);

                // 绘制轨道
                using (StretchImage si = new StretchImage(_cVerticalTrackDc.Hdc, tempDc.Hdc, new Rectangle(0, 0, _cVerticalTrackDc.Width, _cVerticalTrackDc.Height), new Rectangle(0, _iArrowCy, tr.Right, tr.Bottom - (2 * _iArrowCy)), 2, StretchModeEnum.STRETCH_HALFTONE)) { }
                section = 6;
                width = _cVerticalArrowDc.Width / section;

                // 顶部的箭头
                if (hitTest == SB_HITEST.topArrow) {
                    if (leftKeyPressed())
                        offset = 2;
                    else
                        offset = 1;
                } else {
                    offset = 0;
                }
                using (StretchImage si = new StretchImage(_cVerticalArrowDc.Hdc, tempDc.Hdc, new Rectangle(offset * width, 0, width, _cVerticalArrowDc.Height), new Rectangle(0, 0, tr.Right, _iArrowCy), 2, StretchModeEnum.STRETCH_HALFTONE)) { }

                // 底部的箭头
                if (hitTest == SB_HITEST.bottomArrow) {
                    if (leftKeyPressed())
                        offset = 5;
                    else
                        offset = 4;
                } else {
                    offset = 3;
                }
                using (StretchImage si = new StretchImage(_cVerticalArrowDc.Hdc, tempDc.Hdc, new Rectangle(offset * width, 0, width, _cVerticalArrowDc.Height), new Rectangle(0, tr.Bottom - _iArrowCy, tr.Right, _iArrowCy), 2, StretchModeEnum.STRETCH_HALFTONE)) { }

                // 绘制拇指
                section = 3;
                width = _cVerticalThumbDc.Width / section;
                if (hitTest == SB_HITEST.button) {
                    if (leftKeyPressed())
                        offset = 2;
                    else
                        offset = 1;
                } else {
                    offset = 0;
                }

                pst.X = sb.xyThumbTop;
                pst.Y = sb.xyThumbBottom;
                using (StretchImage si = new StretchImage(_cVerticalThumbDc.Hdc, tempDc.Hdc, new Rectangle(offset * width, 0, width, _cVerticalThumbDc.Height), new Rectangle(0, pst.X, _iArrowCx, pst.Y - pst.X), 2, StretchModeEnum.STRETCH_HALFTONE)) { }

                hdc = GetDC(_hVerticalMaskWnd);
                BitBlt(hdc, 0, 0, tr.Right, tr.Bottom, tempDc.Hdc, 0, 0, 0xCC0020);
                ReleaseDC(_hVerticalMaskWnd, hdc);
            }
            #endregion
            tempDc.Dispose();
        }

        public void scrollFader() {
            if (TransitionGraphic != null) {
                SB_HITEST hitTest;
                SCROLLBARINFO sb = new SCROLLBARINFO();
                sb.cbSize = Marshal.SizeOf(sb);
                if (hasHorizontal()) {
                    hitTest = scrollbarHitTest(Orientation.Horizontal);

                    if ((hitTest == SB_HITEST.button) && (!leftKeyPressed())) {
                        GetScrollBarInfo(_hControlWnd, OBJID_HSCROLL, ref sb);
                        // 在这里开始过渡例程
                        // size of mask - new Rectangle(sb.xyThumbTop, 2, sb.xyThumbBottom - sb.xyThumbTop, sb.rcScrollBar.Bottom)                        
                    }
                }
                if (hasVertical()) {
                    hitTest = scrollbarHitTest(Orientation.Vertical);

                    if ((hitTest == SB_HITEST.button) && (!leftKeyPressed())) {
                        GetScrollBarInfo(_hControlWnd, OBJID_VSCROLL, ref sb);
                        // 在这里开始过渡例程
                        // size of mask - new Rectangle(sb.xyThumbTop, 2, sb.xyThumbBottom - sb.xyThumbTop, sb.rcScrollBar.Bottom)
                    }
                }
            }
        }

        /// <summary>
        /// 是否存在水平滚动条
        /// </summary>
        /// <returns></returns>
        private bool hasHorizontal() {
            return ((GetWindowLong(_hControlWnd, GWL_STYLE) & WS_HSCROLL) == WS_HSCROLL);
        }
        /// <summary>
        /// 垂直/水平滚动条都存在
        /// </summary>
        /// <returns></returns>
        private bool hasSizer() {
            return (hasHorizontal() && hasVertical());
        }
        /// <summary>
        /// 是否存在垂直滚动条
        /// </summary>
        /// <returns></returns>
        private bool hasVertical() {
            return ((GetWindowLong(_hControlWnd, GWL_STYLE) & WS_VSCROLL) == WS_VSCROLL);
        }

        private void invalidateWindow(bool messaged) {
            if (messaged)
                RedrawWindow(_hControlWnd, IntPtr.Zero, IntPtr.Zero, RDW_INTERNALPAINT);
            else
                RedrawWindow(_hControlWnd, IntPtr.Zero, IntPtr.Zero, RDW_INVALIDATE | RDW_UPDATENOW);
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

        private SB_HITEST scrollbarHitTest(Orientation orient) {
            Point pt = new Point();
            RECT tr = new RECT();
            RECT tp = new RECT();
            SCROLLBARINFO sb = new SCROLLBARINFO();
            sb.cbSize = Marshal.SizeOf(sb);

            GetCursorPos(ref pt);

            if (orient == Orientation.Horizontal) {
                GetScrollBarInfo(_hControlWnd, OBJID_HSCROLL, ref sb);
                tr = sb.rcScrollBar;
                //OffsetRect(ref tr, -tr.Left, -tr.Top);
                tp = tr;
                if (PtInRect(ref tr, pt)) {
                    // left arrow
                    tp.Right = tp.Left + _iArrowCx;
                    if (PtInRect(ref tp, pt))
                        return SB_HITEST.leftArrow;
                    // right arrow
                    tp.Left = tr.Right - _iArrowCx;
                    tp.Right = tr.Right;
                    if (PtInRect(ref tp, pt))
                        return SB_HITEST.rightArrow;
                    // button
                    tp.Left = tr.Left + sb.xyThumbTop;
                    tp.Right = tr.Left + sb.xyThumbBottom;
                    if (PtInRect(ref tp, pt))//
                        return SB_HITEST.button;
                    // track
                    return SB_HITEST.track;
                }
            } else {
                GetScrollBarInfo(_hControlWnd, OBJID_VSCROLL, ref sb);
                tr = sb.rcScrollBar;
                tp = tr;

                if (PtInRect(ref tr, pt)) {
                    // top arrow
                    tp.Bottom = tr.Top + _iArrowCy;
                    if (PtInRect(ref tp, pt))
                        return SB_HITEST.topArrow;
                    // bottom arrow
                    tp.Top = tr.Bottom - _iArrowCy;
                    tp.Bottom = tr.Bottom;
                    if (PtInRect(ref tp, pt))
                        return SB_HITEST.bottomArrow;
                    // button
                    tp.Top = tr.Top + sb.xyThumbTop;
                    tp.Bottom = tr.Top + sb.xyThumbBottom;//scm
                    //tp.Bottom = tr.Bottom + sb.xyThumbBottom;//scm
                    if (PtInRect(ref tp, pt))
                        return SB_HITEST.button;
                    // track
                    return SB_HITEST.track;
                }
            }
            return SB_HITEST.offControl;
        }

        private void reSizeMask() {
            RECT tr = new RECT();
            SCROLLBARINFO sb = new SCROLLBARINFO();
            sb.cbSize = Marshal.SizeOf(sb);
            IntPtr hParent = GetParent(_hControlWnd);
            Point pt = new Point();

            if (hasVertical()) {
                GetScrollBarInfo(_hControlWnd, OBJID_VSCROLL, ref sb);
                tr = sb.rcScrollBar;
                pt.X = tr.Left;
                pt.Y = tr.Top;
                ScreenToClient(hParent, ref pt);
                SetWindowPos(_hVerticalMaskWnd, IntPtr.Zero, pt.X, pt.Y, tr.Right - tr.Left, tr.Bottom - tr.Top, SWP_NOACTIVATE | SWP_NOOWNERZORDER | SWP_NOZORDER | SWP_SHOWWINDOW);
            }
            if (hasHorizontal()) {
                GetScrollBarInfo(_hControlWnd, OBJID_HSCROLL, ref sb);
                tr = sb.rcScrollBar;
                pt.X = tr.Left;
                pt.Y = tr.Top;
                ScreenToClient(hParent, ref pt);
                SetWindowPos(_hHorizontalMaskWnd, IntPtr.Zero, pt.X, pt.Y, tr.Right - tr.Left, tr.Bottom - tr.Top, SWP_NOACTIVATE | SWP_NOOWNERZORDER | SWP_NOZORDER | SWP_SHOWWINDOW);
            }
            if (hasSizer()) {
                GetScrollBarInfo(_hControlWnd, OBJID_HSCROLL, ref sb);
                tr = new RECT(sb.rcScrollBar.Right, sb.rcScrollBar.Top, sb.rcScrollBar.Right + _iArrowCx, sb.rcScrollBar.Bottom);
                pt.X = tr.Left;
                pt.Y = tr.Top;
                ScreenToClient(hParent, ref pt);
                SetWindowPos(_hSizerMaskWnd, IntPtr.Zero, pt.X, pt.Y, tr.Right - tr.Left, tr.Bottom - tr.Top, SWP_NOACTIVATE | SWP_NOOWNERZORDER | SWP_NOZORDER | SWP_SHOWWINDOW);
            }
        }

        private void scrollbarMetrics() {
            _iArrowCx = GetSystemMetrics(SYSTEM_METRICS.SM_CXVSCROLL);
            _iArrowCy = GetSystemMetrics(SYSTEM_METRICS.SM_CYVSCROLL);
        }

        private void scrollHorizontal(bool Right) {
            if (Right)
                SendMessage(_hControlWnd, WM_HSCROLL, SB_LINERIGHT, 0);
            else
                SendMessage(_hControlWnd, WM_HSCROLL, SB_LINELEFT, 0);

        }

        private void scrollVertical(bool Down) {
            if (Down)
                SendMessage(_hControlWnd, WM_VSCROLL, SB_LINEDOWN, 0);
            else
                SendMessage(_hControlWnd, WM_VSCROLL, SB_LINEUP, 0);
        }

        public void Dispose() {
            try {
                this.ReleaseHandle();
                if (_oVerticalArrowBitmap != null) _oVerticalArrowBitmap.Dispose();
                if (_cVerticalArrowDc != null) _cVerticalArrowDc.Dispose();
                if (_oVerticalThumbBitmap != null) _oVerticalThumbBitmap.Dispose();
                if (_cVerticalThumbDc != null) _cVerticalThumbDc.Dispose();
                if (_oVerticalTrackBitmap != null) _oVerticalTrackBitmap.Dispose();
                if (_cVerticalTrackDc != null) _cVerticalTrackDc.Dispose();
                if (_oHorizontalArrowBitmap != null) _oHorizontalArrowBitmap.Dispose();
                if (_cHorizontalArrowDc != null) _cHorizontalArrowDc.Dispose();
                if (_oHorizontalThumbBitmap != null) _oHorizontalThumbBitmap.Dispose();
                if (_cHorizontalThumbDc != null) _cHorizontalThumbDc.Dispose();
                if (_oHorizontalTrackBitmap != null) _oHorizontalTrackBitmap.Dispose();
                if (_cHorizontalTrackDc != null) _cHorizontalTrackDc.Dispose();
                if (_hVerticalMaskWnd != IntPtr.Zero) DestroyWindow(_hVerticalMaskWnd);
                if (_hHorizontalMaskWnd != IntPtr.Zero) DestroyWindow(_hHorizontalMaskWnd);
                if (_hSizerMaskWnd != IntPtr.Zero) DestroyWindow(_hSizerMaskWnd);
            } catch { }
            GC.SuppressFinalize(this);
        }
        #endregion

        #region WndProc
        protected override void WndProc(ref Message m) {
            try {
                switch (m.Msg) {
                    case WM_NCPAINT:
                        drawScrollBar();
                        base.WndProc(ref m);
                        break;

                    case WM_HSCROLL:
                    case WM_VSCROLL:

                    case WM_NCMOUSEMOVE:
                        _bTrackingMouse = true;
                        drawScrollBar();
                        scrollFader();
                        base.WndProc(ref m);
                        break;

                    //case WM_NCMOUSELEAVE://scm
                    //    drawScrollBar();
                    //    base.WndProc(ref m);
                    //    break;

                    case WM_MOUSEMOVE:
                        if (_bTrackingMouse)
                            _bTrackingMouse = false;
                        base.WndProc(ref m);
                        break;

                    case WM_SIZE:
                    case WM_MOVE:
                        reSizeMask();
                        base.WndProc(ref m);
                        break;

                    default:
                        checkBarState();
                        drawScrollBar();
                        base.WndProc(ref m);
                        break;
                }
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}

