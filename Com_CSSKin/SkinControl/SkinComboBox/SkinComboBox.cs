
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Com_CSSkin.SkinClass;
using Com_CSSkin.Win32;
using Com_CSSkin.Win32.Struct;
using Com_CSSkin.Win32.Const;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(ComboBox))]
    public class SkinComboBox : ComboBox
    {
        #region 变量
        private EditNativeWindow _editNativeWindow;
        private bool _bPainting;
        #endregion

        #region 无参构造
        public SkinComboBox()
            : base() {
            base.SetStyle(
               ControlStyles.AllPaintingInWmPaint |
               ControlStyles.OptimizedDoubleBuffer |
               ControlStyles.ResizeRedraw |
               ControlStyles.DoubleBuffer, true);
            this.DrawMode = DrawMode.OwnerDrawFixed;
        }
        #endregion

        #region 属性
        private Color mouseColor = Color.FromArgb(62, 151, 216);
        [Browsable(true), Category("DropDown")]
        [DefaultValue(typeof(Color), "62, 151, 216")]
        [Description("项被选中后的高亮度颜色")]
        public Color MouseColor {
            get { return mouseColor; }
            set {
                mouseColor = value;
                this.Invalidate();
            }
        }

        private Color mouseGradientColor = Color.FromArgb(51, 137, 201);
        [DefaultValue(typeof(Color), "51, 137, 201")]
        [Browsable(true), Category("DropDown"), Description("项被选中后的渐变颜色")]
        public Color MouseGradientColor {
            get { return mouseGradientColor; }
            set {
                mouseGradientColor = value;
                this.Invalidate();
            }
        }

        private Color dropBackColor = Color.White;
        [DefaultValue(typeof(Color), "White")]
        [Browsable(true), Category("DropDown"), Description("下拉框背景色")]
        public Color DropBackColor {
            get { return dropBackColor; }
            set {
                dropBackColor = value;
                this.Invalidate();
            }
        }

        private Color itemBorderColor = Color.CornflowerBlue;
        [DefaultValue(typeof(Color), "CornflowerBlue")]
        [Browsable(true), Category("DropDown"), Description("项被选中时的边框颜色")]
        public Color ItemBorderColor {
            get { return itemBorderColor; }
            set {
                itemBorderColor = value;
                this.Invalidate();
            }
        }

        private Color itemHoverForeColor = Color.White;
        [DefaultValue(typeof(Color), "White")]
        [Browsable(true), Category("DropDown"), Description("项被选中时的字体颜色")]
        public Color ItemHoverForeColor {
            get { return itemHoverForeColor; }
            set {
                itemHoverForeColor = value;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 水印文字
        /// </summary>
        private string _waterText = string.Empty;
        [Description("水印文字"), Category("Skin")]
        public string WaterText {
            get { return this._waterText; }
            set {
                this._waterText = value;
                base.Invalidate();
            }
        }
        /// <summary>
        /// 水印的颜色
        /// </summary>
        private Color _waterColor = Color.FromArgb(127, 127, 127);
        [Description("水印的颜色"), Category("Skin")]
        [DefaultValue(typeof(Color), "127, 127, 127")]
        public Color WaterColor {
            get { return this._waterColor; }
            set {
                this._waterColor = value;
                base.Invalidate();
            }
        }

        private Color _baseColor = Color.FromArgb(51, 161, 224);
        [DefaultValue(typeof(Color), "51, 161, 224"), Category("Base"), Description("下拉按钮背景色")]
        public Color BaseColor {
            get { return _baseColor; }
            set {
                if (_baseColor != value) {
                    _baseColor = value;
                    base.Invalidate();
                }
            }
        }

        private Color _borderColor = Color.FromArgb(51, 161, 224);
        [DefaultValue(typeof(Color), "51, 161, 224"), Category("Base"), Description("边框颜色")]
        public Color BorderColor {
            get { return _borderColor; }
            set {
                if (_borderColor != value) {
                    _borderColor = value;
                    base.Invalidate();
                }
            }
        }

        private Color _arrowColor = Color.FromArgb(19, 88, 128);
        [DefaultValue(typeof(Color), "19, 88, 128"), Category("Base"), Description("箭头颜色")]
        public Color ArrowColor {
            get { return _arrowColor; }
            set {
                if (_arrowColor != value) {
                    _arrowColor = value;
                    base.Invalidate();
                }
            }
        }

        private ControlState _buttonState;
        internal ControlState ButtonState {
            get { return _buttonState; }
            set {
                if (_buttonState != value) {
                    _buttonState = value;
                    Invalidate(ButtonRect);
                }
            }
        }

        internal Rectangle ButtonRect {
            get {
                return GetDropDownButtonRect();
            }
        }

        internal bool ButtonPressed {
            get {
                if (IsHandleCreated) {
                    return GetComboBoxButtonPressed();
                }
                return false;
            }
        }

        private IntPtr _editHandle;
        internal IntPtr EditHandle {
            get { return _editHandle; }
        }

        internal Rectangle EditRect {
            get {
                if (DropDownStyle == ComboBoxStyle.DropDownList) {
                    Rectangle rect = new Rectangle(
                        3, 3, Width - ButtonRect.Width - 6, Height - 6);
                    if (RightToLeft == RightToLeft.Yes) {
                        rect.X += ButtonRect.Right;
                    }
                    return rect;
                }
                if (IsHandleCreated && EditHandle != IntPtr.Zero) {
                    RECT rcClient = new RECT();
                    NativeMethods.GetWindowRect(EditHandle, ref rcClient);
                    return RectangleToClient(rcClient.Rect);
                }
                return Rectangle.Empty;
            }
        }

        #endregion

        #region 重载事件
        protected override void OnDrawItem(DrawItemEventArgs e) {
            base.OnDrawItem(e);
            if (e.Index == -1) { return; }
            Graphics g = e.Graphics;
            //最高质量绘制文字
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            //鼠标选中在这个项上
            if ((e.State & DrawItemState.Selected) != 0) {
                //渐变画刷
                LinearGradientBrush brush = new LinearGradientBrush(e.Bounds, MouseColor,
                                                 mouseGradientColor, LinearGradientMode.Vertical);
                Rectangle borderRect = new Rectangle(1, e.Bounds.Y + 1, e.Bounds.Width - 3, e.Bounds.Height - 3);

                g.FillRectangle(brush, borderRect);

                //画边框
                Pen pen = new Pen(ItemBorderColor);
                g.DrawRectangle(pen, borderRect);
            } else {
                SolidBrush brush = new SolidBrush(DropBackColor);
                g.FillRectangle(brush, e.Bounds);
            }

            //获得项文本内容,绘制文本
            String itemText = this.GetItemText(this.Items[e.Index]);

            //文本格式垂直居中
            Color fore = (e.State & DrawItemState.Selected) != 0 ? ItemHoverForeColor : ForeColor;
            StringFormat strFormat = new StringFormat();
            strFormat.LineAlignment = StringAlignment.Center;
            g.DrawString(itemText, Font, new SolidBrush(fore), e.Bounds, strFormat);
        }

        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            NativeMethods.ComboBoxInfo cbi = new NativeMethods.ComboBoxInfo();
            cbi.cbSize = Marshal.SizeOf(cbi);
            NativeMethods.GetComboBoxInfo(base.Handle, ref cbi);
            _editHandle = cbi.hwndEdit;
            if (DropDownStyle != ComboBoxStyle.DropDownList) {
                _editNativeWindow = new EditNativeWindow(this);
            }
        }

        protected override void OnHandleDestroyed(EventArgs e) {
            base.OnHandleDestroyed(e);
            if (_editNativeWindow != null) {
                _editNativeWindow.Dispose();
                _editNativeWindow = null;
            }
        }

        protected override void OnCreateControl() {
            base.OnCreateControl();

            NativeMethods.ComboBoxInfo cbi = GetComboBoxInfo();
            _editHandle = cbi.hwndEdit;
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            Point point = e.Location;
            if (ButtonRect.Contains(point)) {
                ButtonState = ControlState.Hover;
            } else {
                ButtonState = ControlState.Normal;
            }
        }

        protected override void OnMouseEnter(EventArgs e) {
            base.OnMouseEnter(e);

            Point point = PointToClient(Cursor.Position);
            if (ButtonRect.Contains(point)) {
                ButtonState = ControlState.Hover;
            }
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);

            ButtonState = ControlState.Normal;
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            ButtonState = ControlState.Normal;
        }
        #endregion

        #region 拦截消息并处理
        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case WM.WM_PAINT:
                    WmPaint(ref m);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        //处理重绘事件
        private void WmPaint(ref Message m) {
            if (base.DropDownStyle == ComboBoxStyle.Simple) {
                base.WndProc(ref m);
                return;
            }

            if (base.DropDownStyle == ComboBoxStyle.DropDown) {
                if (!_bPainting) {
                    PAINTSTRUCT ps =
                        new PAINTSTRUCT();

                    _bPainting = true;
                    NativeMethods.BeginPaint(m.HWnd, ref ps);

                    RenderComboBox(ref m);

                    NativeMethods.EndPaint(m.HWnd, ref ps);
                    _bPainting = false;
                    m.Result = Result.TRUE;
                } else {
                    base.WndProc(ref m);
                }
            } else {
                base.WndProc(ref m);
                RenderComboBox(ref m);
            }
        }
        #endregion

        #region 绘制方法
        private void RenderComboBox(ref Message m) {
            Rectangle rect = new Rectangle(Point.Empty, Size);
            Rectangle buttonRect = ButtonRect;
            ControlState state = ButtonPressed ?
                ControlState.Pressed : ButtonState;
            using (Graphics g = Graphics.FromHwnd(m.HWnd)) {
                RenderComboBoxBackground(g, rect, buttonRect);
                RenderConboBoxDropDownButton(g, ButtonRect, state);
                RenderConboBoxBorder(g, rect);
            }
        }

        private void RenderConboBoxBorder(
            Graphics g, Rectangle rect) {
            Color borderColor = base.Enabled ?
                _borderColor : SystemColors.ControlDarkDark;
            using (Pen pen = new Pen(borderColor)) {
                rect.Width--;
                rect.Height--;
                g.DrawRectangle(pen, rect);
            }
        }

        private void RenderComboBoxBackground(
            Graphics g,
            Rectangle rect,
            Rectangle buttonRect) {
            Color backColor = base.Enabled ?
                base.BackColor : SystemColors.Control;
            using (SolidBrush brush = new SolidBrush(backColor)) {
                buttonRect.Inflate(-1, -1);
                rect.Inflate(-1, -1);
                using (Region region = new Region(rect)) {
                    region.Exclude(buttonRect);
                    region.Exclude(EditRect);
                    g.FillRegion(brush, region);
                }
            }
        }

        private void RenderConboBoxDropDownButton(
            Graphics g,
            Rectangle buttonRect,
            ControlState state) {
            Color baseColor;
            Color backColor = Color.FromArgb(160, 250, 250, 250);
            Color borderColor = base.Enabled ?
                _borderColor : SystemColors.ControlDarkDark;
            Color arrowColor = base.Enabled ?
                _arrowColor : SystemColors.ControlDarkDark;
            Rectangle rect = buttonRect;

            if (base.Enabled) {
                switch (state) {
                    case ControlState.Hover:
                        baseColor = RenderHelper.GetColor(
                            _baseColor, 0, -33, -22, -13);
                        break;
                    case ControlState.Pressed:
                        baseColor = RenderHelper.GetColor(
                            _baseColor, 0, -65, -47, -25);
                        break;
                    default:
                        baseColor = _baseColor;
                        break;
                }
            } else {
                baseColor = SystemColors.ControlDark;
            }

            rect.Inflate(-1, -1);

            RenderScrollBarArrowInternal(
                g,
                rect,
                baseColor,
                borderColor,
                backColor,
                arrowColor,
                RoundStyle.None,
                true,
                false,
                ArrowDirection.Down,
                LinearGradientMode.Vertical);
        }

        internal void RenderScrollBarArrowInternal(
           Graphics g,
           Rectangle rect,
           Color baseColor,
           Color borderColor,
           Color innerBorderColor,
           Color arrowColor,
           RoundStyle roundStyle,
           bool drawBorder,
           bool drawGlass,
           ArrowDirection arrowDirection,
           LinearGradientMode mode) {
            RenderHelper.RenderBackgroundInternal(
               g,
               rect,
               baseColor,
               borderColor,
               innerBorderColor,
               roundStyle,
               0,
               .45F,
               drawBorder,
               drawGlass,
               mode);

            using (SolidBrush brush = new SolidBrush(arrowColor)) {
                RenderArrowInternal(
                    g,
                    rect,
                    arrowDirection,
                    brush);
            }
        }

        internal void RenderArrowInternal(
            Graphics g,
            Rectangle dropDownRect,
            ArrowDirection direction,
            Brush brush) {
            Point point = new Point(
                dropDownRect.Left + (dropDownRect.Width / 2),
                dropDownRect.Top + (dropDownRect.Height / 2));
            Point[] points = null;
            switch (direction) {
                case ArrowDirection.Left:
                    points = new Point[] { 
                        new Point(point.X + 2, point.Y - 3), 
                        new Point(point.X + 2, point.Y + 3), 
                        new Point(point.X - 1, point.Y) };
                    break;

                case ArrowDirection.Up:
                    points = new Point[] { 
                        new Point(point.X - 3, point.Y + 2), 
                        new Point(point.X + 3, point.Y + 2), 
                        new Point(point.X, point.Y - 2) };
                    break;

                case ArrowDirection.Right:
                    points = new Point[] {
                        new Point(point.X - 2, point.Y - 3), 
                        new Point(point.X - 2, point.Y + 3), 
                        new Point(point.X + 1, point.Y) };
                    break;

                default:
                    points = new Point[] {
                        new Point(point.X - 2, point.Y - 1), 
                        new Point(point.X + 3, point.Y - 1), 
                        new Point(point.X, point.Y + 2) };
                    break;
            }
            g.FillPolygon(brush, points);
        }

        #endregion

        #region 返回方法

        private NativeMethods.ComboBoxInfo GetComboBoxInfo() {
            NativeMethods.ComboBoxInfo cbi = new NativeMethods.ComboBoxInfo();
            cbi.cbSize = Marshal.SizeOf(cbi);
            NativeMethods.GetComboBoxInfo(base.Handle, ref cbi);
            return cbi;
        }

        private bool GetComboBoxButtonPressed() {
            NativeMethods.ComboBoxInfo cbi = GetComboBoxInfo();
            return cbi.stateButton ==
                NativeMethods.ComboBoxButtonState.STATE_SYSTEM_PRESSED;
        }

        private Rectangle GetDropDownButtonRect() {
            NativeMethods.ComboBoxInfo cbi = GetComboBoxInfo();
            return cbi.rcButton.Rect;
        }

        #endregion

        #region 提供窗口句柄和窗口过程的低级封装
        private class EditNativeWindow : NativeWindow, IDisposable
        {
            private SkinComboBox _owner;
            private const int WM_PAINT = 0xF;

            public EditNativeWindow(SkinComboBox owner)
                : base() {
                _owner = owner;
                AssignHandle(_owner.EditHandle);
            }

            [DllImport("user32.dll")]
            private static extern IntPtr GetDC(IntPtr ptr);

            [DllImport("user32.dll")]
            private static extern int ReleaseDC(IntPtr hwnd, IntPtr hDC);

            protected override void WndProc(ref Message m) {
                base.WndProc(ref m);
                if (m.Msg == WM_PAINT) {
                    IntPtr handle = m.HWnd;
                    IntPtr hdc = GetDC(handle);
                    if (hdc == IntPtr.Zero) {
                        return;
                    }
                    try {
                        using (Graphics graphics = Graphics.FromHdc(hdc)) {
                            if (_owner.Text.Length == 0
                                && !_owner.Focused
                                && !string.IsNullOrEmpty(_owner.WaterText)) {
                                TextFormatFlags format =
                                    TextFormatFlags.EndEllipsis |
                                    TextFormatFlags.VerticalCenter;

                                if (_owner.RightToLeft == RightToLeft.Yes) {
                                    format |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
                                }

                                TextRenderer.DrawText(
                                    graphics,
                                    _owner.WaterText,
                                    new Font("微软雅黑", 8.5f),
                                    new Rectangle(0, 0, _owner.EditRect.Width, _owner.EditRect.Height),
                                    _owner.WaterColor,
                                    format);
                            }
                        }
                    } finally {
                        ReleaseDC(handle, hdc);
                    }
                }
            }
            #region IDisposable 成员

            public void Dispose() {
                ReleaseHandle();
                _owner = null;
            }

            #endregion
        }
        #endregion
    }
}
