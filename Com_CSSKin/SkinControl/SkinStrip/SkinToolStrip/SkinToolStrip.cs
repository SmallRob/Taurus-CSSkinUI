
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(ToolStrip))]
    public class SkinToolStrip : ToolStrip
    {
        ToolStripColorTable colorTable;
        public SkinToolStrip() {
            //初始化
            Init();
            colorTable = new ToolStripColorTable();
            //更新Renderer
            PaintRenderer();
        }
        #region 属性
        private TabControl bindTabControl;
        [Category("Skin")]
        [Description("绑定要操作的TabControl")]
        public TabControl BindTabControl {
            get { return bindTabControl; }
            set { bindTabControl = value; }
        }


        [Category("Base")]
        [Description("九宫绘画区域")]
        public Rectangle BackRectangle {
            get { return colorTable.BackRectangle; }
            set {
                colorTable.BackRectangle = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("BaseItem悬浮时背景图")]
        public Image BaseItemMouse {
            get { return colorTable.BaseItemMouse; }
            set {
                colorTable.BaseItemMouse = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("BaseItem按下时背景图")]
        public Image BaseItemDown {
            get { return colorTable.BaseItemDown; }
            set {
                colorTable.BaseItemDown = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("BaseItem默认背景图")]
        public Image BaseItemNorml {
            get { return colorTable.BaseItemNorml; }
            set {
                colorTable.BaseItemNorml = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("BaseItem文本偏移度")]
        public Point BaseForeOffset {
            get { return colorTable.BaseForeOffset; }
            set {
                colorTable.BaseForeOffset = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("BaseItem字体是否有辉光效果")]
        public bool BaseForeAnamorphosis {
            get { return colorTable.BaseForeAnamorphosis; }
            set {
                colorTable.BaseForeAnamorphosis = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("BaseItem辉光字体光圈大小")]
        public int BaseForeAnamorphosisBorder {
            get { return colorTable.BaseForeAnamorphosisBorder; }
            set {
                colorTable.BaseForeAnamorphosisBorder = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("BaseItem辉光字体光圈颜色")]
        public Color BaseForeAnamorphosisColor {
            get { return colorTable.BaseForeAnamorphosisColor; }
            set {
                colorTable.BaseForeAnamorphosisColor = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("BaseItem分隔符颜色")]
        public Color BaseItemSplitter {
            get { return colorTable.BaseItemSplitter; }
            set {
                colorTable.BaseItemSplitter = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("BaseItem点击时颜色")]
        public Color BaseItemPressed {
            get { return colorTable.BaseItemPressed; }
            set {
                colorTable.BaseItemPressed = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("BaseItem悬浮时颜色")]
        public Color BaseItemHover {
            get { return colorTable.BaseItemHover; }
            set {
                colorTable.BaseItemHover = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("BaseItem边框颜色")]
        public Color BaseItemBorder {
            get { return colorTable.BaseItemBorder; }
            set {
                colorTable.BaseItemBorder = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("BaseItem是否显示边框")]
        public bool BaseItemBorderShow {
            get { return colorTable.BaseItemBorderShow; }
            set {
                colorTable.BaseItemBorderShow = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("颜色绘制BaseItem时，是否启用颜色渐变效果")]
        public bool BaseItemAnamorphosis {
            get { return colorTable.BaseItemAnamorphosis; }
            set {
                colorTable.BaseItemAnamorphosis = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("Base圆角大小")]
        public int BaseItemRadius {
            get { return colorTable.BaseItemRadius; }
            set {
                colorTable.BaseItemRadius = value < 1 ? 1 : value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("Base圆角样式")]
        public RoundStyle BaseItemRadiusStyle {
            get { return colorTable.BaseItemRadiusStyle; }
            set {
                colorTable.BaseItemRadiusStyle = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("Base字体颜色")]
        public Color BaseFore {
            get { return colorTable.BaseFore; }
            set {
                colorTable.BaseFore = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("Base悬浮时字体颜色")]
        public Color BaseHoverFore {
            get { return colorTable.BaseHoverFore; }
            set {
                colorTable.BaseHoverFore = value;
                PaintRenderer();
            }
        }

        [Category("Skin")]
        [Description("箭头颜色")]
        public Color Arrow {
            get { return colorTable.Arrow; }
            set {
                colorTable.Arrow = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("Base背景颜色")]
        public Color Base {
            get { return colorTable.Base; }
            set {
                colorTable.Base = value;
                PaintRenderer();
            }
        }

        [Category("Item")]
        [Description("Item边框颜色")]
        public Color ItemBorder {
            get { return colorTable.ItemBorder; }
            set {
                colorTable.ItemBorder = value;
                PaintRenderer();
            }
        }

        [Category("Item")]
        [Description("Item圆角样式")]
        public RoundStyle ItemRadiusStyle {
            get { return colorTable.ItemRadiusStyle; }
            set {
                colorTable.ItemRadiusStyle = value;
                PaintRenderer();
            }
        }

        [Category("Item")]
        [Description("Item圆角大小")]
        public int ItemRadius {
            get { return colorTable.ItemRadius; }
            set {
                colorTable.ItemRadius = value < 1 ? 1 : value;
                PaintRenderer();
            }
        }

        [Category("Skin")]
        [Description("字体颜色是否统一变换")]
        public bool SkinAllColor {
            get { return colorTable.SkinAllColor; }
            set {
                colorTable.SkinAllColor = value;
                PaintRenderer();
            }
        }

        [Category("Skin")]
        [Description("控件背景色")]
        public Color Back {
            get { return colorTable.Back; }
            set {
                colorTable.Back = value;
                PaintRenderer();
            }
        }

        [Category("Item")]
        [Description("Item悬浮时背景色")]
        public Color ItemHover {
            get { return colorTable.ItemHover; }
            set {
                colorTable.ItemHover = value;
                PaintRenderer();
            }
        }

        [Category("Item")]
        [Description("Item按下时背景色")]
        public Color ItemPressed {
            get { return colorTable.ItemPressed; }
            set {
                colorTable.ItemPressed = value;
                PaintRenderer();
            }
        }

        [Category("Item")]
        [Description("Item是否启用渐变")]
        public bool ItemAnamorphosis {
            get { return colorTable.ItemAnamorphosis; }
            set {
                colorTable.ItemAnamorphosis = value;
                PaintRenderer();
            }
        }

        [Category("Item")]
        [Description("Item是否显示边框")]
        public bool ItemBorderShow {
            get { return colorTable.ItemBorderShow; }
            set {
                colorTable.ItemBorderShow = value;
                PaintRenderer();
            }
        }

        [Category("Skin")]
        [Description("控件字体颜色")]
        public Color Fore {
            get { return colorTable.Fore; }
            set {
                colorTable.Fore = value;
                PaintRenderer();
            }
        }

        [Category("Skin")]
        [Description("控件悬浮时字体颜色")]
        public Color HoverFore {
            get { return colorTable.HoverFore; }
            set {
                colorTable.HoverFore = value;
                PaintRenderer();
            }
        }

        [Category("Skin")]
        [Description("弹出菜单分隔符与边框的颜色")]
        public Color DropDownImageSeparator {
            get { return colorTable.DropDownImageSeparator; }
            set {
                colorTable.DropDownImageSeparator = value;
                PaintRenderer();
            }
        }

        [Category("Skin")]
        [Description("控件圆角大小")]
        public int BackRadius {
            get { return colorTable.BackRadius; }
            set {
                colorTable.BackRadius = value < 1 ? 1 : value;
                PaintRenderer();
            }
        }

        [Category("Skin")]
        [Description("控件圆角样式")]
        public RoundStyle RadiusStyle {
            get { return colorTable.RadiusStyle; }
            set {
                colorTable.RadiusStyle = value;
                PaintRenderer();
            }
        }

        [Category("Title")]
        [Description("菜单标头背景色")]
        public Color TitleColor {
            get { return colorTable.TitleColor; }
            set {
                colorTable.TitleColor = value;
                PaintRenderer();
            }
        }

        [Category("Title")]
        [Description("菜单标头背景色是否启用渐变")]
        public bool TitleAnamorphosis {
            get { return colorTable.TitleAnamorphosis; }
            set {
                colorTable.TitleAnamorphosis = value;
                PaintRenderer();
            }
        }

        [Category("Title")]
        [Description("菜单标头圆角大小")]
        public int TitleRadius {
            get { return colorTable.TitleRadius; }
            set {
                colorTable.TitleRadius = value < 1 ? 1 : value;
                PaintRenderer();
            }
        }

        [Category("Title")]
        [Description("菜单标头圆角样式")]
        public RoundStyle TitleRadiusStyle {
            get { return colorTable.TitleRadiusStyle; }
            set {
                colorTable.TitleRadiusStyle = value;
                PaintRenderer();
            }
        }
        #endregion

        #region 初始化
        public void Init() {
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
        }
        #endregion

        #region 重载与事件
        //重绘Renderer
        public void PaintRenderer() {
            if (RenderMode != ToolStripRenderMode.System) {
                this.Renderer = new ProfessionalToolStripRendererEx(colorTable);
            }
        }

        //Renderer更改时
        protected override void OnRendererChanged(EventArgs e) {
            if (RenderMode == ToolStripRenderMode.ManagerRenderMode || RenderMode == ToolStripRenderMode.Professional) {
                this.Renderer = new ProfessionalToolStripRendererEx(colorTable);
            }
            base.OnRendererChanged(e);
        }

        #endregion

        #region 实现与TabControl的绑定
        /// <summary>
        /// 初始化绑定的Tab
        /// </summary>
        protected override void OnCreateControl() {
            base.OnCreateControl();
            if (bindTabControl != null) {
                if (!DesignMode) {
                    if (bindTabControl is SkinTabControl && !DesignMode) {
                        SkinTabControl tab = (SkinTabControl)bindTabControl;
                        tab.DrawType = DrawStyle.None;
                    }
                    bindTabControl.ItemSize = new Size(0, 1);
                }
                bindTabControl.SelectedIndexChanged += bindTabControl_SelectedIndexChanged;
                bindTabControl_SelectedIndexChanged(bindTabControl, null);
            }
        }

        /// <summary>
        /// 每添加一个ToolStripButton都给他加一个CheckedChanged事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnItemAdded(ToolStripItemEventArgs e) {
            base.OnItemAdded(e);
            if (bindTabControl != null) {
                if (e.Item is ToolStripButton) {
                    ToolStripButton btn = (ToolStripButton)e.Item;
                    btn.CheckedChanged += btn_CheckedChanged;
                }
            }
        }

        /// <summary>
        /// 每一项Btn的CheckedChanged事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btn_CheckedChanged(object sender, EventArgs e) {
            ToolStripButton btnCheck = (ToolStripButton)sender;
            if (btnCheck.Checked) {
                if (bindTabControl != null) {
                    int index;
                    bool IsInt = int.TryParse(Convert.ToString(btnCheck.Tag), out index);
                    if (IsInt) {
                        if (index >= 0 && index <= bindTabControl.TabPages.Count - 1) {
                            foreach (var item in this.Items) {
                                if (item is ToolStripButton) {
                                    ToolStripButton btn = (ToolStripButton)item;
                                    btn.Checked = item == btnCheck;
                                }
                            }
                            bindTabControl.SelectedIndex = index;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 实现TabControl切换Tabpage时也切换Item按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bindTabControl_SelectedIndexChanged(object sender, EventArgs e) {
            if (sender != null) {
                TabControl tab = (TabControl)sender;
                if (tab != null) {
                    //如果属性中的Tab和操作事件响应的Tab是同一个
                    if (tab == bindTabControl) {
                        for (int i = 0; i < Items.Count; i++) {
                            if (Items[i] is ToolStripButton) {
                                int index;
                                bool IsInt = int.TryParse(Convert.ToString(Items[i].Tag), out index);
                                if (IsInt) {
                                    ToolStripButton btn = (ToolStripButton)Items[i];
                                    btn.Checked = index == tab.SelectedIndex;
                                }
                            }
                        }
                    } else {
                        //如果属性中的Tab和操作事件响应的Tab不是同一个则清除这个事件
                        tab.SelectedIndexChanged -= bindTabControl_SelectedIndexChanged;
                    }
                }
            }
        }

        /// <summary>
        /// 实现按钮点击时切换TabPage
        /// </summary>
        /// <param name="e"></param>
        protected override void OnItemClicked(ToolStripItemClickedEventArgs e) {
            base.OnItemClicked(e);
            if (bindTabControl != null) {
                if (e.ClickedItem is ToolStripButton) {
                    ToolStripButton btnCheck = (ToolStripButton)e.ClickedItem;
                    int index;
                    bool IsInt = int.TryParse(Convert.ToString(btnCheck.Tag), out index);
                    if (IsInt) {
                        if (index >= 0 && index <= bindTabControl.TabPages.Count - 1) {
                            btnCheck.Checked = true;
                        }
                    }
                }
            }
        }
        #endregion
    }
}
