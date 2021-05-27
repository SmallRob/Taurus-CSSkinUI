
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;
using System.Drawing.Drawing2D;
using Com_CSSkin.SkinClass;
using System.ComponentModel.Design;

namespace Com_CSSkin.SkinControl
{
    [ToolboxBitmap(typeof(ListBox))]
    [Designer(typeof(UC_SmartTagChatListBoxDesigner))]
    public partial class ChatListBox : Control
    {
        public ChatListBox() {
            InitializeComponent();
            //设置自定义控件Style
            this.SetStyle(ControlStyles.ResizeRedraw, true);//调整大小时重绘
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);// 双缓冲
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);// 禁止擦除背景.
            this.SetStyle(ControlStyles.UserPaint, true);//自行绘制
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.ResizeRedraw = true;
            //初始化值
            this.Size = new Size(150, 250);
            this.iconSizeMode = ChatListItemIcon.Large;
            this.Font = new Font("微软雅黑", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            this.ForeColor = Color.Black;
            this.items = new ChatListItemCollection(this);
            chatVScroll = new ChatListVScroll(this);
            this.BackColor = Color.FromArgb(50, 255, 255, 255);
        }
        #region 小箭头快捷设置
        [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
        public class UC_SmartTagChatListBoxDesigner :
                 System.Windows.Forms.Design.ControlDesigner
        {
            private DesignerActionListCollection actionLists;

            // Use pull model to populate smart tag menu.
            public override DesignerActionListCollection ActionLists {
                get {
                    if (null == actionLists) {
                        actionLists = new DesignerActionListCollection();
                        actionLists.Add(
                            new UC_SmartTagSupportActionList(this.Component));
                    }
                    return actionLists;
                }
            }
        }


        // DesignerActionList-derived class defines smart tag entries and
        // resultant actions.
        public class UC_SmartTagSupportActionList :
                  System.ComponentModel.Design.DesignerActionList
        {
            private ChatListBox control;

            private DesignerActionUIService designerActionUISvc = null;

            //The constructor associates the control 
            //with the smart tag list.
            public UC_SmartTagSupportActionList(IComponent component)
                : base(component) {
                this.control = component as ChatListBox;

                // Cache a reference to DesignerActionUIService, so the
                // DesigneractionList can be refreshed.
                this.designerActionUISvc =
                    GetService(typeof(DesignerActionUIService))
                    as DesignerActionUIService;
            }

            // Helper method to retrieve control properties. Use of 
            // GetProperties enables undo and menu updates to work properly.
            private PropertyDescriptor GetPropertyByName(String propName) {
                PropertyDescriptor prop = TypeDescriptor.GetProperties(control)[propName];
                if (null == prop)
                    throw new ArgumentException(
                         "Matching ColorLabel property not found!",
                          propName);
                else
                    return prop;
            }

            // Properties that are targets of DesignerActionPropertyItem entries.
            public ChatListItemCollection Items {
                get {
                    return control.Items;
                }
                set {
                    GetPropertyByName("列表中的项").SetValue(control, value);
                }
            }

            // Implementation of this abstract method creates smart tag  
            // items, associates their targets, and collects into list.
            public override DesignerActionItemCollection GetSortedActionItems() {
                DesignerActionItemCollection items = new DesignerActionItemCollection();

                //Define static section header entries.
                items.Add(new DesignerActionHeaderItem("Skin"));

                items.Add(new DesignerActionPropertyItem("Items",
                                     "列表中的项", "Skin",
                                     "列表中的项，包括分组和好友"));

                //Create entries for static Information section.
                StringBuilder location = new StringBuilder("位置: ");
                location.Append(control.Location);
                StringBuilder size = new StringBuilder("大小: ");
                size.Append(control.Size);
                items.Add(new DesignerActionTextItem(location.ToString(),
                                 "Information"));
                items.Add(new DesignerActionTextItem(size.ToString(),
                                 "Information"));

                return items;
            }
        }
        #endregion

        #region 属性
        int rollSize = 50;
        /// <summary>
        /// 滚轮每格滚动的像素值
        /// </summary>
        [Category("滚动条")]
        [DefaultValue(50)]
        [Description("滚轮每格滚动的像素值")]
        public int RollSize {
            get { return rollSize; }
            set { rollSize = value; }
        }

        bool smoothScroll = true;
        /// <summary>
        /// 是否平滑滚动
        /// </summary>
        [Category("滚动条")]
        [DefaultValue(true)]
        [Description("是否平滑滚动")]
        public bool SmoothScroll {
            get { return smoothScroll; }
            set {
                if (smoothScroll != value) {
                    smoothScroll = value;
                    scorllTimer.Enabled = value;
                }
            }
        }

        private ContextMenuStrip subItemMenu;
        /// <summary>
        /// 当用户右击分组时显示的快捷菜单。
        /// </summary>
        [Category("Skin")]
        [Description("当用户右击分组时显示的快捷菜单。")]
        public ContextMenuStrip SubItemMenu {
            get { return subItemMenu; }
            set {
                if (subItemMenu != value) {
                    subItemMenu = value;
                }
            }
        }


        private ContextMenuStrip listsubItemMenu;
        /// <summary>
        /// 当用户右击好友时显示的快捷菜单。
        /// </summary>
        [Category("Skin")]
        [Description("当用户右击好友时显示的快捷菜单。")]
        public ContextMenuStrip ListSubItemMenu {
            get { return listsubItemMenu; }
            set {
                if (listsubItemMenu != value) {
                    listsubItemMenu = value;
                }
            }
        }

        private ChatListItemIcon iconSizeMode;
        /// <summary>
        /// 与列表关联的图标模式
        /// </summary>
        [DefaultValue(ChatListItemIcon.Large)]
        [Category("Skin")]
        [Description("与列表关联的图标模式")]
        public ChatListItemIcon IconSizeMode {
            get { return iconSizeMode; }
            set {
                if (iconSizeMode == value) return;
                iconSizeMode = value;
                this.Invalidate();
            }
        }

        private ChatListItemCollection items;
        /// <summary>
        /// 获取列表中所有列表项的集合
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Skin")]
        [Description("列表框中的项")]
        public ChatListItemCollection Items {
            get {
                if (items == null)
                    items = new ChatListItemCollection(this);
                return items;
            }
        }

        private ChatListSubItem selectSubItem;
        /// <summary>
        /// 当前选中的子项
        /// </summary>
        [Browsable(false)]
        public ChatListSubItem SelectSubItem {
            get { return selectSubItem; }
            set {
                selectSubItem = value;
                MouseDowmSubItems = value;
                if (value != null) {
                    selectSubItem.OwnerListItem.IsOpen = true;
                }
            }
        }

        private ChatListItem selectItem;
        /// <summary>
        /// 当前选中的组
        /// </summary>
        [Browsable(false)]
        public ChatListItem SelectItem {
            get { return selectItem; }
        }

        /// <summary>
        /// 获取或者设置滚动条背景色
        /// </summary>
        [DefaultValue(typeof(Color), "50, 224, 239, 235"), Category("滚动条")]
        [Description("滚动条的背景颜色")]
        public Color ScrollBackColor {
            get { return chatVScroll.BackColor; }
            set {
                chatVScroll.BackColor = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 获取或者设置滚动条滑块默认颜色
        /// </summary>
        [DefaultValue(typeof(Color), "100, 110, 111, 112"), Category("滚动条")]
        [Description("滚动条滑块默认情况下的颜色")]
        public Color ScrollSliderDefaultColor {
            get { return chatVScroll.SliderDefaultColor; }
            set {
                chatVScroll.SliderDefaultColor = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 获取或者设置滚动条点下的颜色
        /// </summary>
        [DefaultValue(typeof(Color), "200, 110, 111, 112"), Category("滚动条")]
        [Description("滚动条滑块被点击或者鼠标移动到上面时候的颜色")]
        public Color ScrollSliderDownColor {
            get { return chatVScroll.SliderDownColor; }
            set {
                chatVScroll.SliderDownColor = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 获取或者设置滚动条箭头的背景色
        /// </summary>
        [DefaultValue(typeof(Color), "Transparent"), Category("滚动条")]
        [Description("滚动条箭头的背景颜色")]
        public Color ScrollArrowBackColor {
            get { return chatVScroll.ArrowBackColor; }
            set {
                chatVScroll.ArrowBackColor = value;
                this.Invalidate();
            }
        }
        /// <summary>
        /// 获取或者设置滚动条的箭头颜色
        /// </summary>
        [DefaultValue(typeof(Color), "200, 148, 150, 151"), Category("滚动条")]
        [Description("滚动条箭头的颜色")]
        public Color ScrollArrowColor {
            get { return chatVScroll.ArrowColor; }
            set {
                chatVScroll.ArrowColor = value;
                this.Invalidate();
            }
        }

        private Color arrowColor = Color.FromArgb(101, 103, 103);
        /// <summary>
        /// 获取或者设置列表项箭头的颜色
        /// </summary>
        [DefaultValue(typeof(Color), "101, 103, 103"), Category("分组")]
        [Description("列表项上面的箭头的颜色")]
        public Color ArrowColor {
            get { return arrowColor; }
            set {
                if (arrowColor == value) return;
                arrowColor = value;
                this.Invalidate();
            }
        }

        private Color itemColor = Color.Transparent;
        /// <summary>
        /// 获取或者设置列表项背景色
        /// </summary>
        [DefaultValue(typeof(Color), "Transparent"), Category("分组")]
        [Description("列表项的背景色")]
        public Color ItemColor {
            get { return itemColor; }
            set {
                if (itemColor == value) return;
                itemColor = value;
                this.Invalidate();
            }
        }
        private bool friendsMobile = true;
        /// <summary>
        /// 获取或者设置好友是否能拖动到其他分组
        /// </summary>
        [DefaultValue(typeof(Color), "true"), Category("好友")]
        [Description("获取或者设置好友是否能拖动到其他分组")]
        public bool FriendsMobile {
            get { return friendsMobile; }
            set {
                if (friendsMobile == value) return;
                friendsMobile = value;
                this.Invalidate();
            }
        }

        private Color subItemColor = Color.Transparent;
        /// <summary>
        /// 获取或者设置子项的背景色
        /// </summary>
        [DefaultValue(typeof(Color), "Transparent"), Category("好友")]
        [Description("列表子项的背景色")]
        public Color SubItemColor {
            get { return subItemColor; }
            set {
                if (subItemColor == value) return;
                subItemColor = value;
                this.Invalidate();
            }
        }

        private Color itemMouseOnColor = Color.FromArgb(150, 230, 238, 241);
        /// <summary>
        /// 获取或者设置当鼠标移动到列表项的颜色
        /// </summary>
        [DefaultValue(typeof(Color), "150, 230, 238, 241"), Category("分组")]
        [Description("当鼠标移动到列表项上面的颜色")]
        public Color ItemMouseOnColor {
            get { return itemMouseOnColor; }
            set {
                itemMouseOnColor = value;
                this.Invalidate();
            }
        }

        private Color subItemMouseOnColor = Color.FromArgb(200, 252, 240, 193);
        /// <summary>
        /// 获取或者设置当鼠标移动到子项的颜色
        /// </summary>
        [DefaultValue(typeof(Color), "200, 252, 240, 193"), Category("好友")]
        [Description("当鼠标移动到子项上面的颜色")]
        public Color SubItemMouseOnColor {
            get { return subItemMouseOnColor; }
            set {
                subItemMouseOnColor = value;
                this.Invalidate();
            }
        }

        private Color subItemSelectColor = Color.FromArgb(200, 252, 236, 172);
        /// <summary>
        /// 获取或者设置选中的子项的颜色
        /// </summary>
        [DefaultValue(typeof(Color), "200, 252, 236, 172"), Category("好友")]
        [Description("当列表子项被选中时候的颜色")]
        public Color SubItemSelectColor {
            get { return subItemSelectColor; }
            set {
                subItemSelectColor = value;
                this.Invalidate();
            }
        }

        private Color vipFontColor = Color.Red;
        /// <summary>
        /// 用户备注字体的颜色
        /// </summary>
        [DefaultValue(typeof(Color), "Red"), Category("好友")]
        [Description("Vip用户备注字体的颜色")]
        public Color VipFontColor {
            get { return vipFontColor; }
            set {
                vipFontColor = value;
                this.Invalidate();
            }
        }
        #endregion

        #region 事件
        public delegate void ChatListClickEventHandler(object sender, ChatListClickEventArgs e, MouseEventArgs es);
        public delegate void ChatListDownEventHandler(object sender, ChatListClickEventArgs e, MouseEventArgs es);
        public delegate void ChatListUpEventHandler(object sender, ChatListClickEventArgs e, MouseEventArgs es);
        public delegate void ChatListEventHandler(object sender, ChatListEventArgs e, MouseEventArgs es);
        public delegate void ChatHeadEventHandler(object sender, ChatListEventArgs e);
        public delegate void DragListEventHandler(object sender, DragListEventArgs e);
        //public delegate void GroupMouseClickEventHandler(object sender, MouseEventArgs e);

        [Description("用鼠标按下子项时发生")]
        [Category("子项操作")]
        public event ChatListClickEventHandler DownSubItem;
        [Description("用鼠标按下并释放子项时发生")]
        [Category("子项操作")]
        public event ChatListClickEventHandler UpSubItem;
        [Description("用鼠标单击子项时发生")]
        [Category("子项操作")]
        public event ChatListClickEventHandler ClickSubItem;
        [Description("用鼠标双击子项时发生")]
        [Category("子项操作")]
        public event ChatListEventHandler DoubleClickSubItem;
        [Description("在鼠标进入子项中的头像时发生")]
        [Category("子项操作")]
        public event ChatHeadEventHandler MouseEnterHead;
        [Description("在鼠标离开子项中的头像时发生")]
        [Category("子项操作")]
        public event ChatHeadEventHandler MouseLeaveHead;
        [Description("拖动子项操作完成后发生")]
        [Category("子项操作")]
        public event DragListEventHandler DragSubItemDrop;
        //[Description("用鼠标单击分组时发生")]
        //[Category("分组操作")]
        //public event GroupMouseClickEventHandler ClickGroupItem;

        protected virtual void OnDownSubItem(ChatListClickEventArgs e, MouseEventArgs es) {
            if (this.DownSubItem != null)
                DownSubItem(this, e, es);
        }

        protected virtual void OnUpSubItem(ChatListClickEventArgs e, MouseEventArgs es) {
            if (this.UpSubItem != null)
                UpSubItem(this, e, es);
        }

        protected virtual void OnClickSubItem(ChatListClickEventArgs e, MouseEventArgs es) {
            if (this.ClickSubItem != null)
                ClickSubItem(this, e, es);
        }

        protected virtual void OnDoubleClickSubItem(ChatListEventArgs e, MouseEventArgs es) {
            if (this.DoubleClickSubItem != null)
                DoubleClickSubItem(this, e, es);
        }

        protected virtual void OnMouseEnterHead(ChatListEventArgs e) {
            if (this.MouseEnterHead != null)
                MouseEnterHead(this, e);
        }

        protected virtual void OnMouseLeaveHead(ChatListEventArgs e) {
            if (this.MouseLeaveHead != null)
                MouseLeaveHead(this, e);
        }

        protected virtual void OnDragSubItemDrop(DragListEventArgs e) {
            if (this.DragSubItemDrop != null)
                DragSubItemDrop(this, e);
        }

        //protected virtual void OnClickGroupItem(MouseEventArgs e) {
        //    if (this.ClickGroupItem != null)
        //        ClickGroupItem(this, e);
        //}
        #endregion

        #region 变量
        private Point m_ptMousePos;             //鼠标的位置
        public ChatListVScroll chatVScroll;    //滚动条
        private ChatListItem m_mouseOnItem;
        private bool m_bOnMouseEnterHeaded;     //确定用户绑定事件是否被触发
        private ChatListSubItem m_mouseOnSubItem;
        #endregion

        #region 按下并释放按钮时(OnMouseUp)
        protected override void OnMouseUp(MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                chatVScroll.IsMouseDown = false;
            }
            //结束标记
            MouseDowns = false;
            //触发事件
            if (SelectSubItem != null) {
                OnUpSubItem(new ChatListClickEventArgs(this.SelectSubItem), e);
            }
            if (e.Button == MouseButtons.Left) {
                //是否允许拖动好友
                if (FriendsMobile) {
                    //判断是否在一个子项上点击
                    for (int i = 0, Len = items.Count; i < Len; i++) {      //然后判断鼠标是否移动到某一列表项或者子项
                        if (items[i].Bounds.Contains(m_ptMousePos)) {
                            if (!items[i].IsOpen && MouseMoveItems) {
                                if (m_mouseOnItem != null) {
                                    if (m_mouseOnItem != MouseDowmSubItems.OwnerListItem) {
                                        //使用深拷贝克隆一个拖动前的好友
                                        ChatListSubItem chatQSubItem = MouseDowmSubItems.Clone();
                                        //删除原先位置的好友
                                        MouseDowmSubItems.OwnerListItem.SubItems.Remove(MouseDowmSubItems);
                                        //展开拖动前的好友列表
                                        MouseDowmSubItems.OwnerListItem.IsOpen = true;
                                        //更改所属分组
                                        MouseDowmSubItems.OwnerListItem = m_mouseOnItem;
                                        //将好友移至新分组
                                        m_mouseOnItem.SubItems.AddAccordingToStatus(MouseDowmSubItems);
                                        //展开拖动后的好友列表
                                        m_mouseOnItem.IsOpen = true;
                                        //引发拖动事件
                                        OnDragSubItemDrop(new DragListEventArgs(chatQSubItem, MouseDowmSubItems));
                                        //移动结束
                                        MouseMoveItems = false;
                                    }
                                }
                            }
                        }
                    }
                }
            } else if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                m_ptMousePos = e.Location;
                m_ptMousePos.Y += chatVScroll.Value;
                //如果在列表上点击 展开或者关闭 在子项上面点击则选中
                foreach (ChatListItem item in items) {
                    if (item.Bounds.Contains(m_ptMousePos)) {
                        if (item.IsOpen) {
                            foreach (ChatListSubItem subItem in item.SubItems) {
                                if (subItem.Bounds.Contains(m_ptMousePos)) {
                                    if (ListSubItemMenu == null) { return; }
                                    ListSubItemMenu.Show(Cursor.Position);
                                    return;
                                }
                            }
                            if (new Rectangle(0, item.Bounds.Top, this.Width, 25).Contains(m_ptMousePos)) {
                                if (SubItemMenu == null) return;
                                SubItemMenu.Show(Cursor.Position);
                                return;
                            }
                        } else {
                            if (SubItemMenu == null) return;
                            SubItemMenu.Show(Cursor.Position);
                        }
                    }
                }
            }
            base.OnMouseUp(e);
        }
        #endregion

        #region 鼠标按下时(OnMouseDown)
        bool MouseDowns = false;
        ChatListSubItem MouseDowmSubItems;
        int CursorY;
        protected override void OnMouseDown(MouseEventArgs e) {
            this.Focus();
            //如果左键在滚动条滑块上点击
            if (chatVScroll.SliderBounds.Contains(m_ptMousePos)) {

                if (e.Button == MouseButtons.Left) {
                    chatVScroll.IsMouseDown = true;
                    chatVScroll.MouseDownY = e.Y;
                }
            } else {
                m_ptMousePos = e.Location;
                m_ptMousePos.Y += chatVScroll.Value;
                //如果在列表上点击 展开或者关闭 在子项上面点击则选中
                foreach (ChatListItem item in items) {
                    //存储选中分组
                    selectItem = item;
                    if (item.Bounds.Contains(m_ptMousePos)) {
                        if (item.IsOpen) {
                            foreach (ChatListSubItem subItem in item.SubItems) {
                                if (subItem.Bounds.Contains(m_ptMousePos)) {
                                    selectSubItem = subItem;
                                    //触发事件
                                    OnDownSubItem(new ChatListClickEventArgs(this.SelectSubItem), e);
                                    this.Invalidate();
                                    if (e.Button == MouseButtons.Left) {
                                        CursorY = Cursor.Position.Y;
                                        MouseDowns = true;
                                        //保存并设置选中Item
                                        MouseDowmSubItems = subItem;
                                    }
                                    return;
                                }
                            }
                            if (new Rectangle(0, item.Bounds.Top, this.Width, 25).Contains(m_ptMousePos)) {
                                selectSubItem = null;
                                this.Invalidate();

                                if (listHadOpenGroup != null && listHadOpenGroup.Contains(item)) {
                                    listHadOpenGroup.Remove(item);
                                }

                                if (e.Button == MouseButtons.Left) {
                                    item.IsOpen = !item.IsOpen;
                                }
                                return;
                            }
                        } else {

                            selectSubItem = null;
                            this.Invalidate();
                            if (e.Button == MouseButtons.Left) {
                                item.IsOpen = !item.IsOpen;
                                if (listHadOpenGroup == null) {
                                    listHadOpenGroup = new List<ChatListItem>();
                                }
                                listHadOpenGroup.Add(item);
                            }
                            return;
                        }
                    } else {
                        selectItem = null;
                    }
                }
            }
            base.OnMouseDown(e);
        }
        #endregion

        #region 鼠标滚轮滑动时(OnMouseWheel)
        protected override void OnMouseWheel(MouseEventArgs e) {
            //调用平滑移动
            if (SmoothScroll) {
                if (e.Delta > 0) scrollSpeed -= 10;
                if (e.Delta < 0) scrollSpeed += 10;
            } else {
                //普通移动
                if (e.Delta > 0) chatVScroll.Value -= RollSize;
                if (e.Delta < 0) chatVScroll.Value += RollSize;
            }
            base.OnMouseWheel(e);
        }
        #endregion

        #region 重绘(OnPaint)
        protected override void OnPaint(PaintEventArgs e) {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality; //高质量
            g.PixelOffsetMode = PixelOffsetMode.HighQuality; //高像素偏移质量
            //最高质量绘制文字
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            g.TranslateTransform(0, -chatVScroll.Value);        //根据滚动条的值设置坐标偏移
            int SubItemWidth = chatVScroll.ShouldBeDraw ? this.Width - 9 : this.Width;
            Rectangle rectItem = new Rectangle(0, 1, SubItemWidth, 25);                       //列表项区域
            Rectangle rectSubItem = new Rectangle(0, 26, SubItemWidth, (int)iconSizeMode);    //子项区域
            SolidBrush sb = new SolidBrush(this.itemColor);
            try {
                for (int i = 0, lenItem = items.Count; i < lenItem; i++) {
                    //判断是否在容器可视范围内
                    Rectangle SubRc = new Rectangle(rectItem.X, rectItem.Y - chatVScroll.Value, rectItem.Width, rectItem.Height);
                    //判断是否在容器可视范围内
                    if (this.ClientRectangle.IntersectsWith(SubRc)) {
                        DrawItem(g, items[i], rectItem, sb);        //绘制分组项
                    }
                    if (items[i].IsOpen) {
                        //如果列表项展开绘制子项
                        rectSubItem.Y = rectItem.Bottom + 1;
                        for (int j = 0, lenSubItem = items[i].SubItems.Count; j < lenSubItem; j++) {
                            //克隆一个子项位置，用于判断是否绘制
                            Rectangle subItemClone = new Rectangle(rectSubItem.X, rectSubItem.Y, rectSubItem.Width, rectSubItem.Height);
                            //取得正确子项位置
                            DrawSubItem(g, items[i].SubItems[j], ref subItemClone, sb, false);
                            //判断是否在容器可视范围内
                            Rectangle subItemRc = new Rectangle(subItemClone.X, subItemClone.Y - chatVScroll.Value, subItemClone.Width, subItemClone.Height);
                            //是否绘制
                            bool IsDraw = this.ClientRectangle.IntersectsWith(subItemRc);
                            DrawSubItem(g, items[i].SubItems[j], ref rectSubItem, sb, IsDraw);  //绘制子项
                            rectSubItem.Y = rectSubItem.Bottom + 1;             //计算下一个子项的区域
                            rectSubItem.Height = (int)iconSizeMode;
                        }
                        rectItem.Height = rectSubItem.Bottom - rectItem.Top - (int)iconSizeMode - 1;
                    }
                    items[i].Bounds = new Rectangle(rectItem.Location, rectItem.Size);
                    rectItem.Y = rectItem.Bottom + 1;           //计算下一个列表项区域
                    rectItem.Height = 25;
                }
                g.ResetTransform();             //重置坐标系
                chatVScroll.VirtualHeight = rectItem.Bottom - 26;   //绘制完成计算虚拟高度决定是否绘制滚动条
                if (chatVScroll.ShouldBeDraw)   //是否绘制滚动条
                    chatVScroll.ReDrawScroll(g);
            } finally {
                sb.Dispose();
                if (!DesignMode) {
                    GC.Collect();
                }
            }
            base.OnPaint(e);
        }
        #endregion

        #region 初始化控件时(OnCreateControl)
        protected override void OnCreateControl() {
            Thread threadInvalidate = new Thread(new ThreadStart(() => {
                Rectangle rectReDraw = new Rectangle(0, 0, this.Width, this.Height);
                while (true) {          //后台检测要闪动的图标然后重绘
                    for (int i = 0, lenI = this.items.Count; i < lenI; i++) {
                        if (items[i].IsOpen) {
                            for (int j = 0, lenJ = items[i].SubItems.Count; j < lenJ; j++) {
                                if (items[i].SubItems[j].IsTwinkle) {
                                    items[i].SubItems[j].IsTwinkleHide = !items[i].SubItems[j].IsTwinkleHide;
                                    rectReDraw.Y = items[i].SubItems[j].Bounds.Y - chatVScroll.Value;
                                    rectReDraw.Height = items[i].SubItems[j].Bounds.Height;
                                    this.Invalidate(rectReDraw);
                                } else if (items[i].SubItems[j].IsTwinkleHide) {
                                    items[i].SubItems[j].IsTwinkleHide = false;
                                    rectReDraw.Y = items[i].SubItems[j].Bounds.Y - chatVScroll.Value;
                                    rectReDraw.Height = items[i].SubItems[j].Bounds.Height;
                                    this.Invalidate(rectReDraw);
                                }
                            }
                        } else {
                            rectReDraw.Y = items[i].Bounds.Y - chatVScroll.Value;
                            rectReDraw.Height = items[i].Bounds.Height;
                            if (items[i].TwinkleSubItemNumber > 0) {
                                items[i].IsTwinkleHide = !items[i].IsTwinkleHide;
                                this.Invalidate(rectReDraw);
                            } else if (items[i].IsTwinkleHide) {
                                items[i].IsTwinkleHide = !items[i].IsTwinkleHide;
                                this.Invalidate(rectReDraw);
                            }
                        }
                    }
                    Thread.Sleep(210);
                }
            }));
            threadInvalidate.IsBackground = true;
            threadInvalidate.Start();
            base.OnCreateControl();
        }
        #endregion

        #region 鼠标悬浮在控件区域时(OnMouseMove)
        bool MouseMoveItems = false;
        protected override void OnMouseMove(MouseEventArgs e) {
            m_ptMousePos = e.Location;
            if (chatVScroll.IsMouseDown) {          //如果滚动条的滑块处于被点击 那么移动
                chatVScroll.MoveSliderFromLocation(e.Y);
                return;
            }
            if (chatVScroll.ShouldBeDraw) {
                //如果控件上有滚动条 判断鼠标是否在滚动条区域移动
                if (chatVScroll.Bounds.Contains(m_ptMousePos)) {
                    ClearItemMouseOn();
                    ClearSubItemMouseOn();
                    //if (chatVScroll.SliderBounds.Contains(m_ptMousePos))
                    //{
                    chatVScroll.IsMouseOnSlider = true;
                    //}
                    //else
                    //{
                    //    chatVScroll.IsMouseOnSlider = false;
                    //}
                    //if (chatVScroll.UpBounds.Contains(m_ptMousePos))
                    chatVScroll.IsMouseOnUp = true;
                    //else
                    //chatVScroll.IsMouseOnUp = false;
                    //if (chatVScroll.DownBounds.Contains(m_ptMousePos))
                    chatVScroll.IsMouseOnDown = true;
                    //else
                    //chatVScroll.IsMouseOnDown = false;
                    return;
                } else {
                    chatVScroll.ClearAllMouseOn();
                }
            }
            m_ptMousePos.Y += chatVScroll.Value;        //如果不在滚动条范围类 那么根据滚动条当前值计算虚拟的一个坐标
            for (int i = 0, Len = items.Count; i < Len; i++) {      //然后判断鼠标是否移动到某一列表项或者子项
                if (items[i].Bounds.Contains(m_ptMousePos)) {
                    if (items[i].IsOpen) {              //如果展开 判断鼠标是否在某一子项上面
                        for (int j = 0, lenSubItem = items[i].SubItems.Count; j < lenSubItem; j++) {
                            if (items[i].SubItems[j].Bounds.Contains(m_ptMousePos)) {
                                if (m_mouseOnSubItem != null) {             //如果当前鼠标下子项不为空
                                    if (items[i].SubItems[j].HeadRect.Contains(m_ptMousePos)) {     //判断鼠标是否在头像内
                                        if (!m_bOnMouseEnterHeaded) {       //如果没有触发进入事件 那么触发用户绑定的事件
                                            OnMouseEnterHead(new ChatListEventArgs(this.m_mouseOnSubItem, this.selectSubItem));
                                            m_bOnMouseEnterHeaded = true;
                                        }
                                    } else {
                                        if (m_bOnMouseEnterHeaded) {        //如果已经执行过进入事件 那触发用户绑定的离开事件
                                            OnMouseLeaveHead(new ChatListEventArgs(this.m_mouseOnSubItem, this.selectSubItem));
                                            m_bOnMouseEnterHeaded = false;
                                        }
                                    }

                                    #region 点击并移动子项时
                                    //如果点击并移动了子项
                                    if (MouseDowns && Math.Abs(CursorY - Cursor.Position.Y) > 4 && FriendsMobile) {
                                        //将所有的父节点设置为不展开
                                        for (int z = 0; z < Items.Count; z++) {
                                            if (Items[z].IsOpen) {
                                                Items[z].IsOpen = false;
                                            }
                                        }

                                        //开始设置鼠标幻影
                                        m_mouseOnSubItem.OwnerListItem.IsOpen = false;
                                        //开始移动
                                        MouseMoveItems = true;

                                        //获取选中后颜色，再不透明处理
                                        Color color = Color.FromArgb(250, SubItemSelectColor.R, SubItemSelectColor.G, SubItemSelectColor.B);
                                        string strDraw = string.IsNullOrEmpty(m_mouseOnSubItem.DisplayName) ? (string.IsNullOrEmpty(m_mouseOnSubItem.NicName) ? m_mouseOnSubItem.ID.ToString() : m_mouseOnSubItem.NicName) : m_mouseOnSubItem.DisplayName;
                                        Size szFont = TextRenderer.MeasureText(strDraw, this.Font);
                                        int bmpWidth = (45 + szFont.Width + 10);
                                        int bmpHeight = 45;
                                        Bitmap bmp = new Bitmap(bmpWidth * 2, bmpHeight * 2);
                                        Graphics g = Graphics.FromImage(bmp);
                                        g.FillRectangle(new SolidBrush(color), bmpWidth, bmpHeight, bmpWidth, bmpHeight);
                                        g.DrawImage(m_mouseOnSubItem.HeadImage, bmpWidth, bmpHeight, 45, 45);

                                        Brush normalBrush = Brushes.Black;
                                        if (m_mouseOnSubItem.IsVip) {
                                            normalBrush = new SolidBrush(VipFontColor);
                                        }
                                        //判断是否有备注名称
                                        if (szFont.Width > 0) {
                                            g.DrawString(strDraw, this.Font, normalBrush, bmpWidth + bmpHeight + 5, bmpHeight + (bmpHeight - szFont.Height) / 2);
                                        } else //如果没有备注名称 这直接绘制昵称
                                        {
                                            g.DrawString(m_mouseOnSubItem.NicName, this.Font, normalBrush, bmpWidth + bmpHeight + 5, bmpHeight + (bmpHeight - szFont.Height) / 2);
                                        }

                                        Cursor cur = new Cursor(bmp.GetHicon());
                                        Cursor.Current = cur;
                                    }
                                    #endregion
                                }
                                if (items[i].SubItems[j].Equals(m_mouseOnSubItem)) {
                                    return;
                                }
                                ClearSubItemMouseOn();
                                ClearItemMouseOn();
                                m_mouseOnSubItem = items[i].SubItems[j];
                                this.Invalidate(new Rectangle(
                                    m_mouseOnSubItem.Bounds.X, m_mouseOnSubItem.Bounds.Y - chatVScroll.Value,
                                    m_mouseOnSubItem.Bounds.Width, m_mouseOnSubItem.Bounds.Height));
                                return;
                            }
                        }
                        ClearSubItemMouseOn();      //循环做完没发现子项 那么判断是否在列表上面
                        if (new Rectangle(0, items[i].Bounds.Top - chatVScroll.Value, this.Width, 25).Contains(e.Location)) {
                            if (items[i].Equals(m_mouseOnItem))
                                return;
                            ClearItemMouseOn();
                            m_mouseOnItem = items[i];
                            this.Invalidate(new Rectangle(
                                m_mouseOnItem.Bounds.X, m_mouseOnItem.Bounds.Y - chatVScroll.Value,
                                m_mouseOnItem.Bounds.Width, m_mouseOnItem.Bounds.Height));
                            return;
                        }
                    } else {        //如果列表项没有展开 重绘列表项
                        if (items[i].Equals(m_mouseOnItem))
                            return;
                        ClearItemMouseOn();
                        ClearSubItemMouseOn();
                        m_mouseOnItem = items[i];
                        this.Invalidate(new Rectangle(
                                m_mouseOnItem.Bounds.X, m_mouseOnItem.Bounds.Y - chatVScroll.Value,
                                m_mouseOnItem.Bounds.Width, m_mouseOnItem.Bounds.Height));
                        return;
                    }
                }
            }
            //若循环结束 既不在列表上也不再子项上 清空上面的颜色
            ClearItemMouseOn();
            ClearSubItemMouseOn();
            base.OnMouseMove(e);
        }
        #endregion

        #region 鼠标离开控件区域时(OnMouseLeave)
        protected override void OnMouseLeave(EventArgs e) {
            ClearItemMouseOn();
            ClearSubItemMouseOn();
            chatVScroll.ClearAllMouseOn();
            if (m_bOnMouseEnterHeaded) {        //如果已经执行过进入事件 那触发用户绑定的离开事件
                OnMouseLeaveHead(new ChatListEventArgs(this.m_mouseOnSubItem, this.selectSubItem));
                m_bOnMouseEnterHeaded = false;
            }
            base.OnMouseLeave(e);
        }
        #endregion

        #region 在控件区域完成一次点击后(OnMouseClick)
        protected override void OnMouseClick(MouseEventArgs e) {
            if (chatVScroll.IsMouseDown) return;    //MouseUp事件触发在Click后 滚动条滑块为点下状态 单击无效
            if (chatVScroll.ShouldBeDraw) {         //如果有滚动条 判断是否在滚动条类点击
                if (chatVScroll.Bounds.Contains(m_ptMousePos)) {        //判断在滚动条那个位置点击
                    if (chatVScroll.UpBounds.Contains(m_ptMousePos))
                        chatVScroll.Value -= 50;
                    else if (chatVScroll.DownBounds.Contains(m_ptMousePos))
                        chatVScroll.Value += 50;
                    else if (!chatVScroll.SliderBounds.Contains(m_ptMousePos))
                        chatVScroll.MoveSliderToLocation(m_ptMousePos.Y);
                    return;
                }
            }
            //判断是否悬浮在好友上，是则返回好友，不是则返回null
            if (m_mouseOnSubItem != null) {
                //在好友范围内
                OnClickSubItem(new ChatListClickEventArgs(this.SelectSubItem), e);
            }
            base.OnMouseClick(e);
        }

        /// <summary>
        /// 判断是否悬浮在好友上，是则返回好友，不是则返回null
        /// </summary>
        /// <returns>ChatListSubItem好友对象</returns>
        public ChatListSubItem GetMouseSubItemBool() {
            ChatListSubItem Item = null;
            //然后判断鼠标是否移动到某一列表项或者子项
            for (int i = 0, Len = items.Count; i < Len; i++) {
                if (items[i].Bounds.Contains(m_ptMousePos)) {
                    if (items[i].IsOpen) {              //如果展开 判断鼠标是否在某一子项上面
                        for (int j = 0, lenSubItem = items[i].SubItems.Count; j < lenSubItem; j++) {
                            if (items[i].SubItems[j].Bounds.Contains(m_ptMousePos)) {
                                if (m_mouseOnSubItem != null) {             //如果当前鼠标下子项不为空
                                    if (items[i].SubItems[j].Bounds.Contains(m_ptMousePos)) {
                                        return items[i].SubItems[j];
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return Item;
        }

        //protected override void OnClick(EventArgs e) {
        //    if (chatVScroll.IsMouseDown) return;    //MouseUp事件触发在Click后 滚动条滑块为点下状态 单击无效
        //    if (chatVScroll.ShouldBeDraw) {         //如果有滚动条 判断是否在滚动条类点击
        //        if (chatVScroll.Bounds.Contains(m_ptMousePos)) {        //判断在滚动条那个位置点击
        //            if (chatVScroll.UpBounds.Contains(m_ptMousePos))
        //                chatVScroll.Value -= 50;
        //            else if (chatVScroll.DownBounds.Contains(m_ptMousePos))
        //                chatVScroll.Value += 50;
        //            else if (!chatVScroll.SliderBounds.Contains(m_ptMousePos))
        //                chatVScroll.MoveSliderToLocation(m_ptMousePos.Y);
        //            return;
        //        }
        //    }

        //    base.OnClick(e);
        //}
        #endregion

        #region 在控件区域完成连续两次点击后(OnMouseDoubleClick)
        protected override void OnMouseDoubleClick(MouseEventArgs e) {
            //this.OnMouseClick(e);        //双击时 再次触发一下单击事件  不然双击列表项 相当于只点击了一下列表项
            if (chatVScroll.Bounds.Contains(PointToClient(MousePosition))) return;  //如果双击在滚动条上返回
            if (this.selectSubItem != null)     //如果选中项不为空 那么触发用户绑定的双击事件
                OnDoubleClickSubItem(new ChatListEventArgs(this.m_mouseOnSubItem, this.selectSubItem), e);
            base.OnMouseDoubleClick(e);
        }

        //protected override void OnDoubleClick(EventArgs e) {
        //    this.OnClick(e);        //双击时 再次触发一下单击事件  不然双击列表项 相当于只点击了一下列表项
        //    if (chatVScroll.Bounds.Contains(PointToClient(MousePosition))) return;  //如果双击在滚动条上返回
        //    if (this.selectSubItem != null)     //如果选中项不为空 那么触发用户绑定的双击事件
        //        OnDoubleClickSubItem(new ChatListEventArgs(this.m_mouseOnSubItem, this.selectSubItem));
        //    base.OnDoubleClick(e);
        //}
        #endregion

        #region 绘制列表项(DrawItem)
        /// <summary>
        /// 绘制列表项
        /// </summary>
        /// <param name="g">绘图表面</param>
        /// <param name="item">要绘制的列表项</param>
        /// <param name="rectItem">该列表项的区域</param>
        /// <param name="sb">画刷</param>
        protected virtual void DrawItem(Graphics g, ChatListItem item, Rectangle rectItem, SolidBrush sb) {
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.SetTabStops(0.0F, new float[] { 20.0F });
            if (item.Equals(m_mouseOnItem)) {          //根据列表项现在的状态绘制相应的背景色
                sb.Color = this.ItemMouseOnColor;
            } else {
                sb.Color = this.itemColor;
            }
            //分组颜色
            g.FillRectangle(sb, rectItem);
            if (item.IsOpen) {                      //如果展开的画绘制 展开的三角形
                sb.Color = this.arrowColor;
                g.FillPolygon(sb, new Point[] { 
                        new Point(2, rectItem.Top + 11), 
                        new Point(12, rectItem.Top + 11), 
                        new Point(7, rectItem.Top + 16) });
            } else {
                sb.Color = this.arrowColor;
                g.FillPolygon(sb, new Point[] { 
                        new Point(5, rectItem.Top + 8), 
                        new Point(5, rectItem.Top + 18), 
                        new Point(10, rectItem.Top + 13) });
                //如果没有展开判断该列表项下面的子项闪动的个数
                if (item.TwinkleSubItemNumber > 0) {
                    //如果列表项下面有子项闪动 那么判断闪动状态 是否绘制或者不绘制
                    if (item.IsTwinkleHide)             //该布尔值 在线程中不停 取反赋值
                    {
                        return;
                    }
                }
            }
            string strItem = "\t" + item.Text;
            sb.Color = this.ForeColor;
            sf.Alignment = StringAlignment.Near;
            g.DrawString(strItem, this.Font, sb, rectItem, sf);
            Size Itemsize = TextRenderer.MeasureText(item.Text, this.Font);
            sf.Alignment = StringAlignment.Near;
            g.DrawString("[" + item.SubItems.GetOnLineNumber() + "/" + item.SubItems.Count + "]", this.Font, sb,
                new Rectangle(rectItem.X + Convert.ToInt32(Itemsize.Width) + 25, rectItem.Y, rectItem.Width - 15, rectItem.Height), sf);
        }
        #endregion

        #region 绘制列表子项(DrawSubItem)
        /// <summary>
        /// 绘制列表子项
        /// </summary>
        /// <param name="g">绘图表面</param>
        /// <param name="subItem">要绘制的子项</param>
        /// <param name="rectSubItem">该子项的区域</param>
        /// <param name="sb">画刷</param>
        protected virtual void DrawSubItem(Graphics g, ChatListSubItem subItem, ref Rectangle rectSubItem, SolidBrush sb, bool Draw = true) {
            if (subItem.Equals(selectSubItem)) {
                //判断改子项是否被选中
                rectSubItem.Height = (int)ChatListItemIcon.Large;   //如果选中则绘制成大图标
                sb.Color = this.subItemSelectColor;
                //是否绘制
                if (Draw) {
                    g.FillRectangle(sb, rectSubItem);
                    DrawHeadImage(g, subItem, rectSubItem);         //绘制头像
                    DrawLargeSubItem(g, subItem, rectSubItem);      //绘制大图标 显示的个人信息
                }
                subItem.Bounds = new Rectangle(rectSubItem.Location, rectSubItem.Size);
                return;
            } else if (subItem.Equals(m_mouseOnSubItem)) {
                sb.Color = this.subItemMouseOnColor;
            } else {
                sb.Color = this.subItemColor;
            }
            //是否绘制
            if (Draw) {
                g.FillRectangle(sb, rectSubItem);
                DrawHeadImage(g, subItem, rectSubItem);
            }

            //是否绘制
            if (Draw) {
                if (iconSizeMode == ChatListItemIcon.Large)         //没有选中则根据 图标模式绘制
                    DrawLargeSubItem(g, subItem, rectSubItem);
                else
                    DrawSmallSubItem(g, subItem, rectSubItem);
            }

            subItem.Bounds = new Rectangle(rectSubItem.Location, rectSubItem.Size);
        }
        #endregion

        #region 绘制列表子项的头像(DrawHeadImage)
        /// <summary>
        /// 绘制列表子项的头像
        /// </summary>
        /// <param name="g">绘图表面</param>
        /// <param name="subItem">要绘制头像的子项</param>
        /// <param name="rectSubItem">该子项的区域</param>
        protected virtual void DrawHeadImage(Graphics g, ChatListSubItem subItem, Rectangle rectSubItem) {
            if (subItem.IsTwinkle) {        //判断改头像是否闪动
                if (subItem.IsTwinkleHide)  //同理该值 在线程中 取反赋值
                    return;
            }

            int imageHeight = rectSubItem.Height == 53 ? 40 : 20;      //根据子项的大小计算头像的区域
            subItem.HeadRect = new Rectangle(5, rectSubItem.Top + (rectSubItem.Height - imageHeight) / 2, imageHeight, imageHeight);

            if (subItem.HeadImage == null)                 //如果头像为空 用默认资源给定的头像
            {
                subItem.HeadImage = global::Com_CSSkin.Properties.Resources._1_100;
            }
            if (subItem.Status == ChatListSubItem.UserStatus.OffLine) {
                g.DrawImage(subItem.GetDarkImage(), subItem.HeadRect);
            } else {
                g.DrawImage(subItem.HeadImage, subItem.HeadRect);       //如果在线根据在想状态绘制小图标
                if (this.IconSizeMode == ChatListItemIcon.Small) //小图标模式
                {
                    if (subItem.PlatformTypes == PlatformType.PC) //PC端
                    {
                        if (subItem.Status == ChatListSubItem.UserStatus.QMe)
                            g.DrawImage(global::Com_CSSkin.Properties.Resources.Small_Qme, new Rectangle(subItem.HeadRect.Right - 9, subItem.HeadRect.Bottom - 9, 9, 9));
                        if (subItem.Status == ChatListSubItem.UserStatus.Away)
                            g.DrawImage(global::Com_CSSkin.Properties.Resources.Small_away, new Rectangle(subItem.HeadRect.Right - 9, subItem.HeadRect.Bottom - 9, 9, 9));
                        if (subItem.Status == ChatListSubItem.UserStatus.Busy)
                            g.DrawImage(global::Com_CSSkin.Properties.Resources.Small_busy, new Rectangle(subItem.HeadRect.Right - 9, subItem.HeadRect.Bottom - 9, 9, 9));
                        if (subItem.Status == ChatListSubItem.UserStatus.DontDisturb)
                            g.DrawImage(global::Com_CSSkin.Properties.Resources.Small_mute, new Rectangle(subItem.HeadRect.Right - 9, subItem.HeadRect.Bottom - 9, 9, 9));
                    } else if (subItem.PlatformTypes == PlatformType.Iphone) //Iphone端
                    {
                        g.DrawImage(global::Com_CSSkin.Properties.Resources.Small_IPhoneQQ_Head_Small, new Rectangle(subItem.HeadRect.Right - 9, subItem.HeadRect.Bottom - 11, 9, 11));
                    } else if (subItem.PlatformTypes == PlatformType.WebQQ) //WebQQ端
                    {
                        if (subItem.Status == ChatListSubItem.UserStatus.Away)
                            g.DrawImage(global::Com_CSSkin.Properties.Resources.Small_ImQQAway, new Rectangle(subItem.HeadRect.Right - 10, subItem.HeadRect.Bottom - 10, 10, 10));
                        if (subItem.Status == ChatListSubItem.UserStatus.Online)
                            g.DrawImage(global::Com_CSSkin.Properties.Resources.Small_ImQQOnline, new Rectangle(subItem.HeadRect.Right - 10, subItem.HeadRect.Bottom - 10, 10, 10));
                    } else if (subItem.PlatformTypes == PlatformType.Aandroid) //安卓端
                    {
                        g.DrawImage(global::Com_CSSkin.Properties.Resources.Small_MobilePhoneQQOn, new Rectangle(subItem.HeadRect.Right - 13, subItem.HeadRect.Bottom - 13, 13, 13));
                    }
                } else //大图标模式
                {
                    if (subItem.PlatformTypes == PlatformType.PC) //PC端
                    {
                        if (subItem.Status == ChatListSubItem.UserStatus.QMe)
                            g.DrawImage(global::Com_CSSkin.Properties.Resources.QMe, new Rectangle(subItem.HeadRect.Right - 11, subItem.HeadRect.Bottom - 11, 11, 11));
                        if (subItem.Status == ChatListSubItem.UserStatus.Away)
                            g.DrawImage(global::Com_CSSkin.Properties.Resources.Away, new Rectangle(subItem.HeadRect.Right - 11, subItem.HeadRect.Bottom - 11, 11, 11));
                        if (subItem.Status == ChatListSubItem.UserStatus.Busy)
                            g.DrawImage(global::Com_CSSkin.Properties.Resources.Busy, new Rectangle(subItem.HeadRect.Right - 11, subItem.HeadRect.Bottom - 11, 11, 11));
                        if (subItem.Status == ChatListSubItem.UserStatus.DontDisturb)
                            g.DrawImage(global::Com_CSSkin.Properties.Resources.Dont_Disturb, new Rectangle(subItem.HeadRect.Right - 11, subItem.HeadRect.Bottom - 11, 11, 11));
                    } else if (subItem.PlatformTypes == PlatformType.Iphone) //Iphone端
                    {
                        g.DrawImage(global::Com_CSSkin.Properties.Resources.IPhoneQQ_Head_Big, new Rectangle(subItem.HeadRect.Right - 14, subItem.HeadRect.Bottom - 20, 14, 20));
                    } else if (subItem.PlatformTypes == PlatformType.WebQQ) //WebQQ端
                    {
                        if (subItem.Status == ChatListSubItem.UserStatus.QMe)
                            g.DrawImage(global::Com_CSSkin.Properties.Resources.WebQQQme, new Rectangle(subItem.HeadRect.Right - 20, subItem.HeadRect.Bottom - 20, 20, 20));
                        if (subItem.Status == ChatListSubItem.UserStatus.Away)
                            g.DrawImage(global::Com_CSSkin.Properties.Resources.WebQQAway, new Rectangle(subItem.HeadRect.Right - 20, subItem.HeadRect.Bottom - 20, 20, 20));
                        if (subItem.Status == ChatListSubItem.UserStatus.Busy)
                            g.DrawImage(global::Com_CSSkin.Properties.Resources.WebQQBusy, new Rectangle(subItem.HeadRect.Right - 20, subItem.HeadRect.Bottom - 20, 20, 20));
                        if (subItem.Status == ChatListSubItem.UserStatus.DontDisturb)
                            g.DrawImage(global::Com_CSSkin.Properties.Resources.WebQQMute, new Rectangle(subItem.HeadRect.Right - 20, subItem.HeadRect.Bottom - 20, 20, 20));
                        if (subItem.Status == ChatListSubItem.UserStatus.Online)
                            g.DrawImage(global::Com_CSSkin.Properties.Resources.WebQQOnline, new Rectangle(subItem.HeadRect.Right - 20, subItem.HeadRect.Bottom - 20, 20, 20));
                    } else if (subItem.PlatformTypes == PlatformType.Aandroid) //安卓端
                    {
                        g.DrawImage(global::Com_CSSkin.Properties.Resources.MobilePhoneQQOn, new Rectangle(subItem.HeadRect.Right - 20, subItem.HeadRect.Bottom - 20, 20, 20));
                    }
                }
            }

            if (subItem.Equals(selectSubItem))              //根据是否选中头像绘制头像的外边框
            {
                g.DrawImage(Properties.Resources.MainPanel, subItem.HeadRect.X - 3, subItem.HeadRect.Y - 3, 46, 46);
            } else {
                Pen pen = new Pen(Color.FromArgb(200, 255, 255, 255));
                g.DrawRectangle(pen, subItem.HeadRect);
            }
        }
        #endregion

        #region 绘制大图标模式的个人信息(DrawLargeSubItem)
        /// <summary>
        /// 绘制大图标模式的个人信息
        /// </summary>
        /// <param name="g">绘图表面</param>
        /// <param name="subItem">要绘制信息的子项</param>
        /// <param name="rectSubItem">该子项的区域</param>
        protected virtual void DrawLargeSubItem(Graphics g, ChatListSubItem subItem, Rectangle rectSubItem) {
            rectSubItem.Height = (int)ChatListItemIcon.Large;       //重新赋值一个高度
            string strDraw = subItem.DisplayName;
            Size szFont = TextRenderer.MeasureText(strDraw, this.Font);
            Size NickNameFont = TextRenderer.MeasureText(subItem.NicName, this.Font);
            StringFormat Sf = new StringFormat(StringFormatFlags.NoWrap);
            Sf.Trimming = StringTrimming.Word;
            Rectangle Rc = new Rectangle(new Point(rectSubItem.Height, rectSubItem.Top + 8), new Size(this.Width - 9 - rectSubItem.Height, szFont.Height));
            Rectangle NickNameRc = new Rectangle(new Point(rectSubItem.Height + szFont.Width, rectSubItem.Top + 8), new Size(this.Width - 9 - rectSubItem.Height - szFont.Width, szFont.Height));

            SolidBrush normalBrush = new SolidBrush(this.ForeColor);
            if (subItem.IsVip) {
                normalBrush = new SolidBrush(VipFontColor);
            }
            //判断是否有备注名称
            if (szFont.Width > 0) {
                g.DrawString(strDraw, this.Font, normalBrush, Rc, Sf);
                g.DrawString("(" + subItem.NicName + ")",
                    this.Font, Brushes.Gray, NickNameRc, Sf);
            } else  //如果没有备注名称 这直接绘制昵称
            {
                Rectangle nkNameRc = new Rectangle(new Point(rectSubItem.Height, rectSubItem.Top + 8), new Size(this.Width - 9 - rectSubItem.Height, szFont.Height));
                g.DrawString(subItem.NicName, this.Font, normalBrush, nkNameRc, Sf);
            }
            Size MsgFont = TextRenderer.MeasureText(subItem.PersonalMsg, this.Font);
            Rectangle MsgRc = new Rectangle(new Point(rectSubItem.Height, rectSubItem.Top + 11 + this.Font.Height), new Size(this.Width - rectSubItem.Height, MsgFont.Height));
            //绘制个人签名
            g.DrawString(subItem.PersonalMsg, this.Font, Brushes.Gray, MsgRc, Sf);
        }
        #endregion

        #region 绘制小图标模式的个人信息(DrawSmallSubItem)
        /// <summary>
        /// 绘制小图标模式的个人信息
        /// </summary>
        /// <param name="g">绘图表面</param>
        /// <param name="subItem">要绘制信息的子项</param>
        /// <param name="rectSubItem">该子项的区域</param>
        protected virtual void DrawSmallSubItem(Graphics g, ChatListSubItem subItem, Rectangle rectSubItem) {
            rectSubItem.Height = (int)ChatListItemIcon.Small;               //重新赋值一个高度
            StringFormat sf = new StringFormat();
            sf.LineAlignment = StringAlignment.Center;
            sf.FormatFlags = StringFormatFlags.NoWrap;
            string strDraw = subItem.DisplayName;
            SolidBrush normalBrush = new SolidBrush(this.ForeColor);
            if (subItem.IsVip) {
                normalBrush = new SolidBrush(VipFontColor);
            }
            if (string.IsNullOrEmpty(strDraw))
                strDraw = subItem.NicName;   //如果没有备注绘制昵称
            Size szFont = TextRenderer.MeasureText(strDraw, this.Font);
            sf.SetTabStops(0.0F, new float[] { rectSubItem.Height });
            g.DrawString("\t" + strDraw, this.Font, normalBrush, rectSubItem, sf);
            sf.SetTabStops(0.0F, new float[] { rectSubItem.Height + 5 + szFont.Width });
            g.DrawString("\t" + subItem.PersonalMsg, this.Font, Brushes.Gray, rectSubItem, sf);
        }
        #endregion

        #region 清除悬浮分组的悬浮样式(ClearItemMouseOn)
        private void ClearItemMouseOn() {
            if (m_mouseOnItem != null) {
                int z = 0;
                if (chatVScroll.ShouldBeDraw) {
                    z = 9;
                }
                this.Invalidate(new Rectangle(
                    m_mouseOnItem.Bounds.X, m_mouseOnItem.Bounds.Y - chatVScroll.Value,
                    m_mouseOnItem.Bounds.Width + z, m_mouseOnItem.Bounds.Height));
                m_mouseOnItem = null;
            }
        }
        #endregion

        #region 清除悬浮好友的悬浮样式(ClearSubItemMouseOn)
        private void ClearSubItemMouseOn() {
            if (m_mouseOnSubItem != null) {
                int z = 0;
                if (chatVScroll.ShouldBeDraw) {
                    z = 9;
                }
                this.Invalidate(new Rectangle(
                    m_mouseOnSubItem.Bounds.X, m_mouseOnSubItem.Bounds.Y - chatVScroll.Value,
                    m_mouseOnSubItem.Bounds.Width + z, m_mouseOnSubItem.Bounds.Height));
                m_mouseOnSubItem = null;
            }
        }
        #endregion

        #region 根据id返回一组列表子项(GetSubItemsById)
        /// <summary>
        /// 根据id返回一组列表子项
        /// </summary>
        /// <param name="userId">要返回的id</param>
        /// <returns>列表子项的数组</returns>
        public ChatListSubItem[] GetSubItemsById(uint userId) {
            List<ChatListSubItem> subItems = new List<ChatListSubItem>();
            for (int i = 0, lenI = this.items.Count; i < lenI; i++) {
                for (int j = 0, lenJ = items[i].SubItems.Count; j < lenJ; j++) {
                    if (userId == items[i].SubItems[j].ID) {
                        subItems.Add(items[i].SubItems[j]);
                        break;
                    }
                }
            }
            return subItems.ToArray();
        }
        #endregion

        #region 根据昵称返回一组列表子项(GetSubItemsByNicName)
        /// <summary>
        /// 根据昵称返回一组列表子项
        /// </summary>
        /// <param name="nicName">要返回的昵称</param>
        /// <returns>列表子项的数组</returns>
        public ChatListSubItem[] GetSubItemsByNicName(string nicName) {
            List<ChatListSubItem> subItems = new List<ChatListSubItem>();
            for (int i = 0, lenI = this.items.Count; i < lenI; i++) {
                for (int j = 0, lenJ = items[i].SubItems.Count; j < lenJ; j++) {
                    if (nicName == items[i].SubItems[j].NicName)
                        subItems.Add(items[i].SubItems[j]);
                }
            }
            return subItems.ToArray();
        }
        #endregion

        #region 根据备注名称返回一组列表子项(GetSubItemsByDisplayName)
        /// <summary>
        /// 根据备注名称返回一组列表子项
        /// </summary>
        /// <param name="displayName">要返回的备注名称</param>
        /// <returns>列表子项的数组</returns>
        public ChatListSubItem[] GetSubItemsByDisplayName(string displayName) {
            List<ChatListSubItem> subItems = new List<ChatListSubItem>();
            for (int i = 0, lenI = this.items.Count; i < lenI; i++) {
                for (int j = 0, lenJ = items[i].SubItems.Count; j < lenJ; j++) {
                    if (displayName == items[i].SubItems[j].DisplayName)
                        subItems.Add(items[i].SubItems[j]);
                }
            }
            return subItems.ToArray();
        }
        #endregion

        #region 根据IP返回一组列表子项(GetSubItemsByIp)
        /// <summary>
        /// 根据IP返回一组列表子项
        /// </summary>
        /// <param name="Ip">要返回的Ip</param>
        /// <returns>列表子项的数组</returns>
        public ChatListSubItem[] GetSubItemsByIp(string Ip) {
            List<ChatListSubItem> subItems = new List<ChatListSubItem>();
            for (int i = 0, lenI = this.items.Count; i < lenI; i++) {
                for (int j = 0, lenJ = items[i].SubItems.Count; j < lenJ; j++) {
                    if (Ip == items[i].SubItems[j].IpAddress)
                        subItems.Add(items[i].SubItems[j]);
                }
            }
            return subItems.ToArray();
        }
        #endregion

        #region 根据文本搜索(GetSubItemsByText)
        /// <summary>
        /// 根据文本搜索
        /// </summary>
        /// <param name="nicName">要返回的昵称</param>
        /// <returns>列表子项的数组</returns>
        public ChatListSubItem[] GetSubItemsByText(string text) {
            List<ChatListSubItem> subItems = new List<ChatListSubItem>();

            for (int i = 0, lenI = this.items.Count; i < lenI; i++) {
                for (int j = 0, lenJ = items[i].SubItems.Count; j < lenJ; j++) {
                    //昵称和备注名称
                    if (items[i].SubItems[j].NicName != null
                        && items[i].SubItems[j].NicName.ToLower().Contains(text)) {
                        subItems.Add(items[i].SubItems[j]);
                    } else if (items[i].SubItems[j].DisplayName != null
                          && items[i].SubItems[j].DisplayName.ToLower().Contains(text)) {
                        subItems.Add(items[i].SubItems[j]);
                    }

                }
            }
            return subItems.ToArray();
        }
        #endregion

        #region 记录当前打开的组与关闭当前的组
        List<ChatListItem> listHadOpenGroup;
        /// <summary>
        /// 记录当前打开的组 便于点击时候恢复
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<ChatListItem> ListHadOpenGroup {
            get { return listHadOpenGroup; }
            set { listHadOpenGroup = value; }
        }
        public void CollapseAll() {
            foreach (ChatListItem item in items) {
                item.IsOpen = false;
            }
            this.Invalidate();
        }

        public void ExpandAll() {
            foreach (ChatListItem item in items) {
                item.IsOpen = true;
            }
            this.Invalidate();
        }

        /// <summary>
        /// 恢复到上次打开的用户组
        /// </summary>
        public void Regain() {
            if (listHadOpenGroup != null && listHadOpenGroup.Count > 0) {
                foreach (var item in listHadOpenGroup) {
                    item.IsOpen = true;
                }
            }
        }
        #endregion

        #region 平滑滚动计时器
        /// <summary>
        /// 平滑滚动速度
        /// </summary>
        int scrollSpeed = 0;
        private void scorllTimer_Tick(object sender, EventArgs e) {
            if (scrollSpeed > RollSize)//限制速度
            {
                scrollSpeed = RollSize;
            }
            if (scrollSpeed < -RollSize) {
                scrollSpeed = -RollSize;
            }

            if (scrollSpeed > 1)//平滑速度
            {
                scrollSpeed -= 1;
            } else if (scrollSpeed < -1) {
                scrollSpeed += 1;
            } else {
                scrollSpeed = 0;
            }

            if (scrollSpeed != 0) {
                this.chatVScroll.Value += scrollSpeed;
                this.Invalidate();
            }
        }
        #endregion

        #region 资源释放
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            if (items.Count > 0) {
                foreach (ChatListItem item in items) {
                    item.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}
