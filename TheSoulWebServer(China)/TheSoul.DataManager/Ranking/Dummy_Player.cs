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
    public class System_Ruby_PvP_Soul
    {
        public long soul { get; set; }
        public byte classcode { get; set; }
        public long buffid { get; set; }
        public long setBuffID { get; set; }
        public long pairBuffID { get; set; }
        public long setEpicbuffID { get; set; }
    }
}

namespace TheSoul.DataManager
{
    public static class Dummy_Define
    {
        public const string DummyPrefix = "System_Dummy";
        public const string DummyInfo_DB = "sharding";
        public const string RubyPvP_Soul_Info_DB = "sharding";
        public static string AccountDBTableName = string.Format("{0}_{1}", DummyPrefix, "Account");
        public static string CharacterTableName = string.Format("{0}_{1}", DummyPrefix, "Character");

        public static string RubyPvPSoulDBTableName = "System_Ruby_PvP_Soul";
        public static readonly Dictionary<int, long> RubyPvPDummyCID = new Dictionary<int, long>()
        {
            { (int)Character_Define.SystemClassType.Class_Warrior, 8888888 },
            { (int)Character_Define.SystemClassType.Class_Swordmaster, 9999999 },
            { (int)Character_Define.SystemClassType.Class_Taoist, 10000000},        // TODO : set dummy aid for taoist
        };
        public static string User_Inven_Prefix = string.Format("{0}_{1}", DummyPrefix, "UserInven");
        public static string Item_User_Inven_Table = string.Format("{0}_{1}", DummyPrefix, "User_Inven");
        public static string Item_User_Inven_Option_Table = string.Format("{0}_{1}", DummyPrefix, "User_Inven_Option");

        public static string User_ActiveSoul_Table = string.Format("{0}_{1}", DummyPrefix, "User_ActiveSoul");
        public static string User_ActiveSoul_Equip_Table = string.Format("{0}_{1}", DummyPrefix, "User_ActiveSoul_Equip");
        public static string User_PassiveSoul_Table = string.Format("{0}_{1}", DummyPrefix, "User_PassiveSoul");
        public static string User_ActiveSoul_Special_Buff_Table = string.Format("{0}_{1}", DummyPrefix, "User_ActiveSoul_Special_Buff");
        public static string User_Character_Equip_Soul_Table = string.Format("{0}_{1}", DummyPrefix, "User_Character_Equip_Soul");

        public static string User_Soul_Prefix = string.Format("{0}_{1}", DummyPrefix, "UserSoul");
        public static string User_Soul_Equip_Prefix = string.Format("{0}_{1}", DummyPrefix, "UserSoulEquip");
        public static string SystemSoul_Prefix = string.Format("{0}_{1}", DummyPrefix, "SystemSoul");
    }

    public static class DummyManager
    {
        public static System_Ruby_PvP_Soul GetSystem_Ruby_PvP_Soul_Info(ref TxnBlock TB, long soulID, int classCode, long buffID, bool Flush = false, string dbkey = Dummy_Define.RubyPvP_Soul_Info_DB)
        {
            string setKey = string.Format("{0}_{1}", Dummy_Define.DummyPrefix, Dummy_Define.RubyPvPSoulDBTableName);
            string setField = string.Format("{0}_{1}_{2}", soulID, classCode, buffID);
            System_Ruby_PvP_Soul retObj = new System_Ruby_PvP_Soul();

            if (!Flush)
                retObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_Hash_Field<System_Ruby_PvP_Soul>(DataManager_Define.RedisServerAlias_System, setKey, setField);

            if (retObj == null || Flush)
            {
                string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE soul = {1} AND classcode = {2} AND buffid = {3}", Dummy_Define.RubyPvPSoulDBTableName, soulID, classCode, buffID);
                retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<System_Ruby_PvP_Soul>(ref TB, setQuery, dbkey);
                if (retObj == null)
                    retObj = new System_Ruby_PvP_Soul();
                else
                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, setField, retObj);
            }

