using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

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

namespace TheSoulGMTool.kr
{
    public partial class bossraid_list : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                GetDataList(1);
        }

        protected void GetDataList(int pageIndex)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                    

                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

                    long totalCount = GMDataManager.GetBossraidCount(ref tb);

                    List<BossRaidCreation> logList = GMDataManager.GetBossraidList(ref tb, pageIndex);
                    logList.ForEach(item =>
                    {
                        System_NPC itemInfo = NPC_Manager.GetNPCInfo(ref tb, item.NpcID);
                        item.DoReward = GMDataManager.GetItmeName(ref tb, itemInfo.NamingCN);
                    });
                    dataList.DataSource = logList;
                    dataList.DataBind();

                    GMDataManager.PopulatePager(ref dlPager, totalCount, pageIndex);
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
                    tb.Dispose();
                }
            }
        }

        protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "PageNo")
            {
                int page = System.Convert.ToInt32(e.CommandArgument);
                this.GetDataList(page);
            }
        }

        protected void stop_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            long bossID = System.Convert.ToInt64(dataList.DataKeys[row.RowIndex].Values[0]);

            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
            int ca2 = queryFetcher.QueryParam_FetchInt("ca2", 1);
            TxnBlock tb = new TxnBlock();
            try
            {
                GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                tb.IsoLevel = IsolationLevel.ReadCommitted;
               
                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                retError = GMDataManager.SetStopBossRaid(ref tb, bossID);
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
                if (queryFetcher.Render_errorFlag)
                {
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    string errorMsg = "alert('Fail');";
                    Page.ClientScript.RegisterStartupScript(GetType(), "alert", errorMsg, true);
                }
            }
        }
    }
}