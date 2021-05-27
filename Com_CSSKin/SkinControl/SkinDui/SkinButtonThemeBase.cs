
using Com_CSSkin.SkinClass;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    public class SkinButtonThemeBase : IDisposable
    {
        public Padding InnerPadding { get; set; }
        public RoundStyle RoundedStyle { get; set; }
        public int RoundedRadius { get; set; }

        public ButtonColorTable ColorTable { get; set; }
        public Font TextFont { get; set; }

        public SkinButtonThemeBase()
        {
            InnerPadding = new Padding(16, 6, 16, 6);
            RoundedStyle = RoundStyle.All;
            RoundedRadius = 4;
            TextFont = new Font("微软雅黑", 9.0f);
            ColorTable = GetColor();
        }

        private ButtonColorTable GetColor()
        {
            ButtonColorTable table = new ButtonColorTable();

            table.ForeColorNormal = table.ForeColorHover = table.ForeColorPressed = Color.Black;
            table.ForeColorDisabled = Color.Gray;

            table.BorderColorNormal = table.BorderColorHover = table.BorderColorPressed =
                table.BorderColorDisabled = Color.FromArgb(178, 183, 189);

            Color c = Color.FromArgb(231, 236, 242);
            table.BackColorNormal = c;
            table.BackColorHover = ColorHelper.GetLighterColor(c, 40);
            table.BackColorPressed = ColorHelper.GetDarkerColor(c, 10);
            table.BackColorDisabled = ColorHelper.GetLighterColor(Color.Gray, 90);

            return table;
        }

        #region IDisposable

        public void Dispose()
        {
            if (TextFont != null && !TextFont.IsSystemFont)
                TextFont.Dispose();
        }

        #endregion
    }
}
