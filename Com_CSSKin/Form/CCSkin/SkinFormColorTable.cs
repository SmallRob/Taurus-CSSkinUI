
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Com_CSSkin
{
    public class SkinFormColorTable
    {
        //Color.FromArgb(75, 188, 254);
        public static readonly Color _captionActive =
            Color.Transparent;
        //Color.FromArgb(131, 209, 255);
        public static readonly Color _captionDeactive =
            Color.Transparent;
        //Color.FromArgb(40, 111, 152);
        public static readonly Color _captionText =
            Color.Black;
        //Color.FromArgb(55, 126, 168);
        public static Color _border =
            Color.FromArgb(100, 0, 0, 0);
        //Color.FromArgb(200, 250, 250, 250);
        public static Color _innerBorder =
            Color.FromArgb(100, 250, 250, 250);
        public static readonly Color _back =
            Color.FromArgb(128, 208, 255);
        public static Color _controlBoxActive =
            Color.FromArgb(51, 153, 204);
        public static Color _controlBoxDeactive =
            Color.FromArgb(88, 172, 218);
        public static readonly Color _controlBoxHover =
            Color.FromArgb(150, 39, 175, 231);
        public static readonly Color _controlBoxPressed =
           Color.FromArgb(150, 29, 142, 190);
        private static readonly Color _controlCloseBoxHover =
            Color.FromArgb(213, 66, 22);
        private static readonly Color _controlCloseBoxPressed =
            Color.FromArgb(171, 53, 17);
        public static readonly Color _controlBoxInnerBorder =
            Color.FromArgb(128, 250, 250, 250);

        public virtual Color CaptionActive
        {
            get { return _captionActive; }
        }

        public virtual Color CaptionDeactive
        {
            get { return _captionDeactive; }
        }

        public virtual Color CaptionText
        {
            get { return _captionText; }
        }

        //public virtual Color Border
        //{
        //    get { return _border; }
        //    set { _border = value; }
        //}

        //public virtual Color InnerBorder
        //{
        //    get { return _innerBorder; }
        //    set { _innerBorder = value; }
        //}

        public virtual Color Back
        {
            get { return _back; }
        }

        public virtual Color ControlBoxActive
        {
            get { return _controlBoxActive; }
            set { _controlBoxActive = value; }
        }

        public virtual Color ControlBoxDeactive
        {
            get { return _controlBoxDeactive; }
            set { _controlBoxDeactive = value; }
        }

        public virtual Color ControlBoxHover
        {
            get { return _controlBoxHover; }
        }

        public virtual Color ControlBoxPressed
        {
            get { return _controlBoxPressed; }
        }

        public virtual Color ControlCloseBoxHover
        {
            get { return _controlCloseBoxHover; }
        }

        public virtual Color ControlCloseBoxPressed
        {
            get { return _controlCloseBoxPressed; }
        }

        public virtual Color ControlBoxInnerBorder
        {
            get { return _controlBoxInnerBorder; }
        }
    }
}
