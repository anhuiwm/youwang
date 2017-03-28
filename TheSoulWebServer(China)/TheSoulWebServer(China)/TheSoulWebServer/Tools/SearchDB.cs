using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;

namespace TheSoulWebServer.Tools
{
    public class SearchDB
    {
        public static void DBOpen(ref TxnBlock tb, DBEndpoint setDBEndPoint, string dbAlias = "")
        {
            tb.IsoLevel = IsolationLevel.ReadUncommitted;     // set transaction IsolationLevel (default ReadUncommitted)            
            tb.DBConn(setDBEndPoint, string.IsNullOrEmpty(dbAlias) ? setDBEndPoint.SetDBAlias : dbAlias);
        }

        public void GlobalDBOpen(ref TxnBlock tb, DBEndpoint setDBEndPoint)
        {
            tb.IsoLevel = IsolationLevel.ReadUncommitted;     // set transaction IsolationLevel (default ReadUncommitted)
            tb.DBConn(setDBEndPoint, DataManager_Define.GlobalDB);
        }

        public void CommonDBOpen(ref TxnBlock tb, string conn_str)
        {
            tb.IsoLevel = IsolationLevel.ReadUncommitted;     // set transaction IsolationLevel (default ReadUncommitted)
            tb.DBConn(conn_str, DataManager_Define.CommonDB);
            //tb.Elog = TheSoul.DataManager.RedisConst.RedisConstErrorLog;
        }

        // TDOO : remove lagacy code
        public void CommonDBOpen(ref TxnBlock tb)
        {
            //tb.Elog = TheSoul.DataManager.RedisConst.RedisConstErrorLog;
            // use DbEndpoint (for simple config)
            DBEndpoint setDB = new DBEndpoint();
            setDB.Host = "210.122.11.197,31433";
            setDB.Database = DataManager_Define.CommonDB;
            setDB.UserID = "sa";
            setDB.UserPW = "dpaTlemrpdlawm!@#";
            tb.IsoLevel = IsolationLevel.ReadUncommitted;     // set transaction IsolationLevel (default ReadUncommitted)
            tb.DBConn(setDB, DataManager_Define.CommonDB);        // make alias name for this connection
        }

        public void ShardingDBOpen(ref TxnBlock tb, int DB_INDEX)
        {
            string connSharding = TheSoulDBcon.GetInstance().GetShardingDB(DB_INDEX);
            tb.IsoLevel = IsolationLevel.ReadUncommitted;     // set transaction IsolationLevel (default ReadUncommitted)
            tb.DBConn(connSharding, DataManager_Define.ShardingDB); // make default alias name for this connection
        }

        public void LogDBOpen(ref TxnBlock tb, int DB_INDEX = 1)
        {
            string connLog = TheSoulDBcon.GetInstance().GetLogDB(DB_INDEX);
            tb.IsoLevel = IsolationLevel.ReadUncommitted;     // set transaction IsolationLevel (default ReadUncommitted)
            tb.DBConn(connLog, DataManager_Define.LogDB);
        }
    }
}