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
using TheSoulCheatServer.lib;
using System.Net.Json;


namespace TheSoulCheatServer
{
    public partial class AccountCharge : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            JsonObjectCollection res = new JsonObjectCollection();
            if (Request["username"] == null || Request["username"] == "")
            {
                res.Add(new JsonNumericValue("resultcode", 1));
                res.Add(new JsonStringValue("message", "Post 값 전달 실패"));
                string json_text = res.ToString();
                Response.Write(json_text);
            }
            else
            {
                
                WebQueryParam queryFetcher = new WebQueryParam();
                string retJson = "";
                queryFetcher.SetDebugMode = true;
                string username = queryFetcher.QueryParam_Fetch("username", "");
                int cash = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("cash","0"));
                int key = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("key", "0"));
                int ticket = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("ticket", "0"));
                int gold = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("gold", "0"));
                int honorpoint = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("honorpoint", "0"));
                int medal = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("medal", "0"));
                int ChallengeTicket = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("ChallengeTicket", "0"));
                
                int Battlepint = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("Battlepint", "0"));
                int Partypoint = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("Partypoint", "0"));
                int Donationpoint = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("Donationpoint", "0"));
                int Expeditionpoint = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("Expeditionpoint", "0"));
                int overloadpoint = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("overloadpoint", "0"));

                TxnBlock tb = new TxnBlock();
                {
                    long AID = 0;
                    try
                    {
                        queryFetcher.TxnBlockInit(ref tb, ref AID);
                        queryFetcher.GlobalDBOpen(ref tb);

                        User_account user_server = cheatData.GetUserAID(ref tb, username);
                        AID =  user_server.AID;
                        
                        Result_Define.eResult retErr = Result_Define.eResult.POST_DATA_ERROR;
                        Account userInfo = AccountManager.FlushAccountData(ref tb, AID, ref retErr);
                        if (overloadpoint > 0)
                        {
                            if (userInfo.OverlordPoint > 0)
                                retErr = AccountManager.UseUserOverlordRankingPoint(ref tb, AID, userInfo.OverlordPoint);
                            else
                                retErr = Result_Define.eResult.SUCCESS;

                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                retErr = AccountManager.AddUserOverlordRankingPoint(ref tb, AID, overloadpoint);
                            }
                        }
                        if (cash > 0)
                        {
                            if ((userInfo.Cash + userInfo.EventCash) > 0)
                                retErr = AccountManager.UseUserCash(ref tb, AID, (userInfo.Cash + userInfo.EventCash));
                            else
                                retErr = Result_Define.eResult.SUCCESS;
                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                retErr = AccountManager.AddUserEventCash(ref tb, AID, cash);
                            }
                        }
                        if (gold > 0)
                        {
                            if(userInfo.Gold > 0)
                                retErr = AccountManager.UseUserGold(ref tb, AID, userInfo.Gold);
                            else
                                retErr = Result_Define.eResult.SUCCESS;

                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                retErr = AccountManager.AddUserGold(ref tb, AID, gold);
                            }
                        }
                        if (key > 0)
                        {
                            if (userInfo.Key > 0)
                                retErr = AccountManager.UseUserKey(ref tb, AID, userInfo.Key);
                            else
                                retErr = Result_Define.eResult.SUCCESS;

                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                retErr = AccountManager.AddUserKey(ref tb, AID, key);
                            }
                        }
                        if (ticket > 0)
                        {
                            if(userInfo.Ticket > 0)
                                retErr = AccountManager.UseUserTicket(ref tb, AID, userInfo.Ticket);
                            else
                                retErr = Result_Define.eResult.SUCCESS;

                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                retErr = AccountManager.AddUserTicket(ref tb, AID, ticket);
                            }
                        }
                        if (honorpoint > 0)
                        {
                            if(userInfo.Honorpoint > 0)
                                retErr = AccountManager.UseUserHonor(ref tb, AID, userInfo.Honorpoint);
                            else
                                retErr = Result_Define.eResult.SUCCESS;

                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                retErr = AccountManager.AddUserHonor(ref tb, AID, honorpoint);
                            }
                        }
                        if (Battlepint > 0)
                        {
                            if (userInfo.CombatPoint > 0)
                                retErr = AccountManager.UseUserCombatPoint(ref tb, AID, userInfo.CombatPoint);
                            else
                                retErr = Result_Define.eResult.SUCCESS;

                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                retErr = AccountManager.AddUserCombatPoint(ref tb, AID, Battlepint);
                            }
                        }
                        if (Partypoint > 0)
                        {
                            if (userInfo.PartyDungeonPoint > 0)
                                retErr = AccountManager.UseUserPartyDungeonPoint(ref tb, AID, userInfo.PartyDungeonPoint);
                            else
                                retErr = Result_Define.eResult.SUCCESS;

                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                retErr = AccountManager.AddUserPartyDungeonPoint(ref tb, AID, Partypoint);
                            }
                        }
                        if (Donationpoint > 0)
                        {
                            if (userInfo.ContributionPoint > 0)
                                retErr = GuildManager.UseGuildContributionPoint(ref tb, AID, userInfo.ContributionPoint);
                            else
                                retErr = Result_Define.eResult.SUCCESS;

                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                retErr = GuildManager.AddGuildContributionPoint(ref tb, AID, Donationpoint);
                            }
                        }
                        if (Expeditionpoint > 0)
                        {
                            if (userInfo.ExpeditionPoint > 0)
                                retErr = AccountManager.UseUserExpeditionPoint(ref tb, AID, userInfo.ExpeditionPoint);
                            else
                                retErr = Result_Define.eResult.SUCCESS;

                            if (retErr == Result_Define.eResult.SUCCESS)
                            {
                                retErr = AccountManager.AddUserExpeditionPoint(ref tb, AID, Expeditionpoint);
                            }
                        }
                        if (medal > 0)
                        {
                            string setQuery = string.Format("Update {0} Set Medal = {1} Where AID = {2}", Account_Define.AccountDBTableName, medal, AID);
                            retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB,setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        }
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            AccountManager.FlushAccountData(ref tb, AID, ref retErr);
                        }
                        if (ChallengeTicket > 0)
                        {
                            string setQuery = string.Format("Update {0} Set ChallengeTicket = {1} Where AID = {2}", Account_Define.AccountDBTableName, ChallengeTicket, AID);
                            retErr = tb.ExcuteSqlCommand(Account_Define.AccountShardingDB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                        }
                        if (retErr == Result_Define.eResult.SUCCESS)
                        {
                            AccountManager.FlushAccountData(ref tb, AID, ref retErr);
                        }

                        retJson = queryFetcher.Render("", retErr);

                    }
                    catch (Exception errorEx)
                    {
                        retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                    }
                    tb.EndTransaction(queryFetcher.Render_errorFlag);
                    tb.Dispose();
                }
            }

        }
    }
}