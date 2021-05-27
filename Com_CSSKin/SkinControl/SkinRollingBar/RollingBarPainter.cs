
using Com_CSSkin.SkinClass;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Com_CSSkin.SkinControl
{
    public class RollingBarPainter
    {
        public static void RenderDefault(Graphics g, Rectangle rect, Color backColor, float startAngle,
            int radius1, int radius2, int spokeNum, float penWidth, Color[] colorArray)
        {

            if (spokeNum < 1)
                throw new ArgumentException("spokeNum must bigger than 0", "spokeNum");
            if (spokeNum > colorArray.Length)
                throw new ArgumentException("spokeNum must NOT bigger than the length of colorArray. ", "spokeNum");
            using (SolidBrush sb = new SolidBrush(backColor))
            {
                using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, SmoothingMode.HighSpeed))
                {
                    g.FillRectangle(sb, rect);
                }
            }
            Point centerPoint = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            PointF p1, p2;
            p1 = new PointF(0f, 0f);
            p2 = p1;
            double sweepAngle = 2 * Math.PI / spokeNum;
            startAngle = (float)(startAngle * 2 * Math.PI / 360f);
            using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, SmoothingMode.AntiAlias))
            {
                using (Pen p = new Pen(Color.White, penWidth))
                {
                    p.StartCap = LineCap.Round;
                    p.EndCap = LineCap.Round;
                    for (int i = 0; i < spokeNum; i++)
                    {
                        double angle = startAngle + sweepAngle * i;
                        p1.X = centerPoint.X + (float)(radius1 / 2 * Math.Cos(angle));
                        p1.Y = centerPoint.Y + (float)(radius1 / 2 * Math.Sin(angle));
                        p2.X = centerPoint.X + (float)(radius2 / 2 * Math.Cos(angle));
                        p2.Y = centerPoint.Y + (float)(radius2 / 2 * Math.Sin(angle));
                        p.Color = colorArray[i];
                        g.DrawLine(p, p1, p2);
                    }
                }
            }
        }

        public static void RenderChromeOneQuarter(Graphics g, Rectangle rect, Color backColor,
            float startAngle, int radius, Color baseColor)
        {
            using (SolidBrush sb = new SolidBrush(backColor))
            {
                using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, SmoothingMode.HighSpeed))
                {
                    g.FillRectangle(sb, rect);
                }
            }
            Rectangle rectInner = new Rectangle(
                rect.X + (rect.Width - radius) / 2, rect.Y + (rect.Height - radius) / 2, radius, radius);
            using (Pen p = new Pen(baseColor, 3))
            {
                p.StartCap = LineCap.Round;
                p.EndCap = LineCap.Round;
                using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, SmoothingMode.AntiAlias))
                {
                    g.DrawArc(p, rectInner, startAngle, 120);
                }
            }
        }

        public static void RenderDiamondRing(Graphics g, Rectangle rect, Color backColor, float startAngle,
            int radius, Color baseColor, Color diamondColor)
        {
            using (SolidBrush sb = new SolidBrush(backColor))
            {
                using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, SmoothingMode.HighSpeed))
                {
                    g.FillRectangle(sb, rect);
                }
            }
            Point centerPoint = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            startAngle = (float)(startAngle * 2 * Math.PI / 360f);
            PointF pf = PointF.Empty;
            pf.X = centerPoint.X + (float)(Math.Cos(startAngle) * radius / 2);
            pf.Y = centerPoint.Y + (float)(Math.Sin(startAngle) * radius / 2);

            Rectangle rectCircle = new Rectangle(
                rect.X + (rect.Width - radius) / 2, rect.Y + (rect.Height - radius) / 2, radius, radius);
            float width = 4f;
            RectangleF rectDiamond = new RectangleF(pf.X - width / 2, pf.Y - width / 2, width, width);
            using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, SmoothingMode.AntiAlias))
            {
                using (Pen p = new Pen(baseColor))
                {
                    g.DrawEllipse(p, rectCircle);                    
                }
                using (SolidBrush sb = new SolidBrush(diamondColor))
                {
                    g.FillEllipse(sb, rectDiamond);
                }
            }
        }

        public static void RenderTheseGuys(Graphics g, Rectangle rect, Color backColor, float startAngle,
            int radius, Color baseColor)
        {
            using (SolidBrush sb = new SolidBrush(backColor))
            {
                using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, SmoothingMode.HighSpeed))
                {
                    g.FillRectangle(sb, rect);
                }
            }
            Point centerPoint = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            startAngle = (float)(startAngle * 2 * Math.PI / 360f);
            PointF pf = PointF.Empty;            
            double sweepAngle = 2 * Math.PI / 10;            
            RectangleF rectGuy = RectangleF.Empty;
            float[] widthArray = new float[] { 5f, 4f, 3f, 2f, 2f };
            Color[] colorArray = ColorHelper.GetLighterArrayColors(baseColor, 5, 50f);
            using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, SmoothingMode.AntiAlias))
            {
                using (SolidBrush sb = new SolidBrush(baseColor))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        pf.X = centerPoint.X + (float)(Math.Cos(startAngle - i * sweepAngle) * radius / 2);
                        pf.Y = centerPoint.Y + (float)(Math.Sin(startAngle - i * sweepAngle) * radius / 2);
                        rectGuy = new RectangleF(
                            pf.X - widthArray[i] / 2, pf.Y - widthArray[i] / 2, widthArray[i], widthArray[i]);
                        sb.Color = colorArray[4 - i];
                        g.FillEllipse(sb, rectGuy);
                    }
                }
            }
        }
    }
}
