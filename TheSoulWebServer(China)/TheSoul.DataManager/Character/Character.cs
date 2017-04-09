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
    public static class CharacterManager
    {
        //const string CharacterDBName = "sharding";
        //const string CharacterTableName = "Character";
        //const string CharacterGroupTable = "User_CharacterGroup";
        //const string CharacterPrefix = "Character";

        private static Dictionary<long, Character> FetchFromDB(ref TxnBlock TB, string setQuery, string dbkey = Character_Define.CharacterDBName)
        {
            SqlDataReader getDr = null;
            TB.ExcuteSqlCommand(dbkey, setQuery, ref getDr);
            Dictionary<long, Character> retSet = new Dictionary<long, Character>();

            if (getDr != null)
            {
                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);
                getDr.Dispose(); getDr.Close();
                Character[] setData = mJsonSerializer.JsonToObject<Character[]>(json);

                foreach (Character charInfo in setData)
                {
                    retSet.Add(charInfo.cid, charInfo);
                }
            }

            return retSet;
        }

        public static Account_Simple_With_Character GetSimpleAccountCharacterInfo_ByCharacterManager(ref TxnBlock TB, long CID, bool Flush = false, string dbkey = Character_Define.CharacterDBName)
        {
            string setKey = string.Format("{0}_{1}_{2}_Simple_AccInfo", Character_Define.CharacterPrefix, Character_Define.CharacterTableName, CID);
            Account_Simple_With_Character retObj = null;
            if (!Flush)
            {
                retObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis<Account_Simple_With_Character>(DataManager_Define.RedisServerAlias_User, setKey);
            }

            if (retObj == null)
                Flush = true;

            if (Flush)
            {
                string setQuery = string.Format("SELECT aid FROM {0} WITH(NOLOCK)  WHERE cid = {1}", Character_Define.CharacterTableName, CID);
                Account_OnlyAID setAID = TheSoul.DataManager.GenericFetch.FetchFromDB<Account_OnlyAID>(ref TB, setQuery, dbkey);
                if (setAID != null)
                {
                    retObj = AccountManager.GetSimpleAccountCharacterInfo(ref TB, setAID.aid, CID, Flush, dbkey);
                    if (retObj.cid > 0)
                        RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, retObj);
                }
            }

            return retObj;
        }

        private static string GetCharacterRedisKey(long AID)
        {
            return string.Format("{0}_{1}_{2}", Character_Define.CharacterPrefix, Character_Define.CharacterTableName, AID);
        }

        public static List<Character> GetCharacterList(ref TxnBlock TB, long AID,  bool Flush = false, string dbkey = Character_Define.CharacterDBName)
        {
            string setKey = GetCharacterRedisKey(AID);
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
                string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK, INDEX(IDX_Character_AID)) WHERE aid = {1}", Character_Define.CharacterTableName, AID);
                retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Character>(ref TB, setQuery, dbkey);

                retObj.ForEach(item =>
                {
                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, item.cid.ToString(), item);
                });
            }

            //RedisConst.GetRedisInstance().SetExpireTimeHash(DataManager_Define.RedisServerAlias_User, setKey);

            return retObj;
        }

        public static Character GetCharacter_FromDB(ref TxnBlock TB, long AID, long CID, string dbkey = Character_Define.CharacterDBName)
        {
            string setQuery = string.Format("SELECT cid, aid, class as Class, level, equipflag FROM {0} WITH(NOLOCK) WHERE aid = {1} AND cid = {2}", Character_Define.CharacterTableName, AID, CID);
            Character retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<Character>(ref TB, setQuery, dbkey);
            return (retObj == null) ? new Character() : retObj;
        }

        public static int GetCharacterCount_FromDB(ref TxnBlock TB, long AID, string dbkey = Character_Define.CharacterDBName)
        {
            string setQuery = string.Format("SELECT COUNT(*) as count FROM {0} WITH(NOLOCK) WHERE aid = {1}", Character_Define.CharacterTableName, AID);
            Rank_Count retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<Rank_Count>(ref TB, setQuery, dbkey);
            return (int)((retObj == null) ? 0 : retObj.count);
        }

        public static Character GetCharacter(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Character_Define.CharacterDBName)
        {
            string setKey = GetCharacterRedisKey(AID);
            Character retObj = new Character();

            if(!Flush)
                retObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_Hash_Field<Character>(DataManager_Define.RedisServerAlias_User, setKey, CID.ToString());

            if (retObj == null || Flush)
            {
                RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
                string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE aid = {1}", Character_Define.CharacterTableName, AID);
                List<Character> retList = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<Character>(ref TB, setQuery, dbkey);

                retList.ForEach(item =>
                {
                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, item.cid.ToString(), item);
                });

                retObj = retList.Find(charinfo => charinfo.cid == CID);

                if (retObj == null)
                    retObj = new Character();
                else
                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, CID.ToString(), retObj);
            }

            TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_role_level], retObj.level);

            return retObj;
        }

        public static void RemoveCacheCharInfo(long CID)
        {
            //string setKey = string.Format("{0}_{1}_{2}", Character_Define.CharacterPrefix, Character_Define.CharacterTableName, AID);
            RemoveCacheCharacterStat(CID);
        }

        public static Character FlushCharacter(ref TxnBlock TB, long AID, long CID, string dbkey = Character_Define.CharacterDBName)
        {
            return GetCharacter(ref TB, AID, CID, true, dbkey);
        }

        public static List<Character> FlushCharacterList(ref TxnBlock TB, long AID, string dbkey = Character_Define.CharacterDBName)
        {
            return GetCharacterList(ref TB, AID, true, dbkey);
        }

        public static Character_Detail_With_HP GetCharacterInfoWithEquip(ref TxnBlock TB, long AID, long CID, bool Flush = false)
        {
            Character setChar = CharacterManager.GetCharacter(ref TB, AID, CID, Flush);

            Character_Detail_With_HP retObj = new Character_Detail_With_HP(setChar, -1, -1);
            retObj.equiplist = ItemManager.GetEquipList(ref TB, AID, CID, Flush);
            retObj.equip_active_soul = SoulManager.GetRet_Active_Soul_Equip_List(ref TB, AID, CID, Flush);
            retObj.equip_passive_soul = SoulManager.GetRet_Passive_Soul_Equip_List(ref TB, AID, CID, Flush);
            return retObj;
        }

        public static List<Character_Detail> GetCharacterListWithEquip(ref TxnBlock TB, long AID, bool Flush = false)
        {
            List<Character> userCharList = CharacterManager.GetCharacterList(ref TB, AID, Flush);
            List<Character_Detail> retList = new List<Character_Detail>();
            foreach (Character setChar in userCharList)
            {
                Character_Detail setObj = new Character_Detail(setChar);
                setObj.equiplist = ItemManager.GetEquipList(ref TB, AID, setChar.cid, true, Flush);
                setObj.equip_active_soul = SoulManager.GetRet_Active_Soul_Equip_List(ref TB, AID, setChar.cid, Flush);
                setObj.equip_passive_soul = SoulManager.GetRet_Passive_Soul_Equip_List(ref TB, AID, setChar.cid, Flush);
                setObj.equip_ultimate = ItemManager.GetEquipUltimate(ref TB, AID, setChar.cid, true, Flush);

                retList.Add(setObj);
            }
            return retList;
        }

        public static List<Character_Detail_With_HP> GetCharacterListWithEquip_HP(ref TxnBlock TB, long AID, bool Flush = false)
        {
            List<Character> userCharList = CharacterManager.GetCharacterList(ref TB, AID);
            List<Character_Detail_With_HP> retList = new List<Character_Detail_With_HP>();

            foreach (Character setChar in userCharList)
            {
                Character_Stat getStat = GetCharacterStat(ref TB, setChar.cid, Flush);
                setChar.warpoint = getStat.WAR_POINT + getStat.ACTIVE_SOUL_WAR_POINT + getStat.PASSIVE_SOUL_WAR_POINT;
                Character_Detail_With_HP setObj = new Character_Detail_With_HP(setChar, getStat.HPMax, getStat.HPMax);
                setObj.equiplist = ItemManager.GetEquipList(ref TB, AID, setChar.cid);
                setObj.equip_active_soul = SoulManager.GetRet_Active_Soul_Equip_List(ref TB, AID, setChar.cid);
                setObj.equip_passive_soul = SoulManager.GetRet_Passive_Soul_Equip_List(ref TB, AID, setChar.cid);
                setObj.equip_ultimate = ItemManager.GetEquipUltimate(ref TB, AID, setChar.cid);

                retList.Add(setObj);
            }
            return retList;
        }

        public static Character_Detail_With_HP GetCharacterWithEquip_HP(ref TxnBlock TB, long AID, long CID, string dbkey = Character_Define.CharacterDBName, bool Flush = false)
        {
            Character setChar = CharacterManager.GetCharacter(ref TB, AID, CID, Flush, dbkey);
            Character_Stat getStat = GetCharacterStat(ref TB, CID, Flush, dbkey);
            setChar.warpoint = getStat.WAR_POINT + getStat.ACTIVE_SOUL_WAR_POINT + getStat.PASSIVE_SOUL_WAR_POINT;
            Character_Detail_With_HP setObj = new Character_Detail_With_HP(setChar, getStat.HPMax, getStat.HPMax);
            setObj.equiplist = ItemManager.GetEquipList(ref TB, AID, CID, true, Flush, dbkey);
            setObj.equip_active_soul = SoulManager.GetRet_Active_Soul_Equip_List(ref TB, AID, CID, Flush, dbkey);
            setObj.equip_passive_soul = SoulManager.GetRet_Passive_Soul_Equip_List(ref TB, AID, CID, Flush, dbkey);
            setObj.equip_ultimate = ItemManager.GetEquipUltimate(ref TB, AID, setChar.cid);

            return setObj;
        }

        // Get System PC_Base Info
        const string SystemPCBASE_Prefix = "System_PC_BASE";
        const string SystemPC_Base_TableName = "System_PC_BASE";

        public static System_PC_BASE GetPCbaseInfo(ref TxnBlock TB, int pccode, string dbkey = Character_Define.CharacterDBName, bool Flush = false)
        {
            string setKey = string.Format("{0}_{1}", SystemPCBASE_Prefix, SystemPC_Base_TableName);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE PC_Code = {1}", SystemPC_Base_TableName, pccode);
            return TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_PC_BASE>(ref  TB, DataManager_Define.RedisServerAlias_System, setKey, pccode.ToString(), setQuery, dbkey, Flush);
        }

        public static Result_Define.eResult CreateCharacterSharding(ref TxnBlock TB, long AID, long CID, short setClass, string dbkey = Character_Define.CharacterDBName, bool Flush = false)
        {
            SqlCommand commandCreateCharacter = new SqlCommand();
            commandCreateCharacter.CommandText = "System_CreateCharacter";
            var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandCreateCharacter.Parameters.Add("@aid", SqlDbType.BigInt).Value = AID;
            commandCreateCharacter.Parameters.Add("@cid", SqlDbType.NVarChar, 32).Value = CID;
            commandCreateCharacter.Parameters.Add("@class", SqlDbType.TinyInt).Value = setClass;
            commandCreateCharacter.Parameters.Add(result);

            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandCreateCharacter))
            {
                if (System.Convert.ToInt32(result.Value) < 0)
                {
                    int dbresult = System.Convert.ToInt32(result.Value) * -1;       // re delcare result code 
                    commandCreateCharacter.Dispose();
                    if ((Result_Define.eResult)dbresult == Result_Define.eResult.CHARACTER_ID_ALREAD_CREATED)
                        return Result_Define.eResult.CHARACTER_ID_ALREAD_CREATED;
                    else
                        return Result_Define.eResult.CHARACTER_ID_CRETE_FAIL;
                }
                else
                {
                    commandCreateCharacter.Dispose();
                    return Result_Define.eResult.SUCCESS;
                }
            }
            else
            {
                commandCreateCharacter.Dispose();
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }
        }

        public static Result_Define.eResult RegistCharacterGlobal(ref TxnBlock TB, ref long CID, long AID, long server_group_id, bool isADD, string dbkey = DataManager_Define.GlobalDB, bool Flush = false)
        {
            CID = 0;
            SqlCommand commandRegisterCharacter = new SqlCommand();
            commandRegisterCharacter.CommandText = "System_Reg_PlayCharacter";
            var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandRegisterCharacter.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            commandRegisterCharacter.Parameters.Add("@ADD_FLAG", SqlDbType.TinyInt).Value = isADD ? 1 : 0;
            commandRegisterCharacter.Parameters.Add("@ServerGroupID", SqlDbType.BigInt).Value = server_group_id;
            commandRegisterCharacter.Parameters.Add(result);

            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandRegisterCharacter))
            {
                if (System.Convert.ToInt64(result.Value) < 0)
                {
                    long dbresult = System.Convert.ToInt64(result.Value) * -1;       // re delcare result code 
                    commandRegisterCharacter.Dispose();
                    if ((Result_Define.eResult)dbresult == Result_Define.eResult.CHARACTER_ID_GLOBAL_REGIST_ALREADY)
                        return Result_Define.eResult.CHARACTER_ID_GLOBAL_REGIST_ALREADY;
                    else
                        return Result_Define.eResult.CHARACTER_ID_GLOBAL_REGIST_FAIL;
                }
                else
                {
                    CID = System.Convert.ToInt64(result.Value);
                    commandRegisterCharacter.Dispose();
                    return Result_Define.eResult.SUCCESS;
                }
            }
            else
            {
                commandRegisterCharacter.Dispose();
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }
        }

        public static void GetCharacterMaxLevel_EXP(ref TxnBlock TB, long AID, out short outLevel, out long outExp, string dbkey = Character_Define.CharacterDBName)
        {
            List<Character> charList = GetCharacterList(ref TB, AID, false, dbkey);
            outLevel = charList.Max(item => item.level);
            outExp = charList.Max(item => item.totalexp);
        }

        public static int GetCharacterMaxLevel_FromDB(ref TxnBlock TB, long AID, string dbkey = Character_Define.CharacterDBName)
        {
            string setQuery = string.Format("SELECT MAX(level) as count FROM {0} WITH(NOLOCK) WHERE aid = {1}", Character_Define.CharacterTableName, AID);
            Rank_Count retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<Rank_Count>(ref TB, setQuery, dbkey);
            return (int)((retObj == null) ? 0 : retObj.count);
            //return GetCharacterList(ref TB, AID, false, dbkey).Max(item => item.level);
        }

        public static int GetCharacterMaxLevel_FromRedis(ref TxnBlock TB, long AID, string dbkey = Character_Define.CharacterDBName)
        {
            return GetCharacterList(ref TB, AID, false, dbkey).Max(item => item.level);
        }

        public static Result_Define.eResult UpdateCharacterInfo(ref TxnBlock TB, long CID, long AID, int EXP, int Gold = 0, string dbkey = Character_Define.CharacterDBName, bool Flush = false, bool gmtool = false)
        {
            List<Character> charList = GetCharacterList(ref TB, AID, Flush, dbkey);

            Character charInfo = charList.Find(setitem => setitem.cid == CID);
            if (charInfo == null && !Flush)
            {
                Flush = true;
                charList = GetCharacterList(ref TB, AID, Flush, dbkey);
                charInfo = charList.Find(setitem => setitem.cid == CID);
                if (charInfo == null)
                    return Result_Define.eResult.CHARACTER_NOT_FOUND;
            }else if (charInfo == null)
                return Result_Define.eResult.CHARACTER_NOT_FOUND;

            int CurLevel = charInfo.level;
            long SetExp = 0;
            short SetMaxLevel = charList.Max(item => item.level);
            int SetMaxExp = charList.Max(item => item.totalexp);
            //GetCharacterMaxLevel_EXP(ref TB, AID, out SetMaxLevel, out SetMaxExp);
            
            charInfo.level = SetMaxLevel;
            charInfo.totalexp = SetMaxExp;
            short SetLevel = CalcCharcterLevel(ref TB, SetMaxExp, EXP, ref SetExp, dbkey);
            SetMaxExp += EXP;
            
            Result_Define.eResult retErr = SetLevel > 0 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
            int SetActiveSlot = 0;
            int SetPassiveSlot = 0;

            if (retErr == Result_Define.eResult.SUCCESS)
            {
                foreach (Character setCharInfo in charList)
                {
                    SoulManager.CalcSoulSlot(ref TB, SetLevel, setCharInfo.activeslot, setCharInfo.passiveslot, out SetActiveSlot, out SetPassiveSlot);
                    string setQuery = string.Format(@"UPDATE {0} SET totalexp={1} ,level={2}, exp={3}, activeslot={4}, passiveslot={5} WHERE aid = {6} AND cid= {7} AND delflag='N'"
                                                            , Character_Define.CharacterTableName, SetMaxExp, SetLevel, SetExp, SetActiveSlot, SetPassiveSlot, AID, setCharInfo.cid);
                    if(!TB.ExcuteSqlCommand(dbkey, setQuery))
                        return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                }
            }
            if (CurLevel < SetLevel && retErr == Result_Define.eResult.SUCCESS && !gmtool) // dif level (it's mean Level up)
            {
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_role_upgrade_log]);
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_logtype], charInfo.Class);
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_level_before], CurLevel);
                TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_level_after], SetLevel);

                //List<Character> charList = GetCharacterList(ref TB, AID);
                Account userInfo = AccountManager.FlushAccountData(ref TB, AID, ref retErr);
                if (retErr != Result_Define.eResult.SUCCESS)
                    return retErr;

                //int SetMaxLevel = GetCharacterMaxLevel(ref TB, AID, dbkey);

                int SetFromLevel = SetMaxLevel;
                bool bMaxOver = SetMaxLevel < SetLevel;
                SetMaxLevel = bMaxOver ? SetLevel : SetMaxLevel;
                                
                short SetKeyMax = 0;
                short SetTicketMax = 0;
                short SetKeyInc = 0;
                short SetTicketInc = 0;

                CalcPlayEnerge(ref TB, ref SetKeyMax, ref SetTicketMax, ref SetKeyInc, ref SetTicketInc, bMaxOver, SetMaxLevel, SetFromLevel);

                userInfo.KeyFillMaxEA = SetKeyMax;
                userInfo.TicketFillMaxEA = SetTicketMax;
                userInfo.Key += SetKeyInc;
                userInfo.Ticket += SetTicketInc;

                // not use yet
                /*
                float G3VS3_Energy_Init = (float)SystemData.GetConstValue(ref TB, "DEF_ENERGY_G3VS3_INIT_VALUE");
                float G3VS3_Energy_Inc_Level = (float)SystemData.GetConstValue(ref TB, "DEF_ENERGY_G3VS3_INC_LEVEL");
                float G3VS3_Energy_Inc_Value = (float)SystemData.GetConstValue(ref TB, "DEF_ENERGY_G3VS3_INC_VALUE");

                int SetChallengeInc = 0;
                try
                {
                    SetChallengeInc = System.Convert.ToInt32((G3VS3_Energy_Inc_Level * G3VS3_Energy_Inc_Value) > 0 ? G3VS3_Energy_Init + ((G3VS3_Energy_Inc_Value * SetMaxLevel) / G3VS3_Energy_Inc_Level) : G3VS3_Energy_Init);
                }
                catch (Exception e)
                {
                    SetChallengeInc = 0;
                }
                userInfo.ChallengeTicket = (userInfo.ChallengeTicket < SetChallengeInc) ? SetChallengeInc : userInfo.ChallengeTicket;
                */

                string setQuery = string.Format(@"UPDATE {0} SET
                                                        [Key]={1} ,               KeyFillMaxEA={2} ,              KeyLastChargeTime = DATEDIFF(SS,'1970-01-01',GETDATE()),
                                                        Ticket={3} ,            TicketFillMaxEA={4} ,            TicketLastChargeTime = DATEDIFF(SS,'1970-01-01',GETDATE()),
                                                        ChallengeTicket={5} ,   ChallengeTicketFillMaxEA={6} ,  ChallengeTicketLastChargeTime = DATEDIFF(SS,'1970-01-01',GETDATE()),
                                                        PVEPlayState = 0,
                                                        [LV]={7}
                                                    WHERE AID={8}",
                    Account_Define.AccountDBTableName,  
                    userInfo.Key, userInfo.KeyFillMaxEA,
                    userInfo.Ticket, userInfo.TicketFillMaxEA,
                    userInfo.ChallengeTicket, userInfo.ChallengeTicketFillMaxEA,
                    SetLevel,
                    AID
                    );
                retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

                if (retErr == Result_Define.eResult.SUCCESS)
                {
                    retErr = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.Class);
                }
            }

            if (retErr == Result_Define.eResult.SUCCESS && 0 != Gold)
            {
                retErr = AccountManager.AddUserGold(ref TB, AID, Gold);
            }

            if(retErr == Result_Define.eResult.SUCCESS)
                charInfo = FlushCharacter(ref TB, AID, CID, dbkey);

            return retErr;
        }

        public static void CalcPlayEnerge(ref TxnBlock TB, ref short SetKeyMax, ref short SetTicketMax, ref short SetKeyInc, ref short SetTicketInc, bool bMaxOver, int SetMaxLevel, int SetFromLevel)
        {
            double PvE_Energy_Init = SystemData.GetConstValue(ref TB, Character_Define.Character_Const_Def_Key_List[Character_Define.eCharacterConstDef.DEF_ENERGY_PVE_INIT_VALUE]);
            double PvE_Energy_Inc_Level = SystemData.GetConstValue(ref TB, Character_Define.Character_Const_Def_Key_List[Character_Define.eCharacterConstDef.DEF_ENERGY_PVE_INC_LEVEL]);
            double PvE_Energy_Inc_Value = SystemData.GetConstValue(ref TB, Character_Define.Character_Const_Def_Key_List[Character_Define.eCharacterConstDef.DEF_ENERGY_PVE_INC_VALUE]);

            // KeyMax = DEF_ENERGY_PVE_INIT_VALUE+ INT((Lv x DEF_ENERGY_PVE_INC_VALUE) / DEF_ENERGY_PVE_INC_LEVEL)
            if (PvE_Energy_Inc_Level > 0)
                SetKeyMax = System.Convert.ToInt16(PvE_Energy_Init + (int)((SetMaxLevel * PvE_Energy_Inc_Value) / PvE_Energy_Inc_Level));

            double PvP_Energy_Init = SystemData.GetConstValue(ref TB, Character_Define.Character_Const_Def_Key_List[Character_Define.eCharacterConstDef.DEF_ENERGY_PVP_INIT_VALUE]);
            double PvP_Energy_Inc_Level = SystemData.GetConstValue(ref TB, Character_Define.Character_Const_Def_Key_List[Character_Define.eCharacterConstDef.DEF_ENERGY_PVP_INC_LEVEL]);
            double PvP_Energy_Inc_Value = SystemData.GetConstValue(ref TB, Character_Define.Character_Const_Def_Key_List[Character_Define.eCharacterConstDef.DEF_ENERGY_PVP_INC_VALUE]);

            // TicketMax = DEF_ENERGY_PVP_INIT_VALUE+ INT((Lv x DEF_ENERGY_PVP_INC_VALUE) / DEF_ENERGY_PVP_INC_LEVEL)
            if (PvP_Energy_Inc_Value > 0)
                SetTicketMax = System.Convert.ToInt16(PvP_Energy_Init + (int)((PvP_Energy_Inc_Value * SetMaxLevel) / PvP_Energy_Inc_Level));

            if (bMaxOver)
            {
                while(SetFromLevel < SetMaxLevel)
                {
                    SetFromLevel++;
                    // KeyInc = DEF_ENERGY_PVE_LEVELUP_CHARGE_INIT_VALUE + INT((Lv / DEF_ENERGY_PVE_LEVELUP_CHARGE_PERIOD_VALUE) x DEF_ENERGY_PVE_LEVELUP_CHARGE_INC_VALUE)
                    int PvE_Energe_LevelUp_Init = SystemData.GetConstValueInt(ref TB, Character_Define.Character_Const_Def_Key_List[Character_Define.eCharacterConstDef.DEF_ENERGY_PVE_LEVELUP_CHARGE_INIT_VALUE]);
                    int PvE_Energe_LevelUp_Charge_Value = SystemData.GetConstValueInt(ref TB, Character_Define.Character_Const_Def_Key_List[Character_Define.eCharacterConstDef.DEF_ENERGY_PVE_LEVELUP_CHARGE_PERIOD_VALUE]);
                    int PvE_Energe_LevelUp_Charge_Inc = SystemData.GetConstValueInt(ref TB, Character_Define.Character_Const_Def_Key_List[Character_Define.eCharacterConstDef.DEF_ENERGY_PVE_LEVELUP_CHARGE_INC_VALUE]);

                    if (PvE_Energe_LevelUp_Charge_Value > 0)
                        SetKeyInc += System.Convert.ToInt16(PvE_Energe_LevelUp_Init + (int)(((double)SetFromLevel / PvE_Energe_LevelUp_Charge_Value) * PvE_Energe_LevelUp_Charge_Inc));

                    // TicketInc = DEF_ENERGY_PVP_LEVELUP_CHARGE_INIT_VALUE + INT((Lv / DEF_ENERGY_PVP_LEVELUP_CHARGE_PERIOD_VALUE) x DEF_ENERGY_PVP_LEVELUP_CHARGE_INC_VALUE)
                    int PvP_Energe_LevelUp_Init = SystemData.GetConstValueInt(ref TB, Character_Define.Character_Const_Def_Key_List[Character_Define.eCharacterConstDef.DEF_ENERGY_PVP_LEVELUP_CHARGE_INIT_VALUE]);
                    int PvP_Energe_LevelUp_Charge_Value = SystemData.GetConstValueInt(ref TB, Character_Define.Character_Const_Def_Key_List[Character_Define.eCharacterConstDef.DEF_ENERGY_PVP_LEVELUP_CHARGE_PERIOD_VALUE]);
                    int PvP_Energe_LevelUp_Charge_Inc = SystemData.GetConstValueInt(ref TB, Character_Define.Character_Const_Def_Key_List[Character_Define.eCharacterConstDef.DEF_ENERGY_PVP_LEVELUP_CHARGE_INC_VALUE]);

                    if (PvP_Energe_LevelUp_Charge_Value > 0)
                        SetTicketInc += System.Convert.ToInt16(PvP_Energe_LevelUp_Init + (int)(((double)SetFromLevel / PvP_Energe_LevelUp_Charge_Value) * PvP_Energe_LevelUp_Charge_Inc));
                }
            }
        }

        public const string CharacterExpTableName = "System_Character_EXP";

        public static System_Character_EXP GetSystemExp(ref TxnBlock TB, int Level, string dbkey = Character_Define.CharacterDBName)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) Where level = {1} ", CharacterExpTableName, Level - 1);
            System_Character_EXP retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<System_Character_EXP>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new System_Character_EXP();
        }

        private static short CalcCharcterLevel(ref TxnBlock TB, long curExp, long addExp, ref long setExp, string dbkey = Character_Define.CharacterDBName)
        {
            string setKey = string.Format("{0}", CharacterExpTableName);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) ", CharacterExpTableName);
            List<System_Character_EXP> expList = TheSoul.DataManager.GenericFetch.FetchFromRedis_MultipleRow<System_Character_EXP>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey);

            if(expList.Count < 1)
                return 0;

            short SetLevel = 1;
            long Exp = curExp + addExp;
            long lastCheckExp = 0;
            List<System_Character_EXP> checkList = expList.OrderBy(item => item.Level).ToList();

            foreach (System_Character_EXP checkExp in checkList)
            {
                SetLevel = checkExp.Level;
                if (Exp < checkExp.ACC_exp)
                {
                    setExp = Exp - lastCheckExp;
                    break;
                }
                lastCheckExp = checkExp.ACC_exp;
            }

            return SetLevel;
        }

        public static Result_Define.eResult UpdateCharacterSoulSlot(ref TxnBlock TB, long AID, long CID, int activeSoul, int passivesoul, string dbkey = Character_Define.CharacterDBName, bool Flush = false)
        {
            string setQuery = string.Format(@"UPDATE {0} SET activeslot={1}, passiveslot={2} WHERE aid = {3} AND cid={4} AND delflag='N'"
                                                    , Character_Define.CharacterTableName, activeSoul, passivesoul, AID, CID);
            Result_Define.eResult retErr = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            
            if(retErr == Result_Define.eResult.SUCCESS)
                FlushCharacter(ref TB, AID, CID);

            return retErr;
        }
        
        public static Result_Define.eResult UpdateCharacterPassiveSoulExp(ref TxnBlock TB, long AID, long CID, int PassiveSoulEXP, string dbkey = Character_Define.CharacterDBName, bool Flush = false)
        {
            string setQuery = string.Format(@"UPDATE {0} SET PassiveSoulEXP=PassiveSoulEXP+{1} WHERE AID={2} AND cid={3} AND delflag='N'", Character_Define.CharacterTableName, PassiveSoulEXP, AID, CID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult UseCharacterPassiveSoulExp(ref TxnBlock TB, long AID, long CID, int PassiveSoulEXP, string dbkey = Character_Define.CharacterDBName, bool Flush = false)
        {
            string setQuery = string.Format(@"UPDATE {0} SET PassiveSoulEXP=PassiveSoulEXP-{1} WHERE AID={2} AND cid={3} AND delflag='N'", Character_Define.CharacterTableName, PassiveSoulEXP, AID, CID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult UpdateCharacterGroup(ref TxnBlock TB, long AID, User_CharacterGroup setGroup, string dbkey = Character_Define.CharacterDBName, bool Flush = false)
        {
            string setQuery = string.Format(@"UPDATE {0} SET cid1 = {2}, cid2 = {3}, cid3 = {4} WHERE aid = {1}", Character_Define.CharacterGroupTable, AID, setGroup.cid1, setGroup.cid2, setGroup.cid3);

            Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            if (retError == Result_Define.eResult.SUCCESS)
            {
                setQuery = string.Format(@"UPDATE {0} SET equipflag = 'N' WHERE aid = {1} AND cid <> {2}", Character_Define.CharacterTableName, AID, setGroup.cid1);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }
            if (retError == Result_Define.eResult.SUCCESS)
            {
                setQuery = string.Format(@"UPDATE {0} SET equipflag = 'Y' WHERE aid = {1} AND cid = {2}", Character_Define.CharacterTableName, AID, setGroup.cid1);
                retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }
            RemoveCacheCharacterGruop(AID);
            return retError;
        }

        private static void RemoveCacheCharacterGruop(long AID)
        {
            string setKey = string.Format("{0}_{1}_{2}", Character_Define.CharacterPrefix, Character_Define.CharacterGroupTable, AID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static User_CharacterGroup GetCharacterGroupInfo(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Dungeon_Define.Dungeon_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}", Character_Define.CharacterPrefix, Character_Define.CharacterGroupTable, AID);

            User_CharacterGroup userCharacterGroupInfo = Flush ? null : GenericFetch.FetchFromOnly_Redis<User_CharacterGroup>(DataManager_Define.RedisServerAlias_User, setKey);
            if (userCharacterGroupInfo == null)
            {
                SqlCommand commandUser_GroupInfo = new SqlCommand();
                commandUser_GroupInfo.CommandText = "System_Get_UserCharacterGroup";
                var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
                commandUser_GroupInfo.Parameters.Add("@aid", SqlDbType.BigInt).Value = AID;
                commandUser_GroupInfo.Parameters.Add("@cid", SqlDbType.BigInt).Value = CID;
                commandUser_GroupInfo.Parameters.Add(result);

                SqlDataReader getDr = null;
                if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUser_GroupInfo, ref getDr))
                {
                    if (getDr != null)
                    {
                        var r = SQLtoJson.Serialize(ref getDr);
                        string json = mJsonSerializer.ToJsonString(r);
                        getDr.Dispose(); getDr.Close();
                        int checkValue = System.Convert.ToInt32(result.Value);

                        if (checkValue < 0)
                            return null;

                        User_CharacterGroup[] retSet = mJsonSerializer.JsonToObject<User_CharacterGroup[]>(json);

                        if (retSet.Length > 0)
                            userCharacterGroupInfo = retSet[0];

                        RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, userCharacterGroupInfo);
                    }
                }
                commandUser_GroupInfo.Dispose();
            }
            return userCharacterGroupInfo == null ? new User_CharacterGroup() : userCharacterGroupInfo;
        }


        public static string GetWarpointRedisKey(long CID)
        {
            return string.Format("{0}_{1}_{2}", GoldExpedition_Define.UserGE_Prefix, GoldExpedition_Define.User_WarPoint_Table_Name, CID);
        }

        private static void RemoveCacheCharacterWarpoint(long CID)
        {
            string setKey = GetWarpointRedisKey(CID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static Result_Define.eResult UpdateCharacterWarpoint(ref TxnBlock TB, long AID, long CID, long Warpoint, string dbkey = Character_Define.CharacterDBName, bool Flush = false)
        {
            string setQuery = string.Format(@"UPDATE {0} SET warpoint = {1} WHERE aid = {2} AND cid = {3};", Character_Define.CharacterTableName, Warpoint, AID, CID);
            RemoveCacheCharacterWarpoint(CID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static PvP_WarPoint GetCharacterWarpoint(ref TxnBlock TB, long CID, bool Flush = false, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            string setKey = GetWarpointRedisKey(CID);
            string setQuery = string.Format(@"
                                SELECT aid, cid, (WAR_POINT + ACTIVE_SOUL_WAR_POINT + PASSIVE_SOUL_WAR_POINT) AS warpoint FROM 
                                        (SELECT B.aid, B.cid, A.WAR_POINT, A.ACTIVE_SOUL_WAR_POINT, A.PASSIVE_SOUL_WAR_POINT FROM {0} AS A WITH(NOLOCK) INNER JOIN {1} AS B WITH(NOLOCK) 
                                            ON A.CID = B.cid AND B.cid = {2}) AS CalcTable
                                ", Character_Define.Character_Stat_TableName, Character_Define.CharacterTableName, CID);
            PvP_WarPoint retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<PvP_WarPoint>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new PvP_WarPoint();
            return retObj;
        }

        private static string GetCharacterStatRedisKey(long CID)
        {
            return string.Format("{0}_{1}_{2}", Character_Define.CharacterPrefix, Character_Define.Character_Stat_TableName, CID);
        }

        public static Character_Stat GetCharacterStat(ref TxnBlock TB, long CID, bool Flush = false, string dbkey = GoldExpedition_Define.GoldExpedition_Info_DB)
        {
            string setKey = GetCharacterStatRedisKey(CID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE CID = {1}", Character_Define.Character_Stat_TableName, CID);
            Character_Stat retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis<Character_Stat>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new Character_Stat();
            return retObj;
        }
        public static void RemoveCacheCharacterStat(long CID)
        {
            string setKey = GetCharacterStatRedisKey(CID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static Result_Define.eResult UpdateCharacterStat(ref TxnBlock TB, long CID, ref Character_Stat setStat, string dbkey = Character_Define.CharacterDBName, bool Flush = false)
        {
            if (setStat.MAX_WAR_POINT > setStat.WAR_POINT * 2)
                setStat.MAX_WAR_POINT = setStat.WAR_POINT;

            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON CID = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                BaseHP = {2},
	                                                BaseMP = {3},
	                                                HP = {4},
	                                                HPMax = {5},
	                                                MP = {6},
	                                                MPMax = {7},
	                                                ATTACK_MIN = {8},
	                                                ATTACK_MAX = {9},
	                                                EXP = {10},
	                                                LEVEL = {11},
	                                                DEFENCE_POWER = {12},
	                                                CPR = {13},
	                                                CRITICAL = {14},
	                                                CRITICAL_PROTECTION = {15},
	                                                CRITICAL_RATING = {16},
	                                                MAX_CRITICAL_PROP = {17},
	                                                DEFENCE_CRITICAL_RATING = {18},
	                                                DEFENCE_POINT = {19},
	                                                WAR_POINT = {20},
	                                                ACTIVE_SOUL_WAR_POINT = {21},
	                                                PASSIVE_SOUL_WAR_POINT = {22},
	                                                MAX_WAR_POINT = CASE WHEN {23} > MAX_WAR_POINT THEN {23} ELSE MAX_WAR_POINT END
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ({1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9},
   	                                                 {10}, {11}, {12}, {13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, {21}, {22}, {23});
                                            ", Character_Define.Character_Stat_TableName, CID,
                                                setStat.BaseHP,
                                                setStat.BaseMP,
                                                setStat.HP,
                                                setStat.HPMax,
                                                setStat.MP,
                                                setStat.MPMax,
                                                setStat.ATTACK_MIN,
                                                setStat.ATTACK_MAX,
                                                setStat.EXP,
                                                setStat.LEVEL,
                                                setStat.DEFENCE_POWER,
                                                setStat.CPR,
                                                setStat.CRITICAL,
                                                setStat.CRITICAL_PROTECTION,
                                                setStat.CRITICAL_RATING,
                                                setStat.MAX_CRITICAL_PROP,
                                                setStat.DEFENCE_CRITICAL_RATING,
                                                setStat.DEFENCE_POINT,
                                                setStat.WAR_POINT,
                                                setStat.ACTIVE_SOUL_WAR_POINT,
                                                setStat.PASSIVE_SOUL_WAR_POINT,
                                                setStat.MAX_WAR_POINT
                                             );
            RemoveCacheCharacterStat(CID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }
    }
}
