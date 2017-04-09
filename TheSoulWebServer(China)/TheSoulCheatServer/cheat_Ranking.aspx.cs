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
    public partial class cheat_Ranking : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            JsonObjectCollection res = new JsonObjectCollection();
            if ((Request["guildname"] == null || Request["guildname"] == "") && (Request["username"] == null || Request["username"] == ""))
            {
                res.Add(new JsonNumericValue("resultcode", 1));
                res.Add(new JsonStringValue("message", "Post 값 전달 실패"));
                string json_text = res.ToString();
                Response.Write(json_text);
            }
            else
            {
                string guildname = Request["guildname"];
                string username = Request["username"];

                WebQueryParam queryFetcher = new WebQueryParam();
                string retJson = "";
                queryFetcher.SetDebugMode = true;
                int pvp_free = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("pvp_free", "0"));                
                int pvp_1VS1 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("pvp_1VS1", "0"));
                int guildranking = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("guildranking", "0"));
                int guildwar = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("guildwar", "0"));

                TxnBlock tb = new TxnBlock();
                {
                    long GuildID = 0;
                    try
                    {    
                        queryFetcher.TxnBlockInit(ref tb, ref GuildID);
                        queryFetcher.GlobalDBOpen(ref tb);
                        int week = PvPManager.GetSeperater_Week(ref tb);
                        Result_Define.eResult retErr = Result_Define.eResult.POST_DATA_ERROR;
                        if (!string.IsNullOrEmpty(guildname))
                        {
                            Guild_GuildCreation guild = GuildManager.GetSearchGuildInfo_ByName(ref tb, guildname);
                            if (guild.GuildID > 0)
                            {
                                if (guildranking > 0)
                                {
                                    List<Ret_GuildPvP> PreRankInfo = PvPManager.GetUser_PvP_Guild_Rank_List(ref tb, (guildranking-1), (guildranking-1));
                                    foreach (Ret_GuildPvP rankInfo in PreRankInfo)
                                    {
                                        long updatePoint = System.Convert.ToInt64(rankInfo.last_point) - System.Convert.ToInt64(guild.GuildRankingPoint) + 1;
                                        string setQuery = string.Format("Update {0} Set GuildRankingPoint = GuildRankingPoint+{1} Where guildID = {2}", GuildManager.GuildCreationDBTableName, updatePoint, guild.GuildID);
                                        retErr = tb.ExcuteSqlCommand(GuildManager.GuildcommonDBName, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                        if (retErr == Result_Define.eResult.SUCCESS)
                                        {
                                            string setQuery2 = string.Format("Update {0} Set weekGuildRankPoint = weekGuildRankPoint+({1}), update_date = getDate() Where gid = {2} And seperater = {3}", GuildManager.GuildRankPointDBTableName, updatePoint, guild.GuildID, week-1);
                                            retErr = tb.ExcuteSqlCommand(GuildManager.GuildcommonDBName, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                            if (retErr == Result_Define.eResult.SUCCESS)
                                            {
                                                GuildManager.GetGuildRankPoint(ref tb, guild.GuildID, true);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(username))
                        {
                            long AID = 0;
                            User_account user_server = cheatData.GetUserAID(ref tb, username);
                            AID = user_server.AID;
                            if (pvp_free > 0)
                            {
                                Ret_PvP myRanking = PvPManager.GetUser_PvP_Rank_Info(ref tb, AID, week, PvP_Define.ePvPType.MATCH_FREE);
                                List<Ret_PvP> rankingList = PvPManager.GetUser_PvP_Rank_List(ref tb, AID, week, PvP_Define.ePvPType.MATCH_FREE, (pvp_free - 1), (pvp_free - 1));

                                foreach (Ret_PvP rankInfo in rankingList)
                                {
                                    long updatePoint = 0;
                                    if (myRanking.rank > System.Convert.ToInt64(pvp_free))
                                    {
                                        updatePoint = System.Convert.ToInt64(rankInfo.point) - System.Convert.ToInt64(myRanking.point) - 1;
                                    }
                                    else
                                    {
                                        updatePoint = System.Convert.ToInt64(rankInfo.point) - System.Convert.ToInt64(myRanking.point) + 1;
                                    }
                                    string setQuery = string.Format("Update {0} Set totalhonorpoint = totalhonorpoint + ({3}), weekhonorpoint = weekhonorpoint + ({3}) WHERE pvp_type = {1} AND seperater = {2} And AID= {4}", PvP_Define.PvP_PvP_Weekly_TableName, 1, week, updatePoint, AID);
                                    retErr = tb.ExcuteSqlCommand(PvP_Define.PvP_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                }
                                if (retErr == Result_Define.eResult.SUCCESS)
                                {
                                    PvPManager.GetUser_PvPInfo(ref tb, AID, PvP_Define.ePvPType.MATCH_FREE, true);
                                    PvPManager.SetUserPvP_Rank_Info(ref tb, AID, week, PvP_Define.ePvPType.MATCH_FREE);
                                }
                            }
                            if (pvp_1VS1 > 0)
                            {
                                Ret_PvP myRanking = PvPManager.GetUser_PvP_Rank_Info(ref tb, AID, week, PvP_Define.ePvPType.MATCH_1VS1);
                                List<Ret_PvP> rankingList = PvPManager.GetUser_PvP_Rank_List(ref tb, AID, week, PvP_Define.ePvPType.MATCH_1VS1, (pvp_1VS1 - 1), (pvp_1VS1 - 1));

                                foreach (Ret_PvP rankInfo in rankingList)
                                {
                                    long updatePoint = 0;
                                    if (myRanking.rank > System.Convert.ToInt64(pvp_1VS1))
                                    {
                                        updatePoint = System.Convert.ToInt64(rankInfo.point) - System.Convert.ToInt64(myRanking.point) + 1;
                                    }
                                    else
                                    {
                                        updatePoint = System.Convert.ToInt64(rankInfo.point) - System.Convert.ToInt64(myRanking.point) - 1;
                                    }
                                    string setQuery = string.Format("Update {0} Set totalhonorpoint = totalhonorpoint + ({3}), weekhonorpoint = weekhonorpoint + ({3}) WHERE pvp_type = {1} AND seperater = {2} And AID= {4}", PvP_Define.PvP_PvP_Weekly_TableName, 2, week, updatePoint, AID);
                                    retErr = tb.ExcuteSqlCommand(PvP_Define.PvP_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                }
                                if (retErr == Result_Define.eResult.SUCCESS)
                                {
                                    PvPManager.GetUser_PvPInfo(ref tb, AID, PvP_Define.ePvPType.MATCH_1VS1, true);
                                    PvPManager.SetUserPvP_Rank_Info(ref tb, AID, week, PvP_Define.ePvPType.MATCH_1VS1);
                                }
                            }
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