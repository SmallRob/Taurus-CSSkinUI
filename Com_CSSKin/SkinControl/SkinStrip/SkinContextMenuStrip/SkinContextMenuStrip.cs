
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(ContextMenuStrip))]
    public class SkinContextMenuStrip : ContextMenuStrip
    {
        ToolStripColorTable colorTable;
        public SkinContextMenuStrip()
        {
            //初始化
            Init();
            colorTable = new ToolStripColorTable();
            //更新Renderer
            PaintRenderer();
        }
        #region 重载与事件
        //重绘Renderer
        public void PaintRenderer() 
        {
            if (RenderMode != ToolStripRenderMode.System)
            {
                this.Renderer = new ProfessionalToolStripRendererEx(colorTable);
            }
        }

        //Renderer更改时
        protected override void OnRendererChanged(EventArgs e)
        {
            if (RenderMode == ToolStripRenderMode.ManagerRenderMode || RenderMode == ToolStripRenderMode.Professional)
            {
                this.Renderer = new ProfessionalToolStripRendererEx(colorTable);
            }
            base.OnRendererChanged(e);
        }
        #endregion

        #region 属性
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
        [Description("箭头颜色")]
        public Color Arrow
        {
            get { return colorTable.Arrow; }
            set
            {
                colorTable.Arrow = value;
                PaintRenderer();
            }
        }

        [Category("Base")]
        [Description("Base背景颜色")]
        public Color Base
        {
            get { return colorTable.Base; }
            set
            {
                colorTable.Base = value;
                PaintRenderer();
            }
        }

        [Category("Item")]
        [Description("Item边框颜色")]
        public Color ItemBorder
        {
            get { return colorTable.ItemBorder; }
            set
            {
                colorTable.ItemBorder = value;
                PaintRenderer();
            }
        }

        [Category("Item")]
        [Description("Item圆角样式")]
        public RoundStyle ItemRadiusStyle
        {
            get { return colorTable.ItemRadiusStyle; }
            set
            {
                colorTable.ItemRadiusStyle = value;
                PaintRenderer();
            }
        }

        [Category("Item")]
        [Description("Item圆角大小")]
        public int ItemRadius
        {
            get { return colorTable.ItemRadius; }
            set
            {
                colorTable.ItemRadius = value < 1 ? 1 : value;
                PaintRenderer();
            }
        }

        [Category("Skin")]
        [Description("控件背景色")]
        public Color Back
        {
            get { return colorTable.Back; }
            set
            {
                colorTable.Back = value;
                PaintRenderer();
            }
        }

        [Category("Item")]
        [Description("Item分隔符颜色")]
        public Color ItemSplitter
        {
            get { return colorTable.BaseItemSplitter; }
            set
            {
                colorTable.BaseItemSplitter = value;
                PaintRenderer();
            }
        }

        [Category("Item")]
        [Description("Item悬浮时背景色")]
        public Color ItemHover
        {
            get { return colorTable.ItemHover; }
            set
            {
                colorTable.ItemHover = value;
                PaintRenderer();
            }
        }

        [Category("Item")]
        [Description("Item按下时背景色")]
        public Color ItemPressed
        {
            get { return colorTable.ItemPressed; }
            set
            {
                colorTable.ItemPressed = value;
                PaintRenderer();
            }
        }

        [Category("Item")]
        [Description("Item是否启用渐变")]
        public bool ItemAnamorphosis
        {
            get { return colorTable.ItemAnamorphosis; }
            set
            {
                colorTable.ItemAnamorphosis = value;
                PaintRenderer();
            }
        }

        [Category("Item")]
        [Description("Item是否显示边框")]
        public bool ItemBorderShow
        {
            get { return colorTable.ItemBorderShow; }
            set
            {
                colorTable.ItemBorderShow = value;
                PaintRenderer();
            }
        }

        [Category("Skin")]
        [Description("控件字体颜色")]
        public Color Fore
        {
            get { return colorTable.Fore; }
            set
            {
                colorTable.Fore = value;
                PaintRenderer();
            }
        }

        [Category("Skin")]
        [Description("控件悬浮时字体颜色")]
        public Color HoverFore
        {
            get { return colorTable.HoverFore; }
            set
            {
                colorTable.HoverFore = value;
                PaintRenderer();
            }
        }

        [Category("Skin")]
        [Description("弹出菜单分隔符与边框的颜色")]
        public Color DropDownImageSeparator
        {
            get { return colorTable.DropDownImageSeparator; }
            set
            {
                colorTable.DropDownImageSeparator = value;
                PaintRenderer();
            }
        }

        [Category("Skin")]
        [Description("控件圆角大小")]
        public int BackRadius
        {
            get { return colorTable.BackRadius; }
            set
            {
                colorTable.BackRadius = value < 1 ? 1 : value;
                PaintRenderer();
            }
        }

        [Category("Skin")]
        [Description("控件圆角样式")]
        public RoundStyle RadiusStyle
        {
            get { return colorTable.RadiusStyle; }
            set
            {
                colorTable.RadiusStyle = value;
                PaintRenderer();
            }
        }

        [Category("Title")]
        [Description("菜单标头背景色")]
        public Color TitleColor
        {
            get { return colorTable.TitleColor; }
            set
            {
                colorTable.TitleColor = value;
                PaintRenderer();
            }
        }

        [Category("Title")]
        [Description("菜单标头背景色是否启用渐变")]
        public bool TitleAnamorphosis
        {
            get { return colorTable.TitleAnamorphosis; }
            set
            {
                colorTable.TitleAnamorphosis = value;
                PaintRenderer();
            }
        }

        [Category("Title")]
        [Description("菜单标头圆角大小")]
        public int TitleRadius
        {
            get { return colorTable.TitleRadius; }
            set
            {
                colorTable.TitleRadius = value < 1 ? 1 : value;
                PaintRenderer();
            }
        }

        [Category("Title")]
        [Description("菜单标头圆角样式")]
        public RoundStyle TitleRadiusStyle
        {
            get { return colorTable.TitleRadiusStyle; }
            set
            {
                colorTable.TitleRadiusStyle = value;
                PaintRenderer();
            }
        }
        #endregion

        #region 初始化
        public void Init()
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.UserPaint, true);//自行绘制
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
        }
        #endregion
    }
}
