
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Com_CSSkin.SkinClass;
using System.Threading;
using Com_CSSkin.SkinControl;

namespace Com_CSSkin
{
    public class ControlBoxManager : IDisposable
    {
        private CSSkinMain _owner;
        private bool _mouseDown;
        private ControlBoxState _closBoxState;
        private ControlBoxState _minimizeBoxState;
        private ControlBoxState _maximizeBoxState;

        public ControlBoxManager(CSSkinMain owner) {
            _owner = owner;
        }

        public bool CloseBoxVisibale {
            get { return _owner.ControlBox; }
        }

        public bool MaximizeBoxVisibale {
            get { return _owner.ControlBox && _owner.MaximizeBox; }
        }

        public bool MinimizeBoxVisibale {
            get { return _owner.ControlBox && _owner.MinimizeBox; }
        }

        public Rectangle CloseBoxRect {
            get {
                if (CloseBoxVisibale) {
                    Point offset = ControlBoxOffset;
                    Size size = _owner.CloseBoxSize;
                    return new Rectangle(
                        _owner.Width - offset.X - size.Width,
                        offset.Y,
                        size.Width,
                        size.Height);
                }
                return Rectangle.Empty;
            }
        }

        public Rectangle MaximizeBoxRect {
            get {
                if (MaximizeBoxVisibale) {
                    Point offset = ControlBoxOffset;
                    Size size = _owner.MaxSize;
                    return new Rectangle(
                        CloseBoxRect.X - ControlBoxSpace - size.Width,
                        offset.Y,
                        size.Width,
                        size.Height);
                }
                return Rectangle.Empty;
            }
        }

        public Rectangle MinimizeBoxRect {
            get {
                if (MinimizeBoxVisibale) {
                    Point offset = ControlBoxOffset;
                    Size size = _owner.MiniSize;
                    int x = MaximizeBoxVisibale ?
                        MaximizeBoxRect.X - ControlBoxSpace - size.Width :
                        CloseBoxRect.X - ControlBoxSpace - size.Width;
                    return new Rectangle(
                        x,
                        offset.Y,
                        size.Width,
                        size.Height);
                }
                return Rectangle.Empty;
            }
        }

        public CustomSysButtonCollection SysButtonItems {
            get {
                CmSysButton Sitem = null;
                foreach (CmSysButton item in _owner.SysButtonItems) {
                    Size size = item.Size;
                    int x =
                        MinimizeBoxVisibale ?
                        MinimizeBoxRect.X - ControlBoxSpace - size.Width :
                        MaximizeBoxVisibale ?
                        MaximizeBoxRect.X - ControlBoxSpace - size.Width :
                        CloseBoxRect.X - ControlBoxSpace - size.Width;
                    if (Sitem != null) {
                        if (Sitem.Visibale) {
                            x = Sitem.Bounds.X - ControlBoxSpace - size.Width;
                        }
                    }
                    Rectangle rc = new Rectangle(new Point(x, _owner.ControlBoxOffset.Y), item.Bounds.Size);
                    item.Bounds = rc;
                    Sitem = item;
                }
                return _owner.SysButtonItems;
            }
        }

        public ControlBoxState CloseBoxState {
            get { return _closBoxState; }
            protected set {
                if (_closBoxState != value) {
                    _closBoxState = value;
                    if (_owner != null) {
                        Invalidate(CloseBoxRect);
                    }
                }
            }
        }

        public ControlBoxState MinimizeBoxState {
            get { return _minimizeBoxState; }
            protected set {
                if (_minimizeBoxState != value) {
                    _minimizeBoxState = value;
                    if (_owner != null) {
                        Invalidate(MinimizeBoxRect);
                    }
                }
            }
        }

        public ControlBoxState MaximizeBoxState {
            get { return _maximizeBoxState; }
            protected set {
                if (_maximizeBoxState != value) {
                    _maximizeBoxState = value;
                    if (_owner != null) {
                        Invalidate(MaximizeBoxRect);
                    }
                }
            }
        }