            return retObj;
        }

        public static List<User_WarPoint> GetDummyUserWarPointList(ref TxnBlock TB, int getCount, double setMin, double setMax, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            setMin = setMin < GoldExpedition_Define.Bot_Warpoint_Min ? GoldExpedition_Define.Bot_Warpoint_Min : setMin;
            setMax = setMax < GoldExpedition_Define.Bot_Warpoint_Max ? GoldExpedition_Define.Bot_Warpoint_Max : setMax;

            string setQuery = string.Format("SELECT TOP {1} aid AS AID, 0 as CHARACTER_MAX_LEVEL, warpoint as WAR_POINT  FROM {0} WITH(NOLOCK, INDEX(IDX_System_Dummy_Account_warpoint)) WHERE warpoint >= {2} AND warpoint < {3} AND aid < {4} ORDER BY NEWID()", Dummy_Define.AccountDBTableName, getCount, setMin, setMax, Dummy_Define.RubyPvPDummyCID.Values.Min());
            List<User_WarPoint> retList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_WarPoint>(ref TB, setQuery, dbkey);

            if(retList.Count < getCount)
            {
                setQuery = string.Format("SELECT TOP {1} aid AS AID, 0 as CHARACTER_MAX_LEVEL, warpoint as WAR_POINT FROM {0} WITH(NOLOCK, INDEX(IDX_System_Dummy_Account_warpoint)) WHERE warpoint < {2} AND aid < {3} ORDER BY warpoint DESC, NEWID() ASC", Dummy_Define.AccountDBTableName, getCount, setMax, Dummy_Define.RubyPvPDummyCID.Values.Min());
                retList.AddRange(TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_WarPoint>(ref TB, setQuery, dbkey));
            }

            if (retList.Count < getCount)
            {
                setQuery = string.Format("SELECT TOP {1} aid AS AID, 0 as CHARACTER_MAX_LEVEL, warpoint as WAR_POINT FROM {0} WITH(NOLOCK, INDEX(IDX_System_Dummy_Account_warpoint)) WHERE warpoint >= {2} AND aid < {3} ORDER BY warpoint ASC, NEWID() ASC", Dummy_Define.AccountDBTableName, getCount, setMin, Dummy_Define.RubyPvPDummyCID.Values.Min());
                retList.AddRange(TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_WarPoint>(ref TB, setQuery, dbkey));
            }

            var rnd = new Random();
            if (retList.Count < getCount)
                return retList;

            return retList.OrderBy(item => rnd.Next()).ToList().GetRange(0, getCount);
        }

