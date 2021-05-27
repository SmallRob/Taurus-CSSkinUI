/********************************************************************
 * *
 * * 使本项目源码或本项目生成的DLL前请仔细阅读以下协议内容，如果你同意以下协议才能使用本项目所有的功能，
 * * 否则如果你违反了以下协议，有可能陷入法律纠纷和赔偿，作者保留追究法律责任的权利。
 * *
 * * 1、你可以在开发的软件产品中使用和修改本项目的源码和DLL，但是请保留所有相关的版权信息。
 * * 2、不能将本项目源码与作者的其他项目整合作为一个单独的软件售卖给他人使用。
 * * 3、不能传播本项目的源码和DLL，包括上传到网上、拷贝给他人等方式。
 * * 4、以上协议暂时定制，由于还不完善，作者保留以后修改协议的权利。
 * *
 * * Copyright (C) 2013-? cskin Corporation All rights reserved.
 * * 网站：CSkin界面库 http://www.cskin.net
 * * 作者： 乔克斯 QQ：345015918 .Net项目技术组群：306485590
 * * 请保留以上版权信息，否则作者将保留追究法律责任。
 * *
 * * 创建时间：2016-01-18
 * * 说明：CustomColors.cs
 * *
********************************************************************/

using System.Drawing;
using System.Drawing.Imaging;

namespace Com_CSSkin.SkinControl
{
    /// <summary>
    /// Provides custom colors members to use in the OfficeColorPicker.
    /// </summary>
    class CustomColors
    {      
        internal static Color ButtonHoverLight = Color.FromArgb(255, 240, 207);

        internal static Color ButtonHoverDark = Color.FromArgb(249, 179, 48);

        internal static Color SelectedAndHover = Color.FromArgb(254, 128, 62);
     
        internal static Color ButtonBorder = Color.FromArgb(172, 168, 153);

        internal static Color FocusDashedBorder = Color.FromArgb(83, 87, 102);

        internal static Color SelectedBorder = Color.FromArgb(0, 0, 128);

        internal static Color ColorPickerBackgroundDocked = Color.FromArgb(248, 248, 248);

        #region Color Picker Colors

        // ------------------ First Row -----------------------------
        internal static Color Black = Color.Black;
        internal static Color Brown = Color.FromArgb(153, 51, 0);
        internal static Color OliveGreen = Color.FromArgb(51, 51, 0);
        internal static Color DarkGreen = Color.FromArgb(0, 51, 0);

        internal static Color DarkTeal = Color.FromArgb(0, 51, 102);
        internal static Color DarkBlue = Color.FromArgb(0, 0, 128);
        internal static Color Indigo = Color.FromArgb(51, 51, 153);
        internal static Color Gray80 = Color.FromArgb(51, 51, 51);
        // ------------------ Second Row ----------------------------
        internal static Color DarkRed = Color.FromArgb(128, 0, 0);
        internal static Color Orange = Color.FromArgb(255, 102, 0);
        internal static Color DarkYellow = Color.FromArgb(128, 128, 0);
        internal static Color Green = Color.Green;

        internal static Color Teal = Color.Teal;
        internal static Color Blue = Color.Blue;
        internal static Color BlueGray = Color.FromArgb(102, 102, 153);
        internal static Color Gray50 = Color.FromArgb(128, 128, 128);
        // ------------------ Third Row -----------------------------       
        internal static Color Red = Color.Red;
        internal static Color LightOrange = Color.FromArgb(255, 153, 0);
        internal static Color Lime = Color.FromArgb(153, 204, 0);
        internal static Color SeaGreen = Color.FromArgb(51, 153, 102);

        internal static Color Aqua = Color.FromArgb(51, 204, 204);
        internal static Color LightBlue = Color.FromArgb(51, 102, 255);
        internal static Color Violet = Color.FromArgb(128, 0, 128);
        internal static Color Gray40 = Color.FromArgb(153, 153, 153);
        // ----------------- Forth Row ------------------------------
        internal static Color Pink = Color.FromArgb(255, 0, 255);
        internal static Color Gold = Color.FromArgb(255, 204, 0);
        internal static Color Yellow = Color.FromArgb(255, 255, 0);
        internal static Color BrightGreen = Color.FromArgb(0, 255, 0);