        public Point ControlBoxOffset {
            get { return _owner.ControlBoxOffset; }
        }

        public int ControlBoxSpace {
            get { return _owner.ControlBoxSpace; }
        }

        public void ProcessMouseOperate(
            Point mousePoint, MouseOperationType operate) {
            if (!_owner.ControlBox) {
                return;
            }

            Rectangle closeBoxRect = CloseBoxRect;
            Rectangle minimizeBoxRect = MinimizeBoxRect;
            Rectangle maximizeBoxRect = MaximizeBoxRect;

            bool closeBoxVisibale = CloseBoxVisibale;
            bool minimizeBoxVisibale = MinimizeBoxVisibale;
            bool maximizeBoxVisibale = MaximizeBoxVisibale;

            switch (operate) {
                case MouseOperationType.Move:
                    ProcessMouseMove(
                        mousePoint,
                        closeBoxRect,
                        minimizeBoxRect,
                        maximizeBoxRect,
                        closeBoxVisibale,
                        minimizeBoxVisibale,
                        maximizeBoxVisibale);
                    break;
                case MouseOperationType.Down:
                    ProcessMouseDown(
                        mousePoint,
                        closeBoxRect,
                        minimizeBoxRect,
                        maximizeBoxRect,
                        closeBoxVisibale,
                        minimizeBoxVisibale,
                        maximizeBoxVisibale);
                    break;
                case MouseOperationType.Up:
                    ProcessMouseUP(
                        mousePoint,
                        closeBoxRect,
                        minimizeBoxRect,
                        maximizeBoxRect,
                        closeBoxVisibale,
                        minimizeBoxVisibale,
                        maximizeBoxVisibale);
                    break;
                case MouseOperationType.Leave:
                    ProcessMouseLeave(
                        closeBoxVisibale,
                        minimizeBoxVisibale,
                        maximizeBoxVisibale);
                    break;
                case MouseOperationType.Hover:
                    break;
            }
        }

