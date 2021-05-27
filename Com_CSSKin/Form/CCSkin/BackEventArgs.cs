
using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Com_CSSkin
{
    //自定义事件参数类
    public class BackEventArgs
    {
        private Image beforeBack;
        public Image BeforeBack {
            get { return beforeBack; }
        }

        private Image afterBack;
        public Image AfterBack {
            get { return afterBack; }
        }

        /// <summary>
        /// Back属性值更改时引发的事件
        /// </summary>
        /// <param name="beforeBack">更改前背景</param>
        /// <param name="afterBack">更改后背景</param>
        public BackEventArgs(Image beforeBack, Image afterBack)
        {
            this.beforeBack = beforeBack;
            this.afterBack = afterBack;
        }
    }
}
