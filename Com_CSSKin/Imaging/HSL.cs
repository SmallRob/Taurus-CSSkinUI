
using System;
using System.Collections.Generic;
using System.Text;

namespace Com_CSSkin.Imaging
{
    public class HSL
    {
        private int _hue;
        private double _saturation;
        private double _luminance;

        public int Hue
        {
            get { return _hue; }
            set
            {
                if (value < 0)
                {
                    _hue = 0;
                }
                else if (value <= 360)
                {
                    _hue = value;
                }
                else
                {
                    _hue = value % 360;
                }
            }
        }

        public double Saturation
        {
            get { return _saturation; }
            set
            {
                if (value < 0)
                {
                    _saturation = 0;
                }
                else
                {
                    _saturation = Math.Min(value, 1D);
                }
            }
        }

        public double Luminance
        {
            get { return _luminance; }
            set
            {
                if (value < 0)
                {
                    _luminance = 0;
                }
                else
                {
                    _luminance = Math.Min(value, 1D);
                }
            }
        }

        public HSL() { }

        public HSL(int hue, double saturation, double luminance)
        {
            Hue = hue;
            Saturation = saturation;
            Luminance = luminance;
        }

        public override string ToString()
        {
            return string.Format("HSL [H={0}, S={1}, L={2}]", _hue, _saturation, _luminance);
        }
    }
}
