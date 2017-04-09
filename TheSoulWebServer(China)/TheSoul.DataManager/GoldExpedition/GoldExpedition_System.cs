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

namespace TheSoul.DataManager
{
    public static partial class GoldExpedition_Manager
    {
        public static System_Expedition_Dungeon GetSystem_Expedition_Dungeon(ref TxnBlock TB, long DungeonID, bool Flush = false, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}", GoldExpedition_Define.GoldExpedition_Prefix, GoldExpedition_Define.System_Expedition_Dungeon_TableName, DungeonID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE ExpeditionID = {1}", GoldExpedition_Define.System_Expedition_Dungeon_TableName, DungeonID);

            System_Expedition_Dungeon retObj = GenericFetch.FetchFromRedis<System_Expedition_Dungeon>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_Expedition_Dungeon();
            return retObj;
        }

        public static System_Expedition_Dungeon GetSystem_Expedition_Dungeon(ref TxnBlock TB, int Level, short Stage, bool Flush = false, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            //string setKey = string.Format("{0}_{1}_{2}", GoldExpedition_Define.GoldExpedition_Prefix, GoldExpedition_Define.GameData_ExpeditionDungeon_TableName, DungeonID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE Stage_No = {1} AND MinLevel <= {2} AND MaxLevel >= {2}", GoldExpedition_Define.System_Expedition_Dungeon_TableName, Stage, Level);

            System_Expedition_Dungeon retObj = GenericFetch.FetchFromDB<System_Expedition_Dungeon>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new System_Expedition_Dungeon();
            return retObj;
        }

        public static User_GE_Stage_Info GetUser_GE_Stage_Info(ref TxnBlock TB, long AID, bool bReset = false, bool Flush = false, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}", GoldExpedition_Define.GoldExpedition_Prefix, GoldExpedition_Define.User_ExpeditionInfo_TableName, AID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1}", GoldExpedition_Define.User_GE_Stage_Info_TableName, AID);
            User_GE_Stage_Info retObj = null;

            if (!bReset)
                retObj = GenericFetch.FetchFromRedis<User_GE_Stage_Info>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);

            if (retObj == null)
            {
                retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<User_GE_Stage_Info>(ref TB, setQuery, dbkey);
                if(retObj == null)
                    retObj = new User_GE_Stage_Info();

                Account_Simple userInfo = AccountManager.GetSimpleAccountInfo(ref TB, AID);
                retObj.AID = userInfo.aid;
                retObj.Clear_Stage = 0;
                retObj.AllyCharacter_Info_Json = "";
                retObj.AllyCharacter_Detail_Json = "";
                retObj.AllyUserName = "";

                List<Character_Detail_With_HP> userCharList = CharacterManager.GetCharacterListWithEquip_HP(ref TB, retObj.AID, Flush || bReset).OrderBy(charInfo => charInfo.warpoint).ToList();

                List<User_Character_HP_Info> MyHPInfo = new List<User_Character_HP_Info>();
                userCharList.ForEach(charinfo =>
                {
                    MyHPInfo.Add(new User_Character_HP_Info(charinfo, true));
                });

                retObj.MyCharacter_Info_Json = mJsonSerializer.ToJsonString(MyHPInfo);
                retObj.MyCharacter_Detail_Json = Character_Detail_With_HP.makeCharacter_DetailListJson(ref userCharList);
                //retObj.RegDate = DateTime.Now;
                //retObj.HireCount = 0;

                if (SetUser_GE_Stage_InfoToDB(ref TB, AID, retObj) != Result_Define.eResult.SUCCESS)
                    retObj = new User_GE_Stage_Info();
                else
                    RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, retObj);
            }

            DateTime curDate = DateTime.Parse(retObj.RegDate.ToShortDateString());
            DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            TimeSpan TS = dbDate - curDate;

            if (TS.Days != 0)
            {
                retObj.ResetCount = 0;
                retObj.HireCount = 0;
            }
            return retObj;
        }

        public static List<User_Character_HP_Info> ReCalcUser_GE_Stage_Info(ref TxnBlock TB, long AID, long CID, ref User_GE_Stage_Info setInfo, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            Character_Detail_With_HP setChar = CharacterManager.GetCharacterListWithEquip_HP(ref TB, AID).Find(charinfo => charinfo.cid == CID);
            List<User_Character_HP_Info> setUserHPInfo = mJsonSerializer.JsonToObject<List<User_Character_HP_Info>>(setInfo.MyCharacter_Info_Json);

            if (setChar != null)
            {
                List<Character_Detail_With_HP> setUserDetailInfo = mJsonSerializer.JsonToObject<List<Character_Detail_With_HP>>(setInfo.MyCharacter_Info_Json);

                if (setUserHPInfo.Find(info => info.cid == CID) == null)
                    setUserHPInfo.Add(new User_Character_HP_Info(setChar, true));

                if (setUserDetailInfo.Find(info => info.cid == CID) == null)
                    setUserDetailInfo.Add(setChar);
            }
            return setUserHPInfo;
        }

        public static void RemoveCacheUser_GE_Stage_Info(long AID)
        {
            string setKey = string.Format("{0}_{1}_{2}", GoldExpedition_Define.GoldExpedition_Prefix, GoldExpedition_Define.User_ExpeditionInfo_TableName, AID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static User_GE_CharacterGroup GetGECharacterGroupInfo(ref TxnBlock TB, long AID, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            //string setKey = string.Format("{0}_{1}_{2}", GoldExpedition_Define.GoldExpedition_Prefix, GoldExpedition_Define.User_GE_CharacterGroup_TableName, AID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1}", GoldExpedition_Define.User_GE_CharacterGroup_TableName, AID);
            User_GE_CharacterGroup retObj = GenericFetch.FetchFromDB<User_GE_CharacterGroup>(ref TB, setQuery, dbkey);
            if (retObj == null)
            {                
                retObj = new User_GE_CharacterGroup();
                retObj.aid = AID;
            }
            return retObj;
        }

        public static User_GE_Boost_Item GetUser_GE_Boost_Item(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}", GoldExpedition_Define.GoldExpedition_Prefix, GoldExpedition_Define.User_GE_Boost_Item_TableName, AID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1}", GoldExpedition_Define.User_GE_Boost_Item_TableName, AID);

            User_GE_Boost_Item retObj = GenericFetch.FetchFromRedis<User_GE_Boost_Item>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
            if (retObj == null)
            {
                retObj = new User_GE_Boost_Item();
                retObj.aid = AID;
            }
            return retObj;
        }

        public static void RemoveCacheUser_GE_Boost_Item(long AID)
        {
            string setKey = string.Format("{0}_{1}_{2}", GoldExpedition_Define.GoldExpedition_Prefix, GoldExpedition_Define.User_GE_Boost_Item_TableName, AID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        private static string GetUser_GE_Stage_EnemyRedisKey(long AID)
        {
            return string.Format("{0}_{1}_{2}", GoldExpedition_Define.GoldExpedition_Prefix, GoldExpedition_Define.User_GE_Stage_Enemy_TableName, AID);
        }

        public static List<User_GE_Stage_Enemy> GetUser_GE_Stage_Enemy(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            string setKey = GetUser_GE_Stage_EnemyRedisKey(AID);

            List<User_GE_Stage_Enemy> retObj = new List<User_GE_Stage_Enemy>();

            if (!Flush)
            {
                retObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_Hash_All<User_GE_Stage_Enemy>(DataManager_Define.RedisServerAlias_User, setKey);
                if (retObj.Count == 0)
                    Flush = true;
            }

            if (Flush)
            {
                RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
                string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1} ORDER BY stage", GoldExpedition_Define.User_GE_Stage_Enemy_TableName, AID);
                retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_GE_Stage_Enemy>(ref TB, setQuery, dbkey);

                retObj.ForEach(item =>
                {
                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, item.Stage.ToString(), item);
                });
            }

            RedisConst.GetRedisInstance().SetExpireTimeHash(DataManager_Define.RedisServerAlias_User, setKey);

            return retObj;
        }


        public static User_GE_Stage_Enemy GetUser_GE_Stage_Enemy(ref TxnBlock TB, long AID, short Stage, bool Flush = false, string dbkey = Character_Define.CharacterDBName)
        {
            string setKey = GetUser_GE_Stage_EnemyRedisKey(AID);
            User_GE_Stage_Enemy retObj = null;
            if (!Flush)
                retObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_Hash_Field<User_GE_Stage_Enemy>(DataManager_Define.RedisServerAlias_User, setKey, Stage.ToString());

            if (retObj == null)
            {
                List<User_GE_Stage_Enemy> getList = GetUser_GE_Stage_Enemy(ref TB, AID, true, dbkey);
                retObj = getList.Find(item => item.Stage == Stage);
                if (retObj == null)
                    retObj = new User_GE_Stage_Enemy();
            }

            return retObj;
        }


        public static void RemoveUser_GE_Stage_Enemy(long AID)
        {
            string setKey = GetUser_GE_Stage_EnemyRedisKey(AID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static List<User_Guild_Mercenary_Info> GetUser_Mercenary_Info(ref TxnBlock TB, long AID, string dbkey = GoldExpedition_Define.GoldExpedition_Guild_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1} AND ActiveFlag = 'Y'", GoldExpedition_Define.User_Guild_Mercenary_Info_TableName, AID);
            return GenericFetch.FetchFromDB_MultipleRow<User_Guild_Mercenary_Info>(ref TB, setQuery, dbkey);
        }

        public static List<User_Guild_Mercenary_Info> GetUser_Guild_Mercenary_Info(ref TxnBlock TB, long AID, long GID, string dbkey = GoldExpedition_Define.GoldExpedition_Guild_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE GID = {1} AND AID <> {2} AND ActiveFlag = 'Y'", GoldExpedition_Define.User_Guild_Mercenary_Info_TableName, GID, AID);
            return GenericFetch.FetchFromDB_MultipleRow<User_Guild_Mercenary_Info>(ref TB, setQuery, dbkey);
        }

        public static User_Guild_Mercenary_Info GetGuild_Mercenary_Info(ref TxnBlock TB, long AID, long CID, string dbkey = GoldExpedition_Define.GoldExpedition_Guild_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1} AND CID = {2} AND ActiveFlag = 'Y'", GoldExpedition_Define.User_Guild_Mercenary_Info_TableName, AID, CID);
            User_Guild_Mercenary_Info retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<User_Guild_Mercenary_Info>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new User_Guild_Mercenary_Info();
            return retObj;
        }

        public static User_WarPoint GetUserWarPoint(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}", GoldExpedition_Define.UserGE_Prefix, GoldExpedition_Define.User_WarPoint_Table_Name, AID);
            string setQuery = string.Format(@"
                                SELECT AID, MAX(level) as CHARACTER_MAX_LEVEL, SUM(WAR_POINT + ACTIVE_SOUL_WAR_POINT + PASSIVE_SOUL_WAR_POINT) AS WAR_POINT, SUM(MAX_WAR_POINT) AS MAX_WAR_POINT FROM 
                                        (SELECT B.aid as AID, B.level, A.WAR_POINT, A.ACTIVE_SOUL_WAR_POINT, A.PASSIVE_SOUL_WAR_POINT, A.MAX_WAR_POINT FROM {0} AS A WITH(NOLOCK) INNER JOIN {1} AS B WITH(NOLOCK) ON A.CID = B.cid AND B.aid = {2}) AS CalcTable GROUP BY AID
                                ", Character_Define.Character_Stat_TableName, Character_Define.CharacterTableName, AID);
            User_WarPoint retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<User_WarPoint>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new User_WarPoint();
            return retObj;
        }

        public static User_WarPoint GetUserCharacterMaxWarPoint(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}", GoldExpedition_Define.UserGE_Prefix, GoldExpedition_Define.User_WarPoint_Table_Name, AID);
            string setQuery = string.Format(@"
                                SELECT AID, MAX(level) as CHARACTER_MAX_LEVEL, SUM(MAX_WAR_POINT) AS MAX_WAR_POINT, MAX(MAX_WAR_POINT) as WAR_POINT FROM 
                                        (SELECT B.aid as AID, B.level, A.MAX_WAR_POINT FROM {0} AS A WITH(NOLOCK) INNER JOIN {1} AS B WITH(NOLOCK) ON A.CID = B.cid AND B.aid = {2}) AS CalcTable GROUP BY AID
                                ", Character_Define.Character_Stat_TableName, Character_Define.CharacterTableName, AID);
            User_WarPoint retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<User_WarPoint>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new User_WarPoint();
            return retObj;
        }

        public static List<User_WarPoint> GetUserWarPointList(ref TxnBlock TB, long AID, short getCount, double setMin, double setMax, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            string setQuery = string.Format(@"SELECT TOP {3} * FROM 
            (
            SELECT AID, MAX(level) as CHARACTER_MAX_LEVEL, SUM(CONVERT(BIGINT, (WAR_POINT + ACTIVE_SOUL_WAR_POINT + PASSIVE_SOUL_WAR_POINT))) AS WAR_POINT FROM 
                (SELECT B.aid as AID, B.level, A.WAR_POINT, A.ACTIVE_SOUL_WAR_POINT, A.PASSIVE_SOUL_WAR_POINT FROM {0} AS A WITH(NOLOCK) INNER JOIN {1} AS B WITH(NOLOCK) ON A.CID = B.cid) AS CalcTable GROUP BY AID
            ) AS UserWarPoint
            WHERE AID <> {2} AND CHARACTER_MAX_LEVEL >= {4} AND WAR_POINT >= {5} AND WAR_POINT < {6} ORDER BY NEWID()", Character_Define.Character_Stat_TableName, Character_Define.CharacterTableName, AID, getCount, GoldExpedition_Define.LimitUserLevel, setMin, setMax);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_WarPoint>(ref TB, setQuery, dbkey);
        }


        public static List<User_WarPoint> GetUserWarPointListByCharacterMaxWarPoint(ref TxnBlock TB, long AID, short getCount, double setMin, double setMax, double setMinChar, double setMaxChar, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            string setQuery = string.Format(@"SELECT TOP {3} * FROM 
            (
                SELECT AID, MAX(level) as CHARACTER_MAX_LEVEL, SUM(WAR_POINT) AS MAX_WAR_POINT, MAX(WAR_POINT) as WAR_POINT FROM 
                    (SELECT B.aid as AID, B.level, CONVERT(BIGINT, (A.WAR_POINT + A.ACTIVE_SOUL_WAR_POINT + A.PASSIVE_SOUL_WAR_POINT)) AS WAR_POINT 
                                FROM {0} AS A WITH(NOLOCK)
                                INNER JOIN {1} AS B WITH(NOLOCK)
                                    ON A.CID = B.cid)
                    AS CalcTable GROUP BY AID
            ) AS UserWarPoint
            WHERE 
                AID <> {2} AND CHARACTER_MAX_LEVEL >= {4}
                AND MAX_WAR_POINT >= {5} AND MAX_WAR_POINT < {6}
                AND WAR_POINT  >= {7} AND WAR_POINT < {8}
            ORDER BY NEWID()", Character_Define.Character_Stat_TableName, Character_Define.CharacterTableName, AID, getCount, GoldExpedition_Define.LimitUserLevel, setMin, setMax, setMinChar, setMaxChar);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_WarPoint>(ref TB, setQuery, dbkey);
        }

        //public static User_WarPoint GetUserWarPointList_Top100_Random(ref TxnBlock TB, long AID, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        //{
        //    string setQuery = string.Format("SELECT TOP 1 * FROM (SELECT TOP {2} * FROM {0} WITH(NOLOCK)  WHERE AID <> {1} AND CHARACTER_MAX_LEVEL >= {3} ORDER BY WAR_POINT DESC) AS T ORDER BY NEWID()", GoldExpedition_Define.User_WarPoint_Table_Name, AID, GoldExpedition_Define.LimitTopPlayer, GoldExpedition_Define.LimitUserLevel);
        //    User_WarPoint retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<User_WarPoint>(ref TB, setQuery, dbkey);
        //    if (retObj == null)
        //        retObj = new User_WarPoint();
        //    return retObj;
        //}
    }
}
