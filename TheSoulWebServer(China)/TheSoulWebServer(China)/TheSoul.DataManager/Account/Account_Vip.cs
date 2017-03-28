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
    public class System_VIP_RewardBox
    {
        public long Reward_ID { get; set; }
        public int VIP_Level { get; set; }
        public short ClassType { get; set; }
        public long Item_ID { get; set; }
        public short Level { get; set; }
        public short Grade { get; set; }
        public int Num { get; set; }
    }

    public class System_VIP_Level
    {
        public short VIP_Level { get; set; }
        public int LevelUpPoint { get; set; }
        public string Tooltip { get; set; }
        public string Dev_Description { get; set; }
    }

    public class System_VIP_Favor
    {
        public int VIP_Faver_ID { get; set; }
        public short VIP_Level { get; set; }
        public string VIP_Favor_Type { get; set; }
        public int VIP_Favor_Value { get; set; }
    }
    
    public class User_VIP
    {
        public long aid { get; set; }
        public int viplevel { get; set; }
        public int vippoint { get; set; }
        public int totalvippoint { get; set; }
        public DateTime regdate { get; set; }
    }
}

namespace TheSoul.DataManager
{
    public class VIP_Define
    {
        public const string User_VIP_TableName = "User_Vip";
        public const string User_VIP_Prefix = "User_Vip";
        public const string System_VIP_Surfix = "info";
        public const string VIP_ExpTableName = "System_VIP_Level";
        public const string VIP_FavorTableName = "System_VIP_Favor";
        public const string VIP_RewardTableName = "System_VIP_RewardBox";
        public const string VIP_InfoDB = "sharding";

        public enum eVipType
        {
            BAGSLOT_MAX_ITEM,
            BAGSLOT_MAX_ACC,
            BAGSLOT_MAX_PASSIVESOUL,
            DUNGEONCOUNT_MAX_1VS1REAL,
            DUNGEONCOUNT_MAX_CO_OP,
            DUNGEONCOUNT_MAX_DARK,
            DUNGEONCOUNT_MAX_ELITE,
            DUNGEONCOUNT_MAX_NORMAL,
            DUNGEONCOUNT_MAX_RANKING,
            DUNGEONCOUNT_MAX_GLADIATOR,
            DUNGEONCOUNT_RESET_CO_OP,
            DUNGEONCOUNT_RESET_DARK,
            DUNGEONCOUNT_RESET_ELITE,
            DUNGEONCOUNT_RESET_EXPEDITION,
            DUNGEONCOUNT_RESET_NORMAL,
            EXPEDITION_HERO_REGI_MAX,
            EXPEDITION_HERO_HIRE_MAX,
            SHOPCOUNT_GOLD,
            SHOP_BUY_MAX_KEY,
            SHOP_BUY_MAX_TIKET,
            SHOP_BUY_MAX_GOLD,
            SHOP_RESET_BATTLE,
            SHOP_RESET_CO_OP,
            SHOP_RESET_FELLOW,
            SHOP_RESET_GUILD,
            SHOP_RESET_HONOR,
            SHOP_RESET_PVP,
            SHOP_RESET_BLACKMARKET,
            UNLOCK_CHARACTER_1ST,
            UNLOCK_CHARACTER_2ND,
            UNLOCK_CHARACTER_3RD,
            UNLOCK_1STARSWEEP,
            UNLOCK_5SWEEP,
            UNLOCK_5SWEEP_DARK,
            UNLOCK_5SWEEP_ELITE,
            UNLOCK_SPECIALCHEST,
            SHOP_OPEN_BLACKMARKET_1,
            SHOP_OPEN_BLACKMARKET_2,
            SHOP_BUY_MAX_ORB,
            SHOP_BUY_MAX_BREAKSTONE,
        }

