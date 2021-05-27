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
 * * 说明：ToolStripFontComboBox.cs
 * *
********************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    internal class ToolStripFontComboBox : ToolStripComboBox
    {
        #region  Private Member Declarations

        private readonly Dictionary<string, Font> _fontCache;
        private int _itemHeight;
        private int _previewFontSize;
        private StringFormat _stringFormat;

        #endregion  Private Member Declarations

        #region  Public Constructors

        public ToolStripFontComboBox()
        {
            _fontCache = new Dictionary<string, Font>();

            DrawMode = DrawMode.OwnerDrawVariable;
            if (!ComboBox.IsNull())
            {
                ComboBox.DrawItem += OnDrawItem;
                ComboBox.MeasureItem += OnMeasureItem;
            }
            Sorted = true;
            PreviewFontSize = 12;

            CalculateLayout();
            CreateStringFormat();
        }

        #endregion  Public Constructors

        #region  Events

        public event EventHandler PreviewFontSizeChanged;

        #endregion  Events

        #region  Protected Overridden Methods

        protected override void Dispose(bool disposing)
        {
            ClearFontCache();

            if (!_stringFormat.IsNull())
                _stringFormat.Dispose();

            base.Dispose(disposing);
        }

        protected void OnDrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index <= -1 || e.Index >= Items.Count) return;
            e.DrawBackground();

            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
                e.DrawFocusRectangle();

            using (var textBrush = new SolidBrush(e.ForeColor))
            {
                string fontFamilyName = Items[e.Index].ToString();
                e.Graphics.DrawString(fontFamilyName, GetFont(fontFamilyName), textBrush, e.Bounds, _stringFormat);
            }
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            _internalCall = false;
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            _internalCall = false;
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            CalculateLayout();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            LoadFontFamilies();

            base.OnGotFocus(e);
        }

        protected void OnMeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index > -1 && e.Index < Items.Count)
            {
                e.ItemHeight = _itemHeight;
            }
        }

        protected override void OnRightToLeftChanged(EventArgs e)
        {
            base.OnRightToLeftChanged(e);

            CreateStringFormat();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            if (Items.Count == 0)
            {
                LoadFontFamilies();

                int selectedIndex = FindStringExact(Text);
                if (selectedIndex != -1)
                    SelectedIndex = selectedIndex;
            }
        }

        #endregion  Protected Overridden Methods

        #region  Public Methods

        public void LoadFontFamilies()
        {
            if (Items.Count == 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                foreach (FontFamily fontFamily in FontFamily.Families)
                    Items.Add(fontFamily.Name);

                Cursor.Current = Cursors.Default;
            }
        }

        #endregion  Public Methods

        #region  Public Properties

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public DrawMode DrawMode
        {
            get
            {
                if (!ComboBox.IsNull()) return ComboBox.DrawMode;
                return DrawMode.Normal;
            }
            set { if (!ComboBox.IsNull()) ComboBox.DrawMode = value; }
        }

        [Category("Appearance"), DefaultValue(12)]
        public int PreviewFontSize
        {
            get { return _previewFontSize; }
            set
            {
                _previewFontSize = value;

                OnPreviewFontSizeChanged(EventArgs.Empty);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Never)]
        public new bool Sorted
        {
            get { return base.Sorted; }
            set { base.Sorted = value; }
        }

        private bool _internalCall;

        public bool InternalCall
        {
            get { return _internalCall; }
            set { _internalCall = value; }
        }

        private int _count;
        private bool _gotit;

        public string SelectedFontNameItem
        {
            get
            {
                if (SelectedIndex > -1)
                    return ((Font)SelectedItem).Name;
                return string.Empty;
            }

            set
            {
                _gotit = false;
                _count = 0;
                foreach (object item in Items)
                {
                    if (((Font)item).Name == value)
                    {
                        _gotit = true;
                        break;
                    }
                    _count++;
                }
                if (_gotit)
                {
                    _internalCall = true;
                    SelectedIndex = _count;
                }
            }
        }

        public Font SelectedFontItem
        {
            get
            {
                if (SelectedIndex > -1)
                    return (Font)SelectedItem;
                return null;
            }

            set
            {
                _gotit = false;
                _count = 0;
                foreach (object item in Items)
                {
                    if (((Font)item).Name == value.Name)
                    {
                        _gotit = true;
                        break;
                    }
                    _count++;
                }
                if (_gotit)
                {
                    _internalCall = true;
                    SelectedIndex = _count;
                }
            }
        }

        #endregion  Public Properties

        #region  Private Methods

        private void CalculateLayout()
        {
            ClearFontCache();

            using (var font = new Font(Font.FontFamily, PreviewFontSize))
            {
                Size textSize = TextRenderer.MeasureText("yY", font);
                _itemHeight = textSize.Height + 2;
            }
        }

        private bool IsUsingRTL(Control control)
        {
            bool result;

            if (control.RightToLeft == RightToLeft.Yes)
                result = true;
            else if (control.RightToLeft == RightToLeft.Inherit && !control.Parent.IsNull())
                result = IsUsingRTL(control.Parent);
            else
                result = false;

            return result;
        }

        #endregion  Private Methods

        #region  Protected Methods

        private void ClearFontCache()
        {
            if (!_fontCache.IsNull())
            {
                foreach (string key in _fontCache.Keys)
                    _fontCache[key].Dispose();
                _fontCache.Clear();
            }
        }

        private void CreateStringFormat()
        {
            if (!_stringFormat.IsNull())
                _stringFormat.Dispose();

            _stringFormat = new StringFormat(StringFormatFlags.NoWrap)
            {
                Trimming = StringTrimming.EllipsisCharacter,
                HotkeyPrefix = HotkeyPrefix.None,
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };

            if (IsUsingRTL(ComboBox))
                _stringFormat.FormatFlags |= StringFormatFlags.DirectionRightToLeft;
        }

        private Font GetFont(string fontFamilyName)
        {
            lock (_fontCache)
            {
                if (!_fontCache.ContainsKey(fontFamilyName))
                {
                    Font font = (((GetFont(fontFamilyName, FontStyle.Regular) ??
                                   GetFont(fontFamilyName, FontStyle.Bold)) ??
                                  GetFont(fontFamilyName, FontStyle.Italic)) ??
                                 GetFont(fontFamilyName, FontStyle.Bold | FontStyle.Italic)) ??
                                (Font)Font.Clone();

                    _fontCache.Add(fontFamilyName, font);
                }
            }

            return _fontCache[fontFamilyName];
        }

        private Font GetFont(string fontFamilyName, FontStyle fontStyle)
        {
            Font font;

            try
            {
                font = new Font(fontFamilyName, PreviewFontSize, fontStyle);
            }
            catch
            {
                font = null;
            }

            return font;
        }

        private void OnPreviewFontSizeChanged(EventArgs e)
        {
            if (!PreviewFontSizeChanged.IsNull())
                PreviewFontSizeChanged(this, e);

            CalculateLayout();
        }

        #endregion  Protected Methods
    }
}
