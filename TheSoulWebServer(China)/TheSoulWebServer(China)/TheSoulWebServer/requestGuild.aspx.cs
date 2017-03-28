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
    public partial class requestGuild : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] ops = new string[] {
                "guild_create",
                "guild_name_check",
                "guild_info",
                "guild_managed",
                "guild_join",
                "guild_recommend",
                "guild_recommend_refresh",
                "guild_joiner_state_change",
                "guild_state_change",
                "guild_entrust_ask",
                "guild_entrust_check",
                "guild_entrust_reply",
                "guild_daily_mission_reward"
            };

            WebQueryParam queryFetcher = new WebQueryParam();
            string retJson = "";

            TxnBlock tb = new TxnBlock();
            {
                long AID = 0;
                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
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
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.op], requestOp);
                        tb.SetLogData(SnailLog_Define.SetLogKey[SnailLog_Define.SnailLogKey.aid], AID);

                        if (requestOp.Equals("guild_create"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            string guildname = queryFetcher.QueryParam_Fetch("guildname");
                            int markindex = queryFetcher.QueryParam_FetchInt("markindex");
                            short guildtype = queryFetcher.QueryParam_FetchShort("guildtype");

                            if (markindex == 0)
                                retError = Result_Define.eResult.NO_GUILD_MARK;
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            bool check = GuildManager.GuildNameCheck(ref tb, guildname);
                            if (check)
                            {
                                retError = Result_Define.eResult.ALREADY_EXIST_NICKNAME;
                            }
                            else
                            {
                                retError = Result_Define.eResult.SUCCESS;
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                long GuildID = 0;
                                retError = GuildManager.CreateGuild(ref tb, ref GuildID, AID, guildname, markindex, guildtype);
                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = AccountManager.UpdateGuildID(ref tb, AID, GuildID);
                                int gold = 0;
                                int cash = 0;
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    Account userInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                    gold = userInfo.Gold;
                                    cash = (userInfo.Cash + userInfo.EventCash);
                                }
                                if (retError == Result_Define.eResult.SUCCESS)
                                {                                    
                                    json = mJsonSerializer.AddJson(json, "guildid", GuildID.ToString());
                                    json = mJsonSerializer.AddJson(json, "gold", gold.ToString());
                                    json = mJsonSerializer.AddJson(json, "ruby", cash.ToString());
                                }
                            }
                        }
                        else if (requestOp.Equals("guild_name_check"))
                        {
                            string guildname = queryFetcher.QueryParam_Fetch("guildname");
                            if (string.IsNullOrEmpty(guildname))
                            {
                                retError = Result_Define.eResult.CANT_USE_NICKNAME;
                            }
                            else
                            {
                                bool check = GuildManager.GuildNameCheck(ref tb, guildname);
                                retError = check ? Result_Define.eResult.ALREADY_EXIST_NICKNAME : Result_Define.eResult.SUCCESS;
                            }
                        }
                        else if (requestOp.Equals("guild_info"))
                        {
                            Account userInfo = AccountManager.GetAccountData(ref tb, AID, ref retError);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                int resultCode = 0;
                                int popupType = 0;
                                long resultAID = 0;
                                int myCash = 0;
                                myCash = userInfo.EventCash + userInfo.Cash;
                                string strGuildID = queryFetcher.QueryParam_Fetch("guildid", "0");
                                long guildid = 0;
                                if (!string.IsNullOrEmpty(strGuildID))
                                    guildid = System.Convert.ToInt64(strGuildID);

                                if (guildid == 0)
                                {
                                    Guild_GuildCreation guildInfo = GuildManager.GetGuilData(ref tb, userInfo.GuildID, true);
                                    if (userInfo.GuildID > 0)
                                    {
                                        Ret_MyGuild_Info retGuild = GuildManager.GetMyGuildInfo(ref tb, ref guildInfo, AID, true);
                                        //string guildInfoJson = mJsonSerializer.SetJsonKeyLow(mJsonSerializer.ToJsonString(retGuild));
                                        //json = JsonObject.Parse(guildInfoJson);
                                        json = JsonObject.Parse(mJsonSerializer.ToJsonString(retGuild));
                                    }
                                }
                                else
                                {
                                    Guild_GuildCreation guildInfo = GuildManager.GetGuilData(ref tb, guildid);
                                    if (!string.IsNullOrEmpty(guildInfo.GuildDeleteDate.ToString()))
                                        retError = Result_Define.eResult.GUILD_NOEXIST_INFO;
                                    else
                                    {
                                        Ret_Guild_Info retGuild = GuildManager.GetGuildInfo(ref tb, ref guildInfo, AID);
                                        //string guildInfoJson = mJsonSerializer.SetJsonKeyLow(mJsonSerializer.ToJsonString(retGuild));
                                        //json = JsonObject.Parse(guildInfoJson);
                                        json = JsonObject.Parse(mJsonSerializer.ToJsonString(retGuild));
                                    }
                                }

                                GuildManager.SelectGuildInfoEntrust(ref tb, ref resultCode, ref popupType, ref resultAID, AID);
                                
                                if (resultCode == (int)Result_Define.eResult.SUCCESS && retError != Result_Define.eResult.GUILD_NOEXIST_INFO)
                                    retError = Result_Define.eResult.SUCCESS;

                                if (retError == Result_Define.eResult.SUCCESS)
                                {   
                                    json = mJsonSerializer.AddJson(json, "popuptype", popupType.ToString());
                                    json = mJsonSerializer.AddJson(json, "resultaid", resultAID.ToString());
                                    json = mJsonSerializer.AddJson(json, "mycash", myCash.ToString());
                                }
                            }

                        }
                        else if (requestOp.Equals("guild_join"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;

                            long guildid = queryFetcher.QueryParam_FetchLong("guildid");
                            Int16 guildstate = queryFetcher.QueryParam_FetchShort("guildstate");
                            retError = GuildManager.GuildJoin(ref tb, AID, guildid, guildstate);
                            json = mJsonSerializer.AddJson(json, "guildstate", guildstate.ToString());
                            if ((retError == Result_Define.eResult.SUCCESS && guildstate == 0) || retError == Result_Define.eResult.GUILD_JOINED_CHANGE_PRIVATE_TO_PUBLIC)
                            {
                                if (retError == Result_Define.eResult.GUILD_JOINED_CHANGE_PRIVATE_TO_PUBLIC)
                                {
                                    retError = AccountManager.UpdateGuildID(ref tb, AID, guildid);
                                    if (retError == Result_Define.eResult.SUCCESS)
                                        retError = Result_Define.eResult.GUILD_JOINED_CHANGE_PRIVATE_TO_PUBLIC;
                                }
                                else
                                {
                                    retError = AccountManager.UpdateGuildID(ref tb, AID, guildid);
                                }
                                Result_Define.eResult retError2 = Result_Define.eResult.SUCCESS;
                                Account userInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError2);
                                if (retError2 != Result_Define.eResult.SUCCESS)
                                    retError = retError2;
                                GuildManager.GetGuildJoinerInfoList(ref tb, guildid, true);
                            }
                        }
                        else if (requestOp.Equals("guild_managed"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long guildid = queryFetcher.QueryParam_FetchLong("guildid");
                            int guildtype = queryFetcher.QueryParam_FetchInt("guildtype");
                            int managetype = queryFetcher.QueryParam_FetchInt("managetype");
                            Guild_GuildCreation guildInfo = GuildManager.GetGuilData(ref tb, guildid);
                            if (guildInfo.GuildID <= 0)
                                retError = Result_Define.eResult.GUILD_NOEXIST_INFO;                            
                            else
                                retError = Result_Define.eResult.SUCCESS;
                            
                            if (AID != guildInfo.GuildCreateAID)
                                retError = Result_Define.eResult.ONLY_GUILD_MASTER;
                            else
                                retError = Result_Define.eResult.SUCCESS;

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                int joinerCount = GuildManager.GetGuildJoinerCount(ref tb, guildid);
                                int MaxJoinerCount = GuildManager.GetSystemGuildData(ref tb, guildInfo.GuildLevel).Max_Member;
                                List<ManagedJoiner> joinerList = GuildManager.GetGuildManagedJoinerList(ref tb, guildid, guildInfo.GuildCreateAID, guildtype, managetype, true).Values.ToList();
                                retError = Result_Define.eResult.SUCCESS;
                                json = mJsonSerializer.AddJson(json, "guildusercnt", joinerCount.ToString());
                                json = mJsonSerializer.AddJson(json, "guildmaxusercnt", MaxJoinerCount.ToString());
                                if (joinerList.Count > 0 && joinerList != null)
                                {
                                    json = mJsonSerializer.AddJson(json, "guildjoiner", mJsonSerializer.ToJsonString(joinerList));
                                }
                            }
                        }
                        else if (requestOp.Equals("guild_recommend"))
                        {
                            string guildname = queryFetcher.QueryParam_Fetch("guildname");
                            List<GuildRecommend> recommendList = null;
                            if (string.IsNullOrEmpty(guildname))
                            {
                                queryFetcher.DBLog("check guild_recommend");
                                recommendList = GuildManager.GetRecommandGuildList(ref tb, AID);
                                GuildJoiner joiner = GuildManager.GetJoinerData(ref tb, AID);
                                if (joiner.GuildID > 0 && joiner.JoinerState == "I")
                                    json = mJsonSerializer.AddJson(json, "isguildwait", "Y");
                                else
                                    json = mJsonSerializer.AddJson(json, "isguildwait", "N");

                                retError = Result_Define.eResult.SUCCESS;
                            }
                            else
                            {
                                retError = GuildManager.GetSearchGuildList(ref tb, ref recommendList, AID, guildname);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    GuildJoiner joiner = GuildManager.GetJoinerData(ref tb, AID, true);
                                    if (joiner.GuildID > 0 && joiner.JoinerState == "I")
                                        json = mJsonSerializer.AddJson(json, "isguildwait", "Y");
                                    else
                                        json = mJsonSerializer.AddJson(json, "isguildwait", "N");
                                }
                            }                            

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                json = mJsonSerializer.AddJson(json, "guildlist", mJsonSerializer.ToJsonString(recommendList));   
                            }
                        }
                        else if (requestOp.Equals("guild_joiner_state_change"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long joineraid = queryFetcher.QueryParam_FetchLong("joineraid");
                            int changetype = queryFetcher.QueryParam_FetchInt("changetype"); 

                            retError = GuildManager.GuildJoinserChange(ref tb, AID, joineraid, changetype);
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                
                                json = mJsonSerializer.AddJson(json, "changetype", changetype.ToString());

                                GuildJoiner joinerInfo = GuildManager.GetJoinerData(ref tb, AID, true);
                                Guild_GuildCreation guildInfo = GuildManager.GetGuilData(ref tb, joinerInfo.GuildID, true);
                                Account userInfo = AccountManager.FlushAccountData(ref tb, joineraid, ref retError);

                                if (changetype == 1)
                                {
                                    json = mJsonSerializer.AddJson(json, "guildname", guildInfo.GuildName);
                                }                                
                            }
                        }
                        else if (requestOp.Equals("guild_entrust_ask"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long entrustid = queryFetcher.QueryParam_FetchLong("entrustid");

                            if (entrustid == 0)
                            {
                                retError = Result_Define.eResult.NO_GUILD_USER;
                            }
                            else
                            {
                                retError = GuildManager.GuildEntrustAsk(ref tb, AID, entrustid);
                            }
                        }
                        else if (requestOp.Equals("guild_entrust_check"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            long entrustid = queryFetcher.QueryParam_FetchLong("entrustid");
                            int YNButton = 0;
                            long prevAID = 0;
                            string prevName = "";
                            if (entrustid == 0)
                            {
                                retError = Result_Define.eResult.NO_GUILD_USER;
                            }
                            else
                            {
                                retError = GuildManager.GuildEntrustCheck(ref tb, ref YNButton, ref prevAID, ref prevName, AID, entrustid);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, "ynbutton", YNButton.ToString());
                                    json = mJsonSerializer.AddJson(json, "prevaid", prevAID.ToString());
                                    json = mJsonSerializer.AddJson(json, "prevname", prevName.ToString());
                                }
                            }
                        }
                        else if (requestOp.Equals("guild_entrust_reply"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            int changeType = queryFetcher.QueryParam_FetchInt("changetype");
                            string changedata = queryFetcher.QueryParam_Fetch("changedata");

                            retError = GuildManager.GuildEntrustReply(ref tb, AID, changeType, changedata);
                        }
                        else if (requestOp.Equals("guild_state_change"))
                        {
                            tb.IsoLevel = IsolationLevel.ReadCommitted;
                            int changeType = queryFetcher.QueryParam_FetchInt("changetype");
                            string changedata = queryFetcher.QueryParam_Fetch("changedata");
                            Account userInfo = AccountManager.GetAccountData(ref tb, AID, ref retError);
                            long guildMasterAID = GuildManager.GetGuildMasterAID(ref tb, userInfo.GuildID);
                            if (changeType == (int)GuildStateChangeType.DONATION && retError == Result_Define.eResult.SUCCESS) // 기부
                            {
                                retError = GuildManager.CheckGuildDonation(ref tb, AID, changedata);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    //기부처리
                                    int plusExp = 0;
                                    int donationpoint = 0;
                                    retError = GuildManager.GuildDonation(ref tb, ref donationpoint, ref plusExp, ref userInfo, changedata);
                                    Guild_GuildCreation guildInfo = GuildManager.GetGuilData(ref tb, userInfo.GuildID, true);

                                    json = mJsonSerializer.AddJson(json, "guildlv", guildInfo.GuildLevel.ToString());
                                    json = mJsonSerializer.AddJson(json, "guildexp", (guildInfo.GuildExp+guildInfo.GuildWithdrawExp).ToString());
                                    json = mJsonSerializer.AddJson(json, "donationexp", plusExp.ToString());
                                    json = mJsonSerializer.AddJson(json, "donationpoint", donationpoint.ToString());

                                    userInfo = AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                    json = mJsonSerializer.AddJson(json, "resultgold", userInfo.Gold.ToString());
                                    json = mJsonSerializer.AddJson(json, "resultcash", (userInfo.Cash + userInfo.EventCash).ToString());
                                    json = mJsonSerializer.AddJson(json, "contributionpoint", userInfo.ContributionPoint.ToString());

                                    if (retError == Result_Define.eResult.SUCCESS)
                                    {
                                        List<DonateInfo> donationlist = GuildManager.GetDonateList(ref tb, userInfo.GuildID, true);
                                        json = mJsonSerializer.AddJson(json, "donationlist", mJsonSerializer.ToJsonString(donationlist));
                                    }
                                }
                            }
                            else if (changeType == (int)GuildStateChangeType.INTRODUCE) // 소개
                            {
                                if (AID != guildMasterAID)
                                    retError = Result_Define.eResult.ONLY_GUILD_MASTER;
                                else
                                    retError = GuildManager.GuildIntroduceModify(ref tb, userInfo.GuildID, changedata);
                            }
                            else if (changeType == (int)GuildStateChangeType.NOTICE) // 공지
                            {
                                if (AID != guildMasterAID)
                                    retError = Result_Define.eResult.ONLY_GUILD_MASTER;
                                else
                                    retError = GuildManager.GuildNoticeModify(ref tb, userInfo.GuildID, changedata);
                            }
                            else if (changeType == (int)GuildStateChangeType.CHECKATTEND) // 출석
                            {
                                GuildJoiner joinerInfo = GuildManager.GetJoinerData(ref tb, AID, true);
                                if (joinerInfo.TodayAttendDate == null)
                                {
                                    if (joinerInfo.GuildID > 0)
                                    {
                                        if (CharacterManager.GetCharacter(ref tb, AID, userInfo.EquipCID).level < 10)
                                            retError = Result_Define.eResult.GUILD_DONATION_LEVELNOT_ENOUGH;
                                        else
                                            retError = GuildManager.SetGuildAttend(ref tb, joinerInfo.GuildID, AID);
                                    }
                                    else
                                        retError = Result_Define.eResult.GUILD_NOEXIST_INFO;
                                }
                            }
                            else if (changeType == (int)GuildStateChangeType.DISSOLUTION)//해산
                            {
                                if (AID != guildMasterAID)
                                    retError = Result_Define.eResult.ONLY_GUILD_MASTER;
                                else
                                    retError = GuildManager.GuildDissolution(ref tb, userInfo.GuildID);
                            }
                            else
                            {
                                //비공개, 공개 처리 필요
                                if (AID != guildMasterAID)
                                    retError = Result_Define.eResult.ONLY_GUILD_MASTER;
                                else
                                    retError = GuildManager.GuildStateChange(ref tb, ref json, userInfo.GuildID);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                GuildManager.GetGuilData(ref tb, userInfo.GuildID, true);
                                AccountManager.FlushAccountData(ref tb, AID, ref retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    json = mJsonSerializer.AddJson(json, "changetype", changeType.ToString());
                                }
                            }
                        }
                        
                        retJson = queryFetcher.Render(mJsonSerializer.SetJsonKeyLow(json.ToJson()), retError);
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
                    if (retError == Result_Define.eResult.GUILD_REQUESTJOIN_CHANGE_PUBLIC_TO_PRIVATE || retError == Result_Define.eResult.GUILD_JOINED_CHANGE_PRIVATE_TO_PUBLIC)
                        queryFetcher.SetShowLogMode = tb.EndTransaction(true);
                    else
                        queryFetcher.SetShowLogMode = tb.EndTransaction(queryFetcher.Render_errorFlag);
                    queryFetcher.ErrorLogWrite(retJson, ref tb);
                    tb.Dispose();
                } 
            }
        }

    }
}