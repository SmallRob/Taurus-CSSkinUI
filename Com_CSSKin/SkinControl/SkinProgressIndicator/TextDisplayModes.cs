
using System;

namespace Com_CSSkin.SkinControl
{
    /// <summary>
    /// 此枚举用于选择什么样的文本应该在控件显示。
    /// </summary>
    [Flags]
    public enum TextDisplayModes
    {
        /// <summary>
        /// 没有文本将显示在控件。
        /// </summary>
        None = 0,

        /// <summary>
        /// 控件将显示属性值的百分比。
        /// </summary>
        Percentage = 1,

        /// <summary>
        /// 控件将显示文本属性的值。
        /// </summary>
        Text = 2,

        /// <summary>
        /// 控件将显示值的文本和百分比特性。
        /// </summary>
        Both = Percentage | Text
    }
}