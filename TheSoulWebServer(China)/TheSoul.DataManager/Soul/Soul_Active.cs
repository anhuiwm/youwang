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
        // update change itemid , grade, level
        public static Result_Define.eResult UpdateActiveSoulInfo(ref TxnBlock TB, User_ActiveSoul setSoul, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET 
                                                    soulid = {1},
                                                    soulgroup = {2},
                                                    starlevel = {3},
                                                    grade = {4},
                                                    level = {5},
                                                    soulparts_ea = {6}
                                            WHERE soulseq = {7} AND aid = {8}
                                            ",
                                             Soul_Define.User_ActiveSoul_Table,
                                             setSoul.soulid,
                                             setSoul.soulgroup,
                                             setSoul.starlevel,
                                             setSoul.grade,
                                             setSoul.level,
                                             setSoul.soulparts_ea,
                                             setSoul.soulseq,
                                             setSoul.aid
                );
            RemoveCacheUser_ActiveSoul(setSoul.aid);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }
        
        public static Result_Define.eResult MakeSoulParts(ref TxnBlock TB, long Class_IndexID, long AID, int makeCount, int maxStack, string dbkey = Soul_Define.Soul_InvenDB)
        {
            System_Soul_Parts partsInfo = GetSoul_System_Soul_Parts(ref TB, Class_IndexID);
            System_Soul_Active makeInfo = GetSoul_System_Soul_Active(ref TB, partsInfo.Soul_Group, Soul_Define.Soul_Base_Grade);

            if (makeInfo.SoulID > 0)
            {
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandText = "User_Insert_ActiveSoul_Parts";
                Cmd.Parameters.Add("@make_count", SqlDbType.Int).Value = makeCount;
                Cmd.Parameters.Add("@aid", SqlDbType.BigInt).Value = AID;
                Cmd.Parameters.Add("@soulid", SqlDbType.BigInt).Value = makeInfo.SoulID;
                Cmd.Parameters.Add("@soulgroup", SqlDbType.BigInt).Value = makeInfo.SoulGroup;
                Cmd.Parameters.Add("@delflag", SqlDbType.Char).Value = "N";
                Cmd.Parameters.Add("@StackMAX", SqlDbType.Int).Value = maxStack;
                var result = new SqlParameter("@ret_result", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
                Cmd.Parameters.Add(result);

                SqlDataReader getDr = null;
                TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd, ref getDr);

                if (getDr != null)
                {
                    int checkValue = System.Convert.ToInt32(result.Value);

                    if (checkValue < 0)
                    {
                        getDr.Dispose(); getDr.Close();
                        Cmd.Dispose();

                        return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                    }

                    var r = SQLtoJson.Serialize(ref getDr);
                    string json = mJsonSerializer.ToJsonString(r);

                    getDr.Dispose(); getDr.Close();
                    Cmd.Dispose();
                    User_ActiveSoul MakeSoul = mJsonSerializer.JsonToObject<User_ActiveSoul[]>(json).FirstOrDefault();
                    if (MakeSoul.soulseq < 1)
                        return Result_Define.eResult.SOUL_ID_NOT_FOUND;
                }
                else
                {
                    Cmd.Dispose();
                    return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                }

                RemoveCacheUser_ActiveSoul(AID);

                return TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.Soul_Piece_Acquire, Class_IndexID, 0, makeCount);
            }
            else
                return Result_Define.eResult.SUCCESS;
        }

        public static Result_Define.eResult SoulActivation(ref TxnBlock TB, long AID, long soulseq, string dbkey = Soul_Define.Soul_InvenDB)
        {
            List<User_ActiveSoul> userActiveSoul = GetUser_ActiveSoul(ref TB, AID, true);

            var findSoul = userActiveSoul.Find(item => item.soulseq == soulseq);
            if (findSoul == null)
                return Result_Define.eResult.SOUL_ID_NOT_FOUND;

            if (findSoul.grade > 0 && findSoul.level > 0 && findSoul.starlevel > 0)
                return Result_Define.eResult.SOUL_ALREADY_ACTIVATE;

            System_Soul_Parts partsInfo = GetSoul_System_Soul_Parts_By_Soul_Group(ref TB, findSoul.soulgroup);
            if (partsInfo.SoulPartsIndex < 0)
                return Result_Define.eResult.SOUL_SYSTEM_INFO_NOT_FOUND;

            if (partsInfo.SoulPartsValue > findSoul.soulparts_ea)
                return Result_Define.eResult.SOUL_PARTS_NOT_ENOUGH;

            findSoul.grade = Soul_Define.Soul_Base_Grade;
            findSoul.level = Soul_Define.Soul_Base_Level;
            findSoul.starlevel = partsInfo.Create_Star_Level;
            findSoul.soulparts_ea -= partsInfo.SoulPartsValue;

            Result_Define.eResult retError = UpdateActiveSoulInfo(ref TB, findSoul);
            if (retError == Result_Define.eResult.SUCCESS)
                RemoveCacheUser_ActiveSoul(AID);

            if (retError == Result_Define.eResult.SUCCESS)
                retError = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.Soul_Acquire);

            return retError;
        }

        public static Result_Define.eResult MakeSoulEquip(ref TxnBlock TB, long Class_IndexID, long AID, int makeCount, int maxStack, string dbkey = Soul_Define.Soul_InvenDB)
        {
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "User_Insert_Soul_Equip";
            Cmd.Parameters.Add("@make_count", SqlDbType.Int).Value = makeCount;
            Cmd.Parameters.Add("@aid", SqlDbType.BigInt).Value = AID;
            Cmd.Parameters.Add("@soul_equip_id", SqlDbType.BigInt).Value = Class_IndexID;
            Cmd.Parameters.Add("@StackMAX", SqlDbType.Int).Value = maxStack;
            var result = new SqlParameter("@ret_result", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            Cmd.Parameters.Add(result);

            SqlDataReader getDr = null;
            TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd, ref getDr);

            if (getDr != null)
            {
                int checkValue = System.Convert.ToInt32(result.Value);

                if (checkValue < 0)
                {
                    getDr.Dispose(); getDr.Close();
                    Cmd.Dispose();
                    return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                }

                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);

                getDr.Dispose(); getDr.Close();
                Cmd.Dispose();
                User_Soul_Equip_Inven MakeSoul = mJsonSerializer.JsonToObject<User_Soul_Equip_Inven[]>(json).FirstOrDefault();
                if (MakeSoul.equipinvenseq < 1)
                    return Result_Define.eResult.SOUL_EQUIP_ID_INVALIDE;
            }
            else
            {
                Cmd.Dispose();
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }

            RemoveCacheUser_Soul_Equip_Inven(AID);
            return Result_Define.eResult.SUCCESS;
        }

        public static Result_Define.eResult UpdateSoulEquipToSoul(ref TxnBlock TB, long AID, long SoulSeq, long EquipSeq, long SoulEquipID, ref User_ActiveSoul_Equip MakeSoulEquipInfo, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setQuery = string.Format(@"INSERT INTO {0} ( aid, soulseq, soul_equip_id, delflag ) VALUES (
                                                                {1}, {2}, {3}, '{4}'
                                                                )"
                                            , Soul_Define.User_ActiveSoul_Equip_Table
                                            , AID, SoulSeq, SoulEquipID, "N"
                                            );
            
            Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            if (retError == Result_Define.eResult.SUCCESS)
            {
                RemoveCacheUser_Soul_Equip_Inven(AID);
                RemoveCacheUser_ActiveSoul_Equip(AID);
            }
            //SqlCommand Cmd = new SqlCommand();
            //Cmd.CommandText = "User_Update_ActiveSoul_Equip";
            //Cmd.Parameters.Add("@aid", SqlDbType.BigInt).Value = AID;
            //Cmd.Parameters.Add("@soulseq", SqlDbType.BigInt).Value = SoulSeq;
            //Cmd.Parameters.Add("@equipinvenseq", SqlDbType.BigInt).Value = EquipSeq;
            //Cmd.Parameters.Add("@soul_equip_id", SqlDbType.BigInt).Value = SoulEquipID;
            //var result = new SqlParameter("@ret_result", SqlDbType.BigInt) { Direction = ParameterDirection.Output };
            //Cmd.Parameters.Add(result);

            //SqlDataReader getDr = null;
            //TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd, ref getDr);

            //if (getDr != null)
            //{
            //    int checkValue = System.Convert.ToInt32(result.Value);

            //    if (checkValue < 0)
            //    {
            //        getDr.Dispose(); getDr.Close();
            //        Cmd.Dispose();

            //        checkValue = checkValue * -1;       // re delcare result code 
            //        if (checkValue == (long)Result_Define.eResult.SOUL_EQUIP_ID_EQUIP_FAIL)
            //            return Result_Define.eResult.SOUL_EQUIP_ID_EQUIP_FAIL;
            //        else
            //            return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            //    }

            //    var r = SQLtoJson.Serialize(ref getDr);
            //    string json = mJsonSerializer.ToJsonString(r);

            //    getDr.Dispose(); getDr.Close();
            //    Cmd.Dispose();

            //    MakeSoulEquipInfo = mJsonSerializer.JsonToObject<User_ActiveSoul_Equip[]>(json).FirstOrDefault();
            //    if (MakeSoulEquipInfo.soulequipseq < 1)
            //        return Result_Define.eResult.SOUL_EQUIP_ID_EQUIP_FAIL;
            //}
            //else
            //{
            //    Cmd.Dispose();
            //    return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            //}
            //RemoveCacheUser_Soul_Equip_Inven(AID);
            //RemoveCacheUser_ActiveSoul_Equip(AID);

            return retError;
        }

        public static List<User_Soul_Equip_Inven> GetSoul_Equip_Ret_List(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            //List<User_Soul_Equip_Inven> getActiveSoulList = SoulManager.GetUser_Soul_Equip_Inven(ref TB, AID);
            return SoulManager.GetUser_Soul_Equip_Inven(ref TB, AID, Flush);
        }


        public static Result_Define.eResult Equip_SoulEquipToActiveSoul(ref TxnBlock TB, long AID, long SoulSeq, long SoulEquipSeq, ref List<Return_DisassableSoulEquip_List> DeletedItemList, string dbkey = Soul_Define.Soul_InvenDB)
        {
            List<User_ActiveSoul> getActiveSoulList = SoulManager.GetUser_ActiveSoul_WithEquip(ref TB, AID, true);
            var findSoul = getActiveSoulList.Find(soul => soul.soulseq == SoulSeq);

            if (findSoul == null)
                return Result_Define.eResult.SOUL_ID_NOT_FOUND;

            System_Soul_Active baseInfo = GetSoul_System_Soul_Active(ref TB, findSoul.soulid);
            if (baseInfo.SoulID < 1)
                return Result_Define.eResult.SOUL_SYSTEM_INFO_NOT_FOUND;

            List<long> needEquipList = new List<long>()
            {
                baseInfo.Need_Soul_Equip_1,
                baseInfo.Need_Soul_Equip_2,
                baseInfo.Need_Soul_Equip_3,
                baseInfo.Need_Soul_Equip_4,
                baseInfo.Need_Soul_Equip_5,
                baseInfo.Need_Soul_Equip_6,
            };

            Dictionary<long, int> CheckID_Count = new Dictionary<long, int>();
            needEquipList.ForEach(equip =>
            {
                CheckID_Count[equip] = CheckID_Count.ContainsKey(equip) ? CheckID_Count[equip]+1 : 1 ;
            }
            );

            List<User_Soul_Equip_Inven> userSoulEquipList = GetUser_Soul_Equip_Inven(ref TB, AID);

            var findEquip = userSoulEquipList.Find(item => item.equipinvenseq == SoulEquipSeq);

            if (findEquip == null)
                return Result_Define.eResult.SOUL_EQUIP_ID_EQUIP_FAIL;

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            if (CheckID_Count.Keys.Contains(findEquip.soul_equip_id))
            {
                int equipCount = 0;
                findSoul.soulequiplist.ForEach(equip => {
                    if (equip.soul_equip_id == findEquip.soul_equip_id)
                        equipCount++;
                });

                int leftCount = CheckID_Count[findEquip.soul_equip_id] - equipCount;

                if (leftCount > 0)
                {
                    User_ActiveSoul_Equip retEquipInfo = new User_ActiveSoul_Equip();
                    retError = UpdateSoulEquipToSoul(ref TB, AID, SoulSeq, SoulEquipSeq, findEquip.soul_equip_id, ref retEquipInfo);
                }
                else
                    return Result_Define.eResult.SOUL_EQUIP_ID_SLOT_FULL;
            }
            else
                return Result_Define.eResult.SOUL_EQUIP_ID_INVALIDE;

            if (retError == Result_Define.eResult.SUCCESS)
                UseSoulEquip(ref TB, AID, findEquip.soul_equip_id, 1, ref DeletedItemList);

            return retError;
        }

        // upgrade soul level
        public static Result_Define.eResult ActiveSoulLevelUp(ref TxnBlock TB, long AID, long SoulSeq, byte levelUpCount = 1, string dbkey = Soul_Define.Soul_InvenDB)
        {
            List<User_ActiveSoul> getActiveSoulList = SoulManager.GetUser_ActiveSoul(ref TB, AID);
            var findSoul = getActiveSoulList.Find(soul => soul.soulseq == SoulSeq);

            if (findSoul == null)
                return Result_Define.eResult.SOUL_ID_NOT_FOUND;

            if (findSoul.level + levelUpCount > Soul_Define.Soul_Max_Level)
                return Result_Define.eResult.SOUL_ENHANCE_LEVEL_MAX;

            int CharMaxLevel = CharacterManager.GetCharacterMaxLevel_FromDB(ref TB, AID);

            if (findSoul.level + levelUpCount > CharMaxLevel)
                return Result_Define.eResult.SOUL_ENHANCE_CANT_OVER_CHAR_LEVEL;

            System_Soul_Active baseInfo = GetSoul_System_Soul_Active(ref TB, findSoul.soulid);
            if (baseInfo.SoulID < 1)
                return Result_Define.eResult.SOUL_SYSTEM_INFO_NOT_FOUND;

            System_Soul_Evolution evolutionInfo = GetSoul_System_Soul_Evolution(ref TB, baseInfo.SoulGroup, findSoul.starlevel);

            if (evolutionInfo.Soul_Evolution_ID < 1)
                return Result_Define.eResult.SOUL_ENHANCE_INFO_NOT_FOUND;

            int needGold = 0;
            for (byte checkCount = 0; checkCount < levelUpCount; checkCount++)
            {
                System_Skill_Level skillInfo = GetSoul_System_Skill_Level(ref TB, evolutionInfo.SKill_Level_Group, (byte)(findSoul.level + checkCount));

                if (skillInfo.Skill_Level_Index < 1)
                    return Result_Define.eResult.SOUL_ENHANCE_INFO_NOT_FOUND;

                needGold += skillInfo.LevelUp_Gold;
            }

            Result_Define.eResult retError = AccountManager.UseUserGold(ref TB, AID, needGold);

            if (retError == Result_Define.eResult.SUCCESS)
            {
                findSoul.level += levelUpCount;
                retError = UpdateActiveSoulInfo(ref TB, findSoul);
                if (retError == Result_Define.eResult.SUCCESS)
                    RemoveCacheUser_ActiveSoul(AID);
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Soul_LvUp, (int)Trigger_Define.eSoul_LvUpType.Level, 0, levelUpCount));
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Soul_Lv, (int)Trigger_Define.eSoul_LvUpType.Level, 0, findSoul.level));
                retError = TriggerManager.ProgressTrigger(ref TB, AID, setDataList);
            }

            return retError;
        }

        // upgrade soul grade
        public static Result_Define.eResult ActiveSoulGradeUp(ref TxnBlock TB, long AID, long SoulSeq, string dbkey = Soul_Define.Soul_InvenDB)
        {
            List<User_ActiveSoul> getActiveSoulList = SoulManager.GetUser_ActiveSoul_WithEquip(ref TB, AID);
            var findSoul = getActiveSoulList.Find(soul => soul.soulseq == SoulSeq);

            if (findSoul == null)
                return Result_Define.eResult.SOUL_ID_NOT_FOUND;

            if (findSoul.grade >= Soul_Define.Soul_Max_Grade)
                return Result_Define.eResult.SOUL_ENHANCE_GRADE_MAX;

            System_Soul_Active baseInfo = GetSoul_System_Soul_Active(ref TB, findSoul.soulid);
            if (baseInfo.SoulID < 1)
                return Result_Define.eResult.SOUL_SYSTEM_INFO_NOT_FOUND;

            List<long> needEquipList = new List<long>()
            {
                baseInfo.Need_Soul_Equip_1,
                baseInfo.Need_Soul_Equip_2,
                baseInfo.Need_Soul_Equip_3,
                baseInfo.Need_Soul_Equip_4,
                baseInfo.Need_Soul_Equip_5,
                baseInfo.Need_Soul_Equip_6,
            };

            Dictionary<long, int> CheckID_Count = new Dictionary<long, int>();
            needEquipList.ForEach(equip =>
            {
                CheckID_Count[equip] = CheckID_Count.ContainsKey(equip) ? CheckID_Count[equip] + 1 : 1;
            }
            );

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            foreach (KeyValuePair<long, int> CheckEquip in CheckID_Count)
            {
                int equipCount = 0;
                findSoul.soulequiplist.ForEach(equip =>
                {
                    if (equip.soul_equip_id == CheckEquip.Key)
                        equipCount++;
                });

                if (equipCount < CheckEquip.Value)
                {
                    retError = Result_Define.eResult.SOUL_ENHANCE_GRADE_NOT_ENOUGH_EQUIP;
                    break;
                }
            }

            if (retError == Result_Define.eResult.SUCCESS)
                retError = DeleteSoulEquipFromActiveSoul(ref TB, AID, findSoul.soulseq);
        
            if (retError == Result_Define.eResult.SUCCESS)
            {
                findSoul.grade++;
                findSoul.soulid = baseInfo.Next_Hon;
                retError = UpdateActiveSoulInfo(ref TB, findSoul);
            }

            if (retError == Result_Define.eResult.SUCCESS && baseInfo.GradeUp_Gold > 0)
                retError = AccountManager.UseUserGold(ref TB, AID, baseInfo.GradeUp_Gold);

            if (retError == Result_Define.eResult.SUCCESS)
            {
                RemoveCacheUser_ActiveSoul_Equip(AID);
                RemoveCacheUser_ActiveSoul(AID);
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Soul_LvUp, (int)Trigger_Define.eSoul_LvUpType.Grade));
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Soul_Lv, (int)Trigger_Define.eSoul_LvUpType.Grade, 0, findSoul.grade));
                retError = TriggerManager.ProgressTrigger(ref TB, AID, setDataList);
            }

            return retError;
        }

        // delete all soul equip
        private static Result_Define.eResult DeleteSoulEquipFromActiveSoul(ref TxnBlock TB, long AID, long SoulSeq, string dbkey = Soul_Define.Soul_InvenDB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET delflag = 'Y' WHERE soulseq = {1} AND aid = {2}
                                            ",
                                             Soul_Define.User_ActiveSoul_Equip_Table,
                                             SoulSeq,
                                             AID
                );
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }
        

        // upgrade soul star level
        public static Result_Define.eResult ActiveSoulStarUp(ref TxnBlock TB, long AID, long SoulSeq, string dbkey = Soul_Define.Soul_InvenDB)
        {
            List<User_ActiveSoul> getActiveSoulList = SoulManager.GetUser_ActiveSoul(ref TB, AID);
            var findSoul = getActiveSoulList.Find(soul => soul.soulseq == SoulSeq);

            if (findSoul == null)
                return Result_Define.eResult.SOUL_ID_NOT_FOUND;

            if (findSoul.starlevel >= Soul_Define.Soul_Max_Star)
                return Result_Define.eResult.SOUL_ENHANCE_STARLEVEL_MAX;

            System_Soul_Active baseInfo = GetSoul_System_Soul_Active(ref TB, findSoul.soulid);
            if (baseInfo.SoulID < 1)
                return Result_Define.eResult.SOUL_SYSTEM_INFO_NOT_FOUND;

            System_Soul_Evolution evolutionInfo = GetSoul_System_Soul_Evolution(ref TB, baseInfo.SoulGroup, findSoul.starlevel);
            if (evolutionInfo.Soul_Evolution_ID < 1)
                return Result_Define.eResult.SOUL_ENHANCE_INFO_NOT_FOUND;

            if (findSoul.soulparts_ea < evolutionInfo.Need_Soul_Parts)
                return Result_Define.eResult.SOUL_ENHANCE_STAR_UP_NOT_ENOUGH_PARTS;

            findSoul.starlevel++;
            findSoul.soulparts_ea -= evolutionInfo.Need_Soul_Parts;

            Result_Define.eResult retError = UpdateActiveSoulInfo(ref TB, findSoul);

            if (retError == Result_Define.eResult.SUCCESS)
            {
                List<User_ActiveSoul_Special_Buff> getSoulBuffList = SoulManager.GetUser_ActiveSoul_Special_Buff(ref TB, findSoul.soulseq);

                foreach (User_ActiveSoul_Special_Buff setBuff in getSoulBuffList)
                {
                    System_Soul_Skill_Group skillInfo = null;
                    System_Soul_Skill_Group nextSkillInfo = null;
                    if (setBuff.special_buffid1 > 0)
                    {
                        skillInfo = GetSoul_System_Soul_Skill_Group(ref TB, setBuff.special_buffid1);
                        nextSkillInfo = GetSoul_System_Soul_Skill_Group(ref TB, skillInfo.GroupID, findSoul.starlevel, skillInfo.Group);
                        if (nextSkillInfo.BuffID > 0) setBuff.special_buffid1 = nextSkillInfo.BuffID;
                    }
                    if (setBuff.special_buffid2 > 0)
                    {
                        skillInfo = GetSoul_System_Soul_Skill_Group(ref TB, setBuff.special_buffid2);
                        nextSkillInfo = GetSoul_System_Soul_Skill_Group(ref TB, skillInfo.GroupID, findSoul.starlevel, skillInfo.Group);
                        if (nextSkillInfo.BuffID > 0) setBuff.special_buffid2 = nextSkillInfo.BuffID;
                    }
                    if (setBuff.special_buffid3 > 0)
                    {
                        skillInfo = GetSoul_System_Soul_Skill_Group(ref TB, setBuff.special_buffid3);
                        nextSkillInfo = GetSoul_System_Soul_Skill_Group(ref TB, skillInfo.GroupID, findSoul.starlevel, skillInfo.Group);
                        if (nextSkillInfo.BuffID > 0) setBuff.special_buffid3 = nextSkillInfo.BuffID;
                    }

                    retError = UpdateSpecialBuff(ref TB, setBuff.aid, setBuff.cid, setBuff);

                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                    //else
                    //    RemoveCacheUser_ActiveSoul_Special_Buff(AID, setBuff.cid);
                }
            }

            if (retError == Result_Define.eResult.SUCCESS)
                RemoveCacheUser_ActiveSoul(AID);

            if (retError == Result_Define.eResult.SUCCESS)
            {
                List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Soul_LvUp, (int)Trigger_Define.eSoul_LvUpType.StarLevel));
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Soul_Lv, (int)Trigger_Define.eSoul_LvUpType.StarLevel, 0, findSoul.starlevel));
                retError = TriggerManager.ProgressTrigger(ref TB, AID, setDataList);
            }

            return retError; 
        }

        public static User_ActiveSoul GetDarkPassageRandomSoulSpecialBuff(ref TxnBlock TB, long AID, long CID, byte dp_soul_starlevel, ref System_Guerrilla_Soul darkPassageSoulInfo)
        {
            Character getCharInfo = CharacterManager.GetCharacter(ref TB, AID, CID);
            User_ActiveSoul setSoul = new User_ActiveSoul();
            setSoul.aid = AID;
            System_Soul_Active getSoulInfo = GetSoul_System_Soul_Active(ref TB, darkPassageSoulInfo.PC_SoulID);
            setSoul.soulid = getSoulInfo.SoulID;
            setSoul.soulgroup = getSoulInfo.SoulGroup;
            setSoul.grade = Soul_Define.Soul_Max_Grade;
            setSoul.level = Soul_Define.Soul_Max_Level;
            setSoul.starlevel = dp_soul_starlevel;

            var rnd = new Random();
            if ((Character_Define.SystemClassType)getCharInfo.Class == Character_Define.SystemClassType.Class_Warrior)
            {
                // pick special buff 1
                List<System_Soul_Skill_Group> getSkillInfoList = GetSoul_System_Soul_Skill_Group(ref TB, darkPassageSoulInfo.Buff_GroupID1_PC1, setSoul.starlevel);
                
                var pickSkill = getSkillInfoList.OrderBy(item => rnd.Next()).FirstOrDefault();
                setSoul.special_buffid1 = pickSkill != null ? pickSkill.BuffID : 0;
                // pick special buff 2
                getSkillInfoList = GetSoul_System_Soul_Skill_Group(ref TB, darkPassageSoulInfo.Buff_GroupID2_PC1, setSoul.starlevel);
                pickSkill = getSkillInfoList.OrderBy(item => rnd.Next()).FirstOrDefault();
                setSoul.special_buffid2 = pickSkill != null ? pickSkill.BuffID : 0;
                // pick special buff 3
                getSkillInfoList = GetSoul_System_Soul_Skill_Group(ref TB, darkPassageSoulInfo.Buff_GroupID3_PC1, setSoul.starlevel);
                pickSkill = getSkillInfoList.OrderBy(item => rnd.Next()).FirstOrDefault();
                setSoul.special_buffid3 = pickSkill != null ? pickSkill.BuffID : 0;
            }
            else if ((Character_Define.SystemClassType)getCharInfo.Class == Character_Define.SystemClassType.Class_Swordmaster)
            {
                // pick special buff 1
                List<System_Soul_Skill_Group> getSkillInfoList = GetSoul_System_Soul_Skill_Group(ref TB, darkPassageSoulInfo.Buff_GroupID1_PC2, setSoul.starlevel);
                var pickSkill = getSkillInfoList.OrderBy(item => rnd.Next()).FirstOrDefault();
                setSoul.special_buffid1 = pickSkill != null ? pickSkill.BuffID : 0;
                // pick special buff 2
                getSkillInfoList = GetSoul_System_Soul_Skill_Group(ref TB, darkPassageSoulInfo.Buff_GroupID2_PC2, setSoul.starlevel);
                pickSkill = getSkillInfoList.OrderBy(item => rnd.Next()).FirstOrDefault();
                setSoul.special_buffid2 = pickSkill != null ? pickSkill.BuffID : 0;
                // pick special buff 3
                getSkillInfoList = GetSoul_System_Soul_Skill_Group(ref TB, darkPassageSoulInfo.Buff_GroupID3_PC2, setSoul.starlevel);
                pickSkill = getSkillInfoList.OrderBy(item => rnd.Next()).FirstOrDefault();
                setSoul.special_buffid3 = pickSkill != null ? pickSkill.BuffID : 0;
            }
            else if ((Character_Define.SystemClassType)getCharInfo.Class == Character_Define.SystemClassType.Class_Taoist)
            {
                // pick special buff 1
                List<System_Soul_Skill_Group> getSkillInfoList = GetSoul_System_Soul_Skill_Group(ref TB, darkPassageSoulInfo.Buff_GroupID1_PC3, setSoul.starlevel);
                var pickSkill = getSkillInfoList.OrderBy(item => rnd.Next()).FirstOrDefault();
                setSoul.special_buffid1 = pickSkill != null ? pickSkill.BuffID : 0;
                // pick special buff 2
                getSkillInfoList = GetSoul_System_Soul_Skill_Group(ref TB, darkPassageSoulInfo.Buff_GroupID2_PC3, setSoul.starlevel);
                pickSkill = getSkillInfoList.OrderBy(item => rnd.Next()).FirstOrDefault();
                setSoul.special_buffid2 = pickSkill != null ? pickSkill.BuffID : 0;
                // pick special buff 3
                getSkillInfoList = GetSoul_System_Soul_Skill_Group(ref TB, darkPassageSoulInfo.Buff_GroupID3_PC3, setSoul.starlevel);
                pickSkill = getSkillInfoList.OrderBy(item => rnd.Next()).FirstOrDefault();
                setSoul.special_buffid3 = pickSkill != null ? pickSkill.BuffID : 0;
            } 
            return setSoul;
        }

        public static Result_Define.eResult UseSoulEquip(ref TxnBlock TB, long AID, long SoulEquipID, int UseCount, ref List<Return_DisassableSoulEquip_List> DeletedItemList)
        {
            if (UseCount < 1)
                return Result_Define.eResult.SUCCESS;

            List<User_Soul_Equip_Inven> getSoulEquipInven = GetUser_Soul_Equip_Inven(ref TB, AID);

            int CurrentCount = 0;
            List<long> targetInvenSeq = new List<long>();

            getSoulEquipInven.Where(item => item.soul_equip_id == SoulEquipID).ToList().ForEach(equip =>
            {
                if (CurrentCount < UseCount)
                {
                    CurrentCount += equip.soul_equip_ea;
                    targetInvenSeq.Add(equip.equipinvenseq);
                }
            }
            );

            if (CurrentCount < UseCount)
                return Result_Define.eResult.SOUL_NOT_ENOUGH_SOUL_EQUIP;

            string setQuery = "";
            foreach (long updateSeq in targetInvenSeq)
            {
                var findItem = getSoulEquipInven.Find(item => item.equipinvenseq == updateSeq);
                int setea = findItem.soul_equip_ea;
                if (setea > UseCount)
                {
                    setea = findItem.soul_equip_ea - UseCount;
                    UseCount = 0;
                }
                else
                {
                    setea = 0;
                    UseCount -= findItem.soul_equip_ea;
                }
                string delflag = setea > 0 ? "N" : "Y";

                setQuery = setQuery + string.Format("UPDATE {0} SET soul_equip_ea = {1}, delflag = '{2}' WHERE AID = {3} AND equipinvenseq = {4};",
                                                        Soul_Define.User_Soul_Equip_Inven_Table, setea, delflag, AID, updateSeq);
                Return_DisassableSoulEquip_List retItem = new Return_DisassableSoulEquip_List();
                retItem.equipinvenseq = findItem.equipinvenseq;
                retItem.soul_equip_id = findItem.soul_equip_id;
                retItem.soul_equip_ea = setea;
                DeletedItemList.Add(retItem);
            }

            Result_Define.eResult retError = TB.ExcuteSqlCommand(Item_Define.Item_InvenDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            if (retError == Result_Define.eResult.SUCCESS)
                RemoveCacheUser_Soul_Equip_Inven(AID);

            return retError;
        }

        public static Result_Define.eResult CraftSoulEquip(ref TxnBlock TB, long AID, long CraftItemID, ref List<User_Inven> makeItem, ref List<Return_DisassableSoulEquip_List> DeletedItemList, string dbkey = Soul_Define.Soul_InvenDB)
        {
            System_Soul_Craft craftInfo = GetSoul_System_Soul_Craft(ref TB, CraftItemID);

            Dictionary<long, int> needMaterialList = new Dictionary<long, int>();

            if (craftInfo.Material1_Index > 0) needMaterialList[craftInfo.Material1_Index] = needMaterialList.ContainsKey(craftInfo.Material1_Index) ? needMaterialList[craftInfo.Material1_Index] + craftInfo.Material1_Value : craftInfo.Material1_Value;
            if (craftInfo.Material2_Index > 0) needMaterialList[craftInfo.Material2_Index] = needMaterialList.ContainsKey(craftInfo.Material2_Index) ? needMaterialList[craftInfo.Material2_Index] + craftInfo.Material2_Value : craftInfo.Material2_Value;
            if (craftInfo.Material3_Index > 0) needMaterialList[craftInfo.Material3_Index] = needMaterialList.ContainsKey(craftInfo.Material3_Index) ? needMaterialList[craftInfo.Material3_Index] + craftInfo.Material3_Value : craftInfo.Material3_Value;
            if (craftInfo.Material4_Index > 0) needMaterialList[craftInfo.Material4_Index] = needMaterialList.ContainsKey(craftInfo.Material4_Index) ? needMaterialList[craftInfo.Material4_Index] + craftInfo.Material4_Value : craftInfo.Material4_Value;

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            foreach (KeyValuePair<long, int> checkItem in needMaterialList)
            {
                retError = UseSoulEquip(ref TB, AID, checkItem.Key, checkItem.Value, ref DeletedItemList);
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }

            if (retError == Result_Define.eResult.SUCCESS)
                retError = ItemManager.MakeItem(ref TB, ref makeItem, AID, craftInfo.Crafted_Item_Index, 1);

            if (retError == Result_Define.eResult.SUCCESS)
                retError = AccountManager.UseUserGold(ref TB, AID, craftInfo.Craft_Gold);

            if (retError == Result_Define.eResult.SUCCESS)
                RemoveCacheUser_Soul_Equip_Inven(AID);

            return retError;
        }

        public static Result_Define.eResult AutoActiveSoulGradeUp(ref TxnBlock TB, long AID, long SoulSeq, ref List<Return_DisassableSoulEquip_List> DeletedItemList, string dbkey = Soul_Define.Soul_InvenDB)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            List<User_ActiveSoul> getActiveSoulList = SoulManager.GetUser_ActiveSoul_WithEquip(ref TB, AID);
            var findSoul = getActiveSoulList.Find(soul => soul.soulseq == SoulSeq);

            if (findSoul == null)
                return Result_Define.eResult.SOUL_ID_NOT_FOUND;

            if (findSoul.grade >= Soul_Define.Soul_Max_Grade)
                return Result_Define.eResult.SOUL_ENHANCE_GRADE_MAX;

            System_Soul_Active baseInfo = GetSoul_System_Soul_Active(ref TB, findSoul.soulid);
            if (baseInfo.SoulID < 1)
                return Result_Define.eResult.SOUL_SYSTEM_INFO_NOT_FOUND;

            List<long> needEquipList = new List<long>()
            {
                baseInfo.Need_Soul_Equip_1,
                baseInfo.Need_Soul_Equip_2,
                baseInfo.Need_Soul_Equip_3,
                baseInfo.Need_Soul_Equip_4,
                baseInfo.Need_Soul_Equip_5,
                baseInfo.Need_Soul_Equip_6,
            };

            Dictionary<long, int> CheckID_Count = new Dictionary<long, int>();
            Dictionary<long, int> RequireID_Count = new Dictionary<long, int>();
            int UseGold = baseInfo.GradeUp_Gold;
            needEquipList.ForEach(equip =>
            {
                CheckID_Count[equip] = CheckID_Count.ContainsKey(equip) ? CheckID_Count[equip] + 1 : 1;
            }
            );

            foreach (KeyValuePair<long, int> CheckEquip in CheckID_Count)
            {
                int equipCount = 0;
                findSoul.soulequiplist.ForEach(equip =>
                {
                    if (equip.soul_equip_id == CheckEquip.Key)
                        equipCount++;
                });

                if (equipCount < CheckEquip.Value)
                {
                    int needCount = CheckEquip.Value - equipCount;
                    RequireID_Count[CheckEquip.Key] = CheckEquip.Value - equipCount;
                }
            }

            List<User_Soul_Equip_Inven> getSoulEquipInven = GetUser_Soul_Equip_Inven(ref TB, AID);
            Dictionary<int, CheckSoulEquipItem> setCheckList = new Dictionary<int,CheckSoulEquipItem>();
            foreach (KeyValuePair<long, int> CheckEquip in RequireID_Count)
            {
                for(int checkCount = 0 ; checkCount < CheckEquip.Value ; checkCount++)
                {
                    setCheckList.Add(setCheckList.Count+1, new CheckSoulEquipItem(CheckEquip.Key));
                    CheckSoulEquipCount(ref TB, AID, ref UseGold, ref getSoulEquipInven, ref setCheckList, dbkey);
                }
            }

            setCheckList = setCheckList.Where(soulequip => !soulequip.Value.bDeleted).ToDictionary(pair => pair.Key, pair => pair.Value);

            bool bAllEquipOwn = setCheckList.Where(soulequip => !soulequip.Value.bOwn).Count() == 0;
            
            if (!bAllEquipOwn)
                return Result_Define.eResult.SOUL_NOT_ENOUGH_SOUL_EQUIP;

            CheckID_Count.Clear();

            foreach (var equip in setCheckList.Values)
            {
                CheckID_Count[equip.SoulEquipID] = CheckID_Count.ContainsKey(equip.SoulEquipID) ? CheckID_Count[equip.SoulEquipID] + 1 : 1;
            }

            foreach (KeyValuePair<long, int> checkItem in CheckID_Count)
            {
                retError = UseSoulEquip(ref TB, AID, checkItem.Key, checkItem.Value, ref DeletedItemList);
                if (retError != Result_Define.eResult.SUCCESS)
                    break;
            }

            int checkAddLevel = 0;
            int charMaxLevel = CharacterManager.GetCharacterMaxLevel_FromDB(ref TB, AID);
            foreach (var checkSoulEquipInfo in setCheckList.Values)
            {
                System_Soul_Equip sysInfo = GetSoul_System_Soul_Equip(ref TB, checkSoulEquipInfo.SoulEquipID);
                if (sysInfo.Soul_Equip_ID > 0)
                {
                    if (sysInfo.EquipSoulLevel > findSoul.level + checkAddLevel)
                    {
                        if (charMaxLevel >= sysInfo.EquipSoulLevel)
                            checkAddLevel = sysInfo.EquipSoulLevel - findSoul.level;
                        else
                            return Result_Define.eResult.SOUL_ENHANCE_CANT_OVER_CHAR_LEVEL;
                    }
                }
                else
                    return Result_Define.eResult.SOUL_EQUIP_ID_INVALIDE;
            }
            Account userInfo = AccountManager.GetAccountData(ref TB, AID, ref retError);
            if (checkAddLevel > 0 && retError == Result_Define.eResult.SUCCESS)
            {
                System_Soul_Evolution evolutionInfo = GetSoul_System_Soul_Evolution(ref TB, baseInfo.SoulGroup, findSoul.starlevel);

                if (evolutionInfo.Soul_Evolution_ID < 1)
                    return Result_Define.eResult.SOUL_ENHANCE_INFO_NOT_FOUND;

                for (byte checkCount = 0; checkCount < checkAddLevel; checkCount++)
                {
                    System_Skill_Level skillInfo = GetSoul_System_Skill_Level(ref TB, evolutionInfo.SKill_Level_Group, (byte)(findSoul.level + checkCount));

                    if (skillInfo.Skill_Level_Index < 1)
                        return Result_Define.eResult.SOUL_ENHANCE_INFO_NOT_FOUND;

                    UseGold += skillInfo.LevelUp_Gold;
                    if (userInfo.Gold < UseGold)
                        return Result_Define.eResult.NOT_ENOUGH_GOLD;
                }
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                findSoul.level += (byte)checkAddLevel;
                findSoul.grade++;
                findSoul.soulid = baseInfo.Next_Hon;
                retError = UpdateActiveSoulInfo(ref TB, findSoul);
                if (retError == Result_Define.eResult.SUCCESS)
                    RemoveCacheUser_ActiveSoul(AID);
            }

            if (retError == Result_Define.eResult.SUCCESS)
                retError = DeleteSoulEquipFromActiveSoul(ref TB, AID, findSoul.soulseq);

            if (retError == Result_Define.eResult.SUCCESS && UseGold > 0)
                retError = AccountManager.UseUserGold(ref TB, AID, UseGold);

            if (retError == Result_Define.eResult.SUCCESS)
            {
                RemoveCacheUser_ActiveSoul_Equip(AID);
                RemoveCacheUser_ActiveSoul(AID);
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Soul_LvUp, (int)Trigger_Define.eSoul_LvUpType.Grade));
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Soul_Lv, (int)Trigger_Define.eSoul_LvUpType.Grade, 0, findSoul.grade));
                if (checkAddLevel > 0)
                {
                    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Soul_LvUp, (int)Trigger_Define.eSoul_LvUpType.Level, 0, checkAddLevel));
                    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Soul_Lv, (int)Trigger_Define.eSoul_LvUpType.Level, 0, findSoul.level));
                }
                retError = TriggerManager.ProgressTrigger(ref TB, AID, setDataList);
            }
            return retError;
        }

        public struct CheckSoulEquipItem
        {
            public long SoulEquipID;
            public bool bChecked;
            public bool bDeleted;
            public bool bOwn;

            public CheckSoulEquipItem(long setSoulEquipID = 0)
            {
                SoulEquipID = setSoulEquipID;
                bChecked = false;
                bDeleted = false;
                bOwn = false;
            }
        };

        public static void CheckSoulEquipCount(ref TxnBlock TB, long AID, ref int UseGold, ref List<User_Soul_Equip_Inven> getSoulEquipInven, ref Dictionary<int, CheckSoulEquipItem> checkSoulEquipList, string dbkey = Soul_Define.Soul_InvenDB)
        {
            List<long> targetInvenSeq = new List<long>();
            Dictionary<int, CheckSoulEquipItem> UnCheckedList = checkSoulEquipList.Where(soulitem => !soulitem.Value.bChecked).ToDictionary(pair => pair.Key, pair => pair.Value);
            bool bReCheck = false;

            foreach (KeyValuePair<int, CheckSoulEquipItem> checkSoul in UnCheckedList)
            {
                CheckSoulEquipItem setSoul = checkSoul.Value;
                var findSoulEquipIndex = getSoulEquipInven.FindIndex(item => item.soul_equip_id == checkSoul.Value.SoulEquipID && item.soul_equip_ea > 0);
                if (findSoulEquipIndex < 0)
                {
                    setSoul.bChecked = true;

                    System_Soul_Craft craftEquipInfo = GetSoul_System_Soul_Craft(ref TB, setSoul.SoulEquipID);
                    if (craftEquipInfo.Crafted_Item_Index > 0)
                    {
                        if (craftEquipInfo.Material1_Index > 0 && craftEquipInfo.Material1_Value > 0)
                        {
                            for (int addCount = 0; addCount < craftEquipInfo.Material1_Value; addCount++)
                                checkSoulEquipList.Add(checkSoulEquipList.Count + 1, new CheckSoulEquipItem(craftEquipInfo.Material1_Index));
                        }
                        if (craftEquipInfo.Material2_Index > 0 && craftEquipInfo.Material2_Value > 0)
                        {
                            for (int addCount = 0; addCount < craftEquipInfo.Material2_Value; addCount++)
                                checkSoulEquipList.Add(checkSoulEquipList.Count + 1, new CheckSoulEquipItem(craftEquipInfo.Material2_Index));
                        }
                        if (craftEquipInfo.Material3_Index > 0 && craftEquipInfo.Material3_Value > 0)
                        {
                            for (int addCount = 0; addCount < craftEquipInfo.Material3_Value; addCount++)
                                checkSoulEquipList.Add(checkSoulEquipList.Count + 1, new CheckSoulEquipItem(craftEquipInfo.Material3_Index));
                        }
                        if (craftEquipInfo.Material4_Index > 0 && craftEquipInfo.Material4_Value > 0)
                        {
                            for (int addCount = 0; addCount < craftEquipInfo.Material4_Value; addCount++)
                                checkSoulEquipList.Add(checkSoulEquipList.Count + 1, new CheckSoulEquipItem(craftEquipInfo.Material4_Index));
                        }

                        UseGold += craftEquipInfo.Craft_Gold;

                        setSoul.bDeleted = true;
                        setSoul.bOwn = true;
                        checkSoulEquipList[checkSoul.Key] = setSoul;
                        bReCheck = true;
                    }
                    else
                        checkSoulEquipList[checkSoul.Key] = setSoul;                 
                }
                else
                {
                    setSoul.bChecked = true;
                    setSoul.bOwn = true;
                    checkSoulEquipList[checkSoul.Key] = setSoul;
                    getSoulEquipInven[findSoulEquipIndex].soul_equip_ea--;
                }
            }

            if(bReCheck)
                CheckSoulEquipCount(ref TB, AID, ref UseGold, ref getSoulEquipInven, ref checkSoulEquipList, dbkey);
        }


        public static Result_Define.eResult MakeSpecialBuff(ref TxnBlock TB, long AID, long CID, long SoulSeq, ref User_ActiveSoul_Special_Buff setBuff, string dbkey = Soul_Define.Soul_InvenDB)
        {
            List<User_ActiveSoul> getActiveSoulList = SoulManager.GetUser_ActiveSoul(ref TB, AID);
            var findSoul = getActiveSoulList.Find(soul => soul.soulseq == SoulSeq);

            if (findSoul == null)
                return Result_Define.eResult.SOUL_ID_NOT_FOUND;
            
            System_Soul_Active baseInfo = GetSoul_System_Soul_Active(ref TB, findSoul.soulid);
            if (baseInfo.SoulID < 1)
                return Result_Define.eResult.SOUL_SYSTEM_INFO_NOT_FOUND;

            Character charInfo = CharacterManager.GetCharacter(ref TB, AID, CID);
            var rnd = new Random();
            bool isUpdated = false;

            DataManager_Define.eCountryCode serviceArea = SystemData.GetServiceArea(ref TB);

            // korea and china version use diff limit grade.
            byte checkNeedGrade = serviceArea == DataManager_Define.eCountryCode.China ? 
                Soul_Define.Soul_Special_Buff_Need_Grade_1_CN :
                Soul_Define.Soul_Special_Buff_Need_Grade_1_KR;

            if (findSoul.grade >= checkNeedGrade && setBuff.special_buffid1 <= 0 && (serviceArea == DataManager_Define.eCountryCode.China || serviceArea == DataManager_Define.eCountryCode.Taiwan))
            {
                long setBuffGroupID = 0;
                if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Warrior)
                    setBuffGroupID = baseInfo.Special_Buff_M_1;
                else if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Swordmaster)
                    setBuffGroupID = baseInfo.Special_Buff_M_2;
                else if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Taoist)
                    setBuffGroupID = baseInfo.Special_Buff_M_3;

                if (setBuffGroupID > 0)
                {
                    //List<System_Soul_Skill_Group> getSkillInfoList = GetSoul_System_Soul_Skill_Group(ref TB, setBuffGroupID, findSoul.starlevel);
                    //var pickSkill = getSkillInfoList.OrderBy(item => rnd.Next()).FirstOrDefault();
                    setBuff.special_buffid1 = SetRandomBuffID(ref TB, setBuffGroupID, findSoul.starlevel);
                    isUpdated = true;
                }
            }

            if (findSoul.grade >= Soul_Define.Soul_Special_Buff_Need_Grade_2 && setBuff.special_buffid2 <= 0 && (serviceArea == DataManager_Define.eCountryCode.China || serviceArea == DataManager_Define.eCountryCode.Taiwan))
            {
                long setBuffGroupID = 0;
                if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Warrior)
                    setBuffGroupID = baseInfo.Special_Buff_R_1;
                else if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Swordmaster)
                    setBuffGroupID = baseInfo.Special_Buff_R_2;
                else if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Taoist)
                    setBuffGroupID = baseInfo.Special_Buff_R_3;

                if (setBuffGroupID > 0)
                {
                    //List<System_Soul_Skill_Group> getSkillInfoList = GetSoul_System_Soul_Skill_Group(ref TB, setBuffGroupID, findSoul.starlevel);
                    //var pickSkill = getSkillInfoList.OrderBy(item => rnd.Next()).FirstOrDefault();
                    setBuff.special_buffid2 = SetRandomBuffID(ref TB, setBuffGroupID, findSoul.starlevel);
                    isUpdated = true;
                }
            }

            if (findSoul.grade >= Soul_Define.Soul_Special_Buff_Need_Grade_3 && setBuff.special_buffid3 <= 0)
            {
                long setBuffGroupID = 0;
                if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Warrior)
                    setBuffGroupID = baseInfo.Special_Buff_E_1;
                else if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Swordmaster)
                    setBuffGroupID = baseInfo.Special_Buff_E_2;
                else if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Taoist)
                    setBuffGroupID = baseInfo.Special_Buff_E_3;

                if (setBuffGroupID > 0)
                {
                    //List<System_Soul_Skill_Group> getSkillInfoList = GetSoul_System_Soul_Skill_Group(ref TB, setBuffGroupID, findSoul.starlevel);
                    //var pickSkill = getSkillInfoList.OrderBy(item => rnd.Next()).FirstOrDefault();
                    setBuff.special_buffid3 = SetRandomBuffID(ref TB, setBuffGroupID, findSoul.starlevel);
                    isUpdated = true;
                }
            }
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            if (isUpdated)
            {
                setBuff.aid = AID;
                setBuff.cid = CID;
                setBuff.soulseq = findSoul.soulseq;
                retError = UpdateSpecialBuff(ref TB, AID, CID, setBuff);
            }

            //if (retError == Result_Define.eResult.SUCCESS)
            //    RemoveCacheUser_ActiveSoul_Special_Buff(AID, CID);

            return retError;
        }

        public static long SetRandomBuffID(ref TxnBlock TB, long setBuffGroupID, byte starlevel)
        {
            var rnd = new Random();
            List<System_Soul_Skill_Group> getSkillInfoList = GetSoul_System_Soul_Skill_Group(ref TB, setBuffGroupID, starlevel);
            var pickSkill = getSkillInfoList.OrderBy(item => rnd.Next()).FirstOrDefault();
            return pickSkill.BuffID;
        }

        public static Result_Define.eResult CheckSpecialBuff(ref TxnBlock TB, ref User_ActiveSoul SoulInfo, List<User_ActiveSoul_Special_Buff> buffList)
        {
            long checkseq = SoulInfo.soulseq;
            if (checkseq > 0)
            {
                var setBuff = buffList.Find(soul => soul.soulseq == checkseq);
                if (setBuff == null)
                    setBuff = new User_ActiveSoul_Special_Buff();

                SoulInfo.special_buffid1 = setBuff.special_buffid1;
                SoulInfo.special_buffid2 = setBuff.special_buffid2;
                SoulInfo.special_buffid3 = setBuff.special_buffid3;

                return Result_Define.eResult.SUCCESS;
            }
            else
                return Result_Define.eResult.SOUL_ID_NOT_FOUND;
        }


        public static Result_Define.eResult SetSpecialBuff(ref TxnBlock TB, long AID, long CID, long SoulSeq, byte Group, byte buffpos, string dbkey = Soul_Define.Soul_InvenDB)
        {
            // china version set random buff. if Group value over by 0 then use group value for set buff.
            DataManager_Define.eCountryCode servicearea = SystemData.GetServiceArea(ref TB);

            if (servicearea == DataManager_Define.eCountryCode.China || servicearea == DataManager_Define.eCountryCode.Taiwan)
                Group = 0;

            List<User_ActiveSoul> getActiveSoulList = SoulManager.GetUser_ActiveSoul(ref TB, AID);
            var findSoul = getActiveSoulList.Find(soul => soul.soulseq == SoulSeq);

            if (findSoul == null)
                return Result_Define.eResult.SOUL_ID_NOT_FOUND;

            System_Soul_Active baseInfo = GetSoul_System_Soul_Active(ref TB, findSoul.soulid);
            if (baseInfo.SoulID < 1)
                return Result_Define.eResult.SOUL_SYSTEM_INFO_NOT_FOUND;

            List<User_ActiveSoul_Special_Buff> getSoulBuffList = SoulManager.GetUser_ActiveSoul_Special_Buff(ref TB, AID, CID, findSoul.soulseq);

            var findBuff = getSoulBuffList.Find(buff => buff.soulseq == findSoul.soulseq);
            if (findBuff == null && (servicearea == DataManager_Define.eCountryCode.China || servicearea == DataManager_Define.eCountryCode.Taiwan))
                return Result_Define.eResult.SOUL_ACTIVE_BUFF_ID_NOT_FOUND;
            else if (findBuff == null)
            {
                findBuff = new User_ActiveSoul_Special_Buff();
                findBuff.aid = AID;
                findBuff.cid = CID;
                findBuff.soulseq = findSoul.soulseq;
            }

            long setBuffID = 0;

            Character charInfo = CharacterManager.GetCharacter(ref TB, AID, CID);
            var rnd = new Random();

            // korea and china version use diff limit grade.
            byte checkNeedGrade = SystemData.GetServiceArea(ref TB) == DataManager_Define.eCountryCode.China?
                Soul_Define.Soul_Special_Buff_Need_Grade_1_CN :
                Soul_Define.Soul_Special_Buff_Need_Grade_1_KR;
            
            if (buffpos == 1 && findSoul.grade >= checkNeedGrade)
            {
                long setBuffGroupID = 0;
                if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Warrior)
                    setBuffGroupID = baseInfo.Special_Buff_M_1;
                else if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Swordmaster)
                    setBuffGroupID = baseInfo.Special_Buff_M_2;
                else if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Taoist)
                    setBuffGroupID = baseInfo.Special_Buff_M_3;

                if (setBuffGroupID > 0)
                    setBuffID = Group > 0 ? GetSoul_System_Soul_Skill_Group(ref TB, setBuffGroupID, findSoul.starlevel, Group).BuffID : SetRandomBuffID(ref TB, setBuffGroupID, findSoul.starlevel);
            }
            else if (buffpos == 2 && findSoul.grade >= Soul_Define.Soul_Special_Buff_Need_Grade_2)
            {
                long setBuffGroupID = 0;
                if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Warrior)
                    setBuffGroupID = baseInfo.Special_Buff_R_1;
                else if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Swordmaster)
                    setBuffGroupID = baseInfo.Special_Buff_R_2;
                else if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Taoist)
                    setBuffGroupID = baseInfo.Special_Buff_R_3;

                if (setBuffGroupID > 0)
                    setBuffID = Group > 0 ? GetSoul_System_Soul_Skill_Group(ref TB, setBuffGroupID, findSoul.starlevel, Group).BuffID : SetRandomBuffID(ref TB, setBuffGroupID, findSoul.starlevel);
            }
            else if (buffpos == 3 && findSoul.grade >= Soul_Define.Soul_Special_Buff_Need_Grade_3)
            {
                long setBuffGroupID = 0;
                if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Warrior)
                    setBuffGroupID = baseInfo.Special_Buff_E_1;
                else if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Swordmaster)
                    setBuffGroupID = baseInfo.Special_Buff_E_2;
                else if (charInfo.Class == (int)Character_Define.SystemClassType.Class_Taoist)
                    setBuffGroupID = baseInfo.Special_Buff_E_3;

                if (setBuffGroupID > 0)
                    setBuffID = Group > 0 ? GetSoul_System_Soul_Skill_Group(ref TB, setBuffGroupID, findSoul.starlevel, Group).BuffID : SetRandomBuffID(ref TB, setBuffGroupID, findSoul.starlevel);
            }
            else return Result_Define.eResult.SOUL_ACTIVE_BUFF_POS_INVALIDE;

            if (buffpos == 1) findBuff.special_buffid1 = setBuffID ;
            else if (buffpos == 2) findBuff.special_buffid2 = setBuffID;
            else if (buffpos == 3) findBuff.special_buffid3 = setBuffID;
            else return Result_Define.eResult.SOUL_ACTIVE_BUFF_POS_INVALIDE;

            Result_Define.eResult retError = UpdateSpecialBuff(ref TB, AID, CID, findBuff);

            int cost = GetMakeActiveSoul_SetSpecialBuffRubyCost(ref TB, buffpos);
            if (retError == Result_Define.eResult.SUCCESS && cost > 0)
                retError = AccountManager.UseUserCash(ref TB, AID, cost);

            return retError;
        }

        public static int GetMakeActiveSoul_SetSpecialBuffRubyCost(ref TxnBlock TB, byte buffPos)
        {
            int useValue = 0;
            if(buffPos == 1)
                useValue = SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_ACTIVE_SOUL_SELECT_SPECIALBUFF_MAGIC_COST_RUBY]);
            else if (buffPos == 2)
                useValue = SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_ACTIVE_SOUL_SELECT_SPECIALBUFF_RARE_COST_RUBY]);
            if(useValue < 1)
                useValue = SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_ACTIVE_SOUL_SELECT_SPECIALBUFF_COST_RUBY]);
            return useValue;
        }

        public static Result_Define.eResult UpdateSpecialBuff(ref TxnBlock TB, long AID, long CID, User_ActiveSoul_Special_Buff setBuff, string dbkey = Soul_Define.Soul_InvenDB)
        {   
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = {1} AND cid = {2} AND soulseq = {3}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                special_buffid1 = {4},
	                                                special_buffid2 = {5},
	                                                special_buffid3 = {6}
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ({1}, {2}, {3}, {4}, {5}, {6});
                                            ", Soul_Define.User_ActiveSoul_Special_Buff_Table,
                                                AID,
                                                CID,
                                                setBuff.soulseq,
                                                setBuff.special_buffid1,
                                                setBuff.special_buffid2,
                                                setBuff.special_buffid3
                                             );
            Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            if (retError == Result_Define.eResult.SUCCESS)
                RemoveCacheUser_ActiveSoul_Special_Buff(AID, CID);

            return retError;
        }
    }
}
