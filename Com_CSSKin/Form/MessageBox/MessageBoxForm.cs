
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using Com_CSSkin.SkinControl;
using System.Drawing.Drawing2D;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin
{
    /// <summary>
    /// 实现用于消息框对话框的窗体。
    /// </summary>
    public class MessageBoxForm : CSSkinMain
    {
        #region 变量
        private MessageBoxArgs _message;
        private Rectangle _iconRect;
        private Rectangle _messageRect;
        private Button[] _innerButtons;
        private const int Spacing = 12;
        #endregion

        #region 无参构造函数
        /// <summary>
        /// 初始化 <see cref="MessageBoxForm"/> 类的新实例。
        /// </summary>
        public MessageBoxForm() {
            InitializeComponent();
        }
        #endregion

        #region 属性
        /// <summary>
        /// 获取消息对话框的参数对象。
        /// </summary>
        protected MessageBoxArgs Message {
            get { return _message; }
        }

        /// <summary>
        /// 获取消息文本的位置与大小。
        /// </summary>
        protected Rectangle MessageRect {
            get { return _messageRect; }
        }

        /// <summary>
        /// 获取消息图标的位置与大小。
        /// </summary>
        protected new Rectangle IconRect {
            get { return _iconRect; }
        }
        #endregion

        #region 重载事件
        protected override void OnShown(EventArgs e) {
            base.OnShown(e);
        }

        /// <summary>
        /// 使用 <see cref="MessageBoxArgs"/> 消息对话框参数显示窗体。
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public DialogResult ShowMessageBoxDialog(MessageBoxArgs message) {
            _message = message;
            return ShowMessageBoxDialog();
        }

        protected override void OnKeyPress(KeyPressEventArgs e) {
            //响应Ctrl+V案件消息，把消息文本拷贝到剪切板。
            if (e.KeyChar == '\x03') {
                e.Handled = true;
                Clipboard.SetDataObject(_message.Caption + Environment.NewLine +
                    Environment.NewLine + _message.Text, true);
            }
            base.OnKeyPress(e);
        }

        protected override void OnVisibleChanged(EventArgs e) {
            base.OnVisibleChanged(e);
            if (Visible && !ContainsFocus) {
                Activate();
            }
        }

        private void MessageBoxForm_Paint(object sender, PaintEventArgs e) {
            Graphics g = e.Graphics;
            //画渐变层
            DrawAlphaPart(this, g);
            //画图标
            if (Message.Icon != null) {
                g.DrawIcon(Message.Icon, _iconRect);
            }
            //画字
            if (!string.IsNullOrEmpty(Message.Text)) {
                TextFormatFlags flags =
                    TextFormatFlags.HidePrefix | TextFormatFlags.ExternalLeading |
                    TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis;
                TextRenderer.DrawText(g, Message.Text, Font, _messageRect, ForeColor, flags);
            }
        }

        //画渐变层
        public void DrawAlphaPart(Form form, Graphics g) {
            //LinearGradientBrush brush = new LinearGradientBrush(

            //    this.ClientRectangle, Color.Transparent,

            //     Color.White, LinearGradientMode.Vertical);

            //brush.SetSigmaBellShape(0.4f);
            //g.FillRectangle(brush, this.ClientRectangle);
            //brush.SetSigmaBellShape(0.6f);
            //g.FillRectangle(brush, this.ClientRectangle); 
            Color[] colors = 
            {
               Color.FromArgb(0, Color.White),
               Color.FromArgb(225,Color.White),
               Color.FromArgb(240, Color.White)
            };

            float[] pos = 
            {
                0.0f,
                0.38f,
                1.0f                       
            };

            ColorBlend colorBlend = new ColorBlend(3);
            colorBlend.Colors = colors;
            colorBlend.Positions = pos;

            int bottomHeight = 35;  //底部未渐变部分的高度
            RectangleF destRect = new RectangleF(0, 0, form.Width, form.Height - bottomHeight);

            //绘制上部白色渐变层
            using (LinearGradientBrush lBrushUp = new LinearGradientBrush(destRect, Color.White, Color.Black, LinearGradientMode.Vertical)) {
                lBrushUp.InterpolationColors = colorBlend;
                g.FillRectangle(lBrushUp, destRect);
            }
            //绘制中间白色分割条
            using (Pen whitePen = new Pen(Color.FromArgb(255, Color.White), 0.1f)) {
                g.DrawLine(whitePen, new Point(0, form.Height - bottomHeight), new Point(form.Width, form.Height - bottomHeight));
            }

            //绘制下部白色固体画刷层
            using (SolidBrush sBrushDown = new SolidBrush(Color.FromArgb(205, Color.White))) {
                g.FillRectangle(sBrushDown, new Rectangle(0, form.Height - bottomHeight + 1, form.Width, form.Height - bottomHeight + 1));
            }
        }

        /// <summary>
        /// 显示消息对话框。
        /// </summary>
        /// <param name="owner">任何实现 <see cref="IWin32Window"/>（表示将拥有模式对话框的顶级窗口）的对象。</param>
        /// <returns><see cref="DialogResult"/> 值之一。</returns>
        protected virtual DialogResult DoShowDialog(IWin32Window owner) {
            ShowDialog(owner);
            return DialogResult;
        }

        #endregion

        #region 实现方法
        DialogResult ShowMessageBoxDialog() {
            Text = Message.Caption;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MinimizeBox = false;
            MaximizeBox = false;
            IWin32Window owner = Message.Owner;

            if (owner == null) {
                Form activeForm = Form.ActiveForm;
                if (activeForm != null && !activeForm.InvokeRequired) {
                    owner = activeForm;
                }
            }

            if (owner != null) {
                Control ownerControl = owner as Control;
                if (ownerControl != null) {
                    if (!ownerControl.Visible) {
                        owner = null;
                    } else {
                        Form ownerForm = ownerControl.FindForm();
                        if (ownerForm != null) {
                            if ((!ownerForm.Visible)
                                || ownerForm.WindowState == FormWindowState.Minimized
                                || ownerForm.Right <= 0
                                || ownerForm.Bottom <= 0) {
                                owner = null;
                            }
                        }
                    }
                }
            }

            if (owner == null) {
                ShowInTaskbar = true;
                StartPosition = FormStartPosition.CenterScreen;
            } else {
                ShowInTaskbar = false;
                StartPosition = FormStartPosition.CenterParent;
            }

            CreateButtons();
            CalcIconBounds();
            CalcMessageBounds();
            CalcFinalSizes();

            Form frm = owner as Form;
            if (frm != null && frm.TopMost) {
                TopMost = true;
            }

            return DoShowDialog(owner);
        }

        SkinButton[] buttonArray = null;
        private void CreateButtons() {
            MessageBoxButtons btns = Message.Buttons;
            MessageBoxDefaultButton defaultBtn = Message.DefaultButton;
            buttonArray = null;
            switch (btns) {
                case MessageBoxButtons.OKCancel:
                    buttonArray = new SkinButton[] {
                        InnerCreateButton(DialogButton.BtnOk, DialogResult.OK), 
                        InnerCreateButton(DialogButton.BtnCancel, DialogResult.Cancel) };
                    base.CancelButton = buttonArray[1];
                    break;

                case MessageBoxButtons.AbortRetryIgnore:
                    buttonArray = new SkinButton[] { 
                        InnerCreateButton(DialogButton.BtnAbort, DialogResult.Abort),
                        InnerCreateButton(DialogButton.BtnRetry, DialogResult.Retry),
                        InnerCreateButton(DialogButton.BtnIgnore, DialogResult.Ignore) };
                    base.CancelButton = buttonArray[2];
                    break;
                case MessageBoxButtons.RetryCancel:
                    buttonArray = new SkinButton[] { 
                        InnerCreateButton(DialogButton.BtnRetry, DialogResult.Retry),
                        InnerCreateButton(DialogButton.BtnCancel, DialogResult.Cancel) };
                    base.CancelButton = buttonArray[1];
                    break;

                case MessageBoxButtons.YesNoCancel:
                    buttonArray = new SkinButton[] {
                        InnerCreateButton(DialogButton.BtnYes, DialogResult.Yes), 
                        InnerCreateButton(DialogButton.BtnNo, DialogResult.No), 
                        InnerCreateButton(DialogButton.BtnCancel, DialogResult.Cancel) };
                    base.CancelButton = buttonArray[2];
                    break;

                case MessageBoxButtons.YesNo:
                    buttonArray = new SkinButton[] {
                        InnerCreateButton(DialogButton.BtnYes, DialogResult.Yes),
                        InnerCreateButton(DialogButton.BtnNo, DialogResult.No) };
                    base.CancelButton = buttonArray[1];
                    break;

                default:
                    buttonArray = new SkinButton[] { 
                        InnerCreateButton(DialogButton.BtnOk, DialogResult.OK) };
                    base.CancelButton = buttonArray[0];
                    break;
            }

            switch (defaultBtn) {
                case MessageBoxDefaultButton.Button1:
                    base.AcceptButton = buttonArray[0];
                    break;

                case MessageBoxDefaultButton.Button2:
                    if (buttonArray.Length > 1) {
                        base.AcceptButton = buttonArray[1];
                    }
                    break;

                case MessageBoxDefaultButton.Button3:
                    if (buttonArray.Length > 2) {
                        base.AcceptButton = buttonArray[2];
                    }
                    break;
            }
            _innerButtons = buttonArray;
        }

        protected override void OnBackColorChanged(EventArgs e) {
            base.OnBackColorChanged(e);
            if (buttonArray != null) {
                foreach (SkinButton btn in buttonArray) {
                    btn.BaseColor = Color.FromArgb(200, this.BorderColor);
                }
            }
        }

        private SkinButton InnerCreateButton(string text, DialogResult result) {
            SkinButton button = new SkinButton();
            button.Size = new Size(60, 26);
            button.UseVisualStyleBackColor = false;
            button.RoundStyle = RoundStyle.None;
            button.InheritColor = false;
            button.BaseColor = Color.FromArgb(100, 192, 192, 255);
            button.ForeColorSuit = true;
            button.Text = text;
            button.DialogResult = result;
            button.Click += new EventHandler(BtnClick);
            base.Controls.Add(button);
            return button;
        }

        private void BtnClick(object sender, EventArgs e) {
            base.DialogResult = ((Button)sender).DialogResult;
            base.Close();
        }

        private void CalcIconBounds() {
            int x = BorderPadding.Left + Spacing;
            int y = BorderPadding.Top + CaptionHeight + Spacing;
            if (Message.Icon != null) {
                _iconRect = new Rectangle(x, y, Message.Icon.Width, Message.Icon.Height);
            } else {
                _iconRect = new Rectangle(x, y, 0, 0);
            }
        }

        private void CalcMessageBounds() {
            int messageTop, messageLeft, messageWidth, messageHeight;
            messageTop = _iconRect.Y;
            messageLeft = (Message.Icon == null) ?
                BorderPadding.Left + Spacing : (_iconRect.Right + Spacing);
            Rectangle workRect = SystemInformation.WorkingArea;
            Size maxSize = MaximumSizeFromMaximinClientSize();
            Size maxTextSize = maxSize;

            if (maxTextSize.Width <= 0) {
                maxTextSize.Width = workRect.Width / 3 * 2;
            }

            if (maxTextSize.Height <= 0) {
                maxTextSize.Height = workRect.Height;
            }

            maxTextSize.Width -= (BorderPadding.Horizontal + Spacing + messageLeft);
            maxTextSize.Height = int.MaxValue;

            if (maxTextSize.Width < 10) {
                maxTextSize.Width = 10;
            }

            TextFormatFlags flags =
                TextFormatFlags.HidePrefix | TextFormatFlags.ExternalLeading |
                TextFormatFlags.WordBreak | TextFormatFlags.EndEllipsis;
            SizeF textSize = TextRenderer.MeasureText(Message.Text, new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134))), maxTextSize, flags);
            messageWidth = (int)Math.Ceiling(textSize.Width);
            int maxTextHeight = maxSize.Height;

            if (maxTextHeight <= 0) {
                maxTextHeight = workRect.Height;
            }

            maxTextHeight -= (BorderPadding.Horizontal + CaptionHeight +
                messageTop + Spacing + _innerButtons[0].Height);

            if (maxTextHeight < 10) {
                maxTextHeight = 10;
            }

            messageHeight = (int)Math.Ceiling(textSize.Height);

            if (messageHeight > maxTextHeight) {
                messageHeight = maxTextHeight;
            }

            _messageRect = new Rectangle(messageLeft, messageTop, messageWidth, messageHeight);
        }

        private void CalcFinalSizes() {
            int buttonsTotalWidth = 0;
            foreach (SkinButton button in _innerButtons) {
                if (buttonsTotalWidth != 0) {
                    buttonsTotalWidth += Spacing;
                }
                buttonsTotalWidth += button.Width;
            }

            int buttonsTop = _messageRect.Bottom + Spacing;

            if (Message.Icon != null && _iconRect.Bottom + Spacing > buttonsTop) {
                buttonsTop = _iconRect.Bottom + Spacing;
            }
            Size minSize = MinimumSizeFromMiniminClientSize();
            int wantedFormWidth = minSize.Width;
            if (wantedFormWidth == 0) {
                wantedFormWidth = SystemInformation.MinimumWindowSize.Width;
            }

            if (wantedFormWidth < _messageRect.Right + Spacing) {
                wantedFormWidth = _messageRect.Right + Spacing;
            }

            if (wantedFormWidth < buttonsTotalWidth + Spacing + Spacing) {
                wantedFormWidth = buttonsTotalWidth + Spacing + Spacing;
            }

            int maxCaptionForcedWidth = SystemInformation.WorkingArea.Width / 3 * 2;
            int captionTextWidth = TextRenderer.MeasureText(Text, CaptionFont,
                new Size(maxCaptionForcedWidth, CaptionHeight),
                TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix |
                TextFormatFlags.SingleLine).Width;

            int captionTextWidthWithButtonsAndSpacing = captionTextWidth + AllButtonWidth(false);
            int captionWidth = Math.Min(maxCaptionForcedWidth, captionTextWidthWithButtonsAndSpacing);

            if (wantedFormWidth < captionWidth) {
                wantedFormWidth = captionWidth;
            }

            Width = wantedFormWidth + BorderPadding.Right;
            Height = buttonsTop + _innerButtons[0].Height + BorderPadding.Bottom + 3;

            if (Height < minSize.Height) {
                Height = minSize.Height;
                buttonsTop = Height - _innerButtons[0].Height - BorderPadding.Bottom - 3;
            }

            int nextButtonPos = BorderPadding.Left + (DisplayRectangle.Width - buttonsTotalWidth) / 2;

            for (int i = 0; i < _innerButtons.Length; ++i) {
                _innerButtons[i].Location = new Point(nextButtonPos, buttonsTop + 2);
                nextButtonPos += _innerButtons[i].Width + Spacing;
            }

            if (Message.Icon == null) {
                _messageRect.Offset((wantedFormWidth - (_messageRect.Right + Spacing)) / 2, 0);
            }

            if (Message.Icon != null && _messageRect.Height < _iconRect.Height) {
                _messageRect.Offset(0, (_iconRect.Height - _messageRect.Height) / 2);
            }
        }
        #endregion

        #region InitializeComponent()自动生成的代码
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageBoxForm));
            this.SuspendLayout();
            // 
            // MessageBoxForm
            // 
            this.Back = ((System.Drawing.Image)(resources.GetObject("$this.Back")));
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(97)))), ((int)(((byte)(159)))), ((int)(((byte)(215)))));
            this.BackLayout = false;
            this.BorderPalace = ((System.Drawing.Image)(resources.GetObject("$this.BorderPalace")));
            this.CanResize = false;
            this.ClientSize = new System.Drawing.Size(250, 158);
            this.CloseBoxSize = new System.Drawing.Size(39, 20);
            this.CloseDownBack = ((System.Drawing.Image)(resources.GetObject("$this.CloseDownBack")));
            this.CloseMouseBack = ((System.Drawing.Image)(resources.GetObject("$this.CloseMouseBack")));
            this.CloseNormlBack = ((System.Drawing.Image)(resources.GetObject("$this.CloseNormlBack")));
            this.ControlBoxOffset = new System.Drawing.Point(0, -1);
            this.DropBack = false;
            this.ForeColor = System.Drawing.Color.Black;
            this.InheritBack = true;
            this.InheritTheme = true;
            this.KeyPreview = true;
            this.MaxDownBack = ((System.Drawing.Image)(resources.GetObject("$this.MaxDownBack")));
            this.MaxMouseBack = ((System.Drawing.Image)(resources.GetObject("$this.MaxMouseBack")));
            this.MaxNormlBack = ((System.Drawing.Image)(resources.GetObject("$this.MaxNormlBack")));
            this.MaxSize = new System.Drawing.Size(28, 20);
            this.MiniDownBack = ((System.Drawing.Image)(resources.GetObject("$this.MiniDownBack")));
            this.MiniMouseBack = ((System.Drawing.Image)(resources.GetObject("$this.MiniMouseBack")));
            this.MiniNormlBack = ((System.Drawing.Image)(resources.GetObject("$this.MiniNormlBack")));
            this.MiniSize = new System.Drawing.Size(28, 20);
            this.Name = "MessageBoxForm";
            this.RestoreDownBack = ((System.Drawing.Image)(resources.GetObject("$this.RestoreDownBack")));
            this.RestoreMouseBack = ((System.Drawing.Image)(resources.GetObject("$this.RestoreMouseBack")));
            this.RestoreNormlBack = ((System.Drawing.Image)(resources.GetObject("$this.RestoreNormlBack")));
            this.ShowBorder = false;
            this.ShowDrawIcon = false;
            this.TitleSuitColor = true;
            this.TopMost = true;
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MessageBoxForm_Paint);
            this.ResumeLayout(false);

        }
        #endregion
    }

    #region 消息框对话消息参数
    /// <summary>
    /// 消息框对话消息参数。
    /// </summary>
    public class MessageBoxArgs
    {
        IWin32Window _owner;
        string _text;
        string _caption;
        MessageBoxButtons _buttons;
        Icon _icon;
        MessageBoxDefaultButton _defaultButton;

        /// <summary>
        /// 初始化 <see cref="MessageBoxArgs"/> 类的新实例。
        /// </summary>
        public MessageBoxArgs() {
        }

        /// <summary>
        ///  初始化 <see cref="MessageBoxArgs"/> 类的新实例。
        /// </summary>
        /// <param name="owner">任何实现 <see cref="IWin32Window"/>（表示将拥有模式对话框的顶级窗口）的对象。</param>
        /// <param name="text">需要显示消息文本。</param>
        /// <param name="caption">需要现实的消息标题。</param>
        /// <param name="buttons"><see cref="MessageBoxButtons"/> 值之一。</param>
        /// <param name="icon">需要显示的消息图标。</param>
        /// <param name="defaultButton"><see cref="MessageBoxDefaultButton"/> 值之一。</param>
        public MessageBoxArgs(IWin32Window owner, string text, string caption,
            MessageBoxButtons buttons, Icon icon, MessageBoxDefaultButton defaultButton) {
            _owner = owner;
            _text = text;
            _caption = caption;
            _buttons = buttons;
            _icon = icon;
            _defaultButton = defaultButton;
        }

        public IWin32Window Owner {
            get { return _owner; }
            set { _owner = value; }
        }

        public string Text {
            get { return _text; }
            set { _text = value; }
        }

        public string Caption {
            get { return _caption; }
            set { _caption = value; }
        }

        public MessageBoxButtons Buttons {
            get { return _buttons; }
            set { _buttons = value; }
        }

        public Icon Icon {
            get { return _icon; }
            set { _icon = value; }
        }

        public MessageBoxDefaultButton DefaultButton {
            get { return _defaultButton; }
            set { _defaultButton = value; }
        }
    }
    #endregion
}
