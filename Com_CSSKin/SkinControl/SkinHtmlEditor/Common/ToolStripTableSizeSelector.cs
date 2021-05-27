/********************************************************************
 * *
 * * 使本项目源码或本项目生成的DLL前请仔细阅读以下协议内容，如果你同意以下协议才能使用本项目所有的功能，
 * * 否则如果你违反了以下协议，有可能陷入法律纠纷和赔偿，作者保留追究法律责任的权利。
 * *
 * * 1、你可以在开发的软件产品中使用和修改本项目的源码和DLL，但是请保留所有相关的版权信息。
 * * 2、不能将本项目源码与作者的其他项目整合作为一个单独的软件售卖给他人使用。
 * * 3、不能传播本项目的源码和DLL，包括上传到网上、拷贝给他人等方式。
 * * 4、以上协议暂时定制，由于还不完善，作者保留以后修改协议的权利。
 * *
 * * Copyright (C) 2013-? cskin Corporation All rights reserved.
 * * 网站：CSkin界面库 http://www.cskin.net
 * * 作者： 乔克斯 QQ：345015918 .Net项目技术组群：306485590
 * * 请保留以上版权信息，否则作者将保留追究法律责任。
 * *
 * * 创建时间：2016-01-18
 * * 说明：ToolStripTableSizeSelector.cs
 * *
********************************************************************/

using System;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    class ToolStripTableSizeSelector : ToolStripDropDown
    {
        public ToolStripTableSizeSelector()
        {
            Items.Add(new ToolStripControlHost(control));

            control.TableSizeSelected += new TableSizeSelectedEventHandler(control_TableSizeSelected);
            control.SelectionCancelled += new EventHandler(control_SelectionCancelled);
        }

        public TableSizeControl Selector
        {
            get { return this.control; }
        }

        private void control_SelectionCancelled(object sender, EventArgs e)
        {
            this.Close(ToolStripDropDownCloseReason.CloseCalled);
        }

        private void control_TableSizeSelected(object sender, TableSizeEventArgs e)
        {
            this.Close(ToolStripDropDownCloseReason.CloseCalled);
        }

        protected override void OnOpening(System.ComponentModel.CancelEventArgs e)
        {
            base.OnOpening(e);

            ToolStripProfessionalRenderer renderer = Renderer as ToolStripProfessionalRenderer;

            if (!renderer.IsNull())
                control.BackColor = renderer.ColorTable.ToolStripDropDownBackground;

            //control.SelectedSize = new Size(0, 0);
            //control.VisibleRange = new Size(5, 4);
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            control.Focus();
        }

        private TableSizeControl control = new TableSizeControl();
    }
}
