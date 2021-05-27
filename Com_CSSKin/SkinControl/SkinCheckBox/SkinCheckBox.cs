
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using Com_CSSkin.SkinClass;
using System.Drawing.Drawing2D;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(CheckBox))]
    public partial class SkinCheckBox : CheckBox
    {
        public SkinCheckBox() {
            //初始化
            Init();
            this.ResizeRedraw = true;
            this.BackColor = System.Drawing.Color.Transparent;//背景设为透明
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        }
        #region 初始化
        public void Init() {
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.UserPaint, true);//自行绘制
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
        }
        #endregion

        #region 属性与变量
        private ControlState _controlState;
        private static readonly ContentAlignment RightAlignment =
          ContentAlignment.TopRight |
          ContentAlignment.BottomRight |
          ContentAlignment.MiddleRight;
        private static readonly ContentAlignment LeftAligbment =
            ContentAlignment.TopLeft |
            ContentAlignment.BottomLeft |
            ContentAlignment.MiddleLeft;

        private Color _baseColor = Color.FromArgb(51, 161, 224);
        /// <summary>
        /// 非图片绘制时CheckBox色调
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(Color), "51, 161, 224")]
        [Description("非图片绘制时CheckBox色调")]
        public Color BaseColor {
            get { return _baseColor; }
            set {
                if (_baseColor != value) {
                    _baseColor = value;
                    this.Invalidate();
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

        private int defaultcheckbuttonwidth = 12;
        /// <summary>
        /// 选择框大小
        /// </summary>
        [Category("Skin")]
        [DefaultValue(12)]
        [Description("选择框大小")]
        public int DefaultCheckButtonWidth {
            get { return defaultcheckbuttonwidth; }
            set {
                if (defaultcheckbuttonwidth != value) {
                    defaultcheckbuttonwidth = value;
                    this.Invalidate();
                }
            }
        }

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

        private Image mouseback;
        /// <summary>
        /// 悬浮时
        /// </summary>
        [Category("MouseEnter")]
        [Description("悬浮时图像")]
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
        [Description("点击时图像")]
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
        [Description("初始时图像")]
        public Image NormlBack {
            get { return normlback; }
            set {
                if (normlback != value) {
                    normlback = value;
                    this.Invalidate();
                }
            }
        }

        private Image selectedmouseback;
        /// <summary>
        /// 悬浮时
        /// </summary>
        [Category("MouseEnter")]
        [Description("选中悬浮时图像")]
        public Image SelectedMouseBack {
            get { return selectedmouseback; }
            set {
                if (selectedmouseback != value) {
                    selectedmouseback = value;
                    this.Invalidate();
                }
            }
        }

        private Image selectedownback;
        /// <summary>
        /// 点击时
        /// </summary>
        [Category("MouseDown")]
        [Description("选中点击时图像")]
        public Image SelectedDownBack {
            get { return selectedownback; }
            set {
                if (selectedownback != value) {
                    selectedownback = value;
                    this.Invalidate();
                }
            }
        }

        private Image selectenormlback;
        /// <summary>
        /// 初始时
        /// </summary>
        [Category("MouseNorml")]
        [Description("选中初始时图像")]
        public Image SelectedNormlBack {
            get { return selectenormlback; }
            set {
                if (selectenormlback != value) {
                    selectenormlback = value;
                    this.Invalidate();
                }
            }
        }

        private bool lighteffect = true;
        /// <summary>
        /// 是否绘制发光字体
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(bool), "true")]
        [Description("是否绘制发光字体")]
        public bool LightEffect {
            get { return lighteffect; }
            set {
                if (lighteffect != value) {
                    lighteffect = value;
                    this.Invalidate();
                }
            }
        }

        private Color lighteffectback = Color.White;
        /// <summary>
        /// 发光字体背景色
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(Color), "White")]
        [Description("发光字体背景色")]
        public Color LightEffectBack {
            get { return lighteffectback; }
            set {
                if (lighteffectback != value) {
                    lighteffectback = value;
                    this.Invalidate();
                }
            }
        }

        private int lighteffectWidth = 4;
        /// <summary>
        /// 光圈大小
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(int), "4")]
        [Description("光圈大小")]
        public int LightEffectWidth {
            get { return lighteffectWidth; }
            set {
                if (lighteffectWidth != value) {
                    lighteffectWidth = value;
                    this.Invalidate();
                }
            }
        }
        #endregion

        #region 重载事件
        //悬浮时
        protected override void OnMouseEnter(EventArgs e) {
            ControlState = ControlState.Hover;
            this.Invalidate();
            base.OnMouseEnter(e);
        }

        //离开时
        protected override void OnMouseLeave(EventArgs e) {
            ControlState = ControlState.Normal;
            this.Invalidate();
            base.OnMouseLeave(e);
        }

        //按下鼠标时
        protected override void OnMouseDown(System.Windows.Forms.MouseEventArgs e) {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left) return;
            ControlState = ControlState.Pressed;
            this.Invalidate();
            base.OnMouseDown(e);
        }

        //松开鼠标时
        protected override void OnMouseUp(System.Windows.Forms.MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && e.Clicks == 1) {
                if (ClientRectangle.Contains(e.Location)) {
                    ControlState = ControlState.Hover;
                } else {
                    ControlState = ControlState.Normal;
                }
            }
            this.Invalidate();
            base.OnMouseUp(e);
        }

        //获得焦点时
        protected override void OnEnter(EventArgs e) {
            ControlState = ControlState.Focused;
            base.OnEnter(e);
        }

        //重绘
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            base.OnPaintBackground(e);
            //获得绘画方法
            Graphics g = e.Graphics;
            //CheckButtom的绘画范围
            Rectangle checkButtonRect;
            //文字的绘画范围
            Rectangle textRect;
            //给CheckButtom与文字的绘画范围赋值
            CalculateRect(out checkButtonRect, out textRect);
            //抗锯齿的呈现
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //最高质量绘制文字
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            //取得当前需要绘画的图像
            Bitmap btm = null;
            //当前无图像时绘画的颜色变量
            Color backColor = ControlPaint.Light(_baseColor);
            Color borderColor = Color.Empty;
            Color innerBorderColor = Color.Empty;
            Color checkColor = Color.Empty;
            bool hover = false;

            //启用状态时
            switch (ControlState) {
                case ControlState.Hover:
                    borderColor = _baseColor;
                    innerBorderColor = _baseColor;
                    checkColor = GetColor(_baseColor, 0, 35, 24, 9);
                    btm = Checked ? (Bitmap)SelectedMouseBack : (Bitmap)MouseBack;
                    hover = true;
                    break;
                case ControlState.Pressed:
                    borderColor = _baseColor;
                    innerBorderColor = GetColor(_baseColor, 0, -13, -8, -3);
                    checkColor = GetColor(_baseColor, 0, -35, -24, -9);
                    btm = Checked ? (Bitmap)SelectedDownBack : (Bitmap)DownBack;
                    hover = true;
                    break;
                default:
                    borderColor = _baseColor;
                    innerBorderColor = Color.Empty;
                    checkColor = _baseColor;
                    btm = Checked ? (Bitmap)SelectedNormlBack : (Bitmap)NormlBack;
                    break;
            }
            if (!this.Enabled && IsEnabledDraw) //禁用状态时
            {
                borderColor = SystemColors.ControlDark;
                innerBorderColor = SystemColors.ControlDark;
                checkColor = SystemColors.ControlDark;
                if (btm != null) {
                    Bitmap map = new Bitmap(btm.Width, btm.Height);
                    using (Graphics gr = Graphics.FromImage(map)) {
                        ControlPaint.DrawImageDisabled(gr, btm, 0, 0, this.BaseColor);
                    }
                    btm = map;
                }
            }

            if (btm == null) {
                using (SolidBrush brush = new SolidBrush(Color.White)) {
                    g.FillRectangle(brush, checkButtonRect);
                }

                if (hover) {
                    using (Pen pen = new Pen(innerBorderColor, 2F)) {
                        g.DrawRectangle(pen, checkButtonRect);
                    }
                }

                //根据状态变换
                switch (CheckState) {
                    case CheckState.Checked:
                        DrawCheckedFlag(
                            g,
                            checkButtonRect,
                            checkColor);
                        break;
                    case CheckState.Indeterminate:
                        checkButtonRect.Inflate(-1, -1);
                        using (GraphicsPath path = new GraphicsPath()) {
                            path.AddEllipse(checkButtonRect);
                            using (PathGradientBrush brush = new PathGradientBrush(path)) {
                                brush.CenterColor = checkColor;
                                brush.SurroundColors = new Color[] { Color.White };
                                Blend blend = new Blend();
                                blend.Positions = new float[] { 0f, 0.4f, 1f };
                                blend.Factors = new float[] { 0f, 0.3f, 1f };
                                brush.Blend = blend;
                                g.FillEllipse(brush, checkButtonRect);
                            }
                        }
                        checkButtonRect.Inflate(1, 1);
                        break;
                }

                using (Pen pen = new Pen(borderColor)) {
                    g.DrawRectangle(pen, checkButtonRect);
                }
            } else {
                //画图像
                g.DrawImage(btm, checkButtonRect);
            }
            //画字体
            Color textColor = Enabled ? ForeColor : SystemColors.GrayText;
            //是否绘画发光字体
            if (LightEffect) {
                if (!string.IsNullOrEmpty(Text)) {
                    Image imgText = SkinTools.ImageLightEffect(Text, Font, textColor, LightEffectBack, LightEffectWidth);
                    g.DrawImage(imgText, textRect);
                }
            } else {
                TextRenderer.DrawText(
                    g,
                    Text,
                    Font,
                    textRect,
                    textColor,
                    GetTextFormatFlags(TextAlign, RightToLeft == RightToLeft.Yes));
            }
        }

        //画点击Check
        private void DrawCheckedFlag(Graphics graphics, Rectangle rect, Color color) {
            PointF[] points = new PointF[3];
            points[0] = new PointF(
                rect.X + rect.Width / 4.5f,
                rect.Y + rect.Height / 2.5f);
            points[1] = new PointF(
                rect.X + rect.Width / 2.5f,
                rect.Bottom - rect.Height / 3f);
            points[2] = new PointF(
                rect.Right - rect.Width / 4.0f,
                rect.Y + rect.Height / 4.5f);
            using (Pen pen = new Pen(color, 2F)) {
                graphics.DrawLines(pen, points);
            }
        }

        //获取字体的组合模式
        public static TextFormatFlags GetTextFormatFlags(
            ContentAlignment alignment,
            bool rightToleft) {
            TextFormatFlags flags = TextFormatFlags.WordBreak |
                TextFormatFlags.SingleLine;
            if (rightToleft) {
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
            }

            switch (alignment) {
                case ContentAlignment.BottomCenter:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.BottomLeft:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;
                case ContentAlignment.BottomRight:
                    flags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;
                case ContentAlignment.MiddleCenter:
                    flags |= TextFormatFlags.HorizontalCenter |
                        TextFormatFlags.VerticalCenter;
                    break;
                case ContentAlignment.MiddleLeft:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case ContentAlignment.MiddleRight:
                    flags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;
                case ContentAlignment.TopCenter:
                    flags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.TopLeft:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;
                case ContentAlignment.TopRight:
                    flags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;
            }
            return flags;
        }

        //获取字体和图像的绘画范围
        private void CalculateRect(
             out Rectangle checkButtonRect, out Rectangle textRect) {
            checkButtonRect = new Rectangle(
                0, 0, DefaultCheckButtonWidth, DefaultCheckButtonWidth);
            textRect = Rectangle.Empty;
            bool bCheckAlignLeft = (int)(LeftAligbment & CheckAlign) != 0;
            bool bCheckAlignRight = (int)(RightAlignment & CheckAlign) != 0;
            bool bRightToLeft = RightToLeft == RightToLeft.Yes;

            if ((bCheckAlignLeft && !bRightToLeft) ||
                (bCheckAlignRight && bRightToLeft)) {
                switch (CheckAlign) {
                    case ContentAlignment.TopRight:
                    case ContentAlignment.TopLeft:
                        checkButtonRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.MiddleLeft:
                        checkButtonRect.Y = (Height - DefaultCheckButtonWidth) / 2;
                        break;
                    case ContentAlignment.BottomRight:
                    case ContentAlignment.BottomLeft:
                        checkButtonRect.Y = Height - DefaultCheckButtonWidth - 2;
                        break;
                }

                checkButtonRect.X = 1;

                textRect = new Rectangle(
                    checkButtonRect.Right + 2,
                    0,
                    Width - checkButtonRect.Right - 4,
                    Height);
            } else if ((bCheckAlignRight && !bRightToLeft)
                  || (bCheckAlignLeft && bRightToLeft)) {
                switch (CheckAlign) {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopRight:
                        checkButtonRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight:
                        checkButtonRect.Y = (Height - DefaultCheckButtonWidth) / 2;
                        break;
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomRight:
                        checkButtonRect.Y = Height - DefaultCheckButtonWidth - 2;
                        break;
                }

                checkButtonRect.X = Width - DefaultCheckButtonWidth - 1;

                textRect = new Rectangle(
                    2, 0, Width - DefaultCheckButtonWidth - 6, Height);
            } else {
                switch (CheckAlign) {
                    case ContentAlignment.TopCenter:
                        checkButtonRect.Y = 2;
                        textRect.Y = checkButtonRect.Bottom + 2;
                        textRect.Height = Height - DefaultCheckButtonWidth - 6;
                        break;
                    case ContentAlignment.MiddleCenter:
                        checkButtonRect.Y = (Height - DefaultCheckButtonWidth) / 2;
                        textRect.Y = 0;
                        textRect.Height = Height;
                        break;
                    case ContentAlignment.BottomCenter:
                        checkButtonRect.Y = Height - DefaultCheckButtonWidth - 2;
                        textRect.Y = 0;
                        textRect.Height = Height - DefaultCheckButtonWidth - 6;
                        break;
                }

                checkButtonRect.X = (Width - DefaultCheckButtonWidth) / 2;

                textRect.X = 2;
                textRect.Width = Width - 4;
            }
        }

        //获取颜色
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
    }
}
