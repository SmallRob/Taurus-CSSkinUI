using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Com_CSSkin.Win32.Struct
{
    [StructLayout(LayoutKind.Sequential)]
    public struct API_MSG
    {
        public IntPtr Hwnd;
        public int Msg;
        public IntPtr WParam;
        public IntPtr LParam;
        public int Time;
        public POINT Pt;

        public Message ToMessage()
        {
            Message res = new Message();
            res.HWnd = Hwnd;
            res.Msg = Msg;
            res.WParam = WParam;
            res.LParam = LParam;
            return res;
        }

        public void FromMessage(ref Message msg)
        {
            Hwnd = msg.HWnd;
            Msg = msg.Msg;
            WParam = msg.WParam;
            LParam = msg.LParam;
        }
    }
}
