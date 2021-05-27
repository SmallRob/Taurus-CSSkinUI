
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Com_CSSkin.SkinClass;
using Com_CSSkin.SkinControl;
using System.Runtime.InteropServices;
using Com_CSSkin.Win32.Const;
using Com_CSSkin.Win32;

namespace Com_CSSkin.SkinControl
{
    public partial class PassKey : Form
    {
        //需要操作的文本框
        private TextBox txt;
        private int X;
        private int Y;
        //位置显示有问题
        public PassKey(int x, int y, TextBox txt)
        {
            InitializeComponent();
            this.txt = txt;
            this.X = x;
            this.Y = y;
            //减少闪烁
            SetStyles();
        }

        #region 窗体打开特效 与 重绘
        protected override void OnPaint(PaintEventArgs e)
        {
            //设置圆角矩形窗体
            SkinTools.CreateRegion(this, 4);
            base.OnPaint(e);
        }
        #endregion

        #region 减少闪烁
        private void SetStyles()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);
            //强制分配样式重新应用到控件上
            UpdateStyles();
        }
        #endregion

        #region 窗体停用时
        private void PassKey_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 关闭
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 删除
        private void btnDelet_Click(object sender, EventArgs e)
        {
            if(txt.Text.Length > 0)
            {
                txt.Text = txt.Text.Substring(0, txt.Text.Length - 1);
            }
        }
        #endregion

        #region 追加文本
        private void btn_Click(object sender, EventArgs e)
        {
            SkinButton btn = (SkinButton)sender;
            txt.AppendText(btn.Text);
        }
        #endregion

        #region 大小写变换
        Image Norml;
        Image Mouse;
        bool ToUpper = false;
        bool flag = false;
        private void btnCapsLock_Click(object sender, EventArgs e)
        {
            ToUpper = !ToUpper;
            flag = !flag;
            foreach (SkinButton btn in this.Controls)
            {
                if (btn.Tag.ToString() == "2")
                {
                    btn.Text = ToUpper ? btn.Text.ToUpper() : btn.Text.ToLower();
                }
            }
            if (flag)
            {
                Norml = btnCapsLock.NormlBack;
                Mouse = btnCapsLock.MouseBack;
                btnCapsLock.NormlBack = btnCapsLock.DownBack;
                btnCapsLock.MouseBack = btnCapsLock.DownBack;
            }
            else 
            {
                btnCapsLock.NormlBack = Norml;
                btnCapsLock.MouseBack = Mouse;
            }
        }
        #endregion

        #region 切换键
        Image Norml2;
        Image Mouse2;
        bool flag2 = false;
        private void btnShift_Click(object sender, EventArgs e)
        {
            ToUpper = !ToUpper;
            flag2 = !flag2;
            //变换大小写
            foreach (SkinButton btn in this.Controls)
            {
                if (btn.Tag.ToString() == "2")
                {
                    btn.Text = ToUpper ? btn.Text.ToUpper() : btn.Text.ToLower();
                }
            }

            //变换数字与符号
            foreach (SkinButton btn in this.Controls)
            {
                if (btn.Tag.ToString().Substring(0, 1) == "1")
                {
                    btn.Text = flag2 ? btn.Tag.ToString().Substring(1, 1) : btn.Tag.ToString().Substring(2);
                }
            }
            if (flag2)
            {
                Norml2 = btnShift.NormlBack;
                Mouse2 = btnShift.MouseBack;
                btnShift.NormlBack = btnShift.DownBack;
                btnShift.MouseBack = btnShift.DownBack;
            }
            else
            {
                btnShift.NormlBack = Norml2;
                btnShift.MouseBack = Mouse2;
            }
        }
        #endregion

        #region 关闭窗体时
        private void PassKey_FormClosing(object sender, FormClosingEventArgs e)
        {
            //开始窗体动画
            NativeMethods.AnimateWindow(this.Handle, 0, AW.AW_BLEND + AW.AW_HIDE);
        }
        #endregion

        #region 窗体加载时
        private void PassKey_Load(object sender, EventArgs e)
        {
            this.Location = new Point(X, Y);
            //开始窗体动画
            NativeMethods.AnimateWindow(this.Handle, 0, AW.AW_SLIDE + AW.AW_BLEND);
        }
        #endregion
    }
}
