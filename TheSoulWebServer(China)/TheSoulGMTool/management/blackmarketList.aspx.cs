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

namespace TheSoulGMTool.management
{
    public partial class blackmarketList : System.Web.UI.Page
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
            int reqSlotID = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("slotid", "1"));
            Dictionary<long, TxnBlock> TxnBlackServer = new Dictionary<long, TxnBlock>();
            TxnBlock tb = new TxnBlock();

            try
            {
                GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                TxnBlackServer.Add(serverID, tb);
                tb.IsoLevel = IsolationLevel.ReadCommitted;
                slotID.Value = reqSlotID.ToString();
                string serverlist = GMDataManager.GetServerCheckList(ref tb, serverID);
                change_server.InnerHtml = serverlist;
                List<System_Shop_BlackMarket> DataList = GMDataManager.GetBlackMarketList(ref tb, reqSlotID);
                dataList.DataSource = DataList;
                dataList.DataBind();

                long goods_ID = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(goodsID.UniqueID, "0"));
                if (!string.IsNullOrEmpty(reqServer) && goods_ID > 0)
                {
                    Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
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
                    retError = GMDataManager.DeleteShopBlackMarket(ref TxnBlackServer, goods_ID);
                    if (retError == Result_Define.eResult.SUCCESS)
                        retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, goods_ID, "", GMResult_Define.ControlType.BLACKMARKET_DELETE, queryFetcher.GetReqParams(), reqServer);
                    if (retError == Result_Define.eResult.SUCCESS)
                        result = true;
                    retJson = queryFetcher.GM_Render("", retError);
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

    }
}