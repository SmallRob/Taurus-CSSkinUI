
using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using Com_CSSkin.Win32.Const;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    /// <summary>
    /// 实现标准的 Windows 跟踪条要求的基本功能。
    /// </summary>
    [ComVisible(true)]
    [DefaultEvent("Scroll")]
    [DefaultProperty("Value")]
    [DefaultBindingProperty("Value")]
    [Designer(typeof(TrackBarBaseDesigner))]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public abstract class TrackBarBase : Control, ISupportInitialize
    {
        #region Fileds
        private ControlState _scrollState;
        private int _cumulativeWheelData;
        private int _requestedDim;
        private Point _mouseDownPoint;
        private int _mouseDownValue;
        private bool _autoSize = true;
        private bool _initializing;
        private int _minimum = 0;
        private int _maximum = 100;
        private int _smallChange = 1;
        private int _largeChange = 5;
        private int _value = 0;
        private Orientation _orientation = Orientation.Horizontal;
        private TickStyle _tickStyle = TickStyle.BottomRight;
        private bool _drawFocus = true;

        protected Rectangle _scrollRect;
        protected Rectangle _tackRect;

        private static readonly object EventRendererChanged = new object();

        private static readonly object EventScroll = new object();
        private static readonly object EventValueChanged = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// 实现标准的 Windows 跟踪条要求的基本功能。
        /// </summary>
        protected TrackBarBase()
            : base()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.Selectable, true);
            SetStyle(ControlStyles.UseTextForAccessibility, false);

            _requestedDim = PreferredDimension;
        }

        #endregion

        #region Events

        public event EventHandler RendererChanged
        {
            add
            {
                base.Events.AddHandler(EventRendererChanged, value);
            }
            remove
            {
                base.Events.RemoveHandler(EventRendererChanged, value);
            }
        }

        public event EventHandler Scroll
        {
            add { base.Events.AddHandler(EventScroll, value); }
            remove { base.Events.RemoveHandler(EventScroll, value); }
        }

        public event EventHandler ValueChanged
        {
            add { base.Events.AddHandler(EventValueChanged, value); }
            remove { base.Events.RemoveHandler(EventValueChanged, value); }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        public new event EventHandler AutoSizeChanged
        {
            add { base.AutoSizeChanged += value; }
            remove { base.AutoSizeChanged -= value; }
        }

        #endregion

        #region Properties
        [DefaultValue(0)]
        [RefreshProperties(RefreshProperties.All)]
        [Category("行为")]
        [Description("SkinTrackBar 上滑块位置的最小值。")]
        public int Minimum
        {
            get { return _minimum; }
            set
            {
                if (value > _maximum)
                {
                    _maximum = value;
                }
                SetRange(value, _maximum);
            }
        }

        [DefaultValue(100)]
        [RefreshProperties(RefreshProperties.All)]
        [Category("行为")]
        [Description("SkinTrackBar 上滑块位置的最大值。")]
        public int Maximum
        {
            get { return _maximum; }
            set
            {
                if (_maximum != value)
                {
                    if (value < _minimum)
                    {
                        _minimum = value;
                    }
                    SetRange(_minimum, value);
                }
            }
        }

        [DefaultValue(1)]
        [Category("外观")]
        [Description("滑块为响应键盘输入(箭头键)而移动的位置数。")]
        public int SmallChange
        {
            get { return _smallChange; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("SmallChange");
                }
                if (_smallChange != value)
                {
                    _smallChange = value;
                }
            }
        }

        [DefaultValue(5)]
        [Category("行为")]
        [Description("滑块为响应鼠标单击或 Page Up 和 Page Down 键而移动的位置数。")]
        public int LargeChange
        {
            get { return _largeChange; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException("LargeChange");
                }
                if (_largeChange != value)
                {
                    _largeChange = value;
                }
            }
        }

        [Bindable(true)]
        [DefaultValue(0)]
        [Category("行为")]
        [Description("滑块的位置。")]
        public int Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    if (!_initializing && ((value < _minimum) || (value > _maximum)))
                    {
                        throw new ArgumentOutOfRangeException("Value");
                    }
                    _value = value;
                    Refresh();
                    OnValueChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(typeof(Orientation), "0")]
        [Localizable(true)]
        [Description("控件的方向")]
        [Category("外观")]
        public Orientation Orientation
        {
            get
            {
                return _orientation;
            }
            set
            {
                if (!ClientUtils.IsEnumValid(value, (int)value, 0, 1))
                {
                    throw new InvalidEnumArgumentException(
                        "value", (int)value, typeof(Orientation));
                }
                if (_orientation != value)
                {
                    _orientation = value;
                    if (_orientation == Orientation.Horizontal)
                    {
                        base.SetStyle(ControlStyles.FixedHeight, _autoSize);
                        base.SetStyle(ControlStyles.FixedWidth, false);
                        base.Width = _requestedDim;
                    }
                    else
                    {
                        base.SetStyle(ControlStyles.FixedHeight, false);
                        base.SetStyle(ControlStyles.FixedWidth, _autoSize);
                        base.Height = _requestedDim;
                    }
                    if (base.IsHandleCreated)
                    {
                        Rectangle bounds = base.Bounds;
                        base.RecreateHandle();
                        base.SetBounds(
                            bounds.X,
                            bounds.Y,
                            bounds.Height,
                            bounds.Width,
                            BoundsSpecified.All);
                        AdjustSize();
                    }

                    Invalidate();
                }
            }
        }

        [DefaultValue(typeof(TickStyle), "2")]
        [Category("Skin")]
        public virtual TickStyle TickStyle
        {
            get
            {
                return _tickStyle;
            }
            set
            {
                if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
                {
                    throw new InvalidEnumArgumentException("value", (int)value, typeof(TickStyle));
                }
                if (_tickStyle != value)
                {
                    _tickStyle = value;
                    Invalidate();
                }
            }
        }

        [DefaultValue(true)]
        [Category("Skin")]
        [Description("是否绘制控件获得焦点时的变换。")]
        public bool DrawFocus
        {
            get { return _drawFocus; }
            set
            {
                if (_drawFocus != value)
                {
                    _drawFocus = value;
                    base.Invalidate();
                }
            }
        }

        [Browsable(true)]
        [DefaultValue(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoSize
        {
            get
            {
                return _autoSize;
            }
            set
            {
                if (_autoSize != value)
                {
                    _autoSize = value;
                    if (_orientation == Orientation.Horizontal)
                    {
                        base.SetStyle(ControlStyles.FixedHeight, _autoSize);
                        base.SetStyle(ControlStyles.FixedWidth, false);
                    }
                    else
                    {
                        base.SetStyle(ControlStyles.FixedWidth, _autoSize);
                        base.SetStyle(ControlStyles.FixedHeight, false);
                    }
                    AdjustSize();
                    OnAutoSizeChanged(EventArgs.Empty);
                }
            }
        }

        protected override Size DefaultSize
        {
            get
            {
                return new Size(0x68, PreferredDimension);
            }
        }

        protected override ImeMode DefaultImeMode
        {
            get { return ImeMode.Disable; }
        }

        protected virtual int DefaultScrollWidth
        {
            get { return 12; }
        }

        protected virtual int DefaultScrollHeight
        {
            get { return 22; }
        }

        protected virtual int DefaultTrackHeight
        {
            get { return 8; }
        }

        internal protected ControlState ScrollState
        {
            get { return _scrollState; }
            set
            {
                if (_scrollState != value)
                {
                    _scrollState = value;
                    base.Invalidate(_scrollRect);
                }
            }
        }

        private int PreferredDimension
        {
            get
            {
                return SystemInformation.HorizontalScrollBarHeight * 8 / 3;
            }
        }

        #endregion

        #region Public Methods

        public void SetRange(int minValue, int maxValue)
        {
            if ((_minimum != minValue) || (_maximum != maxValue))
            {
                if (minValue > maxValue)
                {
                    maxValue = minValue;
                }
                _minimum = minValue;
                _maximum = maxValue;

                if (_value < _minimum)
                {
                    _value = _minimum;
                }
                if (_value > _maximum)
                {
                    _value = _maximum;
                }
                if (base.IsHandleCreated)
                {
                    base.Invalidate();
                }
            }
        }

        public override string ToString()
        {
            string str = base.ToString();
            return (str + ", Minimum: " + Minimum.ToString(CultureInfo.CurrentCulture) +
                ", Maximum: " + Maximum.ToString(CultureInfo.CurrentCulture) +
                ", Value: " + Value.ToString(CultureInfo.CurrentCulture));
        }

        #endregion

        #region ISupportInitialize 成员

        public void BeginInit()
        {
            _initializing = true;
        }

        public void EndInit()
        {
            _initializing = false;
            ConstrainValue();
        }
        #endregion

        #region Protected Methods

        protected virtual void OnValueChanged(EventArgs e)
        {
            EventHandler handler = (EventHandler)base.Events[EventValueChanged];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnScroll(EventArgs e)
        {
            EventHandler handler = (EventHandler)base.Events[EventScroll];
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void CalculateRect()
        {
            int trackWidth;
            switch (_orientation)
            {
                case Orientation.Horizontal:
                    trackWidth = (int)Math.Ceiling(
                        ((float)(_value - _minimum) / (_maximum - _minimum)) *
                        (Width - DefaultScrollWidth));
                    if (trackWidth > Width - DefaultScrollWidth - 1)
                    {
                        trackWidth = Width - DefaultScrollWidth - 1;
                    }

                    _tackRect = new Rectangle(0, (Height - DefaultTrackHeight) / 2,
                        Width, DefaultTrackHeight);
                    _scrollRect = new Rectangle(trackWidth, (Height - DefaultScrollHeight) / 2,
                        DefaultScrollWidth, DefaultScrollHeight);
                    if (RightToLeft == RightToLeft.Yes)
                    {
                        _tackRect.X = Width - _tackRect.Right;
                        _scrollRect.X = Width - _scrollRect.Right;
                    }
                    break;
                default:
                    trackWidth = (int)Math.Ceiling(
                        ((float)(_value - _minimum) / (_maximum - _minimum)) *
                        (Height - DefaultScrollWidth));
                    trackWidth = Height - trackWidth - DefaultScrollWidth;
                    if (trackWidth > Height - DefaultScrollWidth - 1)
                    {
                        trackWidth = Height - DefaultScrollWidth - 1;
                    }
                    _tackRect = new Rectangle(
                        (Width - DefaultTrackHeight) / 2, 0, DefaultTrackHeight, Height);
                    _scrollRect = new Rectangle((Width - DefaultScrollHeight) / 2, 
                        trackWidth, DefaultScrollHeight, DefaultScrollWidth);
                    break;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AdjustSize();
        }

        protected override void SetBoundsCore(
            int x, int y, int width, int height, BoundsSpecified specified)
        {
            _requestedDim = (_orientation == Orientation.Horizontal) ? height : width;
            if (_autoSize)
            {
                if (_orientation == Orientation.Horizontal)
                {
                    if ((specified & BoundsSpecified.Height) != BoundsSpecified.None)
                    {
                        height = PreferredDimension;
                    }
                }
                else if ((specified & BoundsSpecified.Width) != BoundsSpecified.None)
                {
                    width = PreferredDimension;
                }
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            if (_drawFocus)
            {
                base.Invalidate();
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            if (_drawFocus)
            {
                base.Invalidate();
            }
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM.WM_LBUTTONDOWN:
                    WmLButtonDown(ref m);
                    break;
                case WM.WM_LBUTTONUP:
                    WmMouseUp(ref m);
                    break;
                case WM.WM_MOUSEMOVE:
                    WmMouseMove(ref m);
                    break;
                case WM.WM_MOUSELEAVE:
                    WmMouseLeave(ref m);
                    break;
            }
            base.WndProc(ref m);
        }

        protected override bool IsInputKey(Keys keyData)
        {
            if ((keyData & Keys.Alt) == Keys.Alt)
            {
                return false;
            }
            switch ((keyData & Keys.KeyCode))
            {
                case Keys.Prior:
                case Keys.Next:
                case Keys.End:
                case Keys.Home:
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    return true;
            }
            return base.IsInputKey(keyData);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if ((keyData & Keys.Alt) == Keys.Alt)
            {
                return false;
            }

            switch ((keyData & Keys.KeyCode))
            {
                case Keys.Prior:
                    SetValue(_value + _largeChange);
                    return true;
                case Keys.Next:
                    SetValue(_value - _largeChange);
                    return true;
                case Keys.End:
                    SetValue(_maximum);
                    return true;
                case Keys.Home:
                    SetValue(_minimum);
                    return true;
                case Keys.Up:
                case Keys.Right:
                    SetValue(_value + _smallChange);
                    return true;
                case Keys.Down:
                case Keys.Left:
                    SetValue(_value - _smallChange);
                    return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            HandledMouseEventArgs args = e as HandledMouseEventArgs;
            if (args != null)
            {
                if (args.Handled)
                {
                    return;
                }
                args.Handled = true;
            }
            if (((Control.ModifierKeys & (Keys.Alt | Keys.Shift)) == Keys.None)
                && (Control.MouseButtons == MouseButtons.None))
            {
                int mouseWheelScrollLines = SystemInformation.MouseWheelScrollLines;
                if (mouseWheelScrollLines != 0)
                {
                    _cumulativeWheelData += e.Delta;
                    float changeLines = ((float)_cumulativeWheelData) / 120f;
                    if (mouseWheelScrollLines == -1)
                    {
                        mouseWheelScrollLines = _largeChange;
                    }
                    int changeValue = (int)(mouseWheelScrollLines * changeLines);
                    if (changeValue != 0)
                    {
                        int valueTmp;
                        int value;
                        if (changeValue > 0)
                        {
                            valueTmp = changeValue;
                            value = Math.Min(valueTmp + _value, _maximum);
                            _cumulativeWheelData -=
                                (int)(changeValue * (120f / ((float)mouseWheelScrollLines)));
                        }
                        else
                        {
                            valueTmp = -changeValue;
                            value = Math.Max(_value - valueTmp, _minimum);
                            _cumulativeWheelData -=
                                (int)(changeValue * (120f / ((float)mouseWheelScrollLines)));
                        }

                        if (value != _value)
                        {
                            _value = value;
                            Refresh();
                            OnScroll(EventArgs.Empty);
                            OnValueChanged(EventArgs.Empty);
                        }
                    }
                }
            }
        }

        #endregion

        #region Private Methods

        private void AdjustSize()
        {
            if (base.IsHandleCreated)
            {
                int requestedDim = _requestedDim;
                try
                {
                    if (_orientation == Orientation.Horizontal)
                    {
                        base.Height = _autoSize ? PreferredDimension : requestedDim;
                    }
                    else
                    {
                        base.Width = _autoSize ? PreferredDimension : requestedDim;
                    }
                }
                finally
                {
                    _requestedDim = requestedDim;
                }
            }
        }

        private void ConstrainValue()
        {
            if (!_initializing)
            {
                if (Value < _minimum)
                {
                    Value = _minimum;
                }
                if (Value > _maximum)
                {
                    Value = _maximum;
                }
            }
        }

        private void SetValue(int value)
        {
            if (value < _minimum)
            {
                value = _minimum;
            }
            else if (value > _maximum)
            {
                value = _maximum;
            }
            if (_value != value)
            {
                _value = value;
                Refresh();
                OnScroll(EventArgs.Empty);
                OnValueChanged(EventArgs.Empty);
            }
        }

        private void WmLButtonDown(ref Message m)
        {
            Focus();
            _mouseDownPoint = new Point(m.LParam.ToInt32());
            _mouseDownValue = _value;
            if (_scrollRect.Contains(_mouseDownPoint))
            {
                ScrollState = ControlState.Pressed;
            }
            else
            {
                int value;
                int position;
                bool rightToLeft = RightToLeft == RightToLeft.Yes;
                switch (_orientation)
                {
                    case Orientation.Horizontal:
                        if (rightToLeft)
                        {
                            if (_mouseDownPoint.X < DefaultScrollWidth)
                            {
                                position = Width - DefaultScrollWidth;
                            }
                            else if (_mouseDownPoint.X > Width - DefaultScrollWidth)
                            {
                                position = 0;
                            }
                            else
                            {
                                position = Width - DefaultScrollWidth - _mouseDownPoint.X;
                            }
                        }
                        else
                        {
                            if (_mouseDownPoint.X > Width - DefaultScrollWidth)
                            {
                                position = Width - DefaultScrollWidth;
                            }
                            else
                            {
                                position = _mouseDownPoint.X;
                            }
                        }
                        value = Width;
                        break;
                    default:
                        if (_mouseDownPoint.Y < DefaultScrollWidth)
                        {
                            position = Height - DefaultScrollWidth;
                        }
                        else if (_mouseDownPoint.Y > Height - DefaultScrollWidth)
                        {
                            position = 0;
                        }
                        else
                        {
                            position = Height - DefaultScrollWidth;
                        }
                        if (_mouseDownPoint.Y > DefaultScrollWidth)
                        {
                            position = Height - DefaultScrollWidth - _mouseDownPoint.Y;
                        }
                        value = Height;
                        break;
                }

                value = (int)Math.Ceiling(
                           ((float)position / (value - DefaultScrollWidth)) *
                           (_maximum - _minimum));

                value = _minimum + value;

                SetValue(value);
            }
        }

        private void WmMouseUp(ref Message m)
        {
            Point point = new Point(m.LParam.ToInt32());
            if (_scrollRect.Contains(point))
            {
                ScrollState = ControlState.Hover;
            }
            else
            {
                ScrollState = ControlState.Normal;
            }
        }

        private void WmMouseMove(ref Message m)
        {
            Point point = new Point(m.LParam.ToInt32());
            if (_scrollRect.Contains(point))
            {
                if (ScrollState != ControlState.Pressed)
                {
                    ScrollState = ControlState.Hover;
                }
            }
            else
            {
                if (ScrollState != ControlState.Pressed)
                {
                    ScrollState = ControlState.Normal;
                }
            }

            if (ScrollState != ControlState.Pressed)
            {
                return;
            }

            Point position = point;
            bool rightToLeft = RightToLeft == RightToLeft.Yes;
            int offset;
            int width;

            switch (_orientation)
            {
                case Orientation.Horizontal:
                    if (position.X < 0)
                    {
                        position.X = 0;
                    }
                    else if (position.X > Width)
                    {
                        position.X = Width;
                    }
                    offset = position.X - _mouseDownPoint.X;

                    if (rightToLeft)
                    {
                        offset = -offset;
                    }

                    width = Width;
                    break;
                default:
                    if (position.Y < 0)
                    {
                        position.Y = 0;
                    }
                    else if (position.Y > Height)
                    {
                        position.Y = Height;
                    }
                    offset = _mouseDownPoint.Y - position.Y;
                    width = Height;
                    break;
            }

            int change = (int)Math.Ceiling(
                (float)offset / (width - DefaultScrollWidth) *
                (_maximum - _minimum));
            if (change != 0)
            {
                int value = _mouseDownValue + change;
                SetValue(value);
            }
        }

        private void WmMouseLeave(ref Message m)
        {
            ScrollState = ControlState.Normal;
        }

        #endregion
    }
}
