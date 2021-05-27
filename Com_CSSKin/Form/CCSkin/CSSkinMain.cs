
using Com_CSSkin.SkinClass;
using Com_CSSkin.SkinControl;
using Com_CSSkin.Win32;
using Com_CSSkin.Win32.Const;
using Com_CSSkin.Win32.Struct;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Com_CSSkin
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class CSSkinMain : Form
    {
        #region 变量
        //绘制层
        public CSSkinForm skin = null;
        private SkinFormRenderer _renderer;
        private RoundStyle _roundStyle = RoundStyle.All;
        private Rectangle _deltaRect;
        private int _radius = 6;
        private int _captionHeight = 24;
        private Font _captionFont = SystemFonts.CaptionFont;
        private Size _miniSize = new Size(32, 18);
        private Size _maxBoxSize = new Size(32, 18);
        private Size _closeBoxSize = new Size(32, 18);
        private Point _controlBoxOffset = new Point(6, 0);
        private int _controlBoxSpace = 0;
        private bool _active = false;
        private bool _showSystemMenu = false;
        private ControlBoxManager _controlBoxManager;
        private Padding _padding;
        private bool _canResize = true;
        private ToolTip _toolTip;
        private MobileStyle _mobile = MobileStyle.Mobile;
        private static readonly object EventRendererChanged = new object();
        private bool _clientSizeSet;
        private int _inWmWindowPosChanged;
        #endregion

        #region 无参构造函数
        public CSSkinMain()
            : base()
        {
            InitializeComponent();
            SetStyles();
            Init();
        }
        #endregion

        #region 属性
        private Color captionBackColorTop = Color.Transparent;
        /// <summary>
        /// 标题栏颜色是从上到下渐变的，这个值设置上边的颜色值
        /// </summary>
        [Category("Caption")]
        [DefaultValue(typeof(Color), "Transparent")]
        [Description("标题栏颜色是从上到下渐变的，这个值设置上边的颜色值")]
        public Color CaptionBackColorTop
        {
            get => captionBackColorTop;
            set
            {
                captionBackColorTop = value;
                this.Invalidate(CaptionRectToDraw);
            }
        }

        private Color captionBackColorBottom = Color.Transparent;
        /// <summary>
        /// 标题栏颜色是从上到下渐变的，这个值设置下边的颜色值
        /// </summary>
        [Category("Caption")]
        [DefaultValue(typeof(Color), "Transparent")]
        [Description("标题栏颜色是从上到下渐变的，这个值设置下边的颜色值")]
        public Color CaptionBackColorBottom
        {
            get => captionBackColorBottom;
            set
            {
                captionBackColorBottom = value;
                this.Invalidate(CaptionRectToDraw);
            }
        }

        public Rectangle CaptionRectToDraw => new Rectangle(
                    0,
                    0,
                    this.ClientSize.Width,
                    CaptionHeight + BorderPadding.Left);

        /// <summary>
        /// Mdi背景拉伸绘制
        /// </summary>
        [Category("Mdi")]
        [Description("Mdi背景拉伸绘制")]
        public bool MdiStretchImage
        {
            get => MdiClientController == null ? true : MdiClientController.StretchImage;
            set => MdiClientController.StretchImage = value;
        }
        /// <summary>
        /// Mdi背景绘制位置
        /// </summary>
        [Category("Mdi")]
        [Description("Mdi背景绘制位置")]
        public ContentAlignment MdiImageAlign
        {
            get => MdiClientController.ImageAlign;
            set => MdiClientController.ImageAlign = value;
        }
        /// <summary>
        /// Mdi背景图像
        /// </summary>
        [Category("Mdi")]
        [Description("Mdi背景图像")]
        public Image MdiImage
        {
            get => MdiClientController.Image;
            set => MdiClientController.Image = value;
        }
        /// <summary>
        /// 是否显示Mdi滚动条
        /// </summary>
        [Category("Mdi")]
        [Description("是否显示Mdi滚动条")]
        public bool MdiAutoScroll
        {
            get => MdiClientController.AutoScroll;
            set => MdiClientController.AutoScroll = value;
        }
        /// <summary>
        /// Mdi边框样式
        /// </summary>
        [Category("Mdi")]
        [Description("Mdi边框样式")]
        public BorderStyle MdiBorderStyle
        {
            get => MdiClientController.BorderStyle;
            set => MdiClientController.BorderStyle = value;
        }

        /// <summary>
        /// MDI容器背景色
        /// </summary>
        [Category("Mdi")]
        [Description("MDI容器背景色")]
        public Color MdiBackColor
        {
            get => MdiClientController.BackColor;
            set => MdiClientController.BackColor = value;
        }

        private MdiClientController mdiClientController;
        /// <summary>
        /// MDI容器对象
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MdiClientController MdiClientController
        {
            get => mdiClientController;
            set => mdiClientController = value;
        }

        private CSSkinMain xTheme = null;
        /// <summary>
        /// 窗体主题
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public CSSkinMain XTheme
        {
            get => xTheme == null ? this : xTheme;
            set
            {
                if (value != null)
                {
                    #region 主题赋值
                    this.Padding = value.Padding;
                    this.BorderPadding = value.BorderPadding;
                    this.BackLayout = value.BackLayout;
                    this.InnerBorderColor = value.InnerBorderColor;
                    this.ShowBorder = value.ShowBorder;
                    this.BorderColor = value.BorderColor;
                    this.Shadow = value.Shadow;
                    this.IsShadowStraight = value.IsShadowStraight;
                    this.ShadowColor = value.ShadowColor;
                    this.ShadowWidth = value.ShadowWidth;
                    this.ShadowRectangle = value.ShadowRectangle;
                    this.ShadowPalace = value.ShadowPalace;
                    this.BorderRectangle = value.BorderRectangle;
                    this.ControlBoxActive = value.ControlBoxActive;
                    this.ControlBoxDeactive = value.ControlBoxDeactive;
                    this.BackColor = value.BackColor;
                    this.BackPalace = value.BackPalace;
                    this.BackRectangle = value.BackRectangle;
                    this.BackShade = value.BackShade;
                    this.BackToColor = value.BackToColor;
                    this.BorderPalace = value.BorderPalace;
                    this.CaptionHeight = value.CaptionHeight;
                    this.CloseBoxSize = value.CloseBoxSize;
                    this.CloseDownBack = value.CloseDownBack;
                    this.CloseMouseBack = value.CloseMouseBack;
                    this.CloseNormlBack = value.CloseNormlBack;
                    this.ControlBoxOffset = value.ControlBoxOffset;
                    this.ControlBoxSpace = value.ControlBoxSpace;
                    this.MaxDownBack = value.MaxDownBack;
                    this.MaxMouseBack = value.MaxMouseBack;
                    this.MaxNormlBack = value.MaxNormlBack;
                    this.MaxSize = value.MaxSize;
                    this.MiniDownBack = value.MiniDownBack;
                    this.MiniMouseBack = value.MiniMouseBack;
                    this.MiniNormlBack = value.MiniNormlBack;
                    this.MiniSize = value.MiniSize;
                    this.RestoreDownBack = value.RestoreDownBack;
                    this.RestoreMouseBack = value.RestoreMouseBack;
                    this.RestoreNormlBack = value.RestoreNormlBack;
                    this.TitleCenter = value.TitleCenter;
                    this.TitleOffset = value.TitleOffset;
                    this.EffectCaption = value.EffectCaption;
                    this.TitleSuitColor = value.TitleSuitColor;
                    this.CaptionFont = value.CaptionFont;
                    this.EffectBack = value.EffectBack;
                    this.TitleColor = value.TitleColor;
                    this.EffectWidth = value.EffectWidth;
                    this.RoundStyle = value.RoundStyle;
                    this.Radius = value.Radius;
                    this.ShowSystemMenu = value.ShowSystemMenu;
                    this.ICoOffset = value.ICoOffset;
                    this.Font = value.Font;
                    this.ForeColor = value.ForeColor;
                    this.CaptionBackColorTop = value.CaptionBackColorTop;
                    this.CaptionBackColorBottom = value.CaptionBackColorBottom;
                    ////赋值自定义系统按钮
                    //if (value.SysButtonItems.Count > 0) {
                    //    //获取主题窗体的自定义系统按钮
                    //    CmSysButton[] sysl = new CmSysButton[value.SysButtonItems.Count];
                    //    value.SysButtonItems.CopyTo(sysl, 0);
                    //    //清除当前窗体自定义系统按钮
                    //    this.SysButtonItems.Clear();
                    //    //添加主题的自定义按钮
                    //    this.SysButtonItems.AddRange(sysl);
                    //}
                    //更新阴影
                    if (this.skin != null)
                    {
                        this.skin.SetBits();
                    }
                    #endregion
                    xTheme = value;
                    //引发事件
                    OnThemeChanged(new ThemeEventArgs(this));
                }
            }
        }

        private CustomSysButtonCollection sysButtonItems;
        /// <summary>
        /// 获取自定义系统按钮的集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("SysButton")]
        [Description("自定义系统按钮集合的项")]
        public CustomSysButtonCollection SysButtonItems
        {
            get
            {
                if (sysButtonItems == null)
                {
                    sysButtonItems = new CustomSysButtonCollection(this);
                }

                return sysButtonItems;
            }
        }

        private bool inheritBack = false;
        /// <summary>
        /// 是否继承所属窗体的背景
        /// </summary>
        [Category("Skin")]
        [DefaultValue(false)]
        [Description("是否继承所属窗体的背景")]
        public bool InheritBack
        {
            get => inheritBack;
            set => inheritBack = value;
        }

        private bool inheritTheme = false;
        /// <summary>
        /// 是否继承所属窗体的背景
        /// </summary>
        [Category("Skin")]
        [DefaultValue(false)]
        [Description("是否继承所属窗体的主题")]
        public bool InheritTheme
        {
            get => inheritTheme;
            set => inheritTheme = value;
        }

        private Padding _borderPadding = new Padding(4);
        /// <summary>
        /// 获取或设置窗体的边框大小。
        /// </summary>
        protected internal Padding BorderPadding
        {
            get => _borderPadding;
            set => _borderPadding = value;
        }

        private double skinOpacity = 1;
        /// <summary>
        /// 窗体渐变后透明度
        /// </summary>
        [Category("Skin")]
        [Description("窗体渐变后透明度")]
        public double SkinOpacity
        {
            get => skinOpacity;
            set
            {
                if (skinOpacity != value)
                {
                    skinOpacity = value;
                }
            }
        }

        private Image back;
        /// <summary>
        /// 背景
        /// </summary>
        [Category("Skin")]
        [Description("背景")]
        public Image Back
        {
            get => back;
            set
            {
                if (back != value)
                {
                    back = value;
                    if (BackToColor && back != null)
                    {
                        //渐变背景
                        BackColor = BitmapHelper.GetImageAverageColor((Bitmap)back);
                    }
                    this.Invalidate();
                    //引发事件
                    OnBackChanged(new BackEventArgs(back, value));
                }
            }
        }

        private bool mobileApi = true;
        /// <summary>
        /// 窗体移动是否调用Api
        /// </summary>
        [Category("Skin")]
        [Description("窗体移动是否调用Api")]
        public bool MobileApi
        {
            get => mobileApi;
            set
            {
                if (mobileApi != value)
                {
                    mobileApi = value;
                }
            }
        }

        private bool backLayout = true;
        /// <summary>
        /// 是否从左绘制背景
        /// </summary>
        [Category("Skin")]
        [Description("是否从左绘制背景")]
        public bool BackLayout
        {
            get => backLayout;
            set
            {
                if (backLayout != value)
                {
                    backLayout = value;
                    this.Invalidate();
                }
            }
        }

        private Image backpalace;
        /// <summary>
        /// 质感层背景
        /// </summary>
        [Category("Skin")]
        [Description("质感层背景")]
        public Image BackPalace
        {
            get => backpalace;
            set
            {
                if (backpalace != value)
                {
                    backpalace = value;
                    this.Invalidate();
                }
            }
        }

        private Image borderpalace;
        /// <summary>
        /// 边框层背景
        /// </summary>
        [Category("Border")]
        [Description("边框层背景")]
        public Image BorderPalace
        {
            get => borderpalace;
            set
            {
                if (borderpalace != value)
                {
                    borderpalace = value;
                    this.Invalidate();
                }
            }
        }

        private bool showborder = true;
        /// <summary>
        /// 是否在窗体上绘画边框
        /// </summary>
        [Category("Border")]
        [DefaultValue(true)]
        [Description("是否在窗体上绘画边框")]
        public bool ShowBorder
        {
            get => showborder;
            set
            {
                if (showborder != value)
                {
                    showborder = value;
                    this.Invalidate();
                }
            }
        }

        //边框绘画模式的颜色
        private Color _BorderColor = Color.FromArgb(100, 0, 0, 0);
        [Category("Border")]
        [Description("边框绘画模式的边框颜色")]
        [DefaultValue(typeof(Color), "100, 0, 0, 0")]
        public Color BorderColor
        {
            get => _BorderColor;
            set
            {
                _BorderColor = value;
                this.Invalidate();
            }
        }

        //边框绘画模式的内边框颜色
        private Color _InnerBorderColor = Color.FromArgb(100, 250, 250, 250);
        [Category("Border")]
        [Description("边框绘画模式的内边框颜色")]
        [DefaultValue(typeof(Color), "100, 250, 250, 250")]
        public Color InnerBorderColor
        {
            get => _InnerBorderColor;
            set
            {
                _InnerBorderColor = value;
                this.Invalidate();
            }
        }

        private bool showdrawicon = true;
        /// <summary>
        /// 是否在窗体上绘画ICO图标
        /// </summary>
        [Category("窗口样式")]
        [DefaultValue(true)]
        [Description("是否在窗体上绘画ICO图标")]
        public bool ShowDrawIcon
        {
            get => showdrawicon;
            set
            {
                if (showdrawicon != value)
                {
                    showdrawicon = value;
                    this.Invalidate(IconRect);
                }
            }
        }

        private bool special = true;
        /// <summary>
        /// 是否启用窗口淡入淡出
        /// </summary>
        [Category("Skin")]
        [DefaultValue(true)]
        [Description("是否启用窗口淡入淡出")]
        public bool Special
        {
            get => special;
            set
            {
                if (special != value)
                {
                    special = value;
                }
            }
        }

        private bool shadow = true;
        /// <summary>
        /// 是否启用窗体阴影
        /// </summary>
        [Category("Shadow")]
        [DefaultValue(true)]
        [Description("是否启用窗体阴影")]
        public bool Shadow
        {
            get => shadow;
            set
            {
                if (shadow != value)
                {
                    shadow = value;
                    if (!OneVisibles && value && skin == null && !DesignMode)
                    {
                        skin = new CSSkinForm(this);
                        skin.Show(this);
                    }
                    if (skin != null)
                    {
                        //刷新阴影
                        skin.SetBits();
                    }
                }
            }
        }

        private bool isShadowStraight = false;
        /// <summary>
        /// 是否是直角样式的阴影
        /// </summary>
        [Category("Shadow")]
        [DefaultValue(false)]
        [Description("是否是直角样式的阴影")]
        public bool IsShadowStraight
        {
            get => isShadowStraight;
            set
            {
                if (isShadowStraight != value)
                {
                    isShadowStraight = value;
                    if (skin != null)
                    {
                        //刷新阴影
                        skin.SetBits();
                    }
                }
            }
        }

        private Color shadowColor = Color.Black;
        /// <summary>
        /// 窗体阴影颜色
        /// </summary>
        [Category("Shadow")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("窗体阴影颜色")]
        public Color ShadowColor
        {
            get => shadowColor;
            set
            {
                if (shadowColor != value)
                {
                    shadowColor = value;
                    //更新阴影
                    if (skin != null)
                    {
                        skin.SetBits();
                    }
                }
            }
        }

        private int shadowWidth = 4;
        /// <summary>
        /// 窗体阴影宽度
        /// </summary>
        [Category("Shadow")]
        [DefaultValue(typeof(int), "4")]
        [Description("窗体阴影宽度")]
        public int ShadowWidth
        {
            get => shadowWidth;
            set
            {
                if (shadowWidth != value)
                {
                    shadowWidth = value < 1 ? 1 : value;
                    //更新阴影
                    if (skin != null)
                    {
                        skin.SetBits();
                    }
                }
            }
        }

        private Image shadowpalace;
        /// <summary>
        /// 阴影边框图
        /// </summary>
        [Category("Shadow")]
        [Description("阴影边框图")]
        public Image ShadowPalace
        {
            get => shadowpalace;
            set
            {
                if (shadowpalace != value)
                {
                    shadowpalace = value;
                    this.Invalidate();
                }
            }
        }

        private Rectangle shadowRectangle = new Rectangle(10, 10, 10, 10);
        /// <summary>
        /// 阴影九宫绘画区域
        /// </summary>
        [Category("Shadow")]
        [DefaultValue(typeof(Rectangle), "10,10,10,10")]
        [Description("阴影九宫绘画区域")]
        public Rectangle ShadowRectangle
        {
            get => shadowRectangle;
            set
            {
                if (shadowRectangle != value)
                {
                    shadowRectangle = value;
                    this.Invalidate();
                }
            }
        }

        private Rectangle backrectangle = new Rectangle(10, 10, 10, 10);
        /// <summary>
        /// 质感层九宫绘画区域
        /// </summary>
        [Category("Skin")]
        [DefaultValue(typeof(Rectangle), "10,10,10,10")]
        [Description("质感层九宫绘画区域")]
        public Rectangle BackRectangle
        {
            get => backrectangle;
            set
            {
                if (backrectangle != value)
                {
                    backrectangle = value;
                    this.Invalidate();
                }
            }
        }


        private Rectangle borderrectangle = new Rectangle(10, 10, 10, 10);
        /// <summary>
        /// 边框质感层九宫绘画区域
        /// </summary>
        [Category("Border")]
        [DefaultValue(typeof(Rectangle), "10,10,10,10")]
        [Description("边框质感层九宫绘画区域")]
        public Rectangle BorderRectangle
        {
            get => borderrectangle;
            set
            {
                if (borderrectangle != value)
                {
                    borderrectangle = value;
                    this.Invalidate();
                }
            }
        }

        //系统按钮激活时色调颜色
        [Category("Skin")]
        [DefaultValue(typeof(Color), "51, 153, 204")]
        [Description("系统按钮激活时色调颜色")]
        public Color ControlBoxActive
        {
            get => Colortable.ControlBoxActive;
            set
            {
                Colortable.ControlBoxActive = value;
                this.Invalidate();
            }
        }

        //系统按钮停用时色调颜色
        [Category("Skin")]
        [DefaultValue(typeof(Color), "88, 172, 218")]
        [Description("系统按钮停用时色调颜色")]
        public Color ControlBoxDeactive
        {
            get => Colortable.ControlBoxDeactive;
            set
            {
                Colortable.ControlBoxDeactive = value;
                this.Invalidate();
            }
        }

        private SkinFormColorTable _colorTable;
        public SkinFormColorTable Colortable
        {
            get
            {
                if (_colorTable == null)
                {
                    _colorTable = new SkinFormColorTable();
                }
                return _colorTable;
            }
            set => _colorTable = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Description("设置或获取窗体的绘制方法")]
        public SkinFormRenderer Renderer
        {
            get
            {

                if (_renderer == null)
                {
                    _renderer = new SkinFormProfessionalRenderer(Colortable);
                }
                return _renderer;
            }
            set
            {
                _renderer = value;
                OnRendererChanged(EventArgs.Empty);
            }
        }

        [Category("Caption")]
        public override string Text
        {
            get => base.Text;
            set
            {
                base.Text = value;
                base.Invalidate(new Rectangle(
                    0,
                    0,
                    Width,
                    CaptionHeight + 1));
            }
        }

        private bool backtocolor = true;
        /// <summary>
        /// 是否根据背景图决定背景色
        /// </summary>
        [DefaultValue(true)]
        [Category("Skin")]
        [Description("是否根据背景图决定背景色")]
        public bool BackToColor
        {
            get => backtocolor;
            set
            {
                if (backtocolor != value)
                {
                    backtocolor = value;
                    base.Invalidate();
                }
            }
        }

        private bool backshade = true;
        [DefaultValue(true)]
        [Category("Skin")]
        [Description("是否加入背景渐变效果")]
        public bool BackShade
        {
            get => backshade;
            set
            {
                if (backshade != value)
                {
                    backshade = value;
                    base.Invalidate();
                }
            }
        }

        private bool titleCenter = false;
        /// <summary>
        /// 标题是否居中
        /// </summary>
        [Category("Caption")]
        [DefaultValue(false)]
        [Description("标题是否居中")]
        public bool TitleCenter
        {
            get => titleCenter;
            set
            {
                if (titleCenter != value)
                {
                    titleCenter = value;
                    this.Invalidate();
                }
            }
        }

        private TitleType effectcaption = TitleType.EffectTitle;
        [DefaultValue(TitleType.EffectTitle)]
        [Category("Caption")]
        [Description("获取或设置标题的绘制模式")]
        public TitleType EffectCaption
        {
            get => effectcaption;
            set
            {
                if (effectcaption != value)
                {
                    effectcaption = value;
                    base.Invalidate();
                }
            }
        }

        private bool titleSuitColor = false;
        [DefaultValue(false)]
        [Category("Caption")]
        [Description("是否根据背景色自动适应标题颜色。\n(背景色为暗色时标题显示白色，背景为亮色时标题显示黑色。)")]
        public bool TitleSuitColor
        {
            get => titleSuitColor;
            set
            {
                if (titleSuitColor != value)
                {
                    titleSuitColor = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(Font), "CaptionFont")]
        [Category("Caption")]
        [Description("设置或获取窗体标题的字体")]
        public Font CaptionFont
        {
            get => _captionFont;
            set
            {
                if (value == null)
                {
                    _captionFont = SystemFonts.CaptionFont;
                }
                else
                {
                    _captionFont = value;
                }
                base.Invalidate(CaptionRect);
            }
        }

        private Color effectback = Color.White;
        /// <summary>
        /// 发光字体背景色
        /// </summary>
        [Category("Caption")]
        [DefaultValue(typeof(Color), "White")]
        [Description("发光字体背景色")]
        public Color EffectBack
        {
            get => effectback;
            set
            {
                if (effectback != value)
                {
                    effectback = value;
                    this.Invalidate();
                }
            }
        }

        private Color titleColor = Color.Black;
        /// <summary>
        /// 标题颜色
        /// </summary>
        [Category("Caption")]
        [DefaultValue(typeof(Color), "Black")]
        [Description("标题颜色")]
        public Color TitleColor
        {
            get => titleColor;
            set
            {
                if (titleColor != value)
                {
                    titleColor = value;
                    this.Invalidate();
                }
            }
        }

        private Point titleOffset = new Point(0, 0);
        /// <summary>
        /// 设置或获取标题的偏移
        /// </summary>
        [Category("Caption")]
        [DefaultValue(typeof(Point), "0,0")]
        [Description("设置或获取标题的偏移")]
        public Point TitleOffset
        {
            get => titleOffset;
            set
            {
                if (titleOffset != value)
                {
                    titleOffset = value;
                    this.Invalidate();
                }
            }
        }

        private int effectWidth = 6;
        /// <summary>
        /// 光圈大小
        /// </summary>
        [Category("Caption")]
        [DefaultValue(typeof(int), "6")]
        [Description("光圈大小")]
        public int EffectWidth
        {
            get => effectWidth;
            set
            {
                if (effectWidth != value)
                {
                    effectWidth = value;
                    this.Invalidate();
                }
            }
        }

        private bool dropback = true;
        [DefaultValue(true)]
        [Category("Skin")]
        [Description("指示控件是否可以将用户拖动到背景上的图片作为背景(注意:开启前请设置AllowDrop为true,否则无效)")]
        public bool DropBack
        {
            get => dropback;
            set
            {
                if (dropback != value)
                {
                    dropback = value;
                }
            }
        }

        [DefaultValue(typeof(RoundStyle), "1")]
        [Category("Skin")]
        [Description("设置或获取窗体的圆角样式")]
        public RoundStyle RoundStyle
        {
            get => _roundStyle;
            set
            {
                if (_roundStyle != value)
                {
                    _roundStyle = value;
                    SetReion();
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(typeof(MobileStyle), "2")]
        [Category("Skin")]
        [Description("移动窗体的条件")]
        public MobileStyle Mobile
        {
            get => _mobile;
            set
            {
                if (_mobile != value)
                {
                    _mobile = value;
                }
            }
        }

        [DefaultValue(6)]
        [Category("Skin")]
        [Description("设置或获取窗体的圆角的大小")]
        public int Radius
        {
            get => _radius;
            set
            {
                if (_radius != value)
                {
                    _radius = value < 0 ? 0 : value;
                    SetReion();
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(24)]
        [Category("Skin")]
        [Description("设置或获取窗体标题栏的高度")]
        public int CaptionHeight
        {
            get => _captionHeight;
            set
            {
                if (_captionHeight != value)
                {
                    _captionHeight = value < BorderPadding.Left ?
                                    BorderPadding.Left : value;
                    base.Invalidate();
                }
            }
        }

        [Category("MinimizeBox")]
        [DefaultValue(typeof(Size), "32, 18")]
        [Description("设置或获取最小化按钮的大小")]
        public Size MiniSize
        {
            get => _miniSize;
            set
            {
                if (_miniSize != value)
                {
                    _miniSize = value;
                    base.Invalidate(ControlBoxManager.MinimizeBoxRect);
                }
            }
        }

        private Image minimouseback;
        /// <summary>
        /// 最小化按钮悬浮时
        /// </summary>
        [Category("MinimizeBox")]
        [Description("最小化按钮悬浮时背景")]
        public Image MiniMouseBack
        {
            get => minimouseback;
            set
            {
                if (minimouseback != value)
                {
                    minimouseback = value;
                    this.Invalidate(ControlBoxManager.MinimizeBoxRect);
                }
            }
        }

        private Image minidownback;
        /// <summary>
        /// 最小化按钮点击时
        /// </summary>
        [Category("MinimizeBox")]
        [Description("最小化按钮点击时背景")]
        public Image MiniDownBack
        {
            get => minidownback;
            set
            {
                if (minidownback != value)
                {
                    minidownback = value;
                    this.Invalidate(ControlBoxManager.MinimizeBoxRect);
                }
            }
        }

        private Image mininormlback;
        /// <summary>
        /// 最小化按钮初始时
        /// </summary>
        [Category("MinimizeBox")]
        [Description("最小化按钮初始时背景")]
        public Image MiniNormlBack
        {
            get => mininormlback;
            set
            {
                if (mininormlback != value)
                {
                    mininormlback = value;
                    this.Invalidate(ControlBoxManager.MinimizeBoxRect);
                }
            }
        }

        [Category("MaximizeBox")]
        [DefaultValue(typeof(Size), "32, 18")]
        [Description("设置或获取最大化（还原）按钮的大小")]
        public Size MaxSize
        {
            get => _maxBoxSize;
            set
            {
                if (_maxBoxSize != value)
                {
                    _maxBoxSize = value;
                    base.Invalidate(ControlBoxManager.MaximizeBoxRect);
                }
            }
        }

        private Image maxmouseback;
        /// <summary>
        /// 最大化按钮悬浮时背景
        /// </summary>
        [Category("MaximizeBox")]
        [Description("最大化按钮悬浮时背景")]
        public Image MaxMouseBack
        {
            get => maxmouseback;
            set
            {
                if (maxmouseback != value)
                {
                    maxmouseback = value;
                    this.Invalidate(ControlBoxManager.MaximizeBoxRect);
                }
            }
        }

        private Image maxdownback;
        /// <summary>
        /// 最大化按钮点击时背景
        /// </summary>
        [Category("MaximizeBox")]
        [Description("最大化按钮点击时背景")]
        public Image MaxDownBack
        {
            get => maxdownback;
            set
            {
                if (maxdownback != value)
                {
                    maxdownback = value;
                    this.Invalidate(ControlBoxManager.MaximizeBoxRect);
                }
            }
        }

        private Image maxnormlback;
        /// <summary>
        /// 最大化按钮初始时背景
        /// </summary>
        [Category("MaximizeBox")]
        [Description("最大化按钮初始时背景")]
        public Image MaxNormlBack
        {
            get => maxnormlback;
            set
            {
                if (maxnormlback != value)
                {
                    maxnormlback = value;
                    this.Invalidate(ControlBoxManager.MaximizeBoxRect);
                }
            }
        }

        private Image restoremouseback;
        /// <summary>
        /// 还原按钮悬浮时背景
        /// </summary>
        [Category("MaximizeBox")]
        [Description("还原按钮悬浮时背景")]
        public Image RestoreMouseBack
        {
            get => restoremouseback;
            set
            {
                if (restoremouseback != value)
                {
                    restoremouseback = value;
                    this.Invalidate(ControlBoxManager.MaximizeBoxRect);
                }
            }
        }

        private Image restoredownback;
        /// <summary>
        /// 还原按钮点击时背景
        /// </summary>
        [Category("MaximizeBox")]
        [Description("还原按钮点击时背景")]
        public Image RestoreDownBack
        {
            get => restoredownback;
            set
            {
                if (restoredownback != value)
                {
                    restoredownback = value;
                    this.Invalidate(ControlBoxManager.MaximizeBoxRect);
                }
            }
        }

        private Image restorenormlback;
        /// <summary>
        /// 还原按钮初始时背景
        /// </summary>
        [Category("MaximizeBox")]
        [Description("还原按钮初始时背景")]
        public Image RestoreNormlBack
        {
            get => restorenormlback;
            set
            {
                if (restorenormlback != value)
                {
                    restorenormlback = value;
                    this.Invalidate(ControlBoxManager.MaximizeBoxRect);
                }
            }
        }

        [Category("CloseBox")]
        [DefaultValue(typeof(Size), "32, 18")]
        [Description("设置或获取关闭按钮的大小")]
        public Size CloseBoxSize
        {
            get => _closeBoxSize;
            set
            {
                if (_closeBoxSize != value)
                {
                    _closeBoxSize = value;
                    base.Invalidate(ControlBoxManager.CloseBoxRect);
                }
            }
        }

        private Image closemouseback;
        /// <summary>
        /// 关闭按钮悬浮时背景
        /// </summary>
        [Category("CloseBox")]
        [Description("关闭按钮悬浮时背景")]
        public Image CloseMouseBack
        {
            get => closemouseback;
            set
            {
                if (closemouseback != value)
                {
                    closemouseback = value;
                    this.Invalidate(ControlBoxManager.CloseBoxRect);
                }
            }
        }

        private Image closedownback;
        /// <summary>
        /// 关闭按钮点击时背景
        /// </summary>
        [Category("CloseBox")]
        [Description("关闭按钮点击时背景")]
        public Image CloseDownBack
        {
            get => closedownback;
            set
            {
                if (closedownback != value)
                {
                    closedownback = value;
                    this.Invalidate(ControlBoxManager.CloseBoxRect);
                }
            }
        }

        private Image closenormlback;
        /// <summary>
        /// 关闭按钮初始时背景
        /// </summary>
        [Category("CloseBox")]
        [Description("关闭按钮初始时背景")]
        public Image CloseNormlBack
        {
            get => closenormlback;
            set
            {
                if (closenormlback != value)
                {
                    closenormlback = value;
                    this.Invalidate(ControlBoxManager.CloseBoxRect);
                }
            }
        }

        /// <summary>
        /// 获取或设置窗体是否显示系统菜单。
        /// </summary>
        [DefaultValue(false)]
        [Category("Skin")]
        [Description("获取或设置窗体是否显示系统菜单")]
        public bool ShowSystemMenu
        {
            get => _showSystemMenu;
            set => _showSystemMenu = value;
        }

        private Point iCoOffset = new Point(0, 0);
        [DefaultValue(typeof(Point), "0, 0")]
        [Category("Skin")]
        [Description("设置或获取ICO图标的偏移")]
        public Point ICoOffset
        {
            get => iCoOffset;
            set
            {
                if (iCoOffset != value)
                {
                    iCoOffset = value;
                    base.Invalidate();
                }
            }
        }


        [DefaultValue(typeof(Point), "6, 0")]
        [Category("Skin")]
        [Description("设置或获取控制按钮的偏移")]
        public Point ControlBoxOffset
        {
            get => _controlBoxOffset;
            set
            {
                if (_controlBoxOffset != value)
                {
                    _controlBoxOffset = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(0)]
        [Category("Skin")]
        [Description("设置或获取控制按钮的间距")]
        public int ControlBoxSpace
        {
            get => _controlBoxSpace;
            set
            {
                if (_controlBoxSpace != value)
                {
                    _controlBoxSpace = value;
                    base.Invalidate();
                }
            }
        }

        [DefaultValue(true)]
        [Category("Skin")]
        [Description("设置或获取窗体是否可以改变大小")]
        public bool CanResize
        {
            get => _canResize;
            set => _canResize = value;
        }

        [DefaultValue(typeof(Padding), "0")]
        public new Padding Padding
        {
            get => _padding;
            set
            {
                _padding = value;
                base.Padding = new Padding(
                    BorderPadding.Left + _padding.Left,
                    CaptionHeight + BorderPadding.Top + _padding.Top,
                    BorderPadding.Right + _padding.Right,
                    BorderPadding.Bottom + _padding.Bottom);
            }
        }

        public override Rectangle DisplayRectangle
        {
            get
            {
                Rectangle rect = RealClientRect;
                rect.X += (_borderPadding.Left + Padding.Left);
                rect.Y += (_borderPadding.Top + _captionHeight + Padding.Top);
                rect.Width -= (_borderPadding.Horizontal + Padding.Horizontal);
                rect.Height -= (_borderPadding.Vertical + _captionHeight + Padding.Vertical);
                return rect;
            }
        }

        //[Browsable(false)]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FormBorderStyle FormBorderStyle
        {
            get => base.FormBorderStyle;
            set => base.FormBorderStyle = value;
        }

        protected override Padding DefaultPadding => new Padding(
                    BorderPadding.Left,
                    BorderPadding.Top + CaptionHeight,
                    BorderPadding.Right,
                    BorderPadding.Bottom);

        public Rectangle CaptionRect => new Rectangle(0, 0, Width, CaptionHeight);

        public ControlBoxManager ControlBoxManager
        {
            get
            {
                if (_controlBoxManager == null)
                {
                    _controlBoxManager = new ControlBoxManager(this);
                }
                return _controlBoxManager;
            }
        }

        public Rectangle IconRect
        {
            get
            {
                if (this.ShowDrawIcon && base.Icon != null)
                {
                    int width = SystemInformation.SmallIconSize.Width;
                    if (CaptionHeight - BorderPadding.Left - 4 < width)
                    {
                        width = CaptionHeight - BorderPadding.Left - 4;
                    }
                    return new Rectangle(
                        BorderPadding.Left + ICoOffset.X,
                        BorderPadding.Left + (CaptionHeight - BorderPadding.Left - width) / 2 + ICoOffset.Y,
                        width,
                        width);
                }
                return Rectangle.Empty;
            }
        }

        public ToolTip ToolTip => _toolTip;

        /// <summary>
        /// 获取窗体的真实客户区大小。
        /// </summary>
        public Rectangle RealClientRect
        {
            get
            {
                if (base.WindowState == FormWindowState.Maximized)
                {
                    return new Rectangle(
                        _deltaRect.X, _deltaRect.Y,
                        base.Width - _deltaRect.Width, base.Height - _deltaRect.Height);
                }
                else
                {
                    return new Rectangle(Point.Empty, base.Size);
                }
            }
        }

        protected Size MaximumSizeFromMaximinClientSize()
        {
            Size maximumSize = Size.Empty;
            if (MaximumSize != Size.Empty)
            {
                maximumSize.Width = MaximumSize.Width + _borderPadding.Horizontal;
                maximumSize.Height = MaximumSize.Height +
                    _borderPadding.Vertical + _captionHeight;

            }
            return maximumSize;
        }

        /// <summary>
        /// 所以自定义系统按钮的总宽度
        /// </summary>
        /// <param name="space">宽度是否包括ControlBoxSpace间隔值</param>
        /// <returns>总宽度</returns>
        protected int AllSysButtonWidth(bool space)
        {
            int SysWidth = 0;
            foreach (CmSysButton item in ControlBoxManager.SysButtonItems)
            {
                if (item.Visibale)
                {
                    SysWidth += item.Size.Width;
                    if (space)
                    {
                        SysWidth += this.ControlBoxSpace;
                    }
                }
            }
            return SysWidth;
        }

        /// <summary>
        /// 所有系统按钮的总宽度
        /// </summary>
        /// <param name="space">宽度是否包括ControlBoxSpace间隔值</param>
        /// <returns>总宽度</returns>
        public int AllButtonWidth(bool space)
        {
            int SysWidth = 0;
            foreach (CmSysButton item in ControlBoxManager.SysButtonItems)
            {
                if (item.Visibale)
                {
                    SysWidth += item.Size.Width;
                    if (space)
                    {
                        SysWidth += this.ControlBoxSpace;
                    }
                }
            }
            SysWidth +=
                CloseBoxSize.Width +
                (MinimizeBox ? MiniSize.Width + (space ? ControlBoxSpace : 0) : 0) +
                (MaximizeBox ? MaxSize.Width + (space ? ControlBoxSpace : 0) : 0);
            return SysWidth;
        }

        protected virtual Size GetDefaultMinTrackSize()
        {

            return new Size(
                    AllButtonWidth(true) + _borderPadding.Horizontal +
                    SystemInformation.SmallIconSize.Width + 20,
                    CaptionHeight + _borderPadding.Vertical + 2);
        }

        protected Size MinimumSizeFromMiniminClientSize()
        {
            Size minimumSize = GetDefaultMinTrackSize();
            if (MinimumSize != Size.Empty)
            {
                minimumSize.Width = MinimumSize.Width + _borderPadding.Horizontal;
                minimumSize.Height = MinimumSize.Height +
                    _borderPadding.Vertical + _captionHeight;
            }
            return minimumSize;
        }
        #endregion

        #region 自定义事件
        public virtual void SkinPaint(Graphics g, CSSkinMain f) { }

        public delegate void BackEventHandler(object sender, BackEventArgs e);
        public delegate void ThemeEventHandler(object sender, ThemeEventArgs e);
        public delegate void SysBottomEventHandler(object sender, SysButtonEventArgs e);

        [Description("自定义按钮被点击时引发的事件")]
        [Category("Skin")]
        public event SysBottomEventHandler SysBottomClick;
        protected virtual void OnSysBottomClick(object sender, SysButtonEventArgs e)
        {
            if (this.SysBottomClick != null)
            {
                SysBottomClick(this, e);
            }
        }

        public void SysbottomAv(object sender, SysButtonEventArgs e)
        {
            //引发事件
            OnSysBottomClick(sender, e);
        }

        [Description("Back属性值更改时引发的事件")]
        [Category("Skin")]
        public event BackEventHandler BackChanged;
        protected virtual void OnBackChanged(BackEventArgs e)
        {
            if (this.BackChanged != null)
            {
                BackChanged(this, e);
            }
        }

        [Description("XTheme主题属性值更改时引发的事件")]
        [Category("Skin")]
        public event ThemeEventHandler ThemeChanged;
        protected virtual void OnThemeChanged(ThemeEventArgs e)
        {
            if (this.ThemeChanged != null)
            {
                ThemeChanged(this, e);
            }
        }

        public event EventHandler RendererChangled
        {
            add { base.Events.AddHandler(EventRendererChanged, value); }
            remove { base.Events.RemoveHandler(EventRendererChanged, value); }
        }
        #endregion

        #region 重载事件
        //控件首次创建时被调用
        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            SetReion();
            Form onf = Owner == null ? this.ParentForm : Owner;
            if (onf is CSSkinMain)
            {
                CSSkinMain main = (CSSkinMain)onf;
                if (this.InheritTheme)
                {
                    this.XTheme = main.XTheme;
                    main.ThemeChanged += main_ThemeChanged;
                }
                if (InheritBack)
                {
                    this.BackLayout = main.BackLayout;
                    this.Back = main.Back;
                    this.BackToColor = main.BackToColor;
                    this.BackColor = main.BackColor;
                    this.BackgroundImageLayout = main.BackgroundImageLayout;
                    this.BackgroundImage = main.BackgroundImage;
                    main.BackChanged += new BackEventHandler(main_BackChanged);
                }
            }
            else if (onf is Form && this.InheritBack)
            {
                Form main = onf;
                this.Back = main.BackgroundImage;
                this.BackLayout = true;
                this.BackColor = main.BackgroundImage == null ? main.BackColor : this.BackToColor ? (SkinTools.GetImageAverageColor((Bitmap)main.BackgroundImage)) : main.BackColor;
                main.BackgroundImageChanged += new EventHandler(main_BackgroundImageChanged);
            }
            ////MDI模式
            //if (IsMdiContainer) {
            //    this.SuspendLayout();
            //    this.ResumeLayout(false);
            //}
        }

        /// <summary>
        /// 主题变换时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void main_ThemeChanged(object sender, ThemeEventArgs e)
        {
            if (this.InheritTheme)
            {
                CSSkinMain main = (CSSkinMain)sender;
                this.XTheme = main.XTheme;
            }
        }

        /// <summary>
        /// 背景变换时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void main_BackChanged(object sender, BackEventArgs e)
        {
            if (this.InheritBack)
            {
                CSSkinMain main = (CSSkinMain)sender;
                this.BackToColor = true;
                this.Back = main.Back;
                this.BackLayout = main.BackLayout;
            }
        }

        /// <summary>
        /// 背景变换时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void main_BackgroundImageChanged(object sender, EventArgs e)
        {
            if (InheritBack)
            {
                Form main = (Form)sender;
                this.Back = main.BackgroundImage;
                this.BackLayout = true;
                this.BackColor = main.BackgroundImage == null ? main.BackColor : (this.BackToColor ? SkinTools.GetImageAverageColor((Bitmap)main.BackgroundImage) : main.BackColor);
            }
        }

        //Show或Hide被调用时
        private bool OneVisibles = true;
        protected override void OnVisibleChanged(EventArgs e)
        {
            if (Visible)
            {
                //启用窗口淡入淡出
                if (Special && !DesignMode)
                {
                    int House = OneVisibles && Shadow ? 300 : 150;
                    //淡入特效
                    NativeMethods.AnimateWindow(this.Handle, House, AW.AW_BLEND | AW.AW_ACTIVATE);
                    if (Opacity != SkinOpacity)
                    {
                        Opacity = SkinOpacity;
                    }
                }
                //判断不是在设计器中 && Shadow
                if (!DesignMode && skin == null && Shadow)
                {
                    skin = new CSSkinForm(this);
                    skin.Show(this);
                }
                OneVisibles = false;
                base.OnVisibleChanged(e);
            }
            else
            {
                base.OnVisibleChanged(e);
                //启用窗口淡入淡出
                if (Special && !DesignMode)
                {
                    if (Opacity != 1)
                    {
                        Opacity = 1;
                    }
                    //实现窗体的淡出
                    NativeMethods.AnimateWindow(this.Handle, 150, AW.AW_BLEND | AW.AW_HIDE);
                }
            }
        }

        //窗口加载时
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ResizeCore();
        }

        //窗体绘画样式变了的时候
        protected virtual void OnRendererChanged(EventArgs e)
        {
            Renderer.InitSkinForm(this);
            EventHandler handler =
                base.Events[EventRendererChanged] as EventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
            base.Invalidate();
        }

        //点击时
        public bool isMouseDown = false;
        private int currentXPosition = 0;
        private int currentYPosition = 0;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Point point = e.Location;
            if (e.Button == MouseButtons.Left && e.Clicks == 1)
            {
                bool flag = true;
                foreach (CmSysButton item in ControlBoxManager.SysButtonItems)
                {
                    if (item.Bounds.Contains(point))
                    {
                        flag = false;
                        break;
                    }
                }
                //除系统按钮区域以外才能移动窗体
                if (!ControlBoxManager.CloseBoxRect.Contains(point) &&
                    !ControlBoxManager.MaximizeBoxRect.Contains(point) &&
                    !ControlBoxManager.MinimizeBoxRect.Contains(point) &&
                    Mobile != MobileStyle.None && flag)
                {
                    //标题栏以外也可以移动
                    if (Mobile == MobileStyle.Mobile)
                    {
                        //调用Api移动窗体
                        if (MobileApi)
                        {
                            MoveForm();
                        }
                        else if (this.WindowState != FormWindowState.Maximized)
                        {
                            //不调用Api移动窗体
                            //记录开始移动
                            isMouseDown = true;
                            currentXPosition = MousePosition.X;
                            currentYPosition = MousePosition.Y;
                        }
                    }
                    else if (Mobile == MobileStyle.TitleMobile && point.Y < CaptionHeight)
                    {
                        //调用Api移动窗体
                        if (MobileApi)
                        {
                            MoveForm();
                        }
                        else if (this.WindowState != FormWindowState.Maximized)
                        {
                            //不调用Api移动窗体
                            //记录开始移动
                            isMouseDown = true;
                            currentXPosition = MousePosition.X;
                            currentYPosition = MousePosition.Y;
                        }
                    }
                    //调用Api移动窗体
                    if (MobileApi)
                    {
                        OnClick(e);
                        OnMouseClick(e);
                        OnMouseUp(e);
                    }
                }
                else
                {
                    //画窗体按钮的按下样式
                    ControlBoxManager.ProcessMouseOperate(
                        e.Location, MouseOperationType.Down);
                }
            }
        }

        /// <summary>
        /// 移动窗体
        /// </summary>
        public void MoveForm()
        {
            //释放鼠标焦点捕获
            NativeMethods.ReleaseCapture();
            //向当前窗体发送拖动消息
            NativeMethods.SendMessage(this.Handle, 0x0112, 0xF011, 0);
        }

        //点击并释放按钮时
        protected override void OnMouseUp(MouseEventArgs e)
        {
            //不调用Api移动窗体
            if (!MobileApi)
            {
                //停止移动
                isMouseDown = false;
                //位置Top小于0，则恢复Top=0
                if (this.Top < 0)
                {
                    this.Top = 0;
                }
            }
            base.OnMouseUp(e);
            //画窗体按钮按下并释放鼠标时样式
            ControlBoxManager.ProcessMouseOperate(
                e.Location, MouseOperationType.Up);
        }

        //移动时
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            //不调用Api移动窗体
            if (!MobileApi)
            {
                if (isMouseDown)
                {
                    this.Left += MousePosition.X - currentXPosition;
                    this.Top += MousePosition.Y - currentYPosition;
                    currentXPosition = MousePosition.X;
                    currentYPosition = MousePosition.Y;
                }
            }
            ControlBoxManager.ProcessMouseOperate(
                e.Location, MouseOperationType.Move);
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && MaximizeBox)
            {
                bool flag = true;
                Point point = e.Location;
                foreach (CmSysButton item in ControlBoxManager.SysButtonItems)
                {
                    if (item.Bounds.Contains(point))
                    {
                        flag = false;
                        break;
                    }
                }
                //除系统按钮区域以外才能移动窗体
                if (!ControlBoxManager.CloseBoxRect.Contains(point) &&
                    !ControlBoxManager.MaximizeBoxRect.Contains(point) &&
                    !ControlBoxManager.MinimizeBoxRect.Contains(point) && flag)
                {
                    if (Mobile == MobileStyle.Mobile)
                    {
                        WindowState = WindowState == FormWindowState.Maximized ?
                            this.WindowState = FormWindowState.Normal :
                            WindowState = FormWindowState.Maximized;
                    }
                    else if (Mobile == MobileStyle.TitleMobile && e.Y < CaptionHeight)
                    {
                        WindowState = WindowState == FormWindowState.Maximized ?
                            this.WindowState = FormWindowState.Normal :
                            WindowState = FormWindowState.Maximized;
                    }
                }
            }
            base.OnMouseDoubleClick(e);
        }

        //离开时
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            ControlBoxManager.ProcessMouseOperate(
                Point.Empty, MouseOperationType.Leave);
        }

        //悬浮时
        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            ControlBoxManager.ProcessMouseOperate(
                PointToClient(MousePosition), MouseOperationType.Hover);
        }

        //窗体移动时
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
            mStopAnthor();
        }

        public AnchorStyles Aanhor = AnchorStyles.None;
        //更新状态
        private void mStopAnthor()
        {
            if (this.Left <= 0)
            {
                Aanhor = AnchorStyles.Left;
            }
            else if (this.Left >= Screen.PrimaryScreen.Bounds.Width - this.Width)
            {
                Aanhor = AnchorStyles.Right;
            }
            else if (this.Top <= 0)
            {
                Aanhor = AnchorStyles.Top;
            }
            else if (this.Top + this.Height >= Screen.PrimaryScreen.Bounds.Height)
            {
                Aanhor = AnchorStyles.Bottom;
            }
            else
            {
                Aanhor = AnchorStyles.None;
            }
        }

        //重绘
        protected override void OnPaint(PaintEventArgs e)
        {
            if (IsMdiContainer)
            {
                base.OnPaint(e);
            }
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias; //高质量
            //最高质量绘制文字
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            //是否是MDI模式
            if (IsMdiContainer)
            {
                //画背景色
                g.FillRectangle(new SolidBrush(BackColor), this.ClientRectangle);
                //画背景
                Bitmap bmp = new Bitmap(Width, Height);
                DrawImage.ImageFillRect((Bitmap)BackgroundImage, bmp, BackgroundImageLayout);
                g.DrawImage(bmp, 0, 0);
            }
            //画Back背景渐变
            if (Back != null)
            {
                //画Back背景
                if (BackLayout)
                {
                    g.DrawImage(Back, 0, 0, Back.Width, Back.Height);
                }
                else
                {
                    g.DrawImage(Back, -(Back.Width - Width), 0, Back.Width, Back.Height);
                }
                //画渐变背景
                if (BackShade)
                {
                    //背景从左绘制，阴影右画
                    if (BackLayout)
                    {
                        LinearGradientBrush brush = new LinearGradientBrush(
                            new Rectangle(Back.Width - 50, 0, 50, Back.Height), BackColor,
                            Color.Transparent, 180);
                        LinearGradientBrush brushTwo = new LinearGradientBrush(
                            new Rectangle(0, Back.Height - 50, Back.Width, 50), BackColor,
                            Color.Transparent, 270);
                        g.FillRectangle(brush, Back.Width - brush.Rectangle.Width + 1, 0, brush.Rectangle.Width, brush.Rectangle.Height);
                        g.FillRectangle(brushTwo, 0, Back.Height - brushTwo.Rectangle.Height + 1, brushTwo.Rectangle.Width, brushTwo.Rectangle.Height);
                    }
                    else
                    {
                        //背景从右绘制，阴影左画
                        LinearGradientBrush brush = new LinearGradientBrush(
                            new Rectangle(-(Back.Width - Width), 0, 50, Back.Height), BackColor,
                            Color.Transparent, 360);
                        LinearGradientBrush brushTwo = new LinearGradientBrush(
                            new Rectangle(-(Back.Width - Width), Back.Height - 50, Back.Width, 50), BackColor,
                            Color.Transparent, 270);
                        g.FillRectangle(brush, -(Back.Width - Width), 0, brush.Rectangle.Width, brush.Rectangle.Height);
                        g.FillRectangle(brushTwo, -(Back.Width - Width), Back.Height - 50, brushTwo.Rectangle.Width, brushTwo.Rectangle.Height);
                    }
                }
            }
            //画标题栏背景
            DrawCaptionBackground(g);
            if (!IsMdiContainer)
            {
                base.OnPaint(e);
            }
            //不是提示框才绘制特定主题绘制模式
            if (!(this is MessageBoxForm))
            {
                //未变化主题
                if (xTheme == null)
                {
                    this.SkinPaint(g, this);
                }
                else
                {
                    //变化主题，画变化主题中的Paint
                    xTheme.SkinPaint(g, this);
                }
            }
            //调用重载窗体的方法绘制
            SkinPaint(g, this);
            g.SmoothingMode = SmoothingMode.AntiAlias; //高质量
            Rectangle rect = ClientRectangle;
            SkinFormRenderer renderer = Renderer;
            //画九宫质感层
            if (BackPalace != null)
            {
                ImageDrawRect.DrawRect(g, (Bitmap)BackPalace, new Rectangle(ClientRectangle.X - 5, ClientRectangle.Y - 5, ClientRectangle.Width + 10, ClientRectangle.Height + 10), Rectangle.FromLTRB(BackRectangle.X, BackRectangle.Y, BackRectangle.Width, BackRectangle.Height), 1, 1);
            }
            //画关闭按钮
            if (ControlBoxManager.CloseBoxVisibale)
            {
                renderer.DrawSkinFormControlBox(
                    new SkinFormControlBoxRenderEventArgs(
                    this,
                    g,
                    ControlBoxManager.CloseBoxRect,
                    _active,
                    ControlBoxStyle.Close,
                    ControlBoxManager.CloseBoxState));
            }
            //画最大化按钮
            if (ControlBoxManager.MaximizeBoxVisibale)
            {
                renderer.DrawSkinFormControlBox(
                    new SkinFormControlBoxRenderEventArgs(
                    this,
                    g,
                    ControlBoxManager.MaximizeBoxRect,
                    _active,
                    ControlBoxStyle.Maximize,
                    ControlBoxManager.MaximizeBoxState));
            }
            //画最小化按钮
            if (ControlBoxManager.MinimizeBoxVisibale)
            {
                renderer.DrawSkinFormControlBox(
                    new SkinFormControlBoxRenderEventArgs(
                    this,
                    g,
                    ControlBoxManager.MinimizeBoxRect,
                    _active,
                    ControlBoxStyle.Minimize,
                    ControlBoxManager.MinimizeBoxState));
            }
            //画自定义系统按钮
            foreach (CmSysButton item in ControlBoxManager.SysButtonItems)
            {
                if (item.Visibale)
                {
                    renderer.DrawSkinFormControlBox(
                        new SkinFormControlBoxRenderEventArgs(
                        this,
                        g,
                        item.Bounds,
                        _active,
                        ControlBoxStyle.CmSysBottom,
                        item.BoxState,
                        item));
                }
            }
            if (ShowBorder)
            {
                //画边框
                renderer.DrawSkinFormBorder(
                  new SkinFormBorderRenderEventArgs(
                  this, g, rect, _active));
            }
            //画边框质感层
            if (BorderPalace != null)
            {
                ImageDrawRect.DrawRect(g, (Bitmap)BorderPalace, new Rectangle(ClientRectangle.X - 5, ClientRectangle.Y - 5, ClientRectangle.Width + 10, ClientRectangle.Height + 10), Rectangle.FromLTRB(BorderRectangle.X, BorderRectangle.Y, BorderRectangle.Width, BorderRectangle.Height), 1, 1);
            }
            //画标题栏
            renderer.DrawSkinFormCaption(
                new SkinFormCaptionRenderEventArgs(
                this, g, CaptionRect, _active));
        }

        /// <summary>
        /// 画标题栏背景
        /// </summary>
        /// <param name="g"></param>
        private void DrawCaptionBackground(Graphics g)
        {
            using (LinearGradientBrush lb = new LinearGradientBrush(
                 CaptionRectToDraw,
                 CaptionBackColorTop,
                 CaptionBackColorBottom,
                 LinearGradientMode.Vertical))
            {
                g.FillRectangle(lb, CaptionRectToDraw);
            }
        }

        //拖到图片至背景时
        protected override void OnDragDrop(DragEventArgs drgevent)
        {
            if (DropBack)
            {
                //捕获到的字符串数组(包含拖放文件的完整路径名)   
                string[] myFiles = (string[])(drgevent.Data.GetData(DataFormats.FileDrop));
                FileInfo f = new FileInfo(myFiles[0]);
                if (myFiles != null)
                {
                    string Type = f.Extension.Substring(1);
                    string[] TypeList = { "png", "bmp", "jpg", "jpeg", "gif" };
                    if (((IList)TypeList).Contains(Type.ToLower()))
                    {
                        //我这里设置捕获到的第一张图片设为背景   
                        this.Back = Image.FromFile(myFiles[0]);
                    }
                }
            }
            base.OnDragDrop(drgevent);
        }

        //拖到图片并悬浮至背景时，鼠标样式
        protected override void OnDragEnter(DragEventArgs drgevent)
        {
            if (DropBack)
            {
                //拖放时显示的效果   
                drgevent.Effect = DragDropEffects.Link;
            }
            base.OnDragEnter(drgevent);
        }

        protected override void OnStyleChanged(EventArgs e)
        {
            if (_clientSizeSet)
            {
                ClientSize = ClientSize;
                _clientSizeSet = false;
            }
            base.OnStyleChanged(e);
        }

        protected override void OnResize(EventArgs e)
        {
            ResizeCore();
            base.OnResize(e);
        }

        /// <summary>
        /// 窗体改变大小。
        /// </summary>
        protected virtual void ResizeCore()
        {
            CalcDeltaRect();
            SetReion();
        }

        protected void CalcDeltaRect()
        {
            if (base.WindowState == FormWindowState.Maximized)
            {
                Rectangle bounds = base.Bounds;
                Rectangle realRect = Rectangle.Empty;
                if (this.FormBorderStyle == FormBorderStyle.None)
                {
                    realRect = Screen.GetBounds(this);
                }
                else
                {
                    realRect = Screen.GetWorkingArea(this);
                }
                realRect.X -= _borderPadding.Left;
                realRect.Y -= _borderPadding.Top;
                realRect.Width += _borderPadding.Horizontal;
                realRect.Height += _borderPadding.Vertical;

                int x = 0;
                int y = 0;
                int width = 0;
                int height = 0;

                if (bounds.Left < realRect.Left)
                {
                    x = realRect.Left - bounds.Left;
                }

                if (bounds.Top < realRect.Top)
                {
                    y = realRect.Top - bounds.Top;
                }

                if (bounds.Width > realRect.Width)
                {
                    width = bounds.Width - realRect.Width;
                }

                if (bounds.Height > realRect.Height)
                {
                    height = bounds.Height - realRect.Height;
                }
                _deltaRect = new Rectangle(x, y, width, height);
            }
            else
            {
                _deltaRect = Rectangle.Empty;
            }
        }

        //窗体关闭时
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            //先关闭阴影窗体
            if (skin != null)
            {
                skin.Close();
            }
            //启用窗口淡入淡出
            if (Special && !DesignMode)
            {
                //在Form_FormClosing中添加代码实现窗体的淡出
                if (!this.IsDisposed)
                {
                    NativeMethods.AnimateWindow(this.Handle, 150, AW.AW_BLEND | AW.AW_HIDE);
                    Update();
                }
            }
        }
        #endregion

        #region 资源释放
        ~CSSkinMain()
        {
            Dispose(false);
        }
        //释放资源文件
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                if (_controlBoxManager != null)
                {
                    _controlBoxManager.Dispose();
                    _controlBoxManager = null;
                }
                this.XTheme = null;
                this.ShadowPalace = null;
                this.BackPalace = null;
                this.BorderPalace = null;
                this.CloseDownBack = null;
                this.CloseMouseBack = null;
                this.CloseNormlBack = null;
                this.MaxDownBack = null;
                this.MaxMouseBack = null;
                this.MaxNormlBack = null;
                this.MiniDownBack = null;
                this.MiniMouseBack = null;
                this.MiniNormlBack = null;
                this.RestoreDownBack = null;
                this.RestoreMouseBack = null;
                this.RestoreNormlBack = null;
                //可能有BUG
                //this.SysButtonItems.Dispose();
                _renderer = null;
                _toolTip.Dispose();
                GC.Collect();
            }
        }
        #endregion

        #region 处理Windows消息的方法
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;
        //拦截消息
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM.WM_NCHITTEST:
                    WmNcHitTest(ref m);
                    break;
                case WM.WM_NCPAINT:
                    break;
                case WM.WM_NCCALCSIZE:
                    WmNcCalcSize(ref m);
                    break;
                case WM.WM_WINDOWPOSCHANGED:
                    WmWindowPosChanged(ref m);
                    break;
                case WM.WM_GETMINMAXINFO:
                    WmGetMinMaxInfo(ref m);
                    break;
                case WM.WM_NCACTIVATE:
                    WmNcActive(ref m);
                    break;
                case WM.WM_NCRBUTTONUP:
                    WmNcRButtonUp(ref m);
                    break;
                case WM.WM_NCUAHDRAWCAPTION:
                case WM.WM_NCUAHDRAWFRAME:
                    m.Result = Result.TRUE;
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        protected override void DefWndProc(ref Message m)
        {
            base.DefWndProc(ref m);
        }

        /// <summary>
        /// 响应 WM_WINDOWPOSCHANGED 消息。
        /// </summary>
        /// <param name="m"></param>
        protected virtual void WmWindowPosChanged(ref Message m)
        {
            _inWmWindowPosChanged++;
            base.WndProc(ref m);
            _inWmWindowPosChanged--;
        }

        /// <summary>
        /// 响应 WM_NCRBUTTONUP 消息。
        /// </summary>
        /// <param name="m"></param>
        protected virtual void WmNcRButtonUp(ref Message m)
        {
            TrackPopupSysMenu(ref m);
            base.WndProc(ref m);
        }

        protected void TrackPopupSysMenu(ref Message m)
        {
            if (m.WParam.ToInt32() == HITTEST.HTCAPTION)
            {
                TrackPopupSysMenu(m.HWnd, new Point(m.LParam.ToInt32()));
            }
        }

        protected void TrackPopupSysMenu(IntPtr hWnd, Point point)
        {
            if (_showSystemMenu && point.Y <= Top + _borderPadding.Top + _deltaRect.Y + _captionHeight)
            {
                IntPtr hMenu = NativeMethods.GetSystemMenu(hWnd, false);
                IntPtr command = NativeMethods.TrackPopupMenu(hMenu,
                   TPM.TPM_RETURNCMD | TPM.TPM_TOPALIGN | TPM.TPM_LEFTALIGN,
                   point.X, point.Y, 0, hWnd, IntPtr.Zero);
                NativeMethods.PostMessage(hWnd, WM.WM_SYSCOMMAND, command, IntPtr.Zero);
            }
        }

        /// <summary>
        /// 响应 WM_NCCALCSIZE 消息。
        /// </summary>
        /// <param name="m"></param>
        protected virtual void WmNcCalcSize(ref Message m)
        {
            //if (m.WParam == new IntPtr(1)) {
            //    WinAPI.NCCALCSIZE_PARAMS info = (WinAPI.NCCALCSIZE_PARAMS)
            //        Marshal.PtrToStructure(m.LParam, typeof(WinAPI.NCCALCSIZE_PARAMS));
            //    if (IsAboutToMaximize(info.rectNewForm)) {
            //        Rectangle workingRect = Screen.GetWorkingArea(this);
            //        info.rectNewForm.Left = workingRect.Left - BorderPadding.Left;
            //        info.rectNewForm.Top = workingRect.Top - BorderPadding.Top;
            //        info.rectNewForm.Right = workingRect.Right + BorderPadding.Right;
            //        info.rectNewForm.Bottom = workingRect.Bottom + BorderPadding.Bottom;
            //        Marshal.StructureToPtr(info, m.LParam, false);
            //    }
            //}
            if (base.Opacity != 1.0d)
            {
                Invalidate();
            }
        }

        /// <summary>
        /// 判断所接收到的 wm_nc-calc-size 消息是否指示窗体即将最大化
        /// </summary>        
        private bool IsAboutToMaximize(WinAPI.RECT rect)
        {
            /*
             * 判断的方法是，只要窗体的左右、上下都延伸到了屏幕工作区之外，
             * 并且左和右、上和下都延伸相同的量，就认为窗体是要进行最大化
             */

            int left = rect.Left;
            int top = rect.Top;
            int width = rect.Right - rect.Left;
            int height = rect.Bottom - rect.Top;

            if (left < 0 && top < 0)
            {
                Rectangle workingArea = Screen.GetWorkingArea(this);
                if (width == (workingArea.Width + (-left) * 2)
                    && height == (workingArea.Height + (-top) * 2))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 重写该方法解决在VS设计器中，每次保存一个新的尺寸，再打开尺寸会变大的问题
        /// </summary>
        protected override void SetClientSizeCore(int x, int y)
        {
            _clientSizeSet = false;
            Type typeControl = typeof(Control);
            Type typeForm = typeof(Form);
            FieldInfo fiWidth = typeControl.GetField("clientWidth",
                BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo fiHeight = typeControl.GetField("clientHeight",
                BindingFlags.Instance | BindingFlags.NonPublic);
            FieldInfo fi1 = typeForm.GetField("FormStateSetClientSize",
                BindingFlags.NonPublic | BindingFlags.Static),
            fiFormState = typeForm.GetField("formState",
            BindingFlags.NonPublic | BindingFlags.Instance);

            if (fiWidth != null && fiHeight != null &&
                fiFormState != null && fi1 != null)
            {
                _clientSizeSet = true;
                Size = new Size(x, y);
                fiWidth.SetValue(this, x);
                fiHeight.SetValue(this, y);
                BitVector32.Section bi1 = (BitVector32.Section)fi1.GetValue(this);
                BitVector32 state = (BitVector32)fiFormState.GetValue(this);
                state[bi1] = 1;
                fiFormState.SetValue(this, state);
                OnClientSizeChanged(EventArgs.Empty);
                state[bi1] = 0;
                fiFormState.SetValue(this, state);
            }
            else
            {
                base.SetClientSizeCore(x, y);
            }
        }

        protected override Size SizeFromClientSize(Size clientSize)
        {
            return clientSize;
        }

        protected override Rectangle GetScaledBounds(
            Rectangle bounds, SizeF factor, BoundsSpecified specified)
        {
            Rectangle rect = base.GetScaledBounds(bounds, factor, specified);

            Size sz = SizeFromClientSize(Size.Empty);
            if (!GetStyle(ControlStyles.FixedWidth) &&
                ((specified & BoundsSpecified.Width) != BoundsSpecified.None))
            {
                int clientWidth = bounds.Width - sz.Width;
                rect.Width = ((int)Math.Round(
                    clientWidth * factor.Width)) + sz.Width;
            }
            if (!GetStyle(ControlStyles.FixedHeight) &&
                ((specified & BoundsSpecified.Height) != BoundsSpecified.None))
            {
                int clientHeight = bounds.Height - sz.Height;
                rect.Height = ((int)Math.Round(
                    clientHeight * factor.Height)) + sz.Height;
            }
            return rect;
        }

        protected override void ScaleControl(SizeF factor, BoundsSpecified specified)
        {
            Size minSize = MinimumSize;
            Size maxSize = MaximumSize;
            Size sz = SizeFromClientSize(Size.Empty);
            base.ScaleControl(factor, specified);
            if (minSize != Size.Empty)
            {
                minSize -= sz;
                minSize = new Size((int)Math.Round(
                    minSize.Width * factor.Width),
                    (int)Math.Round(minSize.Height * factor.Height)) + sz;
            }
            if (maxSize != Size.Empty)
            {
                maxSize -= sz;
                maxSize = new Size((int)Math.Round(
                    maxSize.Width * factor.Width),
                    (int)Math.Round(maxSize.Height * factor.Height)) + sz;
            }
            MinimumSize = minSize;
            MaximumSize = maxSize;
        }

        /// <summary>
        /// 重写该方法解决窗体每次还原都会变大的问题
        /// </summary> 
        protected override void SetBoundsCore(
            int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (_inWmWindowPosChanged != 0)
            {
                try
                {
                    Type type = typeof(Form);
                    FieldInfo fi1 = type.GetField("FormStateExWindowBoundsWidthIsClientSize",
                        BindingFlags.NonPublic | BindingFlags.Static),
                        fiFormState = type.GetField("formStateEx",
                        BindingFlags.NonPublic | BindingFlags.Instance),
                        fiBounds = type.GetField("restoredWindowBounds",
                        BindingFlags.NonPublic | BindingFlags.Instance);

                    if (fi1 != null && fiFormState != null && fiBounds != null)
                    {
                        Rectangle restoredWindowBounds = (Rectangle)fiBounds.GetValue(this);
                        BitVector32.Section bi1 = (BitVector32.Section)fi1.GetValue(this);
                        BitVector32 state = (BitVector32)fiFormState.GetValue(this);
                        if (state[bi1] == 1)
                        {
                            width = restoredWindowBounds.Width;
                            height = restoredWindowBounds.Height;
                        }
                    }
                }
                catch
                {
                }
            }

            base.SetBoundsCore(x, y, width, height, specified);
        }

        private void WmNcHitTest(ref Message m)
        {
            Point point = new Point(m.LParam.ToInt32());
            point = base.PointToClient(point);
            //是否有菜单
            if (IconRect.Contains(point) && ShowSystemMenu)
            {
                m.Result = new IntPtr(
                    HITTEST.HTSYSMENU);
                return;
            }

            if (_canResize)
            {
                if (point.X < 5 && point.Y < 5)
                {
                    m.Result = new IntPtr(
                        HITTEST.HTTOPLEFT);
                    return;
                }

                if (point.X > Width - 5 && point.Y < 5)
                {
                    m.Result = new IntPtr(
                        HITTEST.HTTOPRIGHT);
                    return;
                }

                if (point.X < 5 && point.Y > Height - 5)
                {
                    m.Result = new IntPtr(
                        HITTEST.HTBOTTOMLEFT);
                    return;
                }

                if (point.X > Width - 5 && point.Y > Height - 5)
                {
                    m.Result = new IntPtr(
                        HITTEST.HTBOTTOMRIGHT);
                    return;
                }

                if (point.Y < 3)
                {
                    m.Result = new IntPtr(
                        HITTEST.HTTOP);
                    return;
                }

                if (point.Y > Height - 3)
                {
                    m.Result = new IntPtr(
                        HITTEST.HTBOTTOM);
                    return;
                }

                if (point.X < 3)
                {
                    m.Result = new IntPtr(
                       HITTEST.HTLEFT);
                    return;
                }

                if (point.X > Width - 3)
                {
                    m.Result = new IntPtr(
                       HITTEST.HTRIGHT);
                    return;
                }
            }
            m.Result = new IntPtr(
                     HITTEST.HTCLIENT);
        }

        private void WmGetMinMaxInfo(ref Message m)
        {
            if (this.FormBorderStyle == FormBorderStyle.None)
            {
                base.WndProc(ref m);
            }
            else
            {
                MINMAXINFO minmax =
                    (MINMAXINFO)Marshal.PtrToStructure(
                    m.LParam, typeof(MINMAXINFO));

                if (MaximumSize != Size.Empty)
                {
                    minmax.maxTrackSize = MaximumSize;
                }
                else
                {
                    //主显屏幕
                    Rectangle zxrect = Screen.PrimaryScreen.WorkingArea;
                    //所在显屏幕
                    Rectangle rect = Screen.GetWorkingArea(this);
                    //Environment.OSVersion.Version.Major小于6则是win7 Vista以下系统
                    int h = this.FormBorderStyle == FormBorderStyle.None ? 0 : Environment.OSVersion.Version.Major < 6 ? 0 : -1;
                    //int h = 0;
                    //主显
                    if (zxrect.Equals(rect))
                    {
                        minmax.maxPosition = new Point(
                            rect.X,
                            rect.Y);
                    }
                    else
                    {
                        //附显
                        minmax.maxPosition = new Point(
                            0,
                            0);
                    }
                    minmax.maxTrackSize = new Size(
                        rect.Width,
                        rect.Height + h);
                }

                if (MinimumSize != Size.Empty)
                {
                    minmax.minTrackSize = MinimumSize;
                }
                else
                {
                    GetDefaultMinTrackSize();
                    minmax.minTrackSize = new Size(
                        AllButtonWidth(true) + ControlBoxOffset.X +
                        SystemInformation.SmallIconSize.Width +
                        BorderPadding.Left * 2 + 3,
                        CaptionHeight);
                }
                Marshal.StructureToPtr(minmax, m.LParam, false);
            }
        }

        private void WmNcActive(ref Message m)
        {
            if (m.WParam.ToInt32() == 1)
            {
                _active = true;
            }
            else
            {
                _active = false;
            }
            m.Result = Result.TRUE;
            if (OneVisibles && Special)
            {
                base.Invalidate();
            }
        }
        #endregion

        #region 私有方法
        //减少闪烁
        private void SetStyles()
        {
            base.SetStyle(
              ControlStyles.UserPaint |
              ControlStyles.AllPaintingInWmPaint |
              ControlStyles.OptimizedDoubleBuffer |
              ControlStyles.ResizeRedraw |
              ControlStyles.DoubleBuffer, true);
            base.UpdateStyles();
            base.AutoScaleMode = AutoScaleMode.None;
        }

        //上一次操作记录
        private Rectangle UpRc = Rectangle.Empty;
        private int? UpRadius = null;
        private RoundStyle? UpRoundStyle = null;
        //窗体圆角
        private void SetReion()
        {
            //对比是否需要变动
            if (this.RealClientRect != UpRc || Radius != UpRadius || RoundStyle != UpRoundStyle)
            {
                if (base.Region != null)
                {
                    base.Region.Dispose();
                }
                if (Radius == 0 || RoundStyle == RoundStyle.None)
                {
                    this.Region = new Region(this.RealClientRect);
                }
                else
                {
                    SkinTools.CreateRegion(this, this.RealClientRect, Radius, RoundStyle);
                }
                //赋值操作记录
                UpRc = this.RealClientRect;
                UpRadius = Radius;
                UpRoundStyle = RoundStyle;
            }
        }

        //初始化
        private void Init()
        {
            _toolTip = new ToolTip();
            base.FormBorderStyle = FormBorderStyle.Sizable;
            base.BackgroundImageLayout = ImageLayout.None;
            Renderer.InitSkinForm(this);
            base.Padding = DefaultPadding;
        }

        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x00020000;
                CreateParams cParms = base.CreateParams;
                cParms.Style = cParms.Style | WS_MINIMIZEBOX;   // 允许最小化操作
                return cParms;
            }
        }
        #endregion
    }
}
