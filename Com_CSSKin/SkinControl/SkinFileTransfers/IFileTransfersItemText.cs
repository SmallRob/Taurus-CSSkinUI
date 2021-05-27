
using System;
using System.Collections.Generic;
using System.Text;

namespace Com_CSSkin.SkinControl
{
    public interface IFileTransfersItemText
    {
        string Save { get; }
        string SaveTo { get; }
        string RefuseReceive { get; }
        string CancelTransfers { get; }
    }
}
