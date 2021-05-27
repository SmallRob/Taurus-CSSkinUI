
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(System.Windows.Forms.DateTimePicker))]
    [DefaultEvent("SelectedValueChange")]
    public partial class SkinDateTimePicker : UserControl
    {
        #region 声明
        private Bitmap _TextBoxBackImg = Properties.Resources.AlDateTimePicker;
        private State state = State.Normal;
        private SkinDropDown _SkinDropDown;
        private SkinMonthCalendar _SkinMonthCalendar;
        private ComboBoxStyle _DropDownStyle = ComboBoxStyle.DropDown;
        private string[] _items;
        private bool _SkinDropDownShow = false;
        private Font _font = new Font("微软雅黑", 9F);
        private bool _AlReadOnly = false;
        private int _DropDownHeight = 180;
        private int _DropDownWidth = 120;
        //枚鼠标状态
        private enum State
        {
            Normal = 1,
            MouseOver = 2,
            MouseDown = 3
        }
        #endregion

        #region 委托
        public delegate void SelectedValueChangeEventHandler(object sender, string Item);
        public event SelectedValueChangeEventHandler SelectedValueChange;
        #endregion

        #region 构造函数
        public SkinDateTimePicker() {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.SetStyle(ControlStyles.StandardDoubleClick, false);
            this.SetStyle(ControlStyles.Selectable, true);
            this.BackColor = Color.Transparent;
            BaseText.Font = this.Font;
        }
        #endregion

        #region 属性
        [Category("Skin"), Description("文本集合")]
        public string[] Items {
            get {
                return _items;
            }
            set {
                _items = value;
            }
        }

        [Category("Skin"), Description("文本内容")]
        public string text {
            get { return BaseText.Text; }
            set {
                BaseText.Text = value;
            }
        }

        [Category("Skin"), Description("文本内容")]
        public override string Text {
            get { return BaseText.Text; }
            set {
                BaseText.Text = value;
            }
        }

        [Category("Skin"), Description("控制组合框外观和功能")]
        public ComboBoxStyle DropDownStyle {
            get { return _DropDownStyle; }
            set {
                _DropDownStyle = value;
                if (_DropDownStyle == ComboBoxStyle.DropDown) {
                    _AlReadOnly = true;
                    BaseText.Visible = true;

                }
                if (_DropDownStyle == ComboBoxStyle.DropDownList) {
                    _AlReadOnly = false;
                    BaseText.Cursor = Cursors.Arrow;
                    BaseText.Visible = true;
                    BaseText.ReadOnly = true;
                    BaseText.MaxLength = 12; //BaseText.Text.Length;
                }
            }

        }

        [Category("Skin"), Description("设置控件中文本字体")]
        public Font font {
            get {
                return _font;
            }
            set {
                _font = value;
                BaseText.Font = value;
                this.Invalidate();
            }
        }

        [Category("Skin"), Description("组合框宽")]

        public int DropDownWidth {
            get { return _DropDownWidth; }
            set { _DropDownWidth = value; }

        }
        [Category("Skin"), Description("组合框高")]

        public int DropDownHeight {
            get { return _DropDownHeight; }
            set { _DropDownHeight = value; }

        }
        #endregion

        #region 方法
        protected override void OnPaint(PaintEventArgs e) {
            Rectangle rc = this.ClientRectangle;
            Graphics g = e.Graphics;
            ImageDrawRect.DrawRect(g, _TextBoxBackImg, rc, Rectangle.FromLTRB(10, 10, 30, 10), (int)state, 3);
            base.OnPaint(e);
        }

        #region AlComboBox
        private void AlDateTimePicker_MouseEnter(object sender, EventArgs e) {
            state = State.MouseOver;
            this.Invalidate();
        }

        private void AlDateTimePicker_MouseDown(object sender, MouseEventArgs e) {
            if (_SkinDropDownShow == false) {
                _SkinDropDownShow = true;
                _SkinMonthCalendar = new SkinMonthCalendar(this);
                _SkinDropDown = new SkinDropDown(_SkinMonthCalendar);
                _SkinDropDown.Show(this, new Point(0, 22));
                _SkinDropDown.Closed += new ToolStripDropDownClosedEventHandler(_SkinDropDown_Closed);
                state = State.MouseDown;
            } else {
                _SkinDropDownShow = false;
                SkinDropDownClose();
                state = State.MouseOver;
            }
            this.Invalidate();

        }

        private void AlDateTimePicker_MouseLeave(object sender, EventArgs e) {
            if (_SkinDropDownShow == false) {
                state = State.Normal;
                this.Invalidate();
            }
        }

        private void AlDateTimePicker_Leave(object sender, EventArgs e) {
            if (_SkinDropDownShow == false) {
                state = State.Normal;
                this.Invalidate();
            }
        }

        #endregion

        #region BaseText
        private void BaseText_MouseEnter(object sender, EventArgs e) {

        }

        private void BaseText_MouseLeave(object sender, EventArgs e) {

        }

        #endregion

        #region BaseLabel


        private void BaseLabel_MouseLeave(object sender, EventArgs e) {

        }
        #endregion

        #region Other
        private void _SkinDropDown_Closed(Object sender, ToolStripDropDownClosedEventArgs e) {
            _SkinDropDownShow = false;
            state = State.Normal;
            this.Invalidate();
            if (SelectedValueChange != null)
                SelectedValueChange(this, this.BaseText.Text);
        }

        public void SkinDropDownClose() {
            if (_SkinDropDown != null) {
                if (!_SkinDropDown.IsDisposed) {
                    _SkinDropDown.Close();
                }
            }
            state = State.Normal;
            this.Invalidate();
        }
        #endregion

        #endregion
    }
}
