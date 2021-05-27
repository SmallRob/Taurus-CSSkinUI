
using System;
using System.Collections.Generic;
using System.Text;

namespace Com_CSSkin.SkinControl
{
    internal class FileTransfersItemText : IFileTransfersItemText
    {
        #region IFileTransfersItemText 成员

        public string Save
        {
            get { return "接收"; }
        }

        public string SaveTo
        {
            get { return "另存为..."; }
        }

        public string RefuseReceive
        {
            get { return "拒绝"; }
        }

        public string CancelTransfers
        {
            get { return "取消"; }
        }

        #endregion
    }
}
