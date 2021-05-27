
namespace Com_CSSkin.SkinControl
{
    #region Directives
    using System;
    using System.Runtime.InteropServices;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    #endregion

    #region Enums
    public enum StretchModeEnum : int
    {
        STRETCH_ANDSCANS = 1,
        STRETCH_ORSCANS = 2,
        STRETCH_DELETESCANS = 3,
        STRETCH_HALFTONE = 4,
    }
    #endregion

    #region GraphicsMode
    public class GraphicsMode : IDisposable
    {
        #region Instance Fields
        private Graphics _cGraphicCopy;
        private SmoothingMode _eOldMode;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the class.
        /// </summary>
        /// <param name="g">Graphics instance.</param>
        /// <param name="mode">Desired Smoothing mode.</param>
        public GraphicsMode(Graphics g, SmoothingMode mode)
        {
            _cGraphicCopy = g;
            _eOldMode = _cGraphicCopy.SmoothingMode;
            _cGraphicCopy.SmoothingMode = mode;
        }

        /// <summary>
        /// Revert the SmoothingMode to original setting.
        /// </summary>
        public void Dispose()
        {
            _cGraphicCopy.SmoothingMode = _eOldMode;
        }
        #endregion
    }
    #endregion

    #region StretchMode
    public class StretchMode : IDisposable
    {
        #region API
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetStretchBltMode(IntPtr hdc, StretchModeEnum iStretchMode);

        [DllImport("gdi32.dll")]
        private static extern int GetStretchBltMode(IntPtr hdc);
        #endregion

        #region Fields
        private StretchModeEnum _eOldMode = StretchModeEnum.STRETCH_ANDSCANS;
        private IntPtr _pHdc = IntPtr.Zero;
        #endregion

        public StretchMode(IntPtr hdc, StretchModeEnum mode)
        {
            _eOldMode = (StretchModeEnum)GetStretchBltMode(hdc);
            _pHdc = hdc;
            SetStretchBltMode(hdc, mode);
        }

        public void Dispose()
        {
            SetStretchBltMode(_pHdc, _eOldMode);
        }
    }
    #endregion

    #region StretchImage
    public class StretchImage : IDisposable
    {
        #region API
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetStretchBltMode(IntPtr hdc, StretchModeEnum eStretchMode);

        [DllImport("gdi32.dll")]
        private static extern int GetStretchBltMode(IntPtr hdc);

        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool StretchBlt(IntPtr hDest, int X, int Y, int nWidth, int nHeight, IntPtr hdcSrc,
        int sX, int sY, int nWidthSrc, int nHeightSrc, int dwRop);
        #endregion

        #region Fields
        private StretchModeEnum _eOldMode = StretchModeEnum.STRETCH_ANDSCANS;
        private IntPtr _pHdc = IntPtr.Zero;
        #endregion

        public StretchImage(IntPtr sourceDc, IntPtr destDc, Rectangle src, Rectangle dest, int depth, StretchModeEnum eStretchMode)
        {
            _eOldMode = (StretchModeEnum)GetStretchBltMode(sourceDc);
            _pHdc = sourceDc;
            SetStretchBltMode(sourceDc, eStretchMode);

            // left
            StretchBlt(destDc, dest.Left, dest.Top, depth, dest.Height, sourceDc, src.Left, 0, depth, src.Height, 0xCC0020);
            // right
            StretchBlt(destDc, dest.Right - depth, dest.Top, depth, dest.Height, sourceDc, src.Right - depth, 0, depth, src.Height, 0xCC0020);
            // top
            StretchBlt(destDc, dest.Left + depth, dest.Top, dest.Width - (2 * depth), depth, sourceDc, src.Left + depth, 0, src.Width - (2 * depth), depth, 0xCC0020);
            // bottom
            StretchBlt(destDc, dest.Left + depth, dest.Bottom - depth, dest.Width - (2 * depth), depth, sourceDc, src.Left + depth, src.Bottom - depth, src.Width - (2 * depth), depth, 0xCC0020);
            // center
            StretchBlt(destDc, dest.Left + depth, dest.Top + depth, dest.Width - (2 * depth), dest.Height - (2 * depth), sourceDc, src.Left + depth, depth, src.Width - (2 * depth), src.Height - (2 * depth), 0xCC0020);
        }

        public void Dispose()
        {
            SetStretchBltMode(_pHdc, _eOldMode);
        }
    }
    #endregion

    #region AlphaStretch
    public class AlphaStretch : IDisposable
    {
        private const byte AC_SRC_OVER = 0x00;
        private const byte AC_SRC_ALPHA = 0x01;

        [StructLayout(LayoutKind.Sequential)]
        private struct BLENDFUNCTION
        {
            byte BlendOp;
            byte BlendFlags;
            byte SourceConstantAlpha;
            byte AlphaFormat;

            internal BLENDFUNCTION(byte op, byte flags, byte alpha, byte format)
            {
                BlendOp = op;
                BlendFlags = flags;
                SourceConstantAlpha = alpha;
                AlphaFormat = format;
            }
        }

        [DllImport("gdi32.dll", EntryPoint = "GdiAlphaBlend")]
        private static extern bool AlphaBlend(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest,
        IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, BLENDFUNCTION blendFunction);

        public AlphaStretch(IntPtr sourceDc, IntPtr destDc, Rectangle src, Rectangle dest, int depth, byte opacity)
        {
            BLENDFUNCTION bf = new BLENDFUNCTION(AC_SRC_OVER, 0x0, opacity, 0x0);
            // left
            AlphaBlend(destDc, dest.Left, dest.Top, depth, dest.Height, sourceDc, src.Left, 0, depth, src.Height, bf);
            // right
            AlphaBlend(destDc, dest.Right - depth, dest.Top, depth, dest.Height, sourceDc, src.Right - depth, 0, depth, src.Height, bf);
            // top
            AlphaBlend(destDc, dest.Left + depth, dest.Top, dest.Width - (2 * depth), depth, sourceDc, src.Left + depth, 0, src.Width - (2 * depth), depth, bf);
            // bottom
            AlphaBlend(destDc, dest.Left + depth, dest.Bottom - depth, dest.Width - (2 * depth), depth, sourceDc, src.Left + depth, src.Bottom - depth, src.Width - (2 * depth), depth, bf);
            // center
            AlphaBlend(destDc, dest.Left + depth, dest.Top + depth, dest.Width - (2 * depth), dest.Height - (2 * depth), sourceDc, src.Left + depth, depth, src.Width - (2 * depth), src.Height - (2 * depth), bf);
        }

        public void Dispose()
        {
            //
        }
    }
    #endregion
}
