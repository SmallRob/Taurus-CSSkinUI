
using Com_CSSkin.Imaging;
using Com_CSSkin.SkinClass;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    /// <summary>
    /// Winless scrollbar
    /// </summary>
    public class WLScrollBar : WLContainerBase
    {
        #region private vars

        // hscroll 滚动块的最小宽度，vscroll滚动块的最小高度
        const int MIN_MIDDLEBUTTON_LENGTH = 10;
        // 最小剩余的滚动块移动空间
        const int MIN_MOVABLE_LENGTH = 10;
        const int TIMER_INTERVAL_SLOW = 400;
        const int TIMER_INTERVAL_FAST = 50;

        CutePointAndValuePresenter presenter;
        bool isMouseDownInMiddleButton;
        bool isMouseDownInSideButton;
        LocationResult sideButtonDownWhere;
        Point sideButtonDownPoint;
        Point middleButtonMovePoint;

        // 用这个timer来模拟鼠标按下时连续触发mousedown
        Timer mouseDownTimer;

        bool middleButtonVisible;

        WLButton wlSideButton1;
        WLButton wlSideButton2;
        WLButton wlMiddleButton;

        private static readonly object EVENT_VALUECHANGED;

        #endregion

        #region private proterties

        private int BlankSpaceLength {
            get {
                int sideBtnLen = XTheme.ShowSideButtons ? XTheme.SideButtonLength * 2 : 0;
                return ScrollBarLength - XTheme.InnerPaddingWidth * 2 - sideBtnLen -
                    XTheme.MiddleButtonOutterSpace1 * 2;
            }
        }

        private int ActualMovableSpaceLength {
            get {
                return BlankSpaceLength - MiddleButtonLength + 1;
            }
        }

        protected int MiddleButtonLength {
            get {
                int len = (int)((float)BlankSpaceLength * MiddleButtonLengthPercentage / 100f);
                if ((BlankSpaceLength - len) < MIN_MOVABLE_LENGTH) {
                    len = BlankSpaceLength - MIN_MOVABLE_LENGTH;
                }
                return Math.Max(MIN_MIDDLEBUTTON_LENGTH, len);
            }
        }

        protected int MiddleButtonMaxPositionDot {
            get {
                return MiddleButtonBeginPositionDot + ActualMovableSpaceLength - 1;
            }
        }

        int _middleButtonCurrentPositionDot;
        protected int MiddleButtonCurrentPositionDot {
            get {
                return _middleButtonCurrentPositionDot;
            }
            set {
                if (_middleButtonCurrentPositionDot == value)
                    return;
                if (value > MiddleButtonMaxPositionDot)
                    _middleButtonCurrentPositionDot = MiddleButtonMaxPositionDot;
                else if (value < MiddleButtonBeginPositionDot)
                    _middleButtonCurrentPositionDot = MiddleButtonBeginPositionDot;
                else
                    _middleButtonCurrentPositionDot = value;

                wlMiddleButton.Bounds = MiddleButtonRect;
                Invalidate();
            }
        }

        /// <summary>
        /// 引发 ValueChanged 事件
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnValueChanged(EventArgs e) {
            EventHandler handler = (EventHandler)base.Events[EVENT_VALUECHANGED];
            if (handler != null) {
                handler(this, e);
            }
        }

        #endregion

        #region private vscroll, hscroll properties

        private int ScrollBarLength {
            get {
                if (BarOrientation == Orientation.Vertical)
                    return base.CtlSize.Height;
                else
                    return base.CtlSize.Width;
            }
        }

        private int MiddleButtonBeginPositionDot {
            get {
                int sizeBtnLen = XTheme.ShowSideButtons ? XTheme.SideButtonLength : 0;
                int left = (BarOrientation == Orientation.Horizontal) ? CtlLeft : CtlTop;
                return left + XTheme.InnerPaddingWidth + sizeBtnLen +
                    XTheme.MiddleButtonOutterSpace1;
            }
        }

        private Rectangle SideButton1Rect {
            get {
                if (BarOrientation == Orientation.Vertical) {
                    int width, height;
                    if (XTheme.ShowSideButtons) {
                        width = base.CtlSize.Width - XTheme.InnerPaddingWidth * 2;
                        height = XTheme.SideButtonLength;

                        int len = ScrollBarLength - XTheme.InnerPaddingWidth * 2;
                        if (len < 0)
                            len = 0;
                        if (len < XTheme.SideButtonLength * 2)
                            height = len / 2;
                    } else {
                        width = height = 0;
                    }
                    return new Rectangle(base.CtlLeft + XTheme.InnerPaddingWidth,
                        base.CtlTop + XTheme.InnerPaddingWidth, width, height);
                } else {
                    int width, height;
                    if (XTheme.ShowSideButtons) {
                        height = base.CtlSize.Height - XTheme.InnerPaddingWidth * 2;
                        width = XTheme.SideButtonLength;

                        int len = ScrollBarLength - XTheme.InnerPaddingWidth * 2;
                        if (len < 0)
                            len = 0;
                        if (len < XTheme.SideButtonLength * 2)
                            width = len / 2;
                    } else {
                        width = height = 0;
                    }
                    return new Rectangle(base.CtlLeft + XTheme.InnerPaddingWidth,
                        base.CtlTop + XTheme.InnerPaddingWidth, width, height);
                }
            }
        }

        private Rectangle SideButton2Rect {
            get {
                if (BarOrientation == Orientation.Vertical) {
                    Rectangle rect1 = SideButton1Rect;
                    return new Rectangle(
                        base.CtlLeft + XTheme.InnerPaddingWidth,
                        base.Bounds.Bottom - XTheme.InnerPaddingWidth - rect1.Height,
                        rect1.Width, rect1.Height);
                } else {
                    Rectangle rect1 = SideButton1Rect;
                    return new Rectangle(
                        base.Bounds.Right - XTheme.InnerPaddingWidth - rect1.Width,
                        base.CtlTop + XTheme.InnerPaddingWidth,
                        rect1.Width, rect1.Height);
                }
            }
        }

        private Rectangle MiddleButtonRect {
            get {
                if (BarOrientation == Orientation.Vertical) {
                    return new Rectangle(base.CtlLeft + XTheme.InnerPaddingWidth + XTheme.MiddleButtonOutterSpace2,
                        this.MiddleButtonCurrentPositionDot,
                        base.CtlSize.Width - XTheme.InnerPaddingWidth * 2 - XTheme.MiddleButtonOutterSpace2 * 2,
                        MiddleButtonLength);
                } else {
                    return new Rectangle(this.MiddleButtonCurrentPositionDot,
                        base.CtlTop + XTheme.InnerPaddingWidth + XTheme.MiddleButtonOutterSpace2,
                        MiddleButtonLength,
                        base.CtlSize.Height - XTheme.InnerPaddingWidth * 2 - XTheme.MiddleButtonOutterSpace2 * 2
                        );
                }
            }
        }

        private Rectangle BeforeMdlBtnRect {
            get {
                if (BarOrientation == Orientation.Vertical) {
                    return new Rectangle(base.CtlLeft + XTheme.InnerPaddingWidth,
                        MiddleButtonBeginPositionDot,
                        CtlSize.Width - XTheme.InnerPaddingWidth * 2,
                        MiddleButtonCurrentPositionDot - MiddleButtonBeginPositionDot);
                } else {
                    return new Rectangle(MiddleButtonBeginPositionDot,
                        CtlTop + XTheme.InnerPaddingWidth,
                        MiddleButtonCurrentPositionDot - MiddleButtonBeginPositionDot,
                        CtlSize.Height - XTheme.InnerPaddingWidth * 2);
                }
            }
        }

        private Rectangle AfterMdlBtnRect {
            get {
                if (BarOrientation == Orientation.Vertical) {
                    int left = CtlLeft + XTheme.InnerPaddingWidth;
                    int top = MiddleButtonRect.Bottom;
                    int width = CtlSize.Width - XTheme.InnerPaddingWidth * 2;
                    int height = SideButton2Rect.Top - top - XTheme.MiddleButtonOutterSpace1;
                    if (!XTheme.ShowSideButtons)
                        height += XTheme.SideButtonLength;
                    return new Rectangle(left, top, width, height);
                } else {
                    int top = CtlTop + XTheme.InnerPaddingWidth;
                    int left = MiddleButtonRect.Right;
                    int height = CtlSize.Height - XTheme.InnerPaddingWidth * 2;
                    int width = SideButton2Rect.Left - left - XTheme.MiddleButtonOutterSpace1;
                    if (!XTheme.ShowSideButtons)
                        width += XTheme.SideButtonLength;
                    return new Rectangle(left, top, width, height);
                }
            }
        }

        private ForePathRatoteDirection SideButton1RotateInfo {
            get {
                if (BarOrientation == Orientation.Vertical)
                    return ForePathRatoteDirection.Up;
                else
                    return ForePathRatoteDirection.Left;
            }
        }

        private ForePathRatoteDirection SideButton2RotateInfo {
            get {
                if (BarOrientation == Orientation.Vertical)
                    return ForePathRatoteDirection.Down;
                else
                    return ForePathRatoteDirection.Right;
            }
        }

        private Rectangle ExtraMiddleLineRect {
            get {
                if (BarOrientation == Orientation.Vertical) {
                    if (!XTheme.ShowSideButtons)
                        return Rectangle.Empty;

                    int y = CtlTop + XTheme.InnerPaddingWidth + XTheme.SideButtonLength / 2;
                    int x = CtlLeft + (CtlSize.Width - XTheme.ExtraMiddleLineLength) / 2;
                    int h = CtlSize.Height - XTheme.InnerPaddingWidth * 2 - XTheme.SideButtonLength;
                    return new Rectangle(x, y, XTheme.ExtraMiddleLineLength, h);
                } else {
                    if (!XTheme.ShowSideButtons)
                        return Rectangle.Empty;

                    int x = CtlLeft + XTheme.InnerPaddingWidth + XTheme.SideButtonLength / 2;
                    int y = CtlTop + (CtlSize.Height - XTheme.ExtraMiddleLineLength) / 2;
                    int w = CtlSize.Width - XTheme.InnerPaddingWidth * 2 - XTheme.SideButtonLength;
                    return new Rectangle(x, y, w, XTheme.ExtraMiddleLineLength);
                }
            }
        }

        #endregion

        #region private methods

        private LocationResult CheckLocation(Point p) {
            if (SideButton1Rect.Contains(p)) {
                return LocationResult.SideButton1;
            }
            if (SideButton2Rect.Contains(p)) {
                return LocationResult.SideButton2;
            }
            if (MiddleButtonRect.Contains(p) && middleButtonVisible) {
                return LocationResult.MiddleButton;
            }
            if (BeforeMdlBtnRect.Contains(p)) {
                return LocationResult.BeforeMiddleButton;
            }
            if (AfterMdlBtnRect.Contains(p)) {
                return LocationResult.AfterMiddleButton;
            }
            return LocationResult.NoWhere;
        }

        private void DoOnMouseDown(Point p) {
            LocationResult where = CheckLocation(p);
            if (where == LocationResult.MiddleButton) {
                isMouseDownInMiddleButton = true;
                middleButtonMovePoint = p;
                //wlMiddleButton.State = GMButtonState.Pressed;
            } else if (where != LocationResult.NoWhere) {
                isMouseDownInSideButton = true;
                sideButtonDownWhere = where;
                sideButtonDownPoint = p;
                MouseDownSideButton(where);
                mouseDownTimer.Enabled = true;
            }
        }

        private void MouseDownSideButton(LocationResult where) {
            int delta = 0;
            switch (where) {
                case LocationResult.SideButton1:
                    delta = -SmallChange;
                    //wlSideButton1.State = GMButtonState.Pressed;
                    break;
                case LocationResult.SideButton2:
                    //wlSideButton2.State = GMButtonState.Pressed;
                    delta = SmallChange;
                    break;
                case LocationResult.AfterMiddleButton:
                    delta = LargeChange;
                    break;
                case LocationResult.BeforeMiddleButton:
                    delta = -LargeChange;
                    break;
            }
            if (delta != 0) {
                ValueAdd(delta);
            }
        }

        private void DealMouseMoveWhenDownInSideButton(Point p) {
            sideButtonDownPoint = p;
        }

        private void DealMouseMoveWhenDownInMiddleButton(Point p) {
            if (BarOrientation == Orientation.Vertical) {
                if (p.Y < MiddleButtonBeginPositionDot || p.Y > (MiddleButtonMaxPositionDot + MiddleButtonLength))
                    return;
                int delta = p.Y - middleButtonMovePoint.Y;
                if (delta != 0)
                    this.DealMiddleButtonMove(delta);
                middleButtonMovePoint = p;
            } else {
                if (p.X < MiddleButtonBeginPositionDot || p.X > (MiddleButtonMaxPositionDot + MiddleButtonLength))
                    return;
                int delta = p.X - middleButtonMovePoint.X;
                if (delta != 0)
                    this.DealMiddleButtonMove(delta);
                middleButtonMovePoint = p;
            }
        }

        private void MouseDownTimerHandler(object sender, EventArgs e) {
            if (mouseDownTimer.Interval != TIMER_INTERVAL_FAST)
                mouseDownTimer.Interval = TIMER_INTERVAL_FAST;
            if (sideButtonDownWhere == CheckLocation(sideButtonDownPoint)) {
                MouseDownSideButton(sideButtonDownWhere);
            }
        }

        private void UpdateScrollInfo() {
            if (presenter == null)
                return;

            int valueCount = Maximum - Minimum + 1;
            int pointCount = ActualMovableSpaceLength;
            if (valueCount != presenter.ValueCount || pointCount != presenter.PointCount) {
                presenter.SetPointAndValueCount(pointCount, valueCount);
                ResetMiddleButtonPosition();
                Invalidate();
            }
        }

        /// <summary>
        /// 根据当前Value值重新设置滚动块的位置
        /// </summary>
        private void ResetMiddleButtonPosition() {
            int beginDot = MiddleButtonBeginPositionDot;
            int p1, p2;
            presenter.GetPointIndexFromValueIndex(Value - Minimum, out p1, out p2);
            p1 += beginDot;
            p2 += beginDot;
            if (MiddleButtonCurrentPositionDot >= p1 && MiddleButtonCurrentPositionDot <= p2)
                return;
            if (Value == Maximum)
                MiddleButtonCurrentPositionDot = p2;
            else
                MiddleButtonCurrentPositionDot = p1;
            Invalidate();
        }

        /// <summary>
        /// 判断当前是否有足够的空间来显示滚动块
        /// </summary>
        private bool HasEnoughRoomForMiddleButton() {
            int lenForMBtn = ScrollBarLength - XTheme.InnerPaddingWidth * 2 -
                XTheme.MiddleButtonOutterSpace1 * 2;
            if (XTheme.ShowSideButtons)
                lenForMBtn -= XTheme.SideButtonLength * 2;
            return (lenForMBtn > MIN_MIDDLEBUTTON_LENGTH);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        public void Refresh() {
            middleButtonVisible = HasEnoughRoomForMiddleButton() && this.Enabled;
            wlMiddleButton.Visible = middleButtonVisible;
            wlSideButton1.Bounds = SideButton1Rect;
            wlSideButton2.Bounds = SideButton2Rect;
            wlMiddleButton.Bounds = MiddleButtonRect;
            this.UpdateScrollInfo();
        }

        private void DoOnResize() {

            middleButtonVisible = HasEnoughRoomForMiddleButton() && this.Enabled;

            wlMiddleButton.Visible = middleButtonVisible;

            wlSideButton1.Bounds = SideButton1Rect;
            wlSideButton2.Bounds = SideButton2Rect;
            wlMiddleButton.Bounds = MiddleButtonRect;

            this.UpdateScrollInfo();
        }

        private void UpdateInfoToSideMiddleButton() {
            SetSideMdlBtnInfo();
            DoOnResize();
            Invalidate();
        }

        private void SetSideMdlBtnInfo() {
            wlSideButton1.SetNewTheme(XTheme.SideButtonTheme);
            wlSideButton2.SetNewTheme(XTheme.SideButtonTheme);
            wlMiddleButton.SetNewTheme(XTheme.MdlButtonTheme);

            wlSideButton1.ForePathGetter = XTheme.SideButtonForePathGetter;
            wlSideButton2.ForePathGetter = XTheme.SideButtonForePathGetter;
            wlSideButton1.ForePathSize = XTheme.SideButtonForePathSize;
            wlSideButton2.ForePathSize = XTheme.SideButtonForePathSize;

            wlSideButton1.HowForePathRender = XTheme.HowSideButtonForePathDraw;
            wlSideButton2.HowForePathRender = XTheme.HowSideButtonForePathDraw;

            wlSideButton1.BorderType = wlSideButton2.BorderType = XTheme.SideButtonBorderType;

            wlSideButton1.Visible = wlSideButton2.Visible = XTheme.ShowSideButtons;
        }

        private void SetButtonState(Point p, SkinButtonState newState) {
            wlMiddleButton.State = wlSideButton2.State = wlSideButton1.State = SkinButtonState.Normal;
            switch (CheckLocation(p)) {
                case LocationResult.MiddleButton:
                    wlMiddleButton.State = newState;
                    break;
                case LocationResult.SideButton1:
                    wlSideButton1.State = newState;
                    break;
                case LocationResult.SideButton2:
                    wlSideButton2.State = newState;
                    break;
            }
        }

        /// <summary>
        /// 移动滚动块时调用该方法来设置新Value
        /// </summary>
        /// <param name="moveDelta">移动偏移量</param>
        private void DealMiddleButtonMove(int moveDelta) {
            MiddleButtonCurrentPositionDot += moveDelta;
            int locIndex = MiddleButtonCurrentPositionDot - MiddleButtonBeginPositionDot;
            int v = 0;
            presenter.GetExactValueIndexFromPointIndex(locIndex, out v);
            Value = Minimum + v;
        }

        #endregion

        #region private rendering

        private void RenderBackground(Graphics g) {
            BasicBlockPainter.RenderFlatBackground(
                g,
                Bounds,
                XTheme.BackColor,
                ButtonBorderType.Rectangle,
                XTheme.BackgroundRadius,
                RoundStyle.All);
        }

        private void RenderBorders(Graphics g) {
            BasicBlockPainter.RenderBorder(
                g,
                Bounds,
                XTheme.BorderColor,
                ButtonBorderType.Rectangle,
                XTheme.BackgroundRadius,
                RoundStyle.All);
        }

        private void MiddleButtonExtraPaint(object sender, PaintEventArgs e) {
            if (!XTheme.DrawLinesInMiddleButton)
                return;

            // 画3段，每段2条线组成，每段间隔1，故8
            int linesLen = 8;

            //如果没有足够空间画线了则不画了
            if (MiddleButtonLength < (linesLen + XTheme.MiddleBtnLineOutterSpace1 * 2))
                return;

            Pen p1 = new Pen(XTheme.MiddleButtonLine1Color);
            Pen p2 = new Pen(XTheme.MiddleButtonLine2Color);
            Rectangle rect = e.ClipRectangle;

            if (BarOrientation == Orientation.Vertical) {
                int x1 = rect.Left + XTheme.MiddleBtnLineOutterSpace2;
                int x2 = rect.Right - XTheme.MiddleBtnLineOutterSpace2 - 1;
                int y = rect.Top + (rect.Height - linesLen) / 2;
                for (int i = 0; i < 3; i++) {
                    e.Graphics.DrawLine(p1, x1, y, x2, y);
                    e.Graphics.DrawLine(p2, x1, y + 1, x2, y + 1);

                    y += 3;
                }
            } else {
                int x = rect.Left + (rect.Width - linesLen) / 2;
                int y1 = rect.Top + XTheme.MiddleBtnLineOutterSpace2;
                int y2 = rect.Bottom - XTheme.MiddleBtnLineOutterSpace2 - 1;
                for (int i = 0; i < 3; i++) {
                    e.Graphics.DrawLine(p1, x, y1, x, y2);
                    e.Graphics.DrawLine(p2, x + 1, y1, x + 1, y2);
                    x += 3;
                }
            }

            p1.Dispose();
            p2.Dispose();
        }

        #endregion

        #region constructors

        static WLScrollBar() {
            EVENT_VALUECHANGED = new object();
        }

        public WLScrollBar(Control owner, Orientation barOrientation)
            : base(owner) {
            _barOrientation = barOrientation;
            presenter = new CutePointAndValuePresenter();
            ButtonsIni();
            mouseDownTimer = new Timer();
            mouseDownTimer.Enabled = false;
            mouseDownTimer.Interval = TIMER_INTERVAL_SLOW;
            mouseDownTimer.Tick += new EventHandler(MouseDownTimerHandler);
        }

        public WLScrollBar(Control owner)
            : this(owner, Orientation.Vertical) {
        }

        /// <summary>
        /// sidebutton1/2, middlebutton 初始化
        /// </summary>
        private void ButtonsIni() {
            wlSideButton1 = new WLButton(base.Owner);
            wlSideButton2 = new WLButton(base.Owner);
            wlMiddleButton = new WLButton(base.Owner);

            wlSideButton2.PressedLeaveDrawAsPressed = wlSideButton1.PressedLeaveDrawAsPressed =
                wlMiddleButton.PressedLeaveDrawAsPressed = true;

            base.WLControls.Add(wlSideButton1);
            base.WLControls.Add(wlSideButton2);
            base.WLControls.Add(wlMiddleButton);

            wlSideButton1.RotateDirection = SideButton1RotateInfo;
            wlSideButton2.RotateDirection = SideButton2RotateInfo;

            SetSideMdlBtnInfo();

            wlMiddleButton.Paint += new PaintEventHandler(MiddleButtonExtraPaint);
        }

        #endregion

        #region control events

        public event EventHandler ValueChanged {
            add {
                base.Events.AddHandler(EVENT_VALUECHANGED, value);
            }
            remove {
                base.Events.RemoveHandler(EVENT_VALUECHANGED, value);
            }
        }

        #endregion

        #region public proterties

        SkinScrollBarThemeBase _xtheme;
        int _maximum = 100;
        int _minimum = 0;
        int _value = 0;
        int _smallChange = 1;
        int _largeChange = 10;
        int _middleButtonLengthPercentage = 10;
        Orientation _barOrientation = Orientation.Vertical;

        // readonly        
        public SkinScrollBarThemeBase XTheme {
            get {
                if (_xtheme == null) {
                    _xtheme = new SkinScrollBarThemeBase();
                    UpdateInfoToSideMiddleButton();
                }
                return _xtheme;
            }
        }

        public Orientation BarOrientation {
            get {
                return _barOrientation;
            }
        }

        // scrollbar setting        
        public int Value {
            get {
                return this._value;
            }
            set {
                if (this._value == value)
                    return;
                if (value < Minimum || value > Maximum)
                    throw new ArgumentOutOfRangeException("Value");

                this._value = value;
                ResetMiddleButtonPosition();
                OnValueChanged(EventArgs.Empty);
            }
        }

        public int Minimum {
            get {
                return this._minimum;
            }
            set {
                if (this._minimum == value)
                    return;

                if (value > this._maximum)
                    this._maximum = value;

                if (this.Value < value)
                    this.Value = value;

                this._minimum = value;
                this.UpdateScrollInfo();
            }
        }

        public int Maximum {
            get {
                return this._maximum;
            }
            set {
                if (this._maximum == value)
                    return;

                if (value < this._minimum)
                    this._minimum = value;

                if (this.Value > value)
                    this.Value = value;

                this._maximum = value;
                this.UpdateScrollInfo();
            }
        }

        public int SmallChange {
            get {
                return _smallChange;
            }
            set {
                _smallChange = value < 1 ? 1 : value;
            }
        }

        public int LargeChange {
            get {
                return _largeChange;
            }
            set {
                _largeChange = value < 1 ? 1 : value;
            }
        }

        public int MiddleButtonLengthPercentage {
            get {
                return this._middleButtonLengthPercentage;
            }
            set {
                if (this._middleButtonLengthPercentage == value)
                    return;
                if (value >= 1 && value <= 99) {
                    this._middleButtonLengthPercentage = value;
                } else {
                    if (value < 1)
                        this._middleButtonLengthPercentage = 1;
                    if (value > 99)
                        this._middleButtonLengthPercentage = 99;
                }
                this.UpdateScrollInfo();
                wlMiddleButton.Bounds = MiddleButtonRect;
            }
        }

        #endregion

        #region public methods

        /// <summary>
        /// 给ScrollBar设置一个新的主题
        /// </summary>
        /// <param name="xtheme">主题实例</param>
        public void SetNewTheme(SkinScrollBarThemeBase xtheme) {
            if (xtheme == null)
                throw new ArgumentNullException("xtheme");
            _xtheme = xtheme;
            UpdateInfoToSideMiddleButton();
            Invalidate();
        }

        /// <summary>
        /// 将Value值加上指定的量
        /// </summary>
        /// <param name="amount">需要增加的量，可正可负</param>
        public void ValueAdd(int amount) {
            int value = Value;
            value += amount;
            if (value < Minimum)
                value = Minimum;
            if (value > Maximum)
                value = Maximum;
            Value = value;
        }

        /// <summary>
        /// 将当前Value值加上SmallChange量
        /// </summary>
        public void SmallAdd() {
            ValueAdd(SmallChange);
        }

        /// <summary>
        /// 将当前Value值减去SmallChange量
        /// </summary>
        public void SmallSub() {
            ValueAdd(-SmallChange);
        }

        /// <summary>
        /// 将当前Value值加上LargeChange量
        /// </summary>
        public void LargeAdd() {
            ValueAdd(LargeChange);
        }

        /// <summary>
        /// 将当前Value值减去LargeChange量
        /// </summary>
        public void LargeSub() {
            ValueAdd(-LargeChange);
        }

        #endregion

        #region override base methods

        #region paint

        protected override void OnPaintBackground(Graphics g, Rectangle clipRect) {
            base.OnPaintBackground(g, clipRect);

            if (XTheme.DrawBackground) {
                RenderBackground(g);
            }
            if (XTheme.DrawExtraMiddleLine) {
                using (NewSmoothModeGraphics ng = new NewSmoothModeGraphics(g, SmoothingMode.HighSpeed)) {
                    using (SolidBrush sb = new SolidBrush(XTheme.ExtraMiddleLineColor)) {
                        g.FillRectangle(sb, ExtraMiddleLineRect);
                    }
                }
            }
        }

        protected override void OnPaintBorder(Graphics g, Rectangle clipRect) {
            base.OnPaintBorder(g, clipRect);
            if (XTheme.DrawBorder) {
                RenderBorders(g);
            }
        }

        protected override void OnPaintContent(Graphics g, Rectangle clipRect) {
            base.OnPaintContent(g, clipRect);
        }

        #endregion

        #region mouse

        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                DoOnMouseDown(e.Location);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                isMouseDownInMiddleButton = false;
                isMouseDownInSideButton = false;
                sideButtonDownWhere = LocationResult.NoWhere;
                mouseDownTimer.Enabled = false;
                mouseDownTimer.Interval = TIMER_INTERVAL_SLOW;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            base.OnMouseMove(e);
            if (isMouseDownInSideButton) {
                DealMouseMoveWhenDownInSideButton(e.Location);
            }
            if (isMouseDownInMiddleButton) {
                DealMouseMoveWhenDownInMiddleButton(e.Location);
            }
        }

        #endregion

        #region size, enable

        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);
            DoOnResize();
        }

        protected override void OnLocationChanged(EventArgs e) {
            base.OnLocationChanged(e);
            DoOnResize();
        }

        protected override void OnEnabledChanged(EventArgs e) {
            base.OnEnabledChanged(e);

            if (wlSideButton1 != null) {
                wlSideButton2.Enabled = base.Enabled;
                wlSideButton1.Enabled = base.Enabled;

                middleButtonVisible = HasEnoughRoomForMiddleButton() && base.Enabled;
                wlMiddleButton.Visible = middleButtonVisible;
                Invalidate();
            }
        }

        #endregion

        #endregion

        #region private class

        /// <summary>
        /// 指示鼠标位于哪个区域
        /// </summary>
        private enum LocationResult
        {
            NoWhere,
            SideButton1,
            SideButton2,
            MiddleButton,
            BeforeMiddleButton,
            AfterMiddleButton
        }

        #endregion
    }
}
