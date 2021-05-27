
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Com_CSSkin.SkinControl
{
    public class PanelColorTable
    {
        private static readonly Color _border = Color.FromArgb(177, 221, 255);
        private static readonly Color _captionBackNormal = Color.FromArgb(160, 218, 255);
        private static readonly Color _captionBackHover = Color.FromArgb(123, 198, 255);
        private static readonly Color _captionBackPressed = Color.FromArgb(100, 188, 255);
        private static readonly Color _captionFore = Color.Black;
        

        public PanelColorTable() { }

        public virtual Color Border
        {
            get { return _border; }
        }

        public virtual Color CaptionBackNormal
        {
            get { return _captionBackNormal; }
        }

        public virtual Color CaptionBackHover
        {
            get { return _captionBackHover; }
        }

        public virtual Color CaptionBackPressed
        {
            get { return _captionBackPressed; }
        }

        public virtual Color CaptionFore
        {
            get { return _captionFore; }
        }
    }
}
