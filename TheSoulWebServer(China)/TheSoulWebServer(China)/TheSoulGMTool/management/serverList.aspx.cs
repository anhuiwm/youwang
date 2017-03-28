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

namespace TheSoulGMTool.management
{
    public partial class serverList : System.Web.UI.Page
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
            string reqServer = queryFetcher.QueryParam_Fetch("serverid", "");

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb);
                    tb.IsoLevel = IsolationLevel.ReadCommitted;

                    List<server_group_config_snail> datalist = GlobalManager.GetServerGroupAllList_Snail(ref tb).OrderBy(item => item.server_group_id).ToList();
                    datalist.RemoveAll(item => item.server_group_id == 0);
                    dataList.DataSource = datalist;
                    dataList.DataBind();
                }
                catch (Exception errorEx)
                {
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

        protected void dataList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dataList.PageIndex = e.NewPageIndex;
            dataList.DataBind();
        }

        protected void dataList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "deleteVersion")
            {
                int rowIndex = System.Convert.ToInt32(e.CommandArgument);
                long serverID = System.Convert.ToInt64(dataList.DataKeys[rowIndex].Values[0]);
                int billingPlatform = System.Convert.ToInt32(dataList.DataKeys[rowIndex].Values[1]);
                int version = System.Convert.ToInt32(dataList.DataKeys[rowIndex].Values[2]);

                //DeleteServerVersion
                WebQueryParam queryFetcher = new WebQueryParam(true);
                string retJson = "";
                
                TxnBlock tb = new TxnBlock();
                {
                    try
                    {
                        GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                        tb.IsoLevel = IsolationLevel.ReadCommitted;

                        Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                        retError = GMDataManager.DeleteServerVersion(ref tb, serverID, billingPlatform, version);
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
                    if (queryFetcher.Render_errorFlag)
                        Response.Redirect(Request.RawUrl);
                }
            }
        }

    }
}