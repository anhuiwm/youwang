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

namespace TheSoulGMTool.User
{
    public partial class paylog : System.Web.UI.Page
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
                    long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
                    GMDataManager.GetServerinit(ref tb, serverID);
                    long idx = queryFetcher.QueryParam_FetchLong("idx");
                    if (idx > 0)
                    {
                        GM_Billing_List getData = GMDataManager.GetBillingData(ref tb, idx);
                        AID.Text = getData.AID.ToString();
                        cid.Text = getData.CID.ToString();
                        uid.Text = getData.Billing_Token;
                        platform.Text = Enum.GetName(typeof(Shop_Define.eBillingType), getData.Buy_Platform);
                        regdate.Text = getData.regdate.ToString("yyyy-MM-dd hh:mm:ss");
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