        public static readonly Dictionary<eVipType, string> Vip_Type_Key_List = new Dictionary<eVipType, string>()
        {
            { eVipType.BAGSLOT_MAX_ITEM, "BAGSLOT_MAX_ITEM"},
            { eVipType.BAGSLOT_MAX_ACC, "BAGSLOT_MAX_ACC"},
            { eVipType.BAGSLOT_MAX_PASSIVESOUL, "BAGSLOT_MAX_PASSIVESOUL"},
            { eVipType.DUNGEONCOUNT_MAX_1VS1REAL, "DUNGEONCOUNT_MAX_1VS1REAL"},
            { eVipType.DUNGEONCOUNT_MAX_CO_OP, "DUNGEONCOUNT_MAX_CO_OP"},
            { eVipType.DUNGEONCOUNT_MAX_DARK, "DUNGEONCOUNT_MAX_DARK"},
            { eVipType.DUNGEONCOUNT_MAX_ELITE, "DUNGEONCOUNT_MAX_ELITE"},
            { eVipType.DUNGEONCOUNT_MAX_NORMAL, "DUNGEONCOUNT_MAX_NORMAL"},
            { eVipType.DUNGEONCOUNT_MAX_RANKING, "DUNGEONCOUNT_MAX_RANKING"},
            { eVipType.DUNGEONCOUNT_MAX_GLADIATOR, "DUNGEONCOUNT_MAX_GLADIATOR"},
            { eVipType.DUNGEONCOUNT_RESET_CO_OP, "DUNGEONCOUNT_RESET_CO_OP"},
            { eVipType.DUNGEONCOUNT_RESET_DARK, "DUNGEONCOUNT_RESET_DARK"},
            { eVipType.DUNGEONCOUNT_RESET_ELITE, "DUNGEONCOUNT_RESET_ELITE"},
            { eVipType.DUNGEONCOUNT_RESET_EXPEDITION, "DUNGEONCOUNT_RESET_EXPEDITION"},
            { eVipType.DUNGEONCOUNT_RESET_NORMAL, "DUNGEONCOUNT_RESET_NORMAL"},
            { eVipType.EXPEDITION_HERO_REGI_MAX, "EXPEDITION_HERO_REGI_MAX"},
            { eVipType.EXPEDITION_HERO_HIRE_MAX, "EXPEDITION_HERO_HIRE_MAX"},
            { eVipType.SHOPCOUNT_GOLD, "SHOPCOUNT_GOLD"},
            { eVipType.SHOP_BUY_MAX_KEY, "SHOP_BUY_MAX_KEY"},
            { eVipType.SHOP_BUY_MAX_TIKET, "SHOP_BUY_MAX_TIKET"},
            { eVipType.SHOP_BUY_MAX_GOLD, "SHOP_BUY_MAX_GOLD"},
            { eVipType.SHOP_RESET_BATTLE, "SHOP_RESET_BATTLE"},
            { eVipType.SHOP_RESET_CO_OP, "SHOP_RESET_CO_OP"},
            { eVipType.SHOP_RESET_FELLOW, "SHOP_RESET_FELLOW"},
            { eVipType.SHOP_RESET_GUILD, "SHOP_RESET_GUILD"},
            { eVipType.SHOP_RESET_HONOR, "SHOP_RESET_HONOR"},
            { eVipType.SHOP_RESET_PVP, "SHOP_RESET_PVP"},
            { eVipType.SHOP_RESET_BLACKMARKET, "SHOP_RESET_BLACKMARKET"},
            { eVipType.UNLOCK_CHARACTER_1ST, "UNLOCK_CHARACTER_1ST"},
            { eVipType.UNLOCK_CHARACTER_2ND, "UNLOCK_CHARACTER_2ND"},
            { eVipType.UNLOCK_CHARACTER_3RD, "UNLOCK_CHARACTER_3RD"},
            { eVipType.UNLOCK_1STARSWEEP, "UNLOCK_1STARSWEEP"},
            { eVipType.UNLOCK_5SWEEP, "UNLOCK_5SWEEP"},
            { eVipType.UNLOCK_5SWEEP_DARK, "UNLOCK_5SWEEP_DARK"},
            { eVipType.UNLOCK_5SWEEP_ELITE, "UNLOCK_5SWEEP_ELITE"},
            { eVipType.UNLOCK_SPECIALCHEST, "UNLOCK_SPECIALCHEST"},
            { eVipType.SHOP_OPEN_BLACKMARKET_1, "SHOP_OPEN_BLACKMARKET_1"},
            { eVipType.SHOP_OPEN_BLACKMARKET_2, "SHOP_OPEN_BLACKMARKET_2"},
            { eVipType.SHOP_BUY_MAX_ORB, "SHOP_BUY_MAX_ORB"},
            { eVipType.SHOP_BUY_MAX_BREAKSTONE, "SHOP_BUY_MAX_BREAKSTONE"},
        };


