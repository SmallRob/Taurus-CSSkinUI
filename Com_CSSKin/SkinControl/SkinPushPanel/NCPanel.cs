
using System;
using System.Windows.Forms;
using System.ComponentModel;
using Com_CSSkin.Win32;
using Com_CSSkin.Win32.Const;
using Com_CSSkin.Win32.Struct;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    [Browsable(false)]
    [DesignTimeVisible(false)]
    [ToolboxItem(false)]
    public class NCPanel : Panel
    {
        private PanelColorTable _colorTable;
        private int _borderWidth = 2;
        private int _radius = 6;
        private RoundStyle _roundStyle = RoundStyle.All;

        public NCPanel()
            : base() {
            base.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
            base.AutoScroll = true;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PanelColorTable ColorTable {
            get {
                if (_colorTable == null) {
                    _colorTable = new PanelColorTable();
                }
                return _colorTable;
            }
            set {
                _colorTable = value;
                base.Invalidate();
            }
        }

        [DefaultValue(6)]
        public int Radius {
            get { return _radius; }
            set {
                if (value != _radius) {
                    _radius = value < 2 ? 2 : value;
                    Redraw();
                }
            }
        }

        [DefaultValue(typeof(RoundStyle), "15")]
        public RoundStyle RoundStyle {
            get { return _roundStyle; }
            set {
                if (value != _roundStyle) {
                    _roundStyle = value;
                    Redraw();
                }
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new BorderStyle BorderStyle {
            get { return base.BorderStyle; }
            set { }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AutoScroll {
            get { return base.AutoScroll; }
            set { base.AutoScroll = value; }
        }

        [DefaultValue(2)]
        public int BorderWidth {
            get { return _borderWidth; }
            set {
                if (_borderWidth != value) {
                    _borderWidth = value < 1 ? 1 : value;
                    ChangeFrame();
                }
            }
        }

        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case WM.WM_NCCALCSIZE:
                    WmNcCalcSize(ref m);
                    base.WndProc(ref m);
                    break;
                case WM.WM_NCPAINT:
                    base.WndProc(ref m);
                    WmNcPaint(ref m);
                    break;
                case WM.WM_PAINT:
                    base.WndProc(ref m);
                    WmNcPaint(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e) {
            if (base.BackgroundImage != null) {
                Graphics g = e.Graphics;
                Rectangle bounds = new Rectangle(Point.Empty, base.Size);
                Rectangle clientRect = base.ClientRectangle;

                WINDOWINFO info = GetWindowInfo();
                Point offset = new Point(
                    info.rcClient.Left - info.rcWindow.Left,
                    info.rcClient.Top - info.rcWindow.Top);

                IntPtr hDC = g.GetHdc();

                try {
                    using (ImageDc imageDc = new ImageDc(bounds.Width, bounds.Height)) {
                        using (Graphics gDc = Graphics.FromHdc(imageDc.Hdc)) {
                            ControlPaintEx.DrawBackgroundImage(
                                gDc,
                                base.BackgroundImage,
                                base.BackColor,
                                base.BackgroundImageLayout,
                                bounds,
                                bounds);
                        }

                        NativeMethods.BitBlt(
                            hDC,
                            clientRect.X,
                            clientRect.Y,
                            clientRect.Width,
                            clientRect.Height,
                            imageDc.Hdc,
                            offset.X,
                            offset.Y,
                            TernaryRasterOperations.SRCCOPY);
                    }
                } catch {
                } finally {
                    g.ReleaseHdc(hDC);
                }
                return;
            }

            if (base.BackColor == Color.Transparent) {
                Graphics g = e.Graphics;
                IntPtr hDC = g.GetHdc();

                try {
                    WINDOWINFO info = GetWindowInfo();
                    Point offset = new Point(
                        info.rcClient.Left - info.rcWindow.Left,
                        info.rcClient.Top - info.rcWindow.Top);

                    offset += (Size)base.Location;

                    DrawTransparentBackground(
                        hDC,
                        base.ClientRectangle,
                        offset);
                } catch {
                } finally {
                    g.ReleaseHdc(hDC);
                }
                return;
            }

            base.OnPaintBackground(e);
        }

        protected virtual void WmNcCalcSize(ref Message m) {
            if (m.WParam != IntPtr.Zero) {
                NCCALCSIZE_PARAMS ncsize = (NCCALCSIZE_PARAMS)m.GetLParam(
                    typeof(NCCALCSIZE_PARAMS));
                ncsize.rgrc0.Left += _borderWidth;
                ncsize.rgrc0.Right -= _borderWidth;
                ncsize.rgrc0.Top += _borderWidth;
                ncsize.rgrc0.Bottom -= _borderWidth;
                Marshal.StructureToPtr(ncsize, m.LParam, false);
            } else {
                RECT rc = (RECT)m.GetLParam(typeof(RECT));
                rc.Left += _borderWidth;
                rc.Right -= _borderWidth;
                rc.Top += _borderWidth;
                rc.Bottom -= _borderWidth;
                Marshal.StructureToPtr(rc, m.LParam, true);
            }
            m.Result = Result.FALSE;
        }

        protected virtual void WmNcPaint(ref Message m) {
            IntPtr hDC = NativeMethods.GetWindowDC(m.HWnd);

            if (hDC == IntPtr.Zero) {
                return;
            }

            try {
                Rectangle bounds = new Rectangle(Point.Empty, base.Size);
                Rectangle client = base.ClientRectangle;
                client.X = _borderWidth;
                client.Y = _borderWidth;

                using (ImageDc bufferedDC = new ImageDc(base.Width, base.Height)) {
                    NativeMethods.ExcludeClipRect(
                        bufferedDC.Hdc,
                        client.Left,
                        client.Top,
                        client.Right,
                        client.Bottom);

                    DrawTransparentBackground(
                        bufferedDC.Hdc,
                        bounds,
                        base.Location);

                    using (Graphics g = Graphics.FromHdc(bufferedDC.Hdc)) {
                        using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                            bounds, _radius, _roundStyle, true)) {
                            g.SetClip(path);
                        }
                        g.ExcludeClip(client);

                        if (base.BackgroundImage != null) {
                            ControlPaintEx.DrawBackgroundImage(
                                g,
                                base.BackgroundImage,
                                base.BackColor,
                                base.BackgroundImageLayout,
                                bounds,
                                bounds);
                        } else if (base.BackColor != Color.Transparent) {
                            using (Brush brush = new SolidBrush(base.BackColor)) {
                                g.FillRectangle(brush, bounds);
                            }
                        }

                        g.ResetClip();

                        using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                            bounds, _radius, _roundStyle, true)) {
                            using (SmoothingModeGraphics antiGraphics = new SmoothingModeGraphics(g)) {
                                g.DrawPath(Pens.Black, path);
                            }
                        }
                    }

                    NativeMethods.ExcludeClipRect(
                        hDC,
                        client.Left,
                        client.Top,
                        client.Right,
                        client.Bottom);

                    NativeMethods.BitBlt(
                        hDC,
                        bounds.X,
                        bounds.Y,
                        bounds.Width,
                        bounds.Height,
                        bufferedDC.Hdc,
                        0,
                        0,
                        TernaryRasterOperations.SRCCOPY);
                }
            } catch {
            } finally {
                NativeMethods.ReleaseDC(m.HWnd, hDC);
            }
        }

        internal WINDOWINFO GetWindowInfo() {
            WINDOWINFO info = new WINDOWINFO();
            info.cbSize = (uint)Marshal.SizeOf(info);

            NativeMethods.GetWindowInfo(base.Handle, ref info);
            return info;
        }

        protected void ChangeFrame() {
            NativeMethods.SetWindowPos(
                base.Handle,
                IntPtr.Zero,
                0, 0, 0, 0,
                SWP.SWP_NOMOVE | SWP.SWP_NOSIZE | SWP.SWP_NOZORDER |
                SWP.SWP_FRAMECHANGED | SWP.SWP_DRAWFRAME);
        }

        protected void Redraw() {
            NativeMethods.RedrawWindow(
                base.Handle,
                IntPtr.Zero,
                IntPtr.Zero,
                RDW.RDW_FRAME | RDW.RDW_INTERNALPAINT |
                RDW.RDW_UPDATENOW | RDW.RDW_NOCHILDREN);
        }

        protected void DrawTransparentBackground(
            IntPtr hDC, Rectangle bounds, Point offset) {
            if (base.Parent == null ||
                !base.Parent.IsHandleCreated ||
                base.Parent.IsDisposed) {
                return;
            }

            Size parentSize = base.Parent.ClientSize;

            using (ImageDc imageDc = new ImageDc(parentSize.Width, parentSize.Height)) {
                NativeMethods.SendMessage(
                    base.Parent.Handle,
                    WM.WM_ERASEBKGND,
                    imageDc.Hdc,
                    0);

                NativeMethods.SendMessage(
                    base.Parent.Handle,
                    WM.WM_PAINT,
                    imageDc.Hdc,
                    0);

                NativeMethods.BitBlt(
                    hDC,
                    bounds.X,
                    bounds.Y,
                    bounds.Width,
                    bounds.Height,
                    imageDc.Hdc,
                    offset.X,
                    offset.Y,
                    TernaryRasterOperations.SRCCOPY);
            }
        }
    }
}
