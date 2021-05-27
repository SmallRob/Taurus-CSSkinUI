
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace Com_CSSkin.SkinControl
{
    //有待解决
    //[TypeConverter(typeof(ExpandableObjectConverter))]
    public class ChatListSubItem : IComparable, IDisposable
    {
        /// <summary>
        /// 深拷贝
        /// </summary>
        /// <returns>深度克隆的ChatListSubItem</returns>
        public ChatListSubItem Clone() {
            ChatListSubItem SubItem = new ChatListSubItem();
            SubItem.Bounds = this.Bounds;
            SubItem.DisplayName = this.DisplayName;
            SubItem.HeadImage = this.HeadImage;
            SubItem.HeadRect = this.HeadRect;
            SubItem.ID = this.ID;
            SubItem.IpAddress = this.IpAddress;
            SubItem.IsTwinkle = this.IsTwinkle;
            SubItem.IsTwinkleHide = this.isTwinkleHide;
            SubItem.NicName = this.NicName;
            SubItem.OwnerListItem = this.OwnerListItem.Clone();
            SubItem.PersonalMsg = this.PersonalMsg;
            SubItem.Status = this.Status;
            SubItem.TcpPort = this.TcpPort;
            SubItem.UpdPort = this.UpdPort;
            SubItem.Tag = this.Tag;
            SubItem.PlatformTypes = this.PlatformTypes;
            SubItem.IsVip = this.IsVip;
            SubItem.QQShow = this.QQShow;
            return SubItem;
        }

        private uint id;
        /// <summary>
        /// 获取或者设置用户账号
        /// </summary>
        public uint ID {
            get { return id; }
            set { id = value; }
        }

        private object tag;
        /// <summary>
        /// 与对象关联的用户定义数据
        /// </summary>
        public object Tag {
            get { return tag; }
            set { tag = value; }
        }

        private PlatformType platformTypes;
        /// <summary>
        /// 获取或设置用户登录平台
        /// </summary>
        public PlatformType PlatformTypes {
            get { return platformTypes; }
            set {
                if (platformTypes == value) return;
                platformTypes = value;
                RedrawSubItem();
            }
        }

        private string nicName;
        /// <summary>
        /// 获取或者设置用户昵称
        /// </summary>
        public string NicName {
            get { return nicName; }
            set {
                nicName = value;
                RedrawSubItem();
            }
        }

        private string displayName;
        /// <summary>
        /// 获取或者设置用户备注名称
        /// </summary>
        public string DisplayName {
            get { return displayName; }
            set { displayName = value; RedrawSubItem(); }
        }

        private string personalMsg;
        /// <summary>
        /// 获取或者设置用户签名信息
        /// </summary>
        public string PersonalMsg {
            get { return personalMsg; }
            set { personalMsg = value; RedrawSubItem(); }
        }

        private Image qqShow;
        /// <summary>
        /// 获取或设置用户QQ秀
        /// </summary>
        public Image QQShow {
            get { return qqShow; }
            set { qqShow = value; }
        }

        private string ipAddress;
        /// <summary>
        /// 获取或者设置用户IP地址
        /// </summary>
        public string IpAddress {
            get { return ipAddress; }
            set {
                if (CheckIpAddress(value)) {
                    ipAddress = value;
                }
            }
        }

        private int updPort;
        /// <summary>
        /// 获取或者设置用户Upd端口
        /// </summary>
        public int UpdPort {
            get { return updPort; }
            set { updPort = value; }
        }

        private int tcpPort;
        /// <summary>
        /// 获取或者设置用户Tcp端口
        /// </summary>
        public int TcpPort {
            get { return tcpPort; }
            set { tcpPort = value; }
        }

        private Image headImage;
        /// <summary>
        /// 获取或者设置用户头像
        /// </summary>
        public Image HeadImage {
            get { return headImage; }
            set { headImage = value; RedrawSubItem(); }
        }

        private UserStatus status;
        /// <summary>
        /// 获取或者设置用户当前状态
        /// </summary>
        public UserStatus Status {
            get { return status; }
            set {
                if (status == value) return;
                status = value;
                if (this.ownerListItem != null)
                    this.ownerListItem.SubItems.Sort();
            }
        }
        private bool isVip;
        /// <summary>
        /// 是否是VIP
        /// </summary>
        public bool IsVip {
            get { return isVip; }
            set { isVip = value; }
        }
        private bool isTwinkle;
        /// <summary>
        /// 获取或者设置是否闪动
        /// </summary>
        public bool IsTwinkle {
            get { return isTwinkle; }
            set {
                if (isTwinkle == value) return;
                if (this.ownerListItem == null) return;
                isTwinkle = value;
                if (isTwinkle)
                    this.ownerListItem.TwinkleSubItemNumber++;
                else
                    this.ownerListItem.TwinkleSubItemNumber--;
                this.ownerListItem.OwnerChatListBox.Invalidate(this.bounds);
            }
        }

        private bool isTwinkleHide;
        public bool IsTwinkleHide {
            get { return isTwinkleHide; }
            set { isTwinkleHide = value; }
        }

        private Rectangle bounds;
        /// <summary>
        /// 获取列表子项显示区域
        /// </summary>
        [Browsable(false)]
        public Rectangle Bounds {
            get { return bounds; }
            set { bounds = value; }
        }

        private Rectangle headRect;
        /// <summary>
        /// 获取头像显示区域
        /// </summary>
        [Browsable(false)]
        public Rectangle HeadRect {
            get { return headRect; }
            set { headRect = value; }
        }

        private ChatListItem ownerListItem;
        /// <summary>
        /// 获取当前列表子项所在的列表项
        /// </summary>
        [Browsable(false)]
        public ChatListItem OwnerListItem {
            get { return ownerListItem; }
            set { ownerListItem = value; }
        }

        private void RedrawSubItem() {
            if (this.ownerListItem != null)
                if (this.ownerListItem.OwnerChatListBox != null)
                    this.ownerListItem.OwnerChatListBox.Invalidate(this.bounds);
        }
        /// <summary>
        /// 获取当前用户的黑白头像
        /// </summary>
        /// <returns>黑白头像</returns>
        public Bitmap GetDarkImage() {
            Bitmap b = new Bitmap(headImage);
            Bitmap bmp = b.Clone(new Rectangle(0, 0, headImage.Width, headImage.Height), PixelFormat.Format24bppRgb);
            b.Dispose();
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            byte[] byColorInfo = new byte[bmp.Height * bmpData.Stride];
            Marshal.Copy(bmpData.Scan0, byColorInfo, 0, byColorInfo.Length);
            for (int x = 0, xLen = bmp.Width; x < xLen; x++) {
                for (int y = 0, yLen = bmp.Height; y < yLen; y++) {
                    byColorInfo[y * bmpData.Stride + x * 3] =
                        byColorInfo[y * bmpData.Stride + x * 3 + 1] =
                        byColorInfo[y * bmpData.Stride + x * 3 + 2] =
                        GetAvg(
                        byColorInfo[y * bmpData.Stride + x * 3],
                        byColorInfo[y * bmpData.Stride + x * 3 + 1],
                        byColorInfo[y * bmpData.Stride + x * 3 + 2]);
                }
            }
            Marshal.Copy(byColorInfo, 0, bmpData.Scan0, byColorInfo.Length);
            bmp.UnlockBits(bmpData);
            return bmp;
        }

        private byte GetAvg(byte b, byte g, byte r) {
            return (byte)((r + g + b) / 3);
        }

        private bool CheckIpAddress(string str) {
            if (null == str) {
                return false;
            }
            string[] strIp = str.Split('.');
            if (strIp.Length != 4)
                return false;
            for (int i = 0; i < 4; i++) {
                try {
                    if (Convert.ToInt32(str[i]) > 255)
                        return false;
                } catch (FormatException) {
                    return false;
                }
            }
            return true;
        }
        //实现排序接口
        int IComparable.CompareTo(object obj) {
            if (!(obj is ChatListSubItem))
                throw new NotImplementedException("obj is not ChatListSubItem");
            ChatListSubItem subItem = obj as ChatListSubItem;
            return (this.status).CompareTo(subItem.status);
        }

        public ChatListSubItem() {
            this.status = UserStatus.Online;
            this.displayName = "displayName";
            this.nicName = "nicName";
            this.personalMsg = "Personal Message ...";
            this.IsVip = false;
            this.PlatformTypes = PlatformType.PC;
            this.HeadImage = Properties.Resources._1_100;
        }
        public ChatListSubItem(string nicname) {
            this.nicName = nicname;
        }
        public ChatListSubItem(string nicname, UserStatus status) {
            this.nicName = nicname;
            this.status = status;
        }
        public ChatListSubItem(string nicname, string displayname, string personalmsg) {
            this.nicName = nicname;
            this.displayName = displayname;
            this.personalMsg = personalmsg;
        }
        public ChatListSubItem(string nicname, string displayname, string personalmsg, UserStatus status) {
            this.nicName = nicname;
            this.displayName = displayname;
            this.personalMsg = personalmsg;
            this.status = status;
        }
        public ChatListSubItem(uint id, string nicname, string displayname, string personalmsg, UserStatus status, Bitmap head) {
            this.id = id;
            this.nicName = nicname;
            this.displayName = displayname;
            this.personalMsg = personalmsg;
            this.status = status;
            this.headImage = head;
        }
        public ChatListSubItem(uint id, string nicname, string displayname, string personalmsg, UserStatus status, PlatformType platformTypes, Bitmap head) {
            this.id = id;
            this.nicName = nicname;
            this.displayName = displayname;
            this.personalMsg = personalmsg;
            this.status = status;
            this.PlatformTypes = platformTypes;
            this.headImage = head;
        }
        public ChatListSubItem(uint id, string nicname, string displayname, string personalmsg, UserStatus status, PlatformType platformTypes, Bitmap head, bool isvip) {
            this.id = id;
            this.nicName = nicname;
            this.displayName = displayname;
            this.personalMsg = personalmsg;
            this.status = status;
            this.PlatformTypes = platformTypes;
            this.headImage = head;
            this.IsVip = isvip;
        }

        //在线状态
        public enum UserStatus
        {
            QMe = 1,
            Online = 2,
            Away = 3,
            Busy = 4,
            DontDisturb = 5,
            OffLine = 6   //貌似对于列表而言 没有隐身状态
        }

        #region 资源释放
        //是否回收完毕
        bool _disposed;
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~ChatListSubItem() {
            Dispose(false);
        }

        //这里的参数表示示是否需要释放那些实现IDisposable接口的托管对象
        protected virtual void Dispose(bool disposing) {
            if (_disposed) return; //如果已经被回收，就中断执行
            if (disposing) {
                //TODO:释放那些实现IDisposable接口的托管对象
                nicName = null;
                displayName = null;
                personalMsg = null;
                tag = null;
                qqShow = null;
                headImage = null;
                ownerListItem = null;

            }
            //TODO:释放非托管资源，设置对象为null

            _disposed = true;
        }
        #endregion
    }
}
