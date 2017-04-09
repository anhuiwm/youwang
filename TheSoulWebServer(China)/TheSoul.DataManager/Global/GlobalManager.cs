using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Tools;

using System.Security.Cryptography;
using System.Net;
using ServiceStack.Text;

namespace TheSoul.DataManager.Global
{
    public class Global_Define
    {
        public static readonly Dictionary<ePlatformType, auth_platform_info> appIDs = new Dictionary<ePlatformType, auth_platform_info>()
        {
            { ePlatformType.EPlatformType_Google, new auth_platform_info(ePlatformType.EPlatformType_Google, "darkblaze", "817162163610-639pkv0va0injkhocf9j15soajskdikj.apps.googleusercontent.com", "riH4s_SaxklZ4qE6QlPzb1dp") },
            { ePlatformType.EPlatformType_Facebook, new auth_platform_info(ePlatformType.EPlatformType_Facebook, "darkblaze", "app_id", "app_secret") },
        };

        public const string RetKey_GlobalServerList = "serverlist";
        public const string RetKey_GlobalServerGroupList = "server_group_list";
        public const string RetKey_GlobalNoticeList = "noticelist";
        public const string RetKey_GlobalNoticeBody = "noticebody";
        public const string RetKey_GlobalDevIPList = "ip_list";

        public const string GlobalDBName = "global";
        public const string Server_Group_TableName = "server_group_config";
        public const string Server_Group_Active_TableName = "server_group_active_platform";
        public const string Server_Config_TableName = "server_config";
        public const string Server_Config_IPv6_TableName = "server_config_ipv6";
        public const string Server_DB_Config_TableName = "server_db_config";

        public const string User_Account_TableName = "user_account_config";
        public const string User_Account_Restrict_TableName = "user_account_restrict";
        public const string User_Platform_ID_TableName = "user_platform_id";
        public const string User_Character_TableName = "user_character_config";
        public const string User_PlayServer_TableName = "user_play_server_config";
        public const string Admin_GlobalNotice_TableName = "Admin_GlobalNotice";

        public const string User_Guest_Auth_ID_TableName = "user_guest_auth_id";
        public const string Snail_IP_TableName = "snail_ip_table";

        public const int ServerStatus_Over_Limit = 4500;
        public const int ServerStatus_Under_Limit = 4000;
        public const int InternationalGroupIDStartPos = 100;

        public const string Default_Sharding_DBName = "sharding";
        public const string Default_Common_DBName = "common";
        public const string Default_Log_DBName = "log";
        // 2 digit Enum,  (Actor + Status)
        //                 1:GM
        //                 2:Manager

        /// <summary>
        ///  服务器状态对应枚举
        /// </summary>
        public enum eServerStatus
        {
            None = -1,
            Normal = 0,
            Hot = 11,
            Recommand = 12,
            New = 13,
            Maintenance = 14,
            Hide = 15,
            PatchStart = 21,   /// 
            PatchEnd = 22,
            ServerError = 23,
        }

        /// <summary>
        ///  服务器类型，对应配置GLOBAL数据库的serverType
        /// </summary>
        public enum eServerType
        {
            NONE_SERVER = -1,
            BASE_SERVER = 0,
            LOGIN_SERVER = 1,
            COMMUNITY_SERVER = 2,
            GAME_SERVER = 3,
            MANAGER_SERVER = 4,
            MATCHING_SERVER = 5,
            CACHE_SERVER = 6,
            GLOBAL_GAME_SERVER = 7,
            GLOBAL_MATCHING_SERVER = 8,
            WEBSERVER = 9,
            MANAGER_AGENT = 10,
            MANAGER_TOOL = 11,
            GLOBAL_DB = 12,
            GAME_DB = 13,
            LOG_DB = 14,
            REDIS_DB = 15,
            GLOBAL_FTP = 16,
            CHARGE_SDK_SERVER = 17,
            COMMON_DB = 18,
            SHARDING_DB = 19,
            REDIS_SYSTEM_DB = 20,
            REDIS_RANKGIN_DB = 21,
            //===================
            SERVER_TYPE_MAX,// LAST
        }