        public static readonly Dictionary<string, eVipType> Vip_StringToType_Key_List = new Dictionary<string, eVipType>()
        {
            { "BAGSLOT_MAX_ITEM", eVipType.BAGSLOT_MAX_ITEM },
            { "BAGSLOT_MAX_ACC", eVipType.BAGSLOT_MAX_ACC },
            { "BAGSLOT_MAX_PASSIVESOUL", eVipType.BAGSLOT_MAX_PASSIVESOUL },
            { "DUNGEONCOUNT_MAX_1VS1REAL", eVipType.DUNGEONCOUNT_MAX_1VS1REAL },
            { "DUNGEONCOUNT_MAX_CO_OP", eVipType.DUNGEONCOUNT_MAX_CO_OP },
            { "DUNGEONCOUNT_MAX_DARK", eVipType.DUNGEONCOUNT_MAX_DARK },
            { "DUNGEONCOUNT_MAX_ELITE", eVipType.DUNGEONCOUNT_MAX_ELITE },
            { "DUNGEONCOUNT_MAX_NORMAL", eVipType.DUNGEONCOUNT_MAX_NORMAL },
            { "DUNGEONCOUNT_MAX_RANKING", eVipType.DUNGEONCOUNT_MAX_RANKING },
            { "DUNGEONCOUNT_RESET_CO_OP", eVipType.DUNGEONCOUNT_RESET_CO_OP },
            { "DUNGEONCOUNT_RESET_DARK", eVipType.DUNGEONCOUNT_RESET_DARK },
            { "DUNGEONCOUNT_RESET_EXPEDITION", eVipType.DUNGEONCOUNT_RESET_EXPEDITION },
            { "DUNGEONCOUNT_RESET_NORMAL", eVipType.DUNGEONCOUNT_RESET_NORMAL },
            { "EXPEDITION_HERO_REGI_MAX", eVipType.EXPEDITION_HERO_REGI_MAX },
            { "EXPEDITION_HERO_HIRE_MAX", eVipType.EXPEDITION_HERO_HIRE_MAX },
            { "SHOPCOUNT_GOLD", eVipType.SHOPCOUNT_GOLD },
            { "SHOP_BUY_MAX_GOLD", eVipType.SHOP_BUY_MAX_GOLD },
            { "SHOP_BUY_MAX_KEY", eVipType.SHOP_BUY_MAX_KEY },
            { "SHOP_BUY_MAX_TIKET", eVipType.SHOP_BUY_MAX_TIKET },
            { "SHOP_RESET_BATTLE", eVipType.SHOP_RESET_BATTLE },
            { "SHOP_RESET_CO_OP", eVipType.SHOP_RESET_CO_OP },
            { "SHOP_RESET_FELLOW", eVipType.SHOP_RESET_FELLOW },
            { "SHOP_RESET_GUILD", eVipType.SHOP_RESET_GUILD },
            { "SHOP_RESET_HONOR", eVipType.SHOP_RESET_HONOR },
            { "SHOP_RESET_PVP", eVipType.SHOP_RESET_PVP },
            { "UNLOCK_CHARACTER_1ST", eVipType.UNLOCK_CHARACTER_1ST },
            { "UNLOCK_CHARACTER_2ND", eVipType.UNLOCK_CHARACTER_2ND },
            { "UNLOCK_CHARACTER_3RD", eVipType.UNLOCK_CHARACTER_3RD },
            { "UNLOCK_5SWEEP", eVipType.UNLOCK_5SWEEP },
            { "UNLOCK_SPECIALCHEST", eVipType.UNLOCK_SPECIALCHEST },
        };

