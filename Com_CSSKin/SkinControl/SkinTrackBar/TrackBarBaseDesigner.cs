
using System;
using System.Windows.Forms.Design;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections;

namespace Com_CSSkin.SkinControl
{
    internal class TrackBarBaseDesigner : ControlDesigner
    {
        public TrackBarBaseDesigner()
        {
            base.AutoResizeHandles = true;
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules selectionRules = base.SelectionRules;
                object component = base.Component;
                selectionRules |= SelectionRules.AllSizeable;
                PropertyDescriptor pdAutoSize =
                    TypeDescriptor.GetProperties(component)["AutoSize"];
                if (pdAutoSize != null)
                {
                    bool flag = (bool)pdAutoSize.GetValue(component);
                    PropertyDescriptor pdOrientation =
                        TypeDescriptor.GetProperties(component)["Orientation"];
                    Orientation horizontal = Orientation.Horizontal;
                    if (pdOrientation != null)
                    {
                        horizontal = (Orientation)pdOrientation.GetValue(component);
                    }
                    if (!flag)
                    {
                        return selectionRules;
                    }
                    switch (horizontal)
                    {
                        case Orientation.Horizontal:
                            return (selectionRules &
                                ~(SelectionRules.BottomSizeable | SelectionRules.TopSizeable));

                        case Orientation.Vertical:
                            return (selectionRules &
                                ~(SelectionRules.RightSizeable | SelectionRules.LeftSizeable));
                    }
                }
                return selectionRules;
            }
        }

        protected override void PostFilterProperties(IDictionary Properties)
        {
            Properties.Remove("BackgroundImage");
            Properties.Remove("BackgroundImageLayout");
            Properties.Remove("Font");
            Properties.Remove("ForeColor");
            Properties.Remove("ImeMode");
            Properties.Remove("Padding");
            Properties.Remove("Text");
        }

        protected override void PostFilterEvents(IDictionary events)
        {
            events.Remove("BackColorChanged");
            events.Remove("BackgroundImageChanged");

            events.Remove("Click");
            events.Remove("MouseClick");
            events.Remove("DoubleClick");
            events.Remove("MouseDoubleClick");
            events.Remove("FontChanged");
            events.Remove("ForeColorChanged");
            events.Remove("ImeModeChanged");

            events.Remove("PaddingChanged");
            events.Remove("Paint");

            events.Remove("TextChanged");

            base.PostFilterEvents(events);
        }
    }
}