        public static Dictionary<string, eServerType> GlobalServerType = new Dictionary<string, eServerType>()
        {
            { "cs_base", eServerType.BASE_SERVER},
            { "cs_login", eServerType.LOGIN_SERVER},
            { "cs_community", eServerType.COMMUNITY_SERVER },
            { "cs_game", eServerType.GAME_SERVER },
            { "cs_manager", eServerType.MANAGER_SERVER },
            { "cs_matching", eServerType.MATCHING_SERVER },
            { "cs_cache", eServerType.CACHE_SERVER},
            { "global_game", eServerType.GLOBAL_GAME_SERVER},
            { "global_matching", eServerType.GLOBAL_MATCHING_SERVER},
            { "cs_agent", eServerType.MANAGER_AGENT },
            { "cs_tool", eServerType.MANAGER_TOOL },
            { "global_db", eServerType.GLOBAL_DB },
            { "db_server", eServerType.GAME_DB },
            { "log_db_server", eServerType.LOG_DB },
            { "redis_server", eServerType.REDIS_DB },
            { "global_ftp", eServerType.GLOBAL_FTP },
            { "charge_server", eServerType.CHARGE_SDK_SERVER},
            { "common_db_server", eServerType.COMMON_DB },
            { "sharding_db_server", eServerType.SHARDING_DB },
            { "redis_server_system", eServerType.REDIS_SYSTEM_DB },
            { "redis_server_ranking", eServerType.REDIS_RANKGIN_DB },
            { "web_server", eServerType.WEBSERVER },
        };

        /// <summary>
        ///  登录平台类型
        /// </summary>
        public enum ePlatformType
        {
            //====================== drop out of the game
            EPlatformType_DropAccount = -1,
            //====================== for Snail & Debug
            EPlatformType_UnityEditer = 0,
            EPlatformType_SnailSDK = 1,
            //====================== for mSeed publishing
            EPlatformType_Guest_Editer = 100,       /// 游客编辑器登录
            EPlatformType_Guest = 101,              /// 游客登录
            EPlatformType_Google = 102,         //全球谷歌（谷歌没有ios）
            EPlatformType_Facebook = 103,

            //new
            EPlatformType_iosFacebook = 113,//全球ios facebook
            //======================
            EPlatformType_UC = 201,	// UC
            EPlatformType_360 = 202,	// 360
            EPlatformType_Baidu = 203,	// 百度
            EPlatformType_Xiaomi = 204,	// 小米
            EPlatformType_Oppo = 205,	// oppo
            EPlatformType_Vivo = 206,	// VIVO
            EPlatformType_Huawei = 207,	// 华为
            EPlatformType_Lenovo = 208,	// 联想
            EPlatformType_Gionee = 209,	// 金立
            EPlatformType_Coolpad = 210,	// 酷派dksy 
            EPlatformType_Meizu = 211,	// 魅族
            EPlatformType_Le = 212,	// " 乐视(러쓰) "
            EPlatformType_Tencent = 213,	// 텐센트
            //======================
            EPlatformType_TW_Guest_Editor = 300,
            EPlatformType_TW_Guest = 301,
            EPlatformType_TW_Google = 302,
            EPlatformType_TW_Facebook = 303,
            EPlatformType_TW_3rd = 304,

            //====================== for mfun  publishing(新马泰需要接入)
            EPlatformType_mfun_aosGoogle = 502,//新马泰谷歌
            EPlatformType_mfun_aosFacebook = 503,  //新马泰安卓facebook   
            EPlatformType_mfun_iosFacebook = 513,//新马泰苹果facebook  

            //====================== for yuenan publishing(越南需要接入)
            EPlatformType_yuenan_aosGoogle = 602,//越南谷歌（谷歌没有ios）
            EPlatformType_yuenan_aosFacebook = 603,//越南安卓facebook
            EPlatformType_yuenan_iosFacebook = 613,//越南苹果facebook


            EPlatformType_KT_CLOUD_BOT = 10000,
            EPlatformType_CNT,
        };

