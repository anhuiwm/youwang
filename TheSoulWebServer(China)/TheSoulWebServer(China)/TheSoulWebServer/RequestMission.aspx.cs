using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using mSeed.mDBTxnBlock;
using mSeed.RedisManager;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using ServiceStack.Text;

namespace TheSoulWebServer
{
    public partial class RequestMission : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                // normal pve
                "mission_clear_info",
                "mission_modeinfo",
                "mission_taskinfo",
                "mission_start",
                "mission_result_sweep",
                "mission_result",
                "mission_rank_reward",
                "pve_revival",
                "pve_count_reset",

                // dark passage
                "dark_passage_start",
                "dark_passage_result",
                "dark_passage_result_sweep",

                // elite pve
                "elite_modeinfo",
                "elite_start",
                "elite_result",
                "elite_result_sweep",

                
                // for test method only work in debug mode
            };


            WebQueryParam queryFetcher = new WebQueryParam();
            string retJson = "";

            TxnBlock tb = new TxnBlock();
            {
                long AID = 0;
                try
                {
                    queryFetcher.TxnBlockInit(ref tb, ref AID);

                    string requestOp = queryFetcher.QueryParam_Fetch("op");
                    JsonObject json = new JsonObject();

                    if (queryFetcher.ReRequestFlag)
                    {
                        retJson = queryFetcher.ReRequestRender();
                    }
                    else if (Array.IndexOf(ops, requestOp) >= 0)
                    {
                        queryFetcher.operation = requestOp;
                        Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                        long CID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("cid"));
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.op], requestOp);
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], AID);

                        if (requestOp.Equals("mission_result") ||requestOp.Equals("mission_result_sweep") ||
                            requestOp.Equals("dark_passage_result") || requestOp.Equals("dark_passage_result_sweep") ||
                            requestOp.Equals("elite_result") || requestOp.Equals("elite_result_sweep")
                            )
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            int WorldID = queryFetcher.QueryParam_FetchInt("worldid");
                            int StageID = queryFetcher.QueryParam_FetchInt("stageid");
                            int DungeonID = queryFetcher.QueryParam_FetchInt("dungeonid");
                            List<UserMission_NPC_KILL> KillCount = mJsonSerializer.JsonToObject<List<UserMission_NPC_KILL>>(queryFetcher.QueryParam_Fetch("killcount", "[]"));
                            int GameExp = queryFetcher.QueryParam_FetchInt("gameexp");
                            int GameMoney = queryFetcher.QueryParam_FetchInt("gamemoney");
                            int MaxCombo = queryFetcher.QueryParam_FetchInt("maxcombo");
                            int AccrueCombo = queryFetcher.QueryParam_FetchInt("accruecombo");
                            int Booster1 = queryFetcher.QueryParam_FetchInt("booster1");
                            int Booster2 = queryFetcher.QueryParam_FetchInt("booster2");
                            int Booster3 = queryFetcher.QueryParam_FetchInt("booster3");
                            int task1value = queryFetcher.QueryParam_FetchInt("task1value");
                            int task2value = queryFetcher.QueryParam_FetchInt("task2value");
                            int task3value = queryFetcher.QueryParam_FetchInt("task3value");
                            int ClearTime = queryFetcher.QueryParam_FetchInt("cleartime");
                            int Clear = queryFetcher.QueryParam_FetchInt("clear");
                            int ObjectCount = queryFetcher.QueryParam_FetchInt("objectcount");
                            int SweepCount = queryFetcher.QueryParam_FetchInt("sweep_count", 1);

                            bool isClear = (Clear > 0) ? true : (requestOp.Equals("dark_passage_result_sweep") || requestOp.Equals("mission_result_sweep") || requestOp.Equals("elite_result_sweep")) ? true : false;
                            int AddGold = 0;
                            int AddExp = 0;
                            int TaskExp = 0;
                            int TaskGold = 0;
                            int TaskRuby = 0;
                            float expRate = 1.0f;
                            float goldRate = 1.0f;
                            int PlayKey = 0;
                            Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                            Character charInfo = CharacterManager.GetCharacter(ref tb, AID, CID);
                            retError = AccountManager.CalcBuffEndTime(ref tb, ref userAccount);
                            Ret_GM_Event GmEvent = new Ret_GM_Event();

                            if (KillCount == null)
                                KillCount = new List<UserMission_NPC_KILL>();

                            if (userAccount.PVEPlayState == 0 &&
                                    (requestOp.Equals("mission_result") ||
                                    requestOp.Equals("dark_passage_result") ||
                                    requestOp.Equals("elite_result")
                                    ) && !queryFetcher.SetDebugMode
                                )
                                retError = Result_Define.eResult.DUPLICATE_PLAYING;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.BAGSLOT_MAX_ITEM))
                                    retError = Result_Define.eResult.SUCCESS;
                                else
                                    retError = Result_Define.eResult.ITEM_INVENTORY_OVER;
                            }

                            if (SweepCount > 1)
                            {
                                if (requestOp.Equals("mission_result_sweep"))
                                {
                                    if (!VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.UNLOCK_5SWEEP))
                                        retError = Result_Define.eResult.VIP_CONTENTS_LOCKED;
                                }
                                else if (requestOp.Equals("dark_passage_result_sweep"))
                                {
                                    if (!VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.UNLOCK_5SWEEP_DARK))
                                        retError = Result_Define.eResult.VIP_CONTENTS_LOCKED;
                                }
                                else if (requestOp.Equals("elite_result_sweep"))
                                {
                                    if (!VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.UNLOCK_5SWEEP_ELITE))
                                        retError = Result_Define.eResult.VIP_CONTENTS_LOCKED;
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS && isClear)
                                retError = AccountManager.CalcGMEventBuffTime(ref tb, ref GmEvent);

                            System_Mission_Stage stageInfo = new System_Mission_Stage();
                            User_Mission_Play userMissionInfo = new User_Mission_Play();
                            System_Guerilla_Dungeon darkPassageInfo = new System_Guerilla_Dungeon();
                            User_GuerrillaDungeon_Play userDarkPassgeInfo = new User_GuerrillaDungeon_Play();
                            System_Elite_Dungeon eliteInfo = new System_Elite_Dungeon();
                            User_EliteDungeon_Play userElitePlayInfo = new User_EliteDungeon_Play();
                            int SetRank = 0;

                            List<User_Event_Data> userEventList = new List<User_Event_Data>();
                            List<User_Event_Data> userAchieveList = new List<User_Event_Data>();
                            List<User_Event_Data> userAchievePvPList = new List<User_Event_Data>();

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                userEventList = TriggerManager.Check_Event_Data_List(ref tb, AID);
                                userAchieveList = TriggerManager.Check_Achieve_Data_List(ref tb, AID);
                                userAchievePvPList = TriggerManager.Check_Achieve_PvP_Data_List(ref tb, AID);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_instance_log]);

                                if (requestOp.Equals("mission_result") || requestOp.Equals("mission_result_sweep"))
                                {
                                    stageInfo = Dungeon_Manager.GetSystem_MissionStageInfo(ref tb, StageID);
                                    userMissionInfo = Dungeon_Manager.GetUser_MissionInfo(ref tb, ref retError, AID, WorldID, StageID);
                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        PlayKey = stageInfo.Condition_PlayCoin * SweepCount;
                                        AddExp = (stageInfo.Base_Reward_EXP + GameExp) * SweepCount;
                                        int npcGold_Min = requestOp.Equals("mission_result") ? (GameMoney < stageInfo.Min_Gold ? stageInfo.Min_Gold : GameMoney) : stageInfo.Min_Gold;
                                        int npcGold_Max = requestOp.Equals("mission_result") ? (GameMoney > stageInfo.Max_Gold ? stageInfo.Max_Gold : GameMoney) : stageInfo.Max_Gold;
                                        SweepCount = requestOp.Equals("mission_result") ? 1 : SweepCount;
                                        npcGold_Max = npcGold_Max < npcGold_Min ? npcGold_Min : npcGold_Max;

                                        for (int i = 0; i < SweepCount; i++)
                                        {
                                            int baseGold = TheSoul.DataManager.Math.GetRandomInt(stageInfo.Base_Reward_GOLD_MIN, stageInfo.Base_Reward_GOLD_MAX);
                                            AddGold += baseGold + TheSoul.DataManager.Math.GetRandomInt(npcGold_Min, npcGold_Max);
                                        }
                                        SetRank = userMissionInfo.rank;
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id], ((int)StageID + SnailLog_Define.Snail_s_id_Seperator_pve_stage).ToString());
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], ((int)StageID + SnailLog_Define.Snail_s_id_Seperator_pve_stage).ToString());
                                    }
                                }
                                else if (requestOp.Equals("dark_passage_result") || requestOp.Equals("dark_passage_result_sweep"))
                                {
                                    //WorldID = System.Convert.ToInt32(userAccount.LastWorldID);
                                    StageID = System.Convert.ToInt32(userAccount.LastStageID);
                                    darkPassageInfo = Dungeon_Manager.GetSystem_DarkPassageInfo(ref tb, DungeonID);
                                    userDarkPassgeInfo = Dungeon_Manager.GetUser_DarkPassagePlayInfo(ref tb, ref retError, AID, DungeonID);

                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        DateTime curDate = DateTime.Parse(userDarkPassgeInfo.regdate.ToShortDateString());
                                        DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                                        TimeSpan TS = dbDate - curDate;

                                        if (TS.Days != 0)
                                        {
                                            userDarkPassgeInfo.challengecount = 0;
                                            userDarkPassgeInfo.challengereset = 0;
                                            userDarkPassgeInfo.regdate = curDate;
                                        }

                                        if (!VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.DUNGEONCOUNT_MAX_DARK, userDarkPassgeInfo.challengecount))
                                            retError = Result_Define.eResult.MISSION_TRY_COUNT_MAX;

                                        PlayKey = darkPassageInfo.Condition_PlayCoin * SweepCount;
                                        AddExp = (darkPassageInfo.Base_Reward_EXP + GameExp) * SweepCount;

                                        int npcGold_Min = requestOp.Equals("dark_passage_result_sweep") ? (GameMoney < darkPassageInfo.Min_Gold ? darkPassageInfo.Min_Gold : GameMoney) : darkPassageInfo.Min_Gold;
                                        int npcGold_Max = requestOp.Equals("dark_passage_result_sweep") ? (GameMoney > darkPassageInfo.Max_Gold ? darkPassageInfo.Max_Gold : GameMoney) : darkPassageInfo.Max_Gold;
                                        SweepCount = requestOp.Equals("dark_passage_result") ? 1 : SweepCount;
                                        npcGold_Max = npcGold_Max < npcGold_Min ? npcGold_Min : npcGold_Max;

                                        for (int i = 0; i < SweepCount; i++)
                                        {
                                            int baseGold = TheSoul.DataManager.Math.GetRandomInt(darkPassageInfo.Base_Reward_GOLD_MIN, darkPassageInfo.Base_Reward_GOLD_MAX);
                                            AddGold += baseGold + TheSoul.DataManager.Math.GetRandomInt(npcGold_Min, npcGold_Max);
                                        }
                                        SetRank = userDarkPassgeInfo.rank;

                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id], ((int)DungeonID + SnailLog_Define.Snail_s_id_Seperator_pve_dark).ToString());
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], ((int)DungeonID + SnailLog_Define.Snail_s_id_Seperator_pve_dark).ToString());
                                    }
                                }
                                else if (requestOp.Equals("elite_result") || requestOp.Equals("elite_result_sweep"))
                                {
                                    //WorldID = System.Convert.ToInt32(userAccount.LastWorldID);
                                    StageID = System.Convert.ToInt32(userAccount.LastStageID);
                                    eliteInfo = Dungeon_Manager.GetSystem_EliteDungeonInfo(ref tb, DungeonID);
                                    userElitePlayInfo = Dungeon_Manager.GetUser_EliteDungeonPlayInfo(ref tb, ref retError, AID, DungeonID).retList.Where(item => item.dungeonid == DungeonID).FirstOrDefault();

                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        if (userElitePlayInfo == null) userElitePlayInfo = new User_EliteDungeon_Play();
                                        DateTime curDate = DateTime.Parse(userElitePlayInfo.regdate.ToShortDateString());
                                        DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                                        TimeSpan TS = dbDate - curDate;

                                        if (TS.Days != 0)
                                        {
                                            userElitePlayInfo.clearcount = 0;
                                            userElitePlayInfo.resetcount = 0;
                                            userElitePlayInfo.regdate = curDate;
                                        }

                                        if (!VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.DUNGEONCOUNT_MAX_ELITE, userElitePlayInfo.clearcount))
                                            retError = Result_Define.eResult.MISSION_TRY_COUNT_MAX;

                                        PlayKey = eliteInfo.Condition_PlayCoin * SweepCount;
                                        AddExp = (eliteInfo.Base_Reward_EXP + GameExp) * SweepCount;

                                        int npcGold_Min = requestOp.Equals("elite_result_sweep") ? (GameMoney < eliteInfo.Min_Gold ? eliteInfo.Min_Gold : GameMoney) : eliteInfo.Min_Gold;
                                        int npcGold_Max = requestOp.Equals("elite_result_sweep") ? (GameMoney > eliteInfo.Max_Gold ? eliteInfo.Max_Gold : GameMoney) : eliteInfo.Max_Gold;
                                        SweepCount = requestOp.Equals("elite_result") ? 1 : SweepCount;
                                        npcGold_Max = npcGold_Max < npcGold_Min ? npcGold_Min : npcGold_Max;

                                        for (int i = 0; i < SweepCount; i++)
                                        {
                                            int baseGold = TheSoul.DataManager.Math.GetRandomInt(eliteInfo.Base_Reward_GOLD_MIN, eliteInfo.Base_Reward_GOLD_MAX);
                                            AddGold += baseGold + TheSoul.DataManager.Math.GetRandomInt(npcGold_Min, npcGold_Max);
                                        }
                                        SetRank = userElitePlayInfo.rank;

                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id], ((int)DungeonID + SnailLog_Define.Snail_s_id_Seperator_pve_elite).ToString());
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], ((int)DungeonID + SnailLog_Define.Snail_s_id_Seperator_pve_elite).ToString());
                                    }
                                }

                                goldRate += GmEvent.GoldBoostRate;

                                expRate += GmEvent.PCEXPBoostRate;
                                expRate += (userAccount.PCEXPBuffEndTime > 0 ? Account_Define.PCEXPBuff_Rate_Type1 : 0);
                                expRate += (userAccount.PCEXPBuffEndTime2 > 0 ? Account_Define.PCEXPBuff_Rate_Type2 : 0);
                                expRate += (Booster3 > 0 ? Account_Define.MissionEXPBoostBuff_Rate_Type2 : 0);
                            }

                            if (retError == Result_Define.eResult.SUCCESS && SetRank < Dungeon_Define.Rank3Star
                                && (requestOp.Equals("mission_result_sweep") || requestOp.Equals("dark_passage_result_sweep") || requestOp.Equals("elite_result_sweep")) )
                            {
                                if (!VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.UNLOCK_1STARSWEEP))
                                    retError = Result_Define.eResult.VIP_CONTENTS_LOCKED;
                            }

                            int SetClearTime = 0;
                            // update mission task
                            if (retError == Result_Define.eResult.SUCCESS && isClear)
                            {
                                if (requestOp.Equals("mission_result"))
                                {
                                    userMissionInfo.ClearTime = (userMissionInfo.ClearTime < stageInfo.Clear_Time) ? stageInfo.Clear_Time : userMissionInfo.ClearTime;
                                    SetRank = (ClearTime <= stageInfo.Get_Star_3) ? Dungeon_Define.Rank3Star :
                                                    (ClearTime <= stageInfo.Get_Star_2) ? Dungeon_Define.Rank2Star :
                                                    (ClearTime <= stageInfo.Clear_Time) ? Dungeon_Define.Rank1Star : 0;
                                    SetClearTime = (ClearTime > userMissionInfo.ClearTime) ? userMissionInfo.ClearTime : ClearTime;

                                    RetTaskResult UserTask1 = new RetTaskResult();
                                    RetTaskResult UserTask2 = new RetTaskResult();
                                    RetTaskResult UserTask3 = new RetTaskResult();

                                    if (stageInfo.Task1ID > 0)
                                        UserTask1 = Dungeon_Manager.CheckTaskStatus(ref tb, stageInfo.Task1ID, userMissionInfo.task1value, task1value, userMissionInfo.task1);
                                    if (stageInfo.Task2ID > 0)
                                        UserTask2 = Dungeon_Manager.CheckTaskStatus(ref tb, stageInfo.Task2ID, userMissionInfo.task2value, task2value, userMissionInfo.task2);
                                    if (stageInfo.Task3ID > 0)
                                        UserTask3 = Dungeon_Manager.CheckTaskStatus(ref tb, stageInfo.Task3ID, userMissionInfo.task3value, task3value, userMissionInfo.task3);

                                    userMissionInfo.rank = userMissionInfo.rank < SetRank ? System.Convert.ToByte(SetRank) : userMissionInfo.rank;
                                    userMissionInfo.ClearTime = SetClearTime;
                                    userMissionInfo.task1 = UserTask1.taskClear;
                                    userMissionInfo.task1value = UserTask1.taskValue;
                                    userMissionInfo.task2 = UserTask2.taskClear;
                                    userMissionInfo.task2value = UserTask2.taskValue;
                                    userMissionInfo.task3 = UserTask3.taskClear;
                                    userMissionInfo.task3value = UserTask3.taskValue;

                                    TaskExp = UserTask1.taskExp + UserTask2.taskExp + UserTask3.taskExp;
                                    TaskGold = UserTask1.taskGold + UserTask2.taskGold + UserTask3.taskGold;
                                    TaskRuby = UserTask1.taskRuby + UserTask2.taskRuby + UserTask3.taskRuby;
                                }
                                else if (requestOp.Equals("dark_passage_result"))
                                {
                                    userDarkPassgeInfo.cleartime = (userDarkPassgeInfo.cleartime < 1) ? darkPassageInfo.Clear_Time : userDarkPassgeInfo.cleartime;
                                    SetRank = (ClearTime <= darkPassageInfo.Get_Star_3) ? Dungeon_Define.Rank3Star :
                                                    (ClearTime <= darkPassageInfo.Get_Star_2) ? Dungeon_Define.Rank2Star :
                                                    (ClearTime <= darkPassageInfo.Clear_Time) ? Dungeon_Define.Rank1Star : 0;

                                    SetClearTime = (ClearTime > userDarkPassgeInfo.cleartime) ? userDarkPassgeInfo.cleartime : ClearTime;
                                    userDarkPassgeInfo.cleartime = SetClearTime;
                                    userDarkPassgeInfo.rank = userDarkPassgeInfo.rank < SetRank ? System.Convert.ToByte(SetRank) : userDarkPassgeInfo.rank;
                                }
                                else if (requestOp.Equals("elite_result"))
                                {
                                    SetRank = (ClearTime <= eliteInfo.Get_Star_3) ? Dungeon_Define.Rank3Star :
                                                    (ClearTime <= eliteInfo.Get_Star_2) ? Dungeon_Define.Rank2Star :
                                                    (ClearTime <= eliteInfo.Clear_Time) ? Dungeon_Define.Rank1Star : 0;

                                    SetClearTime = (ClearTime > eliteInfo.Clear_Time) ? eliteInfo.Clear_Time : ClearTime;
                                    userElitePlayInfo.rank = userElitePlayInfo.rank < SetRank ? System.Convert.ToByte(SetRank) : userElitePlayInfo.rank;
                                }

                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_duration], ClearTime);
                            }

                            List<TriggerProgressData> setTriggerList = new List<TriggerProgressData>();

                            // add Trigger
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                foreach (UserMission_NPC_KILL setKill in KillCount)
                                {
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Kill_NPC, setKill.grade, 0, setKill.count));
                                    //setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Kill_NPC_First, StageID, setKill.npc_id, setKill.count));          // tutorial
                                    //setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Kill_NPC_Appoint, StageID, setKill.npc_id, setKill.count));        // task
                                    //setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Kill_NPC_Appear, StageID, setKill.npc_id, setKill.count));         // task
                                }

                                if (MaxCombo > 0)
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Combo, 0, (int)Trigger_Define.eComboType.Max, MaxCombo));
                                if (AccrueCombo > 0)
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Combo, 0, (int)Trigger_Define.eComboType.Accumulate, AccrueCombo));
                                if (ObjectCount > 0)
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Destroy_Object, 0, 0, ObjectCount));

                                if (requestOp.Equals("mission_result") || requestOp.Equals("mission_result_sweep"))
                                {
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_PVE, (int)Trigger_Define.ePvEType.Scenario, 0, SweepCount));
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_Scenario, WorldID, StageID, SweepCount));
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_Scenario_First, WorldID, StageID, SweepCount));
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Clear_PVE, (int)Trigger_Define.ePvEType.Scenario, isClear ? 1 : 2, SweepCount));
                                    if (isClear)
                                        setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Clear_Scenario, WorldID, StageID, SweepCount));
                                    //setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Clear_Scenario_First, StageID, isClear ? 1 : 2));
                                    if (requestOp.Equals("mission_result_sweep"))
                                        setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Autoclear_Use, 0, 0, SweepCount));
                                }
                                else if (requestOp.Equals("dark_passage_result") || requestOp.Equals("dark_passage_result_sweep"))
                                {
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_PVE, (int)Trigger_Define.ePvEType.Guerilla, 0, SweepCount));
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_Guerilla, WorldID, DungeonID, SweepCount));
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_Guerilla_First, WorldID, DungeonID, SweepCount));
                                    if (requestOp.Equals("dark_passage_result_sweep"))
                                        setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Autoclear_Use, 0, 0, SweepCount));
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Clear_PVE, (int)Trigger_Define.ePvEType.Guerilla, isClear ? 1 : 2, SweepCount));
                                    if (isClear)
                                        setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Clear_Guerilla, WorldID, DungeonID, SweepCount));
                                    //setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Clear_Guerilla_First, DungeonID, isClear ? 1 : 2));
                                }
                                else if (requestOp.Equals("elite_result") || requestOp.Equals("elite_result_sweep"))
                                {
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_PVE, (int)Trigger_Define.ePvEType.Elite, 0, SweepCount));
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_Elite, WorldID, DungeonID, SweepCount));
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_Elite_First, WorldID, DungeonID, SweepCount));
                                    if (requestOp.Equals("elite_result_sweep"))
                                        setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Autoclear_Use, 0, 0, SweepCount));
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Clear_PVE, (int)Trigger_Define.ePvEType.Elite, isClear ? 1 : 2, SweepCount));
                                    if (isClear)
                                    {
                                        setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Clear_Elite, WorldID, DungeonID, SweepCount));
                                        if (SetRank >= Dungeon_Define.Rank3Star)
                                            setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Clear_Elite_Perfect, WorldID, DungeonID, SweepCount));
                                    }
                                    //setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Clear_Elite_First, DungeonID, isClear ? 1 : 2));
                                }
                            }


                            if (retError == Result_Define.eResult.SUCCESS && isClear)
                            {
                                if (requestOp.Equals("mission_result") || requestOp.Equals("mission_result_sweep"))
                                    retError = Dungeon_Manager.UpdateMissionTask(ref tb, ref userMissionInfo, SweepCount);
                                else if (requestOp.Equals("dark_passage_result") || requestOp.Equals("dark_passage_result_sweep"))
                                    retError = Dungeon_Manager.UpdateDarkPassge(ref tb, ref userDarkPassgeInfo, SweepCount);
                                else if (requestOp.Equals("elite_result") || requestOp.Equals("elite_result_sweep"))
                                    retError = Dungeon_Manager.UpdateEliteDungeon(ref tb, ref userElitePlayInfo, SweepCount);
                            }

                            List<User_Inven> makeRealItem = new List<User_Inven>();
                            List<User_Inven> makeDummyItem = new List<User_Inven>();
                            User_Inven showFirstItem = null;
                            // check drop item
                            if (retError == Result_Define.eResult.SUCCESS && isClear)
                            {
                                List<System_Drop_Group> getDropList = new List<System_Drop_Group>();
                                List<System_Drop_Group> getDummyDropList = new List<System_Drop_Group>();
                                if (requestOp.Equals("mission_result") || requestOp.Equals("mission_result_sweep"))
                                {
                                    for (int i = 0; i < SweepCount; i++)
                                    {
                                        List<System_Drop_Group> setDropList = DropManager.GetDropResult(ref tb, AID, stageInfo.UserSelect_DropBoxGroupId, (short)charInfo.Class);
                                        getDropList.AddRange(setDropList);
                                        setDropList = DropManager.GetDropResult(ref tb, AID, stageInfo.Rand_DropBoxGroupId, (short)charInfo.Class);
                                        getDropList.AddRange(setDropList);
                                    }
                                }
                                else if (requestOp.Equals("dark_passage_result") || requestOp.Equals("dark_passage_result_sweep"))
                                {
                                    for (int i = 0; i < SweepCount; i++)
                                    {
                                        List<System_Drop_Group> setDropList = DropManager.GetDropResult(ref tb, AID, darkPassageInfo.Rand_DropBoxGroupId, (short)charInfo.Class);
                                        getDropList.AddRange(setDropList);
                                    }
                                }
                                else if (requestOp.Equals("elite_result") || requestOp.Equals("elite_result_sweep"))
                                {
                                    for (int i = 0; i < SweepCount; i++)
                                    {
                                        List<System_Drop_Group> setDropList = DropManager.GetDropResult(ref tb, AID, eliteInfo.Rand_DropBoxGroupId, (short)charInfo.Class);
                                        getDropList.AddRange(setDropList);
                                    }
                                }

                                foreach (System_Drop_Group setDrop in getDropList)
                                {
                                    List<User_Inven> getItem = new List<User_Inven>();
                                    retError = DropManager.MakeDropItem(ref tb, ref getItem, setDrop, AID, CID);

                                    if (retError != Result_Define.eResult.SUCCESS)
                                        break;

                                    makeRealItem.AddRange(getItem);

                                    if (showFirstItem == null)
                                        showFirstItem = makeRealItem.FirstOrDefault();
                                }

                                if (requestOp.Equals("mission_result"))
                                {
                                    List<System_Drop_Group> setDropList = DropManager.GetDropResult(ref tb, 0, stageInfo.UserSelect_DropBoxGroupId, (short)charInfo.Class, Dungeon_Define.PVEDummyMakeItemCount, 0, showFirstItem);
                                    getDummyDropList.AddRange(setDropList);
                                }

                                foreach (System_Drop_Group setDrop in getDummyDropList)
                                {
                                    User_Inven setDummy = new User_Inven();
                                    setDummy.itemid = setDrop.DropItemID;
                                    setDummy.grade = System.Convert.ToInt16(setDrop.DropItemGrade);
                                    setDummy.level = System.Convert.ToInt16(setDrop.DropItemLevel);

                                    makeDummyItem.Add(setDummy);
                                }

                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = showFirstItem == null ? Result_Define.eResult.ITEM_CREATE_FAIL : Result_Define.eResult.SUCCESS;

                                //if (showFirstItem == null && realCount > 0)
                                //    retError = Result_Define.eResult.ITEM_CREATE_FAIL;
                            }

                            //long keyRemainSec = SystemData.GetConstValueInt(ref tb, Dungeon_Define.Dungen_Const_Def_Key_List[Dungeon_Define.eDungenConstDef.DEF_ENERGY_PVE_TIME_PERIOD]);
                            //long ticketRemainSec = SystemData.GetConstValueInt(ref tb, Dungeon_Define.Dungen_Const_Def_Key_List[Dungeon_Define.eDungenConstDef.DEF_ENERGY_PVP_TIME_PERIOD]);
                            //long challengeRemainSec = SystemData.GetConstValueInt(ref tb, Dungeon_Define.Dungen_Const_Def_Key_List[Dungeon_Define.eDungenConstDef.DEF_ENERGY_G3VS3_TIME_PERIOD]); ;

                            //if (retError == Result_Define.eResult.SUCCESS
                            //    && (requestOp.Equals("mission_result_sweep")
                            //    || requestOp.Equals("dark_passage_result_sweep"))
                            //    )
                            if (retError == Result_Define.eResult.SUCCESS && isClear)
                            {
                                //PlayKey = isClear ? PlayKey : (int)(System.Math.Ceiling(PlayKey / Dungeon_Define.ClearFailDivideValue));
                                retError = AccountManager.UseUserKey(ref tb, AID, PlayKey);
                                userAccount.Key -= PlayKey;
                            }

                            RetBeforeInfo retBefore = new RetBeforeInfo(charInfo.level, charInfo.exp, userAccount.Gold, userAccount.Cash + userAccount.EventCash,
                                                                        userAccount.Key, userAccount.KeyFillMaxEA, userAccount.Ticket, userAccount.TicketFillMaxEA, userAccount.ChallengeTicket);

                            int SetExp = 0;
                            // update character info exp, gold
                            if (retError == Result_Define.eResult.SUCCESS && isClear)
                            {
                                int SetGold = System.Convert.ToInt32((TaskGold + AddGold) * goldRate);
                                SetExp = System.Convert.ToInt32((TaskExp + AddExp) * expRate);

                                if (SetExp > 0)
                                {
                                    int checkContents = 0;
                                    float bonusRate = AccountManager.CheckExpRate(ref tb, out checkContents);
                                    SystemData_Define.eContentsType targetContents = ((requestOp.Equals("mission_result") || requestOp.Equals("mission_result_sweep")) ? SystemData_Define.eContentsType.PVE_SENARIO :
                                                                                        (requestOp.Equals("dark_passage_result") || requestOp.Equals("dark_passage_result_sweep") ? SystemData_Define.eContentsType.PVE_DARK :
                                                                                            (requestOp.Equals("elite_result") || requestOp.Equals("elite_result_sweep") ? SystemData_Define.eContentsType.PVE_ELITE : SystemData_Define.eContentsType.NONE)));
                                    if (bonusRate > 1.0f && TriggerManager.IsSetMask(checkContents, (int)targetContents))
                                        SetExp = (int)System.Math.Floor(SetExp * bonusRate);
                                }

                                retError = CharacterManager.UpdateCharacterInfo(ref tb, CID, AID, SetExp, SetGold);

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    //시나리오, 어둠의 통로일때 길드 기여포인트 추가
                                    if (userAccount.GuildID > 0)
                                    {
                                        int guildDonatePoint = 0;
                                        if (requestOp.Equals("mission_result") || requestOp.Equals("mission_result_sweep"))
                                            guildDonatePoint = Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.MISSION_PALY] * SweepCount;
                                        else if (requestOp.Equals("dark_passage_result") || requestOp.Equals("dark_passage_result_sweep"))
                                            guildDonatePoint = Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.DARK_PASSAGE] * SweepCount;
                                        else if (requestOp.Equals("elite_result") || requestOp.Equals("elite_result_sweep"))
                                            guildDonatePoint = Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.ELIETE_DUNGEON] * SweepCount;

                                        if(guildDonatePoint > 0)
                                            retError = GuildManager.AddGuildPoint(ref tb, userAccount.GuildID, AID, guildDonatePoint);
                                    }
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS && TaskRuby > 0)
                                retError = AccountManager.AddUserEventCash(ref tb, AID, TaskRuby);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], tb.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id]));
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 1);
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);
                                retError = AccountManager.UpdatePVEFlag(ref tb, AID, false, WorldID, StageID);
                            }
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                retError = TriggerManager.ProgressTrigger(ref tb, ref userEventList, ref userAchieveList, ref userAchievePvPList, AID, setTriggerList);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    charInfo = CharacterManager.FlushCharacter(ref tb, AID, CID);
                                    retBefore.levelup = retBefore.beforelevel < charInfo.level ? 1 : 0;
                                    charInfo.exp = retBefore.beforelevel == charInfo.level && charInfo.level == Character_Define.Max_CharacterLevel ? SetExp : charInfo.exp;
                                    Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount, CharacterManager.GetCharacterCount_FromDB(ref tb, AID));

                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CharacterInfo], mJsonSerializer.ToJsonString(charInfo));

                                    if (requestOp.Equals("mission_result") || requestOp.Equals("mission_result_sweep"))
                                    {
                                        userMissionInfo = Dungeon_Manager.GetUser_MissionInfo(ref tb, ref retError, AID, WorldID, StageID, true);
                                        RetMissionTaskInfo retTaskInfo = new RetMissionTaskInfo(System.Convert.ToByte(SetRank), userMissionInfo);
                                        json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.TaskInfo], mJsonSerializer.ToJsonString(retTaskInfo));
                                    }
                                    else if (requestOp.Equals("dark_passage_result") || requestOp.Equals("dark_passage_result_sweep"))
                                    {
                                        userDarkPassgeInfo = Dungeon_Manager.GetUser_DarkPassagePlayInfo(ref tb, ref retError, AID, DungeonID, true);
                                        json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.ClearRank], mJsonSerializer.ToJsonString(SetRank));
                                    }
                                    else if (requestOp.Equals("elite_result") || requestOp.Equals("elite_result_sweep"))
                                    {
                                        userElitePlayInfo = Dungeon_Manager.GetUser_EliteDungeonPlayInfo(ref tb, ref retError, AID, DungeonID, true).retList.Where(item => item.dungeonid == DungeonID).FirstOrDefault();
                                        json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.ClearRank], mJsonSerializer.ToJsonString(SetRank));
                                    }

                                    if (showFirstItem == null)
                                        showFirstItem = new User_Inven();

                                    json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.BeforeInfo], mJsonSerializer.ToJsonString(retBefore));
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                                    json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.Show_Item], mJsonSerializer.ToJsonString(showFirstItem));
                                    json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.Dummy_Item], mJsonSerializer.ToJsonString(makeDummyItem));
                                }
                            }
                        }
                        else if (requestOp.Equals("mission_modeinfo") || requestOp.Equals("mission_clear_info"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            Account userAccount = AccountManager.GetAccountData(ref tb, AID, ref retError);
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                userAccount.LastWorldID = userAccount.LastWorldID < 1 ? 1 : userAccount.LastWorldID;
                                userAccount.LastStageID = userAccount.LastStageID < 1 ? 1 : userAccount.LastStageID;

                                List<RetMissionRank> userMissionList = Dungeon_Manager.GetUser_All_MissionRank(ref tb, AID).Where(item => item.rank > 0).ToList();
                                List<User_GuerrillaDungeon_Play> userGuerrillaList = Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref tb, AID);
                                List<RetEliteDungeonRank> userEliteList = Dungeon_Manager.GetUser_All_EliteDungeonRank(ref tb, AID);
                                List<RetWorldRank> worldRewardList = Dungeon_Manager.GetUser_All_WorldReward(ref tb, AID);

                                Dictionary<int, RetWorldRank> userWolrdList = new Dictionary<int, RetWorldRank>();
                                List<RetGuerrillaDungeonRank> retUserGuerrillaList = new List<RetGuerrillaDungeonRank>();


                                foreach (RetWorldRank setWorldReward in worldRewardList)
                                {
                                    if (!userWolrdList.Keys.Contains(setWorldReward.worldid))
                                        userWolrdList.Add(setWorldReward.worldid, setWorldReward);
                                    else
                                        userWolrdList[setWorldReward.worldid].rank = userWolrdList[setWorldReward.worldid].rank + setWorldReward.rank;
                                }

                                foreach (RetMissionRank setMission in userMissionList)
                                {
                                    if (userWolrdList.Keys.Contains(setMission.worldid))
                                        userWolrdList[setMission.worldid].rank = userWolrdList[setMission.worldid].rank + setMission.rank;
                                    else
                                    {
                                        userWolrdList.Add(setMission.worldid, new RetWorldRank(setMission.worldid, setMission.rank));
                                    }
                                }

                                foreach (User_GuerrillaDungeon_Play setobj in userGuerrillaList)
                                {
                                    RetGuerrillaDungeonRank retObj = new RetGuerrillaDungeonRank();
                                    DateTime curDate = DateTime.Parse(setobj.regdate.ToShortDateString());
                                    DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                                    TimeSpan TS = dbDate - curDate;

                                    if (TS.Days != 0)
                                        setobj.challengecount = 0;

                                    retObj.challengecount = setobj.challengecount;
                                    retObj.dungeonid = setobj.dungeonid;
                                    retObj.maxtrycount = VipManager.User_Vip_Value(ref tb, AID, VIP_Define.eVipType.DUNGEONCOUNT_MAX_DARK);
                                    retObj.rank = setobj.rank;
                                    retObj.resetcount = setobj.challengereset;
                                    retUserGuerrillaList.Add(retObj);

                                    if (!userWolrdList.Keys.Contains(setobj.worldid))
                                    {
                                        userWolrdList.Add(setobj.worldid, new RetWorldRank());
                                        userWolrdList[setobj.worldid].worldid = setobj.worldid;
                                        userWolrdList[setobj.worldid].rank = retObj.rank;
                                        userWolrdList[setobj.worldid].reward1 = 0;
                                        userWolrdList[setobj.worldid].reward1 = 0;
                                    }
                                    else
                                        userWolrdList[setobj.worldid].rank = userWolrdList[setobj.worldid].rank + retObj.rank;
                                }

                                foreach (RetEliteDungeonRank setobj in userEliteList)
                                {
                                    if (!userWolrdList.Keys.Contains(setobj.worldid))
                                    {
                                        userWolrdList.Add(setobj.worldid, new RetWorldRank());
                                        userWolrdList[setobj.worldid].worldid = setobj.worldid;
                                        userWolrdList[setobj.worldid].rank = setobj.rank;
                                        userWolrdList[setobj.worldid].reward1 = 0;
                                        userWolrdList[setobj.worldid].reward1 = 0;
                                    }
                                    else
                                        userWolrdList[setobj.worldid].rank = userWolrdList[setobj.worldid].rank + setobj.rank;
                                }

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.MissionRankList], mJsonSerializer.ToJsonString(userMissionList));
                                    json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.GuerrillaRankList], mJsonSerializer.ToJsonString(retUserGuerrillaList));
                                    json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.EliteRankList], mJsonSerializer.ToJsonString(userEliteList));
                                    json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.WorldRankList], mJsonSerializer.ToJsonString(userWolrdList.Values.ToList()));
                                    json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.LastWorld], mJsonSerializer.ToJsonString(userAccount.LastWorldID));
                                }
                            }
                        }
                        else if (requestOp.Equals("mission_taskinfo"))
                        {
                            int WorldID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("worldid"));
                            int StageID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("stageid"));

                            System_Mission_Stage stageInfo = Dungeon_Manager.GetSystem_MissionStageInfo(ref tb, StageID);
                            User_Mission_Play userMissionInfo = Dungeon_Manager.GetUser_MissionInfo(ref tb, ref retError, AID, WorldID, StageID);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                RetMissionTaskInfo retTaskInfo = new RetMissionTaskInfo(userMissionInfo);
                                json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.TaskInfo], mJsonSerializer.ToJsonString(retTaskInfo));
                                json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.MaxTryCount], mJsonSerializer.ToJsonString(stageInfo.Try_Value));

                                // TODO : remove code - challenge count check - not use (2016.02.15)
                                json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.CurrentCount], mJsonSerializer.ToJsonString(userMissionInfo.ChallengeCnt));
                            }
                        }
                        else if (requestOp.Equals("mission_start") || requestOp.Equals("dark_passage_start") || requestOp.Equals("elite_start"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            int WorldID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("worldid", "0"));
                            int StageID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("stageid", "0"));
                            int Booster1 = System.Convert.ToUInt16(queryFetcher.QueryParam_Fetch("booster1", "0"));
                            int Booster2 = System.Convert.ToUInt16(queryFetcher.QueryParam_Fetch("booster2", "0"));
                            int Booster3 = System.Convert.ToUInt16(queryFetcher.QueryParam_Fetch("booster3", "0"));
                            int DungeonID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("dungeonid", "0"));

                            Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);

                            int PlayKey = 0;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (userAccount.PVEPlayState > 0)
                                    retError = Result_Define.eResult.DUPLICATE_PLAYING;
                                else
                                    retError = Result_Define.eResult.SUCCESS;

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    if (VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.BAGSLOT_MAX_ITEM))
                                        retError = Result_Define.eResult.SUCCESS;
                                    else
                                        retError = Result_Define.eResult.ITEM_INVENTORY_OVER;
                                }
                            }

                            User_Mission_Play userMissionInfo = new User_Mission_Play();
                            int boosterGroup = 0;
                            byte dp_soul_starlevel = 1;
                            List<Return_DisassableItems_List> retDeletedItem = new List<Return_DisassableItems_List>();
                            int key = 0; int keyfillmax = 0; int keyremain = 0;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (requestOp.Equals("mission_start"))
                                {
                                    System_Mission_Stage stageInfo = Dungeon_Manager.GetSystem_MissionStageInfo(ref tb, StageID);
                                    stageInfo = Dungeon_Manager.GetSystem_MissionStageInfo(ref tb, StageID);
                                    userMissionInfo = Dungeon_Manager.GetUser_MissionInfo(ref tb, ref retError, AID, WorldID, StageID);
                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        PlayKey = stageInfo.Condition_PlayCoin;
                                        boosterGroup = stageInfo.Booster_Group_ID;
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id], ((int)StageID + SnailLog_Define.Snail_s_id_Seperator_pve_stage).ToString());
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], ((int)StageID + SnailLog_Define.Snail_s_id_Seperator_pve_stage).ToString());
                                    }
                                }
                                else if (requestOp.Equals("dark_passage_start"))
                                {
                                    //WorldID = System.Convert.ToInt32(userAccount.LastWorldID);
                                    //StageID = System.Convert.ToInt32(userAccount.LastStageID);
                                    System_Guerilla_Dungeon darkPassageInfo = Dungeon_Manager.GetSystem_DarkPassageInfo(ref tb, DungeonID);
                                    dp_soul_starlevel = (byte)darkPassageInfo.Soul_Evolution_Level;
                                    User_GuerrillaDungeon_Play userDarkPassgeInfo = Dungeon_Manager.GetUser_DarkPassagePlayInfo(ref tb, ref retError, AID, DungeonID);
                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        DateTime curDate = DateTime.Parse(userDarkPassgeInfo.regdate.ToShortDateString());
                                        DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                                        TimeSpan TS = dbDate - curDate;

                                        if (TS.Days != 0)
                                        {
                                            userDarkPassgeInfo.challengecount = 0;
                                            userDarkPassgeInfo.challengereset = 0;
                                            userDarkPassgeInfo.regdate = curDate;
                                        }

                                        if (!VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.DUNGEONCOUNT_MAX_DARK, userDarkPassgeInfo.challengecount + 1))
                                            retError = Result_Define.eResult.MISSION_TRY_COUNT_MAX;

                                        PlayKey = darkPassageInfo.Condition_PlayCoin;
                                        boosterGroup = darkPassageInfo.Booster_Group_ID;
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id], ((int)DungeonID + SnailLog_Define.Snail_s_id_Seperator_pve_dark).ToString());
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], ((int)DungeonID + SnailLog_Define.Snail_s_id_Seperator_pve_dark).ToString());
                                    }
                                }
                                else if (requestOp.Equals("elite_start"))
                                {
                                    //WorldID = System.Convert.ToInt32(userAccount.LastWorldID);
                                    //StageID = System.Convert.ToInt32(userAccount.LastStageID);

                                    System_Elite_Dungeon eliteInfo = Dungeon_Manager.GetSystem_EliteDungeonInfo(ref tb, DungeonID);
                                    User_EliteDungeon_Play userElitePlayInfo = Dungeon_Manager.GetUser_EliteDungeonPlayInfo(ref tb, ref retError, AID, DungeonID).retList.Where(item => item.dungeonid == DungeonID).FirstOrDefault();

                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        if (userElitePlayInfo == null) userElitePlayInfo = new User_EliteDungeon_Play();
                                        DateTime curDate = DateTime.Parse(userElitePlayInfo.regdate.ToShortDateString());
                                        DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                                        TimeSpan TS = dbDate - curDate;

                                        if (TS.Days != 0)
                                        {
                                            userElitePlayInfo.clearcount = 0;
                                            userElitePlayInfo.regdate = curDate;
                                        }

                                        if (!VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.DUNGEONCOUNT_MAX_ELITE, userElitePlayInfo.clearcount + 1))
                                            retError = Result_Define.eResult.MISSION_TRY_COUNT_MAX;

                                        PlayKey = eliteInfo.Condition_PlayCoin;
                                        boosterGroup = eliteInfo.Booster_Group_ID;
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id], ((int)DungeonID + SnailLog_Define.Snail_s_id_Seperator_pve_elite).ToString());
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], ((int)DungeonID + SnailLog_Define.Snail_s_id_Seperator_pve_elite).ToString());
                                    }
                                }

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    AccountManager.KeySpendCharge(ref tb, userAccount.AID, 0, ref key, ref keyfillmax, ref keyremain);
                                    userAccount.Key = key;
                                    retError = userAccount.Key < PlayKey ? Result_Define.eResult.NOT_ENOUGH_KEY : Result_Define.eResult.SUCCESS;
                                }

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    System_Booster_Group missionBooster = Dungeon_Manager.GetSystem_BoosterGroup(ref tb, boosterGroup);

                                    if (Booster1 > 0)
                                    {
                                        retError = ItemManager.UseItem(ref tb, AID, missionBooster.Boost1_ItemID, 1, ref retDeletedItem);
                                        if (retError == Result_Define.eResult.NOT_ENOUGH_USE_ITEM)
                                        {
                                            retError = ShopManager.PayBuyPrice(ref tb, AID, missionBooster.Boost1_PriceValue, Item_Define.Item_BuyType_List[missionBooster.Boost1_PriceType]);
                                        }
                                    }
                                    if (Booster2 > 0)
                                    {
                                        retError = ItemManager.UseItem(ref tb, AID, missionBooster.Boost2_ItemID, 1, ref retDeletedItem);
                                        if (retError == Result_Define.eResult.NOT_ENOUGH_USE_ITEM)
                                        {
                                            retError = ShopManager.PayBuyPrice(ref tb, AID, missionBooster.Boost2_PriceValue, Item_Define.Item_BuyType_List[missionBooster.Boost2_PriceType]);
                                        }
                                    }
                                    if (Booster3 > 0)
                                    {
                                        retError = ItemManager.UseItem(ref tb, AID, missionBooster.Boost3_ItemID, 1, ref retDeletedItem);
                                        if (retError == Result_Define.eResult.NOT_ENOUGH_USE_ITEM)
                                        {
                                            retError = ShopManager.PayBuyPrice(ref tb, AID, missionBooster.Boost1_PriceValue, Item_Define.Item_BuyType_List[missionBooster.Boost3_PriceType]);
                                        }
                                    }

                                    //if (retError == Result_Define.eResult.SUCCESS)
                                    //    retError = AccountManager.UseUserGold_And_Key(ref tb, AID, usedGold, PlayKey);
                                }
                            }

                            WorldID = System.Convert.ToInt32(userAccount.LastWorldID);
                            StageID = System.Convert.ToInt32(userAccount.LastStageID);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], tb.GetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id]));
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 0);
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);
                                retError = AccountManager.UpdatePVEFlag(ref tb, AID, true, WorldID, StageID);
                            }
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (requestOp.Equals("mission_start"))
                                {
                                    RetMissionTaskInfo retInfo = new RetMissionTaskInfo(userMissionInfo);
                                    json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.TaskInfo], mJsonSerializer.ToJsonString(retInfo));
                                }
                                else if (requestOp.Equals("dark_passage_start"))
                                {
                                    List<User_ActiveSoul> getActiveSoulList = SoulManager.GetUser_ActiveSoul(ref tb, AID);
                                    List<User_Character_Equip_Soul> getEquipList = SoulManager.GetUser_Character_Equip_Soul(ref tb, AID, CID);
                                    List<User_ActiveSoul> equipSoulList = new List<User_ActiveSoul>();
                                    foreach (User_ActiveSoul item in getActiveSoulList)
                                    {
                                        if (item.soulseq > 0)
                                        {
                                            var findEquip = getEquipList.Find(equip => equip.soul_type == Soul_Define.Equip_Soul_Type_Acitve && equip.soulseq == item.soulseq);
                                            int setSlot = findEquip != null ? findEquip.slot_num : 0;
                                            if (setSlot > 0)
                                            {
                                                System_Soul_Active soulInfo = SoulManager.GetSoul_System_Soul_Active(ref tb, item.soulid);
                                                soulInfo = SoulManager.GetSoul_System_Soul_Active(ref tb, soulInfo.SoulGroup, Soul_Define.Soul_Max_Grade);
                                                item.soulid = soulInfo.SoulID;
                                                equipSoulList.Add(item);
                                            }
                                        }
                                    }

                                    System_Guerrilla_Soul darkPassageSoulInfo;
                                    bool bFind = false;
                                    do
                                    {
                                        bFind = false;
                                        darkPassageSoulInfo = Dungeon_Manager.GetSystem_DarkPassageSoulInfo(ref tb);
                                        System_Soul_Active soulInfo = SoulManager.GetSoul_System_Soul_Active(ref tb, darkPassageSoulInfo.PC_SoulID);
                                        soulInfo = SoulManager.GetSoul_System_Soul_Active(ref tb, soulInfo.SoulGroup, Soul_Define.Soul_Max_Grade);
                                        var findSoul = equipSoulList.Find(item => item.soulid == soulInfo.SoulID);
                                        if (findSoul == null)
                                            bFind = true;
                                        else
                                            bFind = false;

                                    } while (!bFind || darkPassageSoulInfo.Guerrilla_Soul_ID == 0);

                                    User_ActiveSoul setSoul = SoulManager.GetDarkPassageRandomSoulSpecialBuff(ref tb, AID, CID, dp_soul_starlevel, ref darkPassageSoulInfo);
                                    Ret_Soul_Active retSoul = new Ret_Soul_Active(setSoul);
                                    retSoul.cid = CID;
                                    json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.BonusSoulInfo], mJsonSerializer.ToJsonString(retSoul));
                                }
                                userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);

                                json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.LeftKey], mJsonSerializer.ToJsonString(key));
                                json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.LeftKeyRemainTime], mJsonSerializer.ToJsonString(keyremain));
                                json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.LeftGold], mJsonSerializer.ToJsonString(userAccount.Gold));
                                json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.DeletedItem], mJsonSerializer.ToJsonString(retDeletedItem));                                                                
                            }
                        }
                        else if (requestOp.Equals("mission_rank_reward"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            int WorldID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("worldid"));
                            int RewardChoice = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("rewarddiv"));

                            List<RetMissionRank> userMissionList = Dungeon_Manager.GetUser_All_MissionRank(ref tb, AID, WorldID);
                            List<User_GuerrillaDungeon_Play> userGuerrillaList = Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref tb, AID);
                            List<RetEliteDungeonRank> userEliteList = Dungeon_Manager.GetUser_All_EliteDungeonRank(ref tb, AID);

                            List<RetWorldRank> worldRewardList = Dungeon_Manager.GetUser_All_WorldReward(ref tb, AID);
                            Dictionary<int, RetWorldRank> userWolrdList = new Dictionary<int, RetWorldRank>();

                            foreach (RetWorldRank setWorldReward in worldRewardList)
                            {
                                if (!userWolrdList.Keys.Contains(setWorldReward.worldid))
                                    userWolrdList.Add(setWorldReward.worldid, setWorldReward);
                            }

                            int TotalRank = userMissionList.Sum(item => item.rank) + userGuerrillaList.Where(item => item.worldid == WorldID).Sum(item => item.rank) + userEliteList.Where(item => item.worldid == WorldID).Sum(item => item.rank);

                            int FirstRewardRank = SystemData.GetConstValueInt(ref tb, Dungeon_Define.Dungen_Const_Def_Key_List[Dungeon_Define.eDungenConstDef.DEF_SCENARIO_COMPLETE_GIFT_STEP1]);
                            int SecondRewardRank = SystemData.GetConstValueInt(ref tb, Dungeon_Define.Dungen_Const_Def_Key_List[Dungeon_Define.eDungenConstDef.DEF_SCENARIO_COMPLETE_GIFT_STEP2]);

                            retError = ((RewardChoice == 1) ? (TotalRank >= FirstRewardRank) : (TotalRank >= SecondRewardRank)) ?
                                Result_Define.eResult.SUCCESS : Result_Define.eResult.NOT_ENOUGH_RANK_SCORE_REWARD;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id], ((int)WorldID + SnailLog_Define.Snail_s_id_Seperator_pve_stage).ToString());
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], ((int)WorldID + SnailLog_Define.Snail_s_id_Seperator_pve_stage).ToString());
                                RetWorldRank userWorldRewardInfo = Dungeon_Manager.GetUser_WorldReward(ref tb, AID, WorldID);
                                retError = userWorldRewardInfo != null ?
                                                ((RewardChoice == 1) ?
                                                        (userWorldRewardInfo.reward1 < 1 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DUPLICATE_REWARD_MISSION) :
                                                        (userWorldRewardInfo.reward2 < 1 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DUPLICATE_REWARD_MISSION))
                                                : Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    bool isGet = ((RewardChoice == 1) ? (userWorldRewardInfo.reward1 < 1) : (userWorldRewardInfo.reward2 < 1));
                                    retError = isGet ? Result_Define.eResult.SUCCESS : Result_Define.eResult.ALREADY_GIVE_REWARD;
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                retError = Dungeon_Manager.UpdateWorldRankReward(ref tb, AID, WorldID, RewardChoice);
                            }

                            List<User_Inven> makeRealItem = new List<User_Inven>();
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                System_Mission_World MissionWorldInfo = TheSoul.DataManager.Dungeon_Manager.GetSystem_MissionWorldInfo(ref tb, WorldID);
                                long setDropID = RewardChoice == 1 ? MissionWorldInfo.FirstClear_DropBoxGroupId : MissionWorldInfo.SecondClear_DropBoxGroupId;
                                Character charInfo = CharacterManager.GetCharacter(ref tb, AID, CID);

                                List<System_Drop_Group> getDropList = DropManager.GetDropResult(ref tb, AID, setDropID, (short)charInfo.Class);

                                foreach (System_Drop_Group setDrop in getDropList)
                                {
                                    List<User_Inven> getItem = new List<User_Inven>();
                                    retError = DropManager.MakeDropItem(ref tb, ref getItem, setDrop, AID, CID);

                                    if (retError != Result_Define.eResult.SUCCESS)
                                        break;

                                    makeRealItem.AddRange(getItem);
                                }

                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = makeRealItem.Count > 0 ? Result_Define.eResult.SUCCESS : Result_Define.eResult.ITEM_CREATE_FAIL;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account userInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userInfo);
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                                    RetWorldRank userWorldRewardInfo = Dungeon_Manager.GetUser_WorldReward(ref tb, AID, WorldID, true);
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                                }
                            }
                        }
                        else if (requestOp.Equals("elite_modeinfo"))
                        {
                            RetUserEliteDungeon_Play getList = Dungeon_Manager.GetUser_EliteDungeonPlayInfo(ref tb, ref retError, AID, 0);
                            List<RetEliteDungeonList> retList = new List<RetEliteDungeonList>();

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                getList.retList = getList.retList.OrderBy(item => item.dungeonid).ToList();

                                foreach (User_EliteDungeon_Play setinfo in getList.retList)
                                {
                                    DateTime curDate = DateTime.Parse(setinfo.regdate.ToShortDateString());
                                    DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                                    TimeSpan TS = dbDate - curDate;

                                    if (TS.Days != 0)
                                    {
                                        setinfo.clearcount = 0;
                                        setinfo.resetcount = 0;
                                        setinfo.regdate = curDate;
                                    }

                                    retList.Add(new RetEliteDungeonList(setinfo));
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.EliteDungeonList], mJsonSerializer.ToJsonString(retList));
                                json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.MissionLastID], mJsonSerializer.ToJsonString(getList.laststage));
                                json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.ElieteDungeonLastID], mJsonSerializer.ToJsonString(getList.selectedidx));
                            }
                        }
                        else if (requestOp.Equals("pve_revival"))
                        {
                            int pveType = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("pve_type"));
                            //Dungeon_Define.eDungeonBoostType pveType = (Dungeon_Define.eDungeonBoostType)queryFetcher.QueryParam_Fetch_Request(

                            if (!System.Enum.IsDefined(typeof(Dungeon_Define.eDungeonBoostType), (Dungeon_Define.eDungeonBoostType)pveType))
                                retError = Result_Define.eResult.SYSTEM_PARAM_ERROR;
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                //switch ((Dungeon_Define.eDungeonBoostType)pveType)
                                //{
                                //    case Dungeon_Define.eDungeonBoostType.ContentType_Senario:
                                //        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id], (pveType + SnailLog_Define.Snail_s_id_Seperator_pve_stage).ToString());
                                //        break;
                                //    case Dungeon_Define.eDungeonBoostType.ContentType_Guerilla:
                                //        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id], (pveType + SnailLog_Define.Snail_s_id_Seperator_pve_dark).ToString());
                                //        break;
                                //    case Dungeon_Define.eDungeonBoostType.ContentType_Elite:
                                //        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id], (pveType + SnailLog_Define.Snail_s_id_Seperator_pve_elite).ToString());
                                //        break;
                                //}                                

                                Character charInfo = CharacterManager.GetCharacter(ref tb, AID, CID);
                                if (charInfo.cid > 0)
                                {
                                    int useCost = SystemData.GetConstValueInt(ref tb, Dungeon_Define.Dungen_Const_Def_Key_List[Dungeon_Define.eDungenConstDef.DEF_PC_REVIVE_PRICE_RUBY]);
                                    retError = AccountManager.UseUserCash(ref tb, AID, useCost);
                                }
                                else
                                    retError = Result_Define.eResult.CHARACTER_NOT_FOUND;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account UserInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetRuby], (UserInfo.Cash + UserInfo.EventCash).ToString());
                                }
                            }
                        }
                        // pve_count_reset
                        else if (requestOp.Equals("pve_count_reset"))
                        {
                            int pveType = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("pve_type"));
                            int WorldID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("worldid", "0"));
                            int StageID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("stageid", "0"));
                            int DungeonID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("dungeonid", "0"));

                            int useRubyCount = 0;
                            int resetCount = 0;

                            if (!System.Enum.IsDefined(typeof(Dungeon_Define.eDungeonBoostType), (Dungeon_Define.eDungeonBoostType)pveType))
                                retError = Result_Define.eResult.SYSTEM_PARAM_ERROR;
                            else if ((Dungeon_Define.eDungeonBoostType)pveType == Dungeon_Define.eDungeonBoostType.ContentType_Guerilla)
                            {
                                User_GuerrillaDungeon_Play userDarkPassgeInfo = Dungeon_Manager.GetUser_DarkPassagePlayInfo(ref tb, ref retError, AID, DungeonID);
                                if (userDarkPassgeInfo.cleartime < 1 || userDarkPassgeInfo.rank < 1)
                                    retError = Result_Define.eResult.WRONG_STAGEINFO;

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    DateTime curDate = DateTime.Parse(userDarkPassgeInfo.regdate.ToShortDateString());
                                    DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                                    TimeSpan TS = dbDate - curDate;

                                    if (TS.Days != 0)
                                    {
                                        userDarkPassgeInfo.challengecount = 0;
                                        userDarkPassgeInfo.challengereset = 0;
                                        userDarkPassgeInfo.regdate = curDate;
                                    }

                                    //tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id], (DungeonID + SnailLog_Define.Snail_s_id_Seperator_pve_stage).ToString());

                                    if (VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.DUNGEONCOUNT_RESET_DARK, userDarkPassgeInfo.challengereset))
                                    {
                                        useRubyCount = SystemData.GetConstValueInt(ref tb, Dungeon_Define.Dungen_Const_Def_Key_List[Dungeon_Define.eDungenConstDef.DEF_RESET_COST_RUBY_DARK]);
                                        retError = Result_Define.eResult.SUCCESS;

                                    }
                                    else
                                        retError = Result_Define.eResult.VIP_DUNGEON_RESET_COUNT_OVER;
                                }
                            }
                            else if ((Dungeon_Define.eDungeonBoostType)pveType == Dungeon_Define.eDungeonBoostType.ContentType_Elite)
                            {
                                User_EliteDungeon_Play userElitePlayInfo = Dungeon_Manager.GetUser_EliteDungeonPlayInfo(ref tb, ref retError, AID, DungeonID).retList.Where(item => item.dungeonid == DungeonID).FirstOrDefault();

                                if (userElitePlayInfo.rank < 1)
                                    retError = Result_Define.eResult.WRONG_STAGEINFO;

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    DateTime curDate = DateTime.Parse(userElitePlayInfo.regdate.ToShortDateString());
                                    DateTime dbDate = DateTime.Parse(DateTime.Now.ToShortDateString());
                                    TimeSpan TS = dbDate - curDate;

                                    if (TS.Days != 0)
                                    {
                                        userElitePlayInfo.clearcount = 0;
                                        userElitePlayInfo.resetcount = 0;
                                        userElitePlayInfo.regdate = curDate;
                                    }

                                    if (VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.DUNGEONCOUNT_RESET_ELITE, userElitePlayInfo.resetcount))
                                    {
                                        useRubyCount = SystemData.GetConstValueInt(ref tb, Dungeon_Define.Dungen_Const_Def_Key_List[Dungeon_Define.eDungenConstDef.DEF_RESET_COST_RUBY_ELITE]);
                                        retError = Result_Define.eResult.SUCCESS;
                                    }
                                    else
                                        retError = Result_Define.eResult.VIP_DUNGEON_RESET_COUNT_OVER;
                                }
                            }
                            else
                                retError = Result_Define.eResult.VIP_DUNGEON_RESET_TYPE_INVALIDE;

                            if (retError == Result_Define.eResult.SUCCESS && useRubyCount > 0)
                                retError = AccountManager.UseUserCash(ref tb, AID, useRubyCount);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                retError = Dungeon_Manager.ResetPvEPlayCount(ref tb, AID, (Dungeon_Define.eDungeonBoostType)pveType, WorldID, StageID, DungeonID);
                                if ((Dungeon_Define.eDungeonBoostType)pveType == Dungeon_Define.eDungeonBoostType.ContentType_Senario)
                                    resetCount = Dungeon_Manager.GetUser_MissionInfo(ref tb, ref retError, AID, WorldID, StageID, true).ChallengeReset;
                                else if ((Dungeon_Define.eDungeonBoostType)pveType == Dungeon_Define.eDungeonBoostType.ContentType_Guerilla)
                                    resetCount = Dungeon_Manager.GetUser_DarkPassagePlayInfo(ref tb, ref retError, AID, DungeonID, true).challengereset;
                                else if ((Dungeon_Define.eDungeonBoostType)pveType == Dungeon_Define.eDungeonBoostType.ContentType_Elite)
                                    resetCount = Dungeon_Manager.GetUser_EliteDungeonPlayInfo(ref tb, ref retError, AID, DungeonID, true).retList.Where(item => item.dungeonid == DungeonID).FirstOrDefault().resetcount;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account UserInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.RetRuby], (UserInfo.Cash + UserInfo.EventCash).ToString());
                                    json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.CurrentResetCount], resetCount.ToString());
                                }
                            }
                        }
                        retJson = queryFetcher.Render(json.ToJson(), retError);
                    }
                    else
                    {
                        retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(DefineError.System_Unknown_Operation), Result_Define.eResult.System_Unknown_Operation);
                    }
                }
                catch (Exception errorEx)
                {
                    string error = "";
#if DEBUG
                    error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
#else
                    if (queryFetcher.SetDebugMode)
                        error = mJsonSerializer.AddJson(error, "StackTrace", mJsonSerializer.ToJsonString(errorEx.StackTrace));
#endif

                    error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(error), Result_Define.eResult.System_Exception);
                }
                finally
                {
                    //if (AID > 0)
                    //    queryFetcher.CheckSnail_ID(ref tb, AID);
                    queryFetcher.SetShowLogMode = tb.EndTransaction(queryFetcher.Render_errorFlag);
                    queryFetcher.ErrorLogWrite(retJson, ref tb);
                    tb.Dispose();
                } 
            }
        }
    }
}