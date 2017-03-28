using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheSoul.DataManager;

namespace TheSoulGMTool
{
    public class GMData_Define
    {
        public const string GmDBName = "webadmin";
        public const string GlobalDBName = "global";
        public const string CommonDBName = "common";
        public const string CommonLogDBName = "commonLog";
        public const string ShardingDBName = "sharding";
        public const string LogDBName = "log";

        public const string GmUserTable = "admin_user";
        public const string GmUserAuthTable = "admin_userAuth";
        public const string GmLargeMenu = "admin_large_menu";
        public const string GmMenu = "admin_menu";
        public const string GmMenuName = "admin_menuName";
        public const string GmControlLog = "GM_ControlLog";
        public const string GMItemChargeLogTable = "GM_ItemChargeLog";
        public const string GMEventLogTable = "Gm_eventLog";
        public const string GMToolLanguageCodeTable = "admin_language_code";

        public const string LineNoticeTable = "Admin_LineNotice";
        public const string NoticeTable = "Admin_Notice";

        public const string SystemTriggerTypeTable = "System_TriggerType";
        public const string AdminSystemItemTable = "Admin_System_Item";
        public const string AdminStringNamingTable = "System_StringNaming";

        public const string AdminSystemGachaBestIndexTable = "Admin_System_Gacha_Best_Index";
        public const string AdminSystemShopBlackMarketndexTable = "Admin_System_Shop_Blackmarket_Index";
        public const string AdminSystemMailNoticeIndexTable = "Admin_System_MailNotice_Index";
        public const string AdminSystemEventGroupIndexTable = "Admin_System_EventGroup";
        public const string UserRestrictLogTable = "user_restrict_log";

        public const string SystemProductIDTable = "System_Product_ID";

        public const string AdminSystemSoulGroupActiveTable = "Admin_System_SoulGroup_Active";

        public const long AdminSender = 0;
        public const string AdminSenerName = "STRING_UI_MAIL_SYSTEM_012";//운영자 스트링코드

        public const string SystemLogOperation = "system_log_operation";

        public const string SuperAdminID = "superadmin";

        public const int pageSize = 15;
        public const int pageBlock = 10;

        public const int AllLogSearchMinDay = -15;

        public static readonly Dictionary<int, string> Global_Index_TableList = new Dictionary<int, string>()
        {
            { 1, "Admin_SystemID" },
            { 2, "Admin_System_RewardID" },
        };

        public enum eShopProduct
        {
            darkblaze_price_1100 = 1100,
            darkblaze_price_3300 = 3300,
            darkblaze_price_5500 = 5500,
            darkblaze_price_11000 = 11000,
            darkblaze_price_33000 = 33000,
            darkblaze_price_55000 = 55000,
            darkblaze_price_110000 = 110000,
        };

        public enum eChinaShopProduct
        {
            ts01 = 60,
            ts02 = 50,
            ts03 = 30,
            ts04 = 15,
            ts05 = 5,
            ts06 = 1,
        };

        public enum eChinaShopCheapProduct
        {
            ts08 = 30,
            ts09 = 15,
        };

        public static readonly Dictionary<int, string> eOneStoreProductID = new Dictionary<int, string>()
        {
            { 1100, "" },
            { 3300, "0910060211" },
            { 5500, "0910060212" },
            { 11000, "0910060213" },
            { 33000, "0910060214" },
            { 55000, "0910060215" },
            { 110000, "0910060216" },
        };

        public static readonly Dictionary<int, string> eShopProductID = new Dictionary<int, string>()
        {
            { 1100, "darkblaze_price_1100" },
            { 3300, "darkblaze_price_3300" },
            { 5500, "darkblaze_price_5500" },
            { 11000, "darkblaze_price_11000" },
            { 33000, "darkblaze_price_33000" },
            { 55000, "darkblaze_price_55000" },
            { 110000, "darkblaze_price_110000" },
        };

        public static readonly Dictionary<int, int> eShopProductTier = new Dictionary<int, int>()
        {
            { 1100, 1 },
            { 3300, 3 },
            { 5500, 5 },
            { 11000, 9 },
            { 33000, 27 },
            { 55000, 44 },
            { 110000, 58 },
        };

        public enum eGlobalNoticeSearch
        {
            version = 0,
            active,
            platform,
            
        };

        public static readonly Dictionary<eGlobalNoticeSearch, string> eGlobalNoticeSearchKey = new Dictionary<eGlobalNoticeSearch, string>()
        {
            { eGlobalNoticeSearch.version, "target_version" },
            { eGlobalNoticeSearch.platform, "billing_platform_type" },
            { eGlobalNoticeSearch.active, "active" },
        };

