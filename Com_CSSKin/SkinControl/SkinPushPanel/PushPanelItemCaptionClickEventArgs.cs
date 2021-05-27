
using System;

namespace Com_CSSkin.SkinControl
{
    public delegate void PushPanelItemCaptionClickEventHandler(
        object sender,
        PushPanelItemCaptionClickEventArgs e);

    public class PushPanelItemCaptionClickEventArgs : EventArgs
    {
        private PushPanelItem _item;

        public PushPanelItemCaptionClickEventArgs()
            : base()
        {
        }

        public PushPanelItemCaptionClickEventArgs(PushPanelItem item)
            : this()
        {
            _item = item;
        }

        public PushPanelItem Item
        {
            get { return _item; }
            set { _item = value; }
        }
    }
}
