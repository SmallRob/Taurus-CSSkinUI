
using Com_CSSkin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Com_CSSkin
{
    public partial class Skin_Metro : CSSkinMain
    {
        public Skin_Metro() {
            InitializeComponent();
        }
        #region 属性
        private Color internalBorderColor = Color.FromArgb(100, 255, 255, 255);
        /// <summary>
        /// 内部边框颜色
        /// </summary>
        [Category("Metro")]
        [DefaultValue(typeof(Color), "100, 255, 255, 255")]
        [Description("内部边框颜色")]
        public Color InternalBorderColor {
            get { return internalBorderColor; }
            set {
                if (internalBorderColor != value) {
                    internalBorderColor = value;
                    this.Invalidate();
                }
            }
        }

        private Color internalBackColor = Color.FromArgb(240, 240, 240);
        /// <summary>
        /// 内部背景颜色
        /// </summary>
        [Category("Metro")]
        [DefaultValue(typeof(Color), "240, 240, 240")]
        [Description("内部背景颜色")]
        public Color InternalBackColor {
            get { return internalBackColor; }
            set {
                if (internalBackColor != value) {
                    internalBackColor = value;
                    this.Invalidate();
                }
            }
        }
        #endregion

        #region 重绘事件
        public override void SkinPaint(Graphics g, CSSkinMain f) {
            g.SmoothingMode = SmoothingMode.Default; //不消除锯齿
            Pen pen = new Pen(InternalBorderColor, 1);
            //画非Mdi状态渐变层
            if (!f.IsMdiContainer) {
                Brush brs = new SolidBrush(InternalBackColor);
                g.DrawRectangle(pen, 8 - 1, 30 - 1, f.Width - 8 * 2 + 1, f.Height - 30 - 8 + 1);
                g.FillRectangle(brs, 8, 30, f.Width - 8 * 2, f.Height - 30 - 8);
            } else {
                if (MdiBorderStyle == BorderStyle.None) {
                    //画Mdi状态渐变层
                    Brush brs = new SolidBrush(InternalBackColor);
                    g.DrawRectangle(pen, 8 - 1, 30 - 1, f.Width - 8 * 2 + 1, f.Height - 30 - 8 + 1);
                    g.FillRectangle(brs, 8, 30, f.Width - 8 * 2, f.Height - 30 - 8);
                }
            }
            base.SkinPaint(g, f);
        }
        #endregion
    }
}
