
using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using Com_CSSkin.Imaging;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(TrackBar))]
    public class SkinTrackBar : TrackBarBase
    {
        #region 无参构造函数
        public SkinTrackBar()
        {
            BarStyle = HSLTrackBarStyle.Opacity;
            BackColor = Color.Transparent;
        }
        #endregion

        #region 参数变量
        private HSL _hsl = new HSL(0, 1d, 0.5d);
        private ColorBlend _blend;
        private static readonly Color InnerBorderColor = Color.FromArgb(160, 250, 250, 250);
        #endregion

        #region 变量与属性
        private Color _baseColor = Color.Red;
        [DefaultValue(typeof(Color), "Red")]
        [Category("Skin")]
        [Description("非图片绘制时SkinTrackBar色调。")]
        public Color BaseColor
        {
            get { return _baseColor; }
            set
            {
                if (_baseColor != value)
                {
                    _baseColor = value;
                    _hsl = ColorConverterEx.ColorToHSL(_baseColor);
                    CalcColors();
                    Refresh();
                }
            }
        }

        private Image bar;
        /// <summary>
        /// 滑块
        /// </summary>
        [Category("Skin")]
        [Description("滑块")]
        public Image Bar
        {
            get { return bar; }
            set
            {
                if (bar != value)
                {
                    bar = value;
                    this.Invalidate();
                }
            }
        }

        private Image track;
        /// <summary>
        /// 滑块条
        /// </summary>
        [Category("Skin")]
        [Description("滑块条")]
        public Image Track
        {
            get { return track; }
            set
            {
                if (track != value)
                {
                    track = value;
                    this.Invalidate();
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HSL HSL
        {
            get { return _hsl; }
            set
            {
                if (_hsl != value)
                {
                    _hsl = value;
                    _baseColor = ColorConverterEx.HSLToColor(_hsl);
                    CalcColors();
                    Refresh();
                }
            }
        }

        //[Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [DefaultValue(typeof(TickStyle), "2")]
        [Category("外观")]
        [Description("指示在 SkinTrackBar 上的哪些位置上显示刻度。")]
        public override TickStyle TickStyle
        {
            get
            {
                return base.TickStyle;
            }
            set
            {
                base.TickStyle = value;
            }
        }

        private HSLTrackBarStyle _barStyle;
        [DefaultValue(typeof(HSLTrackBarStyle), "0")]
        [Category("Skin")]
        [Description("滑块的绘画模式。")]
        public HSLTrackBarStyle BarStyle
        {
            get { return _barStyle; }
            set
            {
                if (_barStyle != value)
                {
                    _barStyle = value;
                    CalcColors();
                    Invalidate();
                }
            }
        }

        private Rectangle backrectangle = new Rectangle(5, 5, 5, 5);
        /// <summary>
        /// 滑块条九宫绘画区域
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(Rectangle), "5,5,5,5")]
        [Description("滑块条九宫绘画区域")]
        public Rectangle BackRectangle
        {
            get { return backrectangle; }
            set
            {
                if (backrectangle != value)
                {
                    backrectangle = value;
                }
                this.Invalidate();
            }
        }
        #endregion

        #region 重载事件
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //最高质量绘制文字
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            bool horizontal = Orientation == Orientation.Horizontal;
            CalculateRect();

            Color beginColor;
            Color endColor;
            switch (ScrollState)
            {
                case ControlState.Hover:
                    beginColor = Color.FromArgb(200, _baseColor);
                    endColor = Color.FromArgb(200, 255, 255, 255);
                    break;
                case ControlState.Pressed:
                    beginColor = Color.FromArgb(160, _baseColor);
                    endColor = Color.FromArgb(160, 255, 255, 255);
                    break;
                default:
                    beginColor = Color.FromArgb(180, _baseColor);
                    endColor = Color.FromArgb(180, 255, 255, 255);
                    break;
            }

            float angle = horizontal ? 90f : 0f;

            using (new SmoothingModeGraphics(g))
            {
                //选择绘画的滑块条
                switch (_barStyle)
                {
                    case HSLTrackBarStyle.Img:
                        DrawImgTrack(g, _tackRect, horizontal);
                        break;
                    case HSLTrackBarStyle.Opacity:
                        DrawOpacityTrack(g, _tackRect, horizontal);
                        break;
                    case HSLTrackBarStyle.Saturation:
                        DrawSaturationTrack(g, _tackRect, horizontal);
                        break;
                }
                //选择绘画的滑块
                if (_barStyle == HSLTrackBarStyle.Img)
                {
                    DrawImgThunm(g, _scrollRect, Orientation, TickStyle);
                }
                else
                {
                    DrawDefaultThunm(g, _scrollRect, beginColor,
                        endColor, _baseColor, angle, Orientation, TickStyle);
                }
            }
        }
        #endregion

        #region 绘画方法
        //用图片画滑块
        private void DrawImgThunm(Graphics g, Rectangle _scrollRect, Orientation orientation, TickStyle TickStyle)
        {
            if (Bar != null)
            {
                Image myImage = (Image)Bar.Clone();
                //水平
                if (orientation == Orientation.Horizontal)
                {
                    myImage.RotateFlip(TickStyle == TickStyle.TopLeft ? RotateFlipType.Rotate180FlipNone : RotateFlipType.RotateNoneFlipNone);
                }
                else    //垂直
                {
                    myImage.RotateFlip(TickStyle == TickStyle.TopLeft ? RotateFlipType.Rotate90FlipNone : RotateFlipType.Rotate270FlipNone);
                }
                g.DrawImage(myImage, _scrollRect);
            }
        }

        //用图片绘画滑块条
        private void DrawImgTrack(Graphics g, Rectangle _tackRect, bool horizontal)
        {
            if (Track != null)
            {
                Image myImage = (Image)Track.Clone();
                if (Orientation == Orientation.Vertical)
                {
                    myImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
                }
                ImageDrawRect.DrawRect(g, (Bitmap)myImage, _tackRect, BackRectangle, 1, 1);
            }
        }

        private void DrawSaturationTrack(
            Graphics g, Rectangle rect, bool horizontal)
        {
            float angle = horizontal ? 0f : 90f;
            using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                        rect, 6, RoundStyle.All, true))
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect, Color.Empty, Color.Empty, angle))
                {
                    brush.InterpolationColors = _blend;
                    g.FillPath(brush, path);
                }

                using (Pen pen = new Pen(_baseColor))
                {
                    g.DrawPath(pen, path);
                }
            }

            rect.Inflate(-1, -1);
            using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                rect, 6, RoundStyle.All, true))
            {
                using (Pen pen = new Pen(InnerBorderColor))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        private void DrawOpacityTrack(
            Graphics g, Rectangle rect, bool horizontal)
        {
            float mode = horizontal ? 0f : 270f;

            using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                        rect, 6, RoundStyle.All, true))
            {
                g.FillPath(Brushes.White, path);
                using (Pen pen = new Pen(_baseColor))
                {
                    g.DrawPath(pen, path);
                }
            }

            rect.Inflate(-1, -1);
            List<Point> points = GetOpacityBackLinePoints(rect, 3, horizontal);

            g.DrawLines(Pens.Silver, points.ToArray());

            using (GraphicsPath path = GraphicsPathHelper.CreatePath(
                rect, 6, RoundStyle.All, true))
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    rect, _baseColor, Color.Transparent, mode))
                {
                    g.FillPath(brush, path);
                }
            }
        }

        private void DrawDefaultThunm(
            Graphics g, Rectangle thumbRect,
            Color begin, Color end, Color border, float mode,
            Orientation orientation, TickStyle tickStyle)
        {
            ThumbArrowDirection direction = ThumbArrowDirection.None;

            switch (orientation)
            {
                case Orientation.Horizontal:
                    switch (tickStyle)
                    {
                        case TickStyle.None:
                        case TickStyle.BottomRight:
                            direction = ThumbArrowDirection.Down;
                            break;
                        case TickStyle.TopLeft:
                            direction = ThumbArrowDirection.Up;
                            break;
                        case TickStyle.Both:
                            direction = ThumbArrowDirection.None;
                            break;
                    }
                    break;
                case Orientation.Vertical:
                    switch (tickStyle)
                    {
                        case TickStyle.TopLeft:
                            direction = ThumbArrowDirection.Left;
                            break;
                        case TickStyle.None:
                        case TickStyle.BottomRight:
                            direction = ThumbArrowDirection.Right;
                            break;
                        case TickStyle.Both:
                            direction = ThumbArrowDirection.None;
                            break;
                    }
                    break;
            }
            using (GraphicsPath path =
                GraphicsPathHelper.CreateTrackBarThumbPath(
                thumbRect, direction))
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    thumbRect, begin, end, mode))
                {
                    Blend blend = new Blend();
                    blend.Positions = new float[] { 0, .2f, .5f, .8f, 1f };
                    blend.Factors = new float[] { 1f, .7f, 0, .7f, 1f };
                    brush.Blend = blend;

                    g.FillPath(brush, path);
                }
                using (Pen pen = new Pen(border))
                {
                    g.DrawPath(pen, path);
                }
            }

            thumbRect.Inflate(-1, -1);
            using (GraphicsPath path =
               GraphicsPathHelper.CreateTrackBarThumbPath(
               thumbRect, direction))
            {
                using (Pen pen = new Pen(InnerBorderColor))
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        private List<Point> GetOpacityBackLinePoints(
            Rectangle rect, int space, bool horizontal)
        {
            int length = horizontal ?
                (rect.Width - 4) / space : (rect.Height - 4) / space;
            List<Point> points = new List<Point>(length + 1);
            if (horizontal)
            {
                rect.X += 2;
                rect.Width -= 4;
                rect.Height--;

                for (int i = 0; i <= length; i += 2)
                {
                    points.Add(new Point(rect.X + i * space, rect.Bottom));
                    if (i + 1 <= length)
                    {
                        points.Add(new Point(rect.X + (i + 1) * space, rect.Y));
                    }
                }
            }
            else
            {
                rect.Y += 2;
                rect.Height -= 4;
                rect.Width--;
                for (int i = 0; i <= length; i += 2)
                {
                    points.Add(new Point(rect.X, rect.Y + i * space));
                    if (i + 1 <= length)
                    {
                        points.Add(new Point(rect.Right, rect.Y + (i + 1) * space));
                    }
                }
            }

            return points;
        }

        private void CalcColors()
        {
            switch (_barStyle)
            {
                case HSLTrackBarStyle.Saturation:
                    if (_blend == null)
                    {
                        _blend = new ColorBlend();
                    }
                    HSL hsl = _hsl;
                    Color[] colors = new Color[3];
                    hsl.Saturation = 1d;
                    colors[2] = ColorConverterEx.HSLToColor(hsl);
                    hsl.Saturation = 0.5d;
                    colors[1] = ColorConverterEx.HSLToColor(hsl);
                    hsl.Saturation = 0d;
                    colors[0] = ColorConverterEx.HSLToColor(hsl);

                    _blend.Colors = colors;
                    _blend.Positions = new float[] { 0, 0.5f, 1f };
                    break;
            }
        }
        #endregion
    }

    #region 绘画模式枚举
    public enum HSLTrackBarStyle
    {
        None = 0,
        Img = 1,
        Opacity = 2,
        Saturation = 3
    }
    #endregion
}
