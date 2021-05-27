
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections;
using System.Threading;

namespace Com_CSSkin
{
    public partial class FrmPrintscreen : CSSkinMain
    {
        public FrmPrintscreen() {
            InitializeComponent();
        }

        public FrmPrintscreen(Image Img) {
            InitializeComponent();
            this.BackgroundImage = Img;
        }

        List<Image> listImg;

        public List<Image> ListImg {
            get { return listImg; }
            set {
                if (value != listImg) {
                    listImg = value;
                    img = this.BackgroundImage = listImg[Index];
                }
            }
        }
        int index = 0;

        public int Index {
            get { return index; }
            set {
                if (value != index) {
                    index = value;
                    img = this.BackgroundImage = listImg[index];
                }
            }
        }
        public FrmPrintscreen(List<Image> ListImg, int SelectIndex) {
            InitializeComponent();
            if (ListImg != null) {
                this.listImg = ListImg;
                this.BackgroundImage = ListImg[SelectIndex];
                index = SelectIndex;
                this.btnLeftImg.Enabled = this.btnRightImg.Enabled = true;
            }
        }

        Image img;
        int W;
        int H;
        //窗体加载时
        protected override void OnLoad(EventArgs e) {
            img = this.BackgroundImage;
            Control.CheckForIllegalCrossThreadCalls = false;
            Thread threadInvalidate = new Thread(new ThreadStart(() => {
                while (true) {
                    Thread.Sleep(500);
                    btnTs.Visible = false;
                }
            }));
            threadInvalidate.IsBackground = true;
            threadInvalidate.Start();
            this.Size = img.Size;
            Rectangle ScreenArea = Screen.GetWorkingArea(this);
            W = ScreenArea.Width;
            H = ScreenArea.Height;
            this.Location = new Point((W - Width) / 2, (H - Height) / 2);
            base.OnLoad(e);
        }

        //鼠标滚动时
        protected override void OnMouseWheel(MouseEventArgs e) {
            btnTs.Visible = true;
            Size s = img.Size;
            double a = e.Delta / 12;
            int b = int.Parse(btnTs.Text.Replace("%", ""));
            double Bl = b + a;
            if (Bl >= 10 && Bl <= 500) {
                btnTs.Text = Bl + "%";
                Bitmap btm = new Bitmap(img, Convert.ToInt32(s.Width * (Bl / 100)), Convert.ToInt32(s.Height * (Bl / 100)));
                this.BackgroundImage = btm;
                base.OnMouseWheel(e);
            }
        }

        //最大化改变窗体关闭按钮
        protected override void OnSizeChanged(EventArgs e) {
            if (WindowState == FormWindowState.Maximized) {
                CloseBoxSize = new Size(42, 42);
                ControlBoxOffset = new Point(2, 2);
                CloseDownBack = Com_CSSkin.Properties.Resources.Full_close_down;
                CloseMouseBack = Com_CSSkin.Properties.Resources.Full_close_hover;
                CloseNormlBack = Com_CSSkin.Properties.Resources.Full_close_normal;
            } else {
                CloseBoxSize = new Size(25, 25);
                ControlBoxOffset = new Point(4, 4);
                CloseDownBack = Com_CSSkin.Properties.Resources.icon_close_down;
                CloseMouseBack = Com_CSSkin.Properties.Resources.icon_close_hover;
                CloseNormlBack = Com_CSSkin.Properties.Resources.icon_close_normal;
            }
            base.OnSizeChanged(e);
        }

        //窗体上移动时
        private void FrmPrintscreen_MouseMove(object sender, MouseEventArgs e) {
            btnLeftImg.Visible = e.X <= 100 ? true : false;
            btnRightImg.Visible = e.X >= Width - 100 ? true : false;
            pnlTools.Tag = e.Y <= 80 ? true : false;
            timShowTools.Start();

            Rectangle rc = new Rectangle(new Point((Width - this.BackgroundImage.Width) / 2, (Height - this.BackgroundImage.Height) / 2), this.BackgroundImage.Size);
            this.Cursor = rc.Contains(e.Location) ? Cursors.Hand : Cursors.Default;
        }

