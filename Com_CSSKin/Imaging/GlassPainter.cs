
using Com_CSSkin.SkinClass;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Com_CSSkin.Imaging
{
    public class GlassPainter
    {
        #region rectangle glass

        private static Rectangle CalcGlassRect(Rectangle ownerRect, GlassPosition pos, float factor)
        {
            Point location = ownerRect.Location;
            int width = ownerRect.Width;
            int height = ownerRect.Height;

            switch (pos)
            {
                case GlassPosition.Fill:
                    break;
                case GlassPosition.Top:
                    height = (int)(ownerRect.Height * factor);
                    break;
                case GlassPosition.Bottom:
                    height = (int)(ownerRect.Height * factor);
                    location.Offset(0, ownerRect.Height - height);
                    break;
                case GlassPosition.Left:
                    width = (int)(ownerRect.Width * factor);
                    break;
                case GlassPosition.Right:
                    width = (int)(ownerRect.Width * factor);
                    location.Offset(ownerRect.Width - width, 0);
                    break;
            }
            return new Rectangle(location, new Size(width, height));
        }

        private static RoundStyle CalcRoundStyle(GlassPosition pos, int radius, RoundStyle ownerStyle)
        {
            if (radius < 2 || ownerStyle == RoundStyle.None)
                return RoundStyle.None;
            switch (pos)
            {
                case GlassPosition.Fill:
                    return ownerStyle;
                case GlassPosition.Top:
                    if (ownerStyle == RoundStyle.All || ownerStyle == RoundStyle.Top)
                        return RoundStyle.Top;
                    else
                        return RoundStyle.None;
                case GlassPosition.Bottom:
                    if (ownerStyle == RoundStyle.All || ownerStyle == RoundStyle.Bottom)
                        return RoundStyle.Bottom;
                    else
                        return RoundStyle.None;
                case GlassPosition.Left:
                    if (ownerStyle == RoundStyle.All || ownerStyle == RoundStyle.Left)
                        return RoundStyle.Left;
                    else
                        return RoundStyle.None;
                case GlassPosition.Right:
                    if (ownerStyle == RoundStyle.All || ownerStyle == RoundStyle.Right)
                        return RoundStyle.Right;
                    else
                        return RoundStyle.None;
                default:
                    return RoundStyle.None;
            }
        }

        public static void RenderRectangleGlass(Graphics g, Rectangle ownerRect,
            int ownerRadius, RoundStyle ownerRoundTye, GlassPosition position,
            float angle, float glassLengthFactor, Color glassColor, int alpha1, int alpha2)
        {
            if (!(glassLengthFactor > 0 && glassLengthFactor < 1))
                throw new ArgumentException("glassLengthFactor must be between 0 and 1, but not include 0 and 1. ",
                    "glassLengthFactor");

            Rectangle rect = CalcGlassRect(ownerRect, position, glassLengthFactor);
            RoundStyle round = CalcRoundStyle(position, ownerRadius, ownerRoundTye);            
            if (rect.Width < 1 || rect.Height < 1)
                return;

            BasicBlockPainter.RenderLinearGradientBackground(
                g,
                rect,
                Color.FromArgb(alpha1, glassColor),
                Color.FromArgb(alpha2, glassColor),
                angle,
                ownerRadius,
                round);            
        }
        
        public static void RenderRectangleGlass(Graphics g, Rectangle ownerRect, int ownerRadius, RoundStyle ownerRoundTye)
        {
            RenderRectangleGlass(g, ownerRect, ownerRadius, ownerRoundTye, GlassPosition.Top, 90f);
        }

        public static void RenderRectangleGlass(Graphics g, Rectangle ownerRect, int ownerRadius, RoundStyle ownerRoundTye,
            GlassPosition position, float angle)
        {
            RenderRectangleGlass(g, ownerRect, ownerRadius, ownerRoundTye, position, angle, 0.5f);
        }

        public static void RenderRectangleGlass(Graphics g, Rectangle ownerRect, int ownerRadius, RoundStyle ownerRoundTye,
            GlassPosition position, float angle, float glassLengthFactor)
        {
            RenderRectangleGlass(g, ownerRect, ownerRadius, ownerRoundTye,
                position, angle, glassLengthFactor, Color.White, 220, 60);
        }

        #endregion

        #region ellipse glass

        private static PointF GetEllipseGlassCenterPoint(Rectangle rect, GlassPosition position, float factor)
        {
            PointF pf = new PointF(rect.X + rect.Width*0.5f,rect.Y+rect.Height*0.5f);

            switch (position)
            {
                case GlassPosition.Top:
                    pf.Y = rect.Y + rect.Height * factor;
                    break;
                case GlassPosition.Left:
                    pf.X = rect.X + rect.Width * factor;
                    break;
                case GlassPosition.Right:
                    pf.X = rect.Right - rect.Width * factor;
                    break;
                case GlassPosition.Bottom:
                    pf.Y = rect.Bottom - rect.Height * factor;
                    break;
            }

            return pf;
        }

        public static void RenderEllipseGlass(Graphics g, Rectangle ownerRect, GlassPosition position,
            float positionFactor, Color glassColor, int alphaCenter, int alphaSurround)
        {
            if (!(positionFactor > 0 && positionFactor < 1))
                throw new ArgumentException("positionFactor must be between 0 and 1, but not include 0 and 1. ",
                    "positionFactor");

            ownerRect.Height--;
            ownerRect.Width--;

            if (ownerRect.Width < 1 || ownerRect.Height < 1)
                return;

            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddEllipse(ownerRect);
                using (PathGradientBrush pb = new PathGradientBrush(gp))
                {
                    pb.CenterPoint = GetEllipseGlassCenterPoint(ownerRect, position, positionFactor);
                    pb.CenterColor = Color.FromArgb(alphaCenter, glassColor);
                    pb.SurroundColors = new Color[] { Color.FromArgb(alphaSurround, glassColor) };
                    using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, SmoothingMode.AntiAlias))
                    {
                        g.FillPath(pb, gp);
                    }
                }
            }
        }

        public static void RenderEllipseGlass(Graphics g, Rectangle rect, GlassPosition position, float positionFactor)
        {
            RenderEllipseGlass(g, rect, position, positionFactor, Color.White, 180, 40);
        }

        public static void RenderEllipseGlass(Graphics g, Rectangle rect)
        {
            RenderEllipseGlass(g, rect, GlassPosition.Top, 0.25f);
        }

        #endregion
    }
}
