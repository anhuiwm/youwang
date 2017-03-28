using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Linq;

using mSeed.mDBTxnBlock;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;

public class TheSoulDBcon
{
    static TheSoulDBcon dbcon = null;
    static IniParser parser = null;
    static bool bLog = false;    
    static string CommonDB = null;
    static string CommonLogDB = null;
    public static string AppPath = null;
    public static char[] AppPathArray = new char[512];
    public static char[] CommonArray = new char[512];
    public static string iniFileName = "TheSoulDBConnect.ini";
    public static int ShardingCount;
    public static string[] Sharding = { null, null, null, null, null };
    public static string[] log = { null, null, null, null, null };
    public static string[] url = { null, null, null, null, null };
    private static DBEndpoint setGlobalDB = null;
    private static Dictionary<int, List<DBEndpoint>> db_list = new Dictionary<int, List<DBEndpoint>>();
    private static List<snail_ip_table> snail_ips = new List<snail_ip_table>();
    public static bool bIptable = false;

    public static List<snail_ip_table> Snail_ips
    {
        get { return TheSoulDBcon.snail_ips; }
        set { TheSoulDBcon.snail_ips = value; bIptable = true; }
    }
    
    public static Dictionary<int, List<DBEndpoint>> DBConnList
    {
        get { return TheSoulDBcon.db_list; }
        set { TheSoulDBcon.db_list = value; }
    }
    public static string cgpURL = null;
    public static string LogDirectory = null;
    public static string LogOnOff = null;
    public static int server_group_id = 0;
    public static DataManager_Define.eCountryCode service_area = DataManager_Define.eCountryCode.Default;

    public static TheSoulDBcon GetInstance()
    {
        if (dbcon == null)
        {
            dbcon = new TheSoulDBcon();
            return dbcon;
        }
        else
            return dbcon;
    }
    
    public void IniFileLoad(string savePath)
    {
        if (setGlobalDB == null)
        {
            AppPath = new string(AppPathArray);
            AppPath = savePath + @"\dbcon\";//원본 위치
            string sourceFile = Path.Combine(AppPath, iniFileName);
            parser = new IniParser(sourceFile);
            server_group_id = System.Convert.ToInt32(parser.GetSetting("GlobalDB", "server_group_id"));

            setGlobalDB = new DBEndpoint();
            setGlobalDB.Host = parser.GetSetting("GlobalDB", "host");
            setGlobalDB.SetDBAlias = setGlobalDB.Database = parser.GetSetting("GlobalDB", "db");
            setGlobalDB.UserID = parser.GetSetting("GlobalDB", "id");
            setGlobalDB.UserPW = parser.GetSetting("GlobalDB", "pw");

            string areaCode = parser.GetSetting("GlobalDB", SystemData_Define.Service_AreaKey);
            if (string.IsNullOrEmpty(areaCode))
                service_area = DataManager_Define.eCountryCode.Default;
            else
            {
                switch (areaCode.ToLower())
                {
                    case "kr":
                        service_area = DataManager_Define.eCountryCode.Korea; // 한국어
                        break;
                    case "jp":
                        service_area = DataManager_Define.eCountryCode.Japan; // 일본어
                        break;
                    case "cn":
                        service_area = DataManager_Define.eCountryCode.China; // 중국어(간체)
                        break;
                    case "tw":
                        service_area = DataManager_Define.eCountryCode.Taiwan; // 중국어(번체)
                        break;
                    case "intl":
                        service_area = DataManager_Define.eCountryCode.International; // 중국어
                        break;
                    default:
                        service_area = DataManager_Define.eCountryCode.English; // 영어
                        break;
                }
            }
        }
    }

