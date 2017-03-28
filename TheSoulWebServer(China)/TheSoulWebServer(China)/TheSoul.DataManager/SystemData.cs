using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using TheSoul.DataManager.DBClass;

namespace TheSoul.DataManager.DBClass
{
    public class Admin_Notice
    {
        public long idx { get; set; }
        public byte notice_type { get; set; }
        public string title { get; set; }
        public string contents { get; set; }
        public byte active { get; set; }
        public DateTime sdate { get; set; }
        public DateTime edate { get; set; }
        public string type { get; set; }            // admin control text notice sub type
        public int orderNum { get; set; }
        public int repeatTime { get; set; }
        public DateTime regdate { get; set; }
        public string regid { get; set; }
        public DateTime? editedate { get; set; }
        public string editeid { get; set; }
    }

    public class RetNotice
    {
        public long notice_idx { get; set; }
        public byte notice_type { get; set; }
        public string title { get; set; }
        public string contents { get; set; }
        public string type { get; set; }            // admin control text notice sub type
        public int orderNum { get; set; }
        public int repeatTime { get; set; }

        public RetNotice(Admin_Notice setNotice)
        {
            notice_idx = setNotice.idx;
            notice_type = setNotice.notice_type;
            title = setNotice.title;
            contents = setNotice.contents;
            type = setNotice.type;
            orderNum = setNotice.orderNum;
            repeatTime = setNotice.repeatTime;
        }

        public static List<RetNotice> GetRetNoticeList(ref List<Admin_Notice> noticeList)
        {
            List<RetNotice> retList = new List<RetNotice>();

            noticeList.ForEach(setItem =>
            {
                retList.Add(new RetNotice(setItem));
            });

            return retList;
        }
    }
}

namespace TheSoul.DataManager
{
    public class SystemData_Define
    {
        public enum eNoticeType
        {
            None = 0,
            GlobalTextNotice = 1,
            LineNotice = 2,
            //InGameTextNotice = 3,
        }
        public const string NoticeTableDBName = "common";
        public const string ConstTableDBName = "sharding";
        public const string ConstTableName = "System_Const";
        public const string ConstTablePrefix = "System_Const";
        public const string VIPTableName = "GameData_VIP";
        public const string VIPTablePrefix = "System_VIP";
        public const string AdminConstTableName = "Admin_System_Const";
        public const string AdminConstTablePrefix = "Admin_System_Const";
        public const string ServerCreateValue = "ServerCreate_RankingValues";
        public const string ServerCreateValueTablePrefix = "ServerCreateValues";

        public const string NoticeTableName = "Admin_Notice";

        public const string SystemConstDefineKey = "const_key";
        public const string SystemConstDefineValue = "const_value";

        public const string Service_AreaKey = "service_area";

        [Flags]
        public enum eContentsType
        {
            NONE = 0,
            PVE_SENARIO = 1 << 0,          // 시나리오
            PVE_DARK = 1 << 1,             // 어둠의통로
            PVE_ELITE = 1 << 2,            // 정예던전
            PVE_BOSSRAID = 1 << 3,        // 보스레이드
            MATCH_OVERLORD = 1 << 4,         // 패왕의길
            EVENT_ARCHIVE_REWARD = 1 << 5,     // 업적보상
            MATCH_1VS1 = 1 << 6,             // 투신전
            MATCH_FREE = 1 << 7,             // 난전
            MATCH_GOLDEXPEDITION = 1 << 8,   // 황금원정단
            MATCH_PARTY = 1 << 9,           // 협력전
            MATCH_GUILD_WAR = 1 << 10,       // 길드전
            MATCH_RUBY_PVP = 1 << 11,        // 검투사의전장
            EVENT_EVENT_REWARD = 1 << 12,        // 이벤트보상
            EVENT_PVP_ARCHIVE_REWARD = 1 << 13,        // PVP업적보상

            ALL = int.MaxValue,
        }

        public enum eAdminConst
        {
            EXTRA_EXP_START_TIME = 1,
            EXTRA_EXP_END_TIME,
            EXTRA_EXP_RATE,
            EXTRA_EXP_CONTENTS, 

            EXTRA_POINT_START_TIME,
            EXTRA_POINT_END_TIME,
            EXTRA_POINT_RATE,
            EXTRA_POINT_CONTENTS,
        }

        public static readonly Dictionary<eAdminConst, string> AdminConstKey_List = new Dictionary<eAdminConst, string>()
        {
            { eAdminConst.EXTRA_EXP_START_TIME, "EXTRA_EXP_START_TIME" },
            { eAdminConst.EXTRA_EXP_END_TIME, "EXTRA_EXP_END_TIME" },
            { eAdminConst.EXTRA_EXP_RATE, "EXTRA_EXP_RATE" },
            { eAdminConst.EXTRA_EXP_CONTENTS, "EXTRA_EXP_CONTENTS" },
            { eAdminConst.EXTRA_POINT_START_TIME, "EXTRA_POINT_START_TIME" },
            { eAdminConst.EXTRA_POINT_END_TIME, "EXTRA_POINT_END_TIME" },
            { eAdminConst.EXTRA_POINT_RATE, "EXTRA_POINT_RATE" },
            { eAdminConst.EXTRA_POINT_CONTENTS, "EXTRA_POINT_CONTENTS" },
        };
    }

