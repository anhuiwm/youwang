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
    public partial class user_achieve : System.Web.UI.Page
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
            int achieveType = queryFetcher.QueryParam_FetchInt("achieveType", 2);
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
                        User_Event_Data userData = TriggerManager.GetUser_Achieve_Data(ref tb, AID, reqidx);
                        data_user_1.Text = userData.CurrentValue1.ToString();
                        data_user_2.Text = userData.CurrentValue2.ToString();
                        data_clear_1.Text = string.Format(@"clear : {0}<br/>value1 : {1}<br/>value2 : {2}<br/>value3 : {3}"
                                                                    , GMDataManager.GetSystemTriggerType(ref tb, userData.ClearTriggerType1).Trigger
                                                                    , userData.ClearTriggerType1_Value1, userData.ClearTriggerType1_Value2, userData.ClearTriggerType1_Value3);
                        data_clear_2.Text = string.Format(@"clear : {0}<br/>value1 : {1}<br/>value2 : {2}<br/>value3 : {3}"
                                                                    , GMDataManager.GetSystemTriggerType(ref tb, userData.ClearTriggerType2).Trigger
                                                                    , userData.ClearTriggerType2_Value1, userData.ClearTriggerType2_Value2, userData.ClearTriggerType2_Value3);
                        clear_value1.Value = userData.ClearTriggerType1_Value3.ToString();
                        clear_value2.Value = userData.ClearTriggerType2_Value3.ToString(); ;
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
                        long CurrentValue1 = queryFetcher.QueryParam_FetchLong(clear1.UniqueID);
                        long CurrentValue2 = queryFetcher.QueryParam_FetchLong(clear2.UniqueID);

                        User_Event_Data userData = TriggerManager.GetUser_Achieve_Data(ref tb, AID, reqidx);

                        List<TriggerProgressData> setTriggerList = new List<TriggerProgressData>();
                        setTriggerList.Add(new TriggerProgressData(Trigger_Define.TriggerType[userData.ClearTriggerType1], userData.ClearTriggerType1_Value1, userData.ClearTriggerType1_Value2, CurrentValue1));
                        setTriggerList.Add(new TriggerProgressData(Trigger_Define.TriggerType[userData.ClearTriggerType2], userData.ClearTriggerType2_Value1, userData.ClearTriggerType2_Value2, CurrentValue2));

                        List<User_Event_Data> userEventList = TriggerManager.Check_Event_Data_List(ref tb, AID);
                        List<User_Event_Data> userAchieveList = TriggerManager.Check_Achieve_Data_List(ref tb, AID);
                        List<User_Event_Data> userAchievePvPList = TriggerManager.Check_Achieve_PvP_Data_List(ref tb, AID);

                        retError = TriggerManager.ProgressTrigger(ref tb, ref userEventList, ref userAchieveList, ref userAchievePvPList, AID, setTriggerList);
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
                    gmid = GMDataManager.GetUserCookies("userid");
                queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                tb.Dispose();
            }

            if (queryFetcher.Render_errorFlag)
                Response.Redirect("user_achieveList.aspx?ca2=" + ca2 + "&select_server=" + serverID + "&username=" + Username + "&uid=" + Uid + "&achieveType=" + achieveType);
        }
    }
}