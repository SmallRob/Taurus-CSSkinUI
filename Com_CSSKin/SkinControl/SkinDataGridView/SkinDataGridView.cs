
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Com_CSSkin.Win32.Const;
using Com_CSSkin.Win32.Struct;
using Com_CSSkin.Win32;
using Com_CSSkin.Imaging;
using Com_CSSkin.SkinClass;
using System.Reflection;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(DataGridView))]
    public partial class SkinDataGridView : DataGridView
    {
        public SkinDataGridView() {
            InitializeComponent();
            //初始化
            Init();
        }

        #region 属性

        private bool titlepalace = false;
        /// <summary>
        /// 标题背景是否开启九宫绘图
        /// </summary>
        [Category("Title")]
        [DefaultValue(typeof(bool), "false")]
        [Description("标题背景是否开启九宫绘图")]
        public bool TitlePalace {
            get { return titlepalace; }
            set {
                if (titlepalace != value) {
                    titlepalace = value;
                    this.Invalidate();
                }
            }
        }

        private Rectangle titlebackrectangle = new Rectangle(10, 10, 10, 10);
        /// <summary>
        /// 标题背景九宫绘画区域
        /// </summary>
        [Category("Title")]
        [DefaultValue(typeof(Rectangle), "10,10,10,10")]
        [Description("标题背景九宫绘画区域")]
        public Rectangle TitleBackRectangle {
            get { return titlebackrectangle; }
            set {
                if (titlebackrectangle != value) {
                    titlebackrectangle = value;
                }
                this.Invalidate();
            }
        }

        private Image titleback;
        /// <summary>
        /// 标题背景
        /// </summary>
        [Category("Title")]
        [Description("标题背景")]
        public Image TitleBack {
            get { return titleback; }
            set {
                if (titleback != value) {
                    titleback = value;
                    this.Invalidate();
                }
            }
        }

        private Color titleBackColorEnd = Color.FromArgb(0x53, 0xc4, 0xf2);
        [DefaultValue(typeof(Color), "Title")]
        [Category("Title")]
        [Description("标题行背景色")]
        public Color TitleBackColorEnd {
            get { return this.titleBackColorEnd; }
            set {
                this.titleBackColorEnd = value;
                base.Invalidate();
            }
        }

        private Color titleBackColorBegin = Color.White;
        [DefaultValue(typeof(Color), "Title")]
        [Category("Title")]
        [Description("标题行渐变背景色")]
        public Color TitleBackColorBegin {
            get { return this.titleBackColorBegin; }
            set {
                this.titleBackColorBegin = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "WindowText")]
        [Category("Title")]
        [Description("标题行单元格字体颜色")]
        public Color HeadForeColor {
            get { return this.ColumnHeadersDefaultCellStyle.ForeColor; }
            set {
                this.ColumnHeadersDefaultCellStyle.ForeColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "WindowText")]
        [Category("Title")]
        [Description("标题行单元格被选中时的字体颜色")]
        public Color HeadSelectForeColor {
            get { return this.ColumnHeadersDefaultCellStyle.SelectionForeColor; }
            set {
                this.ColumnHeadersDefaultCellStyle.SelectionForeColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "Highlight")]
        [Category("Title")]
        [Description("标题行单元格被选中时的背景颜色")]
        public Color HeadSelectBackColor {
            get { return this.ColumnHeadersDefaultCellStyle.SelectionBackColor; }
            set {
                this.ColumnHeadersDefaultCellStyle.SelectionBackColor = value;
                base.Invalidate();
            }
        }

        [Category("Title")]
        [Description("标题行单元格中文本的字体")]
        public Font HeadFont {
            get { return this.ColumnHeadersDefaultCellStyle.Font; }
            set {
                this.ColumnHeadersDefaultCellStyle.Font = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "WindowText")]
        [Category("Column")]
        [Description("行单元格字体颜色")]
        public Color ColumnForeColor {
            get { return this.RowsDefaultCellStyle.ForeColor; }
            set {
                this.RowsDefaultCellStyle.ForeColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "WindowText")]
        [Category("Column")]
        [Description("行单元格被选中时的字体颜色")]
        public Color ColumnSelectForeColor {
            get { return this.RowsDefaultCellStyle.SelectionForeColor; }
            set {
                this.RowsDefaultCellStyle.SelectionForeColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "Highlight")]
        [Category("Column")]
        [Description("行单元格被选中时的背景颜色")]
        public Color ColumnSelectBackColor {
            get { return this.RowsDefaultCellStyle.SelectionBackColor; }
            set {
                this.RowsDefaultCellStyle.SelectionBackColor = value;
                base.Invalidate();
            }
        }

        [Category("Column")]
        [Description("行单元格中文本的字体")]
        public Font ColumnFont {
            get { return this.RowsDefaultCellStyle.Font; }
            set {
                this.RowsDefaultCellStyle.Font = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "GradientActiveCaption")]
        [Category("Skin")]
        [Description("网格线的颜色")]
        public Color SkinGridColor {
            get { return this.GridColor; }
            set {
                this.GridColor = value;
                base.Invalidate();
            }
        }

        [Category("Skin")]
        [Description("用于显示控件中文本的字体。")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override Font Font {
            get {
                return base.Font;
            }
            set {
                base.Font = value;
            }
        }

        [DefaultValue(typeof(Color), "White")]
        [Category("Cell")]
        [Description("默认行颜色")]
        public Color DefaultCellBackColor {
            get { return this.DefaultCellStyle.BackColor; }
            set {
                this.DefaultCellStyle.BackColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "231, 246, 253")]
        [Category("Cell")]
        [Description("奇数行颜色")]
        public Color AlternatingCellBackColor {
            get { return this.AlternatingRowsDefaultCellStyle.BackColor; }
            set {
                this.AlternatingRowsDefaultCellStyle.BackColor = value;
                base.Invalidate();
            }
        }

        private Color mouseCellBackColor = Color.FromArgb(197, 235, 252);
        [DefaultValue(typeof(Color), "197, 235, 252")]
        [Category("Cell")]
        [Description("悬浮行颜色")]
        public Color MouseCellBackColor {
            get { return mouseCellBackColor; }
            set {
                mouseCellBackColor = value;
                base.Invalidate();
            }
        }

        private bool lineNumber = true;
        [DefaultValue(typeof(bool), "true")]
        [Category("LineNumber")]
        [Description("是否显示行号")]
        public bool LineNumber {
            get { return lineNumber; }
            set {
                lineNumber = value;
                base.Invalidate();
            }
        }

        private Color lineNumberForeColor = Color.Blue;
        [DefaultValue(typeof(Color), "Blue")]
        [Category("LineNumber")]
        [Description("行号字体颜色")]
        public Color LineNumberForeColor {
            get { return lineNumberForeColor; }
            set {
                lineNumberForeColor = value;
                base.Invalidate();
            }
        }
        #endregion

        #region 初始化
        public void Init() {
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            //this.SetStyle(ControlStyles.StandardDoubleClick, false);
            this.SetStyle(ControlStyles.Selectable, true);
            this.UpdateStyles();

            ////指示在为应用程序启用视觉样式的情况下，行标题和列标题是否使用用户当前主题的视觉样式
            //this.EnableHeadersVisualStyles = false;
            ////获取或设置默认列标题BackColor样式
            //this.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(247, 246, 239);
            ////列标题的单行方框样式
            //this.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            ////列标题行的高度
            //this.ColumnHeadersHeight = 26;
            ////用户不能使用鼠标调整列标题的高度
            //this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            ////单元格内字体位置
            //this.ColumnHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            ////单元格字体色
            //this.ColumnForeColor = SystemColors.WindowText;
            ////单元格在被选中时的背景颜色
            //ColumnSelectBackColor = System.Drawing.SystemColors.Highlight;
            ////单元格在被选中时的字体颜色
            //ColumnSelectForeColor = System.Drawing.SystemColors.HighlightText;
            ////单元格内的单元格内容的位置
            //this.RowHeadersDefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            ////行标题单元格的默认背景色
            //DefaultCellBackColor = System.Drawing.SystemColors.Window;
            ////行标题单元格的默认字体色
            //this.RowHeadersDefaultCellStyle.ForeColor = System.Drawing.SystemColors.WindowText;
            ////无边框
            //this.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            //点击编辑时框框的背景颜色
            //this.DefaultCellStyle.SelectionBackColor = Color.FromArgb(59, 188, 240);
            //点击编辑时框框的字体颜色
            //this.DefaultCellStyle.SelectionForeColor = Color.White;
            //默认列颜色
            //this.DefaultCellStyle.BackColor = Color.White;
            //用户不能用鼠标调整列标头的宽度
            //this.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            //网格线的颜色
            //this.SkinGridColor = System.Drawing.SystemColors.GradientActiveCaption;
            //控件背景
            //this.BackgroundColor = System.Drawing.SystemColors.Window;
            //控件边框样式
            //this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            //是否启用手动列重新放置
            //this.AllowUserToOrderColumns = true;
            //是否自动产生列
            this.AutoGenerateColumns = true;
            //奇数行的颜色
            //AlternatingCellBackColor = Color.FromArgb(231, 246, 253);

            ////滚动条
            //base.HorizontalScrollBar.HandleCreated += new EventHandler(this.method_1);
            //base.HorizontalScrollBar.HandleDestroyed += new EventHandler(this.method_2);
            //base.VerticalScrollBar.HandleCreated += new EventHandler(this.method_3);
            //base.VerticalScrollBar.HandleDestroyed += new EventHandler(this.method_4);
        }

        //private SBManger bar;
        //private SBManger bar2;
        //private bool vsbar = false;
        //private void method_1(object sender, EventArgs e) {
        //    if (this.vsbar && (this.bar != null)) {
        //        this.bar.Dispose();
        //    }
        //    this.bar = new SBManger(base.HorizontalScrollBar);
        //}

        //private void method_2(object sender, EventArgs e) {
        //    if (this.bar != null) {
        //        this.bar.Dispose();
        //        this.bar = null;
        //    }
        //}

        //private void method_3(object sender, EventArgs e) {
        //    if (this.vsbar && (this.bar2 != null)) {
        //        this.bar2.Dispose();
        //    }
        //    this.bar2 = new SBManger(base.VerticalScrollBar);
        //}

        //private void method_4(object sender, EventArgs e) {
        //    if (this.bar2 != null) {
        //        this.bar2.Dispose();
        //        this.bar2 = null;
        //    }
        //}

        protected override void OnVisibleChanged(EventArgs e) {

            try {
                base.OnVisibleChanged(e);
                //if (!this.vsbar && base.Visible) {
                //    if (this.bar != null) {
                //        this.bar.Dispose();
                //        this.bar = null;
                //    }
                //    if (base.HorizontalScrollBar.Visible) {
                //        this.bar = new SBManger(base.HorizontalScrollBar);
                //    }
                //    if (this.bar2 != null) {
                //        this.bar2.Dispose();
                //        this.bar2 = null;
                //    }
                //    if (base.VerticalScrollBar.Visible) {
                //        this.bar2 = new SBManger(base.VerticalScrollBar);
                //    }
                //    this.vsbar = true;
                //}
            } catch {
            }
        }
        #endregion

        #region 默认行重绘
        protected override void OnCellPainting(
            DataGridViewCellPaintingEventArgs e) {
            if (e.RowIndex == -1 || e.ColumnIndex == -1) {
                //是否使用图片绘制标题背景
                if (TitleBack.IsNull()) {
                    using (Brush brush = new LinearGradientBrush(
                     e.CellBounds, TitleBackColorBegin, TitleBackColorEnd, LinearGradientMode.Vertical)) {
                        e.Graphics.FillRectangle(brush, e.CellBounds);
                    }
                } else {
                    //是否启用九宫绘图
                    if (TitlePalace) {
                        ImageDrawRect.DrawRect(e.Graphics, (Bitmap)TitleBack, e.CellBounds, Rectangle.FromLTRB(TitleBackRectangle.X, TitleBackRectangle.Y, TitleBackRectangle.Width, TitleBackRectangle.Height), 1, 1);
                    } else {
                        e.Graphics.DrawImage((Bitmap)TitleBack, e.CellBounds);
                    }
                }

                DataGridViewPaintParts pa =
                    DataGridViewPaintParts.Border |
                    DataGridViewPaintParts.ContentBackground |
                    DataGridViewPaintParts.ContentForeground |
                    DataGridViewPaintParts.ErrorIcon |
                    DataGridViewPaintParts.Focus |
                    DataGridViewPaintParts.SelectionBackground;
                e.Paint(e.ClipBounds, pa);
                e.Handled = true;
            }
            base.OnCellPainting(e);
        }
        #endregion

        #region 悬浮行颜色
        //悬浮时，保存默认颜色
        private Color defaultcolor;
        protected override void OnCellMouseEnter(DataGridViewCellEventArgs e) {
            base.OnCellMouseEnter(e);
            try {
                defaultcolor = Rows[e.RowIndex].DefaultCellStyle.BackColor;
            } catch (Exception) {
            }
        }

        //移到单元格时的颜色
        protected override void OnCellMouseMove(DataGridViewCellMouseEventArgs e) {
            base.OnCellMouseMove(e);
            try {
                Rows[e.RowIndex].DefaultCellStyle.BackColor = MouseCellBackColor;
            } catch (Exception) {
            }
        }

        //离开时还原颜色
        protected override void OnCellMouseLeave(DataGridViewCellEventArgs e) {
            base.OnCellMouseLeave(e);
            try {
                Rows[e.RowIndex].DefaultCellStyle.BackColor = defaultcolor;
            } catch (Exception) {

            }
        }
        #endregion

        #region 在生成列表时添加一个行号，颜色默认为红色
        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e) {
            base.OnRowPostPaint(e);
            if (LineNumber && this.RowHeadersVisible) {
                //最高质量绘制文字
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                //自动编号与数据库无关
                Rectangle rectangle = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, RowHeadersWidth - 4, e.RowBounds.Height);
                TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), RowHeadersDefaultCellStyle.Font, rectangle,
                 LineNumberForeColor, TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter);
            }
        }
        #endregion

        #region 滚动条
        cScrollBar csV;
        cScrollBar csH;
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (!DesignMode) {
                SetupScrollBars();
            }
        }

        protected override void OnHandleDestroyed(EventArgs e) {
            base.OnHandleDestroyed(e);
            if (!DesignMode) {
                if (csV != null) {
                    csV.Dispose();
                }
                if (csH != null) {
                    csH.Dispose();
                }
            }
        }

        private void SetupScrollBars() {
            #region 色调模式
            ////色调模式
            //// Vertical Scroll Bar Replacement  
            //Type t = typeof(System.Windows.Forms.DataGridView);
            //FieldInfo fi = t.GetField("vertScrollBar", BindingFlags.NonPublic | BindingFlags.Instance);
            //if (fi == null) return;
            //System.Windows.Forms.ScrollBar sb = fi.GetValue(this) as System.Windows.Forms.ScrollBar;
            //if (sb == null) return;
            //csV = new cScrollBar(sb.Handle, Orientation.Vertical);
            //// Horizontal Scroll Bar Replacement   
            //fi = t.GetField("horizScrollBar", BindingFlags.NonPublic | BindingFlags.Instance);
            //if (fi == null) return;
            //sb = fi.GetValue(this) as System.Windows.Forms.ScrollBar;
            //if (sb == null) return;
            //csH = new cScrollBar(sb.Handle, Orientation.Horizontal); 
            #endregion

            #region 图片模式
            // Vertical Scroll Bar Replacement  
            Type t = typeof(System.Windows.Forms.DataGridView);
            FieldInfo fi = t.GetField("vertScrollBar", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi == null) return;
            System.Windows.Forms.ScrollBar sb = fi.GetValue(this) as System.Windows.Forms.ScrollBar;
            if (sb == null) return;
            csV = new cScrollBar(sb.Handle, Orientation.Vertical,
               ScrollBarDrawImage.ScrollVertThumb,
               ScrollBarDrawImage.ScrollVertShaft,
               ScrollBarDrawImage.ScrollVertArrow,
               ScrollBarDrawImage.Fader);
            // Horizontal Scroll Bar Replacement   
            fi = t.GetField("horizScrollBar", BindingFlags.NonPublic | BindingFlags.Instance);
            if (fi == null) return;
            sb = fi.GetValue(this) as System.Windows.Forms.ScrollBar;
            if (sb == null) return;
            csH = new cScrollBar(sb.Handle, Orientation.Horizontal,
                ScrollBarDrawImage.ScrollHorzThumb,
                ScrollBarDrawImage.ScrollHorzShaft,
                ScrollBarDrawImage.ScrollHorzArrow,
                ScrollBarDrawImage.Fader);
            #endregion
        }
        #endregion
    }
}
