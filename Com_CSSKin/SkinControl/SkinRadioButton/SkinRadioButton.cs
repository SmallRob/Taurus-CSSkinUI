
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Com_CSSkin.SkinClass;
using System.ComponentModel;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(RadioButton))]
    public class SkinRadioButton : RadioButton
    {
        public SkinRadioButton()
            : base() {
            //初始化
            Init();
            this.BackColor = Color.Transparent;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
        }
        #region 初始化
        public void Init() {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
        }
        #endregion

        #region 变量与属性
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
        /// 非图片绘制时RadioButton色调
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(Color), "51, 161, 224")]
        [Description("非图片绘制时RadioButton色调")]
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

        private int defaultradiobuttonwidth = 12;
        /// <summary>
        /// 选择框大小
        /// </summary>
        [Category("Skin")]
        [DefaultValue(12)]
        [Description("选择框大小")]
        public int DefaultRadioButtonWidth {
            get { return defaultradiobuttonwidth; }
            set {
                if (defaultradiobuttonwidth != value) {
                    defaultradiobuttonwidth = value;
                    this.Invalidate();
                }
            }
        }

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
            base.OnMouseEnter(e);
            ControlState = ControlState.Hover;
        }

        //离开时
        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            ControlState = ControlState.Normal;
        }

        //按下时
        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 1) {
                ControlState = ControlState.Pressed;
            }
        }

        //按下并释放鼠标时
        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Left && e.Clicks == 1) {
                if (ClientRectangle.Contains(e.Location)) {
                    ControlState = ControlState.Hover;
                } else {
                    ControlState = ControlState.Normal;
                }
            }
        }

        //重绘时
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            base.OnPaintBackground(e);
            Graphics g = e.Graphics;
            //RadioButton的绘画范围
            Rectangle radioButtonrect;
            //文字绘画范围
            Rectangle textRect;
            //给CheckButtom与文字的绘画范围赋值
            CalculateRect(out radioButtonrect, out textRect);
            //抗锯齿的呈现
            g.SmoothingMode = SmoothingMode.AntiAlias;
            //最高质量绘制文字
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            //取得当前需要绘画的图像
            Bitmap btm = null;
            //当前无图像时绘画的颜色变量
            Color borderColor;
            Color innerBorderColor;
            Color checkColor;
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
            if (!this.Enabled && isEnabledDraw) //禁用状态时
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
                    g.FillEllipse(brush, radioButtonrect);
                }

                if (hover) {
                    using (Pen pen = new Pen(innerBorderColor, 2F)) {
                        g.DrawEllipse(pen, radioButtonrect);
                    }
                }

                if (Checked) {
                    radioButtonrect.Inflate(-2, -2);
                    using (GraphicsPath path = new GraphicsPath()) {
                        path.AddEllipse(radioButtonrect);
                        using (PathGradientBrush brush = new PathGradientBrush(path)) {
                            brush.CenterColor = checkColor;
                            brush.SurroundColors = new Color[] { Color.White };
                            Blend blend = new Blend();
                            blend.Positions = new float[] { 0f, 0.4f, 1f };
                            blend.Factors = new float[] { 0f, 0.4f, 1f };
                            brush.Blend = blend;
                            g.FillEllipse(brush, radioButtonrect);
                        }
                    }
                    radioButtonrect.Inflate(2, 2);
                }

                using (Pen pen = new Pen(borderColor)) {
                    g.DrawEllipse(pen, radioButtonrect);
                }
            } else {
                //画图像
                g.DrawImage(btm, radioButtonrect);
            }

            //画字体
            Color textColor = Enabled ? ForeColor : SystemColors.GrayText;
            if (!string.IsNullOrEmpty(this.Text)) {
                //是否绘画发光字体
                if (LightEffect) {
                    Image imgText = SkinTools.ImageLightEffect(Text, Font, textColor, LightEffectBack, LightEffectWidth);
                    g.DrawImage(imgText, textRect);
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
        }
        #endregion

        #region 绘画方法
        ////指定绘画图像和文本的相对位置
        //private void CalculateRect(
        //    SkinRadioButton page, Rectangle tabRect,
        //    out Rectangle imageRect, out Rectangle textRect) {
        //    Size txtsize = TextRenderer.MeasureText(page.Text, page.Font);
        //    int space = page.Text.Length == 0 ? 0 : ImgTxtSpace;
        //    imageRect = Rectangle.Empty;
        //    textRect = Rectangle.Empty;
        //    //判断图像是否为空，为空则直接返回文本的范围

        //    Image image = null;
        //    if (ImageList != null) {
        //        if (page.ImageIndex != -1) {
        //            image = ImageList.Images[page.ImageIndex];
        //        } else if (page.ImageKey != null) {
        //            image = ImageList.Images[page.ImageKey];
        //        }
        //    }
        //    if (image == null) {
        //        textRect = tabRect;
        //        return;
        //    } else {
        //        switch (ImageAlign) {
        //            case ePageImagePosition.Left:
        //                if (PageTextAlign == ContentAlignment.BottomLeft
        //                    || PageTextAlign == ContentAlignment.MiddleLeft
        //                    || PageTextAlign == ContentAlignment.TopLeft) {
        //                    imageRect = new Rectangle(
        //                        tabRect.X,
        //                        tabRect.Y + (tabRect.Height - ImgSize.Height) / 2,
        //                        ImgSize.Width,
        //                        ImgSize.Height);
        //                    textRect = new Rectangle(
        //                        imageRect.Right + space,
        //                        tabRect.Y + (tabRect.Height - txtsize.Height) / 2,
        //                        //选项卡宽度  -  图片宽度      - 图片左边距               - 图片右边距
        //                        tabRect.Width - ImgSize.Width - (imageRect.X - tabRect.X) - space,
        //                        txtsize.Height);
        //                } else if (PageTextAlign == ContentAlignment.BottomCenter
        //                      || PageTextAlign == ContentAlignment.MiddleCenter
        //                      || PageTextAlign == ContentAlignment.TopCenter) {
        //                    imageRect = new Rectangle(
        //                        tabRect.X + (tabRect.Width - (ImgSize.Width + txtsize.Width + space)) / 2,
        //                        tabRect.Y + (tabRect.Height - ImgSize.Height) / 2,
        //                        ImgSize.Width,
        //                        ImgSize.Height);
        //                    textRect = new Rectangle(
        //                        imageRect.Right + space,
        //                        tabRect.Y + (tabRect.Height - txtsize.Height) / 2,
        //                        //选项卡宽度  -  图片宽度      - 图片左边距               - 图片右边距
        //                        tabRect.Width - ImgSize.Width - (imageRect.X - tabRect.X) - space,
        //                        txtsize.Height);
        //                } else {
        //                    imageRect = new Rectangle(
        //                        tabRect.X + tabRect.Width - (txtsize.Width + space) - ImgSize.Width,
        //                        tabRect.Y + (tabRect.Height - ImgSize.Height) / 2,
        //                        ImgSize.Width,
        //                        ImgSize.Height);
        //                    textRect = new Rectangle(
        //                        imageRect.Right + space,
        //                        tabRect.Y + (tabRect.Height - txtsize.Height) / 2,
        //                        txtsize.Width,
        //                        txtsize.Height);
        //                }
        //                break;
        //            case ePageImagePosition.Top:
        //                imageRect = new Rectangle(
        //                    tabRect.X + (tabRect.Width - ImgSize.Width) / 2,
        //                    tabRect.Y + (tabRect.Height - (ImgSize.Height + txtsize.Height + space)) / 2,
        //                    ImgSize.Width,
        //                    ImgSize.Height);
        //                textRect = new Rectangle(
        //                    tabRect.X,
        //                    imageRect.Bottom + space,
        //                    tabRect.Width,
        //                    tabRect.Height - (imageRect.Bottom + space - tabRect.Top));
        //                break;
        //            case ePageImagePosition.Right:
        //                if (PageTextAlign == ContentAlignment.BottomLeft
        //                    || PageTextAlign == ContentAlignment.MiddleLeft
        //                    || PageTextAlign == ContentAlignment.TopLeft) {
        //                    imageRect = new Rectangle(
        //                        tabRect.X + txtsize.Width + space,
        //                        tabRect.Y + (tabRect.Height - ImgSize.Height) / 2,
        //                        ImgSize.Width,
        //                        ImgSize.Height);
        //                    textRect = new Rectangle(
        //                        tabRect.X,
        //                        tabRect.Y + (tabRect.Height - txtsize.Height) / 2,
        //                        txtsize.Width,
        //                        txtsize.Height);
        //                } else if (PageTextAlign == ContentAlignment.BottomCenter
        //                      || PageTextAlign == ContentAlignment.MiddleCenter
        //                      || PageTextAlign == ContentAlignment.TopCenter) {
        //                    imageRect = new Rectangle(
        //                        tabRect.X + (tabRect.Width - (ImgSize.Width + txtsize.Width + space)) / 2 + (txtsize.Width + space),
        //                        tabRect.Y + (tabRect.Height - ImgSize.Height) / 2,
        //                        ImgSize.Width,
        //                        ImgSize.Height);
        //                    textRect = new Rectangle(
        //                        imageRect.X - txtsize.Width - space,
        //                        tabRect.Y + (tabRect.Height - txtsize.Height) / 2,
        //                        txtsize.Width,
        //                        txtsize.Height);
        //                } else {
        //                    imageRect = new Rectangle(
        //                        tabRect.X + tabRect.Width - ImgSize.Width,
        //                        tabRect.Y + (tabRect.Height - ImgSize.Height) / 2,
        //                        ImgSize.Width,
        //                        ImgSize.Height);
        //                    textRect = new Rectangle(
        //                        imageRect.X - txtsize.Width - space,
        //                        tabRect.Y + (tabRect.Height - txtsize.Height) / 2,
        //                        txtsize.Width,
        //                        txtsize.Height);
        //                }
        //                break;
        //            case ePageImagePosition.Bottom:
        //                imageRect = new Rectangle(
        //                    tabRect.X + (tabRect.Width - ImgSize.Width) / 2,
        //                    tabRect.Y + (tabRect.Height - (ImgSize.Height + txtsize.Height + space)) / 2 + (txtsize.Height + space),
        //                    ImgSize.Width,
        //                    ImgSize.Height);
        //                textRect = new Rectangle(
        //                    tabRect.X,
        //                    tabRect.Y,
        //                    tabRect.Width,
        //                    imageRect.Y - tabRect.Y - space);
        //                break;
        //        }
        //    }
        //}

        private void CalculateRect(
            out Rectangle radioButtonRect, out Rectangle textRect) {
            radioButtonRect = new Rectangle(
                0, 0, DefaultRadioButtonWidth, DefaultRadioButtonWidth);
            textRect = Rectangle.Empty;
            bool bCheckAlignLeft = (int)(LeftAligbment & CheckAlign) != 0;
            bool bCheckAlignRight = (int)(RightAlignment & CheckAlign) != 0;
            bool bRightToLeft = RightToLeft == RightToLeft.Yes;

            if ((bCheckAlignLeft && !bRightToLeft) ||
                (bCheckAlignRight && bRightToLeft)) {
                switch (CheckAlign) {
                    case ContentAlignment.TopRight:
                    case ContentAlignment.TopLeft:
                        radioButtonRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleRight:
                    case ContentAlignment.MiddleLeft:
                        radioButtonRect.Y = (Height - DefaultRadioButtonWidth) / 2;
                        break;
                    case ContentAlignment.BottomRight:
                    case ContentAlignment.BottomLeft:
                        radioButtonRect.Y = Height - DefaultRadioButtonWidth - 2;
                        break;
                }

                radioButtonRect.X = 1;

                textRect = new Rectangle(
                    radioButtonRect.Right + 2,
                    0,
                    Width - radioButtonRect.Right - 4,
                    Height);
            } else if ((bCheckAlignRight && !bRightToLeft)
                  || (bCheckAlignLeft && bRightToLeft)) {
                switch (CheckAlign) {
                    case ContentAlignment.TopLeft:
                    case ContentAlignment.TopRight:
                        radioButtonRect.Y = 2;
                        break;
                    case ContentAlignment.MiddleLeft:
                    case ContentAlignment.MiddleRight:
                        radioButtonRect.Y = (Height - DefaultRadioButtonWidth) / 2;
                        break;
                    case ContentAlignment.BottomLeft:
                    case ContentAlignment.BottomRight:
                        radioButtonRect.Y = Height - DefaultRadioButtonWidth - 2;
                        break;
                }

                radioButtonRect.X = Width - DefaultRadioButtonWidth - 1;

                textRect = new Rectangle(
                    2, 0, Width - DefaultRadioButtonWidth - 6, Height);
            } else {
                switch (CheckAlign) {
                    case ContentAlignment.TopCenter:
                        radioButtonRect.Y = 2;
                        textRect.Y = radioButtonRect.Bottom + 2;
                        textRect.Height = Height - DefaultRadioButtonWidth - 6;
                        break;
                    case ContentAlignment.MiddleCenter:
                        radioButtonRect.Y = (Height - DefaultRadioButtonWidth) / 2;
                        textRect.Y = 0;
                        textRect.Height = Height;
                        break;
                    case ContentAlignment.BottomCenter:
                        radioButtonRect.Y = Height - DefaultRadioButtonWidth - 2;
                        textRect.Y = 0;
                        textRect.Height = Height - DefaultRadioButtonWidth - 6;
                        break;
                }

                radioButtonRect.X = (Width - DefaultRadioButtonWidth) / 2;

                textRect.X = 2;
                textRect.Width = Width - 4;
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
        #endregion
    }
}
