
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    public partial class RichTxtControl : UserControl
    {
        public RichTxtControl() {
            InitializeComponent();
            //双缓冲
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.SupportsTransparentBackColor |
                ControlStyles.CacheText |
                ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque, false);
        }

        /// <summary>
        /// 子控件点击事件
        /// </summary>
        /// <param name="richTextBox">所在的richTextBox</param>
        /// <param name="newP">光标在控件上的位置</param>
        /// <param name="e"></param>
        public virtual void CtrlMouseDown(SkinChatRichTextBox richTextBox, Point newP, MouseEventArgs e) {

        }

        /// <summary>
        /// 子控件移动事件
        /// </summary>
        /// <param name="richTextBox">所在的richTextBox</param>
        /// <param name="newP">光标在控件上的位置</param>
        /// <param name="e"></param>
        public virtual void CtrlMouseMove(SkinChatRichTextBox richTextBox, Point newP, MouseEventArgs e) {

        }
    }
}
