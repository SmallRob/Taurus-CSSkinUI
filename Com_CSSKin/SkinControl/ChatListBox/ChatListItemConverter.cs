
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Reflection;
using System.Windows.Forms;

namespace Com_CSSkin.SkinControl
{
    public class ChatListItemConverter : ExpandableObjectConverter
    {
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) {
            return destinationType == typeof(InstanceDescriptor) ||
                base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture,
            object value, Type destinationType) {
            if (destinationType == null)
                throw new ArgumentNullException("DestinationType cannot be null");
            if (destinationType == typeof(InstanceDescriptor) && (value is ChatListItem)) {
                ConstructorInfo constructor = null;
                ChatListItem item = (ChatListItem)value;
                ChatListSubItem[] subItems = null;
                if (item.SubItems.Count > 0) {
                    subItems = new ChatListSubItem[item.SubItems.Count];
                    item.SubItems.CopyTo(subItems, 0);
                }
                if (item.Text != null && subItems != null)
                    constructor = typeof(ChatListItem).GetConstructor(new Type[] { typeof(string), typeof(ChatListSubItem[]) });
                if (constructor != null)
                    return new InstanceDescriptor(constructor, new object[] { item.Text, subItems }, false);
                
                if (subItems != null)
                    constructor = typeof(ChatListItem).GetConstructor(new Type[] { typeof(ChatListSubItem[]) });
                if (constructor != null)
                    return new InstanceDescriptor(constructor, new object[] { subItems }, false);
                if (item.Text != null) {
                    constructor = typeof(ChatListItem).GetConstructor(new Type[] { typeof(string), typeof(bool) });
                }
                if (constructor != null) {
                    return new InstanceDescriptor(constructor, new object[] { item.Text, item.IsOpen });
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