    public bool GetIniFileLoad(string savePath, ref string newMessage)
    {        
        if (AppPath == null)
        {
            AppPath = new string(AppPathArray);
            AppPath = savePath + @"\dbcon\";//원본 위치
            string sourceFile = Path.Combine(AppPath, iniFileName);
            parser = new IniParser(sourceFile);

            CommonDB = parser.GetSetting("CommonDB", "common");

            server_group_id = System.Convert.ToInt32(parser.GetSetting("GlobalDB", "server_group_id"));
            if (setGlobalDB == null)
            {
                setGlobalDB = new DBEndpoint();
                setGlobalDB.Host = parser.GetSetting("GlobalDB", "host");
                setGlobalDB.Database = parser.GetSetting("GlobalDB", "db");
                setGlobalDB.UserID = parser.GetSetting("GlobalDB", "id");
                setGlobalDB.UserPW = parser.GetSetting("GlobalDB", "pw");
            }

            string count = parser.GetSetting("ShardingDB", "dbcnt");
            ShardingCount = System.Convert.ToInt32(count);
            string section;
            for (int n = 0; n < ShardingCount; ++n)
            {
                section = "dbcon" + (n + 1);
                Sharding[n] = new string(new char[512]);
                Sharding[n] = parser.GetSetting("ShardingDB", section);
                log[n] = new string(new char[512]);
                log[n] = parser.GetSetting("LogDB", section);
                url[n] = new string(new char[512]);
                url[n] = parser.GetSetting("NHNCheckURL", section);
            }

            CommonLogDB = new string(new char[512]);
            CommonLogDB = parser.GetSetting("CommonLogDB", "CommonLog");

            cgpURL = new string(new char[512]);
            cgpURL = parser.GetSetting("CGPURL", "CGPurl");

            LogDirectory = new string(new char[512]);
            LogDirectory = parser.GetSetting("LogDirectory", "LogDir");

            LogOnOff = new string(new char[512]);
            LogOnOff = parser.GetSetting("LogOnOff", "OnOff");
            bLog = LogOnOff.Equals("On");

            if (!RedisConst.ForceInit)
            {
                DataManager_Define.RedisServerList = new System.Collections.Generic.List<mSeed.RedisManager.RedisEndpoint>();
                DataManager_Define.RedisServerList.Add(new mSeed.RedisManager.RedisEndpoint(parser.GetSetting("Redis", "user_redis"), parser.GetSetting("Redis", "user_redis_port"), DataManager_Define.RedisServerAlias_User));
                DataManager_Define.RedisServerList.Add(new mSeed.RedisManager.RedisEndpoint(parser.GetSetting("Redis", "system_redis"), parser.GetSetting("Redis", "system_redis_port"), DataManager_Define.RedisServerAlias_System));
                DataManager_Define.RedisServerList.Add(new mSeed.RedisManager.RedisEndpoint(parser.GetSetting("Redis", "ranking_redis"), parser.GetSetting("Redis", "ranking_redis_port"), DataManager_Define.RedisServerAlias_Ranking));
                RedisConst.SetRedisInstance();
                RedisConst.GetRedisInstance().SetPrefixTag(string.Format("{0}_{1}", DataManager_Define.RedisTagPrefix, server_group_id));
            }
        }

        newMessage = CommonDB;
        return bLog;
    }

    public string GetCommonLog()
    {
        if (CommonLogDB != null)
            return CommonLogDB;
        else
            return "";
    }

    public string GetShardingDB(int shardingIndex)
    {
        shardingIndex = 1;
        if (shardingIndex < 1 || shardingIndex > ShardingCount)
            return "";

        else
            return Sharding[shardingIndex - 1];
    }

    public string GetLogDB(int shardingIndex)
    {
        shardingIndex = 1;
        if (shardingIndex < 1 || shardingIndex > ShardingCount)
            return "";

        else
            return log[shardingIndex - 1];
    }

    public string GetCheckURL(int shardingIndex)
    {
        shardingIndex = 1;
        if (shardingIndex < 1 || shardingIndex > ShardingCount)
            return "";

        else
            return url[shardingIndex - 1];
    }

    public string GetCGPURL()
    {
        if (cgpURL != "" || cgpURL != null)
        {
            return cgpURL;
        }
        else
        {
            return "";
        }
    }

    public string GetLogDirectory()
    {
        if (LogDirectory != "" || LogDirectory != null)
        {
            return LogDirectory;
        }
        else
        {
            return "";
        }
    }

    public string GetLogOnOff()
    {
        if (LogOnOff != "" || LogOnOff != null)
        {
            return LogOnOff;
        }
        else
        {
            return "";
        }
    }

    public long TheSoulGlobalDBInit(ref TxnBlock TB)
    {
        return server_group_id;
    }

    public bool TheSoulDBInitGlobal(ref TxnBlock TB)
    {
        string savePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        TheSoulDBcon.GetInstance().IniFileLoad(savePath);

        SearchDB.DBOpen(ref TB, setGlobalDB, DataManager_Define.GlobalDB);
        return true;
    }

