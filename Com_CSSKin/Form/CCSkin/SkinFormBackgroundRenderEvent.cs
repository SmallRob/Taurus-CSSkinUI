
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Com_CSSkin
{
     public delegate void SkinFormBackgroundRenderEventHandler(
        object sender,
        SkinFormBackgroundRenderEventArgs e);

    public class SkinFormBackgroundRenderEventArgs : PaintEventArgs
    {
        private CSSkinMain _skinForm;

        public SkinFormBackgroundRenderEventArgs(
            CSSkinMain skinForm,
            Graphics g,
            Rectangle clipRect)
            : base(g, clipRect)
        {
            _skinForm = skinForm;
        }

        public CSSkinMain SkinForm
        {
            get { return _skinForm; }
        }
    }
}
