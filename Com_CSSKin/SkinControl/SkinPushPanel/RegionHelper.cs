
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Windows.Forms;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    public static class RegionHelper
    {
        public static void CreateRegion(
            Control control,
            Rectangle bounds,
            int radius,
            RoundStyle roundStyle)
        {
            if (roundStyle != RoundStyle.None)
            {
                using (GraphicsPath path =
                    GraphicsPathHelper.CreatePath(
                    bounds, radius, roundStyle, true))
                {
                    Region region = new Region(path);
                    path.Widen(Pens.White);
                    region.Union(path);
                    if (control.Region != null)
                    {
                        control.Region.Dispose();
                    }
                    control.Region = region;
                }
            }
            else
            {
                if (control.Region != null)
                {
                    control.Region.Dispose();
                    control.Region = null;
                }
            }
        }

        public static void CreateRegion(
            Control control,
            Rectangle bounds)
        {
            CreateRegion(control, bounds, 8, RoundStyle.All);
        }
    }
}