        /// <summary>
        ///  公告类型，系统公告，事件公告，升级公告
        /// </summary>
        public enum eGlobalNoticeTag
        {
            NONE,
            SYSTEM,
            EVENT,
            PROMOTION,
        }

        public static Dictionary<string, eGlobalNoticeTag> GlobalNoticeTag = new Dictionary<string, eGlobalNoticeTag>()
        {
            { string.Empty, eGlobalNoticeTag.NONE },
            { "None", eGlobalNoticeTag.NONE },
            { "System", eGlobalNoticeTag.SYSTEM },
            { "Event", eGlobalNoticeTag.EVENT },
            { "Promotion", eGlobalNoticeTag.PROMOTION }
        };

        /// <summary>
        /// 公告状态，火热公告，新公告
        /// </summary>
        public enum eGlobalNoticeType
        {
            NONE,
            HOT,
            NEW,
        }

        public static Dictionary<string, eGlobalNoticeType> GlobalNoticeType = new Dictionary<string, eGlobalNoticeType>()
        {
            { string.Empty, eGlobalNoticeType.NONE },
            { "None", eGlobalNoticeType.NONE },
            { "Hot", eGlobalNoticeType.HOT },
            { "New", eGlobalNoticeType.NEW },
        };

        public enum eAccountRestrictType
        {
            LOGIN = 1,
            CHAT = 2,
            LOGIN_REMOVE = 11,
            CHAT_REMOVE = 12,
        }

        /// <summary>
        ///  支付操作系统
        ///  安卓，iOS，韩国OneStore，全球安卓，全球IOS，开发
        /// </summary>
        [Flags]
        public enum eNoticeBillingType
        {
            None = 0,
            Android = 1 << 0,
            iOS = 1 << 1,
            OneStore = 1 << 2,
            Global_Android = 1 << 3,
            Global_iOS = 1 << 4,
            Dev_Debug = 1 << 5,
        }

        public static Dictionary<Shop_Define.eBillingType, eNoticeBillingType> GlobalNoticeBillingType = new Dictionary<Shop_Define.eBillingType, eNoticeBillingType>()
        {
            { Shop_Define.eBillingType.UnityDebug, eNoticeBillingType.Dev_Debug},
            { Shop_Define.eBillingType.Kr_aOS_Google, eNoticeBillingType.Android },
            { Shop_Define.eBillingType.Kr_iOS_Appstore, eNoticeBillingType.iOS },
            { Shop_Define.eBillingType.Kr_aOS_OneStore, eNoticeBillingType.OneStore },
            { Shop_Define.eBillingType.Global_aOS_Google, eNoticeBillingType.Global_Android },
            { Shop_Define.eBillingType.Global_iOS_Appstore, eNoticeBillingType.Global_iOS },
        };
    }
}

namespace TheSoul.DataManager.Global
{
    public class snail_ip_table
    {
        public int idx { get; set; }
        public string ip_address { get; set; }
        public snail_ip_table() { }
        public snail_ip_table(string set_ip) { ip_address = set_ip; }
    }

    public class server_group_config
    {
        public long server_group_id { get; set; }
        public string server_group_name { get; set; }
        public int server_group_status { get; set; }
        public long user_account_idx { get; set; }
        public List<server_config> server_info;
    }

    public class server_group_config_snail : server_group_config
    {
        public int billing_platform_type { get; set; }
        public int target_version { get; set; }
    }

    public class server_config
    {
        public long server_idx { get; set; }
        public string server_name { get; set; }
        public string server_private_ip { get; set; }
        public int? server_private_port { get; set; }
        public string server_public_ip { get; set; }
        public int? server_public_port { get; set; }
        public int server_group_id { get; set; }
        public string server_type { get; set; }
        public string server_auth_id { get; set; }
        public string server_auth_pw { get; set; }
        public int server_status { get; set; }
        public DateTime reg_date { get; set; }
        public string server_private_ipv6 { get; set; }
        public int? server_private_ipv6_port { get; set; }
        public string server_public_ipv6 { get; set; }
        public int? server_public_ipv6_port { get; set; }
    }

    public class server_db_config
    {
        public int server_group_id { get; set; }
        public string sharding_db_name { get; set; }
        public string common_db_name { get; set; }
        public string log_db_name { get; set; }

