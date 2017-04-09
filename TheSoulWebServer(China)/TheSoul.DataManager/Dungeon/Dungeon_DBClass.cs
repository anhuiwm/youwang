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

namespace TheSoul.DataManager.DBClass
{
    public class System_Elite_Dungeon
    {
        public int Index { get; set; }
        public int DungeonID { get; set; }
        public string Description { get; set; }
        public string Dungeon_Scene_name { get; set; }
        public string Prefab_name { get; set; }
        public int BossID { get; set; }
        public byte Type { get; set; }
        public int Condition_StageID { get; set; }
        public int Condition_Level { get; set; }
        public short Condition_PlayCoin { get; set; }
        public int Fighting_Power { get; set; }
        public int Clear_Time { get; set; }
        public int Get_Star_3 { get; set; }
        public int Get_Star_2 { get; set; }
        public short Try_Value { get; set; }
        public int Base_Reward_EXP { get; set; }
        public int Base_Reward_GOLD_MIN { get; set; }
        public int Base_Reward_GOLD_MAX { get; set; }
        public int Best_Reward_Item1_PC1 { get; set; }
        public short Item1_Grade_PC1 { get; set; }
        public int Best_Reward_Item1_PC2 { get; set; }
        public short Item1_Grade_PC2 { get; set; }
        public int Best_Reward_Item1_PC3 { get; set; }
        public short Item1_Grade_PC3 { get; set; }
        public int Best_Reward_Item2_PC1 { get; set; }
        public short Item2_Grade_PC1 { get; set; }
        public int Best_Reward_Item2_PC2 { get; set; }
        public short Item2_Grade_PC2 { get; set; }
        public int Best_Reward_Item2_PC3 { get; set; }
        public short Item2_Grade_PC3 { get; set; }
        public int Best_Reward_Item3_PC1 { get; set; }
        public short Item3_Grade_PC1 { get; set; }
        public int Best_Reward_Item3_PC2 { get; set; }
        public short Item3_Grade_PC2 { get; set; }
        public int Best_Reward_Item3_PC3 { get; set; }
        public short Item3_Grade_PC3 { get; set; }
        public int Rand_DropBoxGroupId { get; set; }
        public int Max_Gold { get; set; }
        public int Max_EXP { get; set; }
        public int Boss_HP { get; set; }
        public string Boss_Atlas { get; set; }
        public string Boss_Image { get; set; }
        public int Min_Gold { get; set; }
        public byte Booster_Group_ID { get; set; }
        public int Worldmap_X { get; set; }
        public int Worldmap_Y { get; set; }
    }


    public class System_Guerilla_Dungeon
    {
        public int DungeonID { get; set; }
        public string NamingCN { get; set; }
        public string DescCN1 { get; set; }
        public string DescCN2 { get; set; }
        public string Description { get; set; }
        public string Dungeon_Scene_name { get; set; }
        public int Condition_StageID { get; set; }
        public int Condition_Level { get; set; }
        public short Condition_PlayCoin { get; set; }
        public int Fighting_Power { get; set; }
        public int Clear_Time { get; set; }
        public int Get_Star_3 { get; set; }
        public int Get_Star_2 { get; set; }
        public short Try_Value { get; set; }
        public int Base_Reward_EXP { get; set; }
        public int Base_Reward_GOLD_MIN { get; set; }
        public int Base_Reward_GOLD_MAX { get; set; }
        public int Best_Reward_Item1_PC1 { get; set; }
        public short Item1_Grade_PC1 { get; set; }
        public int Best_Reward_Item1_PC2 { get; set; }
        public short Item1_Grade_PC2 { get; set; }
        public int Best_Reward_Item1_PC3 { get; set; }
        public short Item1_Grade_PC3 { get; set; }
        public int Best_Reward_Item2_PC1 { get; set; }
        public short Item2_Grade_PC1 { get; set; }
        public int Best_Reward_Item2_PC2 { get; set; }
        public short Item2_Grade_PC2 { get; set; }
        public int Best_Reward_Item2_PC3 { get; set; }
        public short Item2_Grade_PC3 { get; set; }
        public int Rand_DropBoxGroupId { get; set; }
        public int Max_Gold { get; set; }
        public int Max_EXP { get; set; }
        public int Boss_HP { get; set; }
        public string Leveldesign_Prefab_Name { get; set; }
        public short Soul_Evolution_Level { get; set; }
        public int Min_Gold { get; set; }
        public byte Booster_Group_ID { get; set; }
        public int Worldmap_X { get; set; }
        public int Worldmap_Y { get; set; }
    }

