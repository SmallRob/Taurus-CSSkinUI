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
 * * 说明：EditHtmlForm.cs
 * *
********************************************************************/

#region Using directives

using System.Windows.Forms;

#endregion

namespace Com_CSSkin.SkinControl
{

    /// <summary>
    /// Form used to Edit or View Html contents
    /// If a property RedOnly is true contents are considered viewable
    /// No Html parsing is performed on the resultant data
    /// </summary>
    internal partial class EditHtmlForm : Form
    {

        // read only property for the form
        private bool _readOnly;

        // string values for the form title
        private const string editCommand = "取消";
        private const string viewCommand = "关闭";

        /// <summary>
        /// Public Form constructor
        /// </summary>
        public EditHtmlForm()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // ensure content is empty
            this.htmlText.Text = string.Empty;
            this.htmlText.MaxLength = int.MaxValue;
            this.ReadOnly = true;

        } //EditHtmlForm

        /// <summary>
        /// Property to modify the caption of the display
        /// </summary>
        public void SetCaption(string caption)
        {
            this.Text = caption;
        }

        /// <summary>
        /// Property to set and get the HTML contents
        /// </summary>
        public string HTML
        {
            get
            {
                return this.htmlText.Text.Trim();
            }
            set
            {
                this.htmlText.Text = (!value.IsNull())?value.Trim():string.Empty;
                this.htmlText.SelectionStart = 0;
                this.htmlText.SelectionLength = 0;
            }

        } //HTML

        /// <summary>
        /// Property that determines if the html is editable
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return _readOnly;
            }
            set
            {
                _readOnly = value;
                this.bOK.Visible = !_readOnly;
                this.htmlText.ReadOnly = _readOnly;
                this.bCancel.Text = _readOnly?viewCommand:editCommand;
            }

        }//ReadOnly

        private void htmlText_KeyDown(object sender, KeyEventArgs e)
        {
            var txtBox = sender as TextBox;
            if (txtBox != null && txtBox.Multiline && e.Control && e.KeyCode == Keys.A)
            {
                txtBox.SelectAll();
                e.SuppressKeyPress = true;
            }
        } 

    }
}
