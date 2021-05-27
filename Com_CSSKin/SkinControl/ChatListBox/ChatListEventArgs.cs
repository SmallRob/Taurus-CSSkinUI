
using System;
using System.Collections.Generic;
using System.Text;

namespace Com_CSSkin.SkinControl
{
    //自定义事件参数类
    public class ChatListEventArgs
    {
        private ChatListSubItem mouseOnSubItem;
        public ChatListSubItem MouseOnSubItem {
            get { return mouseOnSubItem; }
        }

        private ChatListSubItem selectSubItem;
        public ChatListSubItem SelectSubItem {
            get { return selectSubItem; }
        }

        /// <summary>
        /// 悬浮事件
        /// </summary>
        /// <param name="mouseonsubitem">鼠标上所悬浮的好友</param>
        /// <param name="selectsubitem">选中的好友</param>
        public ChatListEventArgs(ChatListSubItem mouseonsubitem, ChatListSubItem selectsubitem) {
            this.mouseOnSubItem = mouseonsubitem;
            this.selectSubItem = selectsubitem;
        }
    }
}
