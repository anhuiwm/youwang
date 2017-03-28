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
    public partial class user_invenList : System.Web.UI.Page
    {
        protected override void InitializeCulture()
        {
            UICulture = GMDataManager.GetGmToolWebLanguageCode();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            WebQueryParam queryFetcher = new WebQueryParam(true);
            string Username = queryFetcher.QueryParam_Fetch("username");
            string Uid = queryFetcher.QueryParam_Fetch("uid");
            string item = queryFetcher.QueryParam_Fetch("itemtype", "ItemClass_Equip");
            if (!Page.IsPostBack)
            {
                GetDataList(1);
            }
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
                    string Uid = queryFetcher.QueryParam_Fetch(uid.UniqueID);
                    string item = queryFetcher.QueryParam_Fetch(itemType.UniqueID, "ItemClass_Equip");
                    long reqidx = queryFetcher.QueryParam_FetchLong(idx.UniqueID);

                    if (!Page.IsPostBack)
                    {
                        Username = string.IsNullOrEmpty(Username) ? queryFetcher.QueryParam_Fetch("username") : Username;
                        Uid = string.IsNullOrEmpty(Uid) ? queryFetcher.QueryParam_Fetch("uid") : Uid;
                        item = string.IsNullOrEmpty(item) ? queryFetcher.QueryParam_Fetch("itemtype", "ItemClass_Equip") : item;
                    }
                    username.Text = Username;
                    uid.Text = Uid;
                    itemType.SelectedValue = item;

                    Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

                    if (!string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Uid))
                    {

                        long AID = 0;
                        if (!string.IsNullOrEmpty(Username))
                            AID = GMDataManager.GetSearchAID_BYUserName(ref tb, Username);
                        if (!string.IsNullOrEmpty(Uid))
                            AID = GMDataManager.GetSearchAID_BYSnailPlatformID(ref tb, Uid).AID;
                        
                        long totalCount = GMDataManager.GetUserInvenCount(ref tb, AID, GMData_Define.DeleteItemTypeList[item]);

                        List<GMUserInvenItem> logList = GMDataManager.GetUserInvenList(ref tb, AID, pageIndex, GMData_Define.DeleteItemTypeList[item]);
                        logList.ForEach(getItem =>
                        {
                            getItem.Name = string.IsNullOrEmpty(getItem.Name) ? getItem.Description : GMDataManager.GetItmeName(ref tb, getItem.Name);
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