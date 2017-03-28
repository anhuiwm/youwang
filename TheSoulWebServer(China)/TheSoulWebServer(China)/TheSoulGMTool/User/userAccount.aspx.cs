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

namespace TheSoulGMTool.User
{
    public partial class userAccount : System.Web.UI.Page
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
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
                    GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                    tb.IsoLevel = IsolationLevel.ReadCommitted;
                    if (Page.IsPostBack)
                    {
                        string platform_id = queryFetcher.QueryParam_Fetch(platform.UniqueID, "");
                        long AID = GMDataManager.GetSearchAID_BYSnailPlatformID(ref tb, platform_id).AID;
                        if (AID > 0)
                        {
                            string set_platform_id = string.Format("Admin_Change_{0}_{1}", (int)GenericFetch.ConvertToMSeedTime(), platform_id);
                            retError = AccountManager.GlobalPlatformID_Unlink(ref tb, set_platform_id, AID);
                            if(retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_USER, AID, AccountManager.GetSimpleAccountInfo(ref tb, AID).username, GMResult_Define.ControlType.USER_INFO_EDIT, queryFetcher.GetReqParams(), serverID.ToString());
                            queryFetcher.GM_Render(retError);
                        }
                        else
                        {
                            string errorMsg = "alert('" + Resources.languageResource.lang_MsgUserNotFound + "');";
                            Page.ClientScript.RegisterStartupScript(GetType(), "alert", errorMsg, true);
                        }
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
                    string msg = "";
                    if (queryFetcher.Render_errorFlag)
                    {
                        platform.Text = "";
                        msg = "alert('" + Resources.languageResource.lang_MsgSuccse + "');";
                        Page.ClientScript.RegisterStartupScript(GetType(), "alert", msg, true);
                    }
                    else
                    {
                        msg = "alert('" + Resources.languageResource.lang_MsgFail + "');";
                        Page.ClientScript.RegisterStartupScript(GetType(), "alert", msg, true);
                    }
                    
                }
            }
        }
    }
}