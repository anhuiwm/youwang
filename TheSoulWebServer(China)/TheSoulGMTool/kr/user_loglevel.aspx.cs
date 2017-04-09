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
using TheSoul.DataManager.TCP_Packet;

namespace TheSoulGMTool.kr
{
    public partial class user_loglevel : System.Web.UI.Page
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
            Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
            bool result = true;
            string msg = "";
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
            TxnBlock tb = new TxnBlock();
            {
                try
                {

                    GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                    tb.IsoLevel = IsolationLevel.ReadCommitted;
                    if (Page.IsPostBack)
                    {
                        string user = queryFetcher.QueryParam_Fetch(username.UniqueID, "");
                        long AID = GMDataManager.GetSearchAID_BYUserName(ref tb, user);
                        string logdate = queryFetcher.QueryParam_Fetch(logDay.UniqueID);
                        int loghour = queryFetcher.QueryParam_FetchInt(logHour.UniqueID);
                        int logmin = queryFetcher.QueryParam_FetchInt(logMin.UniqueID);
                        byte log_level = queryFetcher.QueryParam_FetchByte(logLevel.UniqueID);
                        int cs_log = queryFetcher.QueryParam_FetchInt(csLogLevel.UniqueID);
                        
                        if (AID > 0)
                        {

                            DateTime logEndDate = System.Convert.ToDateTime(string.Format(string.Format("{0} {1}:{2}", logdate, loghour, logmin)));
                            retError = LogManager.SetAdminLogLevel(ref tb, AID, log_level, logEndDate);
                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_USER, AID, AccountManager.GetSimpleAccountInfo(ref tb, AID).username, GMResult_Define.ControlType.USER_INFO_EDIT, queryFetcher.GetReqParams(), serverID.ToString());
                            queryFetcher.GM_Render(retError);

                            if (retError == Result_Define.eResult.SUCCESS && cs_log > 0)
                            {
                                TimeSpan tsLogTime = System.Convert.ToDateTime(string.Format("{0} {1}:{2}", logdate, loghour, logmin)) - DateTime.Now;
                                long logTime = tsLogTime.TotalMinutes > 0 ? (int)tsLogTime.TotalMinutes : 0;

                                server_config serverInfo = GlobalManager.GetServerList(ref tb).Find(item => item.server_group_id == (int)serverID && item.server_type == "cs_game");
                                int port = serverInfo.server_private_port != null ? (int)serverInfo.server_private_port : 0;
                                result = TCP_GM_Operation.User_LogLevel(serverInfo.server_private_ip, port, AID, (TCP_GM_Operation.CS_LOG_LEVEL)cs_log, logTime);
                                if (!result)
                                {
                                    port = serverInfo.server_public_port != null ? (int)serverInfo.server_public_port : 0;
                                    result = TCP_GM_Operation.User_LogLevel(serverInfo.server_public_ip, port, AID, (TCP_GM_Operation.CS_LOG_LEVEL)cs_log, logTime);
                                }
                                if (!result)
                                    msg = "Log Level Setting Fail.";
                            }
                        }
                        else
                        {
                            string errorMsg = "alert('" + Resources.languageResource.lang_MsgUserNotFound + "');";
                            Page.ClientScript.RegisterStartupScript(GetType(), "alert", errorMsg, true);
                        }
                    }
                    else
                    {
                        List<ListItem> hourList = GMDataManager.GetHourList();
                        List<ListItem> minList = GMDataManager.GetMinList(1);
                        logHour.DataSource = hourList;
                        logHour.DataTextField = "Text";
                        logHour.DataValueField = "Value";
                        logHour.DataBind();
                        
                        logMin.DataSource = minList;
                        logMin.DataTextField = "Text";
                        logMin.DataValueField = "Value";
                        logMin.DataBind();

                        logLevel.DataSource = Enum.GetNames(typeof(Log_Define.eLogLevel)).Select(o => new { Text = Enum.Parse(typeof(Log_Define.eLogLevel), o), Value = (int)(Enum.Parse(typeof(Log_Define.eLogLevel), o)) });
                        logLevel.DataTextField = "Text";
                        logLevel.DataValueField = "Value";
                        logLevel.DataBind();

                        csLogLevel.DataSource = Enum.GetNames(typeof(TCP_GM_Operation.CS_LOG_LEVEL)).Select(o => new { Text =Enum.Parse(typeof(TCP_GM_Operation.CS_LOG_LEVEL), o), Value = (int)(Enum.Parse(typeof(TCP_GM_Operation.CS_LOG_LEVEL), o)) });
                        csLogLevel.DataTextField = "Text";
                        csLogLevel.DataValueField = "Value";
                        csLogLevel.DataBind();
                        ListItem selectItem = new ListItem("select", "-1");
                        csLogLevel.Items.RemoveAt(csLogLevel.Items.Count - 1);
                        logLevel.Items.Insert(0, selectItem);
                        csLogLevel.Items.Insert(0, selectItem);
                        
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

                if (Page.IsPostBack)
                {

                    if (queryFetcher.Render_errorFlag)
                    {
                        if (string.IsNullOrEmpty(msg))
                            msg = "Success";
                        string alertmsg = "alert('" + msg + "');location.href='user_logList.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID + "';";
                        Page.ClientScript.RegisterStartupScript(GetType(), "alert", alertmsg, true);
                    }
                }
            }
        }
    }
}