using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using mSeed.mDBTxnBlock;
using mSeed.RedisManager;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using ServiceStack.Text;

namespace TheSoulWebServer
{
    public partial class RequestPrivateServer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                /*
                 op list
                 */
                "server_config",
                "get_user_aid",
                "get_user_platform_id",
                "account_switch",
                "account_drop",
                "set_user_restrict",
                "search_nickname",

                "get_unresolve_buy_list",
                "billing_progress",
                "change_billing_id",
                "billing_status_change",

                "use_account_ticket",
                "use_account_ruby",
                "add_account_point",
                "use_account_point",
                "party_dungeon_result",
                "add_guild_point",
                //"overlord_result",   // 패왕의 길 경험치 계산 // not use yet

                // game user info operation
                "user_full_info",
                "account_info",
                "character_list",
                "character_info",
                "character_group",
                "equip_item_list",
                "equip_soul_list",
                "equip_active_soul_list",
                "equip_passive_soul_list",
                "pvp_info",
                "use_contributionpoint",
                "addcontributionpoint",
                
                //reset count
                "pvp_count_reset",
                "pvp_count_info",

                // pvp opentime
                "pvp_open_time",

                // trigger_progress
                "trigger_progress",

                // make item or mail requeset
                "item_make",
                "send_mail",

                // regist snail UID
                "reg_snail_id",
                // regist snail cdkey
                "reg_snail_cdkey",

                // guild level, attend count
                "guild_info",

                // chracter warpoint
                "get_warpoint",
                "set_warpoint",

                // load system data 
                "get_system_data",
                "get_system_data_value",
                "get_admin_data_value",

                // pvp operation
                "get_pvp_record",
                "get_pvp_record_all",
                "set_pvp_count",
                "set_pvp_record",

                "pvp_guildwar_join",

                "get_ruby_pvp_player_info",
                "get_bot_pvp_info",
                "set_bot_pvp_result",

                // for web request
                "redis_flush",
                "set_chatchannel",

                // Friends info list
                "get_friends_info",
                
