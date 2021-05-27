
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Com_CSSkin
{
    //自定义事件参数类
    public class ThemeEventArgs
    {
        private CSSkinMain afterTheme;
        public CSSkinMain AfterTheme {
            get { return afterTheme; }
        }

        /// <summary>
        /// XTheme主题属性值更改时引发的事件
        /// </summary>
        /// <param name="afterTheme">更改后主题</param>
        public ThemeEventArgs(CSSkinMain afterTheme) {
            this.afterTheme = afterTheme;
        }
    }
}
