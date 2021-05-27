
using System;
using System.ComponentModel;
using System.Collections;

namespace Com_CSSkin.SkinControl
{
    /// <summary>
    /// 带事件的实现IList接口的集合
    /// </summary>
    public class SkinEventsCollection : CollectionBase
    {
        #region constructors

        public SkinEventsCollection()
        {

        }

        #endregion

        #region private var, proterties

        private EventHandlerList _events;
        protected EventHandlerList Events
        {
            get
            {
                if (_events == null)
                    _events = new EventHandlerList();
                return _events;
            }
        }

        #endregion

        #region Events
        
        private static readonly object Event_CollectionChange = new object();

        public event GMCollectionChangeHandler CollectionChange
        {
            add { this.Events.AddHandler(Event_CollectionChange, value); }
            remove { this.Events.RemoveHandler(Event_CollectionChange, value); }
        }

        private void RaiseChangeEvent(GMCollectionChangeAction action,
            int index, object value, object oldValue, object newValue)
        {
            GMCollectionChangeHandler handler = (GMCollectionChangeHandler)this.Events[Event_CollectionChange];
            if (handler != null)
            {
                handler(new GMCollectionChangeArgs(action, index, value, oldValue, newValue));
            }
        }

        #endregion

        #region override onxx

        protected override void OnClear()
        {
            base.OnClear();
            RaiseChangeEvent(GMCollectionChangeAction.BeforeClear, 0, null, null, null);
        }

        protected override void OnClearComplete()
        {
            base.OnClearComplete();
            RaiseChangeEvent(GMCollectionChangeAction.AfterClear, 0, null, null, null);
        }

        protected override void OnInsert(int index, object value)
        {
            base.OnInsert(index, value);
            RaiseChangeEvent(GMCollectionChangeAction.BeforeInsert, index, value, null, null);
        }

        protected override void OnInsertComplete(int index, object value)
        {
            base.OnInsertComplete(index, value);
            RaiseChangeEvent(GMCollectionChangeAction.AfterInsert, index, value, null, null);
        }

        protected override void OnSet(int index, object oldValue, object newValue)
        {
            base.OnSet(index, oldValue, newValue);
            RaiseChangeEvent(GMCollectionChangeAction.BeforeSet, index, null, oldValue, newValue);
        }

        protected override void OnSetComplete(int index, object oldValue, object newValue)
        {
            base.OnSetComplete(index, oldValue, newValue);
            RaiseChangeEvent(GMCollectionChangeAction.AfterSet, index, null, oldValue, newValue);
        }

        protected override void OnRemove(int index, object value)
        {
            base.OnRemove(index, value);
            RaiseChangeEvent(GMCollectionChangeAction.BeforeRemove, index, value, null, null);
        }

        protected override void OnRemoveComplete(int index, object value)
        {
            base.OnRemoveComplete(index, value);
            RaiseChangeEvent(GMCollectionChangeAction.AfterRemove, index, value, null, null);
        }
        
        #endregion

        #region new public methods

        public bool Contains(object value)
        {
            return base.List.Contains(value);
        }

        public int IndexOf(object value)
        {
            return base.List.IndexOf(value);
        }

        public int Add(object value)
        {
            return base.List.Add(value);
        }

        public void Remove(object value)
        {
            base.List.Remove(value);            
        }

        public void Insert(int index, object value)
        {
            base.List.Insert(index, value);
        }

        public void Insert(object value)
        {
            this.Insert(base.Count, value);
        }

        #endregion

        #region new public properties

        public object this[int index]
        {
            get
            {
                if (index < 0 || index > base.Count - 1)
                    return null;

                return base.List[index];
            }
            set
            {
                base.List[index] = value;
            }
        }

        #endregion
    }

    #region delegate, eventargs class

    public class GMCollectionChangeArgs : EventArgs
    {
        private int _index;
        private object _value;
        private object _oldValue;
        private object _newValue;
        private GMCollectionChangeAction _action;

        public GMCollectionChangeArgs(GMCollectionChangeAction action, 
            int index, object value, object oldValue, object newValue)
            : base()
        {
            _index = index;
            _value = value;
            _oldValue = oldValue;
            _newValue = newValue;
            _action = action;
        }

        public GMCollectionChangeArgs(GMCollectionChangeAction action)
            : this(action, 0, null, null, null)
        {
        }

        public GMCollectionChangeArgs(GMCollectionChangeAction action, object value)
            : this(action, 0, value, null, null)
        {
        }

        public GMCollectionChangeArgs(GMCollectionChangeAction action, int index, object value)
            : this(action, index, value, null, null)
        {
        }

        public GMCollectionChangeArgs(GMCollectionChangeAction action, 
            int index, object oldValue, object newValue)
            : this(action, index, null, oldValue, newValue)
        {
        }

        public GMCollectionChangeAction Action
        {
            get { return _action; }
        }

        public int Index
        {
            get { return _index; }
        }

        public object Value
        {
            get { return _value; }
        }

        public object OldValue
        {
            get { return _oldValue; }
        }

        public object NewValue
        {
            get { return _newValue; }
        }
    }

    public delegate void GMCollectionChangeHandler(GMCollectionChangeArgs e);

    public enum GMCollectionChangeAction
    {
        BeforeClear,
        AfterClear,
        BeforeInsert,
        AfterInsert,
        BeforeSet,
        AfterSet,
        BeforeRemove,
        AfterRemove
    }

    #endregion
}