    //public class System_Guerrilla_Soul
    //{
    //    public long Guerrilla_Soul_ID { get; set; }
    //    public string Description { get; set; }
    //    public long PC_SoulID { get; set; }
    //    public long Buff_GroupID1 { get; set; }
    //    public long Buff_GroupID2 { get; set; }
    //    public long Buff_GroupID3 { get; set; }
    //}

    public class System_Guerrilla_Soul
    {
        public long Guerrilla_Soul_ID { get; set; }
        public string Description { get; set; }
        public long PC_SoulID { get; set; }
        public long Buff_GroupID1_PC1 { get; set; }
        public long Buff_GroupID2_PC1 { get; set; }
        public long Buff_GroupID3_PC1 { get; set; }
        public long Buff_GroupID1_PC2 { get; set; }
        public long Buff_GroupID2_PC2 { get; set; }
        public long Buff_GroupID3_PC2 { get; set; }
        public long Buff_GroupID1_PC3 { get; set; }
        public long Buff_GroupID2_PC3 { get; set; }
        public long Buff_GroupID3_PC3 { get; set; }
    }


    public class System_Mission_World
    {
        public int WorldID { get; set; }
        public string NamingCN { get; set; }
        public int Next_WorldID { get; set; }
        public int Normal_DungeonID1 { get; set; }
        public int Normal_DungeonID2 { get; set; }
        public int Normal_DungeonID3 { get; set; }
        public int Normal_DungeonID4 { get; set; }
        public int Subboss_DungeonID { get; set; }
        public int Elite_DungeonID1 { get; set; }
        public int Normal_DungeonID6 { get; set; }
        public int Normal_DungeonID7 { get; set; }
        public int Normal_DungeonID8 { get; set; }
        public int Normal_DungeonID9 { get; set; }
        public int Boss_DungeonID { get; set; }
        public int Dark_DungeonID { get; set; }
        public int Elite_DungeonID2 { get; set; }
        public string WorldBack_Atlas { get; set; }
        public string WorldBack_Image { get; set; }
        public int FirstClear_DropBoxGroupId { get; set; }
        public int SecondClear_DropBoxGroupId { get; set; }
    }

    public class System_Mission_Stage
    {
        public int StageID { get; set; }
        public string NamingCN { get; set; }
        public string DescCN1 { get; set; }
        public string DescCN2 { get; set; }
        public string Description { get; set; }
        public string Stage_Scene_name { get; set; }
        public string Prefab_name { get; set; }
        public int Next_StageID { get; set; }
        public int Condition_Level { get; set; }
        public int Condition_PlayCoin { get; set; }
        public int Fighting_Power { get; set; }
        public string StageType { get; set; }
        public string DungeonType { get; set; }
        public int Clear_Time { get; set; }
        public int Get_Star_3 { get; set; }
        public int Get_Star_2 { get; set; }
        public int Task1ID { get; set; }
        public int Task2ID { get; set; }
        public int Task3ID { get; set; }
        public int Try_Value { get; set; }
        public int Base_Reward_EXP { get; set; }
        public int Base_Reward_GOLD_MIN { get; set; }
        public int Base_Reward_GOLD_MAX { get; set; }
        public int Best_Reward_Item1_PC1 { get; set; }
        public int Item1_Grade_PC1 { get; set; }
        public int Best_Reward_Item1_PC2 { get; set; }
        public int Item1_Grade_PC2 { get; set; }
        public int Best_Reward_Item1_PC3 { get; set; }
        public int Item1_Grade_PC3 { get; set; }
        public int Best_Reward_Item2_PC1 { get; set; }
        public int Item2_Grade_PC1 { get; set; }
        public int Best_Reward_Item2_PC2 { get; set; }
        public int Item2_Grade_PC2 { get; set; }
        public int Best_Reward_Item2_PC3 { get; set; }
        public int Item2_Grade_PC3 { get; set; }
        public int Rand_DropBoxGroupId { get; set; }
        public int UserSelect_DropBoxGroupId { get; set; }
        public string Boss_Atlas { get; set; }
        public string Boss_Image { get; set; }
        public int Max_Gold { get; set; }
        public int Max_EXP { get; set; }
        public int Boss_HP { get; set; }
        public int Min_Gold { get; set; }
        public byte Booster_Group_ID { get; set; }
        public int Worldmap_X { get; set; }
        public int Worldmap_Y { get; set; }
    }

