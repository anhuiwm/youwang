using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSoul.DataManager
{
    public static class Shop_Define
    {
        public const string Shop_Info_DB = "sharding";
        public const string Shop_Info_Surfix = "Info";

        public const string Shop_Vip_TableName = "System_VIP";
        public const string Shop_AccumPrice_TableName = "System_Shop_Cash_Item";

        public const string Shop_Gacha_Level_TableName = "System_Gacha_Level";
        public const string Shop_Gacha_BoxGroup_TableName = "System_Gacha";
        public const string Shop_Gacha_Box_TableName = "System_Gacha_Box";
        public const string Shop_Gacha_Group_TableName = "System_Gacha_Group";
        public const string Shop_UserGacha_Info_TableName = "User_Gacha_Info";
        public const string Shop_User_Gacha_Special_Info_TableName = "User_Gacha_Special_Info";

        public const string Shop_GoodsCode_TableName = "System_Shop_Goods_Code";
        public const string Shop_Limit_TableName = "System_Shop_Limit_List";
        public const string Shop_System_Package_List_TableName = "System_Package_List";
        public const string Shop_System_Package_RewardBox_TableName = "System_Package_RewardBox";
        public const string Shop_System_Package_Cheap_TableName = "System_Package_Cheap";
        public const string Shop_System_Package_Cheap_RewardBox_TableName = "System_Package_Cheap_RewardBox";
        //public const string Shop_System_Package_Cheap_Schedule_TableName = "System_Package_Cheap_Schedule";

        public const string Shop_User_Buy_TableName = "User_Shop_Buy";
        public const string Shop_User_Reset_TableName = "User_Shop_Reset";
        public const string Shop_User_BillingInfo_TableName = "User_Billing_List";
        public const string Shop_User_BillingError_TableName = "User_Billing_Error";
        public const string Shop_User_Billing_AuthKey_TableName = "User_Billing_AuthKey";
        public const string Shop_User_Shop_Subscription_TableName = "User_Shop_Subscription";
        public const string Shop_User_Shop_Subscription_Week_TableName = "User_Shop_Subscription_Week";
        public const string Shop_User_Shop_BlackMarket_TableName = "User_Shop_BlackMarket";
        public const string Shop_User_Shop_TreasureBox_TableName = "User_Shop_TreasureBox";
        public const int Shop_TreasureBox_ItemCount = 10;

        public const string Shop_Gacha_Best_TableName = "System_Gacha_Best";
        public const string Shop_Gacha_Best_DropGrop_TableName = "System_Gacha_Best_DropGrop";

        public const int Shop_Default_Sale_Rate = 0;
        public const eShopSaleType Shop_Default_Sale_Type = eShopSaleType.RegularSale;

        public const int Shop_FreeGacha_Minutes = 10;
        public const int Shop_FreeGacha_MaxCount = 5;
        public const int Shop_Free_Premium_Hours = 24;
        public const int Shop_DayOfYear = 365;
        public const int Shop_UnlimitDay = Shop_DayOfYear * 100;
        public const int Shop_OverPricePrefix = 10000;

        public enum eShopItemType
        {
            None = 0,
            Item,
            Cash,
            Package,
            Chep_Package,
        }

        public enum eShopType
        {
            None = -1,
            Billing = 0,    // 일반 상점 - 결제
            Cash = 1,       // 일반 상점 - 루비
            Guild = 2,          // 길드 상점
            Expedition = 3,     // 황금원정단 상점
            PvP_1vs1 = 4,       // 투신전 상점
            PvP_FreeForAll = 5, // 난전 상점
            Party = 6,          // 협력전 상점
            Ranking = 7,        // 패왕의길 상점
            BlackMarket = 8,    // 암시장 상점
        }

        public static readonly Dictionary<eShopType, string> Shop_Type_TableList = new Dictionary<eShopType, string>()
        {
            { eShopType.Cash,           "System_Shop_Goods" },
            { eShopType.Billing,        "System_Shop_Goods" },
            { eShopType.Guild,          "System_Shop_Guild" },
            { eShopType.Expedition,     "System_Shop_Expedition" },
            { eShopType.PvP_1vs1,       "System_Shop_1vs1Real" },
            { eShopType.PvP_FreeForAll, "System_Shop_FreeForAll" },
            { eShopType.Party,          "System_Shop_Party" },
            { eShopType.Ranking,        "System_Shop_Ranking" },
            { eShopType.BlackMarket,        "System_Shop_BlackMarket" },
        };

        public static readonly Dictionary<int, string> Shop_Billing_ProductID = new Dictionary<int, string>()
        {
            { 1,    "com.snailgames.yhsg.gold.1" },
            { 3,    "com.snailgames.yhsg.gold.3" },
            { 6,    "com.snailgames.yhsg.gold.6" },
            { 30,   "com.snailgames.yhsg.gold.30" },
            { 98,   "com.snailgames.yhsg.gold.98" },
            { 198,  "com.snailgames.yhsg.gold.198" },
            { 328,  "com.snailgames.yhsg.gold.328" },
            { 648,  "com.snailgames.yhsg.gold.648" },
            { 10198, "com.snailgames.yhsg.gold.yearcard" },
            { 10068, "com.snailgames.yhsg.gold.quarterlycard" },
            { 10030, "com.snailgames.yhsg.gold.monthlycard" },
            { 10006, "com.snailgames.yhsg.gold.weekcard" },
        };

        public enum eBillingStatus
        {
            Complete = 0,
            CreateOrderID = 1,
            Fail = 2,
            Error = 3,
            GmComplete = 100,
        }

        public const int shopBillingError = -1;

        public enum eBillingType
        {
            None = 0,
            UnityDebug = 1000,
            iOS_Appstore = 2000,
            iOS_JailBreak = 3000,
            Android_3rdParty = 4000,
            //=================== for Korea publishing
            Kr_aOS_Google = 11000,
            Kr_iOS_Appstore = 12000,
            Kr_aOS_OneStore = 13000,
            //=================== for Global publishing
            Global_aOS_Google = 21000,
            //new(需要实现)
            Global_aOS_PayPal = 21001,
            Global_aOS_MOL = 21002,
            Global_aOS_MyCard = 21003,
            Global_aOS_MOLPin = 21004,//mol点卡
            //end

            Global_iOS_Appstore = 22000,
            //new(需要实现)
            Global_iOS_PayPal = 22001,
            Global_iOS_MOL = 22002,
            Global_iOS_MyCard = 22003,
            Global_iOS_MOLPin = 22004,//mol点卡
            //end
            //=================== for Taiwan aicombo publishing
            Tw_iOS_Appstore = 31000,
            Tw_aOS_GooglePlaystore = 32000,
            Tw_MOL = 33001,
            Tw_mycard_TW = 33002,
            Tw_mycard_HK = 33003,
            Tw_gash_TW = 33004,
            Tw_gash_HK = 33005,
            Tw_TWM = 33006,
            Tw_aicombo = 33007,
            Tw_pepay = 33008,

            //=================== for wannaplay mfun publishing(新马泰还没有完成需要现在自己做的)
            mfun_aOS_Google = 40000,
            mfun_aOS_Paypal = 40001,   
            mfun_aOS_MOL = 40002,
            mfun_aOS_Mycard = 40003,
            mfun_aOS_MOLPin = 40004,//mol点卡

            mfun_iOS_Appstore = 41000,
            mfun_iOS_Paypal = 41001,          
            mfun_iOS_MOL = 41002,
            mfun_iOS_Mycard = 41003,
            mfun_iOS_MOLPin = 41004,//mol点卡

            //=================== for wannaplay yuenan publishing(越南还没有完成需要现在自己做的)
            yuenan_aOS_Google = 50000,
            yuenan_aOS_Mobile = 50001,

            yuenan_iOS_Appstore = 51000,
            yuenan_iOS_Mobile = 51001,

            yuenan_iOS_end = 51999
 
        }

        public enum eShopSaleType
        {
            RegularSale = 0,
            BuyOnceSale = 1,
            Subscription = 2,
            DiscountSale = 3,   // not use yet
            StackPrice = 4,
            Subscription_Upgrade = 5,
            Subscription_Week = 6,
            Package = 100, //gmtool use. package item
        }

        public enum eShopGroupType
        {
            Billing = 1,
            Cash = 2,
        }

        public enum eCashBuyGroupType
        {
            NONE = 0,
            PVECOIN = 1,
            PVPCOIN = 2,
            GOLD = 3,
            BREAKING_STONE = 4,
            ORB_LV1 = 5,
            ORB_LV2 = 6,
        }

        public enum eGachaType
        {
            NORMAL_TRY_ONE = 1,
            NORMAL_TRY_TEN,
            PREMIUM_TRY_ONE,
            PREMIUM_TRY_TEN,
            BEST_TRY_ONE,
            TREASURE_BOX_GOLD = 30,
            TREASURE_BOX_CASH = 40,
            TREASURE_BOX_SPECIAL = 41,
        }

        /*
         0: 일반 상품(이벤트 할인 상품)
        1: 재 구입 불가 상품
        2: 30일 상품 (매일 10개)
        3: 운영툴 이벤트 할인상품 제외
        4: 구매 횟수 누적 상품  -  모든 상품 총 횟수를 누적하므로 우선 제외.
         */

        public enum eShopReturnKeys
        {
            ShopItemList,
            ShopPackageList,
            BuyPriceType,
            BuyPriceValue,
            RetGold,
            RetRuby,
            RetCurrentCount,
            RetLeftSubscription,
            RetLeftWeekSubscription,
            RetRemainTime,
            RetBillingPlatform,
            RetBillingID,
            RetProductID,

            VIPRewardList,
            RetUnResolveBuyCount,
            RetUnResolveList,
            RetItemID,
            RetResetRuby,

            RetGoldBuyCount,
            RetRubyBuyCount,
            RetReplaceBoxItem,
        }

        public static readonly Dictionary<eShopReturnKeys, string> Shop_Ret_KeyList = new Dictionary<eShopReturnKeys, string>()
        {
            { eShopReturnKeys.ShopItemList,           "shopitemlist"          },
            { eShopReturnKeys.ShopPackageList,           "packagelist"          },
            { eShopReturnKeys.BuyPriceType,           "buypricetype"          },
            { eShopReturnKeys.BuyPriceValue,          "buypricevalue"          },
            { eShopReturnKeys.RetGold,                "usegold"          },
            { eShopReturnKeys.RetRuby,                "useruby"          },
            { eShopReturnKeys.RetCurrentCount,        "currentresetcount"          },
            { eShopReturnKeys.RetLeftSubscription,        "leftsubscription"          },
            { eShopReturnKeys.RetLeftWeekSubscription,        "leftweeksubscription"          },
            { eShopReturnKeys.RetRemainTime,        "shopremainsec"          },
            { eShopReturnKeys.RetBillingPlatform,        "billingplatform"          },
            { eShopReturnKeys.RetBillingID,        "billingid"          },
            { eShopReturnKeys.RetProductID,        "product_id"          },
            { eShopReturnKeys.VIPRewardList,        "vip_reward_list"          },
            { eShopReturnKeys.RetUnResolveBuyCount,        "unresolve_count"          },
            { eShopReturnKeys.RetUnResolveList,        "unresolve_list"          },
            { eShopReturnKeys.RetItemID,        "shop_item_id"          },
            { eShopReturnKeys.RetResetRuby,        "reset_ruby"          },
            { eShopReturnKeys.RetGoldBuyCount,        "gold_buy_count"          },
            { eShopReturnKeys.RetRubyBuyCount,        "ruby_buy_count"          },
            { eShopReturnKeys.RetReplaceBoxItem,        "replace_box_item"          },
        };

        public enum eShopConstDef
        {
            DEF_1VS1REAL_RESET_COST_RUBY,
            DEF_GUILD_SHOP_RESET_COST_RUBY,
            DEF_EXPEDITION_SHOP_RESET_COST_RUBY,
            DEF_BATTLE_RANKING_RESET_COST_RUBY,
            DEF_HONOR_SHOP_RESET_COST_RUBY,
            DEF_3PVE_SHOP_RESET_COST_RUBY,
            DEF_BLACKMARKET_SHOP_RESET_COST_RUBY,
            ADMIN_CONST_DEF_BLACKMARKET_OPEN_DAY,
            ADMIN_CONST_DEF_BLACKMARKET_OPEN_START_TIME,
            ADMIN_CONST_DEF_BLACKMARKET_OPEN_END_TIME,
            ADMIN_CONST_DEF_SHOP_NEW_ON_OFF,
            ADMIN_GACHA_BEST_ON_OFF,
            ADMIN_PACKAGE_CHEAP_ON_OFF,
            ADMIN_CONST_DEF_COUPON_ON_OFF,
            ADMIN_CONST_DEF_COUPON_IOS_ON_OFF,
            ADMIN_CONST_DEF_GLOBAL_PVP_MATCHING_ON_OFF,

            DEF_GACHA_SPECIAL_COUNT_01,
            DEF_GACHA_SPECIAL_COUNT_02,
            DEF_GACHA_SPECIAL_COUNT_03,
            DEF_GACHA_SPECIAL_COUNT_04,

            DEF_GACHA_NORMAL_COUNT_01,
            DEF_GACHA_NORMAL_COUNT_02,
            DEF_GACHA_NORMAL_COUNT_03,
            DEF_GACHA_NORMAL_COUNT_04,

            DEF_GACHA_TYPE_SPECIAL_01,
            DEF_GACHA_TYPE_SPECIAL_02,
            DEF_GACHA_TYPE_SPECIAL_03,
            DEF_GACHA_TYPE_SPECIAL_04,

            DEF_GACHA_TYPE_NORMAL_01,
            DEF_GACHA_TYPE_NORMAL_02,
            DEF_GACHA_TYPE_NORMAL_03,
            DEF_GACHA_TYPE_NORMAL_04,
            DEF_GACHA_SOULPART_COUNT_STAR_01,
            DEF_GACHA_SOULPART_COUNT_STAR_02,
            DEF_GACHA_SOULPART_COUNT_STAR_03,
            DEF_GACHA_SOULPART_COUNT_STAR_04,
            DEF_GACHA_SOULPART_COUNT_STAR_05,
            DEF_GACHA_SOUL_COUNT_STAR_01,
            DEF_GACHA_SOUL_COUNT_STAR_02,
            DEF_GACHA_SOUL_COUNT_STAR_03,
            DEF_GACHA_SOUL_COUNT_STAR_04,
            DEF_GACHA_SOUL_COUNT_STAR_05,

            DEF_GACHA_OPENLEVEL_SPECIAL,
            DEF_GACHASTORE_SPEICAL_COUNT_01,
        }

        public static readonly Dictionary<eShopConstDef, string> Shop_Const_Def_Key_List = new Dictionary<eShopConstDef, string>()
        {
            { eShopConstDef.DEF_1VS1REAL_RESET_COST_RUBY, "DEF_1VS1REAL_RESET_COST_RUBY"},
            { eShopConstDef.DEF_GUILD_SHOP_RESET_COST_RUBY, "DEF_GUILD_SHOP_RESET_COST_RUBY"},
            { eShopConstDef.DEF_EXPEDITION_SHOP_RESET_COST_RUBY, "DEF_EXPEDITION_SHOP_RESET_COST_RUBY"},
            { eShopConstDef.DEF_BATTLE_RANKING_RESET_COST_RUBY, "DEF_BATTLE_RANKING_RESET_COST_RUBY"},
            { eShopConstDef.DEF_HONOR_SHOP_RESET_COST_RUBY, "DEF_HONOR_SHOP_RESET_COST_RUBY"},
            { eShopConstDef.DEF_3PVE_SHOP_RESET_COST_RUBY, "DEF_3PVE_SHOP_RESET_COST_RUBY"},
            { eShopConstDef.DEF_BLACKMARKET_SHOP_RESET_COST_RUBY, "DEF_BLACKMARKET_SHOP_RESET_COST_RUBY"},
            
            { eShopConstDef.ADMIN_CONST_DEF_BLACKMARKET_OPEN_DAY, "DEF_BLACKMARKET_OPEN_DAY"},
            { eShopConstDef.ADMIN_CONST_DEF_BLACKMARKET_OPEN_START_TIME, "DEF_BLACKMARKET_OPEN_START_TIME"},
            { eShopConstDef.ADMIN_CONST_DEF_BLACKMARKET_OPEN_END_TIME, "DEF_BLACKMARKET_OPEN_END_TIME"},
            { eShopConstDef.ADMIN_CONST_DEF_SHOP_NEW_ON_OFF, "DEF_SHOP_NEW_ON_OFF"},
            { eShopConstDef.ADMIN_GACHA_BEST_ON_OFF, "GACHA_BEST_ON_OFF"},
            { eShopConstDef.ADMIN_PACKAGE_CHEAP_ON_OFF, "PACKAGE_CHEAP_ON_OFF"},
            { eShopConstDef.ADMIN_CONST_DEF_COUPON_ON_OFF, "ADMIN_CONST_DEF_COUPON_ON_OFF"},
            { eShopConstDef.ADMIN_CONST_DEF_COUPON_IOS_ON_OFF, "ADMIN_CONST_DEF_COUPON_IOS_ON_OFF"},            
            { eShopConstDef.ADMIN_CONST_DEF_GLOBAL_PVP_MATCHING_ON_OFF, "DEF_GLOBAL_PVP_MATCHING_NEWWAY_ON_OFF"},
            
            { eShopConstDef.DEF_GACHA_SPECIAL_COUNT_01, "DEF_GACHA_SPECIAL_COUNT_01"},
            { eShopConstDef.DEF_GACHA_SPECIAL_COUNT_02, "DEF_GACHA_SPECIAL_COUNT_02"},
            { eShopConstDef.DEF_GACHA_SPECIAL_COUNT_03, "DEF_GACHA_SPECIAL_COUNT_03"},
            { eShopConstDef.DEF_GACHA_SPECIAL_COUNT_04, "DEF_GACHA_SPECIAL_COUNT_04"},
            { eShopConstDef.DEF_GACHA_NORMAL_COUNT_01, "DEF_GACHA_NORMAL_COUNT_01"},
            { eShopConstDef.DEF_GACHA_NORMAL_COUNT_02, "DEF_GACHA_NORMAL_COUNT_02"},
            { eShopConstDef.DEF_GACHA_NORMAL_COUNT_03, "DEF_GACHA_NORMAL_COUNT_03"},
            { eShopConstDef.DEF_GACHA_NORMAL_COUNT_04, "DEF_GACHA_NORMAL_COUNT_04"},
            { eShopConstDef.DEF_GACHA_TYPE_SPECIAL_01, "DEF_GACHA_TYPE_SPECIAL_01"},
            { eShopConstDef.DEF_GACHA_TYPE_SPECIAL_02, "DEF_GACHA_TYPE_SPECIAL_02"},
            { eShopConstDef.DEF_GACHA_TYPE_SPECIAL_03, "DEF_GACHA_TYPE_SPECIAL_03"},
            { eShopConstDef.DEF_GACHA_TYPE_SPECIAL_04, "DEF_GACHA_TYPE_SPECIAL_04"},
            { eShopConstDef.DEF_GACHA_TYPE_NORMAL_01, "DEF_GACHA_TYPE_NORMAL_01"},
            { eShopConstDef.DEF_GACHA_TYPE_NORMAL_02, "DEF_GACHA_TYPE_NORMAL_02"},
            { eShopConstDef.DEF_GACHA_TYPE_NORMAL_03, "DEF_GACHA_TYPE_NORMAL_03"},
            { eShopConstDef.DEF_GACHA_TYPE_NORMAL_04, "DEF_GACHA_TYPE_NORMAL_04"},
            { eShopConstDef.DEF_GACHA_SOULPART_COUNT_STAR_01, "DEF_GACHA_SOULPART_COUNT_STAR_01"},
            { eShopConstDef.DEF_GACHA_SOULPART_COUNT_STAR_02, "DEF_GACHA_SOULPART_COUNT_STAR_02"},
            { eShopConstDef.DEF_GACHA_SOULPART_COUNT_STAR_03, "DEF_GACHA_SOULPART_COUNT_STAR_03"},
            { eShopConstDef.DEF_GACHA_SOULPART_COUNT_STAR_04, "DEF_GACHA_SOULPART_COUNT_STAR_04"},
            { eShopConstDef.DEF_GACHA_SOULPART_COUNT_STAR_05, "DEF_GACHA_SOULPART_COUNT_STAR_05"},
            { eShopConstDef.DEF_GACHA_SOUL_COUNT_STAR_01, "DEF_GACHA_SOUL_COUNT_STAR_01"},
            { eShopConstDef.DEF_GACHA_SOUL_COUNT_STAR_02, "DEF_GACHA_SOUL_COUNT_STAR_02"},
            { eShopConstDef.DEF_GACHA_SOUL_COUNT_STAR_03, "DEF_GACHA_SOUL_COUNT_STAR_03"},
            { eShopConstDef.DEF_GACHA_SOUL_COUNT_STAR_04, "DEF_GACHA_SOUL_COUNT_STAR_04"},
            { eShopConstDef.DEF_GACHA_SOUL_COUNT_STAR_05, "DEF_GACHA_SOUL_COUNT_STAR_05"},
            { eShopConstDef.DEF_GACHA_OPENLEVEL_SPECIAL, "DEF_GACHA_OPENLEVEL_SPECIAL"},
            { eShopConstDef.DEF_GACHASTORE_SPEICAL_COUNT_01, "DEF_GACHASTORE_SPEICAL_COUNT_01"},            
        };

        public static readonly eShopConstDef[] Shop_Gacha_Special_Premium_Count_Def_List = 
        { 
            eShopConstDef.DEF_GACHA_SPECIAL_COUNT_01,
            eShopConstDef.DEF_GACHA_SPECIAL_COUNT_02,
            eShopConstDef.DEF_GACHA_SPECIAL_COUNT_03,
            eShopConstDef.DEF_GACHA_SPECIAL_COUNT_04,
        };

        public static readonly eShopConstDef[] Shop_Gacha_Special_Premium_GetType_Def_List = 
        { 
            eShopConstDef.DEF_GACHA_TYPE_SPECIAL_01,
            eShopConstDef.DEF_GACHA_TYPE_SPECIAL_02,
            eShopConstDef.DEF_GACHA_TYPE_SPECIAL_03,
            eShopConstDef.DEF_GACHA_TYPE_SPECIAL_04,
        };

        public static readonly eShopConstDef[] Shop_Gacha_Special_Normal_Count_Def_List = 
        { 
            eShopConstDef.DEF_GACHA_NORMAL_COUNT_01,
            eShopConstDef.DEF_GACHA_NORMAL_COUNT_02,
            eShopConstDef.DEF_GACHA_NORMAL_COUNT_03,
            eShopConstDef.DEF_GACHA_NORMAL_COUNT_04,
        };

        public static readonly eShopConstDef[] Shop_Gacha_Special_Normal_GetType_Def_List = 
        { 
            eShopConstDef.DEF_GACHA_TYPE_NORMAL_01,
            eShopConstDef.DEF_GACHA_TYPE_NORMAL_02,
            eShopConstDef.DEF_GACHA_TYPE_NORMAL_03,
            eShopConstDef.DEF_GACHA_TYPE_NORMAL_04,
        };

        public static readonly eShopConstDef[] Shop_Gacha_SoulParts_Count_Def_List = 
        { 
            eShopConstDef.DEF_GACHA_SOULPART_COUNT_STAR_01,
            eShopConstDef.DEF_GACHA_SOULPART_COUNT_STAR_01,
            eShopConstDef.DEF_GACHA_SOULPART_COUNT_STAR_02,
            eShopConstDef.DEF_GACHA_SOULPART_COUNT_STAR_03,
            eShopConstDef.DEF_GACHA_SOULPART_COUNT_STAR_04,
            eShopConstDef.DEF_GACHA_SOULPART_COUNT_STAR_05,
        };

        public static readonly eShopConstDef[] Shop_Gacha_Soul_Count_Def_List = 
        { 
            eShopConstDef.DEF_GACHA_SOUL_COUNT_STAR_01,
            eShopConstDef.DEF_GACHA_SOUL_COUNT_STAR_01,
            eShopConstDef.DEF_GACHA_SOUL_COUNT_STAR_02,
            eShopConstDef.DEF_GACHA_SOUL_COUNT_STAR_03,
            eShopConstDef.DEF_GACHA_SOUL_COUNT_STAR_04,
            eShopConstDef.DEF_GACHA_SOUL_COUNT_STAR_05,
        };

        public const int Shop_Gacha_Special_MaxGetGroup = 3;

        //public static readonly Dictionary<eShopConstDef, int> ShopGacha_Special_NextCheck = new Dictionary<eShopConstDef, eShopConstDef>()
        //{
        //    { eShopConstDef.DEF_GACHA_SPECIAL_COUNT_01, eShopConstDef.DEF_GACHA_SPECIAL_COUNT_02},
        //    { eShopConstDef.DEF_GACHA_SPECIAL_COUNT_02, eShopConstDef.DEF_GACHA_SPECIAL_COUNT_03},
        //    { eShopConstDef.DEF_GACHA_SPECIAL_COUNT_03, eShopConstDef.DEF_GACHA_SPECIAL_COUNT_04},
        //    { eShopConstDef.DEF_GACHA_SPECIAL_COUNT_04, eShopConstDef.DEF_GACHA_SPECIAL_COUNT_04},
        //};

        //암시장 오픈요일
        [Flags]
        public enum DaysWeek
        {
            NONE = 0,
            MON = 1,
            TUE = 2,
            WED = 4,
            THU = 8,
            FRI = 16,
            SAT = 32,
            SUN = 64
        };
    }
}
