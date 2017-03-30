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
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using ServiceStack.Text;
using logWeb;
using TheSoulWebServer.Platform;

namespace TheSoulWebServer.Platform
{
    public class DealOp
    {
        public DealOp()
        {

        }

        public static void dealRequestOp(ref TxnBlock tb, ref WebQueryParam queryFetcher, ref string requestOp, ref JsonObject json, ref long AID, ref Result_Define.eResult retError)
        {
            queryFetcher.operation = requestOp;
            //Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            long CID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("cid"));

            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.op], requestOp);
            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], AID);
            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.cid], CID);

            DataManager_Define.eCountryCode serviceArea = SystemData.GetServiceArea(ref tb);
            
            // replace gacha contents to treasure box (only use china version)
            if (requestOp.Equals("treasure_box_list") && serviceArea == DataManager_Define.eCountryCode.China)
            {
                int remainTime = 0;
                User_Shop_TreasureBox userBoxInfo = ShopManager.GetUser_TreasureBox(ref tb, AID, CID, ref remainTime, ref retError);

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    //User_Shop_TreasureBox_List retBoxList = mJsonSerializer.JsonToObject<User_Shop_TreasureBox_List>(userBoxInfo.ShopItemList);

                    json = JsonObject.Parse(userBoxInfo.ShopItemList);
                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetGoldBuyCount], userBoxInfo.GoldBuyCount.ToString());
                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetRubyBuyCount], userBoxInfo.RubyBuyCount.ToString());
                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetRemainTime], remainTime.ToString());
                }
            }
            else if (requestOp.Equals("buy_treasure_box") && serviceArea == DataManager_Define.eCountryCode.China)
            {
                string buySlots = queryFetcher.QueryParam_Fetch("buy_slots");
                List<int> buySlotList = mJsonSerializer.JsonToObject<List<int>>(buySlots);
                int boxType = queryFetcher.QueryParam_FetchInt("box_type");

                int remainTime = 0;
                int useRuby = 0;
                int useGold = 0;
                int remainRubyCount = 0;
                List<User_Inven> makeItem = new List<User_Inven>();
                List<RetShopTreasureBoxItem> ReplaceItem = new List<RetShopTreasureBoxItem>();
                User_Shop_TreasureBox userBoxInfo = ShopManager.ReplaceUser_TreasureBox(ref tb, AID, CID, (Shop_Define.eGachaType)boxType, buySlotList, out remainRubyCount, ref makeItem, ref ReplaceItem, ref remainTime, ref retError, out useGold, out useRuby, true);

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    if((Shop_Define.eGachaType)boxType != Shop_Define.eGachaType.TREASURE_BOX_SPECIAL)
                        retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.GACHASHOP, 0, 0, buySlotList.Count);
                    else
                        retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.GACHASHOP_SPECIAL, 0, 0, 1);
                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id],
                        (Shop_Define.eGachaType)boxType == Shop_Define.eGachaType.TREASURE_BOX_GOLD ? (long)SnailLog_Define.GachaOperationSID.TREASURE_BOX_GOLD :
                        ((Shop_Define.eGachaType)boxType == Shop_Define.eGachaType.TREASURE_BOX_CASH ? (long)SnailLog_Define.GachaOperationSID.TREASURE_BOX_CASH :
                        (long)SnailLog_Define.GachaOperationSID.TREASURE_BOX_SPECIAL)
                        );
                }

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                    Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount);

                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeItem));
                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetGold], mJsonSerializer.ToJsonString(useGold));
                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetRuby], mJsonSerializer.ToJsonString(useRuby));
                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetRubyBuyCount], remainRubyCount.ToString());
                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetReplaceBoxItem], mJsonSerializer.ToJsonString(ReplaceItem));
                }
            }
            // not use gacha in china version
            else if ((requestOp.Equals("draw_gacha") || requestOp.Equals("free_gacha") || requestOp.Equals("free_premium_gacha")) && serviceArea != DataManager_Define.eCountryCode.China)
            {
                tb.IsoLevel = IsolationLevel.ReadCommitted;
                Shop_Define.eGachaType GachaType = (Shop_Define.eGachaType)queryFetcher.QueryParam_FetchInt("gacha_type");

                if (VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.BAGSLOT_MAX_ITEM))
                    retError = Result_Define.eResult.SUCCESS;
                else
                    retError = Result_Define.eResult.ITEM_INVENTORY_OVER;

                Character charInfo = null;
                RetGachaInfo setGachainfo = null;
                User_Gacha_Special_Info setSepcialInfo = ShopManager.GetUser_Gacha_Special_Info(ref tb, AID);

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    charInfo = CharacterManager.GetCharacter(ref tb, AID, CID);
                    setGachainfo = new RetGachaInfo(ref tb, ShopManager.GetUserGachaInfo(ref tb, AID), setSepcialInfo);
                }
                else
                {
                    charInfo = new Character();
                    setGachainfo = new RetGachaInfo();
                }

                List<User_Inven> makeRealItem = new List<User_Inven>();

                int needGold = 0;
                int needCash = 0;
                //long gachaSID = requestOp.Equals("free_gacha") || requestOp.Equals("free_premium_gacha") ? SnailLog_Define.Snail_s_id_Seperator_shop_free_gacha : SnailLog_Define.Snail_s_id_Seperator_shop_gacha;

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    if (requestOp.Equals("free_gacha"))
                    {
                        if (setGachainfo.free_left_time > 0)
                            retError = Result_Define.eResult.SHOP_FREE_GACHA_LEFT_TIME_OVER;
                        else if (setGachainfo.use_freecount >= Shop_Define.Shop_FreeGacha_MaxCount)
                            retError = Result_Define.eResult.SHOP_FREE_GACHA_COUNT_OVER;
                        else if (GachaType != Shop_Define.eGachaType.NORMAL_TRY_ONE)
                            retError = Result_Define.eResult.SHOP_FREE_GACHA_TYPE_INVALIDE;
                        else
                            retError = Result_Define.eResult.SUCCESS;
                    }
                    else if (requestOp.Equals("free_premium_gacha"))
                    {
                        if (setGachainfo.premium_left_time > 0)
                            retError = Result_Define.eResult.SHOP_FREE_PREMIUM_GACHA_LEFT_TIME_OVER;
                        else if ((Shop_Define.eGachaType)GachaType != Shop_Define.eGachaType.PREMIUM_TRY_ONE)
                            retError = Result_Define.eResult.SHOP_FREE_GACHA_TYPE_INVALIDE;
                        else
                            retError = Result_Define.eResult.SUCCESS;
                    }
                    else
                        retError = Result_Define.eResult.SUCCESS;
                }

                //if (retError == Result_Define.eResult.SUCCESS && GachaType == Shop_Define.eGachaType.BEST_TRY_ONE)
                //{
                //    if (!VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.UNLOCK_SPECIALCHEST))
                //        retError = Result_Define.eResult.VIP_CONTENTS_LOCKED;
                //}

                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_count], "1");

                // check drop item
                if (charInfo.cid > 0 && retError == Result_Define.eResult.SUCCESS)
                {
                    List<System_Drop_Group> getDropList = new List<System_Drop_Group>();
                    System_Gacha_Level getGachaLevelMatch = ShopManager.GetSystem_Shop_Gacha_Level(ref tb, charInfo.level);

                    int setGachaGroup = 0;

                    if (GachaType == Shop_Define.eGachaType.NORMAL_TRY_ONE && (setGachainfo.normal_special_count + 1) >= setGachainfo.normal_special_max_count)
                        setGachaGroup = SystemData.GetConstValueInt(ref tb, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.Shop_Gacha_Special_Normal_GetType_Def_List[setSepcialInfo.Nomal_GachaSpeciaGetCount]]);
                    else if (GachaType == Shop_Define.eGachaType.PREMIUM_TRY_ONE && (setGachainfo.premium_special_count + 1) >= setGachainfo.premium_special_max_count)
                        setGachaGroup = SystemData.GetConstValueInt(ref tb, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.Shop_Gacha_Special_Premium_GetType_Def_List[setSepcialInfo.Premium_GachaSpecialGetCount]]);
                    else
                        setGachaGroup = (int)GachaType;

                    if (GachaType == Shop_Define.eGachaType.BEST_TRY_ONE)
                    {
                        System_Gacha_Best bestGachaInfo = ShopManager.GetSystem_Shop_Gacha_Best(ref tb);
                        byte checkGachaBestOpen = (byte)SystemData.AdminConstValueFetchFromRedis(ref tb, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.ADMIN_GACHA_BEST_ON_OFF]);
                        int bestGachaOpenLevel = (int)SystemData.GetConstValueInt(ref tb, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.DEF_GACHA_OPENLEVEL_SPECIAL]);
                        if (bestGachaInfo.GachaIndex > 0 && checkGachaBestOpen > 0)
                        {
                            if (CharacterManager.GetCharacterMaxLevel_FromDB(ref tb, AID) >= bestGachaOpenLevel)
                            {
                                needCash = bestGachaInfo.Gacha_Cash;
                                retError = DropManager.PickBestGachaDropGroup(ref tb, AID, ref getDropList, bestGachaInfo.GachaIndex);
                            }
                            else
                                retError = Result_Define.eResult.SHOP_BEST_GACHA_NOT_OPEN;
                        }
                        else
                            retError = Result_Define.eResult.SHOP_BEST_GACHA_NOT_OPEN;
                    }
                    else
                        getDropList = DropManager.GetGachaResult(ref tb, ref needGold, ref needCash, setGachaGroup, getGachaLevelMatch.Level_MatchingID, (short)charInfo.Class);

                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        foreach (System_Drop_Group setDrop in getDropList)
                        {
                            List<User_Inven> getItem = new List<User_Inven>();
                            // drop limite check and make count calc for soul parts
                            System_Drop_Group setRealDrop = setDrop.DropMaxNum >= 10 || setDrop.DropMinNum >= 10 ? DropManager.CheckDropLimit(ref tb, setDrop, AID, CID, ref retError) : setDrop;

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = DropManager.MakeDropItem(ref tb, ref getItem, setRealDrop, AID, CID);

                            if (retError == Result_Define.eResult.SUCCESS)
                                makeRealItem.AddRange(getItem);
                            else
                                break;
                        }
                    }
                }
                else if (retError == Result_Define.eResult.SUCCESS)
                    retError = Result_Define.eResult.CHARACTER_NOT_FOUND;

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    if (makeRealItem.Count < 1)
                        retError = Result_Define.eResult.ITEM_CREATE_FAIL;
                    else
                        retError = Result_Define.eResult.SUCCESS;
                }

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    if (requestOp.Equals("draw_gacha") && (needGold > 0 || needCash > 0))
                        retError = AccountManager.UseUserGold_And_Ruby(ref tb, AID, needGold, needCash);
                    else if (requestOp.Equals("free_gacha"))
                        retError = ShopManager.UpdateUserGachaCount(ref tb, AID, true);
                    else if (requestOp.Equals("free_premium_gacha"))
                        retError = ShopManager.UpdateUserGachaCount(ref tb, AID, false);
                }

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    if (GachaType == Shop_Define.eGachaType.NORMAL_TRY_ONE)
                        retError = ShopManager.UpdateUserNormalGacha_Special_Info(ref tb, AID, setGachainfo.normal_special_count, setGachainfo.normal_special_max_count, setSepcialInfo.Nomal_GachaSpeciaGetCount);
                    else if (GachaType == Shop_Define.eGachaType.PREMIUM_TRY_ONE)
                        retError = ShopManager.UpdateUserPremiumGacha_Special_Info(ref tb, AID, setGachainfo.premium_special_count, setGachainfo.premium_special_max_count, setSepcialInfo.Premium_GachaSpecialGetCount);
                }

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    switch (GachaType)
                    {
                        case Shop_Define.eGachaType.NORMAL_TRY_ONE:
                            retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.Gacha, (int)Trigger_Define.eGachaType.NORMAL_TRY_ONE);
                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], requestOp.Equals("free_gacha") ? (long)SnailLog_Define.GachaOperationSID.FREE_NORMAL_TRY_ONE : (long)SnailLog_Define.GachaOperationSID.NORMAL_TRY_ONE);
                            break;
                        case Shop_Define.eGachaType.NORMAL_TRY_TEN:
                            retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.Gacha, (int)Trigger_Define.eGachaType.NORMAL_TRY_TEN);
                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.GachaOperationSID.NORMAL_TRY_TEN);
                            break;
                        case Shop_Define.eGachaType.PREMIUM_TRY_ONE:
                            retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.Gacha, (int)Trigger_Define.eGachaType.PREMIUM_TRY_ONE);
                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], requestOp.Equals("free_premium_gacha") ? (long)SnailLog_Define.GachaOperationSID.FREE_PREMIUM_TRY_ONE : (long)SnailLog_Define.GachaOperationSID.PREMIUM_TRY_ONE);
                            break;
                        case Shop_Define.eGachaType.PREMIUM_TRY_TEN:
                            retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.Gacha, (int)Trigger_Define.eGachaType.PREMIUM_TRY_TEN);
                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.GachaOperationSID.PREMIUM_TRY_TEN);
                            break;
                        case Shop_Define.eGachaType.BEST_TRY_ONE:
                            retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.Gacha, (int)Trigger_Define.eGachaType.BEST_TRY_ONE);
                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.GachaOperationSID.BEST_TRY_ONE);
                            break;
                    }
                }

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                    Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount);
                    setGachainfo = new RetGachaInfo(ref tb, ShopManager.GetUserGachaInfo(ref tb, AID, true), ShopManager.GetUser_Gacha_Special_Info(ref tb, AID));

                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetGold], mJsonSerializer.ToJsonString(needGold));
                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetRuby], mJsonSerializer.ToJsonString(needCash));
                    json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.FreeGachaInfo], mJsonSerializer.ToJsonString(setGachainfo));
                }
            }
            else if (requestOp.Equals("shop_package_list") || requestOp.Equals("shop_cheap_package_list"))
            {
                retError = Result_Define.eResult.SUCCESS;

                bool bPackageOn = true;
                Shop_Define.eBillingType BillingType = (Shop_Define.eBillingType)queryFetcher.QueryParam_FetchInt("billingtype");

                if (requestOp.Equals("shop_cheap_package_list"))
                    bPackageOn = SystemData.AdminConstValueFetchFromRedis(ref tb, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.ADMIN_PACKAGE_CHEAP_ON_OFF]) > 0;

                List<RetPackageList> setList = bPackageOn ? ShopManager.GetShop_Package_List(ref tb, BillingType, AID, CID, requestOp.Equals("shop_cheap_package_list")) : new List<RetPackageList>();
                json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.ShopPackageList], mJsonSerializer.ToJsonString(setList));
            }
            else if (requestOp.Equals("buy_package") || requestOp.Equals("buy_cheap_package"))
            {
                tb.IsoLevel = IsolationLevel.ReadCommitted;
                long PackageID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("package_id"));
                string Billing_Platform_UID = queryFetcher.QueryParam_Fetch("billing_uid");
                string Billing_ID = queryFetcher.QueryParam_Fetch("billing_id");
                string Billing_Token = queryFetcher.QueryParam_Fetch("billing_token");
                int BillingType = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("billing_type", "0"));
                int BuyCount = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("buy_count", "1"));

                Shop_Define.eShopItemType shopItemType = requestOp.Equals("buy_cheap_package") ? Shop_Define.eShopItemType.Chep_Package : Shop_Define.eShopItemType.Package;
                Item_Define.eItemBuyPriceType BuyPriceType = Item_Define.eItemBuyPriceType.None;
                int BuyPriceValue = 0;
                List<User_Inven> makeRealItem = new List<User_Inven>();

                           

                retError = ShopManager.BuyShopItemProgress(ref tb, AID, CID, PackageID, shopItemType
                                                            , Shop_Define.eShopType.None, BuyCount, ref makeRealItem, ref BuyPriceType
                                                            , ref BuyPriceValue, (Shop_Define.eBillingType)BillingType
                                                            , Billing_Platform_UID, Billing_ID, Billing_Token);


                //mycard (等待封装)
                string AuthCode = string.Empty;
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    if (MyCard.IsMyCard((Shop_Define.eBillingType)BillingType))
                    {
                        string ip = queryFetcher.QueryParam_Fetch("ip", "");
                        string port = queryFetcher.QueryParam_Fetch("port", "");
                        string platBilling_Token = TheSoulDBcon.server_group_id + "-" + Billing_Token;

                        User_Billing_List theorder = ShopManager.GetUserBillingInfoByToken(ref tb, Billing_Token);
                        if (theorder != null && !string.IsNullOrEmpty(theorder.Billing_Token))
                        {
                            int group_id = TheSoulDBcon.server_group_id;

                            platBilling_Token = group_id + "_" + theorder.BillingIndex;
                        }

                        retError = ShopManager.UpdateUserPlatformBillingID(ref tb, platBilling_Token, Billing_ID);
                        if (retError == Result_Define.eResult.SUCCESS)
                        {

                            string webIPandPort = ip + "-" + port;
                            string Uri = "http://107.150.101.9:5100/MyCardAuthcode.aspx";
                            // string Uri = "http://127.0.0.1:5100/MyCardAuthcode.aspx";


                            string oldbilling_type = queryFetcher.QueryParam_Fetch("billing_type");

                            string dataParams = "token=" + platBilling_Token + "&amount=" + BuyPriceValue + "&groupid=" + TheSoulDBcon.server_group_id + "&product_id=" + PackageID + "&billingType=" + oldbilling_type + "&aid=" + AID;

                            string retBody = GlobalManager.GetReqeustURL(Uri, dataParams, false);
                            if (string.IsNullOrEmpty(retBody))
                            {
                                retError = Result_Define.eResult.SHOP_ITEM_BUY_TIME_INVALIDE;   

                            }
                            else
                            {
                                JsonObject jsontem = new JsonObject();
                                jsontem = JsonObject.Parse(retBody);
                                string ReturnCode = string.Empty;    
                                if (jsontem.TryGetValue("AuthCode", out AuthCode))
                                {
                                    //retError = Result_Define.eResult.SUCCESS;
                                    retError = ShopManager.UpdateUserPlatformBillingToken(ref tb, AuthCode, theorder.BillingIndex);  

                                }
                                                
                                            
                            }



                        }
                    }

                }

                //end

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount);
                        User_Shop_Buy userCurrentBuy = ShopManager.GetUserBuyItemCount(ref tb, AID, PackageID, true);

                        json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                        json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                        json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.BuyPriceType], mJsonSerializer.ToJsonString((int)BuyPriceType));
                        json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.BuyPriceValue], mJsonSerializer.ToJsonString(BuyPriceValue));

                        if (MyCard.IsMyCard((Shop_Define.eBillingType)BillingType ))
                        {
                            json = mJsonSerializer.AddJson(json, "authcode", AuthCode);
                        }
                                
                    }
                }
            }
            else if (requestOp.Equals("shop_list"))
            {
                retError = Result_Define.eResult.SUCCESS;
                Shop_Define.eShopType ShopType = (Shop_Define.eShopType)queryFetcher.QueryParam_FetchInt("shoptype");
                Shop_Define.eBillingType BillingType = (Shop_Define.eBillingType)queryFetcher.QueryParam_FetchInt("billingtype");
                int shopRemainTime = 0;
                List<RetShopItem> shopItemList = ShopManager.GetUser_ShopList(ref tb, AID, ShopType, BillingType, ref shopRemainTime, ref retError);
                User_Shop_Reset resetInfo = ShopManager.GetUserShopResetInfo(ref tb, AID, ShopType);

                if (ShopType == Shop_Define.eShopType.Cash || ShopType == Shop_Define.eShopType.Billing)
                {
                    RetShopSubscription main_subscriptInfo = ShopManager.GetUserSubscriptionLeftDay(ref tb, AID, false);
                    RetShopSubscription week_subscriptInfo = ShopManager.GetUserSubscriptionLeftDay(ref tb, AID, true);
                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetLeftSubscription], mJsonSerializer.ToJsonString(main_subscriptInfo));
                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetLeftWeekSubscription], mJsonSerializer.ToJsonString(week_subscriptInfo));
                }
                else if (ShopType == Shop_Define.eShopType.BlackMarket)
                {
                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetRemainTime], shopRemainTime.ToString());
                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetResetRuby], SystemData.AdminConstValueFetchFromRedis(ref tb, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.DEF_BLACKMARKET_SHOP_RESET_COST_RUBY]).ToString());
                }
                json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetCurrentCount], resetInfo.reset_count.ToString());
                json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.ShopItemList], mJsonSerializer.ToJsonString(shopItemList));
            }
            else if (requestOp.Equals("create_billing_id"))
            {
                Shop_Define.eBillingType BillingType = (Shop_Define.eBillingType)queryFetcher.QueryParam_FetchInt("billing_type");
                long buyGoodsID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("shop_item_id"));
                if(buyGoodsID == 0)
                    buyGoodsID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("package_id"));
                string productID = queryFetcher.QueryParam_Fetch("product_id");
                string billingID = string.Empty;
                if (buyGoodsID > 0)
                    retError = ShopManager.CreateBillingID(ref tb, AID, buyGoodsID, BillingType, productID, out billingID);
                else
                    retError = Result_Define.eResult.SHOP_ID_NOT_FOUND;

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    //User_Billing_AuthKey getUserBillingKey = ShopManager.GetBillingID(ref tb, AID, billingID, productID);
                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetBillingID], billingID);
                }
            }
            else if (requestOp.Equals("get_product_id"))
            {
                retError = Result_Define.eResult.SUCCESS;
                long BuyGoodsID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("shop_item_id"));
                Shop_Define.eBillingType BillingType = (Shop_Define.eBillingType)queryFetcher.QueryParam_FetchInt("billing_type");
                System_Shop_Goods_Code getCode = ShopManager.GetSystem_ShopGoodsCode(ref tb, BuyGoodsID, BillingType);
                json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetProductID], string.IsNullOrEmpty(getCode.Product_ID) ? "" : getCode.Product_ID);

            }
            else if (requestOp.Equals("buy_shop_item") || requestOp.Equals("buy_billing_cash"))
            {
                long BuyGoodsID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("shop_item_id"));
                string Billing_Platform_UID = queryFetcher.QueryParam_Fetch("billing_uid", "");
                string Billing_ID = queryFetcher.QueryParam_Fetch("billing_id");
                string Billing_Token = queryFetcher.QueryParam_Fetch("billing_token");
                Shop_Define.eBillingType BillingType = (Shop_Define.eBillingType)queryFetcher.QueryParam_FetchInt("billing_type");
                int BuyCount = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("buy_count", "1"));
                Shop_Define.eShopType ShopType = (Shop_Define.eShopType)System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("shoptype", "0"));
                tb.IsoLevel = IsolationLevel.ReadCommitted;
                string productID = queryFetcher.QueryParam_Fetch("product_id");

                //q下面渠道就重新获得这些订单
                if (BillingType == Shop_Define.eBillingType.Kr_aOS_Google
                    || BillingType == Shop_Define.eBillingType.Kr_iOS_Appstore
                    || BillingType == Shop_Define.eBillingType.Kr_aOS_OneStore
                    || BillingType == Shop_Define.eBillingType.Global_aOS_Google
                    || BillingType == Shop_Define.eBillingType.Global_iOS_Appstore
                    || BillingType == Shop_Define.eBillingType.Global_iOS_MOL
                    || BillingType == Shop_Define.eBillingType.Global_iOS_MOLPin
                    || BillingType == Shop_Define.eBillingType.mfun_iOS_MOL
                    || BillingType == Shop_Define.eBillingType.mfun_iOS_MOLPin
                    || BillingType == Shop_Define.eBillingType.Global_aOS_MyCard
                    || BillingType == Shop_Define.eBillingType.Global_iOS_MyCard
                    || BillingType == Shop_Define.eBillingType.mfun_aOS_Mycard
                    || BillingType == Shop_Define.eBillingType.mfun_iOS_Mycard
                    )
                {
                    User_Billing_AuthKey getUserBillingKey = ShopManager.GetBillingID(ref tb, AID, Billing_ID, productID);

                    if (!string.IsNullOrEmpty(getUserBillingKey.BillingAuthKey))
                    {
                        Billing_ID = getUserBillingKey.BillingAuthKey;
                        BuyGoodsID = getUserBillingKey.Shop_Goods_ID;
                        productID = getUserBillingKey.Product_ID;
                        retError = Result_Define.eResult.SUCCESS;
                    }
                    else
                        retError = Result_Define.eResult.SHOP_ID_NOT_FOUND;
                }
                else
                    retError = Result_Define.eResult.SUCCESS;

                Shop_Define.eShopItemType shopItemType = requestOp.Equals("buy_billing_cash") ? Shop_Define.eShopItemType.Cash : Shop_Define.eShopItemType.Item;
                Item_Define.eItemBuyPriceType BuyPriceType = Item_Define.eItemBuyPriceType.None;
                int BuyPriceValue = 0;
                List<User_Inven> makeRealItem = new List<User_Inven>();

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    retError = ShopManager.BuyShopItemProgress(ref tb, AID, CID, BuyGoodsID, shopItemType
                                                                , ShopType, BuyCount, ref makeRealItem, ref BuyPriceType
                                                                , ref BuyPriceValue, (Shop_Define.eBillingType)BillingType
                                                                , Billing_Platform_UID, Billing_ID, Billing_Token, productID);
                }


                //mycard (等待封装)
                string AuthCode = string.Empty;
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    if( MyCard.IsMyCard( BillingType ) )
                    {
                        string ip = queryFetcher.QueryParam_Fetch("ip", "");
                        string port = queryFetcher.QueryParam_Fetch("port", "");
                        string platBilling_Token = TheSoulDBcon.server_group_id + "-" + Billing_Token;

                        User_Billing_List theorder = ShopManager.GetUserBillingInfoByToken(ref tb, Billing_Token);
                        if (theorder != null && !string.IsNullOrEmpty(theorder.Billing_Token))
                        {
                            int group_id = TheSoulDBcon.server_group_id;

                            platBilling_Token = group_id + "_" + theorder.BillingIndex;
                        }

                        retError = ShopManager.UpdateUserPlatformBillingID(ref tb, platBilling_Token, Billing_ID);
                        if (retError == Result_Define.eResult.SUCCESS)
                        {

                            string webIPandPort = ip + "-" + port;
                            string Uri = "http://107.150.101.9:5100/MyCardAuthcode.aspx";
                            //string Uri = "http://127.0.0.1:5100/MyCardAuthcode.aspx";
                            string oldbilling_type = queryFetcher.QueryParam_Fetch("billing_type");
                            string dataParams = "token=" + platBilling_Token + "&amount=" + BuyPriceValue + "&groupid=" + TheSoulDBcon.server_group_id + "&product_id=" + productID + "&billingType=" + oldbilling_type + "&aid=" + AID;
                            string retBody = GlobalManager.GetReqeustURL(Uri, dataParams, false);
                            MyLog4NetInfo.LogInfo("mycard retBody::" + retBody);
                            if (string.IsNullOrEmpty(retBody))
                            {
                                retError = Result_Define.eResult.SHOP_ITEM_BUY_TIME_INVALIDE;
                            }
                            else
                            {
                                JsonObject jsontem = new JsonObject();
                                jsontem = JsonObject.Parse(retBody);
                                if (jsontem.TryGetValue("AuthCode", out AuthCode))
                                {
                                    //retError = Result_Define.eResult.SUCCESS;
                                    retError = ShopManager.UpdateUserPlatformBillingToken(ref tb, AuthCode, theorder.BillingIndex);
                                }
                            }



                        }
                    }

                }

                //end


                //paypal (等待封装)
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    if (BillingType == Shop_Define.eBillingType.Global_aOS_PayPal
                        || BillingType == Shop_Define.eBillingType.Global_iOS_PayPal
                        || BillingType == Shop_Define.eBillingType.mfun_aOS_Paypal
                        || BillingType == Shop_Define.eBillingType.mfun_aOS_Paypal)
                    {
                        string platBilling_Token = TheSoulDBcon.server_group_id + "-" + Billing_Token;

                        User_Billing_List theorder = ShopManager.GetUserBillingInfoByToken(ref tb, Billing_Token);
                        if (theorder != null && !string.IsNullOrEmpty(theorder.Billing_Token))
                        {
                            int group_id = TheSoulDBcon.server_group_id;

                            platBilling_Token = group_id + "_" + theorder.BillingIndex;
                        }

                        retError = ShopManager.UpdateUserPlatformBillingID(ref tb, platBilling_Token, Billing_ID);
                        if (retError == Result_Define.eResult.SUCCESS)
                        {
                            //获取paypal token
                            string Uri = "http://107.150.101.9:8980/merchant-sdk-php-master/samples/ExpressCheckout/SetExpressCheckout.php";
                            string oldbilling_type = queryFetcher.QueryParam_Fetch("billing_type");

                            string dataParams = "currencyCode=" + platBilling_Token + "&itemAmount=" + BuyPriceValue + "&groupid=" + TheSoulDBcon.server_group_id + "&product_id=" + productID + "&billingType=" + oldbilling_type + "&aid=" + AID;

                            string retBody = GlobalManager.GetReqeustURL(Uri, dataParams, false);
                            if (string.IsNullOrEmpty(retBody))
                            {
                                retError = Result_Define.eResult.SHOP_ITEM_BUY_TIME_INVALIDE;

                            }
                            else
                            {
                                JsonObject jsontem = new JsonObject();
                                jsontem = JsonObject.Parse(retBody);
                                if (jsontem.TryGetValue("AuthCode", out AuthCode))
                                {
                                    retError = ShopManager.UpdateUserPlatformBillingToken(ref tb, AuthCode, theorder.BillingIndex);
                                }
                            }



                        }
                    }

                }

                //end

                          



                        
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount);
                        User_Shop_Buy userCurrentBuy = ShopManager.GetUserBuyItemCount(ref tb, AID, BuyGoodsID, true);

                        json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                        json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                        json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.BuyPriceType], mJsonSerializer.ToJsonString((int)BuyPriceType));
                        json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.BuyPriceValue], mJsonSerializer.ToJsonString(BuyPriceValue));

                        if (BillingType == Shop_Define.eBillingType.Global_aOS_MyCard
                            || BillingType == Shop_Define.eBillingType.Global_iOS_MyCard
                            || BillingType == Shop_Define.eBillingType.mfun_aOS_Mycard
                            || BillingType == Shop_Define.eBillingType.mfun_iOS_Mycard
                            || BillingType == Shop_Define.eBillingType.Global_aOS_PayPal
                            || BillingType == Shop_Define.eBillingType.Global_iOS_PayPal
                            || BillingType == Shop_Define.eBillingType.mfun_aOS_Paypal
                            || BillingType == Shop_Define.eBillingType.mfun_aOS_Mycard)
                        {
                            json = mJsonSerializer.AddJson(json, "authcode", AuthCode);
                        }
                    }
                }
            }
            else if (requestOp.Equals("shop_buy_count_reset"))
            {
                tb.IsoLevel = IsolationLevel.ReadCommitted;

                retError = Result_Define.eResult.SUCCESS;
                Shop_Define.eShopType ShopType = (Shop_Define.eShopType)System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("shoptype", "0"));

                User_Shop_Reset resetInfo = ShopManager.GetUserShopResetInfo(ref tb, AID, ShopType);
                int useCash = 0;

                switch (ShopType)
                {
                    case Shop_Define.eShopType.PvP_FreeForAll:
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.ShopResetOperationSID.PvP_FreeForAll);
                        if (!VipManager.CheckVIPCountOver(ref tb, AID, 0, VIP_Define.eVipType.SHOP_RESET_HONOR, resetInfo.reset_count))
                            retError = Result_Define.eResult.VIP_SHOP_REST_COUNT_OVER;
                        useCash = SystemData.GetConstValueInt(ref tb, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.DEF_HONOR_SHOP_RESET_COST_RUBY]);
                        break;
                    case Shop_Define.eShopType.PvP_1vs1:
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.ShopResetOperationSID.PvP_1vs1);
                        if (!VipManager.CheckVIPCountOver(ref tb, AID, 0, VIP_Define.eVipType.SHOP_RESET_PVP, resetInfo.reset_count))
                            retError = Result_Define.eResult.VIP_SHOP_REST_COUNT_OVER;
                        useCash = SystemData.GetConstValueInt(ref tb, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.DEF_1VS1REAL_RESET_COST_RUBY]);
                        break;
                    case Shop_Define.eShopType.Party:
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.ShopResetOperationSID.Party);
                        if (!VipManager.CheckVIPCountOver(ref tb, AID, 0, VIP_Define.eVipType.SHOP_RESET_CO_OP, resetInfo.reset_count))
                            retError = Result_Define.eResult.VIP_SHOP_REST_COUNT_OVER;
                        useCash = SystemData.GetConstValueInt(ref tb, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.DEF_3PVE_SHOP_RESET_COST_RUBY]);
                        break;
                    case Shop_Define.eShopType.Guild:
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.ShopResetOperationSID.Guild);
                        if (!VipManager.CheckVIPCountOver(ref tb, AID, 0, VIP_Define.eVipType.SHOP_RESET_GUILD, resetInfo.reset_count))
                            retError = Result_Define.eResult.VIP_SHOP_REST_COUNT_OVER;
                        useCash = SystemData.GetConstValueInt(ref tb, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.DEF_GUILD_SHOP_RESET_COST_RUBY]);
                        break;
                    case Shop_Define.eShopType.Ranking:
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.ShopResetOperationSID.Ranking);
                        if (!VipManager.CheckVIPCountOver(ref tb, AID, 0, VIP_Define.eVipType.SHOP_RESET_BATTLE, resetInfo.reset_count))
                            retError = Result_Define.eResult.VIP_SHOP_REST_COUNT_OVER;
                        useCash = SystemData.GetConstValueInt(ref tb, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.DEF_BATTLE_RANKING_RESET_COST_RUBY]);
                        break;
                    case Shop_Define.eShopType.Expedition:
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.ShopResetOperationSID.Expedition);
                        if (!VipManager.CheckVIPCountOver(ref tb, AID, 0, VIP_Define.eVipType.SHOP_RESET_FELLOW, resetInfo.reset_count))
                            retError = Result_Define.eResult.VIP_SHOP_REST_COUNT_OVER;
                        useCash = SystemData.GetConstValueInt(ref tb, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.DEF_EXPEDITION_SHOP_RESET_COST_RUBY]);
                        break;
                    case Shop_Define.eShopType.BlackMarket:
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.ShopResetOperationSID.BlackMarket);
                        if (!VipManager.CheckVIPCountOver(ref tb, AID, 0, VIP_Define.eVipType.SHOP_RESET_BLACKMARKET, resetInfo.reset_count))
                            retError = Result_Define.eResult.VIP_SHOP_REST_COUNT_OVER;
                        useCash = SystemData.AdminConstValueFetchFromRedis(ref tb, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.DEF_BLACKMARKET_SHOP_RESET_COST_RUBY]);
                        break;
                    default:
                        retError = Result_Define.eResult.VIP_SHOP_REST_TYPE_INVALIDE;
                        break;
                }

                retError = Result_Define.eResult.SUCCESS;

                //tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_item_id], ((int)ShopType + SnailLog_Define.Snail_s_id_Seperator_shop_reset).ToString());
                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_count], "1");

                if (retError == Result_Define.eResult.SUCCESS)
                    retError = ShopManager.AddUserShopResetCount(ref tb, AID, ShopType, ref resetInfo);

                if (retError == Result_Define.eResult.SUCCESS && useCash > 0)
                    retError = AccountManager.UseUserCash(ref tb, AID, useCash);

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    if (ShopType == Shop_Define.eShopType.BlackMarket)
                    {
                        int shopRemainTime = 0;
                        List<RetShopItem> shopItemList = ShopManager.GetUser_ShopList(ref tb, AID, ShopType, Shop_Define.eBillingType.None, ref shopRemainTime, ref retError);

                        foreach (RetShopItem setItem in shopItemList)
                        {
                            retError = ShopManager.ResetUserBuyItemCount(ref tb, AID, setItem.shop_item_id);
                            if (retError != Result_Define.eResult.SUCCESS)
                                break;
                        }
                        if (retError == Result_Define.eResult.SUCCESS)
                        {
                            retError = ShopManager.ResetUser_BlackMarket_List(ref tb, AID);
                            ShopManager.RemoveCache_User_BlackMarket_List(AID);
                        }
                    }
                    else
                    {
                        List<System_Shop_Goods> ShopGoods = ShopType != Shop_Define.eShopType.BlackMarket ? ShopManager.GetSystem_ShopList(ref tb, ShopType) : new List<System_Shop_Goods>();
                        foreach (System_Shop_Limit_List setItem in ShopGoods)
                        {
                            retError = ShopManager.ResetUserBuyItemCount(ref tb, AID, setItem.Shop_Goods_ID);
                            if (retError != Result_Define.eResult.SUCCESS)
                                break;
                        }
                    }
                }

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    List<User_Shop_Buy> userItemBuy = ShopManager.GetUser_All_BuyItemCount(ref tb, AID, ShopType, true);
                    Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetRuby], (userAccount.Cash + userAccount.EventCash).ToString());
                        json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetCurrentCount], resetInfo.reset_count.ToString());
                    }
                }
            }
            else if (requestOp.Equals("blackmarket_soul_disassemble"))
            {
                tb.IsoLevel = IsolationLevel.ReadCommitted;

                List<User_BlackMarketSoulSeq> MaterialSoulList = mJsonSerializer.JsonToObject<List<User_BlackMarketSoulSeq>>(queryFetcher.QueryParam_Fetch("material_soul", "[]"));

                retError = MaterialSoulList.Count > 0 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.SHOP_SOUL_SEQ_EMPTY_OR_INVALIDE;

                List<Ret_Soul_Active> retUpdateSoulList = new List<Ret_Soul_Active>();
                int getBlackMarketPoint = 0;

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    int SoulToPowerCount = SystemData.GetConstValueInt(ref tb, Dungeon_Define.Dungen_Const_Def_Key_List[Dungeon_Define.eDungenConstDef.DEF_BLACKMARKET_POWDOR_NUM]);
                    List<User_ActiveSoul> getActiveSoulList = SoulManager.GetUser_ActiveSoul(ref tb, AID);

                    foreach (User_BlackMarketSoulSeq targetSeq in MaterialSoulList)
                    {
                        var findSoul = getActiveSoulList.Find(soulItem => soulItem.soulseq == targetSeq.soulseq);
                        if (findSoul == null)
                        {
                            retError = Result_Define.eResult.SOUL_ID_NOT_FOUND;
                            break;
                        }
                        else if (findSoul.soulparts_ea < targetSeq.count)
                        {
                            retError = Result_Define.eResult.SOUL_PARTS_NOT_ENOUGH;
                            break;
                        }

                        findSoul.soulparts_ea -= targetSeq.count;
                        retError = SoulManager.UpdateActiveSoulInfo(ref tb, findSoul);

                        if (retError != Result_Define.eResult.SUCCESS)
                            break;

                        getBlackMarketPoint += (SoulToPowerCount * targetSeq.count);
                        retUpdateSoulList.Add(new Ret_Soul_Active(findSoul));
                    }
                }

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    //tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_item_id], (SnailLog_Define.Snail_s_id_Seperator_shop_soul_diss).ToString());

                    retError = AccountManager.AddUserBlackMarketPoint(ref tb, AID, getBlackMarketPoint);
                }

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    Account UserInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.RetBlackMarketPoint], UserInfo.BlackMarketPoint.ToString());
                        json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.ActiveSoulInfoList], mJsonSerializer.ToJsonString(retUpdateSoulList));
                    }
                }
            }
            else if (requestOp.Equals("aosbilling_success"))
            {                     
                Shop_Define.eShopItemType shopItemType = (Shop_Define.eShopItemType)System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("buy_protuct_type"));
                string Billing_Platform_UID = queryFetcher.QueryParam_Fetch("billing_uid");
                Shop_Define.eBillingType BillingType = (Shop_Define.eBillingType)queryFetcher.QueryParam_FetchInt("billing_type");
                int BuyCount = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("buy_count", "1"));

                //string Billing_ID = queryFetcher.QueryParam_Fetch("billing_id");
                //string Billing_Token = queryFetcher.QueryParam_Fetch("billing_token");

                string Billing_ID = Billing_Platform_UID;
                string Billing_Token = Billing_Platform_UID;
 
                int BuyPriceValue = 0;
                bool progressFlag = false;
                tb.IsoLevel = IsolationLevel.ReadCommitted;
                if (shopItemType == Shop_Define.eShopItemType.Cash || shopItemType == Shop_Define.eShopItemType.Item)
                {
                                
                    long BuyGoodsID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("product_GoodsID"));                           
                    Shop_Define.eShopType ShopType = (Shop_Define.eShopType)System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("shoptype", "0"));                             
                    string productID = queryFetcher.QueryParam_Fetch("product_id");


                    Item_Define.eItemBuyPriceType BuyPriceType = Item_Define.eItemBuyPriceType.None;                             
                    List<User_Inven> makeRealItem = new List<User_Inven>();

                    retError = ShopManager.BuyShopItemProgress(ref tb, AID, CID, BuyGoodsID, shopItemType
                                                                , ShopType, BuyCount, ref makeRealItem, ref BuyPriceType
                                                                , ref BuyPriceValue, (Shop_Define.eBillingType)BillingType
                                                                , Billing_Platform_UID, Billing_ID, Billing_Token, productID);
                                
                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        progressFlag = true;
                        Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                    }
                }
                else if (shopItemType == Shop_Define.eShopItemType.Package || shopItemType == Shop_Define.eShopItemType.Chep_Package)
                {

                    long PackageID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("product_GoodsID"));
                    
                    Item_Define.eItemBuyPriceType BuyPriceType = Item_Define.eItemBuyPriceType.None;
                    List<User_Inven> makeRealItem = new List<User_Inven>();
                    retError = ShopManager.BuyShopItemProgress(ref tb, AID, CID, PackageID, shopItemType
                                                                , Shop_Define.eShopType.None, BuyCount, ref makeRealItem, ref BuyPriceType
                                                                , ref BuyPriceValue, (Shop_Define.eBillingType)BillingType
                                                                , Billing_Platform_UID, Billing_ID, Billing_Token);

                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        progressFlag = true;
                        Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);                                   
                    }
                }




                if (progressFlag)
                {

                    User_Billing_List getObj = ShopManager.GetUserBillingInfoByBillingID(ref tb, Billing_ID);
                    List<System_Package_List> getPackageList = ShopManager.GetShop_System_Package_List(ref tb, Shop_Define.eBillingType.None, true, true);
                    System_Package_List pickPackage = getPackageList.Find(item => item.Package_ID == getObj.Shop_Goods_ID);
                    bool isCheap = (pickPackage == null);
                    if (isCheap)
                    {
                        getPackageList = ShopManager.GetShop_System_Package_Cheap_List(ref tb, Shop_Define.eBillingType.None, true);
                        pickPackage = getPackageList.Find(item => item.Package_ID == getObj.Shop_Goods_ID);
                        isCheap = (pickPackage != null);
                    }

                    List<User_Inven> makeRealItem = new List<User_Inven>();
                    List<System_Package_RewardBox> getRewardList = new List<System_Package_RewardBox>();

                    queryFetcher.RequestAID = AID = getObj.AID;
                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], AID);

                    int VipExp = 0;                               
                    int MaxBuy = 0;
                    int BuyGroupID = 0;
                    long ShopGoodsID = getObj.Shop_Goods_ID;

                    float MultipleValue = 1.0f;
                    float zhekouValue = 1.0f;

                    retError = Result_Define.eResult.SUCCESS;

                    if (getObj.BillingIndex > 0 && getObj.Billing_Status == (int)Shop_Define.eBillingStatus.Complete)
                    {
                        retError = Result_Define.eResult.SHOP_BILLING_ALREADY_COMPLETE;
                    }
                    else if (getObj.BillingIndex > 0 && getObj.Shop_Goods_ID > 0 && pickPackage == null && retError == Result_Define.eResult.SUCCESS)
                    {
                        Shop_Define.eShopType ShopType = Shop_Define.eShopType.Billing;
                        List<System_Shop_Goods> ShopGoods = ShopManager.GetSystem_ShopList(ref tb, ShopType, (Shop_Define.eBillingType)getObj.Buy_Platform);
                        System_Shop_Goods PickItem = ShopGoods.Find(item => item.Shop_Goods_ID == getObj.Shop_Goods_ID);

                        /// 判断是否首充双倍
                        {
                            User_Shop_Buy alreadyBuys = ShopManager.GetUserBuyItemCount(ref tb, AID, ShopGoodsID);
                            if (alreadyBuys.Shop_Goods_ID== ShopGoodsID)
                            {                     /// 需要双倍
                                if (PickItem.DoubleSwitch == 1 && alreadyBuys.Buy_Count <= 0)
                                {
                                    MultipleValue = PickItem.Discount / 100.0f;
                                }
                            }

                            /// 折扣
                            if (PickItem.DoubleSwitch == 2)
                            {
                                zhekouValue =  PickItem.Discount / 100.0f;
                            }
                        }

                        if (PickItem != null)
                        {
                            List<User_Inven> makeItems = new List<User_Inven>();
                            int makeCount = PickItem.ItemNum < 0 ? Shop_Define.Shop_UnlimitDay : PickItem.ItemNum;  // for subscription item

                            if (PickItem.ItemID > 0)
                                getRewardList.Add(new System_Package_RewardBox(PickItem.ItemID, (int)(makeCount * MultipleValue)));
                            if (PickItem.Bonus_ItemID > 0 && retError == Result_Define.eResult.SUCCESS)
                            {
                                makeCount = PickItem.ItemNum < 0 ? Shop_Define.Shop_UnlimitDay : PickItem.Bonus_ItemNum;  // for subscription item
                                getRewardList.Add(new System_Package_RewardBox(PickItem.Bonus_ItemID, makeCount));
                            }

                            VipExp = PickItem.VIP_Point;
                            BuyPriceValue = System.Convert.ToInt32(BuyPriceValue * zhekouValue);
                            MaxBuy = PickItem.Max_Buy;
                            BuyGroupID = PickItem.Buy_GroupID;
                        }
                        else
                            retError = Result_Define.eResult.SHOP_ID_NOT_FOUND;

                        if (retError == Result_Define.eResult.SUCCESS)
                            retError = ShopManager.UpdateUserBillingStatus(ref tb, getObj.BillingIndex, Shop_Define.eBillingStatus.Complete);
                    }
                    else if (getObj.BillingIndex > 0 && getObj.Shop_Goods_ID > 0 && pickPackage != null)   // for package list
                    {
                        AID = getObj.AID;
                        CID = getObj.CID;

                        Character charInfo = CharacterManager.GetCharacter(ref tb, AID, CID);

                        if (pickPackage.Reward_Box1ID > 0)
                            getRewardList.AddRange(
                                isCheap ? ShopManager.GetShop_System_Package_Cheap_RewardBox(ref tb, pickPackage.Reward_Box1ID) :
                                ShopManager.GetShop_System_Package_RewardBox(ref tb, pickPackage.Reward_Box1ID)
                                );
                        if (pickPackage.Reward_Box2ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Warrior)
                            getRewardList.AddRange(
                                isCheap ? ShopManager.GetShop_System_Package_Cheap_RewardBox(ref tb, pickPackage.Reward_Box2ID) :
                                ShopManager.GetShop_System_Package_RewardBox(ref tb, pickPackage.Reward_Box2ID)
                                );
                        if (pickPackage.Reward_Box3ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Swordmaster)
                            getRewardList.AddRange(
                                isCheap ? ShopManager.GetShop_System_Package_Cheap_RewardBox(ref tb, pickPackage.Reward_Box3ID) :
                                ShopManager.GetShop_System_Package_RewardBox(ref tb, pickPackage.Reward_Box3ID)
                                );
                        if (pickPackage.Reward_Box4ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Taoist)
                            getRewardList.AddRange(
                                isCheap ? ShopManager.GetShop_System_Package_Cheap_RewardBox(ref tb, pickPackage.Reward_Box4ID) :
                                ShopManager.GetShop_System_Package_RewardBox(ref tb, pickPackage.Reward_Box4ID)
                                );

                        if (retError == Result_Define.eResult.SUCCESS)
                            retError = ShopManager.UpdateUserBillingStatus(ref tb, getObj.BillingIndex, Shop_Define.eBillingStatus.Complete);

                        VipExp = pickPackage.VIP_Point;
                        BuyPriceValue = pickPackage.Buy_PriceValue;
                        MaxBuy = pickPackage.Max_Buy;
                    }
                    else if (getObj.BillingIndex > 0 && getObj.Shop_Goods_ID > 0)
                    {
                        AID = getObj.AID;
                        getObj = ShopManager.GetUserBillingInfo(ref tb, Billing_ID, Billing_Token, Shop_Define.eBillingStatus.Complete);

                        if (getObj.Billing_Status == (int)Shop_Define.eBillingStatus.Complete && getObj.BillingIndex > 0)
                            retError = Result_Define.eResult.SHOP_BILLING_ALREADY_COMPLETE;
                        else
                        {
                            AID = 0;
                            retError = Result_Define.eResult.SHOP_BILLING_ID_NOT_FOUND;
                        }
                    }
                    else if (getObj.BillingIndex == 0)
                    {
                        retError = Result_Define.eResult.SHOP_BILLING_ID_NOT_FOUND;
                    }
                    else
                    {
                        retError = Result_Define.eResult.SHOP_ID_NOT_FOUND;
                    }


                    if (retError == Result_Define.eResult.SUCCESS)
                        retError = ShopManager.BuyShopMakeProgress(ref tb, AID, CID, ShopGoodsID, ref BuyPriceValue, ref makeRealItem, ref getRewardList, Item_Define.eItemBuyPriceType.PriceType_PayReal, 1, MaxBuy, VipExp, BuyGroupID);
                    /// 打折产品
                    BuyPriceValue = System.Convert.ToInt32(BuyPriceValue * zhekouValue);

                    if (retError == Result_Define.eResult.SUCCESS)
                        retError = ShopManager.CheckFirstPayEvent(ref tb, AID, CID);

