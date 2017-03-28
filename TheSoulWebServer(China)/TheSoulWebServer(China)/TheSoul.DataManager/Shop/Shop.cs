using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;
using System.Globalization;
using System.Security.Cryptography;

namespace TheSoul.DataManager
{
    public class ShopManager
    {
        public static List<RetShopItem> GetUser_ShopList(ref TxnBlock TB, long AID, Shop_Define.eShopType ShopType, Shop_Define.eBillingType BillingType, ref int remainTime, ref Result_Define.eResult retError, bool takeAll = false, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            List<RetShopItem> retList = new List<RetShopItem>();
            List<User_Shop_Buy> userItemBuy = ShopManager.GetUser_All_BuyItemCount(ref TB, AID, ShopType);

            if (ShopType == Shop_Define.eShopType.BlackMarket)
            {
                //// Not use yet (why??) - alway open?
                //int AdminOpenTime = SystemData.AdminConstValueFetchFromRedis(ref TB, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.ADMIN_CONST_DEF_BLACKMARKET_OPEN_START_TIME]);
                //int AdminEndTime = SystemData.AdminConstValueFetchFromRedis(ref TB, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.ADMIN_CONST_DEF_BLACKMARKET_OPEN_END_TIME]);

                //User_Shop_BlackMarket userShopInfo = ShopManager.GetUser_BlackMarket_List(ref TB, AID);
                //DateTime startTime = DateTime.Parse(DateTime.Now.ToShortDateString()).AddSeconds(AdminOpenTime);
                //DateTime endTime = DateTime.Parse((AdminOpenTime > AdminEndTime ? DateTime.Now.AddDays(1) : DateTime.Now).ToShortDateString()).AddSeconds(AdminEndTime);
                //TimeSpan remainTS = endTime - DateTime.Now;
                //remainTime = (int)remainTS.TotalSeconds;
                //List<System_Shop_BlackMarket> userShopList = new List<System_Shop_BlackMarket>();

                //if (!(userShopInfo.regdate > startTime && userShopInfo.regdate < endTime) || string.IsNullOrEmpty(userShopInfo.ShopItemList))

                User_Shop_BlackMarket userShopInfo = ShopManager.GetUser_BlackMarket_List(ref TB, AID);
                DateTime regDate = DateTime.Parse(userShopInfo.regdate.ToShortDateString());
                DateTime nextDay = DateTime.Parse(DateTime.Now.ToShortDateString()).AddDays(1);
                TimeSpan remainTS = nextDay - DateTime.Now;
                remainTime = (int)remainTS.TotalSeconds;
                List<System_Shop_BlackMarket> userShopList = new List<System_Shop_BlackMarket>();

                if ((userShopInfo.regdate.ToShortDateString() != DateTime.Now.ToShortDateString()) || string.IsNullOrEmpty(userShopInfo.ShopItemList))
                {
                    List<System_Shop_BlackMarket> ShopGoods = ShopManager.GetSystem_BlackMarket_List(ref TB);

                    Dictionary<int, List<System_Shop_BlackMarket>> ShopItemList = new Dictionary<int, List<System_Shop_BlackMarket>>();

                    ShopGoods.ForEach(shopiteminfo =>
                    {
                        if (shopiteminfo.delflag > 0)
                        {
                            if (!ShopItemList.ContainsKey(shopiteminfo.SlotID))
                                ShopItemList[shopiteminfo.SlotID] = new List<System_Shop_BlackMarket>();
                            ShopItemList[shopiteminfo.SlotID].Add(shopiteminfo);
                        }
                    }
                    );

                    foreach (KeyValuePair<int, List<System_Shop_BlackMarket>> setItems in ShopItemList)
                    {
                        int Max = setItems.Value.Sum(shopitem => shopitem.ItemProb);
                        int curRate = TheSoul.DataManager.Math.GetRandomInt(0, Max);
                        int checkRate = 0;
                        foreach (System_Shop_BlackMarket setShopItems in setItems.Value)
                        {
                            if (setShopItems.ItemProb > 0)
                            {
                                checkRate += setShopItems.ItemProb;
                                if (checkRate >= curRate)
                                {
                                    userShopList.Add(setShopItems);
                                    break;
                                }
                            }
                        }
                    }

                    retError = SetUser_BlackMarket_List(ref TB, AID, userShopList);
                    if (retError == Result_Define.eResult.SUCCESS)
                        userShopInfo = ShopManager.GetUser_BlackMarket_List(ref TB, AID, true);
                }
                else
                    userShopList = mJsonSerializer.JsonToObject<List<System_Shop_BlackMarket>>(userShopInfo.ShopItemList);

                foreach (System_Shop_BlackMarket setItem in userShopList)
                {
                    int BuyCount = 0;
                    int BuyMaxCount = 0;
                    User_Shop_Buy buyinfo = buyinfo = userItemBuy.Find(item => item.Shop_Goods_ID == setItem.Shop_Goods_ID);
                    if (buyinfo != null)
                    {
                        BuyCount = buyinfo.Buy_Count;
                        BuyMaxCount = buyinfo.TotalBuy_Count;
                    }
                    retList.Add(new RetShopItem(setItem, BuyCount));
                }
            }
            else
            {
                List<System_Shop_Goods> ShopGoods = ShopManager.GetSystem_ShopList(ref TB, ShopType, BillingType);
                //List<Object> setList = new List<object>();
                //List<System_Shop_Limit_List> setList = new List<System_Shop_Limit_List>(); // ((List<System_Shop_Limit_List>)ShopGoods).ToList<System_Shop_Limit_List>();

                //if (ShopType == Shop_Define.eShopType.Cash || ShopType == Shop_Define.eShopType.Billing)
                //    setList = ((List<System_Shop_Goods>)ShopGoods).ToList<System_Shop_Limit_List>();
                //else if (ShopType == Shop_Define.eShopType.Guild)
                //    setList = ((List<System_Shop_Guild>)ShopGoods).ToList<System_Shop_Limit_List>();
                //else
                //    setList = ((List<System_Shop_Point>)ShopGoods).ToList<System_Shop_Limit_List>(); ;

                List<System_Shop_Goods> checkBuyOnceItem = ShopGoods.FindAll(shopItem => shopItem.Type == (int)Shop_Define.eShopSaleType.BuyOnceSale);
                foreach (System_Shop_Goods setItem in ShopGoods)
                {
                    int BuyCount = 0;
                    int BuyMaxCount = 0;
                    User_Shop_Buy buyinfo = buyinfo = userItemBuy.Find(item => item.Shop_Goods_ID == setItem.Shop_Goods_ID);
                    if (buyinfo != null)
                    {
                        BuyCount = buyinfo.Buy_Count;
                        BuyMaxCount = buyinfo.TotalBuy_Count;
                    }

                    if (setItem.Type == (int)Shop_Define.eShopSaleType.BuyOnceSale && BuyCount < 1)
                    {
                        RetShopItem ret = new RetShopItem(setItem, BuyCount);
                        ret.DoubleSwitch = setItem.DoubleSwitch;
                        ret.Discount = setItem.Discount;
                        retList.Add(ret);
                    }
                    else if (setItem.Type != (int)Shop_Define.eShopSaleType.BuyOnceSale)
                    {
                        var checkShopItem = checkBuyOnceItem.Find(shopItem => shopItem.ItemID == setItem.ItemID);
                        if (checkShopItem == null)
                        {
                            RetShopItem ret = new RetShopItem(setItem, BuyCount);
                            ret.DoubleSwitch = setItem.DoubleSwitch;
                            ret.Discount = setItem.Discount;
                            retList.Add(ret);
                        }
                        else
                        {
                            var checkBuyItem = userItemBuy.Find(buyItemInfo => buyItemInfo.Shop_Goods_ID == checkShopItem.Shop_Goods_ID && buyItemInfo.Buy_Count > 0);
                            if (checkBuyItem != null)
                            {
                                RetShopItem ret = new RetShopItem(setItem, BuyCount);
                                ret.DoubleSwitch = setItem.DoubleSwitch;
                                ret.Discount = setItem.Discount;
                                retList.Add(ret);
                            }
                        }
                    }
                }
            }

            return retList;
        }

        // buy progress check
        public static Result_Define.eResult BuyShopItemProgress(ref TxnBlock tb, long AID, long CID, long ShopGoodsID, Shop_Define.eShopItemType ShopItemType, Shop_Define.eShopType ShopType, int BuyCount
                                                            , ref List<User_Inven> makeRealItem, ref Item_Define.eItemBuyPriceType BuyPriceType, ref int BuyPriceValue
                                                            , Shop_Define.eBillingType BillingType = Shop_Define.eBillingType.UnityDebug, string Billing_Platform_UID = "", string Billing_ID = "", string Billing_Token = ""
                                                            , string ProductID = "", bool isAdminMake = false, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            if (AID < 1 || CID < 1)
                return AID < 1 ? Result_Define.eResult.ACCOUNT_ID_NOT_FOUND : Result_Define.eResult.CHARACTER_NOT_FOUND;

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            if (BuyCount < 1)
                return Result_Define.eResult.SHOP_BUY_NO_ITEM;
            else if (ShopType == Shop_Define.eShopType.Cash || ShopType == Shop_Define.eShopType.Billing)
                retError = Result_Define.eResult.SUCCESS;
            else if (VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.BAGSLOT_MAX_ITEM))
                retError = Result_Define.eResult.SUCCESS;
            else
                return Result_Define.eResult.ITEM_INVENTORY_OVER;

            List<System_Package_RewardBox> makeItemList = new List<System_Package_RewardBox>();
            bool isSubscription = false;
            bool isBilling = false;
            bool bMakeNow = false;
            int BuyDiscountRate = 0;
            int VIPPoint = 0;
            int BuyGroupID = 0;
            int Max_BuyCount = 0;

            /// 双倍
            float MultipleValue = 1.0f;

            /// 折扣
            float zhekouValue = 1.0f;

