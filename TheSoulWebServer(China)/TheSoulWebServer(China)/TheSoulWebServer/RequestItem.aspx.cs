using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using mSeed.mDBTxnBlock;
using mSeed.RedisManager;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoulWebServer.Tools;
using ServiceStack.Text;

namespace TheSoulWebServer
{
    public partial class RequestItem : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                // inven op
                "getinven",
                "equipitem",
                "unequipitem",
                "sellitem",

                // armor
                "enchantarmor",
                "evolutionarmor",
                "metalworkarmor",

                // weapon
                "enchantweapon",
                "refiningoption",
                "weapon_option_change",

                "getoptiondetail",
                "disassemble",
                "disassemble_info",

                // accessory
                "optionadd",
                "optionadd_reroll",
                "option_allchange",

                // cape
                "useitem",
                "enchantcape",

                // ultimate
                "orb_inven_list",
                "ultimate_unlock",
                "ultimate_equip",
                "ultimate_unequip",
                "ultimate_enchant",
                "orb_equip",
                "orb_unequip",
                "orb_mix",

                // for test method only work in debug mode
                "makeitem",
                "flushitem",
                "deleteitem",
                "useitem_debug",
            };

            WebQueryParam queryFetcher = new WebQueryParam();
            string retJson = "";

            TxnBlock tb = new TxnBlock();
            {
                long AID = 0;
                try
                {
                    queryFetcher.TxnBlockInit(ref tb, ref AID);

                    string requestOp = queryFetcher.QueryParam_Fetch("op");
                    JsonObject json = new JsonObject();

                    if (queryFetcher.ReRequestFlag)
                    {
                        retJson = queryFetcher.ReRequestRender();
                    }
                    else if (Array.IndexOf(ops, requestOp) >= 0)
                    {
                        Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                        queryFetcher.operation = requestOp;

                        long CID = queryFetcher.QueryParam_FetchLong("cid");
                        long ItemSeq = queryFetcher.QueryParam_FetchLong("itemseq");
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.op], requestOp);
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], AID);

                        if (requestOp.Equals("getinven"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            List<User_Inven> CharItem = ItemManager.GetInvenList(ref tb, AID, CID);
                            List<User_Inven> AccountInven = CharItem.Where(item => item.cid == 0).ToList();
                            List<User_Inven> CharInven = CharItem.Where(item => item.cid > 0).ToList();

                            json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.AccountInven], Ret_Inven_Item.makeInvenListJson(ref AccountInven));
                            json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.CharacterInven], Ret_Inven_Item.makeInvenListJson(ref CharInven));
                        }
                        else if (requestOp.Equals("equipitem") || requestOp.Equals("unequipitem"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            List<User_Inven> EquipList = new List<User_Inven>();
                            retError = requestOp.Equals("equipitem") ? ItemManager.EquipItemToCharacter(ref tb, AID, ItemSeq, CID, ref EquipList) :
                                (requestOp.Equals("unequipitem") ? ItemManager.UnEquip(ref tb, AID, ItemSeq, CID, ref EquipList) : Result_Define.eResult.System_Unknown_Operation);
                        }
                        //else if (requestOp.Equals("unequipitem"))
                        //{
                        //    List<User_Inven> EquipList = new List<User_Inven>();
                        //    retError = ItemManager.UnEquip(ref tb, AID, ItemSeq, CID, ref EquipList);
                        //}
                        else if (requestOp.Equals("getoptiondetail"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            List<User_Inven> CharItem = ItemManager.GetInvenList(ref tb, AID, CID);
                            var findItem = CharItem.Find(item => item.invenseq == ItemSeq);
                            Ret_Inven_Item detailItem = new Ret_Inven_Item(findItem, true);
                            json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.ItemOption], detailItem.ToJson(true));
                        }
                        else if (requestOp.Equals("enchantarmor") || requestOp.Equals("evolutionarmor") || requestOp.Equals("metalworkarmor")
                                || requestOp.Equals("enchantweapon") || requestOp.Equals("refiningoption")
                                || requestOp.Equals("optionadd") || requestOp.Equals("optionadd_reroll") || requestOp.Equals("option_allchange")
                                || requestOp.Equals("enchantcape")
                            )
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            List<Return_DisassableItems_List> retDeletedItem = new List<Return_DisassableItems_List>();

                            bool bSuccess = true;

                            long OptionSeq = queryFetcher.QueryParam_FetchLong("optionseq");
                            long MaterialSeq = queryFetcher.QueryParam_FetchLong("materialseq");
                            if (requestOp.Equals("enchantarmor"))
                            {
                                byte try_count = (byte)queryFetcher.QueryParam_FetchInt("try_count", 1);
                                retError = ItemManager.EnchantArmor(ref tb, AID, CID, ItemSeq, ref retDeletedItem, try_count);
                            }
                            else if (requestOp.Equals("evolutionarmor"))
                                retError = ItemManager.EvolutionArmor(ref tb, AID, CID, ItemSeq, ref retDeletedItem);
                            else if (requestOp.Equals("metalworkarmor"))
                                retError = ItemManager.MetalWorkArmor(ref tb, AID, CID, ItemSeq, ref bSuccess, ref retDeletedItem);
                            if (requestOp.Equals("enchantweapon") || requestOp.Equals("enchantcape"))
                            {
                                //int useRuby = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("useruby", "0"));
                                int useRuby = 0;
                                int useTalisman = queryFetcher.QueryParam_FetchInt("usetalisman");

                                if (requestOp.Equals("enchantcape"))
                                    retError = ItemManager.EnchantCape(ref tb, AID, CID, ItemSeq, useRuby, useTalisman, ref bSuccess, ref retDeletedItem);
                                else if (requestOp.Equals("enchantweapon"))
                                    retError = ItemManager.EnchantWeapon(ref tb, AID, CID, ItemSeq, useRuby, useTalisman, ref bSuccess, ref retDeletedItem);
                            }
                            else if (requestOp.Equals("refiningoption"))
                            {
                                int useRuby = queryFetcher.QueryParam_FetchInt("useruby");
                                retError = ItemManager.RefiningWeaponOption(ref tb, AID, CID, ItemSeq, OptionSeq, MaterialSeq, useRuby, ref retDeletedItem);
                            }
                            else if (requestOp.Equals("weapon_option_change"))
                            {
                                retError = ItemManager.WeaponOptionChangeAll(ref tb, AID, CID, ItemSeq, MaterialSeq);
                            }
                            else if (requestOp.Equals("optionadd"))
                                retError = ItemManager.AccessoryOptionAdd(ref tb, AID, CID, ItemSeq, OptionSeq, MaterialSeq, ref retDeletedItem);
                            else if (requestOp.Equals("optionadd_reroll"))
                                retError = ItemManager.AccessoryOptionAddReroll(ref tb, AID, CID, ItemSeq, OptionSeq, MaterialSeq);
                            else if (requestOp.Equals("option_allchange"))
                                retError = ItemManager.AccessoryOptionChangeAll(ref tb, AID, CID, ItemSeq, MaterialSeq);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account UserInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    CharacterManager.FlushCharacter(ref tb, AID, CID);
                                    List<User_Inven> CharItem = ItemManager.GetInvenList(ref tb, AID, CID, true, true);

                                    Ret_Inven_Item updateItem = new Ret_Inven_Item(CharItem.Find(item => item.invenseq == ItemSeq));

                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetGold], UserInfo.Gold.ToString());
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetRuby], (UserInfo.Cash + UserInfo.EventCash).ToString());
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.BSuccess], (bSuccess ? 1 : 0).ToString());
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.UpdateItemInfo], mJsonSerializer.ToJsonString(updateItem));
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.DeletedItem], mJsonSerializer.ToJsonString(retDeletedItem));
                                    if (requestOp.Equals("option_allchange") || requestOp.Equals("weapon_option_change"))
                                    {
                                        Ret_Inven_Item material_updateItem = new Ret_Inven_Item(CharItem.Find(item => item.invenseq == MaterialSeq));
                                        json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.MaterialUpdateItemInfo], mJsonSerializer.ToJsonString(material_updateItem));
                                    }
                                }
                            }

                        }
                        else if (requestOp.Equals("disassemble") || requestOp.Equals("disassemble_info"))
                        {
                            string setItemJson = queryFetcher.QueryParam_Fetch("sourceseq");
                            List<long> material_Items = mJsonSerializer.JsonToObject<List<long>>(setItemJson);

                            tb.IsoLevel = requestOp.Equals("disassemble_info") ? IsolationLevel.ReadUncommitted : IsolationLevel.ReadCommitted;

                            List<User_Inven> RetGetItems = new List<User_Inven>();
                            List<Return_DisassableItems_List> retDeletedItem = new List<Return_DisassableItems_List>();
                            int UseRuby = 0;

                            retError = material_Items.Count < 1 ? Result_Define.eResult.ITEM_ID_NOT_FOUND : Result_Define.eResult.SUCCESS;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                foreach (long invenseq in material_Items)
                                {
                                    retError = ItemManager.DisassembleEquip(ref tb, AID, CID, invenseq, requestOp.Equals("disassemble_info"), ref UseRuby, ref RetGetItems, ref retDeletedItem);
                                    if (retError != Result_Define.eResult.SUCCESS)
                                        break;
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS && !requestOp.Equals("disassemble_info"))
                            {
                                List<User_Inven> retResultList = new List<User_Inven>();
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    foreach (User_Inven setItem in RetGetItems)
                                    {
                                        List<User_Inven> makeItems = new List<User_Inven>();
                                        retError = ItemManager.MakeItem(ref tb, ref makeItems, AID, setItem.itemid, setItem.itemea, CID);
                                        if (retError != Result_Define.eResult.SUCCESS)
                                            break;
                                        makeItems.ForEach(item => item.itemea = setItem.itemea);
                                        retResultList.AddRange(makeItems);
                                    }
                                }

                                RetGetItems = retResultList;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<User_Inven> CharItem = ItemManager.GetInvenList(ref tb, AID, CID, true, true);

                                json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.SmeltUseRuby], mJsonSerializer.ToJsonString(UseRuby));
                                json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], Ret_Inven_Item.makeInvenListJson(ref RetGetItems));
                                json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.DeletedItem], mJsonSerializer.ToJsonString(retDeletedItem));
                            }
                        }
                        else if (requestOp.Equals("useitem"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            List<Return_DisassableItems_List> retDeletedItem = new List<Return_DisassableItems_List>();
                            List<User_Inven> RetGetItems = new List<User_Inven>();

                            retError = ItemManager.UseItem_InGame(ref tb, AID, CID, ItemSeq, ref retDeletedItem, ref RetGetItems);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], Ret_Inven_Item.makeInvenListJson(ref RetGetItems));
                                json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.DeletedItem], mJsonSerializer.ToJsonString(retDeletedItem));
                            }
                        }
                        else if (requestOp.Equals("sellitem"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            string setItemJson = queryFetcher.QueryParam_Fetch("sourceseq");
                            int SellCount = queryFetcher.QueryParam_FetchInt("sellcount");
                            bool ignoreSell = queryFetcher.QueryParam_FetchByte("ignoresell") > 0;

                            List<long> material_Items = mJsonSerializer.JsonToObject<List<long>>(setItemJson);
                            if (material_Items == null)
                                material_Items = new List<long>();

                            List<Return_DisassableItems_List> retDeletedItem = new List<Return_DisassableItems_List>();
                            int SellPrice = 0;

                            retError = ItemManager.SellItem_InGame(ref tb, AID, ref SellPrice, ref retDeletedItem, material_Items, SellCount, ignoreSell);

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = AccountManager.AddUserGold(ref tb, AID, SellPrice);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account UserInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    ItemManager.FlushCharacterInvenList(AID, CID);
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetGold], UserInfo.Gold.ToString());
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetRuby], (UserInfo.Cash + UserInfo.EventCash).ToString());
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.SellPrice], SellPrice.ToString());
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.DeletedItem], mJsonSerializer.ToJsonString(retDeletedItem));
                                }
                            }
                        }
                        else if (requestOp.Equals("orb_inven_list"))
                        {
                            List<User_Orb_Inven> OrbInven = ItemManager.GetUserOrbInvenList(ref tb, AID);
                            json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.ItemInventory_Orb], User_Orb_Inven.makeOrbListJson(ref OrbInven));
                            retError = Result_Define.eResult.SUCCESS;
                        }
                        else if (requestOp.Equals("ultimate_unlock"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long ultimate_id = queryFetcher.QueryParam_FetchLong("ultimate_id");

                            System_Ultimate_Weapon ultimateInfo = ItemManager.GetSystem_Ultimate_Weapon(ref tb, ultimate_id);
                            List<User_Inven> makeItem = new List<User_Inven>();
                            retError = ultimateInfo == null ? Result_Define.eResult.ITEM_EQUIP_INFO_NOT_FOUND : Result_Define.eResult.SUCCESS;
                            
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (Trigger_Define.TriggerType.ContainsKey(ultimateInfo.ActiveTriggerType1)
                                    && Trigger_Define.TriggerType.ContainsKey(ultimateInfo.ActiveTriggerType2)
                                    && Trigger_Define.TriggerType.ContainsKey(ultimateInfo.ActiveTriggerType3)
                                    && Trigger_Define.TriggerType.ContainsKey(ultimateInfo.ActiveTriggerType4)
                                    && Trigger_Define.TriggerType.ContainsKey(ultimateInfo.Beyond_ActiveTriggerType)
                                    )
                                {
                                    List<Character> userCharacter = CharacterManager.GetCharacterList(ref tb, AID);
                                    List<RetMissionRank> userMission = Dungeon_Manager.GetUser_All_MissionRank(ref tb, AID);
                                    List<User_GuerrillaDungeon_Play> userGuerillaMission = Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref tb, AID);
                                    List<RetEliteDungeonRank> userElistMission = Dungeon_Manager.GetUser_All_EliteDungeonRank(ref tb, AID);

                                    if (
                                        (
                                        TriggerManager.CheckClearTrigger(ref tb, AID, Trigger_Define.TriggerType[ultimateInfo.ActiveTriggerType1], ultimateInfo.ActiveTriggerType1_Value1, ultimateInfo.ActiveTriggerType1_Value2, ultimateInfo.ActiveTriggerType1_Value3)
                                        && TriggerManager.CheckClearTrigger(ref tb, AID, Trigger_Define.TriggerType[ultimateInfo.ActiveTriggerType2], ultimateInfo.ActiveTriggerType2_Value1, ultimateInfo.ActiveTriggerType2_Value2, ultimateInfo.ActiveTriggerType2_Value3)
                                        && TriggerManager.CheckClearTrigger(ref tb, AID, Trigger_Define.TriggerType[ultimateInfo.ActiveTriggerType3], ultimateInfo.ActiveTriggerType3_Value1, ultimateInfo.ActiveTriggerType3_Value2, ultimateInfo.ActiveTriggerType3_Value3)
                                        && TriggerManager.CheckClearTrigger(ref tb, AID, Trigger_Define.TriggerType[ultimateInfo.ActiveTriggerType4], ultimateInfo.ActiveTriggerType4_Value1, ultimateInfo.ActiveTriggerType4_Value2, ultimateInfo.ActiveTriggerType4_Value3)
                                        )
                                        ||
                                        TriggerManager.CheckClearTrigger(ref tb, AID, Trigger_Define.TriggerType[ultimateInfo.Beyond_ActiveTriggerType], ultimateInfo.Beyond_ActiveTriggerType_Value1, ultimateInfo.Beyond_ActiveTriggerType_Value2, ultimateInfo.Beyond_ActiveTriggerType_Value3)
                                        )
                                    {
                                        retError = ItemManager.MakeItem(ref tb, ref makeItem, AID, ultimate_id, 1, CID);
                                    }
                                    else
                                        retError = Result_Define.eResult.TRIGGER_IS_NOT_CLEAR;
                                }
                                else
                                    retError = Result_Define.eResult.TRIGGER_EVENT_TYPE_EMPTY;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<User_Ultimate_Inven> ultimateList = ItemManager.GetUserUltimateWeaponList(ref tb, AID, CID);
                                var retObj = ultimateList.Find(item => item.item_id == ultimate_id);
                                if (retObj != null)
                                {
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeItem));
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.UltimateInfo], retObj.ToJson());
                                }
                            }
                        }
                        else if (requestOp.Equals("ultimate_equip") || requestOp.Equals("ultimate_unequip"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long ultimateSeq = queryFetcher.QueryParam_FetchLong("ultimate_seq");
                            bool isEquip = requestOp.Equals("ultimate_equip");

                            List<User_Ultimate_Inven> itemList = ItemManager.GetUserUltimateWeaponList(ref tb, AID, CID);
                            long beforeEquipedSeq = 0;
                            var beforeEquiped = itemList.Find(setItem => setItem.equipflag.Equals("Y"));
                            if (beforeEquiped != null)
                                beforeEquipedSeq = beforeEquiped.ultimate_inven_seq;
                            long afterEquiped = isEquip ? ultimateSeq : 0;

                            retError = ItemManager.EquipUltimate(ref tb, AID, CID, ultimateSeq, isEquip);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.BeforeEquipWeapon], beforeEquipedSeq.ToString());
                                json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.AfterEquipWeapon], afterEquiped.ToString());
                            }
                        }
                        else if (requestOp.Equals("ultimate_enchant"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long ultimateSeq = queryFetcher.QueryParam_FetchLong("ultimate_seq");
                            short tryCount = queryFetcher.QueryParam_FetchShort("try_count", 1);

                            List<Return_DisassableItems_List> retDeletedItem = new List<Return_DisassableItems_List>();
                            User_Ultimate_Inven userItem = new User_Ultimate_Inven();
                            retError = ItemManager.EnchantUltimate(ref tb, AID, CID, ultimateSeq, ref retDeletedItem, ref userItem, tryCount);
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account UserInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                List<User_Inven> CharItem = ItemManager.GetInvenList(ref tb, AID, CID, true, true);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetGold], UserInfo.Gold.ToString());
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetRuby], (UserInfo.Cash + UserInfo.EventCash).ToString());
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.DeletedItem], mJsonSerializer.ToJsonString(retDeletedItem));
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.UltimateInfo], userItem.ToJson());
                                }
                            }
                        }
                        else if (requestOp.Equals("orb_equip") || requestOp.Equals("orb_unequip"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long ultimateSeq = queryFetcher.QueryParam_FetchLong("ultimate_seq");
                            bool isEquip = requestOp.Equals("orb_equip");
                            string equipInfo = queryFetcher.QueryParam_Fetch("orb_equip_info", "[]");

                            List<User_Equip_Orb_Request> equip_Items = mJsonSerializer.JsonToObject<List<User_Equip_Orb_Request>>(equipInfo);
                            if (equip_Items == null)
                                equip_Items = new List<User_Equip_Orb_Request>();

                            List<User_Orb_Inven> retObjInfo = new List<User_Orb_Inven>();
                            foreach (User_Equip_Orb_Request setEquip in equip_Items)
                            {
                                User_Orb_Inven userOrb = new User_Orb_Inven();
                                retError = ItemManager.EquipOrbToUltimate(ref tb, AID, CID, ultimateSeq, setEquip.orb_seq, setEquip.slot_num, isEquip, ref userOrb);
                                retObjInfo.Add(userOrb);
                            }
                            
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                User_Ultimate_Inven userItem = ItemManager.GetUserUltimateWeaponList(ref tb, AID, CID, true, true).Find(item => item.ultimate_inven_seq == ultimateSeq);
                                if (userItem == null)
                                    retError = Result_Define.eResult.ITEM_ID_NOT_FOUND;

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.UltimateInfo], userItem.ToJson());
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.OrbInfo], User_Orb_Inven.makeOrbListJson(ref retObjInfo));
                                }
                            }
                        }
                        else if (requestOp.Equals("orb_mix"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long ultimateSeq = queryFetcher.QueryParam_FetchLong("ultimate_seq");
                            long orbSeq = queryFetcher.QueryParam_FetchLong("orb_seq");
                            string setItemJson = queryFetcher.QueryParam_Fetch("sourceseq");

                            List<long> material_Items = mJsonSerializer.JsonToObject<List<long>>(setItemJson);
                            if (material_Items == null)
                                material_Items = new List<long>();

                            User_Ultimate_Inven userItem = new User_Ultimate_Inven();
                            User_Orb_Inven userOrb = new User_Orb_Inven();
                            retError = ItemManager.MixUltimateOrb(ref tb, AID, CID, ultimateSeq, orbSeq, material_Items, ref userItem, ref userOrb);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.UltimateInfo], userItem.ToJson());
                                json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.OrbInfo], userOrb.ToJsonObj().ToJson());
                            }
                        }

                        // for test operation function
                        else if (Request.Params.AllKeys.Contains("Debug"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            retError = Result_Define.eResult.SUCCESS;
                            if (requestOp.Equals("makeitem"))
                            {
                                long itemID = queryFetcher.QueryParam_FetchLong("itemid");
                                int makeCount = queryFetcher.QueryParam_FetchInt("makecount");
                                short makeGrade = queryFetcher.QueryParam_FetchShort("makegrade");
                                short makeLevel = queryFetcher.QueryParam_FetchShort("makelevel");
                                System_Item_Base setItemInfo = ItemManager.GetSystem_Item_Base(ref tb, itemID);
                                if (setItemInfo != null)
                                    makeCount = (setItemInfo.StackMAX * 10) < makeCount && setItemInfo.StackMAX > 0 ? (setItemInfo.StackMAX * 10): makeCount;                                    
                                else
                                    makeCount = 1;

                                List<User_Inven> makeItem = new List<User_Inven>();
                                retError = ItemManager.MakeItem(ref tb, ref makeItem, AID, itemID, makeCount, CID, makeLevel, makeGrade);

                                if (retError == Result_Define.eResult.SUCCESS)
                                    json = mJsonSerializer.AddJson(json, "makeItem", mJsonSerializer.ToJsonString(makeItem));
                            }
                            else if (requestOp.Equals("flushitem"))
                            {
                                ItemManager.ItemDeleteAll(ref tb, AID, CID);
                                List<User_Inven> itemList = ItemManager.GetInvenList(ref tb, AID, CID, true, true);
                                json = mJsonSerializer.AddJson(json, "itemList", mJsonSerializer.ToJsonString(itemList));
                            }
                            else if (requestOp.Equals("deleteitem"))
                            {
                                ItemManager.ItemDeleteAll(ref tb, AID, CID);
                                List<User_Inven> itemList = ItemManager.GetInvenList(ref tb, AID, CID, true, true);
                                json = mJsonSerializer.AddJson(json, "itemList", mJsonSerializer.ToJsonString(itemList));
                            }

                            else if (requestOp.Equals("useitem_debug"))
                            {
                                int useCount = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("usecount"));
                                List<Return_DisassableItems_List> retDeletedItem = new List<Return_DisassableItems_List>();
                                retError = ItemManager.UseItem(ref tb, AID, ItemSeq, useCount, ref retDeletedItem);
                                List<User_Inven> itemList = ItemManager.GetInvenList(ref tb, AID, CID, true, true);
                                json = mJsonSerializer.AddJson(json, "itemList", mJsonSerializer.ToJsonString(itemList));
                            }
                        }
                        retJson = queryFetcher.Render(json.ToJson(), retError);
                    }
                    else
                    {
                        retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(DefineError.System_Unknown_Operation), Result_Define.eResult.System_Unknown_Operation);
                    }
                }
                catch (Exception errorEx)
                {
                    string error = "";
#if DEBUG
                    error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
#else
                    if (queryFetcher.SetDebugMode)
                        error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
#endif

                    error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(error), Result_Define.eResult.System_Exception);
                }
                finally
                {
                    //if (AID > 0)
                    //    queryFetcher.CheckSnail_ID(ref tb, AID);
                    queryFetcher.SetShowLogMode = tb.EndTransaction(queryFetcher.Render_errorFlag);
                    queryFetcher.ErrorLogWrite(retJson, ref tb);
                    tb.Dispose();
                } 
            }
        }
    }
}