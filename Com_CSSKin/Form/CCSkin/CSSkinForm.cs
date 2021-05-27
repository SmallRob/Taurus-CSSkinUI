
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Threading;
using Com_CSSkin.SkinClass;
using Com_CSSkin.Win32.Const;
using Com_CSSkin.Win32.Struct;
using Com_CSSkin.Win32;
using System.Security.Permissions;

namespace Com_CSSkin
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    //绘图层
    public partial class CSSkinForm : Form
    {
        //控件层
        private CSSkinMain Main;
        //带参构造
        public CSSkinForm(CSSkinMain main) {
            //将控制层传值过来
            this.Main = main;
            InitializeComponent();
            //减少闪烁
            SetStyles();
            //初始化
            Init();
        }
        #region 初始化
        private void Init() {
            //置顶窗体
            Main.TopMost = TopMost = Main.TopMost;
            Main.BringToFront();
            //是否在任务栏显示
            ShowInTaskbar = false;
            //无边框模式
            FormBorderStyle = FormBorderStyle.None;
            //设置绘图层显示位置
            this.Location = new Point(Main.Location.X - Main.ShadowWidth, Main.Location.Y - Main.ShadowWidth);
            //设置ICO
            Icon = Main.Icon;
            ShowIcon = Main.ShowIcon;
            //设置大小
            Width = Main.Width + Main.ShadowWidth * 2;
            Height = Main.Height + Main.ShadowWidth * 2;
            //设置标题名
            //Text = Main.Text;
            //绘图层窗体移动
            Main.LocationChanged += new EventHandler(Main_LocationChanged);
            Main.SizeChanged += new EventHandler(Main_SizeChanged);
            Main.VisibleChanged += new EventHandler(Main_VisibleChanged);
            //还原任务栏右键菜单
            //CommonClass.SetTaskMenu(Main);
            //加载背景
            SetBits();
            //窗口鼠标穿透效果
            CanPenetrate();
        }
        #endregion

        #region 还原任务栏右键菜单
        protected override CreateParams CreateParams {
            get {
                CreateParams cParms = base.CreateParams;
                cParms.ExStyle |= 0x00080000; // WS_EX_LAYERED
                return cParms;
            }
        }
        public class CommonClass
        {
            public static void SetTaskMenu(Form form) {
                int windowLong = (NativeMethods.GetWindowLong(new HandleRef(form, form.Handle), -16));
                NativeMethods.SetWindowLong(new HandleRef(form, form.Handle), -16, windowLong | WS.WS_SYSMENU | WS.WS_MINIMIZEBOX);
            }
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
        }
        #endregion

        #region 控件层相关事件
        //移动主窗体时
        void Main_LocationChanged(object sender, EventArgs e) {
            Location = new Point(Main.Left - Main.ShadowWidth, Main.Top - Main.ShadowWidth);
        }

        //主窗体大小改变时
        void Main_SizeChanged(object sender, EventArgs e) {
            SetBits();
        }

        //主窗体显示或隐藏时
        void Main_VisibleChanged(object sender, EventArgs e) {
            if (Main.Shadow) {
                this.Visible = Main.Visible;
            }
        }

        //窗体关闭时释放
        protected override void OnClosed(EventArgs e) {
            base.OnClosed(e);
            this.Dispose();
        }
        #endregion

        #region 使窗口有鼠标穿透功能
        /// <summary>
        /// 使窗口有鼠标穿透功能
        /// </summary>
        private void CanPenetrate() {
            int intExTemp = NativeMethods.GetWindowLong(this.Handle, GWL.GWL_EXSTYLE);
            int oldGWLEx = NativeMethods.SetWindowLong(this.Handle, GWL.GWL_EXSTYLE, WS_EX.WS_EX_TRANSPARENT | WS_EX.WS_EX_LAYERED);
        }
        #endregion

        #region 绘画阴影方法1
        /// <summary>
        /// 四边阴影的颜色。[0]为阴影内沿颜色，[1]为阴影外沿颜色
        /// </summary>
        public Color[] ShadowColors = { Color.FromArgb(60, Color.Black), Color.Transparent };
        /// <summary>
        /// 圆角阴影的颜色。[0]为阴影内沿颜色，[1]为阴影外沿颜色。
        /// 注：一般来讲，圆角阴影内沿的颜色应当比四边阴影内沿的颜色更深，才会有更好的显示效果。此值应当根据您的实际情况而定。
        /// </summary>
        /// <remarks>由于给扇面上渐变时，起点并不是准确的扇面内弧，因此扇面的内沿颜色可能应比四边的内沿颜色深</remarks>
        public Color[] CornerColors = { Color.FromArgb(155, Color.Black), Color.Transparent };

        /// <summary>
        /// 绘制四角、四边的阴影
        /// </summary>
        /// <param name="g"></param>
        private void DrawShadow(Graphics g) {
            /* 阴影分为9宫格，5为内部背景图部分
             *  1   2   3
             *  4   5   6
             *  7   8   9
             */

            //赋新值
            ShadowColors[0] = Color.FromArgb(60, Main.ShadowColor);
            int S = Main.ShadowWidth;
            int A = S <= 4 ? 180 : S <= 15 ? 155 : S <= 50 ? 140 : S <= 100 ? 135 : 180;
            CornerColors[0] = Color.FromArgb(A, Main.ShadowColor);
            // 四角正方形边长 = 圆角半径 + 阴影宽度
            Size corSize = new Size(Main.ShadowWidth + Main.Radius, Main.ShadowWidth + Main.Radius);

            // 左侧、右侧渐变的尺寸
            Size gradientSize_LR = new Size(Main.ShadowWidth, this.Size.Height - corSize.Height * 2);

            // 顶部、底部渐变的尺寸
            Size gradientSize_TB = new Size(this.Size.Width - corSize.Width * 2, Main.ShadowWidth);

            // 绘制四边
            DrawLines(g, corSize, gradientSize_LR, gradientSize_TB);

            // 绘制四角
            DrawCorners(g, corSize);
        }

        /// <summary>
        /// 绘制四角的阴影
        /// </summary>
        /// <param name="g"></param>
        /// <param name="corSize">圆角区域正方形的大小</param>
        /// <returns></returns>
        private void DrawCorners(Graphics g, Size corSize) {
            /*
             * 四个角，每个角都是一个扇面
             * 画图时扇面由外弧、内弧以及两段的连接线构成图形
             * 然后在内弧中间附近向外做渐变
             *
             * 阴影分为9宫格，5为内部背景图部分
             *  1   2   3
             *  4   5   6
             *  7   8   9
             */
            Action<int> DrawCorenerN = (n) => {
                using (GraphicsPath gp = new GraphicsPath()) {
                    // 扇面外沿、内沿曲线的尺寸
                    Size sizeOutSide = new Size(corSize.Width * 2, corSize.Height * 2);
                    Size sizeInSide = new Size(Main.Radius * 2, Main.Radius * 2);

                    // 扇面外沿、内沿曲线的位置
                    Point locationOutSide, locationInSide;
                    // 当圆角半径小于MinCornerRadius时，内沿不绘制曲线，而以线段绘制近似值。该线段绘制方向是从p1指向p2。
                    Point p1, p2;

                    // 渐变起点位置
                    PointF brushCenter;

                    // 扇面起始角度
                    float startAngle;

                    // 根据四个方位不同，确定扇面的位置、角度及渐变起点位置
                    switch (n) {
                        case 1:
                            locationOutSide = new Point(0, 0);
                            startAngle = 180;
                            brushCenter = new PointF((float)sizeOutSide.Width - sizeInSide.Width * 0.5f, (float)sizeOutSide.Height - sizeInSide.Height * 0.5f);
                            p1 = new Point(corSize.Width, Main.ShadowWidth);
                            p2 = new Point(Main.ShadowWidth, corSize.Height);
                            break;

                        case 3:
                            locationOutSide = new Point(this.Width - sizeOutSide.Width, 0);
                            startAngle = 270;
                            brushCenter = new PointF((float)locationOutSide.X + sizeInSide.Width * 0.5f, (float)sizeOutSide.Height - sizeInSide.Height * 0.5f);
                            p1 = new Point(this.Width - Main.ShadowWidth, corSize.Height);
                            p2 = new Point(this.Width - corSize.Width, Main.ShadowWidth);
                            break;

                        case 7:
                            locationOutSide = new Point(0, this.Height - sizeOutSide.Height);
                            startAngle = 90;
                            brushCenter = new PointF((float)sizeOutSide.Width - sizeInSide.Width * 0.5f, (float)locationOutSide.Y + sizeInSide.Height * 0.5f);
                            p1 = new Point(Main.ShadowWidth, this.Height - corSize.Height);
                            p2 = new Point(corSize.Width, this.Height - Main.ShadowWidth);
                            break;

                        default:
                            locationOutSide = new Point(this.Width - sizeOutSide.Width, this.Height - sizeOutSide.Height);
                            startAngle = 0;
                            brushCenter = new PointF((float)locationOutSide.X + sizeInSide.Width * 0.5f, (float)locationOutSide.Y + sizeInSide.Height * 0.5f);
                            p1 = new Point(this.Width - corSize.Width, this.Height - Main.ShadowWidth);
                            p2 = new Point(this.Width - Main.ShadowWidth, this.Height - corSize.Height);
                            break;
                    }

                    // 扇面外沿曲线
                    Rectangle recOutSide = new Rectangle(locationOutSide, sizeOutSide);

                    // 扇面内沿曲线的位置
                    locationInSide = new Point(locationOutSide.X + (sizeOutSide.Width - sizeInSide.Width) / 2, locationOutSide.Y + (sizeOutSide.Height - sizeInSide.Height) / 2);

                    // 扇面内沿曲线
                    Rectangle recInSide = new Rectangle(locationInSide, sizeInSide);

                    // 将扇面添加到形状，以备绘制
                    gp.AddArc(recOutSide, startAngle, 90);

                    if (Main.Radius > 3) {
                        gp.AddArc(recInSide, startAngle + 90, -90);
                    } else {
                        gp.AddLine(p1, p2);
                    }

                    // 使用渐变笔刷
                    using (PathGradientBrush shadowBrush = new PathGradientBrush(gp)) {
                        Color[] colors = new Color[2];
                        float[] positions = new float[2];
                        ColorBlend sBlend = new ColorBlend();
                        // 扇面外沿色
                        colors[0] = CornerColors[1];
                        // 扇面内沿色
                        colors[1] = CornerColors[0];
                        positions[0] = 0.0f;
                        positions[1] = 1.0f;
                        sBlend.Colors = colors;
                        sBlend.Positions = positions;

                        shadowBrush.InterpolationColors = sBlend;
                        shadowBrush.CenterPoint = brushCenter;
                        // 上色中心点
                        g.FillPath(shadowBrush, gp);
                    }
                }
            };

            DrawCorenerN(1);
            DrawCorenerN(3);
            DrawCorenerN(7);
            DrawCorenerN(9);
        }

        /// <summary>
        /// 绘制上下左右四边的阴影
        /// </summary>
        /// <param name="g"></param>
        /// <param name="corSize"></param>
        /// <param name="gradientSize_LR"></param>
        /// <param name="gradientSize_TB"></param>
        private void DrawLines(Graphics g, Size corSize, Size gradientSize_LR, Size gradientSize_TB) {
            Rectangle rect2 = new Rectangle(new Point(corSize.Width, 0), gradientSize_TB);
            Rectangle rect4 = new Rectangle(new Point(0, corSize.Width), gradientSize_LR);
            Rectangle rect6 = new Rectangle(new Point(this.Size.Width - Main.ShadowWidth, corSize.Width), gradientSize_LR);
            Rectangle rect8 = new Rectangle(new Point(corSize.Width, this.Size.Height - Main.ShadowWidth), gradientSize_TB);

            using (
                LinearGradientBrush brush2 = new LinearGradientBrush(rect2, ShadowColors[1], ShadowColors[0], LinearGradientMode.Vertical),
                 brush4 = new LinearGradientBrush(rect4, this.ShadowColors[1], this.ShadowColors[0], LinearGradientMode.Horizontal),
                 brush6 = new LinearGradientBrush(rect6, this.ShadowColors[0], this.ShadowColors[1], LinearGradientMode.Horizontal),
                 brush8 = new LinearGradientBrush(rect8, this.ShadowColors[0], this.ShadowColors[1], LinearGradientMode.Vertical)
            ) {
                g.FillRectangle(brush2, rect2);
                g.FillRectangle(brush4, rect4);
                g.FillRectangle(brush6, rect6);
                g.FillRectangle(brush8, rect8);
            }
        }
        #endregion

        #region 绘画阴影方法2
        private Bitmap DrawBlurBorder() {
            return (Bitmap)DrawOutsetShadow(Main.ShadowColor, new Rectangle(0, 0, ClientRectangle.Width, ClientRectangle.Height));
        }
        private Image DrawOutsetShadow(Color color, Rectangle shadowCanvasArea) {
            Rectangle rOuter = shadowCanvasArea;
            Rectangle rInner = new Rectangle(Main.ShadowWidth - 1, Main.ShadowWidth - 1, Main.Width + 1, Main.Height + 1);

            Bitmap img = new Bitmap(rOuter.Width, rOuter.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(img);
            using (Pen bgPen = new Pen(Color.FromArgb(30, color))) {
                bgPen.Width = Main.ShadowWidth * 2 - 2;
                g.DrawRectangle(bgPen, rOuter);
            }
            using (Pen bgPen = new Pen(Color.FromArgb(60, color))) {
                g.DrawRectangle(bgPen, rInner);
            }
            g.Flush();
            g.Dispose();
            return img;
        }
        #endregion

        #region 不规则无毛边方法
        public void SetBits() {
            //窗体被释放，或句柄已空则不往下执行
            if (this.IsDisposed || IntPtr.Zero == this.Handle) {
                return;
            }
            //设置大小
            this.Width = Main.Width + Main.ShadowWidth * 2;
            this.Height = Main.Height + Main.ShadowWidth * 2;
            //设置位置
            this.Location = new Point(Main.Left - Main.ShadowWidth, Main.Top - Main.ShadowWidth);
            //绘制绘图层背景
            Bitmap bitmap = new Bitmap(this.Width, this.Height);
            //Rectangle _BacklightLTRB = new Rectangle(20, 20, 20, 20);//窗体光泽重绘边界
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighQuality; //高质量
            g.PixelOffsetMode = PixelOffsetMode.HighQuality; //高像素偏移质量
            //最大化时不绘制阴影 MDI子窗体时不绘制阴影
            if (Main.WindowState != FormWindowState.Maximized && Main.Shadow && !Main.IsMdiChild) {
                if (Main.ShadowPalace == null) {
                    //是否是直角阴影
                    if (Main.IsShadowStraight) {
                        bitmap = DrawBlurBorder();
                    } else {
                        //光圈阴影
                        DrawShadow(g);
                    }
                } else {
                    ////窗体光泽重绘边界
                    Rectangle _BacklightLTRB = Main.ShadowRectangle;
                    ImageDrawRect.DrawRect(g, (Bitmap)Main.ShadowPalace, ClientRectangle, Rectangle.FromLTRB(_BacklightLTRB.X, _BacklightLTRB.Y, _BacklightLTRB.Width, _BacklightLTRB.Height), 1, 1);
                }
            }

            if (!Bitmap.IsCanonicalPixelFormat(bitmap.PixelFormat) || !Bitmap.IsAlphaPixelFormat(bitmap.PixelFormat))
                throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");
            IntPtr oldBits = IntPtr.Zero;
            IntPtr screenDC = NativeMethods.GetDC(IntPtr.Zero);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr memDc = NativeMethods.CreateCompatibleDC(screenDC);

            try {
                NativeMethods.Point topLoc = new NativeMethods.Point(Left, Top);
                NativeMethods.Size bitMapSize = new NativeMethods.Size(Width, Height);
                NativeMethods.BLENDFUNCTION blendFunc = new NativeMethods.BLENDFUNCTION();
                NativeMethods.Point srcLoc = new NativeMethods.Point(0, 0);

                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                oldBits = NativeMethods.SelectObject(memDc, hBitmap);

                blendFunc.BlendOp = AC.AC_SRC_OVER;
                blendFunc.SourceConstantAlpha = Byte.Parse("255");
                blendFunc.AlphaFormat = AC.AC_SRC_ALPHA;
                blendFunc.BlendFlags = 0;

                NativeMethods.UpdateLayeredWindow(Handle, screenDC, ref topLoc, ref bitMapSize, memDc, ref srcLoc, 0, ref blendFunc, NativeMethods.ULW_ALPHA);
            } finally {
                if (hBitmap != IntPtr.Zero) {
                    NativeMethods.SelectObject(memDc, oldBits);
                    NativeMethods.DeleteObject(hBitmap);
                }
                NativeMethods.ReleaseDC(IntPtr.Zero, screenDC);
                NativeMethods.DeleteDC(memDc);
            }
        }
        #endregion
    }
}
