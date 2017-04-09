using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSoul.DataManager
{
    public static partial class PvP_Define
    {
        public const string PvP_Info_DB = "sharding";
        public const string PvP_Guild_Info_DB = "common";

        public const string PvP_Info_Surfix = "Info";

        public const string PvP_FreeForAll_Prefix = "PVP_Free";
        public const string PvP_1vs1_Prefix = "PVP_1vs1";
        public const string PvP_Ruby_Prefix = "PVP_Ruby";
        public const string PvP_PvP_Daily_TableName = "User_PVP_Record_Daily";
        public const string PvP_PvP_Weekly_TableName = "User_PVP_Record_Weekly";
        public const string PvP_PvP_Monthly_TableName = "User_PVP_Record_Monthly";
        public const string PvP_PvP_Season_TableName = "User_PVP_Record_Season";
        public const string PvP_Friend1vs1_Prefix = "PVP_Friend_1VS1";
        public const string PvP_Party_Prefix = "PVP_Pary";

        public const string PvP_Guild_Prefix = "PVP_Guild";
        public const string PvP_Guild_User_Prefix = "PVP_Guild_User";
        //public const string PvP_GuildPvP_TableName = "PVP_Record";
        public const string PvP_GuildPvP_Monthly_TableName = "Guild_PVP_Record_Monthly";
        public const string PvP_GuildUser_Daily_TableName = "Guild_User_PVP_Record_Daily";
        public const string PvP_GuildUser_Weekly_TableName = "Guild_User_PVP_Record_Weekly";
        public const string PvP_GuildUser_Monthly_TableName = "Guild_User_PVP_Record_Monthly";
        public const string PvP_PlayInfo_TableName = "User_PvP_Play";
        public const string PvP_System_Matching_Level_TableName = "System_Matching_Level";

        public const string PvP_Overlord_Prefix = "PVP_Overlord";
        public const string PvP_System_PvP_Overlord_Ranking_Dummy_TableName = "System_PvP_Overlord_Ranking_Dummy";
        public const string PvP_User_PVP_Overlord_Ranking_TableName = "User_PVP_Overlord_Ranking";
        public const string PvP_User_PVP_Overlord_Record_TableName = "User_PVP_Overlord_Record";
        public const string PvP_User_PVP_Overlord_ReadRecord_TableName = "User_PVP_Overlord_ReadRecord";
        public const string PvP_System_Rankup_Reward_TableName = "System_Rankup_Reward";    // for overlord pvp
        
        public const string PvP_Warpoint_Prefix = "PVP_Warpoint";
        public const string PvP_GuildRank_Prefix = "PVP_Guild";

        public const string PvP_GuildWarRank_Prefix = "PVP_GuildWar";//acva
        public const string PvP_GuildWarJoinerRank_Prefix = "PVP_GuildWarJoiner";//acva

        public const int PvP_RankRange = 9;
        public const int PvP_GuilRankRange = 50;
        public const int PvP_GuilRankSearchRange = 4;

        public const int PvP_GuildUserRating = 3;
        public const int PvP_GuildPVPUserCount = 3;
        public const int PvP_RANKING_SORT_SEP = 9999;

        public const int PvP_DefaultTopCount= 99; // if you want get count 100. set to 99. because rank count start by zero

        public const int PvP_1vs1_WinPoint = 11;
        public const int PvP_1vs1_LosePoint = -10;

        public enum ePvPRuby_Grade
        {
            MATCH_RUBY_PVP_GRADE_NONE = 0,
            MATCH_RUBY_PVP_GRADE_1 = 1,
            MATCH_RUBY_PVP_GRADE_2,
            MATCH_RUBY_PVP_GRADE_3,
            MATCH_RUBY_PVP_GRADE_COUNT = MATCH_RUBY_PVP_GRADE_3,
        }

        public static readonly Dictionary<ePvPRuby_Grade, ePvPConstDef> PvP_Ruby_Grade_Win_ConstKey = new Dictionary<ePvPRuby_Grade, ePvPConstDef>()
        {
            { ePvPRuby_Grade.MATCH_RUBY_PVP_GRADE_1, ePvPConstDef.DEF_PVP_GLADIATOR_WINNERGET_GRADE1 },
            { ePvPRuby_Grade.MATCH_RUBY_PVP_GRADE_2, ePvPConstDef.DEF_PVP_GLADIATOR_WINNERGET_GRADE2 },
            { ePvPRuby_Grade.MATCH_RUBY_PVP_GRADE_3, ePvPConstDef.DEF_PVP_GLADIATOR_WINNERGET_GRADE3 },
        };

        public static readonly Dictionary<ePvPRuby_Grade, ePvPConstDef> PvP_Ruby_Grade_Join_ConstKey = new Dictionary<ePvPRuby_Grade, ePvPConstDef>()
        {
            { ePvPRuby_Grade.MATCH_RUBY_PVP_GRADE_1, ePvPConstDef.DEF_PVP_GLADIATOR_JOINCOST_GRADE1 },
            { ePvPRuby_Grade.MATCH_RUBY_PVP_GRADE_2, ePvPConstDef.DEF_PVP_GLADIATOR_JOINCOST_GRADE2 },
            { ePvPRuby_Grade.MATCH_RUBY_PVP_GRADE_3, ePvPConstDef.DEF_PVP_GLADIATOR_JOINCOST_GRADE3 },
        };


        public static readonly Dictionary<ePvPRuby_Grade, ePvPConstDef> PvP_Gold_Grade_Win_ConstKey = new Dictionary<ePvPRuby_Grade, ePvPConstDef>()
        {
            { ePvPRuby_Grade.MATCH_RUBY_PVP_GRADE_1, ePvPConstDef.DEF_GLADIATOR_GOLD_WINNER_1 },
            { ePvPRuby_Grade.MATCH_RUBY_PVP_GRADE_2, ePvPConstDef.DEF_GLADIATOR_GOLD_WINNER_2 },
            { ePvPRuby_Grade.MATCH_RUBY_PVP_GRADE_3, ePvPConstDef.DEF_GLADIATOR_GOLD_WINNER_3 },
        };

        public static readonly Dictionary<ePvPRuby_Grade, ePvPConstDef> PvP_Gold_Grade_Join_ConstKey = new Dictionary<ePvPRuby_Grade, ePvPConstDef>()
        {
            { ePvPRuby_Grade.MATCH_RUBY_PVP_GRADE_1, ePvPConstDef.DEF_GLADIATOR_GOLD_1},
            { ePvPRuby_Grade.MATCH_RUBY_PVP_GRADE_2, ePvPConstDef.DEF_GLADIATOR_GOLD_2 },
            { ePvPRuby_Grade.MATCH_RUBY_PVP_GRADE_3, ePvPConstDef.DEF_GLADIATOR_GOLD_3 },
        };

        public enum ePvPGrade_Free
        {
            Bronze = 1,
            Silver,
            Gold,
            Jade,
            Sapphire,       // 5
            Garnet,
            Crystal,
            Platinum,
        }

        public const byte PvPFree_GradeCheckType_Rank_Position = 1;
        public const byte PvPFree_GradeCheckType_Rank_Rate = 2;

        public static readonly Dictionary<ePvPGrade_Free, KeyValuePair<byte, int>> PvP_Free_Grade_Check_PointList = new Dictionary<ePvPGrade_Free, KeyValuePair<byte, int>>()
        {
            { ePvPGrade_Free.Platinum, new KeyValuePair<byte, int>(PvPFree_GradeCheckType_Rank_Position, 1)  },
            { ePvPGrade_Free.Crystal, new KeyValuePair<byte, int>(PvPFree_GradeCheckType_Rank_Position, 10)  },
            { ePvPGrade_Free.Garnet, new KeyValuePair<byte, int>(PvPFree_GradeCheckType_Rank_Position, 50)  },
            { ePvPGrade_Free.Sapphire, new KeyValuePair<byte, int>(PvPFree_GradeCheckType_Rank_Rate, 1)  },
            { ePvPGrade_Free.Jade, new KeyValuePair<byte, int>(PvPFree_GradeCheckType_Rank_Rate, 10)  },
            { ePvPGrade_Free.Gold, new KeyValuePair<byte, int>(PvPFree_GradeCheckType_Rank_Rate, 30)  },
            { ePvPGrade_Free.Silver, new KeyValuePair<byte, int>(PvPFree_GradeCheckType_Rank_Rate, 60)  },
            { ePvPGrade_Free.Bronze, new KeyValuePair<byte, int>(PvPFree_GradeCheckType_Rank_Rate, 101)  },

        };

        public enum ePvPGrade_1vs1
        {
            Bronze_1 = 1,
            Bronze_2,
            Bronze_3,
            Bronze_4,
            Bronze_5,
            Silver_1,       // 6
            Silver_2,
            Silver_3,
            Silver_4,
            Silver_5,
            Gold_1,         // 11
            Gold_2,
            Gold_3,
            Gold_4,
            Gold_5,
            Platinum_1,     // 16
            Platinum_2,
            Platinum_3,
            Platinum_4,
            Platinum_5,
        }

        public static readonly Dictionary<ePvPGrade_1vs1, int> PvP_1vs1_Grade_Check_PointList = new Dictionary<ePvPGrade_1vs1, int>()
        {
            { ePvPGrade_1vs1.Bronze_1, 1050 },
            { ePvPGrade_1vs1.Bronze_2, 1150 },
            { ePvPGrade_1vs1.Bronze_3, 1250 },
            { ePvPGrade_1vs1.Bronze_4, 1350 },
            { ePvPGrade_1vs1.Bronze_5, 1450 },
            { ePvPGrade_1vs1.Silver_1, 1550 },
            { ePvPGrade_1vs1.Silver_2, 1650 },
            { ePvPGrade_1vs1.Silver_3, 1750 },
            { ePvPGrade_1vs1.Silver_4, 1850 },
            { ePvPGrade_1vs1.Silver_5, 2000 },
            { ePvPGrade_1vs1.Gold_1, 2200 },
            { ePvPGrade_1vs1.Gold_2, 2400 },
            { ePvPGrade_1vs1.Gold_3, 2600 },
            { ePvPGrade_1vs1.Gold_4, 2800 },
            { ePvPGrade_1vs1.Gold_5, 3000 },
            { ePvPGrade_1vs1.Platinum_1, 3500 },
            { ePvPGrade_1vs1.Platinum_2, 4000 },
            { ePvPGrade_1vs1.Platinum_3, 4500 },
            { ePvPGrade_1vs1.Platinum_4, 5000 },
            { ePvPGrade_1vs1.Platinum_5, 999999 },
        };

        public enum ePvPType
        {
            MATCH_NONE = -1,
            MATCH_FREE = 1,
            MATCH_1VS1 = 2,
            MATCH_GUILD_3VS3 = 3,
            // Ruby PvP match - add by manstar 02/12/2015
            MATCH_RUBY_PVP = 4,
            MATCH_FRIEND_1VS1 = 5,	// add by manstar 02/09/2015
            MATCH_PARTY = 6,		// 파티 던젼 
            MATCH_OVERLORD = 7,      //패왕의 길. 랭킹 뺏기.overlord
            MATCH_TEAM,    //삭제예정
            MATCH_COUNT = MATCH_TEAM,
        };

        public const ePvPType LastPvP = ePvPType.MATCH_OVERLORD;


        public enum eCharacterRankType
        {
            WARPOINT = 100,
        };

        public enum eGuildRankType
        {
            GUILDPOINT = 200,
            GUILDWARRANK = 201,
            GUILDWARRANK_JOINER = 202,
        };


        public static readonly Dictionary<ePvPType, string> PvP_RedisKey_List = new Dictionary<ePvPType, string>()
        {
            { ePvPType.MATCH_FREE,           PvP_FreeForAll_Prefix         },
            { ePvPType.MATCH_1VS1,           PvP_1vs1_Prefix          },
            { ePvPType.MATCH_GUILD_3VS3,           PvP_Guild_Prefix          },
            { ePvPType.MATCH_RUBY_PVP,           PvP_Ruby_Prefix          },
            { ePvPType.MATCH_FRIEND_1VS1,           PvP_Friend1vs1_Prefix          },
            { ePvPType.MATCH_PARTY,           PvP_Party_Prefix          },
            { ePvPType.MATCH_OVERLORD,           PvP_Overlord_Prefix          },
        };

        public static readonly Dictionary<ePvPType, string> PvP_Table_List = new Dictionary<ePvPType, string>()
        {
            { ePvPType.MATCH_FREE,           PvP_PvP_Weekly_TableName         },
            { ePvPType.MATCH_1VS1,           PvP_PvP_Season_TableName          },
            { ePvPType.MATCH_GUILD_3VS3,           PvP_GuildPvP_Monthly_TableName          },
            { ePvPType.MATCH_RUBY_PVP,           PvP_PvP_Weekly_TableName          },
        };

        public static readonly Dictionary<eCharacterRankType, string> CharacterRank_RedisKey_List = new Dictionary<eCharacterRankType, string>()
        {
            { eCharacterRankType.WARPOINT,           PvP_Warpoint_Prefix          },
        };

        public static readonly Dictionary<eCharacterRankType, string> CharacterRank_Table_List = new Dictionary<eCharacterRankType, string>()
        {
            { eCharacterRankType.WARPOINT,           Character_Define.CharacterTableName          },
        };

        public static readonly Dictionary<eGuildRankType, string> GuildRank_RedisKey_List = new Dictionary<eGuildRankType, string>()
        {
            { eGuildRankType.GUILDPOINT,           PvP_GuildRank_Prefix          },// 기여포인트
            { eGuildRankType.GUILDWARRANK,           PvP_GuildWarRank_Prefix          },// 길드전 점수
            { eGuildRankType.GUILDWARRANK_JOINER,           PvP_GuildWarJoinerRank_Prefix          },// 길드전 길드원 점수
        };

        public static readonly Dictionary<eGuildRankType, string> GuildRank_Table_List = new Dictionary<eGuildRankType, string>()
        {
            { eGuildRankType.GUILDPOINT,           GuildManager.GuildRankPointDBTableName          },
            { eGuildRankType.GUILDWARRANK,           GuildManager.PVP_GuildWarRecordDBTableName          },// 길드전 점수
            { eGuildRankType.GUILDWARRANK_JOINER,           GuildManager.PVP_GuildWarRecord_JoinerDBTableName          },// 길드전 길드원 점수 기록
        };


        public const double RankPointSeperater = 10000.0f;


        public enum ePvPReturnKeys
        {
            // free for all
            PvP_PlayInfo,
            PvP_PlayInfo_LastWeek,
            PvP_Rank,
            PvP_Top_Rank,
            PvP_TotalPlayer,
            PvP_Friend_Rank,
            PvP_PlayCount,
            PvP_PlayMaxCount,

            PvP_GuildWarRank,
            PvP_GuildWarRank_LastWeek,
            PvP_GuildWarMyPlayInfo,
            PvP_GuildWarMyPlayInfo_LastWeek,

            PvP_OpenTime,
            PvP_ServerTime,
            PvP_OpenFlag,
            PvP_BonusFlag,

            PvP_ConstList,
            PvP_BattleRewardList,

            PvP_Record,
            PvP_GuildRejoinTime,

            PvP_Achive_NewFlag,

            PvP_Overlord_UserInfo,
            PvP_Overlord_List,
            PvP_Overlord_PlayList,
            PvP_Overlord_RankList,
            PvP_Overlord_EnemyInfo,
            PvP_Overlord_MatchResult,
            PvP_Overlord_BeforeRank,
            PvP_Overlord_AfterRank,

            PvP_RubyPvP_List,
            PvP_Party_FriendList,
            PvP_Bot_User_Info,

            PvP_PartyDungeonClear_List,
        };

        public static readonly Dictionary<ePvPReturnKeys, string> PvP_Ret_KeyList = new Dictionary<ePvPReturnKeys, string>()
        {
            { ePvPReturnKeys.PvP_PlayInfo,           "my_pvp_play_info"          },
            { ePvPReturnKeys.PvP_PlayInfo_LastWeek,           "my_pvp_play_last_week_info"          },
            { ePvPReturnKeys.PvP_Rank,           "my_pvp_rank"          },
            { ePvPReturnKeys.PvP_Top_Rank,           "top_pvp_rank_list"          },
            { ePvPReturnKeys.PvP_TotalPlayer,           "total_player"          },
            { ePvPReturnKeys.PvP_Friend_Rank,           "friend_pvp_rank_list"          },
            { ePvPReturnKeys.PvP_PlayCount,           "pvp_play_count"          },
            { ePvPReturnKeys.PvP_PlayMaxCount,           "pvp_play_max_count"          },

            { ePvPReturnKeys.PvP_GuildWarRank,           "guildwar_rank"          },
            { ePvPReturnKeys.PvP_GuildWarRank_LastWeek,           "guildwar_rank_last_week"          },
            { ePvPReturnKeys.PvP_GuildWarMyPlayInfo,           "guildwar_my_play_info"          }, 
            { ePvPReturnKeys.PvP_GuildWarMyPlayInfo_LastWeek,           "guildwar_my_play_last_week_info"          },  

            { ePvPReturnKeys.PvP_OpenTime,           "pvp_open_time"          },  
            { ePvPReturnKeys.PvP_ServerTime,           "pvp_server_time"          },  

            { ePvPReturnKeys.PvP_OpenFlag,           "pvp_open"          },  
            { ePvPReturnKeys.PvP_BonusFlag,           "pvp_bonus"          },  

            { ePvPReturnKeys.PvP_ConstList,           "pvp_const_list"          },  
            { ePvPReturnKeys.PvP_BattleRewardList,           "pvp_reward_list"          },  
            { ePvPReturnKeys.PvP_Record,           "pvp_record"          },  
            { ePvPReturnKeys.PvP_GuildRejoinTime,           "pvp_guild_rejoin"          },  
            { ePvPReturnKeys.PvP_Achive_NewFlag,           "achieve_pvp_new"          },  

            { ePvPReturnKeys.PvP_Overlord_UserInfo,           "overlord_user_info"          },  
            { ePvPReturnKeys.PvP_Overlord_List,           "overlord_lobby"          },  
            { ePvPReturnKeys.PvP_Overlord_PlayList,           "overlord_play_list"          },  
            { ePvPReturnKeys.PvP_Overlord_RankList,           "overlord_rank_list"          },  
            { ePvPReturnKeys.PvP_Overlord_EnemyInfo,           "enemy_char_info"          },  
            { ePvPReturnKeys.PvP_Overlord_MatchResult,           "overlord_match_result"          },  
            { ePvPReturnKeys.PvP_Overlord_BeforeRank,           "overlord_before_rank"          },  
            { ePvPReturnKeys.PvP_Overlord_AfterRank,           "overlord_after_rank"          },  

            { ePvPReturnKeys.PvP_RubyPvP_List,           "ruby_pvp_char_list"          },  
            { ePvPReturnKeys.PvP_Party_FriendList,           "party_friends_info"          },  

            { ePvPReturnKeys.PvP_Bot_User_Info,           "bot_user_info"          },  
            { ePvPReturnKeys.PvP_PartyDungeonClear_List,           "party_dungeon_clear_list"          },  

        };

        public enum ePvPConstDef
        {
            DEF_RESET_COST_RUBY_3PVE,
            DEF_1VS1REAL_START_TIME,
            DEF_1VS1REAL_END_TIME,
            DEF_1VS1REAL_START_TIME_BONUS,
            DEF_1VS1REAL_END_TIME_BOUNS,
            DEF_BATTLE_FREEFORALL_START_TIME_1st,
            DEF_BATTLE_FREEFORALL_END_TIME_1st,
            DEF_BATTLE_FREEFORALL_START_TIME_2nd,
            DEF_BATTLE_FREEFORALL_END_TIME_2nd,
            DEF_PVP_GLADIATOR_START_TIME_1st,
            DEF_PVP_GLADIATOR_END_TIME_1st,
            DEF_PVP_GLADIATOR_START_TIME_2nd,
            DEF_PVP_GLADIATOR_END_TIME_2nd,
            DEF_BATTLE_GUILD_G3VS3_START_TIME_1st,
            DEF_BATTLE_GUILD_G3VS3_END_TIME_1st,
            DEF_BATTLE_GUILD_G3VS3_START_TIME_2nd,
            DEF_BATTLE_GUILD_G3VS3_END_TIME_2nd,

            DEF_PVP_PARTY_START_TIME_1st,
            DEF_PVP_PARTY_END_TIME_1st,
            DEF_PVP_PARTY_START_TIME_2nd,
            DEF_PVP_PARTY_END_TIME_2nd,

            DEF_PVP_FREEFORALL_REWARD_VALUE,
            DEF_PVP_FREEFORALL_BATTLEPOINT_TIER1_VALUE,
            DEF_PVP_FREEFORALL_BATTLEPOINT_TIER2_VALUE,
            DEF_PVP_FREEFORALL_BATTLEPOINT_TIER3_VALUE,
            DEF_PVP_FREEFORALL_BATTLEPOINT_TIER4_VALUE,
            DEF_PVP_FREEFORALL_BATTLEPOINT_TIER5_VALUE,
            DEF_PVP_FREEFORALL_BATTLEPOINT_TIER6_VALUE,
            DEF_PVP_FREEFORALL_BATTLEPOINT_TIER7_VALUE,
            DEF_PVP_FREEFORALL_BATTLEPOINT_TIER8_VALUE,

            DEF_GUILD_G3VS3_ENTER_MAX_COUNT,

            DEF_GUILD_G3VS3_MATCHINGINIT1,
            DEF_GUILD_G3VS3_MATCHINGINIT2,
            DEF_GUILD_G3VS3_MATCHINGINIT3,
            DEF_GUILD_G3VS3_MATCHINGINIT4,
            DEF_GUILD_G3VS3_MATCHINGINIT5,
            DEF_GUILD_G3VS3_MATCHINGINIT6,

            DEF_GUILD_G3VS3_ELOKCONST,
            DEF_GUILD_G3VS3_RESULT_WIN_POINT,
            DEF_GUILD_G3VS3_RESULT_LOSE_POINT,

            DEF_GUILD_G3VS3_COOLTIME_MINUTE,
            DEF_GUILD_G3VS3_COOLTIME_DELETE_RUBY,

            DEF_PVP_GLADIATOR_JOINCOST_GRADE1,
            DEF_PVP_GLADIATOR_JOINCOST_GRADE2,
            DEF_PVP_GLADIATOR_JOINCOST_GRADE3,
            DEF_PVP_GLADIATOR_WINNERGET_GRADE1,
            DEF_PVP_GLADIATOR_WINNERGET_GRADE2,
            DEF_PVP_GLADIATOR_WINNERGET_GRADE3,

            DEF_GLADIATOR_GOLD_1,
            DEF_GLADIATOR_GOLD_2,
            DEF_GLADIATOR_GOLD_3,

            DEF_GLADIATOR_GOLD_WINNER_1,
            DEF_GLADIATOR_GOLD_WINNER_2,
            DEF_GLADIATOR_GOLD_WINNER_3,

            BATTLE_RANKING_ENTER_MAX_COUNT,
            DEF_BATTLE_RANKING_ENTER_COST,
            DEF_BATTLE_RANKING_EX_TRY_RUBY,
            DEF_BATTLE_RANKING_1000_GET_RUBY,
        }

        public static readonly Dictionary<ePvPConstDef, string> PvP_Const_Def_Key_List = new Dictionary<ePvPConstDef, string>()
        {
            { ePvPConstDef.DEF_RESET_COST_RUBY_3PVE, "DEF_RESET_COST_RUBY_3PVE" },
            { ePvPConstDef.DEF_1VS1REAL_START_TIME, "DEF_1VS1REAL_START_TIME" },
            { ePvPConstDef.DEF_1VS1REAL_END_TIME, "DEF_1VS1REAL_END_TIME" },
            { ePvPConstDef.DEF_1VS1REAL_START_TIME_BONUS, "DEF_1VS1REAL_START_TIME_BONUS" },
            { ePvPConstDef.DEF_1VS1REAL_END_TIME_BOUNS, "DEF_1VS1REAL_END_TIME_BOUNS" },
            { ePvPConstDef.DEF_BATTLE_FREEFORALL_START_TIME_1st, "DEF_BATTLE_FREEFORALL_START_TIME_1st" },
            { ePvPConstDef.DEF_BATTLE_FREEFORALL_END_TIME_1st, "DEF_BATTLE_FREEFORALL_END_TIME_1st" },
            { ePvPConstDef.DEF_BATTLE_FREEFORALL_START_TIME_2nd, "DEF_BATTLE_FREEFORALL_START_TIME_2nd" },
            { ePvPConstDef.DEF_BATTLE_FREEFORALL_END_TIME_2nd, "DEF_BATTLE_FREEFORALL_END_TIME_2nd" },
            { ePvPConstDef.DEF_PVP_GLADIATOR_START_TIME_1st, "DEF_PVP_GLADIATOR_START_TIME_1st" },
            { ePvPConstDef.DEF_PVP_GLADIATOR_END_TIME_1st, "DEF_PVP_GLADIATOR_END_TIME_1st" },
            { ePvPConstDef.DEF_PVP_GLADIATOR_START_TIME_2nd, "DEF_PVP_GLADIATOR_START_TIME_2nd" },
            { ePvPConstDef.DEF_PVP_GLADIATOR_END_TIME_2nd, "DEF_PVP_GLADIATOR_END_TIME_2nd" },
            { ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_START_TIME_1st, "DEF_BATTLE_GUILD_G3VS3_START_TIME_1st" },
            { ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_END_TIME_1st, "DEF_BATTLE_GUILD_G3VS3_END_TIME_1st" },
            { ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_START_TIME_2nd, "DEF_BATTLE_GUILD_G3VS3_START_TIME_2nd" },
            { ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_END_TIME_2nd, "DEF_BATTLE_GUILD_G3VS3_END_TIME_2nd" },
            { ePvPConstDef.DEF_PVP_FREEFORALL_REWARD_VALUE, "DEF_PVP_FREEFORALL_REWARD_VALUE" },

            { ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER1_VALUE, "DEF_PVP_FREEFORALL_BATTLEPOINT_TIER1_VALUE" },
            { ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER2_VALUE, "DEF_PVP_FREEFORALL_BATTLEPOINT_TIER2_VALUE" },
            { ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER3_VALUE, "DEF_PVP_FREEFORALL_BATTLEPOINT_TIER3_VALUE" },
            { ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER4_VALUE, "DEF_PVP_FREEFORALL_BATTLEPOINT_TIER4_VALUE" },
            { ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER5_VALUE, "DEF_PVP_FREEFORALL_BATTLEPOINT_TIER5_VALUE" },
            { ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER6_VALUE, "DEF_PVP_FREEFORALL_BATTLEPOINT_TIER6_VALUE" },
            { ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER7_VALUE, "DEF_PVP_FREEFORALL_BATTLEPOINT_TIER7_VALUE" },
            { ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER8_VALUE, "DEF_PVP_FREEFORALL_BATTLEPOINT_TIER8_VALUE" },

            { ePvPConstDef.DEF_GUILD_G3VS3_ENTER_MAX_COUNT, "DEF_GUILD_G3VS3_ENTER_MAX_COUNT" },
            { ePvPConstDef.DEF_GUILD_G3VS3_MATCHINGINIT1, "DEF_GUILD_G3VS3_MATCHINGINIT1" },
            { ePvPConstDef.DEF_GUILD_G3VS3_MATCHINGINIT2, "DEF_GUILD_G3VS3_MATCHINGINIT2" },
            { ePvPConstDef.DEF_GUILD_G3VS3_MATCHINGINIT3, "DEF_GUILD_G3VS3_MATCHINGINIT3" },
            { ePvPConstDef.DEF_GUILD_G3VS3_MATCHINGINIT4, "DEF_GUILD_G3VS3_MATCHINGINIT4" },
            { ePvPConstDef.DEF_GUILD_G3VS3_MATCHINGINIT5, "DEF_GUILD_G3VS3_MATCHINGINIT5" },
            { ePvPConstDef.DEF_GUILD_G3VS3_MATCHINGINIT6, "DEF_GUILD_G3VS3_MATCHINGINIT6" },

            { ePvPConstDef.DEF_GUILD_G3VS3_ELOKCONST, "DEF_GUILD_G3VS3_ELOKCONST" },
            { ePvPConstDef.DEF_GUILD_G3VS3_RESULT_WIN_POINT, "DEF_GUILD_G3VS3_RESULT_WIN_POINT" },
            { ePvPConstDef.DEF_GUILD_G3VS3_RESULT_LOSE_POINT, "DEF_GUILD_G3VS3_RESULT_LOSE_POINT" },

            { ePvPConstDef.DEF_GUILD_G3VS3_COOLTIME_MINUTE, "DEF_GUILD_G3VS3_COOLTIME_MINUTE" },
            { ePvPConstDef.DEF_GUILD_G3VS3_COOLTIME_DELETE_RUBY, "DEF_GUILD_G3VS3_COOLTIME_DELETE_RUBY" },

            { ePvPConstDef.DEF_PVP_PARTY_START_TIME_1st, "DEF_PVP_PARTY_START_TIME_1st" },
            { ePvPConstDef.DEF_PVP_PARTY_END_TIME_1st, "DEF_PVP_PARTY_END_TIME_1st" },
            { ePvPConstDef.DEF_PVP_PARTY_START_TIME_2nd, "DEF_PVP_PARTY_START_TIME_2nd" },
            { ePvPConstDef.DEF_PVP_PARTY_END_TIME_2nd, "DEF_PVP_PARTY_END_TIME_2nd" },
            
            { ePvPConstDef.DEF_PVP_GLADIATOR_JOINCOST_GRADE1, "DEF_PVP_GLADIATOR_JOINCOST_GRADE1" },
            { ePvPConstDef.DEF_PVP_GLADIATOR_JOINCOST_GRADE2, "DEF_PVP_GLADIATOR_JOINCOST_GRADE2" },
            { ePvPConstDef.DEF_PVP_GLADIATOR_JOINCOST_GRADE3, "DEF_PVP_GLADIATOR_JOINCOST_GRADE3" },
            { ePvPConstDef.DEF_PVP_GLADIATOR_WINNERGET_GRADE1, "DEF_PVP_GLADIATOR_WINNERGET_GRADE1" },
            { ePvPConstDef.DEF_PVP_GLADIATOR_WINNERGET_GRADE2, "DEF_PVP_GLADIATOR_WINNERGET_GRADE2" },
            { ePvPConstDef.DEF_PVP_GLADIATOR_WINNERGET_GRADE3, "DEF_PVP_GLADIATOR_WINNERGET_GRADE3" },

            { ePvPConstDef.BATTLE_RANKING_ENTER_MAX_COUNT, "BATTLE_RANKING_ENTER_MAX_COUNT" },
            { ePvPConstDef.DEF_BATTLE_RANKING_ENTER_COST, "DEF_BATTLE_RANKING_ENTER_COST" },
            { ePvPConstDef.DEF_BATTLE_RANKING_EX_TRY_RUBY, "DEF_BATTLE_RANKING_EX_TRY_RUBY" },
            { ePvPConstDef.DEF_BATTLE_RANKING_1000_GET_RUBY, "DEF_BATTLE_RANKING_1000_GET_RUBY" },

            { ePvPConstDef.DEF_GLADIATOR_GOLD_1, "DEF_GLADIATOR_GOLD_1" },
            { ePvPConstDef.DEF_GLADIATOR_GOLD_2, "DEF_GLADIATOR_GOLD_2" },
            { ePvPConstDef.DEF_GLADIATOR_GOLD_3, "DEF_GLADIATOR_GOLD_3" },

            { ePvPConstDef.DEF_GLADIATOR_GOLD_WINNER_1, "DEF_GLADIATOR_GOLD_WINNER_1" },
            { ePvPConstDef.DEF_GLADIATOR_GOLD_WINNER_2, "DEF_GLADIATOR_GOLD_WINNER_2" },
            { ePvPConstDef.DEF_GLADIATOR_GOLD_WINNER_3, "DEF_GLADIATOR_GOLD_WINNER_3" },
        };

        public static readonly ePvPConstDef[] PvP_Free_Grade_Const_Key =
        {
            ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER1_VALUE,
            ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER1_VALUE,
            ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER2_VALUE,
            ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER3_VALUE,
            ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER4_VALUE,
            ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER5_VALUE,
            ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER6_VALUE,
            ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER7_VALUE,
            ePvPConstDef.DEF_PVP_FREEFORALL_BATTLEPOINT_TIER8_VALUE,
        };


        public static readonly Dictionary<KeyValuePair<int, int>, ePvPConstDef> PvPGuildRating_BasePointList = new Dictionary<KeyValuePair<int, int>, ePvPConstDef>()
        {
            { new KeyValuePair<int,int>(19, 28), ePvPConstDef.DEF_GUILD_G3VS3_MATCHINGINIT1 },
            { new KeyValuePair<int,int>(29, 33), ePvPConstDef.DEF_GUILD_G3VS3_MATCHINGINIT2 },
            { new KeyValuePair<int,int>(34, 38), ePvPConstDef.DEF_GUILD_G3VS3_MATCHINGINIT3 },
            { new KeyValuePair<int,int>(39, 43), ePvPConstDef.DEF_GUILD_G3VS3_MATCHINGINIT4 },
            { new KeyValuePair<int,int>(44, 48), ePvPConstDef.DEF_GUILD_G3VS3_MATCHINGINIT5 },
            { new KeyValuePair<int,int>(49, 99999), ePvPConstDef.DEF_GUILD_G3VS3_MATCHINGINIT6 },
        };

        public struct PvPOpenTimeKey
        {
            public ePvPConstDef StartTime;
            public ePvPConstDef EndTime;

            public PvPOpenTimeKey(ePvPConstDef setStart, ePvPConstDef setEnd)
            {
                StartTime = setStart;
                EndTime = setEnd;
            }
        }

        public static readonly Dictionary<ePvPType, PvPOpenTimeKey> PvP_OpenTime_1st_Const_List = new Dictionary<ePvPType, PvPOpenTimeKey>()
        {
            { ePvPType.MATCH_1VS1, new PvPOpenTimeKey(ePvPConstDef.DEF_1VS1REAL_START_TIME, ePvPConstDef.DEF_1VS1REAL_END_TIME) },
            { ePvPType.MATCH_FREE, new PvPOpenTimeKey(ePvPConstDef.DEF_BATTLE_FREEFORALL_START_TIME_1st, ePvPConstDef.DEF_BATTLE_FREEFORALL_END_TIME_1st) },
            { ePvPType.MATCH_RUBY_PVP, new PvPOpenTimeKey(ePvPConstDef.DEF_PVP_GLADIATOR_START_TIME_1st, ePvPConstDef.DEF_PVP_GLADIATOR_END_TIME_1st) },
            { ePvPType.MATCH_GUILD_3VS3, new PvPOpenTimeKey(ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_START_TIME_1st, ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_END_TIME_1st) },
        };

        public static readonly Dictionary<ePvPType, PvPOpenTimeKey> PvP_OpenTime_2nd_Const_List = new Dictionary<ePvPType, PvPOpenTimeKey>()
        {
            { ePvPType.MATCH_1VS1, new PvPOpenTimeKey(ePvPConstDef.DEF_1VS1REAL_START_TIME_BONUS, ePvPConstDef.DEF_1VS1REAL_END_TIME_BOUNS) },
            { ePvPType.MATCH_FREE, new PvPOpenTimeKey(ePvPConstDef.DEF_BATTLE_FREEFORALL_START_TIME_2nd, ePvPConstDef.DEF_BATTLE_FREEFORALL_END_TIME_2nd) },
            { ePvPType.MATCH_RUBY_PVP, new PvPOpenTimeKey(ePvPConstDef.DEF_PVP_GLADIATOR_START_TIME_2nd, ePvPConstDef.DEF_PVP_GLADIATOR_END_TIME_2nd) },
            { ePvPType.MATCH_GUILD_3VS3, new PvPOpenTimeKey(ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_START_TIME_2nd, ePvPConstDef.DEF_BATTLE_GUILD_G3VS3_END_TIME_2nd) },
        };
        
        public struct FindEnemyRange
        {
            public short getCount;
            public double minRate;
            public double maxRate;

            public FindEnemyRange(short setCount, double setMin, double setMax)
            {
                getCount = setCount;
                minRate = setMin;
                maxRate = setMax;
            }
        };

        public const int BotMatchBaseRange_Min = -50;
        public const int BotMatchBaseRange_Max = -40;        

        public enum ePvPPlayFlag
        {
            None = 0,
            Play = 1,
        }

        public const int PercentageDivede = 100;
        public const int RankingTop1 = 1;
        public const int MinimumJumpRank = 20;
        public const int Overlord_NPC_Seperator = 10000000;
        public const int Overlord_Play_Time_Min = 11;
        public const int Overlord_Play_List_Max = 4;
        public const int Overlord_HighGradeReward_Min = 1001;
        public const int PvP_Overlord_Ranking_Dummy_Count = 30;

        public static readonly List<PvP_Define.FindEnemyRange> GetEnemyRangeList = new List<PvP_Define.FindEnemyRange>()
        {
            new PvP_Define.FindEnemyRange(2, -50, -30),
            new PvP_Define.FindEnemyRange(2, -30, -20),
            new PvP_Define.FindEnemyRange(2, -20, -10),
            new PvP_Define.FindEnemyRange(2, -10, 0),
            new PvP_Define.FindEnemyRange(2, 0, 10),
        };

        public static readonly List<PvP_Define.FindEnemyRange> GetEnemyRangeReverseList = new List<PvP_Define.FindEnemyRange>()
        {
            new PvP_Define.FindEnemyRange(2, 0, 10),
            new PvP_Define.FindEnemyRange(2, -10, 0),
            new PvP_Define.FindEnemyRange(2, -20, -10),
            new PvP_Define.FindEnemyRange(2, -30, -20),
            new PvP_Define.FindEnemyRange(2, -50, -30),
        };

        public enum ePvPRewardRepeatType
        {
            Once = 0,
            Daily = 1,
            Weekly,
            Season,
            Monthly,
        }

        public static readonly Dictionary<ePvPType, List<ePvPRewardRepeatType>> PvPReward_CheckList = new Dictionary<ePvPType, List<ePvPRewardRepeatType>>()
        {
            { ePvPType.MATCH_1VS1, new List<ePvPRewardRepeatType>() { ePvPRewardRepeatType.Daily, ePvPRewardRepeatType.Weekly, ePvPRewardRepeatType.Season } },
            { ePvPType.MATCH_FREE, new List<ePvPRewardRepeatType>() { ePvPRewardRepeatType.Daily } },
            { ePvPType.MATCH_GUILD_3VS3, new List<ePvPRewardRepeatType>() { ePvPRewardRepeatType.Daily, ePvPRewardRepeatType.Weekly, ePvPRewardRepeatType.Monthly} },
            { ePvPType.MATCH_OVERLORD, new List<ePvPRewardRepeatType>() { ePvPRewardRepeatType.Daily } },
        };

        // not use yet. now use key only MATCH_PARTY
        //public static readonly Dictionary<int, VIP_Define.eVipType> PvP_VIP_KeyList = new Dictionary<int, VIP_Define.eVipType>()
        //{
        //    { (int)ePvPType.MATCH_PARTY, VIP_Define.eVipType.DUNGEONCOUNT_RESET_CO_OP },
        //};
            //MATCH_NONE = -1,
            //MATCH_FREE = 1,
            //MATCH_1VS1 = 2,
            //MATCH_GUILD_3VS3 = 3,
            //// Ruby PvP match - add by manstar 02/12/2015
            //MATCH_RUBY_PVP = 4,
            //MATCH_FRIEND_1VS1 = 5,	// add by manstar 02/09/2015
            //MATCH_PARTY = 6,		// 파티 던젼 
            //MATCH_OVERLORD = 7,      //패왕의 길. 랭킹 뺏기.overlord
            //MATCH_TEAM,    //삭제예정
            //MATCH_COUNT = MATCH_TEAM,
    }
}