        public static readonly Dictionary<int, string> VIP_Reward_Mail_title = new Dictionary<int, string>()
        {
            { (int)Character_Define.SystemClassType.Class_None,         "#STRING_MSG_VIP_REWARD_TITLE_C0" },
            { (int)Character_Define.SystemClassType.Class_Warrior,      "#STRING_MSG_VIP_REWARD_TITLE_C1" },
            { (int)Character_Define.SystemClassType.Class_Swordmaster,  "#STRING_MSG_VIP_REWARD_TITLE_C2" },
            { (int)Character_Define.SystemClassType.Class_Taoist,       "#STRING_MSG_VIP_REWARD_TITLE_C3" },
        };
        public static readonly Dictionary<int, string> VIP_Reward_Mail_Body = new Dictionary<int, string>()
        {
            { (int)Character_Define.SystemClassType.Class_None,         "#STRING_MSG_VIP_REWARD_DESCRIPTION_C0" },
            { (int)Character_Define.SystemClassType.Class_Warrior,      "#STRING_MSG_VIP_REWARD_DESCRIPTION_C1" },
            { (int)Character_Define.SystemClassType.Class_Swordmaster,  "#STRING_MSG_VIP_REWARD_DESCRIPTION_C2" },
            { (int)Character_Define.SystemClassType.Class_Taoist,       "#STRING_MSG_VIP_REWARD_DESCRIPTION_C3" },
        };
    }
    public static partial class VipManager
    {
        private static string GetRediskey_UserVipInfo(long AID)
        {
            return string.Format("{0}_{1}_{2}", VIP_Define.User_VIP_TableName, AID, VIP_Define.User_VIP_Prefix);
        }

