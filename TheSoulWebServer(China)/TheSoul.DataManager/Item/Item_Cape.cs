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
        // make cape item from cape piece
        private static Result_Define.eResult UseCapePiece(ref TxnBlock TB, long AID, long CID, ref System_Item_Use UseInfo, ref List<Return_DisassableItems_List> retDeletedItem, ref List<User_Inven> MakeList)
        {
            int NeedItemCount = UseInfo.ConditionValue;

            Result_Define.eResult retUseResult = ItemManager.UseItem(ref TB, AID, UseInfo.Item_IndexID, NeedItemCount, ref retDeletedItem);
            if (retUseResult == Result_Define.eResult.SUCCESS)
            {
                Character CharInfo = CharacterManager.GetCharacter(ref TB, AID, CID);
                Character_Define.SystemClassType setClass = (Character_Define.SystemClassType)CharInfo.Class;

                System_Item_Use TierGroupInfo = GetSystem_Item_Use_TierGroup(ref TB, UseInfo.TierGroup, setClass);

                if (TierGroupInfo.Target_IndexID > 0)
                    retUseResult = ItemManager.MakeItem(ref TB, ref MakeList, AID, TierGroupInfo.Target_IndexID, 1, CID);
                else
                    retUseResult = Result_Define.eResult.CHARACTER_CLASS_INVALIDE;
            }

            return retUseResult;
        }

        public static Result_Define.eResult UseItem_InGame(ref TxnBlock TB, long AID, long CID, long InvenSeq, ref List<Return_DisassableItems_List> retDeletedItem, ref List<User_Inven> MakeList)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID);

            var findItem = itemList.Find(item => item.invenseq == InvenSeq);

            long baseItemID = findItem.itemid;

            Object SysItem = TheSoul.DataManager.ItemManager.GetSystemItemInfo(ref TB, baseItemID);

            TheSoul.DataManager.Item_Define.eSystemItemType checkType = (TheSoul.DataManager.Item_Define.eSystemItemType)((System_Item_Base)SysItem).ClassNo;

            Result_Define.eResult retResult = Result_Define.eResult.USE_ITEM_TYPE_INVALIDE;
            switch (checkType)
            {
                case TheSoul.DataManager.Item_Define.eSystemItemType.ItemClass_Use:
                    System_Item_Use SetItemInfo = (System_Item_Use)SysItem;
                    if (Item_Define.ItemType.ContainsKey(SetItemInfo.ItemType))
                    {
                        if (Item_Define.ItemType[SetItemInfo.ItemType] == Item_Define.eItemType.ItemType_MakeCape)
                            retResult = UseCapePiece(ref TB, AID, CID, ref SetItemInfo, ref retDeletedItem, ref MakeList);
                    }
                    else
                        retResult = Result_Define.eResult.USE_ITEM_TYPE_INVALIDE;
                    break;
                default:
                    break;
            }

            return retResult;
        }

        // Weapon item enchant up 
        public static Result_Define.eResult EnchantCape(ref TxnBlock TB, long AID, long CID, long InvenSeq, int useRuby, int useTalisman, ref bool bSuccess, ref List<Return_DisassableItems_List> retDeletedItem, string dbkey = Item_Define.Item_InvenDB)
        {
            List<User_Inven> itemList = GetInvenList(ref TB, AID, CID);

            var findItem = itemList.Find(item => item.invenseq == InvenSeq);
            if (findItem == null)
                return Result_Define.eResult.ITEM_ID_NOT_FOUND;

            System_Item_Equip baseItemInfo = GetSystem_Item_Equip(ref TB, findItem.itemid);

            Item_Define.eItemType setItemType = (Item_Define.eItemType)Item_Define.ItemType[baseItemInfo.ItemType];

            if (!Item_Define.ItemCapeTypeList.Contains(setItemType))
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
                    int RubytoMetal = useRuby > 0 ? (int)(SystemData.GetConstValue(ref TB, Item_Define.Item_Const_Def_Key_List[Item_Define.eItemConstDef.DEF_RUBYTRADE_THREAD]) * useRuby) : 0;

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

            if (bSuccess)
            {
                if (retError == Result_Define.eResult.SUCCESS && setEnchantInfo.EnchantUP_NeedItemCount1 > 0)
                    retError = MakeEnchantHistory(ref TB, findItem.invenseq, setEnchantInfo.EnchantUP_NeedItemID1, setEnchantInfo.EnchantUP_NeedItemCount1);
                if (retError == Result_Define.eResult.SUCCESS && setEnchantInfo.EnchantUP_NeedItemCount2 > 0)
                    retError = MakeEnchantHistory(ref TB, findItem.invenseq, setEnchantInfo.EnchantUP_NeedItemID2, setEnchantInfo.EnchantUP_NeedItemCount2);
                if (retError == Result_Define.eResult.SUCCESS && useTalisman > 0 && TalismanItemID > 0)
                    retError = MakeEnchantHistory(ref TB, findItem.invenseq, TalismanItemID, useTalisman);
            }

            if (retError == Result_Define.eResult.SUCCESS && bSuccess)
            {
                findItem.level++;
                retError = ItemManager.UpdateItemInfo(ref TB, findItem);
            }

            return retError;
        }
    }
}
