using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using mSeed.RedisManager;
using mSeed.mDBTxnBlock;
using System.Data.SqlClient;
using System.Text;
using System.Data;
using TheSoul.DataManager;
using TheSoul.DataManager.DBClass;
using TheSoul.DataManager.Tools;
using TheSoul.DataManager.Global;
using TheSoulWebServer.Tools;
using TheSoulGMTool.DBClass;

namespace TheSoulGMTool.User
{
    public partial class user_hack : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, serverID);
                    tb.IsoLevel = IsolationLevel.ReadCommitted;

                    int checkCount = queryFetcher.QueryParam_FetchInt(check_count.UniqueID, queryFetcher.QueryParam_FetchInt("check_count",100));
                    check_count.Text = checkCount > 0 ? checkCount.ToString() : "";

                    List<User_HackDetect> datalist = SoulManager.GetUserHackDetectList(ref tb, 0, checkCount);
                    dataList.DataSource = datalist;
                    dataList.DataBind();
                }
                catch (Exception errorEx)
                {
                    Console.Write(errorEx.Message);
                }
                finally
                {

                    tb.EndTransaction(true);
                    tb.Dispose();
                }
            }
        }

        protected void dataList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = System.Convert.ToInt32(e.CommandArgument);
            long AID = System.Convert.ToInt64(dataList.DataKeys[rowIndex].Values[0]);
            if (e.CommandName == "restrict")
            {
                Response.Redirect(string.Format("/kr/user_restrict.aspx?{0}&aid={1}", Request.Url.Query, AID));
            }
            else if (e.CommandName == "delete")
            {
                WebQueryParam queryFetcher = new WebQueryParam(true);
                string retJson = "";

                TxnBlock tb = new TxnBlock();
                {
                    try
                    {

                        GMDataManager.GetServerinit(ref tb, ref queryFetcher);
                        tb.IsoLevel = IsolationLevel.ReadCommitted;

                        Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                        retError = SoulManager.DeleteUserHackDetect(ref tb, AID);
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
                    if(queryFetcher.Render_errorFlag)
                        Response.Redirect(string.Format("{0}?{1}&check_count={2}", Request.Url.AbsolutePath, Request.Url.Query, check_count.Text));
                }
            }
        }
    }
}