            /// 充值或者道具
            if (ShopItemType == Shop_Define.eShopItemType.Cash || ShopItemType == Shop_Define.eShopItemType.Item)
            {
                List<System_Shop_BlackMarket> BlackMarketShopGoods = ShopType == Shop_Define.eShopType.BlackMarket ? ShopManager.GetSystem_BlackMarket_List(ref tb) : new List<System_Shop_BlackMarket>();
                List<System_Shop_Goods> ShopGoods = ShopType != Shop_Define.eShopType.BlackMarket ? ShopManager.GetSystem_ShopList(ref tb, ShopType, BillingType) : new List<System_Shop_Goods>();
                int shopRemainTime = 0;

                System_Shop_Goods PickItemProduct = ShopGoods.Find(item => item.Product_ID == ProductID);
                if (PickItemProduct != null)
                {
                    ShopGoodsID = PickItemProduct.Shop_Goods_ID;
                }

                System_Shop_Goods PickItem = ShopGoods.Find(item => item.Shop_Goods_ID == ShopGoodsID);      
                System_Shop_BlackMarket BlackPickItem = BlackMarketShopGoods.Find(item => item.Shop_Goods_ID == ShopGoodsID);


                //充值的时候添加了依靠product来区分
                if (PickItem != null)
                //if (PickItem != null)
                {
                    /// 渠道充值，首冲多倍
                    if (ShopType == Shop_Define.eShopType.Cash || ShopType == Shop_Define.eShopType.Billing)
                    {
                        List<RetShopItem> userShopList = ShopManager.GetUser_ShopList(ref tb, AID, ShopType, BillingType, ref shopRemainTime, ref retError);
                        User_Shop_Buy alreadyBuys = ShopManager.GetUserBuyItemCount(ref tb, AID, ShopGoodsID);
                        var checkUserBuy = userShopList.Find(item => item.shop_item_id == ShopGoodsID);
                        if (checkUserBuy != null)
                        {
                            BuyPriceValue = PickItem.Buy_PriceValue;

                            /// 折扣
                            if (PickItem.DoubleSwitch == 2)
                            {
                                zhekouValue = PickItem.Discount / 100.0f;
                            }
                        }
                        else
                            retError = Result_Define.eResult.SHOP_ID_NOT_FOUND;

                        if (alreadyBuys.Shop_Goods_ID== ShopGoodsID)
                        {                     /// 需要双倍
                            if (PickItem.DoubleSwitch == 1 && alreadyBuys.Buy_Count <= 0)
                            {
                                MultipleValue = PickItem.Discount / 100.0f;
                            }
                        }
                    }
                    else if (ShopType == Shop_Define.eShopType.Guild)
                    {
                        if (PickItem.UseGuildLv > GuildManager.GetGuildLV(ref tb, AID))
                            retError = Result_Define.eResult.GUILD_NOT_ENOUGH_GUILDLV;
                        BuyPriceValue = PickItem.Buy_PriceValue;
                    }
                    else
                    {
                        BuyPriceValue = PickItem.Buy_PriceValue;
                    }

                    BuyPriceType = Item_Define.Item_BuyType_List[PickItem.Buy_PriceType];
                    isSubscription = PickItem.Type == (int)Shop_Define.eShopSaleType.Subscription;
                    isBilling = BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_PayReal;
                    BuyDiscountRate = PickItem.Sale_Rate;
                    BuyGroupID = PickItem.Buy_GroupID;
                    Max_BuyCount = PickItem.Max_Buy;
                    VIPPoint = PickItem.VIP_Point;

                    if (PickItem.ItemID > 0)
                        makeItemList.Add(new System_Package_RewardBox(PickItem.ItemID, (int)(PickItem.ItemNum * MultipleValue)));
                    if (PickItem.Bonus_ItemID > 0)
                        makeItemList.Add(new System_Package_RewardBox(PickItem.Bonus_ItemID, PickItem.Bonus_ItemNum ));  /// 额外的不会双倍
                }
                else if (BlackPickItem != null)
                {
                    BuyPriceValue = BlackPickItem.Buy_PriceValue;
                    BuyPriceType = Item_Define.Item_BuyType_List[BlackPickItem.Buy_PriceType];
                    isSubscription = BlackPickItem.Type == (int)Shop_Define.eShopSaleType.Subscription;
                    isBilling = BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_PayReal;
                    Max_BuyCount = BlackPickItem.Max_Buy;

                    if (BlackPickItem.ItemID > 0)
                        makeItemList.Add(new System_Package_RewardBox(BlackPickItem.ItemID, BlackPickItem.ItemNum));
                }
                else
                    retError = Result_Define.eResult.SHOP_ID_NOT_FOUND;

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_item_id], (ShopGoodsID + SnailLog_Define.Snail_s_id_Seperator_shop_item).ToString());
                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_count], BuyCount);
                }

            }
            else if (ShopItemType == Shop_Define.eShopItemType.Package || ShopItemType == Shop_Define.eShopItemType.Chep_Package)
            {
                bool isCheap = ShopItemType == Shop_Define.eShopItemType.Chep_Package;

                List<System_Package_List> shopPackageList = isCheap ? ShopManager.GetShop_System_Package_Cheap_List(ref tb, BillingType) : ShopManager.GetShop_System_Package_List(ref tb, BillingType);
                System_Package_List pickPackage = shopPackageList.Find(item => item.Package_ID == ShopGoodsID);

                if (pickPackage != null)
                {
                    User_Shop_Buy buyCount = ShopManager.GetUserBuyItemCount(ref tb, AID, ShopGoodsID);
                    BuyPriceType = Item_Define.Item_BuyType_List[pickPackage.Buy_PriceType];
                    if (!CheckPackageLoop((Trigger_Define.eEventLoopType)pickPackage.LoopType, buyCount.regdate))
                        buyCount.Buy_Count = buyCount.TotalBuy_Count;
                    if (buyCount.Buy_Count >= pickPackage.Max_Buy)
                        retError = Result_Define.eResult.SHOP_BUY_MAX_COUNT_OVER;

                    Character charInfo = CharacterManager.GetCharacter(ref tb, AID, CID);
                    if (pickPackage.Reward_Box1ID > 0)
                        makeItemList.AddRange(
                            isCheap ? ShopManager.GetShop_System_Package_Cheap_RewardBox(ref tb, pickPackage.Reward_Box1ID) :
                            ShopManager.GetShop_System_Package_RewardBox(ref tb, pickPackage.Reward_Box1ID)
                            );
                    if (pickPackage.Reward_Box2ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Warrior)
                        makeItemList.AddRange(
                            isCheap ? ShopManager.GetShop_System_Package_Cheap_RewardBox(ref tb, pickPackage.Reward_Box2ID) :
                            ShopManager.GetShop_System_Package_RewardBox(ref tb, pickPackage.Reward_Box2ID)
                            );
                    if (pickPackage.Reward_Box3ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Swordmaster)
                        makeItemList.AddRange(
                            isCheap ? ShopManager.GetShop_System_Package_Cheap_RewardBox(ref tb, pickPackage.Reward_Box3ID) :
                            ShopManager.GetShop_System_Package_RewardBox(ref tb, pickPackage.Reward_Box3ID)
                            );
                    if (pickPackage.Reward_Box4ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Taoist)
                        makeItemList.AddRange(
                            isCheap ? ShopManager.GetShop_System_Package_Cheap_RewardBox(ref tb, pickPackage.Reward_Box4ID) :
                            ShopManager.GetShop_System_Package_RewardBox(ref tb, pickPackage.Reward_Box4ID)
                            );

                    BuyPriceType = Item_Define.Item_BuyType_List[pickPackage.Buy_PriceType];
                    isBilling = BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_PayReal;
                    BuyPriceValue = System.Convert.ToInt32(BuyPriceValue * zhekouValue);
                    Max_BuyCount = pickPackage.Max_Buy;
                    VIPPoint = pickPackage.VIP_Point;
                }
                else
                    retError = Result_Define.eResult.SHOP_ID_NOT_FOUND;

                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_count], "1");
                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_item_id], (ShopGoodsID + SnailLog_Define.Snail_s_id_Seperator_shop_package).ToString());
            }
            else
                retError = Result_Define.eResult.SHOP_ID_NOT_FOUND;

            // insert billing infomation
            if (isBilling && retError == Result_Define.eResult.SUCCESS)
            {
                User_Billing_List getObj = ShopManager.GetUserBillingInfoByBillingID(ref tb, Billing_ID);
                if (getObj.BillingIndex > 0)
                    return Result_Define.eResult.SHOP_BILLING_ID_ALREADY_REGISTERED;

                if (Billing_Token != "")   // = "" 应该报错
                {
                    getObj = ShopManager.GetUserBillingInfoByBillingToken(ref tb, AID, Billing_Token);
                    if (getObj.BillingIndex > 0)
                        return Result_Define.eResult.SHOP_BILLING_ID_ALREADY_REGISTERED;
                }

                if (SystemData.GetServiceArea(ref tb) == DataManager_Define.eCountryCode.China)
                    BuyPriceValue = BuyPriceValue % Shop_Define.Shop_OverPricePrefix;

                // for test by unity or pc client
                if (BillingType == Shop_Define.eBillingType.UnityDebug || isAdminMake)
                {
                    Billing_Platform_UID = GlobalManager.GetUserAccountConfig(ref tb, AID).platform_user_id;
                    Billing_ID = string.Format("{0}_{1}_{2}_{3}", isAdminMake ? "GMTool_Billing" : "Test_Billing", AID, DateTime.Now.ToShortDateString(), (int)GenericFetch.ConvertToMSeedTime());
                    Billing_Token = string.Format("{0}_{1}", Billing_Platform_UID, Billing_ID);
                    retError = ShopManager.InsertUserBillingInfo(ref tb, ShopGoodsID, AID, CID, BuyPriceValue, Billing_ID, Billing_Token, Billing_Platform_UID, (int)BillingType, Shop_Define.eBillingStatus.Complete);
                    bMakeNow = true;
                    if (retError == Result_Define.eResult.SUCCESS)
                        retError = ShopManager.CheckFirstPayEvent(ref tb, AID, CID);
                }
                else if (BillingType == Shop_Define.eBillingType.iOS_Appstore)
                {
                    if (AID > 0 && !(string.IsNullOrEmpty(Billing_Platform_UID) || string.IsNullOrEmpty(Billing_ID)) && ShopGoodsID > 0)
                        retError = ShopManager.InsertUserBillingInfo(ref tb, ShopGoodsID, AID, CID, BuyPriceValue, Billing_ID, Billing_Token, Billing_Platform_UID, (int)BillingType, Shop_Define.eBillingStatus.CreateOrderID);
                    else
                        retError = ShopGoodsID > 0 ? Result_Define.eResult.SHOP_BILLING_ID_NOT_FOUND : Result_Define.eResult.SHOP_ID_NOT_FOUND;
                    bMakeNow = false;
                }
                else if (BillingType == Shop_Define.eBillingType.Android_3rdParty || BillingType == Shop_Define.eBillingType.iOS_JailBreak)
                {
                    if (string.IsNullOrEmpty(Billing_Token))
                        Billing_Token = string.Empty;
                    retError = ShopManager.InsertUserBillingInfo(ref tb, ShopGoodsID, AID, CID, BuyPriceValue, Billing_ID, Billing_Token, Billing_Platform_UID, (int)BillingType, Shop_Define.eBillingStatus.CreateOrderID);
                    bMakeNow = false;
                }
                else if (BillingType == Shop_Define.eBillingType.Kr_aOS_Google
                        || BillingType == Shop_Define.eBillingType.Kr_iOS_Appstore
                        || BillingType == Shop_Define.eBillingType.Kr_aOS_OneStore
                        || BillingType == Shop_Define.eBillingType.Global_aOS_Google                       
                        || BillingType == Shop_Define.eBillingType.Global_aOS_PayPal         
                        || BillingType == Shop_Define.eBillingType.Global_aOS_MOL
                        || BillingType == Shop_Define.eBillingType.Global_aOS_MyCard
                        || BillingType == Shop_Define.eBillingType.Global_aOS_MOLPin                            
                        || BillingType == Shop_Define.eBillingType.Global_iOS_Appstore
                        || BillingType == Shop_Define.eBillingType.Global_iOS_PayPal
                        || BillingType == Shop_Define.eBillingType.Global_iOS_MOL
                        || BillingType == Shop_Define.eBillingType.Global_iOS_MyCard
                        || BillingType == Shop_Define.eBillingType.Global_iOS_MOLPin
                        || BillingType == Shop_Define.eBillingType.mfun_aOS_Google
                        || BillingType == Shop_Define.eBillingType.mfun_aOS_Paypal
                        || BillingType == Shop_Define.eBillingType.mfun_aOS_MOL
                        || BillingType == Shop_Define.eBillingType.mfun_aOS_Mycard
                        || BillingType == Shop_Define.eBillingType.mfun_aOS_MOLPin           
                        || BillingType == Shop_Define.eBillingType.mfun_iOS_Appstore
                        || BillingType ==  Shop_Define.eBillingType.mfun_iOS_Paypal
                        || BillingType == Shop_Define.eBillingType.mfun_iOS_MOL
                        || BillingType == Shop_Define.eBillingType.mfun_iOS_Mycard
                        || BillingType == Shop_Define.eBillingType.mfun_iOS_MOLPin
                        || BillingType == Shop_Define.eBillingType.yuenan_aOS_Google
                        || BillingType == Shop_Define.eBillingType.yuenan_aOS_Mobile
                        || BillingType == Shop_Define.eBillingType.yuenan_iOS_Appstore
                        || BillingType == Shop_Define.eBillingType.yuenan_iOS_Mobile                     
                    )
                {
                    if (AID > 0 && !(string.IsNullOrEmpty(Billing_ID) && string.IsNullOrEmpty(Billing_Token)) && ShopGoodsID > 0)
                        retError = ShopManager.InsertUserBillingInfo(ref tb, ShopGoodsID, AID, CID, BuyPriceValue, Billing_ID, Billing_Token, Billing_Platform_UID, (int)BillingType, Shop_Define.eBillingStatus.CreateOrderID);
                    bMakeNow = false;
                }
            }
            else
                bMakeNow = true;

            if (bMakeNow && retError == Result_Define.eResult.SUCCESS)
            {
                retError = BuyShopMakeProgress(ref tb, AID, CID, ShopGoodsID, ref BuyPriceValue, ref makeRealItem, ref makeItemList, BuyPriceType, BuyCount, Max_BuyCount, VIPPoint, BuyGroupID);
                /// 打折产品
                BuyPriceValue = System.Convert.ToInt32(BuyPriceValue * zhekouValue);
            }

            if ((BuyPriceType != Item_Define.eItemBuyPriceType.PriceType_PayReal || bMakeNow) && retError == Result_Define.eResult.SUCCESS)
            {
                BuyPriceValue = System.Convert.ToInt32(BuyPriceValue * ((100.0f + BuyDiscountRate) / 100.0f));
                retError = ShopManager.PayBuyPrice(ref tb, AID, BuyPriceValue, BuyPriceType);
            }

            return retError;
        }

        // buy progress
        public static Result_Define.eResult BuyShopMakeProgress(ref TxnBlock tb, long AID, long CID, long ShopGoodsID, ref int BuyPriceValue,
                                                                ref List<User_Inven> makeRealItem, ref List<System_Package_RewardBox> rewardItem, Item_Define.eItemBuyPriceType BuyPriceType,
                                                                int BuyCount = 1, int Max_BuyCount = 1, int VipPoint = 0, int BuyGroupID = 0, string dbkey = Shop_Define.Shop_Info_DB)
        {
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            if (VipPoint > 0)
                retError = VipManager.VIPPointAdd(ref tb, AID, VipPoint);

            User_Shop_Buy userCurrentBuy = ShopManager.GetUserBuyItemCount(ref tb, AID, ShopGoodsID, true);
            int makeCount = 0;

            if (BuyGroupID > 0 && retError == Result_Define.eResult.SUCCESS)
            {
                switch ((Shop_Define.eCashBuyGroupType)BuyGroupID)
                {
                    case Shop_Define.eCashBuyGroupType.PVECOIN:
                        if (!VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.SHOP_BUY_MAX_KEY, BuyCount + userCurrentBuy.Buy_Count))
                            retError = Result_Define.eResult.VIP_SHOP_BUY_COUNT_OVER;
                        break;
                    case Shop_Define.eCashBuyGroupType.PVPCOIN:
                        if (!VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.SHOP_BUY_MAX_TIKET, BuyCount + userCurrentBuy.Buy_Count))
                            retError = Result_Define.eResult.VIP_SHOP_BUY_COUNT_OVER;
                        break;
                    case Shop_Define.eCashBuyGroupType.GOLD:
                        if (!VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.SHOP_BUY_MAX_GOLD, BuyCount + userCurrentBuy.Buy_Count))
                            retError = Result_Define.eResult.VIP_SHOP_BUY_COUNT_OVER;
                        break;
                    case Shop_Define.eCashBuyGroupType.BREAKING_STONE:
                        if (!VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.SHOP_BUY_MAX_BREAKSTONE, BuyCount + userCurrentBuy.Buy_Count))
                            retError = Result_Define.eResult.VIP_SHOP_BUY_COUNT_OVER;
                        break;
                    case Shop_Define.eCashBuyGroupType.ORB_LV1:
                    case Shop_Define.eCashBuyGroupType.ORB_LV2:
                        if (!VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.SHOP_BUY_MAX_ORB, BuyCount + userCurrentBuy.Buy_Count))
                            retError = Result_Define.eResult.VIP_SHOP_BUY_COUNT_OVER;
                        break;
                }

                if (retError == Result_Define.eResult.SUCCESS)
                {
                    System_Shop_Cash_Item SystemPriceFrom = ShopManager.GetSystem_Shop_Cash_ItemInfo(ref tb, BuyGroupID, userCurrentBuy.Buy_Count);
                    System_Shop_Cash_Item SystemPriceTo = ShopManager.GetSystem_Shop_Cash_ItemInfo(ref tb, BuyGroupID, userCurrentBuy.Buy_Count + BuyCount);

                    BuyPriceValue = SystemPriceTo.Buy_StackPriceValue_Stack - SystemPriceFrom.Buy_StackPriceValue_Stack;
                    makeCount = SystemPriceTo.Buy_StackItemNum_Stack - SystemPriceFrom.Buy_StackItemNum_Stack;
                    if (SystemPriceTo.Buy_ID == 0)
                        retError = Result_Define.eResult.SHOP_ITEM_BUY_PRICE_INFO_NOT_FOUND;
                }
            }
            else if (retError == Result_Define.eResult.SUCCESS)
            {
                if (Max_BuyCount > 0 && (userCurrentBuy.Buy_Count + BuyCount) > Max_BuyCount)
                    retError = Result_Define.eResult.VIP_SHOP_BUY_COUNT_OVER;
            }

            if (retError == Result_Define.eResult.SUCCESS)
                retError = ShopManager.UpdateUserBuyItemCount(ref tb, AID, ShopGoodsID, BuyCount, BuyPriceType);

            if (retError == Result_Define.eResult.SUCCESS)
            {
                foreach (System_Package_RewardBox makeInfo in rewardItem)
                {
                    List<User_Inven> makeItem = new List<User_Inven>();
                    retError = ItemManager.MakeItem(ref tb, ref makeItem, AID, makeInfo.Item_ID, makeCount > 0 ? makeCount : makeInfo.Item_Num * BuyCount, CID, makeInfo.Item_Level, makeInfo.Item_Grade, ShopGoodsID);
                    if (retError == Result_Define.eResult.SUCCESS)
                        makeRealItem.AddRange(makeItem);
                    else
                        break;
                }
            }

            if (retError == Result_Define.eResult.SUCCESS && makeRealItem.Count < 1)
                retError = Result_Define.eResult.ITEM_CREATE_FAIL;

            return retError;
        }


        public static User_Shop_TreasureBox GetUser_TreasureBox(ref TxnBlock TB, long AID, long CID, ref int remainTime, ref Result_Define.eResult retError, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            User_Shop_TreasureBox userShopInfo = ShopManager.GetUser_TreasureBox_List(ref TB, AID, Flush);
            DateTime regDate = DateTime.Parse(userShopInfo.regdate.ToShortDateString());
            DateTime nextDay = DateTime.Parse(DateTime.Now.ToShortDateString()).AddDays(1);
            TimeSpan remainTS = nextDay - DateTime.Now;
            remainTime = (int)remainTS.TotalSeconds;

            if ((userShopInfo.regdate.ToShortDateString() != DateTime.Now.ToShortDateString()) || string.IsNullOrEmpty(userShopInfo.ShopItemList))
            {
                User_Shop_TreasureBox_List setUserBox = new User_Shop_TreasureBox_List();
                int needGold = 0;
                int needCash = 0;
                int setGachaLevel = 0;      // TreasureBox not use level match value. always 0
                Character charInfo = CharacterManager.GetCharacter(ref TB, AID, CID);
                Shop_Define.eGachaType[] checkGachaType = new Shop_Define.eGachaType[] { Shop_Define.eGachaType.TREASURE_BOX_GOLD, Shop_Define.eGachaType.TREASURE_BOX_CASH };

                foreach (Shop_Define.eGachaType gachaType in checkGachaType)
                {
                    List<System_Drop_Group> getDropList = new List<System_Drop_Group>();
                    int tryCount = 0;
                    while (tryCount < 100 && getDropList.Count < Shop_Define.Shop_TreasureBox_ItemCount)
                    {
                        List<System_Drop_Group> pickList = DropManager.GetGachaResult(ref TB, ref needGold, ref needCash, (int)gachaType, setGachaLevel, (short)charInfo.Class);
                        pickList.ForEach(setItem => { if (getDropList.Count < Shop_Define.Shop_TreasureBox_ItemCount) getDropList.Add(setItem); });
                    }

                    if (getDropList.Count < 1)
                    {
                        retError = Result_Define.eResult.SHOP_TREASURE_BOX_CREATE_FAIL;
                        return userShopInfo;
                    }

                    int setSlot = 1;

                    if (gachaType == Shop_Define.eGachaType.TREASURE_BOX_GOLD)
                    {
                        setUserBox.gold_treasure = new List<RetShopTreasureBoxItem>();
                        getDropList.ForEach(setItem =>
                        {
                            setUserBox.gold_treasure.Add(new RetShopTreasureBoxItem(setItem, setSlot++));
                        });
                        setUserBox.gold_box_need_gold = needGold;
                        setUserBox.gold_box_need_ruby = needCash;
                    }
                    else if (gachaType == Shop_Define.eGachaType.TREASURE_BOX_CASH)
                    {
                        setUserBox.ruby_treasure = new List<RetShopTreasureBoxItem>();
                        getDropList.ForEach(setItem =>
                        {
                            setUserBox.ruby_treasure.Add(new RetShopTreasureBoxItem(setItem, setSlot++));
                        });
                        setUserBox.ruby_box_need_gold = needGold;
                        setUserBox.ruby_box_need_ruby = needCash;
                    }
                }

                userShopInfo.ShopItemList = mJsonSerializer.ToJsonString(setUserBox);
                retError = SetUser_TreasureBox_Info(ref TB, AID, userShopInfo);
            }
            else
            {
                retError = Result_Define.eResult.SUCCESS;
            }
            return userShopInfo;
        }

        public static User_Shop_TreasureBox ReplaceUser_TreasureBox(ref TxnBlock TB, long AID, long CID, Shop_Define.eGachaType BoxType, List<int> buySlots, out int outRubyBuyCount, ref List<User_Inven> makeItem, ref List<RetShopTreasureBoxItem> ReplaceItem, ref int remainTime, ref Result_Define.eResult retError, out int useGold, out int useRuby, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            useGold = 0;
            useRuby = 0;
            int setGachaLevel = 0;      // TreasureBox not use level match value. always 0
            Character charInfo = CharacterManager.GetCharacter(ref TB, AID, CID, Flush);

            User_Shop_TreasureBox userBoxInfo = ShopManager.GetUser_TreasureBox(ref TB, AID, CID, ref remainTime, ref retError);
            List<System_Drop_Group> makeDropList = new List<System_Drop_Group>();
            User_Shop_TreasureBox_List getBox = mJsonSerializer.JsonToObject<User_Shop_TreasureBox_List>(userBoxInfo.ShopItemList);
            outRubyBuyCount = userBoxInfo.RubyBuyCount;

            if (BoxType == Shop_Define.eGachaType.TREASURE_BOX_SPECIAL)
            {
                makeDropList = DropManager.GetGachaResult(ref TB, ref useGold, ref useRuby, (int)BoxType, setGachaLevel, (short)charInfo.Class);
                int specialGachaOpenCount = SystemData.GetConstValueInt(ref TB, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.DEF_GACHASTORE_SPEICAL_COUNT_01]);
                if (userBoxInfo.RubyBuyCount < specialGachaOpenCount)
                {
                    retError = Result_Define.eResult.SHOP_TREASURE_BOX_NOT_ENOUGH_RUBY_BUY_COUNT;
                    return userBoxInfo;
                }

                userBoxInfo.RubyBuyCount -= specialGachaOpenCount;
                outRubyBuyCount = userBoxInfo.RubyBuyCount;
            }
            else
            {
                var checkBoxList = BoxType == Shop_Define.eGachaType.TREASURE_BOX_GOLD ? getBox.gold_treasure :
                                            (BoxType == Shop_Define.eGachaType.TREASURE_BOX_CASH ? getBox.ruby_treasure : null);
                if (checkBoxList == null && BoxType != Shop_Define.eGachaType.TREASURE_BOX_SPECIAL)
                {
                    retError = Result_Define.eResult.SHOP_TREASURE_BOX_CREATE_FAIL;
                    return userBoxInfo;
                }
                else
                    retError = Result_Define.eResult.SUCCESS;

                ReplaceItem = new List<RetShopTreasureBoxItem>();
                //List<RetShopTreasureBoxItem> BaseList = checkBoxList.ToList();

                foreach (var setSlot in buySlots)
                {
                    if (BoxType == Shop_Define.eGachaType.TREASURE_BOX_CASH)
                    {
                        int setFindPos = checkBoxList.FindIndex(item => item.buyslot == setSlot);
                        if (checkBoxList[setFindPos].buyflag == 0)
                        {
                            int needGold = 0;
                            int needRuby = 0;
                            if (needGold == 0 && needRuby == 0)
                                DropManager.GetGachaResult(ref TB, ref needGold, ref needRuby, (int)BoxType, setGachaLevel, (short)charInfo.Class);

                            if (needGold > 0 || needRuby > 0)
                            {
                                useGold += needGold;
                                useRuby += needRuby;
                                makeDropList.Add(new System_Drop_Group(checkBoxList[setFindPos]));
                                RetShopTreasureBoxItem setItem = checkBoxList[setFindPos];
                                setItem.buyflag = 1;
                                ReplaceItem.Add(setItem);
                                checkBoxList[setFindPos] = setItem;
                            }
                        }
                        else
                            retError = Result_Define.eResult.SHOP_TREASURE_BOX_CREATE_FAIL;
                    }
                    else
                    {
                        int setFindPos = checkBoxList.FindIndex(item => item.buyslot == setSlot);

                        if (setFindPos >= 0)
                        {
                            int needGold = 0;
                            int needRuby = 0;
                            List<System_Drop_Group> pickList = DropManager.GetGachaResult(ref TB, ref needGold, ref needRuby, (int)BoxType, setGachaLevel, (short)charInfo.Class);
                            if (pickList.Count > 0)
                            {
                                useGold += needGold;
                                useRuby += needRuby;
                                System_Drop_Group setPick = pickList.FirstOrDefault();
                                RetShopTreasureBoxItem setItem = new RetShopTreasureBoxItem(setPick, setSlot);
                                ReplaceItem.Add(setItem);
                                makeDropList.Add(new System_Drop_Group(checkBoxList[setFindPos]));
                                checkBoxList[setFindPos] = setItem;
                            }
                        }
                    }
                }

                int ReplaceCount = ReplaceItem.Count;

                if (BoxType == Shop_Define.eGachaType.TREASURE_BOX_CASH)
                {
                    int needGold = 0;
                    int needRuby = 0;

                    if (checkBoxList.Count(item => item.buyflag == 1) == checkBoxList.Count)
                    {
                        ReplaceItem = new List<RetShopTreasureBoxItem>();
                        for (int setSlot = 1; setSlot <= checkBoxList.Count; setSlot++)
                        {
                            List<System_Drop_Group> pickList = DropManager.GetGachaResult(ref TB, ref needGold, ref needRuby, (int)BoxType, setGachaLevel, (short)charInfo.Class);
                            if (pickList.Count > 0)
                            {
                                System_Drop_Group setPick = pickList.FirstOrDefault();
                                RetShopTreasureBoxItem setItem = new RetShopTreasureBoxItem(setPick, setSlot);
                                ReplaceItem.Add(setItem);
                                checkBoxList[setSlot - 1] = setItem; // setpos zero base
                            }
                        }
                    }
                }

                if (BoxType == Shop_Define.eGachaType.TREASURE_BOX_GOLD)
                {
                    userBoxInfo.GoldBuyCount += ReplaceCount;
                    getBox.gold_treasure = checkBoxList;
                }
                else if (BoxType == Shop_Define.eGachaType.TREASURE_BOX_CASH)
                {
                    userBoxInfo.RubyBuyCount += ReplaceCount;
                    getBox.ruby_treasure = checkBoxList;
                }
                outRubyBuyCount = userBoxInfo.RubyBuyCount;
            }

            if (useGold > 0 && useRuby == 0 && retError == Result_Define.eResult.SUCCESS)
                retError = AccountManager.UseUserGold(ref TB, AID, useGold);
            if (useGold == 0 && useRuby > 0 && retError == Result_Define.eResult.SUCCESS)
                retError = AccountManager.UseUserCash(ref TB, AID, useRuby);
            if (useGold > 0 && useRuby > 0 && retError == Result_Define.eResult.SUCCESS)
                retError = AccountManager.UseUserGold_And_Ruby(ref TB, AID, useGold, useRuby);

            foreach (System_Drop_Group setDrop in makeDropList)
            {
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    System_Drop_Group setRealDrop = setDrop.DropMaxNum >= 10 || setDrop.DropMinNum >= 10 ? DropManager.CheckDropLimit(ref TB, setDrop, AID, CID, ref retError) : setDrop;
                    retError = DropManager.MakeDropItem(ref TB, ref makeItem, setRealDrop, AID, CID);
                }
                else
                    break;
            }

            if (retError == Result_Define.eResult.SUCCESS)
            {
                userBoxInfo.ShopItemList = mJsonSerializer.ToJsonString(getBox);
                retError = SetUser_TreasureBox_Info(ref TB, AID, userBoxInfo);
            }
            return userBoxInfo;
        }

        // get user User_TreasureBox list
        public static User_Shop_TreasureBox GetUser_TreasureBox_List(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setKey = GetRediskey_User_TreasureBox(AID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE AID = {1}", Shop_Define.Shop_User_Shop_TreasureBox_TableName, AID);
            User_Shop_TreasureBox retObj = GenericFetch.FetchFromRedis<User_Shop_TreasureBox>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
            return retObj == null ? new User_Shop_TreasureBox() : retObj;
        }

        private static string GetRediskey_User_TreasureBox(long AID)
        {
            return string.Format("{0}_{1}", Shop_Define.Shop_User_Shop_TreasureBox_TableName, AID);
        }

        public static void RemoveCache_User_TreasureBox(long AID)
        {
            string setKey = GetRediskey_User_TreasureBox(AID);
            RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_System, setKey);
        }

        public static Result_Define.eResult ResetUser_User_TreasureBox(ref TxnBlock TB, long AID, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET regdate = DATEADD(day, -1, GETDATE()) WHERE AID = {1}", Shop_Define.Shop_User_Shop_TreasureBox_TableName, AID);
            RemoveCache_User_TreasureBox(AID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult SetUser_TreasureBox_Info(ref TxnBlock TB, long AID, User_Shop_TreasureBox setInfo, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                ShopItemList = @setJson,	                                                
	                                                GoldBuyCount = @goldcount,	                                                
	                                                RubyBuyCount = @rubycount,	                                                
	                                                regdate = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ({1}, @setJson, @goldcount, @rubycount, GETDATE());
                                    ", Shop_Define.Shop_User_Shop_TreasureBox_TableName
                             , AID
                             );
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = setQuery;
            cmd.Parameters.AddWithValue("@setJson", setInfo.ShopItemList);
            cmd.Parameters.AddWithValue("@goldcount", setInfo.GoldBuyCount);
            cmd.Parameters.AddWithValue("@rubycount", setInfo.RubyBuyCount);

            RemoveCache_User_TreasureBox(AID);
            return TB.ExcuteSqlCommand(dbkey, ref cmd) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        // get user blackmarket shop list
        public static User_Shop_BlackMarket GetUser_BlackMarket_List(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setKey = GetRediskey_User_BlackMarket_List(AID);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE AID = {1}", Shop_Define.Shop_User_Shop_BlackMarket_TableName, AID);
            User_Shop_BlackMarket retObj = GenericFetch.FetchFromRedis<User_Shop_BlackMarket>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
            return retObj == null ? new User_Shop_BlackMarket() : retObj;
        }

        private static string GetRediskey_User_BlackMarket_List(long AID)
        {
            return string.Format("{0}_{1}", Shop_Define.Shop_User_Shop_BlackMarket_TableName, AID);
        }

        public static void RemoveCache_User_BlackMarket_List(long AID)
        {
            string setKey = GetRediskey_User_BlackMarket_List(AID);
            RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_System, setKey);
        }

        public static Result_Define.eResult ResetUser_BlackMarket_List(ref TxnBlock TB, long AID, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET regdate = DATEADD(day, -1, GETDATE()) WHERE AID = {1}", Shop_Define.Shop_User_Shop_BlackMarket_TableName, AID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult SetUser_BlackMarket_List(ref TxnBlock TB, long AID, List<System_Shop_BlackMarket> setList, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                ShopItemList = @setJson,	                                                
	                                                regdate = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ({1}, @setJson, GETDATE());
                                    ", Shop_Define.Shop_User_Shop_BlackMarket_TableName
                             , AID
                             );
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = setQuery;
            cmd.Parameters.AddWithValue("@setJson", mJsonSerializer.ToJsonString(setList));

            return TB.ExcuteSqlCommand(dbkey, ref cmd) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        // get blackmarket shop list
        public static List<System_Shop_BlackMarket> GetSystem_BlackMarket_List(ref TxnBlock TB, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            //string setKey = GetRediskey_System_ShopList(Shop_Define.eShopType.BlackMarket, Shop_Define.eBillingType.None, dbkey);
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE delflag > 0 ORDER BY SlotID ASC, Shop_Goods_ID ASC", Shop_Define.Shop_Type_TableList[Shop_Define.eShopType.BlackMarket]);
            //return GenericFetch.FetchFromRedis_MultipleRow<System_Shop_BlackMarket>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
            return GenericFetch.FetchFromDB_MultipleRow<System_Shop_BlackMarket>(ref TB, setQuery, dbkey);
        }

        private static string GetRediskey_System_ShopList(Shop_Define.eShopType ShopType, Shop_Define.eBillingType BillingType, string dbkey = Shop_Define.Shop_Info_DB)
        {
            return string.Format("{0}_{1}_(2)", dbkey, Shop_Define.Shop_Type_TableList[ShopType], (int)BillingType);
        }

        // get shop list with admin tool sale rate
        public static List<System_Shop_Goods> GetSystem_ShopList(ref TxnBlock TB, Shop_Define.eShopType ShopType, bool takeAll = false, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            return GetSystem_ShopList(ref TB, ShopType, Shop_Define.eBillingType.None, takeAll, Flush, dbkey);
        }

        public static void RemoveShopList(Shop_Define.eShopType ShopType, Shop_Define.eBillingType BillingType, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setKey = GetRediskey_System_ShopList(ShopType, BillingType, dbkey);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_System, setKey);
        }

        public static Result_Define.eResult SetProductID(ref TxnBlock TB, long ShopGoodsCode, Shop_Define.eShopType ShopType, Shop_Define.eBillingType BillingType, string ProductID, int PriceValue, int PriceTier, string dbkey = Shop_Define.Shop_Info_DB)
        {
            /// 安卓方面的列表，统一使用GOOGLE的配置
            /// IOS方面的列表，统一使用APPLE的配置
            BillingType = ProcessBillintType(BillingType);

            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON Shop_Goods_ID = @setGoods AND Billing_Platform_Type = @setBilling
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                Product_ID = @setID,
	                                                PriceValue = @setValue,
	                                                PriceTier = @setTier
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES (@setGoods, @setBilling, @setID, @setValue, @setTier);
                                    ", Shop_Define.Shop_GoodsCode_TableName
                             );
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = setQuery;
            cmd.Parameters.AddWithValue("@setGoods", ShopGoodsCode);
            cmd.Parameters.AddWithValue("@setBilling", (int)BillingType);
            cmd.Parameters.AddWithValue("@setID", ProductID);
            cmd.Parameters.AddWithValue("@setValue", PriceValue);
            cmd.Parameters.AddWithValue("@setTier", PriceTier);

            Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, ref cmd) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
            //if (retError == Result_Define.eResult.SUCCESS)
            //    RemoveShopList(ShopType, BillingType, dbkey);

            return retError;
        }

        public static System_Shop_Goods_Code GetSystem_ShopGoodsCode(ref TxnBlock TB, long ShopItemID, Shop_Define.eBillingType BillingType, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format("SELECT * FROM {0} WITH(NOLOCK) WHERE Shop_Goods_ID = {1} AND Billing_Platform_Type = {2}"
                , Shop_Define.Shop_GoodsCode_TableName, ShopItemID, (int)BillingType);
            System_Shop_Goods_Code retObj = GenericFetch.FetchFromDB<System_Shop_Goods_Code>(ref TB, setQuery, dbkey);
            return retObj == null ? new System_Shop_Goods_Code() : retObj;
        }

        // get shop list with admin tool sale rate
        public static List<System_Shop_Goods> GetSystem_ShopList(ref TxnBlock TB, Shop_Define.eShopType ShopType, Shop_Define.eBillingType BillingType, bool takeAll = false, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            /// 安卓方面的列表，统一使用GOOGLE的配置
            /// IOS方面的列表，统一使用APPLE的配置
            BillingType = ProcessBillintType(BillingType);

            //string setKey = GetRediskey_System_ShopList(ShopType, BillingType, dbkey);
            string setQuery = string.Format(@"
                                                        SELECT A.*
		                                                        , ISNULL(B.Sale_Rate, {3}) AS Sale_Rate
		                                                        , DATEDIFF(MINUTE, GETDATE(), ISNULL(B.SaleStartTime, GETDATE()-1)) AS SaleStartTime
		                                                        , DATEDIFF(MINUTE, GETDATE(), ISNULL(B.SaleEndTime, getdate()+1)) AS SaleEndTime
		                                                        , ISNULL(B.SaleType, 0) AS SaleType, ISNULL(B.DefaultSaleType, {4}) AS DefaultSaleType
		                                                        , ISNULL(C.Product_ID, '') as Product_ID
		                                                        , ISNULL(C.PriceValue, 0) as ProductPrice
		                                                        , ISNULL(C.PriceTier, 0) as ProductTier	
                                                                                                        FROM {0} AS A WITH(NOLOCK)
                                                                                                        LEFT OUTER JOIN {1} AS B WITH(NOLOCK)
													                                                        ON A.Shop_Goods_ID = B.Shop_Goods_ID 
                                                                                                        LEFT OUTER JOIN {2} AS C WITH(NOLOCK)
													                                                        ON A.Shop_Goods_ID = C.Shop_Goods_ID
													                                                        AND C.Billing_Platform_Type = {5}"
                                                                , Shop_Define.Shop_Type_TableList[ShopType]
                                                                , Shop_Define.Shop_Limit_TableName
                                                                , Shop_Define.Shop_GoodsCode_TableName
                                                                , Shop_Define.Shop_Default_Sale_Rate
                                                                , (int)Shop_Define.Shop_Default_Sale_Type
                                                                , (int)BillingType
                );

            if (!takeAll)
            {
                setQuery = string.Format(@"{0}
                                                                WHERE DATEDIFF(MINUTE, GETDATE(), ISNULL(B.SaleStartTime, GETDATE()-1)) < 0
                                                                    AND DATEDIFF(MINUTE, GETDATE(), ISNULL(B.SaleEndTime, getdate()+1)) > 0", setQuery);
            }

            return GenericFetch.FetchFromDB_MultipleRow<System_Shop_Goods>(ref TB, setQuery, dbkey);

            //if (BillingType == Shop_Define.eBillingType.Kr_aOS_Google || BillingType == Shop_Define.eBillingType.Kr_iOS_Appstore || BillingType == Shop_Define.eBillingType.Kr_aOS_OneStore
            //    || BillingType == Shop_Define.eBillingType.Global_aOS_Google || BillingType == Shop_Define.eBillingType.Global_iOS_Appstore
            //    )
            //    return GenericFetch.FetchFromDB_MultipleRow<System_Shop_Goods>(ref TB, setQuery, dbkey);
            //else
            //    return GenericFetch.FetchFromRedis_MultipleRow<System_Shop_Goods>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
        }

        //use admin tool
        public static System_Shop_Point GetSystem_ShopData(ref TxnBlock TB, long shopID, Shop_Define.eShopType ShopType, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"SELECT A.*, ISNULL(B.Sale_Rate, {2}) AS Sale_Rate, DATEDIFF(MINUTE, GETDATE(), ISNULL(B.SaleStartTime, GETDATE()-1)) AS SaleStartTime
                                                            , DATEDIFF(MINUTE, GETDATE(), ISNULL(B.SaleEndTime, getdate()+1)) AS SaleEndTime
                                                            , ISNULL(B.SaleType, {3}) AS SaleType, ISNULL(B.DefaultSaleType, {3}) AS DefaultSaleType
                                                FROM {0} AS A WITH(NOLOCK) LEFT OUTER JOIN {1} AS B ON A.Shop_Goods_ID = B.Shop_Goods_ID Where A.Shop_Goods_ID = {4} "
                                               , Shop_Define.Shop_Type_TableList[ShopType]
                                               , Shop_Define.Shop_Limit_TableName
                                               , Shop_Define.Shop_Default_Sale_Rate
                                               , (int)Shop_Define.Shop_Default_Sale_Type
                                               , shopID);
            System_Shop_Point retObj = GenericFetch.FetchFromDB<System_Shop_Point>(ref TB, setQuery, dbkey);
            if (retObj == null)
                retObj = new System_Shop_Point();
            return retObj;
        }

        public static Result_Define.eResult CalcBuyPrice(ref TxnBlock TB, long AID, long ShopGoodsID, int BuyCount = 1)
        {
            return Result_Define.eResult.SUCCESS;
        }


        // calc user buy count for buy price
        public static System_Shop_Cash_Item GetSystem_Shop_Cash_ItemInfo(ref TxnBlock TB, int Buy_GroupID, int Buy_Count, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setKey = string.Format("{0}_{1}", Shop_Define.Shop_AccumPrice_TableName, Shop_Define.Shop_Info_Surfix);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE Buy_GroupID = {1} AND Buy_Count = {2}", Shop_Define.Shop_AccumPrice_TableName, Buy_GroupID, Buy_Count);
            string setMember = string.Format("{0}_{1}", Buy_GroupID, Buy_Count);

            System_Shop_Cash_Item retObj = GenericFetch.FetchFromRedis_Hash<System_Shop_Cash_Item>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setMember, setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new System_Shop_Cash_Item();
        }

        public static void RemoveItemBuyCache(long AID)
        {
            string setKey = GetItemBuyItemKey(AID);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static string GetItemBuyItemKey(long AID)
        {
            return string.Format("{0}_{1}", Shop_Define.Shop_User_Buy_TableName, AID);
        }

        public static List<User_Shop_Buy> GetUser_All_BuyItemCount(ref TxnBlock TB, long AID, Shop_Define.eShopType ShopType, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            //string setKey = GetItemBuyItemKey(AID);
            List<User_Shop_Buy> retObj = new List<User_Shop_Buy>();

            //if(!Flush)
            //    retObj = TheSoul.DataManager.GenericFetch.FetchFromOnly_Redis_Hash_All<User_Shop_Buy>(DataManager_Define.RedisServerAlias_User, setKey);

            //if (retObj.Count < 1)            
            {
                string setQuery = string.Format(@"
                                                SELECT A.*, B.[Type] AS GoodsType 
                                                    FROM {0} as A WITH(NOLOCK)
                                                    INNER JOIN {1} AS B WITH(NOLOCK)
                                                        ON A.Shop_Goods_ID = B.Shop_Goods_ID WHERE A.AID = {2}"
                                                , Shop_Define.Shop_User_Buy_TableName
                                                , Shop_Define.Shop_Type_TableList[ShopType]
                                                , AID);
                List<User_Shop_Buy> listObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_Shop_Buy>(ref TB, setQuery, dbkey);
                TimeSpan TS = new TimeSpan();
                foreach (User_Shop_Buy setObj in listObj)
                {
                    DateTime curDate = DateTime.Parse(setObj.regdate.ToShortDateString());
                    DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                    DateTime NextDate = new DateTime(dbDate.Year, dbDate.Month, DateTime.Now.AddDays(1).Day, 0, 0, 0);
                    TS = dbDate - curDate;

                    if (TS.Days != 0 && setObj.GoodsType != (int)Shop_Define.eShopSaleType.BuyOnceSale)
                        setObj.Buy_Count = 0;

                    TS = NextDate - dbDate;
                    //RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, setObj.Shop_Goods_ID.ToString(), setObj);

                    retObj.Add(setObj);
                }
                //RedisConst.GetRedisInstance().SetExpireTimeHash(DataManager_Define.RedisServerAlias_User, setKey, TS);

            }
            return retObj;
        }

        public static string GetRediskey_Shop_System_Package_Cheap()
        {
            string setKey = string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Shop_Define.Shop_System_Package_Cheap_TableName);
            return setKey;
        }

        public static Shop_Define.eBillingType ProcessBillintType( Shop_Define.eBillingType BillingType) 
        {
            /// 安卓方面的列表，统一使用GOOGLE的配置
            /// IOS方面的列表，统一使用APPLE的配置
            if (BillingType >= Shop_Define.eBillingType.Global_aOS_Google && BillingType < Shop_Define.eBillingType.Global_iOS_Appstore)
            {
                BillingType = Shop_Define.eBillingType.Global_aOS_Google;
            }

            if (BillingType >= Shop_Define.eBillingType.Global_iOS_Appstore && BillingType < Shop_Define.eBillingType.Tw_iOS_Appstore)
            {
                BillingType = Shop_Define.eBillingType.Global_iOS_Appstore;
            }

            if (BillingType >= Shop_Define.eBillingType.mfun_aOS_Google && BillingType < Shop_Define.eBillingType.mfun_iOS_Appstore)
            {
                BillingType = Shop_Define.eBillingType.mfun_aOS_Google;
            }

            if (BillingType >= Shop_Define.eBillingType.mfun_iOS_Appstore && BillingType < Shop_Define.eBillingType.yuenan_aOS_Google)
            {
                BillingType = Shop_Define.eBillingType.mfun_iOS_Appstore;
            }

            if (BillingType >= Shop_Define.eBillingType.yuenan_aOS_Google && BillingType < Shop_Define.eBillingType.yuenan_iOS_Appstore)
            {
                BillingType = Shop_Define.eBillingType.yuenan_aOS_Google;
            }

            if (BillingType >= Shop_Define.eBillingType.yuenan_iOS_Appstore && BillingType < Shop_Define.eBillingType.yuenan_iOS_end)
            {
                BillingType = Shop_Define.eBillingType.yuenan_iOS_Appstore;
            }

            return BillingType;
        }

        public static List<System_Package_List> GetShop_System_Package_Cheap_List(ref TxnBlock TB, Shop_Define.eBillingType BillingType, bool takeAll = false, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            /// 安卓方面的列表，统一使用GOOGLE的配置
            /// IOS方面的列表，统一使用APPLE的配置
            BillingType = ProcessBillintType(BillingType);

            /*
                                                                , ISNULL(C.Product_ID, '') as Product_ID
		                                                        , ISNULL(C.PriceValue, 0) as ProductPrice
		                                                        , ISNULL(C.PriceTier, 0) as ProductTier	
                                                                                                        FROM {0} AS A WITH(NOLOCK)
                                                                                                        LEFT OUTER JOIN {1} AS B WITH(NOLOCK)
													                                                        ON A.Shop_Goods_ID = B.Shop_Goods_ID 
                                                                                                        LEFT OUTER JOIN {2} AS C WITH(NOLOCK)
													                                                        ON A.Shop_Goods_ID = C.Shop_Goods_ID
													                                                        AND C.Billing_Platform_Type = {5}"
             */
            //string setKey = GetRediskey_Shop_System_Package_Cheap();
            string setQuery = takeAll ? string.Format(@"SELECT A.*, ISNULL(B.SaleStartTime, GETDATE()-1) AS SaleStartTime, ISNULL(B.SaleEndTime, getdate()+1) AS SaleEndTime
                                                                , ISNULL(C.Product_ID, '') as Product_ID
		                                                        , ISNULL(C.PriceValue, 0) as ProductPrice
		                                                        , ISNULL(C.PriceTier, 0) as ProductTier	
                                                        FROM {0} AS A WITH(NOLOCK)
                                                                LEFT OUTER JOIN {1} AS B WITH(NOLOCK) ON A.Package_ID = B.Shop_Goods_ID
                                                                LEFT OUTER JOIN {2} AS C WITH(NOLOCK)
													                ON A.Package_ID = C.Shop_Goods_ID
													                AND C.Billing_Platform_Type = {3}
                                                        ", Shop_Define.Shop_System_Package_Cheap_TableName
                                                         , Shop_Define.Shop_Limit_TableName
                                                         , Shop_Define.Shop_GoodsCode_TableName
                                                         , (int)BillingType)
                                    : string.Format(@"SELECT A.*, ISNULL(B.SaleStartTime, GETDATE()-1) AS SaleStartTime, ISNULL(B.SaleEndTime, getdate()+1) AS SaleEndTime
                                                                , ISNULL(C.Product_ID, '') as Product_ID
		                                                        , ISNULL(C.PriceValue, 0) as ProductPrice
		                                                        , ISNULL(C.PriceTier, 0) as ProductTier	
                                                        FROM {0} AS A WITH(NOLOCK)
                                                                LEFT OUTER JOIN {1} AS B WITH(NOLOCK) ON A.Package_ID = B.Shop_Goods_ID 
                                                                LEFT OUTER JOIN {2} AS C WITH(NOLOCK)
													                ON A.Package_ID = C.Shop_Goods_ID
													                AND C.Billing_Platform_Type = {3}
                                                        WHERE ActiveType > 0 
                                                            And DATEDIFF(MINUTE, GETDATE(), ISNULL(B.SaleStartTime, GETDATE()-1)) < 0 
                                                            And DATEDIFF(MINUTE, GETDATE(), ISNULL(B.SaleEndTime, getdate()+1)) > 0"
                                                        , Shop_Define.Shop_System_Package_Cheap_TableName
                                                        , Shop_Define.Shop_Limit_TableName
                                                        , Shop_Define.Shop_GoodsCode_TableName
                                                        , (int)BillingType);

            //if (BillingType == Shop_Define.eBillingType.Kr_aOS_Google || BillingType == Shop_Define.eBillingType.Kr_iOS_Appstore
            //    || BillingType == Shop_Define.eBillingType.Global_aOS_Google || BillingType == Shop_Define.eBillingType.Global_iOS_Appstore
            //    )
            //    return GenericFetch.FetchFromDB_MultipleRow<System_Package_List>(ref TB, setQuery, dbkey);
            //else
            //    return GenericFetch.FetchFromRedis_MultipleRow<System_Package_List>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);

            return GenericFetch.FetchFromDB_MultipleRow<System_Package_List>(ref TB, setQuery, dbkey);

        }

        public static string GetRediskey_Shop_System_Package_CheapRewardBox(long RewardBoxID)
        {
            string setKey = string.Format("{0}_{1}_{2}", Trigger_Define.Trigger_Prefix, Shop_Define.Shop_System_Package_Cheap_RewardBox_TableName, RewardBoxID);
            return setKey;
        }

        public static List<System_Package_RewardBox> GetShop_System_Package_Cheap_RewardBox(ref TxnBlock TB, long RewardBoxID, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setKey = GetRediskey_Shop_System_Package_CheapRewardBox(RewardBoxID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE RewardBoxID = {1}"
                                            , Shop_Define.Shop_System_Package_Cheap_RewardBox_TableName, RewardBoxID);
            return GenericFetch.FetchFromRedis_MultipleRow<System_Package_RewardBox>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
        }

        public static void RemoveShop_System_Package_List()
        {
            string setKey = GetRediskey_Shop_System_Package_List();
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_System, setKey);
        }

        public static string GetRediskey_Shop_System_Package_List()
        {
            string setKey = string.Format("{0}_{1}", Trigger_Define.Trigger_Prefix, Shop_Define.Shop_System_Package_List_TableName);
            return setKey;
        }

        public static List<System_Package_List> GetShop_System_Package_List(ref TxnBlock TB, Shop_Define.eBillingType BillingType, bool takeAll = false, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            /// 安卓方面的列表，统一使用GOOGLE的配置
            /// IOS方面的列表，统一使用APPLE的配置
            BillingType = ProcessBillintType(BillingType);

            //string setKey = GetRediskey_Shop_System_Package_List();
            string setQuery = takeAll ? string.Format(@"SELECT A.*, ISNULL(B.SaleStartTime, GETDATE()-1) AS SaleStartTime, ISNULL(B.SaleEndTime, getdate()+1) AS SaleEndTime 
                                                                , ISNULL(C.Product_ID, '') as Product_ID
		                                                        , ISNULL(C.PriceValue, 0) as ProductPrice
		                                                        , ISNULL(C.PriceTier, 0) as ProductTier	
                                                        FROM {0} AS A WITH(NOLOCK) 
                                                                LEFT OUTER JOIN {1} AS B WITH(NOLOCK) ON A.Package_ID = B.Shop_Goods_ID
                                                                LEFT OUTER JOIN {2} AS C WITH(NOLOCK)
													                ON A.Package_ID = C.Shop_Goods_ID
													                AND C.Billing_Platform_Type = {3}"
                                                            , Shop_Define.Shop_System_Package_List_TableName
                                                            , Shop_Define.Shop_Limit_TableName
                                                            , Shop_Define.Shop_GoodsCode_TableName
                                                            , (int)BillingType)
                                    : string.Format(@"SELECT A.*, ISNULL(B.SaleStartTime, GETDATE()-1) AS SaleStartTime, ISNULL(B.SaleEndTime, getdate()+1) AS SaleEndTime
                                                                , ISNULL(C.Product_ID, '') as Product_ID
		                                                        , ISNULL(C.PriceValue, 0) as ProductPrice
		                                                        , ISNULL(C.PriceTier, 0) as ProductTier	
                                                        FROM {0} AS A WITH(NOLOCK) 
                                                                LEFT OUTER JOIN {1} AS B WITH(NOLOCK) ON A.Package_ID = B.Shop_Goods_ID
                                                                LEFT OUTER JOIN {2} AS C WITH(NOLOCK)
													                ON A.Package_ID = C.Shop_Goods_ID
													                AND C.Billing_Platform_Type = {3}
                                                        WHERE ActiveType > 0
                                                                And DATEDIFF(MINUTE, GETDATE(), ISNULL(B.SaleStartTime, GETDATE()-1)) < 0
                                                                And DATEDIFF(MINUTE, GETDATE(), ISNULL(B.SaleEndTime, getdate()+1)) > 0"
                                                            , Shop_Define.Shop_System_Package_List_TableName
                                                            , Shop_Define.Shop_Limit_TableName
                                                            , Shop_Define.Shop_GoodsCode_TableName
                                                            , (int)BillingType);

            return GenericFetch.FetchFromDB_MultipleRow<System_Package_List>(ref TB, setQuery, dbkey);

            //if (BillingType == Shop_Define.eBillingType.Kr_aOS_Google || BillingType == Shop_Define.eBillingType.Kr_iOS_Appstore
            //    || BillingType == Shop_Define.eBillingType.Global_aOS_Google || BillingType == Shop_Define.eBillingType.Global_iOS_Appstore
            //    || takeAll)
            //    return GenericFetch.FetchFromDB_MultipleRow<System_Package_List>(ref TB, setQuery, dbkey);
            //else
            //    return GenericFetch.FetchFromRedis_MultipleRow<System_Package_List>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);

            //return takeAll ? GenericFetch.FetchFromDB_MultipleRow<System_Package_List>(ref TB, setQuery, dbkey) :
            //    GenericFetch.FetchFromRedis_MultipleRow<System_Package_List>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
        }

        public static string GetRediskey_Shop_System_Package_RewardBodx(long RewardBoxID)
        {
            return string.Format("{0}_{1}_{2}", Trigger_Define.Trigger_Prefix, Shop_Define.Shop_System_Package_RewardBox_TableName, RewardBoxID);
        }

        public static List<System_Package_RewardBox> GetShop_System_Package_RewardBox(ref TxnBlock TB, long RewardBoxID, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setKey = GetRediskey_Shop_System_Package_RewardBodx(RewardBoxID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE RewardBoxID = {1}"
                                            , Shop_Define.Shop_System_Package_RewardBox_TableName, RewardBoxID);
            return GenericFetch.FetchFromRedis_MultipleRow_Hash<System_Package_RewardBox>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, RewardBoxID.ToString(), setQuery, dbkey, Flush);
        }

        public static List<RetPackageList> GetShop_Package_List(ref TxnBlock TB, Shop_Define.eBillingType BillingType, long AID, long CID, bool isCheap, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            List<RetPackageList> retObj = new List<RetPackageList>();
            List<System_Package_List> shopPackageList = isCheap ?
                ShopManager.GetShop_System_Package_Cheap_List(ref TB, BillingType).ToList<System_Package_List>() :
                ShopManager.GetShop_System_Package_List(ref TB, BillingType);

            Character charInfo = CharacterManager.GetCharacter(ref TB, AID, CID);

            foreach (System_Package_List setShop in shopPackageList)
            {
                User_Shop_Buy buyCount = GetUserBuyItemCount(ref TB, AID, setShop.Package_ID);
                if (!CheckPackageLoop((Trigger_Define.eEventLoopType)setShop.LoopType, buyCount.regdate))
                    buyCount.Buy_Count = buyCount.TotalBuy_Count;
                RetPackageList setPackage = new RetPackageList(setShop, buyCount.Buy_Count);

                List<System_Package_RewardBox> packageRewardList = new List<System_Package_RewardBox>();
                if (setShop.Reward_Box1ID > 0)
                    packageRewardList.AddRange(
                        isCheap ? ShopManager.GetShop_System_Package_Cheap_RewardBox(ref TB, setShop.Reward_Box1ID) :
                        ShopManager.GetShop_System_Package_RewardBox(ref TB, setShop.Reward_Box1ID)
                        );
                if (setShop.Reward_Box2ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Warrior)
                    packageRewardList.AddRange(
                        isCheap ? ShopManager.GetShop_System_Package_Cheap_RewardBox(ref TB, setShop.Reward_Box2ID) :
                        ShopManager.GetShop_System_Package_RewardBox(ref TB, setShop.Reward_Box2ID)
                        );
                if (setShop.Reward_Box3ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Swordmaster)
                    packageRewardList.AddRange(
                        isCheap ? ShopManager.GetShop_System_Package_Cheap_RewardBox(ref TB, setShop.Reward_Box3ID) :
                        ShopManager.GetShop_System_Package_RewardBox(ref TB, setShop.Reward_Box3ID)
                        );
                if (setShop.Reward_Box4ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Taoist)
                    packageRewardList.AddRange(
                        isCheap ? ShopManager.GetShop_System_Package_Cheap_RewardBox(ref TB, setShop.Reward_Box4ID) :
                        ShopManager.GetShop_System_Package_RewardBox(ref TB, setShop.Reward_Box4ID)
                        );
                packageRewardList.ForEach(setItem =>
                {
                    setPackage.item_list.Add(new RetPackageReward(setItem));
                }
                );

                retObj.Add(setPackage);
            }

            return retObj;
        }

        private static bool CheckPackageLoop(Trigger_Define.eEventLoopType loopType, DateTime startDate)
        {
            bool retObj = false;
            switch (loopType)
            {
                case Trigger_Define.eEventLoopType.None:
                    break;
                case Trigger_Define.eEventLoopType.Day:
                case Trigger_Define.eEventLoopType.Week:
                case Trigger_Define.eEventLoopType.Month:
                    {
                        startDate = DateTime.Parse(startDate.ToShortDateString());
                        DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());

                        if (loopType == Trigger_Define.eEventLoopType.Day)
                        {
                            TimeSpan ts = dbDate - startDate;
                            if (ts.Days != 0)
                                retObj = true;
                        }
                        else if (loopType == Trigger_Define.eEventLoopType.Week)
                        {
                            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
                            Calendar cal = dfi.Calendar;

                            int startWeek = cal.GetWeekOfYear(startDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
                            int currentWeek = cal.GetWeekOfYear(dbDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

                            if (startWeek != currentWeek || startDate.Year != dbDate.Year || startDate.Month != dbDate.Month)
                                retObj = true;
                        }
                        else if (loopType == Trigger_Define.eEventLoopType.Month)
                        {
                            if (startDate.Year != dbDate.Year || startDate.Month != dbDate.Month)
                                retObj = true;
                        }
                        break;
                    }
            }


            return retObj;
        }

        /// 所有角色购买ID的商品的次数，看样子是个数组
        public static List<User_Shop_Buy> GetUserBuyItemCount_BY_ShopGoodsID(ref TxnBlock TB, long shopGoodsID, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format("Select * From {0} WITH(NOLOCK) Where Shop_Goods_ID = {1}", Shop_Define.Shop_User_Buy_TableName, shopGoodsID);
            List<User_Shop_Buy> retObj = GenericFetch.FetchFromDB_MultipleRow<User_Shop_Buy>(ref TB, setQuery, dbkey);
            return retObj;
        }

        //public static List<User_Shop_Buy> GetUser_All_BuyItemList_ByID(ref TxnBlock TB, long AID, long Shop_Goods_ID, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        //{
        //    string setQuery = string.Format("SELECT * FROM {0} WHERE AID = {1} AND Shop_Goods_ID = {2}", Shop_Define.Shop_User_Buy_TableName, AID, Shop_Goods_ID);
        //    List<User_Shop_Buy> retObj = TheSoul.DataManager.GenericFetch.FetchFromDB_MultipleRow<User_Shop_Buy>(ref TB, setQuery, dbkey);
        //    return retObj;
        //}

        /// 指定角色购买指定商品的次数结构体
        public static User_Shop_Buy GetUserBuyItemCount(ref TxnBlock TB, long AID, long ShopGoodsID, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setKey = GetItemBuyItemKey(AID);
            string setQuery = ShopGoodsID > 0 ? string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1} AND Shop_Goods_ID = {2}", Shop_Define.Shop_User_Buy_TableName, AID, ShopGoodsID)
                : string.Format(@"SELECT AID, 0 AS Shop_Goods_ID, SUM(TotalBuy_Count) as TotalBuy_Count, SUM(Buy_Count) as Buy_Count FROM {0} WITH(NOLOCK)  GROUP BY AID HAVING AID = {1}", Shop_Define.Shop_User_Buy_TableName, AID);
            User_Shop_Buy retObj = TheSoul.DataManager.GenericFetch.FetchFromDB<User_Shop_Buy>(ref  TB, setQuery, dbkey);

            if (retObj == null)
            {
                retObj = new User_Shop_Buy();
                retObj.AID = AID;
                retObj.Shop_Goods_ID = ShopGoodsID;
                retObj.regdate = DateTime.Now;
                retObj.Buy_Count = 0;
            }

            DateTime curDate = DateTime.Parse(retObj.regdate.ToShortDateString());
            DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            TimeSpan TS = dbDate - curDate;

            if (TS.Days != 0)
                retObj.Buy_Count = 0;

            //RedisConst.GetRedisInstance().SetHashField(DataManager_Define.RedisServerAlias_User, setKey, retObj.Shop_Goods_ID.ToString(), retObj);
            return retObj;
        }

        public static Result_Define.eResult UpdateUserBuyItemCount(ref TxnBlock TB, long AID, long BuyGoodsID, long Buy_Count, Item_Define.eItemBuyPriceType BuyPriceType, bool isReset = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            SqlCommand commandUpdate_User_Shop_Buy = new SqlCommand();
            commandUpdate_User_Shop_Buy.CommandText = "System_Update_User_Shop_Buy";
            var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandUpdate_User_Shop_Buy.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            commandUpdate_User_Shop_Buy.Parameters.Add("@SHOP_GOODS_ID", SqlDbType.BigInt).Value = BuyGoodsID;
            commandUpdate_User_Shop_Buy.Parameters.Add("@BUY_COUNT", SqlDbType.BigInt).Value = Buy_Count;
            commandUpdate_User_Shop_Buy.Parameters.Add("@Reset_Buy_Count", SqlDbType.Int).Value = isReset ? 1 : 0;
            commandUpdate_User_Shop_Buy.Parameters.Add(result);

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUpdate_User_Shop_Buy))
            {
                if (System.Convert.ToInt64(result.Value) < 0)
                {
                    long dbresult = System.Convert.ToInt64(result.Value) * -1;       // re delcare result code 
                    retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                }
                else
                {
                    if (Buy_Count > 0)
                    {
                        List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                        setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Charge, BuyGoodsID, 0, Buy_Count));
                        if(BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_PayReal)
                            setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Charge_Billing, BuyGoodsID, 0, Buy_Count));
                        retError = TriggerManager.ProgressTrigger(ref TB, AID, setDataList);
                    }

                    RemoveItemBuyCache(AID);
                    retError = Result_Define.eResult.SUCCESS;
                }
            }
            else
                retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            commandUpdate_User_Shop_Buy.Dispose();
            return retError;
        }

        public static User_Shop_Reset GetUserShopResetInfo(ref TxnBlock TB, long AID, Shop_Define.eShopType shoptype, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1} AND shoptype = {2}", Shop_Define.Shop_User_Reset_TableName, AID, (int)shoptype);

            User_Shop_Reset retObj = GenericFetch.FetchFromDB<User_Shop_Reset>(ref TB, setQuery, dbkey);
            if (retObj == null)
            {
                retObj = new User_Shop_Reset();
                retObj.aid = AID;
                retObj.regdate = DateTime.Now;
            }

            DateTime curDate = DateTime.Parse(retObj.regdate.ToShortDateString());
            DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            TimeSpan TS = dbDate - curDate;
            if (TS.Days != 0)
                retObj.reset_count = 0;

            return retObj;
        }

        public static Result_Define.eResult AddUserShopResetCount(ref TxnBlock TB, long AID, Shop_Define.eShopType shoptype, ref User_Shop_Reset setObj, string dbkey = Shop_Define.Shop_Info_DB)
        {
            setObj.shoptype = (int)shoptype;
            setObj.total_reset_count++;
            setObj.reset_count++;
            return UpdateUserShopResetInfo(ref TB, AID, setObj);
        }

        private static Result_Define.eResult UpdateUserShopResetInfo(ref TxnBlock TB, long AID, User_Shop_Reset setInfo, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = {1} AND shoptype = {2}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                total_reset_count = {3},
	                                                reset_count = {4},
	                                                regdate = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ({1}, {2}, {3}, {4}, GETDATE());
                                    ", Shop_Define.Shop_User_Reset_TableName
                                     , AID
                                     , setInfo.shoptype
                                     , setInfo.total_reset_count
                                     , setInfo.reset_count
                                     );
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult ResetUserBuyItemCount(ref TxnBlock TB, long AID, long BuyGoodsID, string dbkey = Shop_Define.Shop_Info_DB)
        {
            return UpdateUserBuyItemCount(ref TB, AID, BuyGoodsID, 0, Item_Define.eItemBuyPriceType.None, true);
        }

        // get best gacha info
        public static System_Gacha_Best GetSystem_Shop_Gacha_Best(ref TxnBlock TB, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"SELECT TOP 1 * FROM {0} WITH(INDEX(IDX_System_Gacha_Best_Date)) WHERE GETDATE() BETWEEN StartDate AND EndDate AND delflag = 'N' ORDER BY GachaIndex ", Shop_Define.Shop_Gacha_Best_TableName);
            System_Gacha_Best retObj = GenericFetch.FetchFromDB<System_Gacha_Best>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new System_Gacha_Best();
        }

        // calc gacha level from char level
        public static System_Gacha_Level GetSystem_Shop_Gacha_Level(ref TxnBlock TB, int level, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setKey = string.Format("{0}_{1}", Shop_Define.Shop_Gacha_Level_TableName, Shop_Define.Shop_Info_Surfix);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE Character_Level = {1}", Shop_Define.Shop_Gacha_Level_TableName, level);

            System_Gacha_Level retObj = GenericFetch.FetchFromRedis_Hash<System_Gacha_Level>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, level.ToString(), setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new System_Gacha_Level();
        }

        public static User_Gacha_Info GetUserGachaInfo(ref TxnBlock TB, long AID, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setKey = string.Format("{0}_{1}_{2}", Shop_Define.Shop_UserGacha_Info_TableName, Shop_Define.Shop_Info_Surfix, AID);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1}", Shop_Define.Shop_UserGacha_Info_TableName, AID);

            User_Gacha_Info retObj = GenericFetch.FetchFromRedis<User_Gacha_Info>(ref TB, DataManager_Define.RedisServerAlias_System, setKey, setQuery, dbkey, Flush);
            if (retObj == null)
            {
                setQuery = string.Format(@"INSERT INTO {0} (AID, TotalFree_GachaCount, Free_GachaCount, FreeGacha_regdate, TotalPremium_GachaCount, PremiumGacha_regdate)
										        VALUES ({1}, 0, 0, DATEADD(DAY, -1,GETDATE()), 0, DATEADD(DAY, -1,GETDATE()))", Shop_Define.Shop_UserGacha_Info_TableName, AID);
                TB.ExcuteSqlCommand(dbkey, setQuery);
                retObj = new User_Gacha_Info();
                retObj.AID = AID;
            }

            TimeSpan TS;
            DateTime FreeRegDate = DateTime.Parse(retObj.FreeGacha_regdate.ToShortDateString());
            DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            TS = FreeRegDate - dbDate;

            if (TS.Days != 0)
                retObj.Free_GachaCount = 0;

            return retObj;
        }

        public static User_Gacha_Special_Info GetUser_Gacha_Special_Info(ref TxnBlock TB, long AID, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE AID = {1}", Shop_Define.Shop_User_Gacha_Special_Info_TableName, AID);

            User_Gacha_Special_Info retObj = GenericFetch.FetchFromDB<User_Gacha_Special_Info>(ref TB, setQuery, dbkey);
            if (retObj == null)
            {
                retObj = new User_Gacha_Special_Info();
                retObj.AID = AID;
                retObj.Nomal_Gacha_regdate = retObj.Premium_Gacha_regdate = DateTime.Now;
            }
            return retObj;
        }

        // check gacha count
        public static Result_Define.eResult UpdateUserGachaCount(ref TxnBlock TB, long AID, bool isFree = true, string dbkey = Shop_Define.Shop_Info_DB)
        {
            SqlCommand commandUpdate_User_GachaCount = new SqlCommand();
            commandUpdate_User_GachaCount.CommandText = isFree ? "System_Update_User_FreeGacha" : "System_Update_User_PremiumGacha";
            var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            commandUpdate_User_GachaCount.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            commandUpdate_User_GachaCount.Parameters.Add(result);
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

            if (TB.ExcuteSqlStoredProcedure(dbkey, ref commandUpdate_User_GachaCount))
            {
                if (System.Convert.ToInt64(result.Value) < 0)
                {
                    long dbresult = System.Convert.ToInt64(result.Value) * -1;       // re delcare result code 
                    retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                }
                else
                    retError = Result_Define.eResult.SUCCESS;
            }
            else
                retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            commandUpdate_User_GachaCount.Dispose();
            return retError;
        }

        public static Result_Define.eResult UpdateUserPremiumGacha_Special_Info(ref TxnBlock TB, long AID, int currnetPremiumCount, int maxPremiumCount, int getPremiumSpecialCount, int addCount = 1, string dbkey = Shop_Define.Shop_Info_DB)
        {
            if (currnetPremiumCount + addCount >= maxPremiumCount)
            {
                getPremiumSpecialCount = getPremiumSpecialCount < Shop_Define.Shop_Gacha_Special_MaxGetGroup ? getPremiumSpecialCount + 1 : getPremiumSpecialCount;
                currnetPremiumCount = (currnetPremiumCount + addCount) - maxPremiumCount;
            }
            else
            {
                currnetPremiumCount = currnetPremiumCount + addCount;
            }

            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                Total_Premium_GachaCount = Total_Premium_GachaCount + {2},
	                                                Premium_GachaSpecialCount = {3},
                                                    Premium_GachaSpecialGetCount = {4},
	                                                Premium_Gacha_regdate = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ({1}, 0, 0, 0, GETDATE(), {2}, {3}, {4}, GETDATE());
                                    ", Shop_Define.Shop_User_Gacha_Special_Info_TableName
                                     , AID
                                     , addCount
                                     , currnetPremiumCount
                                     , getPremiumSpecialCount
                                     );
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        // update PremiumGacha special count
        public static Result_Define.eResult UpdateUserNormalGacha_Special_Info(ref TxnBlock TB, long AID, int currnetNormalCount, int maxNormalCount, int getNormalSpecialCount, int addNormalCount = 1, string dbkey = Shop_Define.Shop_Info_DB)
        {
            if (currnetNormalCount + addNormalCount >= maxNormalCount)
            {
                getNormalSpecialCount = getNormalSpecialCount < Shop_Define.Shop_Gacha_Special_MaxGetGroup ? getNormalSpecialCount + 1 : getNormalSpecialCount;
                currnetNormalCount = (currnetNormalCount + addNormalCount) - maxNormalCount;
            }
            else
            {
                currnetNormalCount = currnetNormalCount + addNormalCount;
            }

            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON aid = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                Total_Normal_GachaCount = Total_Normal_GachaCount + {2},
	                                                Nomal_GachaSpecialCount = {3},
                                                    Nomal_GachaSpeciaGetCount = {4},
	                                                Nomal_Gacha_regdate = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ({1}, {2}, {3}, {4}, GETDATE(), 0, 0, 0, GETDATE());
                                    ", Shop_Define.Shop_User_Gacha_Special_Info_TableName
                                     , AID
                                     , addNormalCount
                                     , currnetNormalCount
                                     , getNormalSpecialCount
                                     );
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        // insert billing request info
        public static Result_Define.eResult InsertUserBillingInfo(ref TxnBlock TB, long BuyGoodsID, long AID, long CID, int Buy_PriceValue, string Billing_ID, string Billing_Token, string Billing_Platform_UID, int BuyPlatform, Shop_Define.eBillingStatus setStatus = Shop_Define.eBillingStatus.Complete, string dbkey = Shop_Define.Shop_Info_DB)
        {
            SqlCommand CmdInsertBilling = new SqlCommand();
            CmdInsertBilling.CommandText = "User_Insert_Billing";
            CmdInsertBilling.Parameters.Add("@AID", SqlDbType.BigInt).Value = AID;
            CmdInsertBilling.Parameters.Add("@CID", SqlDbType.BigInt).Value = CID;
            CmdInsertBilling.Parameters.Add("@Buy_PriceValue", SqlDbType.BigInt).Value = Buy_PriceValue;
            CmdInsertBilling.Parameters.Add("@BuyGoodsID", SqlDbType.BigInt).Value = BuyGoodsID;
            CmdInsertBilling.Parameters.Add("@Billing_ID", SqlDbType.NVarChar).Value = Billing_ID;
            CmdInsertBilling.Parameters.Add("@Billing_Token", SqlDbType.NVarChar).Value = Billing_Token;
            CmdInsertBilling.Parameters.Add("@Billing_Platform_UID", SqlDbType.NVarChar).Value = string.IsNullOrEmpty(Billing_Platform_UID) ? "" : Billing_Platform_UID;
            CmdInsertBilling.Parameters.Add("@BuyPlatform", SqlDbType.Int).Value = BuyPlatform;
            CmdInsertBilling.Parameters.Add("@BuyStatus", SqlDbType.Int).Value = (int)setStatus;
            var result = new SqlParameter("@ret_result", SqlDbType.Int) { Direction = ParameterDirection.Output };
            CmdInsertBilling.Parameters.Add(result);

            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            if (TB.ExcuteSqlStoredProcedure(dbkey, ref CmdInsertBilling))
            {
                if (System.Convert.ToInt32(result.Value) < 0)
                    retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                else
                    retError = Result_Define.eResult.SUCCESS;
            }
            else
                retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            CmdInsertBilling.Dispose();
            return retError;
            //string setQuery = string.Format(@"INSERT INTO {0} VALUES('{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}',GETDATE())"
            //                                                        , Shop_Define.Shop_User_BillingInfo_TableName, AID, BuyGoodsID, Billing_ID, Billing_Token, Billing_Platform_UID, BuyPlatform, (int)setStatus);

            //return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        // check first pay event
        public static Result_Define.eResult CheckFirstPayEvent(ref TxnBlock TB, long AID, long CID, string dbkey = Shop_Define.Shop_Info_DB)
        {
            int admin_firstpayment_flag = SystemData.AdminConstValueFetchFromRedis(ref TB, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.ADMIN_FIRST_PAYMENT_ON_OFF]);
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            if (admin_firstpayment_flag > 0)
            {
                User_Event_Check_Data userDailyInfo = TriggerManager.Check_User_Daily_Event(ref TB, AID);

                List<User_Inven> makeRealItem = new List<User_Inven>();
                if (userDailyInfo.FirstPaymentFlag.Equals("N"))
                {
                    List<User_Billing_List> userBuy = ShopManager.GetUserBillingInfo_List(ref TB, AID, Shop_Define.eBillingStatus.Complete);

                    if (userBuy.Count > 0)
                        retError = TriggerManager.EventFirstPaymentRewardSend(ref TB, ref makeRealItem, AID, CID);
                    if (retError == Result_Define.eResult.SUCCESS)
                        userDailyInfo = TriggerManager.Check_User_Daily_Event(ref TB, AID, true);
                }
            }


            return retError;
        }

        public static long GetUserBillingCash(ref TxnBlock TB, long AID, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"SELECT SUM(Buy_PriceValue) as totalbuy FROM {0} WITH(NOLOCK, INDEX(IDX_User_Billing_List_AID)) WHERE AID = {1} AND Billing_Status = {2}",
                                                Shop_Define.Shop_User_BillingInfo_TableName, AID, (int)Shop_Define.eBillingStatus.Complete);
            User_TotalBuyPrice getValue = GenericFetch.FetchFromDB<User_TotalBuyPrice>(ref TB, setQuery, dbkey);

            if (getValue == null)
                return 0;

            return getValue.totalbuy;
        }

        // get billing all list info
        public static List<User_Billing_List> GetUserBillingInfo_List(ref TxnBlock TB, long AID, Shop_Define.eBillingStatus setStatus = Shop_Define.eBillingStatus.Complete, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK, INDEX(IDX_User_Billing_List_AID)) WHERE AID = {1} AND Billing_Status = {2}",
                                                Shop_Define.Shop_User_BillingInfo_TableName, AID, (int)setStatus);

            return GenericFetch.FetchFromDB_MultipleRow<User_Billing_List>(ref TB, setQuery, dbkey);
        }

        // get billing all UnResolve list info
        public static List<User_Billing_List> GetUserBillingInfo_UnResolveList(ref TxnBlock TB, long AID, Shop_Define.eBillingType buyPlatForm = Shop_Define.eBillingType.iOS_Appstore, Shop_Define.eBillingStatus setStatus = Shop_Define.eBillingStatus.CreateOrderID, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK, INDEX(IDX_User_Billing_List_AID)) WHERE AID = {1}  AND Billing_Status = {2} AND Buy_Platform = {3}",
                                                Shop_Define.Shop_User_BillingInfo_TableName, AID, (int)setStatus, (int)buyPlatForm);

            return GenericFetch.FetchFromDB_MultipleRow<User_Billing_List>(ref TB, setQuery, dbkey);
        }

        // get billing all UnResolve list info
        public static int GetUserBillingInfo_UnResolveCount(ref TxnBlock TB, long AID, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"SELECT COUNT(*) as count FROM {0} WITH(NOLOCK, INDEX(IDX_User_Billing_List_AID)) WHERE AID = {1}  AND Billing_Status = {2}",
                                                Shop_Define.Shop_User_BillingInfo_TableName, AID, (int)Shop_Define.eBillingStatus.CreateOrderID);
            User_GetShopCount retObj = GenericFetch.FetchFromDB<User_GetShopCount>(ref TB, setQuery, dbkey);
            return retObj == null ? 0 : retObj.count;
        }

        // get billing info
        public static User_Billing_List GetUserBillingInfo(ref TxnBlock TB, string Billing_ID, string Billing_Token, Shop_Define.eBillingStatus setStatus, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK, INDEX(IDX_User_Billing_List_Status)) WHERE Billing_Status = {1} AND Billing_ID = N'{2}' AND Billing_Token = N'{3}'",
                                                Shop_Define.Shop_User_BillingInfo_TableName, (int)setStatus, Billing_ID, Billing_Token);

            List<User_Billing_List> getList = GenericFetch.FetchFromDB_MultipleRow<User_Billing_List>(ref TB, setQuery, dbkey);

            User_Billing_List retObj = getList.Find(item => item.Billing_ID.Equals(Billing_ID) && item.Billing_Token.Equals(Billing_Token));
            return (retObj != null) ? retObj : new User_Billing_List();
        }

        // get billing info by billing id
        public static User_Billing_List GetUserBillingInfoByBillingID(ref TxnBlock TB, long AID, string Billing_ID, Shop_Define.eBillingStatus setStatus, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK, INDEX(IDX_User_Billing_List_AID)) WHERE Billing_Status = {1} AND AID = {2} AND Billing_ID = N'{3}'",
                                                Shop_Define.Shop_User_BillingInfo_TableName, (int)setStatus, AID, Billing_ID);

            List<User_Billing_List> getList = GenericFetch.FetchFromDB_MultipleRow<User_Billing_List>(ref TB, setQuery, dbkey);

            User_Billing_List retObj = getList.Find(item => Billing_ID.Equals(Billing_ID));
            return (retObj != null) ? retObj : new User_Billing_List();
        }

        // get billing info by billing id
        public static User_Billing_List GetUserBillingInfoByBillingID(ref TxnBlock TB, string Billing_ID, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK, INDEX(IDX_Unique_User_Billing_ID)) WHERE Billing_ID = N'{1}'",
                                                Shop_Define.Shop_User_BillingInfo_TableName, Billing_ID);

            //User_Billing_List getList = GenericFetch.FetchFromDB_MultipleRow<User_Billing_List>(ref TB, setQuery, dbkey);
            User_Billing_List retObj = GenericFetch.FetchFromDB<User_Billing_List>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new User_Billing_List();
        }



        // get billing info by billing token （---gyt）
        public static User_Billing_List GetUserBillingInfoByToken(ref TxnBlock TB, string Billing_Token, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"SELECT * FROM {0}  WHERE Billing_Token = N'{1}'",
                                                Shop_Define.Shop_User_BillingInfo_TableName, Billing_Token);

            //User_Billing_List getList = GenericFetch.FetchFromDB_MultipleRow<User_Billing_List>(ref TB, setQuery, dbkey);
            User_Billing_List retObj = GenericFetch.FetchFromDB<User_Billing_List>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new User_Billing_List();
        }



        // TODO : need r&d
        // get billing info by billing token (no index full scan)
        public static User_Billing_List GetUserBillingInfoByBillingToken(ref TxnBlock TB, long AID, string BillingToken, string dbkey = Shop_Define.Shop_Info_DB)
        {
            //string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE AID = {1} AND Billing_Token = N'{2}'",
            //                                    Shop_Define.Shop_User_BillingInfo_TableName, AID, BillingToken);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK) WHERE Billing_Token = N'{1}'",
                                                Shop_Define.Shop_User_BillingInfo_TableName, BillingToken);
            //User_Billing_List getList = GenericFetch.FetchFromDB_MultipleRow<User_Billing_List>(ref TB, setQuery, dbkey);
            User_Billing_List retObj = GenericFetch.FetchFromDB<User_Billing_List>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new User_Billing_List();
        }

        // update billing status
        public static Result_Define.eResult UpdateUserBillingStatus(ref TxnBlock TB, long billing_idx, Shop_Define.eBillingStatus setStatus, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET Billing_Status = {1} WHERE BillingIndex = {2}",
                                                    Shop_Define.Shop_User_BillingInfo_TableName, (int)setStatus, billing_idx);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        // change billing id
        public static Result_Define.eResult UpdateUserPlatformBillingID(ref TxnBlock TB, string PlatformBilling_ID, string Billing_ID, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET Platform_UID = N'{1}' WHERE Billing_ID = '{2}'",
                                                    Shop_Define.Shop_User_BillingInfo_TableName, PlatformBilling_ID, Billing_ID);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }


        public static Result_Define.eResult UpdateUserPlatformBillingToken(ref TxnBlock TB, string PlatformBilling_Token, long billing_idx, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET Billing_Token = N'{1}' WHERE BillingIndex = '{2}'",
                                                    Shop_Define.Shop_User_BillingInfo_TableName, PlatformBilling_Token, billing_idx);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

          // change Platformbilling id
        public static Result_Define.eResult UpdateUserBillingID(ref TxnBlock TB, long billing_idx, string Billing_ID, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"UPDATE {0} SET Billing_ID = N'{1}' WHERE BillingIndex = {2}",
                                                    Shop_Define.Shop_User_BillingInfo_TableName, Billing_ID, billing_idx);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }
      


        // update billing status
        public static Result_Define.eResult UpdateUserBillingError(ref TxnBlock TB, long billing_idx, Shop_Define.eBillingStatus setStatus, int errorcode, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON BillingIndex = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                Status = {2},
	                                                ErrorCode = {3},
	                                                regdate = GETDATE()
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ({1}, {2}, {3}, GETDATE());
                                    ", Shop_Define.Shop_User_BillingError_TableName
                                     , billing_idx
                                     , (int)setStatus
                                     , errorcode
                                     );

            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        // get subscription info
        private static User_Shop_Subscription GetUserSubscriptionInfo(ref TxnBlock TB, long AID, bool isWeek, bool Flush = false, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setKey = GetRediskey_UserSubscriptionInfo(AID, isWeek);
            string setQuery = string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE aid = {1}",
                                                isWeek ? Shop_Define.Shop_User_Shop_Subscription_Week_TableName : Shop_Define.Shop_User_Shop_Subscription_TableName, AID);
            User_Shop_Subscription retObj = GenericFetch.FetchFromRedis<User_Shop_Subscription>(ref TB, DataManager_Define.RedisServerAlias_User, setKey, setQuery, dbkey, Flush);
            return (retObj != null) ? retObj : new User_Shop_Subscription();
        }

        public static string GetRediskey_UserSubscriptionInfo(long AID, bool isWeek)
        {
            return string.Format("{0}_{1}_{2}"
                , isWeek ? Shop_Define.Shop_User_Shop_Subscription_Week_TableName : Shop_Define.Shop_User_Shop_Subscription_TableName
                , Shop_Define.Shop_Info_Surfix
                , AID);
        }
        public static void RemoveCacheUserSubscriptionInfo(long AID, bool isWeek)
        {
            string setKey = GetRediskey_UserSubscriptionInfo(AID, isWeek);
            TheSoul.DataManager.RedisConst.GetRedisInstance().RemoveObj(DataManager_Define.RedisServerAlias_User, setKey);
        }

        public static RetShopSubscription GetUserSubscriptionLeftDay(ref TxnBlock TB, long AID, bool isWeek)
        {
            User_Shop_Subscription userInfo = GetUserSubscriptionInfo(ref TB, AID, isWeek);

            TimeSpan TS;
            DateTime endDate = DateTime.Parse(userInfo.EndDate.ToShortDateString());
            DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            DateTime NextDate = new DateTime(dbDate.Year, dbDate.Month, DateTime.Now.AddDays(1).Day, 0, 0, 0);
            TS = endDate - dbDate;

            if (TS.Days > 0)
                return new RetShopSubscription(userInfo.Shop_Goods_ID, TS.Days);
            else
                return new RetShopSubscription();
        }

        public static Result_Define.eResult UpdateUserSubscription(ref TxnBlock TB, long AID, long ShopGoodsID, int setDay, bool isWeek, string dbkey = Shop_Define.Shop_Info_DB)
        {
            RetShopSubscription subscribeInfo = GetUserSubscriptionLeftDay(ref TB, AID, isWeek);

            bool bAddEnable = SystemData.GetServiceArea(ref TB) == DataManager_Define.eCountryCode.Taiwan;
            if (!bAddEnable)
            {
                if (subscribeInfo.left_day > 0 && !isWeek)
                    return Result_Define.eResult.SHOP_BILLING_SUBSCRIPTION_LEFT;
            }

            string setQuery = string.Empty;
            if (bAddEnable)
            {
                setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON AID = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                Shop_Goods_ID = {2},
	                                                StartDate = GETDATE(),
	                                                EndDate = DATEADD(DAY, {3}, {4})
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ({1}, {2}, GETDATE(), DATEADD(DAY, {3}, GETDATE()));
                                    ", isWeek ? Shop_Define.Shop_User_Shop_Subscription_Week_TableName : Shop_Define.Shop_User_Shop_Subscription_TableName
                                         , AID
                                         , ShopGoodsID
                                         , subscribeInfo.left_day + setDay
                                         , "GETDATE()"
                                         );
            }
            else
            {
                setQuery = string.Format(@"
                                                MERGE {0} USING (select 'X' as DUAL) AS B
                                                ON AID = {1}
                                                WHEN MATCHED THEN
                                                   UPDATE SET 
	                                                Shop_Goods_ID = {2},
	                                                StartDate = GETDATE(),
	                                                EndDate = DATEADD(DAY, {3}, {4})
                                                WHEN NOT MATCHED THEN
                                                   INSERT VALUES ({1}, {2}, GETDATE(), DATEADD(DAY, {3}, GETDATE()));
                                    ", isWeek ? Shop_Define.Shop_User_Shop_Subscription_Week_TableName : Shop_Define.Shop_User_Shop_Subscription_TableName
                         , AID
                         , ShopGoodsID
                         , setDay
                         , isWeek && subscribeInfo.left_day > 0 ? "EndDate" : "GETDATE()"
                         );

            }
            RemoveCacheUserSubscriptionInfo(AID, isWeek);
            return TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
        }

        public static Result_Define.eResult PayBuyPrice(ref TxnBlock TB, long AID, int BuyPrice, Item_Define.eItemBuyPriceType BuyPriceType)
        {
            if (BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_PayCash || BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_PayStack)
                return AccountManager.UseUserCash(ref TB, AID, BuyPrice);
            else if (BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_PayGold)
                return AccountManager.UseUserGold(ref TB, AID, BuyPrice);
            else if (BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_PayReal)
            {
                if (SystemData.GetServiceArea(ref TB) == DataManager_Define.eCountryCode.China)
                    BuyPrice = BuyPrice % Shop_Define.Shop_OverPricePrefix;
                return TriggerManager.ProgressTrigger(ref TB, AID, Trigger_Define.eTriggerType.Charge_Price, 0, 0, BuyPrice);
            }
            else if (BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_PayGuildPoint)
                return GuildManager.UseGuildContributionPoint(ref TB, AID, BuyPrice);
            else if (BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_PayExpeditionPoint)
                return AccountManager.UseUserExpeditionPoint(ref TB, AID, BuyPrice) == Result_Define.eResult.SUCCESS ? Result_Define.eResult.SUCCESS : Result_Define.eResult.GOLDEXPEDITION_NOT_ENOUGH_GEPOINT;
            else if (
                BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_HonorPoint
                || BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_PartyDungeonPoint
                || BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_CombatPoint
                || BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_PayGachaCoin
                || BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_PayMedal
                || BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_RankingPoint
                || BuyPriceType == Item_Define.eItemBuyPriceType.PriceType_BlackMarketPoint
                )
                return AccountManager.UseUserPoint(ref TB, AID, BuyPrice, BuyPriceType);
            else
                return Result_Define.eResult.SHOP_UNKNOWN_BUY_TYPE;
        }


        // create billing id
        public static Result_Define.eResult CreateBillingID(ref TxnBlock TB, long AID, long BuyGoodsID, Shop_Define.eBillingType BuyPlatform, string Product_ID, out string BillingID, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string auth_md5_id = string.Empty;
            BillingID = string.Empty;

            if (string.IsNullOrEmpty(Product_ID))
                return Result_Define.eResult.SHOP_ID_NOT_FOUND;

            using (MD5 md5Hash = MD5.Create())
            {
                string server_md5 = MD5Tool.GetMd5Hash(md5Hash, Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
                string client_md5 = MD5Tool.GetMd5Hash(md5Hash, Convert.ToBase64String(Guid.NewGuid().ToByteArray()));
                auth_md5_id = server_md5 + client_md5;
            }

            string setQuery = string.Format(@"INSERT INTO {0} VALUES('{1}', '{2}', '{3}', '{4}', '{5}', GETDATE())"
                                                                    , Shop_Define.Shop_User_Billing_AuthKey_TableName, auth_md5_id, AID, BuyGoodsID, (int)BuyPlatform, Product_ID);

            Result_Define.eResult retError = TB.ExcuteSqlCommand(dbkey, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;

            if (retError == Result_Define.eResult.SUCCESS)
                BillingID = auth_md5_id;

            return retError;
        }

        public static User_Billing_AuthKey GetBillingID(ref TxnBlock TB, long AID, string BillingID, string Product_ID, string dbkey = Shop_Define.Shop_Info_DB)
        {
            string setQuery = String.IsNullOrEmpty(BillingID) ?
                string.Format(@"SELECT TOP 1 * FROM {0} WITH(NOLOCK) WHERE AID = {1} AND Product_ID = '{2}' ORDER BY regdate DESC", Shop_Define.Shop_User_Billing_AuthKey_TableName, AID, Product_ID) :
                string.Format(@"SELECT * FROM {0} WITH(NOLOCK)  WHERE BillingAuthKey = '{1}' AND AID = {2}", Shop_Define.Shop_User_Billing_AuthKey_TableName, BillingID, AID);
            User_Billing_AuthKey retObj = GenericFetch.FetchFromDB<User_Billing_AuthKey>(ref TB, setQuery, dbkey);
            return (retObj != null) ? retObj : new User_Billing_AuthKey();
        }
    }
}