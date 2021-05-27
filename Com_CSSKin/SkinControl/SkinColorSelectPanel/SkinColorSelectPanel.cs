
using System;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Com_CSSkin.Imaging;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(ColorDialog))]
    public class SkinColorSelectPanel : Control
    {
        #region 变量
        Color _selectedColor = Color.White;
        ColorSelectMode _selectMode;
        HSL _drawHsl = new HSL(0, 1f, 0.5f);
        HSL _selectedHsl = new HSL(0, 1f, 1f);
        bool _canDragChangeColor = true;
        Bitmap _bufferBmp;
        private readonly object EventSelectedColorChanged = new object();
        #endregion

        #region 无参构造函数
        public SkinColorSelectPanel()
        {
            //初始化
            Init();
        }
        #endregion

        #region 自定义事件

        public event EventHandler SelectedColorChanged
        {
            add { base.Events.AddHandler(EventSelectedColorChanged, value); }
            remove { base.Events.RemoveHandler(EventSelectedColorChanged, value); }
        }

        #endregion

        #region 属性
        /// <summary>
        /// 获取或设置 <see cref="SkinColorSelectPanel"/> 选择的颜色。
        /// </summary>
        [DefaultValue(typeof(Color), "White")]
        [Category("Skin")]
        [Description("获取或设置选择的颜色。")]
        public Color SelectedColor
        {
            get { return _selectedColor; }
            set 
            {
                if (_selectedColor != value)
                {
                    _selectedColor = value;
                    _selectedHsl = ColorConverterEx.ColorToHSL(value);
                    if (base.DesignMode && _selectedHsl.Luminance == 1.0d)
                    {
                        _drawHsl.Saturation = 1.0d;
                    }
                    else 
                    {
                        _drawHsl.Saturation = _selectedHsl.Saturation;
                    }
                    DisposeBufferImage();
                    Refresh();
                    OnSelectedColorChanged(EventArgs.Empty);
                }
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public HSL SelectedHsl
        {
            get { return _selectedHsl; }
            set
            {
                if (_selectedHsl != value)
                {
                    _selectedHsl = value;
                    _selectedColor = ColorConverterEx.HSLToColor(value);
                    if (base.DesignMode && _selectedHsl.Luminance == 1.0d)
                    {
                        _drawHsl.Saturation = 1.0d;
                    }
                    else
                    {
                        _drawHsl.Saturation = _selectedHsl.Saturation;
                    }
                    DisposeBufferImage();
                    Refresh();
                    OnSelectedColorChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// 获取或设置 <see cref="SkinColorSelectPanel"/> 颜色选择模式。
        /// </summary>
        [DefaultValue(typeof(ColorSelectMode), "0")]
        [Description("获取或设置颜色选择模式。")]
        [Category("Skin")]
        public ColorSelectMode SelectMode
        {
            get { return _selectMode; }
            set { _selectMode = value; }
        }

        [DefaultValue(true)]
        [Description("选择圈是否按下后跟随移动。")]
        [Category("Skin")]
        public bool CanDragChangeColor
        {
            get { return _canDragChangeColor; }
            set { _canDragChangeColor = value; }
        }

        protected RectangleF ColorSelectorRect
        {
            get
            {
                float x, y;
                switch (_selectMode)
                {
                    default:
                        x = (_selectedHsl.Hue * Width) / 360f;
                        y = (float)((1 - _selectedHsl.Luminance) * Height);
                        break;
                }

                RectangleF rect = new RectangleF(x, y, 0f, 0f);
                rect.Inflate(5, 5);
                return rect;
            }
        }

        #endregion

        #region 重载事件

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            DisposeBufferImage();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Focus();
            if (e.Button == MouseButtons.Left)
            {
                SetColor(e.Location);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point point = e.Location;
            if (_canDragChangeColor && e.Button == MouseButtons.Left)
            {
                if (point.X < 0)
                {
                    point.X = 0;
                }
                else if (point.X > Width)
                {
                    point.X = Width;
                }

                if (point.Y < 0)
                {
                    point.Y = 0;
                }
                else if (point.Y > Height)
                {
                    point.Y = Height;
                }

                SetColor(point);
            }

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            RectangleF rect = ClientRectangle;

            if (_bufferBmp != null)
            {
                g.DrawImageUnscaled(_bufferBmp, 0, 0);
            }
            else
            {
                _bufferBmp = new Bitmap(Width, Height, PixelFormat.Format32bppArgb);
                using (Graphics bg = Graphics.FromImage(_bufferBmp))
                {
                    switch (_selectMode)
                    {
                        default:
                            DrawDefaultBack(bg, rect);
                            break;
                    }
                }

                g.DrawImageUnscaled(_bufferBmp, 0, 0);
            }

            DrawSelector(g);
        }

        protected virtual void OnSelectedColorChanged(EventArgs e)
        {
            EventHandler handler = base.Events[EventSelectedColorChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region 绘画方法

        private void DrawDefaultBack(Graphics g, RectangleF rect)
        {
            float width = rect.Width / 360f;
            rect.Width = width;
            using (LinearGradientBrush brush = new LinearGradientBrush(
                new Point(0, 0), new Point(1, 0), Color.White, Color.White))
            {
                ColorBlend blend = new ColorBlend();
                blend.Colors = new Color[] { Color.White, Color.White, Color.Black };
                blend.Positions = new float[] { 0f, .5f, 1f };
                brush.Transform = new Matrix(0f, rect.Height, 1f, 0f, 0f, 0f);
                for (int i = 0; i < 360; i++)
                {
                    _drawHsl.Hue = i;
                    rect.X += width;
                    blend.Colors[1] = ColorConverterEx.HSLToColor(_drawHsl);
                    brush.InterpolationColors = blend;
                    g.FillRectangle(brush, rect);
                }
            }
        }

        private void DrawSelector(Graphics g)
        {
            RectangleF rect = ColorSelectorRect;
            using (new SmoothingModeGraphics(g))
            {
                using (Brush brush = new SolidBrush(Color.FromArgb(160, 255, 255, 255)))
                {
                    g.FillEllipse(brush, rect);
                }
                using (Pen pen = new Pen(Color.FromArgb(60, 230, 230, 230), 2f))
                {
                    g.DrawEllipse(pen, rect);

                    rect.Inflate(-2, -2);
                    pen.Color = Color.Black;
                    g.DrawEllipse(pen, rect);
                }
            }
        }

        #endregion

        #region 私有方法

        private void Init()
        {
            base.SetStyle(
                ControlStyles.UserPaint | 
                ControlStyles.OptimizedDoubleBuffer | 
                ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.ResizeRedraw | 
                ControlStyles.Selectable, true);
        }

        private void SetColor(Point mousePoint)
        {
            int hue;
            double sat, lum;
            switch (_selectMode)
            {
                default:
                    hue = (int)(((double)mousePoint.X / Width) * 360);
                    sat = _drawHsl.Saturation;
                    lum = 1 - (double)mousePoint.Y / Height;
                    break;
            }

            SelectedHsl = new HSL(hue, sat, lum);
        }

        private void DisposeBufferImage()
        {
            if (_bufferBmp != null)
            {
                _bufferBmp.Dispose();
                _bufferBmp = null;
            }
        }

        #endregion
    }
}
