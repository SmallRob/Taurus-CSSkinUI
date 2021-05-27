
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Com_CSSkin
{
    public delegate void SkinFormBorderRenderEventHandler(
        object sender,
        SkinFormBorderRenderEventArgs e);

    public class SkinFormBorderRenderEventArgs : PaintEventArgs
    {
        private CSSkinMain _skinForm;
        private bool _active;

        public SkinFormBorderRenderEventArgs(
            CSSkinMain skinForm,
            Graphics g,
            Rectangle clipRect,
            bool active)
            : base(g, clipRect)
        {
            _skinForm = skinForm;
            _active = active;
        }

        public CSSkinMain SkinForm
        {
            get { return _skinForm; }
        }

        public bool Active
        {
            get { return _active; }
        }
    }
}
