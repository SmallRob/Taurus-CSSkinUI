
using Com_CSSkin.SkinClass;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    public abstract class WLControlBase
    {
        #region private vars

        private Control _owner;

        private static readonly object EVENT_LOCATIONCHANGED;
        private static readonly object EVENT_SIZECHANGED;
        private static readonly object EVENT_ENABLEDCHANGED;
        private static readonly object EVENT_VISIBLECHANGED;

        private System.ComponentModel.EventHandlerList _events;

        #endregion

        #region private or protected proterties

        protected System.ComponentModel.EventHandlerList Events
        {
            get
            {
                if (_events == null)
                {
                    _events = new System.ComponentModel.EventHandlerList();
                }
                return _events;
            }
        }

        protected Control Owner
        {
            get { return _owner; }
        }

        #endregion

        #region constructors

        static WLControlBase()
        {
            EVENT_ENABLEDCHANGED = new object();
            EVENT_LOCATIONCHANGED = new object();
            EVENT_SIZECHANGED = new object();
            EVENT_VISIBLECHANGED = new object();
        }

        public WLControlBase(Control owner)
        {
            this._owner = owner;
        }

        #endregion

        #region public properties

        private Point _location;
        private Size _ctlSize;
        private bool _enabled = true;
        private bool _visible = true;
        private Rectangle _bounds;
        private bool _alwaysDrawBackground = true;
        private object _tag;

        public Point Location
        {
            get
            {
                return _location;
            }
            set
            {
                if (_location != value)
                {
                    _location = value;
                    _bounds = new Rectangle(_location, _ctlSize);
                    OnLocationChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// 获取或设置Winless控件的大小
        /// </summary>
        public Size CtlSize
        {
            get
            {
                return _ctlSize;
            }
            set
            {
                if (_ctlSize != value)
                {
                    _ctlSize = value;
                    _bounds = new Rectangle(_location, _ctlSize);
                    OnSizeChanged(EventArgs.Empty);
                }
            }
        }

        public bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    OnEnabledChanged(EventArgs.Empty);
                }
            }
        }

        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                if (_visible != value)
                {
                    _visible = value;
                    OnVisibleChanged(EventArgs.Empty);
                }
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return _bounds;
            }
            set
            {
                if (_bounds != value)
                {
                    Location = value.Location;
                    CtlSize = value.Size;
                }
            }
        }

        public bool AlwaysDrawBackground
        {
            get
            {
                return _alwaysDrawBackground;
            }
            set
            {
                _alwaysDrawBackground = value;
            }
        }

        /// <summary>
        /// 获取或设置控件的附加数据
        /// </summary>
        public object Tag
        {
            get
            {
                return _tag;
            }
            set
            {
                _tag = value;
            }
        }

        // readonly properties

        /// <summary>
        /// 获取Winless控件的位置的X坐标
        /// </summary>
        public int CtlLeft
        {
            get { return Location.X; }
        }

        /// <summary>
        /// 获取Winless控件的位置的Y坐标
        /// </summary>
        public int CtlTop
        {
            get { return Location.Y; }
        }

        #endregion

        #region protected Onxx methods

        #region properties changed

        protected virtual void OnLocationChanged(EventArgs e)
        {

        }

        protected virtual void OnSizeChanged(EventArgs e)
        {

        }

        protected virtual void OnEnabledChanged(EventArgs e)
        {

        }

        protected virtual void OnVisibleChanged(EventArgs e)
        {

        }

        #endregion

        #region mouse operation

        protected virtual void OnMouseMove(MouseEventArgs e)
        {

        }

        protected virtual void OnMouseDown(MouseEventArgs e)
        {

        }

        protected virtual void OnMouseUp(MouseEventArgs e)
        {

        }

        protected virtual void OnMouseLeave(MouseEventArgs e)
        {

        }

        protected virtual void OnMouseWheel(MouseEventArgs e)
        {

        }

        #endregion

        #region key operation

        protected virtual void OnKeyDown(KeyEventArgs e)
        {
        }

        protected virtual void OnKeyUp(KeyEventArgs e)
        {
        }

        #endregion

        #region paint

        protected virtual void OnPaintBackground(Graphics g, Rectangle clipRect)
        {
        }

        protected virtual void OnPaintContent(Graphics g, Rectangle clipRect)
        {
        }

        protected virtual void OnPaintBorder(Graphics g, Rectangle clipRect)
        {
        }

        #endregion

        #endregion

        #region public methods

        public void Invalidate(Rectangle clipRect)
        {
            this._owner.Invalidate(clipRect);
        }

        public void Invalidate()
        {
            this._owner.Invalidate(Bounds);
        }

        public void PaintControl(Graphics g, Rectangle clipRect)
        {
            if (!Visible)
                return;

            if(AlwaysDrawBackground || RectHelper.EqualOrFullyContains(clipRect,Bounds))
                OnPaintBackground(g,clipRect);

            OnPaintContent(g, clipRect);

            if (AlwaysDrawBackground || RectHelper.EqualOrFullyContains(clipRect, Bounds))
                OnPaintBorder(g, clipRect);
        }

        public void PaintControl(Graphics g)
        {
            PaintControl(g, Bounds);
        }

        public void MouseOperation(MouseEventArgs e, MouseOperationType type)
        {
            if (!Enabled || !Visible)
                return;
            switch (type)
            {
                case MouseOperationType.Move:
                    OnMouseMove(e);
                    break;
                case MouseOperationType.Down:
                    OnMouseDown(e);
                    break;
                case MouseOperationType.Up:
                    OnMouseUp(e);
                    break;
                case MouseOperationType.Leave:
                    OnMouseLeave(e);
                    break;
                case MouseOperationType.Wheel:
                    OnMouseWheel(e);
                    break;
            }
        }

        public void MouseOperation(Point location, MouseOperationType type)
        {
            MouseOperation(new MouseEventArgs(MouseButtons.None, 0, location.X, location.Y, 0), type);                
        }

        public void KeyOperation(KeyEventArgs e, KeyOperationType type)
        {
            switch (type)
            {
                case KeyOperationType.KeyDown:
                    OnKeyDown(e);
                    break;
                case KeyOperationType.KeyUp:
                    OnKeyUp(e);
                    break;
            }
        }

        #endregion
    }
}