        internal static Color Turquoise = Color.FromArgb(0, 255, 255);
        internal static Color SkyBlue = Color.FromArgb(0, 204, 255);
        internal static Color Plum = Color.FromArgb(153, 51, 102);
        internal static Color Gray25 = Color.FromArgb(192, 192, 192);     
        // ----------------- Fifth Row ------------------------------
        internal static Color Rose = Color.FromArgb(255, 153, 204);
        internal static Color Tan = Color.FromArgb(255, 204, 153);
        internal static Color LightYellow = Color.FromArgb(255, 255, 153);
        internal static Color LightGreen = Color.FromArgb(204, 255, 204);

        internal static Color LightTurquoise = Color.FromArgb(204, 255, 255);
        internal static Color PaleBlue = Color.FromArgb(153, 204, 255);
        internal static Color Lavender = Color.FromArgb(204, 153, 255);
        internal static Color White = Color.White;


        internal static Color[] SelectableColors =
            {
                CustomColors.Black, CustomColors.Brown, CustomColors.OliveGreen, CustomColors.DarkGreen, 
                CustomColors.DarkTeal, CustomColors.DarkBlue, CustomColors.Indigo, CustomColors.Gray80, 

                CustomColors.DarkRed, CustomColors.Orange, CustomColors.DarkYellow, CustomColors.Green,
                CustomColors.Teal, CustomColors.Blue, CustomColors.BlueGray, CustomColors.Gray50,

                CustomColors.Red, CustomColors.LightOrange, CustomColors.Lime, CustomColors.SeaGreen, 
                CustomColors.Aqua, CustomColors.LightBlue, CustomColors.Violet, CustomColors.Gray40,

                CustomColors.Pink, CustomColors.Gold, CustomColors.Yellow, CustomColors.BrightGreen, 
                CustomColors.Turquoise, CustomColors.SkyBlue, CustomColors.Plum, CustomColors.Gray25,

                CustomColors.Rose, CustomColors.Tan, CustomColors.LightYellow, CustomColors.LightGreen, 
                CustomColors.LightTurquoise, CustomColors.PaleBlue, CustomColors.Lavender, CustomColors.White,
            };

        /// <summary>
        /// Provides a list of color names that matches the SelectableColors array.
        /// </summary>
        internal static string[] SelectableColorsNames =
            {
                "Black", "Brown" , "Olive Green" , "Dark Green", 
                "Dark Teal" , "Dark Blue" , "Indigo" , "Gray-80%", 

                "Dark Red", "Orange", "Dark Yellow", "Green",
                "Teal", "Blue", "Blue-Gray", "Gray-50%",

                "Red", "Light Orange", "Lime", "Sea Green", 
                "Aqua", "Light Blue", "Violet", "Gray-40%",

                "Pink", "Gold", "Yellow", "Bright Green", 
                "Turquoise", "Sky Blue", "Plum", "Gray-25%",

                "Rose", "Tan", "Light Yellow", "Light Green", 
                "Light Turquoise", "Pale Blue", "Lavender", "White",
                 
                "More Colors"

            };


        #endregion 
        
        /// <summary>
        /// Compare 2 colors by their RGB properties.
        /// </summary>
        /// <param name="color1"></param>
        /// <param name="color2"></param>
        /// <returns>True when R,G and B properties of both colors are equals</returns>
        internal static bool ColorEquals(Color color1, Color color2)
        {
            return 
                color1.R == color2.R && color1.G == color2.G && color1.B == color2.B;
        }

        private static ImageAttributes _grayScaleAttributes;
        /// <summary>
        /// Returns the standard grayscale matrix for image transformations
        /// </summary>
        public static ImageAttributes GrayScaleAttributes
        {
            get
            {
                if (_grayScaleAttributes == null)
                {
                    var matrix = new ColorMatrix
                        {
                            Matrix00 = 1/3f,
                            Matrix01 = 1/3f,
                            Matrix02 = 1/3f,
                            Matrix10 = 1/3f,
                            Matrix11 = 1/3f,
                            Matrix12 = 1/3f,
                            Matrix20 = 1/3f,
                            Matrix21 = 1/3f,
                            Matrix22 = 1/3f
                        };

                    _grayScaleAttributes = new ImageAttributes();

                    _grayScaleAttributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                }

                return _grayScaleAttributes;
            }
        }
      
    }
}
