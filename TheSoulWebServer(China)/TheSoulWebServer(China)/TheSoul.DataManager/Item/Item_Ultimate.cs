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
    public static partial class ItemManager
    {
        // private method : make ultimate weapon
        private static Result_Define.eResult MakeUltimateWeapon(ref TxnBlock TB, ref User_Ultimate_Inven MakeInfo, long ItemID, long AID, long CID, string dbkey = Item_Define.Item_InvenDB)
        {
            System_Ultimate_Weapon ItemInfo = GetSystem_Ultimate_Weapon(ref TB, ItemID, dbkey);
            if (ItemInfo == null)
                return Result_Define.eResult.ITEM_EQUIP_INFO_NOT_FOUND;
            
            if (!Item_Define.ItemType.ContainsKey(ItemInfo.ItemType))
                return Result_Define.eResult.ITEM_TYPE_INVALIDE;

            List<User_Ultimate_Inven> itemList = GetUserUltimateWeaponList(ref TB, AID, CID, true, true);
            foreach (User_Ultimate_Inven chkItem in itemList)
            {
                System_Ultimate_Enchant chkInfo = GetSystem_Ultimate_Enchant(ref TB, chkItem.item_id);
                if (chkInfo.Ultimate_ID == ItemInfo.Ultimate_ID)
                    return Result_Define.eResult.ITEM_ULTIMATE_ALREADY_EXIST;
            }

            Item_Define.eItemType setItemType = Item_Define.ItemType[ItemInfo.ItemType];

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            System_Ultimate_Enchant EnchantInfo = GetSystem_Ultimate_Enchant(ref TB, ItemInfo.Ultimate_ID, Item_Define.UltimateWeaponBaseLevel, Item_Define.UltimateWeaponBaseStep, dbkey);
            // ultimate weapon and orb don't create user_option table. only use system option id (fixed)

            //if (retError != Result_Define.eResult.SUCCESS)
            //    return retError;

            User_Ultimate_Inven setItem = new User_Ultimate_Inven();

            setItem.aid = AID;
            setItem.cid = CID;

            if (setItem.cid > 0)
            {
                Character charInfo = CharacterManager.GetCharacter(ref TB, AID, setItem.cid, false, dbkey);
                Character_Define.SystemClassType setClassType = (Character_Define.SystemClassType)ItemInfo.EquipClass;
                if (setClassType != (Character_Define.SystemClassType)charInfo.Class)
                    return Result_Define.eResult.ITEM_EQUIP_CLASS_TYPE_INVALIDE;
                else
                    setItem.class_type = (byte)charInfo.Class;
            }
            else
                return Result_Define.eResult.ITEM_EQUIP_CLASS_TYPE_INVALIDE;

            setItem.item_id = ItemInfo.Ultimate_ID;
            setItem.level = EnchantInfo.EnchantLevel;
            setItem.step = EnchantInfo.Step;
            setItem.equipflag = "N";

            retError = MakeUltimateItemToDB(ref TB, ref setItem, dbkey);
            
            MakeInfo = setItem;

            RemoveUltimateWeaonCache(AID, CID);

            //retError = MakeInfo.Count > 0 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.ITEM_CREATE_FAIL;

            //if (retError == Result_Define.eResult.SUCCESS)
            //{
            //    if (Item_Define.ItemWeaponTypeList.Contains(setItemType))
            //        retError = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.Equip_Acquire, (int)Trigger_Define.eEquipAccquireType.Weapon, 0, makeCount);
            //    else if (Item_Define.ItemAccessoryTypeList.Contains(setItemType))
            //        retError = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.Equip_Acquire, (int)Trigger_Define.eEquipAccquireType.Accessary, 0, makeCount);
            //}

            return retError;
        }


        // Make Ultimate Item to DB
        private static Result_Define.eResult MakeUltimateItemToDB(ref TxnBlock TB, ref User_Ultimate_Inven setItem, string dbkey = Item_Define.Item_InvenDB)
        {
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "User_Insert_Ultimate_Inven";
            Cmd.Parameters.Add("@aid", SqlDbType.BigInt).Value = setItem.aid;
            Cmd.Parameters.Add("@cid", SqlDbType.BigInt).Value = setItem.cid;
            Cmd.Parameters.Add("@classtype", SqlDbType.Int).Value = setItem.class_type;
            Cmd.Parameters.Add("@item_id", SqlDbType.BigInt).Value = setItem.item_id;
            Cmd.Parameters.Add("@level", SqlDbType.Int).Value = setItem.level;
            Cmd.Parameters.Add("@step", SqlDbType.Int).Value = setItem.step;
            Cmd.Parameters.Add("@equipflag", SqlDbType.Char).Value = setItem.equipflag;
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
                    return Result_Define.eResult.ITEM_CREATE_FAIL;
                }

                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);

                getDr.Dispose(); getDr.Close();
                List<User_Ultimate_Inven> makeItem = mJsonSerializer.JsonToObject<List<User_Ultimate_Inven>>(json);

                Cmd.Dispose();
                if (makeItem != null && makeItem.Count > 0)
                {
                    string setItemLogJson = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.item_log_list]);
                    setItem = makeItem.FirstOrDefault();
                    TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_item_log]);
                    ItemLogInfo setLog = new ItemLogInfo(setItem.item_id, (int)SnailLog_Define.Snail_Money_Event_type.add, 1, 0, 1);
                    setItemLogJson = mJsonSerializer.AddJsonArray(setItemLogJson, mJsonSerializer.ToJsonString(setLog));
                    TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.item_log_list], setItemLogJson);
                }else
                    return Result_Define.eResult.ITEM_CREATE_FAIL;
            }
            else
            {
                Cmd.Dispose();
                return Result_Define.eResult.ITEM_CREATE_FAIL;
            }
            return Result_Define.eResult.SUCCESS;
        }

        private static Result_Define.eResult MakeUltimateOrb(ref TxnBlock TB, ref User_Orb_Inven MakeInfo, long AID, long ItemID, string dbkey = Item_Define.Item_InvenDB)
        {
            MakeInfo.aid = AID;
            MakeInfo.orb_id = ItemID;

            //System_Ultimate_Orb orbInfo = GetSystem_Ultimate_Orb(ref TB, ItemID);            
            //if (orbInfo.Orb_Item_ID == 0)
            //    return Result_Define.eResult.ITEM_SYSTEM_ID_NOT_FOUND;

            return MakeUltimateOrbToDB(ref TB, ref MakeInfo, dbkey);
        }

        // Make Ultimate Orb Item to DB
        private static Result_Define.eResult MakeUltimateOrbToDB(ref TxnBlock TB, ref User_Orb_Inven setItem, string dbkey = Soul_Define.Soul_InvenDB)
        {
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "User_Insert_Orb_Inven";
            Cmd.Parameters.Add("@aid", SqlDbType.BigInt).Value = setItem.aid;
            Cmd.Parameters.Add("@orb_id", SqlDbType.BigInt).Value = setItem.orb_id;
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
                    return Result_Define.eResult.ITEM_CREATE_FAIL;
                }

                var r = SQLtoJson.Serialize(ref getDr);
                string json = mJsonSerializer.ToJsonString(r);

                getDr.Dispose(); getDr.Close();
                List<User_Orb_Inven> makeItem = mJsonSerializer.JsonToObject<List<User_Orb_Inven>>(json);

                Cmd.Dispose();
                if (makeItem != null && makeItem.Count > 0)
                {
                    string setItemLogJson = TB.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.item_log_list]);
                    setItem = makeItem.FirstOrDefault();

                    System_Ultimate_Orb OrbInfo = GetSystem_Ultimate_Orb(ref TB, setItem.orb_id);
                    List<long> OptionList = new List<long>() { OrbInfo.Option_ID1, OrbInfo.Option_ID2, OrbInfo.Option_ID3 };
                    List<User_Inven_Option> getOptionList = new List<User_Inven_Option>();
                    List<System_Item_Option> getTestOptionList = GetSystem_Item_Option_List(ref TB, OptionList);
                    foreach (System_Item_Option setFixOption in getTestOptionList)
                    {
                        if (setFixOption.Option_IndexID > 0)
                        {
                            User_Inven_Option setOption = MakeUserInvenOption(setFixOption, false);
                            setOption.isbase = "Y";
                            getOptionList.Add(setOption);
                        }
                    }
                    setItem.orb_option = getOptionList;

                    //System_Ultimate_Orb OrbInfo = GetSystem_Ultimate_Orb(ref TB, setItem.orb_id);
                    //List<User_Inven_Option> getOrbOptionList = new List<User_Inven_Option>();
                    //System_Item_Option setOrbOption = GetSystem_Item_Option(ref TB, OrbInfo.Option_ID);
                    //if (setOrbOption.Option_IndexID > 0)
                    //{
                    //    List<User_Inven_Option> getOptionList = new List<User_Inven_Option>();
                    //    User_Inven_Option setOption = MakeUserInvenOption(setOrbOption, false);
                    //    setOption.isbase = "Y";
                    //    getOptionList.Add(setOption);
                    //    setItem.orb_option = getOptionList;
                    //}
                    //else
                    //    setItem.orb_option = new List<User_Inven_Option>();

                    TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_item_log]);
                    ItemLogInfo setLog = new ItemLogInfo(setItem.orb_id, (int)SnailLog_Define.Snail_Money_Event_type.add, 1, 0, 1);
                    setItemLogJson = mJsonSerializer.AddJsonArray(setItemLogJson, mJsonSerializer.ToJsonString(setLog));
                    TB.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.item_log_list], setItemLogJson);
                }
                else
                    return Result_Define.eResult.ITEM_CREATE_FAIL;
            }
            else
            {
                Cmd.Dispose();
                return Result_Define.eResult.ITEM_CREATE_FAIL;
            }
            RemoveUltimateOrbCache(setItem.aid, setItem.ultimate_inven_seq);

            return Result_Define.eResult.SUCCESS;
        }

        public static void RemoveUltimateOrbCache(long AID, long WeaponSeq)
        {
            string setKey = GetUltimateOrbKey(AID, WeaponSeq);
            RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
            //setKey = GetUltimateOrbKey(AID, 0);
            //RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static string GetUltimateOrbKey(long AID, long WeaponSeq)
        {
            return string.Format("{0}_{1}_{2}_{3}", Item_Define.User_Inven_Prefix, Item_Define.Item_User_Orb_Inven_Table, AID, WeaponSeq);
        }        

        public static List<User_Orb_Inven> GetUser_Ultimate_OrbList(ref TxnBlock TB, long AID, long WeaponSeq, string dbkey = Item_Define.Item_InvenDB)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK, INDEX(IX_User_Orb_Inven_AID)) WHERE aid = {1} AND ultimate_inven_seq = {2} AND delflag = 'N'", Item_Define.Item_User_Orb_Inven_Table, AID, WeaponSeq);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_Orb_Inven>(ref TB, setQuery, dbkey);
        }

        public static List<User_Orb_Inven> GetUserOrbInvenList(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Item_Define.Item_InvenDB)
        {
            return GetUserOrbInvenList(ref TB, AID, 0, Flush, dbkey);
        }

        public static List<User_Orb_Inven> GetUserOrbInvenList(ref TxnBlock TB, long AID, long WeaponSeq, bool Flush = false, string dbkey = Item_Define.Item_InvenDB)
        {
            string setKey = GetUltimateOrbKey(AID, WeaponSeq);
            List<User_Orb_Inven> retObj = new List<User_Orb_Inven>();
            List<User_Orb_Inven> setOrbList = new List<User_Orb_Inven>();

            if (!Flush)
            {
                setOrbList = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis<List<User_Orb_Inven>>(DataManager_Define.RedisServerAlias_User, setKey);
                if (setOrbList == null || setOrbList.Count == 0)
                    Flush = true;
            }

            if (Flush)
            {
                RemoveUltimateOrbCache(AID, WeaponSeq);
                setOrbList = GetUser_Ultimate_OrbList(ref TB, AID, WeaponSeq);
                foreach (User_Orb_Inven setOrb in setOrbList)
                {
                    System_Ultimate_Orb OrbInfo = GetSystem_Ultimate_Orb(ref TB, setOrb.orb_id);
                    List<long> OptionList = new List<long>() { OrbInfo.Option_ID1, OrbInfo.Option_ID2, OrbInfo.Option_ID3 };
                    List<User_Inven_Option> getOptionList = new List<User_Inven_Option>();
                    List<System_Item_Option> getTestOptionList = GetSystem_Item_Option_List(ref TB, OptionList);
                    foreach (System_Item_Option setFixOption in getTestOptionList)
                    {
                        if (setFixOption.Option_IndexID > 0)
                        {
                            User_Inven_Option setOption = MakeUserInvenOption(setFixOption, false);
                            setOption.isbase = "Y";
                            getOptionList.Add(setOption);
                        }
                    }
                    setOrb.orb_option = getOptionList;
                    retObj.Add(setOrb);

                    //System_Ultimate_Orb OrbInfo = GetSystem_Ultimate_Orb(ref TB, setOrb.orb_id);
                    //List<User_Inven_Option> getOrbOptionList = new List<User_Inven_Option>();
                    //System_Item_Option setOrbOption = GetSystem_Item_Option(ref TB, OrbInfo.Option_ID);
                    //if (setOrbOption.Option_IndexID > 0)
                    //{
                    //    List<User_Inven_Option> getOptionList = new List<User_Inven_Option>();
                    //    User_Inven_Option setOption = MakeUserInvenOption(setOrbOption, false);
                    //    setOption.isbase = "Y";
                    //    getOptionList.Add(setOption);
                    //    setOrb.orb_option = getOptionList;
                    //}
                    //retObj.Add(setOrb);
                }

                RedisConst.GetRedisInstance().SetObj(DataManager_Define.RedisServerAlias_User, setKey, retObj);
            }
            else
                retObj = setOrbList;

            return retObj;
        }

        public static List<User_Ultimate_Inven> GetUser_Ultimate_InvenList(ref TxnBlock TB, long AID, long CID, string dbkey = Item_Define.Item_InvenDB)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK, INDEX(IX_User_Ultimate_Inven))  WHERE aid = {1} AND cid = {2} ", Item_Define.Item_User_Ultimate_Inven_Table, AID, CID);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_Ultimate_Inven>(ref TB, setQuery, dbkey);
        }

        public static void RemoveUltimateWeaonCache(long AID, long CID)
        {
            string setKey = GetUltimateWeaponKey(AID, CID);
            RedisConst.GetRedisInstance().RemoveHash(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static string GetUltimateWeaponKey(long AID, long CID)
        {
            return string.Format("{0}_{1}_{2}_{3}", Item_Define.User_Inven_Prefix, Item_Define.Item_User_Ultimate_Inven_Table, AID, CID);
        }

        public static List<User_Ultimate_Inven> GetUserUltimateWeaponList(ref TxnBlock TB, long AID, long CID, bool bWithOption = true, bool Flush = false, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Ultimate_Inven> retObj = new List<User_Ultimate_Inven>();

            string setKey = GetUltimateWeaponKey(AID, CID);
            List<User_Ultimate_Inven> setObj = new List<User_Ultimate_Inven>();

            if (!Flush)
            {
                setObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_Hash_All<User_Ultimate_Inven>(DataManager_Define.RedisServerAlias_User, setKey);
                if (setObj.Count == 0)
                    Flush = true;
            }

            if (Flush)
            {
                RemoveUltimateWeaonCache(AID, CID);
                setObj = GetUser_Ultimate_InvenList(ref TB, AID, CID);

                foreach (User_Ultimate_Inven setitem in setObj)
                {
                    System_Ultimate_Enchant EnchantInfo = GetSystem_Ultimate_Enchant(ref TB, setitem.item_id, dbkey);

                    List<long> OptionList = new List<long>() { EnchantInfo.Option_ID1, EnchantInfo.Option_ID2, EnchantInfo.Option_ID3, EnchantInfo.Option_ID4 };
                    List<User_Inven_Option> getOptionList = new List<User_Inven_Option>();
                    List<System_Item_Option> getTestOptionList = GetSystem_Item_Option_List(ref TB, OptionList);
                    foreach (System_Item_Option setFixOption in getTestOptionList)
                    {
                        if (setFixOption.Option_IndexID > 0)
                        {
                            User_Inven_Option setOption = MakeUserInvenOption(setFixOption, false);
                            setOption.isbase = "Y";
                            getOptionList.Add(setOption);
                        }
                    }
                    //foreach (long setOptionID in OptionList)
                    //{
                    //    if (setOptionID > 0)
                    //    {
                    //        System_Item_Option setFixOption = GetSystem_Item_Option(ref TB, setOptionID);
                    //        if (setFixOption.Option_IndexID > 0)
                    //        {
                    //            User_Inven_Option setOption = MakeUserInvenOption(setFixOption, false);
                    //            setOption.isbase = "Y";
                    //            getOptionList.Add(setOption);
                    //        }
                    //    }
                    //}
                    setitem.option_list = getOptionList;
                    List<User_Orb_Inven> setOrbList = GetUserOrbInvenList(ref TB, AID, setitem.ultimate_inven_seq);
                    setitem.orb_slot = setOrbList;
                    RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, setitem.ultimate_inven_seq.ToString(), setitem);
                    retObj.Add(setitem);
                }
                RedisConst.GetRedisInstance().SetExpireTimeHash(DataManager_Define.RedisServerAlias_User, setKey);
            }
            else
                retObj = setObj;

            return retObj;
        }


        private static string GetUltimateEquipKey(long AID, long CID)
        {
            return string.Format("{0}_{1}_{2}", Item_Define.User_Inven_Prefix, Item_Define.User_Ultimate_Equip_Prefix, CID);
        }

        public static List<User_Ultimate_Inven> GetEquipUltimate(ref TxnBlock TB, long AID, long CID, bool bWithOption = true, bool Flush = false, string dbkey = Item_Define.Item_InvenDB)
        {
            return GetUserUltimateWeaponList(ref TB, AID, CID, bWithOption, Flush, dbkey);
        }

        public static Result_Define.eResult EquipUltimate(ref TxnBlock TB, long AID, long CID, long InvenSeq, bool EquipFlag, string dbkey = Item_Define.Item_InvenDB)
        {
            string setQuery = EquipFlag ?
                string.Format(@"UPDATE {0}
                                    SET equipflag = CASE WHEN ultimate_inven_seq != {1} THEN 'N' ELSE 'Y' END
                                WHERE AID = {2} AND CID = {3} ",
                                                               Item_Define.Item_User_Ultimate_Inven_Table, InvenSeq, AID, CID) :
                string.Format(@"UPDATE {0}
                                    SET equipflag = 'N'
                                WHERE ultimate_inven_seq = {1} AND AID = {2} AND CID = {3} ",
                                                               Item_Define.Item_User_Ultimate_Inven_Table, InvenSeq, AID, CID);
            RemoveUltimateWeaonCache(AID, CID);

            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

        }

        public static Result_Define.eResult EnchantUltimate(ref TxnBlock TB, long AID, long CID, long InvenSeq, ref List<Return_DisassableItems_List> retDeletedItem, ref User_Ultimate_Inven updateInfo, int tryCount = 0, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Ultimate_Inven> itemList = GetUserUltimateWeaponList(ref TB, AID, CID);

            var findItem = itemList.Find(item => item.ultimate_inven_seq == InvenSeq);
            if (findItem == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            int maxLevel = CharacterManager.GetCharacterMaxLevel_FromDB(ref TB, AID);
            System_Ultimate_Enchant EnchantInfo = GetSystem_Ultimate_Enchant(ref TB, findItem.item_id);
            if (EnchantInfo.NextItemID == 0 && EnchantInfo.Ultimate_ID > 0)
                return Result_Define.eResult.ITEM_ULTIMATE_ENHANCE_MAX;

            int useGold = 0;
            int useRuby = 0;
            Dictionary<long, User_Item_Enchant> useItemList = new Dictionary<long, User_Item_Enchant>();
            for (int chkCount = 0; chkCount < tryCount; chkCount++)
            {
                if (EnchantInfo.Require_NeedCLv > maxLevel)
                    retError = Result_Define.eResult.ITEM_EQUIP_NOT_ENOUGH_LEVEL;

                if (EnchantInfo.NextItemID > 0 && retError == Result_Define.eResult.SUCCESS)
                {
                    if (retError == Result_Define.eResult.SUCCESS && EnchantInfo.Require_NeedItemID > 0 && EnchantInfo.Require_NeedItemNum > 0)
                    {
                        if (useItemList.ContainsKey(EnchantInfo.Require_NeedItemID))
                            useItemList[EnchantInfo.Require_NeedItemID].use_count += EnchantInfo.Require_NeedItemNum;
                        else
                        {
                            User_Item_Enchant setUseItem = new User_Item_Enchant();
                            setUseItem.use_itemid = EnchantInfo.Require_NeedItemID;
                            setUseItem.use_count += EnchantInfo.Require_NeedItemNum;
                            useItemList.Add(EnchantInfo.Require_NeedItemID, setUseItem);
                        }
                    }
                    if (retError == Result_Define.eResult.SUCCESS && EnchantInfo.Require_NeedGold > 0)
                        useGold += EnchantInfo.Require_NeedGold;
                    if (retError == Result_Define.eResult.SUCCESS && EnchantInfo.Require_NeedRuby > 0)
                        useRuby += EnchantInfo.Require_NeedRuby;
                }

                if (EnchantInfo.NextItemID > 0 && EnchantInfo.Ultimate_ID > 0)
                    EnchantInfo = GetSystem_Ultimate_Enchant(ref TB, EnchantInfo.NextItemID);
                else if (EnchantInfo.Ultimate_ID == 0)
                    return Result_Define.eResult.ITEM_SYSTEM_ID_NOT_FOUND;
                else
                    break;

                if (retError != Result_Define.eResult.SUCCESS)
                    return retError;
            }

            foreach(User_Item_Enchant useItem in useItemList.Values)
            {
                if(useItem.use_count > 0)
                    retError = UseItem(ref TB, AID, useItem.use_itemid, useItem.use_count, ref retDeletedItem);

                if (retError != Result_Define.eResult.SUCCESS)
                    return retError;
            }
            if (retError == Result_Define.eResult.SUCCESS && useGold > 0)
                retError = AccountManager.UseUserGold(ref TB, AID, useGold);
            if (retError == Result_Define.eResult.SUCCESS && useRuby > 0)
                retError = AccountManager.UseUserCash(ref TB, AID, useRuby);

            if (retError == Result_Define.eResult.SUCCESS && EnchantInfo.Ultimate_Enchant_Index > 0)
            {
                findItem.item_id = EnchantInfo.Ultimate_Enchant_Index;
                findItem.level = EnchantInfo.EnchantLevel;
                findItem.step = EnchantInfo.Step;
                retError = UpdateUltimateInven(ref TB, AID, CID, InvenSeq, findItem);                
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                updateInfo = GetUserUltimateWeaponList(ref TB, AID, CID).Find(item => item.ultimate_inven_seq == InvenSeq);
            }
            return retError;
        }

        public static Result_Define.eResult UpdateUltimateInven(ref TxnBlock TB, long AID, long CID, long InvenSeq, User_Ultimate_Inven setItem, string dbkey = Item_Define.Item_InvenDB)
        {
            string setQuery = string.Format(@"UPDATE {0}
                                                    SET
                                                        item_id = {1},
                                                        level = {2},
                                                        step = {3}
                                                WHERE ultimate_inven_seq = {4} AND AID = {5} AND CID = {6} "
                                            , Item_Define.Item_User_Ultimate_Inven_Table
                                            , setItem.item_id
                                            , setItem.level
                                            , setItem.step
                                            , InvenSeq
                                            , AID
                                            , CID
                                            );
            RemoveUltimateWeaonCache(AID, CID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult EquipOrbToUltimate(ref TxnBlock TB, long AID, long CID, long Ultimate_InvenSeq, long Orb_InvenSeq, byte SlotNum, bool EquipFlag, ref User_Orb_Inven updateOrdInfo, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Ultimate_Inven> itemList = GetUserUltimateWeaponList(ref TB, AID, CID);

            var findItem = itemList.Find(item => item.ultimate_inven_seq == Ultimate_InvenSeq);
            if (findItem == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_Ultimate_Enchant EnchantInfo = GetSystem_Ultimate_Enchant(ref TB, findItem.item_id);
            System_Ultimate_Weapon ultimateInfo = ItemManager.GetSystem_Ultimate_Weapon(ref TB, EnchantInfo.Ultimate_ID);

            if (EnchantInfo.Ultimate_Enchant_Index == 0 || ultimateInfo.Ultimate_ID == 0)
                return Result_Define.eResult.ITEM_SYSTEM_ID_NOT_FOUND;

            if (!EquipFlag)         // unequip
                SlotNum = 0;

            int chkUltimateLevel = 0;
            switch(SlotNum)
            {
                case 0: chkUltimateLevel = 0; break;
                case 1: chkUltimateLevel = ultimateInfo.OrbSlot_Lv1; break;
                case 2: chkUltimateLevel = ultimateInfo.OrbSlot_Lv2; break;
                case 3: chkUltimateLevel = ultimateInfo.OrbSlot_Lv3; break;
                case 4: chkUltimateLevel = ultimateInfo.OrbSlot_Lv4; break;
                case 5: chkUltimateLevel = ultimateInfo.OrbSlot_Lv5; break;
                case 6: chkUltimateLevel = ultimateInfo.OrbSlot_Lv6; break;
                default:
                    return Result_Define.eResult.ITEM_ULTIMATE_ORB_SLOT_INVALIDE;
            }

            if (chkUltimateLevel > findItem.level)
                return Result_Define.eResult.ITEM_ULTIMATE_LEVEL_NOT_ENOUGH;

            List<User_Orb_Inven> orbList = GetUserOrbInvenList(ref TB, AID);
            orbList.AddRange(GetUserOrbInvenList(ref TB, AID, Ultimate_InvenSeq));

            var findOrb = orbList.Find(orbitem => orbitem.orb_inven_seq == Orb_InvenSeq);
            if (findOrb == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_Ultimate_Orb sysOrbInfo = GetSystem_Ultimate_Orb(ref TB, findOrb.orb_id);
            bool bEnableSlot = !EquipFlag;

            if (Item_Define.UltimateOrbEquipSlot.ContainsKey(sysOrbInfo.Orb_Type) && EquipFlag)
            {
                foreach (byte chkSlot in Item_Define.UltimateOrbEquipSlot[sysOrbInfo.Orb_Type])
                {
                    if (chkSlot == SlotNum)
                    {
                        bEnableSlot = true;
                        break;
                    }
                }
            }

            if (!bEnableSlot)
                return Result_Define.eResult.ITEM_ULTIMATE_ORB_SLOT_INVALIDE;

            if (findOrb.ultimate_inven_seq > 0 && EquipFlag)
                return Result_Define.eResult.ITEM_ULTIMATE_ORB_ALREADY_EQUIP;

            var findEquipedOrb = orbList.Find(orbfind => orbfind.ultimate_inven_seq == Ultimate_InvenSeq && orbfind.slot_num == SlotNum);
            if (findEquipedOrb != null)
                return Result_Define.eResult.ITEM_ULTIMATE_ORB_SLOT_ALREADY_EQUIPED;

            if (SlotNum == 0)
            {
                findOrb.ultimate_inven_seq = 0;
                findOrb.slot_num = 0;
            }
            else
            {
                findOrb.ultimate_inven_seq = Ultimate_InvenSeq;
                findOrb.slot_num = SlotNum;
            }

            Result_Define.eResult retError = UpdateUltimateOrbInven(ref TB, AID, findOrb);

            if (retError == Result_Define.eResult.SUCCESS)
            {
                RemoveUltimateOrbCache(AID, Ultimate_InvenSeq);
                RemoveUltimateOrbCache(AID, 0);
                updateOrdInfo = findOrb;
            }

            return retError;
        }

        public static Result_Define.eResult MixUltimateOrb(ref TxnBlock TB, long AID, long CID, long Ultimate_InvenSeq, long Orb_InvenSeq, List<long> materialSeqList, ref User_Ultimate_Inven updateInfo, ref User_Orb_Inven updateOrdInfo, string dbkey = Item_Define.Item_InvenDB)
        {
            if (materialSeqList.Contains(Orb_InvenSeq))
                return Result_Define.eResult.SYSTEM_PARAM_ERROR;

            List<User_Orb_Inven> orbList = GetUserOrbInvenList(ref TB, AID);
            orbList.AddRange(GetUserOrbInvenList(ref TB, AID, Ultimate_InvenSeq));

            var findOrb = orbList.Find(orbitem => orbitem.orb_inven_seq == Orb_InvenSeq);
            if (findOrb == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_Ultimate_Orb sysOrbInfo = GetSystem_Ultimate_Orb(ref TB, findOrb.orb_id);
            if (sysOrbInfo.Orb_Item_ID <= 0)
                return Result_Define.eResult.ITEM_SYSTEM_ID_NOT_FOUND;

            int getExp = 0;
            List<User_Orb_Inven> deletedOrbList = new List<User_Orb_Inven>();
            foreach (long setOrbSeq in materialSeqList)
            {
                var setOrb = orbList.Find(orbitem => orbitem.orb_inven_seq == setOrbSeq);
                if (setOrb == null)
                    return Result_Define.eResult.ITEM_ID_NOT_FOUND;

                System_Ultimate_Orb matOrbInfo = GetSystem_Ultimate_Orb(ref TB, setOrb.orb_id);
                if (matOrbInfo.Orb_Item_ID <= 0)
                    return Result_Define.eResult.ITEM_SYSTEM_ID_NOT_FOUND;

                getExp += (matOrbInfo.Base_EXP + setOrb.exp);
                setOrb.delflag = "Y";
                deletedOrbList.Add(setOrb);
            }

            findOrb.exp += getExp;

            while (findOrb.exp >= sysOrbInfo.LevelUp_EXP)
            {
                if (sysOrbInfo.LevelUp_EXP <= 0 || sysOrbInfo.NextItemID <= 0)
                {
                    findOrb.exp = sysOrbInfo.LevelUp_EXP;
                    break;
                }

                findOrb.exp -= sysOrbInfo.LevelUp_EXP;
                findOrb.orb_id = sysOrbInfo.NextItemID;
                sysOrbInfo = GetSystem_Ultimate_Orb(ref TB, findOrb.orb_id);
                if (sysOrbInfo.Orb_Item_ID <= 0)
                    return Result_Define.eResult.ITEM_SYSTEM_ID_NOT_FOUND;
            }

            Result_Define.eResult retError = UpdateUltimateOrbInven(ref TB, AID, findOrb);
            if (retError == Result_Define.eResult.SUCCESS)
            {
                foreach (User_Orb_Inven setDelete in deletedOrbList)
                {
                    retError = UpdateUltimateOrbInven(ref TB, AID, setDelete);
                    if (retError != Result_Define.eResult.SUCCESS)
                        return retError;
                }

                updateOrdInfo = findOrb;
            }

            if (findOrb.ultimate_inven_seq > 0)
            {
                RemoveUltimateWeaonCache(AID, CID);
                updateInfo = GetUserUltimateWeaponList(ref TB, AID, CID).Find(item => item.ultimate_inven_seq == findOrb.ultimate_inven_seq);
            }
            return retError;
        }

        public static Result_Define.eResult UpdateUltimateOrbInven(ref TxnBlock TB, long AID, User_Orb_Inven setItem, string dbkey = Item_Define.Item_InvenDB)
        {
            string setQuery = string.Format(@"UPDATE {0}
                                                    SET
                                                        orb_id = {1},
                                                        ultimate_inven_seq = {2},
                                                        slot_num = {3},
                                                        exp = {4},
                                                        delflag = '{5}'
                                                WHERE orb_inven_seq = {6} AND aid = {7}"
                                            , Item_Define.Item_User_Orb_Inven_Table
                                            , setItem.orb_id
                                            , setItem.ultimate_inven_seq
                                            , setItem.slot_num
                                            , setItem.exp
                                            , setItem.delflag
                                            , setItem.orb_inven_seq
                                            , AID
                                            );
            RemoveUltimateOrbCache(AID, setItem.ultimate_inven_seq);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }
    }
}
