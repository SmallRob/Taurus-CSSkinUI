
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Windows.Forms;
using Com_CSSkin.SkinClass;

namespace Com_CSSkin.SkinControl
{
    [ToolboxItem(true)]
    public class SkinRollingBar : SkinControlBase
    {
        #region 构造函数及初始化

        public SkinRollingBar() {
            rollingTimer = new Timer();
            rollingTimer.Tick += new EventHandler(rollingTimer_Tick);
            rollingTimer.Interval = _refleshFrequency;
            rollingTimer.Enabled = false;
        }

        #endregion

        #region private var

        /// <summary>
        /// 以角度为单位的当前的角度位置
        /// </summary>
        float currentAngle = 0f;

        Timer rollingTimer;

        #endregion

        #region 新增的公开属性
        SkinRollingBarThemeBase _xtheme;
        RollingBarStyle _style;
        int _refleshFrequency = 120;

        /// <summary>
        /// 刷新频率
        /// </summary>
        [DefaultValue(120)]
        [Category("Skin")]
        [Description("刷新频率")]
        public int RefleshFrequency {
            get {
                return _refleshFrequency;
            }
            set {
                if (_refleshFrequency != value) {
                    if (value < 1)
                        value = 120;
                    _refleshFrequency = value;
                    rollingTimer.Interval = _refleshFrequency;
                }
            }
        }

        /// <summary>
        /// 当前样式
        /// </summary>
        [DefaultValue(typeof(RollingBarStyle), "0")]
        [Category("Skin")]
        [Description("当前样式（Default才可更改所有属性，其他样式会指定部分属性不可变更）")]
        public RollingBarStyle Style {
            get {
                return _style;
            }
            set {
                if (_style != value) {
                    _style = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 当前主题
        /// </summary>
        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        [Category("Skin")]
        [Description("当前主题")]
        public SkinRollingBarThemeBase XTheme {
            get {
                if (_xtheme == null) {
                    _xtheme = new SkinRollingBarThemeBase();
                }
                return _xtheme;
            }
            set {
                if (_xtheme != value) {
                    _xtheme = value;
                    Invalidate();
                }
            }
        }

        #endregion

        #region 公开的方法

        public void StartRolling() {
            if (rollingTimer.Enabled)
                return;
            rollingTimer.Enabled = true;
        }

        public void StopRolling() {
            rollingTimer.Enabled = false;
        }

        #endregion

        #region 可用XTheme配置的属性
        /// <summary>
        /// 外圈大小
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(int), "10")]
        [Description("外圈大小")]
        public virtual int Radius1 {
            get {
                return XTheme.Radius1;
            }
            set {
                if (XTheme.Radius1 != value) {
                    XTheme.Radius1 = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 内圈大小
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(int), "20")]
        [Description("内圈大小")]
        public virtual int Radius2 {
            get {
                return XTheme.Radius2;
            }
            set {
                if (XTheme.Radius2 != value) {
                    XTheme.Radius2 = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 获取或设置用于在动画圈的残影数量
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(int), "12")]
        [Description("获取或设置用于在动画圈的残影数量")]
        public virtual int SpokeNum {
            get {
                return XTheme.SpokeNum;
            }
            set {
                if (XTheme.SpokeNum != value) {
                    XTheme.SpokeNum = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 画笔宽度
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(float), "2")]
        [Description("画笔宽度")]
        public virtual float PenWidth {
            get {
                return XTheme.PenWidth;
            }
            set {
                if (XTheme.PenWidth != value) {
                    XTheme.PenWidth = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 背景色
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(Color), "Transparent")]
        [Description("背景色")]
        public virtual Color SkinBackColor {
            get {
                return XTheme.BackColor;
            }
            set {
                if (XTheme.BackColor != value) {
                    XTheme.BackColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 主色调
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(Color), "Red")]
        [Description("主色调")]
        public virtual Color BaseColor {
            get {
                return XTheme.BaseColor;
            }
            set {
                if (XTheme.BaseColor != value) {
                    XTheme.BaseColor = value;
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 菱形块颜色
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(Color), "White")]
        [Description("菱形块颜色")]
        public virtual Color DiamondColor {
            get {
                return XTheme.DiamondColor;
            }
            set {
                if (XTheme.DiamondColor != value) {
                    XTheme.DiamondColor = value;
                    Invalidate();
                }
            }
        }
        #endregion

        #region 内部方法

        private void rollingTimer_Tick(object sender, EventArgs e) {
            base.Invalidate();
        }

        #endregion

        #region 内部绘图

        protected virtual void PaintThisRollingBar(Graphics g) {
            switch (Style) {
                case RollingBarStyle.Default:
                    PaintDefault(g);
                    break;

                case RollingBarStyle.ChromeOneQuarter:
                    PaintChromeOneQuarter(g);
                    break;

                case RollingBarStyle.DiamondRing:
                    PaintDiamondRing(g);
                    break;

                case RollingBarStyle.BigGuyLeadsLittleGuys:
                    PaintTheseGuys(g);
                    break;
            }
        }

        private void IncreaseCurrentAngle(int spokeNum) {
            if (rollingTimer.Enabled) {
                currentAngle += 360f / spokeNum;
                if (currentAngle > 360f)
                    currentAngle -= 360f;
            }
        }

        private void PaintDefault(Graphics g) {
            IncreaseCurrentAngle(SpokeNum);

            RollingBarPainter.RenderDefault(
                g,
                ClientRectangle,
                SkinBackColor,
                currentAngle,
                Radius1,
                Radius2,
                SpokeNum,
                PenWidth,
                ColorHelper.GetLighterArrayColors(BaseColor, SpokeNum));
        }

        private void PaintChromeOneQuarter(Graphics g) {
            IncreaseCurrentAngle(10);

            RollingBarPainter.RenderChromeOneQuarter(
                g,
                ClientRectangle,
                SkinBackColor,
                currentAngle,
                Radius1,
                BaseColor);
        }

        private void PaintDiamondRing(Graphics g) {
            IncreaseCurrentAngle(12);

            RollingBarPainter.RenderDiamondRing(
                g,
                ClientRectangle,
                SkinBackColor,
                currentAngle,
                Radius1,
                BaseColor,
                _xtheme == null ? Color.White : _xtheme.DiamondColor);
        }

        private void PaintTheseGuys(Graphics g) {
            IncreaseCurrentAngle(10);

            RollingBarPainter.RenderTheseGuys(
                g,
                ClientRectangle,
                SkinBackColor,
                currentAngle,
                Radius1,
                BaseColor);
        }

        #endregion

        #region 重写基类方法

        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);
            PaintThisRollingBar(e.Graphics);
        }

        #endregion
    }
}
