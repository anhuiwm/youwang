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
    public partial class lineNoticeForm : System.Web.UI.Page
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
            int reqIdx = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch("idx", "0"));
            int reqmode = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(mode.UniqueID, "0"));
            string reqServer = queryFetcher.QueryParam_Fetch("serverid", "");
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            Dictionary<long, TxnBlock> TxnBlackServer = new Dictionary<long, TxnBlock>();
            TxnBlock tb = new TxnBlock();
            try
            {
                GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                TxnBlackServer.Add(serverID, tb);
                tb.IsoLevel = IsolationLevel.ReadCommitted;

                idx.Value = reqIdx.ToString();
                string serverlist = GMDataManager.GetServerCheckList(ref tb, serverID);
                change_server.InnerHtml = serverlist;

                List<ListItem> hourList = GMDataManager.GetHourList();
                List<ListItem> minList = GMDataManager.GetMinList(1);
                sHour.DataSource = hourList;
                sHour.DataBind();

                eHour.DataSource = hourList;
                eHour.DataBind();

                sMin.DataSource = minList;
                sMin.DataBind();

                eMin.DataSource = minList;
                eMin.DataBind();

                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;

                if (reqIdx > 0)
                {
                    TheSoulGMTool.DBClass.Admin_Notice dataInfo = GMDataManager.GetNotice(ref tb, reqIdx);
                    sDate.Text = dataInfo.sdate.ToString("yyyy-MM-dd");
                    eDate.Text = dataInfo.edate.ToString("yyyy-MM-dd");
                    int hour = dataInfo.sdate.Hour;
                    int min = dataInfo.sdate.Minute;
                    if (hour < 10)
                        sHour.SelectedValue = "0" + hour;
                    else
                        sHour.SelectedValue = hour.ToString();
                    if (min < 10)
                        sMin.SelectedValue = "0" + min;
                    else
                        sMin.SelectedValue = min.ToString();
                    hour = dataInfo.edate.Hour;
                    min = dataInfo.edate.Minute;
                    if (hour < 10)
                        eHour.SelectedValue = "0" + hour;
                    else
                        eHour.SelectedValue = hour.ToString();
                    if (min < 10)
                        eMin.SelectedValue = "0" + min;
                    else
                        eMin.SelectedValue = min.ToString();
                    contents.Text = dataInfo.title;
                    int time = 0;
                    if (dataInfo.repeatTime > 0)
                    {
                        time = (int)dataInfo.repeatTime / 60;
                        type.SelectedValue = "2";
                    }
                    else
                    {
                        type.SelectedValue = "1";
                    }
                    displayTime.Text = time.ToString();
                }

                if (!string.IsNullOrEmpty(reqServer))
                {
                    string regid = "test";
                    if (!(HttpContext.Current.Request.Cookies.Count == 0))
                    {
                        regid = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
                    }
                    string sdate = queryFetcher.QueryParam_Fetch(sDate.UniqueID, DateTime.Today.ToString("yyyy-MM-dd"));
                    string edate = queryFetcher.QueryParam_Fetch(eDate.UniqueID, DateTime.Today.ToString("yyyy-MM-dd"));
                    string shour = queryFetcher.QueryParam_Fetch(sHour.UniqueID, "00");
                    string ehour = queryFetcher.QueryParam_Fetch(eHour.UniqueID, "00");
                    string smin = queryFetcher.QueryParam_Fetch(sMin.UniqueID, "00");
                    string emin = queryFetcher.QueryParam_Fetch(eMin.UniqueID, "00");
                    string con = queryFetcher.QueryParam_Fetch(contents.UniqueID, "");
                    short reqType = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch(type.UniqueID, "1"));
                    int time = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(displayTime.UniqueID, "0"));
                    if (shour.Equals("select"))
                        shour = "00";
                    if (smin.Equals("select"))
                        smin = "00";
                    if (ehour.Equals("select"))
                        ehour = "00";
                    if (emin.Equals("select"))
                        emin = "00";
                    string startDate = string.Format("{0} {1}:{2}:00", sdate, shour, smin);
                    string endDate = string.Format("{0} {1}:{2}:59", edate, ehour, emin);
                    if (reqType == 1)
                    {
                        time = 0;
                    }
                    else
                    {
                        if (time > 0)
                        {
                            time = time * 60;
                        }
                    }

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
                    if (reqIdx > 0)
                    {
                        if (reqmode == 1)
                        {
                            retError = GMDataManager.UpdateLineNotice(ref TxnBlackServer, reqIdx, startDate, endDate, reqType, con, regid, time);
                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, reqIdx, "", GMResult_Define.ControlType.LINE_NOTICE_EDIT, queryFetcher.GetReqParams(), reqServer);
                        }
                        else
                        {
                            retError = GMDataManager.DeleteNotice(ref TxnBlackServer, reqIdx, 2);
                        }
                    }
                    else
                    {
                        retError = GMDataManager.InsertLineNotice(ref TxnBlackServer, startDate, endDate, reqType, con, regid, time);
                    }
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
                            gmid = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
                        queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    }
                    setItem.Value.Dispose();
                }
            }

            if (result)
                Response.Redirect("/management/lineNoticeList.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID);
        }
    }
}