
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Com_CSSkin.SkinClass;
using Com_CSSkin.Win32;
using Com_CSSkin.Win32.Const;

namespace Com_CSSkin
{
    //控件层
    public partial class SkinMain : Form
    {
        //绘制层
        public SkinForm skin;
        public SkinMain() {
            InitializeComponent();
            //减少闪烁
            SetStyles();
            //初始化
            Init();
        }
        #region 初始化
        private void Init() {
            //不显示在Windows任务栏中
            ShowInTaskbar = false;
        }
        #endregion

        #region 减少闪烁
        private void SetStyles() {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);
            //强制分配样式重新应用到控件上
            UpdateStyles();
            base.AutoScaleMode = AutoScaleMode.None;
        }
        #endregion

        #region 变量属性
        private bool special = true;
        /// <summary>
        /// 是否启用窗口淡入淡出
        /// </summary>
        [Category("Skin")]
        [DefaultValue(true)]
        [Description("是否启用窗口淡入淡出")]
        public bool Special {
            get { return special; }
            set {
                if (special != value) {
                    special = value;
                }
            }
        }

        //不显示FormBorderStyle属性
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FormBorderStyle FormBorderStyle {
            get { return base.FormBorderStyle; }
            set { base.FormBorderStyle = FormBorderStyle.None; }
        }

        private Image skinback;
        [Category("Skin")]
        [Description("该窗体的背景图像")]
        public Image SkinBack {
            get { return skinback; }
            set {
                if (skinback != value) {
                    skinback = value;
                    if (value != null && show && !DesignMode) {
                        SkinTools.CreateControlRegion(this, TrankBack(), 255);
                    }
                    this.Invalidate();
                    if (skin != null) {
                        skin.BackgroundImage = TrankBack();
                    }
                }
            }
        }

        private Color _skintrankcolor = Color.Transparent;
        [Category("Skin")]
        [Description("背景需要透明的颜色")]
        [DefaultValue(typeof(Color), "Color.Transparent")]
        public Color SkinTrankColor {
            get { return _skintrankcolor; }
            set {
                if (_skintrankcolor != value) {
                    _skintrankcolor = value;
                    this.Invalidate();
                    if (skin != null) {
                        skin.BackgroundImage = TrankBack();
                    }
                }
            }
        }

        private bool _skinshowintaskbar = true;
        [Category("Skin")]
        [Description("绘图层是否出现在Windows任务栏中。")]
        [DefaultValue(typeof(bool), "true")]
        public bool SkinShowInTaskbar {
            get { return _skinshowintaskbar; }
            set {
                if (_skinshowintaskbar != value) {
                    _skinshowintaskbar = value;
                }
            }
        }

        private bool _skinmobile = true;
        [Category("Skin")]
        [Description("窗体是否可以移动")]
        [DefaultValue(typeof(bool), "true")]
        public bool SkinMobile {
            get { return _skinmobile; }
            set {
                if (_skinmobile != value) {
                    _skinmobile = value;
                }
            }
        }

        //获取窗体应用的背景
        public Bitmap TrankBack() {
            if (SkinTrankColor != Color.Transparent) {
                Bitmap bitmap = new Bitmap(this.SkinBack, this.Size);
                bitmap.MakeTransparent(SkinTrankColor);
                return bitmap;
            } else {
                return DesignMode ? (Bitmap)this.SkinBack : new Bitmap(this.SkinBack, this.Size);
            }
        }

        /// <summary>
        /// 设置窗体的不透明度
        /// </summary>
        //[Description("设置窗体的不透明度,0-1"), DefaultValue(1)]
        public new double Opacity {
            get { return base.Opacity; }
            set {
                if (Opacity != value) {
                    base.Opacity = value;
                    //层窗体存在，改变透明度
                    if (skin != null) {
                        skin.SetBits();
                    }
                }

            }
        }
        #endregion

        #region 重载事件
        //重绘时
        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            if (SkinBack != null) {
                if (DesignMode) {
                    g.DrawImage(TrankBack(), 0, 0, Width, Height);
                }
            }
            base.OnPaint(e);
        }

        //窗体关闭时
        bool isClose = true;
        protected override void OnClosing(CancelEventArgs e) {
            if (Owner != null) {
                e.Cancel = isClose;
                base.OnClosing(e);
                if (Special) {
                    this.timShow.Start();
                } else {
                    Owner.Close();
                }
            } else
                base.OnClosing(e);
        }

        //Visble值改变时
        bool show = false;
        double HcOp;
        protected override void OnVisibleChanged(EventArgs e) {
            if (!DesignMode) {
                if (skin != null) {
                    skin.Visible = this.Visible;
                } else {
                    //是否启用渐变
                    if (Special) {
                        HcOp = this.Opacity;
                        this.Opacity = 0;
                    }
                    //窗体异形
                    SkinTools.CreateControlRegion(this, TrankBack(), 255);
                    show = true;
                    skin = new SkinForm(this);
                    skin.Show();
                }
            }
            base.OnVisibleChanged(e);
        }

        //大小改变时
        protected override void OnSizeChanged(EventArgs e) {
            if (SkinBack != null && show) {
                SkinTools.CreateControlRegion(this, TrankBack(), 255);
                skin.Size = this.Size;
            }
            base.OnSizeChanged(e);
        }

        //点击移动
        protected override void OnMouseDown(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left && e.Clicks == 1) {
                if (SkinMobile) {
                    //释放鼠标焦点捕获
                    NativeMethods.ReleaseCapture();
                    //向当前窗体发送拖动消息
                    NativeMethods.SendMessage(skin.Handle, 0x0112, 0xF011, 0);
                    OnClick(e);
                    OnMouseClick(e);
                    OnMouseUp(e);
                }
            }
            base.OnMouseDown(e);
        }
        #endregion

        #region 渐变透明度
        bool shown = true;
        private void timShow_Tick(object sender, EventArgs e) {
            //是否启用渐变
            if (Special) {
                if (!DesignMode) {
                    //第一次显示
                    if (shown) {
                        if (this.Opacity < HcOp) {
                            this.Opacity += 0.1;
                            if (this.Opacity >= HcOp) {
                                this.Opacity = HcOp;
                            }
                        } else {
                            this.Opacity = HcOp;
                            timShow.Stop();
                            shown = false;
                        }
                    } else {
                        //第二次启用定时器是关闭窗体的时候
                        if (this.Opacity > 0) {
                            Opacity -= 0.1;
                        } else {
                            isClose = false;
                            if (null != Owner) {
                                Owner.Close();
                            }
                        }
                    }
                }
            } else {
                timShow.Stop();
            }
        }
        #endregion
    }
}
