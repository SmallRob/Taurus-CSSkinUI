
namespace Com_CSSkin.SkinClass
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;

    public class FastBitmap : IDisposable, ICloneable
    {
        private System.Drawing.Bitmap bmp;
        internal BitmapData bmpData;

        private FastBitmap() {
        }

        public FastBitmap(System.Drawing.Bitmap bitmap) {
            this.bmp = bitmap;
        }

        public FastBitmap(int width, int height, PixelFormat format) {
            this.bmp = new System.Drawing.Bitmap(width, height, format);
        }

        public object Clone() {
            return new FastBitmap { bmp = (System.Drawing.Bitmap)this.bmp.Clone() };
        }

        public void Dispose() {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing) {
            this.Unlock();
            if (disposing) {
                this.bmp.Dispose();
            }
        }

        ~FastBitmap() {
            this.Dispose(false);
        }

        public byte GetIntensity(int x, int y) {
            Color pixel = this.GetPixel(x, y);
            return (byte)((((pixel.R * 0.3) + (pixel.G * 0.59)) + (pixel.B * 0.11)) + 0.5);
        }

        public unsafe Color GetPixel(int x, int y) {
            if (this.bmpData.PixelFormat == PixelFormat.Format32bppArgb) {
                byte* numPtr = (byte*)((((int)this.bmpData.Scan0) + (y * this.bmpData.Stride)) + (x * 4));
                return Color.FromArgb(numPtr[3], numPtr[2], numPtr[1], numPtr[0]);
            }
            if (this.bmpData.PixelFormat == PixelFormat.Format24bppRgb) {
                byte* numPtr2 = (byte*)((((int)this.bmpData.Scan0) + (y * this.bmpData.Stride)) + (x * 3));
                return Color.FromArgb(numPtr2[2], numPtr2[1], numPtr2[0]);
            }
            return Color.Empty;
        }

        public void Lock() {
            this.bmpData = this.bmp.LockBits(new Rectangle(0, 0, this.bmp.Width, this.bmp.Height), ImageLockMode.ReadWrite, this.bmp.PixelFormat);
        }

        public void Save(string filename) {
            this.bmp.Save(filename);
        }

        public void Save(string filename, ImageFormat format) {
            this.bmp.Save(filename, format);
        }

        public unsafe void SetPixel(int x, int y, Color c) {
            if (this.bmpData.PixelFormat == PixelFormat.Format32bppArgb) {
                byte* numPtr = (byte*)((((int)this.bmpData.Scan0) + (y * this.bmpData.Stride)) + (x * 4));
                numPtr[0] = c.B;
                numPtr[1] = c.G;
                numPtr[2] = c.R;
                numPtr[3] = c.A;
            }
            if (this.bmpData.PixelFormat == PixelFormat.Format24bppRgb) {
                byte* numPtr2 = (byte*)((((int)this.bmpData.Scan0) + (y * this.bmpData.Stride)) + (x * 3));
                numPtr2[0] = c.B;
                numPtr2[1] = c.G;
                numPtr2[2] = c.R;
            }
        }

        public void Unlock() {
            if (this.bmpData != null) {
                this.bmp.UnlockBits(this.bmpData);
                this.bmpData = null;
            }
        }

        public System.Drawing.Bitmap Bitmap {
            get {
                return this.bmp;
            }
            set {
                if (value != null) {
                    this.bmp = value;
                }
            }
        }

        public int Height {
            get {
                return this.bmp.Height;
            }
        }

        public int Width {
            get {
                return this.bmp.Width;
            }
        }
    }
}

