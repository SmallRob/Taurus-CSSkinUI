
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Drawing.Drawing2D;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(Panel))]
    public partial class SkinPanel : Panel
    {
        public SkinPanel() {
            //初始化
            Init();
            this.ResizeRedraw = true;
            this.BackColor = System.Drawing.Color.Transparent;//背景设为透明
        }
        #region 初始化
        public void Init() {
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.UserPaint, true);//自行绘制
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.DoubleBuffered = true;
            this.UpdateStyles();
        }
        #endregion

        #region 属性与变量
        private Color borderColor = Color.Transparent;
        [Category("Skin")]
        [Description("边框颜色")]
        [DefaultValue(typeof(Color), "Transparent")]
        public Color BorderColor {
            get { return borderColor; }
            set {
                borderColor = value;
                base.Invalidate(true);
            }
        }

        private ControlState _controlState;
        /// <summary>
        /// 控件状态
        /// </summary>
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
                    //更新圆角
                    UpdateRadius();
                    this.Invalidate();
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
                    //更新圆角
                    UpdateRadius();
                    base.Invalidate();
                }
            }
        }

        //上一次操作记录
        Rectangle UpRc = Rectangle.Empty;
        int? UpRadius = null;
        RoundStyle? UpRoundStyle = null;
        /// <summary>
        /// 更新圆角
        /// </summary>
        public void UpdateRadius() {
            Rectangle rc = this.ClientRectangle;
            //修复滚动条的误差
            if (this.VScroll) {
                rc.Width += 17;
            }
            if (this.HScroll) {
                rc.Height += 17;
            }
            //修复边框的误差
            if (BorderStyle == BorderStyle.Fixed3D || BorderStyle == BorderStyle.FixedSingle) {
                rc.Width += 1;
                rc.Height += 1;
            }
            //对比是否需要变动
            if (rc != UpRc || Radius != UpRadius || RoundStyle != UpRoundStyle) {
                //绘制圆角
                SkinTools.CreateRegion(this, rc, radius, RoundStyle);
                //赋值操作记录
                UpRc = rc;
                UpRadius = Radius;
                UpRoundStyle = RoundStyle;
            }
        }
        #endregion

        #region 重载事件
        //鼠标悬浮时
        protected override void OnMouseEnter(EventArgs e) {
            ControlState = ControlState.Hover;
            base.OnMouseEnter(e);
        }

        //鼠标离开时
        protected override void OnMouseLeave(EventArgs e) {
            ControlState = ControlState.Normal;
            base.OnMouseLeave(e);
        }

        //鼠标点击时
        protected override void OnMouseDown(MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                ControlState = ControlState.Pressed;
            }
            base.OnMouseDown(e);
        }

        //鼠标按下时
        protected override void OnMouseUp(MouseEventArgs e) {
            ControlState = ControlState.Hover;
            base.OnMouseUp(e);
        }

        protected override void OnCreateControl() {
            //更新圆角
            UpdateRadius();
            base.OnCreateControl();
        }

        protected override void OnSizeChanged(EventArgs e) {
            //更新圆角
            UpdateRadius();
            base.OnSizeChanged(e);
        }

        //重绘
        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            //取得当前需要绘画的图像
            Bitmap btm = null;
            switch (_controlState) {
                case ControlState.Pressed:
                    btm = (Bitmap)DownBack;
                    break;
                case ControlState.Hover:
                    btm = (Bitmap)MouseBack;
                    break;
                default:
                    btm = (Bitmap)NormlBack;
                    break;
            }
            if (btm != null) {
                //是否启用九宫绘图
                if (Palace) {
                    ImageDrawRect.DrawRect(g, btm, new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height), Rectangle.FromLTRB(BackRectangle.X, BackRectangle.Y, BackRectangle.Width, BackRectangle.Height), 1, 1);
                } else {
                    g.DrawImage(btm, this.ClientRectangle);
                }
            }
            //画边框
            if (BorderColor != Color.Transparent) {
                using (AntiAliasGraphics antiGraphics = new AntiAliasGraphics(g)) {
                    Rectangle rc = this.ClientRectangle;
                    rc.Width -= 1;
                    rc.Height -= 1;
                    DrawBorder(
                        g,
                        rc,
                        this.RoundStyle,
                        this.Radius);
                }
            }
            base.OnPaint(e);
        }

        //画边框
        private void DrawBorder(
            Graphics g, Rectangle rect, RoundStyle roundStyle, int radius) {
            g.SmoothingMode = SmoothingMode.HighQuality; //高质量
            using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                rect, radius, roundStyle, false)) {
                using (Pen pen = new Pen(BorderColor)) {
                    g.DrawPath(pen, path);
                }
            }
        }
        #endregion

        #region 滚动条
        cTreeView ctv;
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (!DesignMode) {
                SetupScrollBars();
            }
        }

        private void SetupScrollBars() {
            ctv = new cTreeView(this.Handle,
            ScrollBarDrawImage.ScrollHorzShaft,
            ScrollBarDrawImage.ScrollHorzArrow,
            ScrollBarDrawImage.ScrollHorzThumb,
            ScrollBarDrawImage.ScrollVertShaft,
            ScrollBarDrawImage.ScrollVertArrow,
            ScrollBarDrawImage.ScrollVertThumb,
            ScrollBarDrawImage.Fader);
        }
        protected override void OnHandleDestroyed(EventArgs e) {
            base.OnHandleDestroyed(e);
            if (!DesignMode) {
                try {
                    if (ctv != null) {
                        ctv.Dispose();
                    }
                } catch { }
            }
        }
        #endregion
    }
}