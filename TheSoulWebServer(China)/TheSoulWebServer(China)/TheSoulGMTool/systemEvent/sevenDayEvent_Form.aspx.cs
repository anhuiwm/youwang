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

namespace TheSoulGMTool.systemEvent
{
    public partial class sevenDayEvent_Form : System.Web.UI.Page
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
            Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;

            string reqServer = queryFetcher.QueryParam_Fetch("serverid", "");
            long reqidx = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch("idx", "0"));
            long serverID = queryFetcher.QueryParam_FetchLong("select_server", 1);

            Dictionary<long, TxnBlock> TxnBlackServer = new Dictionary<long, TxnBlock>();
            TxnBlock tb = new TxnBlock();

            try
            {
                GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                TxnBlackServer.Add(serverID, tb);
                tb.IsoLevel = IsolationLevel.ReadCommitted;

                idx.Value = reqidx.ToString();

                string serverlist = GMDataManager.GetServerCheckList(ref tb, serverID);
                change_server.InnerHtml = serverlist;

                List<System_VIP_Level> vipLevelList = GMDataManager.GetSystemVIPLevelList(ref tb);
                if (!Page.IsPostBack)
                {
                    pageInit(ref tb, reqidx);
                }
                else
                {
                    if (!string.IsNullOrEmpty(reqServer))
                    {

                        string reqEventTitle = queryFetcher.QueryParam_Fetch(eventTitle.UniqueID, "");
                        string reqTooltip = queryFetcher.QueryParam_Fetch(eventTooltip.UniqueID, "");
                        string reqClear1 = queryFetcher.QueryParam_Fetch(clear_trigger1.UniqueID, "None");
                        int reqClear1Value1 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(clear1_1.UniqueID, "0"));
                        int reqClear1Value2 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(clear1_2.UniqueID, "0"));
                        int reqClear1Value3 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(clear1_3.UniqueID, "0"));
                        string reqClear2 = queryFetcher.QueryParam_Fetch(clear_trigger2.UniqueID, "None");
                        int reqClear2Value1 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(clear2_1.UniqueID, "0"));
                        int reqClear2Value2 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(clear2_2.UniqueID, "0"));
                        int reqClear2Value3 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(clear2_3.UniqueID, "0"));

                        long all_Reward1 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(all_reward1.UniqueID, "0"));
                        long all_Reward2 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(all_reward2.UniqueID, "0"));
                        long all_Reward3 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(all_reward3.UniqueID, "0"));
                        int all_rewardcntCnt1 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(all_rewardcnt1.UniqueID, "0"));
                        int all_rewardcntCnt2 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(all_rewardcnt2.UniqueID, "0"));
                        int all_rewardcntCnt3 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(all_rewardcnt3.UniqueID, "0"));
                        byte all_Grade1 = System.Convert.ToByte(queryFetcher.QueryParam_Fetch(all_grade1.UniqueID, "0"));
                        byte all_Grade2 = System.Convert.ToByte(queryFetcher.QueryParam_Fetch(all_grade2.UniqueID, "0"));
                        byte all_Grade3 = System.Convert.ToByte(queryFetcher.QueryParam_Fetch(all_grade3.UniqueID, "0"));

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
                        System_Event_7Day setData = new System_Event_7Day();
                        List<System_Event_7Day_Reward> rewardBox1 = new List<System_Event_7Day_Reward>();
                        List<System_Event_7Day_Reward> rewardBox2 = new List<System_Event_7Day_Reward>();
                        List<System_Event_7Day_Reward> rewardBox3 = new List<System_Event_7Day_Reward>();
                        List<System_Event_7Day_Reward> rewardBox4 = new List<System_Event_7Day_Reward>();

                        reqTooltip = reqTooltip.Replace("'", "''");
                        reqEventTitle = reqEventTitle.Replace("'", "''");

                        setData.Event_ID = reqidx;
                        setData.Event_Dev_Name = reqEventTitle;
                        setData.Event_Tooltip = reqTooltip;
                        setData.ClearTriggerType1 = reqClear1;
                        setData.ClearTriggerType1_Value1 = reqClear1Value1;
                        setData.ClearTriggerType1_Value2 = reqClear1Value2;
                        setData.ClearTriggerType1_Value3 = reqClear1Value3;
                        setData.ClearTriggerType2 = reqClear2;
                        setData.ClearTriggerType2_Value1 = reqClear2Value1;
                        setData.ClearTriggerType2_Value2 = reqClear2Value2;
                        setData.ClearTriggerType2_Value3 = reqClear2Value3;

                        System_Event dataInfo = GMDataManager.GetSystem_EventData(ref tb, reqidx);
                        byte itemIndexNum = 1;
                        if (all_Reward1 > 0 && all_rewardcntCnt1 > 0)
                        {
                            System_Event_7Day_Reward item = new System_Event_7Day_Reward();
                            item.RewardBoxID = dataInfo.Reward_Box1ID;
                            item.ItemIndex = itemIndexNum;
                            item.Item_ID = all_Reward1;
                            item.Item_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), all_Reward1)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), all_Reward1);
                            item.Item_Num = all_rewardcntCnt1;
                            item.Item_Grade = all_Grade1;
                            rewardBox1.Add(item);
                            itemIndexNum += 1;
                        }
                        if (all_Reward2 > 0 && all_rewardcntCnt2 > 0)
                        {
                            System_Event_7Day_Reward item = new System_Event_7Day_Reward();
                            item.RewardBoxID = dataInfo.Reward_Box1ID;
                            item.ItemIndex = itemIndexNum;
                            item.Item_ID = all_Reward2;
                            item.Item_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), all_Reward2)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), all_Reward2);
                            item.Item_Num = all_rewardcntCnt2;
                            item.Item_Grade = all_Grade2;
                            rewardBox1.Add(item);
                            itemIndexNum += 1;
                        }
                        if (all_Reward3 > 0 && all_rewardcntCnt3 > 0)
                        {
                            System_Event_7Day_Reward item = new System_Event_7Day_Reward();
                            item.RewardBoxID = dataInfo.Reward_Box1ID;
                            item.ItemIndex = itemIndexNum;
                            item.Item_ID = all_Reward3;
                            item.Item_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), all_Reward3)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), all_Reward3);
                            item.Item_Num = all_rewardcntCnt3;
                            item.Item_Grade = all_Grade3;
                            rewardBox1.Add(item);
                            itemIndexNum += 1;
                        }

                        if (reqidx > 0)
                        {
                            retError = GMDataManager.Update_Event_7Day(ref TxnBlackServer, setData, rewardBox1);
                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, reqidx, "", GMResult_Define.ControlType.SEVEN_EVENT_EDIT, queryFetcher.GetReqParams(), reqServer);
                        }
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
            if (retError == Result_Define.eResult.SUCCESS)
                Response.Redirect("sevenDayEvent.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID);
        }

        protected void pageInit(ref TxnBlock TB, long idx)
        {
            List<System_TriggerType> triggerTypeList = GMDataManager.GetSystemTriggerTypeList(ref TB);
            List<Admin_System_Item> allList = GMDataManager.GetNonEquip_Accessory_ItemList(ref TB);

            clear_trigger1.DataSource = triggerTypeList;
            clear_trigger1.DataTextField = "Trigger";
            clear_trigger1.DataValueField = "TriggerType";
            clear_trigger1.DataBind();
            clear_trigger2.DataSource = triggerTypeList;
            clear_trigger2.DataTextField = "Trigger";
            clear_trigger2.DataValueField = "TriggerType";
            clear_trigger2.DataBind();

            ListItem selectItem = new ListItem("select", "-1");
            
            all_reward1.DataSource = allList; all_reward1.DataTextField = "Description"; all_reward1.DataValueField = "Item_IndexID"; all_reward1.DataBind();
            all_reward1.Items.Insert(0, selectItem);
            all_reward2.DataSource = allList; all_reward2.DataTextField = "Description"; all_reward2.DataValueField = "Item_IndexID"; all_reward2.DataBind();
            all_reward2.Items.Insert(0, selectItem);
            all_reward3.DataSource = allList; all_reward3.DataTextField = "Description"; all_reward3.DataValueField = "Item_IndexID"; all_reward3.DataBind();
            all_reward3.Items.Insert(0, selectItem);
                        
            if (idx > 0)
            {
                System_Event_7Day dataInfo = TriggerManager.GetSystem_7Day_Event_Info(ref TB, idx, true);
                eventTitle.Text = dataInfo.Event_Dev_Name;
                eventTooltip.Text = dataInfo.Event_Tooltip;
                clear_trigger1.Value = dataInfo.ClearTriggerType1;
                clear1_1.Text = dataInfo.ClearTriggerType1_Value1.ToString();
                clear1_2.Text = dataInfo.ClearTriggerType1_Value2.ToString();
                clear1_3.Text = dataInfo.ClearTriggerType1_Value3.ToString();
                clear_trigger2.Value = dataInfo.ClearTriggerType2;
                clear2_1.Text = dataInfo.ClearTriggerType2_Value1.ToString();
                clear2_2.Text = dataInfo.ClearTriggerType2_Value2.ToString();
                clear2_3.Text = dataInfo.ClearTriggerType2_Value3.ToString();
                if (dataInfo.Reward_Box1ID > 0)
                {
                    List<System_Event_7Day_Reward> rewardList = TriggerManager.GetSystem_7Day_Event_Package_RewardBox(ref TB, dataInfo.Reward_Box1ID, true);
                    foreach (System_Event_7Day_Reward item in rewardList)
                    {
                        if (item.ItemIndex  == 1)
                        {
                            all_reward1.SelectedValue = item.Item_ID.ToString();
                            all_rewardcnt1.Text = item.Item_Num.ToString();
                            all_grade1.Text = item.Item_Grade.ToString();
                        }
                        else if (item.ItemIndex  == 2)
                        {
                            all_reward2.SelectedValue = item.Item_ID.ToString();
                            all_rewardcnt2.Text = item.Item_Num.ToString();
                            all_grade2.Text = item.Item_Grade.ToString();
                        }
                        else if (item.ItemIndex  == 3)
                        {
                            all_reward3.SelectedValue = item.Item_ID.ToString();
                            all_rewardcnt3.Text = item.Item_Num.ToString();
                            all_grade3.Text = item.Item_Grade.ToString();
                        }
                    }
                }
            }

        }
    }
}