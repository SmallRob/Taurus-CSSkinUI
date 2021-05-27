
using System;
using System.Drawing;
using System.Security.Permissions;
using System.Drawing.Imaging;

namespace Com_CSSkin
{
    public static class BitmapHelper
    {
        [SecurityPermission(SecurityAction.LinkDemand,
            Flags = SecurityPermissionFlag.UnmanagedCode)]
        public static unsafe Color GetImageAverageColor(Bitmap bitmap)
        {
            if (bitmap == null)
            {
                throw new ArgumentNullException("bitmap");
            }

            int width = bitmap.Width;
            int height = bitmap.Height;
            Rectangle rect = new Rectangle(0, 0, width, height);

            try
            {
                BitmapData bitmapData = bitmap.LockBits(
                    rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                byte* scan0 = (byte*)bitmapData.Scan0;
                int strideOffset = bitmapData.Stride - bitmapData.Width * 4;

                int sum = width * height;

                int a = 0;
                int r = 0;
                int g = 0;
                int b = 0;

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        b += *scan0++;
                        g += *scan0++;
                        r += *scan0++;
                        a += *scan0++;
                    }
                    scan0 += strideOffset;
                }

                bitmap.UnlockBits(bitmapData);

                a /= sum;
                r /= sum;
                g /= sum;
                b /= sum;

                return Color.FromArgb(255, r, g, b);
            }
            catch
            {
                return Color.FromArgb(127, 127, 127);
            }
        }
    }
}
