
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(SplitContainer))]
    public class SkinSplitContainer : SplitContainer
    {
        private CollapsePanel _collapsePanel = CollapsePanel.Panel1;
        private SpliterPanelState _spliterPanelState = SpliterPanelState.Expanded;
        private ControlState _mouseState;
        private int _lastDistance;
        private int _minSize;
        private HistTest _histTest;
        private readonly object EventCollapseClick = new object();

        public SkinSplitContainer() {
            base.SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw, true);
            _lastDistance = base.SplitterDistance;
        }

        public event EventHandler CollapseClick {
            add { base.Events.AddHandler(EventCollapseClick, value); }
            remove { base.Events.RemoveHandler(EventCollapseClick, value); }
        }

        [DefaultValue(typeof(CollapsePanel), "1")]
        public CollapsePanel CollapsePanel {
            get { return _collapsePanel; }
            set {
                if (_collapsePanel != value) {
                    Expand();
                    _collapsePanel = value;
                }
            }
        }
        private Color lineBack = Color.FromArgb(206, 238, 255);
        [DefaultValue(typeof(Color), "206, 238, 255")]
        [Category("Skin")]
        [Description("�ָ��߽��䱳��ɫ1")]
        public Color LineBack {
            get { return lineBack; }
            set {
                if (lineBack != value) {
                    lineBack = value;
                    this.Invalidate();
                }
            }
        }
        private Color lineBack2 = Color.FromArgb(105, 200, 254);
        [DefaultValue(typeof(Color), "105, 200, 254")]
        [Category("Skin")]
        [Description("�ָ��߽��䱳��ɫ2")]
        public Color LineBack2 {
            get { return lineBack2; }
            set {
                if (lineBack2 != value) {
                    lineBack2 = value;
                    this.Invalidate();
                }
            }
        }

        private Color arroColor = Color.FromArgb(80, 136, 228);
        [DefaultValue(typeof(Color), "80, 136, 228")]
        [Category("Skin")]
        [Description("��ͷ��ɫ")]
        public Color ArroColor {
            get { return arroColor; }
            set {
                if (arroColor != value) {
                    arroColor = value;
                    this.Invalidate();
                }
            }
        }
        private Color arroHoverColor = Color.FromArgb(21, 66, 139);
        [DefaultValue(typeof(Color), "21, 66, 139")]
        [Category("Skin")]
        [Description("��ͷ����ʱ��ɫ")]
        public Color ArroHoverColor {
            get { return arroHoverColor; }
            set {
                if (arroHoverColor != value) {
                    arroHoverColor = value;
                    this.Invalidate();
                }
            }
        }

        protected virtual int DefaultCollapseWidth {
            get { return 80; }
        }

        protected virtual int DefaultArrowWidth {
            get { return 16; }
        }

        protected Rectangle CollapseRect {
            get {
                if (_collapsePanel == CollapsePanel.None) {
                    return Rectangle.Empty;
                }

                Rectangle rect = base.SplitterRectangle;
                if (base.Orientation == Orientation.Horizontal) {
                    rect.X = (base.Width - DefaultCollapseWidth) / 2;
                    rect.Width = DefaultCollapseWidth;
                } else {
                    rect.Y = (base.Height - DefaultCollapseWidth) / 2;
                    rect.Height = DefaultCollapseWidth;
                }

                return rect;
            }
        }

        internal SpliterPanelState SpliterPanelState {
            get { return _spliterPanelState; }
            set {
                if (_spliterPanelState != value) {
                    switch (value) {
                        case SpliterPanelState.Expanded:
                            Expand();
                            break;
                        case SpliterPanelState.Collapsed:
                            Collapse();
                            break;

                    }
                    _spliterPanelState = value;
                }
            }
        }

        internal ControlState MouseState {
            get { return _mouseState; }
            set {
                if (_mouseState != value) {
                    _mouseState = value;
                    base.Invalidate(CollapseRect);
                }
            }
        }

        public void Collapse() {
            if (_collapsePanel != CollapsePanel.None &&
                _spliterPanelState == SpliterPanelState.Expanded) {
                _lastDistance = base.SplitterDistance;
                if (_collapsePanel == CollapsePanel.Panel1) {
                    _minSize = base.Panel1MinSize;
                    base.Panel1MinSize = 0;
                    base.SplitterDistance = 0;
                } else {
                    int width = base.Orientation == Orientation.Horizontal ?
                        base.Height : base.Width;
                    _minSize = base.Panel2MinSize;
                    base.Panel2MinSize = 0;
                    base.SplitterDistance = width - base.SplitterWidth - base.Padding.Vertical;
                }
                base.Invalidate(base.SplitterRectangle);
            }
        }

        public void Expand() {
            if (_collapsePanel != CollapsePanel.None &&
               _spliterPanelState == SpliterPanelState.Collapsed) {
                if (_collapsePanel == CollapsePanel.Panel1) {
                    base.Panel1MinSize = _minSize;
                } else {
                    base.Panel2MinSize = _minSize;
                }
                base.SplitterDistance = _lastDistance;
                base.Invalidate(base.SplitterRectangle);
            }
        }

        protected virtual void OnCollapseClick(EventArgs e) {
            if (_spliterPanelState == SpliterPanelState.Collapsed) {
                SpliterPanelState = SpliterPanelState.Expanded;
            } else {
                SpliterPanelState = SpliterPanelState.Collapsed;
            }

            EventHandler handler = base.Events[EventCollapseClick] as EventHandler;
            if (handler != null) {
                handler(this, e);
            }
        }

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);

            if (base.Panel1Collapsed || base.Panel2Collapsed) {
                return;
            }

            Graphics g = e.Graphics;
            Rectangle rect = base.SplitterRectangle;
            bool bHorizontal = base.Orientation == Orientation.Horizontal;

            LinearGradientMode gradientMode = bHorizontal ?
                LinearGradientMode.Vertical : LinearGradientMode.Horizontal;

            using (LinearGradientBrush brush = new LinearGradientBrush(
                rect, LineBack,
                LineBack2, gradientMode)) {
                Blend blend = new Blend();
                blend.Positions = new float[] { 0f, .5f, 1f };
                blend.Factors = new float[] { .5F, 1F, .5F };

                brush.Blend = blend;
                g.FillRectangle(brush, rect);
            }

            if (_collapsePanel == CollapsePanel.None) {
                return;
            }

            Rectangle arrowRect;
            Rectangle topLeftRect;
            Rectangle bottomRightRect;

            CalculateRect(
                CollapseRect,
                out arrowRect,
                out topLeftRect,
                out bottomRightRect);

            ArrowDirection direction = ArrowDirection.Left;

            switch (_collapsePanel) {
                case CollapsePanel.Panel1:
                    if (bHorizontal) {
                        direction =
                            _spliterPanelState == SpliterPanelState.Collapsed ?
                            ArrowDirection.Down : ArrowDirection.Up;
                    } else {
                        direction =
                            _spliterPanelState == SpliterPanelState.Collapsed ?
                            ArrowDirection.Right : ArrowDirection.Left;
                    }
                    break;
                case CollapsePanel.Panel2:
                    if (bHorizontal) {
                        direction =
                            _spliterPanelState == SpliterPanelState.Collapsed ?
                            ArrowDirection.Up : ArrowDirection.Down;
                    } else {
                        direction =
                            _spliterPanelState == SpliterPanelState.Collapsed ?
                            ArrowDirection.Left : ArrowDirection.Right;
                    }
                    break;
            }

            Color foreColor = _mouseState == ControlState.Hover ?
                ArroHoverColor : ArroColor;
            using (SmoothingModeGraphics sg = new SmoothingModeGraphics(g)) {
                SplitRenderHelper.RenderGrid(g, topLeftRect, new Size(3, 3), foreColor);
                SplitRenderHelper.RenderGrid(g, bottomRightRect, new Size(3, 3), foreColor);

                using (Brush brush = new SolidBrush(foreColor)) {
                    SplitRenderHelper.RenderArrowInternal(
                        g,
                        arrowRect,
                        direction,
                        brush);
                }
            }
        }

        protected override void OnMouseMove(MouseEventArgs e) {
            //����������û�а��£�����HistTest
            if (e.Button != MouseButtons.Left) {
                _histTest = HistTest.None;
            }

            Rectangle collapseRect = CollapseRect;
            Point mousePoint = e.Location;

            //�����Button��������Ҳ������϶�
            if (collapseRect.Contains(mousePoint) &&
                _histTest != HistTest.Spliter) {
                base.Capture = false;
                SetCursor(Cursors.Hand);
                MouseState = ControlState.Hover;
                return;
            }//����ڷָ���������
            else if (base.SplitterRectangle.Contains(mousePoint)) {
                MouseState = ControlState.Normal;

                //����Ѿ��ڰ�ť�������������Ѿ��������Ͳ������϶���
                if (_histTest == HistTest.Button ||
                    (_collapsePanel != CollapsePanel.None &&
                    _spliterPanelState == SpliterPanelState.Collapsed)) {
                    base.Capture = false;
                    base.Cursor = Cursors.Default;
                    return;
                }

                //���û�а��£�����Split���
                if (_histTest == HistTest.None &&
                    !base.IsSplitterFixed) {
                    if (base.Orientation == Orientation.Horizontal) {
                        SetCursor(Cursors.HSplit);
                    } else {
                        SetCursor(Cursors.VSplit);
                    }
                    return;
                }
            }

            MouseState = ControlState.Normal;

            //�����϶��ָ���
            if (_histTest == HistTest.Spliter &&
                !base.IsSplitterFixed) {
                if (base.Orientation == Orientation.Horizontal) {
                    SetCursor(Cursors.HSplit);
                } else {
                    SetCursor(Cursors.VSplit);
                }
                base.OnMouseMove(e);
                return;
            }

            base.Cursor = Cursors.Default;
            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e) {
            base.Cursor = Cursors.Default;
            MouseState = ControlState.Normal;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e) {
            Rectangle collapseRect = CollapseRect;
            Point mousePoint = e.Location;

            if (collapseRect.Contains(mousePoint) ||
                (_collapsePanel != CollapsePanel.None &&
                _spliterPanelState == SpliterPanelState.Collapsed)) {
                _histTest = HistTest.Button;
                return;
            }

            if (base.SplitterRectangle.Contains(mousePoint)) {
                _histTest = HistTest.Spliter;
            }

            base.OnMouseDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e) {
            base.OnKeyUp(e);
            base.Invalidate(base.SplitterRectangle);
        }

        protected override void OnMouseUp(MouseEventArgs e) {
            base.OnMouseUp(e);
            base.Invalidate(base.SplitterRectangle);

            Rectangle collapseRect = CollapseRect;
            Point mousePoint = e.Location;

            if (_histTest == HistTest.Button &&
                e.Button == MouseButtons.Left &&
                collapseRect.Contains(mousePoint)) {
                OnCollapseClick(EventArgs.Empty);
            }
            _histTest = HistTest.None;
        }

        private void SetCursor(Cursor cursor) {
            if (base.Cursor != cursor) {
                base.Cursor = cursor;
            }
        }

        private void CalculateRect(
            Rectangle collapseRect,
            out Rectangle arrowRect,
            out Rectangle topLeftRect,
            out Rectangle bottomRightRect) {
            int width;
            if (base.Orientation == Orientation.Horizontal) {
                width = (collapseRect.Width - DefaultArrowWidth) / 2;
                arrowRect = new Rectangle(
                    collapseRect.X + width,
                    collapseRect.Y,
                    DefaultArrowWidth,
                    collapseRect.Height);

                topLeftRect = new Rectangle(
                    collapseRect.X,
                    collapseRect.Y + 1,
                    width,
                    collapseRect.Height - 2);

                bottomRightRect = new Rectangle(
                    arrowRect.Right,
                    collapseRect.Y + 1,
                    width,
                    collapseRect.Height - 2);
            } else {
                width = (collapseRect.Height - DefaultArrowWidth) / 2;
                arrowRect = new Rectangle(
                    collapseRect.X,
                    collapseRect.Y + width,
                    collapseRect.Width,
                    DefaultArrowWidth);

                topLeftRect = new Rectangle(
                    collapseRect.X + 1,
                    collapseRect.Y,
                    collapseRect.Width - 2,
                    width);

                bottomRightRect = new Rectangle(
                    collapseRect.X + 1,
                    arrowRect.Bottom,
                    collapseRect.Width - 2,
                    width);
            }
        }

        private enum HistTest
        {
            None,
            Button,
            Spliter
        }
    }
}
