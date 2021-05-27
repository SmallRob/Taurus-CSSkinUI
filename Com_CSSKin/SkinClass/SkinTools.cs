
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Com_CSSkin.SkinControl;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Com_CSSkin.Win32;
using Com_CSSkin.Win32.Struct;
using Com_CSSkin.Imaging;

namespace Com_CSSkin.SkinClass
{
    public class SkinTools
    {
        #region 通过API函数设置Form透明度
        /// <summary>
        /// 通过API函数设置Form透明度
        /// </summary>
        /// <param name="form">窗体</param>
        /// <param name="opacity">透明度0.00-1</param>
        static void SetFormOpacity(Form form, double opacity) {
            //获取透明值
            byte _opacity = (byte)(opacity * 255);
            int exStyle = (int)NativeMethods.GetWindowLong(form.Handle, -20);
            byte bNewAlpha = _opacity;
            int newExStyle = exStyle;

            if (bNewAlpha != 255)
                newExStyle = newExStyle | 0x00080000;

            if (newExStyle != exStyle || (newExStyle & 0x00080000) != 0) {
                if (newExStyle != exStyle)
                    NativeMethods.SetWindowLong(form.Handle, -20, new IntPtr(newExStyle));

                if ((newExStyle & 0x00080000) != 0)
                    NativeMethods.SetLayeredWindowAttributes(form.Handle, 0, bNewAlpha, 2);
            }
            // Since we're making a bunch of PInvokes on the form's native handle,
            // make sure it sticks around
            GC.KeepAlive(form);
        }
        #endregion

