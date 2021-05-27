
using Com_CSSkin.SkinClass;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Com_CSSkin.SkinControl
{
    public class SkinScrollBarThemeBase : IDisposable
    {
        public int InnerPaddingWidth { get; set; }

        /// <summary>
        /// 在可滚动方向上的两头的空白
        /// </summary>
        public int MiddleButtonOutterSpace1 { get; set; }

        /// <summary>
        /// 在不可滚动方向上的两头的空白
        /// </summary>
        public int MiddleButtonOutterSpace2 { get; set; }

        /// <summary>
        /// 滚动条两头的按钮的宽度(HScrollBar)或高度(VScrollBar)
        /// </summary>
        public int SideButtonLength { get; set; }

        /// <summary>
        /// 在非可移动方向上的最佳长度，对 vscroll 来说是 width, 
        /// 对 hscroll 来说是 height
        /// </summary>
        public int BestUndirectLen { get; set; }

        public bool DrawBackground { get; set; }
        public bool DrawBorder { get; set; }
        public bool DrawInnerBorder { get; set; }

        public bool ShowSideButtons { get; set; }

        ///// <summary>
        ///// 滚动条处于最大最小值时两头的按钮是否不可用
        ///// </summary>
        //public bool SideButtonCanDisabled { get; set; }

        public Color BackColor { get; set; }
        public Color BorderColor { get; set; }
        public Color InnerBorderColor { get; set; }

        public Size SideButtonForePathSize { get; set; }
        public ButtonForePathGetter SideButtonForePathGetter { get; set; }
        //public ButtonColorTable SideButtonColorTable { get; set; }
        //public ButtonColorTable MiddleButtonColorTable { get; set; }
        public ForePathRenderMode HowSideButtonForePathDraw { get; set; }

        /// <summary>
        /// 是否在滚动块上画3条线
        /// </summary>
        public bool DrawLinesInMiddleButton { get; set; }
        public Color MiddleButtonLine1Color { get; set; }
        public Color MiddleButtonLine2Color { get; set; }
        public int MiddleBtnLineOutterSpace1 { get; set; }
        public int MiddleBtnLineOutterSpace2 { get; set; }

        //public int SideButtonRadius { get; set; }
        //public int MiddleButtonRadius { get; set; }
        public ButtonBorderType SideButtonBorderType { get; set; }

        /// <summary>
        /// 滚动条控件整体的圆角大小
        /// </summary>
        public int BackgroundRadius { get; set; }

        /// <summary>
        /// 是否在滚动条中部画连接两头SideButton的线，主要用于圆形的SideButton
        /// </summary>
        public bool DrawExtraMiddleLine { get; set; }

        public Color ExtraMiddleLineColor { get; set; }
        public int ExtraMiddleLineLength { get; set; }

        public SkinButtonThemeBase SideButtonTheme { get; set; }
        public SkinButtonThemeBase MdlButtonTheme { get; set; }

        private void SetDefaultValue()
        {
            InnerPaddingWidth = 0;
            MiddleButtonOutterSpace1 = 1;
            MiddleButtonOutterSpace2 = 0;
            SideButtonLength = 16;
            BestUndirectLen = 15;
            DrawBackground = true;
            DrawBorder = false;
            DrawInnerBorder = false;
            ShowSideButtons = true;
            //SideButtonCanDisabled = false;
            BackColor = Color.FromArgb(227,227,227);
            BorderColor = Color.FromArgb(248, 248, 248);

            SideButtonForePathSize = new Size(7, 7);
            SideButtonForePathGetter = new ButtonForePathGetter(
                GraphicsPathHelper.Create7x4In7x7DownTriangleFlag);

            //SideButtonColorTable = SideBtnColor();
            //MiddleButtonColorTable = MdlBtnColor();            

            HowSideButtonForePathDraw = ForePathRenderMode.Draw;

            DrawLinesInMiddleButton = true;
            MiddleBtnLineOutterSpace1 = 4;
            MiddleBtnLineOutterSpace2 = 4;
            MiddleButtonLine1Color = Color.FromArgb(89, 89, 89);
            MiddleButtonLine2Color = Color.FromArgb(182, 182, 182);

            //SideButtonRadius = MiddleButtonRadius = 0;
            SideButtonBorderType = ButtonBorderType.Rectangle;

            BackgroundRadius = 0;
            DrawExtraMiddleLine = false;
            ExtraMiddleLineLength = 6;

            SideButtonTheme = GetSideButtonTheme();
            MdlButtonTheme = GetMdlButtonTheme();
        }

        private ButtonColorTable SideBtnColor()
        {
            ButtonColorTable table = MdlBtnColor();

            table.ForeColorNormal = Color.FromArgb(73, 73, 73);
            table.ForeColorHover = Color.FromArgb(32, 106, 145);
            table.ForeColorPressed = Color.FromArgb(15, 38, 50);
            table.ForeColorDisabled = SystemColors.ControlDarkDark;

            table.BackColorDisabled = Color.FromArgb(227, 227, 227);

            return table;
        }

        private ButtonColorTable MdlBtnColor()
        {
            ButtonColorTable table = new ButtonColorTable();

            table.BorderColorNormal = Color.FromArgb(151, 151, 151);
            table.BorderColorHover = Color.FromArgb(53, 111, 155);
            table.BorderColorPressed = Color.FromArgb(60, 127, 177);

            table.BackColorNormal = Color.FromArgb(217, 218, 219);
            table.BackColorHover = Color.FromArgb(169, 219, 246);
            table.BackColorPressed = Color.FromArgb(111, 202, 240);

            return table;
        }

        private SkinButtonThemeBase GetSideButtonTheme()
        {
            SkinButtonThemeBase theme = new SkinButtonThemeBase();
            theme.ColorTable = SideBtnColor();
            theme.RoundedStyle = RoundStyle.All;
            theme.RoundedRadius = 0;
            return theme;
        }

        private SkinButtonThemeBase GetMdlButtonTheme()
        {
            SkinButtonThemeBase theme = new SkinButtonThemeBase();
            theme.ColorTable = MdlBtnColor();
            theme.RoundedStyle = RoundStyle.All;
            theme.RoundedRadius = 0;
            return theme;
        }

        public SkinScrollBarThemeBase()
        {
            SetDefaultValue();
        }

        #region IDisposable

        public void Dispose()
        {
            //if (SideButtonForePath != null)
            //    SideButtonForePath.Dispose();
        }

        #endregion
    }
}
