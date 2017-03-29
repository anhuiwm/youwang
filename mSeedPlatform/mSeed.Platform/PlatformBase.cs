using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;

namespace mSeed.Platform
{
    public class PlatformBase
    {

        public static void GetPlatformDB(ref TxnBlock TB)
        {
            TB.DBConn(mSeed.Platform.SystemConfig.GetSystemConfigInstance().platformDB, mSeed.Platform.SystemConfig.GetSystemConfigInstance().platformDB.SetDBAlias);
        }
    }

    public class PlatformLogger :IDisposable
    {
        public PlatformLogger() { }
        public PlatformLogger(string settag) { tag = settag;  }

        private List<string> loglist = new List<string>();
        public string tag = "";

        public void DBLog(string e)
        {
            loglist.Add(e);
        }

        public string GetLogString()
        {
            return mJsonSerializer.ToJsonString(loglist);
        }

        public void Dispose()
        {
            if (loglist.Count > 0)
            {
                mSeed.Common.mLogger.mLogger.Debug(this.GetLogString(), tag);
                mSeed.Common.mLogger.mLogger.GetLoggerInstance().FlushLog();
                this.loglist.Clear();
            }
        }
    }
}