        public static User_VIP GetUser_VIPInfo(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = VIP_Define.VIP_InfoDB)
        {
            string setKey = GetRediskey_UserVipInfo(AID);

            User_VIP userVIPInfo = Flush ? null : GenericFetch.FetchFromOnly_Redis<User_VIP>(DataManager_Define.RedisServerAlias_User, setKey);
            if (userVIPInfo == null)
            {
                SqlCommand commandUser_MissionInfo = new SqlCommand();
                commandUser_MissionInfo.CommandText = "System_Get_UserVIPInfo";
                var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                commandUser_MissionInfo.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                commandUser_MissionInfo.Parameters.Add(result);

                SqlDataReader getDr = null;
                if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_MissionInfo, ref getDr))
                {
                    if (getDr != null)
                    {
                        var r = SQLtoJson.Serialize(ref getDr);
                        string json = mJsonSerializer.ToJsonString(r);
                        getDr.Dispose(); getDr.Close();
                        int checkValue = System.Convert.ToInt32(result.Value);
                        commandUser_MissionInfo.Dispose();
                        if (checkValue < 0)
                            return new User_VIP();

                        User_VIP[] retSet = mJsonSerializer.JsonToObject<User_VIP[]>(json);

                        if (retSet.Length > 0)
                            userVIPInfo = retSet[0];
                        else
                            return new User_VIP();

                        RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, userVIPInfo);
                    }
                }
                else
                {
                    commandUser_MissionInfo.Dispose();
                    return new User_VIP();
                }
            }

            TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_vip_level], userVIPInfo.viplevel);

            return userVIPInfo;
        }

        public static void RemoveUser_VIPCache(long AID)
        {
            string setKey = GetRediskey_UserVipInfo(AID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);            
        }

        public static Result_Define.eResult VIPPointAdd(ref TxnBlock TB, long AID, int EXP, string dbkey = VIP_Define.VIP_InfoDB)
        {
            User_VIP vipInfo = GetUser_VIPInfo(ref TB, AID);
            int CurLevel = vipInfo.viplevel;
            long SetExp = 0;
            int SetLevel = CalcVIPLevel(ref TB, vipInfo.totalvippoint, EXP, ref SetExp, dbkey);
            Result_Define.eResult retErr = Result_Define.eResult.SUCCESS;
            if (retErr == Result_Define.eResult.SUCCESS)
            {
                string setQuery = string.Format(@"UPDATE {0} SET TotalVIPPoint=TotalVIPPoint+({1}) , VIPLevel={2}, VIPPoint={3} WHERE AID={4}", VIP_Define.User_VIP_TableName, EXP, SetLevel, SetExp, AID);
                retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }

            if (retErr == Result_Define.eResult.SUCCESS)
            {
                TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.VIP_Point, 0, 0, EXP);
                RemoveUser_VIPCache(AID);
            }

            return retErr;
        }


        private static int CalcVIPLevel(ref TxnBlock TB, long curExp, long addExp, ref long setExp, string dbkey = VIP_Define.VIP_InfoDB)
        {
            string setKey = string.Format("{0}", VIP_Define.VIP_ExpTableName);
            List<System_VIP_Level> expList = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_Hash_All<System_VIP_Level>(DataManager_Define.RedisServerAlias_System, setKey);
            //TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<System_VIP>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey);

            if (expList == null || expList.Count < 1)
            {
                string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)", VIP_Define.VIP_ExpTableName);
                expList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<System_VIP_Level>(ref TB, setQuery, dbkey);

                foreach (System_VIP_Level setObj in expList)
                {
                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_System, setKey, setObj.VIP_Level.ToString(), setObj);
                }
                RedisConst.GetRedisInstance().SetExpireTimeHash(DataManager_Define.RedisServerAlias_System, setKey);
            }

            if (expList.Count < 1)
                return 0;

            int SetLevel = 0;
            long Exp = curExp + addExp;
            long lastCheckExp = 0;

            List<System_VIP_Level> checkList = expList.OrderBy(item => item.VIP_Level).ToList();

            foreach (System_VIP_Level checkExp in checkList)
            {
                SetLevel = checkExp.VIP_Level;
                if (Exp < checkExp.LevelUpPoint)
                {
                    setExp = Exp - lastCheckExp;
                    break;
                }
                lastCheckExp = checkExp.LevelUpPoint;
            }

            return SetLevel;
        }

        public static System_VIP_Level GetSystem_VIP_Level(ref TxnBlock TB, int VIPLevel, bool Flush = false, string dbkey = VIP_Define.VIP_InfoDB)
        {
            string setKey = string.Format("{0}_{1}", VIP_Define.VIP_ExpTableName, VIP_Define.System_VIP_Surfix);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE VIP_Level = {1}", VIP_Define.VIP_ExpTableName, VIPLevel);

            System_VIP_Level retObj = GenericFetch.FetchFromRedis_Hash<System_VIP_Level>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, VIPLevel.ToString(), setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new System_VIP_Level();
        }

        public static System_VIP_Favor GetSystem_VIP_Favor(ref TxnBlock TB, int VIPLevel, string VIPType, bool Flush = false, string dbkey = VIP_Define.VIP_InfoDB)
        {
            string setKey = string.Format("{0}_{1}", VIP_Define.VIP_FavorTableName, VIP_Define.System_VIP_Surfix);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE VIP_Level = {1} AND VIP_Favor_Type = '{2}'", VIP_Define.VIP_FavorTableName, VIPLevel, VIPType);
            string setDict = string.Format("{0}_{1}", VIPLevel, VIPType);
            System_VIP_Favor retObj = GenericFetch.FetchFromRedis_Hash<System_VIP_Favor>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setDict, setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new System_VIP_Favor();
        }

        public static List<System_VIP_RewardBox> GetSystem_VIP_RewardList(ref TxnBlock TB, int VIPLevel, bool Flush = false, string dbkey = VIP_Define.VIP_InfoDB)
        {
            string setKey = string.Format("{0}_{1}", VIP_Define.VIP_RewardTableName, VIP_Define.System_VIP_Surfix);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE VIP_Level = {1}", VIP_Define.VIP_RewardTableName, VIPLevel);
            return GenericFetch.FetchFromRedis_MultipleRow_Hash<System_VIP_RewardBox>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, VIPLevel.ToString(), setQuery, dbkey, Flush);
        }

        public static string GetVIPRewardMailTitle(int VIPLevel, int setClass)
        {
            return string.Format("{0}##{1}", VIP_Define.VIP_Reward_Mail_title[setClass], VIPLevel);
        }

        public static string GetVIPRewardMailBody(int VIPLevel, int setClass)
        {
            return string.Format("{0}##{1}", VIP_Define.VIP_Reward_Mail_Body[setClass], VIPLevel);
        }

        //public static User_VIP GetUser_VIPInfo(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = VIP_Define.VIP_InfoDB)
        //{
        //    string setKey = string.Format("{0}_{1}_{2}", VIP_Define.User_VIP_TableName, AID, VIP_Define.System_VIP_Surfix);
        //    string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE aid = {1}", VIP_Define.User_VIP_TableName, AID);

        //    User_VIP retObj = GenericFetch.FetchFromRedis<User_VIP>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
        //    return (retObj != null) ? retObj : new User_VIP();
        //}

        public static int User_Vip_Value(ref TxnBlock TB, long AID, VIP_Define.eVipType VIPType)
        {
            return User_Vip_Value(ref TB, AID, VIP_Define.Vip_Type_Key_List[VIPType]);
        }
            
        public static int User_Vip_Value(ref TxnBlock TB, long AID, string VIPType, bool Flush = false, string dbkey = VIP_Define.VIP_InfoDB)
        {
            User_VIP getUserVipInfo = GetUser_VIPInfo(ref TB, AID, Flush);
            System_VIP_Favor getSystemVip = GetSystem_VIP_Favor(ref TB, getUserVipInfo.viplevel, VIPType, Flush);
            return getSystemVip.VIP_Favor_Value;
        }

        public static bool CheckVIPCountOver(ref TxnBlock TB, long AID, long CID, VIP_Define.eVipType VIPType, int CheckUserValue = 0, int AddCount = 0)
        {
            if (VIPType == VIP_Define.eVipType.BAGSLOT_MAX_ITEM || VIPType == VIP_Define.eVipType.BAGSLOT_MAX_ACC)
            {
                int CharBagCount = SystemData.GetConstValueInt(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_WEAPONUI_ITEM_SLOT_MAX]) + Item_Define.Bag_Capacity_Limit_Over_Count;
                int AccBagCount = SystemData.GetConstValueInt(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_ARMORUI_ITEM_SLOT_MAX]) + Item_Define.Bag_Capacity_Limit_Over_Count;

                User_Inven_Count userInvenCount = ItemManager.GetUserInvenCount(ref TB, AID, CID);
                return CharBagCount >= userInvenCount.Character_Inven_Count && AccBagCount >= userInvenCount.Account_Inven_Count;
            }

            int UserVipCount = User_Vip_Value(ref TB, AID, VIPType) + AddCount;
            switch (VIPType)
            {
                //case VIP_Define.eVipType.BAGSLOT_MAX_ITEM:
                //case VIP_Define.eVipType.BAGSLOT_MAX_ACC:
                //    User_Inven_Count userInvenCount = ItemManager.GetUserInvenCount(ref TB, AID, CID);
                //    return VIPType == VIP_Define.eVipType.BAGSLOT_MAX_ITEM ? UserVipCount >= userInvenCount.Character_Inven_Count : UserVipCount >= userInvenCount.Account_Inven_Count;

                case VIP_Define.eVipType.BAGSLOT_MAX_PASSIVESOUL:
                    User_Soul_Passive_Store_Count userSoulCount = SoulManager.GetUserPsssiveSoulStoreCount(ref TB, AID, CID);
                    return UserVipCount > userSoulCount.Passive_Store_Count;

                case VIP_Define.eVipType.DUNGEONCOUNT_MAX_NORMAL:
                case VIP_Define.eVipType.DUNGEONCOUNT_MAX_ELITE:
                case VIP_Define.eVipType.DUNGEONCOUNT_MAX_DARK:
                case VIP_Define.eVipType.DUNGEONCOUNT_MAX_1VS1REAL:
                case VIP_Define.eVipType.DUNGEONCOUNT_MAX_CO_OP:
                case VIP_Define.eVipType.DUNGEONCOUNT_MAX_RANKING:
                case VIP_Define.eVipType.DUNGEONCOUNT_MAX_GLADIATOR:
                    return UserVipCount >= CheckUserValue;

                case VIP_Define.eVipType.DUNGEONCOUNT_RESET_CO_OP:
                case VIP_Define.eVipType.DUNGEONCOUNT_RESET_DARK:
                case VIP_Define.eVipType.DUNGEONCOUNT_RESET_EXPEDITION:
                case VIP_Define.eVipType.DUNGEONCOUNT_RESET_NORMAL:
                case VIP_Define.eVipType.DUNGEONCOUNT_RESET_ELITE:
                    return UserVipCount > CheckUserValue;

                case VIP_Define.eVipType.SHOPCOUNT_GOLD:
                case VIP_Define.eVipType.SHOP_BUY_MAX_KEY:
                case VIP_Define.eVipType.SHOP_BUY_MAX_TIKET:
                case VIP_Define.eVipType.SHOP_BUY_MAX_GOLD:
                    return UserVipCount >= CheckUserValue;

                case VIP_Define.eVipType.UNLOCK_CHARACTER_1ST:
                case VIP_Define.eVipType.UNLOCK_CHARACTER_2ND:
                case VIP_Define.eVipType.UNLOCK_CHARACTER_3RD:
                case VIP_Define.eVipType.UNLOCK_1STARSWEEP:
                case VIP_Define.eVipType.UNLOCK_5SWEEP:
                case VIP_Define.eVipType.UNLOCK_5SWEEP_DARK:
                case VIP_Define.eVipType.UNLOCK_5SWEEP_ELITE:
                    return UserVipCount > 0;

                case VIP_Define.eVipType.SHOP_OPEN_BLACKMARKET_1:
                case VIP_Define.eVipType.SHOP_OPEN_BLACKMARKET_2:
                    return UserVipCount > 0;

                case VIP_Define.eVipType.UNLOCK_SPECIALCHEST:
                    return UserVipCount > 0;

                case VIP_Define.eVipType.SHOP_RESET_BATTLE:
                case VIP_Define.eVipType.SHOP_RESET_CO_OP:
                case VIP_Define.eVipType.SHOP_RESET_FELLOW:
                case VIP_Define.eVipType.SHOP_RESET_GUILD:
                case VIP_Define.eVipType.SHOP_RESET_HONOR:
                case VIP_Define.eVipType.SHOP_RESET_PVP:
                case VIP_Define.eVipType.SHOP_RESET_BLACKMARKET:
                case VIP_Define.eVipType.SHOP_BUY_MAX_BREAKSTONE:
                case VIP_Define.eVipType.SHOP_BUY_MAX_ORB:
                    return UserVipCount >= CheckUserValue;

                case VIP_Define.eVipType.EXPEDITION_HERO_REGI_MAX:
                case VIP_Define.eVipType.EXPEDITION_HERO_HIRE_MAX:
                    return UserVipCount > CheckUserValue;

                default:
                    break;

            }
            return false;
        }
    }
}