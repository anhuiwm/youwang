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
    public partial class RequestEvent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                "event_type_list",
                "event_list",
                "event_daily_count",
                "event_daily_count_buy",
                "achieve_list",
                "get_event_reward",
                "get_achive_reward",
                "get_achive_reward_all",
                "get_first_pay_reward",

                "get_user_event_info",

                "pvp_achieve_list",
                "get_pvp_achive_reward",

                "event_7day_list",
                "get_event_7day_reward",
                "buy_event_7day_package",
                "wechat_share",
                "fb_open",
                
                // for test method only work in debug mode
                "trigger_progress",
                "isClear",
                "event_reward_reset",
                "event_check_reset",
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
                    if (Array.IndexOf(ops, requestOp) >= 0)
                    {
                        queryFetcher.operation = requestOp;
                        Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;

                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.op], requestOp);
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], AID);
                        long CID = queryFetcher.QueryParam_FetchLong("cid");

                        if (requestOp.Equals("event_type_list"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            List<User_Event_Data> userEventList = TriggerManager.Check_Event_Data_List(ref tb, AID);
                            List<System_EventGroup_Admin> getEventGroupInfo = TriggerManager.GetSystem_EventGroup_Admin(ref tb);

                            List<string> event_type_group = userEventList.Select(ev => ev.Event_Type).GroupBy(ev => ev).Select(group => group.Key).ToList();

                            List<RetEventGroupAdmin> retObj = new List<RetEventGroupAdmin>();
                            event_type_group.ForEach(setItem =>
                            {
                                var findEvent = getEventGroupInfo.Find(item => item.Event_Type.Equals(setItem));
                                if (findEvent != null)
                                    retObj.Add(new RetEventGroupAdmin(findEvent));
                            }
                            );

                            retObj = retObj.OrderBy(item => item.order_index).ToList();

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.EventTypeList], mJsonSerializer.ToJsonString(event_type_group));
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.EventTypeInfoList], mJsonSerializer.ToJsonString(retObj));
                            }
                        }
                        else if (requestOp.Equals("event_list"))
                        {
                            int eventGroupType = queryFetcher.QueryParam_FetchInt("group_type");

                            //if(string.IsNullOrEmpty(Event_Type))
                            //    retError = Result_Define.eResult.TRIGGER_EVENT_TYPE_EMPTY;
                            //else
                            retError = Result_Define.eResult.SUCCESS;
                            
                            Character charInfo = CharacterManager.GetCharacter(ref tb, AID, CID);

                            if (retError == Result_Define.eResult.SUCCESS)
                                if (charInfo.aid != AID || charInfo.cid != CID)
                                    retError = Result_Define.eResult.CHARACTER_NOT_FOUND;

                            List<Ret_Event_Data> retEventObj = new List<Ret_Event_Data>();
                            List<System_EventGroup_Admin> getEventGroupInfo = new List<System_EventGroup_Admin>();
                            List<RetEventGroupAdmin> retEventAdmin = new List<RetEventGroupAdmin>();

                            List<Character> userCharacter = CharacterManager.GetCharacterList(ref tb, AID);
                            List<RetMissionRank> userMission = Dungeon_Manager.GetUser_All_MissionRank(ref tb, AID);
                            List<User_GuerrillaDungeon_Play> userGuerillaMission = Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref tb, AID);
                            List<RetEliteDungeonRank> userElistMission = Dungeon_Manager.GetUser_All_EliteDungeonRank(ref tb, AID);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<User_Event_Data> userEventList = TriggerManager.Check_Event_Data_List(ref tb, AID);
                                List<System_Event> systemEventInfo = TriggerManager.GetSystem_Event_List(ref tb);
                                getEventGroupInfo = TriggerManager.GetSystem_EventGroup_Admin(ref tb);

                                if (userEventList.Count > 0)
                                {
                                    List<RetEventGroupAdmin> checkEventAdmin = new List<RetEventGroupAdmin>();

                                    getEventGroupInfo.ForEach(setItem =>
                                    {
                                        if (setItem.Event_Group_Type == eventGroupType)
                                            checkEventAdmin.Add(new RetEventGroupAdmin(setItem));
                                    }
                                    );
                                    userEventList = userEventList.OrderBy(userEvent => userEvent.Event_ID).ToList();
                                    userEventList.ForEach(setEvent =>
                                    {
                                        var adminEvent = checkEventAdmin.Find(eventGroup => eventGroup.event_type == setEvent.Event_Type);

                                        if (adminEvent != null)
                                        {
                                            int maxValue1 = 0, maxValue2 = 0;
                                            bool isClear = false;
                                            User_Event_Data evObj = TriggerManager.CheckClear(ref tb, AID, setEvent, out isClear, out maxValue1, out maxValue2
                                                                                                    , userCharacter, userMission, userGuerillaMission, userElistMission);

                                            //if (!isClear || !evObj.RewardFlag.Equals("Y"))
                                            {
                                                Ret_Event_Data setObj = new Ret_Event_Data(evObj, maxValue1, maxValue2, isClear);
                                                setObj.reward_item = new List<Ret_Reward_Item>();

                                                System_Event setSystemInfo = systemEventInfo.Find(ev => ev.Event_ID == evObj.Event_ID);
                                                if (setSystemInfo != null)
                                                {
                                                    setObj.event_price_ruby = setSystemInfo.Event_Price_Ruby;
                                                    setObj.event_name = setSystemInfo.Event_Tooltip;
                                                    setObj.orderid = setSystemInfo.OrderID;

                                                    List<System_Event_Reward_Box> setOpenBoxList = new List<System_Event_Reward_Box>();

                                                    if (setSystemInfo.Reward_Box1ID > 0)
                                                        setOpenBoxList.AddRange(TriggerManager.GetSystem_Event_Reward_Box_List(ref tb, setSystemInfo.Reward_Box1ID));
                                                    if (setSystemInfo.Reward_Box2ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Warrior)
                                                        setOpenBoxList.AddRange(TriggerManager.GetSystem_Event_Reward_Box_List(ref tb, setSystemInfo.Reward_Box2ID));
                                                    if (setSystemInfo.Reward_Box3ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Swordmaster)
                                                        setOpenBoxList.AddRange(TriggerManager.GetSystem_Event_Reward_Box_List(ref tb, setSystemInfo.Reward_Box3ID));
                                                    if (setSystemInfo.Reward_Box4ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Taoist)
                                                        setOpenBoxList.AddRange(TriggerManager.GetSystem_Event_Reward_Box_List(ref tb, setSystemInfo.Reward_Box4ID));

                                                    setOpenBoxList.ForEach(setBox =>
                                                    {
                                                        setObj.reward_item.Add(new Ret_Reward_Item(setBox));
                                                    });
                                                    retEventObj.Add(setObj);
                                                }

                                                if (isClear && evObj.RewardFlag.Equals("N"))
                                                    adminEvent.nflag = 1;
                                            }
                                        }
                                    });

                                    checkEventAdmin.ForEach(adminEvent =>
                                    {
                                        if (userEventList.Count(setEvent => setEvent.Event_Type == adminEvent.event_type) > 0)
                                            retEventAdmin.Add(adminEvent);
                                    }
                                    );
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.EventList], mJsonSerializer.ToJsonString(retEventObj));
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.EventTypeInfoList], mJsonSerializer.ToJsonString(retEventAdmin));
                            }
                        }
                        else if (requestOp.Equals("achieve_list"))
                        {
                            retError = Result_Define.eResult.SUCCESS;

                            List<User_Event_Data> userAchieveList = TriggerManager.Check_Achieve_Data_List(ref tb, AID);

                            List<Ret_Achieve_Data> retObj = new List<Ret_Achieve_Data>();

                            List<Character> userCharacter = CharacterManager.GetCharacterList(ref tb, AID);
                            List<RetMissionRank> userMission = Dungeon_Manager.GetUser_All_MissionRank(ref tb, AID);
                            List<User_GuerrillaDungeon_Play> userGuerillaMission = Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref tb, AID);
                            List<RetEliteDungeonRank> userElistMission = Dungeon_Manager.GetUser_All_EliteDungeonRank(ref tb, AID);

                            userAchieveList.ForEach(setEvent =>
                            {
                                int maxValue1 = 0, maxValue2 = 0;
                                bool isClear = false;
                                User_Event_Data setObj = TriggerManager.CheckClear(ref tb, AID, setEvent, out isClear, out maxValue1, out maxValue2
                                                                                        , userCharacter, userMission, userGuerillaMission, userElistMission);

                                retObj.Add(new Ret_Achieve_Data(setObj, maxValue1, maxValue2, isClear));
                            });

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.AchieveList], mJsonSerializer.ToJsonString(retObj));
                            }
                        }
                        else if (requestOp.Equals("pvp_achieve_list"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            PvP_Define.ePvPType setPvPType = (PvP_Define.ePvPType)queryFetcher.QueryParam_FetchInt("pvp_type", (int)PvP_Define.ePvPType.MATCH_NONE);

                            List<Character> userCharacter = CharacterManager.GetCharacterList(ref tb, AID);
                            List<RetMissionRank> userMission = Dungeon_Manager.GetUser_All_MissionRank(ref tb, AID);
                            List<User_GuerrillaDungeon_Play> userGuerillaMission = Dungeon_Manager.GetUser_All_GuerrillaDungeonRank(ref tb, AID);
                            List<RetEliteDungeonRank> userElistMission = Dungeon_Manager.GetUser_All_EliteDungeonRank(ref tb, AID);

                            Character charInfo = userCharacter.Find(charinfo => charinfo.cid == CID);
                            if (charInfo == null)
                                charInfo = new Character();

                            if (retError == Result_Define.eResult.SUCCESS)
                                if (charInfo.aid != AID || charInfo.cid != CID)
                                    retError = Result_Define.eResult.CHARACTER_NOT_FOUND;

                            List<Ret_Event_Data> retObj = new List<Ret_Event_Data>();
                            int setGrade = 0;
                            int highGrade = 0;
                            if (retError == Result_Define.eResult.SUCCESS)
                            {

                                List<User_Event_Data> userAchieveList = TriggerManager.Check_Achieve_PvP_Data_List(ref tb, AID);
                                List<System_Achieve_PvP> systemEventInfo = TriggerManager.GetSystem_Achieve_PvP_List(ref tb);

                                if (setPvPType == PvP_Define.ePvPType.MATCH_1VS1)
                                {
                                    PVP_Record getInfo = PvPManager.GetUser_PvP_Record(ref tb, AID, 0, setPvPType);
                                    setGrade = PvPManager.Get1vs1PvPGrade(getInfo.totalhonorpoint);
                                    highGrade = PvPManager.GetUser_PvP_High_Grade(ref tb, AID, (int)setPvPType);
                                }
                                else if (setPvPType == PvP_Define.ePvPType.MATCH_FREE)
                                {
                                    long RankTotalPlayerCount = PvPManager.GetTotal_PvP_Rank_Player(ref tb, 0, PvP_Define.ePvPType.MATCH_FREE);

                                    long userRank = PvPManager.GetUser_PvP_Rank(ref tb, AID, 0, PvP_Define.ePvPType.MATCH_FREE);
                                    setGrade = PvPManager.GetFreePvPGrade(userRank, RankTotalPlayerCount);
                                    highGrade = PvPManager.GetUser_PvP_High_Grade(ref tb, AID, (int)setPvPType);
                                }

                                userAchieveList.FindAll(findEvent => findEvent.PVP_Type == (int)setPvPType).ForEach(setEvent =>
                                {
                                    int maxValue1 = 0, maxValue2 = 0;
                                    bool isClear = false;
                                    User_Event_Data evObj = TriggerManager.CheckClear(ref tb, AID, setEvent, out isClear, out maxValue1, out maxValue2, userCharacter, userMission, userGuerillaMission, userElistMission);
                                    Ret_Event_Data setObj = new Ret_Event_Data(evObj, maxValue1, maxValue2, isClear, setEvent.Ranking_List_Type);

                                    setObj.reward_item = new List<Ret_Reward_Item>();

                                    System_Achieve_PvP setSystemInfo = systemEventInfo.Find(ev => ev.AchieveID == setEvent.Event_ID);
                                    if (setSystemInfo != null)
                                    {
                                        setObj.event_name = setSystemInfo.TaskCN;

                                        List<System_Achieve_PvP_RewardBox> setOpenBoxList = new List<System_Achieve_PvP_RewardBox>();

                                        if (setSystemInfo.Reward_Box1ID > 0)
                                            setOpenBoxList.AddRange(TriggerManager.GetSystem_Achieve_PvP_Reward_Box_List(ref tb, setSystemInfo.Reward_Box1ID));
                                        if (setSystemInfo.Reward_Box2ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Warrior)
                                            setOpenBoxList.AddRange(TriggerManager.GetSystem_Achieve_PvP_Reward_Box_List(ref tb, setSystemInfo.Reward_Box2ID));
                                        if (setSystemInfo.Reward_Box3ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Swordmaster)
                                            setOpenBoxList.AddRange(TriggerManager.GetSystem_Achieve_PvP_Reward_Box_List(ref tb, setSystemInfo.Reward_Box3ID));
                                        if (setSystemInfo.Reward_Box4ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Taoist)
                                            setOpenBoxList.AddRange(TriggerManager.GetSystem_Achieve_PvP_Reward_Box_List(ref tb, setSystemInfo.Reward_Box4ID));

                                        setOpenBoxList.ForEach(setBox =>
                                        {
                                            setObj.reward_item.Add(new Ret_Reward_Item(setBox));
                                        });
                                        retObj.Add(setObj);
                                    }
                                });
                            }

                            List<Ret_Reward_Item> rankRewardList = new List<Ret_Reward_Item>();

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                PvP_Define.ePvPRewardRepeatType rewardType = PvPManager.GetPvP_RewardRepeatType(ref tb, setPvPType);
                                List<System_Battle_Reward> systemRewardInfo = PvPManager.GetSystem_Battle_Reward_List(ref tb, setPvPType, rewardType);

                                long userRank = 0;
                                if (setPvPType == PvP_Define.ePvPType.MATCH_GUILD_3VS3)
                                {
                                    long GID = TheSoul.DataManager.GuildManager.GetGuildInfo(ref tb, AID).guild_id;
                                    if (GID > 0)
                                        userRank = PvPManager.GetGuildPvP_Rank(ref tb, GID);
                                }
                                else
                                    userRank = PvPManager.GetUser_PvP_Rank(ref tb, AID, 0, setPvPType);

                                if (userRank > 0)
                                {
                                    var findRewardInfo = systemRewardInfo.Find(setInfo => setInfo.MinValue <= userRank && (setInfo.MaxValue >= userRank || setInfo.MaxValue <= 0));
                                    if (findRewardInfo != null)
                                    {
                                        if (findRewardInfo.Reward1_Type > 0 && findRewardInfo.Reward1_Value > 0)
                                            rankRewardList.Add(new Ret_Reward_Item(findRewardInfo.Reward1_Type, findRewardInfo.Reward1_Value, 1, 0, 0));
                                        if (findRewardInfo.Reward2_Type > 0 && findRewardInfo.Reward2_Value > 0)
                                            rankRewardList.Add(new Ret_Reward_Item(findRewardInfo.Reward2_Type, findRewardInfo.Reward2_Value, 1, 0, 0));
                                    }
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.AchieveList], mJsonSerializer.ToJsonString(retObj));
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.CurrentPvPGrade], mJsonSerializer.ToJsonString(setGrade));
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.HighestPvPGrade], mJsonSerializer.ToJsonString(highGrade));
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.RankingRewardList], mJsonSerializer.ToJsonString(rankRewardList));
                            }
                        }
                        else if (requestOp.Equals("get_event_reward") || requestOp.Equals("get_achive_reward") || requestOp.Equals("get_pvp_achive_reward") || requestOp.Equals("get_achive_reward_all"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            List<long> user_event_idx_list = requestOp.Equals("get_achive_reward_all") ?
                                                                        user_event_idx_list = mJsonSerializer.JsonToObject<List<long>>(queryFetcher.QueryParam_Fetch("user_event_seq", "[]"))
                                                                    :   user_event_idx_list = new List<long>() { queryFetcher.QueryParam_FetchLong("user_event_idx") };
                            

                            //long user_event_idx = queryFetcher.QueryParam_FetchInt("user_event_idx");

                            List<System_Event_Reward_Box> setOpenBoxList = new List<System_Event_Reward_Box>();
                            List<Ret_Event_Data> retNextEventList = new List<Ret_Event_Data>();
                            List<Ret_Event_Data> retEventList = new List<Ret_Event_Data>();

                            Trigger_Define.eEventListType setEventList = requestOp.Equals("get_event_reward") ? Trigger_Define.eEventListType.Event :
                                                (requestOp.Equals("get_achive_reward") || requestOp.Equals("get_achive_reward_all")) ? Trigger_Define.eEventListType.Achive :
                                                requestOp.Equals("get_pvp_achive_reward") ? Trigger_Define.eEventListType.PvP_Achive : Trigger_Define.eEventListType.None;
                            int SetExp = 0;
                            int useRuby = 0;

                            retError = Result_Define.eResult.SUCCESS;
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.BAGSLOT_MAX_ITEM))
                                    retError = Result_Define.eResult.SUCCESS;
                                else
                                    retError = Result_Define.eResult.ITEM_INVENTORY_OVER;
                            }

                            Character charInfo = retError == Result_Define.eResult.SUCCESS ? CharacterManager.GetCharacter(ref tb, AID, CID) : null;
                            RetBeforeInfo retBefore = new RetBeforeInfo();
                            bool checkIgnore = false;

                            foreach (long user_event_idx in user_event_idx_list)
                            {
                                User_Event_Data userEventInfo = setEventList == Trigger_Define.eEventListType.Event ? TriggerManager.GetUser_Event_Data(ref tb, AID, user_event_idx)
                                                                : setEventList == Trigger_Define.eEventListType.Achive ? TriggerManager.GetUser_Achieve_Data(ref tb, AID, user_event_idx)
                                                                : setEventList == Trigger_Define.eEventListType.PvP_Achive ? TriggerManager.GetUser_Achieve_PvP_Data(ref tb, AID, user_event_idx)
                                                                : new User_Event_Data();

                                if (userEventInfo.User_Event_ID == 0)
                                    retError = Result_Define.eResult.TRIGGER_ID_NOT_FOUND;

                                if (retError == Result_Define.eResult.SUCCESS)
                                    if (charInfo.aid != AID || charInfo.cid != CID)
                                        retError = Result_Define.eResult.CHARACTER_NOT_FOUND;

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], TaskLogInfo.GetS_TaskID(userEventInfo.Event_ID, setEventList));
                                }
                                int maxValue1 = 0, maxValue2 = 0;
                                bool isClear = false;
                                List<long> NextEventID = new List<long>();
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    userEventInfo = TriggerManager.CheckClear(ref tb, AID, userEventInfo, out isClear, out maxValue1, out maxValue2);

                                    if (isClear && userEventInfo.RewardFlag.Equals("N"))
                                    {
                                        System_Event systemEventInfo = setEventList == Trigger_Define.eEventListType.Event ? TriggerManager.GetSystem_Event(ref tb, userEventInfo.Event_ID) : null;
                                        System_Achieve systemAchiveInfo = setEventList == Trigger_Define.eEventListType.Achive ? TriggerManager.GetSystem_Achieve(ref tb, userEventInfo.Event_ID) : null;
                                        System_Achieve_PvP systemPvPInfo = setEventList == Trigger_Define.eEventListType.PvP_Achive ? TriggerManager.GetSystem_Achieve_PvP(ref tb, userEventInfo.Event_ID) : null;

                                        if (systemEventInfo != null)
                                        {
                                            useRuby += systemEventInfo.Event_Price_Ruby;
                                            if (systemEventInfo.Reward_Box1ID > 0)
                                                setOpenBoxList.AddRange(TriggerManager.GetSystem_Event_Reward_Box_List(ref tb, systemEventInfo.Reward_Box1ID));
                                            if (systemEventInfo.Reward_Box2ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Warrior)
                                                setOpenBoxList.AddRange(TriggerManager.GetSystem_Event_Reward_Box_List(ref tb, systemEventInfo.Reward_Box2ID));
                                            if (systemEventInfo.Reward_Box3ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Swordmaster)
                                                setOpenBoxList.AddRange(TriggerManager.GetSystem_Event_Reward_Box_List(ref tb, systemEventInfo.Reward_Box3ID));
                                            if (systemEventInfo.Reward_Box4ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Taoist)
                                                setOpenBoxList.AddRange(TriggerManager.GetSystem_Event_Reward_Box_List(ref tb, systemEventInfo.Reward_Box4ID));
                                        }
                                        else if (systemAchiveInfo != null)
                                        {
                                            var findNext = TriggerManager.GetSystem_Achieve_List(ref tb).FindAll(achieve => achieve.Require_AchieveID == userEventInfo.Event_ID);
                                            if (findNext != null)
                                                findNext.ForEach(findObj => { NextEventID.Add(findObj.AchieveID); });

                                            if (systemAchiveInfo.Reward_Box1ID > 0)
                                                setOpenBoxList.AddRange(TriggerManager.GetSystem_Achieve_Reward_Box_List(ref tb, systemAchiveInfo.Reward_Box1ID));
                                            if (systemAchiveInfo.Reward_Box2ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Warrior)
                                                setOpenBoxList.AddRange(TriggerManager.GetSystem_Achieve_Reward_Box_List(ref tb, systemAchiveInfo.Reward_Box2ID));
                                            if (systemAchiveInfo.Reward_Box3ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Swordmaster)
                                                setOpenBoxList.AddRange(TriggerManager.GetSystem_Achieve_Reward_Box_List(ref tb, systemAchiveInfo.Reward_Box3ID));
                                            if (systemAchiveInfo.Reward_Box4ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Taoist)
                                                setOpenBoxList.AddRange(TriggerManager.GetSystem_Achieve_Reward_Box_List(ref tb, systemAchiveInfo.Reward_Box4ID));

                                            if (systemAchiveInfo.Reward_EXP > 0)
                                                SetExp += systemAchiveInfo.Reward_EXP;
                                        }
                                        else if (systemPvPInfo != null)
                                        {
                                            var findNext = TriggerManager.GetSystem_Achieve_PvP_List(ref tb).FindAll(achieve => achieve.Require_AchieveID == userEventInfo.Event_ID);
                                            if (findNext != null)
                                                findNext.ForEach(findObj => { NextEventID.Add(findObj.AchieveID); });

                                            if (systemPvPInfo.Reward_Box1ID > 0)
                                                setOpenBoxList.AddRange(TriggerManager.GetSystem_Achieve_PvP_Reward_Box_List(ref tb, systemPvPInfo.Reward_Box1ID));
                                            if (systemPvPInfo.Reward_Box2ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Warrior)
                                                setOpenBoxList.AddRange(TriggerManager.GetSystem_Achieve_PvP_Reward_Box_List(ref tb, systemPvPInfo.Reward_Box2ID));
                                            if (systemPvPInfo.Reward_Box3ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Swordmaster)
                                                setOpenBoxList.AddRange(TriggerManager.GetSystem_Achieve_PvP_Reward_Box_List(ref tb, systemPvPInfo.Reward_Box3ID));
                                            if (systemPvPInfo.Reward_Box4ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Taoist)
                                                setOpenBoxList.AddRange(TriggerManager.GetSystem_Achieve_PvP_Reward_Box_List(ref tb, systemPvPInfo.Reward_Box4ID));

                                            if (systemPvPInfo.Reward_EXP > 0)
                                                SetExp += systemPvPInfo.Reward_EXP;
                                        }
                                        else
                                        {
                                            retError = Result_Define.eResult.TRIGGER_REWARD_NOT_FOUND;
                                        }
                                    }
                                    else
                                    {
                                        retError = Result_Define.eResult.TRIGGER_IS_NOT_CLEAR;
                                    }

                                    if (userEventInfo.Event_ID == Trigger_Define.CheckEvent_IgnoreRewardID)
                                    {
                                        retError = Result_Define.eResult.SUCCESS;
                                        checkIgnore = true;
                                    }

                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        retError = TriggerManager.UpdateEvent_Data(ref tb, AID, userEventInfo.Event_ID, userEventInfo.User_Event_ID, Trigger_Define.eClearType.Clear, "Y", setEventList, (Trigger_Define.eEventLoopType)userEventInfo.Event_LoopType);
                                        if (retError == Result_Define.eResult.SUCCESS && (Trigger_Define.eEventLoopType)userEventInfo.Event_LoopType == Trigger_Define.eEventLoopType.Repeat)
                                            isClear = false;
                                    }

                                    if (retError == Result_Define.eResult.SUCCESS)
                                        retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.Clear_Event, userEventInfo.Event_ID);

                                    if (setEventList == Trigger_Define.eEventListType.Event)
                                    {
                                        userEventInfo = TriggerManager.GetUser_Event_Data(ref tb, AID, user_event_idx, true);
                                    }
                                    else if (setEventList == Trigger_Define.eEventListType.Achive)
                                    {
                                        userEventInfo = TriggerManager.GetUser_Achieve_Data(ref tb, AID, user_event_idx, true);

                                        if (NextEventID.Count > 0)
                                        {
                                            foreach (long findID in NextEventID)
                                            {
                                                List<User_Event_Data> checkNextEventList = TriggerManager.Check_Achieve_Data_List(ref tb, AID, true).FindAll(item => item.Event_ID == findID);
                                                foreach (User_Event_Data chkObj in checkNextEventList)
                                                {
                                                    bool chkClear = false;
                                                    int chkMax1 = 0, chkMax2 = 0;
                                                    retNextEventList.Add(new Ret_Event_Data(TriggerManager.CheckClear(ref tb, AID, chkObj, out chkClear, out chkMax1, out chkMax2), chkMax1, chkMax2, chkClear));
                                                }
                                            }
                                        }
                                    }
                                    else if (setEventList == Trigger_Define.eEventListType.PvP_Achive)
                                    {
                                        userEventInfo = TriggerManager.GetUser_Achieve_PvP_Data(ref tb, AID, user_event_idx, true);

                                        if (NextEventID.Count > 0)
                                        {
                                            foreach (long findID in NextEventID)
                                            {
                                                List<User_Event_Data> checkNextEventList = TriggerManager.Check_Achieve_PvP_Data_List(ref tb, AID, true).FindAll(item => item.Event_ID == findID);
                                                foreach (User_Event_Data chkObj in checkNextEventList)
                                                {
                                                    bool chkClear = false;
                                                    int chkMax1 = 0, chkMax2 = 0;
                                                    retNextEventList.Add(new Ret_Event_Data(TriggerManager.CheckClear(ref tb, AID, chkObj, out chkClear, out chkMax1, out chkMax2), chkMax1, chkMax2, chkClear));
                                                }
                                            }
                                        }
                                    }

                                    //Ret_Event_Data retEvent = new Ret_Event_Data(userEventInfo, maxValue1, maxValue2, isClear);
                                    retEventList.Add( new Ret_Event_Data(userEventInfo, maxValue1, maxValue2, isClear) );
                                }
                            }

                            if (SetExp > 0)
                            {
                                Account userInfo = AccountManager.GetAccountData(ref tb, AID, ref retError);

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    retBefore = new RetBeforeInfo(charInfo.level, charInfo.exp, userInfo.Gold, userInfo.Cash + userInfo.EventCash,
                                                                userInfo.Key, userInfo.KeyFillMaxEA, userInfo.Ticket, userInfo.TicketFillMaxEA, userInfo.ChallengeTicket);

                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        int checkContents = 0;
                                        float bonusRate = AccountManager.CheckExpRate(ref tb, out checkContents);
                                        SystemData_Define.eContentsType targetContents = ((requestOp.Equals("get_achive_reward") || requestOp.Equals("get_achive_reward_all")) ? SystemData_Define.eContentsType.EVENT_ARCHIVE_REWARD :
                                                                                            (requestOp.Equals("get_event_reward") ? SystemData_Define.eContentsType.EVENT_EVENT_REWARD :                                                                                             
                                                                                                (requestOp.Equals("get_pvp_achive_reward") ? SystemData_Define.eContentsType.EVENT_PVP_ARCHIVE_REWARD : SystemData_Define.eContentsType.NONE)));
                                        if (bonusRate > 1.0f && TriggerManager.IsSetMask(checkContents, (int)targetContents))
                                            SetExp = (int)System.Math.Floor(SetExp * bonusRate);

                                        retError = CharacterManager.UpdateCharacterInfo(ref tb, CID, AID, SetExp);
                                    }

                                    //retError = CharacterManager.UpdateCharacterInfo(ref tb, CID, AID, SetExp);
                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        charInfo = CharacterManager.GetCharacter(ref tb, AID, CID, true);
                                        retBefore.levelup = retBefore.beforelevel < charInfo.level ? 1 : 0;
                                        charInfo.exp = retBefore.beforelevel == charInfo.level && charInfo.level == Character_Define.Max_CharacterLevel ? SetExp : charInfo.exp;
                                    }
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS && useRuby > 0)
                            {
                                retError = AccountManager.UseUserCash(ref tb, AID, useRuby);
                            }

                            List<User_Inven> makeRealItem = new List<User_Inven>();
                            if (retError == Result_Define.eResult.SUCCESS && setOpenBoxList.Count > 0)
                            {
                                User_VIP UserVIP = VipManager.GetUser_VIPInfo(ref tb, AID, true);
                                bool bMake = false;
                                foreach (System_Event_Reward_Box setReward in setOpenBoxList)
                                {
                                    if (UserVIP.viplevel >= setReward.VIP_Level)
                                    {
                                        List<User_Inven> makeItem = new List<User_Inven>();
                                        retError = ItemManager.MakeItem(ref tb, ref makeItem, AID, setReward.EventItem_ID, setReward.EventItem_Num, CID, setReward.EventItem_Level, setReward.EventItem_Grade);
                                        if (retError != Result_Define.eResult.SUCCESS)
                                            break;
                                        makeItem.ForEach(item => item.itemea = setReward.EventItem_Num);
                                        makeRealItem.AddRange(makeItem);
                                        bMake = true;
                                    }
                                }

                                if (makeRealItem.Count < 1 && bMake)
                                    retError = Result_Define.eResult.ITEM_CREATE_FAIL;
                                else
                                    retError = Result_Define.eResult.SUCCESS;
                            }
                            else if (retError == Result_Define.eResult.SUCCESS && !checkIgnore )
                                retError = Result_Define.eResult.TRIGGER_REWARD_EMPTY;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount);

                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CharacterInfo], mJsonSerializer.ToJsonString(charInfo));
                                    json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.BeforeInfo], mJsonSerializer.ToJsonString(retBefore));

                                    Ret_Event_Data retEvent = retEventList.FirstOrDefault();
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                                    json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.RewardInfo], mJsonSerializer.ToJsonString(retEvent));
                                    json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.RewardInfo_List], mJsonSerializer.ToJsonString(retEventList));
                                    json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.NextAchieveInfo], mJsonSerializer.ToJsonString(retNextEventList));
                                }
                            }
                        }
                        else if (requestOp.Equals("event_daily_count_buy") || requestOp.Equals("event_daily_count"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            short buy_count = queryFetcher.QueryParam_FetchByte("buy_count");
                            int DailyCheck_BuyMax = SystemData.AdminConstValueFetchFromRedis(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.DAILY_ADD_MAX]);

                            User_Event_Check_Data userDailyInfo = TriggerManager.Check_User_Daily_Event(ref tb, AID);

                            if (requestOp.Equals("event_daily_count_buy") && (userDailyInfo.AddCount + buy_count) > DailyCheck_BuyMax)
                                retError = Result_Define.eResult.TRIGGER_EVENT_DAILY_ADD_COUNT_OVER;
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            List<User_Inven> makeRealItem = new List<User_Inven>();

                            int admin_firstpayment_flag = SystemData.AdminConstValueFetchFromRedis(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.ADMIN_FIRST_PAYMENT_ON_OFF]);
                            int admin_daily_flag = SystemData.AdminConstValueFetchFromRedis(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.ADMIN_DAILY_ON_OFF]);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (admin_daily_flag > 0)
                                {
                                    //tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], TaskLogInfo.GetS_TaskID(userDailyInfo.CheckCount + SnailLog_Define.Snail_s_id_Seperator_daily_event, Trigger_Define.eEventListType.None));
                                    if (requestOp.Equals("event_daily_count"))
                                    {
                                        buy_count = 1;
                                        retError = TriggerManager.EventDailyRewardSend(ref tb, ref userDailyInfo, ref makeRealItem, AID, CID, buy_count);
                                    }
                                    else if (buy_count > 0)
                                    {
                                        int DailyCheck_RubyPrice = SystemData.AdminConstValueFetchFromRedis(ref tb, "DAILY_ADD_RUBY");
                                        retError = TriggerManager.EventDailyRewardSend(ref tb, ref userDailyInfo, ref makeRealItem, AID, CID, buy_count, true);

                                        if (retError == Result_Define.eResult.SUCCESS)
                                            retError = AccountManager.UseUserCash(ref tb, AID, DailyCheck_RubyPrice * buy_count);
                                    }
                                }
                                else
                                {
                                    retError = Result_Define.eResult.TRIGGER_EVENT_TYPE_EMPTY;
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                userDailyInfo = TriggerManager.Check_User_Daily_Event(ref tb, AID, true);
                                Ret_Event_Check_Data retDailyInfo = new Ret_Event_Check_Data(userDailyInfo, DailyCheck_BuyMax);
                                if (admin_firstpayment_flag < 1)
                                    retDailyInfo.first_pay_flag = "Y";

                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.DailyEventInfo], mJsonSerializer.ToJsonString(retDailyInfo));

                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount);
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                                }
                            }
                        }
                        else if (requestOp.Equals("get_first_pay_reward"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            User_Event_Check_Data userDailyInfo = TriggerManager.Check_User_Daily_Event(ref tb, AID);

                            List<User_Inven> makeRealItem = new List<User_Inven>();
                            retError = Result_Define.eResult.SUCCESS;
                            //if (userDailyInfo.FirstPaymentFlag.Equals("N"))
                            //{
                            //    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], TaskLogInfo.GetS_TaskID(SnailLog_Define.Snail_s_id_Seperator_firstpay_event, true));

                            //    List<User_Billing_List> userBuy = ShopManager.GetUserBillingInfo_All(ref tb, AID);

                            //    if (userBuy.Find(item => item.Billing_Status == (int)Shop_Define.eBillingStatus.Complete) != null)
                            //    {
                            //        retError = TriggerManager.EventFirstPaymentRewardSend(ref tb, ref makeRealItem, AID, CID);
                            //    }
                            //    else
                            //        retError = Result_Define.eResult.TRIGGER_EVENT_FIRSTPAY_NOT_FOUND;
                            //}
                            //else
                            //    retError = Result_Define.eResult.TRIGGER_EVENT_FIRSTPAY_ALREADY_GIVE;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                int DailyCheck_BuyMax = SystemData.AdminConstValueFetchFromRedis(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.DAILY_ADD_MAX]);

                                userDailyInfo = TriggerManager.Check_User_Daily_Event(ref tb, AID, true);
                                Ret_Event_Check_Data retDailyInfo = new Ret_Event_Check_Data(userDailyInfo, DailyCheck_BuyMax);
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.DailyEventInfo], mJsonSerializer.ToJsonString(retDailyInfo));

                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount);
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                                json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                            }
                        }
                        else if (requestOp.Equals("get_user_event_info"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            int DailyCheck_BuyMax = SystemData.AdminConstValueFetchFromRedis(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.DAILY_ADD_MAX]);

                            User_Event_Check_Data userDailyInfo = TriggerManager.Check_User_Daily_Event(ref tb, AID);
                            Ret_Event_Check_Data retDailyInfo = new Ret_Event_Check_Data(userDailyInfo, DailyCheck_BuyMax);
                            json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.DailyEventInfo], mJsonSerializer.ToJsonString(retDailyInfo));
                        }
                        else if (requestOp.Equals("event_7day_list"))
                        {
                            int loginCount = 0;
                            retError = AccountManager.GetUserLoginCount(ref tb, AID, out loginCount);

                            Account userInfo = AccountManager.GetAccountData(ref tb, AID, ref retError);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                int Event_Day_Limit = SystemData.GetConstValueInt(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.DEF_7DAY_EVENT_LIMIT_DAY]);

                                DateTime CheckDate = DateTime.Parse(userInfo.CreationDate.AddDays(Event_Day_Limit).ToShortDateString());
                                DateTime dbDate = DateTime.Now;

                                TimeSpan TS = CheckDate - dbDate;

                                Dictionary<long, List<Ret_Event_7Day_Data>> event_list = TriggerManager.Check_7Day_Data_List(ref tb, AID, CID, loginCount);
                                Dictionary<long, Ret7Day_PackageList> event_package_list = TriggerManager.Check_7Day_Package_List(ref tb, AID, CID);

                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.Event7Day_Count], loginCount.ToString());
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.Event7Day_LeftTime], ((long)TS.TotalSeconds).ToString());
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.Event7Day_List], mJsonSerializer.ToJsonString(event_list));
                                json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.Event7Day_Package_List], mJsonSerializer.ToJsonString(event_package_list));
                            }
                        }
                        else if (requestOp.Equals("get_event_7day_reward"))
                        {
                            long eventID = queryFetcher.QueryParam_FetchLong("event_id");

                            User_Event_7Day_Data userEventInfo = TriggerManager.GetUser_7Day_Event_Info(ref tb, AID, eventID);
                            retError = userEventInfo.ClearFlag.Equals("Y") && userEventInfo.RewardFlag.Equals("N") ? Result_Define.eResult.SUCCESS : Result_Define.eResult.TRIGGER_REWARD_NOT_FOUND;
                            List<Ret7DayReward> rewardList = new List<Ret7DayReward>();

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Character charInfo = CharacterManager.GetCharacter(ref tb, AID, CID);

                                System_Event_7Day systemEventInfo = TriggerManager.GetSystem_7Day_Event_Info(ref tb, userEventInfo.Event_ID);

                                if (systemEventInfo.Reward_Box1ID > 0)
                                    rewardList.AddRange(TriggerManager.Get7Day_Reward_List(ref tb, AID, CID, systemEventInfo.Reward_Box1ID));
                                if (systemEventInfo.Reward_Box2ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Warrior)
                                    rewardList.AddRange(TriggerManager.Get7Day_Reward_List(ref tb, AID, CID, systemEventInfo.Reward_Box2ID));
                                if (systemEventInfo.Reward_Box3ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Swordmaster)
                                    rewardList.AddRange(TriggerManager.Get7Day_Reward_List(ref tb, AID, CID, systemEventInfo.Reward_Box3ID));
                                if (systemEventInfo.Reward_Box4ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Taoist)
                                    rewardList.AddRange(TriggerManager.Get7Day_Reward_List(ref tb, AID, CID, systemEventInfo.Reward_Box4ID));
                            }

                            List<User_Inven> makeRealItem = new List<User_Inven>();
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                foreach (Ret7DayReward makeInfo in rewardList)
                                {
                                    List<User_Inven> makeItem = new List<User_Inven>();
                                    retError = ItemManager.MakeItem(ref tb, ref makeItem, AID, makeInfo.item_id, makeInfo.item_num, CID, makeInfo.item_level, makeInfo.item_grade);
                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        makeItem.ForEach(item => item.itemea = makeInfo.item_num);
                                        makeRealItem.AddRange(makeItem);
                                    }
                                    else
                                        break;
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (makeRealItem.Count < 1)
                                    retError = Result_Define.eResult.ITEM_CREATE_FAIL;
                                else
                                    retError = Result_Define.eResult.SUCCESS;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                userEventInfo.RewardFlag = "Y";
                                retError = TriggerManager.SetUser_7Day_Event_Info(ref tb, AID, userEventInfo);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount);

                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                                }
                            }
                        }
                        else if (requestOp.Equals("buy_event_7day_package"))
                        {
                            long packageID = queryFetcher.QueryParam_FetchLong("package_id");
                            User_Event_7Day_Data userEventInfo = TriggerManager.GetUser_7Day_Package_Info(ref tb, AID, packageID);

                            int loginCount = 0;
                            retError = AccountManager.GetUserLoginCount(ref tb, AID, out loginCount);

                            List<Ret7DayReward> rewardList = new List<Ret7DayReward>();

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Character charInfo = CharacterManager.GetCharacter(ref tb, AID, CID);

                                System_Event_7Day_Package_List systemEventInfo = TriggerManager.GetSystem_7Day_Event_Package_Info(ref tb, userEventInfo.ShopGoodsID);
                                retError = loginCount >= systemEventInfo.Buy_Day && userEventInfo.RewardFlag.Equals("N") ? Result_Define.eResult.SUCCESS : Result_Define.eResult.SHOP_ITEM_BUY_TIME_INVALIDE;

                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = ShopManager.PayBuyPrice(ref tb, AID, systemEventInfo.Buy_PriceValue, Item_Define.Item_BuyType_List[systemEventInfo.Buy_PriceType]);

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    if (systemEventInfo.Reward_Box1ID > 0)
                                        rewardList.AddRange(TriggerManager.Get7Day_Reward_List(ref tb, AID, CID, systemEventInfo.Reward_Box1ID));
                                    if (systemEventInfo.Reward_Box2ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Warrior)
                                        rewardList.AddRange(TriggerManager.Get7Day_Reward_List(ref tb, AID, CID, systemEventInfo.Reward_Box2ID));
                                    if (systemEventInfo.Reward_Box3ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Swordmaster)
                                        rewardList.AddRange(TriggerManager.Get7Day_Reward_List(ref tb, AID, CID, systemEventInfo.Reward_Box3ID));
                                    if (systemEventInfo.Reward_Box4ID > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Taoist)
                                        rewardList.AddRange(TriggerManager.Get7Day_Reward_List(ref tb, AID, CID, systemEventInfo.Reward_Box4ID));
                                }
                            }

                            List<User_Inven> makeRealItem = new List<User_Inven>();
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                foreach (Ret7DayReward makeInfo in rewardList)
                                {
                                    List<User_Inven> makeItem = new List<User_Inven>();
                                    retError = ItemManager.MakeItem(ref tb, ref makeItem, AID, makeInfo.item_id, makeInfo.item_num, CID, makeInfo.item_level, makeInfo.item_grade);
                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        makeItem.ForEach(item => item.itemea = makeInfo.item_num);
                                        makeRealItem.AddRange(makeItem);
                                    }
                                    else
                                        break;
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                userEventInfo.RewardFlag = "Y";
                                retError = TriggerManager.SetUser_7Day_Event_Info(ref tb, AID, userEventInfo);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount);

                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                                json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                            }
                        }
                        else if (requestOp.Equals("wechat_share"))
                        {
                            Trigger_Define.eTriggerType Trigger_type = Trigger_Define.eTriggerType.Wechat_Share;
                            retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_type);
                        }
                        else if (requestOp.Equals("fb_open"))
                        {
                            Trigger_Define.eTriggerType Trigger_type = Trigger_Define.eTriggerType.FaceBook_Open;
                            retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_type);
                        }
                        // for test operation function
                        else if (Request.Params.AllKeys.Contains("Debug"))
                        {
                            if (requestOp.Equals("trigger_progress"))
                            {
                                string Check_Trigger = queryFetcher.QueryParam_Fetch("trigger_type");
                                Trigger_Define.eTriggerType Trigger_type = Trigger_Define.TriggerType[Check_Trigger];
                                int User_Value1 = queryFetcher.QueryParam_FetchInt("user_value1");
                                int User_Value2 = queryFetcher.QueryParam_FetchInt("user_value2");
                                int User_Value3 = queryFetcher.QueryParam_FetchInt("user_value3");

                                retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_type, User_Value1, User_Value2, User_Value3);

                                //json = mJsonSerializer.AddJson(json, "updateEventList", mJsonSerializer.ToJsonString(updateEventList));
                                //json = mJsonSerializer.AddJson(json, "updateAchieveList", mJsonSerializer.ToJsonString(updateAchieveList));
                            }
                            else if (requestOp.Equals("isClear"))
                            {
                                int EventID = queryFetcher.QueryParam_FetchInt("event_id");
                                bool isEvent = queryFetcher.QueryParam_FetchInt("isevent") > 0;
                                bool isClear = false;

                                int maxValue1 = 0, maxValue2 = 0;
                                User_Event_Data setEvent = TriggerManager.CheckClear(ref tb, AID, EventID, isEvent, out isClear, out maxValue1, out maxValue2);

                                json = mJsonSerializer.AddJson(json, "isclear", mJsonSerializer.ToJsonString(setEvent));
                                //json = mJsonSerializer.AddJson(json, "updateAchieveList", mJsonSerializer.ToJsonString(updateAchieveList));
                            }
                            else if (requestOp.Equals("event_reward_reset"))
                            {
                                long user_event_idx = queryFetcher.QueryParam_FetchLong("user_event_idx");

                                string query = string.Format("UPDATE User_Event_Data SET RewardFlag = 'N' WHERE User_Event_ID = {0}", user_event_idx);
                                if (tb.ExcuteSqlCommand("sharding", query))
                                {
                                    retError = Result_Define.eResult.SUCCESS;
                                    TriggerManager.RemoveEventDataFromRedis(AID);
                                }

                                //json = mJsonSerializer.AddJson(json, "updateAchieveList", mJsonSerializer.ToJsonString(updateAchieveList));
                            }
                            else if (requestOp.Equals("event_check_reset"))
                            {

                                // hard coding for field name set
                                string setQuery = string.Format(@"UPDATE {0} SET  RegDate = '2015-01-01 00:00:00',
                                                                        CheckCount = '0',
                                                                        RewardCount = '0',
                                                                        FirstPaymentFlag = 'N'
                                                                    WHERE AID = {1} ",
                                                                        Trigger_Define.User_Event_Check_Data_TableName
                                                                        , AID
                                                                        );

                                if (tb.ExcuteSqlCommand("sharding", setQuery))
                                {
                                    retError = Result_Define.eResult.SUCCESS;
                                    TriggerManager.Check_User_Daily_Event(ref tb, AID, true);
                                }

                                //json = mJsonSerializer.AddJson(json, "updateAchieveList", mJsonSerializer.ToJsonString(updateAchieveList));
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
                    //if (AID == 3)
                    //{
                    //    string DBLog = mJsonSerializer.ToJsonString(queryFetcher.GetDBLog());
                    //    //error = mJsonSerializer.AddJson(error, "Message", mJsonSerializer.ToJsonString(errorEx.Message));
                    //    Response.Write(DBLog);
                    //}
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