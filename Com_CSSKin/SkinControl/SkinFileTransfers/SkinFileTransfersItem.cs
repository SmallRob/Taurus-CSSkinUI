
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    public class SkinFileTransfersItem : Control
    {
        #region Fields

        private Image _image;
        private string _fileName;
        private long _fileSize;
        private long _totalTransfersSize;
        private FileTransfersItemStyle _style;
        private RoundStyle _roundStyle = RoundStyle.All;
        private int _radius = 8;
        private Color _baseColor = Color.FromArgb(191, 233, 255);
        private Color _borderColor = Color.FromArgb(118, 208, 225);
        private Color _progressBarTrackColor = Color.Gainsboro;
        private Color _progressBarBarColor = Color.FromArgb(191, 233, 255);
        private Color _progressBarBorderColor = Color.FromArgb(118, 208, 225);
        private Color _progressBarTextColor = Color.FromArgb(0, 95, 147);
        private int _interval = 1000;
        private IFileTransfersItemText _fileTransfersText;
        private DateTime _startTime = DateTime.Now;
        private System.Threading.Timer _timer;
        private ControlState _saveState;
        private ControlState _saveToState;
        private ControlState _refuseState;
        private ControlState _cancelState;

        private static readonly object EventSaveButtonClick = new object();
        private static readonly object EventSaveToButtonClick = new object();
        private static readonly object EventRefuseButtonClick = new object();
        private static readonly object EventCancelButtonClick = new object();

        #endregion

        #region Constructors

        public SkinFileTransfersItem()
            : base()
        {
            SetStyles();
        }

        #endregion

        #region Events

        public event EventHandler SaveButtonClick
        {
            add { base.Events.AddHandler(EventSaveButtonClick, value); }
            remove { base.Events.RemoveHandler(EventSaveButtonClick, value); }
        }

        public event EventHandler SaveToButtonClick
        {
            add { base.Events.AddHandler(EventSaveToButtonClick, value); }
            remove { base.Events.RemoveHandler(EventSaveToButtonClick, value); }
        }

        public event EventHandler RefuseButtonClick
        {
            add { base.Events.AddHandler(EventRefuseButtonClick, value); }
            remove { base.Events.RemoveHandler(EventRefuseButtonClick, value); }
        }

        public event EventHandler CancelButtonClick
        {
            add { base.Events.AddHandler(EventCancelButtonClick, value); }
            remove { base.Events.RemoveHandler(EventCancelButtonClick, value); }
        }

        #endregion

        #region Properties

        [DefaultValue(typeof(Icon),"null")]
        public Image Image
        {
            get { return _image; }
            set
            {
                _image = value;
                base.Invalidate();
            }
        }

        [DefaultValue("")]
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                base.Invalidate();
            }
        }

        [DefaultValue(0)]
        public long FileSize
        {
            get { return _fileSize; }
            set
            {
                _fileSize = value;
                base.Invalidate();
            }
        }

        [DefaultValue(0)]
        public long TotalTransfersSize
        {
            get { return _totalTransfersSize; }
            set
            {
                if (_totalTransfersSize != value)
                {
                    if (value > _fileSize)
                    {
                        _totalTransfersSize = _fileSize;
                    }
                    else
                    {
                        _totalTransfersSize = value;
                    }
                    base.Invalidate(ProgressBarRect);
                    base.Invalidate(TransfersSizeRect);
                }
            }
        }

        [DefaultValue(typeof(FileTransfersItemStyle),"0")]
        public FileTransfersItemStyle Style
        {
            get { return _style; }
            set
            {
                _style = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(RoundStyle), "1")]
        public RoundStyle RoundStyle
        {
            get { return _roundStyle; }
            set
            {
                if (_roundStyle != value)
                {
                    _roundStyle = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(8)]
        public int Radius
        {
            get { return _radius; }
            set
            {
                if (_radius != value)
                {
                    _radius = value < 4 ? 4 : value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Color), "191, 233, 255")]
        public Color BaseColor
        {
            get { return _baseColor; }
            set
            {
                _baseColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "118, 208, 225")]
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "Gainsboro")]
        public Color ProgressBarTrackColor
        {
            get { return _progressBarTrackColor; }
            set
            {
                _progressBarTrackColor = value;
                base.Invalidate(ProgressBarRect);
            }
        }

        [DefaultValue(typeof(Color), "191, 233, 255")]
        public Color ProgressBarBarColor
        {
            get { return _progressBarBarColor; }
            set
            {
                _progressBarBarColor = value;
                base.Invalidate(ProgressBarRect);
            }
        }

        [DefaultValue(typeof(Color), "118, 208, 225")]
        public Color ProgressBarBorderColor
        {
            get { return _progressBarBorderColor; }
            set
            {
                _progressBarBorderColor = value;
                base.Invalidate(ProgressBarRect);
            }
        }

        [DefaultValue(typeof(Color), "0, 95, 147")]
        public Color ProgressBarTextColor
        {
            get { return _progressBarTextColor; }
            set
            {
                _progressBarTextColor = value;
                base.Invalidate(ProgressBarRect);
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public IFileTransfersItemText FileTransfersText
        {
            get 
            {
                if (_fileTransfersText == null)
                {
                    _fileTransfersText = new FileTransfersItemText();
                }
                return _fileTransfersText; 
            }
            set 
            { 
                _fileTransfersText = value;
                base.Invalidate();
            }
        }

        [DefaultValue(1000)]
        public int Interval
        {
            get { return _interval; }
            set { _interval = value; }
        }

        internal System.Threading.Timer Timer
        {
            get
            {
                if (_timer == null)
                {
                    _timer = new System.Threading.Timer(
                        new System.Threading.TimerCallback(delegate(object obj)
                        {
                            BeginInvoke(new MethodInvoker(delegate()
                            {
                                base.Invalidate(SpeedRect);
                            }));
                        }),
                        null,
                        System.Threading.Timeout.Infinite,
                        _interval);
                }
                return _timer;
            }
        }

        internal Rectangle ImageRect
        {
            get { return new Rectangle(6, 6, 32, 32); }
        }

        internal Rectangle TextRect
        {
            get { return new Rectangle(43, 6, Width - 49, 16); }
        }

        internal Rectangle FileNameRect
        {
            get { return new Rectangle(43, 22, Width - 49, 16); }
        }

        internal Rectangle ProgressBarRect
        {
            get { return new Rectangle(6, 41, Width - 12, 16); }
        }

        internal Rectangle SpeedRect
        {
            get { return new Rectangle(6, 60, Width / 2 - 8, 16); }
        }

        internal Rectangle TransfersSizeRect
        {
            get { return new Rectangle(Width / 2, 60, Width / 2 - 6, 16); }
        }

        internal Rectangle RefuseReceiveRect
        {
            get
            {
                Size size = TextRenderer.MeasureText(
                    FileTransfersText.RefuseReceive,
                    Font);
                return new Rectangle(
                    Width - size.Width - 7,
                    79,
                    size.Width,
                    size.Height);
            }
        }

        internal Rectangle CancelTransfersRect
        {
            get
            {
                Size size = TextRenderer.MeasureText(
                    FileTransfersText.CancelTransfers,
                    Font);
                return new Rectangle(
                    Width - size.Width - 7,
                    79,
                    size.Width,
                    size.Height);
            }
        }

        internal Rectangle SaveToRect
        {
            get
            {
                Size size = TextRenderer.MeasureText(
                   FileTransfersText.SaveTo,
                   Font);
                return new Rectangle(
                    RefuseReceiveRect.X - size.Width - 20,
                    79,
                    size.Width,
                    size.Height);
            }
        }

        internal Rectangle SaveRect
        {
            get
            {
                Size size = TextRenderer.MeasureText(
                   FileTransfersText.Save,
                   Font);
                return new Rectangle(
                    SaveToRect.X - size.Width - 20,
                    79,
                    size.Width,
                    size.Height);
            }
        }

        protected override Size DefaultSize
        {
            get { return new Size(200, 97); }
        }

        private ControlState SaveState
        {
            get { return _saveState; }
            set
            {
                if (_saveState != value)
                {
                    _saveState = value;
                    base.Invalidate(Inflate(SaveRect));
                }
            }
        }

        private ControlState SaveToState
        {
            get { return _saveToState; }
            set
            {
                if (_saveToState != value)
                {
                    _saveToState = value;
                    base.Invalidate(Inflate(SaveToRect));
                }
            }
        }

        private ControlState RefuseState
        {
            get { return _refuseState; }
            set
            {
                if (_refuseState != value)
                {
                    _refuseState = value;
                    base.Invalidate(Inflate(RefuseReceiveRect));
                }
            }
        }

        private ControlState CancelState
        {
            get { return _cancelState; }
            set
            {
                if (_cancelState != value)
                {
                    _cancelState = value;
                    base.Invalidate(Inflate(CancelTransfersRect));
                }
            }
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            _startTime = DateTime.Now;
            Timer.Change(_interval, _interval);
        }

        #endregion

        #region Virtual Methods

        protected virtual void OnSaveButtonClick(EventArgs e)
        {
            EventHandler handler =
                base.Events[EventSaveButtonClick] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnSaveToButtonClick(EventArgs e)
        {
            EventHandler handler =
                base.Events[EventSaveToButtonClick] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnRefuseButtonClick(EventArgs e)
        {
            EventHandler handler =
                base.Events[EventRefuseButtonClick] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnCancelButtonClick(EventArgs e)
        {
            EventHandler handler =
                base.Events[EventCancelButtonClick] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Override Methods

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point point = e.Location;
            switch (_style)
            {
                case FileTransfersItemStyle.ReadyReceive:
                    if (SaveRect.Contains(point))
                    {
                        SaveState = ControlState.Hover;
                    }
                    else
                    {
                        SaveState = ControlState.Normal;
                    }

                    if (SaveToRect.Contains(point))
                    {
                        SaveToState = ControlState.Hover;
                    }
                    else
                    {
                        SaveToState = ControlState.Normal;
                    }

                    if (RefuseReceiveRect.Contains(point))
                    {
                        RefuseState = ControlState.Hover;
                    }
                    else
                    {
                        RefuseState = ControlState.Normal;
                    }
                    break;
                case FileTransfersItemStyle.Receive:
                case FileTransfersItemStyle.Send:
                    if (CancelTransfersRect.Contains(point))
                    {
                        CancelState = ControlState.Hover;
                    }
                    else
                    {
                        CancelState = ControlState.Normal;
                    }
                    break;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            Point point = e.Location;
            switch (_style)
            {
                case FileTransfersItemStyle.ReadyReceive:
                    if (SaveRect.Contains(point))
                    {
                        SaveState = ControlState.Pressed;
                    }

                    if (SaveToRect.Contains(point))
                    {
                        SaveToState = ControlState.Pressed;
                    }
 

                    if (RefuseReceiveRect.Contains(point))
                    {
                        RefuseState = ControlState.Pressed;
                    }
                    break;
                case FileTransfersItemStyle.Receive:
                case FileTransfersItemStyle.Send:
                    if (CancelTransfersRect.Contains(point))
                    {
                        CancelState = ControlState.Pressed;
                    }
                    break;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            Point point = e.Location;
            switch (_style)
            {
                case FileTransfersItemStyle.ReadyReceive:
                    if (SaveRect.Contains(point))
                    {
                        SaveState = ControlState.Hover;
                        OnSaveButtonClick(e);
                    }
                    else
                    {
                        SaveState = ControlState.Normal;
                    }

                    if (SaveToRect.Contains(point))
                    {
                        SaveToState = ControlState.Hover;
                        OnSaveToButtonClick(e);
                    }
                    else
                    {
                        SaveToState = ControlState.Normal;
                    }

                    if (RefuseReceiveRect.Contains(point))
                    {
                        RefuseState = ControlState.Hover;
                        OnRefuseButtonClick(e);
                    }
                    else
                    {
                        RefuseState = ControlState.Normal;
                    }
                    break;
                case FileTransfersItemStyle.Receive:
                case FileTransfersItemStyle.Send:
                    if (CancelTransfersRect.Contains(point))
                    {
                        CancelState = ControlState.Hover;
                        OnCancelButtonClick(e);
                    }
                    else
                    {
                        CancelState = ControlState.Normal;
                    }
                    break;
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);

            switch (_style)
            {
                case FileTransfersItemStyle.ReadyReceive:
                    SaveState = ControlState.Normal;
                    SaveToState = ControlState.Normal;
                    RefuseState = ControlState.Normal;
                    break;
                case FileTransfersItemStyle.Receive:
                case FileTransfersItemStyle.Send:
                    CancelState = ControlState.Normal;
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            g.TextRenderingHint = TextRenderingHint.AntiAlias;

            if (Image != null)
            {
                g.DrawImage(
                    Image,
                    ImageRect,
                    new Rectangle(Point.Empty,Image.Size),
                    GraphicsUnit.Pixel);
            }

            TextFormatFlags flags =
                TextFormatFlags.Left |
                TextFormatFlags.Top |
                TextFormatFlags.SingleLine |
                TextFormatFlags.EndEllipsis;

            TextRenderer.DrawText(
                g,
                Text,
                Font,
                TextRect,
                ForeColor,
                flags);

            TextRenderer.DrawText(
                g,
                FileName,
                Font,
                FileNameRect,
                ForeColor,
                flags);

            Rectangle rect = ProgressBarRect;
            Color innerBorderColor = Color.FromArgb(200, 255, 255, 255);
            RenderBackgroundInternal(
                g,
                rect,
                _progressBarTrackColor,
                _progressBarBorderColor,
                innerBorderColor,
                RoundStyle.All,
                8,
                .45f,
                true,
                false,
                LinearGradientMode.Vertical);

            if (FileSize != 0)
            {
                float percent = (float)TotalTransfersSize / FileSize;
                int width = (int)(rect.Width * percent);
                width = Math.Min(width, rect.Width - 2);
                if (width > 5)
                {
                    Rectangle barRect = new Rectangle(
                        rect.X + 1,
                        rect.Y + 1,
                        width,
                        rect.Height - 2);
                    RenderBackgroundInternal(
                       g,
                       barRect,
                       _progressBarBarColor,
                       _progressBarBarColor,
                       innerBorderColor,
                       RoundStyle.All,
                       8,
                       .45F,
                       true,
                       false,
                       LinearGradientMode.Vertical);
                }
                TextRenderer.DrawText(
                    g,
                    percent.ToString("0.0%"),
                    Font,
                    rect,
                    _progressBarTextColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                string transferSizeText = string.Format(
                    "{0}/{1}",
                    GetText(_totalTransfersSize), GetText(_fileSize));
                TextRenderer.DrawText(
                    g,
                    transferSizeText,
                    Font,
                    TransfersSizeRect,
                    ForeColor,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                if (_totalTransfersSize != 0 && !DesignMode)
                {
                    TextRenderer.DrawText(
                       g,
                       GetSpeedText(),
                       Font,
                       SpeedRect,
                       ForeColor,
                       TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
                }
            }

            flags =
                TextFormatFlags.HorizontalCenter |
                TextFormatFlags.Top |
                TextFormatFlags.SingleLine |
                TextFormatFlags.EndEllipsis;

            switch (_style)
            {
                case FileTransfersItemStyle.ReadyReceive:
                    bool drawBack = false;
                    if (SaveState != ControlState.Normal)
                    {
                        rect = SaveRect;
                        rect.Inflate(2, 2);
                        drawBack = true;
                    }

                    if (SaveToState != ControlState.Normal)
                    {
                        rect = SaveToRect;
                        rect.Inflate(2, 2);
                        drawBack = true;
                    }

                    if (RefuseState != ControlState.Normal)
                    {
                        rect = RefuseReceiveRect;
                        rect.Inflate(2, 2);
                        drawBack = true;
                    }

                    if (drawBack)
                    {
                        RenderBackgroundInternal(
                            g,
                            rect,
                            _baseColor,
                            _borderColor,
                            innerBorderColor,
                            RoundStyle.All,
                            _radius,
                            0.45f,
                            true,
                            true,
                            LinearGradientMode.Vertical);
                    }

                    TextRenderer.DrawText(
                        g,
                        FileTransfersText.RefuseReceive,
                        Font,
                        RefuseReceiveRect,
                        ForeColor,
                        flags);

                    TextRenderer.DrawText(
                        g,
                        FileTransfersText.SaveTo,
                        Font,
                        SaveToRect,
                        ForeColor,
                        flags);

                    TextRenderer.DrawText(
                        g,
                        FileTransfersText.Save,
                        Font,
                        SaveRect,
                        ForeColor,
                        flags);
                    break;
                case FileTransfersItemStyle.Receive:
                case FileTransfersItemStyle.Send:

                    if (CancelState != ControlState.Normal)
                    {
                        rect = CancelTransfersRect;
                        rect.Inflate(2, 2);
                        RenderBackgroundInternal(
                            g,
                            rect,
                            _baseColor,
                            _borderColor,
                            innerBorderColor,
                            RoundStyle.All,
                            _radius,
                            0.45f,
                            true,
                            true,
                            LinearGradientMode.Vertical);
                    }

                    TextRenderer.DrawText(
                       g,
                       FileTransfersText.CancelTransfers,
                       Font,
                       CancelTransfersRect,
                       ForeColor,
                       flags);
                    break;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            Graphics g = e.Graphics;
            Rectangle rect = ClientRectangle;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            rect.Inflate(-1, -1);

            RenderBackgroundInternal(
                g,
                rect,
                _baseColor,
                _borderColor,
                Color.FromArgb(200, 255, 255),
                _roundStyle,
                _radius,
                .45F,
                true,
                false,
                LinearGradientMode.Vertical);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_timer != null)
                {
                    _timer.Dispose();
                }
                _fileTransfersText = null;
            }
        }

        #endregion

        #region Help Methods

        private void SetStyles()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw | 
                ControlStyles.FixedHeight, true);
            SetStyle(ControlStyles.Opaque, false);
            UpdateStyles();
        }

        internal void RenderBackgroundInternal(
          Graphics g,
          Rectangle rect,
          Color baseColor,
          Color borderColor,
          Color innerBorderColor,
          RoundStyle style,
          int roundWidth,
          float basePosition,
          bool drawBorder,
          bool drawGlass,
          LinearGradientMode mode)
        {
            if (drawBorder)
            {
                rect.Width--;
                rect.Height--;
            }

            if (rect.Width <= 0 || rect.Height <= 0)
            {
                return;
            }

            using (LinearGradientBrush brush = new LinearGradientBrush(
                rect, Color.Transparent, Color.Transparent, mode))
            {
                Color[] colors = new Color[4];
                colors[0] = GetColor(baseColor, 0, 35, 24, 9);
                colors[1] = GetColor(baseColor, 0, 13, 8, 3);
                colors[2] = baseColor;
                colors[3] = GetColor(baseColor, 0, 68, 69, 54);

                ColorBlend blend = new ColorBlend();
                blend.Positions = new float[] { 0.0f, basePosition, basePosition + 0.05f, 1.0f };
                blend.Colors = colors;
                brush.InterpolationColors = blend;
                if (style != RoundStyle.None)
                {
                    using (GraphicsPath path =
                        GraphicsPathHelper.CreatePath(rect, roundWidth, style, false))
                    {
                        g.FillPath(brush, path);
                    }

                    if (baseColor.A > 80)
                    {
                        Rectangle rectTop = rect;

                        if (mode == LinearGradientMode.Vertical)
                        {
                            rectTop.Height = (int)(rectTop.Height * basePosition);
                        }
                        else
                        {
                            rectTop.Width = (int)(rect.Width * basePosition);
                        }
                        using (GraphicsPath pathTop = GraphicsPathHelper.CreatePath(
                            rectTop, roundWidth, RoundStyle.Top, false))
                        {
                            using (SolidBrush brushAlpha =
                                new SolidBrush(Color.FromArgb(80, 255, 255, 255)))
                            {
                                g.FillPath(brushAlpha, pathTop);
                            }
                        }
                    }

                    if (drawGlass)
                    {
                        RectangleF glassRect = rect;
                        if (mode == LinearGradientMode.Vertical)
                        {
                            glassRect.Y = rect.Y + rect.Height * basePosition;
                            glassRect.Height = (rect.Height - rect.Height * basePosition) * 2;
                        }
                        else
                        {
                            glassRect.X = rect.X + rect.Width * basePosition;
                            glassRect.Width = (rect.Width - rect.Width * basePosition) * 2;
                        }
                        DrawGlass(g, glassRect, 170, 0);
                    }

                    if (drawBorder)
                    {
                        using (GraphicsPath path =
                            GraphicsPathHelper.CreatePath(rect, roundWidth, style, false))
                        {
                            using (Pen pen = new Pen(borderColor))
                            {
                                g.DrawPath(pen, path);
                            }
                        }

                        rect.Inflate(-1, -1);
                        using (GraphicsPath path =
                            GraphicsPathHelper.CreatePath(rect, roundWidth, style, false))
                        {
                            using (Pen pen = new Pen(innerBorderColor))
                            {
                                g.DrawPath(pen, path);
                            }
                        }
                    }
                }
                else
                {
                    g.FillRectangle(brush, rect);
                    if (baseColor.A > 80)
                    {
                        Rectangle rectTop = rect;
                        if (mode == LinearGradientMode.Vertical)
                        {
                            rectTop.Height = (int)(rectTop.Height * basePosition);
                        }
                        else
                        {
                            rectTop.Width = (int)(rect.Width * basePosition);
                        }
                        using (SolidBrush brushAlpha =
                            new SolidBrush(Color.FromArgb(80, 255, 255, 255)))
                        {
                            g.FillRectangle(brushAlpha, rectTop);
                        }
                    }

                    if (drawGlass)
                    {
                        RectangleF glassRect = rect;
                        if (mode == LinearGradientMode.Vertical)
                        {
                            glassRect.Y = rect.Y + rect.Height * basePosition;
                            glassRect.Height = (rect.Height - rect.Height * basePosition) * 2;
                        }
                        else
                        {
                            glassRect.X = rect.X + rect.Width * basePosition;
                            glassRect.Width = (rect.Width - rect.Width * basePosition) * 2;
                        }
                        DrawGlass(g, glassRect, 200, 0);
                    }

                    if (drawBorder)
                    {
                        using (Pen pen = new Pen(borderColor))
                        {
                            g.DrawRectangle(pen, rect);
                        }

                        rect.Inflate(-1, -1);
                        using (Pen pen = new Pen(innerBorderColor))
                        {
                            g.DrawRectangle(pen, rect);
                        }
                    }
                }
            }
        }

        private void DrawGlass(
            Graphics g, RectangleF glassRect, int alphaCenter, int alphaSurround)
        {
            DrawGlass(g, glassRect, Color.White, alphaCenter, alphaSurround);
        }

        private void DrawGlass(
            Graphics g,
            RectangleF glassRect,
            Color glassColor,
            int alphaCenter,
            int alphaSurround)
        {
            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddEllipse(glassRect);
                using (PathGradientBrush brush = new PathGradientBrush(path))
                {
                    brush.CenterColor = Color.FromArgb(alphaCenter, glassColor);
                    brush.SurroundColors = new Color[] { 
                        Color.FromArgb(alphaSurround, glassColor) };
                    brush.CenterPoint = new PointF(
                        glassRect.X + glassRect.Width / 2,
                        glassRect.Y + glassRect.Height / 2);
                    g.FillPath(brush, path);
                }
            }
        }

        private Color GetColor(Color colorBase, int a, int r, int g, int b)
        {
            int a0 = colorBase.A;
            int r0 = colorBase.R;
            int g0 = colorBase.G;
            int b0 = colorBase.B;

            if (a + a0 > 255) { a = 255; } else { a = Math.Max(a + a0, 0); }
            if (r + r0 > 255) { r = 255; } else { r = Math.Max(r + r0, 0); }
            if (g + g0 > 255) { g = 255; } else { g = Math.Max(g + g0, 0); }
            if (b + b0 > 255) { b = 255; } else { b = Math.Max(b + b0, 0); }

            return Color.FromArgb(a, r, g, b);
        }

        private string GetText(double size)
        {
            if (size < 1024)
            {
                return string.Format(
                    "{0} B", size.ToString("0.0"));
            }
            else if (size < 1024 * 1024)
            {
                return string.Format(
                    "{0} KB", (size / 1024.0f).ToString("0.0"));
            }
            else
            {
                return string.Format(
                    "{0} MB", (size / (1024.0f * 1024.0f)).ToString("0.0"));
            }
        }

        private string GetSpeedText()
        {
            TimeSpan span = DateTime.Now - _startTime;
            double speed = _totalTransfersSize / span.TotalSeconds;
            return string.Format("{0}/s", GetText(speed));
        }

        private Rectangle Inflate(Rectangle rect)
        {
            rect.Inflate(2, 2);
            return rect;
        }

        #endregion
    }
}
