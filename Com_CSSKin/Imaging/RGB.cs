
using System;
using System.Drawing;

namespace Com_CSSkin.Imaging
{
    public class RGB
    {
        private byte _r;
        private byte _g;
        private byte _b;

        public const short RIndex = 2;
        public const short GIndex = 1;
        public const short BIndex = 0;

        public byte R
        {
            get { return _r; }
            set { _r = value; }
        }

        public byte G
        {
            get { return _g; }
            set { _g = value; }
        }

        public byte B
        {
            get { return _b; }
            set { _b = value; }
        }

        public Color Color
        {
            get { return Color.FromArgb(_r, _g, _b); }
            set
            {
                _r = value.R;
                _g = value.G;
                _b = value.B;
            }
        }

        public RGB() { }

        public RGB(byte r, byte g, byte b)
        {
            _r = r;
            _g = g;
            _b = b;
        }

        public RGB(Color color)
        {
            _r = color.R;
            _g = color.G;
            _b = color.B;
        }

        public override string ToString()
        {
            return string.Format("RGB [R={0}, G={1}, B={2}]", _r, _g, _b);
        }
    }
}