        public enum GMDeleteItemType
        {
            ItemClass_Equip,
            Item,
            Soul_Parts,
            Soul_Equip,
            ItemClass_Orb,
        }
        public static readonly Dictionary<string, GMDeleteItemType> DeleteItemTypeList = new Dictionary<string, GMDeleteItemType>(){
            { "ItemClass_Equip", GMDeleteItemType.ItemClass_Equip},
            { "Item", GMDeleteItemType.Item },
            { "Soul_Parts", GMDeleteItemType.Soul_Parts },
            { "Soul_Equip", GMDeleteItemType.Soul_Equip },
            { "ItemClass_Orb", GMDeleteItemType.ItemClass_Orb },
        };

        public static readonly Dictionary<GMDeleteItemType, string> InvenTableList = new Dictionary<GMDeleteItemType, string>()
        {
            { GMDeleteItemType.ItemClass_Equip, Item_Define.Item_User_Inven_Table },
            { GMDeleteItemType.Item, Item_Define.Item_User_Inven_Table },
            { GMDeleteItemType.Soul_Parts, Soul_Define.User_ActiveSoul_Table },
            { GMDeleteItemType.Soul_Equip, Soul_Define.User_Soul_Equip_Inven_Table },
            { GMDeleteItemType.ItemClass_Orb, Item_Define.Item_User_Orb_Inven_Table },
        };

        public static readonly Dictionary<GMDeleteItemType, string> UserInvenTableSeq = new Dictionary<GMDeleteItemType, string>()
        {
            { GMDeleteItemType.ItemClass_Equip, "invenseq" },
            { GMDeleteItemType.Item, "invenseq" },
            { GMDeleteItemType.Soul_Parts, "soulseq" },
            { GMDeleteItemType.Soul_Equip, "equipinvenseq" },
            { GMDeleteItemType.ItemClass_Orb, "orb_inven_seq" },
        };

        public enum eSystemType
        {
            EVENT = 1,
            PACKAGE = 2,
            SEVEN_EVENT = 3,
        }

        public enum eRewardTye : long
        {
            GOLD = 303000001,
            CASH = 303000005,
            KEY = 303000002,
            Ticket = 303000003,
            Battlepoint = 303000018,
            Partypoint = 303000020,
            Honorpoint = 303000019,
            Donationpoint = 303000016,
            Expeditionpoint = 303000015,
            Item,
        };
        
        public enum eOpenTimeConstDef
        {
            //free
            BATTLE_FREEFORALL_START_TIME_1st,
            BATTLE_FREEFORALL_END_TIME_1st,
            BATTLE_FREEFORALL_START_TIME_2nd,
            BATTLE_FREEFORALL_END_TIME_2nd,
            //ruby
            PVP_GLADIATOR_START_TIME_1st,
            PVP_GLADIATOR_END_TIME_1st,
            PVP_GLADIATOR_START_TIME_2nd,
            PVP_GLADIATOR_END_TIME_2nd,
            //1vs1
            DEF_1VS1REAL_START_TIME,
            DEF_1VS1REAL_END_TIME,
            DEF_1VS1REAL_START_TIME_BONUS,
            DEF_1VS1REAL_END_TIME_BOUNS,
            //guild 3v3
            BATTLE_GUILD_G3VS3_START_TIME_1st,
            BATTLE_GUILD_G3VS3_END_TIME_1st,
            BATTLE_GUILD_G3VS3_START_TIME_2nd,
            BATTLE_GUILD_G3VS3_END_TIME_2nd,

            //bossraid rate
            BOSSRAID_APPEAR_PROBABILITY,

            //eventdaily
            DAILY_ADD_RUBY,
            DAILY_ADD_MAX,

            BLACKMARKET_OPEN_DAY,
            BLACKMARKET_OPEN_START_TIME,
            BLACKMARKET_OPEN_END_TIME,

            SHOP_NEW_ON_OFF,
        };

