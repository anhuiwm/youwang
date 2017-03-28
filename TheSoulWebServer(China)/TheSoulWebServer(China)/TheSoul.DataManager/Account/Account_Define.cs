using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSoul.DataManager
{
    public static class Account_Define
    {
        public const string AccountShardingDB = "sharding";
        public const string AccountDBTableName = "Account";

        public const string AccountCommonDB = "common";
        //public const string AccountShardingDB = "sharding";
        public const string User_Account_CommonDB_TableName = "User_account";

        public const string Encrypt_TableName = "EncryptKey";
        public const string Account_Prefix = "User";
        public const string Account_SimpleInfo_Prefix = "User_Simple";
        public const string Account_SimpleInfo_Surfix = "Simple_Info";
        public const string Account_SimpleInfo_WithEquip_Surfix = "Simple_Equip_Info";

        public const string Account_TownInfo_Prefix = "User_Simple_Town";
        public const string Account_TownInfo_Surfix = "Simple_Info_Town";
        // Get System ShardingInfo
        public const string ShardingInfoPrefix = "ShardingInfo";

        public const string Account_WarpointInfo_Prefix = "User_Warpoint";

        public const string PvPInfo_Prefix = "UserPvPInfo";

        public const string User_Chat_Ignore_TableName = "User_Chat_Ignore_List";
        public const string User_Chat_Ignore_Surfix = "info_list";

        public const string System_Tutorial_Step_TableName = "System_Tutorial_Step";
        public const string System_Tutorial_Reward_TableName = "System_Tutorial_Reward";
        public const string User_Tutorial_Step_TableName = "User_Tutorial";

        public const string User_LoginCount_TableName = "User_Login_Count";
        public const string User_Coupon_Key_TableName = "User_Coupon_Key";

        public const int Max_UserName_Length = 8;
        public const int MaxLastConnDay = -5;

        public enum eAccountReturnKeys
        {
            AID,
            PlatformType,
            PlatformUserID,
            PlatformUID,
            UserID,
            CID,
            Account,
            Lobby,
            PvELastStage,

            Ticket,
            TicketRemain,
            RetRemain,

            UserRating,

            ContributionPoint,

            CharacterList,
            CharacterInfo,
            ItemInventory_Account,
            ItemInventory_Character,
            ItemInventory_Orb,
            SoulInventory_Active,
            SoulInventory_Passive,
            TownUserInfo,
            TutorialList,

            // for friend detail
            CharacterDetailInfo,
            CharacterSimpleInfoList,
            AccountSimpleInfo,

            // gold, cash, point
            RetValue,
            RetGold,
            RetRuby,
            RetExpeditionPoint,
            RetBlackMarketPoint,

            // for chat ignore
            ChatIgnoreList,

            // for character group
            CharacterGroup,
            // for new flag to Client
            NewFlags,
            AdminNotice,
            TutorialStepInfo,
            BestGachaInfo,

            Event_7Day_Flag,

            Account_Warpoint,
            User_Login_Day_Count,

            // for encrypt-decrypt
            EncryptKey,

            // for user restrict
            LoginRestrict,
            ChatRestrict,

            // for pvp count
            PvPCountInfo,

            // for server time
            ServerTimeString,
        };

        public static readonly Dictionary<eAccountReturnKeys, string> Account_Ret_KeyList = new Dictionary<eAccountReturnKeys, string>()
        {
            { eAccountReturnKeys.AID,           "aid"          },
            { eAccountReturnKeys.PlatformType,           "platform_type"          },
            { eAccountReturnKeys.PlatformUserID,           "platform_user_id"          },
            { eAccountReturnKeys.PlatformUID,           "platform_uid"          },
            { eAccountReturnKeys.UserID,           "userid"          },
            { eAccountReturnKeys.CID,           "cid"          },
            { eAccountReturnKeys.Account,       "account"          },
            { eAccountReturnKeys.PvELastStage,       "lastclearstage"          },
            { eAccountReturnKeys.Lobby,       "lobby"          },
            { eAccountReturnKeys.Ticket,       "ticket"          },
            { eAccountReturnKeys.TicketRemain,       "ticketremainsec"          },
            { eAccountReturnKeys.UserRating,       "user_rating"          },

            { eAccountReturnKeys.ContributionPoint,       "contributionpoint"          },
            { eAccountReturnKeys.CharacterList,       "characterlist"          },
            { eAccountReturnKeys.CharacterInfo,       "character"          },
            { eAccountReturnKeys.CharacterGroup,       "charactergroup"          },
            { eAccountReturnKeys.ItemInventory_Account,       "accountinven"          },
            { eAccountReturnKeys.ItemInventory_Character,       "characterinven"          },
            { eAccountReturnKeys.ItemInventory_Orb,       "orbinven"          },
            { eAccountReturnKeys.SoulInventory_Active,       "activesoulinven"          },
            { eAccountReturnKeys.SoulInventory_Passive,       "passivesoulinven"          },
            { eAccountReturnKeys.TownUserInfo,       "townuserlist"          },
            { eAccountReturnKeys.TutorialList,       "tutoriallist"          },
            { eAccountReturnKeys.RetRemain,           "retremain"          },
            { eAccountReturnKeys.Account_Warpoint,       "account_warpoint"          },

            // for friend detail
            { eAccountReturnKeys.CharacterDetailInfo,       "character_detail"          },
            { eAccountReturnKeys.CharacterSimpleInfoList,       "character_simple_list"          },
            { eAccountReturnKeys.AccountSimpleInfo,       "account_simple_info"          },
            
            // gold, cash, point
            { eAccountReturnKeys.RetValue,           "retvalue"          },
            { eAccountReturnKeys.RetGold,           "retgold"          },
            { eAccountReturnKeys.RetRuby,           "retruby"          },
            { eAccountReturnKeys.RetExpeditionPoint,           "retexpoint"          },
            { eAccountReturnKeys.RetBlackMarketPoint,           "retblackmarketpoint"          },

            // for chat ignore
            { eAccountReturnKeys.ChatIgnoreList,       "chat_ignore_list"          },

            // for new flag to Client
            { eAccountReturnKeys.NewFlags,       "newflags"          },
            { eAccountReturnKeys.AdminNotice,       "notice"          },
            { eAccountReturnKeys.BestGachaInfo,       "best_gacha_info"          },
            { eAccountReturnKeys.Event_7Day_Flag,       "event_7day_flag"          },
            { eAccountReturnKeys.User_Login_Day_Count,       "login_day"          },
            
            // for tutorial step
            { eAccountReturnKeys.TutorialStepInfo,       "tutorial_info"          },

            // for encrypt-decrypt
            { eAccountReturnKeys.EncryptKey,           "encryptkey"          },

            // for user restrict
            { eAccountReturnKeys.LoginRestrict,           "loginrestrict"          },
            { eAccountReturnKeys.ChatRestrict,           "chatrestrict"          },

            // for pvp count
            { eAccountReturnKeys.PvPCountInfo,           "pvp_count_info"          },

            // for server time
            { eAccountReturnKeys.ServerTimeString,           "time_string"          },
        };

        public const float PCEXPBuff_Rate_Type1 = 0.5f;
        public const float PCEXPBuff_Rate_Type2 = 1.0f;
        public const float MissionEXPBoostBuff_Rate_Type2 = 0.1f;

        public const int TownPlayerCount = 11;
        public const int Max_Characeter_Level_Limit = 90;

        public enum eAccountConstDef
        {
            DEF_2ndCLASS_RESTRICTION_LEVEL,
            DEF_3ndCLASS_RESTRICTION_LEVEL,
            DEF_2ndCLASS_RESTRICTION_SOUL_COUNT,
            DEF_3rdCLASS_RESTRICTION_SOUL_COUNT,
            DAILY_ADD_RUBY,
            DAILY_ADD_MAX,
            ADMIN_FIRST_PAYMENT_ON_OFF,
            ADMIN_DAILY_ON_OFF,
            ADMIN_7DAY_EVENT_ON_OFF,
            DEF_7DAY_EVENT_LIMIT_DAY,
        }

        public static readonly Dictionary<eAccountConstDef, string> Account_Const_Def_Key_List = new Dictionary<eAccountConstDef, string>()
        {
            { eAccountConstDef.DEF_2ndCLASS_RESTRICTION_LEVEL, "DEF_2ndCLASS_RESTRICTION_LEVEL"},
            { eAccountConstDef.DEF_3ndCLASS_RESTRICTION_LEVEL, "DEF_3ndCLASS_RESTRICTION_LEVEL"},
            { eAccountConstDef.DEF_2ndCLASS_RESTRICTION_SOUL_COUNT, "DEF_2ndCLASS_RESTRICTION_SOUL_COUNT"},
            { eAccountConstDef.DEF_3rdCLASS_RESTRICTION_SOUL_COUNT, "DEF_3rdCLASS_RESTRICTION_SOUL_COUNT"},
            { eAccountConstDef.DAILY_ADD_RUBY, "DAILY_ADD_RUBY"},
            { eAccountConstDef.DAILY_ADD_MAX, "DAILY_ADD_MAX"},
            { eAccountConstDef.ADMIN_FIRST_PAYMENT_ON_OFF, "FIRST_PAYMENT_ON_OFF"},
            { eAccountConstDef.ADMIN_DAILY_ON_OFF, "DAILY_ON_OFF"},
            { eAccountConstDef.ADMIN_7DAY_EVENT_ON_OFF, "SEVENDAYEVENT_ON_OFF"},
            { eAccountConstDef.DEF_7DAY_EVENT_LIMIT_DAY, "DEF_7DAYEVENT_DAY_VALUE_1"},
        };

        public enum eTutorialType
        {
            ForcedTutorial = 1,
            ConditionalTutorial = 2,
        }

        public struct FindTownUserRange
        {
            public short getCount;
            public short getVIPCount;
            public int myMinLevel;
            public int myMaxLevel;
            public int getMinLevel;
            public int getMaxLevel;
            public FindTownUserRange(short setCount, short setVipCount, int setMin, int setMax, int getMin, int getMax)
            {
                getCount = setCount;
                getVIPCount = setVipCount;
                myMinLevel = setMin;
                myMaxLevel = setMax;
                getMinLevel = getMin;
                getMaxLevel = getMax;
            }
        };

        public static readonly List<Account_Define.FindTownUserRange> GetTownUser_Count = new List<Account_Define.FindTownUserRange>()
        {
            // 1~10
            new Account_Define.FindTownUserRange(2, 1, 1, 10, 1, 10),
            new Account_Define.FindTownUserRange(3, 1, 1, 10, 11, 20),
            new Account_Define.FindTownUserRange(6, 2, 1, 10, 21, 30),
            // 11~20
            new Account_Define.FindTownUserRange(2, 1, 11, 20, 1, 10),
            new Account_Define.FindTownUserRange(3, 1, 11, 20, 11, 20),
            new Account_Define.FindTownUserRange(6, 2, 11, 20, 21, 30),
            // 21~30
            new Account_Define.FindTownUserRange(2, 1, 21, 30, 11, 20),
            new Account_Define.FindTownUserRange(3, 1, 21, 30, 21, 30),
            new Account_Define.FindTownUserRange(6, 3, 21, 30, 31, 40),
            // 31~40
            new Account_Define.FindTownUserRange(2, 1, 31, 40, 21, 30),
            new Account_Define.FindTownUserRange(3, 1, 31, 40, 31, 40),
            new Account_Define.FindTownUserRange(6, 3, 31, 40, 41, 50),
            // 41~50
            new Account_Define.FindTownUserRange(2, 1, 41, 50, 31, 40),
            new Account_Define.FindTownUserRange(3, 1, 41, 50, 41, 50),
            new Account_Define.FindTownUserRange(6, 3, 41, 50, 51, 60),
            // 51~60
            new Account_Define.FindTownUserRange(2, 1, 51, 60, 41, 50),
            new Account_Define.FindTownUserRange(3, 2, 51, 60, 51, 60),
            new Account_Define.FindTownUserRange(6, 3, 51, 60, 61, 70),
            // 61~70
            new Account_Define.FindTownUserRange(2, 1, 61, 70, 51, 60),
            new Account_Define.FindTownUserRange(3, 2, 61, 70, 61, 70),
            new Account_Define.FindTownUserRange(6, 3, 61, 70, 71, 80),
            // 71~80
            new Account_Define.FindTownUserRange(2, 1, 71, 80, 61, 70),
            new Account_Define.FindTownUserRange(3, 2, 71, 80, 71, 80),
            new Account_Define.FindTownUserRange(6, 3, 71, 80, 81, 90),
            // 81~90
            new Account_Define.FindTownUserRange(2, 1, 81, 90, 61, 70),
            new Account_Define.FindTownUserRange(3, 2, 81, 90, 71, 80),
            new Account_Define.FindTownUserRange(6, 3, 81, 90, 81, 90),
        };

        public const string CouponType_CDKey = "C";
        public const string CouponType_EAI = "E";

        public enum eCouponType
        {
            CDKey = 0,
            EAI = 1,
        }

        public static readonly Dictionary<eCouponType, string> Account_Coupon_Def_Type = new Dictionary<eCouponType, string>()
        {
            { eCouponType.CDKey, CouponType_CDKey },
            { eCouponType.EAI, CouponType_EAI },
        };

        public const string CouponState_Regist = "R";
        public const string CouponState_Use = "U";
        public const string EmptyMailSeq = "";
        public const int BaseChatChannel = 1;

        public enum eCouponState
        {
            Regist = 0,
            Use = 1,
        }

        public static readonly Dictionary<eCouponState, string> Account_Coupon_Def_State = new Dictionary<eCouponState, string>()
        {
            { eCouponState.Regist, CouponState_Regist },
            { eCouponState.Use, CouponState_Use },
        };
    }
}
