using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.Global;

namespace OperationTool
{
    public partial class OperationManager
    {
        const string OperationINIFileName = "CouponDBConnect.ini";

        public static void GetOperationDB(ref TxnBlock TB)
        {
            string savePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;

            DBEndpoint setDB = new DBEndpoint();
            string AppPath = "";
            AppPath = savePath + @"\dbcon\";//원본 위치
            string sourceFile = Path.Combine(AppPath, OperationINIFileName);
            TheSoul.DataManager.Tools.IniParser parser = new TheSoul.DataManager.Tools.IniParser(sourceFile);
            setDB.Host = parser.GetSetting("GlobalDB", "host");
            setDB.Database = parser.GetSetting("GlobalDB", "db");
            setDB.UserID = parser.GetSetting("GlobalDB", "id");
            setDB.UserPW = parser.GetSetting("GlobalDB", "pw");
            string gruopid = parser.GetSetting("GlobalDB", "server_group_id");
            
            TB.IsoLevel = IsolationLevel.ReadUncommitted;     // set transaction IsolationLevel (default ReadUncommited)
            TB.DBConn(setDB, Operation_Define.OperationDBName);        // make alias name for this connection

            TheSoulDBcon.GetInstance().TheSoulDBInitFromGlobal(ref TB, System.Convert.ToInt32(gruopid));
        }
    }
}