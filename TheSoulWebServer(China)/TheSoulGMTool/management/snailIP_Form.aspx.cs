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

namespace TheSoulGMTool.management
{
    public partial class snailIP_Form : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);

            string retJson = "";
            int reqidx = queryFetcher.QueryParam_FetchInt("idx");
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, serverID);
                    if (reqidx > 0)
                    {
                        if (Page.IsPostBack)
                        {
                            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                            string strIP1 = queryFetcher.QueryParam_Fetch(ip1.UniqueID, "0");
                            string strIP2 = queryFetcher.QueryParam_Fetch(ip2.UniqueID, "0");
                            string strIP3 = queryFetcher.QueryParam_Fetch(ip3.UniqueID, "0");
                            string strIP4 = queryFetcher.QueryParam_Fetch(ip4.UniqueID, "0");
                            string IP = string.Format("{0}.{1}.{2}.{3}", strIP1, strIP2, strIP3, strIP4);

                            retError = GMDataManager.InsertSnailIP(ref tb, IP);
                            
                            retJson = queryFetcher.GM_Render(retError);
                        }
                        
                    }
                }
                catch (Exception errorEx)
                {
                    retJson = queryFetcher.Render<ErrorReturnString>(new ErrorReturnString(errorEx.Message), Result_Define.eResult.System_Exception);
                }
                tb.EndTransaction(queryFetcher.Render_errorFlag);
                tb.Dispose();
                if (queryFetcher.Render_errorFlag)
                    Response.Redirect("/management/snailIPList.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID);
            }
        }
    }
}