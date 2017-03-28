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
    public partial class rankingList : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            string userName = queryFetcher.QueryParam_Fetch(username.UniqueID, "");
            long deleteID = queryFetcher.QueryParam_FetchLong(userid.UniqueID);
            if (!Page.IsPostBack || !string.IsNullOrEmpty(userName) || deleteID > 0)
                GetRankingList(1);
        }

        protected void GetRankingList(int pageIndex)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
                    GMDataManager.GetServerinit(ref tb, serverID);
                    int rankType = queryFetcher.QueryParam_FetchInt("rank", 2);
                    string userName = queryFetcher.QueryParam_Fetch(username.UniqueID, "");
                    long deleteID = queryFetcher.QueryParam_FetchLong(userid.UniqueID);
                    long AID = 0;

                    pvptype.Value = rankType.ToString();
                    if (deleteID > 0)
                        tb.IsoLevel = IsolationLevel.ReadCommitted;

                    int startNum = 0;
                    int endNum = GMData_Define.pageSize - 1;

                    PvP_Define.ePvPType pvpType = (PvP_Define.ePvPType)rankType;

                    if (!string.IsNullOrEmpty(userName))
                    {
                        AID = GMDataManager.GetSearchAID_BYUserName(ref tb, userName);
                    }
                    int week = 0;
                    week = PvPManager.GetSeperater(ref tb, pvpType);

                    if (deleteID > 0)
                    {

                        //retError = PvPManager.RemoveUser_PvP_point(ref tb, deleteID, week, (PvP_Define.ePvPType)rankType, shardingdbkey);
                        
                        PVP_Record setObj = PvPManager.GetUser_PvP_Record(ref tb, deleteID, week, pvpType);
                        setObj.totalhonorpoint = 0;
                        retError = PvPManager.SetUser_PvP_Record(ref tb, setObj);
                        if (retError == Result_Define.eResult.SUCCESS)
                            retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_USER, deleteID, AccountManager.GetSimpleAccountInfo(ref tb, AID).username, GMResult_Define.ControlType.RANKING_DELETE, queryFetcher.GetReqParams(), serverID.ToString());
                        userid.Value = string.Empty;
                        retJson = queryFetcher.GM_Render(retError);
                    }
                    else
                    {
                        long totalPlayer = PvPManager.GetTotal_PvP_Rank_Player(ref tb, week, pvpType);
                        if (AID > 0)
                        {

                            List<Ret_PvP> rankList = new List<Ret_PvP>();
                            rankList.Add(PvPManager.GetUser_PvP_Rank_Info(ref tb, AID, week, pvpType));
                            dataList.DataSource = rankList;
                            dataList.DataBind();
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

                            List<Ret_PvP> rankList = PvPManager.GetUser_PvP_Rank_List(ref tb, AID, week, pvpType, startNum, endNum).OrderBy(item => item.rank).ToList();
                            dataList.DataSource = rankList;
                            dataList.DataBind();
                            GMDataManager.PopulatePager(ref dlPager, totalPlayer, pageIndex);

                        }
                        queryFetcher.GM_Render(Result_Define.eResult.SUCCESS);
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
                if (retError == Result_Define.eResult.SUCCESS)
                {
                    Response.Redirect(Request.Url.ToString());
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