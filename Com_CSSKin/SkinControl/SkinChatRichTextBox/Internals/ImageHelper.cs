
namespace Com_CSSkin.SkinControl
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;

    public static class ImageHelper
    {
        private static float[][] ColorMatrix = null;

        static ImageHelper() {
            float[][] numArray = new float[5][];
            numArray[0] = new float[] { 0.299f, 0.299f, 0.299f, 0f, 0f };
            numArray[1] = new float[] { 0.587f, 0.587f, 0.587f, 0f, 0f };
            numArray[2] = new float[] { 0.114f, 0.114f, 0.114f, 0f, 0f };
            float[] numArray2 = new float[5];
            numArray2[3] = 1f;
            numArray[3] = numArray2;
            numArray2 = new float[5];
            numArray2[4] = 1f;
            numArray[4] = numArray2;
            ColorMatrix = numArray;
        }

        public static Image Convert(byte[] buff) {
            MemoryStream stream = new MemoryStream(buff);
            Image image = Image.FromStream(stream);
            stream.Close();
            return image;
        }

        public static byte[] Convert(Image img) {
            MemoryStream stream = new MemoryStream();
            img.Save(stream, ImageFormat.Jpeg);
            byte[] buffer = stream.ToArray();
            stream.Close();
            return buffer;
        }

        public static Bitmap ConvertToGrey(Image origin) {
            Bitmap image = new Bitmap(origin);
            Graphics graphics = Graphics.FromImage(image);
            ImageAttributes imageAttr = new ImageAttributes();
            System.Drawing.Imaging.ColorMatrix newColorMatrix = new System.Drawing.Imaging.ColorMatrix(ColorMatrix);
            imageAttr.SetColorMatrix(newColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            graphics.DrawImage(image, new Rectangle(0, 0, image.Width, image.Height), 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, imageAttr);
            graphics.Dispose();
            return image;
        }

        public static Icon ConvertToIcon(Image img, int iconLength) {
            using (Bitmap bitmap = new Bitmap(img, new Size(iconLength, iconLength))) {
                return Icon.FromHandle(bitmap.GetHicon());
            }
        }

        public static Image ConvertToJPG(Image img) {
            MemoryStream stream = new MemoryStream();
            img.Save(stream, ImageFormat.Jpeg);
            Image image = Image.FromStream(stream);
            stream.Close();
            return image;
        }

        public static bool IsGif(Image img) {
            FrameDimension dimension = new FrameDimension(img.FrameDimensionsList[0]);
            return (img.GetFrameCount(dimension) > 1);
        }

        public static void Save(Image img, string path, ImageFormat format) {
            if ((img != null) && (path != null)) {
                Bitmap image = new Bitmap(img.Width, img.Height, img.PixelFormat);
                Graphics graphics = Graphics.FromImage(image);
                graphics.DrawImage(img, 0, 0, img.Width, img.Height);
                graphics.Dispose();
                image.Save(path, format);
            }
        }
    }
}