//                                 if (serviceArea == DataManager_Define.eCountryCode.China)
//                                     BuyPriceValue = BuyPriceValue % Shop_Define.Shop_OverPricePrefix;

                    if (retError == Result_Define.eResult.SUCCESS)
                        retError = ShopManager.PayBuyPrice(ref tb, AID, BuyPriceValue, Item_Define.eItemBuyPriceType.PriceType_PayReal);

                    // json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.AID], mJsonSerializer.ToJsonString(AID));

                    if (retError == Result_Define.eResult.SUCCESS)
                    {
                        Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);                                
                    }
                }             
                     
            }
            else if (requestOp.Equals("get_vip_reward"))
            {
                int vip_level = queryFetcher.QueryParam_FetchInt("vip_level");
                User_VIP userVIPInfo = VipManager.GetUser_VIPInfo(ref tb, AID);
                retError = userVIPInfo.viplevel < vip_level ? Result_Define.eResult.VIP_REWARD_LEVEL_INVALID : Result_Define.eResult.SUCCESS;

                List<User_Inven> makeRealItem = new List<User_Inven>();
                List<int> getRewardList = new List<int>();
                User_Event_Check_Data userEventData = TriggerManager.GetUser_Event_Check_Data(ref tb, AID);

                // vip reward send
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    if (string.IsNullOrEmpty(userEventData.VIPRewardList))
                        userEventData.VIPRewardList = "[]";
                    getRewardList = mJsonSerializer.JsonToObject<List<int>>(userEventData.VIPRewardList);

                    bool findReward = false;
                    foreach (int getRewardLevel in getRewardList)
                    {
                        if (getRewardLevel == vip_level)
                        {
                            findReward = true;
                            break;
                        }
                    }
                    if (findReward)
                        retError = Result_Define.eResult.VIP_REWARD_LEVEL_INVALID;
                    else
                    {
                        List<System_VIP_RewardBox> systemRewardList = VipManager.GetSystem_VIP_RewardList(ref tb, vip_level);
                        Dictionary<short, List<Set_Mail_Item>> sendMailList = new Dictionary<short, List<Set_Mail_Item>>();

                        systemRewardList.ForEach(setItem =>
                            {
                                if (!sendMailList.ContainsKey(setItem.ClassType))
                                    sendMailList[setItem.ClassType] = new List<Set_Mail_Item>();

                                sendMailList[setItem.ClassType].Add(new Set_Mail_Item(setItem.Item_ID, setItem.Num, setItem.Grade, setItem.Level));
                            }
                        );

                        List<Character> userCharList = CharacterManager.GetCharacterList(ref tb, AID);

                        foreach (KeyValuePair<short, List<Set_Mail_Item>> mailList in sendMailList)
                        {
                            var findChar = mailList.Key > 0 ? userCharList.Find(charInfo => charInfo.Class == mailList.Key) : null;
                            int sendItemCount = 0;
                            List<Set_Mail_Item> setMailItem = new List<Set_Mail_Item>();

                            if (mailList.Key == 0 || findChar != null)
                            {
                                string mailTitle = VipManager.GetVIPRewardMailTitle(vip_level, mailList.Key);
                                string mailBody = VipManager.GetVIPRewardMailBody(vip_level, mailList.Key);

                                foreach (Set_Mail_Item setItem in mailList.Value)
                                {
                                    if (setItem.itemid > 0 && setItem.itemea > 0)
                                    {
                                        sendItemCount++;
                                        setMailItem.Add(setItem);
                                    }
                                    if (sendItemCount >= Mail_Define.Mail_MaxItem)
                                    {
                                        retError = MailManager.SendMail(ref tb, ref setMailItem, AID, 0, "", mailTitle, mailBody, Mail_Define.Mail_VIP_CloseTime_Min);

                                        if (retError == Result_Define.eResult.SUCCESS)
                                        {
                                            setMailItem.Clear();
                                            sendItemCount = 0;
                                        }
                                        else
                                            break;
                                    }
                                }

                                if (retError == Result_Define.eResult.SUCCESS && sendItemCount > 0)
                                {
                                    retError = MailManager.SendMail(ref tb, ref setMailItem, AID, 0, "", mailTitle, mailBody, Mail_Define.Mail_VIP_CloseTime_Min);
                                }
                            }
                        }

                        //foreach (System_VIP_RewardBox setItem in systemRewardList)
                        //{
                        //List<User_Inven> makeItem = new List<User_Inven>();
                        //retError = ItemManager.MakeItem(ref tb, ref makeItem, AID, setItem.Item_ID, setItem.Num, CID, setItem.Level, setItem.Grade);
                        //if (retError == Result_Define.eResult.SUCCESS)
                        //    makeRealItem.AddRange(makeItem);
                        //else
                        //    break;
                        //}

                        //if (makeRealItem.Count < 1 && retError == Result_Define.eResult.SUCCESS)
                        //    retError = Result_Define.eResult.ITEM_CREATE_FAIL;
                    }
                }

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    getRewardList.Add(vip_level);
                    userEventData.VIPRewardList = mJsonSerializer.ToJsonString(getRewardList);
                    retError = TriggerManager.UpdateEvent_Check_Data(ref tb, AID, userEventData.CheckCount, userEventData.RewardCount, userEventData.AddCount, userEventData.VIPRewardList, userEventData.FirstPaymentFlag);
                }

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                    Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount);
                    userEventData = TriggerManager.GetUser_Event_Check_Data(ref tb, AID, true);

                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.VIPRewardList], userEventData.VIPRewardList);
                }
            }
                       
        }
    }
}