
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Com_CSSkin.SkinControl
{
    //自定义列表项集合
    public class ChatListItemCollection : IList, ICollection, IEnumerable
    {
        private int count;      //元素个数
        public int Count { get { return count; } }
        private ChatListItem[] m_arrItem;
        private ChatListBox owner;  //所属的控件

        public ChatListItemCollection(ChatListBox owner) { this.owner = owner; }
        //确认存储空间
        private void EnsureSpace(int elements)
        {
            if (m_arrItem == null)
                m_arrItem = new ChatListItem[Math.Max(elements, 4)];
            else if (this.count + elements > m_arrItem.Length)
            {
                ChatListItem[] arrTemp = new ChatListItem[Math.Max(this.count + elements, m_arrItem.Length * 2)];
                m_arrItem.CopyTo(arrTemp, 0);
                m_arrItem = arrTemp;
            }
        }
        /// <summary>
        /// 获取列表项所在的索引位置
        /// </summary>
        /// <param name="item">要获取的列表项</param>
        /// <returns>索引位置</returns>
        public int IndexOf(ChatListItem item)
        {
            return Array.IndexOf<ChatListItem>(m_arrItem, item);
        }
        /// <summary>
        /// 添加一个列表项
        /// </summary>
        /// <param name="item">要添加的列表项</param>
        public void Add(ChatListItem item)
        {
            if (item == null)       //空引用不添加
                throw new ArgumentNullException("Item cannot be null");
            this.EnsureSpace(1);
            if (-1 == this.IndexOf(item))
            {         //进制添加重复对象
                item.OwnerChatListBox = this.owner;
                m_arrItem[this.count++] = item;
                this.owner.Invalidate();            //添加进去是 进行重绘
            }
        }
        /// <summary>
        /// 添加一个列表项的数组
        /// </summary>
        /// <param name="items">要添加的列表项的数组</param>
        public void AddRange(ChatListItem[] items)
        {
            if (items == null)
                throw new ArgumentNullException("Items cannot be null");
            this.EnsureSpace(items.Length);
            try
            {
                foreach (ChatListItem item in items)
                {
                    if (item == null)
                        throw new ArgumentNullException("Item cannot be null");
                    if (-1 == this.IndexOf(item))
                    {     //重复数据不添加
                        item.OwnerChatListBox = this.owner;
                        m_arrItem[this.count++] = item;
                    }
                }
            }
            finally
            {
                this.owner.Invalidate();
            }
        }
        /// <summary>
        /// 移除一个列表项
        /// </summary>
        /// <param name="item">要移除的列表项</param>
        public void Remove(ChatListItem item)
        {
            int index = this.IndexOf(item);
            if (-1 != index)        //如果存在元素 那么根据索引移除
                this.RemoveAt(index);
        }
        /// <summary>
        /// 根据索引位置删除一个列表项
        /// </summary>
        /// <param name="index">索引位置</param>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= this.count)
                throw new IndexOutOfRangeException("Index was outside the bounds of the array");
            this.count--;
            for (int i = index, Len = this.count; i < Len; i++)
                m_arrItem[i] = m_arrItem[i + 1];
            this.owner.Invalidate();
        }
        /// <summary>
        /// 清空所有列表项
        /// </summary>
        public void Clear()
        {
            this.count = 0;
            m_arrItem = null;
            this.owner.Invalidate();
        }
        /// <summary>
        /// 根据索引位置插入一个列表项
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <param name="item">要插入的列表项</param>
        public void Insert(int index, ChatListItem item)
        {
            if (index < 0 || index >= this.count)
                throw new IndexOutOfRangeException("Index was outside the bounds of the array");
            if (item == null)
                throw new ArgumentNullException("Item cannot be null");
            this.EnsureSpace(1);
            for (int i = this.count; i > index; i--)
                m_arrItem[i] = m_arrItem[i - 1];
            item.OwnerChatListBox = this.owner;
            m_arrItem[index] = item;
            this.count++;
            this.owner.Invalidate();
        }
        /// <summary>
        /// 判断一个列表项是否在集合内
        /// </summary>
        /// <param name="item">要判断的列表项</param>
        /// <returns>是否在列表项</returns>
        public bool Contains(ChatListItem item)
        {
            return this.IndexOf(item) != -1;
        }
        /// <summary>
        /// 将列表项的集合拷贝至一个数组
        /// </summary>
        /// <param name="array">目标数组</param>
        /// <param name="index">拷贝的索引位置</param>
        public void CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array cannot be null");
            m_arrItem.CopyTo(array, index);
        }
        /// <summary>
        /// 根据索引获取一个列表项
        /// </summary>
        /// <param name="index">索引位置</param>
        /// <returns>列表项</returns>
        public ChatListItem this[int index]
        {
            get
            {
                if (index < 0 || index >= this.count)
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                return m_arrItem[index];
            }
            set
            {
                if (index < 0 || index >= this.count)
                    throw new IndexOutOfRangeException("Index was outside the bounds of the array");
                m_arrItem[index] = value;
            }
        }
        //实现接口
        int IList.Add(object value)
        {
            if (!(value is ChatListItem))
                throw new ArgumentException("Value cannot convert to ListItem");
            this.Add((ChatListItem)value);
            return this.IndexOf((ChatListItem)value);
        }

        void IList.Clear()
        {
            this.Clear();
        }

        bool IList.Contains(object value)
        {
            if (!(value is ChatListItem))
                throw new ArgumentException("Value cannot convert to ListItem");
            return this.Contains((ChatListItem)value);
        }

        int IList.IndexOf(object value)
        {
            if (!(value is ChatListItem))
                throw new ArgumentException("Value cannot convert to ListItem");
            return this.IndexOf((ChatListItem)value);
        }

        void IList.Insert(int index, object value)
        {
            if (!(value is ChatListItem))
                throw new ArgumentException("Value cannot convert to ListItem");
            this.Insert(index, (ChatListItem)value);
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return false; }
        }

        void IList.Remove(object value)
        {
            if (!(value is ChatListItem))
                throw new ArgumentException("Value cannot convert to ListItem");
            this.Remove((ChatListItem)value);
        }

        void IList.RemoveAt(int index)
        {
            this.RemoveAt(index);
        }

        object IList.this[int index]
        {
            get { return this[index]; }
            set
            {
                if (!(value is ChatListItem))
                    throw new ArgumentException("Value cannot convert to ListItem");
                this[index] = (ChatListItem)value;
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo(array, index);
        }

        int ICollection.Count
        {
            get { return this.count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return true; }
        }

        object ICollection.SyncRoot
        {
            get { return this; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            for (int i = 0, Len = this.count; i < Len; i++)
                yield return m_arrItem[i];
        }
    }
}
