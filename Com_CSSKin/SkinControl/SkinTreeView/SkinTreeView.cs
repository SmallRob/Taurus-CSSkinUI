
using Com_CSSkin.Win32;
using Com_CSSkin.Win32.Const;
using Com_CSSkin.Win32.Struct;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(TreeView))]
    public partial class SkinTreeView : TreeView
    {
        public SkinTreeView() {
            InitializeComponent();
            // 开启双缓冲
            base.SetStyle(
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.ResizeRedraw |
              ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.EnableNotifyMessage, true);
            base.UpdateStyles();
            ////参数初始化
            //this.DrawMode = TreeViewDrawMode.Normal;
            //this.FullRowSelect = true;
            //this.ItemHeight = 23;
            //this.HotTracking = true;
            //this.ShowLines = true;
        }
        #region 属性
        //private Color drawTextColor = Color.FromArgb(81, 81, 81);
        //[DefaultValue(typeof(Color), "81, 81, 81")]
        //[Category("Skin")]
        //[Description("字体颜色")]
        //public Color DrawTextColor {
        //    get { return drawTextColor; }
        //    set { drawTextColor = value; }
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
        #endregion

        //#region 重载事件
        //protected override void OnDrawNode(DrawTreeNodeEventArgs e) {
        //    base.OnDrawNode(e);
        //    Graphics g = e.Graphics;
        //    //节点背景绘制
        //    if (e.Node.IsSelected) {
        //        g.FillRectangle(new SolidBrush(Color.FromArgb(56, 195, 238)), e.Bounds);
        //    } else if ((e.State & TreeNodeStates.Hot) != 0)//|| currentMouseMoveNode == e.Node)
        //    {
        //        g.FillRectangle(new SolidBrush(Color.FromArgb(202, 238, 234)), e.Bounds);
        //    } else {
        //        e.Graphics.FillRectangle(Brushes.White, e.Bounds);
        //    }

        //    SolidBrush ArroBs = new SolidBrush(Color.Black);
        //    //节点头图标绘制
        //    if (e.Node.IsExpanded) {
        //        g.FillPolygon(ArroBs, new Point[] { 
        //                new Point(e.Node.Bounds.X-10 + 2, e.Node.Bounds.Y + 11), 
        //                new Point(e.Node.Bounds.X-10 + 12, e.Node.Bounds.Y + 11), 
        //                new Point(e.Node.Bounds.X-10 + 7,e.Node.Bounds.Y + 16) });

        //    } else if (e.Node.IsExpanded == false && e.Node.Nodes.Count > 0) {
        //        g.FillPolygon(ArroBs, new Point[] { 
        //                new Point(e.Node.Bounds.X-10 + 5, e.Node.Bounds.Y + 8), 
        //                new Point(e.Node.Bounds.X-10 + 5, e.Node.Bounds.Y + 18), 
        //                new Point(e.Node.Bounds.X-10 + 10, e.Node.Bounds.Y + 13) });
        //    }

        //    //文本绘制
        //    using (Font foreFont = new Font(this.Font, FontStyle.Regular))
        //    using (Brush drawTextBrush = new SolidBrush(drawTextColor)) {
        //        e.Graphics.DrawString(e.Node.Text, foreFont, drawTextBrush, e.Node.Bounds.Left + 5, e.Node.Bounds.Top + 5);
        //    }
        //}

        //protected override void OnMouseDoubleClick(MouseEventArgs e) {
        //    base.OnMouseDoubleClick(e);
        //    //TreeNode tn = this.GetNodeAt(e.Location);
        //    ////调整【点击测试区域】大小，包括图标
        //    //Rectangle bounds = new Rectangle(tn.Bounds.Left - 12, tn.Bounds.Y, tn.Bounds.Width - 5, tn.Bounds.Height);
        //    //if (tn != null && bounds.Contains(e.Location) == false) {
        //    //    if (tn.IsExpanded == false)
        //    //        tn.Expand();
        //    //    else
        //    //        tn.Collapse();
        //    //}
        //}

        //protected override void OnMouseClick(MouseEventArgs e) {
        //    base.OnMouseClick(e);
        //    //TreeNode tn = this.GetNodeAt(e.Location);
        //    //this.SelectedNode = tn;
        //}

        //TreeNode currentNode = null;
        //protected override void OnMouseMove(MouseEventArgs e) {
        //    base.OnMouseMove(e);
        //    //TreeNode tn = this.GetNodeAt(e.Location);
        //    //Graphics g = this.CreateGraphics();
        //    //if (currentNode != tn) {
        //    //    //绘制当前节点的hover背景
        //    //    if (tn != null)
        //    //        OnDrawNode(new DrawTreeNodeEventArgs(g, tn, new Rectangle(0, tn.Bounds.Y, this.Width, tn.Bounds.Height), TreeNodeStates.Hot));

        //    //    //取消之前hover的节点背景
        //    //    if (currentNode != null)
        //    //        OnDrawNode(new DrawTreeNodeEventArgs(g, currentNode, new Rectangle(0, currentNode.Bounds.Y, this.Width, currentNode.Bounds.Height), TreeNodeStates.Default));
        //    //}
        //    //currentNode = tn;
        //    //g.Dispose();
        //}


        //protected override void OnMouseLeave(EventArgs e) {
        //    base.OnMouseLeave(e);
        //    ////移出控件时取消Hover背景
        //    //if (currentNode != null) {
        //    //    Graphics g = this.CreateGraphics();
        //    //    OnDrawNode(new DrawTreeNodeEventArgs(g, currentNode, new Rectangle(0, currentNode.Bounds.Y, this.Width, currentNode.Bounds.Height), TreeNodeStates.Default));
        //    //}
        //}
        //#endregion

        #region 滚动条
        cTreeView ctv;
        protected override void OnHandleCreated(EventArgs e) {
            base.OnHandleCreated(e);
            if (!DesignMode) {
                SetupScrollBars();
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
            if (!DesignMode) {
                try {
                    if (ctv != null) {
                        ctv.Dispose();
                    }
                } catch { }
            }
        }
        #endregion

        #region Windows消息事件
        //private Boolean elv = false;
        protected override void WndProc(ref Message m) {
            switch (m.Msg) {
                case 15:
                case WM.WM_NCPAINT:
                    WmNcPaint(ref m);
                    //Paint event Areo效果
                    //if (!elv && !this.DesignMode) {
                    //    //1-time run needed
                    //    NativeMethods.SetWindowTheme(this.Handle, "explorer", null); //Explorer style
                    //    NativeMethods.SendMessage(this.Handle, NativeMethods.LVM_SETEXTENDEDLISTVIEWSTYLE, NativeMethods.LVS_EX_DOUBLEBUFFER, NativeMethods.LVS_EX_DOUBLEBUFFER); //Blue selection, keeps other extended styles
                    //    elv = true;
                    //}
                    break;
                case 0x0014: // 禁掉清除背景消息
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

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
    }
}
