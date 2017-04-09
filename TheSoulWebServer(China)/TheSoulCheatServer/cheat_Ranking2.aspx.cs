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
    public partial class cheat_Ranking2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            JsonObjectCollection res = new JsonObjectCollection();  
            if (Request["pvpType"] == null || Request["pvpType"] == "")
            {
                res.Add(new JsonNumericValue("resultcode", 1));
                res.Add(new JsonStringValue("message", "Post 값 전달 실패"));
                string json_text = res.ToString();
                Response.Write(json_text);
            }
            else
            {
                string pvpType = Request["pvpType"];
                
                WebQueryParam queryFetcher = new WebQueryParam();
                string retJson = "";
                queryFetcher.SetDebugMode = true;
                
                TxnBlock tb = new TxnBlock();
                {
                    long GuildID = 1;
                    try
                    {    
                        queryFetcher.TxnBlockInit(ref tb, ref GuildID);
                        queryFetcher.GlobalDBOpen(ref tb);
                        int week = PvPManager.GetSeperater_Week(ref tb);
                        Result_Define.eResult retErr = Result_Define.eResult.POST_DATA_ERROR;
                        if (RedisConst.GetRedisInstance().redisConnActive)
                        {
                            PvP_Define.ePvPType setPvPType = PvP_Define.ePvPType.MATCH_FREE;
                            if (pvpType.Equals("guild"))
                            {
                                List<Ret_GuildPvP> pvpList = PvPManager.GetUser_PvP_Guild_Rank_List(ref tb).OrderBy(item => item.rank).ToList();
                                GridView1.DataSource = pvpList;
                                GridView1.DataBind();
                                foreach (Ret_GuildPvP setData in pvpList)
                                {
                                    string data = queryFetcher.QueryParam_Fetch(setData.gid.ToString(), "");
                                    if (!string.IsNullOrEmpty(data))
                                    {
                                        string setQuery = string.Format("Update {0} Set GuildRankingPoint = {1} Where guildID = {2}", GuildManager.GuildCreationDBTableName, data, setData.gid);
                                        retErr = tb.ExcuteSqlCommand(GuildManager.GuildcommonDBName, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                        if (retErr == Result_Define.eResult.SUCCESS)
                                        {
                                            string setQuery2 = string.Format("Update {0} Set weekGuildRankPoint = {1}, update_date = getDate() Where gid = {2} And seperater = {3}", GuildManager.GuildRankPointDBTableName, data, setData.gid, week);
                                            retErr = tb.ExcuteSqlCommand(GuildManager.GuildcommonDBName, setQuery2) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                            if (retErr == Result_Define.eResult.SUCCESS)
                                            {
                                                GuildManager.GetGuildRankPoint(ref tb, setData.gid, true);
                                            }
                                        }
                                    }
                                }
                                if (retErr == Result_Define.eResult.SUCCESS)
                                {
                                    pvpList = PvPManager.GetUser_PvP_Guild_Rank_List(ref tb).OrderBy(item => item.rank).ToList();
                                    GridView1.DataSource = pvpList;
                                    GridView1.DataBind();
                                }
                                if (pvpList.Count > 0)
                                {
                                    retErr = Result_Define.eResult.SUCCESS;
                                }
                            }
                            else if (pvpType.Equals("1vs1") || pvpType.Equals("free"))
                            {
                                int setType = 1;
                                if (pvpType.Equals("1vs1"))
                                {
                                    setPvPType = PvP_Define.ePvPType.MATCH_1VS1;
                                    setType = 2;
                                }
                                List<Ret_PvP> pvpList = PvPManager.GetUser_PvP_Rank_List(ref tb, GuildID, week, setPvPType).OrderBy(item => item.rank).ToList();
                                DataGrid2.DataSource = pvpList;
                                DataGrid2.DataBind();
                                foreach (Ret_PvP setData in pvpList)
                                {
                                    string data = queryFetcher.QueryParam_Fetch(setData.aid.ToString(), "");
                                    if (!string.IsNullOrEmpty(data))
                                    {
                                        string setQuery = string.Format("Update {0} Set totalhonorpoint = {3}, weekhonorpoint = {3} WHERE pvp_type = {1} AND seperater = {2} And AID= {4}", PvP_Define.PvP_PvP_Weekly_TableName, setType, week, data, setData.aid);
                                        retErr = tb.ExcuteSqlCommand(PvP_Define.PvP_Info_DB, setQuery) ? Result_Define.eResult.SUCCESS : Result_Define.eResult.DB_ERROR;
                                        if (retErr == Result_Define.eResult.SUCCESS)
                                        {
                                            PvPManager.SetUserPvP_Rank_Info(ref tb, setData.aid, week, setPvPType);
                                        }
                                    }
                                }
                                if (retErr == Result_Define.eResult.SUCCESS)
                                {
                                    pvpList = PvPManager.GetUser_PvP_Rank_List(ref tb, GuildID, week, setPvPType).OrderBy(item => item.rank).ToList();
                                    DataGrid2.DataSource = pvpList;
                                    DataGrid2.DataBind();
                                }
                                if (pvpList.Count > 0)
                                {
                                    retErr = Result_Define.eResult.SUCCESS;
                                }
                            }
                        }
                        else
                            retErr = Result_Define.eResult.REDIS_COMMAND_FAIL;
                        retJson = queryFetcher.Render(retJson, retErr);

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