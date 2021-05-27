
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Com_CSSkin.Win32;
using System.Drawing.Design;

namespace Com_CSSkin.SkinControl
{
    /// <summary>
    /// 提供自定义全局热键支持
    /// </summary>
    [DefaultEvent("HotKeyTrigger"), DefaultProperty("Key")]
    public class SkinHotKey : Component
    {
        int id;
        /// <summary>
        /// 热键ID
        /// </summary>
        [Browsable(false)]
        [Category("Skin")]
        public int Id
        {
            get { return id; }
        }

        KeyModifiers keyModifier = KeyModifiers.None;
        /// <summary>
        /// 辅助按键
        /// </summary>
        [Category("Skin")]
        [Description("辅助按键，多辅助键写法  Alt, Ctrl")]
        public KeyModifiers KeyModifier
        {
            get { return keyModifier; }
            set
            {
                keyModifier = value;
                Activate();
            }
        }

        Keys key;
        /// <summary>
        /// 按键必须，必须填写按键，请勿添加修饰键，修饰键在辅助键 KeyModifier 属性那边设置
        /// </summary>
        [Category("Skin")]
        [Description("按键，必须填写按键，请勿添加修饰键，修饰键在辅助键 KeyModifier 属性那边设置")]
        public Keys Key
        {
            get { return key; }
            set
            {
                key = value;
                Activate();
            }
        }

        bool isRegistered = false;
        /// <summary>
        /// 是否已成功注册
        /// </summary>
        [Category("Skin")]
        [Browsable(false)]
        public bool IsRegistered
        {
            get { return isRegistered; }
        }

