
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Security.Permissions;

namespace Com_CSSkin
{
    public abstract class SkinFormRenderer
    {
        #region 委托
        private EventHandlerList _events;
        private static readonly object EventRenderSkinFormCaption = new object();
        private static readonly object EventRenderSkinFormBorder = new object();
        private static readonly object EventRenderSkinFormControlBox = new object();
        #endregion

        #region 无参构造

        protected SkinFormRenderer()
        {
        }

        #endregion

        #region 属性

        protected EventHandlerList Events
        {
            get
            {
                if (_events == null)
                {
                    _events = new EventHandlerList();
                }
                return _events;
            }
        }

        #endregion

        #region 事件
        public event SkinFormCaptionRenderEventHandler RenderSkinFormCaption
        {
            add { AddHandler(EventRenderSkinFormCaption, value); }
            remove { RemoveHandler(EventRenderSkinFormCaption, value); }
        }

        public event SkinFormBorderRenderEventHandler RenderSkinFormBorder
        {
            add { AddHandler(EventRenderSkinFormBorder, value); }
            remove { RemoveHandler(EventRenderSkinFormBorder, value); }
        }

        public event SkinFormControlBoxRenderEventHandler RenderSkinFormControlBox
        {
            add { AddHandler(EventRenderSkinFormControlBox, value); }
            remove { RemoveHandler(EventRenderSkinFormControlBox, value); }
        }
        #endregion

        #region 绘画方法

        public abstract Region CreateRegion(CSSkinMain form);

        public abstract void InitSkinForm(CSSkinMain  form);

        public void DrawSkinFormCaption(
            SkinFormCaptionRenderEventArgs e)
        {
            OnRenderSkinFormCaption(e);
            SkinFormCaptionRenderEventHandler handle =
                Events[EventRenderSkinFormCaption]
                as SkinFormCaptionRenderEventHandler;
            if (handle != null)
            {
                handle(this, e);
            }
        }


        public void DrawSkinFormBorder(
            SkinFormBorderRenderEventArgs e)
        {
            OnRenderSkinFormBorder(e);
            SkinFormBorderRenderEventHandler handle =
                Events[EventRenderSkinFormBorder]
                as SkinFormBorderRenderEventHandler;
            if (handle != null)
            {
                handle(this, e);
            }
        }

        public void DrawSkinFormControlBox(
            SkinFormControlBoxRenderEventArgs e)
        {
            OnRenderSkinFormControlBox(e);
            SkinFormControlBoxRenderEventHandler handle =
                Events[EventRenderSkinFormControlBox]
                as SkinFormControlBoxRenderEventHandler;
            if (handle != null)
            {
                handle(this, e);
            }
        }

        #endregion

        #region 抽象方法

        protected abstract void OnRenderSkinFormCaption(
            SkinFormCaptionRenderEventArgs e);

        protected abstract void OnRenderSkinFormBorder(
            SkinFormBorderRenderEventArgs e);

        protected abstract void OnRenderSkinFormControlBox(
            SkinFormControlBoxRenderEventArgs e);

        #endregion

        #region 添加与删除事件

        [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
        protected void AddHandler(object key, Delegate value)
        {
            Events.AddHandler(key, value);
        }

        [UIPermission(SecurityAction.Demand, Window = UIPermissionWindow.AllWindows)]
        protected void RemoveHandler(object key, Delegate value)
        {
            Events.RemoveHandler(key, value);
        }

        #endregion
    }
}
