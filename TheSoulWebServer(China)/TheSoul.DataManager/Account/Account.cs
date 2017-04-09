using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;

namespace TheSoul.DataManager
{
    public static partial class AccountManager
    {
        public static long GlobalGet_UAID(ref TxnBlock TB, ref string platform_user_id, Global_Define.ePlatformType platform_type, string dbkey = DataManager_Define.GlobalDB)
        {
            SqlCommand command_userInfo = new SqlCommand();
            command_userInfo.CommandText = "System_Get_UAID";
            command_userInfo.Parameters.Add("@platform_type", SqlDbType.Int).Value = (int)platform_type;
            command_userInfo.Parameters.Add("@platform_user_id", SqlDbType.NVarChar, 128).Value = platform_user_id;
            var outputResult = new SqlParameter("@ret_result", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            var outputID = new SqlParameter("@ret_platform_user_id", SqlDbType.NVarChar, 128) { Direction = ParameterDirection.Output };
            command_userInfo.Parameters.Add(outputResult);
            command_userInfo.Parameters.Add(outputID);

            long retValue = 0;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref command_userInfo))
            {
                platform_user_id = System.Convert.ToString(outputID.Value);
                retValue = System.Convert.ToInt64(outputResult.Value);
            }

            command_userInfo.Dispose();
            return retValue;
        }

