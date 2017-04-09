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


namespace TheSoul.DataManager
{
    public static class Dungeon_Define
    {
        public const string Dungeon_Info_DB = "sharding";

        public const string Mission_World_TableName = "System_Mission_World";
        public const string Mission_World_Surfix = "Info";

        public const string Mission_Stage_TableName = "System_Mission_Stage";
        public const string Mission_Stage_Surfix = "Info";

        public const string Mission_Play_TableName = "User_Mission_Play";
        public const string Mission_Play_Surfix = "Info";

        public const string World_Rank_Reward_TableName = "User_Mission_World_Rank_Reward";

        public const string Dark_Passage_TableName = "System_Guerilla_Dungeon";
        public const string Dark_Passage_Soul_TableName = "System_Guerrilla_Soul";
        public const string Eliete_Dungeon_TableName = "System_Elite_Dungeon";

        public const string Dark_Passage_Play_TableName = "User_GuerrillaDungeon_Play";
        public const string EliteDungeon_Play_TableName = "User_EliteDungeon_Play";

        public const string System_Booster_Group_TableName = "System_Booster_Group";

        public const string Mission_Task_TableName = "System_Mission_Stage_Task";
        public const string Mission_Task_Surfix = "Info";

        public const byte Rank1Star = 1;
        public const byte Rank2Star = 2;
        public const byte Rank3Star = 3;

        public const double ClearFailDivideValue = 3.0f;

        //public static List<string> TaskCheckTypeMaxOver = new List<string>() { "Time1", "Skill2", "HP2", };
        //public static List<string> TaskCheckTypeZeroOver = new List<string>() { "Item", "Die", };
        //public static List<string> TaskCheckTypeMaxLower = new List<string>() { "HP1", };
        //public static List<string> TaskCheckTypeIDOver = new List<string>() { "Time2", "Kill", };

        public enum eTaskCheckType
        {
            Combo,
            Die,
            HP1,
            HP2,
            Item,
            Kill,
            Skill2,
            Time1,
            Time2,
        }
        public static readonly Dictionary<string, eTaskCheckType> Dungeon_Task_TypeList = new Dictionary<string, eTaskCheckType>()
        {
            { "Combo", eTaskCheckType.Combo },
            { "Die", eTaskCheckType.Die },
            { "HP1", eTaskCheckType.HP1 },
            { "HP2", eTaskCheckType.HP2 },
            { "Item", eTaskCheckType.Item },
            { "Kill", eTaskCheckType.Kill },
            { "Skill2", eTaskCheckType.Skill2 },
            { "Time1", eTaskCheckType.Time1 },
            { "Time2", eTaskCheckType.Time2 },
        };

        public const int PVEDummyMakeItemCount = 2;

        //public const int PVERankReward_Need_Rank_First = 11;
        //public const int PVERankReward_Need_Rank_Second = 21;

        public enum eDungeonBoostType
        {
            ContentType_Senario,
            ContentType_Guerilla,
            ContentType_Elite,
            ContentType_BossRaid,
            ContentType_Expedition
        }
        public static readonly Dictionary<eDungeonBoostType, string> Dungeon_Boost_KeyList = new Dictionary<eDungeonBoostType, string>()
        {
            { eDungeonBoostType.ContentType_Senario,           "ContentType_Senario"          },
            { eDungeonBoostType.ContentType_Guerilla,           "ContentType_Guerilla"          },
            { eDungeonBoostType.ContentType_Elite,           "ContentType_Elite"          },
            { eDungeonBoostType.ContentType_BossRaid,           "ContentType_BossRaid"          },
            { eDungeonBoostType.ContentType_Expedition,           "ContentType_Expedition"          },
        };


        public enum eDungeonReturnKeys
        {
            Show_Item,
            Dummy_Item,
            TaskInfo,
            BeforeInfo,
            MissionRankList,
            GuerrillaRankList,
            EliteRankList,
            WorldRankList,
            LastWorld,
            LeftKey,
            LeftKeyFillMax,
            LeftKeyRemainTime,
            LeftTikcet,
            LeftTikcetFillMax,
            LeftTikcetRemainTime,
            LeftGold,
            BonusSoulInfo,
            EliteDungeonList,
            MissionLastID,
            ElieteDungeonLastID,
            ClearRank,
            MaxTryCount,
            CurrentCount,
            BeforeGold,
            BeforeCharacterLevel,
            BeforeCharacterExp,
            CharacterLevel,
            CharacterExp,
            CurrentResetCount,
        };

