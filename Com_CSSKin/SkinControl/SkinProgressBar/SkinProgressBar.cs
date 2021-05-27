
using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using Com_CSSkin.Win32;
using Com_CSSkin.SkinClass;
using Com_CSSkin.Win32.Const;
using Com_CSSkin.Win32.Struct;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(ProgressBar))]
    public class SkinProgressBar : ProgressBar
    {
        #region 变量
        private BufferedGraphicsContext _context;
        private BufferedGraphics _bufferedGraphics;
        private bool _bPainting = false;
        private int _trackX = -100;
        private Timer _timer;
        private string _formatString = "{0:0.0%}";
        private const int Internal = 10;
        private const int MarqueeWidth = 100;
        #endregion

        #region 无参构造
        public SkinProgressBar()
            : base() {
            _context = BufferedGraphicsManager.Current;
            _context.MaximumBuffer = new Size(Width + 1, Height + 1);
            _bufferedGraphics = _context.Allocate(
                CreateGraphics(),
                new Rectangle(Point.Empty, Size));
            ForeColor = Color.Red;
            //减少闪烁
            SetStyles();
        }
        #endregion

        #region 减少闪烁方法
        private void SetStyles() {
            //设置自定义控件Style
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            UpdateStyles();
        }
        #endregion

        #region 枚举
        /// <summary>
        /// 文本的格式
        /// </summary>
        public enum TxtFormat
        {
            /// <summary>
            ///  没有文本。
            /// </summary>
            None = 0,
            /// <summary>
            /// 百分比。
            /// </summary>
            Percentage = 1,
            /// <summary>
            /// 比例。
            /// </summary>
            Proportion = 2
        }
        #endregion

        #region 属性
        private bool create = false;
        /// <summary>
        /// 是否开启不规则控件
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(bool), "false")]
        [Description("是否开启:根据所绘图限制控件范围")]
        public bool Create {
            get { return create; }
            set {
                if (create != value) {
                    create = value;
                    this.Invalidate();
                }
            }
        }

        private bool barGlass = true;
        [DefaultValue(typeof(bool), "true")]
        [Category("Bar")]
        [Description("滚动条是否启用颜色渐变")]
        public bool BarGlass {
            get { return barGlass; }
            set {
                if (barGlass != value) {
                    barGlass = value;
                    base.Invalidate();
                }
            }
        }


        private bool glass = true;
        [DefaultValue(typeof(bool), "true")]
        [Category("Skin")]
        [Description("控件是否启用颜色渐变")]
        public bool Glass {
            get { return glass; }
            set {
                if (glass != value) {
                    glass = value;
                    base.Invalidate();
                }
            }
        }

        private BackStyle barBackStyle = BackStyle.Tile;
        [DefaultValue(typeof(BackStyle), "0")]
        [Category("Bar")]
        [Description("进度条的图像绘制模式")]
        public BackStyle BarBackStyle {
            get { return barBackStyle; }
            set {
                if (barBackStyle != value) {
                    barBackStyle = value;
                    base.Invalidate();
                }
            }
        }

        private Size barMinusSize = new Size(1, 1);
        [DefaultValue(typeof(Size), "1,1")]
        [Category("Bar")]
        [Description("自减宽高。")]
        public Size BarMinusSize {
            get { return barMinusSize; }
            set {
                if (barMinusSize != value) {

                    barMinusSize = value;
                    base.Invalidate();
                }
            }
        }

        private TxtFormat textFormat = TxtFormat.Percentage;
        [Category("Skin")]
        [DefaultValue(typeof(TxtFormat), "1")]
        [Description("进度条文本格式")]
        public TxtFormat TextFormat {
            get { return textFormat; }
            set {
                if (textFormat != value) {
                    textFormat = value;
                    base.Invalidate();
                }
            }
        }

        private int radius = 2;
        [Category("Skin")]
        [DefaultValue(typeof(int), "2")]
        [Description("控件圆角大小")]
        public int Radius {
            get { return radius; }
            set {
                if (radius != value) {
                    radius = value < 1 ? 1 : value;
                    SetRegion();
                    base.Invalidate();
                }
            }
        }

        private RoundStyle radiusStyle = RoundStyle.All;
        [Category("Skin")]
        [DefaultValue(typeof(RoundStyle), "1")]
        [Description("控件圆角样式")]
        public RoundStyle RadiusStyle {
            get { return radiusStyle; }
            set {
                if (radiusStyle != value) {
                    radiusStyle = value;
                    SetRegion();
                    base.Invalidate();
                }
            }
        }

        private int barradius = 2;
        [Category("Bar")]
        [DefaultValue(typeof(int), "2")]
        [Description("进度条圆角大小")]
        public int BarRadius {
            get { return barradius; }
            set {
                if (barradius != value) {
                    barradius = value < 1 ? 1 : value;
                    base.Invalidate();
                }
            }
        }

        private RoundStyle barradiusStyle = RoundStyle.All;
        [Category("Bar")]
        [DefaultValue(typeof(RoundStyle), "1")]
        [Description("进度条圆角样式")]
        public RoundStyle BarRadiusStyle {
            get { return barradiusStyle; }
            set {
                if (barradiusStyle != value) {
                    barradiusStyle = value;
                    base.Invalidate();
                }
            }
        }

        private Image back;
        [Category("Skin")]
        [Description("控件背景图片")]
        public Image Back {
            get { return back; }
            set {
                if (back != value) {
                    back = value;
                    base.Invalidate();
                }
            }
        }

        private Image barBack;
        [Category("Bar")]
        [Description("进度条图片")]
        public Image BarBack {
            get { return barBack; }
            set {
                if (barBack != value) {
                    barBack = value;
                    base.Invalidate();
                }
            }
        }

        /// <summary>
        /// 显示进度完成信息的格式化字符串。
        /// </summary>
        [Category("Skin")]
        [Description("进度百分比格式")]
        [DefaultValue("{0:0.0%}")]
        public string FormatString {
            get { return _formatString; }
            set {
                if (_formatString != value) {
                    _formatString = value;
                    base.Invalidate();
                }
            }
        }

        private Color _trackBack = Color.FromArgb(185, 185, 185);
        [Category("Skin")]
        [DefaultValue(typeof(Color), "185, 185, 185")]
        public Color TrackBack {
            get {
                return _trackBack;
            }
            set {
                if (_trackBack != value) {
                    _trackBack = value;
                    base.Invalidate();
                }
            }
        }

        private Color _trackFore = Color.FromArgb(15, 181, 43);
        [Category("Bar")]
        [DefaultValue(typeof(Color), "15, 181, 43")]
        public Color TrackFore {
            get {
                return _trackFore;
            }
            set {
                if (_trackFore != value) {
                    _trackFore = value;
                    base.Invalidate();
                }
            }
        }

        private Color _border = Color.FromArgb(158, 158, 158);
        [Category("Skin")]
        [DefaultValue(typeof(Color), "158, 158, 158")]
        public Color Border {
            get {
                return _border;
            }
            set {
                if (_border != value) {
                    _border = value;
                    base.Invalidate();
                }
            }
        }

        private Color _innerBorder = Color.FromArgb(200, 250, 250, 250);
        [Category("Skin")]
        [DefaultValue(typeof(Color), "200, 250, 250, 250")]
        public Color InnerBorder {
            get {
                return _innerBorder;
            }
            set {
                if (_innerBorder != value) {
                    _innerBorder = value;
                    base.Invalidate();
                }
            }
        }

        public new ProgressBarStyle Style {
            get { return base.Style; }
            set {
                if (base.Style != value) {
                    base.Style = value;

                    if (value == ProgressBarStyle.Marquee) {
                        if (_timer != null) {
                            _timer.Dispose();
                        }

                        _timer = new Timer();
                        _timer.Interval = Internal;
                        _timer.Tick += delegate(object sender, EventArgs e) {
                            _trackX += (int)Math.Ceiling((float)Width / base.MarqueeAnimationSpeed);
                            if (_trackX > Width) {
                                _trackX = -MarqueeWidth;
                            }
                            base.Invalidate();
                        };

                        if (!base.DesignMode) {
                            _timer.Start();
                        }
                    } else {
                        if (_timer != null) {
                            _timer.Dispose();
                            _timer = null;
                        }
                    }
                }
            }
        }

        [Browsable(true)]
        public override Font Font {
            get { return base.Font; }
            set { base.Font = value; }
        }
        #endregion

        #region 重载事件
        //控件首次创建时被调用
        protected override void OnCreateControl() {
            base.OnCreateControl();
            SetRegion();
        }

        //改变窗体大小时
        protected override void OnResize(EventArgs e) {
            base.OnResize(e);

            SetRegion();

            _context.MaximumBuffer = new Size(Width + 1, Height + 1);
            if (_bufferedGraphics != null) {
                _bufferedGraphics.Dispose();
                _bufferedGraphics = null;
            }

            _bufferedGraphics = _context.Allocate(
                CreateGraphics(),
                new Rectangle(Point.Empty, Size));
        }

        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case WM.WM_PAINT:
                    if (!_bPainting) {
                        _bPainting = true;

                        PAINTSTRUCT ps = new PAINTSTRUCT();

                        NativeMethods.BeginPaint(m.HWnd, ref ps);

                        try {
                            DrawProgressBar(m.HWnd);
                        } catch {
                        }

                        NativeMethods.ValidateRect(m.HWnd, ref ps.rcPaint);
                        NativeMethods.EndPaint(m.HWnd, ref ps);

                        _bPainting = false;
                        m.Result = Result.TRUE;
                    } else {
                        base.WndProc(ref m);
                    }
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            if (disposing) {
                if (_timer != null) {
                    _timer.Dispose();
                    _timer = null;
                }

                if (_bufferedGraphics != null) {
                    _bufferedGraphics.Dispose();
                    _bufferedGraphics = null;
                }

                if (_context != null) {
                    _context = null;
                }
            }
        }

        #endregion

        #region 绘画方法
        private void DrawProgressBar(IntPtr hWnd) {
            Graphics g = _bufferedGraphics.Graphics;
            //最高质量绘制文字
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.Clear(Color.Transparent);
            Rectangle rect = new Rectangle(Point.Empty, Size);

            bool bBlock = Style != ProgressBarStyle.Marquee || base.DesignMode;
            float basePosition = bBlock ? .30f : .45f;

            SmoothingModeGraphics sg = new SmoothingModeGraphics(g);
            if (Back != null) {
                Bitmap btm = new Bitmap(Back, this.Size);
                if (Create) {
                    SetRegion(btm);
                }
                g.DrawImage(Back, rect);
            } else {
                PBarRenderHelper.RenderBackgroundInternal(
                    g,
                    rect,
                    TrackBack,
                    Border,
                    InnerBorder,
                    RadiusStyle,
                    Radius,
                    basePosition,
                    true,
                    Glass,
                    LinearGradientMode.Vertical);
            }
            Rectangle trackRect = rect;
            trackRect.Inflate(-BarMinusSize.Width, -BarMinusSize.Height);
            if (bBlock) {
                trackRect.Width = (int)(((double)Value / (Maximum - Minimum)) * trackRect.Width);
                if (BarBack != null) {
                    if (BarBackStyle == BackStyle.Tile) {
                        using (TextureBrush Txbrus = new TextureBrush(BarBack)) {
                            Txbrus.WrapMode = System.Drawing.Drawing2D.WrapMode.Tile;
                            g.FillRectangle(Txbrus, trackRect);
                        }

                    } else {
                        Bitmap btm = new Bitmap(BarBack, this.Size);
                        g.DrawImageUnscaledAndClipped(btm, trackRect);
                    }
                } else {
                    trackRect.Height -= 1;
                    PBarRenderHelper.RenderBackgroundInternal(
                        g,
                        trackRect,
                        TrackFore,
                        Border,
                        InnerBorder,
                        BarRadiusStyle,
                        BarRadius,
                        basePosition,
                        false,
                        BarGlass,
                        LinearGradientMode.Vertical);
                }
                if (!string.IsNullOrEmpty(_formatString) && TextFormat == TxtFormat.Percentage) {
                    TextRenderer.DrawText(
                        g,
                        string.Format(_formatString, (double)Value / (Maximum - Minimum)),
                        base.Font,
                        rect,
                        base.ForeColor,
                        TextFormatFlags.VerticalCenter |
                        TextFormatFlags.HorizontalCenter |
                        TextFormatFlags.SingleLine |
                        TextFormatFlags.WordEllipsis);
                } else if (TextFormat == TxtFormat.Proportion) {
                    TextRenderer.DrawText(
                            g,
                            Value + "/" + Maximum,
                            base.Font,
                            rect,
                            base.ForeColor,
                            TextFormatFlags.VerticalCenter |
                            TextFormatFlags.HorizontalCenter |
                            TextFormatFlags.SingleLine |
                            TextFormatFlags.WordEllipsis);
                }
            } else {
                GraphicsState state = g.Save();

                g.SetClip(trackRect);

                trackRect.X = _trackX;
                trackRect.Width = MarqueeWidth;

                using (GraphicsPath path = new GraphicsPath()) {
                    path.AddEllipse(trackRect);
                    g.SetClip(path, CombineMode.Intersect);
                }

                PBarRenderHelper.RenderBackgroundInternal(
                    g,
                    trackRect,
                    TrackFore,
                    Border,
                    InnerBorder,
                    RoundStyle.None,
                    8,
                    basePosition,
                    false,
                    BarGlass,
                    LinearGradientMode.Vertical);

                using (LinearGradientBrush brush = new LinearGradientBrush(
                    trackRect, InnerBorder, Color.Transparent, 0f)) {
                    Blend blend = new Blend();
                    blend.Factors = new float[] { 0f, 1f, 0f };
                    blend.Positions = new float[] { 0f, .5f, 1f };
                    brush.Blend = blend;

                    g.FillRectangle(brush, trackRect);
                }

                g.Restore(state);
            }

            sg.Dispose();

            IntPtr hDC = NativeMethods.GetDC(hWnd);
            _bufferedGraphics.Render(hDC);
            NativeMethods.ReleaseDC(hWnd, hDC);
        }

        //上一次操作记录
        Bitmap UpBtm = null;
        Rectangle UpRc = Rectangle.Empty;
        int? UpRadius = null;
        RoundStyle? UpRoundStyle = null;
        //实现圆角
        private void SetRegion(Bitmap btm = null) {
            if (Create) {
                //对比是否需要变动
                if (btm != UpBtm) {
                    if (btm != null) {
                        SkinTools.CreateControlRegion(this, btm, 200);
                        //赋值操作记录
                        UpBtm = btm;
                    }
                }
            } else {
                Rectangle rc = new Rectangle(Point.Empty, Size);
                //对比是否需要变动
                if (rc != UpRc || Radius != UpRadius || RadiusStyle != UpRoundStyle) {
                    //绘制圆角
                    SkinTools.CreateRegion(this, rc, radius, RadiusStyle);
                    //赋值操作记录
                    UpRc = rc;
                    UpRadius = Radius;
                    UpRoundStyle = RadiusStyle;
                }
            }
        }
        #endregion
    }
}