        private void ProcessMouseMove(
            Point mousePoint,
            Rectangle closeBoxRect,
            Rectangle minimizeBoxRect,
            Rectangle maximizeBoxRect,
            bool closeBoxVisibale,
            bool minimizeBoxVisibale,
            bool maximizeBoxVisibale) {
            string toolTip = string.Empty;
            bool hide = true;

            if (closeBoxVisibale) {
                if (closeBoxRect.Contains(mousePoint)) {
                    hide = false;
                    if (!_mouseDown) {
                        if (CloseBoxState != ControlBoxState.Hover) {
                            toolTip = "关闭";
                        }
                        CloseBoxState = ControlBoxState.Hover;
                    } else {
                        if (CloseBoxState == ControlBoxState.PressedLeave) {
                            CloseBoxState = ControlBoxState.Pressed;
                        }
                    }
                } else {
                    if (!_mouseDown) {
                        CloseBoxState = ControlBoxState.Normal;
                    } else {
                        if (CloseBoxState == ControlBoxState.Pressed) {
                            CloseBoxState = ControlBoxState.PressedLeave;
                        }
                    }
                }
            }

            if (minimizeBoxVisibale) {
                if (minimizeBoxRect.Contains(mousePoint)) {
                    hide = false;
                    if (!_mouseDown) {
                        if (MinimizeBoxState != ControlBoxState.Hover) {
                            toolTip = "最小化";
                        }
                        MinimizeBoxState = ControlBoxState.Hover;
                    } else {
                        if (MinimizeBoxState == ControlBoxState.PressedLeave) {
                            MinimizeBoxState = ControlBoxState.Pressed;
                        }
                    }
                } else {
                    if (!_mouseDown) {
                        MinimizeBoxState = ControlBoxState.Normal;
                    } else {
                        if (MinimizeBoxState == ControlBoxState.Pressed) {
                            MinimizeBoxState = ControlBoxState.PressedLeave;
                        }
                    }
                }
            }

            if (maximizeBoxVisibale) {
                if (maximizeBoxRect.Contains(mousePoint)) {
                    hide = false;
                    if (!_mouseDown) {
                        if (MaximizeBoxState != ControlBoxState.Hover) {
                            bool maximize =
                                _owner.WindowState == FormWindowState.Maximized;
                            toolTip = maximize ? "还原" : "最大化";
                        }
                        MaximizeBoxState = ControlBoxState.Hover;
                    } else {
                        if (MaximizeBoxState == ControlBoxState.PressedLeave) {
                            MaximizeBoxState = ControlBoxState.Pressed;
                        }
                    }
                } else {
                    if (!_mouseDown) {
                        MaximizeBoxState = ControlBoxState.Normal;
                    } else {
                        if (MaximizeBoxState == ControlBoxState.Pressed) {
                            MaximizeBoxState = ControlBoxState.PressedLeave;
                        }
                    }
                }
            }
            //自定义系统按钮
            foreach (CmSysButton item in SysButtonItems) {
                if (item.Visibale) {
                    if (item.Bounds.Contains(mousePoint)) {
                        hide = false;
                        if (!_mouseDown) {
                            if (item.BoxState != ControlBoxState.Hover) {
                                toolTip = item.ToolTip;
                            }
                            item.BoxState = ControlBoxState.Hover;
                        } else {
                            if (item.BoxState == ControlBoxState.PressedLeave) {
                                item.BoxState = ControlBoxState.Pressed;
                            }
                        }
                    } else {
                        if (!_mouseDown) {
                            item.BoxState = ControlBoxState.Normal;
                        } else {
                            if (item.BoxState == ControlBoxState.Pressed) {
                                item.BoxState = ControlBoxState.PressedLeave;
                            }
                        }
                    }
                }
            }

            if (toolTip != string.Empty) {
                HideToolTip();
                ShowTooTip(toolTip);
            }

            if (hide) {
                HideToolTip();
            }
        }

        private void ProcessMouseDown(
            Point mousePoint,
            Rectangle closeBoxRect,
            Rectangle minimizeBoxRect,
            Rectangle maximizeBoxRect,
            bool closeBoxVisibale,
            bool minimizeBoxVisibale,
            bool maximizeBoxVisibale) {
            _mouseDown = true;

            if (closeBoxVisibale) {
                if (closeBoxRect.Contains(mousePoint)) {
                    CloseBoxState = ControlBoxState.Pressed;
                    return;
                }
            }

            if (minimizeBoxVisibale) {
                if (minimizeBoxRect.Contains(mousePoint)) {
                    MinimizeBoxState = ControlBoxState.Pressed;
                    return;
                }
            }

            foreach (CmSysButton item in SysButtonItems) {
                if (item.Visibale) {
                    if (item.Bounds.Contains(mousePoint)) {
                        item.BoxState = ControlBoxState.Pressed;
                        return;
                    }
                }
            }

            if (maximizeBoxVisibale) {
                if (maximizeBoxRect.Contains(mousePoint)) {
                    MaximizeBoxState = ControlBoxState.Pressed;
                    return;
                }
            }
        }

