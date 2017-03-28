using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
    public partial class serverStateForm : System.Web.UI.Page
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
            bool result = false;
            long select_server = queryFetcher.QueryParam_FetchLong("select_server", 1);

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, ref queryFetcher);
                    tb.IsoLevel = IsolationLevel.ReadCommitted;
                    Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;

                    string dbkey = GMData_Define.ShardingDBName + select_server;
                    if (!Page.IsPostBack)
                    {
                        string serverlist = GMDataManager.GetServerCheckList(ref tb, select_server);
                        change_server.InnerHtml = serverlist;
                    }
                    else
                    {
                        string strServerList = queryFetcher.QueryParam_Fetch("serverid", "");
                        int state = queryFetcher.QueryParam_FetchInt(serverState.UniqueID, -1);
                        
                        string[] ServerList = System.Text.RegularExpressions.Regex.Split(strServerList, ",");
                        foreach (string server in ServerList)
                        {
                            long serverID = System.Convert.ToInt64(server);
                            server_group_config serverInfo = GlobalManager.GetServerGroupList(ref tb).Find(item => item.server_group_id == serverID);
                            if (serverInfo != null)
                            {
                                if (serverInfo.server_group_status < 20 && state >= 0)
                                {
                                    retError = GMDataManager.SetServerState(ref tb, serverID, (Global_Define.eServerStatus)state);
                                }
                            }
                        }
                        if (retError == Result_Define.eResult.SUCCESS)
                            retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, 0, "", GMResult_Define.ControlType.SERVER_STATE_EDIT, serverState.ToString(), strServerList);
                        if (retError == Result_Define.eResult.SUCCESS)
                            result = true;
                        retJson = queryFetcher.GM_Render("", retError);
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

                if (result)
                    Response.Redirect("serverState.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + select_server);
            }
        }

    }
}