        public static readonly Dictionary<eDungeonReturnKeys, string> Dungeon_Ret_KeyList = new Dictionary<eDungeonReturnKeys, string>()
        {
            { eDungeonReturnKeys.Show_Item,           "showitem"          },
            { eDungeonReturnKeys.Dummy_Item,          "dummyitem"          },
            { eDungeonReturnKeys.TaskInfo,            "taskinfo"          },
            { eDungeonReturnKeys.BeforeInfo,          "beforeinfo"          },
            { eDungeonReturnKeys.MissionRankList,          "missionranklist"          },
            { eDungeonReturnKeys.GuerrillaRankList,          "guerrilladungeonranklist"          },
            { eDungeonReturnKeys.EliteRankList,          "elitedungeonranklist"          },
            { eDungeonReturnKeys.WorldRankList,          "worldranklist"          },
            { eDungeonReturnKeys.LastWorld,          "lastworld"          },
            { eDungeonReturnKeys.LeftKey,          "key"          },
            { eDungeonReturnKeys.LeftKeyFillMax,          "keyfillmax"          },
            { eDungeonReturnKeys.LeftKeyRemainTime,          "keyremainchargesec"          },
            { eDungeonReturnKeys.LeftGold,          "gold"          },
            { eDungeonReturnKeys.BonusSoulInfo,          "bonus_soul"          },
            { eDungeonReturnKeys.EliteDungeonList,          "elitedungeonlist"          },
            { eDungeonReturnKeys.MissionLastID,          "laststage"          },
            { eDungeonReturnKeys.ElieteDungeonLastID,          "selectedidx"          },
            { eDungeonReturnKeys.ClearRank,          "rank"          },
            { eDungeonReturnKeys.MaxTryCount,          "maxtrycount"          },
            { eDungeonReturnKeys.CurrentCount,          "currentcount"          },
            { eDungeonReturnKeys.BeforeGold,          "before_gold"          },
            { eDungeonReturnKeys.BeforeCharacterLevel,          "before_level"          },
            { eDungeonReturnKeys.BeforeCharacterExp,          "before_exp"          },
            { eDungeonReturnKeys.CharacterLevel,          "level"          },
            { eDungeonReturnKeys.CharacterExp,          "exp"          },
            { eDungeonReturnKeys.CurrentResetCount,          "currentresetcount"          },
            { eDungeonReturnKeys.LeftTikcet,          "ticket"          },
            { eDungeonReturnKeys.LeftTikcetFillMax,          "ticketfillmax"          },
            { eDungeonReturnKeys.LeftTikcetRemainTime,          "ticketremainchargesec"          },

        };

        public enum eDungenConstDef
        {
            DEF_ENERGY_PVE_TIME_PERIOD,
            DEF_ENERGY_PVP_TIME_PERIOD,
            DEF_ENERGY_G3VS3_TIME_PERIOD,
            DEF_PC_REVIVE_PRICE_RUBY,

            DEF_RESET_COST_RUBY_SCENARIO,
            DEF_RESET_COST_RUBY_BOSS,
            DEF_RESET_COST_RUBY_DARK,
            DEF_RESET_COST_RUBY_ELITE,
            DEF_BLACKMARKET_POWDOR_NUM,
            DEF_SCENARIO_COMPLETE_GIFT_STEP1,
            DEF_SCENARIO_COMPLETE_GIFT_STEP2,
        }

        public static readonly Dictionary<eDungenConstDef, string> Dungen_Const_Def_Key_List = new Dictionary<eDungenConstDef, string>()
        {
            { eDungenConstDef.DEF_ENERGY_PVE_TIME_PERIOD, "DEF_ENERGY_PVE_TIME_PERIOD" },
            { eDungenConstDef.DEF_ENERGY_PVP_TIME_PERIOD, "DEF_ENERGY_PVP_TIME_PERIOD" },
            { eDungenConstDef.DEF_ENERGY_G3VS3_TIME_PERIOD, "DEF_ENERGY_G3VS3_TIME_PERIOD" },
            { eDungenConstDef.DEF_PC_REVIVE_PRICE_RUBY, "DEF_PC_REVIVE_PRICE_RUBY" },
            { eDungenConstDef.DEF_RESET_COST_RUBY_SCENARIO, "DEF_RESET_COST_RUBY_SCENARIO" },
            { eDungenConstDef.DEF_RESET_COST_RUBY_BOSS, "DEF_RESET_COST_RUBY_BOSS" },
            { eDungenConstDef.DEF_RESET_COST_RUBY_DARK, "DEF_RESET_COST_RUBY_DARK" },
            { eDungenConstDef.DEF_RESET_COST_RUBY_ELITE, "DEF_RESET_COST_RUBY_ELITE" },
            { eDungenConstDef.DEF_BLACKMARKET_POWDOR_NUM, "DEF_BLACKMARKET_POWDOR_NUM" },
            { eDungenConstDef.DEF_SCENARIO_COMPLETE_GIFT_STEP1, "DEF_SCENARIO_COMPLETE_GIFT_STEP1" },
            { eDungenConstDef.DEF_SCENARIO_COMPLETE_GIFT_STEP2, "DEF_SCENARIO_COMPLETE_GIFT_STEP2" },
        };
    }
}
