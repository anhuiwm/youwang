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
        // Make Enchant History to DB
        private static Result_Define.eResult MakeEnchantHistory(ref TxnBlock TB, long invenseq, long use_itemid, int use_count, string dbkey = Item_Define.Item_InvenDB)
        {
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandText = "User_Insert_User_Item_Enchant";
            Cmd.Parameters.Add("@invenseq", SqlDbType.BigInt).Value = invenseq;
            Cmd.Parameters.Add("@use_itemid", SqlDbType.BigInt).Value = use_itemid;
            Cmd.Parameters.Add("@use_count", SqlDbType.Int).Value = use_count;
            var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            Cmd.Parameters.Add(result);

            if (TB.ExcuteSqlStoredProcedure(dbkey, ref Cmd))
            {
                int checkValue = System.Convert.ToInt32(result.Value);
                Cmd.Dispose();
                if (checkValue < 0)
                    return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                else
                    return Result_Define.eResult.SUCCESS;
            }
            else
            {
                Cmd.Dispose();
                return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            }
        }

        // Weapon item enchant up 
        public static Result_Define.eResult EnchantWeapon(ref TxnBlock TB, long AID, long CID, long InvenSeq, int useRuby, int useTalisman, ref bool bSuccess, ref List<Return_DisassableItems_List> retDeletedItem, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID, CID);

            var findItem = itemList.Find(item => item.invenseq == InvenSeq);
            if (findItem == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_Item_Equip baseItemInfo = GetSystem_Item_Equip(ref TB, findItem.itemid);

            Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];

            if (!Item_Define.ItemWeaponTypeList.Contains(setItemType))
                return Result_Define.eResult.ITEM_ENCHANCE_TYPE_INVALIDE;

            int enchantMaxLevel = SystemData.GetConstValueInt(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_ITEM_MAX_ENCHANT_CNT]);
            if ((findItem.level + 1) > enchantMaxLevel)
                return Result_Define.eResult.ITEM_ENCHANCE_LEVEL_MAX;

            System_Item_Enchant setEnchantInfo = GetSystem_Item_Enchant(ref TB, baseItemInfo.GrowthTableID1);
            if (!(setEnchantInfo.Tier == baseItemInfo.Tier && setEnchantInfo.EnchantLevel == findItem.level))
                setEnchantInfo = GetSystem_Item_Enchant(ref TB, setEnchantInfo.EnchantGroup, baseItemInfo.Tier, findItem.level);

            int HaveItemCount = itemList.FindAll(item => item.itemid == setEnchantInfo.EnchantUP_NeedItemID1).Sum(item => item.itemea);

            Result_Define.eResult retError = setEnchantInfo.Enchant_IndexID > 0 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.ITEM_ENCHANCE_DB_NOT_FOUND;

            long TalismanItemID = 0;
            // china version always Success. don't use enchant rate yet.
            if (SystemData.GetServiceArea(ref TB) != DataManager_Define.eCountryCode.China)
            {
                int setRate = setEnchantInfo.EnchantUP_SuccessRate;
                int curRate = TheSoul.DataManager.Math.GetRandomInt(0, Item_Define.Item_MaxRate);
                if (useTalisman > 0 && retError == Result_Define.eResult.SUCCESS)
                {
                    useTalisman = SystemData.GetServiceArea(ref TB) != DataManager_Define.eCountryCode.China ? useTalisman : 1;    // korean version allow only one.
                    int addRate = useTalisman * SystemData.GetConstValueInt(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_WPNINCHANT_PROBUP_VALUE]);
                    setRate += addRate;
                    TalismanItemID = SystemData.GetConstValueLong(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_WPNINCHANT_PROBUP_ID]);
                    retError = ItemManager.UseItem(ref TB, AID, TalismanItemID, useTalisman, ref retDeletedItem);
                }
                bSuccess = (setRate >= curRate);
            }
            else
                bSuccess = true;


            if (setEnchantInfo.EnchantUP_NeedItemID1 > 0 && setEnchantInfo.EnchantUP_NeedItemCount1 > 0 && retError == Result_Define.eResult.SUCCESS)
            {
                int UseCount = HaveItemCount >= setEnchantInfo.EnchantUP_NeedItemCount1 ? setEnchantInfo.EnchantUP_NeedItemCount1 : HaveItemCount;

                if (UseCount < setEnchantInfo.EnchantUP_NeedItemCount1)
                {
                    int RubytoMetal = useRuby > 0 ? (int)(SystemData.GetConstValue(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_RUBYTRADE_METAL]) * useRuby) : 0;

                    if (UseCount + RubytoMetal >= setEnchantInfo.EnchantUP_NeedItemCount1 && useRuby > 0)
                        retError = AccountManager.UseUserCash(ref TB, AID, useRuby);
                    else
                        retError = Result_Define.eResult.NOT_ENOUGH_USE_ITEM;
                }

                if (retError == Result_Define.eResult.SUCCESS)
                    retError = ItemManager.UseItem(ref TB, AID, setEnchantInfo.EnchantUP_NeedItemID1, UseCount, ref retDeletedItem);
            }

            if (setEnchantInfo.EnchantUP_NeedItemID2 > 0 && setEnchantInfo.EnchantUP_NeedItemCount2 > 0 && retError == Result_Define.eResult.SUCCESS)
                retError = ItemManager.UseItem(ref TB, AID, setEnchantInfo.EnchantUP_NeedItemID2, setEnchantInfo.EnchantUP_NeedItemCount2, ref retDeletedItem);

            if (setEnchantInfo.GradeUP_NeedGold > 0 && retError == Result_Define.eResult.SUCCESS)
                retError = AccountManager.UseUserGold(ref TB, AID, setEnchantInfo.GradeUP_NeedGold);

            if (retError == Result_Define.eResult.SUCCESS && bSuccess)
            {
                findItem.level++;
                retError = ItemManager.UpdateItemInfo(ref TB, findItem);
            }

            if (bSuccess)
            {
                if (retError == Result_Define.eResult.SUCCESS && setEnchantInfo.EnchantUP_NeedItemCount1 > 0)
                    retError = MakeEnchantHistory(ref TB, findItem.invenseq, setEnchantInfo.EnchantUP_NeedItemID1, setEnchantInfo.EnchantUP_NeedItemCount1);
                if (retError == Result_Define.eResult.SUCCESS && setEnchantInfo.EnchantUP_NeedItemCount2 > 0)
                    retError = MakeEnchantHistory(ref TB, findItem.invenseq, setEnchantInfo.EnchantUP_NeedItemID2, setEnchantInfo.EnchantUP_NeedItemCount2);
                if (retError == Result_Define.eResult.SUCCESS && useTalisman > 0 && TalismanItemID > 0)
                    retError = MakeEnchantHistory(ref TB, findItem.invenseq, TalismanItemID, useTalisman);
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Weapon_LvUp, (int)(Trigger_Define.eCheckSuccess.Try)));
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Weapon_LvUp, (int)(bSuccess ? Trigger_Define.eCheckSuccess.Success : Trigger_Define.eCheckSuccess.Fail)));
                
                /// 当前武器的等级是否超过了活动规定的等级
                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Weapon_Lv, 0, 0, findItem.level));

                retError = TriggerManager.ProgressTrigger(ref TB, AID, setDataList);
            }

            return retError;
        }

        // Weapon Item Option Change
        public static Result_Define.eResult WeaponOptionChangeAll(ref TxnBlock TB, long AID, long CID, long InvenSeq, long materialSeq, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID, CID);
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            var findItem = itemList.Find(item => item.invenseq == InvenSeq);
            var materialItem = itemList.Find(item => item.invenseq == materialSeq);

            if (findItem == null || materialItem == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            if (findItem.grade != materialItem.grade)
                return Result_Define.eResult.ITEM_RIFINING_NEED_SAME_GRADE_ITEM;

            return retError;
        }

        // Weapon Item Refining Option
        public static Result_Define.eResult RefiningWeaponOption(ref TxnBlock TB, long AID, long CID, long InvenSeq, long OptionSeq, long materialSeq, int useRuby, ref List<Return_DisassableItems_List> retDeletedItem, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID, CID, true, true);

            var findItem = itemList.Find(item => item.invenseq == InvenSeq);
            var materialItem = itemList.Find(item => item.invenseq == materialSeq);

            if (findItem == null || materialItem == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            if (findItem.grade != materialItem.grade)
                return Result_Define.eResult.ITEM_RIFINING_NEED_SAME_GRADE_ITEM;

            System_Item_Equip baseItemInfo = GetSystem_Item_Equip(ref TB, findItem.itemid);

            Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];

            if (!Item_Define.ItemWeaponTypeList.Contains(setItemType))
                return Result_Define.eResult.ITEM_ENCHANCE_TYPE_INVALIDE;

            short limitLevel = SystemData.GetConstValueShort(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_REFINING_START_ITEMLEVEL]);

            Result_Define.eResult retError = (findItem.level >= limitLevel && findItem.grade >= Item_Define.ItemMax_Weapon_Grade) ? Result_Define.eResult.SUCCESS
                : (findItem.grade < Item_Define.ItemMax_Weapon_Grade ? Result_Define.eResult.ITEM_RIFINING_GRADE_NOT_ENOUGH : Result_Define.eResult.ITEM_RIFINING_LEVEL_NOT_ENOUGH);

            if (retError != Result_Define.eResult.SUCCESS)
                return retError;

            var findOption = findItem.random_option.Find(option => option.optionseq == OptionSeq);
            if (findOption == null)
                return Result_Define.eResult.ITEM_OPTION_ID_NOT_FOUND;

            if (findOption.option_level >= Item_Define.ItemMax_Weapon_Option_Level)
                return Result_Define.eResult.ITEM_RIFINING_LEVEL_MAX;

            Item_Define.eItemParamType setParamType;
            if (!Item_Define.Item_Param_Enum_List.TryGetValue(findOption.optiontype, out setParamType))
                return Result_Define.eResult.ITEM_OPTION_TYPE_INVALIDE;

            Item_Define.eItemRefiningType setRifinigType;
            if (!Item_Define.Item_Rifining_Check_List.TryGetValue(setParamType, out setRifinigType))
                return Result_Define.eResult.ITEM_RIFINING_TYPE_INVALIDE;

            System_Item_Refining_Weapon setRifiningInfo = GetSystem_Item_Refining_Weapon(ref TB, Item_Define.Item_Rifining_Key_List[setRifinigType], baseItemInfo.Tier, findOption.option_level);

            if(setRifiningInfo.Refining_IndexID == 0)
                return Result_Define.eResult.ITEM_ENCHANCE_DB_NOT_FOUND;

            int getExp = TheSoul.DataManager.Math.GetRandomInt(setRifiningInfo.GetEXP_min, setRifiningInfo.GetEXP_max);

            int add_rate_count = materialItem.random_option.Count(item => item.optiontype == Item_Define.Item_Rifining_ParamKey_List[setRifinigType]);
            double expAddRate = add_rate_count == 0 ? 0 : ((SystemData.GetConstValue(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_REFINING_EXPBONUS]) * add_rate_count) / 100.0f);

            int setExp = findOption.option_exp + getExp + System.Convert.ToInt32(getExp * expAddRate);
            byte setLevel = findOption.option_level;
            int setAddValue = findOption.option_add_value;

            int HaveItemCount = itemList.FindAll(item => item.itemid == setRifiningInfo.NeedItem1).Sum(item => item.itemea);

            if (setRifiningInfo.NeedItem1 > 0 && setRifiningInfo.NeedItem1_Count > 0 && retError == Result_Define.eResult.SUCCESS)
            {
                int UseCount = HaveItemCount >= setRifiningInfo.NeedItem1_Count ? setRifiningInfo.NeedItem1_Count : HaveItemCount;

                if (UseCount < setRifiningInfo.NeedItem1_Count)
                {
                    int RubytoMetal = (int)(SystemData.GetConstValue(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_RUBYTRADE_METAL]) * useRuby);
                    //int RubytoMetal = System.Convert.ToInt32(SystemData.GetConstValue(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_RUBYTRADE_METAL])) * useRuby;

                    if (UseCount + RubytoMetal >= setRifiningInfo.NeedItem1_Count)
                        retError = AccountManager.UseUserCash(ref TB, AID, useRuby);
                    else
                        retError = Result_Define.eResult.NOT_ENOUGH_USE_ITEM;
                }

                if (retError == Result_Define.eResult.SUCCESS)
                    retError = ItemManager.UseItem(ref TB, AID, setRifiningInfo.NeedItem1, UseCount, ref retDeletedItem);
            }

            //if (setRifiningInfo.NeedItem1 > 0 && setRifiningInfo.NeedItem1_Count > 0 && retError == Result_Define.eResult.SUCCESS)
            //    retError = ItemManager.UseItem(ref TB, AID, setRifiningInfo.NeedItem1, setRifiningInfo.NeedItem2_Count, ref retDeletedItem);
            if (setRifiningInfo.NeedItem2 > 0 && setRifiningInfo.NeedItem2_Count > 0 && retError == Result_Define.eResult.SUCCESS)
                retError = ItemManager.UseItem(ref TB, AID, setRifiningInfo.NeedItem2, setRifiningInfo.NeedItem2_Count, ref retDeletedItem);

            if (retError == Result_Define.eResult.SUCCESS)
            {
                while (setExp >= setRifiningInfo.NeedEXP && setLevel < Item_Define.ItemMax_Weapon_Option_Level)
                {
                    setLevel++;
                    setExp -= setRifiningInfo.NeedEXP;
                    setRifiningInfo = GetSystem_Item_Refining_Weapon(ref TB, Item_Define.Item_Rifining_Key_List[setRifinigType], baseItemInfo.Tier, setLevel);
                    if (setRifiningInfo.Refining_IndexID < 1)
                        break;
                }

                if (setLevel != findOption.option_level)
                {
                    //setAddValue = GetSystem_Item_Refining_Weapon_TierAll(ref TB, Item_Define.Item_Rifining_Key_List[setRifinigType], baseItemInfo.Tier).
                    //                        Where(item => item.RefiningLevel < setLevel).
                    //                            Sum(info => info.RefiningUP_Value);
                    setAddValue = GetSystem_Item_Refining_Weapon(ref TB, Item_Define.Item_Rifining_Key_List[setRifinigType], baseItemInfo.Tier, setLevel).RefiningUP_Value;
                }

                findOption.option_level = setLevel;
                findOption.option_exp = setExp;
                findOption.option_add_value = setAddValue;

                retError = UpdateOptionInfo(ref TB, findOption);
            }

            if (retError == Result_Define.eResult.SUCCESS)
                retError = ItemManager.DeleteItem(ref TB, AID, materialItem.invenseq, ref retDeletedItem);

            if (retError == Result_Define.eResult.SUCCESS && setRifiningInfo.NeedItem1 > 0)
                retError = MakeEnchantHistory(ref TB, findItem.invenseq, setRifiningInfo.NeedItem1, setRifiningInfo.NeedItem1_Count);
            if (retError == Result_Define.eResult.SUCCESS && setRifiningInfo.NeedItem2 > 0)
                retError = MakeEnchantHistory(ref TB, findItem.invenseq, setRifiningInfo.NeedItem2, setRifiningInfo.NeedItem2_Count);
            if (retError == Result_Define.eResult.SUCCESS)
                retError = TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.WEAPON_REFINING);

            return retError;
        }

        // Weapon Item disassemble
        public static Result_Define.eResult DisassembleEquip(ref TxnBlock TB, long AID, long CID, long InvenSeq, bool isReqInfo, ref int UseRuby, ref List<User_Inven> retResultItem, ref List<Return_DisassableItems_List> retDeletedItem, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID, CID);

            var findItem = itemList.Find(item => item.invenseq == InvenSeq);

            if (findItem == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_Item_Equip baseItemInfo = GetSystem_Item_Equip(ref TB, findItem.itemid);
            Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];

            bool bIsWeapon = Item_Define.ItemWeaponTypeList.Contains(setItemType);
            bool bIsCape = Item_Define.ItemCapeTypeList.Contains(setItemType);
            if (!(bIsWeapon || bIsCape))
                return Result_Define.eResult.ITEM_DISASSAEMBLE_TYPE_INVALIDE;

            short Item_Weapon_Disassemble_Level = SystemData.GetConstValueShort(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_DISASSEMBLE_LEVEL]);
            bool isAdvDissemble = bIsWeapon ? (findItem.grade >= Item_Define.Item_Weapon_Adv_Disassemble_Grade && findItem.level > Item_Weapon_Disassemble_Level) : false;
            double retRate = 0;
            List<long> retTargetItems = new List<long>();

            if (isAdvDissemble)
            {
                retTargetItems.Add(SystemData.GetConstValueLong(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_RECOVERY_ITEMID_METAL]));
                retTargetItems.Add(SystemData.GetConstValueLong(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_RECOVERY_ITEMID_LVUP_STONE]));
                retTargetItems.Add(SystemData.GetConstValueLong(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_RECOVERY_ITEMID_TALISMAN]));
                retTargetItems.Add(SystemData.GetConstValueLong(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_RECOVERY_ITEMID_HP_STONE]));
                retTargetItems.Add(SystemData.GetConstValueLong(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_RECOVERY_ITEMID_AP_STONE]));
                retTargetItems.Add(SystemData.GetConstValueLong(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_RECOVERY_ITEMID_DFP_STONE]));
            }
            else
            {
                retTargetItems.Add(SystemData.GetConstValueLong(ref TB, bIsWeapon ?
                    Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_RECOVERY_ITEMID_METAL]
                    : Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_RECOVERY_ITEMID_THREAD]));
            }

            if (bIsWeapon)
            {
                short Item_Weapon_Adv_Disassemble_Level = SystemData.GetConstValueShort(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_SMELTING_LEVEL1]);
                short Item_Weapon_Adv_Disassemble_High_Rate_Level = SystemData.GetConstValueShort(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_SMELTING_LEVEL2]);

                if (!isAdvDissemble && findItem.grade < Item_Define.Item_Weapon_Adv_Disassemble_Grade)
                    retRate = SystemData.GetConstValue(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_DISASSEMBLE_RATE_1]);
                else if (!isAdvDissemble && findItem.level < Item_Weapon_Adv_Disassemble_Level)
                    retRate = SystemData.GetConstValue(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_DISASSEMBLE_RATE_2]);
                else if (isAdvDissemble && findItem.level < Item_Weapon_Adv_Disassemble_High_Rate_Level)
                    retRate = SystemData.GetConstValue(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_SMELTING_RATE_1]);
                else if (isAdvDissemble && findItem.level >= Item_Weapon_Adv_Disassemble_High_Rate_Level)
                    retRate = SystemData.GetConstValue(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_SMELTING_RATE_2]);
            }
            else
            {
                retRate = SystemData.GetConstValue(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_DISASSEMBLE_RATE_1]);
            }
            retRate = retRate / 100;

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            if (bIsWeapon)
            {
                System_Item_Grade_Weapon setEnchantInfo = GetSystem_Item_Grade_Weapon(ref TB, baseItemInfo.GrowthTableID2);
                if (!(setEnchantInfo.Tier == baseItemInfo.Tier && setEnchantInfo.Grade == findItem.grade))
                    setEnchantInfo = GetSystem_Item_Grade_Weapon(ref TB, setEnchantInfo.WeaponGroup, baseItemInfo.Tier, findItem.grade);

                retError = CalcDisassembleReturn(ref TB, findItem.invenseq, retRate, ref retTargetItems, ref retResultItem);
                int makecount = System.Convert.ToInt32(System.Math.Floor(setEnchantInfo.Disassemble_MakeItemCount * retRate));
                var finditem = retResultItem.Find(item => item.itemid == setEnchantInfo.Disassemble_MakeItemID);
                if (finditem != null)
                    finditem.itemea = finditem.itemea + makecount;
                else if (setEnchantInfo.Disassemble_MakeItemID > 0)
                    retResultItem.Add(new User_Inven(setEnchantInfo.Disassemble_MakeItemID, makecount));
            }
            else
            {
                System_Item_Cape setEnchantInfo = GetSystem_Item_Cape(ref TB, baseItemInfo.GrowthTableID2);
                retError = CalcDisassembleReturn(ref TB, findItem.invenseq, retRate, ref retTargetItems, ref retResultItem);
                int makecount = System.Convert.ToInt32(System.Math.Floor(setEnchantInfo.Disassemble_MakeItemCount * retRate));
                //makecount = makecount < 1 ? 1 : makecount;
                //setEnchantInfo.Disassemble_MakeItemID = setEnchantInfo.Disassemble_MakeItemID < 1 ? 303040012 : setEnchantInfo.Disassemble_MakeItemID;
                var finditem = retResultItem.Find(item => item.itemid == setEnchantInfo.Disassemble_MakeItemID);
                if (finditem != null)
                    finditem.itemea = finditem.itemea + makecount;
                else if (setEnchantInfo.Disassemble_MakeItemID > 0)
                    retResultItem.Add(new User_Inven(setEnchantInfo.Disassemble_MakeItemID, makecount));
            }

            if (isAdvDissemble)
                UseRuby = SystemData.GetConstValueInt(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_SMELTING_PRICE]);

            if (isReqInfo)
            {
                Return_DisassableItems_List retItem = new Return_DisassableItems_List();
                retItem.itemseq = findItem.invenseq;
                retItem.itemid = findItem.itemid;
                retItem.itemea = findItem.itemea;
                retDeletedItem.Add(retItem);
                return retError;
            }

            if (retError == Result_Define.eResult.SUCCESS)
                retError = ItemManager.DeleteItem(ref TB, AID, findItem.invenseq, ref retDeletedItem);

            if (retError == Result_Define.eResult.SUCCESS && UseRuby > 0)
                retError = AccountManager.UseUserCash(ref TB, AID, UseRuby);

            return retError;
        }

        private static Result_Define.eResult CalcDisassembleReturn(ref TxnBlock TB, long invenseq, double retRate, ref List<long> targetItemIDs, ref List<User_Inven> retResultItem, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Item_Enchant> usedItemList = GetUserEnhantInfo(ref TB, invenseq);
            foreach (User_Item_Enchant useitem in usedItemList)
            {
                if (targetItemIDs.Contains(useitem.use_itemid))
                {
                    User_Inven setItem = new User_Inven();
                    setItem.itemid = useitem.use_itemid;
                    setItem.itemea = System.Convert.ToInt32(useitem.use_count * retRate);
                    retResultItem.Add(setItem);
                }
            }            
            return Result_Define.eResult.SUCCESS;
        }

        private static List<User_Item_Enchant> GetUserEnhantInfo(ref TxnBlock TB, long invenseq, string dbkey = Item_Define.Item_InvenDB)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK)  WHERE invenseq = {1}", Item_Define.Item_User_Item_Enchant_Table, invenseq);
            return TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_Item_Enchant>(ref TB, setQuery, dbkey);            
        }

        /*
        // get system item enchant weapon table use by redis hash
        const string SystemItem_Enchant_WPN_TableName = "System_ITEM_ENCHANT_WPN";
        const string SystemItem_Enchant_WPN_Surfix_Class = "Enchant_WPN";

        private static System_ITEM_ENCHANT_WPN GetSystemInfo_Enchant_WPN(ref TxnBlock TB, int EnchantGroup, short Tier, short EnchantLevel, bool Flush = false, string dbkey = InvenDBName)
        {
            string setKey = string.Format("{0}_{1}_{2}", SystemItemPrefix, SystemItem_Enchant_WPN_TableName, SystemItem_Enchant_WPN_Surfix_Class);
            string setQuery = string.Format("SELECT * FROM {0} WHERE EnchantGroup = {1} AND Tier = {2} AND EnchantLevel = {3}", SystemItem_Enchant_WPN_TableName, EnchantGroup, Tier, EnchantLevel);
            string memebrKey = string.Format("{0}_{1}_{2}", EnchantGroup, Tier, EnchantLevel);
            System_ITEM_ENCHANT_WPN retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_ITEM_ENCHANT_WPN>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, memebrKey, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_ITEM_ENCHANT_WPN();
            return retObj;
        }

        // get system item evolution weapon table use by redis hash
        const string SystemItem_Evolution_WPN_TableName = "System_ITEM_EVOL_WEAPON";
        const string SystemItem_Evolution_WPN_Surfix_Class = "Evolution_WPN";

        private static System_ITEM_EVOL_WEAPON GetSystemInfo_Evolution_WPN(ref TxnBlock TB, int EnchantGroup, short Tier, short EvolGrade, bool Flush = false, string dbkey = InvenDBName)
        {
            string setKey = string.Format("{0}_{1}_{2}", SystemItemPrefix, SystemItem_Evolution_WPN_TableName, SystemItem_Evolution_WPN_Surfix_Class);
            string setQuery = string.Format("SELECT * FROM {0} WHERE EvolGroup = {1} AND Tier = {2} AND Grade = {3}", SystemItem_Evolution_WPN_TableName, EnchantGroup, Tier, EvolGrade);
            string memebrKey = string.Format("{0}_{1}_{2}", EnchantGroup, Tier, EvolGrade);
            System_ITEM_EVOL_WEAPON retObj = TheSoul.DataManager.GenericFetch.FetchFromRedis_Hash<System_ITEM_EVOL_WEAPON>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, memebrKey, setQuery, dbkey, Flush);
            if (retObj == null)
                retObj = new System_ITEM_EVOL_WEAPON();
            return retObj;
        }


        // Weapon item enchant up 
        public static Result_Define.eResult EnchantWeapon(ref TxnBlock TB, long AID, long CID, long ItemID, int useRuby, ref short baseLevel, ref bool bSuccess, ref List<Return_DisassableItems_List> retDeletedItem, string dbkey = InvenDBName)
        {
            Dictionary<long, User_Inven> itemList = GetInvenList(ref TB, AID, CID, dbkey);
            if (!itemList.ContainsKey(ItemID))
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_ITEM_EQUIP baseItemInfo = GetSystemInfo_EquipItem(ref TB, itemList[ItemID].itemid);

            Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];

            if (!Item_Define.ItemWeaponTypeList.Contains(setItemType))
                return Result_Define.eResult.ITEM_ENCHANCE_TYPE_INVALIDE;

            short baseTier = baseItemInfo.Tier;
            short baseGrade = itemList[ItemID].enchant_grade;
            baseLevel = itemList[ItemID].enchant_level;

            System_ITEM_ENCHANT_WPN baseEnchantInfo = GetSystemInfo_Enchant_WPN(ref TB, baseItemInfo.GrowthTableID2, baseTier, baseLevel);

            Dictionary<long, int> setNeedItem = new Dictionary<long, int>();

            if (baseEnchantInfo.EnchantUP_NeedItemID1 > 0 && baseEnchantInfo.EnchantUP_NeedItemCount1 > 0)
                setNeedItem.Add(baseEnchantInfo.EnchantUP_NeedItemID1, baseEnchantInfo.EnchantUP_NeedItemCount1);
            if (baseEnchantInfo.EnchantUP_NeedItemID2 > 0 && baseEnchantInfo.EnchantUP_NeedItemCount2 > 0)
                setNeedItem.Add(baseEnchantInfo.EnchantUP_NeedItemID2, baseEnchantInfo.EnchantUP_NeedItemCount2);

            if (setNeedItem.Count < 1 || baseEnchantInfo.Enchant_IndexID < 1)
                return Result_Define.eResult.ITEM_ENCHANCE_DB_NOT_FOUND;

            if ((baseLevel + 1) > Item_Define.ItemMax_Weapon_Level)
                return Result_Define.eResult.ITEM_ENCHANCE_LEVEL_MAX;

            int calcEnchantPrice = baseEnchantInfo.GradeUP_NeedGold;

            Result_Define.eResult retUseResult = AccountManager.UseUserGold_And_Ruby(ref TB, AID, calcEnchantPrice, useRuby);
            if (retUseResult != Result_Define.eResult.SUCCESS)
                return retUseResult;

            foreach (KeyValuePair<long, int> material in setNeedItem)
            {
                retUseResult = ItemManager.UseItem(ref TB, AID, material.Key, material.Value, ref retDeletedItem);
                if (retUseResult != Result_Define.eResult.SUCCESS)
                    return retUseResult;
            }

            double curRate = TheSoul.DataManager.Math.GetRandomDouble(0, Item_Define.Item_MaxRate);
            double checkrate = baseEnchantInfo.EnchantUP_SuccessRate + (useRuby * (System.Convert.ToDouble(baseEnchantInfo.ProbabilityUpCash) / 10));

            if (checkrate >= curRate)
            {
                baseLevel++;
                string setQuery = string.Format("UPDATE {0} SET enchant_level={1} WHERE AID = {2} AND invenseq = {3}",
                                                    InvenDBTableName, baseLevel, AID, ItemID);
                if (!TB.ExcuteSqlCommand(InvenDBName, setQuery))
                    return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                bSuccess = true;
                FlushInvenList(ref TB, AID);
            }

            return Result_Define.eResult.SUCCESS;
        }

        // Weapon item Evolution (Grade Up)
        public static Result_Define.eResult EvolutionWeapon(ref TxnBlock TB, long AID, long CID, long ItemID, ref List<Return_DisassableItems_List> retDeletedItem, string dbkey = InvenDBName)
        {
            Dictionary<long, User_Inven> itemList = GetInvenList(ref TB, AID, CID, dbkey);
            if (!itemList.ContainsKey(ItemID))
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_ITEM_EQUIP baseItemInfo = GetSystemInfo_EquipItem(ref TB, itemList[ItemID].itemid);

            Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];

            if (!Item_Define.ItemWeaponTypeList.Contains(setItemType))
                return Result_Define.eResult.ITEM_EVOLUTION_TYPE_INVALIDE;

            short baseTier = baseItemInfo.Tier;
            short baseGrade = itemList[ItemID].enchant_grade;
            short baseLevel = itemList[ItemID].enchant_level;

            if (baseGrade >= Item_Define.ItemMaxEvolutionGrade)
                return Result_Define.eResult.ITEM_EVOLUTION_GRADE_MAX;

            System_ITEM_EVOL_WEAPON baseEvolutionInfo = GetSystemInfo_Evolution_WPN(ref TB, baseItemInfo.GrowthTableID1, baseTier, baseGrade);

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

            FlushInvenList(ref TB, AID);

            return Result_Define.eResult.SUCCESS;
        }

        // Weapon Item Disassemble 
        public static Result_Define.eResult DisassembleWeapon(ref TxnBlock TB, long AID, long CID, ref List<Return_DisassableItems> resultSet, List<long> material_itemID_List, ref List<Return_DisassableItems_List> retDeletedItem, string dbkey = InvenDBName)
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

                if (!Item_Define.ItemWeaponTypeList.Contains(setItemType))
                    return Result_Define.eResult.ITEM_DISASSAEMBLE_TYPE_INVALIDE;

                short baseTier = baseItemInfo.Tier;
                short baseGrade = itemList[targetItemID].enchant_grade;
                short baseLevel = itemList[targetItemID].enchant_level;

                for (short checkGrade = 1; checkGrade <= baseGrade; checkGrade++)
                {
                    System_ITEM_EVOL_WEAPON baseEvolutionInfo = GetSystemInfo_Evolution_WPN(ref TB, baseItemInfo.GrowthTableID1, baseTier, checkGrade);

                    RetGetItems[baseEvolutionInfo.Disassemble_MakeItemID] = RetGetItems.ContainsKey(baseEvolutionInfo.Disassemble_MakeItemID) ?
                        RetGetItems[baseEvolutionInfo.Disassemble_MakeItemID] + baseEvolutionInfo.Disassemble_MakeItemCount :
                            baseEvolutionInfo.Disassemble_MakeItemCount;

                    if (checkGrade >= Item_Define.ItemMaxGradeLevel)
                    {
                        Dictionary<long, long> GetGradeItem = new Dictionary<long, long>();

                        for (short checkLevel = 1; checkLevel < baseLevel; checkLevel++)
                        {
                            System_ITEM_ENCHANT_WPN baseEnchantInfo = GetSystemInfo_Enchant_WPN(ref TB, baseItemInfo.GrowthTableID2, baseTier, checkLevel);

                            if (baseEnchantInfo.EnchantUP_NeedItemID1 > 0)
                            {
                                GetGradeItem[baseEnchantInfo.EnchantUP_NeedItemID1] = GetGradeItem.ContainsKey(baseEnchantInfo.EnchantUP_NeedItemID1) ?
                                    GetGradeItem[baseEnchantInfo.EnchantUP_NeedItemID1] + baseEnchantInfo.EnchantUP_NeedItemCount1 :
                                        baseEnchantInfo.EnchantUP_NeedItemCount1;
                            }

                            if (baseEnchantInfo.EnchantUP_NeedItemID2 > 0)
                            {
                                GetGradeItem[baseEnchantInfo.EnchantUP_NeedItemID2] = GetGradeItem.ContainsKey(baseEnchantInfo.EnchantUP_NeedItemID2) ?
                                    GetGradeItem[baseEnchantInfo.EnchantUP_NeedItemID2] + baseEnchantInfo.EnchantUP_NeedItemCount2 :
                                        baseEnchantInfo.EnchantUP_NeedItemCount2;
                            }
                        }

                        foreach (KeyValuePair<long, long> setItem in GetGradeItem)
                        {
                            long makeCount = System.Convert.ToInt64(System.Math.Floor(setItem.Value * Item_Define.Item_Disassemble_Level_Rate[baseLevel]));
                            RetGetItems[setItem.Key] = RetGetItems.ContainsKey(setItem.Key) ?
                                RetGetItems[setItem.Key] + makeCount :
                                    makeCount;
                        }
                    }
                }
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
                    return Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
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
         */
    }
}