        public server_db_config()
        {
            sharding_db_name = Global_Define.Default_Sharding_DBName;
            common_db_name = Global_Define.Default_Common_DBName;
            log_db_name = Global_Define.Default_Log_DBName;
        }
    }

    public class user_account_config
    {
        public long user_account_idx { get; set; }
        public int platform_type { get; set; }
        public string platform_user_id { get; set; }
        public int user_account_status { get; set; }
        public DateTime reg_date { get; set; }
    }

    public class user_platform_id
    {
        public long platform_idx { get; set; }
        public string platform_user_id { get; set; }
        public DateTime reg_date { get; set; }

        public user_platform_id(long setidx)
        {
            platform_idx = setidx;
            platform_user_id = string.Empty;
            reg_date = DateTime.Now;
        }

        public user_platform_id(string setID)
        {
            platform_idx = 0;
            platform_user_id = setID;
            reg_date = DateTime.Now;
        }
    }

    public class user_account_restrict
    {
        public long user_account_idx { get; set; }
        public DateTime login_restrict_enddate { get; set; }
        public DateTime login_restrict_reg_date { get; set; }
        public DateTime chat_restrict_endate { get; set; }
        public DateTime chat_restrict_reg_date { get; set; }

        public user_account_restrict(long setidx = 0)
        {
            user_account_idx = setidx;
            login_restrict_enddate = login_restrict_reg_date = chat_restrict_endate = chat_restrict_reg_date = DateTime.Now;
        }
    }

    public class ret_user_aid
    {
        public long aid { get; set; }
        public string userid { get; set; }
        public string encryptkey { get; set; }
        public long loginrestrict { get; set; }
        public long chatrestrict { get; set; }
        public int unresolve_count { get; set; }
        public string operation { get; set; }
        public int resultcode { get; set; }
        public byte result { get; set; }
    }

    public class user_character_config
    {
        public long user_character_idx { get; set; }
        public long user_account_idx { get; set; }
        public long server_group_id { get; set; }
        public DateTime reg_date { get; set; }
    }

    public class user_play_server_config : server_group_config
    {
        public long user_playserver_idx { get; set; }
        public string user_server_nickname { get; set; }
        public int user_server_status { get; set; }
        public DateTime reg_date { get; set; }
    }

    public class server_group_info
    {
        public long server_group_id { get; set; }
        public string server_group_name { get; set; }
        public int server_group_status { get; set; }
        //public int isplay { get; set; }
        public List<server_info> server_info { get; set; }
    }

    public class server_info
    {
        public string server_public_ip { get; set; }
        public int? server_public_port { get; set; }
        public string server_public_ipv6 { get; set; }
        public int? server_public_ipv6_port { get; set; }
        public string server_type { get; set; }
        public int server_status { get; set; }
    }

    public class Admin_GlobalNotice
    {
        public long idx { get; set; }
        public string noticeTag { get; set; }
        public string noticeStyle { get; set; }
        public string title { get; set; }
        public string contents { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public byte active { get; set; }
        public int orderNumber { get; set; }
        public DateTime regdate { get; set; }
        public string regid { get; set; }
        public DateTime editdate { get; set; }
        public string editid { get; set; }
        public int billing_platform_type { get; set; }
        public int target_version { get; set; }

        public Admin_GlobalNotice()
        {
            idx = 0;
            editdate = regdate = endDate = startDate = DateTime.Now;
            noticeTag = noticeStyle = title = contents = regid = editid = string.Empty;
            target_version = billing_platform_type = 0;
        }
    }

    public class RetGlobalNoticeList
    {
        public long idx { get; set; }
        public int noticeTag { get; set; }
        public int noticeStyle { get; set; }
        public string title { get; set; }

        public RetGlobalNoticeList()
        {
            idx = noticeTag = noticeStyle = 0;
            title = string.Empty;
        }

