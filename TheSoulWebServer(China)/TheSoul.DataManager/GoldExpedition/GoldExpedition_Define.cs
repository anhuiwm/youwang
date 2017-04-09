using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSoul.DataManager
{
    public class GoldExpedition_Define
    {
        public const string GoldExpedition_Guild_Info_DB = "common";
        public const string GoldExpedition_Info_DB = "sharding";

        public const string System_Expedition_Dungeon_TableName = "System_Expedition_Dungeon";
        public const string User_GE_Stage_Info_TableName = "User_GE_Stage_Info";
        public const string User_GE_CharacterGroup_TableName = "User_GE_CharacterGroup";
        public const string User_GE_Boost_Item_TableName = "User_GE_Boost_Item";
        public const string User_GE_Stage_Enemy_TableName = "User_GE_Stage_Enemy";
        public const string User_Guild_Mercenary_Info_TableName = "User_Guild_Mercenary_Info";
        public const string User_WarPoint_Table_Name = "User_WarPoint";

        public const int GoldExpeditionEnemyCount = 15;
        public const string UserGE_Prefix = "UserGE";
        public const string SystemGE_Prefix = "SystemGE";

        public const int PercentageDivede = 100;
        public const int LimitUserLevel = 10;
        public const int LimitTopPlayer = 100;

        public const float Boost3RecorveryHPRate = 0.5f;
        public const float DayResetCount = 1;

        public const short SerchOverCountRate = 3;

        public const int MercenaryTimeIncomeMinutes = 10;
        public const int MercenaryTimeIncomeGoldPerLevel = 10;
        public const int MercenaryEmployIncomeGoldPerLevel = 1000;
        public const float MercenaryEmployIncomeRate = 0.3f;
        public const int MercenaryIncomGold_LimteMax = 999999;
        public const int MercenaryCallLimitTime = 60 * 30;  // sec * min (30min)

        public const long GE_Fake_Gold_ItemID = 303000001; // hard coding - not good for me :(
        public const int Bot_Warpoint_Min = 50770;
        public const int Bot_Warpoint_Max = 51156;


        public enum eGEBoostType
        {
            BoostATK,
            BoostDEF,
            BoostHP,
        }

        public static readonly Dictionary<eGEBoostType, long> GetBoostItemIDList = new Dictionary<eGEBoostType, long>()
        {
            { eGEBoostType.BoostATK, 303000034},
            { eGEBoostType.BoostDEF, 303000035},
            { eGEBoostType.BoostHP, 303000036},
        };

        public static readonly List<PvP_Define.FindEnemyRange> GetEnemyRangeList = new List<PvP_Define.FindEnemyRange>()
        {
            new PvP_Define.FindEnemyRange(3, -10, 0),
            new PvP_Define.FindEnemyRange(3, 0, 10),
            new PvP_Define.FindEnemyRange(3, 10, 20),
            new PvP_Define.FindEnemyRange(3, 20, 30),
            new PvP_Define.FindEnemyRange(3, 30, 50),
        };

        public enum eGEConstDef
        {
            DEF_EXPEDITION_MATCHING_VALUE,
            DEF_EXPEDITION_HIRE_RUBY_COST,
            DEF_EXPEDITION_DUNGEON_REWARD_GOLDBASE,
            DEF_EXPEDITION_DUNGEON_REWARD_LEVEL1,
            DEF_EXPEDITION_DUNGEON_REWARD_LEVEL2,
            DEF_EXPEDITION_DUNGEON_REWARD_GOLD1,
            DEF_EXPEDITION_DUNGEON_REWARD_GOLD2,
            DEF_EXPEDITION_DUNGEON_REWARD_STAGECOUNT,
        }

        public static readonly Dictionary<eGEConstDef, string> GE_Const_Def_Key_List = new Dictionary<eGEConstDef, string>()
        {
            { eGEConstDef.DEF_EXPEDITION_MATCHING_VALUE, "DEF_EXPEDITION_MATCHING_VALUE" },
            { eGEConstDef.DEF_EXPEDITION_HIRE_RUBY_COST, "DEF_EXPEDITION_HIRE_RUBY_COST" },
            { eGEConstDef.DEF_EXPEDITION_DUNGEON_REWARD_GOLDBASE, "DEF_EXPEDITION_DUNGEON_REWARD_GOLDBASE" },
            { eGEConstDef.DEF_EXPEDITION_DUNGEON_REWARD_LEVEL1, "DEF_EXPEDITION_DUNGEON_REWARD_LEVEL1" },
            { eGEConstDef.DEF_EXPEDITION_DUNGEON_REWARD_LEVEL2, "DEF_EXPEDITION_DUNGEON_REWARD_LEVEL2" },
            { eGEConstDef.DEF_EXPEDITION_DUNGEON_REWARD_GOLD1, "DEF_EXPEDITION_DUNGEON_REWARD_GOLD1" },
            { eGEConstDef.DEF_EXPEDITION_DUNGEON_REWARD_GOLD2, "DEF_EXPEDITION_DUNGEON_REWARD_GOLD2" },
            { eGEConstDef.DEF_EXPEDITION_DUNGEON_REWARD_STAGECOUNT, "DEF_EXPEDITION_DUNGEON_REWARD_STAGECOUNT" },
        };



        // old code

        public const string GameData_ExpeditionDungeon_TableName = "GameData_ExpeditionDungeon";

        public const string User_ExpeditionInfo_TableName = "GoldExpeditionInfo";
        public const string User_GoldExpeditionSnapshot_TableName = "GoldExpeditionSnapshot";
        public const string User_GoldExpeditionStageInfo_TableName = "GoldExpeditionStageInfo";
        public const string User_GoldExpeditionCharInfo_TableName = "GoldExpeditionCharInfo";

        public const string GoldExpedition_Prefix = "GoldExpedition";

        public enum eGEReturnKeys
        {
            Clear_Stage,
            MyCharacterInfo,
            AllyUserName,
            AllyCharacterInfo,
            AllyCharacterDetailInfo,
            StageInfo,
            BoostInfo,
            DungeonID,
            ResetCount,
            GE_CharacterGruop,
            MyMercenaryInfo,
            GuildMercenaryInfo,
        };

        public static readonly Dictionary<eGEReturnKeys, string> GE_Ret_KeyList = new Dictionary<eGEReturnKeys, string>()
        {
            { eGEReturnKeys.Clear_Stage,           "clear_stage"          },
            { eGEReturnKeys.MyCharacterInfo,           "my_character_info"          },
            { eGEReturnKeys.AllyUserName,           "ally_name"          },
            { eGEReturnKeys.AllyCharacterInfo,           "ally_character_info"          },
            { eGEReturnKeys.AllyCharacterDetailInfo,           "ally_character_detail_info"          },
            { eGEReturnKeys.StageInfo,           "stage_info"          },
            { eGEReturnKeys.BoostInfo,           "boost_info"          },
            { eGEReturnKeys.DungeonID,           "dungeonid"          },
            { eGEReturnKeys.ResetCount,           "reset_count"          },
            { eGEReturnKeys.GE_CharacterGruop,           "ge_character_group"          },
            { eGEReturnKeys.MyMercenaryInfo,           "my_mercenary_info"          },
            { eGEReturnKeys.GuildMercenaryInfo,           "guild_mercenary_info"          },
        };
    }
}
