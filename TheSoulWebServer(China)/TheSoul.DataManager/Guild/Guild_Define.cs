using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSoul.DataManager
{
    public static class Guild_Define
    {
        public enum ePlayType
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

            MISSION_PALY = 100, //시나리오
            DARK_PASSAGE = 101, //어둠의 통로
            ELIETE_DUNGEON = 102,   // 정예던전
            BOSSRAID = 103,         //보스레이드
            GOLDEXPEDITION = 104,    //황금원정단
        };
        
        public static readonly Dictionary<ePlayType, int> AddGuildPoint_List = new Dictionary<ePlayType, int>()
        {
            //palyType에 따라 해당 값이 길드 포인트로 적립됨.
            { ePlayType.MATCH_FREE,         100 },
            { ePlayType.MATCH_1VS1,         100 },
            { ePlayType.MATCH_GUILD_3VS3,   100 },
            { ePlayType.MATCH_OVERLORD,     100 },
            { ePlayType.MATCH_PARTY,        100 },
            { ePlayType.MISSION_PALY,        50 },
            { ePlayType.DARK_PASSAGE,        80 },
            { ePlayType.ELIETE_DUNGEON,        100 },
            { ePlayType.BOSSRAID,        50 },
            { ePlayType.GOLDEXPEDITION,        50 },
            
        };

        public enum eGuildReturnKeys
        {
            GuildID,
            GuildLevel,
            YesterDayAttend,
            ToDayAttend,
        };

        public static readonly Dictionary<eGuildReturnKeys, string> Guild_Ret_KeyList = new Dictionary<eGuildReturnKeys, string>()
        {
            { eGuildReturnKeys.GuildID,           "gid"          },
            { eGuildReturnKeys.GuildLevel,           "guild_level"          },
            { eGuildReturnKeys.YesterDayAttend,           "yesterday_count"          },
            { eGuildReturnKeys.ToDayAttend,           "today_count"          },
        };


    }
}
