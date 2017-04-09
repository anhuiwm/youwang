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
namespace TheSoulGMTool.kr
{
    public partial class userRestore : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //GetAllUserAccountConfig
            GetDeleteUSer();
        }

        protected void GetDeleteUSer()
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
            int ca2 = queryFetcher.QueryParam_FetchInt("ca2", 1);
            TxnBlock tb = new TxnBlock();

            try
            {
                GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                tb.IsoLevel = IsolationLevel.ReadCommitted;
                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                string Username = queryFetcher.QueryParam_Fetch(username.UniqueID, "");
                string Uid = queryFetcher.QueryParam_Fetch(uid.UniqueID, "");
                if (!string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Uid))
                {

                    long AID = 0;
                    if (!string.IsNullOrEmpty(Username))
                        AID = GMDataManager.GetSearchAID_BYUserName(ref tb, Username);
                    if (!string.IsNullOrEmpty(Uid))
                        AID = GMDataManager.GetSearchAID_BYSnailPlatformID(ref tb, Uid).AID;
                    if (AID > 0)
                    {
                        string platformID = GMDataManager.GetUserGloblaSimpleInfo(ref tb, AID).platform_user_id;
                        if (platformID.Contains("_#Deleted_"))
                        {
                            string[] setData = System.Text.RegularExpressions.Regex.Split(platformID, "_#Deleted_");
                            platformID = setData[0];
                        }
                        List<GM_user_account_config> list = GMDataManager.GetAllUserAccountConfig(ref tb, platformID);
                        dataList.DataSource = list;
                        dataList.DataBind();

                        queryFetcher.GM_Render(retError);
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
        }

        protected void account_restore_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            GridViewRow row = btn.NamingContainer as GridViewRow;
            long AID = System.Convert.ToInt64(dataList.DataKeys[row.RowIndex].Values[0]);
            string platformid = dataList.DataKeys[row.RowIndex].Values[1].ToString();
            int platform_type = System.Convert.ToInt32(dataList.DataKeys[row.RowIndex].Values[2]);
            //GlobalAccountDrop

            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
            int ca2 = queryFetcher.QueryParam_FetchInt("ca2", 1);
            TxnBlock tb = new TxnBlock();
            try
            {
                GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                tb.IsoLevel = IsolationLevel.ReadCommitted;
                user_account_config preUser = GlobalManager.GetUserAccountConfig(ref tb, GMDataManager.GetSearchAID_BYSnailPlatformID(ref tb, platformid).AID);
                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                retError = AccountManager.GlobalAccountDrop(ref tb, preUser.user_account_idx, preUser.platform_user_id, (Global_Define.ePlatformType)preUser.platform_type);
                if(retError ==  Result_Define.eResult.SUCCESS)
                    retError = AccountManager.GlobalmSeedPlatformID_Update(ref tb, AID, platformid, (Global_Define.ePlatformType)platform_type);
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
                if (queryFetcher.Render_errorFlag)
                {
                    GetDeleteUSer();
                }
                else
                {
                    string errorMsg = "alert('Fail');";
                    Page.ClientScript.RegisterStartupScript(GetType(), "alert", errorMsg, true);
                }
            }
        }
    }
}