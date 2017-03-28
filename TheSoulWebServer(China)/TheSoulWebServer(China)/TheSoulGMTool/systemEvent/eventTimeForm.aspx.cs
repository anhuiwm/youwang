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
    public partial class eventTimeForm : System.Web.UI.Page
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
            long reqidx = queryFetcher.QueryParam_FetchLong("idx");
            string eventType = queryFetcher.QueryParam_Fetch("eventtype", "");
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

                if (!Page.IsPostBack)
                {
                    pageInit(ref tb, reqidx);
                }
                else
                {
                    if (!string.IsNullOrEmpty(reqServer))
                    {
                        Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                        string sdate = queryFetcher.QueryParam_Fetch(startDay.UniqueID, "");
                        string edate = queryFetcher.QueryParam_Fetch(endDay.UniqueID, "");
                        string shour = queryFetcher.QueryParam_Fetch(startHour.UniqueID, "");
                        string ehour = queryFetcher.QueryParam_Fetch(endHour.UniqueID, "");
                        string smin = queryFetcher.QueryParam_Fetch(startMin.UniqueID, "");
                        string emin = queryFetcher.QueryParam_Fetch(endMin.UniqueID, "");

                        int all_vip1 = queryFetcher.QueryParam_FetchInt(all_vipLevel1.UniqueID);
                        int all_vip2 = queryFetcher.QueryParam_FetchInt(all_vipLevel2.UniqueID);
                        int all_vip3 = queryFetcher.QueryParam_FetchInt(all_vipLevel3.UniqueID);
                        int all_vip4 = queryFetcher.QueryParam_FetchInt(all_vipLevel4.UniqueID);
                        int all_vip5 = queryFetcher.QueryParam_FetchInt(all_vipLevel5.UniqueID);
                        long all_Reward1 = queryFetcher.QueryParam_FetchLong(all_reward1.UniqueID);
                        long all_Reward2 = queryFetcher.QueryParam_FetchLong(all_reward2.UniqueID);
                        long all_Reward3 = queryFetcher.QueryParam_FetchLong(all_reward3.UniqueID);
                        long all_Reward4 = queryFetcher.QueryParam_FetchLong(all_reward4.UniqueID);
                        long all_Reward5 = queryFetcher.QueryParam_FetchLong(all_reward5.UniqueID);
                        int all_rewardcntCnt1 = queryFetcher.QueryParam_FetchInt(all_rewardcnt1.UniqueID);
                        int all_rewardcntCnt2 = queryFetcher.QueryParam_FetchInt(all_rewardcnt2.UniqueID);
                        int all_rewardcntCnt3 = queryFetcher.QueryParam_FetchInt(all_rewardcnt3.UniqueID);
                        int all_rewardcntCnt4 = queryFetcher.QueryParam_FetchInt(all_rewardcnt4.UniqueID);
                        int all_rewardcntCnt5 = queryFetcher.QueryParam_FetchInt(all_rewardcnt5.UniqueID);
                        byte all_Grade1 = queryFetcher.QueryParam_FetchByte(all_grade1.UniqueID);
                        byte all_Grade2 = queryFetcher.QueryParam_FetchByte(all_grade2.UniqueID);
                        byte all_Grade3 = queryFetcher.QueryParam_FetchByte(all_grade3.UniqueID);
                        byte all_Grade4 = queryFetcher.QueryParam_FetchByte(all_grade4.UniqueID);
                        byte all_Grade5 = queryFetcher.QueryParam_FetchByte(all_grade5.UniqueID);

                        int warrior_vip1 = queryFetcher.QueryParam_FetchInt(warrior_vipLevel1.UniqueID);
                        int warrior_vip2 = queryFetcher.QueryParam_FetchInt(warrior_vipLevel2.UniqueID);
                        int warrior_vip3 = queryFetcher.QueryParam_FetchInt(warrior_vipLevel3.UniqueID);
                        int warrior_vip4 = queryFetcher.QueryParam_FetchInt(warrior_vipLevel4.UniqueID);
                        int warrior_vip5 = queryFetcher.QueryParam_FetchInt(warrior_vipLevel5.UniqueID);
                        long warrior_Reward1 = queryFetcher.QueryParam_FetchLong(warrior_reward1.UniqueID);
                        long warrior_Reward2 = queryFetcher.QueryParam_FetchLong(warrior_reward2.UniqueID);
                        long warrior_Reward3 = queryFetcher.QueryParam_FetchLong(warrior_reward3.UniqueID);
                        long warrior_Reward4 = queryFetcher.QueryParam_FetchLong(warrior_reward4.UniqueID);
                        long warrior_Reward5 = queryFetcher.QueryParam_FetchLong(warrior_reward5.UniqueID);
                        byte warrior_Level1 = queryFetcher.QueryParam_FetchByte(warrior_level1.UniqueID);
                        byte warrior_Level2 = queryFetcher.QueryParam_FetchByte(warrior_level2.UniqueID);
                        byte warrior_Level3 = queryFetcher.QueryParam_FetchByte(warrior_level3.UniqueID);
                        byte warrior_Level4 = queryFetcher.QueryParam_FetchByte(warrior_level4.UniqueID);
                        byte warrior_Level5 = queryFetcher.QueryParam_FetchByte(warrior_level5.UniqueID);
                        byte warrior_Grade1 = queryFetcher.QueryParam_FetchByte(warrior_grade1.UniqueID);
                        byte warrior_Grade2 = queryFetcher.QueryParam_FetchByte(warrior_grade2.UniqueID);
                        byte warrior_Grade3 = queryFetcher.QueryParam_FetchByte(warrior_grade3.UniqueID);
                        byte warrior_Grade4 = queryFetcher.QueryParam_FetchByte(warrior_grade4.UniqueID);
                        byte warrior_Grade5 = queryFetcher.QueryParam_FetchByte(warrior_grade5.UniqueID);

                        int sword_vip1 = queryFetcher.QueryParam_FetchInt(sword_vipLevel1.UniqueID);
                        int sword_vip2 = queryFetcher.QueryParam_FetchInt(sword_vipLevel2.UniqueID);
                        int sword_vip3 = queryFetcher.QueryParam_FetchInt(sword_vipLevel3.UniqueID);
                        int sword_vip4 = queryFetcher.QueryParam_FetchInt(sword_vipLevel4.UniqueID);
                        int sword_vip5 = queryFetcher.QueryParam_FetchInt(sword_vipLevel5.UniqueID);
                        long sword_Reward1 = queryFetcher.QueryParam_FetchLong(sword_reward1.UniqueID);
                        long sword_Reward2 = queryFetcher.QueryParam_FetchLong(sword_reward2.UniqueID);
                        long sword_Reward3 = queryFetcher.QueryParam_FetchLong(sword_reward3.UniqueID);
                        long sword_Reward4 = queryFetcher.QueryParam_FetchLong(sword_reward4.UniqueID);
                        long sword_Reward5 = queryFetcher.QueryParam_FetchLong(sword_reward5.UniqueID);
                        byte sword_Level1 = queryFetcher.QueryParam_FetchByte(sword_level1.UniqueID);
                        byte sword_Level2 = queryFetcher.QueryParam_FetchByte(sword_level2.UniqueID);
                        byte sword_Level3 = queryFetcher.QueryParam_FetchByte(sword_level3.UniqueID);
                        byte sword_Level4 = queryFetcher.QueryParam_FetchByte(sword_level4.UniqueID);
                        byte sword_Level5 = queryFetcher.QueryParam_FetchByte(sword_level5.UniqueID);
                        byte sword_Grade1 = queryFetcher.QueryParam_FetchByte(sword_grade1.UniqueID);
                        byte sword_Grade2 = queryFetcher.QueryParam_FetchByte(sword_grade2.UniqueID);
                        byte sword_Grade3 = queryFetcher.QueryParam_FetchByte(sword_grade3.UniqueID);
                        byte sword_Grade4 = queryFetcher.QueryParam_FetchByte(sword_grade4.UniqueID);
                        byte sword_Grade5 = queryFetcher.QueryParam_FetchByte(sword_grade5.UniqueID);

                        int taoist_vip1 = queryFetcher.QueryParam_FetchInt(taoist_vipLevel1.UniqueID);
                        int taoist_vip2 = queryFetcher.QueryParam_FetchInt(taoist_vipLevel2.UniqueID);
                        int taoist_vip3 = queryFetcher.QueryParam_FetchInt(taoist_vipLevel3.UniqueID);
                        int taoist_vip4 = queryFetcher.QueryParam_FetchInt(taoist_vipLevel4.UniqueID);
                        int taoist_vip5 = queryFetcher.QueryParam_FetchInt(taoist_vipLevel5.UniqueID);
                        long taoist_Reward1 = queryFetcher.QueryParam_FetchLong(taoist_reward1.UniqueID);
                        long taoist_Reward2 = queryFetcher.QueryParam_FetchLong(taoist_reward2.UniqueID);
                        long taoist_Reward3 = queryFetcher.QueryParam_FetchLong(taoist_reward3.UniqueID);
                        long taoist_Reward4 = queryFetcher.QueryParam_FetchLong(taoist_reward4.UniqueID);
                        long taoist_Reward5 = queryFetcher.QueryParam_FetchLong(taoist_reward5.UniqueID);
                        byte taoist_Level1 = queryFetcher.QueryParam_FetchByte(taoist_level1.UniqueID);
                        byte taoist_Level2 = queryFetcher.QueryParam_FetchByte(taoist_level2.UniqueID);
                        byte taoist_Level3 = queryFetcher.QueryParam_FetchByte(taoist_level3.UniqueID);
                        byte taoist_Level4 = queryFetcher.QueryParam_FetchByte(taoist_level4.UniqueID);
                        byte taoist_Level5 = queryFetcher.QueryParam_FetchByte(taoist_level5.UniqueID);
                        byte taoist_Grade1 = queryFetcher.QueryParam_FetchByte(taoist_grade1.UniqueID);
                        byte taoist_Grade2 = queryFetcher.QueryParam_FetchByte(taoist_grade2.UniqueID);
                        byte taoist_Grade3 = queryFetcher.QueryParam_FetchByte(taoist_grade3.UniqueID);
                        byte taoist_Grade4 = queryFetcher.QueryParam_FetchByte(taoist_grade4.UniqueID);
                        byte taoist_Grade5 = queryFetcher.QueryParam_FetchByte(taoist_grade5.UniqueID);

                        string reqTitle = queryFetcher.QueryParam_Fetch(eventTitle.UniqueID, "");
                        //string reqTooltip = queryFetcher.QueryParam_Fetch(eventTooltip.UniqueID, "");
                        reqTitle = reqTitle.Replace("'", "''");
                        //reqTooltip = reqTooltip.Replace("'", "''");

                        string startDate = string.Format("{0} {1}:{2}:00", sdate, shour, smin);
                        string endDate = string.Format("{0} {1}:{2}:59", edate, ehour, emin);

                        List<System_Event_Reward_Box> rewardBox1 = new List<System_Event_Reward_Box>();
                        List<System_Event_Reward_Box> rewardBox2 = new List<System_Event_Reward_Box>();
                        List<System_Event_Reward_Box> rewardBox3 = new List<System_Event_Reward_Box>();
                        List<System_Event_Reward_Box> rewardBox4 = new List<System_Event_Reward_Box>();
                        System_Event dataInfo = GMDataManager.GetSystem_EventData(ref tb, reqidx);
                        byte itemIndexNum = 1;
                        if (all_Reward1 > 0 && all_rewardcntCnt1 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box1ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = all_Reward1;
                            item.VIP_Level = all_vip1;
                            item.EventItem_TargetType = Enum.GetName(typeof(GMData_Define.eRewardTye), all_Reward1);
                            item.EventItem_Num = all_rewardcntCnt1;
                            item.EventItem_Grade = all_Grade1;
                            rewardBox1.Add(item);
                            itemIndexNum += 1;
                        }
                        if (all_Reward2 > 0 && all_rewardcntCnt2 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box1ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = all_Reward2;
                            item.VIP_Level = all_vip2;
                            item.EventItem_TargetType = Enum.GetName(typeof(GMData_Define.eRewardTye), all_Reward2);
                            item.EventItem_Num = all_rewardcntCnt2;
                            item.EventItem_Grade = all_Grade2;
                            rewardBox1.Add(item);
                            itemIndexNum += 1;
                        }
                        if (all_Reward3 > 0 && all_rewardcntCnt3 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box1ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = all_Reward3;
                            item.VIP_Level = all_vip3;
                            item.EventItem_TargetType = Enum.GetName(typeof(GMData_Define.eRewardTye), all_Reward3);
                            item.EventItem_Num = all_rewardcntCnt3;
                            item.EventItem_Grade = all_Grade3;
                            rewardBox1.Add(item);
                            itemIndexNum += 1;
                        }
                        if (all_Reward4 > 0 && all_rewardcntCnt4 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box1ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = all_Reward4;
                            item.VIP_Level = all_vip4;
                            item.EventItem_TargetType = Enum.GetName(typeof(GMData_Define.eRewardTye), all_Reward4);
                            item.EventItem_Num = all_rewardcntCnt4;
                            item.EventItem_Grade = all_Grade4;
                            rewardBox1.Add(item);
                            itemIndexNum += 1;
                        }
                        if (all_Reward5 > 0 && all_rewardcntCnt5 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box1ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = all_Reward5;
                            item.VIP_Level = all_vip5;
                            item.EventItem_TargetType = Enum.GetName(typeof(GMData_Define.eRewardTye), all_Reward5);
                            item.EventItem_Num = all_rewardcntCnt5;
                            item.EventItem_Grade = all_Grade5;
                            rewardBox1.Add(item);
                            itemIndexNum += 1;
                        }

                        itemIndexNum = 1;

                        if (warrior_Reward1 > 0 && warrior_Grade1 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box2ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = warrior_Reward1;
                            item.VIP_Level = warrior_vip1;
                            item.EventItem_Grade = warrior_Grade1;
                            item.EventItem_Level = warrior_Level1;
                            item.EventItem_TargetType = "Item";
                            item.EventItem_Num = 1;
                            rewardBox2.Add(item);
                            itemIndexNum += 1;
                        }
                        if (warrior_Reward2 > 0 && warrior_Grade2 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box2ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = warrior_Reward2;
                            item.VIP_Level = warrior_vip2;
                            item.EventItem_Grade = warrior_Grade2;
                            item.EventItem_Level = warrior_Level2;
                            item.EventItem_TargetType = "Item";
                            item.EventItem_Num = 1;
                            rewardBox2.Add(item);
                            itemIndexNum += 1;
                        }
                        if (warrior_Reward3 > 0 && warrior_Grade3 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box2ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = warrior_Reward3;
                            item.VIP_Level = warrior_vip3;
                            item.EventItem_Grade = warrior_Grade3;
                            item.EventItem_Level = warrior_Level3;
                            item.EventItem_TargetType = "Item";
                            item.EventItem_Num = 1;
                            rewardBox2.Add(item);
                            itemIndexNum += 1;
                        }
                        if (warrior_Reward4 > 0 && warrior_Grade4 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box2ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = warrior_Reward4;
                            item.VIP_Level = warrior_vip4;
                            item.EventItem_Grade = warrior_Grade4;
                            item.EventItem_Level = warrior_Level4;
                            item.EventItem_TargetType = "Item";
                            item.EventItem_Num = 1;
                            rewardBox2.Add(item);
                            itemIndexNum += 1;
                        }
                        if (warrior_Reward5 > 0 && warrior_Grade5 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box2ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = warrior_Reward5;
                            item.VIP_Level = warrior_vip5;
                            item.EventItem_Grade = warrior_Grade5;
                            item.EventItem_Level = warrior_Level5;
                            item.EventItem_TargetType = "Item";
                            item.EventItem_Num = 1;
                            rewardBox2.Add(item);
                            itemIndexNum += 1;
                        }

                        itemIndexNum = 1;

                        if (sword_Reward1 > 0 && sword_Grade1 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box3ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = sword_Reward1;
                            item.VIP_Level = sword_vip1;
                            item.EventItem_Grade = sword_Grade1;
                            item.EventItem_Level = sword_Level1;
                            item.EventItem_TargetType = "Item";
                            item.EventItem_Num = 1;
                            rewardBox3.Add(item);
                            itemIndexNum += 1;
                        }
                        if (sword_Reward2 > 0 && sword_Grade2 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box3ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = sword_Reward2;
                            item.VIP_Level = sword_vip2;
                            item.EventItem_Grade = sword_Grade2;
                            item.EventItem_Level = sword_Level2;
                            item.EventItem_TargetType = "Item";
                            item.EventItem_Num = 1;
                            rewardBox3.Add(item);
                            itemIndexNum += 1;
                        }
                        if (sword_Reward3 > 0 && sword_Level3 > 0 && sword_Grade3 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box3ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = sword_Reward3;
                            item.VIP_Level = sword_vip3;
                            item.EventItem_Grade = sword_Grade3;
                            item.EventItem_Level = sword_Level3;
                            item.EventItem_TargetType = "Item";
                            item.EventItem_Num = 1;
                            rewardBox3.Add(item);
                            itemIndexNum += 1;
                        }
                        if (sword_Reward4 > 0 && sword_Grade4 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box3ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = sword_Reward4;
                            item.VIP_Level = sword_vip4;
                            item.EventItem_Grade = sword_Grade4;
                            item.EventItem_Level = sword_Level4;
                            item.EventItem_TargetType = "Item";
                            item.EventItem_Num = 1;
                            rewardBox3.Add(item);
                            itemIndexNum += 1;
                        }
                        if (sword_Reward5 > 0 && sword_Grade5 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box3ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = sword_Reward5;
                            item.VIP_Level = sword_vip5;
                            item.EventItem_Grade = sword_Grade5;
                            item.EventItem_Level = sword_Level5;
                            item.EventItem_TargetType = "Item";
                            item.EventItem_Num = 1;
                            rewardBox3.Add(item);
                            itemIndexNum += 1;
                        }

                        itemIndexNum = 1;

                        if (taoist_Reward1 > 0 && taoist_Grade1 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box4ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = taoist_Reward1;
                            item.VIP_Level = taoist_vip1;
                            item.EventItem_Grade = taoist_Grade1;
                            item.EventItem_Level = taoist_Level1;
                            item.EventItem_TargetType = "Item";
                            item.EventItem_Num = 1;
                            rewardBox4.Add(item);
                            itemIndexNum += 1;
                        }
                        if (taoist_Reward2 > 0 && taoist_Grade2 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box4ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = taoist_Reward2;
                            item.VIP_Level = taoist_vip2;
                            item.EventItem_Grade = taoist_Grade2;
                            item.EventItem_Level = taoist_Level2;
                            item.EventItem_TargetType = "Item";
                            item.EventItem_Num = 1;
                            rewardBox4.Add(item);
                            itemIndexNum += 1;
                        }
                        if (taoist_Reward3 > 0 && taoist_Level3 > 0 && taoist_Grade3 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box4ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = taoist_Reward3;
                            item.VIP_Level = taoist_vip3;
                            item.EventItem_Grade = taoist_Grade3;
                            item.EventItem_Level = taoist_Level3;
                            item.EventItem_TargetType = "Item";
                            item.EventItem_Num = 1;
                            rewardBox4.Add(item);
                            itemIndexNum += 1;
                        }
                        if (taoist_Reward4 > 0 && taoist_Grade4 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box4ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = taoist_Reward4;
                            item.VIP_Level = taoist_vip4;
                            item.EventItem_Grade = taoist_Grade4;
                            item.EventItem_Level = taoist_Level4;
                            item.EventItem_TargetType = "Item";
                            item.EventItem_Num = 1;
                            rewardBox4.Add(item);
                            itemIndexNum += 1;
                        }
                        if (taoist_Reward5 > 0 && taoist_Grade5 > 0)
                        {
                            System_Event_Reward_Box item = new System_Event_Reward_Box();
                            item.EventBoxID = dataInfo.Reward_Box4ID;
                            item.BoxItemIndex = itemIndexNum;
                            item.EventItem_ID = taoist_Reward5;
                            item.VIP_Level = taoist_vip5;
                            item.EventItem_Grade = taoist_Grade5;
                            item.EventItem_Level = taoist_Level5;
                            item.EventItem_TargetType = "Item";
                            item.EventItem_Num = 1;
                            rewardBox4.Add(item);
                            itemIndexNum += 1;
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

                        retError = GMDataManager.SetEventDate(ref TxnBlackServer, reqidx, reqTitle, reqTitle, startDate, endDate, dataInfo, rewardBox1, rewardBox2, rewardBox3, rewardBox4);
                        if (retError == Result_Define.eResult.SUCCESS)
                            retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, reqidx, "", GMResult_Define.ControlType.EVENT_EDIT, queryFetcher.GetReqParams(), reqServer);
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
                Response.Redirect("eventList.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID + "&group=1&eventType=" + eventType);
        }

        protected void pageInit(ref TxnBlock TB, long idx)
        {
            List<ListItem> hourList = GMDataManager.GetHourList();
            List<ListItem> minList = GMDataManager.GetMinList(1);

            List<System_VIP_Level> vipLevelList = GMDataManager.GetSystemVIPLevelList(ref TB);
            List<Admin_System_Item> allList = GMDataManager.GetNonEquip_Accessory_ItemList(ref TB);
            List<Admin_System_Item> warriorList = GMDataManager.GetEquipItemList(ref TB, "Warrior");
            List<Admin_System_Item> swordList = GMDataManager.GetEquipItemList(ref TB, "Swordmaster");
            List<Admin_System_Item> taoistList = GMDataManager.GetEquipItemList(ref TB, "Taoist");
            startHour.DataSource = hourList;
            startHour.DataTextField = "Text";
            startHour.DataValueField = "Value";
            startHour.DataBind();

            endHour.DataSource = hourList;
            endHour.DataTextField = "Text";
            endHour.DataValueField = "Value";
            endHour.DataBind();

            startMin.DataSource = minList;
            startMin.DataTextField = "Text";
            startMin.DataValueField = "Value";
            startMin.DataBind();

            endMin.DataSource = minList;
            endMin.DataTextField = "Text";
            endMin.DataValueField = "Value";
            endMin.DataBind();

            ListItem selectItem = new ListItem("select", "-1");
            all_vipLevel1.DataSource = vipLevelList; all_vipLevel1.DataTextField = "VIP_Level"; all_vipLevel1.DataValueField = "VIP_Level"; all_vipLevel1.DataBind();
            all_vipLevel1.Items.Insert(0, selectItem);
            all_vipLevel2.DataSource = vipLevelList; all_vipLevel2.DataTextField = "VIP_Level"; all_vipLevel2.DataValueField = "VIP_Level"; all_vipLevel2.DataBind();
            all_vipLevel2.Items.Insert(0, selectItem);
            all_vipLevel3.DataSource = vipLevelList; all_vipLevel3.DataTextField = "VIP_Level"; all_vipLevel3.DataValueField = "VIP_Level"; all_vipLevel3.DataBind();
            all_vipLevel3.Items.Insert(0, selectItem);
            all_vipLevel4.DataSource = vipLevelList; all_vipLevel4.DataTextField = "VIP_Level"; all_vipLevel4.DataValueField = "VIP_Level"; all_vipLevel4.DataBind();
            all_vipLevel4.Items.Insert(0, selectItem);
            all_vipLevel5.DataSource = vipLevelList; all_vipLevel5.DataTextField = "VIP_Level"; all_vipLevel5.DataValueField = "VIP_Level"; all_vipLevel5.DataBind();
            all_vipLevel5.Items.Insert(0, selectItem);
            warrior_vipLevel1.DataSource = vipLevelList; warrior_vipLevel1.DataTextField = "VIP_Level"; warrior_vipLevel1.DataValueField = "VIP_Level"; warrior_vipLevel1.DataBind();
            warrior_vipLevel1.Items.Insert(0, selectItem);
            warrior_vipLevel2.DataSource = vipLevelList; warrior_vipLevel2.DataTextField = "VIP_Level"; warrior_vipLevel2.DataValueField = "VIP_Level"; warrior_vipLevel2.DataBind();
            warrior_vipLevel2.Items.Insert(0, selectItem);
            warrior_vipLevel3.DataSource = vipLevelList; warrior_vipLevel3.DataTextField = "VIP_Level"; warrior_vipLevel3.DataValueField = "VIP_Level"; warrior_vipLevel3.DataBind();
            warrior_vipLevel3.Items.Insert(0, selectItem);
            warrior_vipLevel4.DataSource = vipLevelList; warrior_vipLevel4.DataTextField = "VIP_Level"; warrior_vipLevel4.DataValueField = "VIP_Level"; warrior_vipLevel4.DataBind();
            warrior_vipLevel4.Items.Insert(0, selectItem);
            warrior_vipLevel5.DataSource = vipLevelList; warrior_vipLevel5.DataTextField = "VIP_Level"; warrior_vipLevel5.DataValueField = "VIP_Level"; warrior_vipLevel5.DataBind();
            warrior_vipLevel5.Items.Insert(0, selectItem);
            sword_vipLevel1.DataSource = vipLevelList; sword_vipLevel1.DataTextField = "VIP_Level"; sword_vipLevel1.DataValueField = "VIP_Level"; sword_vipLevel1.DataBind();
            sword_vipLevel1.Items.Insert(0, selectItem);
            sword_vipLevel2.DataSource = vipLevelList; sword_vipLevel2.DataTextField = "VIP_Level"; sword_vipLevel2.DataValueField = "VIP_Level"; sword_vipLevel2.DataBind();
            sword_vipLevel2.Items.Insert(0, selectItem);
            sword_vipLevel3.DataSource = vipLevelList; sword_vipLevel3.DataTextField = "VIP_Level"; sword_vipLevel3.DataValueField = "VIP_Level"; sword_vipLevel3.DataBind();
            sword_vipLevel3.Items.Insert(0, selectItem);
            sword_vipLevel4.DataSource = vipLevelList; sword_vipLevel4.DataTextField = "VIP_Level"; sword_vipLevel4.DataValueField = "VIP_Level"; sword_vipLevel4.DataBind();
            sword_vipLevel4.Items.Insert(0, selectItem);
            sword_vipLevel5.DataSource = vipLevelList; sword_vipLevel5.DataTextField = "VIP_Level"; sword_vipLevel5.DataValueField = "VIP_Level"; sword_vipLevel5.DataBind();
            sword_vipLevel5.Items.Insert(0, selectItem);
            taoist_vipLevel1.DataSource = vipLevelList; taoist_vipLevel1.DataTextField = "VIP_Level"; taoist_vipLevel1.DataValueField = "VIP_Level"; taoist_vipLevel1.DataBind();
            taoist_vipLevel1.Items.Insert(0, selectItem);
            taoist_vipLevel2.DataSource = vipLevelList; taoist_vipLevel2.DataTextField = "VIP_Level"; taoist_vipLevel2.DataValueField = "VIP_Level"; taoist_vipLevel2.DataBind();
            taoist_vipLevel2.Items.Insert(0, selectItem);
            taoist_vipLevel3.DataSource = vipLevelList; taoist_vipLevel3.DataTextField = "VIP_Level"; taoist_vipLevel3.DataValueField = "VIP_Level"; taoist_vipLevel3.DataBind();
            taoist_vipLevel3.Items.Insert(0, selectItem);
            taoist_vipLevel4.DataSource = vipLevelList; taoist_vipLevel4.DataTextField = "VIP_Level"; taoist_vipLevel4.DataValueField = "VIP_Level"; taoist_vipLevel4.DataBind();
            taoist_vipLevel4.Items.Insert(0, selectItem);
            taoist_vipLevel5.DataSource = vipLevelList; taoist_vipLevel5.DataTextField = "VIP_Level"; taoist_vipLevel5.DataValueField = "VIP_Level"; taoist_vipLevel5.DataBind();
            taoist_vipLevel5.Items.Insert(0, selectItem);

            all_reward1.DataSource = allList; all_reward1.DataTextField = "Description"; all_reward1.DataValueField = "Item_IndexID"; all_reward1.DataBind();
            all_reward1.Items.Insert(0, selectItem);
            all_reward2.DataSource = allList; all_reward2.DataTextField = "Description"; all_reward2.DataValueField = "Item_IndexID"; all_reward2.DataBind();
            all_reward2.Items.Insert(0, selectItem);
            all_reward3.DataSource = allList; all_reward3.DataTextField = "Description"; all_reward3.DataValueField = "Item_IndexID"; all_reward3.DataBind();
            all_reward3.Items.Insert(0, selectItem);
            all_reward4.DataSource = allList; all_reward4.DataTextField = "Description"; all_reward4.DataValueField = "Item_IndexID"; all_reward4.DataBind();
            all_reward4.Items.Insert(0, selectItem);
            all_reward5.DataSource = allList; all_reward5.DataTextField = "Description"; all_reward5.DataValueField = "Item_IndexID"; all_reward5.DataBind();
            all_reward5.Items.Insert(0, selectItem);
            warrior_reward1.DataSource = warriorList; warrior_reward1.DataTextField = "Description"; warrior_reward1.DataValueField = "Item_IndexID"; warrior_reward1.DataBind();
            warrior_reward1.Items.Insert(0, selectItem);
            warrior_reward2.DataSource = warriorList; warrior_reward2.DataTextField = "Description"; warrior_reward2.DataValueField = "Item_IndexID"; warrior_reward2.DataBind();
            warrior_reward2.Items.Insert(0, selectItem);
            warrior_reward3.DataSource = warriorList; warrior_reward3.DataTextField = "Description"; warrior_reward3.DataValueField = "Item_IndexID"; warrior_reward3.DataBind();
            warrior_reward3.Items.Insert(0, selectItem);
            warrior_reward4.DataSource = warriorList; warrior_reward4.DataTextField = "Description"; warrior_reward4.DataValueField = "Item_IndexID"; warrior_reward4.DataBind();
            warrior_reward4.Items.Insert(0, selectItem);
            warrior_reward5.DataSource = warriorList; warrior_reward5.DataTextField = "Description"; warrior_reward5.DataValueField = "Item_IndexID"; warrior_reward5.DataBind();
            warrior_reward5.Items.Insert(0, selectItem);
            sword_reward1.DataSource = swordList; sword_reward1.DataTextField = "Description"; sword_reward1.DataValueField = "Item_IndexID"; sword_reward1.DataBind();
            sword_reward1.Items.Insert(0, selectItem);
            sword_reward2.DataSource = swordList; sword_reward2.DataTextField = "Description"; sword_reward2.DataValueField = "Item_IndexID"; sword_reward2.DataBind();
            sword_reward2.Items.Insert(0, selectItem);
            sword_reward3.DataSource = swordList; sword_reward3.DataTextField = "Description"; sword_reward3.DataValueField = "Item_IndexID"; sword_reward3.DataBind();
            sword_reward3.Items.Insert(0, selectItem);
            sword_reward4.DataSource = swordList; sword_reward4.DataTextField = "Description"; sword_reward4.DataValueField = "Item_IndexID"; sword_reward4.DataBind();
            sword_reward4.Items.Insert(0, selectItem);
            sword_reward5.DataSource = swordList; sword_reward5.DataTextField = "Description"; sword_reward5.DataValueField = "Item_IndexID"; sword_reward5.DataBind();
            sword_reward5.Items.Insert(0, selectItem);
            taoist_reward1.DataSource = taoistList; taoist_reward1.DataTextField = "Description"; taoist_reward1.DataValueField = "Item_IndexID"; taoist_reward1.DataBind();
            taoist_reward1.Items.Insert(0, selectItem);
            taoist_reward2.DataSource = taoistList; taoist_reward2.DataTextField = "Description"; taoist_reward2.DataValueField = "Item_IndexID"; taoist_reward2.DataBind();
            taoist_reward2.Items.Insert(0, selectItem);
            taoist_reward3.DataSource = taoistList; taoist_reward3.DataTextField = "Description"; taoist_reward3.DataValueField = "Item_IndexID"; taoist_reward3.DataBind();
            taoist_reward3.Items.Insert(0, selectItem);
            taoist_reward4.DataSource = taoistList; taoist_reward4.DataTextField = "Description"; taoist_reward4.DataValueField = "Item_IndexID"; taoist_reward4.DataBind();
            taoist_reward4.Items.Insert(0, selectItem);
            taoist_reward5.DataSource = taoistList; taoist_reward5.DataTextField = "Description"; taoist_reward5.DataValueField = "Item_IndexID"; taoist_reward5.DataBind();
            taoist_reward5.Items.Insert(0, selectItem);

            
            System_Event dataInfo = GMDataManager.GetSystem_EventData(ref TB, idx);
            startDay.Text = dataInfo.Event_StartTime.ToString("yyyy-MM-dd");
            startHour.SelectedValue = dataInfo.Event_StartTime.ToString("HH");
            startMin.SelectedValue = dataInfo.Event_StartTime.ToString("mm");
            endDay.Text = dataInfo.Event_EndTime.ToString("yyyy-MM-dd");
            endHour.SelectedValue = dataInfo.Event_EndTime.ToString("HH");
            endMin.SelectedValue = dataInfo.Event_EndTime.ToString("mm");
            eventTitle.Text = dataInfo.Event_Tooltip;
            labeventType.Text = dataInfo.Event_Type;
            if (dataInfo.Reward_Box1ID > 0)
            {
                List<System_Event_Reward_Box> rewardList = TriggerManager.GetSystem_Event_Reward_Box_List(ref TB, dataInfo.Reward_Box1ID, true);
                foreach (System_Event_Reward_Box item in rewardList)
                {
                    if (item.BoxItemIndex == 1)
                    {
                        all_vipLevel1.SelectedValue = item.VIP_Level.ToString();
                        all_reward1.SelectedValue = item.EventItem_ID.ToString();
                        all_rewardcnt1.Text = item.EventItem_Num.ToString();
                        all_grade1.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 2)
                    {
                        all_vipLevel2.SelectedValue = item.VIP_Level.ToString();
                        all_reward2.SelectedValue = item.EventItem_ID.ToString();
                        all_rewardcnt2.Text = item.EventItem_Num.ToString();
                        all_grade2.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 3)
                    {
                        all_vipLevel3.SelectedValue = item.VIP_Level.ToString();
                        all_reward3.SelectedValue = item.EventItem_ID.ToString();
                        all_rewardcnt3.Text = item.EventItem_Num.ToString();
                        all_grade3.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 4)
                    {
                        all_vipLevel4.SelectedValue = item.VIP_Level.ToString();
                        all_reward4.SelectedValue = item.EventItem_ID.ToString();
                        all_rewardcnt4.Text = item.EventItem_Num.ToString();
                        all_grade4.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 5)
                    {
                        all_vipLevel5.SelectedValue = item.VIP_Level.ToString();
                        all_reward5.SelectedValue = item.EventItem_ID.ToString();
                        all_rewardcnt5.Text = item.EventItem_Num.ToString();
                        all_grade5.Text = item.EventItem_Grade.ToString();
                    }
                }
            }

            if (dataInfo.Reward_Box2ID > 0)
            {
                List<System_Event_Reward_Box> rewardList = TriggerManager.GetSystem_Event_Reward_Box_List(ref TB, dataInfo.Reward_Box2ID, true);
                foreach (System_Event_Reward_Box item in rewardList)
                {
                    if (item.BoxItemIndex == 1)
                    {
                        warrior_vipLevel1.SelectedValue = item.VIP_Level.ToString();
                        warrior_reward1.SelectedValue = item.EventItem_ID.ToString();
                        warrior_level1.Text = item.EventItem_Level.ToString();
                        warrior_grade1.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 2)
                    {
                        warrior_vipLevel2.SelectedValue = item.VIP_Level.ToString();
                        warrior_reward2.SelectedValue = item.EventItem_ID.ToString();
                        warrior_level2.Text = item.EventItem_Level.ToString();
                        warrior_grade2.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 3)
                    {
                        warrior_vipLevel3.SelectedValue = item.VIP_Level.ToString();
                        warrior_reward3.SelectedValue = item.EventItem_ID.ToString();
                        warrior_level3.Text = item.EventItem_Level.ToString();
                        warrior_grade3.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 4)
                    {
                        warrior_vipLevel4.SelectedValue = item.VIP_Level.ToString();
                        warrior_reward4.SelectedValue = item.EventItem_ID.ToString();
                        warrior_level4.Text = item.EventItem_Level.ToString();
                        warrior_grade4.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 5)
                    {
                        warrior_vipLevel5.SelectedValue = item.VIP_Level.ToString();
                        warrior_reward5.SelectedValue = item.EventItem_ID.ToString();
                        warrior_level5.Text = item.EventItem_Level.ToString();
                        warrior_grade5.Text = item.EventItem_Grade.ToString();
                    }
                }
            }
            if (dataInfo.Reward_Box3ID > 0)
            {
                List<System_Event_Reward_Box> rewardList = TriggerManager.GetSystem_Event_Reward_Box_List(ref TB, dataInfo.Reward_Box3ID, true);
                foreach (System_Event_Reward_Box item in rewardList)
                {
                    if (item.BoxItemIndex == 1)
                    {
                        sword_vipLevel1.SelectedValue = item.VIP_Level.ToString();
                        sword_reward1.SelectedValue = item.EventItem_ID.ToString();
                        sword_level1.Text = item.EventItem_Level.ToString();
                        sword_grade1.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 2)
                    {
                        sword_vipLevel2.SelectedValue = item.VIP_Level.ToString();
                        sword_reward2.SelectedValue = item.EventItem_ID.ToString();
                        sword_level2.Text = item.EventItem_Level.ToString();
                        sword_grade2.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 3)
                    {
                        sword_vipLevel3.SelectedValue = item.VIP_Level.ToString();
                        sword_reward3.SelectedValue = item.EventItem_ID.ToString();
                        sword_level3.Text = item.EventItem_Level.ToString();
                        sword_grade3.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 4)
                    {
                        sword_vipLevel4.SelectedValue = item.VIP_Level.ToString();
                        sword_reward4.SelectedValue = item.EventItem_ID.ToString();
                        sword_level4.Text = item.EventItem_Level.ToString();
                        sword_grade4.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 5)
                    {
                        sword_vipLevel5.SelectedValue = item.VIP_Level.ToString();
                        sword_reward5.SelectedValue = item.EventItem_ID.ToString();
                        sword_level5.Text = item.EventItem_Level.ToString();
                        sword_grade5.Text = item.EventItem_Grade.ToString();
                    }
                }
            }
            if (dataInfo.Reward_Box4ID > 0)
            {
                List<System_Event_Reward_Box> rewardList = TriggerManager.GetSystem_Event_Reward_Box_List(ref TB, dataInfo.Reward_Box4ID, true);
                foreach (System_Event_Reward_Box item in rewardList)
                {
                    if (item.BoxItemIndex == 1)
                    {
                        taoist_vipLevel1.SelectedValue = item.VIP_Level.ToString();
                        taoist_reward1.SelectedValue = item.EventItem_ID.ToString();
                        taoist_level1.Text = item.EventItem_Level.ToString();
                        taoist_grade1.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 2)
                    {
                        taoist_vipLevel2.SelectedValue = item.VIP_Level.ToString();
                        taoist_reward2.SelectedValue = item.EventItem_ID.ToString();
                        taoist_level2.Text = item.EventItem_Level.ToString();
                        taoist_grade2.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 3)
                    {
                        taoist_vipLevel3.SelectedValue = item.VIP_Level.ToString();
                        taoist_reward3.SelectedValue = item.EventItem_ID.ToString();
                        taoist_level3.Text = item.EventItem_Level.ToString();
                        taoist_grade3.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 4)
                    {
                        taoist_vipLevel4.SelectedValue = item.VIP_Level.ToString();
                        taoist_reward4.SelectedValue = item.EventItem_ID.ToString();
                        taoist_level4.Text = item.EventItem_Level.ToString();
                        taoist_grade4.Text = item.EventItem_Grade.ToString();
                    }
                    else if (item.BoxItemIndex == 5)
                    {
                        taoist_vipLevel5.SelectedValue = item.VIP_Level.ToString();
                        taoist_reward5.SelectedValue = item.EventItem_ID.ToString();
                        taoist_level5.Text = item.EventItem_Level.ToString();
                        taoist_grade5.Text = item.EventItem_Grade.ToString();
                    }
                }
            }
        }
    }
}