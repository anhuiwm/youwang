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
    public partial class user_restrict : System.Web.UI.Page
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
            bool result = false;
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
                        long AID = 0;
                        string user = queryFetcher.QueryParam_Fetch(username.UniqueID, "");
                        long useraid = queryFetcher.QueryParam_FetchLong(userAID.UniqueID);
                        if (useraid == 0)
                            AID = GMDataManager.GetSearchAID_BYUserName(ref tb, user);
                        else
                            AID = useraid;
                        string logindate = queryFetcher.QueryParam_Fetch(loginDay.UniqueID);
                        int loginhour = queryFetcher.QueryParam_FetchInt(loginHour.UniqueID);
                        int loginmin = queryFetcher.QueryParam_FetchInt(loginMin.UniqueID);
                        string chatdate = queryFetcher.QueryParam_Fetch(chatDay.UniqueID);
                        int chathour = queryFetcher.QueryParam_FetchInt(chatHour.UniqueID);
                        int chatnmin = queryFetcher.QueryParam_FetchInt(chatMin.UniqueID);
                        string reqMemo = queryFetcher.QueryParam_Fetch(memo.UniqueID);
                        if (AID > 0)
                        {
                            int loginRestrict = 0;
                            int chatRestrict = 0;
                            string loginEndDate = "";
                            string chatEndDate = "";
                            if (!string.IsNullOrEmpty(logindate))
                            {
                                loginEndDate = string.Format("{0} {1}:{2}", logindate, loginhour, loginmin);
                                TimeSpan tsloginRestrict = System.Convert.ToDateTime(loginEndDate) - DateTime.Now;
                                loginRestrict = tsloginRestrict.TotalMinutes > 0 ? (int)tsloginRestrict.TotalMinutes : 0;
                            }
                            if (!string.IsNullOrEmpty(chatdate))
                            {
                                chatEndDate = string.Format("{0} {1}:{2}", chatdate, chathour, chatnmin);
                                TimeSpan tschatRestrict = System.Convert.ToDateTime(chatEndDate) - DateTime.Now;
                                chatRestrict = tschatRestrict.TotalMinutes > 0 ? (int)tschatRestrict.TotalMinutes : 0;
                            }

                            if (loginRestrict > 0 && retError == Result_Define.eResult.SUCCESS)
                                retError = GlobalManager.SetUserRestrict(ref tb, AID, loginRestrict, Global_Define.eAccountRestrictType.LOGIN);

                            if (chatRestrict > 0 && retError == Result_Define.eResult.SUCCESS)
                                retError = GlobalManager.SetUserRestrict(ref tb, AID, chatRestrict, Global_Define.eAccountRestrictType.CHAT);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                user_restrict_log setData = new user_restrict_log();
                                setData.user_account_idx = AID;
                                setData.memo = reqMemo.Replace("'", "''");
                                setData.login_restrict_enddate = string.IsNullOrEmpty(logindate) ? DateTime.Today.AddDays(-1) : System.Convert.ToDateTime(logindate);
                                setData.chat_restrict_endate = string.IsNullOrEmpty(chatdate) ? DateTime.Today.AddDays(-1) : System.Convert.ToDateTime(chatdate);
                                retError = GMDataManager.InsertUserRestrictLog(ref tb, setData);
                            }

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_USER, AID, AccountManager.GetSimpleAccountInfo(ref tb, AID).username, GMResult_Define.ControlType.USER_INFO_EDIT, queryFetcher.GetReqParams(), serverID.ToString());
                            queryFetcher.GM_Render(retError);

                            if (retError == Result_Define.eResult.SUCCESS)
                            {
                                List<server_config> serverInfo = GlobalManager.GetServerList(ref tb).FindAll(item => item.server_type == "cs_game");
                                serverInfo.ForEach(item =>
                                {
                                    if (loginRestrict > 0)
                                    {
                                        int port = item.server_private_port != null ? (int)item.server_private_port : 0;
                                        result = TCP_GM_Operation.User_LoginRestrict(item.server_private_ip, port, AID, System.Convert.ToInt64(loginRestrict));
                                        if (!result)
                                        {
                                            port = item.server_public_port != null ? (int)item.server_public_port : 0;
                                            result = TCP_GM_Operation.User_LoginRestrict(item.server_public_ip, port, AID, System.Convert.ToInt64(loginRestrict));
                                        }
                                        if (!result)
                                            msg = msg + item.server_group_id.ToString() + "Loin Restrict Fail.";
                                    }

                                    if (chatRestrict > 0)
                                    {
                                        int port = item.server_private_port != null ? (int)item.server_private_port : 0;
                                        result = TCP_GM_Operation.User_ChatRestrict(item.server_private_ip, port, AID, System.Convert.ToInt64(chatRestrict));
                                        if (!result)
                                        {
                                            port = item.server_public_port != null ? (int)item.server_public_port : 0;
                                            result = TCP_GM_Operation.User_ChatRestrict(item.server_public_ip, port, AID, System.Convert.ToInt64(chatRestrict));
                                        }
                                        if (!result)
                                            msg = msg + item.server_group_id.ToString() + " Chat Restrict Fail.";
                                    }
                                });
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
                        string AID = queryFetcher.QueryParam_Fetch("aid");
                        userAID.Text = AID;

                        List<ListItem> hourList = GMDataManager.GetHourList();
                        List<ListItem> minList = GMDataManager.GetMinList(1);
                        loginHour.DataSource = hourList;
                        loginHour.DataTextField = "Text";
                        loginHour.DataValueField = "Value";
                        loginHour.DataBind();

                        chatHour.DataSource = hourList;
                        chatHour.DataTextField = "Text";
                        chatHour.DataValueField = "Value";
                        chatHour.DataBind();

                        loginMin.DataSource = minList;
                        loginMin.DataTextField = "Text";
                        loginMin.DataValueField = "Value";
                        loginMin.DataBind();

                        chatMin.DataSource = minList;
                        chatMin.DataTextField = "Text";
                        chatMin.DataValueField = "Value";
                        chatMin.DataBind();
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
                        gmid = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
                    queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    tb.Dispose();
                }

                if (Page.IsPostBack)
                {

                    if (queryFetcher.Render_errorFlag)
                    {
                        if (string.IsNullOrEmpty(msg))
                            msg = "Success";
                        string alertmsg = "alert('" + msg + "');location.href='user_restrictList.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID+"';";
                        Page.ClientScript.RegisterStartupScript(GetType(), "alert", alertmsg, true);
                    }
                }
            }
        }
    }
}