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
    public static partial class SoulManager
    {
        // update change level , soulexp, stateflag, delflag
        public static Result_Define.eResult UpdatePassiveSoulInfo(ref TxnBlock TB, User_PassiveSoul setSoul, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET 
                                                    soulid = {1},
                                                    soulgroup = {2},
                                                    level = {3},
                                                    stateflag = '{4}',
                                                    delflag = '{5}'
                                            WHERE soulseq = {6} AND aid = {7} AND cid = {8}
                                            ",
                                             Soul_Define.User_PassiveSoul_Table,
                                             setSoul.soulid,
                                             setSoul.soulgroup,
                                             setSoul.level,
                                             setSoul.stateflag,
                                             setSoul.delflag,
                                             setSoul.soulseq,
                                             setSoul.aid,
                                             setSoul.cid
                );
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static int GetMakePassiveSoul_LimitCount(ref TxnBlock TB)
        {
            return SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_LIMIT_PASSIVE_SOUL_COST_RUBY]);
        }

        public static int GetMakePassiveSoul_NeedStoneCount(ref TxnBlock TB)
        {
            return SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_PASSIVE_SOUL_COST_STONE]);
        }
        public static int GetMakePassiveSoul_NeedRubyCount(ref TxnBlock TB)
        {
            return SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_PASSIVE_SOUL_COST_RUBY]);
        }

        //public static int GetPassive_Soul_StorageCount(ref TxnBlock TB)
        //{
        //    // not use yet
        //    return SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_PASSIVE_SOUL_BASIC_STORAGE]);
        //}

        public static Result_Define.eResult MakePassiveSoul(ref TxnBlock TB, long AID, long CID, int tryCount, ref List<User_PassiveSoul> retMakeSoul, ref int rubyMakeCount, string dbkey = Soul_Define.Soul_InvenDB)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            Account UserInfo = AccountManager.GetAccountData(ref TB, AID, ref retError);

            User_Soul_Make_Info makeCountInfo = GetUserSoulMakeInfo(ref TB, AID, true);
            int needStone = GetMakePassiveSoul_NeedStoneCount(ref TB);
            int rubyBuyCount = 0;
            int totalmakeCount = 0;
            int LimitCount = GetMakePassiveSoul_LimitCount(ref TB);
            List<System_Soul_Passive_Prob> passiveAllProbList = GetSoul_System_Soul_Passive_Prob_All(ref TB);
            double Max = passiveAllProbList.Sum(item => item.PROB);

            for (int makeCount = 0; makeCount < tryCount; makeCount++)
            {
                totalmakeCount++; 
                if (UserInfo.Stone >= needStone && retError == Result_Define.eResult.SUCCESS)
                {
                    retError = AccountManager.UseUserSoulStone(ref TB, AID, needStone);
                    UserInfo.Stone -= needStone;
                }
                else
                {
                    if (makeCountInfo.Passive_Make_Count + rubyBuyCount >= LimitCount)
                        return Result_Define.eResult.SOUL_PASSIVE_CREATE_LIMIT;

                    retError = AccountManager.UseUserCash(ref TB, AID, GetMakePassiveSoul_NeedRubyCount(ref TB));
                    rubyBuyCount++;
                }

                long makePassiveSoulID = 0;
                if (retError == Result_Define.eResult.SUCCESS && makeCountInfo.Total_Passive_Make_Count > 1)
                {                    
                    double curRate = TheSoul.DataManager.Math.GetRandomDouble(0, Max);
                    double checkRate = 0.0f;

                    System_Soul_Passive_Prob retDropGroup = new System_Soul_Passive_Prob();

                    foreach (System_Soul_Passive_Prob setDropGroup in passiveAllProbList)
                    {
                        if (setDropGroup.PROB > 0)
                        {
                            checkRate += setDropGroup.PROB;
                            if (checkRate >= curRate)
                            {
                                retDropGroup = setDropGroup;
                                break;
                            }
                        }
                    }

                    if (retDropGroup.PassiveID < 1)
                        return Result_Define.eResult.SOUL_SYSTEM_INFO_NOT_FOUND;

                    makePassiveSoulID = retDropGroup.PassiveID;
                }
                else if( makeCountInfo.Total_Passive_Make_Count == 0)
                    makePassiveSoulID = SystemData.GetConstValueLong(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_PASSIVE_SOUL_FIRST_GET_ID]);
                else
                    makePassiveSoulID = SystemData.GetConstValueLong(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_PASSIVE_SOUL_SECOND_GET_ID]);

                if (retError == Result_Define.eResult.SUCCESS && makePassiveSoulID > 0)
                {
                    User_PassiveSoul makeSoul = new User_PassiveSoul();
                    retError = MakePassiveSoulToDB(ref TB, makePassiveSoulID, AID, CID, ref makeSoul, dbkey);
                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        retMakeSoul.Add(makeSoul);
                        makeCountInfo.Total_Passive_Make_Count++;
                    }
                }
            }

            if (retError == Result_Define.eResult.SUCCESS && totalmakeCount > 0)
            {
                rubyMakeCount = makeCountInfo.Passive_Make_Count + rubyBuyCount;
                retError = UpdateUserSoulMakeInfo(ref TB, AID, totalmakeCount, rubyBuyCount);
                if (retError == Result_Define.eResult.SUCCESS)
                    GetUserSoulMakeInfo(ref TB, AID, true);
            }

            if (retError == Result_Define.eResult.SUCCESS)
                retError = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.PASSIVESOUL_ACTION, (int)Trigger_Define.ePassiveSoulUseType.Create);

            return retError;
        }

        public static Result_Define.eResult MakePassiveSoulID(ref TxnBlock TB, long PassiveID, long AID, long CID)
        {
            User_PassiveSoul retObj = new User_PassiveSoul();
            Result_Define.eResult retError = MakePassiveSoulToDB(ref TB, PassiveID, AID, CID, ref retObj);
            if (retError == Result_Define.eResult.SUCCESS)
                RemoveCacheUser_PassiveSoul(AID, CID);
            return retError;
        }
        
        private static Result_Define.eResult MakePassiveSoulToDB(ref TxnBlock TB, long PassiveID, long AID, long CID, ref User_PassiveSoul retMakeSoul, string dbkey = Soul_Define.Soul_InvenDB)
        {
            System_Soul_Passive makeInfo = GetSoul_System_Soul_Passive(ref TB, PassiveID);
            if (makeInfo.Soul_PassiveID < 1)
                return Result_Define.eResult.SOUL_SYSTEM_INFO_NOT_FOUND;

            Character charInfo = CharacterManager.GetCharacter(ref TB, AID, CID);

            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "User_Insert_PassiveSoul";
            Cmd.Parameters.Add("@aid", SqlDbType.BigInt).Value = AID;
            Cmd.Parameters.Add("@cid", SqlDbType.BigInt).Value = CID;
            Cmd.Parameters.Add("@soulid", SqlDbType.BigInt).Value = makeInfo.Soul_PassiveID;
            Cmd.Parameters.Add("@soulgroup", SqlDbType.BigInt).Value = makeInfo.GroupIndex;
            Cmd.Parameters.Add("@class_type", SqlDbType.TinyInt).Value = charInfo.Class;
            Cmd.Parameters.Add("@stateflag", SqlDbType.Char).Value = Soul_Define.PassiveSoulStates[Soul_Define.ePassiveSoulStates.Store];
            Cmd.Parameters.Add("@delflag", SqlDbType.Char).Value = "N";
            var result = new SqlParameter("@ret_result", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            Cmd.Parameters.Add(result);

            SqlDataReader getDr = null;
            TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd, ref getDr);

            if (getDr != null)
            {
                long checkValue = System.Convert.ToInt64(result.Value);

                if (checkValue < 0)
                {
                    getDr.Dispose(); getDr.Close();
                    Cmd.Dispose();
                    checkValue = checkValue * -1;       // re delcare result code 
                    if (checkValue == (long)Result_Define.eResult.SOUL_PASSIVE_ALREADY_CREATED)
                        return Result_Define.eResult.SOUL_PASSIVE_ALREADY_CREATED;
                    else
                        return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                }
                else
                {
                    var r = SQLtoJson.Serialize(ref getDr);
                    string json = mJsonSerializer.ToJsonString(r);

                    getDr.Dispose(); getDr.Close();
                    Cmd.Dispose();
                    retMakeSoul = mJsonSerializer.JsonToObject<User_PassiveSoul[]>(json).FirstOrDefault();
                    if (retMakeSoul.soulseq < 1)
                        return Result_Define.eResult.SOUL_ID_NOT_FOUND;
                }
            }
            else
            {
                Cmd.Dispose();
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }

            RemoveCacheUser_PassiveSoul(AID, CID);
            return Result_Define.eResult.SUCCESS;
        }


        // update passive soul make count
        public static Result_Define.eResult UpdateUserSoulMakeInfo(ref TxnBlock TB, long AID, int MakeCount, int RubyBuyCount, string dbkey = Soul_Define.Soul_InvenDB)
        {
            SqlCommand commandUpdate_User_SoulMakeInfo = new SqlCommand();
            commandUpdate_User_SoulMakeInfo.CommandText = "System_Update_User_Soul_Make";
            var result = new SqlParameter("@ret_result", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            commandUpdate_User_SoulMakeInfo.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            commandUpdate_User_SoulMakeInfo.Parameters.Add("@MAKE_COUNT", SqlDbType.Int).Value = MakeCount;
            commandUpdate_User_SoulMakeInfo.Parameters.Add("@BUY_COUNT", SqlDbType.Int).Value = RubyBuyCount;
            commandUpdate_User_SoulMakeInfo.Parameters.Add(result);

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUpdate_User_SoulMakeInfo))
            {
                if (System.Convert.ToInt64(result.Value) < 0)
                    //long dbresult = System.Convert.ToInt64(result.Value) * -1;       // re delcare result code 
                    retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                else
                    retError = Result_Define.eResult.SUCCESS;
            }
            else
                retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            commandUpdate_User_SoulMakeInfo.Dispose();
            return retError;
        }

        public static Result_Define.eResult ExtractPassiveSoul(ref TxnBlock TB, long AID, long CID, List<long> SoulSeqList, ref int GetExp, ref Character retCharacter, string dbkey = Soul_Define.Soul_InvenDB)
        {
            List<User_PassiveSoul> getPassiveSoulList = SoulManager.GetUser_PassiveSoul(ref TB, AID, CID);
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            foreach (long SoulSeq in SoulSeqList)
            {
                var findSoul = getPassiveSoulList.Find(item => item.soulseq == SoulSeq);
                if (findSoul == null)
                    return Result_Define.eResult.SOUL_ID_NOT_FOUND;

                System_Soul_Passive makeInfo = GetSoul_System_Soul_Passive(ref TB, findSoul.soulid);
                if (makeInfo.Soul_PassiveID < 1)
                    return Result_Define.eResult.SOUL_SYSTEM_INFO_NOT_FOUND;

                retError = CharacterManager.UpdateCharacterPassiveSoulExp(ref TB, AID, CID, makeInfo.Material_EXP);

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    findSoul.stateflag = Soul_Define.PassiveSoulStates[Soul_Define.ePassiveSoulStates.Deleted];
                    findSoul.delflag = "Y";
                    retError = UpdatePassiveSoulInfo(ref TB, findSoul);
                    GetExp += makeInfo.Material_EXP;
                }
                else
                    return retError;
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                RemoveCacheUser_PassiveSoul(AID, CID);
                retCharacter = CharacterManager.FlushCharacter(ref TB, AID, CID);
            }

            if (retError == Result_Define.eResult.SUCCESS)
                retError = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.PASSIVESOUL_ACTION, (int)Trigger_Define.ePassiveSoulUseType.Extract, 0, SoulSeqList.Count);

            return Result_Define.eResult.SUCCESS;
        }

        public static Result_Define.eResult KeepStoragePassiveSoul(ref TxnBlock TB, long AID, long CID, long SoulSeq, string dbkey = Soul_Define.Soul_InvenDB)
        {
            List<User_PassiveSoul> getPassiveSoulList = SoulManager.GetUser_PassiveSoul(ref TB, AID, CID);
            List<User_Character_Equip_Soul> getEquipList = SoulManager.GetUser_Character_Equip_Soul(ref TB, AID, CID);
            
            if (!VipManager.CheckVIPCountOver(ref TB, AID, CID, VIP_Define.eVipType.BAGSLOT_MAX_PASSIVESOUL))
                return Result_Define.eResult.SOUL_PASSIVE_STORAGE_MAX;

            var findSoul = getPassiveSoulList.Find(item => item.soulseq == SoulSeq);
            var findEquip = getEquipList.Find(equip => equip.soul_type == Soul_Define.Equip_Soul_Type_Passive && equip.soulseq == findSoul.soulseq);
            short setSlot = (short)(findEquip != null ? findEquip.slot_num : 0);

            if (findSoul == null)
                return Result_Define.eResult.SOUL_ID_NOT_FOUND;

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            // unequip passive soul
            if (setSlot > 0 && findSoul.stateflag.Equals(Soul_Define.PassiveSoulStates[Soul_Define.ePassiveSoulStates.Equip]))
            {
                User_Character_Equip_Soul updateEquipSoul = new User_Character_Equip_Soul();
                retError = SoulManager.UpdateUserSoulEquip(ref TB, AID, CID, 0, Soul_Define.Equip_Soul_Type_Passive, findSoul.class_type, setSlot, ref updateEquipSoul);
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                findSoul.stateflag = Soul_Define.PassiveSoulStates[Soul_Define.ePassiveSoulStates.Store];
                findSoul.delflag = "N";
                retError = UpdatePassiveSoulInfo(ref TB, findSoul);
            }

            if (retError == Result_Define.eResult.SUCCESS)
                RemoveCacheUser_PassiveSoul(AID, CID);

            return Result_Define.eResult.SUCCESS;
        }

        // upgrade soul level
        public static Result_Define.eResult PassiveSoulLevelUp(ref TxnBlock TB, long AID, long CID, long SoulSeq, ref int UseExp, ref Character retCharacter, byte levelUpCount, string dbkey = Soul_Define.Soul_InvenDB)
        {
            List<User_PassiveSoul> getPassiveSoulList = SoulManager.GetUser_PassiveSoul(ref TB, AID, CID);
            var findSoul = getPassiveSoulList.Find(soul => soul.soulseq == SoulSeq);

            if (findSoul == null)
                return Result_Define.eResult.SOUL_ID_NOT_FOUND;

            int needExp = 0;
            for (byte checkCount = 0; checkCount < levelUpCount; checkCount++)
            {
                System_Soul_Passive baseInfo = GetSoul_System_Soul_Passive(ref TB, findSoul.soulid);
                if (baseInfo.LevelUp_EXP == 0)
                {
                    levelUpCount = checkCount;
                    break;
                }

                System_Soul_Passive nextInfo = GetSoul_System_Soul_Passive(ref TB, baseInfo.Grade, baseInfo.GroupIndex, (baseInfo.Level + 1));

                if (nextInfo.Soul_PassiveID < 1)
                    return Result_Define.eResult.SOUL_SYSTEM_INFO_NOT_FOUND;

                findSoul.soulid = nextInfo.Soul_PassiveID;
                findSoul.level = nextInfo.Level;
                needExp += baseInfo.LevelUp_EXP;

                if (retCharacter.passivesoulexp < needExp)
                    return Result_Define.eResult.SOUL_PASSIVE_NOT_ENOUGH_EXP;
            }

            //if(needExp == 0)
            //    return Result_Define.eResult.SOUL_ENHANCE_LEVEL_MAX;

            Result_Define.eResult retError = needExp > 0 ? CharacterManager.UseCharacterPassiveSoulExp(ref TB, AID, CID, needExp) : Result_Define.eResult.SUCCESS;

            if (retError == Result_Define.eResult.SUCCESS && needExp > 0)
            {
                retError = UpdatePassiveSoulInfo(ref TB, findSoul);
            }

            if (retError == Result_Define.eResult.SUCCESS && needExp > 0)
            {
                RemoveCacheUser_PassiveSoul(AID, CID);
                retCharacter = CharacterManager.FlushCharacter(ref TB, AID, CID);
                UseExp = needExp;
            }

            if (retError == Result_Define.eResult.SUCCESS && needExp > 0)
                retError = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.PASSIVESOUL_ACTION, (int)Trigger_Define.ePassiveSoulUseType.LevelUp, 0, levelUpCount);

            return retError;
        }

        public static User_Soul_Passive_Store_Count GetUserPsssiveSoulStoreCount(ref TxnBlock TB, long AID, long CID, Soul_Define.ePassiveSoulStates getStateType = Soul_Define.ePassiveSoulStates.Store, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setQuery = string.Format(@"SELECT COUNT(*) as Passive_Store_Count FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND cid = {2} AND delflag = 'N' AND stateflag = '{3}'", Soul_Define.User_PassiveSoul_Table, AID, CID, Soul_Define.PassiveSoulStates[getStateType]);
            User_Soul_Passive_Store_Count retObj = GenericFetch.FetchFromDB<User_Soul_Passive_Store_Count>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new User_Soul_Passive_Store_Count();
        }
    }
}