        public static Result_Define.eResult GlobalPlatformID_Update(ref TxnBlock TB, string platform_user_id, string old_platform_user_id, long AID, long platform_idx, string dbkey = DataManager_Define.GlobalDB)
        {
            SqlCommand command_userInfo = new SqlCommand();
            command_userInfo.CommandText = "System_PlatformID_Update";
            command_userInfo.Parameters.Add("@user_index_id", SqlDbType.BigInt).Value = AID;
            command_userInfo.Parameters.Add("@platform_idx", SqlDbType.BigInt).Value = platform_idx;
            command_userInfo.Parameters.Add("@platform_user_id", SqlDbType.NVarChar, 128).Value = platform_user_id;
            command_userInfo.Parameters.Add("@old_platform_user_id", SqlDbType.NVarChar, 128).Value = old_platform_user_id;
            var outputResult = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command_userInfo.Parameters.Add(outputResult);

            int retValue = -1;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref command_userInfo))
                retValue = System.Convert.ToInt32(outputResult.Value);
            command_userInfo.Dispose();
            return (retValue < 0) ? Result_Define.eResult.DB_STOREDPROCEDURE_ERROR : Result_Define.eResult.SUCCESS;
        }

        public static Result_Define.eResult mSeedPlatformID_Update(ref TxnBlock TB, long AID, string platform_user_id, Global_Define.ePlatformType platform_type, string dbkey = DataManager_Define.GlobalDB)
        {
            Result_Define.eResult retError = GlobalmSeedPlatformID_Update(ref TB, AID, platform_user_id, platform_type);
            if (retError == Result_Define.eResult.SUCCESS)
                retError = CommonPlatformID_LinkChange(ref TB, platform_user_id, AID);
            return retError;
        }

        public static Result_Define.eResult GlobalmSeedPlatformID_Update(ref TxnBlock TB, long AID, string platform_user_id, Global_Define.ePlatformType platform_type, string dbkey = DataManager_Define.GlobalDB)
        {
            SqlCommand command_userInfo = new SqlCommand();
            command_userInfo.CommandText = "System_mSeed_PlatformID_Update";
            command_userInfo.Parameters.Add("@user_index_id", SqlDbType.BigInt).Value = AID;
            command_userInfo.Parameters.Add("@after_platform_user_id", SqlDbType.NVarChar, 128).Value = platform_user_id;
            command_userInfo.Parameters.Add("@after_platform_type", SqlDbType.Int).Value = (int)platform_type;
            //command_userInfo.Parameters.Add("@before_platform_user_id", SqlDbType.NVarChar, 128).Value = old_platform_user_id;
            var outputResult = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            command_userInfo.Parameters.Add(outputResult);

            int retValue = -1;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref command_userInfo))
                retValue = System.Convert.ToInt32(outputResult.Value);
            command_userInfo.Dispose();
            return (retValue < 0) ? Result_Define.eResult.DB_STOREDPROCEDURE_ERROR : Result_Define.eResult.SUCCESS;
        }

        public static Result_Define.eResult GlobalAccountDrop(ref TxnBlock TB, long AID, string platform_user_id, Global_Define.ePlatformType platform_type, string dbkey = DataManager_Define.GlobalDB)
        {
            platform_user_id = string.Format("{0}_#Deleted_{1}_{2}", platform_user_id, AID, (int)platform_type);
            platform_type = Global_Define.ePlatformType.EPlatformType_DropAccount;
            Result_Define.eResult retError = GlobalmSeedPlatformID_Update(ref TB, AID, platform_user_id, platform_type);
            if (retError == Result_Define.eResult.SUCCESS)
                retError = CommonPlatformID_LinkChange(ref TB, platform_user_id, AID);

            return retError;
        }

        public static Result_Define.eResult CommonPlatformID_LinkChange(ref TxnBlock TB, string platform_user_id, long AID, string dbkey = DataManager_Define.CommonDB)
        {
            string setQuery = string.Format("UPDATE {0} SET platform_user_id = '{1}' WHERE aid = {2}; ", Account_Define.User_Account_CommonDB_TableName, platform_user_id, AID);
            return (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }


        public static Result_Define.eResult GlobalPlatformID_Unlink(ref TxnBlock TB, string platform_user_id, long AID, string dbkey = DataManager_Define.GlobalDB)
        {
            string setQuery = string.Format("UPDATE {0} SET platform_user_id = '{1}' WHERE user_account_idx = {2}; ", Global_Define.User_Account_TableName, platform_user_id, AID);
            return (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Account_Simple GetSimpleAccountInfo(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Account_Define.AccountShardingDB)
        {
            if (AID == 0)
                return new Account_Simple();

            string setKey = string.Format("{0}_{1}_{2}_{3}", Account_Define.Account_SimpleInfo_Prefix, Account_Define.AccountDBTableName, AID, Account_Define.Account_SimpleInfo_Surfix);
            string setQuery = string.Format("SELECT AID as aid, UserName as username FROM {0} WITH(NOLOCK) WHERE AID = {1} ", Account_Define.AccountDBTableName, AID);
            Account_Simple retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<Account_Simple>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);

            if(retObj == null)
                retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<Account_Simple>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, true);

            return (retObj == null) ? new Account_Simple() : retObj;
        }

        public static Account_Simple_With_Character GetSimpleAccountCharacterInfo(ref TxnBlock TB, long AID, string dbkey = Account_Define.AccountShardingDB)
        {
            return GetSimpleAccountCharacterInfo(ref TB, AID, 0, false, dbkey);
        }

        public static Account_Simple_With_Character GetSimpleAccountCharacterInfo(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Account_Define.AccountShardingDB)
        {
            if (AID == 0)
                return new Account_Simple_With_Character();

            string setKey = string.Format("{0}_{1}_{2}_{3}", Account_Define.Account_SimpleInfo_Prefix, Account_Define.AccountDBTableName, AID, Account_Define.Account_SimpleInfo_WithEquip_Surfix);
            string setQuery = string.Format("SELECT AID as aid, UserName as username, EquipCID as cid FROM {0} WITH(NOLOCK) WHERE AID = {1} ", Account_Define.AccountDBTableName, AID);
            Account_Simple_With_Character retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<Account_Simple_With_Character>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);

            if (retObj != null)
            {
                if (CID > 0)
                    retObj.charinfo = new Character_Simple(CharacterManager.GetCharacter(ref TB, AID, CID));
                else if (retObj.cid > 0)
                    retObj.charinfo = new Character_Simple(CharacterManager.GetCharacter(ref TB, AID, retObj.cid));
                else
                    retObj.charinfo = new Character_Simple();
            }
            else
            {
                retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<Account_Simple_With_Character>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, true);
            }

            if (retObj == null)
                retObj = new Account_Simple_With_Character();

            if (retObj.charinfo == null)
            {
                if (CID > 0)
                    retObj.charinfo = new Character_Simple(CharacterManager.FlushCharacter(ref TB, AID, CID));
                else if (retObj.cid > 0)
                    retObj.charinfo = new Character_Simple(CharacterManager.FlushCharacter(ref TB, AID, retObj.cid));
                else
                    retObj.charinfo = new Character_Simple();
            }
            
            return retObj;
        }


        public static List<Account_TownSimple> GetSimpleAccount_TownInfo(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Account_Define.AccountShardingDB)
        {
            if (AID == 0)
                return new List<Account_TownSimple>();

            string setKey = string.Format("{0}_{1}_{2}_{3}", Account_Define.Account_TownInfo_Prefix, Account_Define.AccountDBTableName, AID, Account_Define.Account_TownInfo_Surfix);
            List<Account_TownSimple> retObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_MultipleRow<Account_TownSimple>(DataManager_Define.RedisServerAlias_User, setKey);

            if (retObj.Count < 1)
            {
                int userLevel = CharacterManager.GetCharacterMaxLevel_FromDB(ref TB, AID);

                List<Account_Define.FindTownUserRange> setCountInfo = Account_Define.GetTownUser_Count.FindAll(chkInfo => chkInfo.myMinLevel <= userLevel && chkInfo.myMaxLevel >= userLevel);

                int leftCount = 0;
                int findCount = 0;
                retObj = new List<Account_TownSimple>();

                foreach (Account_Define.FindTownUserRange findInfo in setCountInfo)
                {
                    if (findInfo.getVIPCount > 0)
                    {
                        findCount = findInfo.getVIPCount + leftCount;
                        string setQuery = string.Format(@"
                                                        SELECT TOP {4} A.AID as aid, A.UserName as username, A.EquipCID as cid
                                                         FROM 
	                                                            (SELECT Acc.* FROM (SELECT AID, UserName, EquipCID FROM {0} WITH(NOLOCK) WHERE LastConnTime > DATEADD(day, {7}, GETDATE()) ) as Acc WITH(NOLOCK) 
	                                                            INNER JOIN
	                                                            {1} as C WITH(NOLOCK)
	                                                            ON C.aid = Acc.AID AND Acc.EquipCID = C.cid AND C.equipflag = 'Y' 
	                                                            ) AS A
	                                                            INNER JOIN 
	                                                            {2} as B WITH(NOLOCK, INDEX(IDX_Character_cid_level))
                                                         ON A.AID = B.aid AND B.cid = A.EquipCID AND B.level >= {5} AND B.level <= {6}
                                                        WHERE A.EquipCID > 0 AND A.AID <> {3}  ORDER BY NEWID()
                                                        "
                                                            , Account_Define.AccountDBTableName, Item_Define.Item_User_Character_VIP_Costume_Table, Character_Define.CharacterTableName, AID
                                                            , findCount, findInfo.getMinLevel, findInfo.getMaxLevel, Account_Define.MaxLastConnDay);
                        List<Account_TownSimple> setObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Account_TownSimple>(ref TB, setQuery, dbkey);
                        leftCount = (findCount - setObj.Count);

                        foreach (Account_TownSimple setItem in setObj)
                        {
                            Guild guildInfo = TheSoul.DataManager.GuildManager.GetGuildInfo(ref TB, setItem.aid);
                            setItem.guild_id = guildInfo.guild_id;
                            setItem.guildname = guildInfo.guild_name;
                            setItem.guild_mark = guildInfo.guild_mark;
                            Character getCharInfo = CharacterManager.GetCharacter(ref TB, setItem.aid, setItem.cid);
                            List<User_Inven> getEquipList = ItemManager.GetEquipList(ref TB, setItem.aid, setItem.cid, false);

                            setItem.charinfo = new Character_Simple_With_Equip(getCharInfo, getEquipList);
                            setItem.charinfo.equip_ultimate = ItemManager.GetEquipUltimate(ref TB, AID, setItem.cid).FindAll(chkEquip => chkEquip.equipflag.Equals("Y"));

                            retObj.Add(setItem);
                        }
                    }

                    if (findInfo.getCount > 0)
                    {
                        findCount = findInfo.getCount - findInfo.getVIPCount + leftCount;

                        string setQuery = string.Format(@"
                                                        SELECT TOP {3} A.AID as aid, A.UserName as username, A.EquipCID as cid,  B.level
                                                            FROM 
                        	                                (SELECT AID, UserName, EquipCID FROM {0} WITH(NOLOCK) WHERE LastConnTime > DATEADD(day, {6}, GETDATE()) ) as A
                        	                                INNER JOIN 
                        	                                {1} as B WITH(NOLOCK)
                                                            ON A.AID = B.aid AND B.cid = A.EquipCID AND B.level >= {4} AND B.level <= {5}
                                                        WHERE A.EquipCID > 0 AND A.AID <> {2}  ORDER BY NEWID()
                                                        "
                                                            , Account_Define.AccountDBTableName, Character_Define.CharacterTableName, AID
                                                            , findCount, findInfo.getMinLevel, findInfo.getMaxLevel, Account_Define.MaxLastConnDay);

                        List<Account_TownSimple> setObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Account_TownSimple>(ref TB, setQuery, dbkey);
                        leftCount = (findCount - setObj.Count);

                        foreach (Account_TownSimple setItem in setObj)
                        {
                            if (retObj.Find(userinfo => userinfo.aid == setItem.aid) == null)
                            {
                                Guild guildInfo = TheSoul.DataManager.GuildManager.GetGuildInfo(ref TB, setItem.aid);
                                setItem.guild_id = guildInfo.guild_id;
                                setItem.guildname = guildInfo.guild_name;
                                setItem.guild_mark = guildInfo.guild_mark;
                                Character getCharInfo = CharacterManager.GetCharacter(ref TB, setItem.aid, setItem.cid);
                                List<User_Inven> getEquipList = ItemManager.GetEquipList(ref TB, setItem.aid, setItem.cid, false);

                                setItem.charinfo = new Character_Simple_With_Equip(getCharInfo, getEquipList);
                                setItem.charinfo.equip_ultimate = ItemManager.GetEquipUltimate(ref TB, AID, setItem.cid).FindAll(chkEquip => chkEquip.equipflag.Equals("Y"));
                                retObj.Add(setItem);
                            }
                        }
                    }
                }

                if (retObj.Count < Account_Define.TownPlayerCount)
                {
                    findCount = Account_Define.TownPlayerCount - retObj.Count;

                    string setQuery = string.Format(@"
                                                        SELECT TOP {3} A.AID as aid, A.UserName as username, A.EquipCID as cid,  B.level
                                                            FROM 
                        	                                (SELECT AID, UserName, EquipCID FROM {0} WITH(NOLOCK) WHERE LastConnTime > DATEADD(day, {6}, GETDATE()) ) as A
                        	                                INNER JOIN 
                        	                                {1} as B WITH(NOLOCK)
                                                            ON A.AID = B.aid AND B.cid = A.EquipCID AND B.level >= {4} AND B.level <= {5}
                                                        WHERE A.EquipCID > 0 AND A.AID <> {2}  ORDER BY NEWID()
                                                        "
                                    , Account_Define.AccountDBTableName, Character_Define.CharacterTableName, AID
                                    , findCount, 0, Character_Define.Max_CharacterLevel, Account_Define.MaxLastConnDay);

                    List<Account_TownSimple> setObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Account_TownSimple>(ref TB, setQuery, dbkey);
                    foreach (Account_TownSimple setItem in setObj)
                    {
                        if (retObj.Find(userinfo => userinfo.aid == setItem.aid) == null)
                        {
                            Guild guildInfo = TheSoul.DataManager.GuildManager.GetGuildInfo(ref TB, setItem.aid);
                            setItem.guild_id = guildInfo.guild_id;
                            setItem.guildname = guildInfo.guild_name;
                            setItem.guild_mark = guildInfo.guild_mark;
                            Character getCharInfo = CharacterManager.GetCharacter(ref TB, setItem.aid, setItem.cid);
                            List<User_Inven> getEquipList = ItemManager.GetEquipList(ref TB, setItem.aid, setItem.cid, false);

                            setItem.charinfo = new Character_Simple_With_Equip(getCharInfo, getEquipList);
                            setItem.charinfo.equip_ultimate = ItemManager.GetEquipUltimate(ref TB, AID, setItem.cid).FindAll(chkEquip => chkEquip.equipflag.Equals("Y"));

                            retObj.Add(setItem);
                        }
                    }
                }

                if (retObj.Count > 0)
                    RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, retObj);
            }
            
            return retObj;
        }

        private static Account FetchFromDB(ref TxnBlock TB, long AID)
        {
            SqlDataReader getDr = null;
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1}", Account_Define.AccountDBTableName, AID);
            //string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) ", DBTableName);
            TB.ExcuteSqlCommand(setQuery, ref getDr);

            if (getDr != null)            
            {
                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);

                //var setDBResult = getDr.Cast<Account>();
                getDr.Dispose(); getDr.Close();
                Account[] retSet = mJsonSerializer.JsonToObject<Account[]>(json);

                if (retSet.Length > 0)
                    return retSet[0];
                else
                    return null;
            }

            return default(Account);
        }

        private static Account FetchFromRedis(ref TxnBlock TB, long AID, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}_{2}", Account_Define.Account_Prefix, Account_Define.AccountDBTableName, AID);
            Account getAccountInfo = getAccountInfo = RedisConst.GetRedisInstance().GetObj<Account>(setKey);

            if (getAccountInfo != null && !Flush)
            {
                if (getAccountInfo.AID != AID)
                {
                    getAccountInfo = FetchFromDB(ref TB, AID);
                    RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, getAccountInfo);
                }
            }
            else
            {
                getAccountInfo = FetchFromDB(ref TB, AID);
                RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, getAccountInfo);
            }
            return getAccountInfo;
        }


        public static Result_Define.eResult AddUserGold(ref TxnBlock TB, long AID, int addValue, string dbkey = Account_Define.AccountShardingDB)                   { return AddUserValue(ref TB, AID, addValue, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult AddUserCash(ref TxnBlock TB, long AID, int addValue, string dbkey = Account_Define.AccountShardingDB)                   { return AddUserValue(ref TB, AID, 0, addValue, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult AddUserEventCash(ref TxnBlock TB, long AID, int addValue, string dbkey = Account_Define.AccountShardingDB)              { return AddUserValue(ref TB, AID, 0, 0, addValue, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult AddUserKey(ref TxnBlock TB, long AID, int addValue, string dbkey = Account_Define.AccountShardingDB)                    { return AddUserValue(ref TB, AID, 0, 0, 0, addValue, 0, 0, 0, 0, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult AddUserTicket(ref TxnBlock TB, long AID, int addValue, string dbkey = Account_Define.AccountShardingDB)                 { return AddUserValue(ref TB, AID, 0, 0, 0, 0, addValue, 0, 0, 0, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult AddUserHonor(ref TxnBlock TB, long AID, int addValue, string dbkey = Account_Define.AccountShardingDB)                  { return AddUserValue(ref TB, AID, 0, 0, 0, 0, 0, addValue, 0, 0, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult AddUserFriendlyPoint(ref TxnBlock TB, long AID, int addValue, string dbkey = Account_Define.AccountShardingDB)          { return AddUserValue(ref TB, AID, 0, 0, 0, 0, 0, 0, addValue, 0, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult AddUserSoulStone(ref TxnBlock TB, long AID, int addValue, string dbkey = Account_Define.AccountShardingDB)              { return AddUserValue(ref TB, AID, 0, 0, 0, 0, 0, 0, 0, addValue, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult AddUserExpeditionPoint(ref TxnBlock TB, long AID, int addValue, string dbkey = Account_Define.AccountShardingDB)        { return AddUserValue(ref TB, AID, 0, 0, 0, 0, 0, 0, 0, 0, addValue, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult AddUserOverlordRankingPoint(ref TxnBlock TB, long AID, int addValue, string dbkey = Account_Define.AccountShardingDB)   { return AddUserValue(ref TB, AID, 0, 0, 0, 0, 0, 0, 0, 0, 0, addValue, 0, 0, 0, dbkey); }
        public static Result_Define.eResult AddUserCombatPoint(ref TxnBlock TB, long AID, int addValue, string dbkey = Account_Define.AccountShardingDB)            { return AddUserValue(ref TB, AID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, addValue, 0, 0, dbkey); }
        public static Result_Define.eResult AddUserPartyDungeonPoint(ref TxnBlock TB, long AID, int addValue, string dbkey = Account_Define.AccountShardingDB)      { return AddUserValue(ref TB, AID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, addValue, 0, dbkey); }
        public static Result_Define.eResult AddUserBlackMarketPoint(ref TxnBlock TB, long AID, int addValue, string dbkey = Account_Define.AccountShardingDB)       { return AddUserValue(ref TB, AID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, addValue, dbkey); }
        public static Result_Define.eResult AddUserGold_And_Ruby(ref TxnBlock TB, long AID, int addGold, int addRuby, string dbkey = Account_Define.AccountShardingDB)
        { return AddUserValue(ref TB, AID, addGold, 0, addRuby, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, dbkey); }

        private static Result_Define.eResult AddUserValue(ref TxnBlock TB, long AID, int addGold, int addCash, int addEventCash, int addKey, int addTicket, int addHonor, int addFriendPoint, int addSoulStone, int addExpeditionPoint, int addOverlordPoint, int addCombatPoint, int addPartyPoint, int addBlackMarketPoint, string dbkey = Account_Define.AccountShardingDB, bool Flush = false)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            Account UserInfo = FlushAccountData(ref TB, AID, ref retError);

            if (retError != Result_Define.eResult.SUCCESS)
                return retError;

            SqlCommand cmdUserValue = new SqlCommand();
            cmdUserValue.CommandText = "System_addUserValue";
            var retResult = new SqlParameter("@retResult", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmdUserValue.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            cmdUserValue.Parameters.Add("@AddGold", SqlDbType.Int).Value = addGold;
            cmdUserValue.Parameters.Add("@AddCash", SqlDbType.Int).Value = addCash;
            cmdUserValue.Parameters.Add("@AddEventCash", SqlDbType.Int).Value = addEventCash;
            cmdUserValue.Parameters.Add("@AddKey", SqlDbType.Int).Value = addKey;
            cmdUserValue.Parameters.Add("@AddTicket", SqlDbType.Int).Value = addTicket;
            cmdUserValue.Parameters.Add("@AddHonor", SqlDbType.Int).Value = addHonor;
            cmdUserValue.Parameters.Add("@AddFriendPoint", SqlDbType.Int).Value = addFriendPoint;
            cmdUserValue.Parameters.Add("@AddSoulStone", SqlDbType.Int).Value = addSoulStone;
            cmdUserValue.Parameters.Add("@AddExpeditionPoint", SqlDbType.Int).Value = addExpeditionPoint;
            cmdUserValue.Parameters.Add("@AddOverlordPoint", SqlDbType.Int).Value = addOverlordPoint;
            cmdUserValue.Parameters.Add("@AddCombatPoint", SqlDbType.Int).Value = addCombatPoint;
            cmdUserValue.Parameters.Add("@AddPartyDungeonPoint", SqlDbType.Int).Value = addPartyPoint;
            cmdUserValue.Parameters.Add("@AddBlackMarketPoint", SqlDbType.Int).Value = addBlackMarketPoint;
            cmdUserValue.Parameters.Add(retResult);
            TB.ExcuteSqlStoredProcedure(dbkey, ref cmdUserValue);

            int result = System.Convert.ToInt32(retResult.Value.ToString());
            cmdUserValue.Dispose();

            if (result != 0)
                retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            string setMoneyLogJson = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.money_log_list]);

            if (addGold > 0 && retError == Result_Define.eResult.SUCCESS)
            {
                // snail : moneylog
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_money_log]);
                MoneyLogInfo setLog = new MoneyLogInfo((int)SnailLog_Define.Snail_Money_type.gold, (int)SnailLog_Define.Snail_Money_Event_type.add, addGold, UserInfo.Gold, UserInfo.Gold + addGold);
                setMoneyLogJson = mJsonSerializer.AddJsonArray(setMoneyLogJson, mJsonSerializer.ToJsonString(setLog));

                retError =  TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.Gold, 0, 0, addGold);
            }

            if ((addCash + addEventCash) > 0 && retError == Result_Define.eResult.SUCCESS)
            {
                // snail : moneylog                
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_money_log]);
                MoneyLogInfo setLog = new MoneyLogInfo((int)SnailLog_Define.Snail_Money_type.ruby, (int)SnailLog_Define.Snail_Money_Event_type.add, (addCash + addEventCash), (UserInfo.Cash + UserInfo.EventCash), (UserInfo.Cash + UserInfo.EventCash) + (addCash + addEventCash));
                setMoneyLogJson = mJsonSerializer.AddJsonArray(setMoneyLogJson, mJsonSerializer.ToJsonString(setLog));
            }

            TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.money_log_list], setMoneyLogJson);

            return Result_Define.eResult.SUCCESS;
        }

        public static Result_Define.eResult UseUserGold(ref TxnBlock TB, long AID, int useValue, string dbkey = Account_Define.AccountShardingDB)                           { return UseUserValue(ref TB, AID, useValue, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult UseUserCash(ref TxnBlock TB, long AID, int useValue, string dbkey = Account_Define.AccountShardingDB)                           { return UseUserValue(ref TB, AID, 0, useValue, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult UseUserKey(ref TxnBlock TB, long AID, int useValue, string dbkey = Account_Define.AccountShardingDB)                            { return UseUserValue(ref TB, AID, 0, 0, useValue, 0, 0, 0, 0, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult UseUserTicket(ref TxnBlock TB, long AID, int useValue, string dbkey = Account_Define.AccountShardingDB)                         { return UseUserValue(ref TB, AID, 0, 0, 0, useValue, 0, 0, 0, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult UseUserHonor(ref TxnBlock TB, long AID, int useValue, string dbkey = Account_Define.AccountShardingDB)                          { return UseUserValue(ref TB, AID, 0, 0, 0, 0, useValue, 0, 0, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult UseUserFriendlyPoint(ref TxnBlock TB, long AID, int useValue, string dbkey = Account_Define.AccountShardingDB)                  { return UseUserValue(ref TB, AID, 0, 0, 0, 0, 0, useValue, 0, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult UseUserSoulStone(ref TxnBlock TB, long AID, int useValue, string dbkey = Account_Define.AccountShardingDB)                      { return UseUserValue(ref TB, AID, 0, 0, 0, 0, 0, 0, useValue, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult UseUserExpeditionPoint(ref TxnBlock TB, long AID, int useValue, string dbkey = Account_Define.AccountShardingDB)                { return UseUserValue(ref TB, AID, 0, 0, 0, 0, 0, 0, 0, useValue, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult UseUserOverlordRankingPoint(ref TxnBlock TB, long AID, int useValue, string dbkey = Account_Define.AccountShardingDB)           { return UseUserValue(ref TB, AID, 0, 0, 0, 0, 0, 0, 0, 0, useValue, 0, 0, 0, dbkey); }
        public static Result_Define.eResult UseUserCombatPoint(ref TxnBlock TB, long AID, int useValue, string dbkey = Account_Define.AccountShardingDB)                    { return UseUserValue(ref TB, AID, 0, 0, 0, 0, 0, 0, 0, 0, 0, useValue, 0, 0, dbkey); }
        public static Result_Define.eResult UseUserPartyDungeonPoint(ref TxnBlock TB, long AID, int useValue, string dbkey = Account_Define.AccountShardingDB)              { return UseUserValue(ref TB, AID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, useValue, 0, dbkey); }
        public static Result_Define.eResult UseUserBlackMarketPoint(ref TxnBlock TB, long AID, int useValue, string dbkey = Account_Define.AccountShardingDB)               { return UseUserValue(ref TB, AID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, useValue, dbkey); }

        public static Result_Define.eResult UseUserGold_And_Ruby(ref TxnBlock TB, long AID, int useGold, int useRuby, string dbkey = Account_Define.AccountShardingDB)
        { return UseUserValue(ref TB, AID, useGold, useRuby, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, dbkey); }
        public static Result_Define.eResult UseUserGold_And_Key(ref TxnBlock TB, long AID, int useGold, int useKey, string dbkey = Account_Define.AccountShardingDB)
        { return UseUserValue(ref TB, AID, useGold, 0, useKey, 0, 0, 0, 0, 0, 0, 0, 0, 0, dbkey); }

        private static Result_Define.eResult UseUserValue(ref TxnBlock TB, long AID, int useGold, int useCash, int useKey, int useTicket, int useHonor, int useFriendPoint, int useSoulStone, int useExpeditionPoint, int useOverlordPoint, int useCombatPoint, int usePartyPoint, int useBlackMarketPoint, string dbkey = Account_Define.AccountShardingDB, bool Flush = false)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            int key = 0; int keyfillmax = 0; int keyremain = 0;

            Account UserInfo = GetAccountData(ref TB, AID, ref retError);
            if (retError != Result_Define.eResult.SUCCESS)
                return retError;

            if (useKey > 0)
            {
                KeySpendCharge(ref TB, AID, 0, ref key, ref keyfillmax, ref keyremain);
                UserInfo.Key = key;
            }

            if (useTicket > 0)
            {
                TicketSpendCharge(ref TB, AID, 0, ref key, ref keyfillmax, ref keyremain);
                UserInfo.Ticket = key;
            }

            if (UserInfo.Gold < useGold)
                return Result_Define.eResult.NOT_ENOUGH_GOLD;
            else if((UserInfo.Cash + UserInfo.EventCash) < useCash)
                return Result_Define.eResult.NOT_ENOUGH_CASH;
            else if(UserInfo.Key < useKey)
                return Result_Define.eResult.NOT_ENOUGH_KEY;
            else if(UserInfo.Ticket < useTicket)
                return Result_Define.eResult.NOT_ENOUGH_TICKET;
            else if(UserInfo.Honorpoint < useHonor)
                return Result_Define.eResult.NOT_ENOUGH_HONOR;
            else if (UserInfo.FriendlyPoint < useFriendPoint)
                return Result_Define.eResult.NOT_ENOUGH_FRIENDLYPOINT;
            else if (UserInfo.Stone < useSoulStone)
                return Result_Define.eResult.SOUL_NOT_ENOUGH_PASSIVE_STONE;
            else if (UserInfo.OverlordPoint < useOverlordPoint)
                return Result_Define.eResult.SHOP_NOT_ENOUGH_OVERLORD_POINT;
            else if (UserInfo.ExpeditionPoint < useExpeditionPoint)
                return Result_Define.eResult.SHOP_NOT_ENOUGH_GE_POINT;
            else if (UserInfo.BlackMarketPoint < useBlackMarketPoint)
                return Result_Define.eResult.SHOP_NOT_ENOUGH_BLACKMARKET_POINT;

            SqlCommand cmdUserValue= new SqlCommand();
            cmdUserValue.CommandText = "System_UseUserValue";
            var retResult = new SqlParameter("@retResult", SqlDbType.Int) { Direction = ParameterDirection.Output };
            cmdUserValue.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            cmdUserValue.Parameters.Add("@UseGold", SqlDbType.Int).Value = useGold;
            cmdUserValue.Parameters.Add("@UseCash", SqlDbType.Int).Value = useCash;
            cmdUserValue.Parameters.Add("@UseKey", SqlDbType.Int).Value = useKey;
            cmdUserValue.Parameters.Add("@UseTicket", SqlDbType.Int).Value = useTicket;
            cmdUserValue.Parameters.Add("@UseHonor", SqlDbType.Int).Value = useHonor;
            cmdUserValue.Parameters.Add("@UseFriendPoint", SqlDbType.Int).Value = useFriendPoint;
            cmdUserValue.Parameters.Add("@UseSoulStone", SqlDbType.Int).Value = useSoulStone;
            cmdUserValue.Parameters.Add("@UseExpeditionPoint", SqlDbType.Int).Value = useExpeditionPoint;
            cmdUserValue.Parameters.Add("@UseOverlordPoint", SqlDbType.Int).Value = useOverlordPoint;
            cmdUserValue.Parameters.Add("@UseCombatPoint", SqlDbType.Int).Value = useCombatPoint;
            cmdUserValue.Parameters.Add("@UsePartyPoint", SqlDbType.Int).Value = usePartyPoint;
            cmdUserValue.Parameters.Add("@UseBlackMarketPoint", SqlDbType.Int).Value = useBlackMarketPoint;
            cmdUserValue.Parameters.Add(retResult);
            TB.ExcuteSqlStoredProcedure(dbkey, ref cmdUserValue);
            
            // ChallengeTicket calc : not use yet
            //if(true)
            //    ChallengeTicketSpendCharge(ref TB, AID, useTicket, ref key, ref keyfillmax, ref keyremain);

            if (retResult.Value == null)
            {
                cmdUserValue.Dispose();
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }

            int result = System.Convert.ToInt32(retResult.Value.ToString());
            cmdUserValue.Dispose();
            if (result != 0)
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            string setMoneyLogJson = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.money_log_list]);
            if (useGold > 0 && retError == Result_Define.eResult.SUCCESS)
            {
                // snail : moneylog                
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_money_log]);
                MoneyLogInfo setLog = new MoneyLogInfo((int)SnailLog_Define.Snail_Money_type.gold, (int)SnailLog_Define.Snail_Money_Event_type.use, useGold, UserInfo.Gold, UserInfo.Gold - useGold);
                setMoneyLogJson = mJsonSerializer.AddJsonArray(setMoneyLogJson, mJsonSerializer.ToJsonString(setLog));
            }

            List<TriggerProgressData> setDataList = new List<TriggerProgressData>();

            if (useCash > 0 && retError == Result_Define.eResult.SUCCESS)
            {
                // snail : moneylog                
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_money_log]);
                MoneyLogInfo setLog = new MoneyLogInfo((int)SnailLog_Define.Snail_Money_type.ruby, (int)SnailLog_Define.Snail_Money_Event_type.use, useCash, UserInfo.Cash + UserInfo.EventCash, (UserInfo.Cash + UserInfo.EventCash) - useCash);
                setMoneyLogJson = mJsonSerializer.AddJsonArray(setMoneyLogJson, mJsonSerializer.ToJsonString(setLog));

                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Ruby_Use, (int)Trigger_Define.eRubyUseType.Single, 0, useCash));
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Ruby_Use, (int)Trigger_Define.eRubyUseType.Accumulate, 0, useCash));
            }

            TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.money_log_list], setMoneyLogJson);
            
            if (useKey > 0 && retError == Result_Define.eResult.SUCCESS)
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Energy_Use, (int)Trigger_Define.eEnergyUseType.Key, 0, useKey));
            if (useTicket > 0 && retError == Result_Define.eResult.SUCCESS)
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Energy_Use, (int)Trigger_Define.eEnergyUseType.Ticket, 0, useTicket));

            if(retError == Result_Define.eResult.SUCCESS)
                retError = TriggerManager.ProgressTrigger(ref TB, AID, setDataList);

            return retError;
        }

        public static Result_Define.eResult AddUserPoint(ref TxnBlock TB, long AID, int AddPoint, Item_Define.eItemBuyPriceType PriceTYpe)
        {
            // PriceType_PayExpeditionPoint, PriceType_PayGachaCoin, PriceType_PayGuildPoint, PriceType_PayMedal
            // Honorpoint, ContributionPoint, PartyDungeonPoint, CombatPoint, Gold, Cash, RealPay
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            Account userInfo = AccountManager.GetAccountData(ref TB, AID, ref retError);
            if (retError != Result_Define.eResult.SUCCESS)
                return retError;

            switch (PriceTYpe)
            {
                case Item_Define.eItemBuyPriceType.PriceType_PayCash:
                    return AccountManager.AddUserCash(ref TB, AID, AddPoint);

                case Item_Define.eItemBuyPriceType.PriceType_PayGold:
                    return AccountManager.AddUserGold(ref TB, AID, AddPoint);

                case Item_Define.eItemBuyPriceType.PriceType_Key:
                    return AccountManager.AddUserKey(ref TB, AID, AddPoint);

                case Item_Define.eItemBuyPriceType.PriceType_Ticket:
                    return AccountManager.AddUserTicket(ref TB, AID, AddPoint);

                case Item_Define.eItemBuyPriceType.PriceType_HonorPoint:
                    return AccountManager.AddUserHonor(ref TB, AID, AddPoint);

                case Item_Define.eItemBuyPriceType.PriceType_PartyDungeonPoint:
                    return AccountManager.AddUserPartyDungeonPoint(ref TB, AID, AddPoint)
                        ;
                case Item_Define.eItemBuyPriceType.PriceType_CombatPoint:
                    return AccountManager.AddUserCombatPoint(ref TB, AID, AddPoint);

                case Item_Define.eItemBuyPriceType.PriceType_PayExpeditionPoint:
                    return AccountManager.AddUserExpeditionPoint(ref TB, AID, AddPoint);

                case Item_Define.eItemBuyPriceType.PriceType_RankingPoint:
                    return AccountManager.AddUserOverlordRankingPoint(ref TB, AID, AddPoint);

                case Item_Define.eItemBuyPriceType.PriceType_PayGuildPoint:
                    return GuildManager.AddGuildContributionPoint(ref TB, AID, AddPoint);

                case Item_Define.eItemBuyPriceType.PriceType_BlackMarketPoint:
                    return AccountManager.AddUserBlackMarketPoint(ref TB, AID, AddPoint);

                case Item_Define.eItemBuyPriceType.PriceType_PayGachaCoin:
                case Item_Define.eItemBuyPriceType.PriceType_PayMedal:
                default:
                    return Result_Define.eResult.SUCCESS;
            }
        }

        public static Result_Define.eResult UseUserPoint(ref TxnBlock TB, long AID, int usePoint, Item_Define.eItemBuyPriceType PriceTYpe)
        {            
            // PriceType_PayExpeditionPoint, PriceType_PayGachaCoin, PriceType_PayGuildPoint, PriceType_PayMedal
            // Honorpoint, ContributionPoint, PartyDungeonPoint, CombatPoint, Gold, Cash, RealPay
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            Account userInfo = AccountManager.GetAccountData(ref TB, AID, ref retError);
            if (retError != Result_Define.eResult.SUCCESS)
                return retError;

            switch (PriceTYpe)
            {
                case Item_Define.eItemBuyPriceType.PriceType_PayCash:
                    if (userInfo.Cash + userInfo.EventCash < usePoint)
                        return Result_Define.eResult.NOT_ENOUGH_CASH;
                    return AccountManager.UseUserCash(ref TB, AID, usePoint);

                case Item_Define.eItemBuyPriceType.PriceType_PayGold:
                    if (userInfo.Gold < usePoint)
                        return Result_Define.eResult.NOT_ENOUGH_GOLD;
                    return AccountManager.UseUserGold(ref TB, AID, usePoint);

                case Item_Define.eItemBuyPriceType.PriceType_Key:
                    if (userInfo.Key < usePoint)
                        return Result_Define.eResult.NOT_ENOUGH_KEY;
                    return AccountManager.UseUserKey(ref TB, AID, usePoint);

                case Item_Define.eItemBuyPriceType.PriceType_Ticket:
                    if (userInfo.Ticket < usePoint)
                        return Result_Define.eResult.NOT_ENOUGH_TICKET;
                    return AccountManager.UseUserTicket(ref TB, AID, usePoint);

                case Item_Define.eItemBuyPriceType.PriceType_HonorPoint:
                    if (userInfo.Honorpoint < usePoint)
                        return Result_Define.eResult.NOT_ENOUGH_HONOR;
                    return AccountManager.UseUserHonor(ref TB, AID, usePoint);

                case Item_Define.eItemBuyPriceType.PriceType_PartyDungeonPoint:
                    if (userInfo.PartyDungeonPoint < usePoint)
                        return Result_Define.eResult.SHOP_NOT_ENOUGH_PARTY_DUNGEON_POINT;
                    return AccountManager.UseUserPartyDungeonPoint(ref TB, AID, usePoint);

                case Item_Define.eItemBuyPriceType.PriceType_CombatPoint:
                    if (userInfo.CombatPoint < usePoint)
                        return Result_Define.eResult.SHOP_NOT_ENOUGH_COMBAT_POINT;
                    return AccountManager.UseUserCombatPoint(ref TB, AID, usePoint);

                case Item_Define.eItemBuyPriceType.PriceType_PayExpeditionPoint:
                    if (userInfo.ExpeditionPoint < usePoint)
                        return Result_Define.eResult.SHOP_NOT_ENOUGH_GE_POINT;
                    return AccountManager.UseUserExpeditionPoint(ref TB, AID, usePoint);

                case Item_Define.eItemBuyPriceType.PriceType_RankingPoint:
                    if (userInfo.OverlordPoint < usePoint)
                        return Result_Define.eResult.SHOP_NOT_ENOUGH_OVERLORD_POINT;
                    return AccountManager.UseUserOverlordRankingPoint(ref TB, AID, usePoint);

                case Item_Define.eItemBuyPriceType.PriceType_PayGuildPoint:
                    if (userInfo.OverlordPoint < usePoint)
                        return Result_Define.eResult.NOT_ENOUGH_CONTIRIBUTION_POINT;                    
                    return GuildManager.UseGuildContributionPoint(ref TB, AID, usePoint);

                case Item_Define.eItemBuyPriceType.PriceType_BlackMarketPoint:
                    if (userInfo.BlackMarketPoint < usePoint)
                        return Result_Define.eResult.SHOP_NOT_ENOUGH_BLACKMARKET_POINT;
                    return AccountManager.UseUserBlackMarketPoint(ref TB, AID, usePoint);

                case Item_Define.eItemBuyPriceType.PriceType_PayGachaCoin:
                case Item_Define.eItemBuyPriceType.PriceType_PayMedal:
                default:
                    return Result_Define.eResult.SUCCESS;
            }
        }

        public static Account GetAccountData(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Account_Define.AccountShardingDB)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            return GetAccountData(ref TB, AID, ref retError, Flush, dbkey);
        }

        public static Account GetAccountData(ref TxnBlock TB, long AID, ref Result_Define.eResult retError, bool Flush = false, string dbkey = Account_Define.AccountShardingDB)
        {
            string setKey = string.Format("{0}_{1}_{2}", Account_Define.Account_Prefix, Account_Define.AccountDBTableName, AID);
            string setQuery = string.Format(@"  SELECT
                                                        AID, SNO, UserID, UserName, Cash,
                                                        EventCash, Gold, CreationDate, DATEDIFF(SS,'1970-01-01',LastConnTime) as LastConnTime, PVEPlayState, 
                                                        CharSlot, [Key], Ticket, KeyLastChargeTime, KeyFillMaxEA,
                                                        TicketLastChargeTime, TicketFillMaxEA, RewardBuff1, BuffEndTime1, RewardBuff2,
                                                        BuffEndTime2, LV, equipclass, UpdateTime, OldTime,
                                                        RecommendReward, RecommendCNT, Tutorial, VirginBossRaid, PCEXPBuffEndTime,
                                                        SoulEXPBuffEndTime, InvenSlot, InvenSlotCNT, SoulInvenSlot, SoulInvenSlotCNT,
                                                        TreasureInvenSlot, TreasureInvenSlotCNT, Flatform, CountryCode, LastWorldID,
                                                        LastStageID, OS, EquipCID, DailyKey, DailyTicket, DailyRuby, 
                                                        LanguageCode, GuildID, PCEXPBuffEndTime2, SoulEXPBuffEndTime2, friendscount,
                                                        friendswait, FNO, FriendlyPoint, Medal, ChallengeTicket,
                                                        ChallengeTicketLastChargeTime, ChallengeTicketFillMaxEA, Stone, LuckyBoxCount, ItemFreeGachaCoolTime,
                                                        SoulFreeGachaCoolTime, LuckyBoxBonus, PassiveSoulPoint, PvPFriendFlag, Honorpoint, 
                                                        ContributionPoint, PartyDungeonPoint, CombatPoint, OverlordPoint, ExpeditionPoint, BlackMarketPoint
                                                FROM {0} WITH(NOLOCK)
                                                        WHERE AID = {1}
                                                ", Account_Define.AccountDBTableName, AID);
            Account retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<Account>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);

            if (retObj == null)
                retObj = new Account();

            bool bDataValidate = false;

            if (retObj.AID != AID)
            {
                retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<Account>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, true);
                if (retObj != null)
                {
                    Flush = true;
                    if (retObj.AID == AID)
                        bDataValidate = true;
                }
            }
            else
            {
                bDataValidate = true;
            }

            if (bDataValidate)
            {
                if(Flush)
                    retObj.SetGuildInfo(TheSoul.DataManager.GuildManager.GetGuildInfo(ref TB, AID));
                TheSoul.DataManager.RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, retObj);

                if (retObj.SNO == 0)
                {
                    user_account_config userConfig = GlobalManager.GetUserAccountConfig(ref TB, AID);
                    user_platform_id userPlatformInfo = GlobalManager.GetUserPlatformInfo_ByPlatformID(ref TB, userConfig.platform_user_id);
                    if (userConfig.platform_type == (int)Global_Define.ePlatformType.EPlatformType_SnailSDK)
                    {
                        retObj.SNO = userPlatformInfo.platform_idx;
                        retObj.UserID = userPlatformInfo.platform_user_id;
                        retError = AccountManager.UpdateSNO(ref TB, AID, userPlatformInfo.platform_idx, userPlatformInfo.platform_user_id);
                    }
                }
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_account], retObj.SNO);
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_create_acc], retObj.UserID);
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_guild_id], retObj.GuildID);
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_role], retObj.UserName);
                retError = Result_Define.eResult.SUCCESS;
            }
            else
            {
                retError = Result_Define.eResult.ACCOUNT_ID_NOT_FOUND;
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                if (retObj.Gold < 0)
                    retError = Result_Define.eResult.NOT_ENOUGH_GOLD;
                else if ((retObj.Cash + retObj.EventCash) < 0)
                    retError = Result_Define.eResult.NOT_ENOUGH_CASH;
                else if (retObj.Key < 0)
                    retError = Result_Define.eResult.NOT_ENOUGH_KEY;
                else if (retObj.Ticket < 0)
                    retError = Result_Define.eResult.NOT_ENOUGH_TICKET;
                else if (retObj.Honorpoint < 0)
                    retError = Result_Define.eResult.NOT_ENOUGH_HONOR;
                else if (retObj.FriendlyPoint < 0)
                    retError = Result_Define.eResult.NOT_ENOUGH_FRIENDLYPOINT;
                else if (retObj.Stone < 0)
                    retError = Result_Define.eResult.SOUL_NOT_ENOUGH_PASSIVE_STONE;
                else if (retObj.OverlordPoint < 0)
                    retError = Result_Define.eResult.SHOP_NOT_ENOUGH_OVERLORD_POINT;
                else if (retObj.ExpeditionPoint < 0)
                    retError = Result_Define.eResult.SHOP_NOT_ENOUGH_GE_POINT;
                else if (retObj.BlackMarketPoint < 0)
                    retError = Result_Define.eResult.SHOP_NOT_ENOUGH_BLACKMARKET_POINT;
            }

            return retObj;
            //return FetchFromRedis(ref TB, AID);
        }

        public static Account FlushAccountData(ref TxnBlock TB, long AID, ref Result_Define.eResult retError)
        {
            return GetAccountData(ref TB, AID, ref retError, true);
            //return FetchFromRedis(ref TB, AID);
        }
        
        public const string Ret_EncryptKey = "updatetime";

        public static EncryptKey UpdateEncryptKey(ref TxnBlock TB, long AID, string dbkey = Account_Define.AccountShardingDB, bool Flush = true)
        {
            SqlCommand commandUpdateCheck = new SqlCommand();
            commandUpdateCheck.CommandText = "UpdateCheck";
            var outputUpdateTime = new SqlParameter("@RESULTUPDATETIME", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandUpdateCheck.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            commandUpdateCheck.Parameters.Add(outputUpdateTime);
            TB.ExcuteSqlStoredProcedure(dbkey, ref commandUpdateCheck);

            uint retUpdateTime = System.Convert.ToUInt32(outputUpdateTime.Value.ToString());

            EncryptKey updateKey = new EncryptKey();
            updateKey.UpdateTime = retUpdateTime;
            commandUpdateCheck.Dispose();
            return updateKey;
        }


        public static GetAccountDB GetShardingInfo(ref TxnBlock TB, long AID, string dbkey = Account_Define.AccountCommonDB, bool Flush = false)
        {
            GetAccountDB ShardingInfo = new GetAccountDB();
            ShardingInfo.RetDB_INDEX = 1;
            return ShardingInfo;

            // not use DB sharding yet
            //string setKey = string.Format("{0}_{1}", Account_Define.ShardingInfoPrefix, AID);
            //GetAccountDB ShardingInfo = null;
            //if (!Flush)
            //    ShardingInfo = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis<GetAccountDB>(DataManager_Define.RedisServerAlias_System, setKey);

            //if (ShardingInfo == null)
            //    Flush = true;
            //else if (ShardingInfo.RetDB_INDEX < 1)
            //    Flush = true;

            //if (Flush)
            //{
            //    SqlCommand Cmd = new SqlCommand();
            //    Cmd.CommandText = "GetAccountDB";

            //    //var indexcommand = new SqlCommand { CommandType = CommandType.StoredProcedure, Connection = DB_common, CommandText = "GetAccountDB" };
            //    var outputDB_INDEX = new SqlParameter("@RetDB_INDEX", SqlDbType.Int) { Direction = ParameterDirection.Output };
            //    var outputRetUserName = new SqlParameter("@RetUserName", SqlDbType.NVarChar, 32) { Direction = ParameterDirection.Output };
            //    var outputResultAccount = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            //    Cmd.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            //    Cmd.Parameters.Add(outputDB_INDEX);
            //    Cmd.Parameters.Add(outputRetUserName);
            //    Cmd.Parameters.Add(outputResultAccount);

            //    TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd);
            //    ShardingInfo = new GetAccountDB();
            //    ShardingInfo.RetDB_INDEX = long.Parse(outputDB_INDEX.Value.ToString());
            //    ShardingInfo.RetUserName = outputRetUserName.Value.ToString();
            //    ShardingInfo.Result = long.Parse(outputResultAccount.Value.ToString());
            //    Cmd.Dispose();
            //    TheSoul.DataManager.GenericFetch.SetToRedis(DataManager_Define.RedisServerAlias_System, setKey, ShardingInfo);
            //}

            //return ShardingInfo;
        }

        public static PvPInfo GetPvPInfo(ref TxnBlock TB, long AID, string dbkey = Account_Define.AccountShardingDB)
        {
            string setKey = string.Format("{0}_{1}", Account_Define.PvPInfo_Prefix, AID);
            PvPInfo getPvPInfo = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis<PvPInfo>(DataManager_Define.RedisServerAlias_User, setKey);

            if (getPvPInfo == null)
            {
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "GetPvPInfo";
                Cmd.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
                SqlDataReader getDr = null;

                TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd, ref getDr, false);

                if (getDr != null)
                {
                    var r = SQLtoJson.Serialize(ref getDr);
                    string json = mJsonSerializer.ToJsonString(r);

                    getDr.Dispose(); getDr.Close();
                    PvPInfo[] retList = mJsonSerializer.JsonToObject<PvPInfo[]>(json);

                    if (retList.Length > 0)
                        getPvPInfo = retList[0];
                    else
                        getPvPInfo = new PvPInfo();
                }
                else
                    getPvPInfo = new PvPInfo();
                Cmd.Dispose();
                TheSoul.DataManager.GenericFetch.SetToRedis(DataManager_Define.RedisServerAlias_User, setKey, getPvPInfo);
            }


            return getPvPInfo;
        }

        public static Result_Define.eResult CreateAccountCommon(ref TxnBlock TB, long AID, string userID, string userNickName, int shard_dbcnt = 1, string dbkey = Account_Define.AccountCommonDB, bool Flush = false)
        {
            SqlCommand commandCreateAccount = new SqlCommand();
            commandCreateAccount.CommandText = "System_CreateAccount";
            var result = new SqlParameter("@Result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandCreateAccount.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            commandCreateAccount.Parameters.Add("@PlatformID", SqlDbType.NVarChar, 128).Value = userID;
            commandCreateAccount.Parameters.Add("@NickName", SqlDbType.NVarChar, 32).Value = userNickName;
            commandCreateAccount.Parameters.Add("@shard_dbcnt", SqlDbType.Int).Value = shard_dbcnt;
            commandCreateAccount.Parameters.Add(result);

            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandCreateAccount))
            {
                if (System.Convert.ToInt32(result.Value) < 0)
                {
                    commandCreateAccount.Dispose();
                    int dbresult = System.Convert.ToInt32(result.Value) * -1;       // re delcare result code 
                    if ((Result_Define.eResult)dbresult == Result_Define.eResult.ACCOUNT_ID_ALREAD_CREATED)
                        return Result_Define.eResult.ACCOUNT_ID_ALREAD_CREATED;
                    else if ((Result_Define.eResult)dbresult == Result_Define.eResult.ACCOUNT_NICKNAME_DUPLICATED)
                        return Result_Define.eResult.ACCOUNT_NICKNAME_DUPLICATED;
                    else
                        return Result_Define.eResult.ACCOUNT_ID_CRETE_FAIL;
                }
                else
                {
                    commandCreateAccount.Dispose();
                    return Result_Define.eResult.SUCCESS;
                }
            }
            else
            {
                commandCreateAccount.Dispose();
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }
        }

        public static Result_Define.eResult RegistAccountGlobal(ref TxnBlock TB, long AID, string userNickName, long server_group_id, string dbkey = DataManager_Define.GlobalDB, bool Flush = false)
        {
            SqlCommand commandRegisterAccount = new SqlCommand();
            commandRegisterAccount.CommandText = "System_Reg_PlayServer";
            var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandRegisterAccount.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            commandRegisterAccount.Parameters.Add("@Nickname", SqlDbType.NVarChar, 32).Value = userNickName;
            commandRegisterAccount.Parameters.Add("@ServerGroupID", SqlDbType.BigInt).Value = server_group_id;
            commandRegisterAccount.Parameters.Add(result);

            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandRegisterAccount))
            {
                if (System.Convert.ToInt64(result.Value) < 0)
                {
                    commandRegisterAccount.Dispose();
                    long dbresult = System.Convert.ToInt64(result.Value) * -1;       // re delcare result code 
                    if ((Result_Define.eResult)dbresult == Result_Define.eResult.ACCOUNT_ID_GLOBAL_REGIST_ALREADY)
                        return Result_Define.eResult.ACCOUNT_ID_GLOBAL_REGIST_ALREADY;
                    else
                        return Result_Define.eResult.ACCOUNT_ID_GLOBAL_REGIST_FAIL;
                }
                else
                {
                    commandRegisterAccount.Dispose();
                    return Result_Define.eResult.SUCCESS;
                }
            }
            else
            {
                commandRegisterAccount.Dispose();
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }
        }

        public static Result_Define.eResult CreateAccountSharding(ref TxnBlock TB, long AID, string userNickName, string CountryCode, int LanguageCode, string dbkey = Account_Define.AccountShardingDB, bool Flush = false)
        {
            short SetKeyMax = 0;
            short SetTicketMax = 0;
            short SetKeyInc = 0;
            short SetTicketInc = 0;

            CharacterManager.CalcPlayEnerge(ref TB, ref SetKeyMax, ref SetTicketMax, ref SetKeyInc, ref SetTicketInc, true, 1, 0);

            //int VIPLevel = 0;
            //character info
            //System_VIP_Level vipinfo = TheSoul.DataManager.AccountManager.GetSystem_VIP_Level(ref TB, VIPLevel);
            int BAGSLOT_MAX_ITEM = TheSoul.DataManager.VipManager.User_Vip_Value(ref TB, AID, VIP_Define.Vip_Type_Key_List[VIP_Define.eVipType.BAGSLOT_MAX_ITEM]);

            //float DEF_SOUL_INVEN_DEFAULT_NUM = (float)TheSoul.DataManager.SystemData.GetConstValue(ref TB, "DEF_SOUL_INVEN_DEFAULT_NUM", "sharding", false);
            //float DEF_ENERGY_PVE_INIT_VALUE = (float)TheSoul.DataManager.SystemData.GetConstValue(ref TB, "DEF_ENERGY_PVE_INIT_VALUE", "sharding", false);
            //float DEF_ENERGY_PVP_INIT_VALUE = (float)TheSoul.DataManager.SystemData.GetConstValue(ref TB, "DEF_ENERGY_PVP_INIT_VALUE", "sharding", false);
            //float DEF_ENERGY_G3VS3_INIT_VALUE = (float)TheSoul.DataManager.SystemData.GetConstValue(ref TB, "DEF_ENERGY_G3VS3_INIT_VALUE", "sharding", false);
            System_PC_BASE PcBaseInfo = CharacterManager.GetPCbaseInfo(ref TB, 1);

            SqlCommand commandCreateAccount = new SqlCommand();
            commandCreateAccount.CommandText = "System_CreateAccount_sharding";
            var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandCreateAccount.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            commandCreateAccount.Parameters.Add("@UserName", SqlDbType.NVarChar).Value = userNickName;
            commandCreateAccount.Parameters.Add("@CountryCode", SqlDbType.Char, 2).Value = CountryCode;
            commandCreateAccount.Parameters.Add("@LanguageCode", SqlDbType.Int).Value = LanguageCode;
            commandCreateAccount.Parameters.Add("@ItemBag", SqlDbType.Int).Value = BAGSLOT_MAX_ITEM;
            commandCreateAccount.Parameters.Add("@SoulBag", SqlDbType.Int).Value = 0; // DEF_SOUL_INVEN_DEFAULT_NUM; // not use yet
            commandCreateAccount.Parameters.Add("@KeyInit", SqlDbType.Int).Value = SetKeyMax;
            commandCreateAccount.Parameters.Add("@TicketInit", SqlDbType.Int).Value = SetTicketMax;
            commandCreateAccount.Parameters.Add("@ChallengeTicketInit", SqlDbType.Int).Value = 0; // DEF_ENERGY_G3VS3_INIT_VALUE; // not use yet
            commandCreateAccount.Parameters.Add("@Base_Gold", SqlDbType.Int).Value = PcBaseInfo.Base_Gold;
            commandCreateAccount.Parameters.Add("@Base_Cash", SqlDbType.Int).Value = PcBaseInfo.Base_Cash;
            commandCreateAccount.Parameters.Add(result);

            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandCreateAccount))
            {
                if (System.Convert.ToInt32(result.Value) < 0)
                {
                    commandCreateAccount.Dispose();
                    int dbresult = System.Convert.ToInt32(result.Value) * -1;       // re delcare result code 
                    if ((Result_Define.eResult)dbresult == Result_Define.eResult.ACCOUNT_ID_ALREAD_CREATED)
                        return Result_Define.eResult.ACCOUNT_ID_ALREAD_CREATED;
                    else
                        return Result_Define.eResult.ACCOUNT_ID_CRETE_FAIL;
                }
                else
                {
                    commandCreateAccount.Dispose();
                    return Result_Define.eResult.SUCCESS;
                }
            }
            else
            {
                commandCreateAccount.Dispose();
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }
        }

        public static Result_Define.eResult KeySpendCharge(ref TxnBlock TB, long AID, int usePoint, ref int setKey, ref int setSec, ref int setTime, string dbkey = Account_Define.AccountShardingDB, bool Flush = false)
        {
            SqlCommand commandKeySpendCharge = new SqlCommand();
            commandKeySpendCharge.CommandText = "wsp_key_Spend_Charge";
            var retKey = new SqlParameter("@RESULTMYKEY", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var retSec = new SqlParameter("@RESULTREMAINCHARGESEC", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var retTime = new SqlParameter("@RESULTSERVERTIME", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandKeySpendCharge.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            commandKeySpendCharge.Parameters.Add("@SPENDPOINT", SqlDbType.Int).Value = usePoint;
            commandKeySpendCharge.Parameters.Add(retKey);
            commandKeySpendCharge.Parameters.Add(retSec);
            commandKeySpendCharge.Parameters.Add(retTime);

            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandKeySpendCharge))
            {
                commandKeySpendCharge.Dispose();
                setKey = System.Convert.ToInt32(retKey.Value);
                setSec = System.Convert.ToInt32(retSec.Value);
                setTime = System.Convert.ToInt32(retTime.Value);
                return Result_Define.eResult.SUCCESS;
            }
            else
            {
                commandKeySpendCharge.Dispose();
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }
        }


        public static Result_Define.eResult TicketSpendCharge(ref TxnBlock TB, long AID, int useTicket, ref int setTicket, ref int setSec, ref int setTime, string dbkey = Account_Define.AccountShardingDB, bool Flush = false)
        {
            SqlCommand commandKeySpendCharge = new SqlCommand();
            commandKeySpendCharge.CommandText = "wsp_ticket_Spend_Charge";
            var retKey = new SqlParameter("@RESULTMYTICKET", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var retSec = new SqlParameter("@RESULTREMAINCHARGESEC", SqlDbType.Int) { Direction = ParameterDirection.Output };
            var retTime = new SqlParameter("@RESULTSERVERTIME", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandKeySpendCharge.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            commandKeySpendCharge.Parameters.Add("@SPENDPOINT", SqlDbType.Int).Value = useTicket;
            commandKeySpendCharge.Parameters.Add(retKey);
            commandKeySpendCharge.Parameters.Add(retSec);
            commandKeySpendCharge.Parameters.Add(retTime);

            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandKeySpendCharge))
            {
                setTicket = System.Convert.ToInt32(retKey.Value);
                setSec = System.Convert.ToInt32(retSec.Value);
                setTime = System.Convert.ToInt32(retTime.Value);
                return Result_Define.eResult.SUCCESS;
            }
            else
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        // not use yet
        //public static Result_Define.eResult ChallengeTicketSpendCharge(ref TxnBlock TB, long AID, int useTicket, ref int setTicket, ref int setSec, ref int setTime, string dbkey = Account_Define.AccountShardingDB, bool Flush = false)
        //{
        //    SqlCommand commandKeySpendCharge = new SqlCommand();
        //    commandKeySpendCharge.CommandText = "wsp_challengeticket_Spend_Charge";
        //    var retKey = new SqlParameter("@RESULTMYCHALLENGETICKET", SqlDbType.Int) { Direction = ParameterDirection.Output };
        //    var retSec = new SqlParameter("@RESULTREMAINCHARGESEC", SqlDbType.Int) { Direction = ParameterDirection.Output };
        //    var retTime = new SqlParameter("@RESULTSERVERTIME", SqlDbType.Int) { Direction = ParameterDirection.Output };
        //    commandKeySpendCharge.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
        //    commandKeySpendCharge.Parameters.Add("@SPENDPOINT", SqlDbType.Int).Value = useTicket;
        //    commandKeySpendCharge.Parameters.Add(retKey);
        //    commandKeySpendCharge.Parameters.Add(retSec);
        //    commandKeySpendCharge.Parameters.Add(retTime);

        //    if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandKeySpendCharge))
        //    {
        //        setTicket = System.Convert.ToInt32(retKey.Value);
        //        setSec = System.Convert.ToInt32(retSec.Value);
        //        setTime = System.Convert.ToInt32(retTime.Value);
        //        return Result_Define.eResult.SUCCESS;
        //    }
        //    else
        //        return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        //}

        public static Result_Define.eResult CalcBuffEndTime(ref TxnBlock TB, ref Account userAccount, string dbkey = Account_Define.AccountShardingDB, bool Flush = false)
        {
            userAccount.BuffEndTime1 = 0;
            userAccount.BuffEndTime2 = 0;
            userAccount.PCEXPBuffEndTime = 0;
            userAccount.PCEXPBuffEndTime2 = 0;
            userAccount.SoulEXPBuffEndTime = 0;
            userAccount.SoulEXPBuffEndTime2 = 0;
            return Result_Define.eResult.SUCCESS;
        }

        public static Result_Define.eResult CalcGMEventBuffTime(ref TxnBlock TB, ref Ret_GM_Event GmEvent, string dbkey = Account_Define.AccountShardingDB, bool Flush = false)
        {
            GmEvent.GoldBoostRate = 0;
            GmEvent.PCEXPBoostRate = 0;

            return Result_Define.eResult.SUCCESS;
        }


        public static Result_Define.eResult CalcLimitBuy(ref TxnBlock TB, ref string view, string dbkey = Account_Define.AccountShardingDB, bool Flush = false)
        {
            view = "N";
            return Result_Define.eResult.SUCCESS;
        }


        public static Result_Define.eResult UpdatePVEFlag(ref TxnBlock TB, long AID, bool isEnter, int WorldID, int StageID, string dbkey = Account_Define.AccountShardingDB)
        {
            WorldID = WorldID < 1 ? 1 : WorldID;
            StageID = StageID < 1 ? 1 : StageID;
            string setQuery = string.Format("UPDATE {0} SET PVEPlayState = {2}, LastWorldID = {3}, LastStageID = {4} WHERE AID = {1} ", Account_Define.AccountDBTableName, AID, isEnter ? 1 : 0, WorldID, StageID);
            return (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult UpdatePVPFriendFlag(ref TxnBlock TB, long AID, bool isSet, string dbkey = Account_Define.AccountShardingDB)
        {
            string setQuery = string.Format("UPDATE {0} SET PvPFriendFlag = '{2}' WHERE AID = {1} ", Account_Define.AccountDBTableName, AID, isSet ? "Y" : "N");
            return (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult UpdateLastConnectionTime(ref TxnBlock TB, long AID, string dbkey = Account_Define.AccountShardingDB)
        {
            string setQuery = string.Format("UPDATE {0} SET LastConnTime = GETDATE() WHERE AID = {1} ", Account_Define.AccountDBTableName, AID);
            return (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult UpdateGuildID(ref TxnBlock TB, long AID, long guildID, string dbkey = Account_Define.AccountShardingDB)
        {
            string setQuery = string.Format("UPDATE {0} SET GuildID = {2} WHERE AID = {1} ", Account_Define.AccountDBTableName, AID, guildID);
            return (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        const int setFillMax = 1000;
        public static Ret_Login_Info SetRetLoginData(ref TxnBlock TB, ref Account userAccount, int charCount = 0)
        {
            Ret_Login_Info retAccount = new Ret_Login_Info();

            retAccount.aid = userAccount.AID;
            retAccount.accountname = userAccount.UserName;
            retAccount.cash = userAccount.Cash + userAccount.EventCash;
            retAccount.gold = userAccount.Gold;
            retAccount.stone = userAccount.Stone;
            retAccount.medal = userAccount.Medal;
            retAccount.characterslot = userAccount.CharSlot;
            retAccount.charactercount = charCount;
            int key = 0; int keyremainsec = 0; int keyservertime = 0;
            AccountManager.KeySpendCharge(ref TB, userAccount.AID, 0, ref key, ref keyremainsec, ref keyservertime);
            retAccount.key = key;
            retAccount.keyfillmax = userAccount.KeyFillMaxEA;
            retAccount.keyremainchargesec = keyremainsec;

            int ticket = 0; int ticketremainsec = 0;
            AccountManager.TicketSpendCharge(ref TB, userAccount.AID, 0, ref ticket, ref ticketremainsec, ref keyservertime);
            retAccount.ticket = ticket;
            retAccount.ticketfillmax = userAccount.TicketFillMaxEA;
            retAccount.ticketremainchargesec = ticketremainsec;

            retAccount.laststageid = System.Convert.ToInt32(userAccount.LastStageID);

            //int challengeticket = 0; int challengeremainsec = 0;
            //AccountManager.TicketSpendCharge(ref TB, userAccount.AID, 0, ref challengeticket, ref challengeremainsec, ref keyservertime);
            //retAccount.challengeticket = challengeticket;
            //retAccount.challengeticketfillmax = userAccount.ChallengeTicketFillMaxEA;
            //retAccount.challengeticketremainchargesec = challengeremainsec;

            retAccount.servertime = keyservertime;
            retAccount.honorpoint = userAccount.Honorpoint;
            retAccount.tutorial = userAccount.Tutorial;

            retAccount.countrycode = userAccount.CountryCode;

            // for PvP Friend
            retAccount.pvpfriendflag = userAccount.PvPFriendFlag;
    
            retAccount.vipinfo = new RetUserVIP(VipManager.GetUser_VIPInfo(ref TB, userAccount.AID));

            // for Guild
            retAccount.guild_id = userAccount.GuildID;
            retAccount.guildname = userAccount.GuildName;
            retAccount.guildstate = userAccount.GuildState;
            retAccount.guildattend = userAccount.GuildAttend;

            // for user shop point
            retAccount.honor_point = userAccount.Honorpoint;
            retAccount.overlord_point = userAccount.OverlordPoint;
            retAccount.contribution_point = userAccount.ContributionPoint;
            retAccount.partydungeon_point = userAccount.PartyDungeonPoint;
            retAccount.combat_point = userAccount.CombatPoint;

            retAccount.expedition_point = userAccount.ExpeditionPoint;
            retAccount.blackmarket_point = userAccount.BlackMarketPoint;

            return retAccount;
        }

        // account chat
        public static List<ChatIgnore> GetUserIgnoreList(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Account_Define.AccountShardingDB)
        {
            string setKey = string.Format("{0}_{1}_{2}", Account_Define.User_Chat_Ignore_TableName, AID, Account_Define.User_Chat_Ignore_Surfix);
            string setQuery = string.Format(@"SELECT A.ignore_aid AS aid, B.UserName AS username FROM {0} AS A WITH(NOLOCK) , {1} AS B WITH(NOLOCK) WHERE A.aid = {2} AND A.ignore_aid = B.AID", Account_Define.User_Chat_Ignore_TableName, Account_Define.AccountDBTableName, AID);

            List<ChatIgnore> retObj = GenericFetch.FetchFromRedis_MultipleRow_Hash<ChatIgnore>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, AID.ToString(), setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new List<ChatIgnore>();
        }

        public static Result_Define.eResult AddUserIgnoreList(ref TxnBlock TB, long AID, long IgnoreAID, string dbkey = Account_Define.AccountShardingDB)
        {
            string setQuery = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = {1} AND ignore_aid = {2}
                                                WHEN MATCHED THEN
                                                    UPDATE SET 
                                                        regdate = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                    INSERT (aid, ignore_aid, regdate) VALUES('{1}', '{2}', GETDATE());"
                                                        , Account_Define.User_Chat_Ignore_TableName, AID, IgnoreAID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult RemoveUserIgnoreList(ref TxnBlock TB, long AID, long IgnoreAID, string dbkey = Account_Define.AccountShardingDB)
        {
            string setQuery = string.Format(@"DELETE FROM {0} WHERE aid = {1} AND ignore_aid = {2};"
                                                        , Account_Define.User_Chat_Ignore_TableName, AID, IgnoreAID);

            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult UpdateEquipCID(ref TxnBlock TB, long AID, long CID, string dbkey = Account_Define.AccountShardingDB)
        {
            if (AID < 1|| CID < 1)
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            string setQuery = string.Format(@"UPDATE {0} SET EquipCID = {2} WHERE AID = {1} ", Account_Define.AccountDBTableName, AID, CID);
            return (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static System_Tutorial_Step GetSystemTutorial(ref TxnBlock TB, long StepID, bool Flush = false, string dbkey = Account_Define.AccountShardingDB)
        {
            string setKey = string.Format("{0}_{1}", Account_Define.System_Tutorial_Step_TableName, StepID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE Tutorial_Index = {1}", Account_Define.System_Tutorial_Step_TableName, StepID);

            System_Tutorial_Step retObj = GenericFetch.FetchFromRedis_Hash<System_Tutorial_Step>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, StepID.ToString(), setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new System_Tutorial_Step();
        }

        public static void RemoveLoginCountInfoCache(long AID)
        {
            string setKey = string.Format("{0}_{1}", Account_Define.User_LoginCount_TableName, AID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        private static User_Login_Count GetUserLoginCountInfo(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Account_Define.AccountShardingDB)
        {
            string setKey = string.Format("{0}_{1}", Account_Define.User_LoginCount_TableName, AID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE AID = {1}", Account_Define.User_LoginCount_TableName, AID);

            User_Login_Count retObj = GenericFetch.FetchFromRedis<User_Login_Count>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new User_Login_Count(AID);
        }

        public static Result_Define.eResult GetUserLoginCount(ref TxnBlock TB, long AID, out int LoginCount, bool bBackground = false, bool Flush = false, string dbkey = Account_Define.AccountShardingDB)
        {
            User_Login_Count getInfo = GetUserLoginCountInfo(ref TB, AID, Flush);

            DateTime CheckDate = DateTime.Parse(getInfo.regdate.ToShortDateString());
            DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());

            TimeSpan TS = dbDate - CheckDate;
            Result_Define.eResult reterror = Result_Define.eResult.SUCCESS;
            if (TS.Days != 0)
            {
                if (bBackground)
                    TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.renew_role_log]);

                getInfo.TotalLoginCount++;
                getInfo.regdate = DateTime.Now;
                reterror = SetUserLoginCount(ref TB, AID, getInfo.TotalLoginCount);
            }

            LoginCount = getInfo.TotalLoginCount;
            return reterror;
        }

        public static Result_Define.eResult SetUserLoginCount(ref TxnBlock TB, long AID, int loginCount, string dbkey = Trigger_Define.Trigger_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON AID = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                TotalLoginCount = TotalLoginCount + 1,
	                                                regdate = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ({1}, {2}, GETDATE());
                                    ", Account_Define.User_LoginCount_TableName
                                     , AID
                                     , loginCount
                                     );
            RemoveLoginCountInfoCache(AID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult UpdateSNO(ref TxnBlock TB, long AID, long SNO, string UserID, string dbkey = Account_Define.AccountShardingDB)
        {
            if (AID < 1 || SNO < 1)
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            string setQuery = string.Format(@"UPDATE {0} SET SNO = {2}, UserID = '{3}' WHERE AID = {1} ", Account_Define.AccountDBTableName, AID, SNO, UserID);
            return (TB.ExcuteSqlCommand(dbkey, setQuery)) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        private static string GetRediskey_UserWarPoint(long AID)
        {
            return string.Format("{0}_{1}_{2}", Account_Define.Account_WarpointInfo_Prefix, Account_Define.AccountDBTableName, AID);
        }

        public static User_WarPoint GetUserWarPoint(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Account_Define.AccountShardingDB)
        {
            string setKey = GetRediskey_UserWarPoint(AID);
            string setQuery = string.Format(@"SELECT AID, MAX(level) as CHARACTER_MAX_LEVEL, SUM(CONVERT(BIGINT, (WAR_POINT + ACTIVE_SOUL_WAR_POINT + PASSIVE_SOUL_WAR_POINT))) AS WAR_POINT FROM 
                                                    (SELECT B.aid as AID, B.level, A.WAR_POINT, A.ACTIVE_SOUL_WAR_POINT, A.PASSIVE_SOUL_WAR_POINT FROM {0} AS A WITH(NOLOCK) INNER JOIN {1} AS B WITH(NOLOCK)
                                                            ON A.CID = B.cid AND B.AID = {2}) AS CalcTable GROUP BY AID
                ", Character_Define.Character_Stat_TableName, Character_Define.CharacterTableName, AID);
            User_WarPoint retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<User_WarPoint>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
            return retObj == null ? new User_WarPoint() : retObj;
        }

        public static void RemoveUser_UserWarPoint(long AID)
        {
            string setKey = GetRediskey_UserWarPoint(AID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static RefreshNewFlag CheckUserNewFlag(ref TxnBlock TB, ref Account userInfo, int ChatChannel, ref Dictionary<long, Friends> reqFriendList)
        {
            long AID = userInfo.AID;
            RefreshNewFlag setUserRefresh = new RefreshNewFlag();
            // Mission reward
            setUserRefresh.mission = CheckMissionReward(ref TB, AID);
            // Achive reward
            setUserRefresh.achive = CheckAchiveReward(ref TB, AID);
            // Event reward (UserEvent, GiftEvent)
            byte bUserEventReward = 0;
            byte bGiftEventReward = 0;
            CheckEventReward(ref TB, AID, out bGiftEventReward, out bUserEventReward);
            setUserRefresh.giftevent = bGiftEventReward;
            setUserRefresh.userevent = bUserEventReward;

            // 7Day Event reward
            setUserRefresh.seven_day = Check7DayReward(ref TB, AID, userInfo.CreationDate);

            // Today Guild Attent
            setUserRefresh.guild = (byte)((userInfo.GuildAttend.Equals("N") && userInfo.GuildID > 0) ? 1 : 0);

            // guild pvp
            bool isBonus = false;
            setUserRefresh.pvp_guild = (byte)(PvPManager.CheckPvPOpenTime(ref TB, PvP_Define.ePvPType.MATCH_GUILD_3VS3, out isBonus) ? 1 : 0);

            // Mail Unread
            setUserRefresh.mail = CheckMailUnread(ref TB, AID);
            // New FriendReqeust 
            setUserRefresh.friend = CheckFriendRequest(ref TB, AID, ref reqFriendList);

            // shop new flag (by admin tool)
            setUserRefresh.shop = (byte)SystemData.AdminConstValueFetchFromRedis(ref TB, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.ADMIN_CONST_DEF_SHOP_NEW_ON_OFF]);
            setUserRefresh.black_market = CheckBlackMarket(ref TB, AID);

            // gacha shop new flag
            setUserRefresh.gacha_shop = CheckGachaShop(ref TB, AID);

            // Check Bossraid Activation
            byte bBossRaidActive = 0;
            bool bBossRaidReward = false;
            CheckBossRaidActive(ref TB, AID, ChatChannel, ref bBossRaidActive, ref bBossRaidReward);
            setUserRefresh.bossraid_active = bBossRaidActive;
            setUserRefresh.bossraid_reward = (byte)(bBossRaidReward ? 1 : 0);

            // shop new flag (by admin tool)
            setUserRefresh.coupon = (byte)SystemData.AdminConstValueFetchFromRedis(ref TB, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.ADMIN_CONST_DEF_COUPON_ON_OFF]);
            setUserRefresh.ios_coupon = (byte)SystemData.AdminConstValueFetchFromRedis(ref TB, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.ADMIN_CONST_DEF_COUPON_IOS_ON_OFF]);
            return setUserRefresh;
        }

        
        static byte CheckBlackMarket(ref TxnBlock TB, long AID)
        {
            return (byte)1;

            // legacy code : game design change -> black market always open for all user

            //// alway Shop open check (SHOP_OPEN_BLACKMARKET_2 is always up)
            //bool OpenPermission = VipManager.CheckVIPCountOver(ref TB, AID, 0, VIP_Define.eVipType.SHOP_OPEN_BLACKMARKET_2);
            //if (OpenPermission)
            //    return (byte) 1 ;

            //OpenPermission = VipManager.CheckVIPCountOver(ref TB, AID, 0, VIP_Define.eVipType.SHOP_OPEN_BLACKMARKET_1);
            //bool OpenTime = false;

            //if (OpenPermission)
            //{
            //    // black market timecheck
            //    int AdminOpenTime = SystemData.AdminConstValueFetchFromRedis(ref TB, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.ADMIN_CONST_DEF_BLACKMARKET_OPEN_START_TIME]);
            //    int AdminEndTime = SystemData.AdminConstValueFetchFromRedis(ref TB, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.ADMIN_CONST_DEF_BLACKMARKET_OPEN_END_TIME]);

            //    DateTime currentDayStart = DateTime.Parse(DateTime.Now.ToShortDateString());
            //    DateTime currentTime = DateTime.Now;
            //    TimeSpan checkTS = currentTime - currentDayStart;

            //    OpenTime = (AdminOpenTime > AdminEndTime && checkTS.TotalSeconds > AdminOpenTime) || (AdminOpenTime < AdminEndTime && checkTS.TotalSeconds > AdminOpenTime && checkTS.TotalSeconds < AdminEndTime) || (AdminOpenTime == AdminEndTime);

            //    if (OpenTime)
            //    {
            //        Shop_Define.DaysWeek flagDay = Shop_Define.DaysWeek.NONE;
            //        if (DateTime.Now.DayOfWeek == DayOfWeek.Monday)
            //            flagDay = Shop_Define.DaysWeek.MON;
            //        else if (DateTime.Now.DayOfWeek == DayOfWeek.Tuesday)
            //            flagDay = Shop_Define.DaysWeek.TUE;
            //        else if (DateTime.Now.DayOfWeek == DayOfWeek.Wednesday)
            //            flagDay = Shop_Define.DaysWeek.WED;
            //        else if (DateTime.Now.DayOfWeek == DayOfWeek.Thursday)
            //            flagDay = Shop_Define.DaysWeek.THU;
            //        else if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            //            flagDay = Shop_Define.DaysWeek.FRI;
            //        else if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday)
            //            flagDay = Shop_Define.DaysWeek.SAT;
            //        else if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
            //            flagDay = Shop_Define.DaysWeek.SUN;

            //        int AdminOpenDay = SystemData.AdminConstValueFetchFromRedis(ref TB, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.ADMIN_CONST_DEF_BLACKMARKET_OPEN_DAY]);
            //        if ((Shop_Define.DaysWeek)AdminOpenDay == Shop_Define.DaysWeek.NONE)
            //            OpenTime = false;
            //        else
            //            OpenTime = FlagsHelper.IsSet((Shop_Define.DaysWeek)AdminOpenDay, flagDay);
            //    }
            //}

            //return (byte)(OpenPermission && OpenTime ? 1 : 0);
        }

        static byte CheckMissionReward(ref TxnBlock TB, long AID)
        {
            // mission reward
            byte bMissionReward = 0;
            List<RetMissionRank> userMissionList = Dungeon_Manager.GetUser_All_MissionRank(ref TB, AID).Where(item => item.rank > 0).ToList();
            List<User_GuerrillaDungeon_Play> userGuerrillaList = Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref TB, AID);
            List<RetEliteDungeonRank> userEliteList = Dungeon_Manager.GetUser_All_EliteDungeonRank(ref TB, AID);
            Dictionary<int, RetWorldRank> userWolrdList = new Dictionary<int, RetWorldRank>();

            foreach (RetMissionRank setMission in userMissionList)
            {
                if (userWolrdList.Keys.Contains(setMission.worldid))
                    userWolrdList[setMission.worldid].rank = userWolrdList[setMission.worldid].rank + setMission.rank;
                else
                {
                    userWolrdList.Add(setMission.worldid, new RetWorldRank(setMission.worldid, setMission.rank));
                }
            }

            foreach (KeyValuePair<int, RetWorldRank> setRank in userWolrdList)
            {
                RetWorldRank userWorldRewardInfo = Dungeon_Manager.GetUser_WorldReward(ref TB, AID, setRank.Key);
                int TotalRank = setRank.Value.rank + userGuerrillaList.Where(item => item.worldid == setRank.Key).Sum(item => item.rank) + userEliteList.Where(item => item.worldid == setRank.Key).Sum(item => item.rank);
                int FirstRewardRank = SystemData.GetConstValueInt(ref TB, Dungeon_Define.Dungen_Const_Def_Key_List[Dungeon_Define.eDungenConstDef.DEF_SCENARIO_COMPLETE_GIFT_STEP1]);
                int SecondRewardRank = SystemData.GetConstValueInt(ref TB, Dungeon_Define.Dungen_Const_Def_Key_List[Dungeon_Define.eDungenConstDef.DEF_SCENARIO_COMPLETE_GIFT_STEP2]);
                if ((TotalRank >= FirstRewardRank && userWorldRewardInfo.reward1 == 0) || (TotalRank >= SecondRewardRank && userWorldRewardInfo.reward2 == 0))
                {
                    bMissionReward = 1;
                    break;
                }
            }
            return bMissionReward;
        }

        static byte CheckAchiveReward(ref TxnBlock TB, long AID)
        {
            List<User_Event_Data> userAchiveList = TriggerManager.Check_Achieve_Data_List(ref TB, AID);            

            List<Character> userCharacter = CharacterManager.GetCharacterList(ref TB, AID);
            List<RetMissionRank> userMission = Dungeon_Manager.GetUser_All_MissionRank(ref TB, AID);
            List<User_GuerrillaDungeon_Play> userGuerillaMission = Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref TB, AID);
            List<RetEliteDungeonRank> userElistMission = Dungeon_Manager.GetUser_All_EliteDungeonRank(ref TB, AID);

            foreach (User_Event_Data checkEvent in userAchiveList)
            {
                if (checkEvent.RewardFlag.Equals("N"))
                {
                    bool isClear = false;
                    int maxValue1 = 0;
                    int maxValue2 = 0;
                    TriggerManager.CheckClear(ref TB, AID, checkEvent, out isClear, out maxValue1, out maxValue2, userCharacter, userMission, userGuerillaMission, userElistMission);

                    if (isClear)
                        return 1;
                }
            }

            return 0;
        }

        static void CheckEventReward(ref TxnBlock TB, long AID, out byte bGiftEvent, out byte bUserEvent)
        {
            List<User_Event_Data> userEventList = TriggerManager.Check_Event_Data_List(ref TB, AID);
            List<System_EventGroup_Admin> baseEventGroupAllList = TriggerManager.GetSystem_EventGroup_Admin(ref TB);
            List<System_EventGroup_Admin> baseEventGroupGiftEventList = baseEventGroupAllList.FindAll(baseInfo => baseInfo.Event_Group_Type == (int)Trigger_Define.eEventGroupType.GiftEvent);

            List<User_Event_Data> userTargetEventList = userEventList.FindAll(eventItem => baseEventGroupGiftEventList.Find(baseInfo => baseInfo.Event_Type.ToLower() == eventItem.Event_Type.ToLower()) != null);

            List<Character> userCharacter = CharacterManager.GetCharacterList(ref TB, AID);
            List<RetMissionRank> userMission = Dungeon_Manager.GetUser_All_MissionRank(ref TB, AID);
            List<User_GuerrillaDungeon_Play> userGuerillaMission = Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref TB, AID);
            List<RetEliteDungeonRank> userElistMission = Dungeon_Manager.GetUser_All_EliteDungeonRank(ref TB, AID);

            bGiftEvent = 0;

            foreach (User_Event_Data checkEvent in userTargetEventList)
            {
                if (checkEvent.RewardFlag.Equals("N"))
                {
                    bool isClear = false;
                    int maxValue1 = 0;
                    int maxValue2 = 0;
                    TriggerManager.CheckClear(ref TB, AID, checkEvent, out isClear, out maxValue1, out maxValue2, userCharacter, userMission, userGuerillaMission, userElistMission);

                    if (isClear)
                    {
                        bGiftEvent = 1;
                        break;
                    }
                }
            }
            
            List<System_EventGroup_Admin> baseEventGroupUserEventList = baseEventGroupAllList.FindAll(baseInfo => baseInfo.Event_Group_Type == (int)Trigger_Define.eEventGroupType.UserEvent);
            userTargetEventList = userEventList.FindAll(eventItem => baseEventGroupUserEventList.Find(baseInfo => baseInfo.Event_Type.ToLower() == eventItem.Event_Type.ToLower()) != null);

            bUserEvent = 0;

            foreach (User_Event_Data checkEvent in userTargetEventList)
            {
                if (checkEvent.RewardFlag.Equals("N"))
                {
                    bool isClear = false;
                    int maxValue1 = 0;
                    int maxValue2 = 0;
                    TriggerManager.CheckClear(ref TB, AID, checkEvent, out isClear, out maxValue1, out maxValue2, userCharacter, userMission, userGuerillaMission, userElistMission);

                    if (isClear)
                    {
                        bUserEvent = 1;
                        break;
                    }
                }
            }
        }

        static byte Check7DayReward(ref TxnBlock TB, long AID, DateTime creationdate)
        {
            int admin_7Day_flag = TriggerManager.Check7DayEvent(ref TB, AID, creationdate);
            bool checkFlag = false;

            if (admin_7Day_flag > 0)
            {
                // check clear event
                int loginCount = 0;
                Result_Define.eResult retError = AccountManager.GetUserLoginCount(ref TB, AID, out loginCount);

                List<System_Event_7Day> system7DayInfo = TriggerManager.GetSystem_7Day_Event_List(ref TB);
                long current = 0;                
                foreach (System_Event_7Day setItem in system7DayInfo)
                {
                    User_Event_7Day_Data userEventInfo = TriggerManager.GetUser_7Day_Event_Info(ref TB, AID, setItem.Event_ID);
                    if (userEventInfo.ClearFlag.Equals("Y") && userEventInfo.RewardFlag.Equals("N"))
                        checkFlag = true;
                    else if (userEventInfo.ClearFlag.Equals("Y") && userEventInfo.RewardFlag.Equals("Y"))
                        continue;
                    else
                    {
                        bool ClearFlag1 = TriggerManager.Check7Day_Clear_Trigger(ref TB, AID, out current, Trigger_Define.TriggerType[setItem.ClearTriggerType1], setItem.ClearTriggerType1_Value1, setItem.ClearTriggerType1_Value2, setItem.ClearTriggerType1_Value3, loginCount);
                        bool ClearFlag2 = TriggerManager.Check7Day_Clear_Trigger(ref TB, AID, out current, Trigger_Define.TriggerType[setItem.ClearTriggerType2], setItem.ClearTriggerType2_Value1, setItem.ClearTriggerType2_Value2, setItem.ClearTriggerType2_Value3, loginCount);

                        if (ClearFlag1 && ClearFlag2 && userEventInfo.RewardFlag.Equals("N"))
                            checkFlag = true;
                    }

                    if (checkFlag)
                        break;
                }

                // check event buy
                if (!checkFlag)
                {
                    List<System_Event_7Day_Package_List> shopPackageList = TriggerManager.GetSystem_7Day_Event_Package_List(ref TB);

                    foreach (System_Event_7Day_Package_List shopitem in shopPackageList)
                    {
                        User_Event_7Day_Data userEventInfo = TriggerManager.GetUser_7Day_Package_Info(ref TB, AID, shopitem.Package_ID);
                        checkFlag = loginCount >= shopitem.Buy_Day && userEventInfo.RewardFlag.Equals("N");

                        if (checkFlag)
                            break;
                    }
                }
            }
            return (byte)(checkFlag ? 1 : 0);
        }

        static byte CheckMailUnread(ref TxnBlock TB, long AID)
        {
            User_Admin_Mail_Check userAdminCheck = MailManager.GetUser_Admin_Mail_Check(ref TB, AID);
            List<Admin_System_MailNotice> adminMailList = MailManager.GetAdmin_NoticeMailList(ref TB, userAdminCheck.last_checked_main_idx);
            long lastcheckseq = userAdminCheck.last_checked_main_idx;

            foreach (Admin_System_MailNotice sendmail in adminMailList)
            {
                userAdminCheck.last_checked_main_idx = sendmail.idx > userAdminCheck.last_checked_main_idx ? sendmail.idx : userAdminCheck.last_checked_main_idx;

                List<Set_Mail_Item> setMailItem = sendmail.MailType == (int)Mail_Define.eMailNoticeType.MAILNOTICE ?
                                                    new List<Set_Mail_Item>() :
                                                    MailManager.GetAdminSendMailItemList(ref TB, sendmail.idx);

                TimeSpan TS = sendmail.endDate - DateTime.Now;
                MailManager.SendMail(ref TB, ref setMailItem, AID, 0, sendmail.senderName, sendmail.title, sendmail.message, (int)TS.TotalMinutes);
            }

            if (lastcheckseq != userAdminCheck.last_checked_main_idx)
                MailManager.SetUser_Admin_Mail_Check(ref TB, userAdminCheck);

            List<User_MailBox> simpleMailList = new List<User_MailBox>(MailManager.GetUser_Mail_List(ref TB, AID));

            var findUnreadMail = simpleMailList.Find(mailitem => mailitem.readflag.Equals("N"));
            return (byte)(findUnreadMail != null ? 1 : 0);
        }

        static byte CheckFriendRequest(ref TxnBlock TB, long AID, ref Dictionary<long, Friends> reqFriendList)
        {
            reqFriendList = FriendManager.GetRequestFriendsList(ref TB, AID);

            return (byte)(reqFriendList.Count > 0 ? 1 : 0);
        }

        static byte CheckGachaShop(ref TxnBlock TB, long AID)
        {
            User_Shop_TreasureBox userShopInfo = ShopManager.GetUser_TreasureBox_List(ref TB, AID);
            DateTime curDate = DateTime.Parse(userShopInfo.regdate.ToShortDateString());
            DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            TimeSpan TS = dbDate - curDate;

            return (byte)(TS.Days != 0 ? 1 : 0);
        }

        static void CheckBossRaidActive(ref TxnBlock TB, long AID, int ChatChannel, ref byte bBossRaidActive, ref bool bBossRaidReward)
        {
            ChatChannel = ChatChannel > 0 ? ChatChannel : Account_Define.BaseChatChannel;
            bool setFlush = BossRaid.CheckPublicRaid(ref TB);
            ActiveBossRaid_Info ActiveBossList = BossRaid.GetActiveBossRaid(ref TB, setFlush);
            List<long> MyFriendAID_List = FriendManager.GetFriend_AID_List(ref TB, AID);
            List<long> setBossIDs = new List<long>();
            MyFriendAID_List.Add(AID);

            if (ActiveBossList.BossList.Count(chkInfo => chkInfo.CreaterAID == AID) > 0)
                bBossRaidActive = 2;
            else
            {
                foreach (BossRaidCreation bossInfo in ActiveBossList.BossList)
                {
                    if (MyFriendAID_List.Contains(bossInfo.CreaterAID) || (bossInfo.PublicChnnel > 0 && bossInfo.PublicChnnel == ChatChannel && bossInfo.PublicDate < ActiveBossList.CurrentDate))
                    {
                        if (bossInfo.Status.Equals(BossRaid_Define.BossRaidStatus[BossRaid_Define.eRaidStatus.Active]) || bossInfo.Status.Equals(BossRaid_Define.BossRaidStatus[BossRaid_Define.eRaidStatus.Clear]))
                            bBossRaidActive = 1;
                    }
                }
            }

            bBossRaidReward = BossRaid.GetBossRaidCount(ref TB, AID, BossRaid_Define.eRaidStatus.Clear, true) > 0;
            //if (BossRaid.GetBossRaidCount(ref TB, AID, BossRaid_Define.eRaidStatus.Clear, true) > 0)
            //    bBossRaidReward = bBossRaidActive = true;
        }

        public static User_Coupon_Key GetUser_Coupon_Key(ref TxnBlock TB, string couponKey, string dbkey = Account_Define.AccountShardingDB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE coupon_key = '{1}'", Account_Define.User_Coupon_Key_TableName, couponKey);
            User_Coupon_Key retObj = GenericFetch.FetchFromDB<User_Coupon_Key>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new User_Coupon_Key();
        }

        public static Result_Define.eResult SetUser_Coupon_Key(ref TxnBlock TB, long AID, string PlatformID, string couponKey, string mailSeqList, Account_Define.eCouponType setType, Account_Define.eCouponState setState, string dbkey = Account_Define.AccountShardingDB)
        {
            string setQuery = string.Format(@"MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON coupon_key = '{1}' AND aid = '{2}' AND platform_user_id = '{3}'
                                                WHEN MATCHED THEN
                                                    UPDATE SET 
                                                        mailseq_json = '{5}',
                                                        stateflag = '{6}',
                                                        updatedate = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                    INSERT (coupon_key, aid, platform_user_id, coupon_type, mailseq_json, stateflag, regdate, updatedate) VALUES ('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', GETDATE(), GETDATE());
                                                ", Account_Define.User_Coupon_Key_TableName
                                                 , couponKey
                                                 , AID
                                                 , PlatformID
                                                 , Account_Define.Account_Coupon_Def_Type[setType]
                                                 , mailSeqList
                                                 , Account_Define.Account_Coupon_Def_State[setState]);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static float CheckPointRate(ref TxnBlock TB, out int targetContents)
        {
            bool isBonus = false;
            targetContents = 0;
            float bonusRate = 1.0f;

            long currentTime = (long)(DateTime.Now - DateTime.Parse(DateTime.Now.ToShortDateString())).TotalSeconds;
            int openStart = SystemData.AdminConstValueFetchFromRedis(ref TB, SystemData_Define.AdminConstKey_List[SystemData_Define.eAdminConst.EXTRA_POINT_START_TIME]);
            int openEnd = SystemData.AdminConstValueFetchFromRedis(ref TB, SystemData_Define.AdminConstKey_List[SystemData_Define.eAdminConst.EXTRA_POINT_END_TIME]);

            if (openStart < openEnd)
                isBonus = currentTime > openStart && currentTime < openEnd;
            else if (openStart > openEnd)
                isBonus = currentTime > openStart || currentTime < openEnd;
            else
                isBonus = true;

            if (isBonus)
            {
                int ratePercent = SystemData.AdminConstValueFetchFromRedis(ref TB, SystemData_Define.AdminConstKey_List[SystemData_Define.eAdminConst.EXTRA_POINT_RATE]);
                if (ratePercent > 100)
                {
                    bonusRate = ratePercent / 100.0f;
                    targetContents = SystemData.AdminConstValueFetchFromRedis(ref TB, SystemData_Define.AdminConstKey_List[SystemData_Define.eAdminConst.EXTRA_POINT_CONTENTS]);
                }

            }

            return bonusRate;
        }

        public static float CheckExpRate(ref TxnBlock TB, out int targetContents)
        {
            bool isBonus = false;
            targetContents = 0;
            float bonusRate = 1.0f;

            long currentTime = (long)(DateTime.Now - DateTime.Parse(DateTime.Now.ToShortDateString())).TotalSeconds;
            int openStart = SystemData.AdminConstValueFetchFromRedis(ref TB, SystemData_Define.AdminConstKey_List[SystemData_Define.eAdminConst.EXTRA_EXP_START_TIME]);
            int openEnd = SystemData.AdminConstValueFetchFromRedis(ref TB, SystemData_Define.AdminConstKey_List[SystemData_Define.eAdminConst.EXTRA_EXP_END_TIME]);

            if (openStart < openEnd)
                isBonus = currentTime > openStart && currentTime < openEnd;
            else if (openStart > openEnd)
                isBonus = currentTime > openStart || currentTime < openEnd;
            else
                isBonus = true;

            if (isBonus)
            {
                int ratePercent = SystemData.AdminConstValueFetchFromRedis(ref TB, SystemData_Define.AdminConstKey_List[SystemData_Define.eAdminConst.EXTRA_EXP_RATE]);
                if (ratePercent > 100)
                {
                    bonusRate = ratePercent / 100.0f;
                    targetContents = SystemData.AdminConstValueFetchFromRedis(ref TB, SystemData_Define.AdminConstKey_List[SystemData_Define.eAdminConst.EXTRA_EXP_CONTENTS]);
                }
            }

            return bonusRate;
        }
    }
}
