
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Com_CSSkin.Win32.Struct;
using System.Runtime.InteropServices;
using Com_CSSkin.Win32;
using System.Drawing;
using System.Drawing.Drawing2D;
using Com_CSSkin.Win32.Const;
using System.Drawing.Imaging;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    [Browsable(false)]
    [DesignTimeVisible(false)]
    [ToolboxItem(false)]
    public class BorderPanel : PanelBase
    {
        public BorderPanel()
            : base()
        {
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle rect = base.DisplayRectangle;
                int borderWidth = base.BorderWidth;
                rect.Inflate(-borderWidth, -borderWidth);
                return rect;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            OnPaintBorder(e);
        }

        protected virtual void OnPaintBorder(PaintEventArgs e)
        {
            RenderBorder(e.Graphics, base.ClientRectangle);
        }

        private void RenderBorder(Graphics g, Rectangle bounds)
        {
            if (RoundStyle == RoundStyle.None)
            {
                ControlPaint.DrawBorder(
                    g,
                    bounds,
                    ColorTable.Border,
                    ButtonBorderStyle.Solid);
            }
            else
            {
                using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
                {
                    using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                        bounds, Radius, RoundStyle, true))
                    {
                        using (Pen pen = new Pen(ColorTable.Border))
                        {
                            g.DrawPath(pen, path);
                        }
                    }
                }
            }
        }
    }
}
