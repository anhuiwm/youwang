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
    public partial class user_invenForm : System.Web.UI.Page
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
            long reqidx = queryFetcher.QueryParam_FetchLong("idx");
            string Username = queryFetcher.QueryParam_Fetch("username");
            string Uid = queryFetcher.QueryParam_Fetch("uid");
            string itemtype = queryFetcher.QueryParam_Fetch("itemtype");
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
            int ca2 = queryFetcher.QueryParam_FetchInt("ca2", 1);
            idx.Value = reqidx.ToString();

            TxnBlock tb = new TxnBlock();

            try
            {
                GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                tb.IsoLevel = IsolationLevel.ReadCommitted;

                string serverlist = GMDataManager.GetServerCheckList(ref tb, serverID);

                if (!Page.IsPostBack)
                {
                    //이벤트 데이터
                    long AID = 0;
                    if (!string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Uid))
                    {
                        if (!string.IsNullOrEmpty(Username))
                            AID = GMDataManager.GetSearchAID_BYUserName(ref tb, Username);
                        if (!string.IsNullOrEmpty(Uid))
                            AID = GMDataManager.GetSearchAID_BYSnailPlatformID(ref tb, Uid).AID;
                    }
                    if (AID > 0)
                    {
                        GMUserInvenItem userData = GMDataManager.GetUserInven(ref tb, AID, reqidx, GMData_Define.DeleteItemTypeList[itemtype]);
                        itemInfo.Text = string.Format("{0} (Grade : {1} / Level : {2}) {3}", userData.Name = string.IsNullOrEmpty(userData.Name) ? userData.Description : GMDataManager.GetItmeName(ref tb, userData.Name), userData.grade, userData.level, userData.itemea);
                        item_count.Value = userData.itemea.ToString();
                    }
                }
                else
                {
                    Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                    long AID = 0;
                    if (!string.IsNullOrEmpty(Username) || !string.IsNullOrEmpty(Uid))
                    {
                        if (!string.IsNullOrEmpty(Username))
                            AID = GMDataManager.GetSearchAID_BYUserName(ref tb, Username);
                        if (!string.IsNullOrEmpty(Uid))
                            AID = GMDataManager.GetSearchAID_BYSnailPlatformID(ref tb, Uid).AID;
                    }
                    if (AID > 0)
                    {
                        int itemCount = queryFetcher.QueryParam_FetchInt(setData.UniqueID);
                        if(itemCount > 0)
                            retError = GMDataManager.DeleteUserItem(ref tb, reqidx, itemCount, GMData_Define.DeleteItemTypeList[itemtype]);
                    }
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
                    gmid = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
                queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                tb.Dispose();
            }

            if (queryFetcher.Render_errorFlag)
                Response.Redirect("user_invenList.aspx?ca2=" + ca2 + "&select_server=" + serverID + "&username=" + Username + "&uid=" + Uid + "&itemtype=" + itemtype);
        }
    }
}