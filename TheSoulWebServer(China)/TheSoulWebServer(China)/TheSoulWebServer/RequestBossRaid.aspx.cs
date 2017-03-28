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
using TheSoulWebServer.Tools;
using ServiceStack.Text;

namespace TheSoulWebServer
{
    public partial class RequestBossRaid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                "check",
                "activecheck",
                "list",
                "detail",
                "enter",
                "result",
                "reward",
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

                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.op], requestOp);
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], AID);

                        if (requestOp.Equals("check"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            int StageID = queryFetcher.QueryParam_FetchInt("stageid");

                            BossRaid setBossRaid = new BossRaid();
                            if (StageID > BossRaid_Define.BossRaid_Stage_Min)
                                setBossRaid.CreateBossRaid(ref tb, AID, StageID);
                            else
                                setBossRaid.ErrorCode = Result_Define.eResult.BOSSRAID_CREATE_RATE_CHECK_FAIL;

                            json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.DungeonID], setBossRaid.setDungeonID.ToString());
                            json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.RetJoinersCount], "0");
                            json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.SNO_List], "[]");
                            retError = setBossRaid.ErrorCode;
                        }
                        else if (requestOp.Equals("activecheck"))
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            bool setFlush = BossRaid.CheckPublicRaid(ref tb);
                            ActiveBossRaid_Info ActiveBossList = BossRaid.GetActiveBossRaid(ref tb, setFlush);
                            int ChatChannel = queryFetcher.QueryParam_FetchInt("chatchannel");
                            List<long> MyFriendAID_List = FriendManager.GetFriend_AID_List(ref tb, AID);
                            byte BossCount = 0;
                            MyFriendAID_List.Add(AID);

                            foreach (BossRaidCreation bossInfo in ActiveBossList.BossList)
                            {
                                if (MyFriendAID_List.Contains(bossInfo.CreaterAID) || (bossInfo.PublicChnnel > 0 && bossInfo.PublicChnnel == ChatChannel && bossInfo.PublicDate < ActiveBossList.CurrentDate))
                                {
                                    if (bossInfo.Status.Equals(BossRaid_Define.BossRaidStatus[BossRaid_Define.eRaidStatus.Active]) || bossInfo.Status.Equals(BossRaid_Define.BossRaidStatus[BossRaid_Define.eRaidStatus.Clear]))
                                    {
                                        BossCount = 1;
                                        break;
                                    }
                                }
                            }
                            json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.ActiveCount], BossCount.ToString());
                        }
                        else if (requestOp.Equals("list"))
                        {
                            int TAB = queryFetcher.QueryParam_FetchInt("tabidx");
                            int ChatChannel = queryFetcher.QueryParam_FetchInt("chatchannel", 1);

                            BossRaid_Define.eRaidStatus setTab = (BossRaid_Define.eRaidStatus)TAB;
                            retError = Result_Define.eResult.SUCCESS;
                            List<BossRaidJoinerList> SetBossList = new List<BossRaidJoinerList>();
                            List<long> MyFriendAID_List = FriendManager.GetFriend_AID_List(ref tb, AID);
                            MyFriendAID_List.Add(AID);
                            bool setFlush = BossRaid.CheckPublicRaid(ref tb);                            
                            ActiveBossRaid_Info ActiveBossList = BossRaid.GetActiveBossRaid(ref tb, setFlush);
                            int diffInSeconds = System.Convert.ToInt32((DateTime.Now - ActiveBossList.CurrentDate).TotalSeconds);

                            if (setTab == BossRaid_Define.eRaidStatus.Active)
                            {
                                //string setPublicOpen = "N";
                                foreach (BossRaidCreation bossInfo in ActiveBossList.BossList)
                                {
                                    if (MyFriendAID_List.Contains(bossInfo.CreaterAID) || (bossInfo.PublicChnnel > 0 && bossInfo.PublicChnnel == ChatChannel && bossInfo.PublicDate < ActiveBossList.CurrentDate) || (bossInfo.PublicChnnel == 0))
                                    {
                                        if (bossInfo.RemainSec - diffInSeconds > 0)
                                        {
                                            BossRaidJoinerList setJoinInfo = new BossRaidJoinerList();
                                            setJoinInfo.bossraidid = bossInfo.BossRaidID;
                                            setJoinInfo.createraid = bossInfo.CreaterAID;
                                            setJoinInfo.creaternick = bossInfo.CreaterNick;
                                            setJoinInfo.doreward = "N";
                                            setJoinInfo.dungeonid = bossInfo.DungeonID;
                                            setJoinInfo.getreward = "N";
                                            setJoinInfo.maxhp = bossInfo.HP;
                                            setJoinInfo.remainhp = System.Convert.ToInt64(bossInfo.RemainHP);
                                            setJoinInfo.remainsec = bossInfo.RemainSec - diffInSeconds;
                                            setJoinInfo.status = bossInfo.Status;
                                            SetBossList.Add(setJoinInfo);
                                        }
                                        //setPublicOpen = "Y";
                                    }
                                }
                            }
                            else if (setTab == BossRaid_Define.eRaidStatus.Clear)
                            {
                                List<BossRaidJoiner> RewardBossList = BossRaid.GetRewardBossRaid(ref tb, AID);
                                foreach (BossRaidJoiner bossInfo in RewardBossList)
                                {
                                    if (MyFriendAID_List.Contains(bossInfo.CreaterAID) || bossInfo.PublicChnnel > 0)
                                    {
                                        if (bossInfo.RemainSec - diffInSeconds > 0 && bossInfo.GetReward.Equals("N"))
                                        {
                                            BossRaidJoinerList setJoinInfo = new BossRaidJoinerList();
                                            setJoinInfo.bossraidid = bossInfo.BossRaidID;
                                            setJoinInfo.createraid = bossInfo.CreaterAID;
                                            setJoinInfo.creaternick = bossInfo.CreaterNick;
                                            setJoinInfo.doreward = bossInfo.DoReward;
                                            setJoinInfo.dungeonid = bossInfo.DungeonID;
                                            setJoinInfo.getreward = bossInfo.GetReward;
                                            setJoinInfo.maxhp = bossInfo.MaxHP;
                                            setJoinInfo.remainhp = System.Convert.ToInt64(bossInfo.RemainHP);
                                            setJoinInfo.remainsec = bossInfo.RemainSec - diffInSeconds;
                                            setJoinInfo.status = bossInfo.Status;
                                            SetBossList.Add(setJoinInfo);
                                        }
                                    }
                                }
                            }
                            else if (setTab == BossRaid_Define.eRaidStatus.Fail)
                            {
                                //List<BossRaidJoiner> RewardBossList = BossRaid.GetFailBossRaid(ref tb, AID);
                                //
                                //foreach (BossRaidJoiner bossInfo in RewardBossList)
                                foreach (BossRaidCreation bossInfo in ActiveBossList.BossList)
                                {
                                    if (bossInfo.BossDeadDate == null)
                                        bossInfo.BossDeadDate = bossInfo.ExpireDate;
                                    if (MyFriendAID_List.Contains(bossInfo.CreaterAID))
                                    {
                                        if (bossInfo.RemainSec - diffInSeconds <= 0 && bossInfo.RemainHP > 0)
                                        {
                                            BossRaidJoinerList setJoinInfo = new BossRaidJoinerList();
                                            setJoinInfo.bossraidid = bossInfo.BossRaidID;
                                            setJoinInfo.createraid = bossInfo.CreaterAID;
                                            setJoinInfo.creaternick = bossInfo.CreaterNick;
                                            setJoinInfo.doreward = bossInfo.DoReward;
                                            setJoinInfo.dungeonid = bossInfo.DungeonID;
                                            setJoinInfo.getreward = "N";
                                            setJoinInfo.maxhp = bossInfo.HP;
                                            setJoinInfo.remainhp = System.Convert.ToInt64(bossInfo.RemainHP);
                                            setJoinInfo.remainsec = bossInfo.RemainSec - diffInSeconds;
                                            setJoinInfo.status = (bossInfo.KillerAID != AID) ? BossRaid_Define.BossRaidStatus[BossRaid_Define.eRaidStatus.Fail] : BossRaid_Define.BossRaidStatus[BossRaid_Define.eRaidStatus.Clear];
                                            SetBossList.Add(setJoinInfo);
                                        }
                                    }
                                }
                            }

                            json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.BossList], mJsonSerializer.ToJsonString(SetBossList));
                            json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.BossRewardCount], BossRaid.GetBossRaidCount(ref tb, AID, BossRaid_Define.eRaidStatus.Clear, true).ToString());
                        }
                        else if (requestOp.Equals("detail"))
                        {
                            int RaidID = queryFetcher.QueryParam_FetchInt("raidid");
                            retError = Result_Define.eResult.SUCCESS;
                            int EnterCost = 0;
                            JsonArrayObjects setObjectArray = BossRaid.GetBossRaidDetailJsonObj(ref tb, AID, RaidID, out EnterCost);
                            json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.EnterCost], EnterCost.ToString());
                            json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.DetailInfo], setObjectArray.ToJson());
                        }
                        else if (requestOp.Equals("enter"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            int RaidID = queryFetcher.QueryParam_FetchInt("raidid");
                            int gamemoney = queryFetcher.QueryParam_FetchInt("gamemoney");
                            int Booster1 = queryFetcher.QueryParam_FetchInt("booster1");
                            int Booster2 = queryFetcher.QueryParam_FetchInt("booster2");
                            int Booster3 = queryFetcher.QueryParam_FetchInt("booster3");
                            Account UserInfo = AccountManager.GetAccountData(ref tb, AID, ref retError);                            
                            Character PlayCharacter = retError == Result_Define.eResult.SUCCESS ? CharacterManager.GetCharacter(ref tb, AID, UserInfo.EquipCID) : new Character();

                            if (UserInfo != null && PlayCharacter != null && retError == Result_Define.eResult.SUCCESS)
                            {
                                retError = Result_Define.eResult.SUCCESS;
                                List<long> MyFriendAID_List = FriendManager.GetFriend_AID_List(ref tb, AID);
                                MyFriendAID_List.Add(AID);

                                BossRaidDetail_Info DetailInfo = BossRaid.GetBossRaidDetail(ref tb, RaidID);

                                //BossRaidCreation DetailInfo = BossRaid.GetBossRaidInfo(ref tb, RaidID);

                                if (DetailInfo.CreaterInfo.BossRaidID < 1)
                                    retError = Result_Define.eResult.BOSSRAID_ID_NOT_FOUND;
                                else if (!MyFriendAID_List.Contains(DetailInfo.CreaterInfo.CreaterAID) && DetailInfo.CreaterInfo.PublicChnnel < 1)
                                    retError = Result_Define.eResult.BOSSRAID_CANT_JOIN_IS_NOT_PUBLIC_OR_NOT_IN_FRIEND_LIST;

                                System_BOSS_RAID getBossRaidInfo = BossRaid.GetBossInfo(ref tb, System.Convert.ToInt32(DetailInfo.CreaterInfo.DungeonID));

                                List<Return_DisassableItems_List> retDeletedItem = new List<Return_DisassableItems_List>();
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    System_Booster_Group missionBooster = Dungeon_Manager.GetSystem_BoosterGroup(ref tb, getBossRaidInfo.Booster_Group_ID);

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
                                }

                                if (SystemData.GetServiceArea(ref tb) == DataManager_Define.eCountryCode.China)
                                {
                                    if (retError == Result_Define.eResult.SUCCESS && getBossRaidInfo.Condition_PlayCoin > 0)
                                        retError = AccountManager.UseUserKey(ref tb, AID, getBossRaidInfo.Condition_PlayCoin) != Result_Define.eResult.SUCCESS ? Result_Define.eResult.BOSSRAID_NOT_ENOUGH_COST : Result_Define.eResult.SUCCESS;
                                }
                                else
                                {
                                    int EnterCost = 0;
                                    if (DetailInfo.CreaterInfo.CreaterAID == AID)
                                    {
                                        if (DetailInfo.JoinerInfo_List.Find(joiner => joiner.RaidJoinTime > 0 && AID == joiner.JoinerAID) != null)
                                            EnterCost = SystemData.GetConstValueInt(ref tb, BossRaid_Define.BossRaidConstKey[BossRaid_Define.eRaidConst.DEF_BOSSRAID_FINDER_ENTER]);
                                        else
                                            EnterCost = SystemData.GetConstValueInt(ref tb, BossRaid_Define.BossRaidConstKey[BossRaid_Define.eRaidConst.DEF_BOSSRAID_FINDER_FISRTENTER]);
                                    }
                                    else
                                        EnterCost = SystemData.GetConstValueInt(ref tb, BossRaid_Define.BossRaidConstKey[BossRaid_Define.eRaidConst.DEF_BOSSRAID_USER]);

                                    if (retError == Result_Define.eResult.SUCCESS && EnterCost > 0)
                                        retError = AccountManager.UseUserTicket(ref tb, AID, EnterCost)  != Result_Define.eResult.SUCCESS ? Result_Define.eResult.BOSSRAID_NOT_ENOUGH_COST : Result_Define.eResult.SUCCESS;
                                }

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 0);
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], ((int)DetailInfo.CreaterInfo.DungeonID + SnailLog_Define.Snail_s_id_Seperator_pve_boss).ToString());
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);

                                    UserInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref UserInfo, CharacterManager.GetCharacterCount_FromDB(ref tb, AID));

                                        json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));

                                        // make return json
                                        json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.BossRaidID], DetailInfo.CreaterInfo.BossRaidID.ToString());
                                        json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.RemainHP], DetailInfo.CreaterInfo.RemainHP.ToString());
                                        //json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.RetKeyFillMax], UserInfo.Key.ToString());
                                        //json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.RetHonorPoint], UserInfo.Honorpoint.ToString());
                                        //json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.RetGold], UserInfo.Gold.ToString());
                                        json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.DungeonID], DetailInfo.CreaterInfo.DungeonID.ToString());
                                        json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.DeletedItem], mJsonSerializer.ToJsonString(retDeletedItem));
                                    }
                                }
                            }
                            else
                                retError = Result_Define.eResult.ACCOUNT_ID_NOT_FOUND;
                        }
                        else if (requestOp.Equals("result"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            int RaidID = queryFetcher.QueryParam_FetchInt("raidid");
                            int Damage = queryFetcher.QueryParam_FetchInt("damage");
                            int PlayTime = queryFetcher.QueryParam_FetchInt("playtime");
                            List<UserMission_NPC_KILL> KillCount = mJsonSerializer.JsonToObject<List<UserMission_NPC_KILL>>(queryFetcher.QueryParam_Fetch("killcount", "[]"));
                            if (KillCount == null)
                                KillCount = new List<UserMission_NPC_KILL>();

                            Account userAccount = AccountManager.GetAccountData(ref tb, AID, ref retError);
                            long CID = userAccount.EquipCID;
                            Character charInfo = retError == Result_Define.eResult.SUCCESS ? CharacterManager.GetCharacter(ref tb, AID, CID) : new Character();

                            List<Client_Use_SkillInfo> useSkillList = mJsonSerializer.JsonToObject<List<Client_Use_SkillInfo>>(queryFetcher.QueryParam_Fetch("maxdmg", "[]"));
                            bool bVerify = false;
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Error_Skill_Info errSkill = new Error_Skill_Info();
                                bVerify = SoulManager.CheckSkillVerify(ref tb, AID, useSkillList, ref retError, out errSkill);
                                json = mJsonSerializer.AddJson(json, "errskill", mJsonSerializer.ToJsonString(errSkill));
                                if (!bVerify && retError == Result_Define.eResult.SUCCESS)
                                    retError = Result_Define.eResult.System_Hack_Detected;
                            }

                            int retArchiveID = 0;
                            string BossStatus = "E";
                            if (userAccount != null && charInfo != null && retError == Result_Define.eResult.SUCCESS)
                            {
                                List<long> MyFriendAID_List = FriendManager.GetFriend_AID_List(ref tb, AID);
                                MyFriendAID_List.Add(AID);
                                BossRaidCreation getBossInfo = BossRaid.GetBossRaidInfoFromDB(ref tb, RaidID);

                                if (getBossInfo.BossRaidID < 1)
                                    retError = Result_Define.eResult.BOSSRAID_ID_NOT_FOUND;
                                else if (!MyFriendAID_List.Contains(getBossInfo.CreaterAID) && getBossInfo.PublicChnnel < 1)
                                    retError = Result_Define.eResult.BOSSRAID_CANT_JOIN_IS_NOT_PUBLIC_OR_NOT_IN_FRIEND_LIST;
                                //else if (getBossInfo.Status != BossRaid_Define.BossRaidStatus[BossRaid_Define.eRaidStatus.Active])
                                //    retError = Result_Define.eResult.BOSSRAID_HAS_BEEN_CLOSED;
                                else
                                    retError = Result_Define.eResult.SUCCESS;

                                BossRaidJoiner setJoiner = new BossRaidJoiner();

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    setJoiner.BossRaidID = getBossInfo.BossRaidID;
                                    setJoiner.JoinerAID = userAccount.AID;
                                    setJoiner.JoinerNick = userAccount.UserName;
                                    setJoiner.JoinerClass = charInfo.Class;
                                    setJoiner.JoinerLevel = charInfo.level;
                                    setJoiner.RaidJoinTime = PlayTime;
                                    setJoiner.Damage = Damage;

                                    BossStatus = BossRaid.SetBossRaidResult(ref tb, setJoiner);
                                    Guild userGuildInfo = GuildManager.GetGuildInfo(ref tb, AID);
                                    if (userGuildInfo.guild_id > 0)
                                        retError = GuildManager.AddGuildPoint(ref tb, userGuildInfo.guild_id, AID, Guild_Define.AddGuildPoint_List[Guild_Define.ePlayType.BOSSRAID]);
                                    //retArchiveID = AccountManager.CalulateAchievement(ref tb, UserInfo.AID, Result_Define.eArchiveSubType.BOSSRAID);
                                    retArchiveID = 0;
                                                                        
                                    if (BossStatus.Equals(BossRaid_Define.BossRaidStatus[BossRaid_Define.eRaidStatus.Error]))
                                        retError = Result_Define.eResult.DB_STOREDPROCEDURE_ERROR;
                                }

                                List<TriggerProgressData> setTriggerList = new List<TriggerProgressData>();

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    foreach (UserMission_NPC_KILL setKill in KillCount)
                                    {
                                        setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Kill_NPC, setKill.grade, 0, setKill.count));
                                    }
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_PVE, (int)Trigger_Define.ePvEType.Bossraid));
                                    setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Play_Bossraid, getBossInfo.DungeonID));

                                    if (getBossInfo.HP > 0 && getBossInfo.HP <= setJoiner.Damage)       // isClear
                                    {
                                        setTriggerList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Clear_PVE, (int)Trigger_Define.ePvEType.Bossraid));
                                    }
                                }
                                int SetExp = 0;
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    List<User_Event_Data> userEventList = TriggerManager.Check_Event_Data_List(ref tb, AID);
                                    List<User_Event_Data> userAchieveList = TriggerManager.Check_Achieve_Data_List(ref tb, AID);
                                    List<User_Event_Data> userAchievePvPList = TriggerManager.Check_Achieve_PvP_Data_List(ref tb, AID);

                                    retError = TriggerManager.ProgressTrigger(ref tb, ref userEventList, ref userAchieveList, ref userAchievePvPList, AID, setTriggerList);
                                }

                                RetBeforeInfo retBefore = new RetBeforeInfo(charInfo.level, charInfo.exp, userAccount.Gold, userAccount.Cash + userAccount.EventCash,
                                                                            userAccount.Key, userAccount.KeyFillMaxEA, userAccount.Ticket, userAccount.TicketFillMaxEA, userAccount.ChallengeTicket);

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    System_BOSS_RAID getBossSystemInfo = BossRaid.GetBossInfo(ref tb, System.Convert.ToInt32(getBossInfo.DungeonID));
                                    if (getBossSystemInfo.Base_Reward_EXP > 0)
                                    {
                                        int checkContents = 0;
                                        float bonusRate = AccountManager.CheckExpRate(ref tb, out checkContents);

                                        if (bonusRate > 1.0f && TriggerManager.IsSetMask(checkContents, (int)SystemData_Define.eContentsType.PVE_BOSSRAID))
                                            SetExp = (int)System.Math.Floor(getBossSystemInfo.Base_Reward_EXP * bonusRate);
                                        else
                                            SetExp = getBossSystemInfo.Base_Reward_EXP;

                                        retError = CharacterManager.UpdateCharacterInfo(ref tb, CID, AID, SetExp);
                                    }
                                }

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_instance_log]);
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_scene_id], ((int)getBossInfo.DungeonID + SnailLog_Define.Snail_s_id_Seperator_pve_boss).ToString());
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_act_id], ((int)getBossInfo.DungeonID + SnailLog_Define.Snail_s_id_Seperator_pve_boss).ToString());
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_duration], PlayTime);
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.n_act_type], 1);
                                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_game_player_action_log]);

                                        charInfo = SetExp > 0 ? CharacterManager.GetCharacter(ref tb, AID, CID) : charInfo;                                        
                                        retBefore.levelup = retBefore.beforelevel < charInfo.level ? 1 : 0;
                                        charInfo.exp = retBefore.beforelevel == charInfo.level && charInfo.level == Character_Define.Max_CharacterLevel ? SetExp : charInfo.exp;
                                        Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount, CharacterManager.GetCharacterCount_FromDB(ref tb, AID));

                                        int EnterCost = 0;
                                        JsonArrayObjects setObjectArray = BossRaid.GetBossRaidDetailJsonObj(ref tb, AID, RaidID, out EnterCost);

                                        json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                                        json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.CharacterInfo], mJsonSerializer.ToJsonString(charInfo));
                                        json = mJsonSerializer.AddJson(json, Dungeon_Define.Dungeon_Ret_KeyList[Dungeon_Define.eDungeonReturnKeys.BeforeInfo], mJsonSerializer.ToJsonString(retBefore));

                                        json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.PopupAchiveID], retArchiveID.ToString());
                                        json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.DetailInfo], setObjectArray.ToJson());
                                        json = mJsonSerializer.AddJson(json, BossRaid_Define.BossRaid_Ret_KeyList[BossRaid_Define.eRaidReturnKeys.Status], BossStatus);
                                    }
                                }
                            }
                            else if(retError == Result_Define.eResult.SUCCESS)
                                retError = Result_Define.eResult.ACCOUNT_ID_NOT_FOUND;
                        }
                        else if (requestOp.Equals("reward"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            int CID = queryFetcher.QueryParam_FetchInt("cid");
                            long BossRaidID = queryFetcher.QueryParam_FetchInt("raidid");

                            Character PlayCharacter = CharacterManager.GetCharacter(ref tb, AID, CID);
                            BossRaidJoiner getJoinerInfo = BossRaid.GetBossRaidJoinerInfo(ref tb, AID, BossRaidID);

                            if (VipManager.CheckVIPCountOver(ref tb, AID, CID, VIP_Define.eVipType.BAGSLOT_MAX_ITEM))
                                retError = Result_Define.eResult.SUCCESS;
                            else
                                retError = Result_Define.eResult.ITEM_INVENTORY_OVER;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (getJoinerInfo.GetReward != "N")
                                    retError = Result_Define.eResult.BOSSRAID_REWARD_ALREADY_GIVE;
                                if (getJoinerInfo.DoReward != "Y")
                                    retError = Result_Define.eResult.BOSSRAID_REWARD_ALREADY_GIVE;
                            }

                            List<User_Inven> makeRealItem = new List<User_Inven>();
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                BossRaidCreationInfo getBossRaidInfo = BossRaid.GetBossRaidInfoWithDamage(ref tb, BossRaidID);
                                System_BOSS_RAID getBossSystemInfo = BossRaid.GetBossInfo(ref tb, System.Convert.ToInt32(getBossRaidInfo.DungeonID));

                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    List<long> GetRewardBox = new List<long>();
                                    GetRewardBox.Add(getBossSystemInfo.Base_RandBox_DropBoxGroupId);

                                    if (AID == getJoinerInfo.CreaterAID || AID == getBossRaidInfo.MaxDamageAID) // || AID == getJoinerInfo.KillerAID )
                                        GetRewardBox.Add(getBossSystemInfo.Add_RandBox_DropBoxGroupId);

                                    foreach (long dropID in GetRewardBox)
                                    {
                                        if (getBossRaidInfo.Status == BossRaid_Define.BossRaidStatus[BossRaid_Define.eRaidStatus.Clear] && retError == Result_Define.eResult.SUCCESS)
                                        {
                                            List<System_Drop_Group> getDropList = DropManager.GetDropResult(ref tb, AID, dropID, (short)PlayCharacter.Class);

                                            foreach (System_Drop_Group setDrop in getDropList)
                                            {
                                                List<User_Inven> getItem = new List<User_Inven>();
                                                retError = DropManager.MakeDropItem(ref tb, ref getItem, setDrop, AID, CID);
                                                makeRealItem.AddRange(getItem);
                                            }

                                            if (makeRealItem.Count < 1)
                                                retError = Result_Define.eResult.BOSSRAID_REWARD_CREATE_FAIL;
                                        }
                                    }
                                }
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = BossRaid.UpdateBossRaidReward(ref tb, AID, BossRaidID);

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.Reward_Acquire_Bossraid);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount, CharacterManager.GetCharacterCount_FromDB(ref tb, AID));

                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
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