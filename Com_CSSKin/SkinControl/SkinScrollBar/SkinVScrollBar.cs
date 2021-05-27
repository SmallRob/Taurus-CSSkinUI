
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Com_CSSkin.SkinControl
{
    [ToolboxItem(true)]
    public class SkinVScrollBar : SkinScrollBarBase
    {
        protected override System.Windows.Forms.Orientation BarOrientation
        {
            get { return System.Windows.Forms.Orientation.Vertical; }
        }
    }
}
