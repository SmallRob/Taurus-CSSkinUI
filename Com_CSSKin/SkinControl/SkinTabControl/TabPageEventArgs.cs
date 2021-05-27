
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    //自定义事件参数类
    public class TabPageEventArgs
    {
        private TabPage colseTabPage;
        /// <summary>
        /// 关闭的容器
        /// </summary>
        public TabPage ColseTabPage {
            get { return colseTabPage; }
        }

        /// <summary>
        /// 悬浮事件
        /// </summary>
        /// <param name="selectsubitem">选中的好友</param>
        public TabPageEventArgs(TabPage ColseTabPage) {
            this.colseTabPage = ColseTabPage;
        }
    }
}
