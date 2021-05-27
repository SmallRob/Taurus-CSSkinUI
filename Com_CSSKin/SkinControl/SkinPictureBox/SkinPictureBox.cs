
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(PictureBox))]
    public partial class SkinPictureBox : PictureBox
    {
        public SkinPictureBox()
        {
            InitializeComponent();
            //初始化
            Init();
            this.BackColor = System.Drawing.Color.Transparent;//背景设为透明
        }

        #region 初始化
        public void Init()
        {
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
        }
        #endregion
    }
}