        public RetGlobalNoticeList(Admin_GlobalNotice setNotice)
        {
            idx = setNotice.idx;
            noticeTag = (int)(Global_Define.GlobalNoticeTag.ContainsKey(setNotice.noticeTag) ? Global_Define.GlobalNoticeTag[setNotice.noticeTag] : Global_Define.eGlobalNoticeTag.SYSTEM);
            noticeStyle = (int)(Global_Define.GlobalNoticeType.ContainsKey(setNotice.noticeStyle) ? Global_Define.GlobalNoticeType[setNotice.noticeStyle] : Global_Define.eGlobalNoticeType.HOT);
            title = setNotice.title;
        }

        public static string MakeRetGlobalNoticeJson(ref List<Admin_GlobalNotice> setNoticeList)
        {
            string json = string.Empty;
            List<RetGlobalNoticeList> retList = new List<RetGlobalNoticeList>();
            if (setNoticeList.Count > 0)
            {
                setNoticeList.ForEach(notice =>
                {
                    notice.title = notice.title.Replace("\\\\", "\\");
                    retList.Add(new RetGlobalNoticeList(notice));
                }
                );
            }

            json = mJsonSerializer.ToJsonString(retList);
            return json;
        }
    }

    public class user_guest_auth_id
    {
        public string auth_md5_id { get; set; }
        public string server_auth_token { get; set; }
        public string client_auth_token { get; set; }
        public string server_auth_md5 { get; set; }
        public string client_auth_md5 { get; set; }
        public DateTime reg_date { get; set; }

        public user_guest_auth_id() { }
        public user_guest_auth_id(string set_auth, string set_server, string set_client, string set_server_md5, string set_client_md5)
        {
            auth_md5_id = set_auth;
            server_auth_token = set_server;
            client_auth_token = set_client;
            server_auth_md5 = set_server_md5;
            client_auth_md5 = set_client_md5;
        }
    }

    public class auth_platform_info
    {
        public Global_Define.ePlatformType auth_type { get; set; }
        public string package_name { get; set; }
        public string app_id { get; set; }
        public string app_secret { get; set; }

        public auth_platform_info()
        {
            auth_type = Global_Define.ePlatformType.EPlatformType_UnityEditer;
            app_id = app_secret = "";
        }
        public auth_platform_info(Global_Define.ePlatformType setType, string setName, string setID, string setSecret)
        {
            package_name = setName;
            auth_type = setType;
            app_id = setID;
            app_secret = setSecret;
        }
    }
}

namespace TheSoul.DataManager.Global
{
    public class MD5Tool
    {
        public static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class GlobalManager
    {
        const string GlobalIniFileName = "TheSoulDBConnect.ini";
        private static server_db_config currentDB = null;

        public static server_db_config CurrentDB
        {
            get { return GlobalManager.currentDB; }
            set
            {
                if (value == null)
                    GlobalManager.currentDB = new server_db_config();
                else
                    GlobalManager.currentDB = value;
            }
        }

        public static server_db_config GM_CurrentDB
        {
            get { return GlobalManager.currentDB; }
            set { GlobalManager.currentDB = value; }
        }


        public static Global_Define.eServerStatus lastStatus = Global_Define.eServerStatus.None;

        public static long GetGlobalServerIni(ref TxnBlock TB, string savePath)
        {
            long ServerGroupID = 1;
            DBEndpoint setDB = new DBEndpoint();
            string AppPath = "";
            AppPath = savePath + @"\dbcon\";//원본 위치
            string sourceFile = Path.Combine(AppPath, GlobalIniFileName);
            TheSoul.DataManager.Tools.IniParser parser = new TheSoul.DataManager.Tools.IniParser(sourceFile);
            setDB.Host = parser.GetSetting("GlobalDB", "host");
            setDB.Database = parser.GetSetting("GlobalDB", "db");
            setDB.UserID = parser.GetSetting("GlobalDB", "id");
            setDB.UserPW = parser.GetSetting("GlobalDB", "pw");
            string gruopid = parser.GetSetting("GlobalDB", "server_group_id");
            ServerGroupID = System.Convert.ToInt64(gruopid);

            TB.IsoLevel = IsolationLevel.ReadUncommitted;     // set transaction IsolationLevel (default ReadUncommited)
            TB.DBConn(setDB, "global");        // make alias name for this connection
            return ServerGroupID;
        }

        public static List<server_config> GetServerList(ref TxnBlock TB, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format(@"
SELECT SC_IPV4.*,
 ISNULL(SC_IPV6.server_private_ipv6, '') as server_private_ipv6, 
 ISNULL(SC_IPV6.server_private_ipv6_port, 0) as server_private_ipv6_port, 
 ISNULL(SC_IPV6.server_public_ipv6, '') as server_public_ipv6, 
 ISNULL(SC_IPV6.server_public_ipv6_port, 0) as server_public_ipv6_port
  FROM {0} AS SC_IPV4 WITH(NOLOCK)
LEFT OUTER JOIN
{1} AS SC_IPV6 WITH(NOLOCK)
ON SC_IPV4.server_idx = SC_IPV6.server_idx
 ", Global_Define.Server_Config_TableName, Global_Define.Server_Config_IPv6_TableName);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<server_config>(ref TB, setQuery, dbkey);
        }

        public static List<snail_ip_table> GetSnailIPList(ref TxnBlock TB, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format("SELECT ip_address FROM {0} WITH(NOLOCK) ", Global_Define.Snail_IP_TableName);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<snail_ip_table>(ref TB, setQuery, dbkey);
        }

        public static List<server_config> GetServerListByGroup(ref TxnBlock TB, int serverGroupID, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE server_group_id = {1}", Global_Define.Server_Config_TableName, serverGroupID);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<server_config>(ref TB, setQuery, dbkey);
        }

        public static List<server_group_config> GetServerGroupList(ref TxnBlock TB, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) ", Global_Define.Server_Group_TableName);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<server_group_config>(ref TB, setQuery, dbkey);
        }

        public static List<server_group_config_snail> GetServerGroupList_Snail(ref TxnBlock TB, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) ", Global_Define.Server_Group_TableName);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<server_group_config_snail>(ref TB, setQuery, dbkey);
        }

