
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Com_CSSkin
{
    public class CmSysButton : IDisposable
    {
        //无参构造函数
        public CmSysButton() { }

        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns>深度克隆的自定义系统按钮</returns>
        public CmSysButton Clone() {
            CmSysButton SysButton = new CmSysButton();
            SysButton.Bounds = this.Bounds;
            SysButton.Location = this.Location;
            SysButton.size = this.Size;
            SysButton.ToolTip = this.ToolTip; ;
            SysButton.SysButtonNorml = this.SysButtonNorml;
            SysButton.SysButtonMouse = this.SysButtonMouse;
            SysButton.SysButtonDown = this.SysButtonDown;
            SysButton.OwnerForm = this.OwnerForm;
            SysButton.Name = this.Name;
            return SysButton;
        }

        private string name;
        /// <summary>
        /// 与对象关联的用户定义数据
        /// </summary>
        public string Name {
            get { return name; }
            set { name = value; }
        }

        private ControlBoxState boxState;
        /// <summary>
        /// 按钮的状态
        /// </summary>
        [Browsable(false)]
        public ControlBoxState BoxState {
            get { return boxState; }
            set {
                if (boxState != value) {
                    boxState = value;
                    if (OwnerForm != null) {
                        OwnerForm.Invalidate(Bounds);
                    }
                }
            }
        }

        private Rectangle bounds;
        /// <summary>
        /// 获取或设置自定义系统按钮的显示区域
        /// </summary>
        [Browsable(false)]
        public Rectangle Bounds {
            get {
                if (bounds == Rectangle.Empty) {
                    bounds = new Rectangle();
                }
                bounds.Location = Location;
                bounds.Size = Size;
                return bounds;
            }
            set {
                bounds = value;
                Location = bounds.Location;
                Size = bounds.Size;
            }
        }

        private Point location = new Point(0, 0);
        /// <summary>
        /// 按钮的位置
        /// </summary>
        [Browsable(false)]
        [Category("按钮的位置")]
        public Point Location {
            get { return location; }
            set {
                if (location != value) {
                    location = value;
                }
            }
        }

        private Size size = new Size(28, 20);
        /// <summary>
        /// 按钮的大小
        /// </summary>
        [DefaultValue(typeof(Size), "28, 20")]
        [Description("设置或获取自定义系统按钮的大小")]
        [Category("按钮大小")]
        public Size Size {
            get { return size; }
            set {
                if (size != value) {
                    size = value;
                }
            }
        }

        private string toolTip;
        /// <summary>
        /// 自定义系统按钮悬浮提示
        /// </summary>
        [Category("悬浮提示")]
        [Description("自定义系统按钮悬浮提示")]
        public string ToolTip {
            get { return toolTip; }
            set {
                if (toolTip != value) {
                    toolTip = value;
                }
            }
        }

        private bool visibale = true;
        /// <summary>
        /// 自定义系统按钮是否显示
        /// </summary>
        [Category("是否显示")]
        [DefaultValue(typeof(bool), "true")]
        [Description("自定义系统按钮是否显示")]
        public bool Visibale {
            get { return visibale; }
            set {
                if (visibale != value) {
                    visibale = value;
                }
            }
        }

        private Image sysButtonMouse;
        /// <summary>
        /// 自定义系统按钮悬浮时
        /// </summary>
        [Category("按钮图像")]
        [Description("自定义系统按钮悬浮时")]
        public Image SysButtonMouse {
            get { return sysButtonMouse; }
            set {
                if (sysButtonMouse != value) {
                    sysButtonMouse = value;
                }
            }
        }

        private Image sysButtonDown;
        /// <summary>
        /// 自定义系统按钮点击时
        /// </summary>
        [Category("按钮图像")]
        [Description("自定义系统按钮点击时")]
        public Image SysButtonDown {
            get { return sysButtonDown; }
            set {
                if (sysButtonDown != value) {
                    sysButtonDown = value;
                }
            }
        }

        private Image sysButtonNorml;
        /// <summary>
        /// 自定义系统按钮初始时
        /// </summary>
        [Category("按钮图像")]
        [Description("自定义系统按钮初始时")]
        public Image SysButtonNorml {
            get { return sysButtonNorml; }
            set {
                if (sysButtonNorml != value) {
                    sysButtonNorml = value;
                }
            }
        }

        private CSSkinMain ownerForm;
        /// <summary>
        /// 获取自定义系统按钮所在的窗体
        /// </summary>
        [Browsable(false)]
        public CSSkinMain OwnerForm {
            get { return ownerForm; }
            set { ownerForm = value; }
        }

        #region 资源释放
        //是否回收完毕
        bool _disposed;
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~CmSysButton() {
            Dispose(false);
        }

        //这里的参数表示示是否需要释放那些实现IDisposable接口的托管对象
        protected virtual void Dispose(bool disposing) {
            if (_disposed) return; //如果已经被回收，就中断执行
            if (disposing) {
                //TODO:释放那些实现IDisposable接口的托管对象
                ToolTip = null;
                SysButtonNorml = null;
                SysButtonMouse = null;
                SysButtonDown = null;
                OwnerForm = null;
                Name = null;
            }
            //TODO:释放非托管资源，设置对象为null
            _disposed = true;
        }
        #endregion
    }
}
