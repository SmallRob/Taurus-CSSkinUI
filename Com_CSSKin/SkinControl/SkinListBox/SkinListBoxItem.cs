
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.ComponentModel;

namespace Com_CSSkin.SkinControl
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class SkinListBoxItem : IDisposable
    {
        #region 变量

        private string _text = "ListBoxExItem";
        private Image _image;
        private object _tag;

        #endregion

        #region 无参与带参构造
        public SkinListBoxItem()
        {
        }

        public SkinListBoxItem(string text)
            : this(text, null)
        {
        }

        public SkinListBoxItem(string text, Image image)
        {
            _text = text;
            _image = image;
        }

        #endregion

        #region 属性

        [DefaultValue("ImageComboBoxItem")]
        [Localizable(true)]
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        [DefaultValue(typeof(Image), "null")]
        public Image Image
        {
            get { return _image; }
            set { _image = value; }
        }

        [Bindable(true)]
        [Localizable(false)]
        [DefaultValue("")]
        [TypeConverter(typeof(StringConverter))]
        [DesignerSerializationVisibility(
            DesignerSerializationVisibility.Hidden)]
        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        #endregion

        #region 重载事件

        public override string ToString()
        {
            return _text;
        }

        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            _image = null;
            _tag = null;
        }

        #endregion
    }
}
