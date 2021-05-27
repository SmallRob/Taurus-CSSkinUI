
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Com_CSSkin.Win32;
using Com_CSSkin.Win32.Struct;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(NumericUpDown))]
    public class SkinNumericUpDown : NumericUpDown
    {
        #region 变量
        private UpDownButtonNativeWindow _upDownButtonNativeWindow;
        private Color _baseColor = Color.FromArgb(166, 222, 255);
        private Color _borderColor = Color.FromArgb(23, 169, 254);
        private Color _arrowColor = Color.FromArgb(0, 95, 152);
        private static readonly object EventPaintUpDownButton = new object();
        #endregion

        #region 无参构造
        public SkinNumericUpDown()
            : base()
        {
        }
        #endregion

        #region 自定义事件
        public event UpDownButtonPaintEventHandler PaintUpDownButton
        {
            add { base.Events.AddHandler(EventPaintUpDownButton, value); }
            remove { base.Events.RemoveHandler(EventPaintUpDownButton, value); }
        }
        #endregion

        #region 属性
        [Category("Skin")]
        [Description("色调")]
        [DefaultValue(typeof(Color), "166, 222, 255")]
        public Color BaseColor
        {
            get { return _baseColor; }
            set 
            {
                _baseColor = value;
                UpDownButton.Invalidate();
            }
        }

        [Category("Skin")]
        [Description("边框颜色")]
        [DefaultValue(typeof(Color), "23, 169, 254")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set 
            { 
                _borderColor = value;
                base.Invalidate(true);
            }
        }

        [Category("Skin")]
        [Description("箭头颜色")]
        [DefaultValue(typeof(Color), "0, 95, 152")]
        public Color ArrowColor
        {
            get { return _arrowColor; }
            set 
            { 
                _arrowColor = value;
                UpDownButton.Invalidate();
            }
        }

        public override Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                base.Invalidate(true);
            }
        }

        public Control UpDownButton
        {
            get { return base.Controls[0]; }
        }
        #endregion

        #region 重载事件
        protected virtual void OnPaintUpDownButton(
            UpDownButtonPaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = e.ClipRectangle;

            Color upButtonBaseColor = _baseColor;
            Color upButtonBorderColor = _borderColor;
            Color upButtonArrowColor = _arrowColor;

            Color downButtonBaseColor = _baseColor;
            Color downButtonBorderColor = _borderColor;
            Color downButtonArrowColor = _arrowColor;

            Rectangle upButtonRect = rect;
            upButtonRect.Y += 1;
            upButtonRect.Width -= 2;
            upButtonRect.Height = rect.Height / 2 - 2;

            Rectangle downButtonRect = rect;
            downButtonRect.Y = upButtonRect.Bottom + 2;
            downButtonRect.Height = rect.Height - upButtonRect.Bottom - 4;
            downButtonRect.Width -= 2;

            if (Enabled)
            {
                if (e.MouseOver)
                {
                    if (e.MousePress)
                    {
                        if (e.MouseInUpButton)
                        {
                            upButtonBaseColor = GetColor(_baseColor, 0, -35, -24, -9);
                        }
                        else
                        {
                            downButtonBaseColor = GetColor(_baseColor, 0, -35, -24, -9);
                        }
                    }
                    else
                    {
                        if (e.MouseInUpButton)
                        {
                            upButtonBaseColor = GetColor(_baseColor, 0, 35, 24, 9);
                        }
                        else
                        {
                            downButtonBaseColor = GetColor(_baseColor, 0, 35, 24, 9);
                        }
                    }
                }
            }
            else
            {
                upButtonBaseColor = SystemColors.Control;
                upButtonBorderColor = SystemColors.ControlDark;
                upButtonArrowColor = SystemColors.ControlDark;

                downButtonBaseColor = SystemColors.Control;
                downButtonBorderColor = SystemColors.ControlDark;
                downButtonArrowColor = SystemColors.ControlDark;
            }

            g.SmoothingMode = SmoothingMode.AntiAlias;

            Color backColor = Enabled ? base.BackColor : SystemColors.Control;

            using (SolidBrush brush = new SolidBrush(backColor))
            {
                rect.Inflate(1, 1);
                g.FillRectangle(brush, rect);
            }

            RenderButton(
                g,
                upButtonRect,
                upButtonBaseColor,
                upButtonBorderColor,
                upButtonArrowColor,
                ArrowDirection.Up);
            RenderButton(
                g,
                downButtonRect,
                downButtonBaseColor,
                downButtonBorderColor,
                downButtonArrowColor,
                ArrowDirection.Down);

            UpDownButtonPaintEventHandler handler =
                base.Events[EventPaintUpDownButton] as UpDownButtonPaintEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (_upDownButtonNativeWindow == null)
            {
                _upDownButtonNativeWindow = new UpDownButtonNativeWindow(this);
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);
            if (_upDownButtonNativeWindow != null)
            {
                _upDownButtonNativeWindow.Dispose();
                _upDownButtonNativeWindow = null;
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case 0xF:
                    base.WndProc(ref m);
                    if (BorderStyle != BorderStyle.None)
                    {
                        Color borderColor = Enabled ?
                           _borderColor : SystemColors.ControlDark;
                        using (Graphics g = Graphics.FromHwnd(m.HWnd))
                        {
                            ControlPaint.DrawBorder(
                               g,
                               ClientRectangle,
                               borderColor,
                               ButtonBorderStyle.Solid);
                        }
                    }
                    break;
                case 0x85:
                    if (BorderStyle != BorderStyle.None)
                    {
                        Color backColor = Enabled ?
                            base.BackColor : SystemColors.Control;
                        Rectangle rect = new Rectangle(0, 0, Width, Height);

                        IntPtr hdc = NativeMethods.GetWindowDC(m.HWnd);
                        if (hdc == IntPtr.Zero)
                        {
                            throw new Win32Exception();
                        }
                        try
                        {
                            using (Graphics g = Graphics.FromHdc(hdc))
                            {
                                using (Brush brush = new SolidBrush(backColor))
                                {
                                    g.FillRectangle(brush, rect);
                                }
                            }
                        }
                        finally
                        {
                            NativeMethods.ReleaseDC(m.HWnd, hdc);
                        }
                    }
                    m.Result = IntPtr.Zero;
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        #endregion

        #region 绘画方法
        public void RenderArrowInternal(
             Graphics g,
             Rectangle dropDownRect,
             ArrowDirection direction,
             Brush brush)
        {
            Point point = new Point(
                dropDownRect.Left + (dropDownRect.Width / 2),
                dropDownRect.Top + (dropDownRect.Height / 2));
            Point[] points = null;
            switch (direction)
            {
                case ArrowDirection.Left:
                    points = new Point[] { 
                        new Point(point.X + 2, point.Y - 3), 
                        new Point(point.X + 2, point.Y + 3), 
                        new Point(point.X - 1, point.Y) };
                    break;

                case ArrowDirection.Up:
                    points = new Point[] { 
                        new Point(point.X - 3, point.Y + 1), 
                        new Point(point.X + 3, point.Y + 1), 
                        new Point(point.X, point.Y - 1) };
                    break;

                case ArrowDirection.Right:
                    points = new Point[] {
                        new Point(point.X - 2, point.Y - 3), 
                        new Point(point.X - 2, point.Y + 3), 
                        new Point(point.X + 1, point.Y) };
                    break;

                default:
                    points = new Point[] {
                        new Point(point.X - 3, point.Y - 1), 
                        new Point(point.X + 3, point.Y - 1), 
                        new Point(point.X, point.Y + 1) };
                    break;
            }
            g.FillPolygon(brush, points);
        }

        public void RenderButton(
            Graphics g,
            Rectangle rect,
            Color baseColor,
            Color borderColor,
            Color arrowColor,
            ArrowDirection direction)
        {
            RenderBackgroundInternal(
                g,
                rect,
                baseColor,
                borderColor,
                0.45f,
                true,
                LinearGradientMode.Vertical);
            using (SolidBrush brush = new SolidBrush(arrowColor))
            {
                RenderArrowInternal(
                    g,
                    rect,
                    direction,
                    brush);
            }
        }

        public void RenderBackgroundInternal(
          Graphics g,
          Rectangle rect,
          Color baseColor,
          Color borderColor,
          float basePosition,
          bool drawBorder,
          LinearGradientMode mode)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
               rect, Color.Transparent, Color.Transparent, mode))
            {
                Color[] colors = new Color[4];
                colors[0] = GetColor(baseColor, 0, 35, 24, 9);
                colors[1] = GetColor(baseColor, 0, 13, 8, 3);
                colors[2] = baseColor;
                colors[3] = GetColor(baseColor, 0, 68, 69, 54);

                ColorBlend blend = new ColorBlend();
                blend.Positions = 
                    new float[] { 0.0f, basePosition, basePosition + 0.05f, 1.0f };
                blend.Colors = colors;
                brush.InterpolationColors = blend;
                g.FillRectangle(brush, rect);
            }
            if (baseColor.A > 80)
            {
                Rectangle rectTop = rect;
                if (mode == LinearGradientMode.Vertical)
                {
                    rectTop.Height = (int)(rectTop.Height * basePosition);
                }
                else
                {
                    rectTop.Width = (int)(rect.Width * basePosition);
                }
                using (SolidBrush brushAlpha =
                    new SolidBrush(Color.FromArgb(80, 255, 255, 255)))
                {
                    g.FillRectangle(brushAlpha, rectTop);
                }
            }

            if (drawBorder)
            {
                using (Pen pen = new Pen(borderColor))
                {
                    g.DrawRectangle(pen, rect);
                }
            }
        }

        private Color GetColor(Color colorBase, int a, int r, int g, int b)
        {
            int a0 = colorBase.A;
            int r0 = colorBase.R;
            int g0 = colorBase.G;
            int b0 = colorBase.B;

            if (a + a0 > 255) { a = 255; } else { a = Math.Max(a + a0, 0); }
            if (r + r0 > 255) { r = 255; } else { r = Math.Max(r + r0, 0); }
            if (g + g0 > 255) { g = 255; } else { g = Math.Max(g + g0, 0); }
            if (b + b0 > 255) { b = 255; } else { b = Math.Max(b + b0, 0); }

            return Color.FromArgb(a, r, g, b);
        }
        #endregion

        #region UpDownButtonNativeWindow
        private class UpDownButtonNativeWindow : NativeWindow, IDisposable
        {
            #region 变量
            private SkinNumericUpDown _owner;
            private Control _upDownButton;
            private IntPtr _upDownButtonWnd;
            private bool _bPainting;
            private const int WM_PAINT = 0xF;
            private const int VK_LBUTTON = 0x1;
            private const int VK_RBUTTON = 0x2;
            private static readonly IntPtr TRUE = new IntPtr(1);
            #endregion

            #region 无参构造
            public UpDownButtonNativeWindow(SkinNumericUpDown owner)
                : base()
            {
                _owner = owner;
                _upDownButton = owner.UpDownButton;
                _upDownButtonWnd = _upDownButton.Handle;

                if (Environment.OSVersion.Version.Major > 5)
                {
                    if (NativeMethods.IsAppThemed())
                    {
                        NativeMethods.SetWindowTheme(_upDownButtonWnd, "", "");
                    }
                }
                base.AssignHandle(_upDownButtonWnd);
            }
            #endregion

            #region 私有方法
            private bool LeftKeyPressed()
            {
                if (SystemInformation.MouseButtonsSwapped)
                {
                    return (NativeMethods.GetKeyState(VK_RBUTTON) < 0);
                }
                else
                {
                    return (NativeMethods.GetKeyState(VK_LBUTTON) < 0);
                }
            }

            private void DrawUpDownButton()
            {
                bool mouseOver = false;
                bool mousePress = LeftKeyPressed();
                bool mouseInUpButton = false;

                Rectangle clipRect = _upDownButton.ClientRectangle;

                RECT windowRect = new RECT();
                NativeMethods.Point cursorPoint = new NativeMethods.Point();
                NativeMethods.GetCursorPos(ref cursorPoint);
                NativeMethods.GetWindowRect(_upDownButtonWnd, ref windowRect);

                mouseOver = NativeMethods.PtInRect(ref windowRect, cursorPoint);

                cursorPoint.x -= windowRect.Left;
                cursorPoint.y -= windowRect.Top;

                mouseInUpButton = cursorPoint.y < clipRect.Height / 2;

                using (Graphics g = Graphics.FromHwnd(_upDownButtonWnd))
                {
                    UpDownButtonPaintEventArgs e =
                        new UpDownButtonPaintEventArgs(
                        g,
                        clipRect,
                        mouseOver,
                        mousePress,
                        mouseInUpButton);
                    _owner.OnPaintUpDownButton(e);
                }
            }
            #endregion

            #region 重载事件
            //拦截消息
            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case WM_PAINT:
                        if (!_bPainting)
                        {
                            _bPainting = true;
                            PAINTSTRUCT ps = new PAINTSTRUCT();
                            NativeMethods.BeginPaint(m.HWnd, ref ps);
                            DrawUpDownButton();
                            NativeMethods.EndPaint(m.HWnd, ref ps);
                            _bPainting = false;
                            m.Result = TRUE;
                        }
                        else
                        {
                            base.WndProc(ref m);
                        }
                        break;

                    default:
                        base.WndProc(ref m);
                        break;
                }
            }
            #endregion

            #region IDisposable 成员
            public void Dispose()
            {
                _owner = null;
                _upDownButton = null;
                base.ReleaseHandle();
            }
            #endregion
        }
        #endregion
    }
}
