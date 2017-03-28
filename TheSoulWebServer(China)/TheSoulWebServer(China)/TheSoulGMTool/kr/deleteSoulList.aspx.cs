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
    public partial class deleteSoulList : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
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
                    GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                    string sdate = queryFetcher.QueryParam_Fetch(sDate.UniqueID, "");
                    string edate = queryFetcher.QueryParam_Fetch(eDate.UniqueID, "");
                    string Username = queryFetcher.QueryParam_Fetch(username.UniqueID, "");
                    string Uid = queryFetcher.QueryParam_Fetch(uid.UniqueID, "");
                    long idx = queryFetcher.QueryParam_FetchLong(mailidx.UniqueID);
                    string startDate = queryFetcher.QueryParam_Fetch(sDate.UniqueID, DateTime.Today.ToString("yyyy-MM-dd")).Replace("/", "-");
                    string endDate = queryFetcher.QueryParam_Fetch(eDate.UniqueID, DateTime.Today.ToString("yyyy-MM-dd")).Replace("/", "-");
                    username.Text = Username;
                    uid.Text = Uid;
                    sDate.Text = startDate.ToString();
                    eDate.Text = endDate.ToString();
                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

                    if (!string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Uid))
                    {

                        long AID = 0;
                        if (!string.IsNullOrEmpty(Username))
                            AID = GMDataManager.GetSearchAID_BYUserName(ref tb, Username);
                        if (!string.IsNullOrEmpty(Uid))
                            AID = GMDataManager.GetSearchAID_BYSnailPlatformID(ref tb, Uid).AID;
                        if (idx > 0 && AID > 0)
                        {
                            //retError = GMDataManager.SetUserItemRestore(ref tb, AID, idx, GMData_Define.eUserInven.inven);
                            mailidx.Value = "";
                        }
                        long totalCount = GMDataManager.GetUserDeletePassiveSoulCount(ref tb, AID, startDate, endDate);

                        List<User_PassiveSoul> logList = GMDataManager.GetUserDeletePassiveSoulList(ref tb, AID, pageIndex, startDate, endDate);
                        logList.ForEach(item =>
                        {
                            System_Soul_Passive itemInfo = SoulManager.GetSoul_System_Soul_Passive(ref tb, item.soulid, false);
                            item.delflag = GMDataManager.GetItmeName(ref tb, itemInfo.NamingCN);
                            item.stateflag = item.class_type > 0 ? Character_Define.ClassEnumToType[(Character_Define.SystemClassType)item.class_type] : "";
                            item.class_type = itemInfo.Grade;
                        });
                        dataList.DataSource = logList;
                        dataList.DataBind();

                        GMDataManager.PopulatePager(ref dlPager, totalCount, pageIndex);
                        queryFetcher.GM_Render(retError);
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

        protected void Button1_Click(object sender, EventArgs e)
        {
            GetDataList(1);
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
    }
}