        public static List<server_group_config_snail> GetServerGroupList_Snail(ref TxnBlock TB, int BillingType, int TargetVersion, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format(@"SELECT SG.*, ISNULL(AP.[billing_platform_type], 0) as [billing_platform_type], ISNULL(AP.[target_version], 0) as [target_version]
                                                 FROM {0} AS SG WITH(NOLOCK) 
                                                 LEFT OUTER JOIN {1} AS AP WITH(NOLOCK, INDEX([IDX_server_group_active_platform_group])) 
                                                 ON SG.server_group_id = AP.server_group_id
                                                 AND AP.[billing_platform_type] = {2}
                                                 AND AP.[target_version] = {3}
                                            ", Global_Define.Server_Group_TableName, Global_Define.Server_Group_Active_TableName, BillingType, TargetVersion);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<server_group_config_snail>(ref TB, setQuery, dbkey);
        }

        public static List<server_group_config_snail> GetServerGroupAllList_Snail(ref TxnBlock TB, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format(@"SELECT SG.*, ISNULL(AP.[billing_platform_type], 0) as [billing_platform_type], ISNULL(AP.[target_version], 0) as [target_version]
                                                 FROM {0} AS SG WITH(NOLOCK) 
                                                 LEFT OUTER JOIN {1} AS AP WITH(NOLOCK, INDEX([IDX_server_group_active_platform_group])) 
                                                 ON SG.server_group_id = AP.server_group_id
                                            ", Global_Define.Server_Group_TableName, Global_Define.Server_Group_Active_TableName);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<server_group_config_snail>(ref TB, setQuery, dbkey);
        }

        public static List<server_group_config_snail> GetServerGroupList(ref TxnBlock TB, long AID, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format("SELECT SG.*, US.user_account_idx FROM {0} AS SG WITH(NOLOCK) LEFT OUTER JOIN {1} AS US WITH(NOLOCK) ON SG.server_group_id = US.server_group_id AND US.user_account_idx = {2}", Global_Define.Server_Group_TableName, Global_Define.User_PlayServer_TableName, AID);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<server_group_config_snail>(ref TB, setQuery, dbkey);
        }

        public static List<server_db_config> GetServerDBConfigList(ref TxnBlock TB, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)", Global_Define.Server_DB_Config_TableName);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<server_db_config>(ref TB, setQuery, dbkey);
        }

        public static List<Admin_GlobalNotice> GetAdminNoticeList(ref TxnBlock TB, int VersionNo, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK, INDEX(IDX_Admin_GlobalNotice_with_date)) 
                                                WHERE startDate <= GETDATE() AND endDate >= GETDATE() And active > 0 AND target_version = {1} 
                                                    ORDER BY orderNumber DESC", Global_Define.Admin_GlobalNotice_TableName, VersionNo);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Admin_GlobalNotice>(ref TB, setQuery, dbkey);
        }

