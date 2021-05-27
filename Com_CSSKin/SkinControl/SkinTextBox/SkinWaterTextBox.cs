
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using Com_CSSkin.SkinClass;
using Com_CSSkin.Win32;
using Com_CSSkin.Win32.Const;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(TextBox))]
    public class SkinWaterTextBox : TextBox
    {
        #region 变量
        /// <summary>
        /// 水印文字
        /// </summary>
        private string _waterText = string.Empty;
        /// <summary>
        /// 水印文字的颜色
        /// </summary>
        private Color _waterColor = Color.FromArgb(127, 127, 127);
        #endregion

        #region 属性
        /// <summary>
        /// 
        /// </summary>
        [Description("水印文字"), Category("Skin")]
        public string WaterText {
            get { return this._waterText; }
            set {
                this._waterText = value;
                base.Invalidate();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("水印的颜色"), Category("Skin")]
        public Color WaterColor {
            get { return this._waterColor; }
            set {
                this._waterColor = value;
                base.Invalidate();
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 绘制水印
        /// </summary>
        private void WmPaintWater(ref Message m) {
            using (Graphics g = Graphics.FromHwnd(base.Handle)) {
                //最高质量绘制文字
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                if (this.Text.Length == 0 &&
                    !string.IsNullOrEmpty(this._waterText) &&
                    !this.Focused) {
                    TextFormatFlags flags = TextFormatFlags.EndEllipsis | (Multiline ? TextFormatFlags.Default : TextFormatFlags.VerticalCenter);
                    if (this.RightToLeft == System.Windows.Forms.RightToLeft.Yes) {
                        flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;
                    }
                    TextRenderer.DrawText(g, this._waterText, new Font("微软雅黑", 8.5f), this.ClientRectangle, this._waterColor, flags);
                }
            }
        }
        #endregion

        #region 重载事件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);
            if (m.Msg == WM.WM_PAINT || m.Msg == WM.WM_CTLCOLOREDIT)
                this.WmPaintWater(ref m);//绘制水印
        }

        ////控件首次创建时被调用
        //protected override void OnCreateControl() {
        //    ScrollBarHelper _ScrollBarHelper = new ScrollBarHelper(
        //       Handle,
        //       Properties.Resources.vista_ScrollHorzShaft,
        //       Properties.Resources.vista_ScrollHorzArrow,
        //       Properties.Resources.vista_ScrollHorzThumb,
        //       Properties.Resources.vista_ScrollVertShaft,
        //       Properties.Resources.vista_ScrollVertArrow,
        //       Properties.Resources.vista_ScrollVertThumb,
        //       Properties.Resources.vista_ScrollHorzArrow
        //       );
        //    base.OnCreateControl();
        //}
        #endregion

        #region 滚动条
        ScrollBarHelper _ScrollBarHelper;
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (!DesignMode) {
                _ScrollBarHelper = new ScrollBarHelper(
                  Handle,
                  ScrollBarDrawImage.ScrollHorzShaft,
                  ScrollBarDrawImage.ScrollHorzArrow,
                  ScrollBarDrawImage.ScrollHorzThumb,
                  ScrollBarDrawImage.ScrollVertShaft,
                  ScrollBarDrawImage.ScrollVertArrow,
                  ScrollBarDrawImage.ScrollVertThumb,
                  ScrollBarDrawImage.ScrollHorzArrow
                  );
            }
        }
        protected override void OnHandleDestroyed(EventArgs e) {
            base.OnHandleDestroyed(e);
            if (!DesignMode) {
                try {
                    if (_ScrollBarHelper != null) {
                        _ScrollBarHelper.Dispose();
                    }
                } catch { }
            }
        }
        #endregion
    }
}
