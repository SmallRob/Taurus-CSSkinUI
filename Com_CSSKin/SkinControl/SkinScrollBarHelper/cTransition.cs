
namespace Com_CSSkin.SkinControl
{
    #region Directives
    using System;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text;
    using System.Drawing.Drawing2D;
    using System.Diagnostics;
    using System.Timers;
    using System.ComponentModel;
    #endregion

    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class cTransition : NativeWindow, IDisposable
    {
        #region Constants
        private const int WM_PAINT = 0xF;
        private const int WM_NCPAINT = 0x85;
        private const int WM_TIMER = 0x113;
        private const int WM_NCMOUSEMOVE = 0xA0;
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_MOUSELEAVE = 0x2A3;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_RBUTTONDOWN = 0x204;
        #endregion

        #region Structs
        [StructLayout(LayoutKind.Sequential)]
        internal struct RECT
        {
            internal RECT(int X, int Y, int Width, int Height)
            {
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
        #endregion

        #region API
        [DllImport("user32.dll")]
        private static extern IntPtr BeginPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool EndPaint(IntPtr hWnd, ref PAINTSTRUCT ps);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDC(IntPtr handle);

        [DllImport("user32.dll")]
        private static extern int ReleaseDC(IntPtr handle, IntPtr hdc);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool BitBlt(IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, int dwRop);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool PtInRect([In] ref RECT lprc, Point pt);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("user32.dll")]
        private static extern int ScreenToClient(IntPtr hwnd, ref Point lpPoint);

        [DllImport("gdi32.dll")]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hObject);

        [DllImport("user32.dll")]
        private static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

        [DllImport("user32.dll")]
        private extern static int OffsetRect(ref RECT lpRect, int x, int y);

        [DllImport("user32.dll")]
        private static extern IntPtr SetTimer(IntPtr hWnd, int nIDEvent, uint uElapse, IntPtr lpTimerFunc);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool KillTimer(IntPtr hWnd, uint uIDEvent);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ValidateRect(IntPtr hWnd, ref RECT lpRect);
        #endregion

        #region Fields
        private bool _bPainting = false;
        private bool _bFadeIn = false;
        private int _maskTimer = 0;
        private int _safeTimer = 0;
        private Rectangle _areaRect;
        private IntPtr _hControlWnd = IntPtr.Zero;
        private IntPtr _hParentWnd = IntPtr.Zero;
        private Bitmap _oMask;
        private cStoreDc _cMaskDc = new cStoreDc();
        private cStoreDc _cTransitionDc = new cStoreDc();
        #endregion

        #region Events
        public delegate void DisposingDelegate();
        public event DisposingDelegate Disposing;
        #endregion

        #region Constructor
        public cTransition(IntPtr hWnd, IntPtr hParent, Bitmap mask, Rectangle area)
        {
            if (hWnd == IntPtr.Zero)
                throw new Exception("The control handle is invalid.");
            _hControlWnd = hWnd;
            if (mask != null)
            {
                TransitionGraphic = mask;
                _areaRect = area;
                if (hParent != IntPtr.Zero)
                {
                    _hParentWnd = hParent;
                    this.AssignHandle(_hParentWnd);
                }
                else
                {
                    _hParentWnd = _hControlWnd;
                    this.AssignHandle(_hControlWnd);
                }
                startTimer();
            }
        }

        public void Dispose()
        {
            this.ReleaseHandle();
            if (Disposing != null) Disposing();
            stopTimer();
            if (_cTransitionDc != null) _cTransitionDc.Dispose();
            if (_cMaskDc != null) _cMaskDc.Dispose();
            GC.SuppressFinalize(this);
        }
        #endregion

        #region Properties
        private Bitmap TransitionGraphic
        {
            get { return _oMask; }
            set
            {
                _oMask = value;
                if (_cTransitionDc.Hdc != IntPtr.Zero)
                {
                    _cTransitionDc.Dispose();
                    _cTransitionDc = new cStoreDc();
                }
                _cTransitionDc.Width = _oMask.Width;
                _cTransitionDc.Height = _oMask.Height;
                SelectObject(_cTransitionDc.Hdc, _oMask.GetHbitmap());
            }
        }
        #endregion

        #region Methods
        private void startTimer()
        {
            if (_safeTimer > 0)
                stopTimer();
            SetTimer(_hParentWnd, 66, 25, IntPtr.Zero);
        }

        private void stopTimer()
        {
            if (_safeTimer > 0)
            {
                KillTimer(_hParentWnd, 66);
                _safeTimer = 0;
            }
        }

        private void fadeIn()
        {
            if (_bFadeIn == false)
            {
                _bFadeIn = true;
                captureDc();
            }
            if (_maskTimer < 10)
                _maskTimer++;
            drawMask();

        }

        private void fadeOut()
        {
            if (_bFadeIn == true)
                _bFadeIn = false;
            if (_maskTimer > 1)
            {
                _maskTimer--;
                drawMask();
            }
            else
            {
                Control ct = Control.FromHandle(_hParentWnd);
                if (ct != null)
                    ct.Refresh();
                this.Dispose();
            }
        }

        private void drawMask()
        {
            byte bt = 0;
            IntPtr hdc = IntPtr.Zero;
            cStoreDc tempDc = new cStoreDc();
            RECT tr = new RECT();

            GetWindowRect(_hControlWnd, ref tr);
            OffsetRect(ref tr, -tr.Left, -tr.Top);

            Rectangle bounds = new Rectangle(_areaRect.Left, _areaRect.Top, _areaRect.Right - _areaRect.Left, _areaRect.Bottom - _areaRect.Top);
            tempDc.Width = tr.Right;
            tempDc.Height = tr.Bottom;

            bt = (byte)(_maskTimer * 15);
            BitBlt(tempDc.Hdc, 0, 0, tr.Right, tr.Bottom, _cMaskDc.Hdc, 0, 0, 0xCC0020);
            using (AlphaStretch al = new AlphaStretch(_cTransitionDc.Hdc, tempDc.Hdc, new Rectangle(0, 0, _cTransitionDc.Width, _cTransitionDc.Height), bounds, 2, bt)) { }
            hdc = GetDC(_hControlWnd);
            BitBlt(hdc, 0, 0, tr.Right, tr.Bottom, tempDc.Hdc, 0, 0, 0xCC0020);
            ReleaseDC(_hControlWnd, hdc);
            tempDc.Dispose();
            ValidateRect(_hControlWnd, ref tr);
        }

        private void captureDc()
        {
            RECT tr = new RECT();
            GetWindowRect(_hControlWnd, ref tr);

            _cMaskDc.Width = tr.Right - tr.Left;
            _cMaskDc.Height = tr.Bottom - tr.Top;
            if (_cMaskDc.Hdc != IntPtr.Zero)
            {
                using (Graphics g = Graphics.FromHdc(_cMaskDc.Hdc))
                    g.CopyFromScreen(tr.Left, tr.Top, 0, 0, new Size(_cMaskDc.Width, _cMaskDc.Height), CopyPixelOperation.SourceCopy);
            }
        }

        private bool inArea()
        {
            Point pt = new Point();
            RECT tr = new RECT(_areaRect.Left, _areaRect.Top, _areaRect.Right, _areaRect.Bottom);
            GetCursorPos(ref pt);
            ScreenToClient(_hParentWnd, ref pt);
            if (PtInRect(ref tr, pt))
                return true;
            return false;
        }
        #endregion

        #region WndProc
        protected override void WndProc(ref Message m)
        {
            PAINTSTRUCT ps = new PAINTSTRUCT();

            switch (m.Msg)
            {
                case WM_PAINT:
                    if (!_bPainting && _maskTimer > 1)
                    {
                        _bPainting = true;
                        // start painting engine
                        BeginPaint(m.HWnd, ref ps);
                        drawMask();
                        // done
                        EndPaint(m.HWnd, ref ps);
                        _bPainting = false;
                    }
                    else
                    {
                        base.WndProc(ref m);
                    }
                    break;

                case WM_TIMER:
                    if ((_safeTimer > 50) && (!inArea()))
                    {
                        stopTimer();
                    }
                    else
                    {
                        if (_bFadeIn == true)
                            fadeIn();
                        else
                            fadeOut();
                    }
                    _safeTimer++;
                    base.WndProc(ref m);
                    break;

                case WM_MOUSEMOVE:
                    if (inArea())
                        fadeIn();
                    else
                        fadeOut();
                    base.WndProc(ref m);
                    break;

                case WM_MOUSELEAVE:
                    fadeOut();
                    base.WndProc(ref m);
                    break;

                case WM_LBUTTONDOWN:
                    Dispose();
                    base.WndProc(ref m);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        #endregion
    }
}
