
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Com_CSSkin.SkinClass
{
    /// <summary>
    /// 获得桌面图标名称和位置
    /// </summary>
    public class DesktopIcon
    {
        #region Api声明
        private const uint LVM_FIRST = 0x1000;
        private const uint LVM_GETITEMCOUNT = LVM_FIRST + 4;
        private const uint LVM_GETITEMW = LVM_FIRST + 75;
        private const uint LVM_GETITEMPOSITION = LVM_FIRST + 16;
        [System.Runtime.InteropServices.DllImport("user32.DLL")]
        private static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.DLL")]
        private static extern IntPtr FindWindow(string lpszClass, string lpszWindow);
        [System.Runtime.InteropServices.DllImport("user32.DLL")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd,
out uint dwProcessId);
        private const uint PROCESS_VM_OPERATION = 0x0008;
        private const uint PROCESS_VM_READ = 0x0010;
        private const uint PROCESS_VM_WRITE = 0x0020;
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);
        private const uint MEM_COMMIT = 0x1000;
        private const uint MEM_RELEASE = 0x8000;
        private const uint MEM_RESERVE = 0x2000;
        private const uint PAGE_READWRITE = 4;
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flAllocationType, uint flProtect);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint dwFreeType);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress,
           IntPtr lpBuffer, int nSize, ref uint vNumberOfBytesRead);
        private struct LVITEM  //结构体
        {
            public int mask;
            public int iItem;
            public int iSubItem;
            public IntPtr pszText; // string
            public int cchTextMax;
        }
        private int LVIF_TEXT = 0x0001;
        /// <summary>
        /// 节点个数,通过SendMessage 发送获取
        /// </summary>
        /// <param name="AHandle"></param>
        /// <returns></returns>
        private int ListView_GetItemCount(IntPtr AHandle)
        {
            return SendMessage(AHandle, LVM_GETITEMCOUNT, 0, 0);
        }
        /// <summary>
        /// 图标位置
        /// </summary>
        /// <param name="AHandle"></param>
        /// <param name="AIndex"></param>
        /// <param name="APoint"></param>
        /// <returns></returns>
        private bool ListView_GetItemPosition(IntPtr AHandle, int AIndex, IntPtr APoint)
        {
            return SendMessage(AHandle, LVM_GETITEMPOSITION, AIndex, APoint.ToInt32()) != 0;
        }
        #endregion

        public string GetSysVer()
        {
            string Ver = Environment.OSVersion.Version.ToString();
            return Ver;
        }

        /// <summary>
        /// 获取桌面项目的名称
        /// </summary>
        /// <returns></returns>
        public string[] GetIcoName()
        {
            //桌面上SysListView32的窗口句柄 
            IntPtr vHandle;
            //xp是Progman ; win7 网上说应该是 "WorkerW"  但是 spy++ 没找到 程序也不正常
            vHandle = FindWindow("Progman", null);
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "SysListView32", null);
            int vItemCount = ListView_GetItemCount(vHandle);//个数
            uint vProcessId; //进程 pid
            GetWindowThreadProcessId(vHandle, out vProcessId);
            //打开并插入进程 
            IntPtr vProcess = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ |
                PROCESS_VM_WRITE, false, vProcessId);
            IntPtr vPointer = VirtualAllocEx(vProcess, IntPtr.Zero, 4096,
                MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
            string[] tempStr = new string[vItemCount];
            try
            {
                for (int i = 0; i < vItemCount; i++)
                {
                    byte[] vBuffer = new byte[256];
                    LVITEM[] vItem = new LVITEM[1];
                    vItem[0].mask = LVIF_TEXT;
                    vItem[0].iItem = i;
                    vItem[0].iSubItem = 0;
                    vItem[0].cchTextMax = vBuffer.Length;
                    vItem[0].pszText = (IntPtr)((int)vPointer + System.Runtime.InteropServices.Marshal.SizeOf(typeof(LVITEM)));
                    uint vNumberOfBytesRead = 0;
                    /// 分配内存空间
                    WriteProcessMemory(vProcess, vPointer,
                        System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0),
                        System.Runtime.InteropServices.Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);
                    //发送信息 获取响应
                    SendMessage(vHandle, LVM_GETITEMW, i, vPointer.ToInt32());
                    ReadProcessMemory(vProcess,
                        (IntPtr)((int)vPointer + System.Runtime.InteropServices.Marshal.SizeOf(typeof(LVITEM))),
                        System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                        vBuffer.Length, ref vNumberOfBytesRead);
                    string vText = System.Text.Encoding.Unicode.GetString(vBuffer, 0,
                        (int)vNumberOfBytesRead).TrimEnd('\0');
                    tempStr[i] = vText;
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                VirtualFreeEx(vProcess, vPointer, 0, MEM_RELEASE);
                CloseHandle(vProcess);
            }
            return tempStr;
        }

        /// <summary>
        /// 根据项目名称获得ICO图标位置
        /// </summary>
        /// <param name="icoName">项目名称</param>
        /// <returns></returns>
        public System.Drawing.Point GetIcoPoint(string icoName)
        {
            //桌面上SysListView32的窗口句柄 
            IntPtr vHandle;
            //xp是Progman ; win7 网上说应该是 "WorkerW"  但是 spy++ 没找到 程序也不正常
            vHandle = FindWindow("WorkerW", null);
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "SHELLDLL_DefView", null);
            vHandle = FindWindowEx(vHandle, IntPtr.Zero, "SysListView32", null);
            int vItemCount = ListView_GetItemCount(vHandle);//个数
            uint vProcessId; //进程 pid
            GetWindowThreadProcessId(vHandle, out vProcessId);
            //打开并插入进程 
            IntPtr vProcess = OpenProcess(PROCESS_VM_OPERATION | PROCESS_VM_READ |
                PROCESS_VM_WRITE, false, vProcessId);
            IntPtr vPointer = VirtualAllocEx(vProcess, IntPtr.Zero, 4096,
                MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);
            System.Drawing.Point[] vPoint = new System.Drawing.Point[1];
            try
            {
                for (int i = 0; i < vItemCount; i++)
                {
                    byte[] vBuffer = new byte[256];
                    LVITEM[] vItem = new LVITEM[1];
                    vItem[0].mask = LVIF_TEXT;
                    vItem[0].iItem = i;
                    vItem[0].iSubItem = 0;
                    vItem[0].cchTextMax = vBuffer.Length;
                    vItem[0].pszText = (IntPtr)((int)vPointer + System.Runtime.InteropServices.Marshal.SizeOf(typeof(LVITEM)));
                    uint vNumberOfBytesRead = 0;
                    // 分配内存空间
                    WriteProcessMemory(vProcess, vPointer,
                        System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(vItem, 0),
                        System.Runtime.InteropServices.Marshal.SizeOf(typeof(LVITEM)), ref vNumberOfBytesRead);
                    //发送信息 获取响应
                    SendMessage(vHandle, LVM_GETITEMW, i, vPointer.ToInt32());
                    ReadProcessMemory(vProcess,
                        (IntPtr)((int)vPointer + System.Runtime.InteropServices.Marshal.SizeOf(typeof(LVITEM))),
                        System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(vBuffer, 0),
                        vBuffer.Length, ref vNumberOfBytesRead);
                    string vText = System.Text.Encoding.Unicode.GetString(vBuffer, 0,
                        (int)vNumberOfBytesRead).TrimEnd('\0');
                    if (vText == icoName)
                    {
                        ListView_GetItemPosition(vHandle, i, vPointer);
                        ReadProcessMemory(vProcess, vPointer,
                            System.Runtime.InteropServices.Marshal.UnsafeAddrOfPinnedArrayElement(vPoint, 0), System.Runtime.InteropServices.Marshal.SizeOf(typeof(System.Drawing.Point)), ref vNumberOfBytesRead);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }
            finally
            {
                VirtualFreeEx(vProcess, vPointer, 0, MEM_RELEASE);
                CloseHandle(vProcess);
            }
            return vPoint[0];
        }
    }
}
