
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Com_CSSkin
{
    public class AntiAliasGraphics : IDisposable
    {
        private SmoothingMode _oldMode;
        private Graphics _graphics;

        public AntiAliasGraphics(Graphics graphics)
        {
            _graphics = graphics;
            _oldMode = graphics.SmoothingMode;
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            _graphics.SmoothingMode = _oldMode;
        }

        #endregion
    }
}
