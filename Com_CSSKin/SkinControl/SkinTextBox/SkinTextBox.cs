
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Com_CSSkin.SkinClass;
using System.Drawing.Drawing2D;
using System.Windows.Forms.Design;
using System.ComponentModel.Design;

namespace Com_CSSkin.SkinControl
{
    //[Designer(typeof(SkinTextBoxDesigner))]
    //[Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(IDesigner))]
    [ToolboxBitmap(typeof(TextBox))]
    public partial class SkinTextBox : Panel
    {
        public SkinTextBox()
        {
            InitializeComponent();
            //初始化
            Init();
            this.InitEvents();
            this.BackColor = Color.Transparent;
        }
        #region 初始化
        public void Init()
        {
            this.SetStyle(
                    ControlStyles.UserPaint |
                    ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 加载事件
        /// </summary>
        private void InitEvents()
        {
            //为SkinTetxBox添加悬浮离开移动事件
            this.MouseEnter += new EventHandler(BaseText_MouseEnter);
            this.MouseLeave += new EventHandler(BaseText_MouseLeave);
            this.MouseMove += new MouseEventHandler(BaseText_MouseMove);
            //为文本框添加悬浮离开移动事件
            this.BaseText.MouseEnter += new EventHandler(BaseText_MouseEnter);
            this.BaseText.MouseLeave += new EventHandler(BaseText_MouseLeave);
            this.BaseText.MouseMove += new MouseEventHandler(BaseText_MouseMove);
            this.BaseText.MouseDown += BaseText_MouseDown;
        }

        /// <summary>
        /// 偏移文本框
        /// </summary>
        bool flag = false;
        protected void PositionTextBox()
        {
            if (this._icon != null && !flag)
            {
                this.Padding = new Padding(this.Padding.Left, this.Padding.Top, this.Padding.Right + 23, this.Padding.Bottom);
                flag = true;
            }
            else if (this._icon == null && flag)
            {
                this.Padding = new Padding(this.Padding.Left, this.Padding.Top, this.Padding.Right - 23, this.Padding.Bottom);
                flag = false;
            }
        }
        #endregion

        #region 自定义事件
        public event EventHandler IconClick;
        private void OnIconClick()
        {
            if (this.IconClick != null)
                this.IconClick(this, EventArgs.Empty);
        }
        #endregion

        #region 变量
        private Cursor _cursor = Cursors.IBeam;
        private ControlState _mouseState = ControlState.Normal;
        private ControlState _iconMouseState = ControlState.Normal;
        private Bitmap mouseback;
        private Bitmap normlback;
        private Bitmap downback;
        private bool _iconIsButton;
        private Image _icon;
        #endregion

        #region 属性
        #region 文本

        [Description("指定可以在编辑控件中输入的最大字符数。"), Category("行为")]
        public int MaxLength
        {
            get { return this.BaseText.MaxLength; }
            set { this.BaseText.MaxLength = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("控件编辑控件的文本是否能够跨越多行。"), Category("行为")]
        public bool Multiline
        {
            get { return this.BaseText.Multiline; }
            set { this.BaseText.Multiline = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("指示将为单行编辑控件的密码输入显示的字符。"), Category("行为")]
        public char IsPasswordChat
        {
            get { return this.BaseText.PasswordChar; }
            set { this.BaseText.PasswordChar = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("控制能否更改编辑控件中的文本。"), Category("行为")]
        public bool ReadOnly
        {
            get { return this.BaseText.ReadOnly; }
            set { this.BaseText.ReadOnly = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("指示编辑控件中的文本是否以默认的密码字符显示。"), Category("行为")]
        public bool IsSystemPasswordChar
        {
            get { return this.BaseText.UseSystemPasswordChar; }
            set { this.BaseText.UseSystemPasswordChar = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("指示多行编辑控件是否自动换行。"), Category("行为")]
        public bool WordWrap
        {
            get { return this.BaseText.WordWrap; }
            set { this.BaseText.WordWrap = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("用于显示控件中文本的字体。"), Category("外观")]
        new public Font Font
        {
            get { return this.BaseText.Font; }
            set { this.BaseText.Font = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("此组件的前景色，用于显示文本。"), Category("外观")]
        new public Color ForeColor
        {
            get { return this.BaseText.ForeColor; }
            set { this.BaseText.ForeColor = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("多行编辑中的文本行，作为字符串值的数组。"), Category("外观")]
        public string[] Lines
        {
            get { return this.BaseText.Lines; }
            set { this.BaseText.Lines = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("指示对于多行编辑控件，将为此控件显示哪些滚动条。"), Category("外观")]
        public ScrollBars ScrollBars
        {
            get { return this.BaseText.ScrollBars; }
            set { this.BaseText.ScrollBars = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("指示应该如何对齐编辑控件的文本。"), Category("外观")]
        public HorizontalAlignment TextAlign
        {
            get { return this.BaseText.TextAlign; }
            set { this.BaseText.TextAlign = value; }
        }
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override string Text
        {
            get
            {
                return BaseText.Text;
            }
            set
            {
                BaseText.Text = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [Description("水印文字"), Category("Skin")]
        public string WaterText
        {
            get { return this.BaseText.WaterText; }
            set { this.BaseText.WaterText = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        [Description("水印颜色"), Category("Skin")]
        public Color WaterColor
        {
            get { return this.BaseText.WaterColor; }
            set { this.BaseText.WaterColor = value; }
        }
        #endregion

        private StopStates stopState = StopStates.NoStop;
        //停止当前状态
        [Category("Skin")]
        [Description("停止当前状态")]
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public StopStates StopState
        {
            get { return stopState; }
            set
            {
                stopState = value;
                base.Invalidate();
            }
        }

        [Description("透明水印文本框。"), Category("TextBox"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public SkinWaterTextBox SkinTxt
        {
            get { return this.BaseText; }
            set { this.BaseText = value; }
        }

        [Description("悬浮时背景框。"), Category("Skin")]
        public Bitmap MouseBack
        {
            get { return mouseback; }
            set
            {
                if (mouseback != value)
                {
                    mouseback = value;
                    this.Invalidate();
                }
            }
        }

        [Description("正常状态时背景框。"), Category("Skin")]
        public Bitmap NormlBack
        {
            get { return normlback; }
            set
            {
                if (normlback != value)
                {
                    normlback = value;
                    this.Invalidate();
                }
            }
        }

        [Description("按下时背景框。"), Category("Skin")]
        public Bitmap DownBack
        {
            get { return downback; }
            set
            {
                if (downback != value)
                {
                    downback = value;
                    this.Invalidate();
                }
            }
        }

        [Description("文本框的图标"), Category("Skin")]
        public Image Icon
        {
            get { return this._icon; }
            set
            {
                if (this._icon != value)
                {
                    this._icon = value;
                    base.Invalidate(this.IconRect);
                    this.PositionTextBox();
                }
            }
        }

        [Description("文本框的图标是否是按钮"), Category("Skin")]
        public bool IconIsButton
        {
            get { return this._iconIsButton; }
            set { this._iconIsButton = value; }
        }

        public override Cursor Cursor
        {
            get { return this._cursor; }
            set { this._cursor = value; }
        }

        /// <summary>
        /// 当前文本框状态
        /// </summary>
        public ControlState MouseState
        {
            get { return this._mouseState; }
            set
            {
                this._mouseState = value;
                base.Invalidate();
            }
        }

        /// <summary>
        /// 当前ICO图标状态
        /// </summary>
        public ControlState IconMouseState
        {
            get { return this._iconMouseState; }
            set
            {
                this._iconMouseState = value;
                base.Invalidate();
            }
        }
        /// <summary>
        /// 图标的绘制区域
        /// </summary>
        protected Rectangle IconRect
        {
            get { return new Rectangle(this.Width - 23, 3, 20, 20); }
        }
        #endregion

        #region 事件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BaseText_MouseLeave(object sender, EventArgs e)
        {
            this.MouseState = ControlState.Normal;
            this.Invalidate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BaseText_MouseEnter(object sender, EventArgs e)
        {
            this.MouseState = ControlState.Hover;
            this.Invalidate();
        }

        void BaseText_MouseMove(object sender, MouseEventArgs e)
        {
            if (BaseText.Focused)
            {
                this.MouseState = ControlState.Pressed;
            }
            else
            {
                this.MouseState = ControlState.Hover;
            }
        }
        #endregion

        #region 重载事件
        protected override void OnEnter(EventArgs e)
        {
            this.SkinTxt.Focus();
            base.OnEnter(e);
        }

        protected override void OnClick(EventArgs e)
        {
            this.SkinTxt.Focus();
            base.OnClick(e);
        }

        /// <summary>
        /// 当文本框的大小发生改变时，将文本框的类型换成多行文本
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (this.Height > 28)
            {
                this.BaseText.Multiline = true;
            }
            else
            {
                this.BaseText.Multiline = false;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Bitmap mouse = MouseBack == null ? Properties.Resources.frameBorderEffect_mouseDownDraw : MouseBack;
            Bitmap norml = NormlBack == null ? Properties.Resources.frameBorderEffect_normalDraw : NormlBack;
            //停止当前状态
            if (StopState != StopStates.NoStop)
            {
                _mouseState = (ControlState)StopState;
            }
            Bitmap DownBtm = DownBack == null ? mouse : DownBack;
            Bitmap btm = this._mouseState == ControlState.Pressed ? DownBtm : this._mouseState == ControlState.Hover ? mouse : norml;
            if (btm != null)
            {
                DrawHelper.RendererBackground(g, this.ClientRectangle, btm, true);
            }
            if (this._icon != null)
            {
                Rectangle iconRect = this.IconRect;
                if (this._iconMouseState == ControlState.Pressed)
                {
                    iconRect.X += 1;
                    iconRect.Y += 1;
                }
                g.DrawImage(this._icon, iconRect, 0, 0, this._icon.Width, this._icon.Height, GraphicsUnit.Pixel);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this.MouseState = ControlState.Hover;
            this.Invalidate();

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (this._icon != null && this.IconRect.Contains(e.Location))
            {
                if (this._iconIsButton)
                {
                    this.Cursor = Cursors.Hand;
                }
                else
                {
                    this.Cursor = Cursors.IBeam;
                }
            }
            else
            {
                this.Cursor = Cursors.IBeam;
            }
        }

        /// <summary>
        /// 文本框的按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BaseText_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.MouseState = ControlState.Pressed;
                this.Invalidate();
            }
        }

        /// <summary>
        /// 文本框边框的按下事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left)
            {
                if (this._icon != null && this._iconIsButton)
                {
                    if (this.IconRect.Contains(e.Location))
                    {
                        this.IconMouseState = ControlState.Pressed;
                    }
                }
                this.MouseState = ControlState.Pressed;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (this._icon != null && this._iconIsButton)
            {
                this.IconMouseState = ControlState.Hover;
                if (e.Button == MouseButtons.Left && this.IconRect.Contains(e.Location))
                    this.OnIconClick();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this.MouseState = ControlState.Normal;
            this.Invalidate();
        }
        #endregion
    }

    class SkinTextBoxDesigner : ParentControlDesigner
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            SkinTextBox uc = component as SkinTextBox;
            this.EnableDesignMode(uc.SkinTxt, "BaseText");
        }
    }
}