        public static readonly Dictionary<eOpenTimeConstDef, string> OpenTime_Const_Def_Key_List = new Dictionary<eOpenTimeConstDef, string>()
        {
            { eOpenTimeConstDef.BATTLE_FREEFORALL_START_TIME_1st, "DEF_BATTLE_FREEFORALL_START_TIME_1st" },
            { eOpenTimeConstDef.BATTLE_FREEFORALL_END_TIME_1st, "DEF_BATTLE_FREEFORALL_END_TIME_1st" },
            { eOpenTimeConstDef.BATTLE_FREEFORALL_START_TIME_2nd, "DEF_BATTLE_FREEFORALL_START_TIME_2nd" },
            { eOpenTimeConstDef.BATTLE_FREEFORALL_END_TIME_2nd, "DEF_BATTLE_FREEFORALL_END_TIME_2nd" },

            { eOpenTimeConstDef.PVP_GLADIATOR_START_TIME_1st, "DEF_PVP_GLADIATOR_START_TIME_1st" },
            { eOpenTimeConstDef.PVP_GLADIATOR_END_TIME_1st, "DEF_PVP_GLADIATOR_END_TIME_1st" },
            { eOpenTimeConstDef.PVP_GLADIATOR_START_TIME_2nd, "DEF_PVP_GLADIATOR_START_TIME_2nd" },
            { eOpenTimeConstDef.PVP_GLADIATOR_END_TIME_2nd, "DEF_PVP_GLADIATOR_END_TIME_2nd" },

            { eOpenTimeConstDef.DEF_1VS1REAL_START_TIME, "DEF_1VS1REAL_START_TIME" },
            { eOpenTimeConstDef.DEF_1VS1REAL_END_TIME, "DEF_1VS1REAL_END_TIME" },
            { eOpenTimeConstDef.DEF_1VS1REAL_START_TIME_BONUS, "DEF_1VS1REAL_START_TIME_BONUS" },
            { eOpenTimeConstDef.DEF_1VS1REAL_END_TIME_BOUNS, "DEF_1VS1REAL_END_TIME_BOUNS" },

            { eOpenTimeConstDef.BATTLE_GUILD_G3VS3_START_TIME_1st, "DEF_BATTLE_GUILD_G3VS3_START_TIME_1st" },
            { eOpenTimeConstDef.BATTLE_GUILD_G3VS3_END_TIME_1st, "DEF_BATTLE_GUILD_G3VS3_END_TIME_1st" },
            { eOpenTimeConstDef.BATTLE_GUILD_G3VS3_START_TIME_2nd, "DEF_BATTLE_GUILD_G3VS3_START_TIME_2nd" },
            { eOpenTimeConstDef.BATTLE_GUILD_G3VS3_END_TIME_2nd, "DEF_BATTLE_GUILD_G3VS3_END_TIME_2nd" },

            { eOpenTimeConstDef.BOSSRAID_APPEAR_PROBABILITY, "BOSSRAID_APPEAR_PROBABILITY" },

            { eOpenTimeConstDef.DAILY_ADD_RUBY, "DAILY_ADD_RUBY" },
            { eOpenTimeConstDef.DAILY_ADD_MAX, "DAILY_ADD_MAX" },
            { eOpenTimeConstDef.BLACKMARKET_OPEN_DAY, "DEF_BLACKMARKET_OPEN_DAY" },
            { eOpenTimeConstDef.BLACKMARKET_OPEN_START_TIME, "DEF_BLACKMARKET_OPEN_START_TIME" },
            { eOpenTimeConstDef.BLACKMARKET_OPEN_END_TIME, "DEF_BLACKMARKET_OPEN_END_TIME" },

            { eOpenTimeConstDef.SHOP_NEW_ON_OFF, "DEF_SHOP_NEW_ON_OFF" },
        };

        public static readonly Dictionary<TheSoul.DataManager.Shop_Define.eShopSaleType, string> ShopSaleType_List = new Dictionary<TheSoul.DataManager.Shop_Define.eShopSaleType, string>()
        {
            { TheSoul.DataManager.Shop_Define.eShopSaleType.RegularSale, Resources.languageResource.lang_shoptype1 },
            { TheSoul.DataManager.Shop_Define.eShopSaleType.BuyOnceSale, Resources.languageResource.lang_shoptype2 },
            { TheSoul.DataManager.Shop_Define.eShopSaleType.Subscription, Resources.languageResource.lang_shoptype3 },
            { TheSoul.DataManager.Shop_Define.eShopSaleType.DiscountSale, "" },
            { TheSoul.DataManager.Shop_Define.eShopSaleType.StackPrice, Resources.languageResource.lang_shoptype4 },
            { TheSoul.DataManager.Shop_Define.eShopSaleType.Subscription_Upgrade, Resources.languageResource.lang_shoptype5 },
            { TheSoul.DataManager.Shop_Define.eShopSaleType.Subscription_Week, Resources.languageResource.lang_shoptype6 },
            { TheSoul.DataManager.Shop_Define.eShopSaleType.Package, Resources.languageResource.lang_shoptype7 },
        };

