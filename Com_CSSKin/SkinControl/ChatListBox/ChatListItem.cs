
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Drawing;
using System.ComponentModel;

namespace Com_CSSkin.SkinControl
{
    //TypeConverter未解决
    //[DefaultProperty("Text"),TypeConverter(typeof(ChatListItemConverter))]
    public class ChatListItem : IDisposable
    {
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns>深度克隆的ChatListItem</returns>
        public ChatListItem Clone() {
            ChatListItem ListItem = new ChatListItem();
            ListItem.Bounds = this.Bounds;
            ListItem.IsOpen = this.IsOpen;
            ListItem.IsTwinkleHide = this.IsTwinkleHide;
            ListItem.OwnerChatListBox = this.OwnerChatListBox;
            ListItem.SubItems = this.SubItems;
            ListItem.Text = this.text;
            ListItem.TwinkleSubItemNumber = this.TwinkleSubItemNumber;
            return ListItem;
        }
        private string text = "Item";
        /// <summary>
        /// 获取或者设置列表项的显示文本
        /// </summary>
        //[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string Text {
            get { return text; }
            set {
                text = value;
                if (this.ownerChatListBox != null)
                    this.ownerChatListBox.Invalidate(this.bounds);
            }
        }

        private object tag;
        /// <summary>
        /// 自定义数据
        /// </summary>
        public object Tag {
            get { return tag; }
            set { tag = value; }
        }

        private bool isOpen;
        /// <summary>
        /// 获取或者设置列表项是否展开
        /// </summary>
        [DefaultValue(false)]
        public bool IsOpen {
            get { return isOpen; }
            set {
                isOpen = value;
                if (this.ownerChatListBox != null)
                    this.ownerChatListBox.Invalidate();
            }
        }

        private int twinkleSubItemNumber;
        /// <summary>
        /// 当前列表项下面闪烁图标的个数
        /// </summary>
        [Browsable(false)]
        public int TwinkleSubItemNumber {
            get { return twinkleSubItemNumber; }
            set { twinkleSubItemNumber = value; }
        }

        private bool isTwinkleHide;
        public bool IsTwinkleHide {
            get { return isTwinkleHide; }
            set { isTwinkleHide = value; }
        }

        private Rectangle bounds;
        /// <summary>
        /// 获取列表项的显示区域
        /// </summary>
        [Browsable(false)]
        public Rectangle Bounds {
            get { return bounds; }
            set { bounds = value; }
        }

        private ChatListBox ownerChatListBox;
        /// <summary>
        /// 获取列表项所在的控件
        /// </summary>
        [Browsable(false)]
        public ChatListBox OwnerChatListBox {
            get { return ownerChatListBox; }
            set { ownerChatListBox = value; }
        }

