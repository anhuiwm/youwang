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
    public partial class guildRanking : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            string guildName = queryFetcher.QueryParam_Fetch(guildname.UniqueID, "");
            long deleteID = queryFetcher.QueryParam_FetchLong(userid.UniqueID);
            if (!Page.IsPostBack || !string.IsNullOrEmpty(guildName) || deleteID>0)
                GetRankingList(1);
        }

        protected void GetRankingList(int pageIndex)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, serverID);
                    int rankType = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("rank", "3"));
                    string guildName = queryFetcher.QueryParam_Fetch(guildname.UniqueID, "");
                    long deleteID = queryFetcher.QueryParam_FetchLong(userid.UniqueID);
                    long GID = 0;

                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                    pvptype.Value = rankType.ToString();
                    int startNum = 0;
                    int endNum = GMData_Define.pageSize - 1;
                    if(deleteID > 0)
                        tb.IsoLevel = IsolationLevel.ReadCommitted;

                    if (!string.IsNullOrEmpty(guildName))
                    {
                        GID = GuildManager.GetSearchGuildInfo_ByName(ref tb, guildName).GuildID;
                    }
                    int week = 0;
                    if (rankType == 3)// redis key 호출
                        week = PvPManager.GetSeperater(ref tb, PvP_Define.ePvPType.MATCH_GUILD_3VS3);
                    else
                        week = PvPManager.GetSeperater_Week(ref tb);

                    if (deleteID > 0)
                    {

                        if (rankType == 3)
                        {
                            Guild_PVP_Record setInfo = PvPManager.GetGuild_PvP_Record(ref tb, deleteID, week);
                            setInfo.rankpoint = 0;
                            retError = PvPManager.SetGuild_PvP_Record(ref tb, setInfo);
                            if (string.IsNullOrEmpty(guildName))
                                GID = 0;
                        }
                        else
                        {
                            retError = GMDataManager.SetGuild_point(ref tb, deleteID, week-1);
                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                retError = PvPManager.SetUser_GuildRanking_Point(ref tb, deleteID, 0, week-1);
                            }
                        }
                        userid.Value = string.Empty;
                    }

                    string setKey = "";
                    if (rankType == 3)// redis key 호출
                        setKey = PvPManager.GetGuildPvP_GuildWar_Rank_Key(ref tb, week);
                    else
                        setKey = PvPManager.GetPvP_Guild_RankKey(ref tb, week, PvP_Define.eGuildRankType.GUILDPOINT);

                    long totalPlayer = RedisConst.GetRedisInstance().GetSortedSetCount(DataManager_Define.RedisServerAlias_Ranking, setKey); // 전체 랭킹 인원 호출
                    if (GID > 0)
                    {
                        if (rankType == 3)
                        {
                            List<Ret_GuildWarPvP> rankList = new List<Ret_GuildWarPvP>();
                            rankList.Add(PvPManager.GetGuildPvP_GuildWar_Rank_Info(ref tb, GID, ref totalPlayer, week));
                            dataList.DataSource = rankList;
                            dataList.DataBind();
                        }
                        else
                        {
                            List<Ret_GuildPvP> rankList = new List<Ret_GuildPvP>();
                            rankList.Add(PvPManager.GetUser_PvP_Guild_Rank_Info(ref tb, GID, ref totalPlayer, week));
                            dataList.DataSource = rankList;
                            dataList.DataBind();
                        }
                        GMDataManager.PopulatePager(ref dlPager, totalPlayer, 1);
                    }
                    else
                    {
                        if (pageIndex > 1) // 1page가 아닐때 뽑아올 랭킹 계산
                        {
                            //0부터 시작이라 1씩 마이너스 처리
                            startNum = (pageIndex - 1) * GMData_Define.pageSize;
                            endNum = (pageIndex * GMData_Define.pageSize) - 1;
                        }
                        if (endNum > (totalPlayer - 1))
                            endNum = (int)totalPlayer - 1;
                        if (rankType == 3)
                        {
                            List<Ret_GuildWarPvP> rankList = PvPManager.GetGuildPvP_GuildWar_Rank_List(ref tb, startNum, endNum, week);
                            dataList.DataSource = rankList;
                            dataList.DataBind();
                        }
                        else
                        {
                            List<Ret_GuildPvP> rankList = PvPManager.GetUser_PvP_Guild_Rank_List(ref tb, startNum, endNum, week).OrderBy(item => item.rank).ToList();
                            BoundField field = (BoundField)this.dataList.Columns[2];
                            field.DataField = "last_point";
                            dataList.DataSource = rankList;
                            dataList.DataBind();
                        }
                        GMDataManager.PopulatePager(ref dlPager, totalPlayer, pageIndex);
                            
                    }
                    queryFetcher.GM_Render(retError);
                    
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
            }
        }
        
        protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "PageNo")
            {
                int page = System.Convert.ToInt32(e.CommandArgument);
                this.GetRankingList(page);
            }
        }
    }
}