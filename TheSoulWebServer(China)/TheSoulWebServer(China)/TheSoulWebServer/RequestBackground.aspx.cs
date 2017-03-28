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
    public partial class RequestBackground : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                // 계정 정보 갱신
                "check_refresh",
                "townuser_list",
                "tutorial_set",
                "tutorial_reward",
                "tutorial_end",
                "check_day_count",
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
                        tb.IsoLevel = IsolationLevel.ReadCommitted;

                        Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                        queryFetcher.operation = requestOp;
                        long CID = queryFetcher.QueryParam_FetchLong("cid");

                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.op], requestOp);
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], AID);

                        if (requestOp.Equals("check_refresh"))  // check all contents new flag
                        {
                            retError = Result_Define.eResult.SUCCESS;

                            int ChatChannel = queryFetcher.QueryParam_FetchInt("chatchannel", 0);
                            int LoginTime = queryFetcher.QueryParam_FetchInt("logintime", 0);     // time minute
                            int NoticeTime = queryFetcher.QueryParam_FetchInt("noticetime", 0);     // time minute

                            Dictionary<long, Friends> reqFriendList = new Dictionary<long, Friends>();
                            Account userInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = AccountManager.UpdateLastConnectionTime(ref tb, AID);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                if (LoginTime > 0)
                                {
                                    TriggerManager.ProgressTrigger(ref tb, AID, Trigger_Define.eTriggerType.Game_Access, (int)Trigger_Define.eGameAccessType.AccumulateLoginTime, 0, LoginTime);
                                    tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.update_role_log], LoginTime);
                                }

                                if (NoticeTime > 0)
                                {
                                    List<Admin_Notice> noticeList = SystemData.GetAdminNotice(ref tb);
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.AdminNotice], mJsonSerializer.ToJsonString(RetNotice.GetRetNoticeList(ref noticeList)));
                                }

                                BestGachaInfo setBestGachaInfo = new BestGachaInfo();
                                byte bestGachaOnOff = (byte)SystemData.AdminConstValueFetchFromRedis(ref tb, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.ADMIN_GACHA_BEST_ON_OFF]);
                                int bestGachaOpenLevel = (int)SystemData.GetConstValueInt(ref tb, Shop_Define.Shop_Const_Def_Key_List[Shop_Define.eShopConstDef.DEF_GACHA_OPENLEVEL_SPECIAL]);
                                if (bestGachaOnOff == 1)
                                {
                                    if (CharacterManager.GetCharacterMaxLevel_FromDB(ref tb, AID) >= bestGachaOpenLevel)
                                    {
                                        System_Gacha_Best bestGachaInfo = ShopManager.GetSystem_Shop_Gacha_Best(ref tb);
                                        if (bestGachaInfo.GachaIndex > 0)
                                        {
                                            setBestGachaInfo.gacha_cash = bestGachaInfo.Gacha_Cash;
                                            setBestGachaInfo.soulid_list.Add(bestGachaInfo.Main_SoulItemID > 0 ? bestGachaInfo.Main_SoulItemID : 0);
                                            setBestGachaInfo.soulid_list.Add(bestGachaInfo.Sub_SoulItemID_1 > 0 ? bestGachaInfo.Sub_SoulItemID_1 : 0);
                                            setBestGachaInfo.soulid_list.Add(bestGachaInfo.Sub_SoulItemID_2 > 0 ? bestGachaInfo.Sub_SoulItemID_2 : 0);
                                            setBestGachaInfo.soulid_list.Add(bestGachaInfo.Sub_SoulItemID_3 > 0 ? bestGachaInfo.Sub_SoulItemID_3 : 0);
                                        }
                                    }
                                }

                                RefreshNewFlag setUserRefresh = AccountManager.CheckUserNewFlag(ref tb, ref userInfo, ChatChannel, ref reqFriendList);

                                List<ServerCreate_RankingReward> rewardList = PvPManager.GetRankRewardList(ref tb, AID);
                                if (rewardList.Count > 0) PvPManager.SetPvP_RewardMail(ref tb, ref rewardList);

                                Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userInfo);
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.NewFlags], mJsonSerializer.ToJsonString(setUserRefresh));
                                json = mJsonSerializer.AddJson(json, Friend_Define.Friend_Ret_KeyList[Friend_Define.eFriendReturnKeys.ReqFriendList], mJsonSerializer.ToJsonString(reqFriendList.Values.ToList()));
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.BestGachaInfo], mJsonSerializer.ToJsonString(setBestGachaInfo));
                                
                                long currentTime = (long)(DateTime.Now - DateTime.Parse(DateTime.Now.ToShortDateString())).TotalSeconds;
                                json = mJsonSerializer.AddJson(json, PvP_Define.PvP_Ret_KeyList[PvP_Define.ePvPReturnKeys.PvP_ServerTime], mJsonSerializer.ToJsonString(currentTime));
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.ServerTimeString], GenericFetch.GetServerTimeString());

                                int loginCount = 0;
                                retError = AccountManager.GetUserLoginCount(ref tb, AID, out loginCount, true);
                                json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.User_Login_Day_Count], loginCount.ToString());
                            }
                        }
                        else if (requestOp.Equals("townuser_list"))  // townuser request
                        {
                            retError = Result_Define.eResult.SUCCESS;
                            List<Account_TownSimple> townUser = AccountManager.GetSimpleAccount_TownInfo(ref tb, AID);
                            json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.TownUserInfo], Account_TownSimple.makeAccount_TownSimpleListJson(ref townUser));
                        }
                        else if (requestOp.Equals("tutorial_set"))  // tutoral step set by client
                        {
                            long tutorialStep = queryFetcher.QueryParam_FetchLong("step", 0);
                            
                            System_Tutorial_Step tutorialInfo = AccountManager.GetSystemTutorial(ref tb, tutorialStep);
                            retError = Result_Define.eResult.SUCCESS;
                            
                            Tutorial_Step userTutorialStep = AccountManager.Get_User_Tutorial(ref tb, AID);
                            //json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.NextTutorialStep], mJsonSerializer.ToJsonString(tutorialStep));

                            List<System_Tutorial_Reward> tutorialRewardList = new List<System_Tutorial_Reward>();
                            bool bMakeNow = false;
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Character charInfo = CharacterManager.GetCharacter(ref tb, AID, CID);
                                if (charInfo.cid > 0)
                                {
                                    if (tutorialInfo.BoxID_all > 0)
                                        tutorialRewardList.AddRange(AccountManager.GetSystem_Tutorial_RewardBox(ref tb, tutorialInfo.BoxID_all));
                                    if (tutorialInfo.BoxID1 > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Warrior)
                                        tutorialRewardList.AddRange(AccountManager.GetSystem_Tutorial_RewardBox(ref tb, tutorialInfo.BoxID1));
                                    if (tutorialInfo.BoxID2 > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Swordmaster)
                                        tutorialRewardList.AddRange(AccountManager.GetSystem_Tutorial_RewardBox(ref tb, tutorialInfo.BoxID2));
                                    if (tutorialInfo.BoxID3 > 0 && charInfo.Class == (short)Character_Define.SystemClassType.Class_Taoist)
                                        tutorialRewardList.AddRange(AccountManager.GetSystem_Tutorial_RewardBox(ref tb, tutorialInfo.BoxID3));

                                    bMakeNow = tutorialRewardList.Count > 0;
                                }
                                else
                                    retError = Result_Define.eResult.CHARACTER_NOT_FOUND;
                            }

                            List<User_Inven> makeRealItem = new List<User_Inven>();
                            if (bMakeNow && retError == Result_Define.eResult.SUCCESS)
                            {
                                foreach (System_Tutorial_Reward makeInfo in tutorialRewardList)
                                {
                                    List<User_Inven> makeItem = new List<User_Inven>();
                                    retError = ItemManager.MakeItem(ref tb, ref makeItem, AID, makeInfo.Item_ID, makeInfo.Item_Num, CID, makeInfo.Item_Level, makeInfo.Item_Grade);

                                    if (retError != Result_Define.eResult.SUCCESS)
                                        break;
                                    makeItem.ForEach(item => item.itemea = makeInfo.Item_Num);
                                    makeRealItem.AddRange(makeItem);                                    
                                }
                            }

                            if (bMakeNow && retError == Result_Define.eResult.SUCCESS)
                            {
                                if (makeRealItem.Count < 1)
                                    retError = Result_Define.eResult.ITEM_CREATE_FAIL;
                                else
                                    retError = Result_Define.eResult.SUCCESS;
                            }

                            if (tutorialInfo.ServerSave > 0 && retError == Result_Define.eResult.SUCCESS)
                            {
                                bool bUpdate = false;
                                if (tutorialInfo.Type == (int)Account_Define.eTutorialType.ForcedTutorial && userTutorialStep.forced_tutorial < tutorialInfo.ServerSave)
                                {
                                    userTutorialStep.forced_tutorial = tutorialInfo.ServerSave;
                                    bUpdate = true;
                                }
                                else if (tutorialInfo.Type == (int)Account_Define.eTutorialType.ConditionalTutorial)
                                {
                                    if (!userTutorialStep.conditional_tutorial.Contains(tutorialInfo.ServerSave))
                                    {
                                        userTutorialStep.conditional_tutorial.Add(tutorialInfo.ServerSave);
                                        bUpdate = true;
                                    }
                                }

                                if(bUpdate)
                                    retError = AccountManager.Set_User_Tutorial(ref tb, AID, userTutorialStep);
                            }
                                

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount, 0);
                                    // user tutorial info
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.TutorialStepInfo], mJsonSerializer.ToJsonString(userTutorialStep));

                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                                }
                            }
                        }
                        else if (requestOp.Equals("tutorial_reward"))  // request tutoral reward item by client
                        {
                            long reward_Id = queryFetcher.QueryParam_FetchLong("reward_id", 0);
                            List<System_Tutorial_Reward> tutorialRewardList = new List<System_Tutorial_Reward>();
                            tutorialRewardList.AddRange(AccountManager.GetSystem_Tutorial_RewardBox(ref tb, reward_Id));

                            List<User_Inven> makeRealItem = new List<User_Inven>();
                            foreach (System_Tutorial_Reward makeInfo in tutorialRewardList)
                            {
                                List<User_Inven> makeItem = new List<User_Inven>();
                                retError = ItemManager.MakeItem(ref tb, ref makeItem, AID, makeInfo.Item_ID, makeInfo.Item_Num, CID, makeInfo.Item_Level, makeInfo.Item_Grade);

                                if (retError != Result_Define.eResult.SUCCESS)
                                    break;
                                makeItem.ForEach(item => item.itemea = makeInfo.Item_Num);
                                makeRealItem.AddRange(makeItem);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                Account userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    Ret_Login_Info retAccount = AccountManager.SetRetLoginData(ref tb, ref userAccount, 0);
                                    json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.Account], mJsonSerializer.ToJsonString(retAccount));
                                    json = mJsonSerializer.AddJson(json, Item_Define.Item_Ret_KeyList[Item_Define.eItemReturnKeys.GetItemList], mJsonSerializer.ToJsonString(makeRealItem));
                                }
                            }
                        }
                        else if (requestOp.Equals("tutorial_end"))  // tutorial_end set
                        {
                            retError = AccountManager.End_User_Tutorial(ref tb, AID);
                        }
                        else if (requestOp.Equals("check_day_count"))  // check_day_count for client day over check
                        {
                            int clientLoginCount = queryFetcher.QueryParam_FetchInt("login_count", 0);
                            int loginCount = 0;
                            retError = AccountManager.GetUserLoginCount(ref tb, AID, out loginCount, true);

                            if (clientLoginCount < loginCount && clientLoginCount > 0)
                            {
                                List<TriggerProgressData> setDataList = new List<TriggerProgressData>();
                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.CHARGE_FIXED));
                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Game_Access, (int)Trigger_Define.eGameAccessType.TotalLoginCount));
                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Game_Access, (int)Trigger_Define.eGameAccessType.AccumulateLoginDay));
                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Game_Access, (int)Trigger_Define.eGameAccessType.CountinueLoginDay));
                                setDataList.Add(new TriggerProgressData(Trigger_Define.eTriggerType.Game_Access, (int)Trigger_Define.eGameAccessType.CountLoginDay));
                                TriggerManager.ProgressTrigger(ref tb, AID, setDataList);
                            }

                            Account userAccount = new Account();
                            if (retError == Result_Define.eResult.SUCCESS)
                                userAccount = AccountManager.FlushAccountData(ref tb, AID, ref retError);

                            int admin_7Day_flag = TriggerManager.Check7DayEvent(ref tb, AID, userAccount.CreationDate);
                            int admin_firstpayment_flag = SystemData.AdminConstValueFetchFromRedis(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.ADMIN_FIRST_PAYMENT_ON_OFF]);
                            int admin_daily_flag = SystemData.AdminConstValueFetchFromRedis(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.ADMIN_DAILY_ON_OFF]);

                            Ret_Daily_Event_List dailyEventList = new Ret_Daily_Event_List();
                            User_Event_Check_Data userDailyInfo = new User_Event_Check_Data(AID);
                            int DailyCheck_RubyPrice = 0;
                            int DailyCheck_BuyMax = 0;

                            if (admin_daily_flag > 0)
                            {
                                dailyEventList = TriggerManager.GetDailyEvent_RewardList(ref tb);
                                userDailyInfo = TriggerManager.Check_User_Daily_Event(ref tb, AID, true);
                                DailyCheck_RubyPrice = SystemData.AdminConstValueFetchFromRedis(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.DAILY_ADD_RUBY]);
                                DailyCheck_BuyMax = SystemData.AdminConstValueFetchFromRedis(ref tb, Account_Define.Account_Const_Def_Key_List[Account_Define.eAccountConstDef.DAILY_ADD_MAX]);
                            }

                            Ret_Event_Check_Data retDailyInfo = new Ret_Event_Check_Data(userDailyInfo, DailyCheck_BuyMax);

                            List<ServerCreate_RankingReward> rewardList = PvPManager.GetRankRewardList(ref tb, AID);
                            if (rewardList.Count > 0)
                                retError = PvPManager.SetPvP_RewardMail(ref tb, ref rewardList);

                            json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.DailyEventInfo], mJsonSerializer.ToJsonString(retDailyInfo));
                            json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.DailyCheckBuyRubyPrice], mJsonSerializer.ToJsonString(DailyCheck_RubyPrice));
                            json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.DailyCheckBuyMax], mJsonSerializer.ToJsonString(DailyCheck_BuyMax));
                            json = mJsonSerializer.AddJson(json, Trigger_Define.Trigger_Ret_KeyList[Trigger_Define.eTriggerReturnKeys.DailyEventList], mJsonSerializer.ToJsonString(dailyEventList));
                            // user 7day event info
                            json = mJsonSerializer.AddJson(json, Account_Define.Account_Ret_KeyList[Account_Define.eAccountReturnKeys.User_Login_Day_Count], loginCount.ToString());
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