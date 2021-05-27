
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Com_CSSkin.SkinControl.Internals;
using System.IO;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Drawing.Imaging;

namespace Com_CSSkin.SkinControl
{
    public enum ChatBoxContextMenuMode
    {
        None = 0,
        ForInput,
        ForOutput
    }

    /// <summary>
    /// 支持图片和动画的RichTextBox。
    /// </summary>
    [ToolboxBitmap(typeof(RichTextBox))]
    public class SkinChatRichTextBox : RichTextBox
    {
        #region 变量
        private Dictionary<uint, Image> defaultEmotionDictionary = new Dictionary<uint, Image>();
        private ContextMenuStrip contextMenuStrip1;
        private IContainer components;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ContextMenuStrip contextMenuStrip2;
        private ToolStripMenuItem toolStripMenuItem3;
        private ToolStripMenuItem toolStripMenuItem4;
        private ToolStripMenuItem toolStripMenuItem5; //表情图片在内置列表中的index - emotion
        private Image imageOnRightClick = null;
        private ToolStripMenuItem toolStripMenuItem6;
        private ContextMenuStrip contextMenuStrip3;
        private ToolStripMenuItem toolStripMenuItem7;
        public delegate void CbGeneric<T>(T obj);
        #endregion

        #region 自定义事件
        /// <summary>
        /// 当文件（夹）拖放到控件内时，触发此事件。参数：文件路径的集合。
        /// </summary>
        [Description("当文件（夹）拖放到控件内时，触发此事件。")]
        [Category("Skin")]
        public event CbGeneric<string[]> FileOrFolderDragDrop;
        #endregion

        #region 属性
        #region ContextMenuMode
        private ChatBoxContextMenuMode contextMenuMode = ChatBoxContextMenuMode.ForInput;
        private ContextMenuStrip contextMenuStrip4;
        private ToolStripMenuItem toolStripMenuItem8;
        private ToolStripMenuItem toolStripMenuItem9;
        /// <summary>
        /// 快捷菜单的模式。
        /// </summary>
        [Description("快捷菜单的模式。")]
        [Category("Skin")]
        [DefaultValue(typeof(ChatBoxContextMenuMode), "ForInput")]
        public ChatBoxContextMenuMode ContextMenuMode {
            get { return contextMenuMode; }
            set { contextMenuMode = value; }
        }
        #endregion

        #region PopoutImageWhenDoubleClick
        private bool popoutImageWhenDoubleClick = true;
        /// <summary>
        /// 双击图片时，是否弹出图片。
        /// </summary>
        [Description("双击图片时，是否弹出图片预览窗。")]
        [Category("Skin")]
        [DefaultValue(typeof(bool), "true")]
        public bool PopoutImageWhenDoubleClick {
            get { return popoutImageWhenDoubleClick; }
            set { popoutImageWhenDoubleClick = value; }
        }
        #endregion

