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
    public partial class overloadRanking : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            string userName = queryFetcher.QueryParam_Fetch(username.UniqueID, "");
            long deleteID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(userid.UniqueID, "0"));
            if (!Page.IsPostBack || !string.IsNullOrEmpty(userName) || deleteID > 0)
                GetRankingList(1);
        }

        protected void GetRankingList(int pageIndex)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, serverID);
                    string userName = queryFetcher.QueryParam_Fetch(username.UniqueID, "");
                    long AID = 0;

                    int startNum = 0;
                    int endNum = GMData_Define.pageSize - 1;

                    PvP_Define.ePvPType pvpType = PvP_Define.ePvPType.MATCH_OVERLORD;

                    if (!string.IsNullOrEmpty(userName))
                    {
                        AID = GMDataManager.GetSearchAID_BYUserName(ref tb, userName);
                    }
                    int week = 0;
                    week = PvPManager.GetSeperater(ref tb, pvpType);

                    long totalPlayer = PvPManager.GetUser_Overlord_Ranking_TotalPlayer(ref tb);

                    if (pageIndex > 1) // 1page가 아닐때 뽑아올 랭킹 계산
                    {
                        //0부터 시작이라 1씩 마이너스 처리
                        startNum = (pageIndex - 1) * GMData_Define.pageSize;
                        endNum = (pageIndex * GMData_Define.pageSize) - 1;
                    }
                    if (endNum > (totalPlayer - 1))
                        endNum = (int)totalPlayer - 1;
                    List<GM_Overlord_Ranking> rankList = GMDataManager.GetOverLoadRanking(ref tb, ref totalPlayer, pageIndex, userName);
                    dataList.DataSource = rankList;
                    dataList.DataBind();
                    if (string.IsNullOrEmpty(userName))
                        GMDataManager.PopulatePager(ref dlPager, totalPlayer, pageIndex);
                    else
                        GMDataManager.PopulatePager(ref dlPager, totalPlayer, 1);
                    queryFetcher.GM_Render(Result_Define.eResult.SUCCESS);
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