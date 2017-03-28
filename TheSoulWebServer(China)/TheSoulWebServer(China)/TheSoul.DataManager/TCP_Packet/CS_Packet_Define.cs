using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace TheSoul.DataManager.TCP_Packet
{


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode, Pack = 1)]
    public struct StoSGMOperation
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string operatorName;
        public UInt32 op;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string jsonParam;
    };

    public class RESTRICT_CHAT
    {
        public long aid { get; set; }
        public long restrict_time { get; set; }

        public RESTRICT_CHAT(long setAid, long setTime)
        {
            aid = setAid;
            restrict_time = setTime;
        }

        public RESTRICT_CHAT() { }
    }

    public class LOG_LEVEL
    {
        public long aid { get; set; }
        public int log_level { get; set; }
        public long log_time { get; set; }

        public LOG_LEVEL(long setAid, int setlevel, long setTime)
        {
            aid = setAid;
            log_level = setlevel;
            log_time = setTime;
        }

        public LOG_LEVEL() { }
    }
}