        /// <summary>
        /// 将图像转换成灰色介
        /// </summary>
        /// <returns>灰色图像</returns>
        public static Bitmap GaryImg(Bitmap b) {
            if (b.Width != 0 || b.Height != 0) {
                return null;
            }
            Bitmap bmp = b.Clone(new Rectangle(0, 0, b.Width, b.Height), PixelFormat.Format24bppRgb);
            b.Dispose();
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            byte[] byColorInfo = new byte[bmp.Height * bmpData.Stride];
            Marshal.Copy(bmpData.Scan0, byColorInfo, 0, byColorInfo.Length);
            for (int x = 0, xLen = bmp.Width; x < xLen; x++) {
                for (int y = 0, yLen = bmp.Height; y < yLen; y++) {
                    byColorInfo[y * bmpData.Stride + x * 3] =
                        byColorInfo[y * bmpData.Stride + x * 3 + 1] =
                        byColorInfo[y * bmpData.Stride + x * 3 + 2] =
                        GetAvg(
                        byColorInfo[y * bmpData.Stride + x * 3],
                        byColorInfo[y * bmpData.Stride + x * 3 + 1],
                        byColorInfo[y * bmpData.Stride + x * 3 + 2]);
                }
            }
            Marshal.Copy(byColorInfo, 0, bmpData.Scan0, byColorInfo.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
        private static byte GetAvg(byte b, byte g, byte r) {
            return (byte)((r + g + b) / 3);
        }

        /// <summary>
        /// 获取图片主色调
        /// </summary>
        /// <param name="back">图片</param>
        public static Color GetImageAverageColor(Bitmap back) {
            return BitmapHelper.GetImageAverageColor(back);
        }

        /// <summary>
        /// 判断颜色偏向于暗色或亮色(true为偏向于暗色，false位偏向于亮色。)
        /// </summary>
        /// <param name="c">要判断的颜色</param>
        /// <returns>true为偏向于暗色，false位偏向于亮色。</returns>
        public static bool ColorSlantsDarkOrBright(Color c) {
            HSL hsl = ColorConverterEx.ColorToHSL(c);
            if (hsl.Luminance < 0.15d) {
                return true;
            } else if (hsl.Luminance < 0.35d) {
                return true;
            } else if (hsl.Luminance < 0.85d) {
                return false;
            } else {
                return false;
            }
        }

        /// <summary>
        /// 绘制组件圆角
        /// </summary>
        /// <param name="frm">要绘制的组件</param>
        /// <param name="RgnRadius">圆角大小</param>
        public static void CreateRegion(Control ctrl, int RgnRadius) {
            int Rgn = NativeMethods.CreateRoundRectRgn(0, 0, ctrl.ClientRectangle.Width + 1, ctrl.ClientRectangle.Height + 1, RgnRadius, RgnRadius);
            NativeMethods.SetWindowRgn(ctrl.Handle, Rgn, true);
        }

        /// <summary>
        /// 样式绘制圆角
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="bounds">范围</param>
        /// <param name="radius">圆角大小</param>
        /// <param name="roundStyle">圆角样式</param>
        public static void CreateRegion(
             Control control,
             Rectangle bounds,
             int radius,
             RoundStyle roundStyle) {
            using (GraphicsPath path =
                GraphicsPathHelper.CreatePath(
                bounds, radius, roundStyle, true)) {
                Region region = new Region(path);
                path.Widen(Pens.White);
                region.Union(path);
                control.Region = region;
            }
        }

        /// <summary>
        /// 绘制四个角弧度为8
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="bounds">范围</param>
        public static void CreateRegion(
            Control control,
            Rectangle bounds) {
            CreateRegion(control, bounds, 8, RoundStyle.All);
        }

        /// <summary>
        /// 样式绘制圆角
        /// </summary>
        /// <param name="hWnd">控件句柄</param>
        /// <param name="radius">圆角</param>
        /// <param name="roundStyle">圆角样式</param>
        /// <param name="redraw">是否重画</param>
        public static void CreateRegion(
           IntPtr hWnd,
           int radius,
           RoundStyle roundStyle,
           bool redraw) {
            RECT bounds = new RECT();
            NativeMethods.GetWindowRect(hWnd, ref bounds);

            Rectangle rect = new Rectangle(
                Point.Empty, bounds.Size);

            if (roundStyle != RoundStyle.None) {
                using (GraphicsPath path =
                    GraphicsPathHelper.CreatePath(
                    rect, radius, roundStyle, true)) {
                    using (Region region = new Region(path)) {
                        path.Widen(Pens.White);
                        region.Union(path);
                        IntPtr hDc = NativeMethods.GetWindowDC(hWnd);
                        try {
                            using (Graphics g = Graphics.FromHdc(hDc)) {
                                NativeMethods.SetWindowRgn(hWnd, region.GetHrgn(g), redraw);
                            }
                        } finally {
                            NativeMethods.ReleaseDC(hWnd, hDc);
                        }
                    }
                }
            } else {
                IntPtr hRgn = NativeMethods.CreateRectRgn(0, 0, rect.Width, rect.Height);
                NativeMethods.SetWindowRgn(hWnd, hRgn, redraw);
            }
        }

        /// <summary>
        /// 设置图形边缘半透明
        /// </summary>
        /// <param name="p_Bitmap">图形</param>
        /// <param name="p_CentralTransparent">true中心透明 false边缘透明</param>
        /// <param name="p_Crossdirection">true横 false纵</param>
        /// <returns></returns>
        public static Bitmap BothAlpha(Bitmap p_Bitmap, bool p_CentralTransparent, bool p_Crossdirection) {
            Bitmap _SetBitmap = new Bitmap(p_Bitmap.Width, p_Bitmap.Height);
            Graphics _GraphisSetBitmap = Graphics.FromImage(_SetBitmap);
            _GraphisSetBitmap.DrawImage(p_Bitmap, new Rectangle(0, 0, p_Bitmap.Width, p_Bitmap.Height));
            _GraphisSetBitmap.Dispose();

            Bitmap _Bitmap = new Bitmap(_SetBitmap.Width, _SetBitmap.Height);
            Graphics _Graphcis = Graphics.FromImage(_Bitmap);

            Point _Left1 = new Point(0, 0);
            Point _Left2 = new Point(_Bitmap.Width, 0);
            Point _Left3 = new Point(_Bitmap.Width, _Bitmap.Height / 2);
            Point _Left4 = new Point(0, _Bitmap.Height / 2);

            if (p_Crossdirection) {
                _Left1 = new Point(0, 0);
                _Left2 = new Point(_Bitmap.Width / 2, 0);
                _Left3 = new Point(_Bitmap.Width / 2, _Bitmap.Height);
                _Left4 = new Point(0, _Bitmap.Height);
            }

            Point[] _Point = new Point[] { _Left1, _Left2, _Left3, _Left4 };
            PathGradientBrush _SetBruhs = new PathGradientBrush(_Point, WrapMode.TileFlipY);

            _SetBruhs.CenterPoint = new PointF(0, 0);
            _SetBruhs.FocusScales = new PointF(_Bitmap.Width / 2, 0);
            _SetBruhs.CenterColor = Color.FromArgb(0, 255, 255, 255);
            _SetBruhs.SurroundColors = new Color[] { Color.FromArgb(255, 255, 255, 255) };
            if (p_Crossdirection) {
                _SetBruhs.FocusScales = new PointF(0, _Bitmap.Height);
                _SetBruhs.WrapMode = WrapMode.TileFlipX;
            }

            if (p_CentralTransparent) {
                _SetBruhs.CenterColor = Color.FromArgb(255, 255, 255, 255);
                _SetBruhs.SurroundColors = new Color[] { Color.FromArgb(0, 255, 255, 255) };
            }

            _Graphcis.FillRectangle(_SetBruhs, new Rectangle(0, 0, _Bitmap.Width, _Bitmap.Height));
            _Graphcis.Dispose();

            BitmapData _NewData = _Bitmap.LockBits(new Rectangle(0, 0, _Bitmap.Width, _Bitmap.Height), ImageLockMode.ReadOnly, _Bitmap.PixelFormat);
            byte[] _NewBytes = new byte[_NewData.Stride * _NewData.Height];
            Marshal.Copy(_NewData.Scan0, _NewBytes, 0, _NewBytes.Length);
            _Bitmap.UnlockBits(_NewData);

            BitmapData _SetData = _SetBitmap.LockBits(new Rectangle(0, 0, _SetBitmap.Width, _SetBitmap.Height), ImageLockMode.ReadWrite, _SetBitmap.PixelFormat);
            byte[] _SetBytes = new byte[_SetData.Stride * _SetData.Height];
            Marshal.Copy(_SetData.Scan0, _SetBytes, 0, _SetBytes.Length);

            int _WriteIndex = 0;

            for (int i = 0; i != _SetData.Height; i++) {
                _WriteIndex = i * _SetData.Stride + 3;
                for (int z = 0; z != _SetData.Width; z++) {
                    _SetBytes[_WriteIndex] = _NewBytes[_WriteIndex];
                    _WriteIndex += 4;
                }
            }
            Marshal.Copy(_SetBytes, 0, _SetData.Scan0, _SetBytes.Length);
            _SetBitmap.UnlockBits(_SetData);
            return _SetBitmap;
        }

        /// <summary>
        /// 绘制发光字体
        /// </summary>
        /// <param name="Str">字体</param>
        /// <param name="F">字体样式</param>
        /// <param name="ColorFore">字体颜色</param>
        /// <param name="ColorBack">光圈颜色</param>
        /// <param name="BlurConsideration">光圈大小</param>
        /// <returns>Image格式图</returns>
        public static Image ImageLightEffect(string Str, Font F, Color ColorFore, Color ColorBack, int BlurConsideration) {
            Bitmap Var_Bitmap = null;//实例化Bitmap类
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))//实例化Graphics类
            {
                SizeF Var_Size = g.MeasureString(Str, F);//对字符串进行测量
                using (Bitmap Var_bmp = new Bitmap((int)Var_Size.Width, (int)Var_Size.Height))//通过文字的大小实例化Bitmap类
                using (Graphics Var_G_Bmp = Graphics.FromImage(Var_bmp))//实例化Bitmap类
                using (SolidBrush Var_BrushBack = new SolidBrush(Color.FromArgb(16, ColorBack.R, ColorBack.G, ColorBack.B)))//根据RGB的值定义画刷
                using (SolidBrush Var_BrushFore = new SolidBrush(ColorFore))//定义画刷
                {
                    Var_G_Bmp.SmoothingMode = SmoothingMode.HighQuality;//设置为高质量
                    Var_G_Bmp.InterpolationMode = InterpolationMode.HighQualityBilinear;//设置为高质量的收缩
                    Var_G_Bmp.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;//消除锯齿
                    Var_G_Bmp.DrawString(Str, F, Var_BrushBack, 0, 0);//给制文字
                    Var_Bitmap = new Bitmap(Var_bmp.Width + BlurConsideration, Var_bmp.Height + BlurConsideration);//根据辉光文字的大小实例化Bitmap类
                    using (Graphics Var_G_Bitmap = Graphics.FromImage(Var_Bitmap))//实例化Graphics类
                    {
                        Var_G_Bitmap.SmoothingMode = SmoothingMode.HighQuality;//设置为高质量
                        Var_G_Bitmap.InterpolationMode = InterpolationMode.HighQualityBilinear;//设置为高质量的收缩
                        Var_G_Bitmap.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;//消除锯齿
                        //遍历辉光文字的各象素点
                        for (int x = 0; x <= BlurConsideration; x++) {
                            for (int y = 0; y <= BlurConsideration; y++) {
                                if (null != Var_G_Bitmap) {
                                    if (!Var_G_Bitmap.IsNull()) {
                                        if (!Var_bmp.IsNull()) {
                                            Var_G_Bitmap.DrawImageUnscaled(Var_bmp, x, y);//绘制辉光文字的点
                                        }
                                    }
                                }
                            }
                        }
                        Var_G_Bitmap.DrawString(Str, F, Var_BrushFore, BlurConsideration / 2, BlurConsideration / 2);//绘制文字
                    }
                }
            }
            return Var_Bitmap;
        }

