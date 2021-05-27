
using System;
using System.ComponentModel;
using System.Windows.Forms;
using Com_CSSkin.Win32;
using Com_CSSkin.Win32.Struct;
using Com_CSSkin.Win32.Const;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(Panel))]
    public class SkinCaptionPanel : PanelBase
    {
        #region Fileds

        private int _captionHeight = 24;
        private CaptionStyle _captionStyle = CaptionStyle.Top;
        private bool _showBorder;
        private Image _image;
        private ContentAlignment _textAlign = ContentAlignment.MiddleLeft;
        private Font _captionFont = SystemFonts.CaptionFont;
        private ControlState _captionState;

        private static readonly object EventCaptionMouseMove = new object();
        private static readonly object EventCaptionMouseEnter = new object();
        private static readonly object EventCaptionMouseLeave = new object();
        private static readonly object EventCaptionMouseDown = new object();
        private static readonly object EventCaptionMouseUp = new object();
        private static readonly object EventCaptionMouseClick = new object();

        #endregion

        #region Constructors

        public SkinCaptionPanel()
            : base()
        {
        }

        #endregion

        #region Events

        public event MouseEventHandler CaptionMouseMove
        {
            add { base.Events.AddHandler(EventCaptionMouseMove, value); }
            remove { base.Events.RemoveHandler(EventCaptionMouseMove, value); }
        }

        public event EventHandler CaptionMouseEnter
        {
            add { base.Events.AddHandler(EventCaptionMouseEnter, value); }
            remove { base.Events.RemoveHandler(EventCaptionMouseEnter, value); }
        }

        public event EventHandler CaptionMouseLeave
        {
            add { base.Events.AddHandler(EventCaptionMouseLeave, value); }
            remove { base.Events.RemoveHandler(EventCaptionMouseLeave, value); }
        }

        public event MouseEventHandler CaptionMouseDown
        {
            add { base.Events.AddHandler(EventCaptionMouseDown, value); }
            remove { base.Events.RemoveHandler(EventCaptionMouseDown, value); }
        }

        public event MouseEventHandler CaptionMouseUp
        {
            add { base.Events.AddHandler(EventCaptionMouseUp, value); }
            remove { base.Events.RemoveHandler(EventCaptionMouseUp, value); }
        }

        public event MouseEventHandler CaptionMouseClick
        {
            add { base.Events.AddHandler(EventCaptionMouseClick, value); }
            remove { base.Events.RemoveHandler(EventCaptionMouseClick, value); }
        }

        #endregion

        #region Properties

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get { return base.Text; }
            set
            {
                if (base.Text != value)
                {
                    base.Text = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Image), "")]
        public virtual Image Image
        {
            get { return _image; }
            set
            {
                _image = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(ContentAlignment), "16")]
        public virtual ContentAlignment TextAlign
        {
            get { return _textAlign; }
            set
            {
                if (_textAlign != value)
                {
                    _textAlign = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(24)]
        public virtual int CaptionHeight
        {
            get { return _captionHeight; }
            set
            {
                if (_captionHeight != value)
                {
                    _captionHeight = value < 8 ? 8 : value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(CaptionStyle), "0")]
        public virtual CaptionStyle CaptionStyle
        {
            get { return _captionStyle; }
            set
            {
                if (_captionStyle != value)
                {
                    _captionStyle = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Font), "CaptionFont")]
        public Font CaptionFont
        {
            get { return _captionFont; }
            set
            {
                _captionFont = value;
                base.Invalidate();
            }
        }

        [DefaultValue(false)]
        public virtual bool ShowBorder
        {
            get { return _showBorder; }
            set
            {
                if (_showBorder != value)
                {
                    _showBorder = value;
                    base.Invalidate();
                }
            }
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle rect = base.DisplayRectangle;
                int borderWidth = base.BorderWidth;
                switch (_captionStyle)
                {
                    case CaptionStyle.Top:
                        rect.Y += _captionHeight;
                        rect.Height -= (_captionHeight + borderWidth);
                        rect.X += borderWidth;
                        rect.Width -= borderWidth * 2;
                        break;
                    case CaptionStyle.Bottom:
                        rect.Y += borderWidth;
                        rect.Height -= (_captionHeight + borderWidth);
                        rect.X += borderWidth;
                        rect.Width -= borderWidth * 2;
                        break;
                    case CaptionStyle.Left:
                        rect.X += _captionHeight;
                        rect.Width -= (_captionHeight + borderWidth);
                        rect.Y += borderWidth;
                        rect.Height -= borderWidth * 2;
                        break;
                    case CaptionStyle.Right:
                        rect.X += borderWidth;
                        rect.Width -= (_captionHeight + borderWidth);
                        rect.Y += borderWidth;
                        rect.Height -= borderWidth * 2;
                        break;
                }
                return rect;
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool AutoScroll
        {
            get { return base.AutoScroll; }
            set { base.AutoScroll = false; }
        }

        internal protected Rectangle CaptionRect
        {
            get
            {
                Rectangle captionRect = base.ClientRectangle;

                switch (_captionStyle)
                {
                    case CaptionStyle.Top:
                        captionRect.Height = _captionHeight;
                        break;
                    case CaptionStyle.Left:
                        captionRect.Width = _captionHeight;
                        break;
                    case CaptionStyle.Bottom:
                        captionRect.Y = captionRect.Bottom - _captionHeight;
                        captionRect.Height = _captionHeight;
                        break;
                    case CaptionStyle.Right:
                        captionRect.X = captionRect.Right - _captionHeight;
                        captionRect.Width = _captionHeight;
                        break;
                }

                return captionRect;
            }
        }

        protected virtual ControlState CaptionState
        {
            get { return _captionState; }
            set { _captionState = value; }
        }

        #endregion

        #region Protected Methods

        protected virtual void OnCaptionMouseMove(MouseEventArgs e)
        {
            MouseEventHandler handler = 
                base.Events[EventCaptionMouseMove] as MouseEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }

        }

        protected virtual void OnCaptionMouseDown(MouseEventArgs e)
        {
            MouseEventHandler handler =
               base.Events[EventCaptionMouseDown] as MouseEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }

        }

        protected virtual void OnCaptionMouseUp(MouseEventArgs e)
        {
            MouseEventHandler handler =
               base.Events[EventCaptionMouseUp] as MouseEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }

            //System.Diagnostics.Trace.WriteLine("Up: " + CaptionState.ToString());
        }

        protected virtual void OnCaptionMouseClick(MouseEventArgs e)
        {
            MouseEventHandler handler =
               base.Events[EventCaptionMouseClick] as MouseEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnCaptionMouseEnter(EventArgs e)
        {
            EventHandler handler =
               base.Events[EventCaptionMouseEnter] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnCaptionMouseLeave(EventArgs e)
        {
            EventHandler handler =
               base.Events[EventCaptionMouseLeave] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (CaptionRect.Contains(e.Location))
            {
                if (CaptionState == ControlState.Normal)
                {
                    CaptionState = ControlState.Hover;
                    OnCaptionMouseEnter(e);
                }

                OnCaptionMouseMove(e);
            }
            else
            {
                if (CaptionState == ControlState.Hover)
                {
                    CaptionState = ControlState.Normal;
                    OnCaptionMouseLeave(e);
                }
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            if (CaptionRect.Contains(e.Location))
            {
                CaptionState = ControlState.Pressed;
                OnCaptionMouseDown(e);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);

            if (CaptionState == ControlState.Pressed &&
                CaptionRect.Contains(e.Location))
            {
                CaptionState = ControlState.Hover;
                OnCaptionMouseUp(e);
                OnCaptionMouseClick(e);
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            if (CaptionState == ControlState.Hover)
            {
                CaptionState = ControlState.Normal;
                OnCaptionMouseLeave(e);
            }
        }

        protected override void OnMouseCaptureChanged(EventArgs e)
        {
            base.OnMouseCaptureChanged(e);
            if (CaptionState == ControlState.Pressed && 
                !CaptionRect.Contains(base.PointToClient(Control.MousePosition)))
            {
                CaptionState = ControlState.Normal;
                OnCaptionMouseLeave(e);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            OnPaintCaption(e);
        }

        protected virtual void OnPaintCaption(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle captionRect = CaptionRect;
            LinearGradientMode gradientMode = LinearGradientMode.Vertical;
            RoundStyle roundStyle = RoundStyle.None;

            switch (_captionStyle)
            {
                case CaptionStyle.Top:
                    roundStyle = RoundStyle & RoundStyle.Top;
                    break;
                case CaptionStyle.Left:
                    gradientMode = LinearGradientMode.Horizontal;
                    roundStyle = RoundStyle & RoundStyle.Left;
                    break;
                case CaptionStyle.Bottom:
                    roundStyle = RoundStyle & RoundStyle.Bottom;
                    break;
                case CaptionStyle.Right:
                    gradientMode = LinearGradientMode.Horizontal;
                    roundStyle = RoundStyle & RoundStyle.Right;
                    break;
            }

            if (!_showBorder)
            {
                roundStyle = RoundStyle.All;
            }

            using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
            {
                RenderHelper.RenderBackgroundInternal(
                    g,
                    captionRect,
                    ColorTable.CaptionBackNormal,
                    ColorTable.Border,
                    Color.White,
                    roundStyle,
                    Radius,
                    true,
                    true,
                    gradientMode);
            }

            RenderImageAndText(g, captionRect);

            RenderBorder(g, base.ClientRectangle);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _image = null;
                if (_captionFont != null)
                {
                    _captionFont.Dispose();
                    _captionFont = null;
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Render Methods

        internal void RenderBorder(Graphics g, Rectangle bounds)
        {
            switch (_captionStyle)
            {
                case CaptionStyle.Top:
                case CaptionStyle.Bottom:
                    if (base.Height <= _captionHeight)
                    {
                        return;
                    }
                    break;
                case CaptionStyle.Left:
                case CaptionStyle.Right:
                    if (base.Width <= _captionHeight)
                    {
                        return;
                    }
                    break;
            }

            if (_showBorder)
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

        private void RenderImageAndText(Graphics g, Rectangle captionRect)
        {
            Rectangle imageRect = Rectangle.Empty;
            Rectangle textRect = Rectangle.Empty;
            int bordWidth = base.BorderWidth;
            int imageWidth = _captionHeight - 6;

            StringFormat sf = new StringFormat();
            sf.FormatFlags = StringFormatFlags.NoWrap;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            bool rightToLeft = base.RightToLeft == RightToLeft.Yes;

            if (rightToLeft)
            {
                sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            }

            switch (_captionStyle)
            {
                case CaptionStyle.Top:
                case CaptionStyle.Bottom:
                    if (_image != null)
                    {
                        imageRect = new Rectangle(
                            bordWidth, captionRect.Y + 3, imageWidth, imageWidth);
                    }
                    else
                    {
                        imageRect.X = bordWidth - 3;
                    }

                    textRect = new Rectangle(
                        imageRect.Right + 3,
                        captionRect.Y,
                        captionRect.Width - (imageRect.Right + 3) - bordWidth,
                        captionRect.Height);

                    if (rightToLeft)
                    {
                        imageRect.X = captionRect.Right - imageRect.Right;
                        textRect.X = captionRect.Right - textRect.Right;
                    }

                    sf.LineAlignment = StringAlignment.Center;

                    switch (_textAlign)
                    {
                        case ContentAlignment.BottomCenter:
                        case ContentAlignment.MiddleCenter:
                        case ContentAlignment.TopCenter:
                            sf.Alignment = StringAlignment.Center;
                            break;
                        case ContentAlignment.BottomLeft:
                        case ContentAlignment.MiddleLeft:
                        case ContentAlignment.TopLeft:
                            sf.Alignment = StringAlignment.Near;
                            break;
                        case ContentAlignment.BottomRight:
                        case ContentAlignment.MiddleRight:
                        case ContentAlignment.TopRight:
                            sf.Alignment = StringAlignment.Far;
                            break;
                    }

                    if (!string.IsNullOrEmpty(base.Text))
                    {
                        using(Brush brush = new SolidBrush(ColorTable.CaptionFore))
                        {
                            g.DrawString(
                                base.Text,
                                _captionFont,
                                brush,
                                textRect,
                                sf);
                        }
                    }
                    break;
                case CaptionStyle.Left:
                    if (_image != null)
                    {
                        imageRect = new Rectangle(
                            captionRect.X + 3, captionRect.Bottom - bordWidth - imageWidth,
                            imageWidth, imageWidth);
                    }
                    else
                    {
                        imageRect.Y = captionRect.Bottom - (bordWidth - 3);
                    }

                    textRect = new Rectangle(
                        captionRect.X,
                        captionRect.Y + bordWidth,
                        captionRect.Width,
                        imageRect.Y - 3 - bordWidth);

                    if (rightToLeft)
                    {
                        imageRect.Y = captionRect.Bottom - imageRect.Bottom;
                        textRect.Y = captionRect.Bottom - textRect.Bottom;
                    }

                    sf.LineAlignment = StringAlignment.Center;

                    switch (_textAlign)
                    {
                        case ContentAlignment.BottomCenter:
                        case ContentAlignment.MiddleCenter:
                        case ContentAlignment.TopCenter:
                            sf.Alignment = StringAlignment.Center;
                            break;
                        case ContentAlignment.BottomLeft:
                        case ContentAlignment.MiddleLeft:
                        case ContentAlignment.TopLeft:
                            sf.Alignment = StringAlignment.Near;
                            break;
                        case ContentAlignment.BottomRight:
                        case ContentAlignment.MiddleRight:
                        case ContentAlignment.TopRight:
                            sf.Alignment = StringAlignment.Far;
                            break;
                    }
                    RenderHelper.RenderVerticalText(
                        g,
                        base.Text,
                        textRect,
                        _captionFont,
                        ColorTable.CaptionFore,
                        sf,
                        true);
                    break;
                case CaptionStyle.Right:
                    if (_image != null)
                    {
                        imageRect = new Rectangle(
                            captionRect.X + 3, bordWidth,
                            imageWidth, imageWidth);
                    }
                    else
                    {
                        imageRect.Y = bordWidth - 3;
                    }

                    textRect = new Rectangle(
                        captionRect.X,
                        imageRect.Bottom + 3,
                        captionRect.Width,
                        captionRect.Height - (imageRect.Bottom + 3) - bordWidth);

                    if (rightToLeft)
                    {
                        imageRect.Y = captionRect.Bottom - imageRect.Bottom;
                        textRect.Y = captionRect.Bottom - textRect.Bottom;
                    }

                    sf.LineAlignment = StringAlignment.Center;

                    switch (_textAlign)
                    {
                        case ContentAlignment.BottomCenter:
                        case ContentAlignment.MiddleCenter:
                        case ContentAlignment.TopCenter:
                            sf.Alignment = StringAlignment.Center;
                            break;
                        case ContentAlignment.BottomLeft:
                        case ContentAlignment.MiddleLeft:
                        case ContentAlignment.TopLeft:
                            sf.Alignment = StringAlignment.Near;
                            break;
                        case ContentAlignment.BottomRight:
                        case ContentAlignment.MiddleRight:
                        case ContentAlignment.TopRight:
                            sf.Alignment = StringAlignment.Far;
                            break;
                    }
                    RenderHelper.RenderVerticalText(
                       g,
                       base.Text,
                       textRect,
                       _captionFont,
                       ColorTable.CaptionFore,
                       sf,
                       false);
                    break;
            }

            if (_image != null)
            {
                using (InterpolationModeGraphics ig = new InterpolationModeGraphics(g))
                {
                    g.DrawImage(
                        _image,
                        imageRect,
                        0,
                        0,
                        _image.Width,
                        _image.Height,
                        GraphicsUnit.Pixel);
                }
            }
        }

        #endregion
    }
}
