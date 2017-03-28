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

namespace TheSoulGMTool.management
{
    public partial class itemChargeLog : System.Web.UI.Page
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
                    if (Page.IsPostBack)
                    {
                        long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
                        GMDataManager.GetServerinit(ref tb, serverID);

                        string sdate = queryFetcher.QueryParam_Fetch(sDate.UniqueID, "").Replace("/", "-");
                        string edate = queryFetcher.QueryParam_Fetch(eDate.UniqueID, "").Replace("/", "-");
                        List<GM_ItemChargeLog> logList = GMDataManager.GetItemChargeLog(ref tb, sdate, edate);
                        logList.ForEach(item =>
                            {
                                string itemName = "";
                                if (item.item_id_1 > 0)
                                {
                                    itemName = ItemManager.GetSystem_Item_Base(ref tb, item.item_id_1).Name;
                                    item.item_name_1 = GMDataManager.GetItmeName(ref tb, itemName);
                                }
                                if (item.item_id_2 > 0)
                                {
                                    itemName = ItemManager.GetSystem_Item_Base(ref tb, item.item_id_2).Name;
                                    item.item_name_2 = GMDataManager.GetItmeName(ref tb, itemName);
                                }
                                if (item.item_id_2 > 0)
                                {
                                    itemName = ItemManager.GetSystem_Item_Base(ref tb, item.item_id_3).Name;
                                    item.item_name_3 = GMDataManager.GetItmeName(ref tb, itemName);
                                }
                                if (item.item_id_4 > 0)
                                {
                                    itemName = ItemManager.GetSystem_Item_Base(ref tb, item.item_id_4).Name;
                                    item.item_name_4 = GMDataManager.GetItmeName(ref tb, itemName);
                                }
                                if (item.item_id_5 > 0)
                                {
                                    itemName = ItemManager.GetSystem_Item_Base(ref tb, item.item_id_5).Name;
                                    item.item_name_5 = GMDataManager.GetItmeName(ref tb, itemName);
                                }
                            }
                        );
                        dataList.DataSource = logList;
                        dataList.DataBind();
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

        protected void dataList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dataList.PageIndex = e.NewPageIndex;
            dataList.DataBind();
        }
    }
}