        public static Admin_GlobalNotice GetAdminNoticeBody(ref TxnBlock TB, long noticeidx, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE idx = {1}", Global_Define.Admin_GlobalNotice_TableName, noticeidx);
            Admin_GlobalNotice retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<Admin_GlobalNotice>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new Admin_GlobalNotice();
        }

        public static user_account_restrict GetUserRestrict(ref TxnBlock TB, long user_idx, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE user_account_idx = {1}", Global_Define.User_Account_Restrict_TableName, user_idx);
            user_account_restrict retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<user_account_restrict>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new user_account_restrict(user_idx);
        }

        public static user_account_config GetUserAccountConfig(ref TxnBlock TB, long user_idx, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE user_account_idx = {1}", Global_Define.User_Account_TableName, user_idx);
            user_account_config retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<user_account_config>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new user_account_config();
        }

        public static user_account_config GetUserAccountConfigByPlatformID(ref TxnBlock TB, string platform_id, Global_Define.ePlatformType platform_type, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE platform_user_id = N'{1}' AND platform_type = {2}", Global_Define.User_Account_TableName, platform_id, (int)platform_type);
            user_account_config retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<user_account_config>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new user_account_config();
        }

        public static user_platform_id GetUserPlatformInfo(ref TxnBlock TB, long platform_idx, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE platform_idx = {1}", Global_Define.User_Platform_ID_TableName, platform_idx);
            user_platform_id retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<user_platform_id>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new user_platform_id(platform_idx);
        }


        public static user_platform_id GetUserPlatformInfo_ByPlatformID(ref TxnBlock TB, string platform_user_id, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK, INDEX(IDX_PlatformID)) WHERE platform_user_id = '{1}'", Global_Define.User_Platform_ID_TableName, platform_user_id);
            user_platform_id retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<user_platform_id>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new user_platform_id(platform_user_id);
        }

        public static Result_Define.eResult SetUserRestrict(ref TxnBlock TB, long user_idx, int RestrictTime, Global_Define.eAccountRestrictType RestrictType, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Empty;
            if (RestrictType == Global_Define.eAccountRestrictType.LOGIN)
            {
                setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON user_account_idx = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    login_restrict_enddate = CASE WHEN login_restrict_enddate > GETDATE() THEN DATEADD(MINUTE, {2}, login_restrict_enddate) ELSE DATEADD(MINUTE, {2}, GETDATE()) END,
                                                    login_restrict_reg_date = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT (user_account_idx, login_restrict_enddate, login_restrict_reg_date, chat_restrict_endate, chat_restrict_reg_date)
                                                   VALUES ('{1}', DATEADD(MINUTE, {2}, GETDATE()), GETDATE(), GETDATE(), GETDATE());
                                    ", Global_Define.User_Account_Restrict_TableName, user_idx, RestrictTime);
            }
            else if (RestrictType == Global_Define.eAccountRestrictType.CHAT)
            {
                setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON user_account_idx = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    chat_restrict_endate = CASE WHEN chat_restrict_endate > GETDATE() THEN DATEADD(MINUTE, {2}, chat_restrict_endate) ELSE DATEADD(MINUTE, {2}, GETDATE()) END,
                                                    chat_restrict_reg_date = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT (user_account_idx, login_restrict_enddate, login_restrict_reg_date, chat_restrict_endate, chat_restrict_reg_date)
                                                   VALUES ('{1}', GETDATE(), GETDATE(), DATEADD(MINUTE, {2}, GETDATE()), GETDATE());
                                    ", Global_Define.User_Account_Restrict_TableName, user_idx, RestrictTime);
            }
            else if (RestrictType == Global_Define.eAccountRestrictType.LOGIN_REMOVE)
            {
                setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON user_account_idx = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    login_restrict_enddate = GETDATE(),
                                                    login_restrict_reg_date = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT (user_account_idx, login_restrict_enddate, login_restrict_reg_date, chat_restrict_endate, chat_restrict_reg_date)
                                                   VALUES ('{1}', GETDATE(), GETDATE(), GETDATE(), GETDATE());
                                    ", Global_Define.User_Account_Restrict_TableName, user_idx
                    //, RestrictTime
                                     );
            }
            else if (RestrictType == Global_Define.eAccountRestrictType.CHAT_REMOVE)
            {
                setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON user_account_idx = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    chat_restrict_endate = GETDATE(),
                                                    chat_restrict_reg_date = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT (user_account_idx, login_restrict_enddate, login_restrict_reg_date, chat_restrict_endate, chat_restrict_reg_date)
                                                   VALUES ('{1}', GETDATE(), GETDATE(), GETDATE(), GETDATE());
                                    ", Global_Define.User_Account_Restrict_TableName, user_idx
                    //, RestrictTime
                                     );
            }

