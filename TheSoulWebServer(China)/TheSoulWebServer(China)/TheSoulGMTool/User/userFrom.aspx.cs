using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Tools;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using TheSoulGMTool.DBClass;

namespace TheSoulGMTool.User
{
    public partial class userFrom : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            
            string pUserName = queryFetcher.QueryParam_Fetch("username", "");
            string pSLevel = queryFetcher.QueryParam_Fetch("slevel", "");
            string pELevel = queryFetcher.QueryParam_Fetch("elevel", "");
            string pSDate = queryFetcher.QueryParam_Fetch("sdate", "");
            string pEDate = queryFetcher.QueryParam_Fetch("edate", "");
            string pPlatformid = queryFetcher.QueryParam_Fetch("platformid", "");
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                    tb.IsoLevel = IsolationLevel.ReadCommitted;
                    long AID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("aid", "0"));
                    useridx.Value = AID.ToString();
                    if (serverID > -1 && AID > 0)
                    {
                        if (!Page.IsPostBack)
                            pageInit(ref tb);
                        else
                        {
                            int viplevel = queryFetcher.QueryParam_FetchInt(vipLevel.UniqueID, -1);
                            int Pc1level = queryFetcher.QueryParam_FetchInt(Pc1Level.UniqueID, -1);
                            int reqCash = queryFetcher.QueryParam_FetchInt(cash.UniqueID);
                            int reqeventCash = queryFetcher.QueryParam_FetchInt(eventCash.UniqueID);
                            int reqGold = queryFetcher.QueryParam_FetchInt(gold.UniqueID);
                            int reqKey = queryFetcher.QueryParam_FetchInt(key.UniqueID);
                            int reqTicket = queryFetcher.QueryParam_FetchInt(ticket.UniqueID);
                            int reqBattle = queryFetcher.QueryParam_FetchInt(Battlepint.UniqueID);
                            int reqParty = queryFetcher.QueryParam_FetchInt(Partypoint.UniqueID);
                            int reqHonor = queryFetcher.QueryParam_FetchInt(Honorpoint.UniqueID);
                            int reqDonation = queryFetcher.QueryParam_FetchInt(Donationpoint.UniqueID);
                            int reqExpedition = queryFetcher.QueryParam_FetchInt(Expeditionpoint.UniqueID);
                            int reqBlackMarket = queryFetcher.QueryParam_FetchInt(BlackMarketPoint.UniqueID);
                            int reqOverload = queryFetcher.QueryParam_FetchInt(OverloadPoint.UniqueID);
                            int wordID = queryFetcher.QueryParam_FetchInt(wordid.UniqueID);
                            int missionID = queryFetcher.QueryParam_FetchInt(missionid.UniqueID);
                            string reqTutorial = queryFetcher.QueryParam_Fetch(tutorial.UniqueID, "");
                            int reqVipPoint = queryFetcher.QueryParam_FetchInt(vipPoint.UniqueID);
                            int reqStone = queryFetcher.QueryParam_FetchInt(stone.UniqueID);

                            Account userinfo = AccountManager.GetAccountData(ref tb, AID, true);
                            List<MoneyLogInfo> logList = new List<MoneyLogInfo>();

                            if (reqVipPoint > 0 || reqVipPoint < 0)
                                retError = VipManager.VIPPointAdd(ref tb, AID, reqVipPoint);

                            if (!string.IsNullOrEmpty(reqTutorial))
                            {
                                bool bEnd = System.Convert.ToInt32(reqTutorial) == 1 ? true : false;
                                retError = AccountManager.End_User_Tutorial(ref tb, AID, bEnd);
                            }

                            if (reqStone > 0)
                            {
                                retError = AccountManager.AddUserSoulStone(ref tb, AID, reqStone);
                            }
                            else if (reqStone < 0)
                            {
                                retError = AccountManager.UseUserSoulStone(ref tb, AID, (reqStone * -1));
                            }

                            if (reqCash > 0)
                            {
                                retError = AccountManager.AddUserCash(ref tb, AID, reqCash);
                                logList.Add(new MoneyLogInfo((int)SnailLog_Define.Snail_Money_type.ruby, (int)SnailLog_Define.Snail_Money_Event_type.add, reqCash, (userinfo.Cash + userinfo.EventCash), (userinfo.Cash + userinfo.EventCash) + reqCash));
                            }
                            else if (reqCash < 0)
                            {
                                retError = GMDataManager.UseUserCash(ref tb, AID, (reqCash * -1));
                                logList.Add(new MoneyLogInfo((int)SnailLog_Define.Snail_Money_type.ruby, (int)SnailLog_Define.Snail_Money_Event_type.use, (reqCash * -1), (userinfo.Cash + userinfo.EventCash), (userinfo.Cash + userinfo.EventCash) + reqeventCash));
                            }

                            if (reqeventCash > 0)
                            {
                                retError = AccountManager.AddUserEventCash(ref tb, AID, reqeventCash);
                                int beforCash = reqCash == 0 ? (userinfo.Cash + userinfo.EventCash) : (userinfo.Cash + userinfo.EventCash) + reqCash;
                                logList.Add(new MoneyLogInfo((int)SnailLog_Define.Snail_Money_type.ruby, (int)SnailLog_Define.Snail_Money_Event_type.add, reqeventCash, beforCash, beforCash + reqeventCash));
                            }
                            else if (reqeventCash < 0)
                            {
                                retError = GMDataManager.UseUserEventCash(ref tb, AID, (reqeventCash * -1));
                                int beforCash = reqCash == 0 ? (userinfo.Cash + userinfo.EventCash) : (userinfo.Cash + userinfo.EventCash) + reqCash;
                                logList.Add(new MoneyLogInfo((int)SnailLog_Define.Snail_Money_type.ruby, (int)SnailLog_Define.Snail_Money_Event_type.use, (reqeventCash * -1), beforCash, beforCash + reqeventCash));
                            }

                            if (reqGold > 0)
                            {
                                retError = AccountManager.AddUserGold(ref tb, AID, reqGold);
                                logList.Add(new MoneyLogInfo((int)SnailLog_Define.Snail_Money_type.gold, (int)SnailLog_Define.Snail_Money_Event_type.add, reqGold, userinfo.Gold, userinfo.Gold + reqGold));
                            }
                            else if (reqGold < 0)
                            {
                                retError = AccountManager.UseUserGold(ref tb, AID, (reqGold * -1));
                                logList.Add(new MoneyLogInfo((int)SnailLog_Define.Snail_Money_type.gold, (int)SnailLog_Define.Snail_Money_Event_type.use, (reqGold * -1), userinfo.Gold, userinfo.Gold + reqGold));
                            }


                            if (reqKey > 0)
                                retError = AccountManager.AddUserKey(ref tb, AID, reqKey);
                            else if(reqKey < 0)
                                retError = AccountManager.UseUserKey(ref tb, AID, (reqKey * -1));

                            if (reqTicket > 0)
                                retError = AccountManager.AddUserTicket(ref tb, AID, reqTicket);
                            else if (reqTicket < 0)
                                retError = AccountManager.UseUserTicket(ref tb, AID, (reqTicket * -1));

                            if (reqBattle > 0)
                                retError = AccountManager.AddUserCombatPoint(ref tb, AID, reqBattle);
                            else if (reqBattle < 0)
                                retError = AccountManager.UseUserCombatPoint(ref tb, AID, (reqBattle * -1));

                            if (reqParty > 0)
                                retError = AccountManager.AddUserPartyDungeonPoint(ref tb, AID, reqParty);
                            else if (reqParty < 0)
                                retError = AccountManager.UseUserPartyDungeonPoint(ref tb, AID, (reqParty * -1));

                            if (reqHonor > 0)
                                retError = AccountManager.AddUserHonor(ref tb, AID, reqHonor);
                            else if (reqHonor < 0)
                                retError = AccountManager.UseUserHonor(ref tb, AID, (reqHonor * -1));

                            if (reqDonation > 0)
                                retError = GuildManager.AddGuildContributionPoint(ref tb, AID, reqDonation);
                            else if (reqDonation < 0)
                                retError = GuildManager.UseGuildContributionPoint(ref tb, AID, (reqDonation * -1));

                            if (reqExpedition > 0)
                                retError = AccountManager.AddUserExpeditionPoint(ref tb, AID, reqExpedition);
                            else if (reqExpedition < 0)
                                retError = AccountManager.UseUserExpeditionPoint(ref tb, AID, (reqExpedition * -1));

                            if (reqBlackMarket > 0)
                                retError = AccountManager.AddUserBlackMarketPoint(ref tb, AID, reqBlackMarket);
                            else if (reqBlackMarket < 0)
                                retError = AccountManager.UseUserBlackMarketPoint(ref tb, AID, (reqBlackMarket * -1));

                            if (reqOverload > 0)
                                retError = AccountManager.AddUserOverlordRankingPoint(ref tb, AID, reqOverload);
                            else if (reqOverload < 0)
                                retError = AccountManager.UseUserOverlordRankingPoint(ref tb, AID, (reqOverload * -1));

                            if (viplevel > -1)
                            {
                                retError = GMDataManager.SetVIPLevel(ref tb, AID, viplevel);
                            }
                            
                            if (Pc1level > -1)
                            {
                                retError = GMDataManager.SetCharacterLevel(ref tb, AID, userinfo.EquipCID, Pc1level);
                            }

                            if(wordID > 0 && missionID > 0)
                                retError = GMDataManager.SetMissionDungeon(ref tb, AID, wordID, missionID);

                            if (retError == Result_Define.eResult.SUCCESS && logList.Count > 0)
                            {
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.write_money_log]);
                                tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.s_event_id], "GM_Edit");
                                SnailLogManager.SnailLog_write_money_log(ref tb, AID, ref logList);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                CharacterManager.FlushCharacterList(ref tb, AID);
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_USER, AID, userinfo.UserName, GMResult_Define.ControlType.USER_INFO_EDIT, queryFetcher.GetReqParams(), serverID.ToString());
                            }
                            retJson = queryFetcher.GM_Render("", retError);

                        }
                    }
                }
                catch (Exception errorEx)
                {
                    queryFetcher.DBLog("StackTrace" + mJsonSerializer.ToJsonString(errorEx.StackTrace));
                    queryFetcher.DBLog(errorEx.Message);
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                }
                finally
                {
                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    string gmid = "";
                    if (Request.Cookies.Count > 0)
                        gmid = GMDataManager.GetUserCookies("userid");
                    queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    tb.Dispose();
                }
                
                if (queryFetcher.Render_errorFlag)
                {
                    Response.Redirect("/User/userList.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID + "&platformid=" + pPlatformid + "&username=" + pUserName + "&slevel=" + pSLevel + "&elevel=" + pELevel + "&sdate=" + pSDate + "&edate=" + pEDate);
                }
            }
        }

        private void pageInit(ref TxnBlock TB)
        {
            List<System_VIP_Level> vipLevelList = GMDataManager.GetSystemVIPLevelList(ref TB);
            ListItem selectItem = new ListItem("select", "-1");
            vipLevel.DataSource = vipLevelList;
            vipLevel.DataTextField = "VIP_Level";
            vipLevel.DataValueField = "VIP_Level";
            vipLevel.DataBind();
            vipLevel.Items.Insert(0, selectItem);
            List<System_Character_EXP> pcLevelList = GMDataManager.GetCharacterLevelList(ref TB);
            Pc1Level.DataSource = pcLevelList;
            Pc1Level.DataTextField = "Level";
            Pc1Level.DataValueField = "Level";
            Pc1Level.DataBind();
            Pc1Level.Items.Insert(0, selectItem);

            int wordCount = Dungeon_Manager.GetSystem_MissionWorldList(ref TB).Count;
            int missionCount = 10;
            wordid.DataSource = GMDataManager.GetSelectBoxList(wordCount);
            wordid.DataBind();

            missionid.DataSource = GMDataManager.GetSelectBoxList(missionCount);
            missionid.DataBind();
                        
            long AID = System.Convert.ToInt64(useridx.Value);
            Account userInfo = AccountManager.GetAccountData(ref TB, AID, true);
            if (AID > 0)
            {
                User_VIP vipInfo = VipManager.GetUser_VIPInfo(ref TB, AID, true);
                Character charaterInfo = CharacterManager.GetCharacter(ref TB, AID, userInfo.EquipCID, true);
                labCash.Text = userInfo.Cash.ToString();
                labEventCash.Text = userInfo.EventCash.ToString();
                labGold.Text = userInfo.Gold.ToString();
                labExpeditionpoint.Text = userInfo.ExpeditionPoint.ToString();
                labHonorpoint.Text = userInfo.Honorpoint.ToString();
                labKey.Text = userInfo.Key.ToString();
                labPartypoint.Text = userInfo.PartyDungeonPoint.ToString();
                labDonationpoint.Text = userInfo.ContributionPoint.ToString();
                labTicket.Text = userInfo.Ticket.ToString();
                labVipLevel.Text = vipInfo.viplevel.ToString();
                labBlackMarketPoint.Text = userInfo.BlackMarketPoint.ToString();
                labBattlepoint.Text = userInfo.CombatPoint.ToString();
                labOverloadPoint.Text = userInfo.OverlordPoint.ToString();
                labPc1Level.Text = charaterInfo.level.ToString();
                labTutorial.Text = userInfo.Tutorial == 0 ? "On" : "Off";
                labVipPoint.Text = string.Format("{0} ({1})", vipInfo.vippoint, vipInfo.totalvippoint);
                lab_stone.Text = userInfo.Stone.ToString();

                int lastMissionID = Dungeon_Manager.GetUser_Mission_LastStage(ref TB, AID);
                if (lastMissionID == 0)
                    lastMissionID = 1;

                string missionName = Dungeon_Manager.GetSystem_MissionStageInfo(ref TB, lastMissionID).NamingCN.Replace("STRING_NAMING_SCENARIO_WORLD_", "").Replace("_STAGE", "");
                if (!string.IsNullOrEmpty(missionName))
                {
                    string[] arryMissionName = missionName.Split('_');
                    int wordID = System.Convert.ToInt32(arryMissionName[0]);
                    int missionID = System.Convert.ToInt32(arryMissionName[0]);
                    labMissionName.Text = wordID.ToString() + "-" + missionID.ToString();
                }

            }
        }
    }
}