        bool enabled = false;
        /// <summary>
        /// 是否启用
        /// </summary>
        [Category("Skin")]
        [Description("是否启用")]
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (this.enabled != value)
                {
                    this.enabled = value;

                    Activate();

                }
            }
        }

        private IntPtr handle;
        /// <summary>
        /// 热键所注册的句柄
        /// </summary>
        [Browsable(false)]
        public IntPtr Handle
        {
            get { return handle; }
        }

        object tag;
        /// <summary>
        /// 与对象关联的用户定义数据
        /// </summary>
        [Category("Skin")]
        public object Tag
        {
            get { return tag; }
            set { tag = value; }
        }

        private HotKeyWindow hotKeyWindow;
        private GCHandle hotKeyRoot;

        /// <summary>
        /// 初始化热键
        /// </summary>
        /// <param name="keyModifier">辅助键</param>
        /// <param name="key">按键</param>
        public SkinHotKey(KeyModifiers keyModifier, Keys key)
            : this()
        {
            this.keyModifier = keyModifier;
            this.key = key;
        }

        /// <summary>
        /// 初始化热键
        /// </summary>
        public SkinHotKey()
        {
            Random r = new Random();
            id = r.Next(1, 1000);
        }

        /// <summary>
        /// 初始化热键
        /// </summary>             
        public SkinHotKey(IContainer container)
            : this()
        {
            container.Add(this);
        }

        /// <summary>
        /// 注册热键
        /// </summary>
        private void RegisterHotKey()
        {
            if (NativeMethods.RegisterHotKey(this.handle, id, keyModifier, key))
            {
                isRegistered = true;
            }
        }
        /// <summary>
        /// 卸载热键
        /// </summary>
        private void UnregisterHotKey()
        {
            if (NativeMethods.UnregisterHotKey(this.handle, id))
            {
                isRegistered = false;
            }
        }

        /// <summary>
        /// 启用
        /// </summary>
        private void Activate()
        {
            if (key != Keys.None)
            {
                if (!this.DesignMode)
                {
                    if (enabled)
                    {
                        if (hotKeyWindow == null)
                        {
                            hotKeyWindow = new HotKeyWindow(this);
                        }
                        if (isRegistered)//如果原先注册了就卸载
                        {
                            UnregisterHotKey();
                        }
                        if (!this.hotKeyRoot.IsAllocated)
                        {
                            this.hotKeyRoot = GCHandle.Alloc(this);
                            hotKeyWindow.CreateHandle();
                            this.handle = hotKeyWindow.Handle;
                        }
                        RegisterHotKey();
                    }
                    else
                    {
                        UnregisterHotKey();
                        if (this.hotKeyRoot.IsAllocated)
                        {
                            if (hotKeyWindow != null)
                            {
                                hotKeyWindow.DestroyHandle();
                            }
                            this.hotKeyRoot.Free();
                        }
                    }
                }

            }
        }


        protected override void Dispose(bool disposing)
        {
            UnregisterHotKey();
            if (disposing && hotKeyWindow != null && hotKeyWindow.Handle != IntPtr.Zero)
            {
                hotKeyWindow.DestroyHandle();
            }
            if (this.hotKeyRoot.IsAllocated)
            {
                this.hotKeyRoot.Free();
            }
            enabled = false;
            hotKeyWindow = null;
            base.Dispose(disposing);
        }

        protected void OnTriggerHotKey(HotKeyEventArgs e)
        {
            if (HotKeyTrigger != null)
            {
                HotKeyTrigger(this, e);
            }
        }

        /// <summary>
        /// 热键被按下的事件
        /// </summary>
        [Category("Skin")]
        [Description("热键被按下时发生")]
        public event EventHandler<HotKeyEventArgs> HotKeyTrigger;

        private class HotKeyWindow : NativeWindow
        {
            SkinHotKey owner;
            static HandleRef HWND_MESSAGE = new HandleRef(null, new IntPtr(-3));

            public HotKeyWindow(SkinHotKey owner)
            {
                this.owner = owner;
            }

            /// <summary>
            /// 创建句柄
            /// </summary>
            /// <returns></returns>
            public bool CreateHandle()
            {
                if (base.Handle == IntPtr.Zero)
                {
                    CreateParams cp = new CreateParams
                    {
                        Style = 0,
                        ExStyle = 0,
                        ClassStyle = 0,
                        Caption = base.GetType().Name
                    };
                    if (Environment.OSVersion.Platform == PlatformID.Win32NT)
                    {
                        cp.Parent = (IntPtr)HWND_MESSAGE;
                    }
                    this.CreateHandle(cp);
                }
                return (base.Handle != IntPtr.Zero);
            }

            protected override void WndProc(ref Message m)
            {
                switch (m.Msg)
                {
                    case 0x0312: //这个是window消息定义的注册的热键消息 
                        if (owner.Id != 0 && m.WParam.ToString().Equals(owner.Id.ToString()))
                        {
                            owner.OnTriggerHotKey(new HotKeyEventArgs(owner.KeyModifier, owner.Key));
                        }
                        break;
                }
                base.WndProc(ref m);
            }

        }
    }

    /// <summary>
    /// 热键事件数据类
    /// </summary>
    public class HotKeyEventArgs : EventArgs
    {

        KeyModifiers keyModifier = KeyModifiers.None;
        /// <summary>
        /// 辅助按键
        /// </summary>
        public KeyModifiers KeyModifier
        {
            get { return keyModifier; }
            set { keyModifier = value; }
        }

        Keys key;
        /// <summary>
        /// 按键
        /// </summary>
        public Keys Key
        {
            get { return key; }
            set { key = value; }
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="keyModifier"></param>
        /// <param name="key"></param>
        public HotKeyEventArgs(KeyModifiers keyModifier, Keys key)
        {
            this.keyModifier = keyModifier;
            this.key = key;
        }

    }
    /// <summary>
    /// 定义了辅助键的名称（将数字转变为字符以便于记忆，也可去除此枚举而直接使用数值）
    /// </summary>
    [Flags()]
    public enum KeyModifiers { None = 0, Alt = 1, Control = 2, Shift = 4, WindowKeys = 8 }
}
