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
        private static User_Inven_Option GetUser_Inven_Option(ref TxnBlock TB, long invenseq, long optionseq, string dbkey = Item_Define.Item_InvenDB)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE optionseq = {1} AND invenseq = {2}", Item_Define.Item_User_Inven_Option_Table, optionseq, invenseq);
            return TheSoul.DataManager.GenericFetch.FetchFromDB<User_Inven_Option>(ref TB, setQuery, dbkey);
        }

        private static List<User_Inven_Option> GetUser_Inven_Option_List(ref TxnBlock TB, long invenseq, string dbkey = Item_Define.Item_InvenDB)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE invenseq = {1}", Item_Define.Item_User_Inven_Option_Table, invenseq);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_Inven_Option>(ref TB, setQuery, dbkey);
        }

        // Accessory pick random option
        public static Result_Define.eResult AccessoryOptionAdd(ref TxnBlock TB, long AID, long CID, long InvenSeq, long optionseq, long materialSeq, ref List<Return_DisassableItems_List> retDeletedItem, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID, CID);

            var findItem = itemList.Find(item => item.invenseq == InvenSeq);
            var materialItem = itemList.Find(item => item.invenseq == materialSeq);

            if (findItem == null || materialItem == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            User_Inven_Option findOption = findItem.random_option.Find(option => option.optionseq == optionseq);

            if (optionseq > 0 && findOption == null)
                return Result_Define.eResult.ITEM_OPTION_ID_NOT_FOUND;
            else if (findItem.random_option.Count > Item_Define.Item_Accessory_Option_Max_Count)
                return Result_Define.eResult.ITEM_OPTION_MAX;

            System_Item_Equip baseItemInfo = GetSystem_Item_Equip(ref TB, findItem.itemid);

            Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];

            if (!Item_Define.ItemAccessoryTypeList.Contains(setItemType))
                return Result_Define.eResult.ITEM_TUNING_TYPE_INVALIDE;

            int totalRate = 0;
            Dictionary<long, int> setOptionRateList = new Dictionary<long, int>();

            foreach (User_Inven_Option item in materialItem.random_option)
            {
                totalRate += SystemData.GetConstValueInt(ref TB, Item_Define.Item_Const_Def_Option_Grade_Weight[item.option_grade]);
                setOptionRateList.Add(item.optionseq, totalRate);
            }

            int currentRate = TheSoul.DataManager.Math.GetRandomInt(0, totalRate);            
            User_Inven_Option setoption = null;

            foreach (KeyValuePair<long, int> pickItem in setOptionRateList)
            {
                if (currentRate <= pickItem.Value)
                {
                    setoption = materialItem.random_option.Find(item => item.optionseq == pickItem.Key);
                    break;
                }
            }

            if (setoption == null)
                return Result_Define.eResult.ITEM_OPTION_ID_NOT_FOUND;

            Result_Define.eResult retError = ItemManager.DeleteItem(ref TB, AID, materialItem.invenseq, ref retDeletedItem);

            if (retError == Result_Define.eResult.SUCCESS)
            {
                System_Item_Grade_Accessory equipBaseInfo = GetSystem_Item_Grade_Accessory(ref TB, baseItemInfo.GrowthTableID2, findItem.grade);    // accessory use grade table for base option
                retError = AccountManager.UseUserGold(ref TB, AID, equipBaseInfo.OptionGold_One);
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                if (optionseq > 0)
                {
                    User_Inven_Option.CopyOption(ref findOption, ref setoption);
                    retError = UpdateOptionInfo(ref TB, findOption);
                }
                else
                {
                    User_Inven_Option setNewOption = new User_Inven_Option();
                    User_Inven_Option.CopyOption(ref setNewOption, ref setoption);
                    setNewOption.delflag = "N";
                    setNewOption.isbase = "N";
                    setNewOption.invenseq = InvenSeq;
                    retError = MakeItemOptionToDB(ref TB, ref setNewOption);
                }
            }

            if (retError == Result_Define.eResult.SUCCESS)
                retError = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.ACCESSORY_REMODELING);

            return retError;
        }

        // Accessory set random option
        public static Result_Define.eResult AccessoryOptionAddReroll(ref TxnBlock TB, long AID, long CID, long InvenSeq, long optionseq, long materialSeq, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID, CID);

            var findItem = itemList.Find(item => item.invenseq == InvenSeq);

            if (findItem == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            User_Inven_Option findOption = findItem.random_option.Find(option => option.optionseq == optionseq);
            if (optionseq > 0 && findOption == null)
                    return Result_Define.eResult.ITEM_OPTION_ID_NOT_FOUND;
            else if (findItem.random_option.Count > Item_Define.Item_Accessory_Option_Max_Count)
                return Result_Define.eResult.ITEM_OPTION_MAX;

            System_Item_Equip baseItemInfo = GetSystem_Item_Equip(ref TB, findItem.itemid);
            Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];

            if (!Item_Define.ItemAccessoryTypeList.Contains(setItemType))
                return Result_Define.eResult.ITEM_TUNING_TYPE_INVALIDE;

            List<User_Inven_Option> random_option = GetUser_Inven_Option_List(ref TB, materialSeq).FindAll(item => item.isbase.Equals("N"));
            
            int totalRate = 0;
            Dictionary<long, int> setOptionRateList = new Dictionary<long, int>();

            foreach (User_Inven_Option item in random_option)
            {
                totalRate += SystemData.GetConstValueInt(ref TB, Item_Define.Item_Const_Def_Option_Grade_Weight[item.option_grade]);
                setOptionRateList.Add(item.optionseq, totalRate);
            }

            int currentRate = TheSoul.DataManager.Math.GetRandomInt(0, totalRate);
            User_Inven_Option setoption = null;

            foreach (KeyValuePair<long, int> pickItem in setOptionRateList)
            {
                if (currentRate <= pickItem.Value)
                {
                    setoption = random_option.Find(item => item.optionseq == pickItem.Key);
                    break;
                }
            }

            if (setoption == null)
                return Result_Define.eResult.ITEM_OPTION_ID_NOT_FOUND;

            User_Inven_Option.CopyOption(ref findOption, ref setoption);
            Result_Define.eResult retError = UpdateOptionInfo(ref TB, findOption);

            if (retError == Result_Define.eResult.SUCCESS)
            {
                int useRerollRuby = SystemData.GetConstValueInt(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_RUBYRETRY_MOD]);
                if (useRerollRuby > 0)
                    retError = AccountManager.UseUserCash(ref TB, AID, useRerollRuby);
            }

            if (retError == Result_Define.eResult.SUCCESS)
                retError = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.ACCESSORY_REMODELING);

            return retError;
        }
        
        // Accessory Change All
        public static Result_Define.eResult AccessoryOptionChangeAll(ref TxnBlock TB, long AID, long CID, long InvenSeq, long materialSeq, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID, CID);

            var findItem = itemList.Find(item => item.invenseq == InvenSeq);
            var materialItem = itemList.Find(item => item.invenseq == materialSeq);

            if (findItem == null || materialItem == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            if (findItem.grade < Item_Define.Item_Accessory_Option_Change_Min_Grade || materialItem.grade < Item_Define.Item_Accessory_Option_Change_Min_Grade)
                return Result_Define.eResult.ITEM_TUNING_NOT_ENOUGH_GRADE;

            List<System_Item_Equip> baseItemInfo = new List<System_Item_Equip>() {
                 GetSystem_Item_Equip(ref TB, findItem.itemid),
                 GetSystem_Item_Equip(ref TB, materialItem.itemid)
            };

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            baseItemInfo.ForEach(item =>           // check item type
            {
                if (!Item_Define.ItemAccessoryTypeList.Contains((Item_Define.eItemType)Item_Define.ItemType[item.ItemType]) && retError == Result_Define.eResult.SUCCESS)
                    retError =  Result_Define.eResult.ITEM_TUNING_TYPE_INVALIDE;
            });

            if (retError == Result_Define.eResult.SUCCESS)
            {
                int maxTire = baseItemInfo.Max(item => item.Tier);
                int TierPrice = SystemData.GetConstValueInt(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_GOLD_OPTIONCHANGEALL_EACHTIER]);
                retError = AccountManager.UseUserGold(ref TB, AID, maxTire * TierPrice);
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                foreach (User_Inven_Option item in findItem.random_option)
                {
                    item.invenseq = materialItem.invenseq;
                    retError = UpdateOptionInvenSeq(ref TB, item);
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                foreach (User_Inven_Option item in materialItem.random_option)
                {
                    item.invenseq = findItem.invenseq;
                    retError = UpdateOptionInvenSeq(ref TB, item);
                    if (retError != Result_Define.eResult.SUCCESS)
                        break;
                }
            }

            if (retError == Result_Define.eResult.SUCCESS)
                retError = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.ACCESSORY_REMODELING);

            return retError;
        }


        /*
        // get system item evolution accessory table use by redis hash
        const string SystemItem_Evolution_Accessory_TableName = "System_ITEM_EVOL_ACCESSORY";
        const string SystemItem_Evolution_Accessory_Surfix_Class = "Evolution_Accessory";

        private static System_ITEM_EVOL_ACCESSORY GetSystemInfo_Evolution_Accessory(ref TxnBlock TB, long EvolutionIndex, int EvolutionGrade, bool Flush = false, string dbkey = InvenDBName)
        {
            string setKey = string.Format("{0}_{1}_{2}", SystemItemPrefix, SystemItem_Evolution_Accessory_TableName, SystemItem_Evolution_Accessory_Surfix_Class);
            string setQuery = string.Format("SELECT * FROM {0} AS GetEvol, (SELECT EvolGroup FROM {0} WHERE Evol_IndexID = {1}) AS BaseEvol WHERE BaseEvol.EvolGroup = GetEvol.EvolGroup AND Grade = {2}",
                                            SystemItem_Evolution_Accessory_TableName, EvolutionIndex, EvolutionGrade);
            string memebrKey = string.Format("{0}_{1}", EvolutionIndex, EvolutionGrade);
            System_ITEM_EVOL_ACCESSORY retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_ITEM_EVOL_ACCESSORY>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, memebrKey, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_ITEM_EVOL_ACCESSORY();
            return retObj;
        }

        // accessory item Evolution (Grade Up)
        public static Result_Define.eResult EvolutionAccessory(ref TxnBlock TB, long AID, long CID, long ItemID, int useRuby, ref bool bSuccess, ref List<Return_DisassableItems_List> retDeletedItem, string dbkey = InvenDBName)
        {
            Dictionary<long, User_Inven> itemList = GetInvenList(ref TB, AID, CID, dbkey);
            if (!itemList.ContainsKey(ItemID))
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_ITEM_EQUIP baseItemInfo = GetSystemInfo_EquipItem(ref TB, itemList[ItemID].itemid);

            Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];

            if (!Item_Define.ItemAccessoryTypeList.Contains(setItemType))
                return Result_Define.eResult.ITEM_EVOLUTION_TYPE_INVALIDE;

            short baseTier = baseItemInfo.Tier;
            short baseGrade = itemList[ItemID].enchant_grade;
            short baseLevel = itemList[ItemID].enchant_level;

            if (baseGrade >= Item_Define.ItemMaxEvolutionGrade)
                return Result_Define.eResult.ITEM_EVOLUTION_GRADE_MAX;

            System_ITEM_EVOL_ACCESSORY baseEvolutionInfo = GetSystemInfo_Evolution_Accessory(ref TB, baseItemInfo.GrowthTableID1, baseGrade);

            int calcEnchantPrice = baseEvolutionInfo.GradeUP_NeedGold;

            Result_Define.eResult retUseResult = AccountManager.UseUserGold(ref TB, AID, calcEnchantPrice);
            if (retUseResult != Result_Define.eResult.SUCCESS)
                return retUseResult;

            if (baseEvolutionInfo.GradeUP_NeedItemID > 0 && baseEvolutionInfo.GradeUP_NeedItemCount > 0)
            {
                retUseResult = ItemManager.UseItem(ref TB, AID, baseEvolutionInfo.GradeUP_NeedItemID, baseEvolutionInfo.GradeUP_NeedItemCount, ref retDeletedItem);
                if (retUseResult != Result_Define.eResult.SUCCESS)
                    return retUseResult;
            }
            else
                return Result_Define.eResult.ITEM_EVOLUTION_DB_NOT_FOUND;

            double curRate = TheSoul.DataManager.Math.GetRandomDouble(0, Item_Define.Item_MaxRate);
            double checkrate = baseEvolutionInfo.ProbabilityValue + (useRuby * (System.Convert.ToDouble(baseEvolutionInfo.ProbabilityUpCash) / 10));

            if (checkrate >= curRate)
            {
                baseGrade++;
                List<User_Inven_Option_type> currentOptionList = new List<User_Inven_Option_type>();

                if (!string.IsNullOrEmpty(itemList[ItemID].optiontype1)) currentOptionList.Add(new User_Inven_Option_type(itemList[ItemID].optiontype1, itemList[ItemID].optionvalue1));
                if (!string.IsNullOrEmpty(itemList[ItemID].optiontype2)) currentOptionList.Add(new User_Inven_Option_type(itemList[ItemID].optiontype2, itemList[ItemID].optionvalue2));
                if (!string.IsNullOrEmpty(itemList[ItemID].optiontype3)) currentOptionList.Add(new User_Inven_Option_type(itemList[ItemID].optiontype3, itemList[ItemID].optionvalue3));
                if (!string.IsNullOrEmpty(itemList[ItemID].optiontype4)) currentOptionList.Add(new User_Inven_Option_type(itemList[ItemID].optiontype4, itemList[ItemID].optionvalue4));
                if (!string.IsNullOrEmpty(itemList[ItemID].optiontype5)) currentOptionList.Add(new User_Inven_Option_type(itemList[ItemID].optiontype5, itemList[ItemID].optionvalue5));

                bool setAddOption = true;
                while (setAddOption)
                {
                    List<User_Inven_Option_type> getOptionList = GetSystemItemOption(ref TB, setItemType, baseGrade, baseTier, dbkey);

                    if (getOptionList.Count > 0)
                    {
                        foreach (User_Inven_Option_type setOption in getOptionList)
                        {
                            if (getOptionList.Count > (currentOptionList.Count - itemList[ItemID].fixoption))
                            {
                                bool isAdd = true;
                                foreach (User_Inven_Option_type haveOption in currentOptionList)
                                {
                                    if (haveOption.optiontype.Equals(setOption.optiontype))
                                    {
                                        isAdd = false;
                                        break;
                                    }
                                }
                                if (isAdd)
                                    currentOptionList.Add(setOption);
                            }
                            else
                                setAddOption = false;

                        }
                    }
                    else
                        setAddOption = false;

                }

                User_Inven setItem = new User_Inven();
                int OptionPos = 0;
                foreach (User_Inven_Option_type setOption in currentOptionList)
                {
                    if (OptionPos < Item_Define.Item_MaxOptionCount)
                    {
                        OptionPos++;
                        setItem[string.Format("{0}{1}", Item_Define.Item_OptionTypeColumn, OptionPos)] = setOption.optiontype;
                        setItem[string.Format("{0}{1}", Item_Define.Item_OptionValueColumn, OptionPos)] = setOption.optionvalue;
                    }
                }

                for (OptionPos++; OptionPos <= Item_Define.Item_MaxOptionCount; OptionPos++)
                {
                    setItem[string.Format("{0}{1}", Item_Define.Item_OptionTypeColumn, OptionPos)] = string.Empty;
                    setItem[string.Format("{0}{1}", Item_Define.Item_OptionValueColumn, OptionPos)] = 0;
                }

                string setQuery = string.Format(@"UPDATE {0} SET    enchant_grade = {1},
                                                                optiontype1 = '{2}', optionvalue1 = {3},
                                                                optiontype2 = '{4}', optionvalue2 = {5},
                                                                optiontype3 = '{6}', optionvalue3 = {7},
                                                                optiontype4 = '{8}', optionvalue4 = {9},
                                                                optiontype5 = '{10}', optionvalue5 = {11}
                                                    WHERE AID = {12} AND invenseq = {13}",
                                                        InvenDBTableName, baseGrade,
                                                        setItem.optiontype1, setItem.optionvalue1,
                                                        setItem.optiontype2, setItem.optionvalue2,
                                                        setItem.optiontype3, setItem.optionvalue3,
                                                        setItem.optiontype4, setItem.optionvalue4,
                                                        setItem.optiontype5, setItem.optionvalue5,
                                                        AID, ItemID);

                if (!TB.ExcuteSqlCommand(InvenDBName, setQuery))
                    return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                bSuccess = true;
                FlushInvenList(ref TB, AID);
            }

            return Result_Define.eResult.SUCCESS;
        }


        // Accessory Item Disassemble 
        public static Result_Define.eResult DisassembleAccessory(ref TxnBlock TB, long AID, long CID, ref List<Return_DisassableItems> resultSet, List<long> material_itemID_List, ref List<Return_DisassableItems_List> retDeletedItem, string dbkey = InvenDBName)
        {
            Dictionary<long, User_Inven> itemList = GetInvenList(ref TB, AID, CID, dbkey);
            Dictionary<long, long> RetGetItems = new Dictionary<long, long>();
            foreach (long targetItemID in material_itemID_List)
            {
                if (!itemList.ContainsKey(targetItemID))
                    return Result_Define.eResult.ITEM_ID_NOT_FOUND;
                else if (itemList[targetItemID].equipflag == DataManager_Define.DB_CHAR_FLAG_TRUE)
                    return Result_Define.eResult.EQUIPITEM_CHOOSE_ERROR;
                else if (itemList[targetItemID].delflag == DataManager_Define.DB_CHAR_FLAG_TRUE)
                    return Result_Define.eResult.DELETEITEM_CHOOSE_ERROR;

                System_ITEM_EQUIP baseItemInfo = GetSystemInfo_EquipItem(ref TB, itemList[targetItemID].itemid);
                Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];

                if (!Item_Define.ItemAccessoryTypeList.Contains(setItemType))
                    return Result_Define.eResult.ITEM_DISASSAEMBLE_TYPE_INVALIDE;

                short baseTier = baseItemInfo.Tier;
                short baseGrade = itemList[targetItemID].enchant_grade;
                short baseLevel = itemList[targetItemID].enchant_level;

                System_ITEM_EVOL_ACCESSORY baseEvolutionInfo = GetSystemInfo_Evolution_Accessory(ref TB, baseItemInfo.GrowthTableID1, baseGrade);
                RetGetItems[baseEvolutionInfo.Disassemble_MakeItemID] = RetGetItems.ContainsKey(baseEvolutionInfo.Disassemble_MakeItemID) ?
                                        RetGetItems[baseEvolutionInfo.Disassemble_MakeItemID] + baseEvolutionInfo.Disassemble_MakeItemCount :
                                            baseEvolutionInfo.Disassemble_MakeItemCount;
            }

            foreach (long targetItemID in material_itemID_List)
            {
                if (!ItemManager.DeleteItem(ref TB, AID, targetItemID, itemList[targetItemID].itemid, ref retDeletedItem))
                    return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }

            foreach (KeyValuePair<long, long> makeItem in RetGetItems)
            {
                User_Inven[] makeInvenItem = ItemManager.MakeItem(ref TB, AID, makeItem.Key, System.Convert.ToInt32(makeItem.Value), CID);
                if (makeInvenItem == null)
                    return Result_Define.eResult.ITEM_CREATE_FAIL;
                Return_DisassableItems setMakeItem = new Return_DisassableItems();
                setMakeItem.makeitemid = makeItem.Key;
                setMakeItem.makeitemcount = makeItem.Value;

                setMakeItem.makeitemlist = new Return_DisassableItems_List[makeInvenItem.Length];
                int setPos = 0;
                foreach (User_Inven setItemseq in makeInvenItem)
                {
                    setMakeItem.makeitemlist[setPos] = new Return_DisassableItems_List();
                    setMakeItem.makeitemlist[setPos].itemid = setItemseq.itemid;
                    setMakeItem.makeitemlist[setPos].itemseq = setItemseq.invenseq;
                    setMakeItem.makeitemlist[setPos].itemea = setItemseq.itemea;
                    setPos++;
                }

                resultSet.Add(setMakeItem);
            }

            FlushInvenList(ref TB, AID);

            return Result_Define.eResult.SUCCESS;
        }

        // make cape item from cape piece
        private static Result_Define.eResult UseCapePiece(ref TxnBlock TB, long AID, long CID, long ItemID, ref List<Return_DisassableItems_List> retDeletedItem, ref User_Inven MakeItemInfo)
        {
            long NeedItemID = ItemID;

            System_ITEM_USE UseInfo = GetSystemInfo_UseItem(ref TB, NeedItemID);
            List<System_ITEM_USE> TierGroupInfo = GetSystemInfo_UseItem_TierGroup(ref TB, UseInfo.TierGroup);

            int NeedItemCount = UseInfo.ConditionValue;

            Result_Define.eResult retUseResult = ItemManager.UseItem(ref TB, AID, NeedItemID, NeedItemCount, ref retDeletedItem);
            if (retUseResult != Result_Define.eResult.SUCCESS)
                return retUseResult;

            Character CharInfo = CharacterManager.GetCharacter(ref TB, AID, CID);

            Character_Define.SystemClassType setClass = (Character_Define.SystemClassType)CharInfo.Class;
            long makeItemID = 0;
            foreach (System_ITEM_USE chkInfo in TierGroupInfo)
            {
                if (Character_Define.ClassTypeToEnum.ContainsKey(chkInfo.TargetCondition))
                {
                    if (Character_Define.ClassTypeToEnum[chkInfo.TargetCondition] == setClass)
                    {
                        makeItemID = chkInfo.Target_IndexID;
                        break;
                    }
                }
            }

            if (makeItemID <= 0)
                return Result_Define.eResult.System_ItemBase_NOT_FOUND;

            User_Inven[] makeInvenItem = ItemManager.MakeItem(ref TB, AID, makeItemID, 1, CID);
            if (makeInvenItem == null || makeInvenItem.Length <= 0)
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            MakeItemInfo = makeInvenItem[0];

            return Result_Define.eResult.SUCCESS;
        }
         
         */
    }
}