    public class SystemData
    {
        public class System_Const
        {
            public int ID { get; set; }
            public string ConstDefine { get; set; }
            public double Value { get; set; }
        }

        public class Const
        {
            public double Value { get; set; }
        }

        public class AdminConst
        {
            public int Value { get; set; }
        }

        public class ServerValue
        {
            public string Value { get; set; }
        }

        public static List<System_Const> GetSystem_Const_All(ref TxnBlock TB, string dbkey = SystemData_Define.ConstTableDBName, bool Flush = false)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)", SystemData_Define.ConstTableName);
            List<System_Const> retList = GenericFetch.FetchFromDB_MultipleRow<System_Const>(ref TB, setQuery, dbkey);

            retList.ForEach(setConst =>
            {
                RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_System, SystemData_Define.ConstTablePrefix, setConst.ConstDefine, setConst.Value);
            }
            );

            return retList;
        }

        private static double ConstValueFetchFromRedis(ref TxnBlock TB, string constKey, string dbkey = SystemData_Define.ConstTableDBName, bool Flush = false)
        {
            string setQuery = string.Format("SELECT Value FROM {0} WITH(NOLOCK)  WHERE ConstDefine = '{1}'", SystemData_Define.ConstTableName, constKey);
            System_Const retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_Const>(ref TB, DataManager_Define.RedisServerAlias_System, SystemData_Define.ConstTablePrefix, constKey, setQuery, dbkey, Flush);
            return retObj == null ? 0 : retObj.Value;
        }

        public static double GetConstValue(ref TxnBlock TB, string ConstKey, string dbkey = SystemData_Define.ConstTableDBName, bool Flush = false)
        {
            return ConstValueFetchFromRedis(ref TB, ConstKey, dbkey, Flush);
        }

        public static int GetConstValueInt(ref TxnBlock TB, string ConstKey, string dbkey = SystemData_Define.ConstTableDBName, bool Flush = false)
        {
            return System.Convert.ToInt32(ConstValueFetchFromRedis(ref TB, ConstKey, dbkey, Flush));
        }

        public static long GetConstValueLong(ref TxnBlock TB, string ConstKey, string dbkey = SystemData_Define.ConstTableDBName, bool Flush = false)
        {
            return System.Convert.ToInt64(ConstValueFetchFromRedis(ref TB, ConstKey, dbkey, Flush));
        }

        public static short GetConstValueShort(ref TxnBlock TB, string ConstKey, string dbkey = SystemData_Define.ConstTableDBName, bool Flush = false)
        {
            return System.Convert.ToInt16(ConstValueFetchFromRedis(ref TB, ConstKey, dbkey, Flush));
        }

        public static List<Admin_Notice> GetAdminNotice(ref TxnBlock TB, string dbkey = SystemData_Define.NoticeTableDBName, bool Flush = false)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE active > 0 AND (GETDATE() BETWEEN sdate AND edate) ", SystemData_Define.NoticeTableName);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Admin_Notice>(ref TB, setQuery, dbkey);
        }
        
        public static int AdminConstValueFetchFromRedis(ref TxnBlock TB, string constKey, string dbkey = SystemData_Define.ConstTableDBName, bool Flush = false)
        {
            string setQuery = string.Format("SELECT Value FROM {0} WITH(NOLOCK)  WHERE ConstDefine = '{1}'", SystemData_Define.AdminConstTableName, constKey);
            AdminConst retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<AdminConst>(ref TB, DataManager_Define.RedisServerAlias_System, SystemData_Define.AdminConstTablePrefix, constKey, setQuery, dbkey, Flush);
            return retObj == null ? 0 : retObj.Value;
        }

        public static string GetServerCreateValue(ref TxnBlock TB, string constKey, bool Flush = false, string dbkey = SystemData_Define.ConstTableDBName)
        {
            string setQuery = string.Format("SELECT Value FROM {0} WITH(NOLOCK)  WHERE key = '{1}'", SystemData_Define.ServerCreateValue, constKey);
            ServerValue retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<ServerValue>(ref TB, DataManager_Define.RedisServerAlias_System, SystemData_Define.ServerCreateValueTablePrefix, constKey, setQuery, dbkey, Flush);
            return retObj == null ? string.Empty : retObj.Value;
        }

        // get service area code in ini setting
        public static DataManager_Define.eCountryCode GetServiceArea(ref TxnBlock TB)
        {
            string countryCode = TB.GetLogData(SystemData_Define.Service_AreaKey);
            return (string.IsNullOrEmpty(countryCode) ? DataManager_Define.eCountryCode.Default : (DataManager_Define.eCountryCode)int.Parse(countryCode));
        }
    }

    public static class  TheSoul_String_Define
    {
        public enum eSystemString
        {
            System_Mail_Sender = 1,
            System_Mail_SendGift,
        }
        public static Dictionary<eSystemString, string> SetString_List = new Dictionary<eSystemString, string>()
        {
            { eSystemString.System_Mail_Sender,     "#STRING_UI_MAIL_SYSTEM_011"},
            { eSystemString.System_Mail_SendGift,   "#STRING_UI_MAIL_SYSTEM_001"},
        };
    }
}
