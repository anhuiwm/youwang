using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSoul.DataManager
{
    public static partial class PvP_Define
    {
        public const string ServerRankingReward_DB_Info = "sharding";
        public const string System_ServerCreate_RankingReward_TableName = "ServerCreate_RankingReward";
        public const string System_Battle_Reward_TableName = "System_Battle_Reward";
        public const int BasePvPPoint = 1000;
        public const int BaseGuildPvPPoint = 3000;
        public const long RewardItemID_Ruby = 303000005;
        public const string PvP_Reward_Prefix = "PVP_Reward";
        public const string PvP_User_PartyDungeon_Clear_TableName = "User_PartyDungeon_Clear";
        public const string PvP_System_Party_Dungeon_TableName = "System_Party_Dungeon";
        
        public static readonly Dictionary<string, ePvPType> PvPBattleReawrd_PvPTypeList = new Dictionary<string, ePvPType>()
        {
            { "BattleType_Ranking", ePvPType.MATCH_OVERLORD },
            { "BattleType_1VS1", ePvPType.MATCH_1VS1 },
            { "BattleType_G3VS3", ePvPType.MATCH_GUILD_3VS3 },
            { "BattleType_FREEFORALL", ePvPType.MATCH_FREE },
        };

        public static readonly Dictionary<ePvPType, string> PvPType_BattleRewardStringList = new Dictionary<ePvPType, string>()
        {
            { ePvPType.MATCH_OVERLORD, "BattleType_Ranking" },
            { ePvPType.MATCH_1VS1, "BattleType_1VS1" },
            { ePvPType.MATCH_GUILD_3VS3, "BattleType_G3VS3" },
            { ePvPType.MATCH_FREE, "BattleType_FREEFORALL" },
        };        

        public static readonly Dictionary<KeyValuePair<ePvPType, ePvPRewardRepeatType>, string> PvPType_BattleRewardMailStringList = new Dictionary<KeyValuePair<ePvPType, ePvPRewardRepeatType>, string>()
        {
            { new KeyValuePair<ePvPType, ePvPRewardRepeatType>( ePvPType.MATCH_OVERLORD, ePvPRewardRepeatType.Daily ) , "#STRING_MSG_MAIL_RANKING_REWARD" }, // 패왕의길 {0:d} 위 보상을 받았습니다.
            { new KeyValuePair<ePvPType, ePvPRewardRepeatType>( ePvPType.MATCH_OVERLORD, ePvPRewardRepeatType.Once ) , "#STRING_MSG_MAIL_RANKING_TOP_REWARD" }, // 패왕의길 1000등이내 최초 순위 달성 보상을 받았습니다. {0:d}위 달성!
            { new KeyValuePair<ePvPType, ePvPRewardRepeatType>( ePvPType.MATCH_1VS1, ePvPRewardRepeatType.Daily ) , "#STRING_MSG_MAIL_1VSV1REAL_REWARD_DAY" }, // 투신전 {0:s} 등급 일일 보상을 받았습니다.
            { new KeyValuePair<ePvPType, ePvPRewardRepeatType>( ePvPType.MATCH_1VS1, ePvPRewardRepeatType.Weekly ) , "#STRING_MSG_MAIL_1VSV1REAL_REWARD_WEEK" }, // 투신전 {0:s} 등급 주간 보상을 받았습니다.
            { new KeyValuePair<ePvPType, ePvPRewardRepeatType>( ePvPType.MATCH_1VS1, ePvPRewardRepeatType.Season ) , "#STRING_MSG_MAIL_1VSV1REAL_REWARD_SEASON" }, // 투신전 {0:s} 등급 시즌 보상을 받았습니다.
            { new KeyValuePair<ePvPType, ePvPRewardRepeatType>( ePvPType.MATCH_FREE, ePvPRewardRepeatType.Daily ) , "#STRING_MSG_MAIL_FREEFORALL_REWARD" }, // 난전 랭킹 {0:s}위 일일 보상을 받았습니다.
            { new KeyValuePair<ePvPType, ePvPRewardRepeatType>( ePvPType.MATCH_GUILD_3VS3, ePvPRewardRepeatType.Daily ) , "#STRING_MSG_MAIL_GUILD_G3VS3_REWARD_DAY" }, // 길드전 랭킹 {0:s}위 일일 보상을 받았습니다.
            { new KeyValuePair<ePvPType, ePvPRewardRepeatType>( ePvPType.MATCH_GUILD_3VS3, ePvPRewardRepeatType.Weekly ) , "#STRING_MSG_MAIL_GUILD_G3VS3_REWARD_WEEK" }, // 길드전 랭킹 {0:s}위 주간 보상을 받았습니다.
            { new KeyValuePair<ePvPType, ePvPRewardRepeatType>( ePvPType.MATCH_GUILD_3VS3, ePvPRewardRepeatType.Monthly ) , "#STRING_MSG_MAIL_GUILD_G3VS3_REWARD_MONTH" }, // 길드전 랭킹 {0:s}위 월간 보상을 받았습니다.
        };
    }
}
