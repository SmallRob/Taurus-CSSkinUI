
using System;
using System.Collections.Generic;
using System.Text;

namespace Com_CSSkin.SkinControl
{
    //自定义事件参数类
    public class ChatListClickEventArgs
    {
        private ChatListSubItem selectSubItem;
        /// <summary>
        /// 选中的好友
        /// </summary>
        public ChatListSubItem SelectSubItem {
            get { return selectSubItem; }
        }

        /// <summary>
        /// 悬浮事件
        /// </summary>
        /// <param name="selectsubitem">选中的好友</param>
        public ChatListClickEventArgs(ChatListSubItem selectsubitem) {
            this.selectSubItem = selectsubitem;
        }
    }
}
