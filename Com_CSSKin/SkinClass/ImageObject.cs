
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace Com_CSSkin.SkinClass
{
    public static class ImageObject
    {
        /// <summary>
        /// 得到要绘置的图片对像
        /// </summary>
        /// <param name="str">图像在程序集中的地址</param>
        /// <returns></returns>
        public static Bitmap GetResBitmap(string str)
        {
            Stream sm = FindStream(str);
            if (sm == null) return null;
            return new Bitmap(sm);
        }

        /// <summary>
        /// 得到图程序集中的图片对像
        /// </summary>
        /// <param name="str">图像在程序集中的地址</param>
        /// <returns></returns>
        private static Stream FindStream(string str)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string[] resNames = assembly.GetManifestResourceNames();
            foreach (string s in resNames)
            {
                if (s == str)
                {
                    return assembly.GetManifestResourceStream(s);
                }
            }
            return null;
        }
    }
}
