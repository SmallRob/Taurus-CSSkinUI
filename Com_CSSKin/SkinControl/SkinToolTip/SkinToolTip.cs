
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using Com_CSSkin.Win32.Struct;
using Com_CSSkin.Win32;
using Com_CSSkin.Win32.Const;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin
{
    [ToolboxBitmap(typeof(ToolTip))]
    public class SkinToolTip : ToolTip
    {
        #region 变量
        private ImageDc _backDc;
        private Image _image;
        private double _opacity = 1d;
        private Font _titleFont = new Font("宋体", 9, FontStyle.Bold);
        private Size _imageSize = SystemInformation.SmallIconSize;
        #endregion

        #region 无参构造与带参构造

        public SkinToolTip()
            : base() {
            InitOwnerDraw();
        }

        public SkinToolTip(IContainer cont)
            : base(cont) {
            InitOwnerDraw();
        }

        #endregion

        #region 属性
        private Color _border = Color.FromArgb(204, 153, 51);
        [Description("边框颜色。"), Category("Skin")]
        [DefaultValue(typeof(Color), "204, 153, 51")]
        public Color Border {
            get { return _border; }
            set {
                if (_border != value) {
                    _border = value;
                }
            }
        }
        [Browsable(false)]
        public new Color ForeColor {
            get { return base.ForeColor; }
            set { base.ForeColor = value; }
        }
        [Description("渐变背景色1。"), Category("Skin")]
        public new Color BackColor {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        private Color _backHover = Color.FromArgb(255, 180, 105);
        [Description("渐变背景色2。"), Category("Skin")]
        [DefaultValue(typeof(Color), "255, 180, 105")]
        public Color BackColor2 {
            get { return _backHover; }
            set {
                if (_backHover != value) {
                    _backHover = value;
                }
            }
        }

        private Color _titleFore = Color.Brown;
        [Description("标题字体色。"), Category("Skin")]
        [DefaultValue(typeof(Color), "Brown")]
        public Color TitleFore {
            get { return _titleFore; }
            set {
                if (_titleFore != value) {
                    _titleFore = value;
                }
            }
        }

        private Color _tipFore = Color.Chocolate;
        [Description("提示字体色。"), Category("Skin")]
        [DefaultValue(typeof(Color), "Chocolate")]
        public Color TipFore {
            get { return _tipFore; }
            set {
                if (_tipFore != value) {
                    _tipFore = value;
                }
            }
        }

        [DefaultValue(typeof(Font), "宋体, 9pt, style=Bold")]
        Font TitleFont {
            get { return _titleFont; }
            set {
                if (_titleFont == null) {
                    throw new ArgumentNullException("TitleFont");
                }

                if (!_titleFont.IsSystemFont) {
                    _titleFont.Dispose();
                }

                _titleFont = value;
            }
        }

        [Description("ICon图标模式。"), Category("Skin")]
        public new ToolTipIcon ToolTipIcon {
            get { return base.ToolTipIcon; }
            set {
                if (_image != null) {
                    base.ToolTipIcon = ToolTipIcon.Info;
                } else {
                    base.ToolTipIcon = value;
                }
            }
        }

        [DefaultValue(null)]
        [Description("ICon图标。"), Category("Skin")]
        public Image Image {
            get { return _image; }
            set {
                _image = value;
                if (_image == null) {
                    base.ToolTipIcon = ToolTipIcon.None;
                } else {
                    base.ToolTipIcon = ToolTipIcon.Info;
                }
            }
        }

        [DefaultValue(typeof(Size), "16, 16")]
        Size ImageSize {
            get { return _imageSize; }
            set {
                if (_imageSize != value) {
                    _imageSize = value;
                    if (_imageSize.Width > 32) {
                        _imageSize.Width = 32;
                    }

                    if (_imageSize.Height > 32) {
                        _imageSize.Height = 32;
                    }
                }
            }
        }

        [DefaultValue(1d)]
        [TypeConverter(typeof(OpacityConverter))]
        [Description("透明度。"), Category("Skin")]
        public double Opacity {
            get { return _opacity; }
            set {
                if (value < 0 && value > 1) {
                    throw new ArgumentOutOfRangeException("Opacity");
                }

                _opacity = value;
            }
        }

        protected IntPtr Handle {
            get {
                if (!DesignMode) {
                    Type t = typeof(ToolTip);
                    PropertyInfo pi = t.GetProperty(
                        "Handle",
                         BindingFlags.NonPublic | BindingFlags.Instance);

                    IntPtr handle = (IntPtr)pi.GetValue(this, null);
                    return handle;
                }

                return IntPtr.Zero;
            }
        }

        #endregion

        #region Dispose

        protected override void Dispose(bool disposing) {
            base.Dispose(disposing);
            if (disposing) {
                if (_backDc != null) {
                    _backDc.Dispose();
                    _backDc = null;
                }

                if (!_titleFont.IsSystemFont) {
                    _titleFont.Dispose();
                }
                _titleFont = null;

                _image = null;
            }
        }

        #endregion

        #region 辅助方法

        private void InitOwnerDraw() {
            base.OwnerDraw = true;
            base.ReshowDelay = 800;
            base.InitialDelay = 500;
            base.Draw += new DrawToolTipEventHandler(ToolTipExDraw);
            base.Popup += new PopupEventHandler(ToolTipExPopup);
        }

        private void ToolTipExPopup(
            object sender, PopupEventArgs e) {
            if (_opacity < 1D) {
                //如果使用背景透明，获取背景图。
                TipCapture();
            }
        }

        private void ToolTipExDraw(
            object sender, DrawToolTipEventArgs e) {
            Graphics g = e.Graphics;
            //最高质量绘制文字
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            Rectangle bounds = e.Bounds;
            int alpha = (int)(_opacity * 255);

            int defaultXOffset = 5;
            int defaultTopHeight = 20;

            int tipTextXOffset = 5;
            int tipTextYOffset = 3;

            if (Handle != IntPtr.Zero && _opacity < 1D) {
                IntPtr hDC = g.GetHdc();

                NativeMethods.BitBlt(
                    hDC, 0, 0, bounds.Width, bounds.Height,
                    _backDc.Hdc, 0, 0, 0xCC0020);
                g.ReleaseHdc(hDC);
            }

            Color backNormalColor = Color.FromArgb(
               alpha, this.BackColor);
            Color baseColor = Color.FromArgb(
                alpha, this.BackColor2);
            Color borderColor = Color.FromArgb(
                alpha, Border);

            using (LinearGradientBrush brush = new LinearGradientBrush(
                bounds,
                backNormalColor,
                baseColor,
                LinearGradientMode.Vertical)) {
                g.FillRectangle(brush, bounds);
            }

            ControlPaint.DrawBorder(
                g,
                bounds,
                borderColor,
                ButtonBorderStyle.Solid);

            Rectangle imageRect = Rectangle.Empty;
            Rectangle titleRect;
            Rectangle tipRect;

            if (base.ToolTipIcon != ToolTipIcon.None) {
                tipTextXOffset = defaultXOffset;
                tipTextYOffset = defaultTopHeight;

                imageRect = new Rectangle(
                    bounds.X + defaultXOffset - (ImageSize.Width - 16) / 2,
                    bounds.Y + (defaultTopHeight - _imageSize.Height) / 2 + 2,
                    _imageSize.Width,
                    _imageSize.Height);

                Image image = _image;
                bool bDispose = false;

                if (image == null) {
                    Icon icon = GetIcon();
                    if (icon != null) {
                        image = icon.ToBitmap();
                        bDispose = true;
                    }
                }

                if (image != null) {
                    using (InterpolationModeGraphics ig =
                        new InterpolationModeGraphics(g)) {
                        if (_opacity < 1D) {
                            ToolTipRenderHelper.RenderAlphaImage(
                                g,
                                image,
                                imageRect,
                                (float)_opacity);
                        } else {
                            g.DrawImage(
                                image,
                                imageRect,
                                0,
                                0,
                                image.Width,
                                image.Height,
                                GraphicsUnit.Pixel);
                        }
                    }
                    if (bDispose) {
                        image.Dispose();
                    }
                }
            }

            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;

            if (!string.IsNullOrEmpty(base.ToolTipTitle)) {
                tipTextXOffset = defaultXOffset;
                tipTextYOffset = defaultTopHeight;

                int x = imageRect.IsEmpty ?
                    defaultXOffset : imageRect.Right + 3;

                titleRect = new Rectangle(
                    x,
                    bounds.Y + 2,
                    bounds.Width - x,
                    defaultTopHeight);

                Color foreColor = Color.FromArgb(
                    alpha, TitleFore);

                using (Brush brush = new SolidBrush(foreColor)) {
                    g.DrawString(
                        base.ToolTipTitle,
                        TitleFont,
                        brush,
                        titleRect,
                        sf);
                }
            }

            if (!string.IsNullOrEmpty(e.ToolTipText)) {
                int x = imageRect.IsEmpty ?
                     bounds.X + tipTextXOffset : imageRect.Right + 3;
                tipRect = new Rectangle(
                    x,
                    bounds.Y + tipTextYOffset,
                    bounds.Width - tipTextXOffset * 2,
                    bounds.Height - tipTextYOffset);

                sf = StringFormat.GenericTypographic;

                Color foreColor = Color.FromArgb(
                   alpha, TipFore);

                using (Brush brush = new SolidBrush(foreColor)) {
                    g.DrawString(
                        e.ToolTipText,
                        e.Font,
                        brush,
                        tipRect,
                        sf);
                }
            }
        }

        private void TipCapture() {
            IntPtr handle = Handle;
            if (handle == IntPtr.Zero) {
                return;
            }

            RECT rect = new RECT();

            NativeMethods.GetWindowRect(handle, ref rect);

            Size size = new Size(
                rect.Right - rect.Left,
                rect.Bottom - rect.Top);

            _backDc = new ImageDc(size.Width, size.Height);
            IntPtr pD = NativeMethods.GetDesktopWindow();
            IntPtr pH = NativeMethods.GetDC(pD);

            NativeMethods.BitBlt(
                _backDc.Hdc,
                0, 0, size.Width, size.Height,
                pH, rect.Left, rect.Top, 0xCC0020);
            NativeMethods.ReleaseDC(pD, pH);
        }

        private Icon GetIcon() {
            switch (base.ToolTipIcon) {
                case ToolTipIcon.Info:
                    return SystemIcons.Information;
                case ToolTipIcon.Warning:
                    return SystemIcons.Warning;
                case ToolTipIcon.Error:
                    return SystemIcons.Error;
                default:
                    return null;
            }
        }
        #endregion
    }
}
