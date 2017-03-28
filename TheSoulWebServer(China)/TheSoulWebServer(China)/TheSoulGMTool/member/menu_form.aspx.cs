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

namespace TheSoulGMTool.member
{
    public partial class menu_form : System.Web.UI.Page
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
            int reqidx = queryFetcher.QueryParam_FetchInt("idx");
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
            TxnBlock tb = new TxnBlock();
            {
                try
                {
                    GMDataManager.GetServerinit(ref tb, ref queryFetcher);
                    if (reqidx > 0)
                    {
                        if (Page.IsPostBack)
                        {
                            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                            string menuName = queryFetcher.QueryParam_Fetch(menu.UniqueID, "None");
                            string isusing = queryFetcher.QueryParam_Fetch(active.UniqueID, "N");
                            int mdiv = queryFetcher.QueryParam_FetchInt(largeMenu.UniqueID);
                            int order = queryFetcher.QueryParam_FetchInt(orderNum.UniqueID);
                            retError = GMDataManager.SetMenu(ref tb, reqidx, menuName, mdiv, order, isusing);
                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, mdiv, "", GMResult_Define.ControlType.GMUSER_EDIT, queryFetcher.GetReqParams(), "-1");
                            retJson = queryFetcher.GM_Render(retError);
                        }
                        else
                        {
                            List<GM_menu_large> largeMenuList = GMDataManager.GetLargeMenu(ref tb);
                            largeMenu.DataSource = largeMenuList;
                            largeMenu.DataTextField = "menuname";
                            largeMenu.DataValueField = "idx";
                            largeMenu.DataBind();

                            GM_menu getData = GMDataManager.GetMenuData(ref tb, reqidx);
                            menu.Text = getData.menuname;
                            largeMenu.SelectedValue = getData.mdiv.ToString();
                            active.SelectedValue = getData.isusing;
                            orderNum.Text = getData.viewidx.ToString();
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
                if(queryFetcher.Render_errorFlag)
                    Response.Redirect("/member/menuList.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID);
            }
        }
    }
}