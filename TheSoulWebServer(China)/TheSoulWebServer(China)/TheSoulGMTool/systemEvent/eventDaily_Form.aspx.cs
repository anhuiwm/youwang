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
    public partial class eventDaily_Form : System.Web.UI.Page
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
            long reqidx = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("idx", "0"));
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);
            if (reqidx > 0)
            {
                Dictionary<long, TxnBlock> TxnBlackServer = new Dictionary<long, TxnBlock>();
                TxnBlock tb = new TxnBlock();

                GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                TxnBlackServer.Add(serverID, tb);
                try
                {
                    tb.IsoLevel = IsolationLevel.ReadCommitted;

                    eventIdx.Value = reqidx.ToString();
                    string serverlist = GMDataManager.GetServerCheckList(ref tb, serverID);
                    change_server.InnerHtml = serverlist;

                    Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                    if (serverID > -1 && reqidx > 0)
                    {
                        System_Event_Daily eventInfo = TriggerManager.GetSystem_Event_Daily(ref tb, reqidx);
                        if (!Page.IsPostBack)
                        {
                            pageInit(ref tb);

                            List<System_Event_Reward_Box> rewardInfo = TriggerManager.GetSystem_Event_Reward_Box_List(ref tb, eventInfo.Reward_Box1ID, true);
                            eventName.Text = eventInfo.Event_DevName;
                            eventType.Text = eventInfo.Event_Daily_Type;
                            loopType.Text = eventInfo.Event_Loop > 0 ? "O" : "X";
                            string description = "";
                            if (Trigger_Define.eEventLoopType.Even_Month == (Trigger_Define.eEventLoopType)eventInfo.Event_LoopType)
                                description = GetGlobalResourceObject("languageResource", "lang_evenNumer").ToString();
                            else if (Trigger_Define.eEventLoopType.Odd_Month == (Trigger_Define.eEventLoopType)eventInfo.Event_LoopType)
                                description = GetGlobalResourceObject("languageResource", "lang_oddNumber").ToString();
                            loop.Text = string.Format("{0} ({1})", ((Trigger_Define.eEventLoopType)eventInfo.Event_LoopType).ToString(), description);
                            vipLevel1.SelectedValue = eventInfo.Reward1_VIP_Level.ToString();
                            vipLevel2.SelectedValue = eventInfo.Reward2_VIP_Level.ToString();
                            vipLevel3.SelectedValue = eventInfo.Reward3_VIP_Level.ToString();
                            vipLevel4.SelectedValue = eventInfo.Reward4_VIP_Level.ToString();
                            vipLevel5.SelectedValue = eventInfo.Reward5_VIP_Level.ToString();
                            foreach (System_Event_Reward_Box item in rewardInfo)
                            {
                                if (item.BoxItemIndex == 1)
                                {
                                    reward1.SelectedValue = item.EventItem_ID.ToString();
                                    rewardcnt1.Text = item.EventItem_Num.ToString();
                                    grade1.Text = item.EventItem_Grade.ToString();
                                    vipLevel1.SelectedValue = item.VIP_Level.ToString();
                                }
                                else if (item.BoxItemIndex == 2)
                                {
                                    reward2.SelectedValue = item.EventItem_ID.ToString();
                                    rewardcnt2.Text = item.EventItem_Num.ToString();
                                    grade2.Text = item.EventItem_Grade.ToString();
                                    vipLevel2.SelectedValue = item.VIP_Level.ToString();
                                }
                                else if (item.BoxItemIndex == 3)
                                {
                                    reward3.SelectedValue = item.EventItem_ID.ToString();
                                    rewardcnt3.Text = item.EventItem_Num.ToString();
                                    grade3.Text = item.EventItem_Grade.ToString();
                                    vipLevel3.SelectedValue = item.VIP_Level.ToString();
                                }
                                else if (item.BoxItemIndex == 4)
                                {
                                    reward4.SelectedValue = item.EventItem_ID.ToString();
                                    rewardcnt4.Text = item.EventItem_Num.ToString();
                                    grade4.Text = item.EventItem_Grade.ToString();
                                    vipLevel5.SelectedValue = item.VIP_Level.ToString();
                                }
                                else if (item.BoxItemIndex == 5)
                                {
                                    reward5.SelectedValue = item.EventItem_ID.ToString();
                                    rewardcnt5.Text = item.EventItem_Num.ToString();
                                    grade5.Text = item.EventItem_Grade.ToString();
                                    vipLevel5.SelectedValue = item.VIP_Level.ToString();
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(reqServer))
                            {
                                string reqEventType = queryFetcher.QueryParam_Fetch(eventType.UniqueID, "");
                                short reqloopType = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch(loopType.UniqueID, "0"));
                                short reqloop = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch(loop.UniqueID, "0"));
                                int reqvip1 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(vipLevel1.UniqueID, "0"));
                                int reqvip2 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(vipLevel2.UniqueID, "0"));
                                int reqvip3 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(vipLevel3.UniqueID, "0"));
                                int reqvip4 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(vipLevel4.UniqueID, "0"));
                                int reqvip5 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(vipLevel5.UniqueID, "0"));
                                long reqReward1 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(reward1.UniqueID, "0"));
                                long reqReward2 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(reward2.UniqueID, "0"));
                                long reqReward3 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(reward3.UniqueID, "0"));
                                long reqReward4 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(reward4.UniqueID, "0"));
                                long reqReward5 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(reward5.UniqueID, "0"));
                                int reqRewardcnt1 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(rewardcnt1.UniqueID, "0"));
                                int reqRewardcnt2 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(rewardcnt2.UniqueID, "0"));
                                int reqRewardcnt3 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(rewardcnt3.UniqueID, "0"));
                                int reqRewardcnt4 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(rewardcnt4.UniqueID, "0"));
                                int reqRewardcnt5 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(rewardcnt5.UniqueID, "0"));
                                byte reqGrade1 = System.Convert.ToByte(queryFetcher.QueryParam_Fetch(grade1.UniqueID, "1"));
                                byte reqGrade2 = System.Convert.ToByte(queryFetcher.QueryParam_Fetch(grade2.UniqueID, "1"));
                                byte reqGrade3 = System.Convert.ToByte(queryFetcher.QueryParam_Fetch(grade3.UniqueID, "1"));
                                byte reqGrade4 = System.Convert.ToByte(queryFetcher.QueryParam_Fetch(grade4.UniqueID, "1"));
                                byte reqGrade5 = System.Convert.ToByte(queryFetcher.QueryParam_Fetch(grade5.UniqueID, "1"));

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
                                List<System_Event_Reward_Box> reqItemList = new List<System_Event_Reward_Box>();
                                byte itemIndexNum = 1;
                                if (reqReward1 > 0 && reqRewardcnt1 > 0)
                                {
                                    System_Event_Reward_Box item = new System_Event_Reward_Box();
                                    item.EventBoxID = eventInfo.Reward_Box1ID;
                                    item.BoxItemIndex = itemIndexNum;
                                    item.EventItem_ID = reqReward1;
                                    item.VIP_Level = reqvip1;
                                    item.EventItem_Grade = reqGrade1;
                                    item.EventItem_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), reqReward1)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), reqReward1);
                                    item.EventItem_Num = reqRewardcnt1;
                                    reqItemList.Add(item);
                                    itemIndexNum += 1;
                                }
                                if (reqReward2 > 0 && reqRewardcnt2 > 0)
                                {
                                    System_Event_Reward_Box item = new System_Event_Reward_Box();
                                    item.EventBoxID = eventInfo.Reward_Box1ID;
                                    item.BoxItemIndex = itemIndexNum;
                                    item.EventItem_ID = reqReward2;
                                    item.VIP_Level = reqvip2;
                                    item.EventItem_Grade = reqGrade2;
                                    item.EventItem_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), reqReward2)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), reqReward2);
                                    item.EventItem_Num = reqRewardcnt2;
                                    reqItemList.Add(item);
                                    itemIndexNum += 1;
                                }
                                if (reqReward3 > 0 && reqRewardcnt3 > 0)
                                {
                                    System_Event_Reward_Box item = new System_Event_Reward_Box();
                                    item.EventBoxID = eventInfo.Reward_Box1ID;
                                    item.BoxItemIndex = itemIndexNum;
                                    item.EventItem_ID = reqReward3;
                                    item.VIP_Level = reqvip3;
                                    item.EventItem_Grade = reqGrade3;
                                    item.EventItem_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), reqReward3)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), reqReward3);
                                    item.EventItem_Num = reqRewardcnt3;
                                    reqItemList.Add(item);
                                    itemIndexNum += 1;
                                }
                                if (reqReward4 > 0 && reqRewardcnt4 > 0)
                                {
                                    System_Event_Reward_Box item = new System_Event_Reward_Box();
                                    item.EventBoxID = eventInfo.Reward_Box1ID;
                                    item.BoxItemIndex = itemIndexNum;
                                    item.EventItem_ID = reqReward4;
                                    item.VIP_Level = reqvip4;
                                    item.EventItem_Grade = reqGrade4;
                                    item.EventItem_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), reqReward4)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), reqReward4);
                                    item.EventItem_Num = reqRewardcnt4;
                                    reqItemList.Add(item);
                                    itemIndexNum += 1;
                                }
                                if (reqReward5 > 0 && reqRewardcnt5 > 0)
                                {
                                    System_Event_Reward_Box item = new System_Event_Reward_Box();
                                    item.EventBoxID = eventInfo.Reward_Box1ID;
                                    item.BoxItemIndex = itemIndexNum;
                                    item.EventItem_ID = reqReward5;
                                    item.VIP_Level = reqvip5;
                                    item.EventItem_Grade = reqGrade5;
                                    item.EventItem_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), reqReward5)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), reqReward5);
                                    item.EventItem_Num = reqRewardcnt5;
                                    reqItemList.Add(item);
                                    itemIndexNum += 1;
                                }

                                retError = GMDataManager.UpdateDailyEvent(ref TxnBlackServer, reqidx, reqvip1, reqvip2, reqvip3, reqvip4, reqvip5);
                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = GMDataManager.UpdateEventRewardBox(ref TxnBlackServer, eventInfo.Reward_Box1ID, reqItemList);
                                if (retError == Result_Define.eResult.SUCCESS)
                                    retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, reqidx, "", GMResult_Define.ControlType.DAILY_EVENT_EDIT, queryFetcher.GetReqParams(), reqServer);
                                if (retError == Result_Define.eResult.SUCCESS)
                                {
                                    result = true;
                                }
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
                    Response.Redirect("eventDailyList.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID);
            }
        }

        protected void pageInit(ref TxnBlock TB)
        {
            List<System_VIP_Level> vipLevelList = GMDataManager.GetSystemVIPLevelList(ref TB);

            List<Admin_System_Item> rewardList = GMDataManager.GetNonEquip_Accessory_ItemList(ref TB);
            ListItem selectItem = new ListItem("select", "-1");
            vipLevel1.DataSource = vipLevelList;
            vipLevel1.DataTextField = "VIP_Level";
            vipLevel1.DataValueField = "VIP_Level";
            vipLevel1.DataBind();
            vipLevel1.Items.Insert(0, selectItem);

            vipLevel2.DataSource = vipLevelList;
            vipLevel2.DataTextField = "VIP_Level";
            vipLevel2.DataValueField = "VIP_Level";
            vipLevel2.DataBind();
            vipLevel2.Items.Insert(0, selectItem);

            vipLevel3.DataSource = vipLevelList;
            vipLevel3.DataTextField = "VIP_Level";
            vipLevel3.DataValueField = "VIP_Level";
            vipLevel3.DataBind();
            vipLevel3.Items.Insert(0, selectItem);

            vipLevel4.DataSource = vipLevelList;
            vipLevel4.DataTextField = "VIP_Level";
            vipLevel4.DataValueField = "VIP_Level";
            vipLevel4.DataBind();
            vipLevel4.Items.Insert(0, selectItem);

            vipLevel5.DataSource = vipLevelList;
            vipLevel5.DataTextField = "VIP_Level";
            vipLevel5.DataValueField = "VIP_Level";
            vipLevel5.DataBind();
            vipLevel5.Items.Insert(0, selectItem);

            reward1.DataSource = rewardList;
            reward1.DataTextField = "Description";
            reward1.DataValueField = "Item_IndexID";
            reward1.DataBind();
            reward1.Items.Insert(0, selectItem);

            reward2.DataSource = rewardList;
            reward2.DataTextField = "Description";
            reward2.DataValueField = "Item_IndexID";
            reward2.DataBind();
            reward2.Items.Insert(0, selectItem);

            reward3.DataSource = rewardList;
            reward3.DataTextField = "Description";
            reward3.DataValueField = "Item_IndexID";
            reward3.DataBind();
            reward3.Items.Insert(0, selectItem);

            reward4.DataSource = rewardList;
            reward4.DataTextField = "Description";
            reward4.DataValueField = "Item_IndexID";
            reward4.DataBind();
            reward4.Items.Insert(0, selectItem);

            reward5.DataSource = rewardList;
            reward5.DataTextField = "Description";
            reward5.DataValueField = "Item_IndexID";
            reward5.DataBind();
            reward5.Items.Insert(0, selectItem);
        }
    }
}