
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace Com_CSSkin.SkinControl
{
    [ListBindable(false)]
    public class SkinListBoxItemCollection 
        : IList,ICollection,IEnumerable
    {
        #region 变量

        private SkinListBox _owner;

        #endregion

        #region 无参构造
        public SkinListBoxItemCollection(SkinListBox owner)
        {
            _owner = owner;
        }
        #endregion

        #region 属性

        internal SkinListBox Owner
        {
            get { return _owner; }
        }

        public SkinListBoxItem this[int index]
        {
            get { return Owner.OldItems[index] as SkinListBoxItem; }
            set { Owner.OldItems[index] = value; }
        }

        public int Count
        {
            get { return Owner.OldItems.Count; }
        }

        public bool IsReadOnly 
        {
            get { return Owner.OldItems.IsReadOnly; }
        }

        #endregion

        #region 操作方法

        public int Add(SkinListBoxItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            return Owner.OldItems.Add(item);
        }

        public void AddRange(SkinListBoxItemCollection value)
        {
            foreach (SkinListBoxItem item in value)
            {
                Add(item);
            }
        }

        public void AddRange(SkinListBoxItem[] items)
        {
            Owner.OldItems.AddRange(items);
        }

        public void Clear()
        {
            Owner.OldItems.Clear();
        }

        public bool Contains(SkinListBoxItem item)
        {
            return Owner.OldItems.Contains(item);
        }

        public void CopyTo(
            SkinListBoxItem[] destination, 
            int arrayIndex)
        {
            Owner.OldItems.CopyTo(destination, arrayIndex);
        }

        public int IndexOf(SkinListBoxItem item)
        {
            return Owner.OldItems.IndexOf(item);
        }

        public void Insert(int index, SkinListBoxItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            Owner.OldItems.Insert(index, item);
        }

        public void Remove(SkinListBoxItem item)
        {
            Owner.OldItems.Remove(item);
        }

        public void RemoveAt(int index)
        {
            Owner.OldItems.RemoveAt(index);
        }

        public IEnumerator GetEnumerator()
        {
            return Owner.OldItems.GetEnumerator();
        }

        #endregion

        #region IList 成员

        int IList.Add(object value)
        {
            if (!(value is SkinListBoxItem))
            {
                throw new ArgumentException();
            }
            return Add(value as SkinListBoxItem);
        }

        void IList.Clear()
        {
            Clear();
        }

        bool IList.Contains(object value)
        {
            return Contains(value as SkinListBoxItem);
        }

        int IList.IndexOf(object value)
        {
            return IndexOf(value as SkinListBoxItem);
        }

        void IList.Insert(int index, object value)
        {
            if (!(value is SkinListBoxItem))
            {
                throw new ArgumentException();
            }
            Insert(index, value as SkinListBoxItem);
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool IList.IsReadOnly
        {
            get { return IsReadOnly; }
        }

        void IList.Remove(object value)
        {
            Remove(value as SkinListBoxItem);
        }

        void IList.RemoveAt(int index)
        {
            RemoveAt(index);
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                if (!(value is SkinListBoxItem))
                {
                    throw new ArgumentException();
                }
                this[index] = value as SkinListBoxItem;
            }
        }

        #endregion

        #region ICollection 成员

        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo((SkinListBoxItem[])array, index);
        }

        int ICollection.Count
        {
            get { return Count; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot
        {
            get { return this; }
        }

        #endregion

        #region IEnumerable 成员

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
