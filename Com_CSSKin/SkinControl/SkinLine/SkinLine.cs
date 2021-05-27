
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    public partial class SkinLine : Control
    {
        private int _LineHeight = 1;
        /// <summary>
        /// 线高度
        /// </summary>
        public int LineHeight
        {
            get { return _LineHeight; }
            set
            {
                _LineHeight = value;
                Height += value;
                this.Invalidate();
            }
        }

        private Color _LineColor = Color.Black;
        /// <summary>
        /// 前景色
        /// </summary>
        public Color LineColor
        {
            get
            {
                return _LineColor;
            }
            set
            {
                _LineColor = value;
                this.Invalidate();
            }
        }

        private Pen _linePen;
        private SolidBrush sb;

        public SkinLine()
        {
            SetStyles();
            //this.AutoScaleMode = AutoScaleMode.None;
            //this.BorderStyle = BorderStyle.None;
            this.Height = 1;
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            sb = new SolidBrush(_LineColor);
            _linePen = new Pen(sb, LineHeight);
            e.Graphics.DrawLine(_linePen, new Point(1, 1), new Point(Width - 1, 1));
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            this.Invalidate();
        }
        #region 减少闪烁方法
        private void SetStyles()
        {
            //设置自定义控件Style
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            UpdateStyles();
        }
        #endregion
    }
}
