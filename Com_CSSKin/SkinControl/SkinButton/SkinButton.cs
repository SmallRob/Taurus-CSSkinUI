
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Com_CSSkin.SkinClass;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Threading;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(Button))]
    public partial class SkinButton : Button
    {
        #region 无参构造函数
        System.Windows.Forms.Timer timer = null;

        public System.Windows.Forms.Timer DhTimer {
            get {
                if (timer == null) {
                    //初始化计时器
                    timer = new System.Windows.Forms.Timer();
                    timer.Tick += new EventHandler(timer_Tick);
                    timer.Interval = animationLength / framesCount;
                }
                return timer;
            }
            set {
                timer = value;
            }
        }
        public SkinButton() {
            //初始化
            Init();
            this.ResizeRedraw = true;
            this.BackColor = Color.Transparent;
        }
        #endregion

        #region 初始化
        public void Init() {
            //设置自定义控件Style
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            UpdateStyles();
        }
        #endregion

        #region 属性与变量
        private Size borderInflate = new Size(-1, -1);
        /// <summary>
        /// 边框放大指定变量
        /// </summary>
        [Category("Border")]
        [DefaultValue(typeof(Size), "-1,-1")]
        [Description("边框放大指定变量。")]
        public Size BorderInflate {
            get { return borderInflate; }
            set {
                if (borderInflate != value) {
                    borderInflate = value;
                    this.Invalidate();
                }
            }
        }
        private bool isdrawBorder = true;
        [DefaultValue(true)]
        [Category("Border")]
        [Description("是否绘制边框。)")]
        public bool IsDrawBorder {
            get { return isdrawBorder; }
            set {
                if (isdrawBorder != value) {
                    isdrawBorder = value;
                    base.Invalidate();
                }
            }
        }

        private Color borderColor = Color.FromArgb(9, 163, 220);
        [DefaultValue(typeof(Color), "9, 163, 220")]
        [Category("Border")]
        [Description("外边框颜色。)")]
        public Color BorderColor {
            get { return borderColor; }
            set {
                if (borderColor != value) {
                    borderColor = value;
                    base.Invalidate();
                }
            }
        }

        private Color innerBorderColor = Color.FromArgb(200, 255, 255, 255);
        [DefaultValue(typeof(Color), "200, 255, 255, 255")]
        [Category("Border")]
        [Description("内边框颜色。)")]
        public Color InnerBorderColor {
            get { return innerBorderColor; }
            set {
                if (innerBorderColor != value) {
                    innerBorderColor = value;
                    base.Invalidate();
                }
            }
        }


        private bool isdrawGlass = true;
        [DefaultValue(true)]
        [Category("Skin")]
        [Description("是否启用渐变色Glass效果。")]
        public bool IsDrawGlass {
            get { return isdrawGlass; }
            set {
                if (isdrawGlass != value) {
                    isdrawGlass = value;
                    base.Invalidate();
                }
            }
        }

        private bool foreColorSuit = false;
        [DefaultValue(false)]
        [Category("Skin")]
        [Description("是否根据背景色自动适应文本颜色。\n(背景色为暗色时文本显示白色，背景为亮色时文本显示黑色。)")]
        public bool ForeColorSuit {
            get { return foreColorSuit; }
            set {
                if (foreColorSuit != value) {
                    foreColorSuit = value;
                    base.Invalidate();
                }
            }
        }

        private bool isEnabledDraw = true;
        [Category("Skin")]
        [DefaultValue(true)]
        [Description("是否画禁用状态下的效果。")]
        public bool IsEnabledDraw {
            get { return isEnabledDraw; }
            set {
                if (isEnabledDraw != value) {
                    isEnabledDraw = value;
                    base.Invalidate();
                }
            }
        }

        private bool inheritColor = false;
        /// <summary>
        /// 是否继承所在窗体的色调
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(bool), "false")]
        [Description("是否继承所在窗体的色调。")]
        public bool InheritColor {
            get { return inheritColor; }
            set {
                if (inheritColor != value) {
                    inheritColor = value;
                    this.Invalidate();
                }
            }
        }


        private bool fadeGlow = true;
        /// <summary>
        /// 是否开启动画渐变效果
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(bool), "true")]
        [Description("是否开启动画渐变效果(只有DrawType属性设置为Draw才有效)")]
        public bool FadeGlow {
            get { return fadeGlow; }
            set {
                if (fadeGlow != value) {
                    fadeGlow = value;
                    this.Invalidate();
                }
            }
        }


        private Color glowColor = Color.White;
        //动画渐变Glow颜色
        [DefaultValue(typeof(Color), "White"), Category("Skin"), Description("动画渐变Glow颜色")]
        public virtual Color GlowColor {
            get { return glowColor; }
            set {
                if (glowColor != value) {
                    glowColor = value;
                    CreateFrames();
                    if (IsHandleCreated) {
                        Invalidate();
                    }
                }
            }
        }

        private StopStates stopState = StopStates.NoStop;
        //停止当前状态
        [Category("Skin")]
        [Description("停止当前状态")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StopStates StopState {
            get { return stopState; }
            set {
                stopState = value;
                base.Invalidate();
            }
        }

        private DrawStyle drawType = DrawStyle.Draw;
        [DefaultValue(typeof(DrawStyle), "2")]
        [Category("Skin")]
        [Description("按钮的绘画模式")]
        public DrawStyle DrawType {
            get { return drawType; }
            set {
                if (drawType != value) {
                    drawType = value;
                    base.Invalidate();
                }
            }
        }

        private Color _mouseBaseColor = Color.FromArgb(60, 195, 245);
        [Category("Skin")]
        [DefaultValue(typeof(Color), "60,195,245")]
        [Description("Bottom悬浮时色调(非图片绘制时和Glass模式时生效)")]
        public Color MouseBaseColor {
            get { return _mouseBaseColor; }
            set {
                _mouseBaseColor = value;
                base.Invalidate();
            }
        }

        private Color _downBaseColor = Color.FromArgb(9, 140, 188);
        [Category("Skin")]
        [DefaultValue(typeof(Color), "9,140,188")]
        [Description("Bottom按下时色调(非图片绘制时和Glass模式时生效)")]
        public Color DownBaseColor {
            get { return _downBaseColor; }
            set {
                _downBaseColor = value;
                base.Invalidate();
            }
        }

        private Color _baseColor = Color.FromArgb(9, 163, 220);
        [Category("Skin")]
        [DefaultValue(typeof(Color), "9,163,220")]
        [Description("Bottom色调(非图片绘制模式时生效)")]
        public Color BaseColor {
            get { return _baseColor; }
            set {
                _baseColor = value;
                base.Invalidate();
            }
        }

        private Size _imageSize = new Size(18, 18);
        [Category("Skin")]
        [DefaultValue(typeof(Size), "18,18")]
        [Description("设置或获取图像的大小")]
        public Size ImageSize {
            get { return _imageSize; }
            set {
                if (value != _imageSize) {

                    _imageSize = value;
                    base.Invalidate();
                }
            }
        }

        private RoundStyle _roundStyle = RoundStyle.None;
        [Category("Skin")]
        [DefaultValue(typeof(RoundStyle), "0")]
        [Description("设置或获取按钮圆角的样式")]
        public RoundStyle RoundStyle {
            get { return _roundStyle; }
            set {
                if (_roundStyle != value) {
                    _roundStyle = value;
                    base.Invalidate();
                }
            }
        }

        private int radius = 8;
        /// <summary>
        /// 圆角大小
        /// </summary>
        [DefaultValue(typeof(int), "8")]
        [Category("Skin")]
        [Description("圆角大小")]
        public int Radius {
            get {
                return radius;
            }
            set {
                if (radius != value) {
                    radius = value < 4 ? 4 : value;
                    this.Invalidate();
                }
            }
        }

        private ControlState _controlState;
        /// <summary>
        /// 控件状态
        /// </summary>
        [Description("控件状态")]
        public ControlState ControlState {
            get { return _controlState; }
            set {
                if (_controlState != value) {
                    _controlState = value;
                    base.Invalidate();
                }
            }
        }

        private bool palace = false;
        /// <summary>
        /// 是否开启九宫绘图
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(bool), "false")]
        [Description("是否开启九宫绘图")]
        public bool Palace {
            get { return palace; }
            set {
                if (palace != value) {
                    palace = value;
                    this.Invalidate();
                }
            }
        }

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

        private Rectangle backrectangle = new Rectangle(10, 10, 10, 10);
        /// <summary>
        /// 九宫绘画区域
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(Rectangle), "10,10,10,10")]
        [Description("九宫绘画区域")]
        public Rectangle BackRectangle {
            get { return backrectangle; }
            set {
                if (backrectangle != value) {
                    backrectangle = value;
                }
                this.Invalidate();
            }
        }

        private Image mouseback;
        /// <summary>
        /// 悬浮时
        /// </summary>
        [Category("MouseEnter")]
        [Description("悬浮时背景")]
        public Image MouseBack {
            get { return mouseback; }
            set {
                if (mouseback != value) {
                    mouseback = value;
                    this.Invalidate();
                }
            }
        }

        private Image downback;
        /// <summary>
        /// 点击时
        /// </summary>
        [Category("MouseDown")]
        [Description("点击时背景")]
        public Image DownBack {
            get { return downback; }
            set {
                if (downback != value) {
                    downback = value;
                    this.Invalidate();
                }
            }
        }

        private Image normlback;
        /// <summary>
        /// 初始时
        /// </summary>
        [Category("MouseNorml")]
        [Description("初始时背景")]
        public Image NormlBack {
            get { return normlback; }
            set {
                if (normlback != value) {
                    normlback = value;
                    this.Invalidate();
                }
            }
        }
        #endregion

        #region 重载事件
        protected override void OnParentBackColorChanged(EventArgs e) {
            base.OnParentBackColorChanged(e);
            this.Invalidate();
        }

        protected override void OnEnabledChanged(EventArgs e) {
            this.Invalidate();
            base.OnEnabledChanged(e);
        }

        protected override void OnMouseEnter(EventArgs e) {
            base.OnMouseEnter(e);
            //执行动画渐变效果
            if (FadeGlow) {
                //渐显
                FadeIn();
            }
            _controlState = ControlState.Hover;
            this.Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            //执行动画渐变效果
            if (FadeGlow) {
                //渐离
                FadeOut();
            }
            _controlState = ControlState.Normal;
            this.Invalidate();
        }

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
            if (disposing) {
                if (DhTimer != null) {
                    DhTimer.Dispose();
                    DhTimer = null;
                }
                if (null != frames) {
                    if (frames.Count > 0) {
                        frames.Clear();
                        frames = null;
                    }
                }
            }
        }

        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e) {
            base.OnMouseDown(e);
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left) return;

            _controlState = ControlState.Pressed;
            this.Invalidate();
        }

        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e) {
            base.OnMouseUp(e);
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) {
                _controlState = ControlState.Hover;
                this.Invalidate();
            }
        }

        private Button imageButton;
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            base.OnPaintBackground(e);
            //变量与初始化
            Graphics g = e.Graphics;
            Rectangle rc = this.ClientRectangle;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //最高质量绘制文字
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            Color baseColor;
            Color _broderColor = BorderColor;
            Color _InnerBorderColor = InnerBorderColor;
            Color CpBaseColor = BaseColor;
            Bitmap btm = null;
            //停止当前状态
            if (StopState != StopStates.NoStop) {
                _controlState = (ControlState)StopState;
            }
            //获取父容器色调
            if (InheritColor) {
                CpBaseColor = this.Parent.BackColor;
            }
            //取得当前需要绘画的图像与色调
            switch (_controlState) {
                case ControlState.Hover:
                    btm = (Bitmap)MouseBack;
                    if (IsDrawGlass) {
                        baseColor = GetColor(CpBaseColor, 0, -13, -8, -3);
                    } else {
                        baseColor = MouseBaseColor;
                    }
                    break;
                case ControlState.Pressed:
                    btm = (Bitmap)DownBack;
                    if (IsDrawGlass) {
                        baseColor = GetColor(CpBaseColor, 0, -35, -24, -9);
                    } else {
                        baseColor = DownBaseColor;
                    }
                    break;
                default:
                    btm = (Bitmap)NormlBack;
                    baseColor = CpBaseColor;
                    break;
            }
            if (!this.Enabled && IsEnabledDraw) {
                baseColor = SystemColors.ControlDark;
                _broderColor = SystemColors.ControlDark; ;
                _InnerBorderColor = SystemColors.ControlDark; ;
                if (btm != null) {
                    Bitmap map = new Bitmap(btm.Width, btm.Height);
                    using (Graphics gr = Graphics.FromImage(map)) {
                        ControlPaint.DrawImageDisabled(gr, btm, 0, 0, this.BaseColor);
                    }
                    btm = map;
                }
            }
            //如果有图则用图绘画
            if (btm != null && DrawType == DrawStyle.Img) {
                //设置不规则区域
                rc = SetCreate(rc, btm);
                //是否启用九宫绘图
                if (Palace) {
                    ImageDrawRect.DrawRect(g, btm, rc, Rectangle.FromLTRB(BackRectangle.X, BackRectangle.Y, BackRectangle.Width, BackRectangle.Height), 1, 1);
                } else {
                    g.DrawImage(btm, 0, 0, this.Width, this.Height);
                }
            } else if (DrawType == DrawStyle.Draw)  //无图则用色调绘图
            {
                RenderBackgroundInternal(
                    g,
                    rc,
                    baseColor,
                    _broderColor,
                    _InnerBorderColor,
                    RoundStyle,
                    Radius,
                    0.35f,
                    IsDrawBorder,
                    IsDrawGlass,
                    LinearGradientMode.Vertical);
                //执行动画渐变效果
                if (FadeGlow)
                    DrawButtonBackgroundFromBuffer(e.Graphics);
            }

            #region 绘画文字和图像
            Image img = null;
            if (imageButton == null) {
                imageButton = new Button();
                imageButton.Parent = new TransparentControl();
                imageButton.SuspendLayout();
                imageButton.BackColor = Color.Transparent;
                imageButton.FlatAppearance.BorderSize = 0;
                imageButton.FlatStyle = FlatStyle.Flat;
            } else {
                imageButton.SuspendLayout();
            }
            imageButton.AutoEllipsis = AutoEllipsis;
            if (Enabled) {
                Color txtColor = this.Enabled ? ForeColor : SystemColors.ControlDark;
                //是否根据背景色决定字体颜色
                if (ForeColorSuit) {
                    //如果背景色为暗色
                    if (SkinTools.ColorSlantsDarkOrBright(CpBaseColor)) {
                        txtColor = Color.White;
                    } else//如果背景色为亮色
                    {
                        txtColor = Color.Black;
                    }
                }
                imageButton.ForeColor = txtColor;
            } else {
                imageButton.ForeColor = HuiseColor(ForeColor);
            }
            imageButton.Font = Font;
            imageButton.RightToLeft = RightToLeft;
            if (imageButton.Image != Image && imageButton.Image != null) {
                imageButton.Image.Dispose();
            }
            if (Image != null) {
                img = this.Image;
                if (this.ImageList != null && this.ImageIndex != -1) {
                    img = this.ImageList.Images[this.ImageIndex];
                }
                Bitmap map = new Bitmap(img, ImageSize);
                if (!Enabled) {
                    using (Graphics gr = Graphics.FromImage(map)) {
                        ControlPaint.DrawImageDisabled(gr, map, 0, 0, this.BaseColor);
                    }
                }
                imageButton.Image = map;
            }
            imageButton.ImageAlign = ImageAlign;
            imageButton.ImageIndex = ImageIndex;
            imageButton.ImageKey = ImageKey;
            imageButton.ImageList = ImageList;
            imageButton.Padding = Padding;
            imageButton.Size = Size;
            imageButton.Text = Text;
            imageButton.TextAlign = TextAlign;
            imageButton.TextImageRelation = TextImageRelation;
            imageButton.UseCompatibleTextRendering = UseCompatibleTextRendering;
            imageButton.UseMnemonic = UseMnemonic;
            imageButton.ResumeLayout();
            InvokePaint(imageButton, e);
            #endregion

        }

        /// <summary>
        /// 将颜色灰色度处理
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public Color HuiseColor(Color c) {
            return Color.FromArgb((3 * c.R + SystemColors.ControlDark.R) >> 2,
                      (3 * c.G + SystemColors.ControlDark.G) >> 2,
                      (3 * c.B + SystemColors.ControlDark.B) >> 2);
        }

        //上一次操作记录
        Bitmap UpBtm = null;
        Rectangle UpRc = Rectangle.Empty;
        int? UpRadius = null;
        RoundStyle? UpRoundStyle = null;
        /// <summary>
        /// 设置Region区域
        /// </summary>
        /// <param name="rc"></param>
        /// <param name="btm"></param>
        /// <returns></returns>
        private Rectangle SetCreate(Rectangle rc, Bitmap btm) {
            //绘制不规则区域
            if (Create) {
                //对比是否需要变动
                if (btm != UpBtm) {
                    SkinTools.CreateControlRegion(this, btm, 1);
                    //赋值操作记录
                    UpBtm = btm;
                }
            } else {
                //对比是否需要变动
                if (rc != UpRc || Radius != UpRadius || RoundStyle != UpRoundStyle) {
                    //绘制圆角
                    SkinTools.CreateRegion(this, rc, Radius, RoundStyle);
                    //赋值操作记录
                    UpRc = rc;
                    UpRadius = Radius;
                    UpRoundStyle = RoundStyle;
                }
            }
            return rc;
        }

        private class TransparentControl : Control
        {
            protected override void OnPaintBackground(PaintEventArgs pevent) { }
            protected override void OnPaint(PaintEventArgs e) { }
        }
        #endregion

        #region 绘画方法
        public void RenderBackgroundInternal(
           Graphics g,
           Rectangle rect,
           Color baseColor,
           Color borderColor,
           Color innerBorderColor,
           RoundStyle style,
           int roundWidth,
           float basePosition,
           bool drawBorder,
           bool drawGlass,
           LinearGradientMode mode) {
            //多一像素消减
            rect.Width--;
            rect.Height--;
            //if (drawBorder) {
            //}

            using (LinearGradientBrush brush = new LinearGradientBrush(
                rect, Color.Transparent, Color.Transparent, mode)) {
                Color[] colors = new Color[4];
                colors[0] = GetColor(baseColor, 0, 35, 24, 9);
                colors[1] = GetColor(baseColor, 0, 13, 8, 3);
                colors[2] = baseColor;
                colors[3] = GetColor(baseColor, 0, 68, 69, 54);

                ColorBlend blend = new ColorBlend();
                blend.Positions = new float[] { 0.0f, basePosition, basePosition + 0.05f, 1.0f };
                blend.Colors = colors;
                brush.InterpolationColors = blend;
                if (style != RoundStyle.None) {
                    using (GraphicsPath path =
                        GraphicsPathHelper.CreatePath(rect, roundWidth, style, false)) {
                        if (drawGlass) {
                            g.FillPath(brush, path);
                            if (baseColor.A > 80) {
                                Rectangle rectTop = rect;

                                if (mode == LinearGradientMode.Vertical) {
                                    rectTop.Height = (int)(rectTop.Height * basePosition);
                                } else {
                                    rectTop.Width = (int)(rect.Width * basePosition);
                                }
                                using (GraphicsPath pathTop = GraphicsPathHelper.CreatePath(
                                    rectTop, roundWidth, RoundStyle.Top, false)) {
                                    using (SolidBrush brushAlpha =
                                        new SolidBrush(Color.FromArgb(80, 255, 255, 255))) {
                                        g.FillPath(brushAlpha, pathTop);
                                    }
                                }
                            }
                        } else {
                            g.FillPath(new SolidBrush(baseColor), path);
                        }
                    }

                    if (drawGlass) {
                        RectangleF glassRect = rect;
                        if (mode == LinearGradientMode.Vertical) {
                            glassRect.Y = rect.Y + rect.Height * basePosition;
                            glassRect.Height = (rect.Height - rect.Height * basePosition) * 2;
                        } else {
                            glassRect.X = rect.X + rect.Width * basePosition;
                            glassRect.Width = (rect.Width - rect.Width * basePosition) * 2;
                        }
                        DrawGlass(g, glassRect, 170, 0);
                    }

                    if (drawBorder) {
                        if (borderColor != Color.Transparent) {
                            using (GraphicsPath path =
                                GraphicsPathHelper.CreatePath(rect, roundWidth, style, false)) {
                                using (Pen pen = new Pen(borderColor)) {
                                    g.DrawPath(pen, path);
                                }
                            }
                        }

                        rect.Inflate(BorderInflate);
                        if (innerBorderColor != Color.Transparent) {
                            using (GraphicsPath path =
                                GraphicsPathHelper.CreatePath(rect, roundWidth, style, false)) {
                                using (Pen pen = new Pen(innerBorderColor)) {
                                    g.DrawPath(pen, path);
                                }
                            }
                        }
                    }
                } else {
                    if (drawGlass) {
                        g.FillRectangle(brush, rect);
                        if (baseColor.A > 80) {
                            Rectangle rectTop = rect;
                            if (mode == LinearGradientMode.Vertical) {
                                rectTop.Height = (int)(rectTop.Height * basePosition);
                            } else {
                                rectTop.Width = (int)(rect.Width * basePosition);
                            }
                            using (SolidBrush brushAlpha =
                                new SolidBrush(Color.FromArgb(80, 255, 255, 255))) {
                                g.FillRectangle(brushAlpha, rectTop);
                            }
                        }
                    } else {
                        g.FillRectangle(new SolidBrush(baseColor), rect);
                    }

                    if (drawGlass) {
                        RectangleF glassRect = rect;
                        if (mode == LinearGradientMode.Vertical) {
                            glassRect.Y = rect.Y + rect.Height * basePosition;
                            glassRect.Height = (rect.Height - rect.Height * basePosition) * 2;
                        } else {
                            glassRect.X = rect.X + rect.Width * basePosition;
                            glassRect.Width = (rect.Width - rect.Width * basePosition) * 2;
                        }
                        DrawGlass(g, glassRect, 200, 0);
                    }

                    if (drawBorder) {
                        if (borderColor != Color.Transparent) {
                            using (Pen pen = new Pen(borderColor)) {
                                g.DrawRectangle(pen, rect);
                            }
                        }

                        rect.Inflate(BorderInflate);
                        if (innerBorderColor != Color.Transparent) {
                            using (Pen pen = new Pen(innerBorderColor)) {
                                g.DrawRectangle(pen, rect);
                            }
                        }
                    }
                }
            }
        }

        private void DrawGlass(
            Graphics g, RectangleF glassRect, int alphaCenter, int alphaSurround) {
            DrawGlass(g, glassRect, Color.White, alphaCenter, alphaSurround);
        }

        private void DrawGlass(
            Graphics g,
            RectangleF glassRect,
            Color glassColor,
            int alphaCenter,
            int alphaSurround) {
            using (GraphicsPath path = new GraphicsPath()) {
                path.AddEllipse(glassRect);
                using (PathGradientBrush brush = new PathGradientBrush(path)) {
                    brush.CenterColor = Color.FromArgb(alphaCenter, glassColor);
                    brush.SurroundColors = new Color[] { 
                        Color.FromArgb(alphaSurround, glassColor) };
                    brush.CenterPoint = new PointF(
                        glassRect.X + glassRect.Width / 2,
                        glassRect.Y + glassRect.Height / 2);
                    g.FillPath(brush, path);
                }
            }
        }

        private Color GetColor(Color colorBase, int a, int r, int g, int b) {
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

        #region 动画绘画方法

        private void DrawButtonBackgroundFromBuffer(Graphics graphics) {
            int frame;
            if (!Enabled) {
                frame = FRAME_DISABLED;
            } else if (ControlState == ControlState.Pressed) {
                frame = FRAME_PRESSED;
            } else if (!isAnimating && currentFrame == 0) {
                frame = FRAME_NORMAL;
            } else {
                if (!HasAnimationFrames) {
                    CreateFrames(true);
                }
                frame = FRAME_ANIMATED + currentFrame;
            }
            if (frames == null || frames.Count == 0) {
                CreateFrames();
            }
            graphics.DrawImage(frames[frame], Point.Empty);
        }

        public Image CreateBackgroundFrame(bool pressed, bool hovered,
            bool animating, bool enabled, float glowOpacity) {
            Rectangle rect = ClientRectangle;
            if (rect.Width <= 0) {
                rect.Width = 1;
            }
            if (rect.Height <= 0) {
                rect.Height = 1;
            }
            Image img = new Bitmap(rect.Width, rect.Height);
            using (Graphics g = Graphics.FromImage(img)) {
                g.Clear(Color.Transparent);
                DrawButtonBackground(g, rect, pressed, hovered, animating, enabled, glowColor, glowOpacity);
            }
            return img;
        }

        private static void DrawButtonBackground(Graphics g, Rectangle rectangle,
            bool pressed, bool hovered, bool animating, bool enabled,
            Color glowColor, float glowOpacity) {
            SmoothingMode sm = g.SmoothingMode;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = rectangle;
            rect.Width--;
            rect.Height--;
            rect.X++;
            rect.Y++;
            rect.Width -= 2;
            rect.Height -= 2;
            Rectangle rect2 = rect;
            rect2.Height >>= 1;

            #region " glow "
            if ((hovered || animating) && !pressed) {
                using (GraphicsPath clip = CreateRoundRectangle(rect, 2)) {
                    g.SetClip(clip, CombineMode.Intersect);
                    using (GraphicsPath brad = CreateBottomRadialPath(rect)) {
                        using (PathGradientBrush pgr = new PathGradientBrush(brad)) {
                            unchecked {
                                int opacity = (int)(0xB2 * glowOpacity + .5f);
                                RectangleF bounds = brad.GetBounds();
                                pgr.CenterPoint = new PointF((bounds.Left + bounds.Right) / 2f, (bounds.Top + bounds.Bottom) / 2f);
                                pgr.CenterColor = Color.FromArgb(opacity, glowColor);
                                pgr.SurroundColors = new Color[] { Color.FromArgb(0, glowColor) };
                            }
                            g.FillPath(pgr, brad);
                        }
                    }
                    g.ResetClip();
                }
            }
            #endregion
            g.SmoothingMode = sm;
        }

        private static GraphicsPath CreateRoundRectangle(Rectangle rectangle, int radius) {
            GraphicsPath path = new GraphicsPath();
            int l = rectangle.Left;
            int t = rectangle.Top;
            int w = rectangle.Width;
            int h = rectangle.Height;
            int d = radius << 1;
            path.AddArc(l, t, d, d, 180, 90); // topleft
            path.AddLine(l + radius, t, l + w - radius, t); // top
            path.AddArc(l + w - d, t, d, d, 270, 90); // topright
            path.AddLine(l + w, t + radius, l + w, t + h - radius); // right
            path.AddArc(l + w - d, t + h - d, d, d, 0, 90); // bottomright
            path.AddLine(l + w - radius, t + h, l + radius, t + h); // bottom
            path.AddArc(l, t + h - d, d, d, 90, 90); // bottomleft
            path.AddLine(l, t + h - radius, l, t + radius); // left
            path.CloseFigure();
            return path;
        }

        private static GraphicsPath CreateBottomRadialPath(Rectangle rectangle) {
            GraphicsPath path = new GraphicsPath();
            RectangleF rect = rectangle;
            rect.X -= rect.Width * .35f;
            rect.Y -= rect.Height * .15f;
            rect.Width *= 1.7f;
            rect.Height *= 2.3f;
            path.AddEllipse(rect);
            path.CloseFigure();
            return path;
        }

        #endregion

        #region 动画支持

        private List<Image> frames;

        private const int FRAME_DISABLED = 0;
        private const int FRAME_PRESSED = 1;
        private const int FRAME_NORMAL = 2;
        private const int FRAME_ANIMATED = 3;

        private bool HasAnimationFrames {
            get {
                return frames != null && frames.Count > FRAME_ANIMATED;
            }
        }

        private void CreateFrames() {
            CreateFrames(false);
        }

        private void CreateFrames(bool withAnimationFrames) {
            DestroyFrames();
            if (!IsHandleCreated) {
                return;
            }
            if (frames == null) {
                frames = new List<Image>();
            }
            frames.Add(CreateBackgroundFrame(false, false, false, false, 0));
            frames.Add(CreateBackgroundFrame(true, true, false, true, 0));
            frames.Add(CreateBackgroundFrame(false, false, false, true, 0));
            if (!withAnimationFrames) {
                return;
            }
            for (int i = 0; i < framesCount; i++) {
                frames.Add(CreateBackgroundFrame(false, true, true, true, (float)i / (framesCount - 1F)));
            }
        }

        private void DestroyFrames() {
            if (frames != null) {
                while (frames.Count > 0) {
                    frames[frames.Count - 1].Dispose();
                    frames.RemoveAt(frames.Count - 1);
                }
            }
        }

        private const int animationLength = 300;
        private const int framesCount = 10;
        private int currentFrame;
        private int direction;

        private bool isAnimating {
            get {
                return direction != 0;
            }
        }

        private void FadeIn() {
            direction = 1;
            DhTimer.Enabled = true;
        }

        private void FadeOut() {
            direction = -1;
            DhTimer.Enabled = true;
        }

        private void timer_Tick(object sender, EventArgs e) {
            if (!DhTimer.Enabled) {
                return;
            }
            Refresh();
            currentFrame += direction;
            if (currentFrame == -1) {
                currentFrame = 0;
                DhTimer.Enabled = false;
                direction = 0;
                return;
            }
            if (currentFrame == framesCount) {
                currentFrame = framesCount - 1;
                DhTimer.Enabled = false;
                direction = 0;
            }
        }
        #endregion
    }
}
