
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    public partial class SkinCode : Control
    {
        public SkinCode() {
            InitializeComponent();
            Init();
        }
        #region 初始化
        public void Init() {
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.UpdateStyles();
        }
        #endregion

        #region 属性
        private Image codeImg;
        /// <summary>
        /// 验证码的图像
        /// </summary>
        [DefaultValue(false)]
        [Category("Skin")]
        [Description("验证码的图像")]
        public Image CodeImg {
            get { return codeImg; }
            set { codeImg = value; }
        }

        private string codeStr;
        /// <summary>
        /// 验证码的值
        /// </summary>
        [Category("Skin")]
        [Description("验证码的值")]
        public string CodeStr {
            get { return codeStr; }
        }

        private bool clickNewCode = true;
        /// <summary>
        /// 是否可以点击刷新验证码
        /// </summary>
        [DefaultValue(true)]
        [Category("Skin")]
        [Description("是否可以点击刷新验证码")]
        public bool ClickNewCode {
            get { return clickNewCode; }
            set { clickNewCode = value; }
        }

        private Color[] color_BackGround = { Color.FromArgb(247, 254, 236), Color.FromArgb(234, 248, 255), Color.FromArgb(244, 250, 246), Color.FromArgb(248, 248, 248) };
        /// <summary>
        /// 背景颜色集
        /// </summary>
        [Category("Skin")]
        [Description("背景颜色集")]
        public Color[] Color_BackGround {
            get { return color_BackGround; }
            set {
                if (color_BackGround != value) {
                    color_BackGround = value;
                    //重绘验证码图片颜色信息
                    UpdateImg();
                }
            }
        }

        private string[] vcArray = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F", "G", "H", "J", "K", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
        /// <summary>
        /// 验证码字符集
        /// </summary>
        [Category("Skin")]
        [Description("验证码字符集")]
        public string[] VcArray {
            get { return vcArray; }
            set {
                if (vcArray != value) {
                    vcArray = value;
                    //刷新验证码
                    NewCode();
                }
            }
        }

        private int codeCount = 4;
        /// <summary>
        /// 验证码字数
        /// </summary>
        [DefaultValue(4)]
        [Category("Skin")]
        [Description("验证码字数")]
        public int CodeCount {
            get { return codeCount; }
            set {
                if (codeCount != value) {
                    codeCount = value;
                    //刷新验证码
                    NewCode();
                }
            }
        }
        #endregion

        #region 重载事件与方法
        /// <summary>
        /// 创建新的Code验证码，并返回其验证码
        /// </summary>
        /// <returns></returns>
        public string NewCode() {
            codeStr = RndNum(CodeCount);
            CodeImg = CreateImage(CodeStr);
            this.Invalidate();
            return CodeStr;
        }

        /// <summary>
        /// 控件创建时
        /// </summary>
        protected override void OnCreateControl() {
            base.OnCreateControl();
            //生成一个新的验证码
            NewCode();
        }

        /// <summary>
        /// 点击换验证码
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClick(EventArgs e) {
            base.OnClick(e);
            if (ClickNewCode) {
                //生成一个新的验证码
                NewCode();
            }
        }

        /// <summary>
        /// 字体颜色改变时
        /// </summary>
        /// <param name="e"></param>
        protected override void OnForeColorChanged(EventArgs e) {
            base.OnForeColorChanged(e);
            //重绘验证码图片颜色信息
            UpdateImg();
        }

        /// <summary>
        /// 背景色改变时
        /// </summary>
        /// <param name="e"></param>
        protected override void OnBackColorChanged(EventArgs e) {
            base.OnBackColorChanged(e);
            if (Color_BackGround.Length == 0) {
                //重绘验证码图片颜色信息
                UpdateImg();
            }
        }

        /// <summary>
        /// 重绘验证码图片颜色信息
        /// </summary>
        private void UpdateImg() {
            CodeImg = CreateImage(CodeStr);
            this.Invalidate();
        }

        /// <summary>
        /// 重绘时
        /// </summary>
        /// <param name="pe"></param>
        protected override void OnPaint(PaintEventArgs pe) {
            base.OnPaint(pe);
            if (CodeImg != null) {
                Graphics g = pe.Graphics;
                g.DrawImage(CodeImg, this.ClientRectangle);
            }
        }
        #endregion

        #region 验证码绘制方法
        protected string VCODE_SESSION = "vcode";
        protected Types VCODE_TYPE = Types.Number;
        protected int VCODE_LENGTH = 4;
        protected bool VCODE_IsEncrypt = true;
        protected bool VCODE_IsIgnore = false;
        //产生图片 宽度：_WIDTH, 高度：_HEIGHT
        private readonly int _WIDTH = 130, _HEIGHT = 53;
        //字体集
        private readonly string[] _FONT_FAMIly = { "Arial", "Arial Black", "Arial Italic", "Courier New", "Courier New Bold Italic", "Courier New Italic", "Franklin Gothic Medium", "Franklin Gothic Medium Italic" };
        //字体大小集
        private readonly int[] _FONT_SIZE = { 20, 25, 30 };
        //文本布局信息
        private StringFormat _DL_FORMAT = new StringFormat(StringFormatFlags.NoClip);
        //左右旋转角度
        private readonly int _ANGLE = 60;

        private Image CreateImage(string code) {
            Image codeImg = null;
            if (!string.IsNullOrEmpty(code)) {
                _DL_FORMAT.Alignment = StringAlignment.Center;
                _DL_FORMAT.LineAlignment = StringAlignment.Center;

                long tick = DateTime.Now.Ticks;
                Random Rnd = new Random((int)(tick & 0xffffffffL) | (int)(tick >> 32));

                using (Bitmap _img = new Bitmap(_WIDTH, _HEIGHT)) {
                    using (Graphics g = Graphics.FromImage(_img)) {
                        g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

                        Point dot = new Point(20, 20);

                        // 定义一个无干扰线区间和一个起始位置
                        int nor = Rnd.Next(_HEIGHT), rsta = Rnd.Next(_WIDTH);
                        // 绘制干扰正弦曲线 M:曲线平折度, D:Y轴常量 V:X轴焦距
                        int M = Rnd.Next(15) + 5, D = Rnd.Next(20) + 15, V = Rnd.Next(5) + 1;

                        //int ColorIndex = Rnd.Next(4);

                        float Px_x = 0.0F;
                        float Px_y = Convert.ToSingle(M * Math.Sin(V * Px_x * Math.PI / 180) + D);
                        float Py_x, Py_y;

                        //填充背景
                        g.Clear(Color_BackGround.Length == 0 ? BackColor : Color_BackGround[Rnd.Next(Color_BackGround.Length)]);

                        //前景刷子 //背景刷子
                        using (Brush _BrushFace = new SolidBrush(ForeColor)) {
                            #region 绘制正弦线
                            for (int i = 0; i < 131; i++) {

                                //初始化y点
                                Py_x = Px_x + 1;
                                Py_y = Convert.ToSingle(M * Math.Sin(V * Py_x * Math.PI / 180) + D);

                                //确定线条颜色
                                if (rsta >= i || i > (rsta + nor))
                                    //初始化画笔
                                    using (Pen _pen = new Pen(_BrushFace, Rnd.Next(1, 3) + 1.1F)) {
                                        //绘制线条
                                        g.DrawLine(_pen, Px_x, Px_y, Py_x, Py_y);
                                    }

                                //交替x,y坐标点
                                Px_x = Py_x;
                                Px_y = Py_y;
                            }
                            #endregion

                            //初始化光标的开始位置
                            g.TranslateTransform(18, 4);
                            #region 绘制校验码字符串
                            for (int i = 0; i < code.Length; i++) {
                                //随机旋转 角度
                                int angle = Rnd.Next(-_ANGLE, _ANGLE);
                                //移动光标到指定位置
                                g.TranslateTransform(dot.X, dot.Y);
                                //旋转
                                g.RotateTransform(angle);

                                //初始化字体
                                using (Font _font = new Font(_FONT_FAMIly[Rnd.Next(0, 8)], _FONT_SIZE[Rnd.Next(0, 3)])) {
                                    //绘制
                                    g.DrawString(code[i].ToString(), _font, _BrushFace, 1, 1, _DL_FORMAT);
                                }
                                //反转
                                g.RotateTransform(-angle);
                                //重新定位光标位置
                                g.TranslateTransform(-2, -dot.Y);
                            }
                            #endregion
                        }
                    }
                    codeImg = (Image)_img.Clone();
                }
            } else {
                using (Bitmap _img = new Bitmap(_WIDTH, _HEIGHT)) {
                    using (Graphics g = Graphics.FromImage(_img)) {
                        g.DrawString("验证码", this.Font, new SolidBrush(this.ForeColor), new Point(0, 0));
                    }
                    codeImg = (Image)_img.Clone();
                }
            }
            return codeImg;
        }

        protected enum Types
        {
            Number,
            Character
        }

        /// <summary> 
        /// 生成随机的字母   
        /// </summary>    
        /// <param name="VcodeNum">生成字母的个数</param>                                                                
        /// <returns>string</returns>
        Random rand = new Random();
        private string RndNum(int VcodeNum) {
            string VNum = ""; //由于字符串很短，就不用StringBuilder了
            //存储上一个参数的index
            int temp = -1;
            for (int i = 1; i < VcodeNum + 1; i++) {
                int t = rand.Next(VcArray.Length);
                //简单循环取相邻两值不同
                while (temp == t) {
                    t = rand.Next(VcArray.Length);
                }
                temp = t;
                VNum += VcArray[t];
            }
            return VNum;
        }
        #endregion
    }
}