        public static List<User_WarPoint> GetDummyUserWarPointListByCharacterWarPoint(ref TxnBlock TB, int getCount, double setMin, double setMax, double setMinChar, double setMaxChar, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            setMin = setMin <= GoldExpedition_Define.Bot_Warpoint_Min ? GoldExpedition_Define.Bot_Warpoint_Min-1 : setMin;
            setMax = (setMax <= GoldExpedition_Define.Bot_Warpoint_Max) || (setMax <= GoldExpedition_Define.Bot_Warpoint_Min) ? GoldExpedition_Define.Bot_Warpoint_Max + 1 : setMax;

            //string setQuery = string.Format("SELECT TOP {1} aid AS AID, 0 as CHARACTER_MAX_LEVEL, MAX(warpoint) as WAR_POINT  FROM {0} WITH(NOLOCK) WHERE warpoint >= {2} AND warpoint < {3} AND aid < {4} GROUP BY aid ORDER BY NEWID()", Dummy_Define.CharacterTableName, getCount, setMin, setMax, Dummy_Define.RubyPvPDummyCID.Values.Min());

            // nearest LV search (by ABS Level check)
//            string setQuery = string.Format(@"SELECT TOP {0} *, ABS(60 - Temp.CHARACTER_MAX_LEVEL) as ChkLevel FROM
//                                                    (SELECT TOP 20 * FROM
//			                                            (
//				                                            SELECT AID, MAX(level) as CHARACTER_MAX_LEVEL, SUM(WAR_POINT) AS MAX_WAR_POINT, MAX(WAR_POINT) as WAR_POINT FROM 
//					                                            (SELECT aid, level, cid, warpoint AS WAR_POINT FROM {1})  AS CalcTable
//					                                            GROUP BY AID
//			                                            ) AS UserWarPoint
//			                                            WHERE 
//                                                            MAX_WAR_POINT >= {2} AND MAX_WAR_POINT < {3}
//                                                            AND WAR_POINT >= {4} AND WAR_POINT < {5}
//                                                            AND AID < {6}
//                                                        ORDER BY NEWID()
//                                                    ) ORDER BY ChkLevel
//                                            ", getCount, Dummy_Define.CharacterTableName, setMin, setMax, setMinChar, setMaxChar, Dummy_Define.RubyPvPDummyCID.Values.Min());

            string setQuery = string.Format(@"SELECT TOP {0} * FROM
			                                            (
				                                            SELECT AID, MAX(level) as CHARACTER_MAX_LEVEL, SUM(WAR_POINT) AS MAX_WAR_POINT, MAX(WAR_POINT) as WAR_POINT FROM 
					                                            (SELECT aid, level, cid, warpoint AS WAR_POINT FROM {1})  AS CalcTable
					                                            GROUP BY AID
			                                            ) AS UserWarPoint
			                                            WHERE 
                                                            MAX_WAR_POINT >= {2} AND MAX_WAR_POINT < {3}
                                                            AND WAR_POINT >= {4} AND WAR_POINT < {5}
                                                            AND AID < {6}
                                                        ORDER BY NEWID()  
                                            ", getCount, Dummy_Define.CharacterTableName, setMin, setMax, setMinChar, setMaxChar, Dummy_Define.RubyPvPDummyCID.Values.Min());
            List<User_WarPoint> retList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_WarPoint>(ref TB, setQuery, dbkey);

            if (retList.Count < getCount)
            {
                //setQuery = string.Format("SELECT TOP {1} aid AS AID, 0 as CHARACTER_MAX_LEVEL, MAX(warpoint) as WAR_POINT FROM {0} WITH(NOLOCK) WHERE warpoint < {2} AND aid < {3}  GROUP BY aid ORDER BY warpoint DESC, NEWID() ASC", Dummy_Define.CharacterTableName, getCount, setMax, Dummy_Define.RubyPvPDummyCID.Values.Min());
                setQuery = string.Format(@"SELECT TOP {0} * FROM
			                                            (
				                                            SELECT AID, MAX(level) as CHARACTER_MAX_LEVEL, SUM(WAR_POINT) AS MAX_WAR_POINT, MAX(WAR_POINT) as WAR_POINT FROM 
					                                            (SELECT aid, level, cid, warpoint AS WAR_POINT FROM {1})  AS CalcTable
					                                            GROUP BY AID
			                                            ) AS UserWarPoint
			                                            WHERE 
                                                            MAX_WAR_POINT < {2}
                                                            AND WAR_POINT < {3}
                                                            AND AID < {4}
                                                        ORDER BY WAR_POINT DESC, NEWID() 
                                            ", getCount * GoldExpedition_Define.SerchOverCountRate, Dummy_Define.CharacterTableName, setMax, setMaxChar, Dummy_Define.RubyPvPDummyCID.Values.Min());
                retList.AddRange(TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_WarPoint>(ref TB, setQuery, dbkey));
            }

            if (retList.Count < getCount)
            {
                //setQuery = string.Format("SELECT TOP {1} aid AS AID, 0 as CHARACTER_MAX_LEVEL, MAX(warpoint) as WAR_POINT FROM {0} WITH(NOLOCK) WHERE warpoint >= {2} AND aid < {3}  GROUP BY aid ORDER BY warpoint ASC, NEWID() ASC", Dummy_Define.CharacterTableName, getCount, setMin, Dummy_Define.RubyPvPDummyCID.Values.Min());
                setQuery = string.Format(@"SELECT TOP {0} * FROM
			                                            (
				                                            SELECT AID, MAX(level) as CHARACTER_MAX_LEVEL, SUM(WAR_POINT) AS MAX_WAR_POINT, MAX(WAR_POINT) as WAR_POINT FROM
					                                            (SELECT aid, level, cid, warpoint AS WAR_POINT FROM {1})  AS CalcTable
					                                            GROUP BY AID
			                                            ) AS UserWarPoint
			                                            WHERE 
                                                            MAX_WAR_POINT >= {2}
                                                            AND WAR_POINT < {3}
                                                            AND AID < {4}
                                                        ORDER BY WAR_POINT ASC, NEWID() 
                                            ", getCount * GoldExpedition_Define.SerchOverCountRate, Dummy_Define.CharacterTableName, setMin, setMaxChar, Dummy_Define.RubyPvPDummyCID.Values.Min());
                retList.AddRange(TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_WarPoint>(ref TB, setQuery, dbkey));
            }
            var rnd = new Random();
            if (retList.Count < getCount)
                return retList;
            return retList.OrderBy(item => rnd.Next()).ToList().GetRange(0, getCount);
        }

        public static User_WarPoint GetAccountWarpointInfo(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            if (AID == 0)
                return new User_WarPoint();

            string setKey = string.Format("{0}_{1}_{2}_{3}", Account_Define.Account_SimpleInfo_Prefix, Dummy_Define.AccountDBTableName, AID, Account_Define.Account_SimpleInfo_Surfix);
            string setQuery = string.Format("SELECT aid AS AID, 0 as CHARACTER_MAX_LEVEL, warpoint as WAR_POINT FROM {0} WITH(NOLOCK) WHERE AID = {1} ", Dummy_Define.AccountDBTableName, AID);
            User_WarPoint retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<User_WarPoint>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
            return (retObj == null) ? new User_WarPoint() : retObj;
        }

        public static Account_Simple GetSimpleAccountInfo(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            if (AID == 0)
                return new Account_Simple();

            string setKey = string.Format("{0}_{1}_{2}_{3}", Account_Define.Account_SimpleInfo_Prefix, Dummy_Define.AccountDBTableName, AID, Account_Define.Account_SimpleInfo_Surfix);
            string setQuery = string.Format("SELECT aid, user_name as username FROM {0} WITH(NOLOCK) WHERE AID = {1} ", Dummy_Define.AccountDBTableName, AID);
            Account_Simple retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<Account_Simple>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
            return (retObj == null) ? new Account_Simple() : retObj;
        }

        public static Account_Simple_With_Character GetSimpleAccountCharacterInfo(ref TxnBlock TB, long AID, long CID = 0, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            if (AID == 0)
                return new Account_Simple_With_Character();

            string setKey = string.Format("{0}_{1}_{2}_{3}", Account_Define.Account_SimpleInfo_Prefix, Dummy_Define.AccountDBTableName, AID, Account_Define.Account_SimpleInfo_WithEquip_Surfix);
            string setQuery = string.Format("SELECT aid, user_name as username FROM {0} WITH(NOLOCK) WHERE AID = {1} ", Dummy_Define.AccountDBTableName, AID);
            Account_Simple_With_Character retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<Account_Simple_With_Character>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);

            if (retObj != null)
            {                
                if (CID > 0)
                    retObj.charinfo = new Character_Simple(DummyManager.GetCharacter(ref TB, AID, CID));
                else
                    retObj.charinfo = new Character_Simple(DummyManager.GetCharacter(ref TB, AID, 0));
            }
            return (retObj == null) ? new Account_Simple_With_Character() : retObj;
        }

        public static User_WarPoint GetUserWarPoint(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}", Account_Define.Account_WarpointInfo_Prefix, Account_Define.AccountDBTableName, AID);

            string setQuery = string.Format(@"SELECT aid, MAX(level) as CHARACTER_MAX_LEVEL, SUM(warpoint) AS WAR_POINT FROM {0} B WITH(NOLOCK) WHERE AID = {1} GROUP BY AID ", Dummy_Define.CharacterTableName, AID);
            User_WarPoint retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<User_WarPoint>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
            return retObj == null ? new User_WarPoint() : retObj;
        }


        public static Character_Detail_With_HP GetCharacterInfoWithEquip(ref TxnBlock TB, Character setChar)
        {
            long dummyAID = 0;
            long dummyCID = 0;

            dummyCID = Dummy_Define.RubyPvPDummyCID.TryGetValue(setChar.Class, out dummyAID) ? dummyAID : dummyAID = 0;            

            Character_Detail_With_HP retObj = new Character_Detail_With_HP(setChar, -1, -1);
            Character dummyCharInfo = DummyManager.GetCharacter(ref TB, dummyAID, dummyCID);
            retObj.level = dummyCharInfo.level;
            retObj.exp = dummyCharInfo.exp;
            retObj.totalexp = dummyCharInfo.totalexp;
            retObj.warpoint = dummyCharInfo.warpoint;
            retObj.equiplist = DummyManager.GetEquipList(ref TB, dummyAID, dummyCID);
            retObj.equip_active_soul = new List<Ret_Equip_Soul_Active>(); //DummyManager.GetRet_Active_Soul_Equip_List(ref TB, dummyAID, dummyCID, true);
            retObj.equip_passive_soul = new List<Ret_Soul_Passive>(); //DummyManager.GetRet_Passive_Soul_Equip_List(ref TB, dummyAID, dummyCID, true);
            return retObj;
        }

        public static List<Character_Detail_With_HP> GetCharacterListWithEquip_HP(ref TxnBlock TB, long AID)
        {
            List<Character> userCharList = DummyManager.GetCharacterList(ref TB, AID);
            List<Character_Detail_With_HP> retList = new List<Character_Detail_With_HP>();

            foreach (Character setChar in userCharList)
            {
                Character_Detail_With_HP setObj = new Character_Detail_With_HP(setChar, -1, -1);                
                setObj.equiplist = DummyManager.GetEquipList(ref TB, AID, setChar.cid);
                setObj.equip_active_soul = DummyManager.GetRet_Active_Soul_Equip_List(ref TB, AID, setChar.cid);
                setObj.equip_passive_soul = DummyManager.GetRet_Passive_Soul_Equip_List(ref TB, AID, setChar.cid);

                retList.Add(setObj);
            }
            return retList;
        }

        public static List<Character_Detail> GetCharacterListWithEquip(ref TxnBlock TB, long AID)
        {
            List<Character> userCharList = DummyManager.GetCharacterList(ref TB, AID);
            List<Character_Detail> retList = new List<Character_Detail>();

            foreach (Character setChar in userCharList)
            {
                Character_Detail setObj = new Character_Detail(setChar);
                setObj.equiplist = DummyManager.GetEquipList(ref TB, AID, setChar.cid);
                setObj.equip_active_soul = DummyManager.GetRet_Active_Soul_Equip_List(ref TB, AID, setChar.cid);
                setObj.equip_passive_soul = DummyManager.GetRet_Passive_Soul_Equip_List(ref TB, AID, setChar.cid);

                retList.Add(setObj);
            }
            return retList;
        }

        public static Character GetCharacterByLevel(ref TxnBlock TB, int level, byte classtype, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            string setKey = string.Format("Debug_{0}_{1}_{2}_{3}", Dummy_Define.DummyPrefix, Dummy_Define.CharacterTableName, level, classtype);
            string setQuery = string.Format("SELECT TOP 1 * FROM {0} WITH(NOLOCK) WHERE [level] = {1} AND [class] = {2} ORDER BY NEWID()", Dummy_Define.CharacterTableName, level, classtype);
            Character retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<Character>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
            return retObj == null ? new Character() : retObj;
        }

        public static Character GetCharacterByCID(ref TxnBlock TB, long CID, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            string setKey = string.Format("Debug_{0}_{1}_{2}_ONLY_CID", Dummy_Define.DummyPrefix, Dummy_Define.CharacterTableName, CID);
            string setQuery = string.Format("SELECT TOP 1 * FROM {0} WITH(NOLOCK) WHERE [cid] = {1} ORDER BY NEWID()", Dummy_Define.CharacterTableName, CID);
            Character retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<Character>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
            return retObj == null ? new Character() : retObj;
        }

        public static Character GetCharacter(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            Character retObj = new Character();
            if (CID == 0)
            {
                List<Character> charList = DummyManager.GetCharacterList(ref TB, AID);

                if (charList.Count > 0)
                    retObj = charList.FirstOrDefault();
            }
            else
            {
                string setKey = string.Format("{0}_{1}_{2}", Dummy_Define.DummyPrefix, Dummy_Define.CharacterTableName, AID);                

                if (!Flush)
                    retObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_Hash_Field<Character>(DataManager_Define.RedisServerAlias_User, setKey, CID.ToString());

                if (retObj == null || Flush)
                {
                    string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE aid = {1} AND cid = {2}", Dummy_Define.CharacterTableName, AID, CID);
                    retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<Character>(ref TB, setQuery, dbkey);
                    if (retObj == null)
                        retObj = new Character();
                    else
                        RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, CID.ToString(), retObj);
                }
            }
            return retObj;
        }

