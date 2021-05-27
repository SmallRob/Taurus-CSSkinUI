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
 * * 说明：ContextMenuForm.cs
 * *
********************************************************************/

using System;
using System.Drawing;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    /// <summary>
    /// Provides a System.Windows.Forms.Form that have a ContextMenu behavior.
    /// Use this Form by extending it or by adding the control using the method:
    /// <code>SetContainingControl(Control control)</code>
    /// </summary>
    public partial class ContextMenuForm : Form
    {
        private bool _locked;

        /// <summary>
        /// Gets or sets a value indicating that the form is locked.
        /// The form should be locked when opening a Dialog on it.
        /// </summary>
        public bool Locked
        {
            get { return _locked; }
            set 
            {
                _locked = value; 
            }
        }

        private Control _parentControl = null;

        /// <summary>
        /// Initialize a new instace of the ContextMenuForm in order to hold a control that
        /// needes to have a ContextMenu behavior.
        /// </summary>
        public ContextMenuForm()
        {
            InitializeComponent();
        }
    
        /// <summary>
        /// Shows the form on the specifies parent in the specifies location.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="startLocation"></param>
        /// <param name="width"></param>
        public void Show(Control parent, Point startLocation, int width)
        {
            _parentControl = parent;
            Point location = parent.PointToScreen(startLocation);
            this.Location = location;            
            this.Width = width;
            this.Show();
        }
        /// <summary>
        /// Set the control that will populate the ContextMenuForm.
        /// <remarks>
        /// Any scrolling should be implemented in the control it self, the 
        /// ContextMenuForm will not support scrolling.
        /// </remarks>
        /// </summary>
        /// <param name="control"></param>
        public void SetContainingControl(Control control)
        {
            panelMain.Controls.Clear();
            control.Dock = DockStyle.Fill;
            panelMain.Controls.Add(control);
        }

        private void ContextMenuPanel_Deactivate(object sender, EventArgs e)
        {
            if (!Locked)
            {
                Hide();                
            }
        }

        private void ContextMenuPanel_Leave(object sender, EventArgs e)
        {
            if (!Locked)
            {
                Hide();
            }
        }

        new public void Hide()
        {
            base.Hide();
        }
    }
}