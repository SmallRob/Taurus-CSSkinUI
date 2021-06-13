
using System;
using System.Windows.Forms;
using Com_CSSkin.Win32.Const;
using System.Runtime.InteropServices;

namespace Com_CSSkin.Win32
{
    public static class Helper
    {
        public static bool LeftKeyPressed()
        {
            if (SystemInformation.MouseButtonsSwapped)
            {
                return (NativeMethods.GetKeyState(VK.VK_RBUTTON) < 0);
            }
            else
            {
                return (NativeMethods.GetKeyState(VK.VK_LBUTTON) < 0);
            }
        }

        public static int HIWORD(int n)
        {
            return ((n >> 0x10) & 0xffff);
        }

        public static int HIWORD(IntPtr n)
        {
            return HIWORD((int)((long)n));
        }

        public static int LOWORD(int n)
        {
            return (n & 0xffff);
        }

        public static int LOWORD(IntPtr n)
        {
            return LOWORD((int)((long)n));
        }

        public static int MAKELONG(int low, int high)
        {
            return ((high << 0x10) | (low & 0xffff));
        }

        public static IntPtr MAKELPARAM(int low, int high)
        {
            return (IntPtr)((high << 0x10) | (low & 0xffff));
        }

        public static int SignedHIWORD(int n)
        {
            return (short)((n >> 0x10) & 0xffff);
        }

        public static int SignedHIWORD(IntPtr n)
        {
            return SignedHIWORD((int)((long)n));
        }

        public static int SignedLOWORD(int n)
        {
            return (short)(n & 0xffff);
        }

        public static int SignedLOWORD(IntPtr n)
        {
            return SignedLOWORD((int)((long)n));
        }

        public static void Swap(ref int x, ref int y)
        {
            int tmp = x;
            x = y;
            y = tmp;
        }

        public static IntPtr ToIntPtr(object structure)
        {
            IntPtr lparam = IntPtr.Zero;
            lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(structure));
            Marshal.StructureToPtr(structure, lparam, false);
            return lparam;
        }

        public static void SetRedraw(IntPtr hWnd, bool redraw)
        {
            IntPtr ptr = redraw ? Result.TRUE : Result.FALSE;
            NativeMethods.SendMessage(hWnd, WM.WM_SETREDRAW, ptr, 0);
        }
    }
}
