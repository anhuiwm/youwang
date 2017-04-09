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
        private static Result_Define.eResult UpdateArmorOption(ref TxnBlock TB, System_Item_Enchant_Armor setEnchantInfo, List<User_Inven_Option> currentOption, string dbkey = Item_Define.Item_InvenDB)
        {
            setEnchantInfo = GetSystem_Item_Enchant_Armor(ref TB, setEnchantInfo.EnchantGroup, setEnchantInfo.EnchantGrade, setEnchantInfo.EnchantLevel);

            if (setEnchantInfo.Enchant_IndexID > 0)
            {
                User_Inven_Option setMainOption = currentOption.FirstOrDefault();
                User_Inven_Option setSecondOption = currentOption.ElementAt(1);

                if (setEnchantInfo.MainParameter_Type.Length > 0 && setEnchantInfo.MainParameter_Value1 > 0)
                {
                    setMainOption.optiontype = setEnchantInfo.MainParameter_Type;
                    setMainOption.option_value = setEnchantInfo.MainParameter_Value1;
                }
                if (setEnchantInfo.SubParameter_Type.Length > 0 && setEnchantInfo.SubParameter_Value1 > 0)
                {
                    setSecondOption.optiontype = setEnchantInfo.SubParameter_Type;
                    setSecondOption.option_value = setEnchantInfo.SubParameter_Value1;
                }

                Result_Define.eResult retError = ItemManager.UpdateOptionInfo(ref TB, setMainOption, dbkey);
                if(retError == Result_Define.eResult.SUCCESS)
                    retError = ItemManager.UpdateOptionInfo(ref TB, setSecondOption, dbkey);

                return retError;
            }
            else
                return Result_Define.eResult.ITEM_ENCHANCE_DB_NOT_FOUND;
        }

        // check armore upgrade cost - enchant, evolotion, 
        private static Result_Define.eResult CheckUpgradeArmorCost(ref TxnBlock TB, long AID, int CharLevel, ref System_Item_Enchant_Armor setEnchantInfo, ref List<Return_DisassableItems_List> retDeletedItem, ref int needGold, string dbkey = Item_Define.Item_InvenDB)
        {
            if (setEnchantInfo.NeedLevel > CharLevel)
                return Result_Define.eResult.ITEM_EQUIP_NOT_ENOUGH_LEVEL;
            else if (setEnchantInfo.NeedLevel < 0)
                return Result_Define.eResult.ITEM_EQUIP_CAN_NOT_USE;

            Result_Define.eResult retError = setEnchantInfo.Enchant_IndexID > 0 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.ITEM_ENCHANCE_DB_NOT_FOUND;
            
            if (setEnchantInfo.NeedItem > 0 && setEnchantInfo.ItemCount > 0 && retError == Result_Define.eResult.SUCCESS)
                retError = ItemManager.UseItem(ref TB, AID, setEnchantInfo.NeedItem, setEnchantInfo.ItemCount, ref retDeletedItem);

            needGold += setEnchantInfo.NeedGold;
            //if (setEnchantInfo.NeedGold > 0 && retError == Result_Define.eResult.SUCCESS)
            //    retError = AccountManager.UseUserGold(ref TB, AID, setEnchantInfo.NeedGold);

            return retError;
        }


        // check armore upgrade cost - enchant, evolotion, 
        private static Result_Define.eResult CheckMetalWorkUpgradeArmorCost(ref TxnBlock TB, long AID, int CharLevel, int HaveItemCount, ref bool bSuccess, ref System_Item_Enchant_Armor setEnchantInfo, ref List<Return_DisassableItems_List> retDeletedItem, string dbkey = Item_Define.Item_InvenDB)
        {
            if (setEnchantInfo.NeedLevel < 0)
                return Result_Define.eResult.ITEM_EQUIP_CAN_NOT_USE;

            if (setEnchantInfo.NeedLevel > CharLevel)
                return Result_Define.eResult.ITEM_EQUIP_NOT_ENOUGH_LEVEL;

            Result_Define.eResult retError = setEnchantInfo.Enchant_IndexID > 0 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.ITEM_ENCHANCE_DB_NOT_FOUND;
            
            double setRate = HaveItemCount >= setEnchantInfo.ItemCount ? 1.0f : System.Convert.ToDouble(HaveItemCount) / System.Convert.ToDouble(setEnchantInfo.ItemCount);
            double curRate = TheSoul.DataManager.Math.GetRandomDouble(0, 1.0f);

            bSuccess = (setRate >= curRate && setRate > 0);
            
            if (setEnchantInfo.NeedItem > 0 && retError == Result_Define.eResult.SUCCESS)
            {
                int UseCount = HaveItemCount >= setEnchantInfo.ItemCount ? setEnchantInfo.ItemCount : HaveItemCount;
                if (UseCount > 0)
                    retError = ItemManager.UseItem(ref TB, AID, setEnchantInfo.NeedItem, UseCount, ref retDeletedItem);
                else
                    retError = Result_Define.eResult.ITEM_ENCHANCE_DB_NOT_FOUND;
            }

            if (setEnchantInfo.NeedGold > 0 && retError == Result_Define.eResult.SUCCESS)
                retError = AccountManager.UseUserGold(ref TB, AID, setEnchantInfo.NeedGold);

            return retError;
        }

        // Armor item enchant up 
        public static Result_Define.eResult EnchantArmor(ref TxnBlock TB, long AID, long CID, long InvenSeq, ref List<Return_DisassableItems_List> retDeletedItem, byte enhanceCount = 1, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID, CID);
            
            var findItem = itemList.Find(item => item.invenseq == InvenSeq) ;
            if (findItem == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_Item_Equip baseItemInfo = GetSystem_Item_Equip(ref TB, findItem.itemid);

            Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];

            if (!Item_Define.ItemArmorTypeList.Contains(setItemType))
                return Result_Define.eResult.ITEM_ENCHANCE_TYPE_INVALIDE;

            if ((findItem.level + enhanceCount) > Item_Define.ItemMaxGradeLevel)
                return Result_Define.eResult.ITEM_ENCHANCE_LEVEL_MAX;

            Character charInfo = CharacterManager.GetCharacter(ref TB, AID, CID);

            System_Item_Enchant_Armor setEnchantInfo = GetSystem_Item_Enchant_Armor(ref TB, baseItemInfo.GrowthTableID1);

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            int needGold = 0;
            for (int checkCount = 0; checkCount < enhanceCount; checkCount++)
            {
                if (!(setEnchantInfo.EnchantGrade == findItem.grade && setEnchantInfo.EnchantLevel == findItem.level + checkCount))
                    setEnchantInfo = GetSystem_Item_Enchant_Armor(ref TB, setEnchantInfo.EnchantGroup, findItem.grade, findItem.level + checkCount);

                retError = CheckUpgradeArmorCost(ref TB, AID, charInfo.level, ref setEnchantInfo, ref retDeletedItem, ref needGold, dbkey);
                if (retError != Result_Define.eResult.SUCCESS)
                    return retError;
            }

            if (needGold > 0 && retError == Result_Define.eResult.SUCCESS)
                retError = AccountManager.UseUserGold(ref TB, AID, needGold);

            if(retError == Result_Define.eResult.SUCCESS)
            {
                findItem.level+= enhanceCount;
                retError = ItemManager.UpdateItemInfo(ref TB, findItem);
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                setEnchantInfo.EnchantLevel = findItem.level;
                retError = ItemManager.UpdateArmorOption(ref TB, setEnchantInfo, findItem.base_option, dbkey);
            }

            if (retError == Result_Define.eResult.SUCCESS)
                retError = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.Armor_LvUp, 0, 0, enhanceCount);

            return retError;
        }


        // Armor Item Grade Up 
        public static Result_Define.eResult EvolutionArmor(ref TxnBlock TB, long AID, long CID, long InvenSeq, ref List<Return_DisassableItems_List> retDeletedItem, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID, CID);

            var findItem = itemList.Find(item => item.invenseq == InvenSeq);
            if (findItem == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_Item_Equip baseItemInfo = GetSystem_Item_Equip(ref TB, findItem.itemid);

            Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];

            if (!Item_Define.ItemArmorTypeList.Contains(setItemType))
                return Result_Define.eResult.ITEM_ENCHANCE_TYPE_INVALIDE;
            
            if (findItem.level < Item_Define.ItemMaxGradeLevel)
                return Result_Define.eResult.ITEM_EVOLUTION_LEVEL_NOT_ENOUGH;
            if ((findItem.grade +1) > Item_Define.ItemMaxEvolutionGrade)
                return Result_Define.eResult.ITEM_EVOLUTION_GRADE_MAX;

            System_Item_Enchant_Armor setEnchantInfo = GetSystem_Item_Enchant_Armor(ref TB, baseItemInfo.GrowthTableID1);
            if (!(setEnchantInfo.EnchantGrade == findItem.grade && setEnchantInfo.EnchantLevel == findItem.level))
                setEnchantInfo = GetSystem_Item_Enchant_Armor(ref TB, setEnchantInfo.EnchantGroup, findItem.grade, findItem.level);

            Character charInfo = CharacterManager.GetCharacter(ref TB, AID, CID);

            int needGold = 0;
            Result_Define.eResult retError = CheckUpgradeArmorCost(ref TB, AID, charInfo.level, ref setEnchantInfo, ref retDeletedItem, ref needGold, dbkey);
            
            if (needGold > 0 && retError == Result_Define.eResult.SUCCESS)
                retError = AccountManager.UseUserGold(ref TB, AID, needGold);

            if (retError == Result_Define.eResult.SUCCESS)
            {
                findItem.grade++;
                findItem.level = 0;
                retError = ItemManager.UpdateItemInfo(ref TB, findItem);
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                setEnchantInfo = GetSystem_Item_Enchant_Armor(ref TB, setEnchantInfo.EnchantGroup, findItem.grade, findItem.level);
                retError = ItemManager.UpdateArmorOption(ref TB, setEnchantInfo, findItem.base_option, dbkey);
            }

            if (retError == Result_Define.eResult.SUCCESS)
                retError = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.Armor_GradeUp);


            return retError;
        }

        // Armor Item Grade Up 
        public static Result_Define.eResult MetalWorkArmor(ref TxnBlock TB, long AID, long CID, long InvenSeq, ref bool bSuccess, ref List<Return_DisassableItems_List> retDeletedItem, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID, CID);

            var findItem = itemList.Find(item => item.invenseq == InvenSeq);
            if (findItem == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_Item_Equip baseItemInfo = GetSystem_Item_Equip(ref TB, findItem.itemid);

            Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];

            if (!Item_Define.ItemArmorTypeList.Contains(setItemType))
                return Result_Define.eResult.ITEM_ENCHANCE_TYPE_INVALIDE;
            
            if (findItem.level < Item_Define.ItemMaxGradeLevel)
                return Result_Define.eResult.ITEM_EVOLUTION_LEVEL_NOT_ENOUGH;
            if (findItem.grade < Item_Define.ItemMaxEvolutionGrade)
                return Result_Define.eResult.ITEM_EVOLUTION_GRADE_NOT_ENOUGH;

            System_Item_Enchant_Armor setEnchantInfo = GetSystem_Item_Enchant_Armor(ref TB, baseItemInfo.GrowthTableID1);
            if (!(setEnchantInfo.EnchantGrade == findItem.grade && setEnchantInfo.EnchantLevel == findItem.level))
                setEnchantInfo = GetSystem_Item_Enchant_Armor(ref TB, setEnchantInfo.EnchantGroup, findItem.grade, findItem.level);
            
            int haveCount = itemList.FindAll(item => item.itemid == setEnchantInfo.NeedItem).Sum(item => item.itemea);
            if (haveCount < 1)
                return Result_Define.eResult.NOT_ENOUGH_USE_ITEM;
            
            baseItemInfo = GetSystem_Item_Equip(ref TB, setEnchantInfo.NextItem);

            Character charInfo = CharacterManager.GetCharacter(ref TB, AID, CID);
            //Result_Define.eResult retError = charInfo.level >= baseItemInfo.EquipLevel ? Result_Define.eResult.SUCCESS : Result_Define.eResult.ITEM_EQUIP_NOT_ENOUGH_LEVEL;
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            if (retError == Result_Define.eResult.SUCCESS)
                retError = CheckMetalWorkUpgradeArmorCost(ref TB, AID, charInfo.level, haveCount, ref bSuccess, ref setEnchantInfo, ref retDeletedItem, dbkey);

            if (retError == Result_Define.eResult.SUCCESS && bSuccess)
            {
                if (baseItemInfo.Item_IndexID > 0)
                {
                    findItem.itemid = baseItemInfo.Item_IndexID;
                    findItem.grade = Item_Define.Item_Armor_Base_Grade;
                    findItem.level = Item_Define.Item_Armor_Base_Level;
                    retError = ItemManager.UpdateItemInfo(ref TB, findItem);
                }
                else
                    retError = Result_Define.eResult.System_ItemBase_NOT_FOUND;
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                baseItemInfo = GetSystem_Item_Equip(ref TB, findItem.itemid);
                setEnchantInfo = GetSystem_Item_Enchant_Armor(ref TB, baseItemInfo.GrowthTableID1);
                if (!(setEnchantInfo.EnchantGrade == findItem.grade && setEnchantInfo.EnchantLevel == findItem.level))
                    setEnchantInfo = GetSystem_Item_Enchant_Armor(ref TB, setEnchantInfo.EnchantGroup, findItem.grade, findItem.level); 
                retError = ItemManager.UpdateArmorOption(ref TB, setEnchantInfo, findItem.base_option, dbkey);
            }

            if (retError == Result_Define.eResult.SUCCESS)
                retError = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.ARMOR_METALWORK);

            return retError;
        }

        /*
        // get system item get exp table use by redis hash
        const string SystemItem_GetExp_TableName = "System_ITEM_GETEXP";
        const string SystemItem_GetExp_Surfix_Class = "GetExp";

        private static System_ITEM_GETEXP GetSystemInfo_GetExp(ref TxnBlock TB, short grade, short Tier, bool Flush = false, string dbkey = InvenDBName)
        {
            string setKey = string.Format("{0}_{1}_{2}", SystemItemPrefix, SystemItem_GetExp_TableName, SystemItem_GetExp_Surfix_Class);
            string setQuery = string.Format("SELECT * FROM {0} WHERE GetEXP_Tier = {1} AND GetEXP_Grade = {2}", SystemItem_GetExp_TableName, Tier, grade);
            string memebrKey = string.Format("{0}_{1}", Tier, grade);
            System_ITEM_GETEXP retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_ITEM_GETEXP>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, memebrKey, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_ITEM_GETEXP();
            return retObj;
        }

        // get system item need exp table use by redis hash
        const string SystemItem_NeedExp_TableName = "System_ITEM_NEEDEXP";
        const string SystemItem_NeedExp_Surfix_Class = "NeedExp";

        private static System_ITEM_NEEDEXP GetSystemInfo_NeedExp(ref TxnBlock TB, int ExpGroup, short EnchantGrade, short EnchantLevel, bool Flush = false, string dbkey = InvenDBName)
        {
            string setKey = string.Format("{0}_{1}_{2}", SystemItemPrefix, SystemItem_NeedExp_TableName, SystemItem_NeedExp_Surfix_Class);
            string setQuery = string.Format("SELECT * FROM {0} WHERE EXPGroup = {1} AND EXPGrade = {2} AND EnchantLevel = {3}", SystemItem_NeedExp_TableName, ExpGroup, EnchantGrade, EnchantLevel);
            string memebrKey = string.Format("{0}_{1}_{2}", ExpGroup, EnchantGrade, EnchantLevel);
            System_ITEM_NEEDEXP retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_ITEM_NEEDEXP>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, memebrKey, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_ITEM_NEEDEXP();
            return retObj;
        }

        // Armor item enchant up 
        public static Result_Define.eResult EnchantArmor(ref TxnBlock TB, long AID, long CID, long ItemID, ref long calcGetExp, ref short baseLevel, List<long> material_itemID_List, ref List<Return_DisassableItems_List> retDeletedItem, string dbkey = InvenDBName)
        {
            Dictionary<long, User_Inven> itemList = GetInvenList(ref TB, AID, CID, dbkey);
            if (!itemList.ContainsKey(ItemID))
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_ITEM_EQUIP baseItemInfo = GetSystemInfo_EquipItem(ref TB, itemList[ItemID].itemid);

            Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];

            if (!Item_Define.ItemArmorTypeList.Contains(setItemType))
                return Result_Define.eResult.ITEM_ENCHANCE_TYPE_INVALIDE;

            short baseTier = baseItemInfo.Tier;
            short baseGrade = itemList[ItemID].enchant_grade;
            baseLevel = itemList[ItemID].enchant_level;
            calcGetExp = itemList[ItemID].enchant_exp;
            int calcEnchantPrice = 0;

            if (baseLevel >= Item_Define.ItemMaxGradeLevel)
            {
                baseLevel = Item_Define.ItemMaxGradeLevel;
                calcGetExp = 0;
                return Result_Define.eResult.ITEM_ENCHANCE_LEVEL_MAX;
            }

            foreach (long targetItemID in material_itemID_List)
            {
                if (!itemList.ContainsKey(targetItemID))
                    return Result_Define.eResult.ITEM_ID_NOT_FOUND;
                else if (itemList[targetItemID].equipflag == DataManager_Define.DB_CHAR_FLAG_TRUE)
                    return Result_Define.eResult.EQUIPITEM_CHOOSE_ERROR;
                else if (itemList[targetItemID].delflag == DataManager_Define.DB_CHAR_FLAG_TRUE)
                    return Result_Define.eResult.DELETEITEM_CHOOSE_ERROR;

                System_ITEM_EQUIP target_EquipInfo = GetSystemInfo_EquipItem(ref TB, itemList[targetItemID].itemid);

                short targetTier = target_EquipInfo.Tier;
                short targetGrade = itemList[targetItemID].enchant_grade;

                int TierDif = targetTier - baseTier;
                float setExpTierRate = (TierDif <= -3) ? Item_Define.Item_Grade_Diff_Rate * -3 :
                                            (TierDif >= 3) ? Item_Define.Item_Grade_Diff_Rate * 3 :
                                                Item_Define.Item_Grade_Diff_Rate * TierDif;
                setExpTierRate += 1.0f;

                calcGetExp += (int)System.Math.Round(GetSystemInfo_GetExp(ref TB, targetGrade, targetTier).GetEXP * setExpTierRate);

                bool bCalcEnd = false;
                while (!bCalcEnd)
                {
                    System_ITEM_NEEDEXP baseNeedExpInfo = GetSystemInfo_NeedExp(ref TB, baseItemInfo.GrowthTableID2, baseGrade, baseLevel);
                    calcEnchantPrice += baseNeedExpInfo.EnchantPrice;

                    if (calcGetExp > baseNeedExpInfo.NeedEXP && baseNeedExpInfo.NeedEXP > 0 && baseNeedExpInfo.EXP_IndexID != 0)
                    {
                        calcGetExp -= baseNeedExpInfo.NeedEXP;
                        baseLevel++;
                    }
                    else
                        bCalcEnd = true;
                }
            }

            if (baseLevel > Item_Define.ItemMaxGradeLevel)
            {
                baseLevel = Item_Define.ItemMaxGradeLevel;
                calcGetExp = 0;
            }

            Result_Define.eResult retUseResult = Result_Define.eResult.SUCCESS;

            foreach (long targetItemID in material_itemID_List)
            {
                if (!ItemManager.DeleteItem(ref TB, AID, targetItemID, itemList[targetItemID].itemid, ref retDeletedItem))
                    return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }

            string setQuery = string.Format("UPDATE {0} SET enchant_exp={1}, enchant_level={2} WHERE AID = {3} AND invenseq = {4}",
                                                InvenDBTableName, calcGetExp, baseLevel, AID, ItemID);
            if (!TB.ExcuteSqlCommand(InvenDBName, setQuery))
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            retUseResult = AccountManager.UseUserGold(ref TB, AID, calcEnchantPrice);
            if (retUseResult != Result_Define.eResult.SUCCESS)
                return retUseResult;

            FlushInvenList(ref TB, AID);

            return Result_Define.eResult.SUCCESS;
        }

        // Armor item Evolution (Grade Up)
        public static Result_Define.eResult EvolutionArmor(ref TxnBlock TB, long AID, long CID, long ItemID, ref List<Return_DisassableItems_List> retDeletedItem, string dbkey = InvenDBName)
        {
            Dictionary<long, User_Inven> itemList = GetInvenList(ref TB, AID, CID, dbkey);
            if (!itemList.ContainsKey(ItemID))
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_ITEM_EQUIP baseItemInfo = GetSystemInfo_EquipItem(ref TB, itemList[ItemID].itemid);

            Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];

            if (!Item_Define.ItemArmorTypeList.Contains(setItemType))
                return Result_Define.eResult.ITEM_EVOLUTION_TYPE_INVALIDE;

            short baseLevel = itemList[ItemID].enchant_level;
            short baseTier = baseItemInfo.Tier;
            short baseGrade = itemList[ItemID].enchant_grade;

            if (baseLevel < Item_Define.ItemMaxGradeLevel)
                return Result_Define.eResult.ITEM_EVOLUTION_LEVEL_NOT_ENOUGH;
            if (baseGrade >= Item_Define.ItemMaxEvolutionGrade)
                return Result_Define.eResult.ITEM_EVOLUTION_GRADE_MAX;

            System_ITEM_NEEDEXP baseNeedExpInfo = GetSystemInfo_NeedExp(ref TB, baseItemInfo.GrowthTableID2, baseGrade, baseLevel);
            if (baseNeedExpInfo.GradeUP_NeedItemID == 0)
                return Result_Define.eResult.ITEM_EVOLUTION_DB_NOT_FOUND;

            Result_Define.eResult retUseResult = ItemManager.UseItem(ref TB, AID, baseNeedExpInfo.GradeUP_NeedItemID, baseNeedExpInfo.GradeUP_NeedItemCount, ref retDeletedItem);
            if (retUseResult != Result_Define.eResult.SUCCESS)
                return retUseResult;
            int calcEnchantPrice = baseNeedExpInfo.GradeUPPrice;

            retUseResult = AccountManager.UseUserGold(ref TB, AID, calcEnchantPrice);
            if (retUseResult != Result_Define.eResult.SUCCESS)
                return retUseResult;

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
                        if (getOptionList.Count > currentOptionList.Count)
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

            string setQuery = string.Format(@"UPDATE {0} SET    enchant_exp= 0, enchant_level= 0, enchant_grade = {1},
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

            FlushInvenList(ref TB, AID);

            return Result_Define.eResult.SUCCESS;
        }

         */
    }
}