    public bool TheSoulDBInitFromGlobal(ref TxnBlock TB, int GroupID = 0, bool bForceInit = false)
    {
        string savePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        TheSoulDBcon.GetInstance().IniFileLoad(savePath);

        SearchDB.DBOpen(ref TB, setGlobalDB, DataManager_Define.GlobalDB);
        int findGroupID = GroupID > 0 ? GroupID : server_group_id;

        if (!DBConnList.ContainsKey(findGroupID) || bForceInit)
        {
            List<server_config> serverList = GlobalManager.GetServerList(ref TB);
            List<server_db_config> serverDBList = GlobalManager.GetServerDBConfigList(ref TB);
            List<DBEndpoint> setNewDBEndPoint = new List<DBEndpoint>();

            var findDB = serverList.Find(serverInfo => Global_Define.GlobalServerType[serverInfo.server_type] == Global_Define.eServerType.GAME_DB && serverInfo.server_group_id == findGroupID);
            if(findDB != null)
            {                
#if DEBUG
                string setHost = !string.IsNullOrEmpty(findDB.server_public_ip) ? (findDB.server_public_ip + "," + findDB.server_public_port) : (findDB.server_private_ip + "," + findDB.server_private_port);
#else
                string setHost = GroupID > Global_Define.InternationalGroupIDStartPos && bForceInit ? !string.IsNullOrEmpty(findDB.server_public_ip) ? (findDB.server_public_ip + "," + findDB.server_public_port) : (findDB.server_private_ip + "," + findDB.server_private_port)
                                                :string.IsNullOrEmpty(findDB.server_private_ip) ? (findDB.server_public_ip + "," + findDB.server_public_port) : (findDB.server_private_ip + "," + findDB.server_private_port);
#endif
                if (GlobalManager.CurrentDB == null || bForceInit)
                    GlobalManager.CurrentDB = serverDBList.Find(db => db.server_group_id == findGroupID);

                setNewDBEndPoint.Add(new DBEndpoint(setHost, GlobalManager.CurrentDB == null ? DataManager_Define.CommonDB : GlobalManager.CurrentDB.common_db_name, findDB.server_auth_id, findDB.server_auth_pw, DataManager_Define.CommonDB));
                setNewDBEndPoint.Add(new DBEndpoint(setHost, GlobalManager.CurrentDB == null ? DataManager_Define.ShardingDB : GlobalManager.CurrentDB.sharding_db_name, findDB.server_auth_id, findDB.server_auth_pw, DataManager_Define.ShardingDB));
                setNewDBEndPoint.Add(new DBEndpoint(setHost, GlobalManager.CurrentDB == null ? DataManager_Define.LogDB : GlobalManager.CurrentDB.log_db_name, findDB.server_auth_id, findDB.server_auth_pw, DataManager_Define.LogDB));
            }

            var findRedisUser = serverList.Find(serverInfo => Global_Define.GlobalServerType[serverInfo.server_type] == Global_Define.eServerType.REDIS_DB && serverInfo.server_group_id == findGroupID);
            var findRedisSystem = serverList.Find(serverInfo => Global_Define.GlobalServerType[serverInfo.server_type] == Global_Define.eServerType.REDIS_SYSTEM_DB && serverInfo.server_group_id == findGroupID);
            var findRedisRanking = serverList.Find(serverInfo => Global_Define.GlobalServerType[serverInfo.server_type] == Global_Define.eServerType.REDIS_RANKGIN_DB && serverInfo.server_group_id == findGroupID);

            DataManager_Define.RedisServerList = new System.Collections.Generic.List<mSeed.RedisManager.RedisEndpoint>();
            if(findRedisUser != null)
            {
#if DEBUG
                string setHost = !string.IsNullOrEmpty(findRedisUser.server_public_ip) ? (findRedisUser.server_public_ip) : (findRedisUser.server_private_ip);
                string setPort =(findRedisUser.server_public_port).ToString(); 
#else
                string setHost = GroupID > Global_Define.InternationalGroupIDStartPos && bForceInit ? !string.IsNullOrEmpty(findRedisUser.server_public_ip) ? (findRedisUser.server_public_ip) : (findRedisUser.server_private_ip)
                                                : string.IsNullOrEmpty(findRedisUser.server_private_ip) ? (findRedisUser.server_public_ip) : (findRedisUser.server_private_ip);
                string setPort = GroupID > Global_Define.InternationalGroupIDStartPos && bForceInit ? (findRedisUser.server_public_port).ToString() : (findRedisUser.server_private_port).ToString();
#endif
                DataManager_Define.RedisServerList.Add(new mSeed.RedisManager.RedisEndpoint(setHost, setPort, DataManager_Define.RedisServerAlias_User));
            }

            if (findRedisSystem != null)
            {
                
#if DEBUG
                string setHost = !string.IsNullOrEmpty(findRedisSystem.server_public_ip) ? (findRedisSystem.server_public_ip) : (findRedisSystem.server_private_ip);
                string setPort = (findRedisSystem.server_public_port).ToString();
#else
                string setHost = GroupID > Global_Define.InternationalGroupIDStartPos && bForceInit ? !string.IsNullOrEmpty(findRedisSystem.server_public_ip) ? (findRedisSystem.server_public_ip) : (findRedisSystem.server_private_ip) :
                                    string.IsNullOrEmpty(findRedisSystem.server_private_ip) ? (findRedisSystem.server_public_ip) : (findRedisSystem.server_private_ip);
                string setPort = GroupID > Global_Define.InternationalGroupIDStartPos && bForceInit ? (findRedisSystem.server_public_port).ToString() : (findRedisSystem.server_private_port).ToString();
#endif

                DataManager_Define.RedisServerList.Add(new mSeed.RedisManager.RedisEndpoint(setHost, setPort, DataManager_Define.RedisServerAlias_System));
            }

            if (findRedisRanking != null)
            {
                
#if DEBUG
                string setHost = !string.IsNullOrEmpty(findRedisRanking.server_public_ip) ? (findRedisRanking.server_public_ip) : (findRedisRanking.server_private_ip);
                string setPort = (findRedisRanking.server_public_port).ToString();
#else
                string setHost = GroupID > Global_Define.InternationalGroupIDStartPos && bForceInit ? !string.IsNullOrEmpty(findRedisRanking.server_public_ip) ? (findRedisRanking.server_public_ip) : (findRedisRanking.server_private_ip) :
                                    string.IsNullOrEmpty(findRedisRanking.server_private_ip) ? (findRedisRanking.server_public_ip) : (findRedisRanking.server_private_ip);
                string setPort = GroupID > Global_Define.InternationalGroupIDStartPos && bForceInit ? (findRedisRanking.server_public_port).ToString() : (findRedisRanking.server_private_port).ToString();
#endif

                DataManager_Define.RedisServerList.Add(new mSeed.RedisManager.RedisEndpoint(setHost, setPort, DataManager_Define.RedisServerAlias_Ranking));
            }

            RedisConst.SetRedisInstance();
            RedisConst.GetRedisInstance().SetPrefixTag(string.Format("{0}_{1}", DataManager_Define.RedisTagPrefix, server_group_id));

            if (setNewDBEndPoint.Count > 0)
            {
                if (DBConnList.ContainsKey(findGroupID))
                    DBConnList[findGroupID] = setNewDBEndPoint;
                else
                    DBConnList.Add(findGroupID, setNewDBEndPoint);
            }
        }

        if (DBConnList.ContainsKey(findGroupID))
        {
            foreach (DBEndpoint setDB in DBConnList[findGroupID])
            {
                SearchDB.DBOpen(ref TB, setDB);
            }
        }

        if (!RedisConst.ForceInit && server_group_id == findGroupID && DataManager_Define.RedisServerList.Count > 0)
        {
            RedisConst.SetRedisInstance();
            RedisConst.GetRedisInstance().SetPrefixTag(string.Format("{0}_{1}", DataManager_Define.RedisTagPrefix, server_group_id));
        }

        return true;
    }