                //coupon
                "coupon_send_mail",
            };

            // set Debug mode true, internal request do not need encrypt yet.
            WebQueryParam queryFetcher = new WebQueryParam(true);
            string retJson = "";

            TxnBlock tb = new TxnBlock();
            {
                long AID = 0;
                try
                {
                    queryFetcher.TxnBlockInit(ref tb, ref AID);

                    string requestOp = queryFetcher.QueryParam_Fetch("op");
                    JsonObject json = new JsonObject();

                    if (!TheSoulDBcon.bIptable)
                        TheSoulDBcon.Snail_ips = GlobalManager.GetSnailIPList(ref tb);

                    string ipfirst = queryFetcher.ipaddress.Split('.')[0];
                    //json = mJsonSerializer.AddJson(json, "ipfirst", mJsonSerializer.ToJsonString(ipfirst));

                    DataManager_Define.eCountryCode serviceArea = SystemData.GetServiceArea(ref tb);
                    bool bIPPass = (serviceArea == DataManager_Define.eCountryCode.China || serviceArea == DataManager_Define.eCountryCode.Taiwan || serviceArea == DataManager_Define.eCountryCode.International) ? 
                                        true :
                                            ((ipfirst.Equals("::1") || ipfirst.Equals("10") || ipfirst.Equals("127") || ipfirst.Equals("172") || ipfirst.Equals("192"))
                                            || TheSoulDBcon.Snail_ips.FindAll(ipaddr => ipaddr.ip_address.Equals(queryFetcher.ipaddress)).Count > 0)
                        ;
                    //json = mJsonSerializer.AddJson(json, "ipaddrs", mJsonSerializer.ToJsonString(TheSoulDBcon.Snail_ips));
                    //json = mJsonSerializer.AddJson(json, "callip", mJsonSerializer.ToJsonString(queryFetcher.ipaddress));
                    //json = mJsonSerializer.AddJson(json, "bIPPass", mJsonSerializer.ToJsonString(bIPPass));

                    if (Array.IndexOf(ops, requestOp) >= 0 && bIPPass)
                    {
                        queryFetcher.operation = requestOp;
                        Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                        long CID = queryFetcher.QueryParam_FetchLong("cid");
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.op], requestOp);
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], AID);

                        if (requestOp.Equals("server_config"))
                        {
                            TheSoulDBcon.GetInstance().TheSoulGlobalDBInit(ref tb);
                            List<server_config> serverList = GlobalManager.GetServerList(ref tb);
                            List<server_group_config> serverGroupList = GlobalManager.GetServerGroupList(ref tb);
                            List<snail_ip_table> ipTable = GlobalManager.GetSnailIPList(ref tb);
                            json = mJsonSerializer.AddJson(json, Global_Define.RetKey_GlobalServerGroupList, mJsonSerializer.ToJsonString(serverGroupList));
                            json = mJsonSerializer.AddJson(json, Global_Define.RetKey_GlobalServerList, mJsonSerializer.ToJsonString(serverList));
                            json = mJsonSerializer.AddJson(json, Global_Define.RetKey_GlobalDevIPList, mJsonSerializer.ToJsonString(ipTable));
                        }
                        else if (requestOp.Equals("get_user_aid"))
                        {
                            Global_Define.ePlatformType platform_type = (Global_Define.ePlatformType)queryFetcher.QueryParam_FetchInt("platform_type");
                            int billingType = queryFetcher.QueryParam_FetchInt("billing_type");
                            string platform_id = queryFetcher.QueryParam_Fetch("user_id");
                            int versionNo = queryFetcher.QueryParam_FetchInt("version");
                            int server_group_id = queryFetcher.QueryParam_FetchInt("server_group_id");
                            long accID = queryFetcher.QueryParam_FetchLong("acc_id", 0);
                            string old_platform_id = platform_id;
                            bool bCheckAccID = platform_type != Global_Define.ePlatformType.EPlatformType_UnityEditer && accID > 0;
                            bool bUpdate = false;
                            if (bCheckAccID)
                            {
                                user_platform_id platformInfo = GlobalManager.GetUserPlatformInfo(ref tb, accID);
                                if (!platform_id.Equals(platformInfo.platform_user_id))
                                {
                                    old_platform_id = string.IsNullOrEmpty(platformInfo.platform_user_id) ? platform_id : platformInfo.platform_user_id;
                                    bUpdate = true;
                                }
                            }
                            
                            long retAID = AccountManager.GlobalGet_UAID(ref tb, ref old_platform_id, platform_type);
                            string setEncryptKey = "";
                            long loginRestrictTime = 0, chatRestrictTime = 0;

                            if (bUpdate && bCheckAccID && retAID > 0 && platform_type >= Global_Define.ePlatformType.EPlatformType_UnityEditer)
                            {
                                retError = AccountManager.GlobalPlatformID_Update(ref tb, platform_id, old_platform_id, retAID, accID);
                            }
                            else if (platform_type >= Global_Define.ePlatformType.EPlatformType_UnityEditer)
                                platform_id = old_platform_id;
                            else
                                retError = Result_Define.eResult.ACCOUNT_PLATFORM_INVALIDE;

                            int checkUnResolveBuyCount = 0;
                            if (retAID > 0 
                                && retError == Result_Define.eResult.SUCCESS
                                && platform_type != (int)Global_Define.ePlatformType.EPlatformType_UnityEditer
                                    && (   billingType == (int)Shop_Define.eBillingType.Kr_aOS_Google
                                        || billingType == (int)Shop_Define.eBillingType.Kr_aOS_OneStore
                                        || billingType == (int)Shop_Define.eBillingType.Kr_iOS_Appstore
                                        || billingType == (int)Shop_Define.eBillingType.Global_aOS_Google
                                        || billingType == (int)Shop_Define.eBillingType.Global_iOS_Appstore
                                        || billingType == (int)Shop_Define.eBillingType.Global_aOS_MOL
                                        || billingType == (int)Shop_Define.eBillingType.Global_iOS_MOL
                                        )
                                )
                            {
                                checkUnResolveBuyCount = ShopManager.GetUserBillingInfo_UnResolveCount(ref tb, retAID);
                            }

                            if (retAID > 0 && retError == Result_Define.eResult.SUCCESS)
                            {
                                setEncryptKey = TheSoulEncrypt.CreateKey_IV();
                                retError = EncryptManager.SetUser_Encrypte(ref tb, retAID, setEncryptKey, Encrypt_Define.eLogLevel.DetailDBLog);

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    user_account_restrict restrictInfo = GlobalManager.GetUserRestrict(ref tb, retAID);
                                    TimeSpan loginRestrict= restrictInfo.login_restrict_enddate - DateTime.Now;
                                    TimeSpan chatRestrict = restrictInfo.chat_restrict_endate - DateTime.Now;
                                    loginRestrictTime = loginRestrict.TotalSeconds > 0 ? (long)loginRestrict.TotalSeconds : 0;
                                    chatRestrictTime = chatRestrict.TotalSeconds > 0 ? (long)chatRestrict.TotalSeconds : 0;
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                /*
                                 * Json issue of conversion.
                                 * 1) Only when setting the text field number
                                 * 2) 'text field' change to 'numeric field'
                                 * Temporary solution for the problem
                                 */
                                ret_user_aid retaid = new ret_user_aid();
                                retaid.aid = retAID;
                                retaid.userid = platform_id;
                                retaid.encryptkey = setEncryptKey;
                                retaid.loginrestrict = loginRestrictTime;
                                retaid.chatrestrict = chatRestrictTime;
                                retaid.unresolve_count = checkUnResolveBuyCount;
                                retaid.operation = requestOp;
                                retaid.resultcode = (int)retError;

                                queryFetcher.Render(mJsonSerializer.ToJsonString(retaid), retError, false);

                                //json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.AID], retAID.ToString());
                                //json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.UserID], platform_id);
                                //json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.EncryptKey], setEncryptKey);

                                //json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.LoginRestrict], loginRestrictTime.ToString());
                                //json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.ChatRestrict], chatRestrictTime.ToString());
                                //json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetUnResolveBuyCount], checkUnResolveBuyCount.ToString());
                            }
                        }
                        else if (requestOp.Equals("get_user_platform_id"))
                        {
                            retError = AID > 1 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.ACCOUNT_ID_NOT_FOUND;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                user_account_config userConfig = GlobalManager.GetUserAccountConfig(ref tb, AID);
                                user_platform_id userPlatformInfo = GlobalManager.GetUserPlatformInfo_ByPlatformID(ref tb, userConfig.platform_user_id);

                                //if (userPlatformInfo.platform_idx < 1)
                                //    retError = Result_Define.eResult.ACCOUNT_PLATFORM_INFO_NOT_FOUND;

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.AID], userConfig.user_account_idx.ToString());
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.PlatformType], userConfig.platform_type.ToString());
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.PlatformUserID], userPlatformInfo.platform_user_id);
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.PlatformUID], userPlatformInfo.platform_idx.ToString());
                                }
                            }
                        }
                        else if (requestOp.Equals("account_switch"))
                        {
                            string beforeID = queryFetcher.QueryParam_Fetch("before_platform_id");
                            Global_Define.ePlatformType before_platform_type = (Global_Define.ePlatformType)queryFetcher.QueryParam_FetchInt("before_platform_type", (int)Global_Define.ePlatformType.EPlatformType_Guest);
                            string afterID = queryFetcher.QueryParam_Fetch("after_platform_id");
                            //Global_Define.ePlatformType after_platform_type = (Global_Define.ePlatformType)queryFetcher.QueryParam_FetchInt("after_platform_type"); 
                            Global_Define.ePlatformType platform_type = (Global_Define.ePlatformType)queryFetcher.QueryParam_FetchInt("after_platform_type");
                            bool bForced = queryFetcher.QueryParam_FetchInt("forced") > 0;

                            user_account_config beforePlatformInfo = GlobalManager.GetUserAccountConfigByPlatformID(ref tb, beforeID, before_platform_type);

                            retError = AID != beforePlatformInfo.user_account_idx? Result_Define.eResult.ACCOUNT_ID_NOT_MATCHED : Result_Define.eResult.SUCCESS;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                user_account_config afterPlatformInfo = GlobalManager.GetUserAccountConfigByPlatformID(ref tb, afterID, platform_type);
                                if (afterPlatformInfo.user_account_idx > 0)
                                    retError = Result_Define.eResult.ACCOUNT_ID_ALREADY_LINKED;

                                if (retError == Result_Define.eResult.ACCOUNT_ID_ALREADY_LINKED && bForced)
                                    retError = AccountManager.GlobalAccountDrop(ref tb, afterPlatformInfo.user_account_idx, afterPlatformInfo.platform_user_id, (Global_Define.ePlatformType)afterPlatformInfo.platform_type);

                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = AccountManager.mSeedPlatformID_Update(ref tb, AID, afterID, platform_type);
                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.ACCOUNT_REGIST);
                            }
                            //user_platform_id userPlatformInfo = GlobalManager.GetUserPlatformInfo_ByPlatformID(ref tb, userConfig.platform_user_id);
                        }
                        else if (requestOp.Equals("account_drop"))
                        {
                            user_account_config userinfo = GlobalManager.GetUserAccountConfig(ref tb, AID);
                            retError = AccountManager.GlobalAccountDrop(ref tb, AID, userinfo.platform_user_id, (Global_Define.ePlatformType)userinfo.platform_type);
                        }
                        else if (requestOp.Equals("search_nickname"))
                        {
                            string searchName = System.Convert.ToString(queryFetcher.QueryParam_Fetch("user_name"));
                            Account_Simple_With_Connection findUserInfo = FriendManager.SearchFriend(ref tb, searchName);
                            json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.AID], findUserInfo.aid.ToString());
                        }
                        else if (requestOp.Equals("set_user_restrict"))
                        {
                            Global_Define.eAccountRestrictType restrictType = (Global_Define.eAccountRestrictType)queryFetcher.QueryParam_FetchInt("restrict_type");
                            int restrictTime = queryFetcher.QueryParam_FetchInt("restrict_time");

                            retError = GlobalManager.SetUserRestrict(ref tb, AID, restrictTime, restrictType);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                long loginRestrictTime = 0, chatRestrictTime = 0;
                                user_account_restrict restrictInfo = GlobalManager.GetUserRestrict(ref tb, AID);
                                TimeSpan loginRestrict = restrictInfo.login_restrict_enddate - DateTime.Now;
                                TimeSpan chatRestrict = restrictInfo.chat_restrict_endate - DateTime.Now;
                                loginRestrictTime = loginRestrict.TotalSeconds > 0 ? (long)loginRestrict.TotalSeconds : 0;
                                chatRestrictTime = chatRestrict.TotalSeconds > 0 ? (long)chatRestrict.TotalSeconds : 0;
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.LoginRestrict], loginRestrictTime.ToString());
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.ChatRestrict], chatRestrictTime.ToString());
                            }
                        }
                        else if (requestOp.Equals("get_unresolve_buy_list"))
                        {
                            Shop_Define.eBillingType billingType = (Shop_Define.eBillingType)queryFetcher.QueryParam_FetchInt("billing_type");

                            List<User_Billing_List> getUserBuyList = ShopManager.GetUserBillingInfo_UnResolveList(ref tb, AID, billingType);
                            Shop_Define.eShopType ShopType = Shop_Define.eShopType.Billing;
                            Object ShopGoods = ShopManager.GetSystem_ShopList(ref tb, ShopType, billingType);
                            List<System_Shop_Goods> setList = ((List<System_Shop_Goods>)ShopGoods);
                            List<Ret_User_Billing_List> retUserBuyList = new List<Ret_User_Billing_List>();

                            List<System_Package_List> getPackageList = ShopManager.GetShop_System_Package_List(ref tb, billingType, true, true);
                            string setProductID = string.Empty;
                            foreach (User_Billing_List getObj in getUserBuyList)
                            {
                                System_Package_List pickPackage = getPackageList.Find(item => item.Package_ID == getObj.Shop_Goods_ID);
                                bool isCheap = (pickPackage == null);
                                if (isCheap)
                                {
                                    getPackageList = ShopManager.GetShop_System_Package_Cheap_List(ref tb, billingType, true);
                                    pickPackage = getPackageList.Find(item => item.Package_ID == getObj.Shop_Goods_ID);
                                    isCheap = (pickPackage != null);
                                }

                                if (getObj.Shop_Goods_ID > 0 && pickPackage == null)
                                {
                                    System_Shop_Goods PickItem = setList.Find(item => item.Shop_Goods_ID == getObj.Shop_Goods_ID);
                                    if (PickItem != null)
                                    {
                                        if (serviceArea == DataManager_Define.eCountryCode.China)
                                        {
                                            if (Shop_Define.Shop_Billing_ProductID.TryGetValue(PickItem.Buy_PriceValue, out setProductID))
                                            {
                                                retUserBuyList.Add(new Ret_User_Billing_List(getObj, setProductID));
                                            }
                                            else if (!string.IsNullOrEmpty(pickPackage.Product_ID))
                                            {
                                                retUserBuyList.Add(new Ret_User_Billing_List(getObj, pickPackage.Product_ID));
                                            }
                                        }
                                        else if (!string.IsNullOrEmpty(PickItem.Product_ID))
                                        {
                                            retUserBuyList.Add(new Ret_User_Billing_List(getObj, PickItem.Product_ID));
                                        }
                                    }
                                    else
                                    {
                                        getObj.Billing_Status = (int)Shop_Define.eBillingStatus.Error;

                                        retError = ShopManager.UpdateUserBillingStatus(ref tb, getObj.BillingIndex, Shop_Define.eBillingStatus.Error);
                                        if (retError == Result_Define.eResult.SUCCESS)
                                            retError = ShopManager.UpdateUserBillingError(ref tb, getObj.BillingIndex, Shop_Define.eBillingStatus.Error, Shop_Define.shopBillingError);
                                    }
                                }
                                else if (getObj.Shop_Goods_ID > 0 && pickPackage != null)   // for package list
                                {
                                    if (serviceArea == DataManager_Define.eCountryCode.China)
                                    {
                                        if (Shop_Define.Shop_Billing_ProductID.TryGetValue(pickPackage.Buy_PriceValue, out setProductID))
                                        {
                                            retUserBuyList.Add(new Ret_User_Billing_List(getObj, setProductID));
                                        }
                                        else if (!string.IsNullOrEmpty(pickPackage.Product_ID))
                                        {
                                            retUserBuyList.Add(new Ret_User_Billing_List(getObj, pickPackage.Product_ID));
                                        }
                                    }
                                    else if (!string.IsNullOrEmpty(pickPackage.Product_ID))
                                    {
                                        retUserBuyList.Add(new Ret_User_Billing_List(getObj, pickPackage.Product_ID));
                                    }
                                }
                            }

                            json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetUnResolveList], mJsonSerializer.ToJsonString(retUserBuyList));
                        }
                        else if (requestOp.Equals("billing_progress"))
                        {
                            //tb.IsoLevel = IsolationLevel.ReadCommitted;
                            string Billing_ID = queryFetcher.QueryParam_Fetch("billing_id", "");
                            string Billing_Token = queryFetcher.QueryParam_Fetch("billing_token", "");

                            User_Billing_List getObj;
                          
                            if (string.IsNullOrEmpty(Billing_ID))
                            {
                                getObj = ShopManager.GetUserBillingInfoByToken(ref tb, Billing_Token);
                            }
                            else
                            {
                                getObj = ShopManager.GetUserBillingInfoByBillingID(ref tb, Billing_ID);
                            }

                            //User_Billing_List getObj = ShopManager.GetUserBillingInfo(ref tb, Billing_ID, Billing_Token, Shop_Define.eBillingStatus.CreateOrderID);
                            //User_Billing_List getObj = ShopManager.GetUserBillingInfoByBillingID(ref tb, Billing_ID);
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
                            int BuyPriceValue = 0;
                            int MaxBuy = 0;
                            int BuyGroupID = 0;
                            long ShopGoodsID = getObj.Shop_Goods_ID;

                            float MultipleValue = 1.0f;
                            float zhekouValue = 1.0f;

                            //switch ((Shop_Define.eBillingType)getObj.Buy_Platform)
                            //{
                            //    case Shop_Define.eBillingType.Kr_aOS_Google:
                            //        retError = GoogleJsonWebToken.GoogleIABVerify(getObj.Billing_Token);
                            //        break;
                            //    default:
                            //        retError = Result_Define.eResult.SUCCESS;
                            //        break;
                            //}
                            retError = Result_Define.eResult.SUCCESS;

                            if (getObj.BillingIndex > 0 && getObj.Billing_Status == (int)Shop_Define.eBillingStatus.Complete)
                            {
                                retError = Result_Define.eResult.SHOP_BILLING_ALREADY_COMPLETE;
                            }
                            else if (getObj.BillingIndex > 0 && getObj.Shop_Goods_ID > 0 && pickPackage == null && retError == Result_Define.eResult.SUCCESS)
                            {
                                Shop_Define.eShopType ShopType = Shop_Define.eShopType.Billing;
                                List<System_Shop_Goods> ShopGoods = ShopManager.GetSystem_ShopList(ref tb, ShopType, (Shop_Define.eBillingType)getObj.Buy_Platform);
                                //List<System_Shop_Goods> setList = ((List<System_Shop_Goods>)ShopGoods);
                                System_Shop_Goods PickItem = ShopGoods.Find(item => item.Shop_Goods_ID == getObj.Shop_Goods_ID);
                                if (PickItem != null)
                                {
                                    List<User_Inven> makeItems = new List<User_Inven>();
                                    int makeCount = PickItem.ItemNum < 0 ? Shop_Define.Shop_UnlimitDay : PickItem.ItemNum;  // for subscription item


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
                                            zhekouValue = PickItem.Discount / 100.0f;
                                        }
                                    }

                                    if (PickItem.ItemID > 0)
                                        getRewardList.Add(new System_Package_RewardBox(PickItem.ItemID, (int)(makeCount * MultipleValue)));
                                    if (PickItem.Bonus_ItemID > 0 && retError == Result_Define.eResult.SUCCESS)
                                    {
                                        makeCount = PickItem.ItemNum < 0 ? Shop_Define.Shop_UnlimitDay : PickItem.Bonus_ItemNum;  // for subscription item
                                        getRewardList.Add(new System_Package_RewardBox(PickItem.Bonus_ItemID, makeCount));
                                    }

                                    VipExp = PickItem.VIP_Point;
                                    BuyPriceValue = (int)(PickItem.Buy_PriceValue * zhekouValue);
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

                            if (serviceArea == DataManager_Define.eCountryCode.China)
                                BuyPriceValue = BuyPriceValue % Shop_Define.Shop_OverPricePrefix;

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = ShopManager.PayBuyPrice(ref tb, AID, BuyPriceValue, Item_Define.eItemBuyPriceType.PriceType_PayReal);

                            json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.AID], mJsonSerializer.ToJsonString(AID));

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetRuby], mJsonSerializer.ToJsonString(userAccount.Cash + userAccount.EventCash));
                                    //json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.AID], mJsonSerializer.ToJsonString(userAccount.AID));
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                                    json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetItemID], getObj.Shop_Goods_ID.ToString());
                                }
                            }
                        }
                        else if (requestOp.Equals("change_billing_id"))// iOS 정품 빌링시, iOS 결제가 끝나고 난 후 CS에 전송된 결제정보로, buy_billing_cash 를 통해 임시 발급됐던 billing_id를 실제 billing_id로 교체함. 
                        {
                            //tb.IsoLevel = IsolationLevel.ReadCommitted;
                            string Billing_ID = queryFetcher.QueryParam_Fetch("billing_id", "");
                            string New_Billing_ID = queryFetcher.QueryParam_Fetch("new_billing_id", "");

                            if (null == New_Billing_ID || 0 == New_Billing_ID.Length)
                                retError = Result_Define.eResult.FAIL_CHANGE_BILLING_ID;
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                User_Billing_List getObj = ShopManager.GetUserBillingInfoByBillingID(ref tb, AID, Billing_ID, Shop_Define.eBillingStatus.CreateOrderID);

                                if (getObj.Shop_Goods_ID > 0)
                                {
                                    AID = getObj.AID;
                                    retError = ShopManager.UpdateUserBillingID(ref tb, getObj.BillingIndex, New_Billing_ID);
                                }
                                else
                                {
                                    // 에러지만 이곳은 앱스토어 결제가 진행된 뒤에 ChargeSDK 확인전에 오기 때문에 실패해선 안됨. 실패정보가 없다면 넣어준다.
                                    string Billing_Platform_UID = queryFetcher.QueryParam_Fetch("billing_uid", "");
                                    long BuyGoodsID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("shop_item_id"));
                                    int BuyPlatform = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("billing_platform", "0"));

                                    retError = ShopManager.InsertUserBillingInfo(ref tb, BuyGoodsID, AID, CID, 0, New_Billing_ID, New_Billing_ID, Billing_Platform_UID, BuyPlatform, Shop_Define.eBillingStatus.CreateOrderID);
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                json = mJsonSerializer.AddJson(json, Shop_Define.Shop_Ret_KeyList[Shop_Define.eShopReturnKeys.RetBillingID], New_Billing_ID);
                            }
                        }
                        else if (requestOp.Equals("billing_status_change"))
                        {
                            string Billing_ID = queryFetcher.QueryParam_Fetch("billing_id", "");
                            int errorcode = queryFetcher.QueryParam_FetchInt("errorcode", Shop_Define.shopBillingError);

                            User_Billing_List getObj = ShopManager.GetUserBillingInfoByBillingID(ref tb, AID, Billing_ID, Shop_Define.eBillingStatus.CreateOrderID);
                            if (getObj.Shop_Goods_ID > 0 && errorcode >= 0)
                            {
                                int setStatus = errorcode > 0 ? (int)Shop_Define.eBillingStatus.Error : queryFetcher.QueryParam_FetchInt("status", (int)Shop_Define.eBillingStatus.CreateOrderID);
                                retError = ShopManager.UpdateUserBillingStatus(ref tb, getObj.BillingIndex, (Shop_Define.eBillingStatus)setStatus);
                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = ShopManager.UpdateUserBillingError(ref tb, getObj.BillingIndex, (Shop_Define.eBillingStatus)setStatus, errorcode);
                            }
                        }
                        else if (requestOp.Equals("use_account_ticket"))
                        {
                            //tb.IsoLevel = IsolationLevel.ReadCommitted;
                            int ticket_count = queryFetcher.QueryParam_FetchInt("ticket_count", 0);
                            bool isplay = queryFetcher.QueryParam_FetchInt("isplay", 0) > 0;
                            PvP_Define.ePvPType pvpType = (PvP_Define.ePvPType)queryFetcher.QueryParam_FetchInt("pvp_type", (int)PvP_Define.ePvPType.MATCH_NONE);

                            int PvPStartTime = 0, PvPEndTime = 0, PvPBonusStartTime = 0, PvPBonusEndTime = 0, UserRating = 0; // RejoinLeftTime = 0;
                            int TodaySecond = PvPManager.GetTodayTotalSeconds();
                            retError = Result_Define.eResult.SUCCESS;

                            List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                            if (pvpType == PvP_Define.ePvPType.MATCH_1VS1)
                            {
                                PvPStartTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_1VS1REAL_START_TIME]);
                                PvPEndTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_1VS1REAL_END_TIME]);

                                PvPBonusStartTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_1VS1REAL_START_TIME_BONUS]);
                                PvPBonusEndTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_1VS1REAL_END_TIME_BOUNS]);
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.PvPOperationSID.MATCH_1VS1);
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], (long)SnailLog_Define.PvPOperationSID.MATCH_1VS1);
                            }
                            else if (pvpType == PvP_Define.ePvPType.MATCH_FREE)
                            {
                                PvPStartTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_FREEFORALL_START_TIME_1st]);
                                PvPEndTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_FREEFORALL_END_TIME_1st]);

                                PvPBonusStartTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_FREEFORALL_START_TIME_2nd]);
                                PvPBonusEndTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_FREEFORALL_END_TIME_2nd]);
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.PvPOperationSID.MATCH_FREE);
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], (long)SnailLog_Define.PvPOperationSID.MATCH_FREE);
                            }
                            else if (pvpType == PvP_Define.ePvPType.MATCH_GUILD_3VS3)
                            {
                                PvPStartTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_START_TIME_1st]);
                                PvPEndTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_END_TIME_1st]);

                                PvPBonusStartTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_START_TIME_2nd]);
                                PvPBonusEndTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_END_TIME_2nd]);

                                long userGID = GuildManager.GetGuildInfo(ref tb, AID).guild_id;
                                Guild_User_PVP_Record getInfo = PvPManager.GetGuild_User_PvP_Record(ref tb, AID, userGID, PvP_Define.PvP_GuildUser_Monthly_TableName);
                                UserRating = getInfo.rating_point;

                                int setCoolTime = SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_GUILD_G3VS3_COOLTIME_MINUTE]);
                                DateTime checkTime = getInfo.lastjoin_date.AddMinutes(setCoolTime);
                                TimeSpan TS = checkTime - DateTime.Now;

                                if (TS.TotalSeconds > 0)
                                    retError = Result_Define.eResult.PVP_GUILD_REJOIN_TIME_LEFT;
                                else if (ticket_count > 0)
                                    retError = PvPManager.SetGuild_User_PvP_Record_JoinSet(ref tb, getInfo, PvP_Define.PvP_GuildUser_Monthly_TableName, setCoolTime);

                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.PvPOperationSID.MATCH_GUILD_3VS3);
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], (long)SnailLog_Define.PvPOperationSID.MATCH_GUILD_3VS3);
                            }
                            else if (pvpType == PvP_Define.ePvPType.MATCH_RUBY_PVP)
                            {
                                PvPStartTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_GLADIATOR_START_TIME_1st]);
                                PvPEndTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_GLADIATOR_END_TIME_1st]);
                                PvPBonusStartTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_GLADIATOR_START_TIME_2nd]);
                                PvPBonusEndTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_GLADIATOR_END_TIME_2nd]);

                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.PvPOperationSID.MATCH_RUBY_PVP);
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], (long)SnailLog_Define.PvPOperationSID.MATCH_RUBY_PVP);
                            }
                            else if (pvpType == PvP_Define.ePvPType.MATCH_PARTY)
                            {
                                PvPStartTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_PARTY_START_TIME_1st]);
                                PvPEndTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_PARTY_END_TIME_1st]);
                                PvPBonusStartTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_PARTY_START_TIME_2nd]);
                                PvPBonusEndTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_PARTY_END_TIME_2nd]);

                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.PvPOperationSID.MATCH_PARTY);
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], (long)SnailLog_Define.PvPOperationSID.MATCH_PARTY);
                                //int dungeonID = queryFetcher.QueryParam_FetchInt("map_index");
                                //setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_Party, dungeonID));
                                //setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_Party_First, dungeonID));
                                //retError = TriggerManager.ProgressTrigger(ref tb, AID, setDataList);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                bool bOpen = (PvPStartTime >= PvPEndTime && TodaySecond > PvPStartTime)
                                                || (PvPStartTime < PvPEndTime && TodaySecond > PvPStartTime && TodaySecond < PvPEndTime)
                                                || (PvPBonusStartTime > PvPBonusEndTime && TodaySecond > PvPBonusStartTime)
                                                || (PvPBonusStartTime < PvPBonusEndTime && TodaySecond > PvPBonusStartTime && TodaySecond < PvPBonusEndTime);
                                retError = bOpen ? Result_Define.eResult.SUCCESS : Result_Define.eResult.PVP_PLAYTIME_CLOSED;
                            }

                            int key = 0; int keyfillmax = 0; int keyremain = 0;

                            if (ticket_count > 0 && retError == Result_Define.eResult.SUCCESS)
                                retError = AccountManager.UseUserTicket(ref tb, AID, ticket_count);

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = AccountManager.TicketSpendCharge(ref tb, AID, 0, ref key, ref keyremain, ref keyfillmax);

                            if (retError == Result_Define.eResult.SUCCESS & isplay)
                            {
                                Trigger_Define.ePvPType setTriggerType = Trigger_Define.ePvPType.NONE;
                                switch (pvpType)
                                {
                                    case PvP_Define.ePvPType.MATCH_1VS1: setTriggerType = Trigger_Define.ePvPType.MATCH_1VS1; break;
                                    case PvP_Define.ePvPType.MATCH_FREE: setTriggerType = Trigger_Define.ePvPType.MATCH_FREE; break;
                                    case PvP_Define.ePvPType.MATCH_GUILD_3VS3: setTriggerType = Trigger_Define.ePvPType.MATCH_GUILD_WAR; break;
                                    case PvP_Define.ePvPType.MATCH_RUBY_PVP: setTriggerType = Trigger_Define.ePvPType.MATCH_RUBY_PVP; break;
                                    case PvP_Define.ePvPType.MATCH_PARTY: setTriggerType = Trigger_Define.ePvPType.MATCH_PARTY; break;
                                }
                                if(setTriggerType != Trigger_Define.ePvPType.NONE)
                                    retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.Play_PVP, (int)setTriggerType);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 0);
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);
                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Ticket], mJsonSerializer.ToJsonString(userAccount.Ticket));
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.TicketRemain], mJsonSerializer.ToJsonString(keyremain));
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.UserRating], mJsonSerializer.ToJsonString(UserRating));
                                    //json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.RetRuby], mJsonSerializer.ToJsonString(userAccount.Cash + userAccount.EventCash));
                                    //json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_GuildRejoinTime], mJsonSerializer.ToJsonString(RejoinLeftTime));
                                }
                            }
                        }
                        else if (requestOp.Equals("use_account_ruby"))
                        {
                            //tb.IsoLevel = IsolationLevel.ReadCommitted;
                            int ruby_count = queryFetcher.QueryParam_FetchInt("ruby_count", 0);
                            //bool isplay = queryFetcher.QueryParam_FetchInt("isplay", 0) > 0;

                            if (ruby_count > 0)
                                retError = AccountManager.UseUserCash(ref tb, AID, ruby_count);
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            //if (retError == Result_Define.eResult.SUCCESS & isplay)
                            //{
                            //    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.PvPOperationSID.MATCH_RUBY_PVP);
                            //    retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.Play_PVP, (int)PvP_Define.ePvPType.MATCH_RUBY_PVP);
                            //}

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.RetRuby], mJsonSerializer.ToJsonString(userAccount.Cash + userAccount.EventCash));
                            }
                        }
                        else if (requestOp.Equals("add_account_point"))
                        {
                            Item_Define.eItemBuyPriceType priceType = (Item_Define.eItemBuyPriceType)queryFetcher.QueryParam_FetchInt("price_type");
                            int userAddPoint = queryFetcher.QueryParam_FetchInt("add_value");
                            if (priceType != Item_Define.eItemBuyPriceType.None && userAddPoint > 0)
                            {
                                retError = AccountManager.AddUserPoint(ref tb, AID, userAddPoint, priceType);
                            }
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                            long retValue = 0;
                            long retRemainTime = 0;

                            if (retError == Result_Define.eResult.SUCCESS && priceType != Item_Define.eItemBuyPriceType.None)// && userAddPoint > 0)
                            {
                                switch (priceType)
                                {
                                    case Item_Define.eItemBuyPriceType.PriceType_PayCash:
                                        retValue = userAccount.Cash + userAccount.EventCash;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_PayGold:
                                        retValue = userAccount.Gold;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_Key:
                                        {
                                            int key = 0; int keyfillmax = 0; int keyremain = 0;
                                            retError = AccountManager.KeySpendCharge(ref tb, AID, 0, ref key, ref keyremain, ref keyfillmax);
                                            userAccount.Key = key;
                                            retValue = userAccount.Key;
                                            retRemainTime = keyremain;
                                        }
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_Ticket:
                                        {
                                            int key = 0; int keyfillmax = 0; int keyremain = 0;
                                            retError = AccountManager.TicketSpendCharge(ref tb, AID, 0, ref key, ref keyremain, ref keyfillmax);
                                            userAccount.Ticket = key;
                                            retValue = userAccount.Ticket;
                                            retRemainTime = keyremain;
                                        }
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_HonorPoint:
                                        retValue = userAccount.Honorpoint;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_PartyDungeonPoint:
                                        retValue = userAccount.PartyDungeonPoint;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_CombatPoint:
                                        retValue = userAccount.CombatPoint;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_PayExpeditionPoint:
                                        retValue = userAccount.ExpeditionPoint;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_RankingPoint:
                                        retValue = userAccount.OverlordPoint;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_PayGuildPoint:
                                        retValue = userAccount.ContributionPoint;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_BlackMarketPoint:
                                        retValue = userAccount.BlackMarketPoint;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_PayGachaCoin:
                                    case Item_Define.eItemBuyPriceType.PriceType_PayMedal:
                                    default:
                                        retValue = 0; ;
                                        break;
                                }
                            }
                            json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.RetValue], mJsonSerializer.ToJsonString(retValue));
                            json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.RetRemain], mJsonSerializer.ToJsonString(retRemainTime));
                        }
                        else if (requestOp.Equals("use_account_point"))
                        {
                            Item_Define.eItemBuyPriceType priceType = (Item_Define.eItemBuyPriceType)queryFetcher.QueryParam_FetchInt("price_type");
                            int userUsePoint = queryFetcher.QueryParam_FetchInt("use_value");
                            if (priceType != Item_Define.eItemBuyPriceType.None && userUsePoint > 0)
                            {
                                retError = AccountManager.UseUserPoint(ref tb, AID, userUsePoint, priceType);
                            }
                            else
                                retError = Result_Define.eResult.SUCCESS;
                            Account userAccount = new Account();

                            if (retError == Result_Define.eResult.SUCCESS)
                                userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);

                            long retValue = 0;
                            long retRemainTime = 0;

                            if (retError == Result_Define.eResult.SUCCESS && priceType != Item_Define.eItemBuyPriceType.None) // && userUsePoint > 0)
                            {
                                switch (priceType)
                                {
                                    case Item_Define.eItemBuyPriceType.PriceType_PayCash:
                                        retValue = userAccount.Cash + userAccount.EventCash;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_PayGold:
                                        retValue = userAccount.Gold;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_Key:
                                        {
                                            int key = 0; int keyfillmax = 0; int keyremain = 0;
                                            retError = AccountManager.KeySpendCharge(ref tb, AID, 0, ref key, ref keyremain, ref keyfillmax);
                                            userAccount.Key = key;
                                            retValue = userAccount.Key;
                                            retRemainTime = keyremain;
                                        }
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_Ticket:
                                        {
                                            int key = 0; int keyfillmax = 0; int keyremain = 0;
                                            retError = AccountManager.TicketSpendCharge(ref tb, AID, 0, ref key, ref keyremain, ref keyfillmax);
                                            userAccount.Ticket = key;
                                            retValue = userAccount.Ticket;
                                            retRemainTime = keyremain;
                                        }
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_HonorPoint:
                                        retValue = userAccount.Honorpoint;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_PartyDungeonPoint:
                                        retValue = userAccount.PartyDungeonPoint;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_CombatPoint:
                                        retValue = userAccount.CombatPoint;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_PayExpeditionPoint:
                                        retValue = userAccount.ExpeditionPoint;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_RankingPoint:
                                        retValue = userAccount.OverlordPoint;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_PayGuildPoint:
                                        retValue = userAccount.ContributionPoint;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_BlackMarketPoint:
                                        retValue = userAccount.BlackMarketPoint;
                                        break;
                                    case Item_Define.eItemBuyPriceType.PriceType_PayGachaCoin:
                                    case Item_Define.eItemBuyPriceType.PriceType_PayMedal:
                                    default:
                                        retValue = 0; ;
                                        break;
                                }
                            }
                            json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.RetValue], mJsonSerializer.ToJsonString(retValue));
                            json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.RetRemain], mJsonSerializer.ToJsonString(retRemainTime));
                        }
                        else if (requestOp.Equals("use_contributionpoint"))
                        {
                            //tb.IsoLevel = IsolationLevel.ReadCommitted;
                            int contributionpoint = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("contributionpoint", ""));

                            retError = GuildManager.UseGuildContributionPoint(ref tb, AID, contributionpoint);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.ContributionPoint], mJsonSerializer.ToJsonString(userAccount.ContributionPoint));
                            }
                        }
                        else if (requestOp.Equals("addcontributionpoint"))
                        {
                            //tb.IsoLevel = IsolationLevel.ReadCommitted;
                            int contributionpoint = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("contributionpoint", ""));

                            retError = GuildManager.AddGuildContributionPoint(ref tb, AID, contributionpoint);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.ContributionPoint], mJsonSerializer.ToJsonString(userAccount.ContributionPoint));
                                }
                            }
                        }
                        else if (requestOp.Equals("party_dungeon_result"))
                        {
                            //tb.IsoLevel = IsolationLevel.ReadCommitted;
                            int dungeonID = queryFetcher.QueryParam_FetchInt("dungeonid");
                            long dropboxID = queryFetcher.QueryParam_FetchLong("dropboxid");
                            int getExp = queryFetcher.QueryParam_FetchInt("getexp");
                            int getGold = queryFetcher.QueryParam_FetchInt("getgold");
                            bool bWin = queryFetcher.QueryParam_FetchInt("iswin", 1) > 0;
                            bool playTime = queryFetcher.QueryParam_FetchInt("play_time", 1) > 0;
                            Character charInfo = CharacterManager.GetCharacter(ref tb, AID, CID);

                            retError = Result_Define.eResult.SUCCESS;

                            List<User_Inven> makeRealItem = new List<User_Inven>();
                            if (retError == Result_Define.eResult.SUCCESS && bWin)
                            {
                                List<System_Drop_Group> getDropList = new List<System_Drop_Group>();
                                getDropList = DropManager.GetDropResult(ref tb, AID, dropboxID, (short)charInfo.Class);

                                foreach (System_Drop_Group setDrop in getDropList)
                                {
                                    List<User_Inven> getItem = new List<User_Inven>();
                                    retError = DropManager.MakeDropItem(ref tb, ref getItem, setDrop, AID, CID);

                                    makeRealItem.AddRange(getItem);
                                }

                                if (makeRealItem.Count < 1)
                                    retError = Result_Define.eResult.ITEM_CREATE_FAIL;
                            }

                            long before_level = charInfo.level;
                            long before_exp = charInfo.exp;

                            //Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                            //RetBeforeInfo retBefore = new RetBeforeInfo(charInfo.level, charInfo.exp, userAccount.Gold, userAccount.Cash + userAccount.EventCash,
                            //                userAccount.Key, userAccount.KeyFillMaxEA, userAccount.Ticket, userAccount.TicketFillMaxEA, userAccount.ChallengeTicket);

                            // update character info exp, gold
                            if (retError == Result_Define.eResult.SUCCESS && bWin)
                            {

                                if (getExp > 0)
                                {
                                    int checkContents = 0;
                                    float bonusRate = AccountManager.CheckExpRate(ref tb, out checkContents);
                                    if (bonusRate > 1.0f && TriggerManager.IsSetMask(checkContents, (int)SystemData_Define.eContentsType.MATCH_PARTY ))
                                        getExp = (int)System.Math.Floor(getExp * bonusRate);
                                }
                                retError = CharacterManager.UpdateCharacterInfo(ref tb, CID, AID, getExp, getGold);
                            }

                            // set party dungeon clear
                            if (retError == Result_Define.eResult.SUCCESS && bWin)
                            {
                                List<User_PartyDungeon_Clear> clearList = PvPManager.GetUserPartyDungeonClearList(ref tb, AID);
                                var checkClear = clearList.Find(map => map.map_index == dungeonID && map.clear > 0);

                                if (checkClear == null)
                                    retError = PvPManager.SetUserPartyDungeonClear(ref tb, AID, dungeonID);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                                if (bWin)
                                {
                                    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Clear_Party, dungeonID));
                                    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Clear_Party_First, dungeonID));
                                }

                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_Party, dungeonID));
                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_Party_First, dungeonID));
                                retError = TriggerManager.ProgressTrigger(ref tb, AID, setDataList);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 1);
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);

                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], (long)SnailLog_Define.PvPOperationSID.MATCH_PARTY_RESULT);
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_instance_log]);
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id], ((int)dungeonID + SnailLog_Define.Snail_s_id_Seperator_pve_party).ToString());
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_duration], playTime);

                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                charInfo = CharacterManager.GetCharacter(ref tb, AID, CID, true);
                                //retBefore.levelup = retBefore.beforelevel < charInfo.level ? 1 : 0;
                                //charInfo.exp = retBefore.beforelevel == charInfo.level && charInfo.level == Character_Define.Max_CharacterLevel ? getExp : charInfo.exp;
                                charInfo.exp = before_level == charInfo.level && charInfo.level == Character_Define.Max_CharacterLevel ? getExp : charInfo.exp;

                                json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.LeftGold], mJsonSerializer.ToJsonString(userAccount.Gold));
                                json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.BeforeCharacterLevel], mJsonSerializer.ToJsonString(before_level));
                                json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.BeforeCharacterExp], mJsonSerializer.ToJsonString(before_exp));
                                json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.CharacterLevel], mJsonSerializer.ToJsonString(charInfo.level));
                                json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.CharacterExp], mJsonSerializer.ToJsonString(charInfo.exp));
                                json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                            }
                        }
                        else if (requestOp.Equals("add_guild_point"))
                        {
                            //tb.IsoLevel = IsolationLevel.ReadCommitted;
                            Guild_Define.ePlayType pvpType = (Guild_Define.ePlayType)System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("pvptype", "0"));
                            Guild userGuildInfo = GuildManager.GetGuildInfo(ref tb, AID);
                            if (userGuildInfo.guild_id > 0)
                            {
                                switch (pvpType)
                                {
                                    case Guild_Define.ePlayType.MATCH_FREE:
                                        GuildManager.AddGuildPoint(ref tb, userGuildInfo.guild_id, AID, Guild_Define.AddGuildPoint_List[pvpType]);
                                        break;
                                    case Guild_Define.ePlayType.MATCH_1VS1:
                                        GuildManager.AddGuildPoint(ref tb, userGuildInfo.guild_id, AID, Guild_Define.AddGuildPoint_List[pvpType]);
                                        break;
                                    case Guild_Define.ePlayType.MATCH_GUILD_3VS3:
                                        GuildManager.AddGuildPoint(ref tb, userGuildInfo.guild_id, AID, Guild_Define.AddGuildPoint_List[pvpType]);
                                        break;
                                    case Guild_Define.ePlayType.MATCH_OVERLORD:
                                        GuildManager.AddGuildPoint(ref tb, userGuildInfo.guild_id, AID, Guild_Define.AddGuildPoint_List[pvpType]);
                                        break;
                                    case Guild_Define.ePlayType.MATCH_TEAM:
                                        GuildManager.AddGuildPoint(ref tb, userGuildInfo.guild_id, AID, Guild_Define.AddGuildPoint_List[pvpType]);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        /*
                         "user_full_info"
                         "equip_item_list",
                         "equip_soul_list",
                         "equip_active_soul_list",
                         "equip_passive_soul_list",
                         "character_list",
                         "character_info",
                         "account_info",
                         "pvp_info",
                         */
                        else if (requestOp.Equals("user_full_info"))
                        {
                            Account userInfo = TheSoul.DataManager.AccountManager.GetAccountData(ref tb, AID, ref retError, true);
                            List<Character_Detail> userCharList = TheSoul.DataManager.CharacterManager.GetCharacterListWithEquip(ref tb, AID, true);
                            User_CharacterGroup setCharacterGroup = CharacterManager.GetCharacterGroupInfo(ref tb, AID, userInfo.EquipCID, true);

                            int attendYesterdayCount = 0, attendTodayCount = 0, guildLevel = 0;

                            if (userInfo.GuildID > 0)
                            {
                                attendYesterdayCount = GuildManager.GetGuildAttendCount(ref tb, userInfo.GuildID);
                                attendTodayCount = GuildManager.GetGuildAttendCount(ref tb, userInfo.GuildID);
                                guildLevel = GuildManager.GetGuildLV_From_GID(ref tb, userInfo.GuildID);
                            }

                            List<User_PvP_Play_Info> retPvPList = new List<User_PvP_Play_Info>();
                            foreach (PvP_Define.ePvPType setPvPType in Enum.GetValues(typeof(PvP_Define.ePvPType)))
                            {
                                if (setPvPType > PvP_Define.ePvPType.MATCH_NONE && setPvPType <= PvP_Define.LastPvP)
                                {
                                    List<User_PvP_Play_Info> userPvPInfo = PvPManager.GetUser_PvPInfo_List(ref tb, AID, setPvPType);
                                    retPvPList.AddRange(userPvPInfo);
                                }
                            }

                            int seperaterWeekOrSeason = PvPManager.GetSeperater(ref tb, PvP_Define.ePvPType.MATCH_1VS1);
                            PVP_Record getInfo = PvPManager.GetUser_PvP_Record(ref tb, AID, seperaterWeekOrSeason, PvP_Define.ePvPType.MATCH_1VS1);
                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Record], mJsonSerializer.ToJsonString(getInfo));

                            json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(userInfo));
                            json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CharacterList], Character_Detail.makeCharacter_DetailListJson(ref userCharList));
                            json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CharacterGroup], mJsonSerializer.ToJsonString(setCharacterGroup));

                            json = mJsonSerializer.AddJson(json, Guild_Define.Guild_Ret_KeyList[Guild_Define.eGuildReturnKeys.GuildLevel], guildLevel.ToString());
                            json = mJsonSerializer.AddJson(json, Guild_Define.Guild_Ret_KeyList[Guild_Define.eGuildReturnKeys.YesterDayAttend], attendYesterdayCount.ToString());
                            json = mJsonSerializer.AddJson(json, Guild_Define.Guild_Ret_KeyList[Guild_Define.eGuildReturnKeys.ToDayAttend], attendTodayCount.ToString());

                            json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.PvPCountInfo], mJsonSerializer.ToJsonString(retPvPList));
                        }
                        else if (requestOp.Equals("account_info"))
                        {
                            Account getInfo = TheSoul.DataManager.AccountManager.FlushAccountData(ref tb, AID, ref retError);
                            json = mJsonSerializer.AddJson(json, requestOp, mJsonSerializer.ToJsonString(getInfo));
                        }
                        else if (requestOp.Equals("character_list"))
                        {
                            List<Character> getInfo = TheSoul.DataManager.CharacterManager.GetCharacterList(ref tb, AID, true);
                            json = mJsonSerializer.AddJson(json, requestOp, mJsonSerializer.ToJsonString(getInfo));
                        }
                        else if (requestOp.Equals("character_info"))
                        {
                            Character getInfo = TheSoul.DataManager.CharacterManager.GetCharacterInfoWithEquip(ref tb, AID, CID, true);
                            json = mJsonSerializer.AddJson(json, requestOp, mJsonSerializer.ToJsonString(getInfo));
                        }
                        else if (requestOp.Equals("character_group"))
                        {
                            Account getInfo = TheSoul.DataManager.AccountManager.FlushAccountData(ref tb, AID, ref retError);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (getInfo.EquipCID < 1)
                                {
                                    List<Character> userCharList = TheSoul.DataManager.CharacterManager.GetCharacterList(ref tb, AID, true);
                                    getInfo.EquipCID = userCharList.FirstOrDefault().cid;
                                }
                                User_CharacterGroup setCharacterGroup = CharacterManager.GetCharacterGroupInfo(ref tb, AID, getInfo.EquipCID, true);
                                json = mJsonSerializer.AddJson(json, requestOp, mJsonSerializer.ToJsonString(setCharacterGroup));
                            }
                        }
                        else if (requestOp.Equals("equip_item_list"))
                        {
                            List<User_Inven> equipList = TheSoul.DataManager.ItemManager.GetEquipList(ref tb, AID, CID, true, true);
                            List<User_Ultimate_Inven> equip_ultimate = ItemManager.GetUserUltimateWeaponList(ref tb, AID, CID, true, true);

                            json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.EquipItemList], mJsonSerializer.ToJsonString(equipList));
                            json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.EquipUltimateWeaponList], mJsonSerializer.ToJsonString(equip_ultimate));
                        }
                        else if (requestOp.Equals("equip_soul_list"))
                        {
                            List<Ret_Equip_Soul_Active> equipActiveSoul = SoulManager.GetRet_Active_Soul_Equip_List(ref tb, AID, CID, true);
                            List<Ret_Soul_Passive> equipPassiveSoul = SoulManager.GetRet_Passive_Soul_Equip_List(ref tb, AID, CID, true);

                            json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.EquipActiveSoulList], mJsonSerializer.ToJsonString(equipActiveSoul));
                            json = mJsonSerializer.AddJson(json, Soul_Define.Soul_Ret_KeyList[Soul_Define.eSoulReturnKeys.EquipPassiveSoulList], mJsonSerializer.ToJsonString(equipPassiveSoul));
                        }
                        else if (requestOp.Equals("equip_active_soul_list"))
                        {
                            List<Ret_Equip_Soul_Active> equipActiveSoul = SoulManager.GetRet_Active_Soul_Equip_List(ref tb, AID, CID, true);
                            json = mJsonSerializer.AddJson(json, requestOp, mJsonSerializer.ToJsonString(equipActiveSoul));
                        }
                        else if (requestOp.Equals("equip_passive_soul_list"))
                        {
                            List<Ret_Soul_Passive> equipPassiveSoul = SoulManager.GetRet_Passive_Soul_Equip_List(ref tb, AID, CID, true);
                            json = mJsonSerializer.AddJson(json, requestOp, mJsonSerializer.ToJsonString(equipPassiveSoul));
                        }
                        else if (requestOp.Equals("pvp_info"))
                        {
                            PvPInfo getPvPInfo = TheSoul.DataManager.AccountManager.GetPvPInfo(ref tb, AID);
                            json = mJsonSerializer.AddJson(json, requestOp, mJsonSerializer.ToJsonString(getPvPInfo));
                        }
                        else if (requestOp.Equals("pvp_count_info"))
                        {
                            List<User_PvP_Play_Info> retPvPList = new List<User_PvP_Play_Info>();
                            foreach (PvP_Define.ePvPType setPvPType in Enum.GetValues(typeof(PvP_Define.ePvPType)))
                            {
                                if (setPvPType > PvP_Define.ePvPType.MATCH_NONE && setPvPType <= PvP_Define.LastPvP)
                                {
                                    List<User_PvP_Play_Info> userPvPInfo = PvPManager.GetUser_PvPInfo_List(ref tb, AID, setPvPType);
                                    retPvPList.AddRange(userPvPInfo);
                                }
                            }
                            json = mJsonSerializer.AddJson(json, requestOp, mJsonSerializer.ToJsonString(retPvPList));
                        }
                        else if (requestOp.Equals("pvp_count_reset"))
                        {
                            //tb.IsoLevel = IsolationLevel.ReadCommitted;

                            int pvpType = queryFetcher.QueryParam_FetchInt("pvp_type");
                            int mapIndex = queryFetcher.QueryParam_FetchInt("map_index", 1);

                            int useRubyCount = 0;
                            int resetCount = 0;

                            User_PvP_Play userPvPInfo = PvPManager.GetUser_PvP_Play_Info_DB(ref tb, AID, pvpType, mapIndex);

                            if (pvpType == (int)PvP_Define.ePvPType.MATCH_PARTY)
                            {
                                if (VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.DUNGEONCOUNT_RESET_CO_OP, userPvPInfo.play_resetcount))
                                {
                                    useRubyCount = SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_RESET_COST_RUBY_3PVE]);
                                    resetCount = userPvPInfo.play_resetcount + 1;
                                    retError = Result_Define.eResult.SUCCESS;
                                }
                                else
                                    retError = Result_Define.eResult.VIP_DUNGEON_RESET_COUNT_OVER;
                            }
                            //else
                            //    retError = Result_Define.eResult.VIP_DUNGEON_RESET_TYPE_INVALIDE;

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = PvPManager.SetPvPDailyCount(ref tb, AID, pvpType, 0, mapIndex);

                            if (retError == Result_Define.eResult.SUCCESS && useRubyCount > 0)
                                retError = AccountManager.UseUserCash(ref tb, AID, useRubyCount);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account UserInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetRuby], (UserInfo.Cash + UserInfo.EventCash).ToString());
                                    json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.CurrentResetCount], resetCount.ToString());
                                }
                            }
                        }
                        else if (requestOp.Equals("trigger_progress"))
                        {
                            //tb.IsoLevel = IsolationLevel.ReadCommitted;

                            Trigger_Define.eTriggerType TriggerType = Trigger_Define.TriggerType[queryFetcher.QueryParam_Fetch("trigger_type")];
                            int TriggerValue1 = queryFetcher.QueryParam_FetchInt("trigger_value1", 0);
                            int TriggerValue2 = queryFetcher.QueryParam_FetchInt("trigger_value2", 0);
                            int TriggerValue3 = queryFetcher.QueryParam_FetchInt("trigger_value3", 1);

                            retError = TriggerManager.ProgressTrigger(ref tb, AID, TriggerType, TriggerValue1, TriggerValue2, TriggerValue3);
                        }
                        else if (requestOp.Equals("item_make"))
                        {
                            string cdkey = queryFetcher.QueryParam_Fetch("cdkey");
                            long itemID = queryFetcher.QueryParam_FetchLong("itemid");
                            int makeCount = queryFetcher.QueryParam_FetchInt("itemcount");

                            List<User_Inven> makeItem = new List<User_Inven>();
                            retError = ItemManager.MakeItem(ref tb, ref makeItem, AID, itemID, makeCount, CID);

                            if (retError == Result_Define.eResult.SUCCESS)
                                json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeItem));
                        }
                        else if (requestOp.Equals("send_mail"))
                        {
                            string cdkey = queryFetcher.QueryParam_Fetch("cdkey");
                            long senderAID = queryFetcher.QueryParam_FetchLong("sender", 0);
                            string senderName = queryFetcher.QueryParam_Fetch("sender_name", "");
                            long receiverAID = queryFetcher.QueryParam_FetchLong("receiver");
                            string titleText = queryFetcher.QueryParam_Fetch("title");
                            string bodyText = queryFetcher.QueryParam_Fetch("body");
                            string makeItemJson = queryFetcher.QueryParam_Fetch("itemlist", "[]");

                            if (receiverAID > 0)
                            {
                                List<Set_Mail_Item> mailItem = new List<Set_Mail_Item>();
                                if (!string.IsNullOrEmpty(makeItemJson))
                                    mailItem = mJsonSerializer.JsonToObject<List<Set_Mail_Item>>(makeItemJson);

                                int sendItemCount = 0;
                                Dictionary<short, List<Set_Mail_Item>> sendMailList = new Dictionary<short, List<Set_Mail_Item>>();

                                foreach (Set_Mail_Item setItem in mailItem)
                                {
                                    Item_Define.eSystemItemType checkType = Item_Define.eSystemItemType.ItemClass_NONE;
                                    Object SysItem = ItemManager.CheckItemType(ref tb, setItem.itemid, ref checkType);

                                    if (checkType == Item_Define.eSystemItemType.ItemClass_Equip)
                                    {
                                        System_Item_Equip itemInfo = ItemManager.GetSystem_Item_Equip(ref tb, setItem.itemid);
                                        short setItemClass = (short)(itemInfo.Class_IndexID == 0 ? 0 : Character_Define.ClassTypeToEnum[itemInfo.EquipClass]);

                                        if (!sendMailList.ContainsKey(setItemClass))
                                            sendMailList[setItemClass] = new List<Set_Mail_Item>();

                                        sendMailList[setItemClass].Add(setItem);
                                    }
                                    else
                                    {
                                        if (!sendMailList.ContainsKey((short)Character_Define.SystemClassType.Class_None))
                                            sendMailList[(short)Character_Define.SystemClassType.Class_None] = new List<Set_Mail_Item>();
                                        sendMailList[(short)Character_Define.SystemClassType.Class_None].Add(setItem);
                                    }
                                }

                                List<long> mailSeqList = new List<long>();
                                long mailSeq = 0;

                                foreach (KeyValuePair<short, List<Set_Mail_Item>> mailList in sendMailList)
                                {
                                    List<Set_Mail_Item> setMailItem = new List<Set_Mail_Item>();

                                    //string mailTitle = VipManager.GetVIPRewardMailTitle(vip_level, mailList.Key);
                                    //string mailBody = VipManager.GetVIPRewardMailBody(vip_level, mailList.Key);

                                    foreach (Set_Mail_Item setItem in mailList.Value)
                                    {
                                        if (setItem.itemid > 0 && setItem.itemea > 0)
                                        {
                                            sendItemCount++;
                                            setMailItem.Add(setItem);
                                        }
                                        if (sendItemCount >= Mail_Define.Mail_MaxItem)
                                        {
                                            retError = MailManager.SendMail(ref tb, ref setMailItem, receiverAID, out mailSeq, senderAID, senderName, titleText, bodyText, Mail_Define.Mail_VIP_CloseTime_Min);
                                            if (retError == Result_Define.eResult.SUCCESS)
                                            {
                                                mailSeqList.Add(mailSeq);
                                                setMailItem.Clear();
                                                sendItemCount = 0;
                                            }
                                            else
                                                break;
                                        }
                                    }

                                    if (retError == Result_Define.eResult.SUCCESS && sendItemCount > 0)
                                    {
                                        retError = MailManager.SendMail(ref tb, ref setMailItem, receiverAID, out mailSeq, senderAID, senderName, titleText, bodyText, Mail_Define.Mail_VIP_CloseTime_Min);
                                        if (retError == Result_Define.eResult.SUCCESS)
                                            mailSeqList.Add(mailSeq);
                                    }
                                }

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    // update couponkey log
                                    string couponKey = queryFetcher.QueryParam_Fetch("couponkey");
                                    string platform_id = queryFetcher.QueryParam_Fetch("platform_id");
                                    Account_Define.eCouponType setCouponType = (Account_Define.eCouponType)queryFetcher.QueryParam_FetchInt("coupon_type", (int)Account_Define.eCouponType.EAI);

                                    retError = AccountManager.SetUser_Coupon_Key(ref tb, AID, platform_id, couponKey, mJsonSerializer.ToJsonString(mailSeqList), setCouponType, Account_Define.eCouponState.Use);
                                }
                            }
                            else
                                retError = Result_Define.eResult.ACCOUNT_ID_NOT_FOUND;
                        }
                        else if (requestOp.Equals("reg_snail_id"))
                        {
                            long snailID = queryFetcher.QueryParam_FetchLong("snail_id");
                            string snailUserID = queryFetcher.QueryParam_Fetch("snail_platform_id", "");

                            Account userAccount = AccountManager.GetAccountData(ref tb, AID, ref retError);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (userAccount.SNO != snailID || !userAccount.UserID.Equals(snailUserID))
                                    retError = AccountManager.UpdateSNO(ref tb, AID, snailID, snailUserID);
                                else
                                    retError = Result_Define.eResult.DB_ERROR;
                            }
                        }
                        else if (requestOp.Equals("reg_snail_cdkey"))
                        {
                            string couponKey = queryFetcher.QueryParam_Fetch("couponkey");
                            string platform_id = queryFetcher.QueryParam_Fetch("platform_id");
                            retError = AccountManager.SetUser_Coupon_Key(ref tb, AID, platform_id, couponKey, Account_Define.EmptyMailSeq, Account_Define.eCouponType.CDKey, Account_Define.eCouponState.Regist);
                        }
                        else if (requestOp.Equals("pvp_open_time"))
                        {
                            PvP_Define.ePvPType pvpType = (PvP_Define.ePvPType)queryFetcher.QueryParam_FetchInt("pvp_type");

                            bool isBonus = false;
                            bool isOpen = PvPManager.CheckPvPOpenTime(ref tb, pvpType, out isBonus);

                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_OpenFlag], mJsonSerializer.ToJsonString(isOpen ? 1 : 0));
                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_BonusFlag], mJsonSerializer.ToJsonString(isBonus ? 1 : 0));
                        }
                        else if (requestOp.Equals("guild_info"))
                        {
                            long guildID = queryFetcher.QueryParam_FetchLong("gid");

                            if (guildID == 0)
                                AccountManager.FlushAccountData(ref tb, AID, ref retError);

                            int attendYesterdayCount = GuildManager.GetGuildAttendCount(ref tb, guildID);
                            int attendTodayCount = GuildManager.GetGuildAttendCount(ref tb, guildID);
                            int guildLevel = GuildManager.GetGuildLV_From_GID(ref tb, guildID);

                            json = mJsonSerializer.AddJson(json, Guild_Define.Guild_Ret_KeyList[Guild_Define.eGuildReturnKeys.GuildLevel], guildLevel.ToString());
                            json = mJsonSerializer.AddJson(json, Guild_Define.Guild_Ret_KeyList[Guild_Define.eGuildReturnKeys.YesterDayAttend], attendYesterdayCount.ToString());
                            json = mJsonSerializer.AddJson(json, Guild_Define.Guild_Ret_KeyList[Guild_Define.eGuildReturnKeys.ToDayAttend], attendTodayCount.ToString());
                        }
                        else if (requestOp.Equals("set_warpoint"))
                        {
                            Character_Stat charStat = mJsonSerializer.JsonToObject<Character_Stat>(queryFetcher.QueryParam_Fetch("stat", ""));

                            if (charStat.HPMax > 0 && charStat.ATTACK_MAX > Character_Define.Minimum_Attack
                                && charStat.WAR_POINT < 9216939
                                && charStat.ACTIVE_SOUL_WAR_POINT < 5349700
                                && charStat.PASSIVE_SOUL_WAR_POINT < 1711491
                                //&& charStat.WAR_POINT < 3216939
                                //&& charStat.ACTIVE_SOUL_WAR_POINT < 1349700
                                //&& charStat.PASSIVE_SOUL_WAR_POINT < 711491
                                )
                            {
                                //Character_Stat beforeStat = CharacterManager.GetCharacterStat(ref tb, CID);
                                long total_warpoint = charStat.WAR_POINT + charStat.ACTIVE_SOUL_WAR_POINT + charStat.PASSIVE_SOUL_WAR_POINT;
                                charStat.MAX_WAR_POINT = (int)total_warpoint;
                                retError = CharacterManager.UpdateCharacterWarpoint(ref tb, AID, CID, total_warpoint);
                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = CharacterManager.UpdateCharacterStat(ref tb, CID, ref charStat);
                                PvPManager.SetUser_PvP_Warpoint(AID, CID, total_warpoint);
                            }
                            else
                                retError = Result_Define.eResult.SYSTEM_PARAM_ERROR;
                        }
                        else if (requestOp.Equals("get_warpoint"))
                        {
                            User_WarPoint getWarpoint = GoldExpedition_Manager.GetUserWarPoint(ref tb, AID);

                            json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account_Warpoint], getWarpoint.WAR_POINT.ToString());
                        }
                        /*
                    // load system data 
                    "get_system_data",

                    // pvp operation
                    "get_pvp_record",
                    "set_pvp_count",
                    "set_pvp_record",
                        */
                        else if (requestOp.Equals("get_system_data"))
                        {
                            List<SystemData.System_Const> constList = SystemData.GetSystem_Const_All(ref tb);
                            List<RetBattleReward> rewardList = PvPManager.GetSystem_Battle_Reward_List(ref tb);
                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_ConstList], mJsonSerializer.ToJsonString(constList));
                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_BattleRewardList], mJsonSerializer.ToJsonString(rewardList));
                        }
                        else if (requestOp.Equals("get_system_data_value"))
                        {
                            string getConstKey = queryFetcher.QueryParam_Fetch("const_key");
                            double getConstValue = SystemData.GetConstValue(ref tb, getConstKey);
                            json = mJsonSerializer.AddJson(json, SystemData_Define.SystemConstDefineKey, getConstKey);
                            json = mJsonSerializer.AddJson(json, SystemData_Define.SystemConstDefineValue, getConstValue.ToString());
                        }
                        else if (requestOp.Equals("get_admin_data_value"))
                        {
                            string getConstKey = queryFetcher.QueryParam_Fetch("const_key");
                            double getConstValue = SystemData.AdminConstValueFetchFromRedis(ref tb, getConstKey);
                            json = mJsonSerializer.AddJson(json, SystemData_Define.SystemConstDefineKey, getConstKey);
                            json = mJsonSerializer.AddJson(json, SystemData_Define.SystemConstDefineValue, getConstValue.ToString());
                        }
                        else if (requestOp.Equals("get_pvp_record_all"))
                        {
                            PvP_Define.ePvPType pvpType = (PvP_Define.ePvPType)queryFetcher.QueryParam_FetchInt("pvp_type", (int)PvP_Define.ePvPType.MATCH_NONE);

                            int seperaterWeekOrSeason = PvPManager.GetSeperater(ref tb, pvpType);

                            List<PVP_Record> getInfo = PvPManager.GetUser_PvP_Record_All(ref tb, AID, seperaterWeekOrSeason);
                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Record], mJsonSerializer.ToJsonString(getInfo));
                        }

                        else if (requestOp.Equals("get_pvp_record"))
                        {
                            PvP_Define.ePvPType pvpType = (PvP_Define.ePvPType)queryFetcher.QueryParam_FetchInt("pvp_type", (int)PvP_Define.ePvPType.MATCH_NONE);

                            int seperaterWeekOrSeason = PvPManager.GetSeperater(ref tb, pvpType);

                            PVP_Record getInfo = PvPManager.GetUser_PvP_Record(ref tb, AID, seperaterWeekOrSeason, pvpType);
                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Record], mJsonSerializer.ToJsonString(getInfo));
                        }
                        else if (requestOp.Equals("set_pvp_count"))
                        {
                            PvP_Define.ePvPType pvpType = (PvP_Define.ePvPType)queryFetcher.QueryParam_FetchInt("pvp_type", (int)PvP_Define.ePvPType.MATCH_NONE);
                            int mapIndex = queryFetcher.QueryParam_FetchInt("map_index", 1);
                            int addCount = queryFetcher.QueryParam_FetchInt("add_count", 1);
                            retError = PvPManager.AddUser_PvP_CountToDB(ref tb, AID, pvpType, mapIndex, addCount);
                            User_PvP_Play_Info userCount = PvPManager.GetUser_PvPInfo(ref tb, AID, pvpType);
                            //User_PvP_Play userCount = PvPManager.GetUser_PvP_Play_Info_DB(ref tb, AID, (int)pvpType, mapIndex);

                            //int playMaxCount = 9999999;
                            //switch(pvpType)
                            //{
                            //    case PvP_Define.ePvPType.MATCH_1VS1:
                            //        playMaxCount = VipManager.User_Vip_Value(ref tb, AID, VIP_Define.eVipType.DUNGEONCOUNT_MAX_1VS1REAL);
                            //        break;
                            //    case PvP_Define.ePvPType.MATCH_GUILD_3VS3:
                            //        playMaxCount = SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_GUILD_G3VS3_ENTER_MAX_COUNT]);
                            //        break;
                            //    case PvP_Define.ePvPType.MATCH_RUBY_PVP:
                            //        playMaxCount = VipManager.User_Vip_Value(ref tb, AID, VIP_Define.eVipType.DUNGEONCOUNT_MAX_GLADIATOR);
                            //        break;
                            //    case PvP_Define.ePvPType.MATCH_PARTY:
                            //        playMaxCount = VipManager.User_Vip_Value(ref tb, AID, VIP_Define.eVipType.DUNGEONCOUNT_MAX_CO_OP);
                            //        break;
                            //    case PvP_Define.ePvPType.MATCH_OVERLORD:
                            //        playMaxCount = VipManager.User_Vip_Value(ref tb, AID, VIP_Define.eVipType.DUNGEONCOUNT_MAX_RANKING);
                            //        break;
                            //}

                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_PlayCount], mJsonSerializer.ToJsonString(userCount.play_count));
                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_PlayMaxCount], mJsonSerializer.ToJsonString(userCount.max_play_count));
                        }
                        else if (requestOp.Equals("pvp_guildwar_join"))
                        {
                            Account userAccount = AccountManager.GetAccountData(ref tb, AID, ref retError);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Guild_User_PVP_Record getInfo = PvPManager.GetGuild_User_PvP_Record(ref tb, AID, userAccount.GuildID, PvP_Define.PvP_GuildUser_Monthly_TableName, 0, true);
                                int setCoolTime = SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_GUILD_G3VS3_COOLTIME_MINUTE]);
                                DateTime checkTime = getInfo.lastjoin_date.AddMinutes(setCoolTime);
                                TimeSpan TS = checkTime - DateTime.Now;

                                int RejoinLeftTime = 0;
                                bool bFlush = false;

                                if (TS.TotalSeconds > 0)
                                {
                                    retError = PvPManager.SetGuild_User_PvP_Record_JoinReset(ref tb, getInfo, PvP_Define.PvP_GuildUser_Monthly_TableName, setCoolTime);
                                    bFlush = true;
                                }
                                else
                                    retError = Result_Define.eResult.SUCCESS;

                                if (retError == Result_Define.eResult.SUCCESS && bFlush)
                                {
                                    int useRuby = SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_GUILD_G3VS3_COOLTIME_DELETE_RUBY]);
                                    if(useRuby > 0)
                                        retError = AccountManager.UseUserCash(ref tb, AID, useRuby);
                                    RejoinLeftTime = (int)TS.TotalSeconds;
                                }

                                if (bFlush)
                                    userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);

                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.RetRuby], mJsonSerializer.ToJsonString(userAccount.Cash + userAccount.EventCash));
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_GuildRejoinTime], mJsonSerializer.ToJsonString(RejoinLeftTime));
                            }
                        }
                        else if (requestOp.Equals("set_pvp_record"))
                        {
                            string pvpInfoJson = queryFetcher.QueryParam_Fetch("player_info", "");
                            PvP_Define.ePvPType pvpType = (PvP_Define.ePvPType)queryFetcher.QueryParam_FetchInt("pvp_type", (int)PvP_Define.ePvPType.MATCH_NONE);

                            if (string.IsNullOrEmpty(pvpInfoJson) || pvpType == PvP_Define.ePvPType.MATCH_NONE)
                                retError = Result_Define.eResult.SYSTEM_PARAM_ERROR;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<PvP_Player_Result> userPvP_Result = mJsonSerializer.JsonToObject<List<PvP_Player_Result>>(pvpInfoJson);
                                List<Ret_PvP_Record_Result> retUserInfo = new List<Ret_PvP_Record_Result>();

                                if (pvpType == PvP_Define.ePvPType.MATCH_1VS1)
                                {
                                    int PvP1vs1BonusStartTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_1VS1REAL_START_TIME_BONUS]);
                                    int PvP1vs1BonusEndTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_1VS1REAL_END_TIME_BOUNS]);
                                    int TodaySecond = PvPManager.GetTodayTotalSeconds();
                                    bool bBonus = (PvP1vs1BonusStartTime >= PvP1vs1BonusEndTime && TodaySecond > PvP1vs1BonusStartTime)
                                                    || (PvP1vs1BonusStartTime < PvP1vs1BonusEndTime && TodaySecond > PvP1vs1BonusStartTime && TodaySecond < PvP1vs1BonusEndTime);

                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.PvPOperationSID.MATCH_1VS1_RESULT);
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], (long)SnailLog_Define.PvPOperationSID.MATCH_1VS1_RESULT);

                                    foreach (PvP_Player_Result userResult in userPvP_Result)
                                    {
                                        PVP_Record getInfo = PvPManager.GetUser_PvP_Record(ref tb, userResult.aid, 0, pvpType);
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], userResult.aid);
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 1);
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);

                                        int before_rankpoint = getInfo.totalhonorpoint;
                                        bool bWin = userResult.match_rank > 0;
                                        getInfo.totalhonorpoint += (bWin ? (PvP_Define.PvP_1vs1_WinPoint * (bBonus ? 2 : 1)) : PvP_Define.PvP_1vs1_LosePoint);

                                        getInfo.totalkill += bWin ? 1 : 0;
                                        getInfo.totaldeath += bWin ? 0 : 1;
                                        getInfo.straightwin = bWin ? getInfo.straightwin + 1 : 0;
                                        getInfo.straightlose = bWin ? 0 : getInfo.straightlose + 1;
                                        retError = PvPManager.SetUser_PvP_Record(ref tb, getInfo);

                                        int setGrade = PvPManager.Get1vs1PvPGrade(getInfo.totalhonorpoint);
                                        int highGrade = PvPManager.GetUser_PvP_High_Grade(ref tb, userResult.aid, (int)pvpType);
                                        if (highGrade < setGrade && retError == Result_Define.eResult.SUCCESS)
                                            retError = PvPManager.SetUser_PvP_High_Grade(ref tb, userResult.aid, setGrade, (int)pvpType);

                                        int highPoint = PvPManager.GetUser_PvP_High_Point(ref tb, userResult.aid, (int)pvpType);
                                        if (highPoint < getInfo.totalhonorpoint && retError == Result_Define.eResult.SUCCESS)
                                            retError = PvPManager.SetUser_PvP_High_Point(ref tb, userResult.aid, getInfo.totalhonorpoint, (int)pvpType);

                                        if (retError == Result_Define.eResult.SUCCESS)
                                        {
                                            List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                                            if (bWin)
                                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Win_PVP, (int)Trigger_Define.ePvPType.MATCH_1VS1));

                                            if (getInfo.straightwin > 0)
                                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Straight_PVP, (int)Trigger_Define.ePvPType.MATCH_1VS1, 0, getInfo.straightwin));
                                            setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Kill_Count, (int)Trigger_Define.ePvPType.MATCH_1VS1, (int)Trigger_Define.eKillCountType.Accumulate));
                                            //setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_PVP, (int)Trigger_Define.ePvPType.MATCH_1VS1));
                                            retError = TriggerManager.ProgressTrigger(ref tb, userResult.aid, setDataList);
                                        }

                                        if (retError != Result_Define.eResult.SUCCESS)
                                            break;
                                        Ret_PvP_Record_Result setObj = new Ret_PvP_Record_Result(userResult.aid, before_rankpoint, getInfo.totalhonorpoint, (int)Item_Define.eItemBuyPriceType.PriceType_CombatPoint, 0, 0, getInfo.straightwin, getInfo.straightlose);
                                        retUserInfo.Add(setObj);
                                        queryFetcher.SnailLogWrite(ref tb);
                                    }
                                }
                                else if (pvpType == PvP_Define.ePvPType.MATCH_FREE)
                                {
                                    int totalPlayer = queryFetcher.QueryParam_FetchInt("player_count");

                                    if (totalPlayer > 0)
                                    {
                                        Account userInfo;

                                        long RankTotalPlayerCount = PvPManager.GetTotal_PvP_Rank_Player(ref tb, 0, PvP_Define.ePvPType.MATCH_FREE);
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.PvPOperationSID.MATCH_FREE_RESULT);
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], (long)SnailLog_Define.PvPOperationSID.MATCH_FREE_RESULT);

                                        foreach (PvP_Player_Result userResult in userPvP_Result)
                                        {
                                            Ret_PvP_Record_Result setObj = new Ret_PvP_Record_Result(userResult.aid);
                                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], userResult.aid);
                                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 1);
                                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);

                                            PVP_Record getInfo = PvPManager.GetUser_PvP_Record(ref tb, userResult.aid, 0, pvpType);
                                            int rewardValue = SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_PVP_FREEFORALL_REWARD_VALUE]);
                                            int getRewardPoint = (int)((totalPlayer * rewardValue) / (userResult.match_rank + (float)totalPlayer));
                                            int getUserGrade = PvPManager.GetUser_Free_PvP_Grade(ref tb, CharacterManager.GetCharacterMaxLevel_FromDB(ref tb, userResult.aid));
                                            PvP_Define.ePvPConstDef gradeConstKey = PvP_Define.PvP_Free_Grade_Const_Key[getUserGrade];
                                            int getHonorPoint = getRewardPoint * SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[gradeConstKey]);

                                            setObj.before_rankpoint = getInfo.totalhonorpoint;
                                            getInfo.totalhonorpoint += getHonorPoint;
                                            setObj.after_rankpoint = getInfo.totalhonorpoint;
                                            setObj.add_user_value = getRewardPoint;

                                            getInfo.totalkill += userResult.kill_count;
                                            getInfo.totaldeath += userResult.death_count;
                                            getInfo.straightwin = 0;
                                            getInfo.straightlose = 0;
                                            retError = PvPManager.SetUser_PvP_Record(ref tb, getInfo);

                                            if (retError == Result_Define.eResult.SUCCESS)
                                            {
                                                retError = AccountManager.AddUserHonor(ref tb, userResult.aid, getRewardPoint);
                                                userInfo = AccountManager.GetAccountData(ref tb, userResult.aid, ref retError);
                                                setObj.price_type = (int)Item_Define.eItemBuyPriceType.PriceType_HonorPoint;
                                            }

                                            if (retError == Result_Define.eResult.SUCCESS)
                                            {
                                                long userRank = PvPManager.GetUser_PvP_Rank(ref tb, getInfo.aid, 0, PvP_Define.ePvPType.MATCH_FREE);
                                                int setGrade = PvPManager.GetFreePvPGrade(userRank, RankTotalPlayerCount);
                                                int highGrade = PvPManager.GetUser_PvP_High_Grade(ref tb, userResult.aid, (int)pvpType);
                                                if (highGrade < setGrade && retError == Result_Define.eResult.SUCCESS)
                                                    retError = PvPManager.SetUser_PvP_High_Grade(ref tb, userResult.aid, setGrade, (int)pvpType);

                                                int highPoint = PvPManager.GetUser_PvP_High_Point(ref tb, userResult.aid, (int)pvpType);
                                                if (highPoint < getInfo.totalhonorpoint && retError == Result_Define.eResult.SUCCESS)
                                                    retError = PvPManager.SetUser_PvP_High_Point(ref tb, userResult.aid, getInfo.totalhonorpoint, (int)pvpType);
                                            }

                                            if (retError == Result_Define.eResult.SUCCESS)
                                            {
                                                List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                                                //if (userResult.kill_streak > 0)
                                                //    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Kill_Count, (int)Trigger_Define.ePvPType.MATCH_FREE, 0, userResult.kill_streak));
                                                if (userResult.kill_streak > 0)
                                                    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Kill_Count, (int)Trigger_Define.ePvPType.MATCH_FREE, (int)Trigger_Define.eKillCountType.KillStreak, userResult.kill_streak));
                                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Kill_Count, (int)Trigger_Define.ePvPType.MATCH_FREE, (int)Trigger_Define.eKillCountType.Accumulate, userResult.kill_count));

                                                //if (getInfo.straightwin > 0)  // not check winflag
                                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.PVP_Match_Rank, (int)Trigger_Define.ePvPType.MATCH_FREE, userResult.match_rank));
                                                //setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_PVP, (int)Trigger_Define.ePvPType.MATCH_FREE));
                                                retError = TriggerManager.ProgressTrigger(ref tb, userResult.aid, setDataList);
                                            }

                                            if (retError != Result_Define.eResult.SUCCESS)
                                                break;

                                            retUserInfo.Add(setObj);
                                            queryFetcher.SnailLogWrite(ref tb);
                                        }
                                    }
                                }
                                else if (pvpType == PvP_Define.ePvPType.MATCH_RUBY_PVP)
                                {
                                    PvP_Define.ePvPRuby_Grade rubyGrade = (PvP_Define.ePvPRuby_Grade)queryFetcher.QueryParam_FetchInt("pvp_grade", 0);
                                    bool bUseType = (serviceArea == DataManager_Define.eCountryCode.China || serviceArea == DataManager_Define.eCountryCode.Taiwan);

                                    if (rubyGrade > PvP_Define.ePvPRuby_Grade.MATCH_RUBY_PVP_GRADE_NONE && rubyGrade <= PvP_Define.ePvPRuby_Grade.MATCH_RUBY_PVP_GRADE_COUNT)
                                    {
                                        Account userInfo;
                                        int WinnerGetValue = bUseType ? SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.PvP_Gold_Grade_Win_ConstKey[rubyGrade]]) : SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.PvP_Ruby_Grade_Win_ConstKey[rubyGrade]]);
                                        int JoinCostValue = bUseType ? SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.PvP_Gold_Grade_Join_ConstKey[rubyGrade]]) : SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.PvP_Ruby_Grade_Join_ConstKey[rubyGrade]]);

                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.PvPOperationSID.MATCH_RUBY_PVP_RESULT);
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], (long)SnailLog_Define.PvPOperationSID.MATCH_RUBY_PVP_RESULT);

                                        foreach (PvP_Player_Result userResult in userPvP_Result)
                                        {
                                            Ret_PvP_Record_Result setObj = new Ret_PvP_Record_Result(userResult.aid);
                                            PVP_Record getInfo = PvPManager.GetUser_PvP_Record(ref tb, userResult.aid, 0, pvpType);
                                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], userResult.aid);
                                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 1);
                                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);

                                            bool bWin = userResult.match_rank > 0;
                                            setObj.add_user_value = bWin ? WinnerGetValue : 0;
                                            getInfo.totalkill += bWin ? 1 : 0;
                                            getInfo.totaldeath += bWin ? 0 : 1;
                                            setObj.straightwin = getInfo.straightwin = bWin ? getInfo.straightwin + 1 : 0;
                                            setObj.straightlose = getInfo.straightlose = bWin ? 0 : getInfo.straightlose + 1;
                                            retError = PvPManager.SetUser_PvP_Record(ref tb, getInfo);

                                            if (retError == Result_Define.eResult.SUCCESS)
                                            {
                                                if (bWin)
                                                {
                                                    if (bUseType)
                                                    {
                                                        retError = AccountManager.AddUserGold(ref tb, userResult.aid, WinnerGetValue - JoinCostValue);
                                                        userInfo = AccountManager.FlushAccountData(ref tb, userResult.aid, ref retError);
                                                        setObj.total_user_value = userInfo.Gold;
                                                        setObj.price_type = (int)Item_Define.eItemBuyPriceType.PriceType_PayGold;
                                                    }
                                                    else
                                                    {
                                                        retError = AccountManager.AddUserEventCash(ref tb, userResult.aid, WinnerGetValue - JoinCostValue);
                                                        userInfo = AccountManager.FlushAccountData(ref tb, userResult.aid, ref retError);
                                                        setObj.total_user_value = userInfo.Cash + userInfo.EventCash;
                                                        setObj.price_type = (int)Item_Define.eItemBuyPriceType.PriceType_PayCash;
                                                    }
                                                }
                                                else
                                                {
                                                    if (bUseType)
                                                    {
                                                        retError = AccountManager.UseUserGold(ref tb, userResult.aid, JoinCostValue);
                                                        userInfo = AccountManager.FlushAccountData(ref tb, userResult.aid, ref retError);
                                                        setObj.total_user_value = userInfo.Gold;
                                                        setObj.price_type = (int)Item_Define.eItemBuyPriceType.PriceType_PayGold;
                                                    }
                                                    else
                                                    {
                                                        retError = AccountManager.UseUserCash(ref tb, userResult.aid, JoinCostValue);
                                                        userInfo = AccountManager.FlushAccountData(ref tb, userResult.aid, ref retError);
                                                        setObj.total_user_value = userInfo.Cash + userInfo.EventCash;
                                                        setObj.price_type = (int)Item_Define.eItemBuyPriceType.PriceType_PayCash;
                                                    }
                                                }
                                            }

                                            if (retError == Result_Define.eResult.SUCCESS)
                                            {
                                                List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                                                if (bWin)
                                                    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Win_PVP, (int)Trigger_Define.ePvPType.MATCH_RUBY_PVP));

                                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Kill_Count, (int)Trigger_Define.ePvPType.MATCH_RUBY_PVP, (int)Trigger_Define.eKillCountType.Accumulate));
                                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_PVP, (int)Trigger_Define.ePvPType.MATCH_RUBY_PVP));
                                                retError = TriggerManager.ProgressTrigger(ref tb, userResult.aid, setDataList);
                                            }

                                            if (retError != Result_Define.eResult.SUCCESS)
                                                break;

                                            retUserInfo.Add(setObj);
                                            queryFetcher.SnailLogWrite(ref tb);
                                        }
                                    }
                                }
                                else if (pvpType == PvP_Define.ePvPType.MATCH_GUILD_3VS3)
                                {
                                    string guildInfoJson = queryFetcher.QueryParam_Fetch("guild_info", "");
                                    if (string.IsNullOrEmpty(guildInfoJson))
                                        retError = Result_Define.eResult.SYSTEM_PARAM_ERROR;

                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        PvP_GuildWar_Result guildPvP_Result = mJsonSerializer.JsonToObject<PvP_GuildWar_Result>(guildInfoJson);

                                        bool bMyTeamWin = guildPvP_Result.match_rank == 1;
                                        bool bEnemyTeamWin = guildPvP_Result.match_rank == 0;
                                        int myTeamPoint = 0, enemyTeamPoint = 0;

                                        if (bMyTeamWin || bEnemyTeamWin)
                                        {
                                            //int myTeamCount = userPvP_Result.Count(userResult => userResult.match_rank == guildPvP_Result.match_rank);
                                            //int enemyTeamCount = userPvP_Result.Count(userResult => userResult.match_rank != guildPvP_Result.match_rank);

                                            //double myTeamAvgRating = guildPvP_Result.my_rating / (myTeamCount < 1 ? 1 : myTeamCount);
                                            //double enemyTeamAvgRating = guildPvP_Result.enemy_rating / (enemyTeamCount < 1 ? 1 : enemyTeamCount);

                                            double myTeamAvgRating = guildPvP_Result.my_rating / PvP_Define.PvP_GuildPVPUserCount;
                                            double enemyTeamAvgRating = guildPvP_Result.enemy_rating / PvP_Define.PvP_GuildPVPUserCount;

                                            double myTeamWinRate = 1.0f / (1.0f + System.Math.Pow(10, ((enemyTeamAvgRating - myTeamAvgRating) / 400)));
                                            double enemyTeamWinRate = 1.0f / (1.0f + System.Math.Pow(10, ((myTeamAvgRating - enemyTeamAvgRating) / 400)));

                                            int RateConstValue = SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_GUILD_G3VS3_ELOKCONST]);
                                            myTeamPoint = (int)(RateConstValue * ((bMyTeamWin ? 1 : 0) - myTeamWinRate));
                                            myTeamPoint = myTeamPoint < 1 ? 1 : myTeamPoint;
                                            enemyTeamPoint = (int)(RateConstValue * ((bMyTeamWin ? 0 : 1) - enemyTeamWinRate));
                                            enemyTeamPoint = enemyTeamPoint < 1 ? 1 : enemyTeamPoint;
                                        }

                                        int setSeperator = PvPManager.GetSeperater_Month(ref tb);
                                        int rewardWinPoint = SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_GUILD_G3VS3_RESULT_WIN_POINT]);
                                        int rewardLosePoint = SystemData.GetConstValueInt(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_GUILD_G3VS3_RESULT_LOSE_POINT]);
                                        Guild_PVP_Record getMyGuildInfo = PvPManager.GetGuild_PvP_Record(ref tb, guildPvP_Result.my_gid);
                                        Guild_PVP_Record getEnemyGuildInfo = PvPManager.GetGuild_PvP_Record(ref tb, guildPvP_Result.enemy_gid);
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.PvPOperationSID.MATCH_GUILD_3VS3_RESULT);
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], (long)SnailLog_Define.PvPOperationSID.MATCH_GUILD_3VS3_RESULT);

                                        foreach (PvP_Player_Result userResult in userPvP_Result)
                                        {
                                            bool bMyWin = userResult.match_rank > 0;
                                            Ret_PvP_Record_Result setObj = new Ret_PvP_Record_Result(userResult.aid);
                                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], userResult.aid);
                                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 1);
                                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);

                                            setObj.before_rankpoint = userResult.gid == guildPvP_Result.my_gid ? getMyGuildInfo.rankpoint : getEnemyGuildInfo.rankpoint;
                                            setObj.after_rankpoint = userResult.gid == guildPvP_Result.my_gid ? getMyGuildInfo.rankpoint + myTeamPoint : getEnemyGuildInfo.rankpoint + enemyTeamPoint;
                                            setObj.add_user_value = bMyWin ? rewardWinPoint : rewardLosePoint;

                                            Guild_User_PVP_Record getInfo = PvPManager.GetGuild_User_PvP_Record(ref tb, userResult.aid, userResult.gid, PvP_Define.PvP_GuildUser_Monthly_TableName, setSeperator);
                                            getInfo.rating_point += (userResult.gid == guildPvP_Result.my_gid) ? myTeamPoint : enemyTeamPoint;
                                            getInfo.totalkill += userResult.kill_count;
                                            getInfo.totaldeath += userResult.death_count;
                                            getInfo.totaldeath += userResult.death_count;
                                            getInfo.totalwin += bMyWin ? 1 : 0;
                                            getInfo.totallose += bMyWin ? 0 : 1;
                                            setObj.straightwin = getInfo.straightwin = bMyWin ? getInfo.straightwin + 1 : 0;
                                            setObj.straightlose = getInfo.straightlose = bMyWin ? 0 : getInfo.straightlose + 1;
                                            setObj.price_type = (int)Item_Define.eItemBuyPriceType.PriceType_PayGuildPoint;

                                            retError = PvPManager.SetGuild_User_PvP_Record(ref tb, getInfo, userResult.gid);

                                            if (retError == Result_Define.eResult.SUCCESS)
                                                retError = GuildManager.AddGuildContributionPoint(ref tb, userResult.aid, bMyWin ? rewardWinPoint : rewardLosePoint);


                                            if (retError == Result_Define.eResult.SUCCESS)
                                            {
                                                List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                                                if (bMyWin)
                                                    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Win_PVP, (int)Trigger_Define.ePvPType.MATCH_GUILD_WAR));
                                                if (getInfo.straightwin > 0)
                                                    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Straight_PVP, (int)Trigger_Define.ePvPType.MATCH_GUILD_WAR, 0, getInfo.straightwin));
                                                if (userResult.kill_streak > 0)
                                                    //setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Kill_Count, (int)Trigger_Define.ePvPType.MATCH_GUILD_WAR, 0, userResult.kill_streak));
                                                    setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Kill_Count, (int)Trigger_Define.ePvPType.MATCH_GUILD_WAR, (int)Trigger_Define.eKillCountType.KillStreak, userResult.kill_streak));
                                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Kill_Count, (int)Trigger_Define.ePvPType.MATCH_GUILD_WAR, (int)Trigger_Define.eKillCountType.Accumulate, userResult.kill_count));

                                                //setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_PVP, (int)Trigger_Define.ePvPType.MATCH_GUILD_WAR));
                                                retError = TriggerManager.ProgressTrigger(ref tb, userResult.aid, setDataList);
                                            }

                                            if (retError != Result_Define.eResult.SUCCESS)
                                                break;

                                            retUserInfo.Add(setObj);
                                            queryFetcher.SnailLogWrite(ref tb);
                                        }

                                        if (retError == Result_Define.eResult.SUCCESS)
                                        {
                                            getMyGuildInfo.win += bMyTeamWin ? 1 : 0;
                                            getEnemyGuildInfo.win += bEnemyTeamWin ? 1 : 0;
                                            getMyGuildInfo.lose += bMyTeamWin ? 0 : 1;
                                            getEnemyGuildInfo.lose += bEnemyTeamWin ? 0 : 1;

                                            getMyGuildInfo.straightwin = bMyTeamWin ? getMyGuildInfo.straightwin + 1 : 0;
                                            getEnemyGuildInfo.straightwin = bEnemyTeamWin ? getMyGuildInfo.straightwin + 1 : 0;
                                            getMyGuildInfo.straightlose = bEnemyTeamWin ? getMyGuildInfo.straightlose + 1 : 0;
                                            getEnemyGuildInfo.straightlose = bMyTeamWin ? getMyGuildInfo.straightlose + 1 : 0;

                                            getMyGuildInfo.rankpoint += myTeamPoint;
                                            getEnemyGuildInfo.rankpoint += enemyTeamPoint;

                                            retError = PvPManager.SetGuild_PvP_Record(ref tb, getMyGuildInfo);
                                            if (retError == Result_Define.eResult.SUCCESS)
                                                retError = PvPManager.SetGuild_PvP_Record(ref tb, getEnemyGuildInfo);
                                        }
                                    }
                                }

                                foreach (PvP_Player_Result userResult in userPvP_Result)
                                {
                                    userResult.gid = GuildManager.GetGuildInfo(ref tb, userResult.aid).guild_id;
                                    if (retError == Result_Define.eResult.SUCCESS && userResult.gid > 0 && userResult.match_rank > 0)
                                    {
                                        switch (pvpType)
                                        {
                                            case PvP_Define.ePvPType.MATCH_FREE:
                                                retError = GuildManager.AddGuildPoint(ref tb, userResult.gid, userResult.aid, Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.MATCH_FREE]);
                                                break;
                                            case PvP_Define.ePvPType.MATCH_1VS1:
                                                retError = GuildManager.AddGuildPoint(ref tb, userResult.gid, userResult.aid, Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.MATCH_1VS1]);
                                                break;
                                            case PvP_Define.ePvPType.MATCH_GUILD_3VS3:
                                                retError = GuildManager.AddGuildPoint(ref tb, userResult.gid, userResult.aid, Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.MATCH_GUILD_3VS3]);
                                                break;
                                            case PvP_Define.ePvPType.MATCH_OVERLORD:
                                                retError = GuildManager.AddGuildPoint(ref tb, userResult.gid, userResult.aid, Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.MATCH_OVERLORD]);
                                                break;
                                            case PvP_Define.ePvPType.MATCH_TEAM:
                                                retError = GuildManager.AddGuildPoint(ref tb, userResult.gid, userResult.aid, Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.MATCH_TEAM]);
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], 0);

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Record], mJsonSerializer.ToJsonString(retUserInfo));
                                }
                            }
                        }
                        else if (requestOp.Equals("get_ruby_pvp_player_info"))
                        {
                            List<Character_Detail_With_HP> charList = CharacterManager.GetCharacterListWithEquip_HP(ref tb, AID);
                            List<Character_Detail_With_HP> dummyList = new List<Character_Detail_With_HP>();
                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.PvPOperationSID.MATCH_RUBY_PVP);
                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], (long)SnailLog_Define.PvPOperationSID.MATCH_RUBY_PVP);
                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 0);
                            tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);

                            foreach (Character_Detail_With_HP charInfo in charList)
                            {
                                Character_Detail_With_HP dummyInfo = DummyManager.GetCharacterInfoWithEquip(ref tb, charInfo);
                                dummyInfo.equip_passive_soul = charInfo.equip_passive_soul;
                                dummyInfo.equiplist.ForEach(setItem =>
                                {
                                    setItem.aid = charInfo.aid;
                                    setItem.cid = charInfo.cid;
                                    setItem.creation_date = DateTime.Now;
                                }
                                );
                                List<Ret_Equip_Soul_Active> setDummyActiveSoul = new List<Ret_Equip_Soul_Active>();

                                foreach (Ret_Equip_Soul_Active setActiveSoul in charInfo.equip_active_soul)
                                {
                                    Ret_Equip_Soul_Active dummySoul = setActiveSoul;
                                    dummySoul.level = Soul_Define.Soul_Max_Level;
                                    dummySoul.grade = Soul_Define.Soul_Max_Grade;
                                    dummySoul.starlevel = Soul_Define.Soul_Max_Star;
                                    dummySoul.soulequip_id_list = new List<long>();

                                    long setSoulGroup = SoulManager.GetSoul_System_Soul_Active(ref tb, setActiveSoul.soulid).SoulGroup;
                                    long setSoulID = SoulManager.GetSoul_System_Soul_Active(ref tb, setSoulGroup, dummySoul.grade).SoulID;
                                    dummySoul.soulid = setSoulID;

                                    long tempBuff3ID = 0;
                                    if (setActiveSoul.special_buffid1 == 0 && setActiveSoul.special_buffid2 == 0)
                                    {
                                        System_Ruby_PvP_Soul SetSoulBuff = DummyManager.GetSystem_Ruby_PvP_Soul_Info(ref tb, setSoulGroup, charInfo.Class, setActiveSoul.special_buffid1);
                                        dummySoul.special_buffid1 = SetSoulBuff.setBuffID;
                                        dummySoul.special_buffid2 = SetSoulBuff.pairBuffID;
                                        tempBuff3ID = SetSoulBuff.setEpicbuffID;
                                    }
                                    else if (setActiveSoul.special_buffid1 > 0 && setActiveSoul.special_buffid2 == 0)
                                    {
                                        System_Ruby_PvP_Soul SetSoulBuff = DummyManager.GetSystem_Ruby_PvP_Soul_Info(ref tb, setSoulGroup, charInfo.Class, setActiveSoul.special_buffid1);
                                        dummySoul.special_buffid1 = SetSoulBuff.setBuffID;
                                        dummySoul.special_buffid2 = SetSoulBuff.pairBuffID;
                                        tempBuff3ID = SetSoulBuff.setEpicbuffID;
                                    }
                                    else if (setActiveSoul.special_buffid1 == 0 && setActiveSoul.special_buffid2 > 0)
                                    {
                                        System_Ruby_PvP_Soul SetSoulBuff = DummyManager.GetSystem_Ruby_PvP_Soul_Info(ref tb, setSoulGroup, charInfo.Class, setActiveSoul.special_buffid2);
                                        dummySoul.special_buffid1 = SetSoulBuff.pairBuffID;
                                        dummySoul.special_buffid2 = SetSoulBuff.setBuffID;
                                        tempBuff3ID = SetSoulBuff.setEpicbuffID;
                                    }
                                    else if (setActiveSoul.special_buffid1 > 0 && setActiveSoul.special_buffid2 > 0)
                                    {
                                        System_Ruby_PvP_Soul SetSoulBuff1 = DummyManager.GetSystem_Ruby_PvP_Soul_Info(ref tb, setSoulGroup, charInfo.Class, setActiveSoul.special_buffid1);
                                        System_Ruby_PvP_Soul SetSoulBuff2 = DummyManager.GetSystem_Ruby_PvP_Soul_Info(ref tb, setSoulGroup, charInfo.Class, setActiveSoul.special_buffid2);
                                        dummySoul.special_buffid1 = SetSoulBuff1.setBuffID;
                                        dummySoul.special_buffid2 = SetSoulBuff2.setBuffID;
                                        tempBuff3ID = SetSoulBuff1.setEpicbuffID;
                                    }
                                    else
                                    {
                                        dummySoul.special_buffid1 = setActiveSoul.special_buffid1;
                                        dummySoul.special_buffid2 = setActiveSoul.special_buffid2;
                                        tempBuff3ID = setActiveSoul.special_buffid3;
                                    }
                                    dummySoul.special_buffid3 = tempBuff3ID;

                                    setDummyActiveSoul.Add(dummySoul);
                                }
                                dummyInfo.equip_active_soul = setDummyActiveSoul;
                                dummyList.Add(dummyInfo);
                            }

                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_RubyPvP_List], mJsonSerializer.ToJsonString(dummyList));
                        }
                        else if (requestOp.Equals("get_bot_pvp_info"))
                        {
                            User_WarPoint getUserWP = GoldExpedition_Manager.GetUserWarPoint(ref tb, AID);
                            PVP_Record getInfo = PvPManager.GetUser_PvP_Record(ref tb, AID, 0, PvP_Define.ePvPType.MATCH_1VS1);
                            double rangeMin = getInfo.straightlose > 0 ? getInfo.straightlose * PvP_Define.BotMatchBaseRange_Min : PvP_Define.BotMatchBaseRange_Min;
                            double rangeMax = getInfo.straightlose > 0 ? getInfo.straightlose * PvP_Define.BotMatchBaseRange_Max : PvP_Define.BotMatchBaseRange_Max;
                            rangeMin = rangeMin > -GoldExpedition_Define.PercentageDivede ? rangeMin : -GoldExpedition_Define.PercentageDivede;
                            rangeMax = rangeMax > -GoldExpedition_Define.PercentageDivede ? rangeMax : -GoldExpedition_Define.PercentageDivede;

                            PvP_Define.FindEnemyRange setEnemyRange = new PvP_Define.FindEnemyRange(1, rangeMin, rangeMax);

                            double baseMatch = 1.0f;
                            double setMin = (baseMatch + (setEnemyRange.minRate / GoldExpedition_Define.PercentageDivede)) * getUserWP.WAR_POINT;
                            double setMax = (baseMatch + (setEnemyRange.maxRate / GoldExpedition_Define.PercentageDivede)) * getUserWP.WAR_POINT;

                            List<Character_Detail> getList = new List<Character_Detail>();
                            List<User_WarPoint> checkEnemyList = DummyManager.GetDummyUserWarPointList(ref tb, setEnemyRange.getCount, setMin, setMax);
                            //List<User_WarPoint> checkEnemyList = DummyManager.GetDummyUserWarPointListByCharacterWarPoint(ref tb, setEnemyRange.getCount, setMin, setMax);
                            
                            if (checkEnemyList.Count > 0)
                                getList = DummyManager.makeDummyCharacterListInfo(ref tb, AID, checkEnemyList.FirstOrDefault().AID);
                            else
                                retError = Result_Define.eResult.PVP_BOT_INFO_NOT_FOUND;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account_Simple enemyUserInfo = DummyManager.GetSimpleAccountInfo(ref tb, checkEnemyList.FirstOrDefault().AID);

                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Bot_User_Info], mJsonSerializer.ToJsonString(enemyUserInfo));
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Overlord_EnemyInfo], Character_Detail.makeCharacter_DetailListJson(ref getList));
                            }
                        }
                        else if (requestOp.Equals("set_bot_pvp_result"))
                        {
                            string pvpInfoJson = queryFetcher.QueryParam_Fetch("player_info", "");
                            PvP_Define.ePvPType pvpType = (PvP_Define.ePvPType)queryFetcher.QueryParam_FetchInt("pvp_type", (int)PvP_Define.ePvPType.MATCH_NONE);

                            if (string.IsNullOrEmpty(pvpInfoJson) || pvpType == PvP_Define.ePvPType.MATCH_NONE)
                                retError = Result_Define.eResult.SYSTEM_PARAM_ERROR;

                            List<Client_Use_SkillInfo> useSkillList = mJsonSerializer.JsonToObject<List<Client_Use_SkillInfo>>(queryFetcher.QueryParam_Fetch("maxdmg", "[]"));
                            bool bVerify = false;
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Error_Skill_Info errSkill = new Error_Skill_Info();
                                bVerify = SoulManager.CheckSkillVerify(ref tb, AID, useSkillList, ref retError, out errSkill);
                                json = mJsonSerializer.AddJson(json, "errskill", mJsonSerializer.ToJsonString(errSkill));
                                if (!bVerify && retError == Result_Define.eResult.SUCCESS)
                                    retError = Result_Define.eResult.System_Hack_Detected;
                            }

                            if (retError == Result_Define.eResult.SUCCESS && pvpType == PvP_Define.ePvPType.MATCH_1VS1)
                            {
                                List<PvP_Player_Result> userPvP_Result = mJsonSerializer.JsonToObject<List<PvP_Player_Result>>(pvpInfoJson);
                                List<Ret_PvP_Record_Result> retUserInfo = new List<Ret_PvP_Record_Result>();

                                int PvP1vs1BonusStartTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_1VS1REAL_START_TIME_BONUS]);
                                int PvP1vs1BonusEndTime = SystemData.AdminConstValueFetchFromRedis(ref tb, PvP_Define.PvP_Const_Def_Key_List[PvP_Define.ePvPConstDef.DEF_1VS1REAL_END_TIME_BOUNS]);
                                int TodaySecond = PvPManager.GetTodayTotalSeconds();
                                bool bBonus = (PvP1vs1BonusStartTime >= PvP1vs1BonusEndTime && TodaySecond > PvP1vs1BonusStartTime)
                                                || (PvP1vs1BonusStartTime < PvP1vs1BonusEndTime && TodaySecond > PvP1vs1BonusStartTime && TodaySecond < PvP1vs1BonusEndTime);

                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], (long)SnailLog_Define.PvPOperationSID.MATCH_1VS1_RESULT);
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], (long)SnailLog_Define.PvPOperationSID.MATCH_1VS1_RESULT);

                                foreach (PvP_Player_Result userResult in userPvP_Result)
                                {
                                    PVP_Record getInfo = PvPManager.GetUser_PvP_Record(ref tb, userResult.aid, 0, pvpType);
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], userResult.aid);
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 1);
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);

                                    int before_rankpoint = getInfo.totalhonorpoint;
                                    bool bWin = userResult.match_rank > 0;
                                    getInfo.totalhonorpoint += (bWin ? (PvP_Define.PvP_1vs1_WinPoint * (bBonus ? 2 : 1)) : PvP_Define.PvP_1vs1_LosePoint);

                                    getInfo.totalkill += bWin ? 1 : 0;
                                    getInfo.totaldeath += bWin ? 0 : 1;
                                    getInfo.straightwin = bWin ? getInfo.straightwin + 1 : 0;
                                    getInfo.straightlose = bWin ? 0 : getInfo.straightlose + 1;
                                    retError = PvPManager.SetUser_PvP_Record(ref tb, getInfo);

                                    int setGrade = PvPManager.Get1vs1PvPGrade(getInfo.totalhonorpoint);
                                    int highGrade = PvPManager.GetUser_PvP_High_Grade(ref tb, userResult.aid, (int)pvpType);
                                    if (highGrade < setGrade && retError == Result_Define.eResult.SUCCESS)
                                        retError = PvPManager.SetUser_PvP_High_Grade(ref tb, userResult.aid, setGrade, (int)pvpType);

                                    int highPoint = PvPManager.GetUser_PvP_High_Point(ref tb, userResult.aid, (int)pvpType);
                                    if (highPoint < getInfo.totalhonorpoint && retError == Result_Define.eResult.SUCCESS)
                                        retError = PvPManager.SetUser_PvP_High_Point(ref tb, userResult.aid, getInfo.totalhonorpoint, (int)pvpType);

                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                                        if (bWin)
                                            setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Win_PVP, (int)Trigger_Define.ePvPType.MATCH_1VS1));

                                        if (getInfo.straightwin > 0)
                                            setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Straight_PVP, (int)Trigger_Define.ePvPType.MATCH_1VS1, 0, getInfo.straightwin));
                                        setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Kill_Count, (int)Trigger_Define.ePvPType.MATCH_1VS1, (int)Trigger_Define.eKillCountType.Accumulate));

                                        //setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_PVP, (int)Trigger_Define.ePvPType.MATCH_1VS1));
                                        retError = TriggerManager.ProgressTrigger(ref tb, userResult.aid, setDataList);
                                    }

                                    if (retError != Result_Define.eResult.SUCCESS)
                                        break;
                                    Ret_PvP_Record_Result setObj = new Ret_PvP_Record_Result(userResult.aid, before_rankpoint, getInfo.totalhonorpoint, (int)Item_Define.eItemBuyPriceType.PriceType_CombatPoint, 0, 0, getInfo.straightwin, getInfo.straightlose);
                                    retUserInfo.Add(setObj);
                                    queryFetcher.SnailLogWrite(ref tb);
                                }

                                foreach (PvP_Player_Result userResult in userPvP_Result)
                                {
                                    userResult.gid = GuildManager.GetGuildInfo(ref tb, AID).guild_id;
                                    if (retError == Result_Define.eResult.SUCCESS && userResult.gid > 0 && userResult.match_rank > 0)
                                    {
                                        switch (pvpType)
                                        {
                                            case PvP_Define.ePvPType.MATCH_FREE:
                                                retError = GuildManager.AddGuildPoint(ref tb, userResult.gid, userResult.aid, Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.MATCH_FREE]);
                                                break;
                                            case PvP_Define.ePvPType.MATCH_1VS1:
                                                retError = GuildManager.AddGuildPoint(ref tb, userResult.gid, userResult.aid, Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.MATCH_1VS1]);
                                                break;
                                            case PvP_Define.ePvPType.MATCH_GUILD_3VS3:
                                                retError = GuildManager.AddGuildPoint(ref tb, userResult.gid, userResult.aid, Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.MATCH_GUILD_3VS3]);
                                                break;
                                            case PvP_Define.ePvPType.MATCH_OVERLORD:
                                                retError = GuildManager.AddGuildPoint(ref tb, userResult.gid, userResult.aid, Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.MATCH_OVERLORD]);
                                                break;
                                            case PvP_Define.ePvPType.MATCH_TEAM:
                                                retError = GuildManager.AddGuildPoint(ref tb, userResult.gid, userResult.aid, Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.MATCH_TEAM]);
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }

                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], 0);

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Record], mJsonSerializer.ToJsonString(retUserInfo));
                                }
                            }
                        }
                        else if (requestOp.Equals("redis_flush"))
                        {
                            string serverkey = queryFetcher.QueryParam_Fetch("serverkey", string.Empty);

                            if (string.IsNullOrEmpty(serverkey))
                            {
                                RedisConst.GetRedisInstance().RedisFlush(DataManager_Define.RedisServerAlias_System);
                                RedisConst.GetRedisInstance().RedisFlush(DataManager_Define.RedisServerAlias_User);
                            }
                            else
                                RedisConst.GetRedisInstance().RedisFlush(serverkey);
                        }
                        else if (requestOp.Equals("set_chatchannel"))
                        {
                            string setActiveChannel = queryFetcher.QueryParam_Fetch("channel_list", "[]");
                            long userCount = queryFetcher.QueryParam_FetchLong("user_count");
                            List<int> setChannelList = mJsonSerializer.JsonToObject<List<int>>(setActiveChannel);
                            retError = BossRaid.SetChatChannel(setChannelList);
                            //if (serviceArea == DataManager_Define.eCountryCode.China)
                                SnailLogManager.SnailLog_CurrentUser_Log(ref tb, userCount);
                            server_group_config serverInfo = GlobalManager.GetServerGroupConfig(ref tb, TheSoulDBcon.server_group_id);
                            Global_Define.eServerStatus curStatus = serverInfo != null ? (Global_Define.eServerStatus)serverInfo.server_group_status : Global_Define.eServerStatus.Normal;
                            if (GlobalManager.lastStatus == Global_Define.eServerStatus.None)
                                GlobalManager.lastStatus = curStatus;

                            if ((curStatus == Global_Define.eServerStatus.New || curStatus == Global_Define.eServerStatus.Recommand || curStatus == Global_Define.eServerStatus.Normal)
                                && userCount > Global_Define.ServerStatus_Over_Limit)
                            {
                                GlobalManager.lastStatus = curStatus;
                                retError = GlobalManager.SetServerState(ref tb, TheSoulDBcon.server_group_id, Global_Define.eServerStatus.Hot);
                            }
                            else if (curStatus == Global_Define.eServerStatus.Hot && userCount < Global_Define.ServerStatus_Under_Limit)
                            {
                                retError = GlobalManager.SetServerState(ref tb, TheSoulDBcon.server_group_id, GlobalManager.lastStatus);
                            }
                        }
                        else if (requestOp.Equals("get_friends_info"))
                        {
                            Dictionary<long, Friends> myFriendList = FriendManager.GetFriendsListFetchFromDB(ref tb, AID);
                            StringBuilder sb = new StringBuilder();
                            myFriendList.Keys.ToList().ForEach(friend_aid =>
                            {
                                sb.Append(friend_aid); sb.Append(",");
                            });

                            List<RetFriendsInfo> getFriendsInfo = FriendManager.GetFriendsInfoListFromDB(ref tb, sb.ToString());
                            json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_Party_FriendList], mJsonSerializer.ToJsonString(getFriendsInfo));
                        }
                        else if (requestOp.Equals("coupon_send_mail"))
                        {
                            string coupon = queryFetcher.QueryParam_Fetch("coupon");
                            long receiverAID = queryFetcher.QueryParam_FetchLong("receiver");
                            string itemList = queryFetcher.QueryParam_Fetch("coupon_reward_list", "[]");

                            if (receiverAID > 0)
                            {
                                List<Set_Mail_Item> rewardList = new List<Set_Mail_Item>();
                                if (!string.IsNullOrEmpty(itemList))
                                    rewardList = mJsonSerializer.JsonToObject<List<Set_Mail_Item>>(itemList);
                                int sendItemCount = 0;
                                Dictionary<short, List<Set_Mail_Item>> sendMailList = new Dictionary<short, List<Set_Mail_Item>>();

                                foreach (Set_Mail_Item setItem in rewardList)
                                {
                                    Item_Define.eSystemItemType checkType = Item_Define.eSystemItemType.ItemClass_NONE;
                                    Object SysItem = ItemManager.CheckItemType(ref tb, setItem.itemid, ref checkType);

                                    if (checkType == Item_Define.eSystemItemType.ItemClass_Equip)
                                    {
                                        System_Item_Equip itemInfo = ItemManager.GetSystem_Item_Equip(ref tb, setItem.itemid);
                                        short setItemClass = (short)(itemInfo.Class_IndexID == 0 ? 0 : Character_Define.ClassTypeToEnum[itemInfo.EquipClass]);

                                        if (!sendMailList.ContainsKey(setItemClass))
                                            sendMailList[setItemClass] = new List<Set_Mail_Item>();

                                        sendMailList[setItemClass].Add(setItem);
                                    }
                                    else
                                    {
                                        if (!sendMailList.ContainsKey((short)Character_Define.SystemClassType.Class_None))
                                            sendMailList[(short)Character_Define.SystemClassType.Class_None] = new List<Set_Mail_Item>();
                                        sendMailList[(short)Character_Define.SystemClassType.Class_None].Add(setItem);
                                    }
                                }

                                foreach (KeyValuePair<short, List<Set_Mail_Item>> mailList in sendMailList)
                                {
                                    List<Set_Mail_Item> setMailItem = new List<Set_Mail_Item>();

                                    foreach (Set_Mail_Item setItem in mailList.Value)
                                    {
                                        if (setItem.itemid > 0 && setItem.itemea > 0)
                                        {
                                            sendItemCount++;
                                            setMailItem.Add(setItem);
                                        }
                                        if (sendItemCount >= Mail_Define.Mail_MaxItem)
                                        {
                                            retError = MailManager.SendMail(ref tb, ref setMailItem, AID, 0, "", Mail_Define.Coupon_Mail_Title, Mail_Define.Coupon_Mail_Body, Mail_Define.Mail_VIP_CloseTime_Min);
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
                                        retError = MailManager.SendMail(ref tb, ref setMailItem, AID, 0, "", Mail_Define.Coupon_Mail_Title, Mail_Define.Coupon_Mail_Body, Mail_Define.Mail_VIP_CloseTime_Min);
                                    }

                                }
                            }
                        }
                        else
                        {
                            retError = Result_Define.eResult.System_Unknown_Operation;
                        }

                        retJson = queryFetcher.Render(json.ToJson(), retError);
                    }
                    else if (requestOp.Equals("load_ip_table"))
                    {
                        TheSoulDBcon.Snail_ips = GlobalManager.GetSnailIPList(ref tb);
                        retJson = queryFetcher.Render("", Result_Define.eResult.SUCCESS);
                    }
                    else
                    {
                        retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(DefineError.System_Unknown_Operation), Result_Define.eResult.System_Unknown_Operation);
                    }
                }
                catch (Exception errorEx)
                {
                    string error = "";
                    error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
                    error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
                    error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(error), Result_Define.eResult.System_Exception);
                }
                finally
                {
                    //if (AID > 0)
                    //    queryFetcher.CheckSnail_ID(ref tb, AID);
                    queryFetcher.SetShowLogMode = tb.EndTransaction(queryFetcher.Render_errorFlag);
                    queryFetcher.ErrorLogWrite(retJson, ref tb);

                    //Response.Write(queryFetcher.GetReqParams());
                    //string DBLog = mJsonSerializer.ToJsonString(queryFetcher.GetDBLog());
                    ////error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                    //Response.Write(DBLog);
                    
                    tb.Dispose();
                }  
            }
        }
    }
}