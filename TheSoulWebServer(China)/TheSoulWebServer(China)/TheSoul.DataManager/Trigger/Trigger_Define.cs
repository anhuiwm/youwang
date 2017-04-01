using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSoul.DataManager
{
    public class Trigger_Define
    {
        public const string Trigger_Info_DB = "sharding";

        public const string System_Event_TableName = "System_Event";
        public const string System_EventGroup_Admin_TableName = "System_EventGroup_Admin";
        public const string System_Event_Reward_Box_TableName = "System_Event_Reward_Box";
        public const string System_Event_Daily_TableName = "System_Event_Daily";
        public const string System_Event_First_Payment_TableName = "System_Event_First_Payment";

        public const string System_Achieve_TableName = "System_Achieve";
        public const string System_Achieve_Reward_Box_TableName = "System_Achieve_RewardBox";

        public const string System_Achieve_PvP_TableName = "System_Achieve_PvP";
        public const string System_Achieve_PvP_Reward_Box_TableName = "System_Achieve_PvP_RewardBox";

        public const string User_Event_Data_TableName = "User_Event_Data";
        public const string User_Event_Check_Data_TableName = "User_Event_Check_Data";
        public const string User_Achieve_Data_TableName = "User_Achieve_Data";
        public const string User_Achieve_Data_PvP_TableName = "User_Achieve_PvP_Data";

        public const string Trigger_Prefix = "Trigger";
        public const string User_Event_Prefix = "User_Event";
        public const string User_Event_Daily_Surfix = "Daily";

        public const string System_7Day_Event_TableName = "System_Event_7Day";
        public const string System_7Day_Event_Package_TableName = "System_Event_7Day_Package_List";
        public const string System_7Day_Event_Reward_TableName = "System_Event_7Day_Reward";
        public const string User_Event_7Day_Data_TableName = "User_Event_7Day_Data";

        public const string User_7DayEvent_Prefix = "User_7Day_Event";
        public const string User_7DayPackage_Prefix = "User_7Day_Package";

        public const long CheckEvent_IgnoreRewardID = 1010001;

        public static Dictionary<eEventDailyType, string> DailyType = new Dictionary<eEventDailyType, string>()
        {
            { eEventDailyType.None, "" },
            { eEventDailyType.Count, "Count" },
            { eEventDailyType.Everyday, "Everyday" },
            { eEventDailyType.Over, "Over" },
            { eEventDailyType.Monthly, "Monthly" },
        };

        public enum eEventDailyType
        {
            None = 0,
            Count = 1,
            Everyday = 2,
            Over = 3,
            Monthly = 4,
        }


        public enum eEventListType
        {
            None,
            Event,
            Achive,
            PvP_Achive,
        }


        [Flags]
        public enum ePvEType
        {
            Scenario = 1,
            Guerilla = 2,
            Elite = 4,
            Bossraid = 8,
            Party = 16,
        }

        [Flags]
        public enum ePvPType
        {
            NONE = -1,
            MATCH_OVERLORD = 1,         // 패왕의길
            MATCH_1VS1 = 2,             // 투신전
            MATCH_FREE = 4,             // 난전
            MATCH_GOLDEXPEDITION = 8,   // 황금원정단
            MATCH_PARTY = 16,           // 협력전
            MATCH_GUILD_WAR = 32,       // 길드전
            MATCH_RUBY_PVP = 64,        // 검투사의전장
        }

        public static readonly Dictionary<Trigger_Define.ePvPType, PvP_Define.ePvPType> PvPType_Define_List = new Dictionary<ePvPType, PvP_Define.ePvPType>()
        {
            { Trigger_Define.ePvPType.MATCH_1VS1, PvP_Define.ePvPType.MATCH_1VS1 },
            { Trigger_Define.ePvPType.MATCH_FREE, PvP_Define.ePvPType.MATCH_FREE },
            { Trigger_Define.ePvPType.MATCH_GUILD_WAR, PvP_Define.ePvPType.MATCH_GUILD_3VS3},
            { Trigger_Define.ePvPType.MATCH_RUBY_PVP, PvP_Define.ePvPType.MATCH_RUBY_PVP },
            { Trigger_Define.ePvPType.MATCH_OVERLORD, PvP_Define.ePvPType.MATCH_OVERLORD },
            { Trigger_Define.ePvPType.MATCH_PARTY, PvP_Define.ePvPType.MATCH_PARTY },
        };

        public enum eEventLoopType
        {
            None = 0,
            Repeat = 1,
            Day = 2,
            Week = 3,
            Month = 4,
            Continue = 5,
            Odd_Month = 41,
            Even_Month = 42,
        }

        public enum eClearType
        {
            Playing = 0,
            Clear,
            End,
        }

        public enum ePvPTableType
        {
            None = 0,
            Daily = 1,
            Weekly = 2,
            Monthly = 3,
            Season = 4,
            Guild_Daily = 5,
            Guild_Weekly = 6,
            Guild_Monthly = 7,
        }

        public static Dictionary<eClearType, string> ClearFlag = new Dictionary<eClearType, string>()
        {
            { eClearType.Playing, "P" },
            { eClearType.Clear, "C" },
            { eClearType.End, "E" },
        };

        public static Dictionary<string, eClearType> StringToClearFlag = new Dictionary<string, eClearType>()
        {
            { "P", eClearType.Playing },
            { "C", eClearType.Clear },
            { "E", eClearType.End },
        };

        public enum eTriggerType
        {
            None = 0,
            Class,
            Charge_First,
            Charge,
            Charge_Billing,
            Charge_Price,
            VIP_Point,
            Goods_Purchase,
            Level,
            Game_Access,
            VIP_Level,
            VIP_Level_First,
            Friend_register,
            Wechat_Share,
            Ruby_Use,
            Town_Enter,
            Play_Scenario,
            Play_Guerilla,
            Play_Elite,
            Play_Bossraid,
            Play_Party,
            Play_Scenario_First,
            Play_Guerilla_First,
            Play_Elite_First,
            Play_Party_First,
            Play_PVE,
            Clear_Scenario,
            Clear_Guerilla,
            Clear_Elite,
            Clear_Party,
            Clear_Scenario_First,
            Clear_Guerilla_First,
            Clear_Elite_First,
            Clear_Party_First,
            Clear_PVE,
            Clear_Fail_PVE,
            Clear_Elite_Perfect,
            Clear_Perfect_First,
            Clear_Scenario_Time,
            Clear_Scenario_Restrict,
            Clear_Scenario_HP,
            Reward_Acquire_Bossraid,
            Autoclear_Use,
            Energy_Use,
            Win_PVP,
            Play_PVP,
            Kill_Freeforall,
            Kill_NPC,
            Kill_NPC_Appoint,
            Kill_NPC_First,
            Kill_NPC_Appear,
            Clear_Mission,
            Combo,
            Rank_1vs1Real_First,
            Rank_Freeforall_First,
            Destroy_Object,
            Gacha,
            Equip_Acquire,
            Weapon_LvUp,
            Weapon_Lv,
            Armor_LvUp,
            Armor_GradeUp,
            Accerary_GradeUp,
            Soul_LvUp,
            Soul_Lv,
            Soul_Acquire,
            Soul_Piece_Acquire,
            Friend_Key,
            Guild_Donation,
            Guild_Attendance,
            Guild_Level,
            Gold,
            Achieve_Daily_All,
            Achieve_Daily,
            Guild_User_EXP,
            Time,
            CHARGE_FIXED,
            CHARGE_FIXED_WEEK,
            WEAPON_REFINING,
            ARMOR_METALWORK,
            ACCESSORY_REMODELING,
            PASSIVESOUL_ACTION,
            Event7Day,
            Straight_PVP,
            Kill_Count,
            Grade_PVP,
            PVP_Match_Rank,
            Clear_Event,
            GACHASHOP,
            GACHASHOP_SPECIAL,
            ACCOUNT_REGIST,
            AttackPower,        /// 用户总战力数据比较
            DivineWeapon,       /// 用户最高等级神兵比较
            FaceBook_Open,
            FaceBook_FriendCount,
        }

        public static Dictionary<string, eTriggerType> TriggerType = new Dictionary<string, eTriggerType>()
        {
            { "", eTriggerType.None },
            { "None", eTriggerType.None },
            { "Class", eTriggerType.Class },
            { "Charge_First", eTriggerType.Charge_First },
            { "Charge", eTriggerType.Charge },
            { "Charge_Billing", eTriggerType.Charge_Billing },
            { "Charge_Price", eTriggerType.Charge_Price },
            { "VIP_Point", eTriggerType.VIP_Point },
            { "Goods_Purchase", eTriggerType.Goods_Purchase },
            { "Level", eTriggerType.Level },
            { "Game_Access", eTriggerType.Game_Access },
            { "VIP_Level", eTriggerType.VIP_Level },
            { "VIP_Level_First", eTriggerType.VIP_Level_First },
            { "Friend_register", eTriggerType.Friend_register },
            { "Wechat_Share", eTriggerType.Wechat_Share },
            { "Ruby_Use", eTriggerType.Ruby_Use },
            { "Town_Enter", eTriggerType.Town_Enter },
            { "Play_Scenario", eTriggerType.Play_Scenario },
            { "Play_Guerilla", eTriggerType.Play_Guerilla },
            { "Play_Elite", eTriggerType.Play_Elite },
            { "Play_Bossraid", eTriggerType.Play_Bossraid },
            { "Play_Party", eTriggerType.Play_Party },
            { "Play_Scenario_First", eTriggerType.Play_Scenario_First },
            { "Play_Guerilla_First", eTriggerType.Play_Guerilla_First },
            { "Play_Elite_First", eTriggerType.Play_Elite_First },
            { "Play_Party_First", eTriggerType.Play_Party_First },
            { "Play_PVE", eTriggerType.Play_PVE },
            { "Clear_Scenario", eTriggerType.Clear_Scenario },
            { "Clear_Guerilla", eTriggerType.Clear_Guerilla },
            { "Clear_Elite", eTriggerType.Clear_Elite },
            { "Clear_Party", eTriggerType.Clear_Party },
            { "Clear_Scenario_First", eTriggerType.Clear_Scenario_First },
            { "Clear_Guerilla_First", eTriggerType.Clear_Guerilla_First },
            { "Clear_Elite_First", eTriggerType.Clear_Elite_First },
            { "Clear_Party_First", eTriggerType.Clear_Party_First },
            { "Clear_PVE", eTriggerType.Clear_PVE },
            { "Clear_Fail_PVE", eTriggerType.Clear_Fail_PVE },
            { "Clear_Elite_Perfect", eTriggerType.Clear_Elite_Perfect },
            { "Clear_Perfect_First", eTriggerType.Clear_Perfect_First },
            { "Clear_Scenario_Time", eTriggerType.Clear_Scenario_Time },
            { "Clear_Scenario_Restrict", eTriggerType.Clear_Scenario_Restrict },
            { "Clear_Scenario_HP", eTriggerType.Clear_Scenario_HP },
            { "Reward_Acquire_Bossraid", eTriggerType.Reward_Acquire_Bossraid },
            { "Autoclear_Use", eTriggerType.Autoclear_Use },
            { "Energy_Use", eTriggerType.Energy_Use },
            { "Win_PVP", eTriggerType.Win_PVP },
            { "Play_PVP", eTriggerType.Play_PVP },
            { "Kill_Freeforall", eTriggerType.Kill_Freeforall },
            { "Kill_NPC", eTriggerType.Kill_NPC },
            { "Kill_NPC_Appoint", eTriggerType.Kill_NPC_Appoint },
            { "Kill_NPC_First", eTriggerType.Kill_NPC_First },
            { "Kill_NPC_Appear", eTriggerType.Kill_NPC_Appear },
            { "Clear_Mission", eTriggerType.Clear_Mission },
            { "Combo", eTriggerType.Combo },
            { "Rank_1vs1Real_First", eTriggerType.Rank_1vs1Real_First },
            { "Rank_Freeforall_First", eTriggerType.Rank_Freeforall_First },
            { "Destroy_Object", eTriggerType.Destroy_Object },
            { "Gacha", eTriggerType.Gacha },
            { "Equip_Acquire", eTriggerType.Equip_Acquire },
            { "Weapon_LvUp", eTriggerType.Weapon_LvUp },
            { "Weapon_Lv", eTriggerType.Weapon_Lv },
            { "Armor_LvUp", eTriggerType.Armor_LvUp },
            { "Armor_GradeUp", eTriggerType.Armor_GradeUp },
            { "Accerary_GradeUp", eTriggerType.Accerary_GradeUp },
            { "Soul_LvUp", eTriggerType.Soul_LvUp },
            { "Soul_Lv", eTriggerType.Soul_Lv },
            { "Soul_Acquire", eTriggerType.Soul_Acquire },
            { "Soul_Piece_Acquire", eTriggerType.Soul_Piece_Acquire },
            { "Friend_Key", eTriggerType.Friend_Key },
            { "Guild_Donation", eTriggerType.Guild_Donation },
            { "Guild_Attendance", eTriggerType.Guild_Attendance },
            { "Guild_Level", eTriggerType.Guild_Level },
            { "Gold", eTriggerType.Gold },
            { "Achieve_Daily_All", eTriggerType.Achieve_Daily_All },
            { "Achieve_Daily", eTriggerType.Achieve_Daily },
            { "Guild_User_EXP", eTriggerType.Guild_User_EXP },
            { "Time", eTriggerType.Time },
            { "CHARGE_FIXED", eTriggerType.CHARGE_FIXED },
            { "CHARGE_FIXED_WEEK", eTriggerType.CHARGE_FIXED_WEEK },
            { "WEAPON_REFINING", eTriggerType.WEAPON_REFINING },
            { "ARMOR_METALWORK", eTriggerType.ARMOR_METALWORK },
            { "ACCESSORY_REMODELING", eTriggerType.ACCESSORY_REMODELING },
            { "PASSIVESOUL_ACTION", eTriggerType.PASSIVESOUL_ACTION },
            { "7Day", eTriggerType.Event7Day},
            { "Straight_PVP", eTriggerType.Straight_PVP},
            { "Kill_Count", eTriggerType.Kill_Count},
            { "Grade_PVP", eTriggerType.Grade_PVP},
            { "PVP_Match_Rank", eTriggerType.PVP_Match_Rank},
            { "Clear_Event", eTriggerType.Clear_Event},
            { "GACHASHOP", eTriggerType.GACHASHOP},
            { "GACHASHOP_SPECIAL", eTriggerType.GACHASHOP_SPECIAL},
            { "ACCOUNT_REGIST", eTriggerType.ACCOUNT_REGIST},
            { "AttackPower", eTriggerType.AttackPower},
            { "DivineWeapon", eTriggerType.DivineWeapon},
            { "FaceBook_Open", eTriggerType.FaceBook_Open},
            { "FaceBook_FriendCount", eTriggerType.FaceBook_FriendCount},            
        };

        public static Dictionary<eTriggerType, string> TriggerString = new Dictionary<eTriggerType, string>()
        {
            { eTriggerType.None, "None" },
            { eTriggerType.Class, "Class" },
            { eTriggerType.Charge_First, "Charge_First" },
            { eTriggerType.Charge, "Charge" },
            { eTriggerType.Charge_Billing, "Charge_Billing" },            
            { eTriggerType.Charge_Price, "Charge_Price" },
            { eTriggerType.VIP_Point, "VIP_Point" },
            { eTriggerType.Goods_Purchase, "Goods_Purchase" },
            { eTriggerType.Level, "Level" },
            { eTriggerType.Game_Access, "Game_Access" },
            { eTriggerType.VIP_Level, "VIP_Level" },
            { eTriggerType.VIP_Level_First, "VIP_Level_First" },
            { eTriggerType.Friend_register, "Friend_register" },
            { eTriggerType.Wechat_Share, "Wechat_Share" },
            { eTriggerType.Ruby_Use, "Ruby_Use" },
            { eTriggerType.Town_Enter, "Town_Enter" },
            { eTriggerType.Play_Scenario, "Play_Scenario" },
            { eTriggerType.Play_Guerilla, "Play_Guerilla" },
            { eTriggerType.Play_Elite, "Play_Elite" },
            { eTriggerType.Play_Bossraid, "Play_Bossraid" },
            { eTriggerType.Play_Party, "Play_Party" },
            { eTriggerType.Play_Scenario_First, "Play_Scenario_First" },
            { eTriggerType.Play_Guerilla_First, "Play_Guerilla_First" },
            { eTriggerType.Play_Elite_First, "Play_Elite_First" },
            { eTriggerType.Play_Party_First, "Play_Party_First" },
            { eTriggerType.Play_PVE, "Play_PVE" },
            { eTriggerType.Clear_Scenario, "Clear_Scenario" },
            { eTriggerType.Clear_Guerilla, "Clear_Guerilla" },
            { eTriggerType.Clear_Elite, "Clear_Elite" },
            { eTriggerType.Clear_Party, "Clear_Party" },
            { eTriggerType.Clear_Scenario_First, "Clear_Scenario_First" },
            { eTriggerType.Clear_Guerilla_First, "Clear_Guerilla_First" },
            { eTriggerType.Clear_Elite_First, "Clear_Elite_First" },
            { eTriggerType.Clear_Party_First, "Clear_Party_First" },
            { eTriggerType.Clear_PVE, "Clear_PVE" },
            { eTriggerType.Clear_Fail_PVE, "Clear_Fail_PVE" },
            { eTriggerType.Clear_Elite_Perfect, "Clear_Elite_Perfect" },
            { eTriggerType.Clear_Perfect_First, "Clear_Perfect_First" },
            { eTriggerType.Clear_Scenario_Time, "Clear_Scenario_Time" },
            { eTriggerType.Clear_Scenario_Restrict, "Clear_Scenario_Restrict" },
            { eTriggerType.Clear_Scenario_HP, "Clear_Scenario_HP" },
            { eTriggerType.Reward_Acquire_Bossraid, "Reward_Acquire_Bossraid" },
            { eTriggerType.Autoclear_Use, "Autoclear_Use" },
            { eTriggerType.Energy_Use, "Energy_Use" },
            { eTriggerType.Win_PVP, "Win_PVP" },
            { eTriggerType.Play_PVP, "Play_PVP" },
            { eTriggerType.Kill_Freeforall, "Kill_Freeforall" },
            { eTriggerType.Kill_NPC, "Kill_NPC" },
            { eTriggerType.Kill_NPC_Appoint, "Kill_NPC_Appoint" },
            { eTriggerType.Kill_NPC_First, "Kill_NPC_First" },
            { eTriggerType.Kill_NPC_Appear, "Kill_NPC_Appear" },
            { eTriggerType.Clear_Mission, "Clear_Mission" },
            { eTriggerType.Combo, "Combo" },
            { eTriggerType.Rank_1vs1Real_First, "Rank_1vs1Real_First" },
            { eTriggerType.Rank_Freeforall_First, "Rank_Freeforall_First" },
            { eTriggerType.Destroy_Object, "Destroy_Object" },
            { eTriggerType.Gacha, "Gacha" },
            { eTriggerType.Equip_Acquire, "Equip_Acquire" },
            { eTriggerType.Weapon_LvUp, "Weapon_LvUp" },
            { eTriggerType.Weapon_Lv, "Weapon_Lv" },
            { eTriggerType.Armor_LvUp, "Armor_LvUp" },
            { eTriggerType.Armor_GradeUp, "Armor_GradeUp" },
            { eTriggerType.Accerary_GradeUp, "Accerary_GradeUp" },
            { eTriggerType.Soul_LvUp, "Soul_LvUp" },
            { eTriggerType.Soul_Lv, "Soul_Lv" },
            { eTriggerType.Soul_Acquire, "Soul_Acquire" },
            { eTriggerType.Soul_Piece_Acquire, "Soul_Piece_Acquire" },
            { eTriggerType.Friend_Key, "Friend_Key" },
            { eTriggerType.Guild_Donation, "Guild_Donation" },
            { eTriggerType.Guild_Attendance, "Guild_Attendance" },
            { eTriggerType.Guild_Level, "Guild_Level" },
            { eTriggerType.Guild_User_EXP, "Guild_User_EXP" },
            { eTriggerType.Gold, "Gold" },
            { eTriggerType.Achieve_Daily_All, "Achieve_Daily_All" },
            { eTriggerType.Achieve_Daily, "Achieve_Daily" },
            { eTriggerType.CHARGE_FIXED, "CHARGE_FIXED" },
            { eTriggerType.CHARGE_FIXED_WEEK, "CHARGE_FIXED_WEEK" },
            { eTriggerType.Time, "Time" },
            { eTriggerType.WEAPON_REFINING , "WEAPON_REFINING" },
            { eTriggerType.ARMOR_METALWORK , "ARMOR_METALWORK" },
            { eTriggerType.ACCESSORY_REMODELING , "ACCESSORY_REMODELING" },
            { eTriggerType.PASSIVESOUL_ACTION , "PASSIVESOUL_ACTION" },
            { eTriggerType.Event7Day, "7Day" },
            { eTriggerType.Straight_PVP, "Straight_PVP" },
            { eTriggerType.Kill_Count, "Kill_Count" },
            { eTriggerType.Grade_PVP, "Grade_PVP" },
            { eTriggerType.PVP_Match_Rank, "PVP_Match_Rank" },
            { eTriggerType.Clear_Event, "Clear_Event" },
            { eTriggerType.GACHASHOP, "GACHASHOP"},
            { eTriggerType.GACHASHOP_SPECIAL, "GACHASHOP_SPECIAL"},
            { eTriggerType.ACCOUNT_REGIST, "ACCOUNT_REGIST"},
            { eTriggerType.AttackPower, "AttackPower"},
            { eTriggerType.DivineWeapon, "DivineWeapon"},
            { eTriggerType.FaceBook_Open, "FaceBook_Open"},        
            { eTriggerType.FaceBook_FriendCount, "FaceBook_FriendCount"},            
        };

        public enum eTriggerReturnKeys
        {
            EventTypeList,
            EventTypeInfoList,
            EventList,
            DailyEventList,
            DailyEventInfo,
            AchieveList,
            RewardInfo,
            RewardInfo_List,
            NextAchieveInfo,
            DailyCheckBuyRubyPrice,
            DailyCheckBuyMax,
            FirstPaymentRewardItemList,
            FreeGachaInfo,
            Event7Day_Count,
            Event7Day_List,
            Event7Day_Package_List,
            Event7Day_LeftTime,
            CurrentPvPGrade,
            HighestPvPGrade,
            RankingRewardList,
        };

        public static readonly Dictionary<eTriggerReturnKeys, string> Trigger_Ret_KeyList = new Dictionary<eTriggerReturnKeys, string>()
        {
            { eTriggerReturnKeys.EventTypeList,           "event_type_list"          },
            { eTriggerReturnKeys.EventTypeInfoList,           "event_type_info_list"          },
            { eTriggerReturnKeys.EventList,           "event_list"          },
            { eTriggerReturnKeys.DailyEventList,           "event_daily_list"          },
            { eTriggerReturnKeys.DailyEventInfo,           "my_daily_event"          },
            { eTriggerReturnKeys.AchieveList,           "achieve_list"          },
            { eTriggerReturnKeys.RewardInfo,           "reward_event_info"          },
            { eTriggerReturnKeys.RewardInfo_List,           "reward_event_list"          },
            { eTriggerReturnKeys.NextAchieveInfo,           "next_event_info"          },
            { eTriggerReturnKeys.DailyCheckBuyRubyPrice,           "daily_check_ruby_price"          },
            { eTriggerReturnKeys.DailyCheckBuyMax,           "daily_check_buy_max"          },
            { eTriggerReturnKeys.FirstPaymentRewardItemList,           "firstpay_reward_list"          },
            { eTriggerReturnKeys.FreeGachaInfo,           "gacha_info"          },
            { eTriggerReturnKeys.Event7Day_Count,           "user_7day_count"          },
            { eTriggerReturnKeys.Event7Day_List,           "7day_event_list"          },
            { eTriggerReturnKeys.Event7Day_Package_List,           "7day_shop_list"          },
            { eTriggerReturnKeys.Event7Day_LeftTime,           "event_left_time"          },
            { eTriggerReturnKeys.CurrentPvPGrade,           "set_grade"          },
            { eTriggerReturnKeys.HighestPvPGrade,           "high_grade"          },
            { eTriggerReturnKeys.RankingRewardList,           "rank_reward_list"          },

        };

        public enum eEventGroupType
        {
            GiftEvent = 1,
            UserEvent = 2,
        }

        public enum eGameAccessType
        {
            TotalLoginCount = 1,
            AccumulateLoginDay = 2,
            AccumulateLoginTime = 3,        // check minute
            CountinueLoginDay = 4,
            CountLoginDay = 5,  // event time only
        }

        public enum eEquipAccquireType
        {
            Weapon = 1,
            Accessary = 2,
        }

        public enum eCheckSuccess
        {
            Success = 1,
            Fail = 2,
            Try = 3,
        }

        public enum eSoul_LvUpType
        {
            All = 0,
            Level = 1,
            Grade = 2,
            StarLevel = 3,
        }

        public enum eEnergyUseType
        {
            Key = 1,
            Ticket = 2,
        }

        public enum eRubyUseType
        {
            Single = 1,
            Accumulate = 2,
        }

        public enum eComboType
        {
            Max = 1,
            Accumulate = 2,
        }

        public enum eKillCountType
        {
            KillStreak = 1,
            Accumulate = 2,
        }

        public enum eGradePvPType
        {
            Point = 1,
            Grade = 2,
        }

        [Flags]
        public enum eGachaType
        {
            NONE = 0,
            NORMAL_TRY_ONE = 1,
            NORMAL_TRY_TEN = 2,
            PREMIUM_TRY_ONE = 4,
            PREMIUM_TRY_TEN = 8,
            BEST_TRY_ONE = 16,
            BEST_TRY_TEN = 32,
        }

        public enum ePassiveSoulUseType
        {
            Create = 1,
            Extract,
            LevelUp,
        }

        public enum eGachaCheckType
        {
            TryCount = 0,
            GachaGetCount = 1,
        }


        public enum eClassCheckType
        {
            MaxLevel = 1,
            CharacterLevel = 2,
        }

        public const int DrawOneGachaCount = 1;
        public const int DrawTenGachaCount = 10;

    }
}