        private void ProcessMouseUP(
            Point mousePoint,
            Rectangle closeBoxRect,
            Rectangle minimizeBoxRect,
            Rectangle maximizeBoxRect,
            bool closeBoxVisibale,
            bool minimizeBoxVisibale,
            bool maximizeBoxVisibale) {
            _mouseDown = false;

            if (closeBoxVisibale) {
                if (closeBoxRect.Contains(mousePoint)) {
                    if (CloseBoxState == ControlBoxState.Pressed) {
                        _owner.Close();
                        CloseBoxState = ControlBoxState.Normal;
                        return;
                    }
                }
                CloseBoxState = ControlBoxState.Normal;
            }

            if (minimizeBoxVisibale) {
                if (minimizeBoxRect.Contains(mousePoint)) {
                    if (MinimizeBoxState == ControlBoxState.Pressed) {
                        if (_owner.ShowInTaskbar) {
                            _owner.WindowState = FormWindowState.Minimized;
                        } else {
                            _owner.Hide();
                        }
                        MinimizeBoxState = ControlBoxState.Normal;
                        return;
                    }
                }
                MinimizeBoxState = ControlBoxState.Normal;
            }

            foreach (CmSysButton item in SysButtonItems) {
                if (item.Visibale) {
                    if (item.Bounds.Contains(mousePoint)) {
                        if (item.BoxState == ControlBoxState.Pressed) {
                            //调用事件
                            _owner.SysbottomAv(_owner, new SysButtonEventArgs(item));
                            item.BoxState = ControlBoxState.Normal;
                            return;
                        }
                    }
                    item.BoxState = ControlBoxState.Normal;
                }
            }

            if (maximizeBoxVisibale) {
                if (maximizeBoxRect.Contains(mousePoint)) {
                    if (MaximizeBoxState == ControlBoxState.Pressed) {
                        bool maximize =
                            _owner.WindowState == FormWindowState.Maximized;
                        if (maximize) {
                            _owner.WindowState = FormWindowState.Normal;
                        } else {
                            _owner.WindowState = FormWindowState.Maximized;
                        }

                        MaximizeBoxState = ControlBoxState.Normal;
                        return;
                    }
                }
                MaximizeBoxState = ControlBoxState.Normal;
            }
        }

        private void ProcessMouseLeave(
            bool closeBoxVisibale,
            bool minimizeBoxVisibale,
            bool maximizeBoxVisibale) {
            if (closeBoxVisibale) {
                if (CloseBoxState == ControlBoxState.Pressed) {
                    CloseBoxState = ControlBoxState.PressedLeave;
                } else {
                    CloseBoxState = ControlBoxState.Normal;
                }
            }

            if (minimizeBoxVisibale) {
                if (MinimizeBoxState == ControlBoxState.Pressed) {
                    MinimizeBoxState = ControlBoxState.PressedLeave;
                } else {
                    MinimizeBoxState = ControlBoxState.Normal;
                }
            }

            foreach (CmSysButton item in SysButtonItems) {
                if (item.Visibale) {
                    if (item.BoxState == ControlBoxState.Pressed) {
                        item.BoxState = ControlBoxState.PressedLeave;
                    } else {
                        item.BoxState = ControlBoxState.Normal;
                    }
                }
            }

            if (maximizeBoxVisibale) {
                if (MaximizeBoxState == ControlBoxState.Pressed) {
                    MaximizeBoxState = ControlBoxState.PressedLeave;
                } else {
                    MaximizeBoxState = ControlBoxState.Normal;
                }
            }

            HideToolTip();
        }

        private void Invalidate(Rectangle rect) {
            _owner.Invalidate(rect);
        }

        private void ShowTooTip(string toolTipText) {
            if (_owner != null) {
                _owner.ToolTip.Active = true;
                _owner.ToolTip.SetToolTip(_owner, toolTipText);
            }
        }

        private void HideToolTip() {
            if (_owner != null) {
                _owner.ToolTip.Active = false;
            }
        }

        #region 资源释放
        //是否回收完毕
        bool _disposed;
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~ControlBoxManager() {
            Dispose(false);
        }

        //这里的参数表示示是否需要释放那些实现IDisposable接口的托管对象
        protected virtual void Dispose(bool disposing) {
            if (_disposed) return; //如果已经被回收，就中断执行
            if (disposing) {
                //TODO:释放那些实现IDisposable接口的托管对象
                if (SysButtonItems.Count > 0) {
                    foreach (CmSysButton item in SysButtonItems) {
                        item.Dispose();
                    }
                }
                _owner = null;
            }
            //TODO:释放非托管资源，设置对象为null
            _disposed = true;
        }
        #endregion
    }
}
