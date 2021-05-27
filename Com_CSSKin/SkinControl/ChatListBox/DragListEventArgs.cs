
using System;
using System.Collections.Generic;
using System.Text;

namespace Com_CSSkin.SkinControl
{
    //自定义事件参数类
    public class DragListEventArgs
    {
        private ChatListSubItem qsubitem;
        public ChatListSubItem QSubItem
        {
            get { return qsubitem; }
        }

        private ChatListSubItem hsubitem;
        public ChatListSubItem HSubItem
        {
            get { return hsubitem; }
        }

        /// <summary>
        /// 拖动事件
        /// </summary>
        /// <param name="QSubItem">拖动前好友</param>
        /// <param name="HSubItem">拖动后好友</param>
        public DragListEventArgs(ChatListSubItem QSubItem, ChatListSubItem HSubItem)
        {
            this.qsubitem = QSubItem;
            this.hsubitem = HSubItem;
        }
    }
}