            if (string.IsNullOrEmpty(setQuery))
                return Result_Define.eResult.System_Unknown_Operation;

            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static string GetReqeustURL(string Url, string dataParams, bool doPost = true)
        {
            try
            {
                HttpWebRequest request;
                if (doPost)
                {
                    /* POST */
                    request = (HttpWebRequest)WebRequest.Create(Url);
                    request.Method = "POST";    // 기본값 "GET"
                    request.ContentType = "application/x-www-form-urlencoded";

                    // request param to byte array for IO stream
                    byte[] byteDataParams = UTF8Encoding.UTF8.GetBytes(dataParams);
                    request.ContentLength = byteDataParams.Length;

                    // reqesut byte array write to IO stream
                    Stream stDataParams = request.GetRequestStream();
                    stDataParams.Write(byteDataParams, 0, byteDataParams.Length);
                    stDataParams.Close();
                }
                else
                {
                    /* GET */
                    request = (HttpWebRequest)WebRequest.Create(Url + "?" + dataParams);
                    request.Method = "GET";
                }

                // 요청, 응답 받기
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                // 응답 Stream 읽기
                Stream stReadData = response.GetResponseStream();
                StreamReader srReadData = new StreamReader(stReadData, Encoding.Default);
                return srReadData.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                //return string.Format("http request fail for " + ex.Message);
                //return Url + "?" + dataParams;
                return string.Empty;
            }
        }

        private static void RemoveServerStateCache(long serverID)
        {
            string setKey = string.Format("{0}_LastStatus", Global_Define.Server_Group_TableName);
            RedisConst.GetRedisInstance().RemoveHashItem(DataManager_Define.RedisServerAlias_System, setKey, serverID.ToString());
        }

        public static Result_Define.eResult SetServerState(ref TxnBlock TB, long serverID, Global_Define.eServerStatus state = Global_Define.eServerStatus.Normal, string dbkey = Global_Define.GlobalDBName)
        {
            string setQuery = string.Format("Update {0} Set server_group_status = {1} Where server_group_id = {2}", Global_Define.Server_Group_TableName, (int)state, serverID);
            Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            if (retError == Result_Define.eResult.SUCCESS)
                RemoveServerStateCache(serverID);
            return retError;
        }

        public static server_group_config GetServerGroupConfig(ref TxnBlock TB, long serverGroupID, string dbkey = Global_Define.GlobalDBName)
        {
            string setKey = string.Format("{0}_LastStatus", Global_Define.Server_Group_TableName);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE server_group_id = {1}", Global_Define.Server_Group_TableName, serverGroupID);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<server_group_config>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, serverGroupID.ToString(), setQuery, dbkey);
        }
    }
}