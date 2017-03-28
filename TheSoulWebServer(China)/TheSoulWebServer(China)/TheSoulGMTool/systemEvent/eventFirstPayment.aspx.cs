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
    public partial class eventFirstPayment : System.Web.UI.Page
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
                string setKey = "FIRST_PAYMENT_ON_OFF";
                if (serverID > -1)
                {
                    if (!Page.IsPostBack)
                    {

                        System_Event_First_Payment eventInfo = TriggerManager.GetSystem_Event_First_Payment(ref tb, true);
                        //eventInfo.Reward_Box1ID
                        List<System_Event_Reward_Box> rewardList = TriggerManager.GetSystem_Event_Reward_Box_List(ref tb, eventInfo.Reward_Box1ID, true);
                        string strReward = "";
                        foreach (System_Event_Reward_Box item in rewardList)
                        {
                            //item.EventItem_ID
                            System_Item_Base itemInfo = ItemManager.GetSystem_Item_Base(ref tb, item.EventItem_ID);
                            if (itemInfo.Item_IndexID > 0)
                            {
                                if (string.IsNullOrEmpty(strReward))
                                    strReward = GMDataManager.GetItmeName(ref tb, itemInfo.Name) + " " + item.EventItem_Num;
                                else
                                    strReward = strReward + "<br>" + GMDataManager.GetItmeName(ref tb, itemInfo.Name) + " " + item.EventItem_Num;
                            }
                        }

                        int isActive = SystemData.AdminConstValueFetchFromRedis(ref tb, setKey, GMData_Define.ShardingDBName, true);
                        activeValue.SelectedValue = isActive.ToString();
                        title.Text = eventInfo.Reward_Mail_Subject_CN;
                        contents.Text = eventInfo.Reward_Mail_Text_CN;
                        labReward.Text = strReward;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(reqServer))
                        {
                            //활성화 변경!!!
                            int active = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(activeValue.UniqueID, "0"));
                            string reqTitle = queryFetcher.QueryParam_Fetch(title.UniqueID, "");
                            string reqMsg = queryFetcher.QueryParam_Fetch(contents.UniqueID, "");
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
                            retError = GMDataManager.UpdateFirstPaymentEvent(ref TxnBlackServer, active, reqTitle, reqMsg);
                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.SetAdminConstValue(ref TxnBlackServer, setKey, active);

                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, 0, "", GMResult_Define.ControlType.FIRTS_EVENT_EDIT, queryFetcher.GetReqParams(), reqServer);
                            if (retError == Result_Define.eResult.SUCCESS)
                                result = true;
                            retJson = queryFetcher.GM_Render("", retError);
                        }
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
    }
}