        bool isTrank = false;
        /// <summary>
        /// 背景是否透明
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(bool), "false")]
        [Description("背景是否透明。")]
        public bool IsTrank {
            get { return isTrank; }
            set { isTrank = value; }
        }

        //透明背景
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr LoadLibrary(string lpFileName);
        protected override CreateParams CreateParams {
            get {
                CreateParams prams = base.CreateParams;
                if (IsTrank) {
                    if (LoadLibrary("msftedit.dll") != IntPtr.Zero) {
                        prams.ExStyle |= 0x020; // transparent 
                        prams.ClassName = "RICHEDIT50W";
                    }
                }
                return prams;
            }
        }
        #endregion

        #region 重载方法
        //控件首次创建时被调用-滚动条美化
        protected override void OnCreateControl() {
            ScrollBarHelper _ScrollBarHelper = new ScrollBarHelper(
               Handle,
               Properties.Resources.vista_ScrollHorzShaft,
               Properties.Resources.vista_ScrollHorzArrow,
               Properties.Resources.vista_ScrollHorzThumb,
               Properties.Resources.vista_ScrollVertShaft,
               Properties.Resources.vista_ScrollVertArrow,
               Properties.Resources.vista_ScrollVertThumb,
               Properties.Resources.vista_ScrollHorzArrow
               );
            base.OnCreateControl();
        }
        #endregion

        #region 插入自定义链接变量封装 Interop-Defines
        [StructLayout(LayoutKind.Sequential)]
        private struct CHARFORMAT2_STRUCT
        {
            public UInt32 cbSize;
            public UInt32 dwMask;
            public UInt32 dwEffects;
            public Int32 yHeight;
            public Int32 yOffset;
            public Int32 crTextColor;
            public byte bCharSet;
            public byte bPitchAndFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public char[] szFaceName;
            public UInt16 wWeight;
            public UInt16 sSpacing;
            public int crBackColor; // Color.ToArgb() -> int
            public int lcid;
            public int dwReserved;
            public Int16 sStyle;
            public Int16 wKerning;
            public byte bUnderlineType;
            public byte bAnimation;
            public byte bRevAuthor;
            public byte bReserved1;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        private const int WM_USER = 0x0400;
        private const int EM_GETCHARFORMAT = WM_USER + 58;
        private const int EM_SETCHARFORMAT = WM_USER + 68;

        private const int SCF_SELECTION = 0x0001;
        private const int SCF_WORD = 0x0002;
        private const int SCF_ALL = 0x0004;

        #region CHARFORMAT2 Flags
        private const UInt32 CFE_BOLD = 0x0001;
        private const UInt32 CFE_ITALIC = 0x0002;
        private const UInt32 CFE_UNDERLINE = 0x0004;
        private const UInt32 CFE_STRIKEOUT = 0x0008;
        private const UInt32 CFE_PROTECTED = 0x0010;
        private const UInt32 CFE_LINK = 0x0020;
        private const UInt32 CFE_AUTOCOLOR = 0x40000000;
        private const UInt32 CFE_SUBSCRIPT = 0x00010000;		/* Superscript and subscript are */
        private const UInt32 CFE_SUPERSCRIPT = 0x00020000;		/*  mutually exclusive			 */

        private const int CFM_SMALLCAPS = 0x0040;			/* (*)	*/
        private const int CFM_ALLCAPS = 0x0080;			/* Displayed by 3.0	*/
        private const int CFM_HIDDEN = 0x0100;			/* Hidden by 3.0 */
        private const int CFM_OUTLINE = 0x0200;			/* (*)	*/
        private const int CFM_SHADOW = 0x0400;			/* (*)	*/
        private const int CFM_EMBOSS = 0x0800;			/* (*)	*/
        private const int CFM_IMPRINT = 0x1000;			/* (*)	*/
        private const int CFM_DISABLED = 0x2000;
        private const int CFM_REVISED = 0x4000;

        private const int CFM_BACKCOLOR = 0x04000000;
        private const int CFM_LCID = 0x02000000;
        private const int CFM_UNDERLINETYPE = 0x00800000;		/* Many displayed by 3.0 */
        private const int CFM_WEIGHT = 0x00400000;
        private const int CFM_SPACING = 0x00200000;		/* Displayed by 3.0	*/
        private const int CFM_KERNING = 0x00100000;		/* (*)	*/
        private const int CFM_STYLE = 0x00080000;		/* (*)	*/
        private const int CFM_ANIMATION = 0x00040000;		/* (*)	*/
        private const int CFM_REVAUTHOR = 0x00008000;


        private const UInt32 CFM_BOLD = 0x00000001;
        private const UInt32 CFM_ITALIC = 0x00000002;
        private const UInt32 CFM_UNDERLINE = 0x00000004;
        private const UInt32 CFM_STRIKEOUT = 0x00000008;
        private const UInt32 CFM_PROTECTED = 0x00000010;
        private const UInt32 CFM_LINK = 0x00000020;
        private const UInt32 CFM_SIZE = 0x80000000;
        private const UInt32 CFM_COLOR = 0x40000000;
        private const UInt32 CFM_FACE = 0x20000000;
        private const UInt32 CFM_OFFSET = 0x10000000;
        private const UInt32 CFM_CHARSET = 0x08000000;
        private const UInt32 CFM_SUBSCRIPT = CFE_SUBSCRIPT | CFE_SUPERSCRIPT;
        private const UInt32 CFM_SUPERSCRIPT = CFM_SUBSCRIPT;

        private const byte CFU_UNDERLINENONE = 0x00000000;
        private const byte CFU_UNDERLINE = 0x00000001;
        private const byte CFU_UNDERLINEWORD = 0x00000002; /* (*) displayed as ordinary underline	*/
        private const byte CFU_UNDERLINEDOUBLE = 0x00000003; /* (*) displayed as ordinary underline	*/
        private const byte CFU_UNDERLINEDOTTED = 0x00000004;
        private const byte CFU_UNDERLINEDASH = 0x00000005;
        private const byte CFU_UNDERLINEDASHDOT = 0x00000006;
        private const byte CFU_UNDERLINEDASHDOTDOT = 0x00000007;
        private const byte CFU_UNDERLINEWAVE = 0x00000008;
        private const byte CFU_UNDERLINETHICK = 0x00000009;
        private const byte CFU_UNDERLINEHAIRLINE = 0x0000000A; /* (*) displayed as ordinary underline	*/

        #endregion

        #endregion

        #region 插入自定义链接
        /// <summary>
        /// 插入一个指定链接文本到richtextbox。
        /// </summary>
        /// <param name="text">要插入的文本</param>
        public void InsertLink(string text) {
            InsertLink(text, this.SelectionStart);
        }

        /// <summary>
        /// 在一个指定位置插入链接文本到richtextbox。
        /// </summary>
        /// <param name="text">要插入的文本</param>
        /// <param name="position">插入位置</param>
        public void InsertLink(string text, int position) {
            if (position < 0 || position > this.Text.Length)
                throw new ArgumentOutOfRangeException("position");

            this.SelectionStart = position;
            this.SelectedText = text;
            this.Select(position, text.Length);
            this.SetSelectionLink(true);
            this.Select(position + text.Length, 0);
        }

        /// <summary>
        /// 在当前指定位置插入超链接字符串和超链接到richtextbox。
        /// </summary>
        /// <param name="text">要插入的文本</param>
        /// <param name="hyperlink">要插入的超链接字符串</param>
        public void InsertLink(string text, string hyperlink) {
            InsertLink(text, hyperlink, this.SelectionStart);
        }

        /// <summary>
        /// 在一个指定位置插入超链接字符串和超链接到richtextbox。
        /// </summary>
        /// <param name="text">要插入的文本</param>
        /// <param name="hyperlink">要插入的超链接字符串</param>
        /// <param name="position">插入位置</param>
        public void InsertLink(string text, string hyperlink, int position) {
            if (position < 0 || position > this.Text.Length)
                throw new ArgumentOutOfRangeException("position");

            this.SelectionStart = position;
            this.SelectedRtf = @"{\rtf1\ansi " + text + @"\v #" + hyperlink + @"\v0}";
            this.Select(position, text.Length + hyperlink.Length + 1);
            this.SetSelectionLink(true);
            this.Select(position + text.Length + hyperlink.Length + 1, 0);
        }

        /// <summary>
        /// 设置当前选择的链接样式
        /// </summary>
        /// <param name="link">true: 设置链接风格, false: 清除链接风格</param>
        public void SetSelectionLink(bool link) {
            SetSelectionStyle(CFM_LINK, link ? CFE_LINK : 0);
        }
        /// <summary>
        /// 获取当前选择的链接样式
        /// </summary>
        /// <returns>0: 没有设置链接风格, 1: 链接样式设置, -1: 混合</returns>
        public int GetSelectionLink() {
            return GetSelectionStyle(CFM_LINK, CFE_LINK);
        }


        private void SetSelectionStyle(UInt32 mask, UInt32 effect) {
            CHARFORMAT2_STRUCT cf = new CHARFORMAT2_STRUCT();
            cf.cbSize = (UInt32)Marshal.SizeOf(cf);
            cf.dwMask = mask;
            cf.dwEffects = effect;

            IntPtr wpar = new IntPtr(SCF_SELECTION);
            IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lpar, false);

            IntPtr res = SendMessage(Handle, EM_SETCHARFORMAT, wpar, lpar);

            Marshal.FreeCoTaskMem(lpar);
        }

        private int GetSelectionStyle(UInt32 mask, UInt32 effect) {
            CHARFORMAT2_STRUCT cf = new CHARFORMAT2_STRUCT();
            cf.cbSize = (UInt32)Marshal.SizeOf(cf);
            cf.szFaceName = new char[32];

            IntPtr wpar = new IntPtr(SCF_SELECTION);
            IntPtr lpar = Marshal.AllocCoTaskMem(Marshal.SizeOf(cf));
            Marshal.StructureToPtr(cf, lpar, false);

            IntPtr res = SendMessage(Handle, EM_GETCHARFORMAT, wpar, lpar);

            cf = (CHARFORMAT2_STRUCT)Marshal.PtrToStructure(lpar, typeof(CHARFORMAT2_STRUCT));

            int state;
            // dwMask holds the information which properties are consistent throughout the selection:
            if ((cf.dwMask & mask) == mask) {
                if ((cf.dwEffects & effect) == effect)
                    state = 1;
                else
                    state = 0;
            } else {
                state = -1;
            }

            Marshal.FreeCoTaskMem(lpar);
            return state;
        }
        #endregion

        #region 无参构造
        public SkinChatRichTextBox() {
            this.InitializeComponent();
            // 开启双缓冲
            base.SetStyle(
             ControlStyles.AllPaintingInWmPaint |
             ControlStyles.OptimizedDoubleBuffer |
             ControlStyles.ResizeRedraw |
             ControlStyles.EnableNotifyMessage |
             ControlStyles.DoubleBuffer, true);
            this.AllowDrop = false;
            this.DragDrop += new DragEventHandler(textBoxSend_DragDrop);
            this.DragEnter += new DragEventHandler(textBoxSend_DragEnter);
            this.KeyDown += new KeyEventHandler(ChatBox_KeyDown);
            this.MouseDown += new MouseEventHandler(ChatBox_MouseDown);
            this.MouseMove += SkinChatRichTextBox_MouseMove;
            this.SizeChanged += new EventHandler(ChatBox_SizeChanged);
            this.DoubleClick += new EventHandler(ChatBox_DoubleClick);
            this.LinkClicked += new LinkClickedEventHandler(ChatBox_LinkClicked);
        }
        #endregion

        #region 文本框内的控件操作 - 选中的控件、获取文本框内所有控件
        /// <summary>
        /// 选中的控件
        /// </summary>
        public Control SelectControl { get; set; }
        /// <summary>
        /// 选中的控件的索引位置点Point
        /// </summary>
        public Point SelectControlPoint { get; set; }
        /// <summary>
        /// 选中的控件的索引位置
        /// </summary>
        public int SelectControlIndex { get; set; }

        private List<Control> listControl = new List<Control>();
        /// <summary>
        /// 获取文本框内所有控件
        /// </summary>
        public List<Control> ListControl {
            get {
                listControl.Clear();
                List<REOBJECT> list = this.RichEditOle.GetAllREOBJECT();
                for (int i = 0; i < list.Count; i++) {
                    Control c = (Control)Marshal.GetObjectForIUnknown(list[i].poleobj);
                    listControl.Add(c);
                }
                return listControl;
            }
        }
        #endregion

        #region GetControl获取文本框内控件
        /// <summary>
        /// 根据Point位置获取文本框内控件
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="selectTarget"></param>
        /// <returns></returns>
        private Control GetControl(Point pt, bool selectTarget) {
            int index = this.GetCharIndexFromPosition(pt);
            Point origin = this.GetPositionFromCharIndex(index);
            Control ctrl = null;
            bool backOne = false;
            List<REOBJECT> list = this.RichEditOle.GetAllREOBJECT();
            for (int i = 0; i < list.Count; i++) {
                if (list[i].posistion == index || list[i].posistion + 1 == index) {
                    //赋值到选中的控件
                    SelectControl = ctrl = (Control)Marshal.GetObjectForIUnknown(list[i].poleobj);
                    if (list[i].posistion + 1 == index) {
                        origin = new Point(origin.X - ctrl.Width, origin.Y);
                        backOne = true;
                    }
                    //赋值索引位置
                    SelectControlIndex = backOne ? index - 1 : index;
                    //赋值到选中控件的索引位置点Point
                    SelectControlPoint = origin;
                    break;
                }
            }

            if (ctrl == null) {
                return null;
            }

            Rectangle rect = new Rectangle(origin.X, origin.Y, ctrl.Width, ctrl.Height);
            if (!rect.Contains(pt)) {
                return null;
            }

            if (selectTarget) {
                this.Select(backOne ? index - 1 : index, 1);
            }

            return ctrl;
        }
        #endregion

        #region SkinChatRichTextBox_MouseMove
        void SkinChatRichTextBox_MouseMove(object sender, MouseEventArgs e) {
            //非点击不触发
            if (e.Button == MouseButtons.None) {
                return;
            }
            //获取选中的组件
            Control ScControl = this.GetControl(e.Location, false);
            if (ScControl != null) {
                //索引位置点
                Point indexP = this.SelectControlPoint;
                //实际位置点
                int realityX = e.Location.X - indexP.X;
                int realityY = e.Location.Y - indexP.Y;
                //光标在控件上的位置
                Point newP = new Point(realityX, realityY);
                if (ScControl is RichTxtControl) {
                    RichTxtControl ucfile = (RichTxtControl)ScControl;
                    ucfile.CtrlMouseMove(this, newP, e);
                } else {
                }
            }
        }
        #endregion

        #region ChatBox_MouseDown
        void ChatBox_MouseDown(object sender, MouseEventArgs e) {
            //获取选中的组件
            Control ScControl = this.GetControl(e.Location, true);
            if (ScControl != null) {
                //索引位置点
                Point indexP = this.SelectControlPoint;
                //实际位置点
                int realityX = e.Location.X - indexP.X;
                int realityY = e.Location.Y - indexP.Y;
                //光标在控件上的位置
                Point newP = new Point(realityX, realityY);
                if (ScControl is RichTxtControl) {
                    RichTxtControl ucfile = (RichTxtControl)ScControl;
                    ucfile.CtrlMouseDown(this, newP, e);
                }
                //MessageBox.Show("Test");
            }
            //菜单
            if (e.Button != System.Windows.Forms.MouseButtons.Right) {
                return;
            }
            if (this.contextMenuMode == ChatBoxContextMenuMode.None) {
                this.ContextMenuStrip = null;
                return;
            } else {
                //如果是GifBox默认执行右键菜单
                if (SelectControl is GifBox) {
                    GifBox box = SelectControl == null ? null : (GifBox)SelectControl;
                    if (this.contextMenuMode == ChatBoxContextMenuMode.ForInput) {
                        if (box == null) {
                            this.imageOnRightClick = null;
                        } else {
                            if (string.IsNullOrEmpty(this.SelectedText.Trim())) {
                                this.imageOnRightClick = box.Image;
                            } else {
                                this.imageOnRightClick = null;
                            }
                        }
                        this.toolStripMenuItem9.Visible = !string.IsNullOrEmpty(this.SelectedText);
                        this.toolStripMenuItem1.Visible = !this.ReadOnly;
                        this.ContextMenuStrip = this.contextMenuStrip1;
                        return;
                    } else if (this.contextMenuMode == ChatBoxContextMenuMode.ForOutput) {
                        if (box == null) {
                            if (!string.IsNullOrEmpty(this.SelectedText)) {
                                this.imageOnRightClick = null;
                                this.ContextMenuStrip = this.contextMenuStrip4;
                                return;
                            }
                            this.imageOnRightClick = null;
                            this.ContextMenuStrip = this.contextMenuStrip3;
                            return;
                        }
                        this.imageOnRightClick = box.Image;
                        this.ContextMenuStrip = this.contextMenuStrip2;
                    }
                }
            }
        }
        #endregion

        #region ChatBox_LinkClicked
        void ChatBox_LinkClicked(object sender, LinkClickedEventArgs e) {
            if (e.LinkText.StartsWith("www.") || e.LinkText.StartsWith("http")) {
                System.Diagnostics.Process.Start(e.LinkText);
            }
        }
        #endregion

        #region ChatBox_KeyDown
        void ChatBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.V) {
                if (Clipboard.ContainsImage()) {
                    Image img = Clipboard.GetImage();
                    if (img != null) {
                        this.InsertImage(img);
                    }
                    e.Handled = true;
                    return;
                }
            }
        }
        #endregion

        #region ChatBox_SizeChanged
        void ChatBox_SizeChanged(object sender, EventArgs e) {
            if (this.RichEditOle == null) {
                return;
            }
            List<REOBJECT> list = this.RichEditOle.GetAllREOBJECT();
            for (int i = 0; i < list.Count; i++) {
                Control c = (Control)Marshal.GetObjectForIUnknown(list[i].poleobj);
                if (c is GifBox) {
                    GifBox box = (GifBox)c;
                    box.Size = this.ComputeGifBoxSize(box.Image.Size);
                }
            }
        }
        #endregion

        #region ChatBox_DoubleClick
        void ChatBox_DoubleClick(object sender, EventArgs arg) {
            try {
                if (!this.popoutImageWhenDoubleClick) {
                    return;
                }
                MouseEventArgs e = arg as MouseEventArgs;
                if (e == null) {
                    return;
                }
                //获取选中的组件
                Control SelectControl = this.GetControl(e.Location, true);
                //如果是GifBox默认执行图片预览窗
                if (SelectControl is GifBox) {
                    GifBox box = SelectControl == null ? null : (GifBox)SelectControl;
                    if (box == null) {
                        return;
                    }
                    //如果存在Index,用
                    if (box.Tag != null) {
                        int SelectIndex = 0;
                        List<Image> listImg = new List<Image>();
                        foreach (var item in this.GetContent().ForeignImageDictionary) {
                            if (uint.Parse(box.Tag.ToString()) == item.Key) {
                                SelectIndex = listImg.Count;
                            }
                            listImg.Add(item.Value);
                        }
                        IntPtr handle = Com_CSSkin.Win32.NativeMethods.FindWindow(null, "图片预览窗");
                        if (handle != IntPtr.Zero) {
                            FrmPrintscreen frm = (FrmPrintscreen)Form.FromHandle(handle);
                            frm.ListImg = listImg;
                            frm.Index = SelectIndex;
                            frm.Activate();
                        } else {
                            //图片预览窗
                            FrmPrintscreen frm = new FrmPrintscreen(listImg, SelectIndex);
                            frm.Show(this);
                        }
                    } else {
                        ImageForm form = new ImageForm(box.Image);
                        form.Show();
                    }
                }
            } catch (Exception ee) {
                MessageBox.Show(ee.Message);
            }
        }

        private string GetPathToSave(string title, string defaultName, string iniDir) {
            string extendName = Path.GetExtension(defaultName);
            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = string.Format("The Files (*{0})|*{0}", extendName);
            saveDlg.FileName = defaultName;
            saveDlg.InitialDirectory = iniDir;
            saveDlg.OverwritePrompt = false;
            if (title != null) {
                saveDlg.Title = title;
            }

            DialogResult res = saveDlg.ShowDialog();
            if (res == DialogResult.OK) {
                return saveDlg.FileName;
            }

            return null;
        }
        #endregion

        #region textBoxSend_DragEnter
        void textBoxSend_DragEnter(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                e.Effect = DragDropEffects.Copy;
            } else {
                e.Effect = DragDropEffects.None;
            }
        }
        #endregion

        #region textBoxSend_DragDrop
        void textBoxSend_DragDrop(object sender, DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[] fileOrDirs = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (fileOrDirs == null || fileOrDirs.Length == 0) {
                    return;
                }

                if (this.FileOrFolderDragDrop != null) {
                    this.FileOrFolderDragDrop(fileOrDirs);
                }
            }
        }
        #endregion

        #region RichEditOle
        private RichEditOle richEditOle;
        private RichEditOle RichEditOle {
            get {
                if (richEditOle == null) {
                    if (base.IsHandleCreated) {
                        richEditOle = new RichEditOle(this);
                    }
                }

                return richEditOle;
            }
        }
        #endregion

        #region Initialize
        public void Initialize(Dictionary<uint, Image> defaultEmotions) {
            this.defaultEmotionDictionary = defaultEmotions ?? new Dictionary<uint, Image>();
        }
        #endregion

        #region InsertControl插入控件、InsertImage插入图片 、InsertDefaultEmotion插入表情
        /// <summary>
        /// 插入控件到末尾
        /// </summary>
        /// <param name="_oCtrl">控件</param>
        public void InsertControl(Control _oCtrl) {
            this.RichEditOle.InsertControl(_oCtrl);
        }
        /// <summary>
        /// 插入控件到指定位置
        /// </summary>
        /// <param name="_oCtrl">控件</param>
        /// <param name="_nPostion">位置</param>
        /// <param name="dwUser">Tag</param>
        public void InsertControl(Control _oCtrl, int _nPostion, uint dwUser) {
            this.RichEditOle.InsertControl(_oCtrl, _nPostion, dwUser);
        }
        /// <summary>
        /// 插入表情
        /// </summary>
        /// <param name="emotionID">表情ID</param>
        public void InsertDefaultEmotion(uint emotionID) {
            this.InsertDefaultEmotion(emotionID, this.TextLength);
        }

        /// <summary>
        /// 在position位置处，插入系统内置表情。
        /// </summary>      
        /// <param name="position">插入的位置</param>
        /// <param name="emotionID">表情图片在内置列表中的index</param>
        public void InsertDefaultEmotion(uint emotionID, int position) {
            try {
                Image image = this.defaultEmotionDictionary[emotionID];
                GifBox gif = new GifBox();
                gif.Cursor = Cursors.Hand;
                gif.BackColor = base.BackColor;
                gif.Size = this.ComputeGifBoxSize(image.Size);
                gif.Image = image;
                this.RichEditOle.InsertControl(gif, position, emotionID);
            } catch (Exception ee) {
                MessageBox.Show(ee.Message);
            }
        }
        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="image">图片</param>
        /// <returns></returns>
        public GifBox InsertImage(Image image) {
            return this.InsertImage(image, this.TextLength);
        }

        /// <summary>
        /// 在position位置处，插入图片。
        /// </summary>   
        /// <param name="image">要插入的图片</param>
        /// <param name="position">插入的位置</param>       
        public GifBox InsertImage(Image image, int position) {
            GifBox gif = new GifBox();
            try {
                gif.Cursor = Cursors.Hand;
                gif.BackColor = base.BackColor;
                gif.Size = this.ComputeGifBoxSize(image.Size);
                gif.Image = image;
                //gif.MouseDown += gif_MouseDown;
                gif.Tag = position;
                this.RichEditOle.InsertControl(gif, position, 10000);
            } catch (Exception ee) {
                MessageBox.Show(ee.Message);
            }
            return gif;
        }

        ///// <summary>
        ///// 双击图片进入预览窗体
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //void gif_MouseDown(object sender, MouseEventArgs e) {
        //    if (e.Clicks == 2 && e.Button == MouseButtons.Left) {
        //        GifBox gif = (GifBox)sender;
        //        int SelectIndex = 0;
        //        List<Image> listImg = new List<Image>();
        //        foreach (var item in this.GetContent().ForeignImageDictionary) {
        //            if (uint.Parse(gif.Tag.ToString()) == item.Key) {
        //                SelectIndex = listImg.Count;
        //            }
        //            listImg.Add(item.Value);
        //        }
        //        IntPtr handle = Com_CSSkin.Win32.NativeMethods.FindWindow(null, "图片预览窗");
        //        if (handle != IntPtr.Zero) {
        //            FrmPrintscreen frm = (FrmPrintscreen)Form.FromHandle(handle);
        //            frm.ListImg = listImg;
        //            frm.Index = SelectIndex;
        //            frm.Activate();
        //        } else {
        //            //图片预览窗
        //            FrmPrintscreen frm = new FrmPrintscreen(listImg, SelectIndex);
        //            frm.Show(this);
        //        }
        //    }
        //}

        private Size ComputeGifBoxSize(Size imgSize) {
            int maxWidth = this.Width - 20;

            if (imgSize.Width <= maxWidth) {
                return imgSize;
            }

            int newImgHeight = maxWidth * imgSize.Height / imgSize.Width; ;
            return new Size(maxWidth, newImgHeight);
        }
        #endregion

        #region AppendRtf
        public void AppendRtf(string _rtf) {
            try {
                base.Select(this.TextLength, 0);
                base.SelectedRtf = _rtf;
                base.Update();
                base.Select(this.Rtf.Length, 0);
            } catch (Exception ee) {
                MessageBox.Show(ee.Message);
            }
        }
        #endregion

        #region GetContent
        /// <summary>
        /// 获取Box中的所有内容。
        /// </summary>        
        /// <param name="containsForeignObject">内容中是否包含不是由IImagePathGetter管理的图片对象</param>
        /// <returns>key为位置，val为图片的ID</returns>
        public ChatBoxContent GetContent() {
            ChatBoxContent content = new ChatBoxContent(this.Text, this.Font, this.ForeColor);
            List<REOBJECT> list = this.RichEditOle.GetAllREOBJECT();
            for (int i = 0; i < list.Count; i++) {
                uint pos = (uint)list[i].posistion;
                content.PicturePositions.Add(pos);
                if (list[i].dwUser != 10000) {
                    content.AddEmotion(pos, list[i].dwUser);
                } else {
                    GifBox box = (GifBox)Marshal.GetObjectForIUnknown(list[i].poleobj);
                    content.AddForeignImage(pos, box.Image);
                }
            }

            return content;
        }
        #endregion

        #region AppendRichText
        /// <summary>
        /// 在现有内容后面追加富文本。
        /// </summary>      
        public void AppendRichText(string textContent, Font font, Color color) {
            try {
                int count = this.Text.Length;
                this.AppendText(textContent);

                this.Select(count, textContent.Length);
                if (color != null) {
                    this.SelectionColor = color;
                }
                if (font != null) {
                    this.SelectionFont = font;
                }
            } catch (Exception ee) {
                MessageBox.Show(ee.Message);
            }
        }
        #endregion

        #region AppendChatBoxContent
        public void AppendChatBoxContent(ChatBoxContent content) {
            try {
                if (content == null || content.Text == null) {
                    return;
                }

                int count = this.Text.Length;
                if (content.EmotionDictionary != null) {
                    string pureText = content.Text;
                    //去掉表情和图片的占位符
                    List<uint> emotionPosList = new List<uint>(content.EmotionDictionary.Keys);
                    List<uint> tempList = new List<uint>();
                    tempList.AddRange(emotionPosList);
                    foreach (uint key in content.ForeignImageDictionary.Keys) {
                        tempList.Add(key);
                    }
                    tempList.Sort();

                    for (int i = tempList.Count - 1; i >= 0; i--) {
                        pureText = pureText.Remove((int)tempList[i], 1);
                    }
                    this.AppendText(pureText);
                    //插入表情
                    for (int i = 0; i < tempList.Count; i++) {
                        uint position = tempList[i];
                        if (emotionPosList.Contains(position)) {
                            this.InsertDefaultEmotion(content.EmotionDictionary[position], (int)(count + position));
                        } else {
                            this.InsertImage(content.ForeignImageDictionary[position], (int)(count + position));
                        }
                    }
                } else {
                    this.AppendText(content.Text);
                }

                this.Select(count, content.Text.Length);
                if (content.Color != null) {
                    this.SelectionColor = content.Color;
                }
                if (content.Font != null) {
                    this.SelectionFont = content.Font;
                }
            } catch (Exception ee) {
                MessageBox.Show(ee.Message);
            }
        }
        #endregion

        #region Clear
        public new void Clear() {
            try {
                List<REOBJECT> list = this.RichEditOle.GetAllREOBJECT();
                for (int i = 0; i < list.Count; i++) {
                    if (list[i].dwUser == 10000) {
                        GifBox box = (GifBox)Marshal.GetObjectForIUnknown(list[i].poleobj);
                        box.Dispose();
                    }
                }
                base.Clear();
            } catch (Exception ee) {
                MessageBox.Show(ee.Message);
            }
        }
        #endregion

        #region InitializeComponent
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip4 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip3.SuspendLayout();
            this.contextMenuStrip4.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem9,
            this.toolStripMenuItem2,
            this.toolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 70);
            // 
            // toolStripMenuItem9
            // 
            this.toolStripMenuItem9.Name = "toolStripMenuItem9";
            this.toolStripMenuItem9.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem9.Text = "复制";
            this.toolStripMenuItem9.Click += new System.EventHandler(this.toolStripMenuItem9_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem2.Text = "粘贴";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.toolStripMenuItem1.Text = "插入图片";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem3,
            this.toolStripMenuItem4,
            this.toolStripMenuItem5,
            this.toolStripMenuItem6});
            this.contextMenuStrip2.Name = "contextMenuStrip2";
            this.contextMenuStrip2.Size = new System.Drawing.Size(137, 92);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItem3.Text = "复制";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItem4.Text = "另存为";
            this.toolStripMenuItem4.Click += new System.EventHandler(this.toolStripMenuItem4_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItem5.Text = "新窗口显示";
            this.toolStripMenuItem5.Click += new System.EventHandler(this.toolStripMenuItem5_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItem6.Text = "清屏";
            this.toolStripMenuItem6.Click += new System.EventHandler(this.toolStripMenuItem6_Click);
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem7});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(101, 26);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem7.Text = "清屏";
            this.toolStripMenuItem7.Click += new System.EventHandler(this.toolStripMenuItem7_Click);
            // 
            // contextMenuStrip4
            // 
            this.contextMenuStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem8});
            this.contextMenuStrip4.Name = "contextMenuStrip4";
            this.contextMenuStrip4.Size = new System.Drawing.Size(101, 26);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem8.Text = "复制";
            this.toolStripMenuItem8.Click += new System.EventHandler(this.toolStripMenuItem8_Click);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip3.ResumeLayout(false);
            this.contextMenuStrip4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        //复制
        void toolStripMenuItem9_Click(object sender, EventArgs e) {
            if (this.imageOnRightClick == null) {
                Clipboard.SetText(this.SelectedText);
            } else {
                Clipboard.SetImage(this.imageOnRightClick);
            }
        }

        void toolStripMenuItem8_Click(object sender, EventArgs e) {
            Clipboard.SetText(this.SelectedText);
        }

        void toolStripMenuItem7_Click(object sender, EventArgs e) {
            this.Clear();
        }

        void toolStripMenuItem6_Click(object sender, EventArgs e) {
            this.Clear();
        }

        void toolStripMenuItem5_Click(object sender, EventArgs e) {
            try {
                ImageForm form = new ImageForm(this.imageOnRightClick);
                form.Show();
            } catch (Exception ee) {
                MessageBox.Show(ee.Message);
            }
        }

        //保存图片
        void toolStripMenuItem4_Click(object sender, EventArgs e) {
            try {
                bool gif = ImageHelper.IsGif(this.imageOnRightClick);
                string postfix = gif ? "gif" : "jpg";
                string path = this.GetPathToSave("请选择保存路径", "image." + postfix, null);
                if (path == null) {
                    return;
                }
                ImageFormat format = gif ? ImageFormat.Gif : ImageFormat.Jpeg;
                ImageHelper.Save(this.imageOnRightClick, path, format);
                MessageBox.Show("成功保存图片。");
            } catch (Exception ee) {
                MessageBox.Show(ee.Message);
            }
        }

        //复制
        void toolStripMenuItem3_Click(object sender, EventArgs e) {
            try {
                Clipboard.SetImage(this.imageOnRightClick);
            } catch (Exception ee) {
                MessageBox.Show(ee.Message);
            }
        }

        //粘贴
        void toolStripMenuItem2_Click(object sender, EventArgs e) {
            if (!this.ReadOnly) {
                try {
                    if (Clipboard.ContainsImage()) {
                        Image img = Clipboard.GetImage();
                        if (img != null) {
                            this.InsertImage(img);
                        }
                        return;
                    }
                    this.Paste();
                } catch (Exception ee) {
                    MessageBox.Show(ee.Message);
                }
            }
        }

        //插入图片
        void toolStripMenuItem1_Click(object sender, EventArgs e) {
            if (!this.ReadOnly) {
                try {
                    string file = this.GetFileToOpen2("请选择图片", null, ".jpg", ".bmp", ".png", ".gif");
                    if (file == null) {
                        return;
                    }

                    Image img = Image.FromFile(file);
                    this.InsertImage(img);
                } catch (Exception ee) {
                    MessageBox.Show(ee.Message);
                }
            }
        }

        private string GetFileToOpen2(string title, string iniDir, params string[] extendNames) {
            StringBuilder filterBuilder = new StringBuilder("(");
            for (int i = 0; i < extendNames.Length; i++) {
                filterBuilder.Append("*");
                filterBuilder.Append(extendNames[i]);
                if (i < extendNames.Length - 1) {
                    filterBuilder.Append(";");
                } else {
                    filterBuilder.Append(")");
                }
            }
            filterBuilder.Append("|");
            for (int i = 0; i < extendNames.Length; i++) {
                filterBuilder.Append("*");
                filterBuilder.Append(extendNames[i]);
                if (i < extendNames.Length - 1) {
                    filterBuilder.Append(";");
                }
            }

            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = filterBuilder.ToString();
            openDlg.FileName = "";
            openDlg.InitialDirectory = iniDir;
            if (title != null) {
                openDlg.Title = title;
            }

            openDlg.CheckFileExists = true;
            openDlg.CheckPathExists = true;

            DialogResult res = openDlg.ShowDialog();
            if (res == DialogResult.OK) {
                return openDlg.FileName;
            }

            return null;
        }
        #endregion
    }

    #region ChatBoxContent
    [Serializable]
    public class ChatBoxContent
    {
        public ChatBoxContent() { }
        public ChatBoxContent(string _text, Font _font, Color c) {
            this.text = _text;
            this.font = _font;
            this.color = c;
        }

        #region Text
        private string text = "";
        /// <summary>
        /// 纯文本信息
        /// </summary>
        public string Text {
            get { return text; }
            set { text = value; }
        }
        #endregion

        #region Font
        private Font font;
        public Font Font {
            get { return font; }
            set { font = value; }
        }
        #endregion

        #region Color
        private Color color;
        public Color Color {
            get { return color; }
            set { color = value; }
        }
        #endregion

        #region ForeignImageDictionary
        private Dictionary<uint, Image> foreignImageDictionary = new Dictionary<uint, Image>();
        /// <summary>
        /// 非内置的表情图片。key - 在ChatBox中的位置。
        /// </summary>
        public Dictionary<uint, Image> ForeignImageDictionary {
            get { return foreignImageDictionary; }
            set { foreignImageDictionary = value; }
        }
        #endregion

        #region EmotionDictionary
        private Dictionary<uint, uint> emotionDictionary = new Dictionary<uint, uint>();
        /// <summary>
        /// 内置的表情图片。key - 在ChatBox中的位置 ，value - 表情图片在内置列表中的index。
        /// </summary>
        public Dictionary<uint, uint> EmotionDictionary {
            get { return emotionDictionary; }
            set { emotionDictionary = value; }
        }
        #endregion

        #region PicturePositions
        private List<uint> picturePositions = new List<uint>();
        /// <summary>
        /// 所有图片的位置。从小到大排列。
        /// </summary>
        public List<uint> PicturePositions {
            get { return picturePositions; }
            set { picturePositions = value; }
        }
        #endregion

        public bool IsEmpty() {
            return string.IsNullOrEmpty(this.text) && (this.foreignImageDictionary == null || this.foreignImageDictionary.Count == 0) && (this.emotionDictionary == null || this.emotionDictionary.Count == 0);
        }

        public bool ContainsForeignImage() {
            return this.foreignImageDictionary != null && this.foreignImageDictionary.Count > 0;
        }

        public void AddForeignImage(uint pos, Image img) {
            this.foreignImageDictionary.Add(pos, img);
        }

        public void AddEmotion(uint pos, uint emotionIndex) {
            this.emotionDictionary.Add(pos, emotionIndex);
        }

        public string GetTextWithPicPlaceholder(string placeholder) {
            if (this.picturePositions == null || this.picturePositions.Count == 0) {
                return this.Text;
            }

            string tmp = this.Text;
            for (int i = this.picturePositions.Count - 1; i >= 0; i--) {
                tmp = tmp.Insert((int)this.picturePositions[i], placeholder);
            }
            return tmp;
        }
    }
    #endregion

}