        public static readonly Dictionary<TheSoul.DataManager.SystemData_Define.eContentsType, string> ContentsType_List = new Dictionary<TheSoul.DataManager.SystemData_Define.eContentsType, string>()
        {
            { TheSoul.DataManager.SystemData_Define.eContentsType.ALL, "All" },
            { TheSoul.DataManager.SystemData_Define.eContentsType.EVENT_ARCHIVE_REWARD, Resources.languageResource.lang_achieve },
            { TheSoul.DataManager.SystemData_Define.eContentsType.EVENT_EVENT_REWARD, Resources.languageResource.lang_event },
            { TheSoul.DataManager.SystemData_Define.eContentsType.EVENT_PVP_ARCHIVE_REWARD, Resources.languageResource.lang_pvpAchieve },
            { TheSoul.DataManager.SystemData_Define.eContentsType.MATCH_1VS1, Resources.languageResource.lang_1vs1PVP },
            { TheSoul.DataManager.SystemData_Define.eContentsType.MATCH_FREE, Resources.languageResource.lang_freePVP },
            { TheSoul.DataManager.SystemData_Define.eContentsType.MATCH_GOLDEXPEDITION, Resources.languageResource.lang_expedition },
            { TheSoul.DataManager.SystemData_Define.eContentsType.MATCH_GUILD_WAR, Resources.languageResource.lang_guildWar },
            { TheSoul.DataManager.SystemData_Define.eContentsType.MATCH_OVERLORD, Resources.languageResource.lang_overload },
            { TheSoul.DataManager.SystemData_Define.eContentsType.MATCH_PARTY, Resources.languageResource.lang_party },
            { TheSoul.DataManager.SystemData_Define.eContentsType.MATCH_RUBY_PVP, Resources.languageResource.lang_gladiator },
            { TheSoul.DataManager.SystemData_Define.eContentsType.PVE_BOSSRAID, Resources.languageResource.lang_bossraid },
            { TheSoul.DataManager.SystemData_Define.eContentsType.PVE_DARK, Resources.languageResource.lang_pve_dark},
            { TheSoul.DataManager.SystemData_Define.eContentsType.PVE_ELITE, Resources.languageResource.lang_pve_elite },
            { TheSoul.DataManager.SystemData_Define.eContentsType.PVE_SENARIO, Resources.languageResource.lang_mission },
            { TheSoul.DataManager.SystemData_Define.eContentsType.NONE, "None" },
        };
            
    }

    public class GMResult_Define
    {
        public enum eResult
        {
            SUCCESS = 0,
            NOT_USER_PERMISSION = 1,
            DB_ERROR = 99,

        };

        public enum ControlType
        {
            GM_LOGIN = 0,
            MISSION_PROGRESS = 1,
            GUILD_DISPERSE = 2,
            GUILD_BANISH = 3,
            GUILD_ENTRUST = 4,
            GUILD_DELETE = 5,
            RANKING_DELETE = 6,
            USER_INFO_EDIT = 7,
            NOTICE_ADD = 8,
            NITICE_DELETE = 9,
            NOTICE_EDIT = 10,
            LINE_NOITCE_ADD = 11,
            LINE_NOTICE_EDIT = 12,
            LINE_NOTICE_DELETE = 13,
            OPNE_TIME_EDIT = 15,
            PACKAGE_ADD = 17,
            PACKAGE_EDIT = 18,
            GMUSER_EDIT = 19,
            EVENT_ADD = 20,
            EVENT_EDIT = 21,
            EVENT_DELETE = 22,
            GAME_ONOFF_EDIT = 23, 
            HALFPACKAGE_EDIT = 24,
            MENU_EDIT = 25,
            SEVEN_EVENT_EDIT = 26,
            EVENT_GROUP_EDIT = 27,
            DAILY_EVENT_EDIT = 28,
            FIRTS_EVENT_EDIT = 29,
            CHEAP_PACKAGE_ADD = 30,
            CHEAP_PACKAGE_EDIT = 31,
            BLACKMARKET_ADD = 32,
            BLACKMARKET_DELETE = 33,
            MAIL_NOTICE_ADD = 34,
            MAIL_NOTICE_DELETE = 35,
            SERVER_VERSION_ADD = 36,
            SERVER_VERSION_DELETE = 37,
            SERVER_STATE_EDIT = 38,
            BEST_GACHA_ADD = 39,
            BEST_GACHA_EDIT = 40,
            BEST_GACHA_DELETE = 41,
            USER_ITEM_ADD = 42,
            USER_IMAGINE_PAY = 43,
            PAY_RESULT_EDIT = 44,
            ACCOUNT_BREAK_OFF = 45,
            ETC = 100,

        };

