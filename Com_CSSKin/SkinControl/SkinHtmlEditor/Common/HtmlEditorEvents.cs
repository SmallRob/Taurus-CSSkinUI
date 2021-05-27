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
 * * 说明：HtmlEditorEvents.cs
 * *
********************************************************************/

#region Using directives

using System;

#endregion

namespace Com_CSSkin.SkinControl
{
    # region Application delegate definitions

    // Define delegate for raising an editor exception
    public delegate void HtmlExceptionEventHandler(object sender, HtmlExceptionEventArgs e);

    // Define delegate for handling navigation events
    public delegate void HtmlNavigationEventHandler(object sender, HtmlNavigationEventArgs e);

    // delegate declarations required for the find and replace dialog
    internal delegate void FindReplaceResetDelegate();
    internal delegate bool FindFirstDelegate(string findText, bool matchWhole, bool matchCase);
    internal delegate bool FindNextDelegate(string findText, bool matchWhole, bool matchCase);
    internal delegate bool FindReplaceOneDelegate(string findText, string replaceText, bool matchWhole, bool matchCase);
    internal delegate int  FindReplaceAllDelegate(string findText, string replaceText, bool matchWhole, bool matchCase);

    #endregion

    #region Navigation Event Arguments

    /// <summary>
    /// On a user initiated navigation create an event with the following EventArgs
    /// User can set the cancel property to cancel the navigation
    /// </summary>
    public class HtmlNavigationEventArgs : EventArgs
    {
        //private variables
        private string _url = string.Empty;
        private bool _cancel = false;

        /// <summary>
        /// Constructor for event args
        /// </summary>
        public HtmlNavigationEventArgs(string url) : base()
        {
            _url = url;

        } //HtmlNavigationEventArgs

        /// <summary>
        /// Property for the Form Url
        /// </summary>
        public string Url
        {
            get
            {
                return _url;
            }

        } //Url

        /// <summary>
        /// Defintion of the cancel property
        /// Also allows a set operation
        /// </summary>
        public bool Cancel
        {
            get
            {
                return _cancel;
            }
            set
            {
                _cancel = value;
            }
        }

    } //HtmlNavigationEventArgs

    #endregion

    #region HtmlException defintion and Event Arguments

    /// <summary>
    /// Exception class for HtmlEditor
    /// </summary>
    public class HtmlEditorException : ApplicationException
    {
        private string _operationName;

        /// <summary>
        /// Property for the operation name
        /// </summary>
        public string Operation
        {
            get
            {
                return _operationName;
            }
            set
            {
                _operationName = value;
            }

        } //OperationName


        /// <summary>
        /// Default constructor
        /// </summary>
        public HtmlEditorException () : base()
        {
            _operationName = string.Empty;
        }
   
        /// <summary>
        /// Constructor accepting a single string message
        /// </summary>
        public HtmlEditorException (string message) : base(message)
        {
            _operationName = string.Empty;
        }
   
        /// <summary>
        /// Constructor accepting a string message and an inner exception
        /// </summary>
        public HtmlEditorException(string message, Exception inner) : base(message, inner)
        {
            _operationName = string.Empty;
        }

        /// <summary>
        /// Constructor accepting a single string message and an operation name
        /// </summary>
        public HtmlEditorException(string message, string operation) : base(message)
        {
            _operationName = operation;
        }

        /// <summary>
        /// Constructor accepting a string message an operation and an inner exception
        /// </summary>
        public HtmlEditorException(string message, string operation, Exception inner) : base(message, inner)
        {
            _operationName = operation;
        }

    } //HtmlEditorException


    /// <summary>
    /// Defintion of the Event Arguement for an Html Exception
    /// If capturing an exception internally throw an event with the following EventArgs
    /// </summary>
    public class HtmlExceptionEventArgs : EventArgs
    {
        //private variables
        private string _operation;
        private Exception _exception;

        /// <summary>
        /// Constructor for event args
        /// </summary>
        public HtmlExceptionEventArgs(string operation, Exception exception) : base()
        {
            _operation = operation;
            _exception = exception;

        } //HtmlEditorExceptionEventArgs

        /// <summary>
        /// Property for the operation name
        /// </summary>
        public string Operation
        {
            get
            {
                return _operation;
            }

        } //Operation

        /// <summary>
        /// Property for the Exception for which the event is being raised
        /// </summary>
        /// <value></value>
        public Exception ExceptionObject
        {
            get
            {
                return _exception;
            }

        } //ExceptionObject

    } //HtmlExceptionEventArgs

    #endregion
}
