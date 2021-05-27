
using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Com_CSSkin.SkinClass
{
    public class NewSmoothModeGraphics : IDisposable
    {
        SmoothingMode _oldMode;
        Graphics _graphics;

        public NewSmoothModeGraphics(Graphics g, SmoothingMode newMode)
        {
            _oldMode = g.SmoothingMode;
            g.SmoothingMode = newMode;
            _graphics = g;
        }

        public void Dispose()
        {
            _graphics.SmoothingMode = _oldMode;
        }
    }

    public class NewClipGraphics : IDisposable
    {
        Region _oldClip;
        Region _newClip;
        Graphics _graphics;
        bool _shouldDispose;

        public NewClipGraphics(Graphics g, Region newClip, bool disposeNewClip)
        {
            _oldClip = g.Clip;            
            _graphics = g;
            _shouldDispose = disposeNewClip;
            _newClip = newClip;
            g.Clip = newClip;
        }

        public void Dispose()
        {
            _graphics.Clip = _oldClip;
            if (_shouldDispose)
                _newClip.Dispose();
        }
    }
}
