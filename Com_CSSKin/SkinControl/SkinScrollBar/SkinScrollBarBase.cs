
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    [ToolboxItem(false), DefaultEvent("ValueChanged")]
    public abstract class SkinScrollBarBase : SkinControlBase
    {
        #region 类内部变量, 属性

        private WLScrollBar _innerScrollBar;

        private WLScrollBar InnerScrollBar {
            get {
                if (_innerScrollBar == null) {
                    _innerScrollBar = new WLScrollBar(this, BarOrientation);
                }
                return _innerScrollBar;
            }
        }

        #endregion

        #region 子类要重写的属性

        protected abstract Orientation BarOrientation { get; }

        #endregion

        #region 控件事件
        [Description("在控件的值更改时发生")]
        [Category("滑块操作")]

        public event EventHandler ValueChanged {
            add {
                InnerScrollBar.ValueChanged += value;
            }
            remove {
                InnerScrollBar.ValueChanged -= value;
            }
        }

        #endregion

        #region 公开属性
        #region Attribute
        /// <summary>
        /// 滚动框位置表示的值。
        /// </summary>
        [DefaultValue(0)]
        [Category("Attribute")]
        [Description("滚动框位置表示的值。")]
        public int Value {
            get {
                return InnerScrollBar.Value;
            }
            set {
                if (InnerScrollBar.Value != value) {
                    InnerScrollBar.Value = value;
                }
            }
        }

        /// <summary>
        /// 可滚动范围的下限值。
        /// </summary>
        [DefaultValue(0), RefreshProperties(System.ComponentModel.RefreshProperties.Repaint)]
        [Category("Attribute")]
        [Description("可滚动范围的下限值。")]
        public int Minimum {
            get {
                return InnerScrollBar.Minimum;
            }
            set {
                if (InnerScrollBar.Minimum != value) {
                    InnerScrollBar.Minimum = value;
                }
            }
        }

        /// <summary>
        /// 可滚动范围的上限值。
        /// </summary>
        [DefaultValue(100), RefreshProperties(System.ComponentModel.RefreshProperties.Repaint)]
        [Category("Attribute")]
        [Description("可滚动范围的上限值。")]
        public int Maximum {
            get {
                return InnerScrollBar.Maximum;
            }
            set {
                if (InnerScrollBar.Maximum != value) {
                    InnerScrollBar.Maximum = value;
                }
            }
        }

        /// <summary>
        /// 当用户单击滚动箭头或按箭头键时，滚动框位置变动的幅度。
        /// </summary>
        [DefaultValue(1)]
        [Category("Attribute")]
        [Description("当用户单击滚动箭头或按箭头键时，滚动框位置变动的幅度。")]
        public int SmallChange {
            get {
                return InnerScrollBar.SmallChange;
            }
            set {
                if (InnerScrollBar.SmallChange != value) {
                    InnerScrollBar.SmallChange = value;
                }
            }
        }

        /// <summary>
        /// 当用户单击滚动条或按 Page Up 或 Page Down 键时，滚动框位置变动的幅度。
        /// </summary>
        [DefaultValue(10)]
        [Category("Attribute")]
        [Description("当用户单击滚动条或按 Page Up 或 Page Down 键时，滚动框位置变动的幅度。")]
        public int LargeChange {
            get {
                return InnerScrollBar.LargeChange;
            }
            set {
                if (InnerScrollBar.LargeChange != value) {
                    InnerScrollBar.LargeChange = value;
                }
            }
        }

        /// <summary>
        /// 滑块的长度
        /// </summary>
        [DefaultValue(10)]
        [Category("Attribute")]
        [Description("滑块的长度。")]
        public int MiddleButtonLengthPercentage {
            get {
                return InnerScrollBar.MiddleButtonLengthPercentage;
            }
            set {
                if (InnerScrollBar.MiddleButtonLengthPercentage != value) {
                    InnerScrollBar.MiddleButtonLengthPercentage = value;
                }
            }
        }
        #endregion

        /// <summary>
        /// 当前主题
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Category("Skin")]
        [Description("当前主题")]
        public SkinScrollBarThemeBase XTheme {
            get {
                return InnerScrollBar.XTheme;
            }
        }

        /// <summary>
        /// 按钮主题
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Category("Skin")]
        [Description("按钮主题")]
        public SkinButtonThemeBase MdlButtonTheme {
            get {
                return InnerScrollBar.XTheme.MdlButtonTheme;
            }
        }

        [Category("SkinMdlButton")]
        [DefaultValue(0)]
        [Description("滑块圆角大小")]
        public int MdlButtonRoundedRadius {
            get {
                return MdlButtonTheme.RoundedRadius;
            }
            set {
                if (MdlButtonTheme.RoundedRadius != value) {
                    MdlButtonTheme.RoundedRadius = value;
                    InnerScrollBar.Refresh();
                }
            }
        }

        [Category("Skin")]
        [DefaultValue(0)]   
        [Description("边框内填充宽度")]
        public int InnerPaddingWidth {
            get {
                return XTheme.InnerPaddingWidth;
            }
            set {
                if (XTheme.InnerPaddingWidth != value) {
                    XTheme.InnerPaddingWidth = value;
                    InnerScrollBar.Refresh();
                }
            }
        }

        ///// <summary>
        ///// 在可滚动方向上的两头的空白
        ///// </summary>
        //public int MiddleButtonOutterSpace1 { get; set; }

        ///// <summary>
        ///// 在不可滚动方向上的两头的空白
        ///// </summary>
        //public int MiddleButtonOutterSpace2 { get; set; }

        ///// <summary>
        ///// 滚动条两头的按钮的宽度(HScrollBar)或高度(VScrollBar)
        ///// </summary>
        //public int SideButtonLength { get; set; }

        ///// <summary>
        ///// 在非可移动方向上的最佳长度，对 vscroll 来说是 width, 
        ///// 对 hscroll 来说是 height
        ///// </summary>
        //public int BestUndirectLen { get; set; }

        [Category("Skin")]
        [DefaultValue(true)]
        [Description("是否绘制背景")]
        public bool DrawBackground {
            get {
                return XTheme.DrawBackground;
            }
            set {
                if (XTheme.DrawBackground != value) {
                    XTheme.DrawBackground = value;
                    InnerScrollBar.Invalidate();
                }
            }
        }
        [Category("Skin")]
        [DefaultValue(false)]
        [Description("是否绘制边框")]
        public bool DrawBorder {
            get {
                return XTheme.DrawBorder;
            }
            set {
                if (XTheme.DrawBorder != value) {
                    XTheme.DrawBorder = value;
                    InnerScrollBar.Invalidate();
                }
            }
        }
        [Category("Skin")]
        [DefaultValue(false)]
        [Description("是否绘制内部边框")]
        public bool DrawInnerBorder {
            get {
                return XTheme.DrawInnerBorder;
            }
            set {
                if (XTheme.DrawInnerBorder != value) {
                    XTheme.DrawInnerBorder = value;
                    InnerScrollBar.Invalidate();
                }
            }
        }

        //public bool ShowSideButtons { get; set; }

        /////// <summary>
        /////// 滚动条处于最大最小值时两头的按钮是否不可用
        /////// </summary>
        ////public bool SideButtonCanDisabled { get; set; }

        [Category("Skin")]
        [DefaultValue(typeof(Color), "227, 227, 227")]
        [Description("背景色")]
        public Color SkinBackColor {
            get {
                return XTheme.BackColor;
            }
            set {
                if (XTheme.BackColor != value) {
                    XTheme.BackColor = value;
                    InnerScrollBar.Invalidate();
                }
            }
        }

        [Category("Skin")]
        [DefaultValue(typeof(Color), "248, 248, 248")]
        [Description("边框色")]
        public Color BorderColor {
            get {
                return XTheme.BorderColor;
            }
            set {
                if (XTheme.BorderColor != value) {
                    XTheme.BorderColor = value;
                    InnerScrollBar.Invalidate();
                }
            }
        }

        //[Category("Skin")]
        //[Description("内部边框色")]
        //public Color InnerBorderColor {
        //    get {
        //        return XTheme.InnerBorderColor;
        //    }
        //    set {
        //        if (XTheme.InnerBorderColor != value) {
        //            XTheme.InnerBorderColor = value;
        //            InnerScrollBar.Invalidate();
        //        }
        //    }
        //}

        //public Size SideButtonForePathSize { get; set; }
        //public ButtonForePathGetter SideButtonForePathGetter { get; set; }
        ////public ButtonColorTable SideButtonColorTable { get; set; }
        ////public ButtonColorTable MiddleButtonColorTable { get; set; }
        //public ForePathRenderMode HowSideButtonForePathDraw { get; set; }

        ///// <summary>
        ///// 是否在滚动块上画3条线
        ///// </summary>
        //public bool DrawLinesInMiddleButton { get; set; }
        //public Color MiddleButtonLine1Color { get; set; }
        //public Color MiddleButtonLine2Color { get; set; }
        //public int MiddleBtnLineOutterSpace1 { get; set; }
        //public int MiddleBtnLineOutterSpace2 { get; set; }

        ////public int SideButtonRadius { get; set; }
        ////public int MiddleButtonRadius { get; set; }
        //public ButtonBorderType SideButtonBorderType { get; set; }

        /// <summary>
        /// 滚动条控件整体的圆角大小
        /// </summary>
        [Category("Skin")]
        [DefaultValue(0)]
        [Description("滚动条控件整体的圆角大小")]
        public int BackgroundRadius {
            get {
                return XTheme.BackgroundRadius;
            }
            set {
                if (XTheme.BackgroundRadius != value) {
                    XTheme.BackgroundRadius = value;
                    InnerScrollBar.Invalidate();
                }
            }
        }

        ///// <summary>
        ///// 是否在滚动条中部画连接两头SideButton的线，主要用于圆形的SideButton
        ///// </summary>
        //public bool DrawExtraMiddleLine { get; set; }

        //public Color ExtraMiddleLineColor { get; set; }
        //public int ExtraMiddleLineLength { get; set; }
        #endregion

        #region 公开事件
        /// <summary>
        /// 设置主题
        /// </summary>
        /// <param name="xtheme">主题</param>
        public void SetNewTheme(SkinScrollBarThemeBase xtheme) {
            InnerScrollBar.SetNewTheme(xtheme);
        }

        #endregion

        #region 构造函数及初始化

        public SkinScrollBarBase() {

        }

        #endregion

        #region 重写的方法

        protected override void OnResize(EventArgs e) {
            base.OnResize(e);
            InnerScrollBar.Bounds = ClientRectangle;
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            InnerScrollBar.PaintControl(e.Graphics, e.ClipRectangle);
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            InnerScrollBar.MouseOperation(e, MouseOperationType.Move);
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            InnerScrollBar.MouseOperation(e, MouseOperationType.Down);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            InnerScrollBar.MouseOperation(e, MouseOperationType.Up);
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.OnMouseLeave(e);
            InnerScrollBar.MouseOperation(Point.Empty, MouseOperationType.Leave);
        }

        protected override void OnEnabledChanged(EventArgs e) {
            base.OnEnabledChanged(e);
            InnerScrollBar.Enabled = base.Enabled;
        }

        #endregion
    }
}
