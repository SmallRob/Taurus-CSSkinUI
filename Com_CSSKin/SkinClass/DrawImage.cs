
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace Com_CSSkin.SkinClass
{
    public class DrawImage
    {
        /// <summary>
        /// 将指向图像按指定的填充模式绘制到目标图像上
        /// </summary>
        /// <param name="SourceBmp">要控制填充模式的源图</param>
        /// <param name="TargetBmp">要绘制到的目标图</param>
        /// <param name="_FillMode">填充模式</param>
        /// <remarks></remarks>
        public static void ImageFillRect(Bitmap SourceBmp, Bitmap TargetBmp, ImageLayout _FillMode) {
            try {
                switch (_FillMode) {
                    case ImageLayout.Tile:
                        using (TextureBrush Txbrus = new TextureBrush(SourceBmp)) {
                            Txbrus.WrapMode = WrapMode.Tile;
                            using (Graphics G = Graphics.FromImage(TargetBmp)) {
                                G.FillRectangle(Txbrus, new Rectangle(0, 0, TargetBmp.Width - 1, TargetBmp.Height - 1));
                            }
                        }

                        break;
                    case ImageLayout.Center:
                        using (Graphics G = Graphics.FromImage(TargetBmp)) {
                            int xx = (TargetBmp.Width - SourceBmp.Width) / 2;
                            int yy = (TargetBmp.Height - SourceBmp.Height) / 2;
                            G.DrawImage(SourceBmp, new Rectangle(xx, yy, SourceBmp.Width, SourceBmp.Height), new Rectangle(0, 0, SourceBmp.Width, SourceBmp.Height), GraphicsUnit.Pixel);
                        }

                        break;
                    case ImageLayout.Stretch:
                        using (Graphics G = Graphics.FromImage(TargetBmp)) {
                            G.DrawImage(SourceBmp, new Rectangle(0, 0, TargetBmp.Width, TargetBmp.Height), new Rectangle(0, 0, SourceBmp.Width, SourceBmp.Height), GraphicsUnit.Pixel);
                        }

                        break;
                    case ImageLayout.Zoom:
                        double tm = 0.0;
                        int W = SourceBmp.Width;
                        int H = SourceBmp.Height;
                        if (W > TargetBmp.Width) {
                            tm = TargetBmp.Width / SourceBmp.Width;
                            W = Convert.ToInt32(W * tm);
                            H = Convert.ToInt32(H * tm);
                        }
                        if (H > TargetBmp.Height) {
                            tm = TargetBmp.Height / H;
                            W = Convert.ToInt32(W * tm);
                            H = Convert.ToInt32(H * tm);
                        }
                        using (Bitmap tmpBP = new Bitmap(W, H)) {
                            using (Graphics G2 = Graphics.FromImage(tmpBP)) {
                                G2.DrawImage(SourceBmp, new Rectangle(0, 0, W, H), new Rectangle(0, 0, SourceBmp.Width, SourceBmp.Height), GraphicsUnit.Pixel);
                                using (Graphics G = Graphics.FromImage(TargetBmp)) {
                                    int xx = (TargetBmp.Width - W) / 2;
                                    int yy = (TargetBmp.Height - H) / 2;
                                    G.DrawImage(tmpBP, new Rectangle(xx, yy, W, H), new Rectangle(0, 0, W, H), GraphicsUnit.Pixel);
                                }
                            }
                        }

                        break;
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
