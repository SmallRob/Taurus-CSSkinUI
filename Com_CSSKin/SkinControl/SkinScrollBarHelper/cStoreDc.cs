
namespace Com_CSSkin.SkinControl
{
    #region Directives
    using System;
    using System.Runtime.InteropServices;
    using System.Drawing;
    #endregion

    #region StoreDc
    [System.Security.Permissions.PermissionSet(System.Security.Permissions.SecurityAction.Demand, Name = "FullTrust")]
    public class cStoreDc
    {
        /// <summary>
        /// 函数功能：该函数通过使用指定的名字为一个设备创建设备上下文环境。 
        /// </summary>
        /// <param name="lpszDriver">指向一个以Null结尾的字符串的指针，该字符串为显示驱动指定DISPLAY或者指定一个打印驱动程序名，通常为WINSPOOL。 </param>
        /// <param name="lpszDevice">指向一个以null结尾的字符串的指针，该字符串指定了正在使用的特定输出设备的名字，它不是打印机模式名。LpszDevice参数必须被使用。</param>
        /// <param name="lpszOutput">该参数在32位应用中被忽略；并置为Null，它主要是为了提供与16位应用程序兼容，更多的信息参见下面的注释部分。 </param>
        /// <param name="lpInitData">指向包含设备驱动程序的设备指定初始化数据的DEVMODE结构的指针，DocumentProperties函数检索指定设备获取已填充的结构，如果设备驱动程序使用用户指定的缺省初始化值。则lplnitData参数必须为Null。 </param>
        /// <returns>返回值：成功，返回值是特定设备的设备上下文环境的句柄；失败，返回值为Null。</returns>
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateDCA([MarshalAs(UnmanagedType.LPStr)]string lpszDriver, [MarshalAs(UnmanagedType.LPStr)]string lpszDevice, [MarshalAs(UnmanagedType.LPStr)]string lpszOutput, int lpInitData);

        /// <summary>
        /// 函数功能:该函数通过使用指定的名字为一个设备创建设备上下文环境
        /// </summary>
        /// <param name="lpszDriver"></param>
        /// <param name="lpszDevice"></param>
        /// <param name="lpszOutput"></param>
        /// <param name="lpInitData"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateDCW([MarshalAs(UnmanagedType.LPWStr)]string lpszDriver, [MarshalAs(UnmanagedType.LPWStr)]string lpszDevice, [MarshalAs(UnmanagedType.LPWStr)]string lpszOutput, int lpInitData);

        /// <summary>
        /// 函数功能:该函数通过使用指定的名字为一个设备创建设备上下文环境
        /// </summary>
        /// <param name="lpszDriver"></param>
        /// <param name="lpszDevice"></param>
        /// <param name="lpszOutput"></param>
        /// <param name="lpInitData"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateDC(string lpszDriver, string lpszDevice, string lpszOutput, int lpInitData);

        /// <summary>
        /// 该函数创建一个与指定设备兼容的内存设备上下文环境（DC）。通过GetDc()获取的HDC直接与相关设备沟通，而本函数创建的DC，则是与内存中的一个表面相关联。
        /// </summary>
        /// <param name="hdc"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        /// <summary>
        /// 函数功能:该函数创建与指定的设备环境相关的设备兼容的位图
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="nWidth"></param>
        /// <param name="nHeight"></param>
        /// <returns></returns>
        [DllImport("gdi32.dll")]
        private static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteDC(IntPtr hdc);

        [DllImport("gdi32.dll", PreserveSig = true)]
        private static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DeleteObject(IntPtr hObject);

        private int _iHeight = 0;
        private int _iWidth = 0;
        private IntPtr _pHdc = IntPtr.Zero;
        private IntPtr _pBmp = IntPtr.Zero;
        private IntPtr _pBmpOld = IntPtr.Zero;

        public IntPtr Hdc
        {
            get { return _pHdc; }
        }

        public IntPtr HBmp
        {
            get { return _pBmp; }
        }

        public int Height
        {
            get { return _iHeight; }
            set
            {
                if (_iHeight != value)
                {
                    _iHeight = value;
                    ImageCreate(_iWidth, _iHeight);
                }
            }
        }

        public int Width
        {
            get { return _iWidth; }
            set
            {
                if (_iWidth != value)
                {
                    _iWidth = value;
                    ImageCreate(_iWidth, _iHeight);
                }
            }
        }

        private void ImageCreate(int width, int height)
        {
            IntPtr pHdc = IntPtr.Zero;

            ImageDestroy();
            pHdc = CreateDCA("DISPLAY", "", "", 0);
            _pHdc = CreateCompatibleDC(pHdc);
            _pBmp = CreateCompatibleBitmap(pHdc, _iWidth, _iHeight);
            _pBmpOld = SelectObject(_pHdc, _pBmp);
            if (_pBmpOld == IntPtr.Zero)
            {
                ImageDestroy();
            }
            else
            {
                _iWidth = width;
                _iHeight = height;
            }
            DeleteDC(pHdc);
            pHdc = IntPtr.Zero;
        }

        private void ImageDestroy()
        {
            if (_pBmpOld != IntPtr.Zero)
            {
                SelectObject(_pHdc, _pBmpOld);
                _pBmpOld = IntPtr.Zero;
            }
            if (_pBmp != IntPtr.Zero)
            {
                DeleteObject(_pBmp);
                _pBmp = IntPtr.Zero;
            }
            if (_pHdc != IntPtr.Zero)
            {
                DeleteDC(_pHdc);
                _pHdc = IntPtr.Zero;
            }
        }

        public void Dispose()
        {
            ImageDestroy();
        }
    }
    #endregion
}
