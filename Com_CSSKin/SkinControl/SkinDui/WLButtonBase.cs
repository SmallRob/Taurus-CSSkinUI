
using Com_CSSkin.SkinClass;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    public abstract class WLButtonBase : WLControlBase
    {
        #region constructors

        public WLButtonBase(Control owner)
            : base(owner)
        {
        }

        #endregion

        #region public properties

        private bool _capture = false;
        private SkinButtonState _state = SkinButtonState.Normal;
        private string _text;

        /// <summary>
        /// 获取控件是否捕获了鼠标
        /// </summary>
        public bool Capture
        {
            get { return _capture; }
        }

        /// <summary>
        /// 获取或设置按钮的状态
        /// </summary>
        public SkinButtonState State
        {
            get { return _state; }
            set
            {
                if (value != _state)
                {
                    _state = value;
                    Invalidate(Bounds);
                }
            }
        }

        /// <summary>
        /// 获取或设置控件的文本
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    Invalidate(Bounds);
                }
            }
        }

        #endregion

        #region new protected methods

        protected virtual void OnClick(EventArgs e)
        {

        }

        #endregion

        #region override methods

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (Bounds.Contains(e.Location))
            {
                State = SkinButtonState.Pressed;
                _capture = true;
            }
            else
            {
                _capture = false;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (Bounds.Contains(e.Location))
            {
                if (State == SkinButtonState.Normal)
                {
                    // 没有在窗体其他地方按下按钮
                    if (!Owner.Capture)
                    {
                        State = SkinButtonState.Hover;
                    }
                }
                else if (State == SkinButtonState.PressLeave)
                {
                    State = SkinButtonState.Pressed;
                }

            }
            else
            {
                if (_capture)
                {
                    State = SkinButtonState.PressLeave;
                }
                else
                {
                    State = SkinButtonState.Normal;
                }
            }
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            State = SkinButtonState.Normal;
            _capture = false;
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (Bounds.Contains(e.Location))
            {
                State = SkinButtonState.Hover;
                if (_capture)
                    OnClick(EventArgs.Empty);
            }
            else
            {
                State = SkinButtonState.Normal;
            }
            _capture = false;
        }

        #endregion
    }
}