    public class System_Mission_Stage_Task
    {
        public int TaskID { get; set; }
        public string TaskCN { get; set; }
        public string Description { get; set; }
        public string TaskType { get; set; }
        public int value1 { get; set; }
        public int value2 { get; set; }
        public int value3 { get; set; }
        public string Rewardtype { get; set; }
        public int Rewardvalue { get; set; }
    }

    public class System_Party_Dungeon
    {
        public int PartyDungeonID { get; set; }
        public string NamingCN { get; set; }
        public string Desc { get; set; }
        public string Stage_Scene_name { get; set; }
        public int Condition_Level { get; set; }
        public short Condition_PlayCoin { get; set; }
        public int Fighting_Power { get; set; }
        public short Try_Value { get; set; }
        public int Base_Reward_EXP { get; set; }
        public int Base_Reward_GOLD_MIN { get; set; }
        public int Base_Reward_GOLD_MAX { get; set; }
        public int Base_Reward_GOLD_MIN1 { get; set; }
        public int Base_Reward_GOLD_MAX1 { get; set; }
        public int Base_Reward_GOLD_MIN2 { get; set; }
        public int Base_Reward_GOLD_MAX2 { get; set; }
        public int Base_Reward_GOLD_MIN3 { get; set; }
        public int Base_Reward_GOLD_MAX3 { get; set; }
        public int Best_Reward_Item1_PC1 { get; set; }
        public int Item1_Grade_PC1 { get; set; }
        public int Best_Reward_Item1_PC2 { get; set; }
        public int Item1_Grade_PC2 { get; set; }
        public int Best_Reward_Item1_PC3 { get; set; }
        public int Item1_Grade_PC3 { get; set; }
        public int Best_Reward_Item2_PC1 { get; set; }
        public int Item2_Grade_PC1 { get; set; }
        public int Best_Reward_Item2_PC2 { get; set; }
        public int Item2_Grade_PC2 { get; set; }
        public int Best_Reward_Item2_PC3 { get; set; }
        public int Item2_Grade_PC3 { get; set; }
        public int Best_Reward_Item3_PC1 { get; set; }
        public int Item3_Grade_PC1 { get; set; }
        public int Best_Reward_Item3_PC2 { get; set; }
        public int Item3_Grade_PC2 { get; set; }
        public int Best_Reward_Item3_PC3 { get; set; }
        public int Item3_Grade_PC3 { get; set; }
        public int Rand_DropBoxGroupId { get; set; }
        public int Rand_DropBoxGroupId1 { get; set; }
        public int Rand_DropBoxGroupId2 { get; set; }
        public int Rand_DropBoxGroupId3 { get; set; }
        public string BackImage_Atlas { get; set; }
        public string BackImage_Image { get; set; }
        public int Max_Gold { get; set; }
        public int Max_Gold1 { get; set; }
        public int Max_Gold2 { get; set; }
        public int Max_Gold3 { get; set; }
        public int Max_EXP { get; set; }
        public int Boss_HP { get; set; }
    }

    public class User_Mission_Play
    {
        public long idx { get; set; }
        public long aid { get; set; }
        public int worldid { get; set; }
        public int stageid { get; set; }
        public byte rank { get; set; }
        public string task1 { get; set; }
        public string task2 { get; set; }
        public string task3 { get; set; }
        public int task1value { get; set; }
        public int task2value { get; set; }
        public int task3value { get; set; }
        public string reward1 { get; set; }
        public string reward2 { get; set; }
        public string reward3 { get; set; }
        public int ClearTime { get; set; }
        public byte ChallengeCnt { get; set; }
        public byte ChallengeReset { get; set; }
        public DateTime regdate { get; set; }
    }

