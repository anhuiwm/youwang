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
using TheSoul.DataManager.TCP_Packet;

namespace TheSoulGMTool.kr
{
    public partial class user_all_logout : System.Web.UI.Page
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
            
            
            long select_server = queryFetcher.QueryParam_FetchLong("select_server", 1);

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, ref queryFetcher);
                    tb.IsoLevel = IsolationLevel.ReadCommitted;
                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

                    if (!Page.IsPostBack)
                    {
                        List<server_group_config> serverGourpList = GlobalManager.GetServerGroupList(ref tb);
                        serverGourpList.RemoveAll(item => item.server_group_id == 0);
                        StringBuilder serverlist = new StringBuilder();
                        if(serverGourpList.Count > 0)
                            serverlist.Append("<input type=\"checkbox\" name=\"All_Server\" value=\"0\" onclick=\"serverChecked();\" /> All Server");
                        int serverCount = 1;
                        foreach (server_group_config server in serverGourpList)
                        {
                            serverlist.Append("<br /><input type=\"checkbox\" name=\"serverid\" value=\"" + server.server_group_id + "\" runat=\"server\" /> " + server.server_group_name + " ");
                            serverCount += 1;
                        }
                        change_server.InnerHtml = serverlist.ToString();
                    }
                    else
                    {
                        string strServerList = queryFetcher.QueryParam_Fetch("serverid", "");
                        string[] ServerList = System.Text.RegularExpressions.Regex.Split(strServerList, ",");
                        string failServer = "";
                        List<server_config> serverList = GlobalManager.GetServerList(ref tb);
                        foreach (string server in ServerList)
                        {
                            int serverID = System.Convert.ToInt32(server);
                            server_config serverInfo = serverList.Find(item => item.server_group_id == serverID && item.server_type=="cs_game");
                            if (serverInfo != null)
                            {
                                int port = serverInfo.server_private_port != null ? (int)serverInfo.server_private_port : 0;
                                bool result = TCP_GM_Operation.LogOutAllUser(serverInfo.server_private_ip, port);
                                //Response.Write(serverInfo.server_private_ip +", "+result+ "<br/>");
                                
                                if (!result)
                                {
                                    port = serverInfo.server_public_port != null ? (int)serverInfo.server_public_port : 0;
                                    result = TCP_GM_Operation.LogOutAllUser(serverInfo.server_public_ip, port);
                                    if (!result)
                                        failServer = string.IsNullOrEmpty(failServer) ? string.Format(@"{0}({1},{2})", GlobalManager.GetServerGroupConfig(ref tb, (long)serverID).server_group_name
                                                                                                                    , serverInfo.server_public_ip, port)  :
                                                                                        string.Format("{0},{1}({2},{3})", failServer, GlobalManager.GetServerGroupConfig(ref tb, (long)serverID).server_group_name
                                                                                                                        , serverInfo.server_public_ip, port);
                                    //else{
                                    //    Response.Write(serverInfo.server_public_ip + ", " + result + "<br/>");
                                    //}
                                }
                            }
                        }
                        retJson = queryFetcher.GM_Render(retError);
                        string errorMsg = string.IsNullOrEmpty(failServer) ? "alert('Logout SUCCESS');isFormSubmit=false;" : "alert('[" + failServer + "] Logout Fail');isFormSubmit=false;";
                        Page.ClientScript.RegisterStartupScript(GetType(), "alert", errorMsg, true);
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
                
            }
        }

    }
}