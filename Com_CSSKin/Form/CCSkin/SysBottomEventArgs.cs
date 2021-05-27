
using System;
using System.Collections.Generic;
using System.Text;

namespace Com_CSSkin.SkinControl
{
    /// <summary>
    /// 自定义按钮事件参数
    /// </summary>
    public class SysButtonEventArgs
    {
        /// <summary>
        /// 带参构造
        /// </summary>
        /// <param name="sysButton">所点击的自定义系统按钮</param>
        public SysButtonEventArgs(CmSysButton sysButton) 
        {
            this.SysButton = sysButton;
        }

        private CmSysButton sysButton;
        /// <summary>
        /// 所点击的自定义系统按钮
        /// </summary>
        public CmSysButton SysButton
        {
            get { return sysButton; }
            set { sysButton = value; }
        }
    }
}
