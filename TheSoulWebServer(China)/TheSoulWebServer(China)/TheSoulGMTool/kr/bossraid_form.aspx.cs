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
    public partial class bossraid_form : System.Web.UI.Page
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
            
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            //Dictionary<long, TxnBlock> TxnBlackServer = new Dictionary<long, TxnBlock>();
            TxnBlock tb = new TxnBlock();
            try
            {
                GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                tb.IsoLevel = IsolationLevel.ReadCommitted;
                Result_Define.eResult retError = Result_Define.eResult.SUCCESS;

                if (!Page.IsPostBack)
                {
                    List<ListItem> hourList = GMDataManager.GetHourList();
                    List<ListItem> minList = GMDataManager.GetMinList(1);
                    sHour.DataSource = hourList; sHour.DataTextField = "Text"; sHour.DataValueField = "Value"; sHour.DataBind();
                    eHour.DataSource = hourList; eHour.DataTextField = "Text"; eHour.DataValueField = "Value"; eHour.DataBind();

                    sMin.DataSource = minList; sMin.DataTextField = "Text"; sMin.DataValueField = "Value"; sMin.DataBind();
                    eMin.DataSource = minList; eMin.DataTextField = "Text"; eMin.DataValueField = "Value"; eMin.DataBind();
                    List<System_BOSS_RAID> bossList = GMDataManager.GetSystemBossRaidList(ref tb);
                    bossList.ForEach(item => {
                        System_NPC itemInfo = NPC_Manager.GetNPCInfo(ref tb, item.BossID);
                        item.Description = GMDataManager.GetItmeName(ref tb, itemInfo.NamingCN);
                    });

                    boss.DataSource = bossList;
                    boss.DataTextField = "Description";
                    boss.DataValueField = "DungeonID";
                    boss.DataBind();
                    boss.Items.Insert(0, new ListItem("select", "0"));
                }
                else{
                    string user = queryFetcher.QueryParam_Fetch(username.UniqueID, "EVENT");
                    int bossID = queryFetcher.QueryParam_FetchInt(boss.UniqueID);
                    string sdate = queryFetcher.QueryParam_Fetch(startDay.UniqueID, "");
                    string edate = queryFetcher.QueryParam_Fetch(endDay.UniqueID, "");
                    string shour = queryFetcher.QueryParam_Fetch(sHour.UniqueID, "");
                    string ehour = queryFetcher.QueryParam_Fetch(eHour.UniqueID, "");
                    string smin = queryFetcher.QueryParam_Fetch(sMin.UniqueID, "");
                    string emin = queryFetcher.QueryParam_Fetch(eMin.UniqueID, "");

                    string startDate = string.Format("{0} {1}:{2}:00", sdate, shour, smin);
                    string endDate = string.Format("{0} {1}:{2}:59", edate, ehour, emin);

                    if (bossID > 0)
                    {
                        System_BOSS_RAID bossRaidInfo = BossRaid.GetBossInfo(ref tb, bossID);
                        retError = GMDataManager.CreateBossRaid(ref tb, user, bossRaidInfo, startDate, endDate);
                        if (retError == Result_Define.eResult.SUCCESS)
                            BossRaid.CheckPublicRaid(ref tb);
                    }
                }
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
                //foreach (KeyValuePair<long, TxnBlock> setItem in TxnBlackServer)
                //{
                //    setItem.Value.EndTransaction(queryFetcher.Render_errorFlag);
                //    if (setItem.Key == serverID)
                //    {
                //        string gmid = "";
                //        if (Request.Cookies.Count > 0)
                //            gmid = GMDataManager.GetUserCookies("userid");
                //        queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                //    }
                //    setItem.Value.Dispose();
                //}

                tb.EndTransaction(queryFetcher.Render_errorFlag);
                string gmid = "";
                if (Request.Cookies.Count > 0)
                    gmid = GMDataManager.GetUserCookies("userid");
                queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                tb.Dispose();
            }

            if (result)
                Response.Redirect("bossraid_list.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID);
        }
    }
}