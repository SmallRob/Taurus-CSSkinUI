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
 * * 说明：FindReplaceForm.cs
 * *
********************************************************************/

#region Using directives

using System.Windows.Forms;

#endregion

namespace Com_CSSkin.SkinControl
{

    /// <summary>
    /// Form designed to control Find and Replace operations
    /// Find and Replace operations performed by the user control class
    /// Delegates need to be defined to reference the class instances
    /// </summary> 
    internal partial class FindReplaceForm : Form
    {
        // constants defining the form sizes
        private const int FORM_HEIGHT_WITH_OPTION = 264;
        private const int TAB_HEIGHT_WITH_OPTION = 216;
        private const int FORM_HEIGHT_WITHOUT_OPTION = 200;
        private const int TAB_HEIGHT_WITHOUT_OPTION = 152;

        // private variables defining the state of the form
        private bool options = false;
        private bool findNotReplace  = true;
        private string findText;
        private string replaceText;

        // internal delegate reference
        private FindReplaceResetDelegate FindReplaceReset;
        private FindReplaceOneDelegate FindReplaceOne;
        private FindReplaceAllDelegate FindReplaceAll;
        private FindFirstDelegate FindFirst;
        private FindNextDelegate FindNext;


        /// <summary>
        /// Public constructor that defines the required delegates
        /// Delegates must be defined for the find and replace to operate
        /// </summary>
        public FindReplaceForm(string initText, FindReplaceResetDelegate resetDelegate, FindFirstDelegate findFirstDelegate, FindNextDelegate findNextDelegate, FindReplaceOneDelegate replaceOneDelegate, FindReplaceAllDelegate replaceAllDelegate, bool findOrReplace)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            // Define the initial state of the form assuming a Find command to be displayed first
            DefineFindWindow(findOrReplace);
            DefineOptionsWindow(options);

            // ensure buttons not initially enabled
            this.bFindNext.Enabled = false;
            this.bReplace.Enabled = false;
            this.bReplaceAll.Enabled = false;

            // save the delegates used to perform find and replcae operations
            this.FindReplaceReset = resetDelegate;
            this.FindFirst = findFirstDelegate;
            this.FindNext = findNextDelegate;
            this.FindReplaceOne = replaceOneDelegate;
            this.FindReplaceAll = replaceAllDelegate;

            // define the original text
            this.textFind.Text = initText;

        } //FindReplaceForm


        /// <summary>
        /// Setup the properties based on the find or repalce functionality
        /// </summary>
        private void DefineFindWindow(bool find)
        {
            this.textReplace.Visible = !find;
            this.labelReplace.Visible = !find;
            this.bReplace.Visible = !find;
            this.bReplaceAll.Visible = !find;
            this.textFind.Focus();
            this.tabControl.SelectedIndex = find ? 0 : 1;

        } //DefineFindWindow


        /// <summary>
        /// Defines if the options dialog is shown
        /// </summary>
        private void DefineOptionsWindow(bool options)
        {
            if (options)
            {
                // Form displayed with the options shown
                this.bOptions.Text = "较少选项";
                this.panelOptions.Visible = true;
                this.tabControl.Height = TAB_HEIGHT_WITH_OPTION;
                this.Height = FORM_HEIGHT_WITH_OPTION;
                this.optionMatchCase.Focus();
            }
            else
            {
                // Form displayed without the options shown
                this.bOptions.Text = "更多选项";
                this.panelOptions.Visible = false;
                this.tabControl.Height = TAB_HEIGHT_WITHOUT_OPTION;
                this.Height = FORM_HEIGHT_WITHOUT_OPTION;
                this.textFind.Focus();
            }

        } //DefineOptionsWindow


        /// <summary>
        /// Event defining the visibility of the options
        /// Based on the user clicking the options button
        /// </summary>
        private void bOptions_Click(object sender, System.EventArgs e)
        {
            options = !options;
            DefineOptionsWindow(options);

        } //OptionsClick


        /// <summary>
        /// Event setting the state of the form
        /// Based on the user clicking a new form tab
        /// </summary>
        private void tabControl_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (this.tabControl.SelectedIndex == 0)
            {
                findNotReplace = true;
            }
            else
            {
                findNotReplace = false;
            }
            DefineFindWindow(findNotReplace);
        
        } //SelectedIndexChanged


        /// <summary>
        /// Event replacing a single occurrence of a given text with another
        /// Based on the user clicking the replace button
        /// </summary>
        private void bReplace_Click(object sender, System.EventArgs e)
        {
            // find and replace the given text
            if (!this.FindReplaceOne(findText, replaceText, this.optionMatchWhole.Checked, this.optionMatchCase.Checked)) 
            {
                MessageBox.Show("所有的匹配项已被替换！", "查找和替换");
            }
        
        } //ReplaceClick


        /// <summary>
        /// Event replacing all the occurrences of a given text with another
        /// Based on the user clicking the replace all button
        /// </summary>
        private void bReplaceAll_Click(object sender, System.EventArgs e)
        {
            int found = this.FindReplaceAll(findText, replaceText, this.optionMatchWhole.Checked, this.optionMatchCase.Checked);

            // indicate the number of replaces found
            MessageBox.Show(string.Format("{0} 匹配项被替换", found), "查找和替换");
        
        } // ReplaceAllClick


        /// <summary>
        /// Event finding the next occurrences of a given text
        /// Based on the user clicking the find next button
        /// </summary>
        private void bFindNext_Click(object sender, System.EventArgs e)
        {
            // once find has completed indicate to the user success or failure
            if (!this.FindNext(findText, this.optionMatchWhole.Checked, this.optionMatchCase.Checked))
            {
                MessageBox.Show("没有找到匹配项!", "查找和替换");
            }
        
        } //FindNextClick


        /// <summary>
        /// Once the text has been changed reset the ranges to be worked with
        /// Initially defined by the set in the constructor
        /// </summary>
        private void textFind_TextChanged(object sender, System.EventArgs e)
        {
            ResetTextState();

        } //FindTextChanged


        /// <summary>
        /// Once the text has been changed reset the ranges to be worked with
        /// Initially defined by the set in the constructor
        /// </summary>
        private void textReplace_TextChanged(object sender, System.EventArgs e)
        {
            ResetTextState();

        } //TextChanged


        /// <summary>
        /// Sets the form state based on user input for Replace
        /// </summary>
        private void ResetTextState()
        {
            // reset the range being worked with
            this.FindReplaceReset();

            // determine the text values
            findText = this.textFind.Text.Trim();
            replaceText = this.textReplace.Text.Trim();

            // if no find text available disable find button
            if (findText != string.Empty)
            {
                this.bFindNext.Enabled = true;
            }
            else
            {
                this.bFindNext.Enabled = false;
            }

            // if no find text available disable replace button
            if (this.textFind.Text.Trim() != string.Empty && replaceText != string.Empty)
            {
                this.bReplace.Enabled = true;
                this.bReplaceAll.Enabled = true;
            }
            else
            {
                this.bReplace.Enabled = false;
                this.bReplaceAll.Enabled = false;
            }

        } //ResetTextReplace

    }
}
