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
    public partial class snailIPList : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //GetSnailIPList
            WebQueryParam queryFetcher = new WebQueryParam(true);
            string retJson = "";
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, 1);

                    List<snail_ip_table> list = GlobalManager.GetSnailIPList(ref tb);
                    dataList.DataSource = list;
                    dataList.DataBind();
                }
                catch (Exception errorEx)
                {
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                }
                tb.EndTransaction(queryFetcher.Render_errorFlag);
                tb.Dispose();
            }
        }
        protected void dataList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dataList.PageIndex = e.NewPageIndex;
            dataList.DataBind();
        }

        protected void dataList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "deleteip")
            {
                int rowIndex = System.Convert.ToInt32(e.CommandArgument);
                int idx = System.Convert.ToInt32(dataList.DataKeys[rowIndex].Values[0]);

                //DeleteServerVersion
                WebQueryParam queryFetcher = new WebQueryParam(true);
                string retJson = "";

                TxnBlock tb = new TxnBlock();
                {
                    try
                    {

                        GMDataManager.GetServerinit(ref tb, 1);
                        tb.IsoLevel = IsolationLevel.ReadCommitted;
                        if (idx > 0)
                        {
                            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                            retError = GMDataManager.DeleteSnailIP(ref tb, idx);
                            queryFetcher.GM_Render(retError);
                        }
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
                    if (queryFetcher.Render_errorFlag)
                        Response.Redirect(Request.RawUrl);
                }
            }
        }
    }
}