    public static void TheSoulRedisReconnect()
    {
        if (DataManager_Define.RedisServerList.Count > 0)
        {
            RedisConst.SetRedisInstance();
            RedisConst.GetRedisInstance().SetPrefixTag(string.Format("{0}_{1}", DataManager_Define.RedisTagPrefix, server_group_id));
        }
    }

    public bool TheSoulDBInit(ref TxnBlock TB, long AID)
    {
        string DBconString = "";
        string savePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        bool bLog = TheSoulDBcon.GetInstance().GetIniFileLoad(savePath, ref DBconString);
        SearchDB OpenDB = new SearchDB();

        // GlobalDB Open
        OpenDB.GlobalDBOpen(ref TB, setGlobalDB);
    
        //CommonDBOpen
        OpenDB.CommonDBOpen(ref TB, DBconString);

        // get user DB sharding Info
        GetAccountDB ShardingInfo = TheSoul.DataManager.AccountManager.GetShardingInfo(ref TB, AID);

        //ShardingDBOpen
        OpenDB.ShardingDBOpen(ref TB, System.Convert.ToInt16(ShardingInfo.RetDB_INDEX));

        return bLog;
    }

    public void TheSoulLogDBInit(ref TxnBlock TB)
    {
        string DBconString = "";
        string savePath = System.Web.HttpContext.Current.Request.PhysicalApplicationPath;
        TheSoulDBcon.GetInstance().GetIniFileLoad(savePath, ref DBconString);
        SearchDB OpenDB = new SearchDB();

        //LogDBOpen
        OpenDB.LogDBOpen(ref TB);
    }
}