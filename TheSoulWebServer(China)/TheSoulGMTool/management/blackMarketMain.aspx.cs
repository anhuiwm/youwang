﻿using System;
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

namespace TheSoulGMTool.management
{
    public partial class blackMarketMain : System.Web.UI.Page
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
                    int restCashValue = SystemData.AdminConstValueFetchFromRedis(ref tb, "DEF_BLACKMARKET_SHOP_RESET_COST_RUBY", dbkey, true);
                    resetCash.Text = restCashValue.ToString();
                }
                else
                {
                    if (!string.IsNullOrEmpty(reqServer))
                    {

                        int resetcash = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(resetCash.UniqueID, "0"));
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
                        retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, "DEF_BLACKMARKET_SHOP_RESET_COST_RUBY", resetcash);
                        if (retError == Result_Define.eResult.SUCCESS)
                            retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, 0, "", GMResult_Define.ControlType.ETC, queryFetcher.GetReqParams(), reqServer);
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
                        tb = setItem.Value;
                        queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    }
                    setItem.Value.Dispose();
                }
            }

            if (result)
                Response.Redirect(Request.RawUrl);
        }
        
    }
}