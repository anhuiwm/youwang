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
    public partial class gmtoolLog : System.Web.UI.Page
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

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb);
                    string sdate = queryFetcher.QueryParam_Fetch(sDate.UniqueID, "");
                    string edate = queryFetcher.QueryParam_Fetch(eDate.UniqueID, "");
                    List<GM_ControlLog> logList = GMDataManager.GetGMToolLog(ref tb, sdate, edate);
                    logList.ForEach(item =>{
                        List<string> server = mJsonSerializer.JsonToObject<List<string>>(item.server_id);
                        string serverNames = "";
                        List<server_group_config> serverList = GlobalManager.GetServerGroupList(ref tb);
                        server.ForEach(serverItem =>
                        {
                            var findServer = serverList.Find(serverInfo => serverInfo.server_group_id.ToString() == serverItem && serverInfo.server_group_id > 0);
                            if (findServer != null)
                                serverNames = string.IsNullOrEmpty(serverNames) ? findServer.server_group_name : serverNames + ", " + findServer.server_group_name;
                            else
                            {
                                if(serverItem == "0")
                                    serverNames = string.IsNullOrEmpty(serverNames) ? GMData_Define.GlobalDBName : serverNames + ", " + GMData_Define.GlobalDBName;
                                else if(serverItem == "-1")
                                    serverNames = string.IsNullOrEmpty(serverNames) ? GMData_Define.GmDBName : serverNames + ", " + GMData_Define.GmDBName;
                            }
                        });
                        item.server_id = serverNames;
                    });
                    dataList.DataSource = logList;
                    dataList.DataBind();
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

        protected void dataList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dataList.PageIndex = e.NewPageIndex;
            dataList.DataBind();
        }
    }
}