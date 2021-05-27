
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using Com_CSSkin.SkinControl.Design;

namespace Com_CSSkin.SkinControl
{
    [Browsable(true)]
    [DesignTimeVisible(true)]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(Panel))]
    [DefaultProperty("Items"), DefaultEvent("ItemCaptionClick")]
    [Designer(typeof(PushPanelDesigner))]
    public class SkinPushPanel : BorderPanel, ISupportInitialize
    {
        #region Fields

        private PushPanelItemCollection _items;
        private CaptionImageStyle _imageStyle = CaptionImageStyle.Draw;
        private Image _expandImage;
        private Image _collapseImage;
        private ContentAlignment _captionTextAlign = ContentAlignment.MiddleLeft;
        private PushPanelItem _expandItem;
        private bool _initialize;

        private static readonly object EventItemCaptionClick = new object();

        #endregion

        #region Constructors

        public SkinPushPanel()
            : base()
        {
            _items = new PushPanelItemCollection(this);
        }

        #endregion

        #region Events

        public event PushPanelItemCaptionClickEventHandler ItemCaptionClick
        {
            add { base.Events.AddHandler(EventItemCaptionClick, value); }
            remove { base.Events.RemoveHandler(EventItemCaptionClick, value); }
        }

        #endregion

        #region Properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public PushPanelItemCollection Items
        {
            get { return _items; }
        }

        [DefaultValue(typeof(CaptionImageStyle), "2")]
        public CaptionImageStyle ImageStyle
        {
            get { return _imageStyle; }
            set
            {
                if (_imageStyle != value)
                {
                    _imageStyle = value;
                    base.Invalidate(true);
                }
            }
        }

        [DefaultValue(typeof(Image), "")]
        public Image ExpandImage
        {
            get { return _expandImage; }
            set
            {
                _expandImage = value;
                base.Invalidate(true);
            }
        }

        [DefaultValue(typeof(Image), "")]
        public Image CollapseImage
        {
            get { return _collapseImage; }
            set
            {
                _collapseImage = value;
                base.Invalidate(true);
            }
        }

        [DefaultValue(typeof(ContentAlignment), "16")]
        public ContentAlignment CaptionTextAlign
        {
            get { return _captionTextAlign; }
            set
            {
                if (_captionTextAlign != value)
                {
                    _captionTextAlign = value;
                    foreach (PushPanelItem item in _items)
                    {
                        item.TextAlign = value;
                    }
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new Control.ControlCollection Controls
        {
            get
            {
                return base.Controls;
            }
        }

        #endregion

        #region Public Methods

        public void ExpandItem(int index)
        {
            try
            {
                PushPanelItem item = _items[index];
                if (item != null)
                {
                    ExpandItem(item);
                }
            }
            catch
            {
            }
        }

        public void ExpandItem(string name)
        {
            try
            {
                PushPanelItem item = _items[name];
                if (item != null)
                {
                    ExpandItem(item);
                }
            }
            catch
            {
            }
        }
        
        #endregion

        #region ISupportInitialize ��Ա

        public void BeginInit()
        {
            _initialize = true;
        }

        public void EndInit()
        {
            if (_initialize)
            {
                if (_items.Count > 0)
                {
                    ExpandItem(0);
                }
                _initialize = false;
            }
        }

        #endregion

        #region Protected Methods

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            PushPanelItem item = e.Control as PushPanelItem;

            if (item != null)
            {
                item.Owner = null;
                item.CaptionMouseClick -= ItemCaptionMouseClick;
            }

            base.OnControlRemoved(e);

            ReLayout();
        }

        protected override void OnResize(EventArgs e)
        {
            ChangeItemHeight(_expandItem, true);
            ReLayout();
            base.OnResize(e);
        }

        #endregion

        #region Helper Methods

        internal protected virtual void OnItemCaptionClick(
            PushPanelItemCaptionClickEventArgs e)
        {
            if (e.Item.IsExpanded)
            {
                CollapseItem(e.Item);
            }
            else
            {
                ExpandItem(e.Item);
            }

            PushPanelItemCaptionClickEventHandler handler =
                base.Events[EventItemCaptionClick] as PushPanelItemCaptionClickEventHandler;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        internal void ExpandItem(PushPanelItem item)
        {
            if (item == null || _expandItem == item)
            {
                return;
            }

            if (_expandItem != null)
            {
                ChangeItemHeight(_expandItem, false);
            }

            ChangeItemHeight(item, true);

            ReLayout();
            if (_expandItem != null)
            {
                _expandItem.IsExpanded = false;
            }
            item.IsExpanded = true;
            _expandItem = item;
        }

        internal void CollapseItem(PushPanelItem item)
        {
            if (item == null || !item.IsExpanded)
            {
                return;
            }

            ChangeItemHeight(item, false);
            ReLayout();
            item.IsExpanded = false;
            _expandItem = null;
        }

        internal void ReLayout()
        {
            base.SuspendLayout();

            Rectangle rect = base.DisplayRectangle;
            int y = base.Padding.Top + rect.Y;
            foreach (PushPanelItem item in Items)
            {
                item.Location = new Point(
                    base.Padding.Left + rect.X,
                    y);
                item.Width = rect.Width - base.Padding.Horizontal;
                y += item.Height + 2;
            }

            base.ResumeLayout(true);
        }

        internal int CalcuExpandItemHeight()
        {
            int height = 0;
            foreach (PushPanelItem item in Items)
            {
                height += item.CaptionHeight + 2;
            }

            height = base.DisplayRectangle.Height - (height + base.Padding.Vertical) + 2;
            return height;
        }

        internal void ChangeItemHeight(PushPanelItem item, bool expand)
        {
            if (item == null)
            {
                return;
            }

            if (expand)
            {
                item.Height = item.CaptionHeight + CalcuExpandItemHeight();
            }
            else
            {
                if (item.IsExpanded)
                {
                    item.Height = item.CaptionHeight;
                }
            }
        }

        private void AddItem(PushPanelItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            item.Height = item.CaptionHeight;
            item.Owner = this;
            item.CaptionMouseClick += ItemCaptionMouseClick;

            base.Controls.Add(item);

            ReLayout();
        }

        private void ItemCaptionMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                OnItemCaptionClick(new PushPanelItemCaptionClickEventArgs(
                    (PushPanelItem)sender));
            }
        }

        #endregion

        #region PushPanelItemCollection

        public class PushPanelItemCollection : IList, ICollection, IEnumerable
        {
            #region Fields

            private SkinPushPanel _owner;

            #endregion

            #region Constructors

            public PushPanelItemCollection(SkinPushPanel owner)
            {
                _owner = owner;
            }

            #endregion

            #region Properties

            public SkinPushPanel Owner
            {
                get
                {
                    return _owner;
                }
            }

            public int Count
            {
                get { return _owner.Controls.Count; }
            }

            #endregion

            #region Public Methord

            public PushPanelItem this[int index]
            {
                get
                {
                    return _owner.Controls[index] as PushPanelItem;
                }
            }

            public PushPanelItem this[string name]
            {
                get
                {
                    return _owner.Controls[name] as PushPanelItem;
                }
            }

            public void Add(PushPanelItem item)
            {
                if (item == null)
                {
                    throw new ArgumentNullException("item");
                }
                _owner.AddItem(item);
            }

            public void AddRange(PushPanelItem[] values)
            {
                foreach (PushPanelItem item in values)
                {
                    Add(item);
                }
            }

            public void Clear()
            {
                _owner.Controls.Clear();
            }

            public bool Contains(PushPanelItem item)
            {
                return _owner.Controls.Contains(item);
            }

            public bool ContainsKey(string name)
            {
                return _owner.Controls.ContainsKey(name);
            }

            public IEnumerator GetEnumerator()
            {
                return _owner.Controls.GetEnumerator();
            }

            public int IndexOf(PushPanelItem item)
            {
                return _owner.Controls.IndexOf(item);
            }

            public int IndexOfKey(string name)
            {
                return _owner.Controls.IndexOfKey(name);
            }

            public void Remove(PushPanelItem item)
            {
                _owner.Controls.Remove(item);
            }

            public void RemoveAt(int index)
            {
                _owner.Controls.RemoveAt(index);
            }

            public void RemoveByKey(string name)
            {
                _owner.Controls.RemoveByKey(name);
            }

            #endregion

            #region IList ��Ա

            int IList.Add(object value)
            {
                PushPanelItem item = value as PushPanelItem;
                if (item != null)
                {
                    Add(item);
                    return IndexOf(item);
                }
                return -1;
            }

            void IList.Clear()
            {
                Clear();
            }

            bool IList.Contains(object value)
            {
                PushPanelItem item = value as PushPanelItem;
                if (value != null)
                {
                    return Contains(item);
                }
                return false;
            }

            int IList.IndexOf(object value)
            {
                PushPanelItem item = value as PushPanelItem;
                if (value != null)
                {
                    return IndexOf(item);
                }
                return -1;
            }

            void IList.Insert(int index, object value)
            {
                PushPanelItem item = value as PushPanelItem;
                if (value != null)
                {
                    ((IList)_owner.Controls).Insert(index,value);
                }
            }

            bool IList.IsFixedSize
            {
                get { return false; }
            }

            bool IList.IsReadOnly
            {
                get { return _owner.Controls.IsReadOnly; }
            }

            void IList.Remove(object value)
            {
                PushPanelItem item = value as PushPanelItem;
                if (value != null)
                {
                    Remove(item);
                }
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
                }
            }

            #endregion

            #region ICollection ��Ա

            void ICollection.CopyTo(Array array, int index)
            {
                _owner.Controls.CopyTo(array, index);
            }

            int ICollection.Count
            {
                get { return Count; }
            }

            bool ICollection.IsSynchronized
            {
                get { return ((ICollection)_owner.Controls).IsSynchronized; }
            }

            object ICollection.SyncRoot
            {
                get { return ((ICollection)_owner.Controls).SyncRoot; }
            }

            #endregion

            #region IEnumerable ��Ա

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            #endregion
        }

        #endregion
    }
}
