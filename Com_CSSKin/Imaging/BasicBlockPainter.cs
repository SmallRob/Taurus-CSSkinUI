
using Com_CSSkin.SkinClass;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Com_CSSkin.Imaging
{
    public class BasicBlockPainter
    {

        #region static method

        public static void RenderFlatBackground(Graphics g, Rectangle rect, Color backColor,
            ButtonBorderType borderType, int radius, RoundStyle roundType)
        {
            SmoothingMode newMode;
            bool simpleRect = (borderType == ButtonBorderType.Rectangle && (roundType == RoundStyle.None || radius < 2));
            if (simpleRect)
            {
                newMode = SmoothingMode.HighSpeed;
            }
            else
            {
                newMode = SmoothingMode.AntiAlias;
                rect.Width--;
                rect.Height--;
            }
            using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, newMode))
            {
                using (SolidBrush sb = new SolidBrush(backColor))
                {
                    if (simpleRect)
                    {
                        g.FillRectangle(sb, rect);
                    }
                    else if (borderType == ButtonBorderType.Ellipse)
                    {
                        g.FillEllipse(sb, rect);
                    }
                    else
                    {
                        using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, radius, roundType, false))
                        {
                            g.FillPath(sb, path);
                        }
                    }
                }
            }
        }

        public static void RenderBorder(Graphics g, Rectangle rect, Color borderColor,
            ButtonBorderType borderType, int radius, RoundStyle roundType)
        {
            rect.Width--;
            rect.Height--;
            
            bool simpleRect = (borderType == ButtonBorderType.Rectangle && (roundType == RoundStyle.None || radius < 2));
            SmoothingMode newMode = simpleRect ? SmoothingMode.HighSpeed : SmoothingMode.AntiAlias;

            using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, newMode))
            {
                using (Pen p = new Pen(borderColor))
                {
                    if (simpleRect)
                    {
                        g.DrawRectangle(p, rect);
                    }
                    else if (borderType == ButtonBorderType.Ellipse)
                    {
                        g.DrawEllipse(p, rect);
                    }
                    else
                    {
                        using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, radius, roundType, false))
                        {
                            g.DrawPath(p, path);
                        }
                    }
                }
            }
        }

        public static void RenderFocusRect(Graphics g, Rectangle rect, int inflateAmount)
        {
            RenderFocusRect(g, rect, inflateAmount, Color.Black);
        }

        public static void RenderFocusRect(Graphics g, Rectangle rect, int inflateAmount, Color color)
        {
            using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, SmoothingMode.HighSpeed))
            {
                rect.Width--;
                rect.Height--;
                rect.Inflate(-inflateAmount, -inflateAmount);
                using (Pen p = new Pen(color))
                {
                    p.DashStyle = DashStyle.Dot;
                    g.DrawRectangle(p, rect);
                }
            }
        }

        public static void RenderLinearGradientBackground(Graphics g, Rectangle rect,
            Color color1, Color color2, float angle, int radius, RoundStyle roundType)
        {
            SmoothingMode newMode;
            bool simpleRect = (roundType == RoundStyle.None || radius < 2);
            if (simpleRect)
            {
                newMode = SmoothingMode.HighSpeed;
            }
            else
            {
                newMode = SmoothingMode.AntiAlias;
                rect.Width--;
                rect.Height--;
            }
            
            if (rect.Width < 1 || rect.Height < 1)
                return;

            using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, newMode))
            {
                using (LinearGradientBrush lb = new  LinearGradientBrush(
                    rect,color1,color2,angle))
                {
                    if (simpleRect)
                    {
                        g.FillRectangle(lb, rect);
                    }                    
                    else
                    {
                        using (GraphicsPath path = GraphicsPathHelper.CreatePath(rect, radius, roundType, false))
                        {
                            g.FillPath(lb, path);
                        }
                    }
                }
            }
        }

        public static void RenderBorder(Graphics g, Rectangle rect, Color borderColor,
            ButtonBorderType borderType)
        {
            RenderBorder(g, rect, borderColor, borderType, 0, RoundStyle.None);
        }

        public static void RenderFlatBackground(Graphics g, Rectangle rect, Color backColor,
            ButtonBorderType borderType)
        {
            RenderFlatBackground(g, rect, backColor, borderType, 0, RoundStyle.None);
        }

        public static void RenderResizeGrid(Graphics g, Rectangle rect, Color color, ResizeGridLocation location)
        {
            int x, y;
            Point[] ps;

            switch (location)
            {
                case ResizeGridLocation.BottomLeft:
                    x = rect.Left + 1;
                    y = rect.Bottom - 12;
                    ps = new Point[]{
                        new Point(x+1,y+1),
                        new Point(x+1,y+5),
                        new Point(x+1,y+9),
                        new Point(x+5,y+5),
                        new Point(x+5,y+9),
                        new Point(x+9,y+9)};
                    break;

                case ResizeGridLocation.TopLeft:
                    x = rect.Left + 1;
                    y = rect.Top + 1;
                    ps = new Point[]{
                        new Point(x+1,y+1),
                        new Point(x+1,y+5),
                        new Point(x+1,y+9),
                        new Point(x+5,y+1),
                        new Point(x+5,y+5),
                        new Point(x+9,y+1)};
                    break;

                case ResizeGridLocation.TopRight:
                    x = rect.Right - 12;
                    y = rect.Top + 1;
                    ps = new Point[]{
                        new Point(x+1,y+1),
                        new Point(x+5,y+1),
                        new Point(x+5,y+5),
                        new Point(x+9,y+1),
                        new Point(x+9,y+5),
                        new Point(x+9,y+9)};
                    break;
                default:
                    x = rect.Right - 12;
                    y = rect.Bottom - 12;
                    ps = new Point[]{
                        new Point(x+9,y+1),
                        new Point(x+9,y+5),
                        new Point(x+9,y+9),
                        new Point(x+5,y+5),
                        new Point(x+5,y+9),
                        new Point(x+1,y+9)};
                    break;
            }

            using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, SmoothingMode.HighSpeed))
            {
                using (Pen p = new Pen(color))
                {
                    for (int i = 0; i < ps.Length; i++)
                    {
                        g.DrawLine(p, ps[i].X, ps[i].Y - 1, ps[i].X, ps[i].Y + 1);
                        g.DrawLine(p, ps[i].X - 1, ps[i].Y, ps[i].X + 1, ps[i].Y);
                    }
                }
            }
        }

        #endregion
    }
}