        private ChatListSubItemCollection subItems;
        /// <summary>
        /// 获取当前列表项所有子项的集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ChatListSubItemCollection SubItems {
            get {
                if (subItems == null)
                    subItems = new ChatListSubItemCollection(this);
                return subItems;
            }
            set {
                if (subItems != value) {
                    subItems = value;
                }
            }
        }

        public ChatListItem() { if (this.text == null) this.text = string.Empty; }
        public ChatListItem(string text) { this.text = text; }
        public ChatListItem(string text, bool bOpen) {
            this.text = text;
            this.isOpen = bOpen;
        }
        public ChatListItem(ChatListSubItem[] subItems) {
            this.subItems.AddRange(subItems);
        }
        public ChatListItem(string text, ChatListSubItem[] subItems) {
            this.text = text;
            this.subItems.AddRange(subItems);
        }
        public ChatListItem(string text, bool bOpen, ChatListSubItem[] subItems) {
            this.text = text;
            this.isOpen = bOpen;
            this.subItems.AddRange(subItems);
        }
        //自定义列表子项的集合 注释同 自定义列表项的集合
        public class ChatListSubItemCollection : IList, ICollection, IEnumerable
        {
            private int count;
            public int Count { get { return count; } }
            private ChatListSubItem[] m_arrSubItems;
            private ChatListItem owner;

            public ChatListSubItemCollection(ChatListItem owner) { this.owner = owner; }
            /// <summary>
            /// 对列表进行排序
            /// </summary>
            public void Sort() {
                Array.Sort<ChatListSubItem>(m_arrSubItems, 0, this.count, null);
                if (this.owner.ownerChatListBox != null)
                    this.owner.ownerChatListBox.Invalidate(this.owner.bounds);
            }
            /// <summary>
            /// 获取在线人数
            /// </summary>
            /// <returns>在线人数</returns>
            public int GetOnLineNumber() {
                int num = 0;
                for (int i = 0, len = this.count; i < len; i++) {
                    if (m_arrSubItems[i].Status != ChatListSubItem.UserStatus.OffLine)
                        num++;
                }
                return num;
            }
            //确认存储空间
            private void EnsureSpace(int elements) {
                if (m_arrSubItems == null)
                    m_arrSubItems = new ChatListSubItem[Math.Max(elements, 4)];
                else if (elements + this.count > m_arrSubItems.Length) {
                    ChatListSubItem[] arrTemp = new ChatListSubItem[Math.Max(m_arrSubItems.Length * 2, elements + this.count)];
                    m_arrSubItems.CopyTo(arrTemp, 0);
                    m_arrSubItems = arrTemp;
                }
            }
            /// <summary>
            /// 获取索引位置
            /// </summary>
            /// <param name="subItem">要获取索引的子项</param>
            /// <returns>索引</returns>
            public int IndexOf(ChatListSubItem subItem) {
                return Array.IndexOf<ChatListSubItem>(m_arrSubItems, subItem);
            }
            /// <summary>
            /// 添加一个子项
            /// </summary>
            /// <param name="subItem">要添加的子项</param>
            public void Add(ChatListSubItem subItem) {
                if (subItem == null)
                    throw new ArgumentNullException("SubItem cannot be null");
                this.EnsureSpace(1);
                if (-1 == IndexOf(subItem)) {
                    subItem.OwnerListItem = owner;
                    m_arrSubItems[this.count++] = subItem;
                    if (this.owner.OwnerChatListBox != null)
                        this.owner.OwnerChatListBox.Invalidate();
                }
            }
            /// <summary>
            /// 添加一组子项
            /// </summary>
            /// <param name="subItems">要添加子项的数组</param>
            public void AddRange(ChatListSubItem[] subItems) {
                if (subItems == null)
                    throw new ArgumentNullException("SubItems cannot be null");
                this.EnsureSpace(subItems.Length);
                try {
                    foreach (ChatListSubItem subItem in subItems) {
                        if (subItem == null)
                            throw new ArgumentNullException("SubItem cannot be null");
                        if (-1 == this.IndexOf(subItem)) {
                            subItem.OwnerListItem = this.owner;
                            m_arrSubItems[this.count++] = subItem;
                        }
                    }
                } finally {
                    if (this.owner.OwnerChatListBox != null)
                        this.owner.OwnerChatListBox.Invalidate();
                }
            }
            /// <summary>
            /// 根据在线状态添加一个子项
            /// </summary>
            /// <param name="subItem">要添加的子项</param>
            public void AddAccordingToStatus(ChatListSubItem subItem) {
                if (subItem.Status == ChatListSubItem.UserStatus.OffLine) {
                    this.Add(subItem);
                    return;
                }
                for (int i = 0, len = this.count; i < len; i++) {
                    if (subItem.Status <= m_arrSubItems[i].Status) {
                        this.Insert(i, subItem);
                        return;
                    }
                }
                this.Add(subItem);
            }
            /// <summary>
            /// 移除一个子项
            /// </summary>
            /// <param name="subItem">要移除的子项</param>
            public void Remove(ChatListSubItem subItem) {
                int index = this.IndexOf(subItem);
                if (-1 != index)
                    this.RemoveAt(index);
            }
            /// <summary>
            /// 根据索引移除一个子项
            /// </summary>
            /// <param name="index">要移除子项的索引</param>
            public void RemoveAt(int index) {
                if (index < 0 || index > this.count) {
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                }
                this.count--;
                if (index < this.count) {
                    //先清空闪烁属性
                    if (m_arrSubItems[index].IsTwinkle && m_arrSubItems[index].OwnerListItem.TwinkleSubItemNumber > 0) {
                        m_arrSubItems[index].OwnerListItem.TwinkleSubItemNumber -= 1;
                    }
                    Array.Copy(this.m_arrSubItems, index + 1, this.m_arrSubItems, index, this.count - index);
                }
                m_arrSubItems[this.count] = null;
                if (this.owner.OwnerChatListBox != null) {
                    this.owner.OwnerChatListBox.Invalidate();
                }

            }

            /// <summary>
            /// 清空所有子项*有问题
            /// </summary>
            public void Clear() {
                this.count = 0;
                m_arrSubItems = null;
                //if (this.count > 0) {
                //    Array.Clear(this.m_arrSubItems, 0, this.count);
                //    this.count = 0;
                //}
                if (this.owner.OwnerChatListBox != null)
                    this.owner.OwnerChatListBox.Invalidate();
            }

            /// <summary>
            /// 根据索引插入一个子项
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <param name="subItem">要插入的子项</param>
            public void Insert(int index, ChatListSubItem subItem) {
                if (index < 0 || index > this.count)
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                if (subItem == null)
                    throw new ArgumentNullException("SubItem cannot be null");
                this.EnsureSpace(1);
                for (int i = this.count; i > index; i--)
                    m_arrSubItems[i] = m_arrSubItems[i - 1];
                subItem.OwnerListItem = this.owner;
                m_arrSubItems[index] = subItem;
                this.count++;
                if (this.owner.OwnerChatListBox != null)
                    this.owner.OwnerChatListBox.Invalidate();
            }
            /// <summary>
            /// 将集合类的子项拷贝至数组
            /// </summary>
            /// <param name="array">要拷贝的数组</param>
            /// <param name="index">拷贝的索引位置</param>
            public void CopyTo(Array array, int index) {
                if (array == null)
                    throw new ArgumentNullException("Array cannot be null");
                m_arrSubItems.CopyTo(array, index);
            }
            /// <summary>
            /// 判断子项是否在集合内
            /// </summary>
            /// <param name="subItem">要判断的子项</param>
            /// <returns>是否在集合内</returns>
            public bool Contains(ChatListSubItem subItem) {
                return this.IndexOf(subItem) != -1;
            }
            /// <summary>
            /// 根据索引获取一个列表子项
            /// </summary>
            /// <param name="index">索引位置</param>
            /// <returns>列表子项</returns>
            public ChatListSubItem this[int index] {
                get {
                    if (index < 0 || index > this.count) {
                        throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                    }
                    return m_arrSubItems[index];
                }
                set {
                    if (index < 0 || index > this.count)
                        throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                    m_arrSubItems[index] = value;
                    if (this.owner.OwnerChatListBox != null)
                        this.owner.OwnerChatListBox.Invalidate(m_arrSubItems[index].Bounds);
                }
            }
            //接口实现
            int IList.Add(object value) {
                if (!(value is ChatListSubItem))
                    throw new ArgumentException("Value cannot convert to ListSubItem");
                this.Add((ChatListSubItem)value);
                return this.IndexOf((ChatListSubItem)value);
            }

            void IList.Clear() {
                this.Clear();
            }

            bool IList.Contains(object value) {
                if (!(value is ChatListSubItem))
                    throw new ArgumentException("Value cannot convert to ListSubItem");
                return this.Contains((ChatListSubItem)value);
            }

            int IList.IndexOf(object value) {
                if (!(value is ChatListSubItem))
                    throw new ArgumentException("Value cannot convert to ListSubItem");
                return this.IndexOf((ChatListSubItem)value);
            }

            void IList.Insert(int index, object value) {
                if (!(value is ChatListSubItem))
                    throw new ArgumentException("Value cannot convert to ListSubItem");
                this.Insert(index, (ChatListSubItem)value);
            }

            bool IList.IsFixedSize {
                get { return false; }
            }

            bool IList.IsReadOnly {
                get { return false; }
            }

            void IList.Remove(object value) {
                if (!(value is ChatListSubItem))
                    throw new ArgumentException("Value cannot convert to ListSubItem");
                this.Remove((ChatListSubItem)value);
            }

            void IList.RemoveAt(int index) {
                this.RemoveAt(index);
            }

            object IList.this[int index] {
                get {
                    return this[index];
                }
                set {
                    if (!(value is ChatListSubItem))
                        throw new ArgumentException("Value cannot convert to ListSubItem");
                    this[index] = (ChatListSubItem)value;
                }
            }

            void ICollection.CopyTo(Array array, int index) {
                this.CopyTo(array, index);
            }

            int ICollection.Count {
                get { return this.count; }
            }

            bool ICollection.IsSynchronized {
                get { return true; }
            }

            object ICollection.SyncRoot {
                get { return this; }
            }

            IEnumerator IEnumerable.GetEnumerator() {
                for (int i = 0, Len = this.count; i < Len; i++)
                    yield return m_arrSubItems[i];
            }
        }

        #region 资源释放
        //是否回收完毕
        bool _disposed;
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~ChatListItem() {
            Dispose(false);
        }

        //这里的参数表示示是否需要释放那些实现IDisposable接口的托管对象
        protected virtual void Dispose(bool disposing) {
            if (_disposed) return; //如果已经被回收，就中断执行
            if (disposing) {
                //TODO:释放那些实现IDisposable接口的托管对象
                if (subItems.Count > 0) {
                    foreach (ChatListSubItem item in subItems)
                        item.Dispose();
                }
            }
            //TODO:释放非托管资源，设置对象为null

            _disposed = true;
        }
        #endregion
    }
}
