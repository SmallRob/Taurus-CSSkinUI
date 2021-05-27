
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Com_CSSkin.SkinControl.Internals
{
    [ComImport]
    [Guid("0000000d-0000-0000-C000-000000000046")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IEnumSTATSTG
    {
        // The user needs to allocate an STATSTG array whose size is celt.
        [PreserveSig]
        uint
            Next(
            uint celt,
            [MarshalAs(UnmanagedType.LPArray), Out]
			System.Runtime.InteropServices.ComTypes.STATSTG[] rgelt,
            out uint pceltFetched
            );

        void Skip(uint celt);

        void Reset();

        [return: MarshalAs(UnmanagedType.Interface)]
        IEnumSTATSTG Clone();
    }

}