        public static readonly Dictionary<ControlType, string> ControlType_List = new Dictionary<ControlType, string>()
        {
            { ControlType.GM_LOGIN, "Login" },
            { ControlType.MISSION_PROGRESS, Resources.languageResource.lang_missionProgress },
            { ControlType.GUILD_DISPERSE, Resources.languageResource.lang_guildDisperse },
            { ControlType.GUILD_BANISH, Resources.languageResource.lang_guildBanishment},
            { ControlType.GUILD_ENTRUST, Resources.languageResource.lang_guildEntrust},
            { ControlType.GUILD_DELETE, Resources.languageResource.lang_guilJoinerDelete },
            { ControlType.RANKING_DELETE, Resources.languageResource.lang_rankingDelete },
            { ControlType.USER_INFO_EDIT, Resources.languageResource.lang_userInfoEdit },
            { ControlType.NOTICE_ADD, Resources.languageResource.lang_noticeInsert},
            { ControlType.NITICE_DELETE, Resources.languageResource.lang_noticeDelete},
            { ControlType.NOTICE_EDIT, Resources.languageResource.lang_noticeUpdate },
            { ControlType.LINE_NOITCE_ADD, Resources.languageResource.lang_lineInsert},
            { ControlType.LINE_NOTICE_EDIT, Resources.languageResource.lang_lineUpdate},
            { ControlType.LINE_NOTICE_DELETE, Resources.languageResource.lang_lineDelete },
            { ControlType.OPNE_TIME_EDIT, Resources.languageResource.lang_openTimeEdit },
            { ControlType.PACKAGE_ADD, Resources.languageResource.lang_packageInsert},
            { ControlType.PACKAGE_EDIT, Resources.languageResource.lang_packageUpdate },
            { ControlType.GMUSER_EDIT, "GMUSER_EDIT"},
            { ControlType.EVENT_ADD, "EVENT_ADD"},
            { ControlType.EVENT_EDIT, "EVENT_EDIT"},
            { ControlType.EVENT_DELETE, "EVENT_DELETE"},
            { ControlType.GAME_ONOFF_EDIT, "GAME_ONOFF_EDIT"},
            { ControlType.HALFPACKAGE_EDIT, "HALF_PACKAGE_EDIT"},
            { ControlType.MENU_EDIT, "MENU_EDIT"},
            { ControlType.SEVEN_EVENT_EDIT, "SEVENDAY_EVENT_EDIT"},
            { ControlType.EVENT_GROUP_EDIT, "EVENT_GROUP_EDIT"},
            { ControlType.DAILY_EVENT_EDIT, "DAILY_EVENT_EDIT"},
            { ControlType.FIRTS_EVENT_EDIT, "FIRTS_PAYMENT_EVENT_EDIT"},
            { ControlType.CHEAP_PACKAGE_ADD, "1,3_PACKAGE_ADD"},
            { ControlType.CHEAP_PACKAGE_EDIT, "1,3_PACKAGE_EDIT"},
            { ControlType.BLACKMARKET_ADD, "BLACKMARKET_ADD"},
            { ControlType.BLACKMARKET_DELETE, "BLACKMARKET_DELETE"},
            { ControlType.MAIL_NOTICE_ADD, "MAIL_NOTICE_ADD"},
            { ControlType.MAIL_NOTICE_DELETE, "MAIL_NOTICE_DELETE"},
            { ControlType.SERVER_VERSION_ADD, "SERVER_VERSION_ADD"},
            { ControlType.SERVER_VERSION_DELETE, "SERVER_VERSION_DELETE"},
            { ControlType.SERVER_STATE_EDIT, "SERVER_STATE_EDIT"},
            { ControlType.BEST_GACHA_ADD, "BEST_GACHA_ADD"},
            { ControlType.BEST_GACHA_EDIT, "BEST_GACHA_EDIT"},
            { ControlType.BEST_GACHA_DELETE, "BEST_GACHA_DELETE"},
            { ControlType.USER_ITEM_ADD, "USER_ITEM_ADD"},
            { ControlType.USER_IMAGINE_PAY, "USER_IMAGINE_PAY"},
            { ControlType.PAY_RESULT_EDIT, "PAY_RESULT_EDIT"},
            { ControlType.ACCOUNT_BREAK_OFF, Resources.languageResource.lang_DelCharater},
            { ControlType.ETC, "Etc" },
        };

        public enum TargetType
        {
            GAME_USER = 1,
            GAME_GUILD = 2,
            GAME_SYSTEM = 3,
        };
    }
}