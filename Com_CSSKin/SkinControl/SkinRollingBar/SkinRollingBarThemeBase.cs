
using System;
using System.Drawing;

namespace Com_CSSkin.SkinControl
{
    public class SkinRollingBarThemeBase
    {
        public int Radius1 { get; set; }
        public int Radius2 { get; set; }
        public int SpokeNum { get; set; }
        public float PenWidth { get; set; }
        public Color BackColor { get; set; }
        public Color BaseColor { get; set; }
        public Color DiamondColor { get; set; }

        public SkinRollingBarThemeBase()
        {
            Radius1 = 10;
            Radius2 = 24;
            SpokeNum = 12;
            PenWidth = 2;
            BackColor = Color.Transparent;
            BaseColor = Color.Red;
            DiamondColor = Color.White;
        }
    }
}
