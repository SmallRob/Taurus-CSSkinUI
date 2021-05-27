
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    public partial class SkinAnimatorImg : Panel
    {
        public SkinAnimatorImg() {
            InitializeComponent();
            //初始化
            Init();
        }
        #region 初始化
        public void Init() {
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.UserPaint, true);//自行绘制
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
        }
        #endregion

        #region 属性
        private bool stretch = false;
        [Category("Skin")]
        [Description("是否将帧图像拉伸绘制。")]
        [DefaultValue(false)]
        public bool Stretch {
            get { return stretch; }
            set {
                stretch = value;
                this.Invalidate();
            }
        }

        [Category("Skin")]
        [Description("当前帧。")]
        [DefaultValue(true)]
        public int OneFrame {
            get { return i + 1; }
            set {
                i = value > FrameNumber ? FrameNumber - 1 : value < 1 ? 1 : value - 1;
                this.Invalidate();
            }
        }

        private bool animatorStart = true;
        [Category("Skin")]
        [Description("是否开启动画效果。")]
        [DefaultValue(true)]
        public bool AnimatorStart {
            get { return animatorStart; }
            set {
                timStart.Enabled = animatorStart = value;
                if (!value) {
                    timShow.Enabled = false;
                    i = 0;
                    this.Invalidate();
                }
            }
        }

        private int frameNumber = 4;
        [Category("Skin")]
        [Description("帧数。")]
        [DefaultValue(4)]
        public int FrameNumber {
            get { return frameNumber; }
            set {
                frameNumber = value < 1 ? 1 : value;
                this.Invalidate();
            }
        }

        private bool yAxis = false;
        [Category("Skin")]
        [Description("是否为Y轴切割。")]
        [DefaultValue(false)]
        public bool YAxis {
            get { return yAxis; }
            set {
                yAxis = value;
                this.Invalidate();
            }
        }

        [Category("Skin")]
        [Description("帧速率(毫秒)。")]
        [DefaultValue(100)]
        public int FrameRate {
            get { return timShow.Interval; }
            set { timShow.Interval = value; }
        }

        [Category("Skin")]
        [Description("动画结束后再执行的时间(毫秒)。")]
        [DefaultValue(100)]
        public int StartTime {
            get { return timStart.Interval; }
            set { timStart.Interval = value; }
        }

        private Image image;
        [Category("Skin")]
        [Description("多帧图像。")]
        public Image Image {
            get { return image; }
            set {
                image = value;
                this.Invalidate();
            }
        }
        #endregion

        #region 重载事件
        int i = 0;
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            if (Image == null) return;
            //原图
            Bitmap btm = (Bitmap)Image;
            //裁剪后图
            int mapw = YAxis ? btm.Width : btm.Width / FrameNumber;
            int maph = YAxis ? btm.Height / FrameNumber : btm.Height;
            Bitmap map = new Bitmap(mapw, maph);
            Graphics mapg = Graphics.FromImage(map);
            //设置高质量插值法
            mapg.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
            //设置高质量,低速度呈现平滑程度
            mapg.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            //清空画布并以透明背景色填充
            mapg.Clear(Color.Transparent);
            //裁剪
            int x = YAxis ? 0 : (flag ? i : 0) * mapw;
            int y = YAxis ? (flag ? i : 0) * maph : 0;
            int w = Stretch ? Width : mapw;
            int h = Stretch ? Height : maph;
            mapg.DrawImage(btm, new Rectangle(0, 0, w, h), new Rectangle(x, y, mapw, maph), GraphicsUnit.Pixel);
            g.DrawImage(map, 0, 0);
            mapg.Dispose();
        }
        #endregion

        #region 计时器事件
        //帧速率
        bool flag = false;
        private void timShow_Tick(object sender, EventArgs e) {
            timStart.Stop();
            flag = true;
            i++;
            if (i == FrameNumber) {
                i = 0;
                timShow.Stop();
                if (AnimatorStart) {
                    timStart.Start();
                }
            }
            this.Invalidate();
        }

        //一次动画时间
        private void timStart_Tick(object sender, EventArgs e) {
            //if (!DesignMode)
            {
                timShow.Start();
            }
        }
        #endregion
    }
}