        public static List<Character> GetCharacterList(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}", Dummy_Define.DummyPrefix, Dummy_Define.CharacterTableName, AID);
            List<Character> retObj = new List<Character>();

            if (!Flush)
            {
                retObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_Hash_All<Character>(DataManager_Define.RedisServerAlias_User, setKey);
                if (retObj.Count == 0)
                    Flush = true;
            }

            if (Flush)
            {
                RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
                string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE aid = {1}", Dummy_Define.CharacterTableName, AID);
                retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Character>(ref TB, setQuery, dbkey);

                retObj.ForEach(item =>
                {
                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, item.cid.ToString(), item);
                });
            }

            //RedisConst.GetRedisInstance().SetExpireTimeHash(DataManager_Define.RedisServerAlias_User, setKey);

            return retObj;
        }


        public static List<User_Inven> GetEquipList(ref TxnBlock TB, long AID, long CID, bool bWithOption = true, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            string setEquipKey = DummyManager.GetEquipKey(AID, CID);

            List<User_Inven> UserItem = new List<User_Inven>();
            if (!Flush)
                UserItem = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_Hash_All<User_Inven>(DataManager_Define.RedisServerAlias_User, setEquipKey);

            if (UserItem == null || Flush || UserItem.Count == 0)
            {
                UserItem = DummyManager.GetCharacterInvenList(ref TB, AID, CID, bWithOption, Flush);
                UserItem = UserItem.Where(item => item.equipflag.Equals("Y")).ToList();
            }

            return UserItem;
        }

        private static string GetEquipKey(long AID, long CID)
        {
            return string.Format("{0}_{1}_{2}", Dummy_Define.User_Inven_Prefix, AID, CID);
        }

        private static List<User_Inven_Option> GetInvenOptionList(ref TxnBlock TB, long AID, long CID = 0, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", Dummy_Define.User_Inven_Prefix, Dummy_Define.Item_User_Inven_Option_Table, AID, CID);
            List<User_Inven_Option> retObj = new List<User_Inven_Option>();

            if (!Flush)
            {
                retObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_Hash_All<User_Inven_Option>(DataManager_Define.RedisServerAlias_User, setKey);
                if (retObj.Count == 0)
                    Flush = true;
            }

            if (Flush)
            {
                RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
                string setQuery = string.Format(@"SELECT A.* FROM {0} AS A WITH(NOLOCK), {1} AS B 
                                                WITH(NOLOCK) WHERE B.aid = {2} AND B.cid = {3} AND B.invenseq = A.invenseq
                                            ", Dummy_Define.Item_User_Inven_Option_Table, Dummy_Define.Item_User_Inven_Table, AID, CID);
                retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_Inven_Option>(ref TB, setQuery, dbkey);

                retObj.ForEach(item =>
                {
                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, item.optionseq.ToString(), item);
                });
            }

            RedisConst.GetRedisInstance().SetExpireTimeHash(DataManager_Define.RedisServerAlias_User, setKey);

            return retObj;
        }

        public static List<User_Inven> GetCharacterInvenList(ref TxnBlock TB, long AID, long CID, bool bWithOption = true, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            List<User_Inven> retObj = new List<User_Inven>();

            {
                string setKey = string.Format("{0}_{1}_{2}_{3}", Dummy_Define.User_Inven_Prefix, Dummy_Define.Item_User_Inven_Table, AID, CID);
                List<User_Inven> setObj = new List<User_Inven>();
                if (!Flush)
                {
                    setObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_Hash_All<User_Inven>(DataManager_Define.RedisServerAlias_User, setKey);

                    if (setObj.Count == 0)
                        Flush = true;
                }
                if (Flush)
                {
                    RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
                    string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND cid = {2}", Dummy_Define.Item_User_Inven_Table, AID, CID);
                    setObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_Inven>(ref TB, setQuery, dbkey);

                    if (bWithOption)
                    {
                        List<User_Inven_Option> option_list = DummyManager.GetInvenOptionList(ref TB, AID, CID, Flush, dbkey);
                        setObj.ForEach(item =>
                        {
                            item.base_option = option_list.Where(setoption => setoption.isbase.Equals("Y") && setoption.invenseq == item.invenseq).ToList();
                            item.random_option = option_list.Where(setoption => setoption.isbase.Equals("N") && setoption.invenseq == item.invenseq).ToList();
                            retObj.Add(item);
                            RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, item.invenseq.ToString(), item);
                        });
                    }
                    else
                    {
                        setObj.ForEach(item =>
                        {
                            item.base_option = new List<User_Inven_Option>();
                            item.random_option = new List<User_Inven_Option>();
                            retObj.Add(item);
                        });
                    }
                }
                else
                    retObj.AddRange(setObj);

                RedisConst.GetRedisInstance().SetExpireTimeHash(DataManager_Define.RedisServerAlias_User, setKey);

                if (CID > 0)
                {
                    string setEquipKey = DummyManager.GetEquipKey(AID, CID);
                    RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setEquipKey);

                    setObj.Where(item => item.equipflag.Equals("Y")).ToList().ForEach(item =>
                    {
                        RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setEquipKey, item.invenseq.ToString(), item);
                    }
                    );
                    RedisConst.GetRedisInstance().SetExpireTimeHash(DataManager_Define.RedisServerAlias_User, setEquipKey);
                }
            }

            return retObj;
        }

        public static Result_Define.eResult MakeActiveSoul(ref TxnBlock TB, long AID, long soulID, long soulGroup, short grade, int level, int starlevel, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = {1} AND soulgroup = {2}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
                                                    creation_date = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT (aid, soulid, soulgroup, soulparts_ea,
							                                grade, level, starlevel, 
							                                --special_buffid1, special_buffid2, special_buffid3,
							                                creation_date, delflag )
                                                   VALUES ({1}, {2}, {3}, 0, {4}, {5}, {6}, GETDATE(), 'N');
                                                ", Soul_Define.User_ActiveSoul_Table, AID, soulID, soulGroup, grade, level, starlevel);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static List<System_Soul_Active> GetSoul_System_Soul_Active_Random(ref TxnBlock TB, int getCount = 1, string dbkey = Soul_Define.Soul_InvenDB)
        {
            if (getCount < 1)
                return new List<System_Soul_Active>();
            string setQuery = string.Format("SELECT TOP {1} * FROM {0} WITH(NOLOCK) ORDER BY NEWID()", Soul_Define.Soul_System_Soul_Active_Table, getCount);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_Soul_Active>(ref TB, setQuery, dbkey);
        }

        public static List<Ret_Equip_Soul_Active> GetRet_Active_Soul_Equip_List(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            List<User_ActiveSoul> getActiveSoulList = DummyManager.GetUser_ActiveSoul(ref TB, AID, Flush);
            List<User_Character_Equip_Soul> getEquipList = DummyManager.GetUser_Character_Equip_Soul(ref TB, AID, CID, Flush);
            List<User_ActiveSoul_Equip> getSoulEquipList = new List<User_ActiveSoul_Equip>();
            List<User_ActiveSoul_Special_Buff> getSoulBuffList = DummyManager.GetUser_ActiveSoul_Special_Buff(ref TB, AID, CID, Flush);

            return Ret_Equip_Soul_Active.makeActiveSoulRetEquipList(ref TB, AID, CID, ref getActiveSoulList, ref getEquipList, ref getSoulEquipList, ref getSoulBuffList);
        }


        public static List<User_ActiveSoul> GetUser_ActiveSoul(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setKey = string.Format("{0}_{1}_{2}", Dummy_Define.SystemSoul_Prefix, Dummy_Define.User_ActiveSoul_Table, AID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1}", Dummy_Define.User_ActiveSoul_Table, AID);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<User_ActiveSoul>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
        }


        public static List<User_Character_Equip_Soul> GetUser_Character_Equip_Soul(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", Dummy_Define.SystemSoul_Prefix, Dummy_Define.User_Character_Equip_Soul_Table, AID, CID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND cid = {2} AND soulseq > 0 and slot_num > 0", Dummy_Define.User_Character_Equip_Soul_Table, AID, CID);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<User_Character_Equip_Soul>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
        }

        public static List<User_ActiveSoul_Special_Buff> GetUser_ActiveSoul_Special_Buff(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", Dummy_Define.SystemSoul_Prefix, Dummy_Define.User_ActiveSoul_Special_Buff_Table, AID, CID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND cid = {2}", Dummy_Define.User_ActiveSoul_Special_Buff_Table, AID, CID);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<User_ActiveSoul_Special_Buff>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
        }

        public static List<Ret_Soul_Passive> GetRet_Passive_Soul_Equip_List(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            List<User_PassiveSoul> getPassiveSoulList = DummyManager.GetUser_PassiveSoul(ref TB, AID, CID, Flush);
            List<User_Character_Equip_Soul> getEquipList = DummyManager.GetUser_Character_Equip_Soul(ref TB, AID, CID, Flush);

            return Ret_Soul_Passive.makePassiveSoulRetList(ref getPassiveSoulList, getEquipList, true);
        }

        public static List<User_PassiveSoul> GetUser_PassiveSoul(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Dummy_Define.DummyInfo_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}_{3}", Dummy_Define.SystemSoul_Prefix, Dummy_Define.User_PassiveSoul_Table, AID, CID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND cid = {2}", Dummy_Define.User_PassiveSoul_Table, AID, CID);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<User_PassiveSoul>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
        }

        public static List<Character_Detail> makeDummyCharacterListInfo(ref TxnBlock TB, long AID, long dummyAID, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            List<Character_Detail> enemyCharList = DummyManager.GetCharacterListWithEquip(ref TB, dummyAID).OrderBy(charInfo => charInfo.warpoint).ToList();
            
            return enemyCharList;
        }
    }
}