        /// <summary>
        /// 范围绘制发光字体
        /// </summary>
        /// <param name="Str">字体</param>
        /// <param name="F">字体样式</param>
        /// <param name="ColorFore">字体颜色</param>
        /// <param name="ColorBack">光圈颜色</param>
        /// <param name="BlurConsideration">光圈大小</param>
        /// <param name="rc">文字范围</param>
        /// <param name="auto">是否启用范围绘制发光字体</param>
        /// <returns>Image格式图</returns>
        public static Image ImageLightEffect(string Str, Font F, Color ColorFore, Color ColorBack, int BlurConsideration, Rectangle rc, bool auto) {
            Bitmap Var_Bitmap = null;//实例化Bitmap类
            StringFormat sf = new StringFormat(StringFormatFlags.NoWrap);
            sf.Trimming = auto ? StringTrimming.EllipsisWord : StringTrimming.None;
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))//实例化Graphics类
            {
                SizeF Var_Size = g.MeasureString(Str, F);//对字符串进行测量
                using (Bitmap Var_bmp = new Bitmap((int)Var_Size.Width, (int)Var_Size.Height))//通过文字的大小实例化Bitmap类
                using (Graphics Var_G_Bmp = Graphics.FromImage(Var_bmp))//实例化Bitmap类
                using (SolidBrush Var_BrushBack = new SolidBrush(Color.FromArgb(16, ColorBack.R, ColorBack.G, ColorBack.B)))//根据RGB的值定义画刷
                using (SolidBrush Var_BrushFore = new SolidBrush(ColorFore))//定义画刷
                {
                    Var_G_Bmp.SmoothingMode = SmoothingMode.HighQuality;//设置为高质量
                    Var_G_Bmp.InterpolationMode = InterpolationMode.HighQualityBilinear;//设置为高质量的收缩
                    Var_G_Bmp.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;//消除锯齿
                    Var_G_Bmp.DrawString(Str, F, Var_BrushBack, rc, sf);//给制文字
                    Var_Bitmap = new Bitmap(Var_bmp.Width + BlurConsideration, Var_bmp.Height + BlurConsideration);//根据辉光文字的大小实例化Bitmap类
                    using (Graphics Var_G_Bitmap = Graphics.FromImage(Var_Bitmap))//实例化Graphics类
                    {
                        if (ColorBack != Color.Transparent) {
                            Var_G_Bitmap.SmoothingMode = SmoothingMode.HighQuality;//设置为高质量
                            Var_G_Bitmap.InterpolationMode = InterpolationMode.HighQualityBilinear;//设置为高质量的收缩
                            Var_G_Bitmap.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;//消除锯齿
                            //遍历辉光文字的各象素点
                            for (int x = 0; x <= BlurConsideration; x++) {
                                for (int y = 0; y <= BlurConsideration; y++) {
                                    if (null != Var_G_Bitmap) {
                                        if (!Var_G_Bitmap.IsNull()) {
                                            if (!Var_bmp.IsNull()) {
                                                Var_G_Bitmap.DrawImageUnscaled(Var_bmp, x, y);//绘制辉光文字的点
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        Var_G_Bitmap.DrawString(Str, F, Var_BrushFore, new Rectangle(new Point(Convert.ToInt32(BlurConsideration / 2), Convert.ToInt32(BlurConsideration / 2)), rc.Size), sf);//绘制文字
                    }
                }
            }
            return Var_Bitmap;
        }

        /// <summary>
        /// 执行一次鼠标点击
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public static void CursorClick(int x, int y) {
            int MOUSEEVENTF_LEFTDOWN = 0x2;
            int MOUSEEVENTF_LEFTUP = 0x4;
            //恢复一次鼠标点击
            NativeMethods.mouse_event(MOUSEEVENTF_LEFTDOWN, x * 65536 / 1024, y * 65536 / 768, 0, 0);
            NativeMethods.mouse_event(MOUSEEVENTF_LEFTUP, x * 65536 / 1024, y * 65536 / 768, 0, 0);
        }

        /// <summary>
        /// 图片缩放
        /// </summary>
        /// <param name="b">源图片Bitmap</param>
        /// <param name="dstWidth">目标宽度</param>
        /// <param name="dstHeight">目标高度</param>
        /// <returns>处理完成的图片 Bitmap</returns>
        public static Bitmap ResizeBitmap(Bitmap b, int dstWidth, int dstHeight) {
            Bitmap dstImage = new Bitmap(dstWidth, dstHeight);
            System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dstImage);
            //   设置插值模式 
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
            //   设置平滑模式 
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //用Graphic的DrawImage方法通过设置大小绘制新的图片实现缩放
            g.DrawImage(b, new Rectangle(0, 0, dstImage.Width, dstImage.Height), new Rectangle(0, 0, b.Width, b.Height), GraphicsUnit.Pixel);
            g.Save();
            g.Dispose();
            return dstImage;
        }

        #region 获取窗体不透明区域
        /// <summary> 
        /// 创建支持位图区域的控件（目前有button和form）
        /// </summary> 
        /// <param name="control">控件</param> 
        /// <param name="bitmap">位图</param>
        /// <param name="Alpha">小于此透明值的去除</param> 
        public static void CreateControlRegion(Control control, Bitmap bitmap, int Alpha) {
            //判断是否存在控件和位图
            if (control == null || bitmap == null)
                return;

            control.Width = bitmap.Width;
            control.Height = bitmap.Height;
            //当控件是form时
            if (control is System.Windows.Forms.Form) {
                //强制转换为FORM
                Form form = (Form)control;
                //当FORM的边界FormBorderStyle不为NONE时，应将FORM的大小设置成比位图大小稍大一点
                form.Width = control.Width;
                form.Height = control.Height;
                //没有边界
                form.FormBorderStyle = FormBorderStyle.None;
                //将位图设置成窗体背景图片
                form.BackgroundImage = bitmap;
                //计算位图中不透明部分的边界
                Bitmap back = new Bitmap(bitmap.Width, bitmap.Height);
                Graphics g = Graphics.FromImage(back);
                foreach (Control c in form.Controls) {
                    g.FillRectangle(new SolidBrush(form.BackColor), new Rectangle(c.Location, c.Size));
                }
                g.DrawImage(bitmap, 0, 0);
                GraphicsPath graphicsPath = CalculateControlGraphicsPath(back, Alpha);
                //应用新的区域
                form.Region = new Region(graphicsPath);
                GC.Collect();
            }
                //当控件是button时
            else if (control is SkinButton) {
                //强制转换为 button
                SkinButton button = (SkinButton)control;
                //计算位图中不透明部分的边界
                GraphicsPath graphicsPath = CalculateControlGraphicsPath(bitmap, Alpha);
                //应用新的区域
                button.Region = new Region(graphicsPath);
            } else if (control is SkinProgressBar) {
                //强制转换为 SkinProgressBar
                SkinProgressBar Progressbar = (SkinProgressBar)control;
                //计算位图中不透明部分的边界
                GraphicsPath graphicsPath = CalculateControlGraphicsPath(bitmap, Alpha);
                //应用新的区域
                Progressbar.Region = new Region(graphicsPath);
            }
        }

        //计算位图中不透明部分的边界
        public static GraphicsPath CalculateControlGraphicsPath(Bitmap bitmap, int Alpha) {
            //创建快速位图遍历
            FastBitmap bmp = new FastBitmap(bitmap);
            bmp.Lock();
            //创建 GraphicsPath
            GraphicsPath graphicsPath = new GraphicsPath();
            //第一个找到点的X
            int colOpaquePixel = 0;
            // 偏历所有行（Y方向）
            for (int row = 0; row < bitmap.Height; row++) {
                //重设
                colOpaquePixel = 0;
                //偏历所有列（X方向）
                for (int col = 0; col < bitmap.Width; col++) {
                    //如果是不需要透明处理的点则标记，然后继续偏历
                    if (bmp.GetPixel(col, row).A >= Alpha) {
                        //记录当前
                        colOpaquePixel = col;
                        //建立新变量来记录当前点
                        int colNext = col;
                        ///从找到的不透明点开始，继续寻找不透明点,一直到找到或则达到图片宽度 
                        for (colNext = colOpaquePixel; colNext < bitmap.Width; colNext++)
                            if (bmp.GetPixel(colNext, row).A < Alpha)
                                break;
                        //将不透明点加到graphics path
                        graphicsPath.AddRectangle(new Rectangle(colOpaquePixel, row, colNext - colOpaquePixel, 1));
                        col = colNext;
                    }
                }
            }
            bmp.Unlock();
            return graphicsPath;
        }
        #endregion
    }
}
