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

namespace TheSoulGMTool.kr
{
    public partial class user_restrrict_log : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                GetDataList(1);
        }

        protected void GetDataList(int pageIndex)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            queryFetcher.bDBLog = true;
            string retJson = "";
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, serverID);
                    string Username = queryFetcher.QueryParam_Fetch(username.UniqueID, "");
                    long AID = queryFetcher.QueryParam_FetchLong(aid.UniqueID);

                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

                    if (!string.IsNullOrEmpty(Username) && AID == 0)
                        AID = GMDataManager.GetSearchAID_BYUserName(ref tb, Username);

                    long totalCount = GMDataManager.GetUserRestrictLogCount(ref tb, AID);

                    List<user_restrict_log> logList = GMDataManager.GetUserRestrictLogList(ref tb, pageIndex, AID);
                    logList.ForEach(item =>
                    {
                        string userInfo = "";
                        List<user_play_server_config> userPlayServer = GMDataManager.GetUserPlayServerList(ref tb, item.user_account_idx);
                        userPlayServer.ForEach(playItem => {
                            userInfo = string.IsNullOrEmpty(userInfo) ? string.Format("{0} : {1}", playItem.server_group_name, playItem.user_server_nickname) : string.Format("{0}<br>{1} : {2}", userInfo, playItem.server_group_name, playItem.user_server_nickname);
                        });
                        item.userInfo = userInfo;
                    });

                    dataList.DataSource = logList;
                    dataList.DataBind();

                    GMDataManager.PopulatePager(ref dlPager, totalCount, pageIndex);
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
                }
            }
        }

        protected void dlPager_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "PageNo")
            {
                int page = System.Convert.ToInt32(e.CommandArgument);
                this.GetDataList(page);
            }
        }

        public override void VerifyRenderingInServerForm(System.Web.UI.Control control)
        {
            // Confirms that an HtmlForm control is rendered for the specified ASP.NET server control at run time.
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            GetDataList(1);
        }
    }
}