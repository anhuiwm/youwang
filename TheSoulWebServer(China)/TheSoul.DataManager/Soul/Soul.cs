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
        public static List<Ret_Soul_Active> GetActive_Soul_Ret_List(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            List<User_ActiveSoul> getActiveSoulList = SoulManager.GetUser_ActiveSoul(ref TB, AID, Flush);
            List<User_ActiveSoul_Equip> getSoulEquipList = SoulManager.GetUser_ActiveSoul_Equip(ref TB, AID, Flush);
            List<User_ActiveSoul_Special_Buff> getSoulBuffList = SoulManager.GetUser_ActiveSoul_Special_Buff(ref TB, AID, CID, Flush);

            return Ret_Soul_Active.makeActiveSoulRetList(ref TB, AID, CID, ref getActiveSoulList, ref getSoulEquipList, ref getSoulBuffList); //, getEquipList, CID);
        }

        public static List<Ret_Equip_Soul_Active> GetRet_Active_Soul_Equip_List(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            List<User_ActiveSoul> getActiveSoulList = SoulManager.GetUser_ActiveSoul(ref TB, AID, Flush,dbkey);
            List<User_Character_Equip_Soul> getEquipList = SoulManager.GetUser_Character_Equip_Soul(ref TB, AID, CID, Flush,dbkey);
            List<User_ActiveSoul_Equip> getSoulEquipList = SoulManager.GetUser_ActiveSoul_Equip(ref TB, AID, Flush, dbkey);
            List<User_ActiveSoul_Special_Buff> getSoulBuffList = SoulManager.GetUser_ActiveSoul_Special_Buff(ref TB, AID, CID, Flush, dbkey);

            return Ret_Equip_Soul_Active.makeActiveSoulRetEquipList(ref TB, AID, CID, ref getActiveSoulList, ref getEquipList, ref getSoulEquipList, ref getSoulBuffList);
        }

        public static List<Ret_Soul_Passive> GetPassive_Soul_Ret_List(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            List<User_PassiveSoul> getPassiveSoulList = SoulManager.GetUser_PassiveSoul(ref TB, AID, CID, Flush);
            List<User_Character_Equip_Soul> getEquipList = SoulManager.GetUser_Character_Equip_Soul(ref TB, AID, CID, Flush);

            return Ret_Soul_Passive.makePassiveSoulRetList(ref getPassiveSoulList, getEquipList, false);
        }

        public static List<Ret_Soul_Passive> GetRet_Passive_Soul_Equip_List(ref TxnBlock TB, long AID, long CID, bool Flush = false, string dbkey = Soul_Define.Soul_InvenDB)
        {
            List<User_PassiveSoul> getPassiveSoulList = SoulManager.GetUser_PassiveSoul(ref TB, AID, CID, Flush, dbkey);
            List<User_Character_Equip_Soul> getEquipList = SoulManager.GetUser_Character_Equip_Soul(ref TB, AID, CID, Flush, dbkey);

            return Ret_Soul_Passive.makePassiveSoulRetList(ref getPassiveSoulList, getEquipList, true);
        }

        public static Result_Define.eResult EquipActiveSoulToCharacter(ref TxnBlock TB, long AID, long CID, ref List<SoulEquipSlot> EquipSlot, ref List<User_Character_Equip_Soul> updateEquipSoulList, bool bGroupCheck, string dbkey = Soul_Define.Soul_InvenDB)
        {
            Soul_Define.eSoulConstDef checkConst = Soul_Define.eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT1_EXPAND_LEVEL;
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            Character charInfo = new Character();

            List<User_ActiveSoul> getSoulList = SoulManager.GetUser_ActiveSoul(ref TB, AID);
            List<User_Character_Equip_Soul> getEquipList = new List<User_Character_Equip_Soul>();
            List<Character> charList = CharacterManager.GetCharacterList(ref TB, AID);


            // bGroupCheck set means 'each character can't equip same active soul'
            if (!bGroupCheck)
            {
                EquipSlot.ForEach(setSlot => setSlot.cid = setSlot.cid == 0 ? CID : setSlot.cid);
                EquipSlot = EquipSlot.FindAll(setSlot => setSlot.cid == CID);
            }

            foreach (IGrouping<long, SoulEquipSlot> setSlot in EquipSlot.GroupBy(setSlot => setSlot.cid))
            {
                charInfo = charList.Find(checkChar => checkChar.cid == setSlot.Key);
                if (charInfo == null)
                    return Result_Define.eResult.CHARACTER_NOT_FOUND;
            }
            
            foreach (Character setChar in charList)
            {
                getEquipList.AddRange(SoulManager.GetUser_Character_Equip_Soul(ref TB, AID, setChar.cid));
            }

            List<User_ActiveSoul> setSoulGroupList = new List<User_ActiveSoul>();
            Dictionary<KeyValuePair<long, short>, SoulEquipSlot> setSoulEquip = new Dictionary<KeyValuePair<long, short>, SoulEquipSlot>();

            foreach (var setSlot in EquipSlot)
            {
                charInfo = charList.Find(checkChar => checkChar.cid == setSlot.cid);

                var bGet = Soul_Define.ActiveEquipSlot_ConstDef.TryGetValue(setSlot.slotnum, out checkConst);
                if (!bGet && setSlot.slotnum > 0)
                    return Result_Define.eResult.System_Unknown_Operation;

                if (charInfo.activeslot < setSlot.slotnum && setSlot.soulseq > 0)
                    return Result_Define.eResult.SOUL_EQUIPSLOT_LIMIT_LEVEL_NOT_ENOUGH;

                //var checkcount = EquipSlot.Count(soulset => soulset.slotnum == setSlot.slotnum);
                //if (checkcount > 1 && setSlot.slotnum > 0)
                //    return Result_Define.eResult.SOUL_EQUIPSLOT_INVALIDE;

                var findSoul = getSoulList.Find(item => item.soulseq == setSlot.soulseq);
                if (findSoul != null && setSlot.soulseq > 0 && setSlot.slotnum > 0)
                {
                    setSoulGroupList.Add(findSoul);
                    KeyValuePair<long, short> checkkey = new KeyValuePair<long, short>(setSlot.cid, setSlot.slotnum);
                    if (setSoulEquip.ContainsKey(checkkey))
                        return Result_Define.eResult.SOUL_EQUIPSLOT_INVALIDE;
                    else
                        setSoulEquip[checkkey] = setSlot;
                }
            }

            // check duplicate group

            charList.ForEach(setChar =>
            {
                if (bGroupCheck || setChar.cid == CID)
                {
                    Soul_Define.EquipSoulSlot.ForEach(checkSlot =>
                    {
                        KeyValuePair<long, short> checkkey = new KeyValuePair<long, short>(setChar.cid, checkSlot);

                        if (!setSoulEquip.ContainsKey(checkkey))
                        {
                            setSoulEquip[checkkey] = new SoulEquipSlot();
                            setSoulEquip[checkkey].cid = setChar.cid;
                            setSoulEquip[checkkey].soulseq = 0;
                            setSoulEquip[checkkey].slotnum = checkSlot;
                        }
                    }
                    );
                }
            }
            );

            EquipSlot = setSoulEquip.Values.ToList();

            foreach (var setsoul in setSoulGroupList)
            {
                var checkSoul = setSoulGroupList.FindAll(item => item.soulgroup == setsoul.soulgroup);
                if (checkSoul.Count > 1)
                    return Result_Define.eResult.SOUL_EQUIP_GROUP_INVALIDE;
            }

            // set equip soul
            foreach (SoulEquipSlot setSlot in EquipSlot)
            {
                User_Character_Equip_Soul updateEquipSoul = new User_Character_Equip_Soul();
                if (setSlot.soulseq > 0)
                {
                    var findSoul = getSoulList.Find(item => item.soulseq == setSlot.soulseq);
                    if (findSoul == null)
                        return Result_Define.eResult.SOUL_ID_NOT_FOUND;

                    charInfo = charList.Find(checkChar => checkChar.cid == setSlot.cid);
                    int setClass = charInfo.Class;

                    retError = SoulManager.UpdateUserSoulEquip(ref TB, AID, setSlot.cid, setSlot.soulseq, Soul_Define.Equip_Soul_Type_Acitve, setClass, setSlot.slotnum, ref updateEquipSoul);

                    if (retError != Result_Define.eResult.SUCCESS)
                        return retError;

                    if (retError != Result_Define.eResult.SUCCESS)
                        return retError;
                }
                else
                {
                    retError = SoulManager.UpdateUserSoulEquip(ref TB, AID, setSlot.cid, 0, Soul_Define.Equip_Soul_Type_Acitve, 0, setSlot.slotnum, ref updateEquipSoul);
                }

                if (retError != Result_Define.eResult.SUCCESS)
                    return retError;
            }

            charList.ForEach(setChar => { SoulManager.RemoveCacheUser_Equip_Soul(AID, setChar.cid); });            

            return retError;
        }

        public static Result_Define.eResult EquipPassiveSoulToCharacter(ref TxnBlock TB, long AID, long CID, ref List<SoulEquipSlot> EquipSlot, ref List<User_Character_Equip_Soul> updateEquipSoulList, string dbkey = Soul_Define.Soul_InvenDB)
        {
            Soul_Define.eSoulConstDef checkConst = Soul_Define.eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT1_EXPAND_LEVEL;
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            Character charInfo = new Character();
            List<Character> charList = CharacterManager.GetCharacterList(ref TB, AID);

            List<User_PassiveSoul> getSoulList = new List<User_PassiveSoul>();
            foreach (Character setCID in charList)
            {
                getSoulList.AddRange(SoulManager.GetUser_PassiveSoul(ref TB, AID, setCID.cid));
            }
            List<User_Character_Equip_Soul> getEquipList = new List<User_Character_Equip_Soul>();
            foreach (Character setCID in charList)
            {
                getEquipList.AddRange(SoulManager.GetUser_Character_Equip_Soul(ref TB, AID, setCID.cid));
            }

            List<User_PassiveSoul> setSoulGroupList = new List<User_PassiveSoul>();
            Dictionary<KeyValuePair<long, short>, SoulEquipSlot> setSoulEquip = new Dictionary<KeyValuePair<long, short>, SoulEquipSlot>();

            foreach (var setSlot in EquipSlot)
            {
                charInfo = charList.Find(checkChar => checkChar.cid == setSlot.cid);
                int setClass = charInfo.Class;

                var bGet = Soul_Define.PassiveEquipSlot_ConstDef.TryGetValue(setSlot.slotnum, out checkConst);
                if (!bGet && setSlot.slotnum > 0)
                    return Result_Define.eResult.System_Unknown_Operation;

                if (charInfo.passiveslot < setSlot.slotnum && setSlot.soulseq > 0)
                    return Result_Define.eResult.SOUL_EQUIPSLOT_LIMIT_LEVEL_NOT_ENOUGH;

                //var checkcount = EquipSlot.Count(soulset => soulset.slotnum == setSlot.slotnum);
                //if (checkcount > 1 && setSlot.slotnum > 0)
                //    return Result_Define.eResult.SOUL_EQUIPSLOT_INVALIDE;

                var findSoul = getSoulList.Find(item => item.soulseq == setSlot.soulseq && item.cid == setSlot.cid);
                if (findSoul != null && setSlot.soulseq > 0 && setSlot.slotnum > 0)
                {
                    setSoulGroupList.Add(findSoul);
                    KeyValuePair<long, short> checkkey = new KeyValuePair<long, short>(setSlot.cid, setSlot.slotnum);
                    if (setSoulEquip.ContainsKey(checkkey))
                        return Result_Define.eResult.SOUL_EQUIPSLOT_INVALIDE;
                    else
                        setSoulEquip[checkkey] = setSlot;
                }
            }

            charList.ForEach(setChar =>
            {
                if (setChar.cid == CID)
                {
                    Soul_Define.EquipSoulSlot.ForEach(checkSlot =>
                    {
                        KeyValuePair<long, short> checkkey = new KeyValuePair<long, short>(setChar.cid, checkSlot);

                        if (!setSoulEquip.ContainsKey(checkkey))
                        {
                            setSoulEquip[checkkey] = new SoulEquipSlot();
                            setSoulEquip[checkkey].cid = setChar.cid;
                            setSoulEquip[checkkey].soulseq = 0;
                            setSoulEquip[checkkey].slotnum = checkSlot;
                        }
                    }
                    );
                }
            }
            );

            EquipSlot = setSoulEquip.Values.ToList();

            // check duplicate group
            foreach (var setsoul in setSoulGroupList)
            {
                var checkSoul = setSoulGroupList.FindAll(item => item.soulgroup == setsoul.soulgroup && item.cid == setsoul.cid);
                if (checkSoul.Count > 1)
                    return Result_Define.eResult.SOUL_EQUIP_GROUP_INVALIDE;
            }

            // check unequip soul list
            List<User_Character_Equip_Soul> unEquipList = new List<User_Character_Equip_Soul>();
            foreach (SoulEquipSlot setSlot in EquipSlot)
            {
                var findEquip = getEquipList.Find(equip => equip.soul_type == Soul_Define.Equip_Soul_Type_Passive && equip.slot_num == setSlot.slotnum && equip.cid == setSlot.cid);

                if (findEquip != null)
                {
                    if (EquipSlot.Find(equipSoul => equipSoul.soulseq == findEquip.soulseq) == null)
                        unEquipList.Add(findEquip);
                }
            }

            // update unequip soul
            foreach (User_Character_Equip_Soul setEquip in unEquipList)
            {
                var findSoul = getSoulList.Find(item => item.soulseq == setEquip.soulseq);
                if (findSoul != null)
                {
                    findSoul.stateflag = Soul_Define.PassiveSoulStates[Soul_Define.ePassiveSoulStates.Store];
                    retError = UpdatePassiveSoulInfo(ref TB, findSoul);
                }
                if (retError != Result_Define.eResult.SUCCESS)
                    return retError;
            }

            // set equip soul
            foreach (SoulEquipSlot setSlot in EquipSlot)
            {
                charInfo = charList.Find(checkChar => checkChar.cid == setSlot.cid);
                int setClass = charInfo.Class;

                User_Character_Equip_Soul updateEquipSoul = new User_Character_Equip_Soul();
                if (setSlot.soulseq > 0)
                {
                    var findSoul = getSoulList.Find(item => item.soulseq == setSlot.soulseq);
                    if (findSoul == null)
                        return Result_Define.eResult.SOUL_ID_NOT_FOUND;

                    retError = SoulManager.UpdateUserSoulEquip(ref TB, AID, charInfo.cid, setSlot.soulseq, Soul_Define.Equip_Soul_Type_Passive, setClass, setSlot.slotnum, ref updateEquipSoul);

                    if (retError != Result_Define.eResult.SUCCESS)
                        return retError;

                    findSoul.stateflag = Soul_Define.PassiveSoulStates[Soul_Define.ePassiveSoulStates.Equip];
                    retError = SoulManager.UpdatePassiveSoulInfo(ref TB, findSoul);

                    if (retError != Result_Define.eResult.SUCCESS)
                        return retError;
                }
                else
                {
                    retError = SoulManager.UpdateUserSoulEquip(ref TB, AID, charInfo.cid, 0, Soul_Define.Equip_Soul_Type_Passive, 0, setSlot.slotnum, ref updateEquipSoul);
                }
            }

            foreach (Character setCID in charList)
            {
                SoulManager.RemoveCacheUser_Equip_Soul(AID, setCID.cid);
            }

            return retError;
        }

        public static Result_Define.eResult UpdateUserSoulEquip(ref TxnBlock TB, long AID, long CID, long soulseq, short soul_type, int class_type, short slot_num, ref User_Character_Equip_Soul retEquipInfo, string dbkey = Soul_Define.Soul_InvenDB)
        {
            SqlCommand commandUpdate_User_SoulEquip = new SqlCommand();
            commandUpdate_User_SoulEquip.CommandText = "User_Update_Soul_Equip";
            commandUpdate_User_SoulEquip.Parameters.Add("@aid", SqlDbType.BigInt).Value = AID;
            commandUpdate_User_SoulEquip.Parameters.Add("@cid", SqlDbType.BigInt).Value = CID;
            commandUpdate_User_SoulEquip.Parameters.Add("@soulseq", SqlDbType.BigInt).Value = soulseq;
            commandUpdate_User_SoulEquip.Parameters.Add("@soul_type", SqlDbType.TinyInt).Value = soul_type;
            commandUpdate_User_SoulEquip.Parameters.Add("@class_type", SqlDbType.TinyInt).Value = System.Convert.ToInt16(class_type);
            commandUpdate_User_SoulEquip.Parameters.Add("@slot_num", SqlDbType.TinyInt).Value = slot_num;
            var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandUpdate_User_SoulEquip.Parameters.Add(result);

            SqlDataReader getDr = null;
            TB.ExcuteSqlStoredProcedure(dbkey, ref commandUpdate_User_SoulEquip, ref getDr);

            if (getDr != null)
            {
                long checkValue = System.Convert.ToInt64(result.Value);

                if (checkValue < 0)
                {
                    getDr.Dispose(); getDr.Close();
                    commandUpdate_User_SoulEquip.Dispose();
                    return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                }

                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);

                getDr.Dispose(); getDr.Close();
                commandUpdate_User_SoulEquip.Dispose();
                retEquipInfo = mJsonSerializer.JsonToObject<User_Character_Equip_Soul[]>(json).FirstOrDefault();
                if (retEquipInfo.aid < 1)
                    return Result_Define.eResult.SOUL_ID_NOT_FOUND;
            }
            else
            {
                commandUpdate_User_SoulEquip.Dispose();
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }

            return Result_Define.eResult.SUCCESS;
        }

        public static void CalcSoulSlot(ref TxnBlock TB, int chkLevel, int curactiveslot, int curpassiveslot, out int activeslot, out int passiveslot, string dbkey = Soul_Define.Soul_InvenDB)
        {
            activeslot = 0;
            if (chkLevel >= SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT1_EXPAND_LEVEL])) activeslot++;
            if (chkLevel >= SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT2_EXPAND_LEVEL])) activeslot++;
            if (chkLevel >= SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT3_EXPAND_LEVEL])) activeslot++;
            if (chkLevel >= SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_ACTIVE_SOUL_EQUIPSLOT4_EXPAND_LEVEL])) activeslot++;

            activeslot = curactiveslot > activeslot ? curactiveslot : activeslot;

            passiveslot = 0;
            if (chkLevel >= SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT1_EXPAND_LEVEL])) passiveslot++;
            if (chkLevel >= SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT2_EXPAND_LEVEL])) passiveslot++;
            if (chkLevel >= SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT3_EXPAND_LEVEL])) passiveslot++;
            if (chkLevel >= SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.eSoulConstDef.DEF_PASSIVE_SOUL_EQUIPSLOT4_EXPAND_LEVEL])) passiveslot++;
            passiveslot = curpassiveslot > passiveslot ? curpassiveslot : passiveslot;
        }

        public static Result_Define.eResult BuySoulSlot(ref TxnBlock TB, long AID, long CID, short soul_type, int chkLevel, int curactiveslot, int curpassiveslot, string dbkey = Soul_Define.Soul_InvenDB)
        {
            if (curactiveslot >= Soul_Define.ActiveEquipSlot_ConstDef.Count && soul_type == Soul_Define.Equip_Soul_Type_Acitve)
                return Result_Define.eResult.SOUL_MAX_SLOT;
            else if (curpassiveslot >= Soul_Define.PassiveEquipSlot_ConstDef.Count && soul_type == Soul_Define.Equip_Soul_Type_Passive)
                return Result_Define.eResult.SOUL_MAX_SLOT;

            int CheckSlot = soul_type == Soul_Define.Equip_Soul_Type_Acitve ? curactiveslot + 1 : (soul_type == Soul_Define.Equip_Soul_Type_Passive ? curpassiveslot + 1 : 0);
            if (CheckSlot <= 0
                || (soul_type == Soul_Define.Equip_Soul_Type_Acitve && CheckSlot > Soul_Define.SoulActiveSlot_Price_Key.Count)
                || (soul_type == Soul_Define.Equip_Soul_Type_Passive && CheckSlot > Soul_Define.SoulPassiveSlot_Price_Key.Count)
                )
                return Result_Define.eResult.SOUL_SLOT_TYPE_INVALIDE;

            int BuyPrice = soul_type == Soul_Define.Equip_Soul_Type_Acitve ? SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.SoulActiveSlot_Price_Key[CheckSlot]])
                                                                            : SystemData.GetConstValueInt(ref TB, Soul_Define.Soul_Const_Def_Key_List[Soul_Define.SoulPassiveSlot_Price_Key[CheckSlot]]);

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            if (BuyPrice > 0)
                retError = AccountManager.UseUserCash(ref TB, AID, BuyPrice);

            if (retError == Result_Define.eResult.SUCCESS)
            {
                curactiveslot = soul_type == Soul_Define.Equip_Soul_Type_Acitve ? CheckSlot : curactiveslot;
                curpassiveslot = soul_type == Soul_Define.Equip_Soul_Type_Passive ? CheckSlot : curpassiveslot;

                retError = CharacterManager.UpdateCharacterSoulSlot(ref TB, AID, CID, curactiveslot, curpassiveslot);
            }

            return Result_Define.eResult.SUCCESS;
        }
    }
}
