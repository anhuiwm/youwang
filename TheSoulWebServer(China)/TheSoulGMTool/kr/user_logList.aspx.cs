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
using TheSoul.DataManager.TCP_Packet;

namespace TheSoulGMTool.kr
{
    public partial class user_logList : System.Web.UI.Page
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
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
            long AID = queryFetcher.QueryParam_FetchLong(idx.UniqueID);

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, serverID);

                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;
                    
                    if (AID > 0)
                    {
                        DateTime logEndDate = DateTime.Now;
                        retError = LogManager.SetAdminLogLevel(ref tb, AID, 0, logEndDate);
                        if (retError == Result_Define.eResult.SUCCESS)
                        {
                            bool result = false;
                            server_config serverInfo = GlobalManager.GetServerList(ref tb).Find(item => item.server_group_id == (int)serverID && item.server_type == "cs_game");
                            int port = serverInfo.server_private_port != null ? (int)serverInfo.server_private_port : 0;
                            result = TCP_GM_Operation.User_LogLevel(serverInfo.server_private_ip, port, AID, TCP_GM_Operation.CS_LOG_LEVEL.CS_LOG_LEVEL_NONE, 0);
                            if (!result)
                            {
                                port = serverInfo.server_public_port != null ? (int)serverInfo.server_public_port : 0;
                                result = TCP_GM_Operation.User_LogLevel(serverInfo.server_public_ip, port, AID, TCP_GM_Operation.CS_LOG_LEVEL.CS_LOG_LEVEL_NONE, 0);
                            }
                            if (!result)
                            {
                                string errorMsg = "alert('Log Level Setting Fail');";
                                Page.ClientScript.RegisterStartupScript(GetType(), "alert", errorMsg, true);
                            }
                        }
                        idx.Value = "";
                    }
                    List<Gm_UserLogLevel> logList = GMDataManager.GetUserLogLevelList(ref tb);
                    dataList.DataSource = logList;
                    dataList.DataBind();
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
    }
}