    public class User_GuerrillaDungeon_Play
    {
        public int worldid { get; set; }
        public long idx { get; set; }
        public long aid { get; set; }
        public byte dungeonid { get; set; }
        public byte rank { get; set; }
        public int cleartime { get; set; }
        public byte challengecount { get; set; }
        public byte challengereset { get; set; }
        public int maxtrycount { get; set; }
        public DateTime regdate { get; set; }
    }

    public class RetUserEliteDungeon_Play
    {
        public List<User_EliteDungeon_Play> retList { get; set; }
        public long laststage { get; set; }
        public long selectedidx { get; set; }
    }

    public class User_EliteDungeon_Play
    {
        public int idx { get; set; }
        public long aid { get; set; }
        public int dungeonid { get; set; }
        public int rank { get; set; }
        public int clearcount { get; set; }
        public int maxcount { get; set; }
        public int resetcount { get; set; }
        public DateTime regdate { get; set; }
        public DateTime lastdate { get; set; }
    }

    public class System_Booster_Group
    {
        public int Booster_Group_ID { get; set; }
        public string Desc { get; set; }
        public string ContentType { get; set; }
        public string Boost1_TaskCN { get; set; }
        public string Boost1_Type { get; set; }
        public int Boost1_Value { get; set; }
        public string Boost2_TaskCN { get; set; }
        public string Boost2_Type { get; set; }
        public int Boost2_Value { get; set; }
        public string Boost3_TaskCN { get; set; }
        public string Boost3_Type { get; set; }
        public int Boost3_Value { get; set; }
        public long Boost1_ItemID { get; set; }
        public string Boost1_PriceType { get; set; }
        public int Boost1_PriceValue { get; set; }
        public long Boost2_ItemID { get; set; }
        public string Boost2_PriceType { get; set; }
        public int Boost2_PriceValue { get; set; }
        public long Boost3_ItemID { get; set; }
        public string Boost3_PriceType { get; set; }
        public int Boost3_PriceValue { get; set; }
    }


    public class User_Mission_Last_Stage
    {
        public int stageid { get; set; }
    }
    public class RetTaskResult
    {
        public string taskClear { get; set; }
        public int taskValue { get; set; }
        public int taskGold { get; set; }
        public int taskExp { get; set; }
        public int taskRuby { get; set; }

        public RetTaskResult()
        {
            taskClear = "N";
            taskValue = 0;
            taskGold = 0;
            taskExp = 0;
            taskRuby = 0;
        }

        public RetTaskResult(string setClear, int setValue)
        {
            taskClear = setClear;
            taskValue = setValue;
            taskGold = 0;
            taskExp = 0;
            taskRuby = 0;
        }
    }

    public class RetBeforeInfo
    {
        public int levelup { get; set; }
        public long beforelevel { get; set; }
        public long beforeexp { get; set; }
        public long beforegold { get; set; }
        public long beforeruby { get; set; }
        public long beforekey { get; set; }
        public long beforekeymax { get; set; }
        public long beforeticket { get; set; }
        public long beforeticketmax { get; set; }
        public long beforechallange { get; set; }
        public long keyremainsec { get; set; }
        public long ticketremainsec { get; set; }
        public long challengeremainsec { get; set; }

        public RetBeforeInfo() { }

        public RetBeforeInfo(long beforLevel, long beforeExp, long beforeGold, long beforeRuby, long beforeKey, long beforeKeyMax, long beforeTicket, long beforeTicketMax, long beforeChallange, long keyRemainSec = 0, long ticketRemainSec = 0, long challengeRemainSec = 0)
        {
            levelup = 0;
            beforelevel = beforLevel;
            beforeexp = beforeExp;
            beforegold = beforeGold;
            beforeruby = beforeRuby;
            beforekey = beforeKey;
            beforekeymax = beforeKeyMax;
            beforeticket = beforeTicket;
            beforeticketmax = beforeTicketMax;
            beforechallange = beforeChallange;
            keyremainsec = keyRemainSec;
            ticketremainsec = ticketRemainSec;
            challengeremainsec = challengeRemainSec;
        }

