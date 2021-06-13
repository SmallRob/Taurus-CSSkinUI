
using System;

namespace Com_CSSkin.Win32.Const
{
    public class TernaryRasterOperations
    {
        private TernaryRasterOperations() { }

        public const int SRCCOPY = 0x00CC0020; /* dest = source*/
        public const int SRCPAINT = 0x00EE0086; /* dest = source OR dest*/
        public const int SRCAND = 0x008800C6; /* dest = source AND dest*/
        public const int SRCINVERT = 0x00660046; /* dest = source XOR dest*/
        public const int SRCERASE = 0x00440328; /* dest = source AND (NOT dest )*/
        public const int NOTSRCCOPY = 0x00330008; /* dest = (NOT source)*/
        public const int NOTSRCERASE = 0x001100A6; /* dest = (NOT src) AND (NOT dest) */
        public const int MERGECOPY = 0x00C000CA; /* dest = (source AND pattern)*/
        public const int MERGEPAINT = 0x00BB0226; /* dest = (NOT source) OR dest*/
        public const int PATCOPY = 0x00F00021; /* dest = pattern*/
        public const int PATPAINT = 0x00FB0A09; /* dest = DPSnoo*/
        public const int PATINVERT = 0x005A0049; /* dest = pattern XOR dest*/
        public const int DSTINVERT = 0x00550009; /* dest = (NOT dest)*/
        public const int BLACKNESS = 0x00000042; /* dest = BLACK*/
        public const int WHITENESS = 0x00FF0062; /* dest = WHITE*/
    }
}
