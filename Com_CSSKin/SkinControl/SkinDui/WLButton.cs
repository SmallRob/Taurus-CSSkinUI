
using Com_CSSkin.Imaging;
using Com_CSSkin.SkinClass;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    /// <summary>
    /// WL是winless的缩写，即该Button类是无句柄的
    /// </summary>
    public class WLButton : WLButtonBase
    {
        #region private var
                
        private static readonly object EVENT_CLICK;
        private static readonly object EVENT_PAINT;

        private SkinButtonThemeBase _xtheme;

        #endregion

        #region properties

        #region xtheme

        public SkinButtonThemeBase XTheme
        {
            get
            {
                if (_xtheme == null)
                {
                    _xtheme = new SkinButtonThemeBase();
                }
                return _xtheme;
            }
        }

        #endregion

        #region setting

        /// <summary>
        /// 获取或设置是否将绘制完全限制在指定的区域内
        /// </summary>
        public bool RestrictedBounds { get; set; }

        /// <summary>
        /// 用于在click事件中传回数据
        /// </summary>
        public object ClickSendBackOject { get; set; } 

        #endregion        

        #region shape and look

        // back image
        public Image BackImageNormal { get; set; }
        public Image BackImageHover { get; set; }
        public Image BackImagePressed { get; set; }
        public Image BackImageDisabled { get; set; }

        // whole shape
        //public int Radius { get; set; }
        //public RoundStyle RoundedType { get; set; }
        public ButtonBorderType BorderType { get; set; }
        public bool PressedLeaveDrawAsPressed { get; set; }
        //public Padding InnerPadding { get; set; }

        // fore path
        public ButtonForePathGetter ForePathGetter { get; set; }        
        public Size ForePathSize { get; set; }
        public ForePathRatoteDirection RotateDirection { get; set; }
        public ForePathRenderMode HowForePathRender { get; set; }
        /// <summary>
        /// 画两次可以加深颜色
        /// </summary>
        public bool DrawForePathTwice { get; set; }

        // fore image
        public Image ForeImage { get; set; }
        public Size ForeImageSize { get; set; }
        public ButtonImageAlignment ImageAlign { get; set; }
        public int SpaceBetweenImageAndText { get; set; }

        // others
        //public ButtonColorTable ColorTable { get; set; }        
        public bool DrawLightGlass { get; set; }
        //public Font ForeFont { get; set; }        

        #endregion

        #endregion

        #region private render method

        private void RenderNormal(Graphics g)
        {            
            RenderInternal(g, BackImageNormal, XTheme.ColorTable.BorderColorNormal,
                XTheme.ColorTable.BackColorNormal, XTheme.ColorTable.ForeColorNormal);            
        }

        private void RenderHover(Graphics g)
        {
            RenderInternal(g, BackImageHover, XTheme.ColorTable.BorderColorHover,
                XTheme.ColorTable.BackColorHover, XTheme.ColorTable.ForeColorHover);
        }

        private void RenderPressed(Graphics g)
        {
            RenderInternal(g, BackImagePressed, XTheme.ColorTable.BorderColorPressed,
                XTheme.ColorTable.BackColorPressed, XTheme.ColorTable.ForeColorPressed);            
        }

        private void RenderDisabled(Graphics g)
        {
            RenderInternal(g, BackImageDisabled, XTheme.ColorTable.BorderColorDisabled,
                XTheme.ColorTable.BackColorDisabled, XTheme.ColorTable.ForeColorDisabled);
        }

        private void RenderInternal(Graphics g, Image backImage, Color borderColor,
            Color backColor, Color foreColor)
        {
            Region oldClip = g.Clip;
            Region newClip = null;

            if (RestrictedBounds)
            {
                newClip = new Region(Bounds);
                g.Clip = newClip;
            }

            if (backImage != null)
            {
                g.DrawImage(backImage, Bounds);
            }
            else
            {
                FillInBackground(g, backColor);                
                RenderForePathAndText(g, foreColor);
                DrawBorder(g, borderColor);
            }

            if (RestrictedBounds)
            {
                g.Clip = oldClip;                
                newClip.Dispose();
            }
        }

        private void FillInBackground(Graphics g, Color backColor)
        {
            BasicBlockPainter.RenderFlatBackground(
                g,
                Bounds,
                backColor,
                BorderType,
                XTheme.RoundedRadius,
                XTheme.RoundedStyle);
            
            if (BorderType == ButtonBorderType.Ellipse && DrawLightGlass)
            {                
                GlassPainter.RenderEllipseGlass(g, Bounds);
            }            
        }

        private void DrawBorder(Graphics g, Color borderColor)
        {
            //if (borderColor == Color.Empty || borderColor == Color.Transparent)
            //    return;

            BasicBlockPainter.RenderBorder(
                g,
                Bounds,
                borderColor,
                BorderType,
                XTheme.RoundedRadius,
                XTheme.RoundedStyle);            
        }

        private void RenderForePathAndText(Graphics g, Color foreColor)
        {
            if (ForePathGetter != null || ForeImage != null)
            {
                if(string.IsNullOrEmpty(Text))
                    PathOnly(g,foreColor);
                else
                    PathAndText(g,foreColor);
            }
            else if(!string.IsNullOrEmpty(Text))
                TextOnly(g,foreColor);
        }

        private void CalculatePathAndTextRect(out Rectangle pathRect, out Rectangle textRect)
        {
            Size pathSize = (ForePathGetter != null) ? ForePathSize : ForeImageSize;
            Size textSize = TextRenderer.MeasureText(Text, XTheme.TextFont);
            Rectangle rect = Bounds;
            pathRect = textRect = Rectangle.Empty;

            int x, y;
            switch (ImageAlign)
            {
                case ButtonImageAlignment.Left:
                    x = rect.X + (rect.Width - (pathSize.Width + SpaceBetweenImageAndText + textSize.Width)) / 2;
                    pathRect = new Rectangle(new Point(x, rect.Y + (rect.Height - pathSize.Height) / 2), pathSize);
                    textRect = new Rectangle(new Point(pathRect.Right + SpaceBetweenImageAndText,
                        rect.Y + (rect.Height - textSize.Height) / 2), textSize);
                    break;
                case ButtonImageAlignment.Right:
                    x = rect.X + (rect.Width - (pathSize.Width + SpaceBetweenImageAndText + textSize.Width)) / 2;
                    textRect = new Rectangle(new Point(x, rect.Y + (rect.Height - textSize.Height) / 2), textSize);
                    pathRect = new Rectangle(new Point(textRect.Right + SpaceBetweenImageAndText,
                        rect.Y + (rect.Height - pathSize.Height) / 2), pathSize);
                    break;
                case ButtonImageAlignment.Top:
                    y = rect.Y + (rect.Height - (pathSize.Height + SpaceBetweenImageAndText + textSize.Height)) / 2;
                    pathRect = new Rectangle(new Point(rect.X + (rect.Width - pathSize.Width) / 2, y), pathSize);
                    textRect = new Rectangle(new Point(rect.X + (rect.Width - textSize.Width) / 2,
                        pathRect.Bottom + SpaceBetweenImageAndText), textSize);
                    break;
                case ButtonImageAlignment.Bottom:
                    y = rect.Y + (rect.Height - (pathSize.Height + SpaceBetweenImageAndText + textSize.Height)) / 2;
                    textRect = new Rectangle(new Point(rect.X + (rect.Width - textSize.Width) / 2, y), textSize);
                    pathRect = new Rectangle(new Point(rect.X + (rect.Width - pathSize.Width) / 2,
                        textRect.Bottom + SpaceBetweenImageAndText), pathSize);
                    break;
            }
        }

        private void PathOnly(Graphics g, Color foreColor)
        {
            if (ForePathGetter != null)
            {
                using (GraphicsPath path = ForePathGetter(Bounds))
                {
                    PathWithRotate(g, path, foreColor, Bounds);
                    if (DrawForePathTwice)
                        PathWithRotate(g, path, foreColor, Bounds);
                }
            }
            else if (ForeImage != null)
            {
                int x = Bounds.Left + (Bounds.Width - ForeImageSize.Width) / 2;
                int y = Bounds.Top + (Bounds.Height - ForeImageSize.Height) / 2;
                Rectangle rectImage = new Rectangle(new Point(x, y), ForeImageSize);
                g.DrawImage(ForeImage, rectImage);
            }
        }

        private void TextOnly(Graphics g, Color foreColor)
        {
            Size size = TextRenderer.MeasureText(Text, XTheme.TextFont);
            int x = Bounds.Left + (Bounds.Width - size.Width) / 2;
            int y = Bounds.Top + (Bounds.Height - size.Height) / 2;
            Rectangle textRect = new Rectangle(new Point(x, y), size);
            TextRenderer.DrawText(
                g,
                Text,
                XTheme.TextFont,
                textRect,
                foreColor,
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter);
        }

        private void PathAndText(Graphics g, Color foreColor)
        {            
            Rectangle pathRect, textRect;
            CalculatePathAndTextRect(out pathRect, out textRect);

            if (ForePathGetter != null)
            {
                using (GraphicsPath path = ForePathGetter(pathRect))
                {
                    PathWithRotate(g, path, foreColor, pathRect);
                }
            }
            else if (ForeImage != null)
            {
                g.DrawImage(ForeImage, pathRect);
            }

            TextRenderer.DrawText(
                g,
                Text,
                XTheme.TextFont,
                textRect,
                foreColor,
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.VerticalCenter);
        }

        private void PathWithRotate(Graphics g, GraphicsPath path, Color foreColor, Rectangle pathRect)
        {
            Pen p = new Pen(foreColor);
            SolidBrush sb = new SolidBrush(foreColor);

            if (RotateDirection == ForePathRatoteDirection.Down)
            {
                if (HowForePathRender == ForePathRenderMode.Draw)
                    g.DrawPath(p, path);
                else
                    g.FillPath(sb, path);
            }
            else
            {
                int dx = 0, dy = 0, angle = 0;
                switch (RotateDirection)
                {
                    case ForePathRatoteDirection.Left:
                        dx = ForePathSize.Width - 1;
                        dy = 0;
                        angle = 90;
                        break;
                    case ForePathRatoteDirection.Up:
                        dx = ForePathSize.Width - 1;
                        dy = ForePathSize.Height - 1;
                        angle = 180;
                        break;
                    case ForePathRatoteDirection.Right:
                        dx = 0;
                        dy = ForePathSize.Height - 1;
                        angle = 270;
                        break;
                }
                int pathX = pathRect.Left + (pathRect.Width - ForePathSize.Width) / 2;
                int pathY = pathRect.Top + (pathRect.Height - ForePathSize.Height) / 2;
                g.TranslateTransform(pathX, pathY);
                g.TranslateTransform(dx, dy);
                g.RotateTransform(angle);
                using (GraphicsPath newPath = ForePathGetter(new Rectangle(Point.Empty, ForePathSize)))
                {
                    if (HowForePathRender == ForePathRenderMode.Draw)
                        g.DrawPath(p, newPath);
                    else
                        g.FillPath(sb, newPath);
                }
                g.ResetTransform();
            }
            p.Dispose();
            sb.Dispose();
        }

        #endregion

        #region override paint operation

        protected override void OnPaintContent(Graphics g, Rectangle clipRect)
        {
            base.OnPaintContent(g, clipRect);
            if (Enabled)
            {
                switch (State)
                {
                    case SkinButtonState.Hover:
                        RenderHover(g);
                        break;
                    case SkinButtonState.Pressed:
                        RenderPressed(g);
                        break;
                    case SkinButtonState.PressLeave:
                        if(PressedLeaveDrawAsPressed)
                            RenderPressed(g);
                        else
                            RenderNormal(g);
                        break;
                    default:
                        RenderNormal(g);
                        break;
                }
            }
            else
            {
                RenderDisabled(g);
            }
            OnPaint(new PaintEventArgs(g, Bounds));
        }

        #endregion

        #region event

        public event EventHandler Click
        {
            add { base.Events.AddHandler(EVENT_CLICK, value); }
            remove { base.Events.RemoveHandler(EVENT_CLICK, value); }
        }

        public event PaintEventHandler Paint
        {
            add { base.Events.AddHandler(EVENT_PAINT, value); }
            remove { base.Events.RemoveHandler(EVENT_PAINT, value); }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            EventHandler handle = (EventHandler)base.Events[EVENT_CLICK];
            if (handle != null)
            {
                object obj = (ClickSendBackOject != null) ? ClickSendBackOject : this;
                handle(obj, e);
            }
        }

        protected virtual void OnPaint(PaintEventArgs e)
        {
            PaintEventHandler handler = (PaintEventHandler)base.Events[EVENT_PAINT];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region public method

        public void SetNewTheme(SkinButtonThemeBase xtheme)
        {
            if (xtheme == null)
                throw new ArgumentNullException("xtheme");
            _xtheme = xtheme;
            base.Invalidate();
        }

        #endregion

        #region constructors

        static WLButton()
        {
            EVENT_CLICK = new object();
            EVENT_PAINT = new object();
        }

        public WLButton(Control owner)
            : base(owner)
        {            
            BorderType = ButtonBorderType.Rectangle;
            DrawLightGlass = false;
            RotateDirection = ForePathRatoteDirection.Down;
            HowForePathRender = ForePathRenderMode.Draw;
            RestrictedBounds = true;
            //Radius = 0;
            //RoundedType = RoundStyle.None;
            SpaceBetweenImageAndText = 4;
            ImageAlign = ButtonImageAlignment.Left;
            PressedLeaveDrawAsPressed = false;
        }

        #endregion
    }
}
