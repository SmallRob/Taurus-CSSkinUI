
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Com_CSSkin.SkinControl.Design;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    [ToolboxItem(false)]
    [DesignTimeVisible(false)]
    [Designer(typeof(PushPanelItemDesigner))]
    [DefaultProperty("Text"), DefaultEvent("CaptionClick")]
    public class PushPanelItem : SkinCaptionPanel
    {
        #region Fields

        private SkinPushPanel _owner;
        private bool _isExpanded;

        #endregion

        #region Constructors

        public PushPanelItem()
            : base()
        {
            base.SetStyle(ControlStyles.Selectable, true);
        }

        #endregion

        #region Properties

        [Browsable(false)]
        public bool IsExpanded
        {
            get { return _isExpanded; }
            internal set { _isExpanded = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Image Image
        {
            get { return base.Image; }
            set { base.Image = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int CaptionHeight
        {
            get { return base.CaptionHeight; }
            set { base.CaptionHeight = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override CaptionStyle CaptionStyle
        {
            get { return base.CaptionStyle; }
            set { base.CaptionStyle = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool ShowBorder
        {
            get { return base.ShowBorder; }
            set { base.ShowBorder = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override ContentAlignment TextAlign
        {
            get { return base.TextAlign; }
            set { base.TextAlign = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public new Point Location
        {
            get { return base.Location; }
            set { base.Location = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        public new Size Size
        {
            get { return base.Size; }
            set { base.Size = value; }
        }

        protected override ControlState CaptionState
        {
            get { return base.CaptionState; }
            set
            {
                if (base.CaptionState != value)
                {
                    base.CaptionState = value;
                    base.Invalidate(base.CaptionRect);
                }
            }
        }

        internal SkinPushPanel Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        #endregion

        #region Public Methods

        public void Expand()
        {
            if (Owner != null)
            {
                Owner.ExpandItem(this);
            }
        }

        public void Collapse()
        {
            if (Owner != null)
            {
                Owner.CollapseItem(this);
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnCaptionMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                base.Focus();
            }
            base.OnCaptionMouseClick(e);
        }

        protected override void OnPaintCaption(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle captionRect = CaptionRect;
            LinearGradientMode gradientMode = LinearGradientMode.Vertical;
            RoundStyle roundStyle = RoundStyle.All;

            Color backColor;
            switch (CaptionState)
            {
                case ControlState.Hover:
                    backColor = ColorTable.CaptionBackHover;
                    break;
                case ControlState.Pressed:
                    backColor = ColorTable.CaptionBackPressed;
                    break;
                default:
                    backColor = ColorTable.CaptionBackNormal;
                    break;
            }

            using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
            {
                RenderHelper.RenderBackgroundInternal(
                    g,
                    captionRect,
                    backColor,
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

        #endregion

        #region Render Methods

        private void RenderImageAndText(Graphics g, Rectangle captionRect)
        {
            Rectangle imageRect = Rectangle.Empty;
            Rectangle textRect = Rectangle.Empty;
            int bordWidth = base.BorderWidth;
            int imageWidth = CaptionHeight - 6;

            StringFormat sf = new StringFormat();
            sf.FormatFlags = StringFormatFlags.NoWrap;
            sf.Trimming = StringTrimming.EllipsisCharacter;

            bool rightToLeft = base.RightToLeft == RightToLeft.Yes;
            bool drawImage = Owner.ImageStyle == CaptionImageStyle.Draw ||
                Owner.ImageStyle == CaptionImageStyle.Image;

            if (rightToLeft)
            {
                sf.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
            }

            if (drawImage)
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

            switch (TextAlign)
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
                using (Brush brush = new SolidBrush(ColorTable.CaptionFore))
                {
                    g.DrawString(
                        base.Text,
                        CaptionFont,
                        brush,
                        textRect,
                        sf);
                }
            }

            if (drawImage)
            {
                if (Owner.ImageStyle == CaptionImageStyle.Image)
                {
                    Image image = _isExpanded ? 
                        Owner.ExpandImage : Owner.CollapseImage;
                    if (image != null)
                    {
                        g.DrawImage(
                            image,
                            imageRect,
                            0,
                            0,
                            image.Width,
                            image.Height,
                            GraphicsUnit.Pixel);
                    }
                }
                else
                {
                    ArrowDirection direction = _isExpanded ?
                        ArrowDirection.Up : ArrowDirection.Down;
                    using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g))
                    {
                        using (Brush brush = new SolidBrush(ColorTable.CaptionFore))
                        {
                            RenderHelper.RenderArrowInternal(
                                g,
                                imageRect,
                                direction,
                                brush);
                        }
                    }
                }
            }
        }

        #endregion
    }
}
