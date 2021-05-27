
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Com_CSSkin.SkinControl.Internals
{
    [Flags, ComVisible(false)]
    public enum DVASPECT : int
    {
        DVASPECT_CONTENT = 1,
        DVASPECT_THUMBNAIL = 2,
        DVASPECT_ICON = 4,
        DVASPECT_DOCPRINT = 8,
        DVASPECT_OPAQUE = 16,
        DVASPECT_TRANSPARENT = 32,
    }
}
