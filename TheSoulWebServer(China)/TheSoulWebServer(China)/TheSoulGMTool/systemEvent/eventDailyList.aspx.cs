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
    public partial class eventDailyList : System.Web.UI.Page
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

                string setKey = "DAILY_ON_OFF";

                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                if (serverID > -1)
                {
                    string dbkey = GMData_Define.ShardingDBName;

                    int preOverDay = SystemData.AdminConstValueFetchFromRedis(ref tb, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.DAILY_ADD_MAX], dbkey, true);
                    int preOverRuby = SystemData.AdminConstValueFetchFromRedis(ref tb, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.DAILY_ADD_RUBY], dbkey, true);
                    labOverDay.Text = preOverDay.ToString();
                    labOverRuby.Text = preOverRuby.ToString();
                    int preActive = SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, dbkey, true);
                    string strActive = (preActive > 0) ? "O" : "X";
                    dailyOnOff.SelectedValue = preActive.ToString();
                    labActive.Text = strActive;

                    List<System_Event_Daily> DataList = TriggerManager.GetSystem_Event_Daily_List(ref tb, true);
                    DataList.ForEach(item => {
                        string description = "";
                        if (Trigger_Define.eEventLoopType.Even_Month == (Trigger_Define.eEventLoopType)item.Event_LoopType)
                            description = GetGlobalResourceObject("languageResource", "lang_evenNumer").ToString();
                        else if (Trigger_Define.eEventLoopType.Odd_Month == (Trigger_Define.eEventLoopType)item.Event_LoopType)
                            description = GetGlobalResourceObject("languageResource", "lang_oddNumber").ToString();
                        item.Reward_Mail_Subject_CN = string.Format("{0} ({1})", Enum.GetName(typeof(Trigger_Define.eEventLoopType), item.Event_LoopType), description);
                    });
                    dataList.DataSource = DataList;
                    dataList.DataBind();
                    string modeValue = queryFetcher.QueryParam_Fetch(mode.UniqueID, "");
                    if (!string.IsNullOrEmpty(modeValue))
                    {
                        int overday = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(ovuerDay.UniqueID, "0"));
                        int overruby = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(ovuerRuby.UniqueID, "0"));
                        int active = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(dailyOnOff.UniqueID, "0"));
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

                        if (overday > 0)
                            retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.DAILY_ADD_MAX], overday);

                        if (overruby > 0)
                            retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, GMData_Define.OpenTime_Const_Def_Key_List[GMData_Define.eOpenTimeConstDef.DAILY_ADD_RUBY], overruby);

                        if (!preActive.Equals(active))
                            retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, setKey, active);

                        if (overday == 0 && overruby == 0 && preActive.Equals(active)) // 아무변경이 없을때
                            retError = Result_Define.eResult.SUCCESS;
                        else
                            retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, 0, "", GMResult_Define.ControlType.DAILY_EVENT_EDIT, queryFetcher.GetReqParams(), reqServer);

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
    }
}