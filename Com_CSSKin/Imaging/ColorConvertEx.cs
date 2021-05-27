
using System;
using System.Drawing;

namespace Com_CSSkin.Imaging
{
    public sealed class ColorConverterEx
    {
        private static readonly int[] BT907 =
            new int[] { 2125, 7154, 721, 10000 };
        private static readonly int[] RMY =
            new int[] { 500, 419, 81, 1000 };
        private static readonly int[] Y =
            new int[] { 299, 587, 114, 1000 };

        private ColorConverterEx() { }

        public static HSL RgbToHsl(RGB rgb)
        {
            HSL hsl = new HSL();
            RgbToHsl(rgb, hsl);
            return hsl;
        }

        public static void RgbToHsl(RGB rgb, HSL hsl)
        {
            double r = (rgb.R / 255.0);
            double g = (rgb.G / 255.0);
            double b = (rgb.G / 255.0);

            double min = Math.Min(Math.Min(r, g), b);
            double max = Math.Max(Math.Max(r, g), b);
            double delta = max - min;

            hsl.Luminance = (max + min) / 2;

            if (delta == 0)
            {
                hsl.Hue = 0;
                hsl.Saturation = 0.0;
            }
            else
            {
                hsl.Saturation = (hsl.Luminance < 0.5) ? 
                    (delta / (max + min)) : (delta / (2 - max - min));

                double del_r = (((max - r) / 6) + (delta / 2)) / delta;
                double del_g = (((max - g) / 6) + (delta / 2)) / delta;
                double del_b = (((max - b) / 6) + (delta / 2)) / delta;
                double hue;

                if (r == max)
                {
                    hue = del_b - del_g;
                }
                else if (g == max)
                {
                    hue = (1.0 / 3) + del_r - del_b;
                }
                else
                {
                    hue = (2.0 / 3) + del_g - del_r;
                }

                if (hue < 0)
                {
                    hue += 1;
                }
                if (hue > 1)
                {
                    hue -= 1;
                }

                hsl.Hue = (int)(hue * 360);
            }
        }

        public static HSL ColorToHSL(Color color)
        {
            int hue;
            double luminance;
            double saturation;

            double r = (color.R / 255.0d);
            double g = (color.G / 255.0d);
            double b = (color.B / 255.0d);

            double min = Math.Min(Math.Min(r, g), b);
            double max = Math.Max(Math.Max(r, g), b);
            double delta = max - min;

            // get luminance value
            luminance = (max + min) / 2;

            if (delta == 0)
            {
                // gray color
                hue = 0;
                saturation = 0.0;
            }
            else
            {
                // get saturation value
                saturation = (luminance < 0.5) ?
                    (delta / (max + min)) : (delta / (2 - max - min));

                // get hue value
                double del_r = (((max - r) / 6) + (delta / 2)) / delta;
                double del_g = (((max - g) / 6) + (delta / 2)) / delta;
                double del_b = (((max - b) / 6) + (delta / 2)) / delta;
                double dHue;

                if (r == max)
                {
                    dHue = del_b - del_g;
                }
                else if (g == max)
                {
                    dHue = (1.0 / 3) + del_r - del_b;
                }
                else
                {
                    dHue = (2.0 / 3) + del_g - del_r;
                }

                // correct hue if needed
                if (dHue < 0)
                {
                    dHue += 1;
                }

                if (dHue > 1)
                {
                    dHue -= 1;
                }

                hue = (int)(dHue * 360);
            }

            return new HSL(hue, saturation, luminance);
        }

        public static Color HSLToColor(HSL hsl)
        {
            byte r;
            byte g;
            byte b;

            if (hsl.Saturation == 0)
            {
                // gray values
                r = g = b = (byte)(hsl.Luminance * 255);
            }
            else
            {
                double v1, v2;
                double hue = (double)hsl.Hue / 360;

                v2 = (hsl.Luminance < 0.5) ?
                    (hsl.Luminance * (1 + hsl.Saturation)) :
                    ((hsl.Luminance + hsl.Saturation) - (hsl.Luminance * hsl.Saturation));
                v1 = 2 * hsl.Luminance - v2;

                r = (byte)(255 * HueToRGB(v1, v2, hue + (1.0d / 3)));
                g = (byte)(255 * HueToRGB(v1, v2, hue));
                b = (byte)(255 * HueToRGB(v1, v2, hue - (1.0d / 3)));
            }

            return Color.FromArgb(r, g, b);
        }

        public static RGB HslToRgb(HSL hsl)
        {
            RGB rgb = new RGB();
            HslToRgb(hsl, rgb);
            return rgb;
        }

        public static void HslToRgb(HSL hsl, RGB rgb)
        {
            if (hsl.Saturation == 0)
            {
                rgb.R = rgb.G = rgb.B = (byte)(hsl.Luminance * 255);
            }
            else
            {
                double v1, v2;
                double hue = (double)hsl.Hue / 360;

                v2 = (hsl.Luminance < 0.5) ?
                    (hsl.Luminance * (1 + hsl.Saturation)) :
                    ((hsl.Luminance + hsl.Saturation) - (hsl.Luminance * hsl.Saturation));
                v1 = 2 * hsl.Luminance - v2;

                rgb.R = (byte)(255 * HueToRGB(v1, v2, hue + (1.0 / 3)));
                rgb.G = (byte)(255 * HueToRGB(v1, v2, hue));
                rgb.B = (byte)(255 * HueToRGB(v1, v2, hue - (1.0 / 3)));
            }
        }

        public static RGB RgbToGray(RGB source)
        {
            RGB dest = new RGB();
            RgbToGray(source, dest);
            return dest;
        }

        public static RGB RgbToGray(RGB source, GrayscaleStyle style)
        {
            RGB dest = new RGB();
            RgbToGray(source, dest, style);
            return dest;
        }

        public static void RgbToGray(RGB source, RGB dest)
        {
            RgbToGray(source, dest, GrayscaleStyle.BT907);
        }

        public static void RgbToGray(
            RGB source, RGB dest, GrayscaleStyle style)
        {
            byte gray = 127;
            switch (style)
            {
                case GrayscaleStyle.BT907:
                    gray = GetGray(source, BT907);
                    break;
                case GrayscaleStyle.RMY:
                    gray = GetGray(source, RMY);
                    break;
                case GrayscaleStyle.Y:
                    gray = GetGray(source, Y);
                    break;
            }

            dest.R = dest.G = dest.B = gray;
        }

        #region Private Methods

        private static double HueToRGB(double v1, double v2, double vH)
        {
            if (vH < 0)
            {
                vH += 1;
            }

            if (vH > 1)
            {
                vH -= 1;
            }

            if ((6 * vH) < 1)
            {
                return (v1 + (v2 - v1) * 6 * vH);
            }

            if ((2 * vH) < 1)
            {
                return v2;
            }

            if ((3 * vH) < 2)
            {
                return (v1 + (v2 - v1) * ((2.0 / 3) - vH) * 6);
            }

            return v1;
        }

        private static byte GetGray(RGB rgb, int[] coefficient)
        {
            return (byte)((rgb.R * coefficient[0] +
                rgb.G * coefficient[1] +
                rgb.B * coefficient[2]) / coefficient[3]);
        }

        #endregion
    }
}
