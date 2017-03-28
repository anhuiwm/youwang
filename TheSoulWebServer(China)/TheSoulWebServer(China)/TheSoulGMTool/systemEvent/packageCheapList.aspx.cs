using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
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


namespace TheSoulGMTool.systemEvent
{
    public partial class packageCheapList : System.Web.UI.Page
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
            bool result = false;
            string reqServer = queryFetcher.QueryParam_Fetch("serverid", "");
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            Dictionary<long, TxnBlock> TxnBlackServer = new Dictionary<long, TxnBlock>();
            TxnBlock tb = new TxnBlock();

            try
            {
                GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                TxnBlackServer.Add(serverID, tb);
                tb.IsoLevel = IsolationLevel.ReadCommitted;
                string serverlist = GMDataManager.GetServerCheckList(ref tb, serverID);
                change_server.InnerHtml = serverlist;

                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                string dbkey = GMData_Define.ShardingDBName;
                if (!Page.IsPostBack)
                {
                    int gachaOnOff = SystemData.AdminConstValueFetchFromRedis(ref tb, "PACKAGE_CHEAP_ON_OFF", dbkey, true);
                    gachaonoff.SelectedValue = gachaOnOff.ToString();
                    List<GM_System_Package_List> datalist = GMDataManager.GetPackageCheapList(ref tb, dbkey);
                    datalist.ForEach(item => {
                        int index = datalist.IndexOf(item);
                        item.ToolTipCN = (index%2 == 0) ? "1" : "0";
                    });
                    datalist.RemoveAll(item => item.ToolTipCN.Equals("0"));
                    dataList.DataSource = datalist;
                    dataList.DataBind();
                }
                else
                {
                    if (!string.IsNullOrEmpty(reqServer))
                    {
                        string[] reqServerList = System.Text.RegularExpressions.Regex.Split(reqServer, ",");
                        foreach (string Key in reqServerList)
                        {
                            long ServerKey = System.Convert.ToInt64(Key);
                            if (!TxnBlackServer.ContainsKey(ServerKey))
                            {
                                TxnBlock tb2 = new TxnBlock();
                                TheSoulDBcon.GetInstance().TheSoulDBInitFromGlobal(ref tb2, (int)ServerKey, true);
                                TxnBlackServer.Add(ServerKey, tb2);
                            }
                        }
                        int gachaOnOff = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(gachaonoff.UniqueID, "0"));
                        retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, "PACKAGE_CHEAP_ON_OFF", gachaOnOff);
                        if (retError == Result_Define.eResult.SUCCESS)
                            retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, 0, "", GMResult_Define.ControlType.GAME_ONOFF_EDIT, queryFetcher.GetReqParams(), reqServer);
                        if (retError == Result_Define.eResult.SUCCESS)
                            result = true;
                        retJson = queryFetcher.GM_Render(retError);

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
                foreach (KeyValuePair<long, TxnBlock> setItem in TxnBlackServer)
                {
                    setItem.Value.EndTransaction(queryFetcher.Render_errorFlag);
                    if (setItem.Key == serverID)
                    {
                        string gmid = "";
                        if (Request.Cookies.Count > 0)
                            gmid = GMDataManager.GetUserCookies("userid");
                        queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    }
                    setItem.Value.Dispose();
                }
            }
            if (result)
                Response.Redirect(Request.RawUrl);
        }

        protected void dataList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dataList.PageIndex = e.NewPageIndex;
            dataList.DataBind();
        }

        protected void dataList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dv = e.Row.DataItem as DataRowView;

                e.Row.Cells[0].Text = (e.Row.DataItemIndex + 1).ToString();

            }
        }
    }
}