        //向左切换图片
        private void btnLeftImg_Click(object sender, EventArgs e) {
            index = index == 0 ? listImg.Count - 1 : index - 1;
            img = this.BackgroundImage = listImg[index];
        }

        //向右切换图片
        private void btnRightImg_Click(object sender, EventArgs e) {
            index = index == listImg.Count - 1 ? 0 : index + 1;
            img = this.BackgroundImage = listImg[index];
        }

        //图片区域外才可以拖动
        protected override void OnMouseDown(MouseEventArgs e) {
            //Rectangle rc = new Rectangle(new Point((Width - this.BackgroundImage.Width) / 2, (Height - this.BackgroundImage.Height) / 2), this.BackgroundImage.Size);
            //if (rc.Contains(e.Location) && !CaptionRect.Contains(e.Location))
            //{
            //    if (this.BackgroundImage.Width > this.Width || this.BackgroundImage.Height > this.Height)
            //        return;
            //}
            base.OnMouseDown(e);
        }

        //离开按钮时隐藏左右切换图片的按钮
        private void btnImg_MouseLeave(object sender, EventArgs e) {
            btnLeftImg.Visible = btnRightImg.Visible = false;
        }

        //计时器工具栏上下移动特效
        private void timShowTools_Tick(object sender, EventArgs e) {
            if ((bool)pnlTools.Tag) {
                if (pnlTools.Top < 1) {
                    pnlTools.Top += 4;
                } else {
                    pnlTools.Top = 1;
                    timShowTools.Stop();
                }
            } else {
                if (pnlTools.Top > -pnlTools.Height) {
                    pnlTools.Top -= 4;
                } else {
                    pnlTools.Top = -pnlTools.Height;
                    timShowTools.Stop();
                }
            }
        }

        //离开工具栏时
        private void ToolTools_MouseLeave(object sender, EventArgs e) {
            pnlTools.Tag = false;
            timShowTools.Start();
        }

        //查看全屏
        private void toolMaxMin_Click(object sender, EventArgs e) {
            WindowState = WindowState == FormWindowState.Normal ? FormWindowState.Maximized : FormWindowState.Normal;
            toolMaxMin.ToolTipText = WindowState == FormWindowState.Normal ? "查看全屏" : "取消全屏";
        }

        //查看原图比例
        private void toolOneToOne_Click(object sender, EventArgs e) {
            btnTs.Visible = true;
            btnTs.Text = "100%";
            Bitmap btm = new Bitmap(img);
            this.BackgroundImage = btm;
        }

        //向左旋转图片
        private void toolLeftXY_Click(object sender, EventArgs e) {
            btnTs.Text = "100%";
            img.RotateFlip(RotateFlipType.Rotate90FlipXY);
            this.BackgroundImage = img;
            this.Invalidate();
        }

        //向右旋转图片
        private void toolRightXY_Click(object sender, EventArgs e) {
            btnTs.Text = "100%";
            img.RotateFlip(RotateFlipType.Rotate90FlipNone);
            this.BackgroundImage = img;
            this.Invalidate();
        }

        //复制图片
        private void toolCopy_Click(object sender, EventArgs e) {
            Clipboard.SetImage(img);
        }

        //关闭释放
        protected override void OnClosing(CancelEventArgs e) {
            listImg.Clear();
            listImg = null;
            base.OnClosing(e);
        }

        //图片另存为
        private void toolSave_Click(object sender, EventArgs e) {
            Bitmap m_bmpLayerCurrent = (Bitmap)img;
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "图像文件(*.jpg)|*.jpg|图像文件(*.bmp)|*.bmp";
            //保存时获取当前时间字符串作文默认文件名
            saveDlg.FileName = DateTime.Now.ToString("CC图片yyyyMMddHHmmss");
            if (saveDlg.ShowDialog() == DialogResult.OK) {
                switch (saveDlg.FilterIndex) {
                    case 1:
                        m_bmpLayerCurrent.Save(saveDlg.FileName,
                            System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case 2:
                        m_bmpLayerCurrent.Clone(new Rectangle(0, 0, m_bmpLayerCurrent.Width, m_bmpLayerCurrent.Height),
                            System.Drawing.Imaging.PixelFormat.Format24bppRgb).Save(saveDlg.FileName,
                            System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                }
            }
        }
    }
}