        public RetBeforeInfo(long beforLevel, long beforeExp)
        {
            beforelevel = beforLevel;
            beforeexp = beforeExp;
        }
    }

    public class RetMissionTaskInfo
    {
        public byte rank { get; set; }
        public int resetcount { get; set; }
        public string task1 { get; set; }
        public string task2 { get; set; }
        public string task3 { get; set; }
        public int task1value { get; set; }
        public int task2value { get; set; }
        public int task3value { get; set; }
        public RetMissionTaskInfo(User_Mission_Play setInfo)
        {
            rank = setInfo.rank;
            resetcount = setInfo.ChallengeReset;
            task1 = setInfo.task1;
            task2 = setInfo.task2;
            task3 = setInfo.task3;
            task1value = setInfo.task1value;
            task2value = setInfo.task2value;
            task3value = setInfo.task3value;
        }

        public RetMissionTaskInfo(byte setRank, User_Mission_Play setInfo)
        {
            rank = setRank;
            resetcount = setInfo.ChallengeReset;
            task1 = setInfo.task1;
            task2 = setInfo.task2;
            task3 = setInfo.task3;
            task1value = setInfo.task1value;
            task2value = setInfo.task2value;
            task3value = setInfo.task3value;
        }

        public RetMissionTaskInfo(byte r, int setreset, string t1, string t2, string t3, int v1, int v2, int v3)
        {
            rank = r;
            resetcount = setreset;
            task1 = t1;
            task2 = t2;
            task3 = t3;
            task1value = v1;
            task2value = v2;
            task3value = v3;
        }

        public RetMissionTaskInfo()
        {
            rank = 0;
            task1 = "N";
            task2 = "N";
            task3 = "N";
            task1value = 0;
            task2value = 0;
            task3value = 0;            
        }
    }

    public class RetDarkPassgeInfo
    {
        public long soulid { get; set; }
        public long grade { get; set; }
        public long specialbuff1 { get; set; }
        public long specialbuff2 { get; set; }
        public long specialbuff3 { get; set; }
    }

    public class RetMissionRank
    {
        public int worldid { get; set; }
        public int stageid { get; set; }
        public int rank { get; set; }
    }

    public class RetWorldRank
    {
        public int worldid { get; set; }
        public int rank { get; set; }
        public int reward1 { get; set; }
        public int reward2 { get; set; }

        public RetWorldRank(int setworldid = 0, int setrank = 0, int setreward1 = 0, int setreward2 = 0)
        {
            worldid = setworldid;
            rank = setrank;
            reward1 = setreward1;
            reward2 = setreward2;
        }
    }

    public class RetGuerrillaDungeonRank
    {
        public int dungeonid { get; set; }
        public int rank { get; set; }
        public int challengecount { get; set; }
        public int resetcount { get; set; }
        public int maxtrycount { get; set; }
    }

    public class RetEliteDungeonRank
    {
        public int worldid { get; set; }
        public int dungeonid { get; set; }
        public int rank { get; set; }
    }

    public class RetEliteDungeonList
    {
        public int dungeonid { get; set; }
        public int clearrank { get; set; }
        public int currentclear { get; set; }
        public int maxclear { get; set; }
        public int resetcount { get; set; }

        public RetEliteDungeonList(int id, int rank, int clear, int max, int reset)
        {
            dungeonid = id;
            clearrank = rank;
            currentclear = clear;
            maxclear = max;
            resetcount = reset;
        }

        public RetEliteDungeonList(User_EliteDungeon_Play info)
        {
            dungeonid = info.dungeonid;
            clearrank = info.rank;

            DateTime curDate = DateTime.Parse(info.regdate.ToShortDateString());
            DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
            TimeSpan TS = dbDate - curDate;

            if (TS.Days != 0)
                currentclear = 0;
            else
                currentclear = info.clearcount;
            maxclear = info.maxcount;
            resetcount = info.resetcount;
        }
    }

    public class UserMission_NPC_KILL
    {
        public int npc_id { get; set; }
        public int grade { get; set; }
        public int count { get; set; }
    }
}