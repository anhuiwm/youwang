using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager.DBClass;

namespace TheSoul.DataManager.DBClass
{
    public class User_Admin_LogLevel
    {
        public byte log_level { get; set; }
    }
}

namespace TheSoul.DataManager
{
    public class Log_Define
    {
        public const string User_Admin_LogLevel_TableName = "User_Admin_LogLevel";
        public const string LogLevelDBName = "sharding";

        public enum eLogLevel
        {
            Default = 0,
            None,
            Simple,
            Detail,
            Full,
        }
    }
}

namespace TheSoul.DataManager
{
    public class LogManager
    {
        public static User_Admin_LogLevel GetAdminLogLevel(ref TxnBlock TB, long AID, string dbkey = Log_Define.LogLevelDBName)
        {
            string setQuery = string.Format("SELECT log_level FROM {0} WHERE aid = {1} AND enddate > GETDATE()", Log_Define.User_Admin_LogLevel_TableName, AID);
            User_Admin_LogLevel retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<User_Admin_LogLevel>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new User_Admin_LogLevel();
        }

        public static Result_Define.eResult SetAdminLogLevel(ref TxnBlock TB, long AID, byte LogLevel, DateTime setTime, string dbkey = Log_Define.LogLevelDBName)
        {
            string setQuery = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                            ON aid = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                        log_level = {2},
                                                        enddate = '{3}',
                                                        regdate = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT (aid, log_level, enddate, regdate) 
                                                    VALUES ( {1}, {2}, '{3}', GETDATE());
                                                        ", Log_Define.User_Admin_LogLevel_TableName, AID, LogLevel, setTime.ToString("yyyy-MM-dd HH:mm:ss"));
            TB.ExcuteSqlCommand(dbkey, setQuery);
            return (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }
    }
}