
using Com_CSSkin.Win32;
using Com_CSSkin.Win32.Const;
using Com_CSSkin.Win32.Struct;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(ListView))]
    public partial class SkinListView : ListView
    {
        public SkinListView() {
            InitializeComponent();
            //base.SetStyle(
            //  ControlStyles.AllPaintingInWmPaint |
            //  ControlStyles.UserPaint |
            //  ControlStyles.OptimizedDoubleBuffer |
            //  ControlStyles.ResizeRedraw |
            //  ControlStyles.DoubleBuffer, true);
            base.OwnerDraw = true;
            // 开启双缓冲
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            base.UpdateStyles();
        }
        #region 变量

        private Color _rowBackColor1 = Color.White;
        private Color _rowBackColor2 = Color.FromArgb(254, 216, 249);
        private Color _selectedColor = Color.FromArgb(75, 188, 254);
        private Color _headColor = Color.FromArgb(75, 188, 254);

        private HeaderNativeWindow _headerNativeWindow;

        #endregion

        #region API Const

        private const int LVM_FIRST = 0x1000;
        private const int LVM_GETHEADER = (LVM_FIRST + 31);

        private const int HDI_ORDER = 0x0080;
        private const int HDI_WIDTH = 0x0001;
        private const int HDI_HEIGHT = HDI_WIDTH;

        private const int HDM_FIRST = 0x1200;
        private const int HDM_GETITEMCOUNT = (HDM_FIRST + 0);
        private const int HDM_GETITEMA = (HDM_FIRST + 3);
        private const int HDM_GETITEMRECT = (HDM_FIRST + 7);

        #endregion

        #region 属性
        //private Bitmap _HorizontalArrowBitmap = Properties.Resources.vista_ScrollHorzArrow;
        //[Category("Scroll")]
        //[Description("水平滚动条的箭头样式")]
        //public Bitmap HorizontalArrowBitmap {
        //    get { return _HorizontalArrowBitmap; }
        //    set {
        //        _HorizontalArrowBitmap = value;
        //        PreScroll();
        //    }
        //}

        //private Bitmap _HorizontalThumbBitmap = Properties.Resources.vista_ScrollHorzThumb;
        //[Category("Scroll")]
        //[Description("水平滚动条的滑块样式")]
        //public Bitmap HorizontalThumbBitmap {
        //    get { return _HorizontalThumbBitmap; }
        //    set {
        //        _HorizontalThumbBitmap = value;
        //        PreScroll();
        //    }
        //}

        //private Bitmap _HorizontalTrackBitmap = Properties.Resources.vista_ScrollHorzShaft;
        //[Category("Scroll")]
        //[Description("水平滚动条的轨道样式")]
        //public Bitmap HorizontalTrackBitmap {
        //    get { return _HorizontalTrackBitmap; }
        //    set {
        //        _HorizontalTrackBitmap = value;
        //        PreScroll();
        //    }
        //}

        //private Bitmap _VerticalArrowBitmap = Properties.Resources.vista_ScrollVertArrow;
        //[Category("Scroll")]
        //[Description("垂直滚动条的箭头样式")]
        //public Bitmap VerticalArrowBitmap {
        //    get { return _VerticalArrowBitmap; }
        //    set {
        //        _VerticalArrowBitmap = value;
        //        PreScroll();
        //    }
        //}

        //private Bitmap _VerticalThumbBitmap = Properties.Resources.vista_ScrollVertThumb;
        //[Category("Scroll")]
        //[Description("垂直滚动条的滑块样式")]
        //public Bitmap VerticalThumbBitmap {
        //    get { return _VerticalThumbBitmap; }
        //    set {
        //        _VerticalThumbBitmap = value;
        //        PreScroll();
        //    }
        //}

        //private Bitmap _VerticalTrackBitmap = Properties.Resources.vista_ScrollVertShaft;
        //[Category("Scroll")]
        //[Description("垂直滚动条的轨道样式")]
        //public Bitmap VerticalTrackBitmap {
        //    get { return _VerticalTrackBitmap; }
        //    set {
        //        _VerticalTrackBitmap = value;
        //        PreScroll();
        //    }
        //}

        //private Bitmap _Mask = Properties.Resources.vista_ScrollHorzArrow;
        //[Category("Scroll")]
        //[Description("滚动条的右下方块样式")]
        //public Bitmap Mask {
        //    get { return _Mask; }
        //    set {
        //        _Mask = value;
        //        PreScroll();
        //    }
        //}

        private Color _borderColor = Color.FromArgb(55, 126, 168);
        [DefaultValue(typeof(Color), "55, 126, 168")]
        [Category("Skin")]
        [Description("边框颜色")]
        public Color BorderColor {
            get { return _borderColor; }
            set {
                _borderColor = value;
                base.Invalidate(true);
            }
        }

        [DefaultValue(typeof(Color), "White")]
        [Category("Skin")]
        [Description("Item渐变行颜色1")]
        public Color RowBackColor1 {
            get { return _rowBackColor1; }
            set {
                _rowBackColor1 = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "254, 216, 249")]
        [Category("Skin")]
        [Description("Item渐变行颜色2")]
        public Color RowBackColor2 {
            get { return _rowBackColor2; }
            set {
                _rowBackColor2 = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "75, 188, 254")]
        [Category("Skin")]
        [Description("Item选中颜色")]
        public Color SelectedColor {
            get { return _selectedColor; }
            set {
                _selectedColor = value;
                base.Invalidate();
            }
        }

        [DefaultValue(typeof(Color), "75, 188, 254")]
        [Category("Skin")]
        [Description("标题颜色")]
        public Color HeadColor {
            get { return _headColor; }
            set {
                _headColor = value;
                base.Invalidate(true);
            }
        }

        private IntPtr HeaderWnd {
            get {
                return new IntPtr(NativeMethods.SendMessage(
                base.Handle, LVM_GETHEADER, 0, 0));
            }
        }

        private int ColumnCount {
            get {
                return NativeMethods.SendMessage(
                    HeaderWnd, HDM_GETITEMCOUNT, 0, 0);
            }
        }
        #endregion

        #region 重新赋值绘画滚动条
        //ScrollBarHelper _ScrollBarHelper;
        ////重新赋值绘画滚动条
        //public void PreScroll() {
        //    if (_ScrollBarHelper != null) {
        //        _ScrollBarHelper.HorizontalArrowGraphic = HorizontalArrowBitmap;
        //        _ScrollBarHelper.HorizontalThumbGraphic = HorizontalThumbBitmap;
        //        _ScrollBarHelper.HorizontalTrackGraphic = HorizontalTrackBitmap;
        //        _ScrollBarHelper.VerticalArrowGraphic = VerticalArrowBitmap;
        //        _ScrollBarHelper.VerticalThumbGraphic = VerticalThumbBitmap;
        //        _ScrollBarHelper.VerticalTrackGraphic = VerticalTrackBitmap;
        //        _ScrollBarHelper.TransitionGraphic = Mask;
        //    }
        //}
        #endregion

        #region 重载事件
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

        protected override void OnDrawColumnHeader(
            DrawListViewColumnHeaderEventArgs e) {
            base.OnDrawColumnHeader(e);

            Graphics g = e.Graphics;
            Rectangle bounds = e.Bounds;

            Color baseColor = _headColor;
            Color borderColor = _headColor;
            Color innerBorderColor = Color.FromArgb(150, 255, 255, 255);

            if (e.ColumnIndex != 0) {
                bounds.X--;
                bounds.Width++;
            }

            RenderBackgroundInternal(
                g,
                bounds,
                baseColor,
                borderColor,
                innerBorderColor,
                0.45f,
                true,
                LinearGradientMode.Vertical);

            if (e.ColumnIndex != 0) {
                bounds.X++;
                bounds.Width--;
            }

            TextFormatFlags flags = GetFormatFlags(e.Header.TextAlign);
            Rectangle textRect = new Rectangle(
                       bounds.X + 3,
                       bounds.Y,
                       bounds.Width - 6,
                       bounds.Height); ;

            if (e.Header.ImageList != null) {
                Image image = e.Header.ImageIndex == -1 ?
                    null : e.Header.ImageList.Images[e.Header.ImageIndex];
                if (image != null) {
                    Rectangle imageRect = new Rectangle(
                        bounds.X + 3,
                        bounds.Y + 2,
                        bounds.Height - 4,
                        bounds.Height - 4);
                    g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                    g.DrawImage(image, imageRect);

                    textRect.X = imageRect.Right + 3;
                    textRect.Width -= imageRect.Width;
                }
            }
            TextRenderer.DrawText(
                   g,
                   e.Header.Text,
                   e.Font,
                   textRect,
                   e.ForeColor,
                   flags);
        }

        protected override void OnDrawItem(
            DrawListViewItemEventArgs e) {
            base.OnDrawItem(e);
            DrawItemInternal(e);
        }

        protected override void OnDrawSubItem(
            DrawListViewSubItemEventArgs e) {
            base.OnDrawSubItem(e);
            if (e.ItemIndex == -1) {
                return;
            }

            switch (base.View) {
                case View.Details:
                    DrawSubItemDetails(e);
                    break;
            }
        }

        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case WM.WM_NCPAINT:
                    WmNcPaint(ref m);
                    break;
                case WM.WM_WINDOWPOSCHANGED:
                    base.WndProc(ref m);
                    IntPtr result = m.Result;
                    WmNcPaint(ref m);
                    m.Result = result;
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
        #endregion

        #region 滚动条
        cTreeView ctv;
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (!DesignMode) {
                SetupScrollBars();
            }
            if (_headerNativeWindow == null) {
                if (HeaderWnd != IntPtr.Zero) {
                    _headerNativeWindow = new HeaderNativeWindow(this);
                }
            }
        }

        private void SetupScrollBars() {
            ctv = new cTreeView(this.Handle,
            ScrollBarDrawImage.ScrollHorzShaft,
            ScrollBarDrawImage.ScrollHorzArrow,
            ScrollBarDrawImage.ScrollHorzThumb,
            ScrollBarDrawImage.ScrollVertShaft,
            ScrollBarDrawImage.ScrollVertArrow,
            ScrollBarDrawImage.ScrollVertThumb,
            ScrollBarDrawImage.Fader);
        }
        protected override void OnHandleDestroyed(EventArgs e) {
            base.OnHandleDestroyed(e);
            try {
                if (!DesignMode) {
                    if (ctv != null) {
                        ctv.Dispose();
                    }
                }
                if (_headerNativeWindow != null) {
                    _headerNativeWindow.Dispose();
                    _headerNativeWindow = null;
                }
            } catch { }
        }
        #endregion

        #region Windows消息事件
        private Rectangle AbsoluteClientRectangle {
            get {
                RECT absoluteClientRECT = AbsoluteClientRECT;

                Rectangle rect = Rectangle.FromLTRB(
                    absoluteClientRECT.Left,
                    absoluteClientRECT.Top,
                    absoluteClientRECT.Right,
                    absoluteClientRECT.Bottom);
                CreateParams cp = base.CreateParams;
                bool bHscroll = (cp.Style &
                    WS.WS_HSCROLL) != 0;
                bool bVscroll = (cp.Style &
                    WS.WS_VSCROLL) != 0;

                if (bHscroll) {
                    rect.Height += SystemInformation.HorizontalScrollBarHeight;
                }

                if (bVscroll) {
                    rect.Width += SystemInformation.VerticalScrollBarWidth;
                }

                return rect;
            }
        }

        private RECT AbsoluteClientRECT {
            get {
                RECT lpRect = new RECT();
                CreateParams createParams = CreateParams;
                NativeMethods.AdjustWindowRectEx(
                    ref lpRect,
                    createParams.Style,
                    false,
                    createParams.ExStyle);
                int left = -lpRect.Left;
                int right = -lpRect.Top;
                NativeMethods.GetClientRect(
                    base.Handle,
                    ref lpRect);

                lpRect.Left += left;
                lpRect.Right += left;
                lpRect.Top += right;
                lpRect.Bottom += right;
                return lpRect;
            }
        }

        private void WmNcPaint(ref Message m) {
            base.WndProc(ref m);
            if (base.BorderStyle == BorderStyle.None) {
                return;
            }

            IntPtr hDC = NativeMethods.GetWindowDC(m.HWnd);
            if (hDC == IntPtr.Zero) {
                throw new Win32Exception();
            }
            try {
                Color backColor = BackColor;
                Color borderColor = _borderColor;

                Rectangle bounds = new Rectangle(0, 0, Width, Height);
                using (Graphics g = Graphics.FromHdc(hDC)) {
                    using (Region region = new Region(bounds)) {
                        region.Exclude(AbsoluteClientRectangle);
                        using (Brush brush = new SolidBrush(backColor)) {
                            g.FillRegion(brush, region);
                        }
                    }

                    ControlPaint.DrawBorder(
                        g,
                        bounds,
                        borderColor,
                        ButtonBorderStyle.Solid);
                }
            } finally {
                NativeMethods.ReleaseDC(m.HWnd, hDC);
            }
            m.Result = IntPtr.Zero;
        }

        #endregion

        #region Draw Methods

        internal void DrawItemInternal(DrawListViewItemEventArgs e) {
            if (base.View == View.Details) {
                return;
            }

            Graphics g = e.Graphics;
            ListViewItem item = e.Item;
            ListViewItemStates itemState = e.State;
            ImageList imageList = item.ImageList;

            e.DrawBackground();

            bool bDrawImage =
                (imageList != null) &&
                (item.ImageIndex != -1 ||
                !string.IsNullOrEmpty(item.ImageKey));
            bool bSelected =
                (itemState & ListViewItemStates.Selected) != 0;

            if (bDrawImage) {
                DrawImage(g, item, bSelected);
            }

            if (!string.IsNullOrEmpty(item.Text)) {
                Rectangle textRect =
                    item.GetBounds(ItemBoundsPortion.Label);
                DrawBackground(g, e.ItemIndex, textRect, bSelected);
                DrawText(g, item, textRect, bSelected);
            }
        }

        private void DrawImage(
            Graphics g,
            ListViewItem item,
            bool selected) {
            ImageList imageList = item.ImageList;
            Size imageSize = imageList.ImageSize;
            Rectangle imageRect = item.GetBounds(ItemBoundsPortion.Icon);

            if (imageRect.Width > imageSize.Width) {
                imageRect.X += (imageRect.Width - imageSize.Width) / 2;
                imageRect.Width = imageSize.Width;
            }

            if (imageRect.Height > imageSize.Height) {
                imageRect.Y += (imageRect.Height - imageSize.Height) / 2;
                imageRect.Height = imageSize.Height;
            }

            int imageIndex =
                item.ImageIndex != -1 ?
                item.ImageIndex :
                imageList.Images.IndexOfKey(item.ImageKey);

            if (selected) {
                IntPtr hIcon = NativeMethods.ImageList_GetIcon(
                    imageList.Handle,
                    imageIndex,
                    (int)NativeMethods.ImageListDrawFlags.ILD_SELECTED);
                g.DrawIcon(Icon.FromHandle(hIcon), imageRect);
                NativeMethods.DestroyIcon(hIcon);
            } else {
                Image image = imageList.Images[imageIndex];

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

        private void DrawBackground(
            Graphics g,
            int itemIndex,
            Rectangle rect,
            bool selected) {
            switch (base.View) {
                case View.SmallIcon:
                case View.List:
                    rect.Inflate(-1, 0);
                    break;
            }

            if (selected) {
                Color baseColor = _selectedColor;
                Color borderColor = _selectedColor;
                Color innerBorderColor =
                    Color.FromArgb(150, 255, 255, 255);

                RenderBackgroundInternal(
                    g,
                    rect,
                    baseColor,
                    borderColor,
                    innerBorderColor,
                    0.45f,
                    true,
                    LinearGradientMode.Vertical);
            } else {
                if (base.View == View.List) {
                    Color backColor = itemIndex % 2 == 0 ?
                        _rowBackColor1 : _rowBackColor2;

                    using (SolidBrush brush = new SolidBrush(backColor)) {
                        g.FillRectangle(brush, rect);
                    }
                }
            }
        }

        private void DrawText(
            Graphics g,
            ListViewItem item,
            Rectangle textRect,
            bool selected) {
            StringFormat sf = StringFormat.GenericTypographic;
            switch (base.View) {
                case View.LargeIcon:
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Near;
                    sf.Trimming = StringTrimming.EllipsisCharacter;
                    sf.FormatFlags &= ~(StringFormatFlags.LineLimit);
                    if (selected) {
                        sf.FormatFlags &= ~StringFormatFlags.NoWrap;
                    }
                    break;
                case View.List:
                case View.SmallIcon:
                    textRect.Inflate(-1, 0);
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Trimming = StringTrimming.EllipsisCharacter;
                    sf.FormatFlags &= ~(StringFormatFlags.LineLimit);
                    break;
                case View.Tile:
                    textRect.Inflate(-2, 0);
                    sf.Alignment = StringAlignment.Near;
                    sf.LineAlignment = StringAlignment.Center;
                    sf.Trimming = StringTrimming.EllipsisCharacter;
                    sf.FormatFlags |= StringFormatFlags.NoWrap;
                    break;
            }

            if (base.View != View.Tile ||
                item.SubItems.Count == 1) {
                using (Brush brush = new SolidBrush(item.ForeColor)) {
                    g.DrawString(
                        item.Text,
                        item.Font,
                        brush,
                        textRect,
                        sf);
                }
            } else {
                string subItemText = "A";
                int height;
                bool bBreak = false;
                Rectangle sunItemTextRect = textRect;
                foreach (ListViewItem.ListViewSubItem subItem in item.SubItems) {
                    if (!string.IsNullOrEmpty(subItem.Text)) {
                        subItemText = subItem.Text;
                    }

                    height = TextRenderer.MeasureText(
                        g, subItem.Text, subItem.Font).Height;
                    sunItemTextRect.Height = height;
                    if (sunItemTextRect.Bottom > textRect.Bottom) {
                        sunItemTextRect.Height =
                            textRect.Bottom - sunItemTextRect.Y;
                        bBreak = true;
                    }

                    using (Brush brush = new SolidBrush(subItem.ForeColor)) {
                        g.DrawString(
                            subItemText,
                            subItem.Font,
                            brush,
                            sunItemTextRect,
                            sf);
                    }
                    sunItemTextRect.Y += height;
                    if (bBreak) {
                        break;
                    }
                }
            }
        }

        private void DrawSubItemDetails(
            DrawListViewSubItemEventArgs e) {
            Rectangle bounds = e.Bounds;
            ListViewItemStates itemState = e.ItemState;
            Graphics g = e.Graphics;
            ListViewItem item = e.Item;
            bool bSelected =
                (itemState & ListViewItemStates.Selected) != 0;

            bool bDrawImage = false;
            bool bFistItem = false;
            int imageIndex = -1;

            if (e.ColumnIndex == 0) {
                bFistItem = true;
                if (item.ImageList != null) {
                    if (item.ImageIndex != -1) {
                        imageIndex = item.ImageIndex;
                    } else if (!string.IsNullOrEmpty(item.ImageKey)) {
                        imageIndex =
                            item.ImageList.Images.IndexOfKey(item.ImageKey);
                    }

                    if (imageIndex != -1) {
                        bDrawImage = true;
                    }
                }
            }

            Rectangle backRect = bounds;
            Rectangle imageRect = Rectangle.Empty;
            if (bDrawImage) {
                imageRect = item.GetBounds(ItemBoundsPortion.Icon);
                backRect = item.GetBounds(ItemBoundsPortion.Label);
                backRect.X += 2;
                backRect.Width -= 2;
            }

            if (bSelected &&
                (base.FullRowSelect ||
                (!base.FullRowSelect && bFistItem))) {
                backRect.Height--;
                Color baseColor = _selectedColor;
                Color borderColor = _selectedColor;
                Color innerBorderColor = Color.FromArgb(150, 255, 255, 255);

                RenderBackgroundInternal(
                    g,
                    backRect,
                    baseColor,
                    borderColor,
                    innerBorderColor,
                    0.45f,
                    false,
                    LinearGradientMode.Vertical);

                if (!base.FullRowSelect && bFistItem) {
                    backRect.Width--;
                    using (Pen pen = new Pen(borderColor)) {
                        g.DrawRectangle(pen, backRect);
                    }
                    backRect.Width++;
                } else {
                    Point[] points = new Point[4];
                    points[0] = new Point(backRect.X, backRect.Y);
                    points[1] = new Point(backRect.Right, backRect.Y);
                    points[2] = new Point(backRect.Right, backRect.Bottom);
                    points[3] = new Point(backRect.X, backRect.Bottom);
                    using (Pen pen = new Pen(borderColor)) {
                        if (bFistItem) {
                            g.DrawLine(pen, points[0], points[1]);
                            g.DrawLine(pen, points[0], points[3]);
                            g.DrawLine(pen, points[2], points[3]);
                        }
                        if (e.ColumnIndex == ColumnCount - 1) {
                            points[1].X--;
                            points[2].X--;
                            g.DrawLine(pen, points[0], points[1]);
                            g.DrawLine(pen, points[1], points[2]);
                            g.DrawLine(pen, points[2], points[3]);
                        } else {
                            g.DrawLine(pen, points[0], points[1]);
                            g.DrawLine(pen, points[2], points[3]);
                        }
                    }
                }
            } else {
                Color backColor = e.ItemIndex % 2 == 0 ?
                _rowBackColor1 : _rowBackColor2;

                using (SolidBrush brush = new SolidBrush(backColor)) {
                    g.FillRectangle(brush, backRect);
                }
            }

            TextFormatFlags flags = GetFormatFlags(e.Header.TextAlign);
            if (bDrawImage) {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                if (bSelected) {
                    IntPtr hIcon = NativeMethods.ImageList_GetIcon(
                        item.ImageList.Handle,
                        imageIndex,
                        (int)NativeMethods.ImageListDrawFlags.ILD_SELECTED);
                    g.DrawIcon(Icon.FromHandle(hIcon), imageRect);
                    NativeMethods.DestroyIcon(hIcon);
                } else {
                    Image image = item.ImageList.Images[imageIndex];
                    g.DrawImage(
                        image,
                        imageRect,
                        0,
                        0,
                        image.Width,
                        image.Height,
                        GraphicsUnit.Pixel);
                }

                Rectangle textRect = new Rectangle(
                    imageRect.Right + 3,
                    bounds.Y,
                    bounds.Width - imageRect.Right - 3,
                    bounds.Height);

                TextRenderer.DrawText(
                    g,
                    item.Text,
                    item.Font,
                    textRect,
                    item.ForeColor,
                    flags);
            } else {
                bounds.X += 3;
                TextRenderer.DrawText(
                    g,
                    e.SubItem.Text,
                    e.SubItem.Font,
                    bounds,
                    e.SubItem.ForeColor,
                    flags);
            }
        }

        #endregion

        #region Draw Helper Methods

        protected TextFormatFlags GetFormatFlags(
            HorizontalAlignment align) {
            TextFormatFlags flags =
                    TextFormatFlags.EndEllipsis |
                    TextFormatFlags.VerticalCenter;

            switch (align) {
                case HorizontalAlignment.Center:
                    flags |= TextFormatFlags.HorizontalCenter;
                    break;
                case HorizontalAlignment.Right:
                    flags |= TextFormatFlags.Right;
                    break;
                case HorizontalAlignment.Left:
                    flags |= TextFormatFlags.Left;
                    break;
            }

            return flags;
        }

        internal void RenderBackgroundInternal(
            Graphics g,
            Rectangle rect,
            Color baseColor,
            Color borderColor,
            Color innerBorderColor,
            float basePosition,
            bool drawBorder,
            LinearGradientMode mode) {
            if (drawBorder) {
                rect.Width--;
                rect.Height--;
            }

            if (rect.Width <= 0 || rect.Height <= 0) {
                return;
            }

            using (LinearGradientBrush brush = new LinearGradientBrush(
               rect, Color.Transparent, Color.Transparent, mode)) {
                Color[] colors = new Color[4];
                colors[0] = GetColor(baseColor, 0, 35, 24, 9);
                colors[1] = GetColor(baseColor, 0, 13, 8, 3);
                colors[2] = baseColor;
                colors[3] = GetColor(baseColor, 0, 68, 69, 54);

                ColorBlend blend = new ColorBlend();
                blend.Positions = new float[] {
                    0.0f, basePosition, basePosition + 0.05f, 1.0f };
                blend.Colors = colors;
                brush.InterpolationColors = blend;
                g.FillRectangle(brush, rect);
            }
            if (baseColor.A > 80) {
                Rectangle rectTop = rect;
                if (mode == LinearGradientMode.Vertical) {
                    rectTop.Height = (int)(rectTop.Height * basePosition);
                } else {
                    rectTop.Width = (int)(rect.Width * basePosition);
                }
                using (SolidBrush brushAlpha =
                    new SolidBrush(Color.FromArgb(80, 255, 255, 255))) {
                    g.FillRectangle(brushAlpha, rectTop);
                }
            }

            if (drawBorder) {
                using (Pen pen = new Pen(borderColor)) {
                    g.DrawRectangle(pen, rect);
                }

                rect.Inflate(-1, -1);
                using (Pen pen = new Pen(innerBorderColor)) {
                    g.DrawRectangle(pen, rect);
                }
            }
        }

        private Color GetColor(
            Color colorBase, int a, int r, int g, int b) {
            int a0 = colorBase.A;
            int r0 = colorBase.R;
            int g0 = colorBase.G;
            int b0 = colorBase.B;

            if (a + a0 > 255) { a = 255; } else { a = Math.Max(a + a0, 0); }
            if (r + r0 > 255) { r = 255; } else { r = Math.Max(r + r0, 0); }
            if (g + g0 > 255) { g = 255; } else { g = Math.Max(g + g0, 0); }
            if (b + b0 > 255) { b = 255; } else { b = Math.Max(b + b0, 0); }

            return Color.FromArgb(a, r, g, b);
        }

        #endregion

        #region Helper Methods

        private int ColumnAtIndex(int column) {
            NativeMethods.HDITEM hd = new NativeMethods.HDITEM();
            hd.mask = HDI_ORDER;
            for (int i = 0; i < ColumnCount; i++) {
                if (NativeMethods.SendMessage(
                    HeaderWnd, HDM_GETITEMA, column, ref hd) != IntPtr.Zero) {
                    return hd.iOrder;
                }
            }
            return 0;
        }

        private Rectangle HeaderEndRect() {
            RECT rect = new RECT();
            IntPtr headerWnd = HeaderWnd;
            NativeMethods.SendMessage(
                headerWnd, HDM_GETITEMRECT, ColumnAtIndex(ColumnCount - 1), ref rect);
            int left = rect.Right;
            NativeMethods.GetWindowRect(headerWnd, ref rect);
            NativeMethods.OffsetRect(ref rect, -rect.Left, -rect.Top);
            rect.Left = left;

            return Rectangle.FromLTRB(rect.Left, rect.Top, rect.Right, rect.Bottom);
        }

        #endregion

        #region HeaderNativeWindow Class

        private class HeaderNativeWindow
            : NativeWindow, IDisposable
        {
            private SkinListView _owner;

            public HeaderNativeWindow(SkinListView owner)
                : base() {
                _owner = owner;
                base.AssignHandle(owner.HeaderWnd);
            }

            protected override void WndProc(ref Message m) {
                base.WndProc(ref m);
                if (m.Msg == 0xF || m.Msg == 0x47) {
                    IntPtr hdc = NativeMethods.GetDC(m.HWnd);
                    try {
                        using (Graphics g = Graphics.FromHdc(hdc)) {
                            Rectangle bounds = _owner.HeaderEndRect();
                            Color baseColor = _owner.HeadColor;
                            Color borderColor = _owner.HeadColor;
                            Color innerBorderColor = Color.FromArgb(150, 255, 255, 255);
                            if (_owner.ColumnCount > 0) {
                                bounds.X--;
                                bounds.Width++;
                            }
                            _owner.RenderBackgroundInternal(
                                g,
                                bounds,
                                baseColor,
                                borderColor,
                                innerBorderColor,
                                0.45f,
                                true,
                                LinearGradientMode.Vertical);
                        }
                    } finally {
                        NativeMethods.ReleaseDC(m.HWnd, hdc);
                    }
                }
            }

            #region IDisposable 成员

            public void Dispose() {
                ReleaseHandle();
                _owner = null;
            }

            #endregion
        }

        #endregion
    }
}
