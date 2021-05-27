
namespace Com_CSSkin.SkinControl
{
    #region Directives
    using System;
    using System.Windows.Forms;
    using System.Runtime.InteropServices;
    using System.Drawing;
    #endregion

    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class cTreeView : IDisposable
    {
        #region Fields
        private IntPtr _hTreeviewWnd = IntPtr.Zero;
        private ScrollBarHelper _cInternalScroll;
        #endregion

        #region Constructor
        public cTreeView(IntPtr handle, Bitmap hztrack, Bitmap hzarrow, Bitmap hzthumb, Bitmap vttrack, Bitmap vtarrow, Bitmap vtthumb, Bitmap fader)
        {
            if (handle == IntPtr.Zero)
                throw new Exception("The treeview handle is invalid.");
            _hTreeviewWnd = handle;

            if (hztrack != null && hzarrow != null && hzthumb != null && vttrack != null && vtarrow != null && vtthumb != null)
                _cInternalScroll = new ScrollBarHelper(_hTreeviewWnd, hztrack, hzarrow, hzthumb, vttrack, vtarrow, vtthumb, fader);
            else
                throw new Exception("The treeview image(s) are invalid");
        }

        public void Dispose()
        {
            try
            {
                if (_cInternalScroll != null) _cInternalScroll.Dispose();
            }
            catch { }
        }
        #endregion
    }
}
