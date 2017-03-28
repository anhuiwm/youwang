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
    public partial class packageCheap_Form : System.Web.UI.Page
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

            Dictionary<long, TxnBlock> TxnBlackServer = new Dictionary<long, TxnBlock>();
            TxnBlock tb = new TxnBlock();

            try
            {
                GMDataManager.GetServerinit(ref tb, ref queryFetcher, serverID);
                TxnBlackServer.Add(serverID, tb);
                tb.IsoLevel = IsolationLevel.ReadCommitted;

                idx2.Value = reqidx.ToString();
                List<server_group_config> serverGourpList = GlobalManager.GetServerGroupList(ref tb);
                serverGourpList.RemoveAll(item => item.server_group_id == 0);
                string serverlist = GMDataManager.GetServerCheckList(ref tb, serverID);
                change_server.InnerHtml = serverlist;

                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                if (serverID > -1)
                {
                    string dbkey = GMData_Define.ShardingDBName;
                    if (!Page.IsPostBack)
                        pageInit(ref tb, reqidx, dbkey);
                    else
                    {

                        if (!string.IsNullOrEmpty(reqServer))
                        {
                            string sdate = queryFetcher.QueryParam_Fetch(startDay.UniqueID, DateTime.Today.ToString("yyyy-MM-dd"));
                            string edate = queryFetcher.QueryParam_Fetch(endDay.UniqueID, DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"));
                            string shour = queryFetcher.QueryParam_Fetch(startHour.UniqueID, "00");
                            string ehour = queryFetcher.QueryParam_Fetch(endHour.UniqueID, "00");
                            string smin = queryFetcher.QueryParam_Fetch(startMin.UniqueID, "00");
                            string emin = queryFetcher.QueryParam_Fetch(endMin.UniqueID, "00");
                            long reqidx2 = queryFetcher.QueryParam_FetchLong(idx.UniqueID);
                            long itemID1_1 = queryFetcher.QueryParam_FetchLong(reward1_1.UniqueID);
                            long itemID1_2 = queryFetcher.QueryParam_FetchLong(reward1_2.UniqueID);
                            long itemID1_3 = queryFetcher.QueryParam_FetchLong(reward1_3.UniqueID);
                            long itemID1_4 = queryFetcher.QueryParam_FetchLong(reward1_4.UniqueID);
                            long itemID1_5 = queryFetcher.QueryParam_FetchLong(reward1_5.UniqueID);
                            int itema1_1 = queryFetcher.QueryParam_FetchInt(rewardcnt1_1.UniqueID);
                            int itema1_2 = queryFetcher.QueryParam_FetchInt(rewardcnt1_2.UniqueID);
                            int itema1_3 = queryFetcher.QueryParam_FetchInt(rewardcnt1_3.UniqueID);
                            int itema1_4 = queryFetcher.QueryParam_FetchInt(rewardcnt1_4.UniqueID);
                            int itema1_5 = queryFetcher.QueryParam_FetchInt(rewardcnt1_5.UniqueID);
                            byte itemGrade1_1 = queryFetcher.QueryParam_FetchByte(grade1_1.UniqueID, 1);
                            byte itemGrade1_2 = queryFetcher.QueryParam_FetchByte(grade1_2.UniqueID, 1);
                            byte itemGrade1_3 = queryFetcher.QueryParam_FetchByte(grade1_3.UniqueID, 1);
                            byte itemGrade1_4 = queryFetcher.QueryParam_FetchByte(grade1_4.UniqueID, 1);
                            byte itemGrade1_5 = queryFetcher.QueryParam_FetchByte(grade1_5.UniqueID, 1);
                            long itemID2_1 = queryFetcher.QueryParam_FetchLong(reward2_1.UniqueID);
                            long itemID2_2 = queryFetcher.QueryParam_FetchLong(reward2_2.UniqueID);
                            long itemID2_3 = queryFetcher.QueryParam_FetchLong(reward2_3.UniqueID);
                            long itemID2_4 = queryFetcher.QueryParam_FetchLong(reward2_4.UniqueID);
                            long itemID2_5 = queryFetcher.QueryParam_FetchLong(reward2_5.UniqueID);
                            int itema2_1 = queryFetcher.QueryParam_FetchInt(rewardcnt2_1.UniqueID);
                            int itema2_2 = queryFetcher.QueryParam_FetchInt(rewardcnt2_2.UniqueID);
                            int itema2_3 = queryFetcher.QueryParam_FetchInt(rewardcnt2_3.UniqueID);
                            int itema2_4 = queryFetcher.QueryParam_FetchInt(rewardcnt2_4.UniqueID);
                            int itema2_5 = queryFetcher.QueryParam_FetchInt(rewardcnt2_5.UniqueID);
                            byte itemGrade2_1 = queryFetcher.QueryParam_FetchByte(grade2_1.UniqueID, 1);
                            byte itemGrade2_2 = queryFetcher.QueryParam_FetchByte(grade2_2.UniqueID, 1);
                            byte itemGrade2_3 = queryFetcher.QueryParam_FetchByte(grade2_3.UniqueID, 1);
                            byte itemGrade2_4 = queryFetcher.QueryParam_FetchByte(grade2_4.UniqueID, 1);
                            byte itemGrade2_5 = queryFetcher.QueryParam_FetchByte(grade2_5.UniqueID, 1);
                            byte buyCount1 = queryFetcher.QueryParam_FetchByte(maxCnt.UniqueID);
                            byte buyCount2 = queryFetcher.QueryParam_FetchByte(maxCnt2.UniqueID);
                            byte looptype = queryFetcher.QueryParam_FetchByte(loopType.UniqueID);
                            int price1 = queryFetcher.QueryParam_FetchInt(payValue1.UniqueID, 1100);
                            int price2 = queryFetcher.QueryParam_FetchInt(payValue2.UniqueID, 3300);
                            int vipPoint1 = queryFetcher.QueryParam_FetchInt(vip_point1.UniqueID, 10);
                            int vipPoint2 = queryFetcher.QueryParam_FetchInt(vip_point2.UniqueID, 30);
                            string goodsName1 = queryFetcher.QueryParam_Fetch(title1.UniqueID, "#STRING_NAMING_PACKAGE_BIG_TITEL_001");
                            string goodsName2 = queryFetcher.QueryParam_Fetch(title2.UniqueID, "#STRING_NAMING_PACKAGE_BIG_TITEL_002");
                            string billingType = queryFetcher.QueryParam_Fetch("billing");

                            if (!string.IsNullOrEmpty(billingType))
                            {
                                if (shour.Equals("-1"))
                                    shour = "00";
                                if (smin.Equals("-1"))
                                    smin = "00";
                                if (ehour.Equals("-1"))
                                    ehour = "00";
                                if (emin.Equals("-1"))
                                    emin = "00";
                                List<int> billingList = billingType.Split(',').Select(Int32.Parse).ToList();
                                string startDate = string.Format("{0} {1}:{2}:00", sdate, shour, smin);
                                string endDate = string.Format("{0} {1}:{2}:59", edate, ehour, emin);
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
                                List<System_Package_List> packageList = new List<System_Package_List>();
                                List<System_Package_RewardBox> rewardList1 = new List<System_Package_RewardBox>();
                                List<System_Package_RewardBox> rewardList2 = new List<System_Package_RewardBox>();

                                System_Package_List item1 = new System_Package_List();
                                System_Package_List item2 = new System_Package_List();
                                item1.Package_ID = reqidx;
                                item1.Buy_PriceValue = price2;
                                item1.Max_Buy = buyCount2;
                                item1.VIP_Point = vipPoint2;
                                item1.Reward_Box1ID = reqidx > 0 ? GMDataManager.GetGM_Package_Data(ref tb, reqidx, dbkey, false).Reward_Box1ID : 0;
                                item1.NameCN1 = goodsName2;
                                item1.LoopType = looptype;
                                packageList.Add(item1);

                                item2.Package_ID = reqidx2;
                                item2.Buy_PriceValue = price1;
                                item2.Max_Buy = buyCount1;
                                item2.VIP_Point = vipPoint1;
                                item2.Reward_Box1ID = reqidx2 > 0 ? GMDataManager.GetGM_Package_Data(ref tb, reqidx2, dbkey, false).Reward_Box1ID : 0;
                                item2.NameCN1 = goodsName1;
                                item2.LoopType = looptype;
                                packageList.Add(item2);

                                byte itemIndexNum = 1;
                                if (itemID1_1 > 0 && itema1_1 > 0)
                                {
                                    System_Package_RewardBox item = new System_Package_RewardBox();
                                    item.Item_ID = itemID1_1;
                                    item.Item_Num = itema1_1;
                                    item.Item_Grade = itemGrade1_1;
                                    item.ItemIndex = itemIndexNum;
                                    item.Item_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), itemID1_1)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), itemID1_1);
                                    rewardList1.Add(item);
                                    itemIndexNum += 1;
                                }
                                if (itemID1_2 > 0 && itema1_2 > 0)
                                {
                                    System_Package_RewardBox item = new System_Package_RewardBox();
                                    item.Item_ID = itemID1_2;
                                    item.Item_Num = itema1_2;
                                    item.Item_Grade = itemGrade1_2;
                                    item.ItemIndex = itemIndexNum;
                                    item.Item_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), itemID1_2)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), itemID1_2);
                                    rewardList1.Add(item);
                                    itemIndexNum += 1;
                                }
                                if (itemID1_3 > 0 && itema1_3 > 0)
                                {
                                    System_Package_RewardBox item = new System_Package_RewardBox();
                                    item.Item_ID = itemID1_3;
                                    item.Item_Num = itema1_3;
                                    item.Item_Grade = itemGrade1_3;
                                    item.ItemIndex = itemIndexNum;
                                    item.Item_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), itemID1_3)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), itemID1_3);
                                    rewardList1.Add(item);
                                    itemIndexNum += 1;
                                }
                                if (itemID1_4 > 0 && itema1_4 > 0)
                                {
                                    System_Package_RewardBox item = new System_Package_RewardBox();
                                    item.Item_ID = itemID1_4;
                                    item.Item_Num = itema1_4;
                                    item.Item_Grade = itemGrade1_4;
                                    item.ItemIndex = itemIndexNum;
                                    item.Item_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), itemID1_4)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), itemID1_4);
                                    rewardList1.Add(item);
                                    itemIndexNum += 1;
                                }
                                if (itemID1_5 > 0 && itema1_5 > 0)
                                {
                                    System_Package_RewardBox item = new System_Package_RewardBox();
                                    item.Item_ID = itemID1_5;
                                    item.Item_Num = itema1_5;
                                    item.Item_Grade = itemGrade1_5;
                                    item.ItemIndex = itemIndexNum;
                                    item.Item_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), itemID1_5)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), itemID1_5);
                                    rewardList1.Add(item);
                                    itemIndexNum += 1;
                                }

                                itemIndexNum = 1;
                                if (itemID2_1 > 0 && itema2_1 > 0)
                                {
                                    System_Package_RewardBox item = new System_Package_RewardBox();
                                    item.Item_ID = itemID2_1;
                                    item.Item_Num = itema2_1;
                                    item.Item_Grade = itemGrade2_1;
                                    item.ItemIndex = itemIndexNum;
                                    item.Item_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), itemID2_1)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), itemID2_1);
                                    rewardList2.Add(item);
                                    itemIndexNum += 1;
                                }
                                if (itemID2_2 > 0 && itema2_2 > 0)
                                {
                                    System_Package_RewardBox item = new System_Package_RewardBox();
                                    item.Item_ID = itemID2_2;
                                    item.Item_Num = itema2_2;
                                    item.Item_Grade = itemGrade2_2;
                                    item.ItemIndex = itemIndexNum;
                                    item.Item_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), itemID2_2)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), itemID2_2);
                                    rewardList2.Add(item);
                                    itemIndexNum += 1;
                                }
                                if (itemID2_3 > 0 && itema2_3 > 0)
                                {
                                    System_Package_RewardBox item = new System_Package_RewardBox();
                                    item.Item_ID = itemID2_3;
                                    item.Item_Num = itema2_3;
                                    item.Item_Grade = itemGrade2_3;
                                    item.ItemIndex = itemIndexNum;
                                    item.Item_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), itemID2_3)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), itemID2_3);
                                    rewardList2.Add(item);
                                    itemIndexNum += 1;
                                }
                                if (itemID2_4 > 0 && itema2_4 > 0)
                                {
                                    System_Package_RewardBox item = new System_Package_RewardBox();
                                    item.Item_ID = itemID2_4;
                                    item.Item_Num = itema2_4;
                                    item.Item_Grade = itemGrade2_4;
                                    item.ItemIndex = itemIndexNum;
                                    item.Item_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), itemID2_4)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), itemID2_4);
                                    rewardList2.Add(item);
                                    itemIndexNum += 1;
                                }
                                if (itemID2_5 > 0 && itema2_5 > 0)
                                {
                                    System_Package_RewardBox item = new System_Package_RewardBox();
                                    item.Item_ID = itemID2_5;
                                    item.Item_Num = itema2_5;
                                    item.Item_Grade = itemGrade2_5;
                                    item.ItemIndex = itemIndexNum;
                                    item.Item_TargetType = string.IsNullOrEmpty(Enum.GetName(typeof(GMData_Define.eRewardTye), itemID2_5)) ? "Item" : Enum.GetName(typeof(GMData_Define.eRewardTye), itemID2_5);
                                    rewardList2.Add(item);
                                    itemIndexNum += 1;
                                }
                                bool checkDate = true;
                                List<string> resultServer = GMDataManager.GetPackageCheapMaxDateServerList(ref TxnBlackServer, reqidx, reqidx2, startDate, endDate);
                                if (resultServer.Count > 0)
                                    checkDate = false;
                                
                                if (checkDate)
                                {
                                    if (reqidx > 0)
                                    {
                                        retError = GMDataManager.UpdatePackageCheap(ref TxnBlackServer, startDate, endDate, packageList, rewardList1, rewardList2, billingList);
                                        if (retError == Result_Define.eResult.SUCCESS)
                                            retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, reqidx, "", GMResult_Define.ControlType.CHEAP_PACKAGE_EDIT, queryFetcher.GetReqParams(), reqServer);
                                    }
                                    else
                                    {
                                        retError = GMDataManager.InsertPackageCheap(ref TxnBlackServer, startDate, endDate, packageList, rewardList1, rewardList2, billingList);
                                        if (retError == Result_Define.eResult.SUCCESS)
                                            retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, 0, "", GMResult_Define.ControlType.CHEAP_PACKAGE_ADD, queryFetcher.GetReqParams(), reqServer);
                                    }
                                }
                                else
                                {
                                    string msg = "alert('" + Resources.languageResource.lang_msg_gachaDate + "');";
                                    Page.ClientScript.RegisterStartupScript(GetType(), "alert", msg, true);
                                    SetFrom(ref tb, System.Convert.ToDateTime(startDate), System.Convert.ToDateTime(endDate), packageList, rewardList1, rewardList2);
                                }
                                retJson = queryFetcher.GM_Render(retError);
                                if (retError == Result_Define.eResult.SUCCESS)
                                    result = true;
                            }
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
                            gmid = HttpContext.Current.Request.Cookies["mseedadmin"]["userid"];
                        queryFetcher.GMToolLogToDB(ref tb, gmid, GMData_Define.GmDBName);
                    }
                    setItem.Value.Dispose();
                }
            }


            if (result)
                Response.Redirect("packageCheapList.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID);
        }

        protected void pageInit(ref TxnBlock TB, long Index, string dbkey)
        {
            ListItem selectItem = new ListItem("select", "-1");
            
            List<ListItem> hourList = GMDataManager.GetHourList();
            List<ListItem> minList = GMDataManager.GetMinList(1);
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
            string billing = "";
            int billingCount = 0;



            payValue1.DataSource = (GMDataManager.GetGmToolLanguage() == "kr") ? Enum.GetNames(typeof(GMData_Define.eShopProduct)).Select(o => new { Text = (int)(Enum.Parse(typeof(GMData_Define.eShopProduct), o)), Value = (int)(Enum.Parse(typeof(GMData_Define.eShopProduct), o)) })
                                                                                : Enum.GetNames(typeof(GMData_Define.eChinaShopCheapProduct)).Select(o => new { Text = (int)(Enum.Parse(typeof(GMData_Define.eChinaShopCheapProduct), o)), Value = (int)(Enum.Parse(typeof(GMData_Define.eChinaShopCheapProduct), o)) });
            payValue1.DataTextField = "Text";
            payValue1.DataValueField = "Value";
            payValue1.DataBind();

            payValue2.DataSource = (GMDataManager.GetGmToolLanguage() == "kr") ? Enum.GetNames(typeof(GMData_Define.eShopProduct)).Select(o => new { Text = (int)(Enum.Parse(typeof(GMData_Define.eShopProduct), o)), Value = (int)(Enum.Parse(typeof(GMData_Define.eShopProduct), o)) })
                                                                                : Enum.GetNames(typeof(GMData_Define.eChinaShopCheapProduct)).Select(o => new { Text = (int)(Enum.Parse(typeof(GMData_Define.eChinaShopCheapProduct), o)), Value = (int)(Enum.Parse(typeof(GMData_Define.eChinaShopCheapProduct), o)) });
            payValue2.DataTextField = "Text";
            payValue2.DataValueField = "Value";
            payValue2.DataBind();
            

            List<Admin_System_Item> allList = GMDataManager.GetNonEquip_Accessory_ItemList(ref TB, dbkey);

            reward1_1.DataSource = allList;
            reward1_1.DataTextField = "Description";
            reward1_1.DataValueField = "Item_IndexID";
            reward1_1.DataBind();
            reward1_1.Items.Insert(0, selectItem);

            reward1_2.DataSource = allList;
            reward1_2.DataTextField = "Description";
            reward1_2.DataValueField = "Item_IndexID";
            reward1_2.DataBind();
            reward1_2.Items.Insert(0, selectItem);
            
            reward1_3.DataSource = allList;
            reward1_3.DataTextField = "Description";
            reward1_3.DataValueField = "Item_IndexID";
            reward1_3.DataBind();
            reward1_3.Items.Insert(0, selectItem);

            reward1_4.DataSource = allList;
            reward1_4.DataTextField = "Description";
            reward1_4.DataValueField = "Item_IndexID";
            reward1_4.DataBind();
            reward1_4.Items.Insert(0, selectItem);

            reward1_5.DataSource = allList;
            reward1_5.DataTextField = "Description";
            reward1_5.DataValueField = "Item_IndexID";
            reward1_5.DataBind();
            reward1_5.Items.Insert(0, selectItem);

            reward2_1.DataSource = allList;
            reward2_1.DataTextField = "Description";
            reward2_1.DataValueField = "Item_IndexID";
            reward2_1.DataBind();
            reward2_1.Items.Insert(0, selectItem);

            reward2_2.DataSource = allList;
            reward2_2.DataTextField = "Description";
            reward2_2.DataValueField = "Item_IndexID";
            reward2_2.DataBind();
            reward2_2.Items.Insert(0, selectItem);

            reward2_3.DataSource = allList;
            reward2_3.DataTextField = "Description";
            reward2_3.DataValueField = "Item_IndexID";
            reward2_3.DataBind();
            reward2_3.Items.Insert(0, selectItem);

            reward2_4.DataSource = allList;
            reward2_4.DataTextField = "Description";
            reward2_4.DataValueField = "Item_IndexID";
            reward2_4.DataBind();
            reward2_4.Items.Insert(0, selectItem);

            reward2_5.DataSource = allList;
            reward2_5.DataTextField = "Description";
            reward2_5.DataValueField = "Item_IndexID";
            reward2_5.DataBind();
            reward2_5.Items.Insert(0, selectItem);

            if (Index > 0)
            {
                bool checkPackage = false;
                GM_System_Package_List dataInfo1 = GMDataManager.GetGM_Package_Data(ref TB, Index, dbkey, checkPackage);
                bool checkEdit = GMDataManager.GetDateBetween(DateTime.Now, dataInfo1.SaleStartTime, dataInfo1.SaleEndTime);
                long index2 = GMDataManager.GetPackageCheapIndex1Yuan(ref TB, Index, dataInfo1.SaleStartTime.ToString("yyyy-MM-dd HH:mm:ss"), dataInfo1.SaleEndTime.ToString("yyyy-MM-dd HH:mm:ss"), dbkey);
                GM_System_Package_List dataInfo2 = GMDataManager.GetGM_Package_Data(ref TB, index2, dbkey, checkPackage);
                List<GM_Number> checkbilling = GMDataManager.GetShopGoodsCodeList(ref TB, dataInfo1.Package_ID);
                foreach (var item in Enum.GetValues(typeof(Shop_Define.eBillingType)))
                {
                    string isCheck = checkbilling.Find(checkItem => checkItem.number == (int)item) == null ? "" : "checked";
                    billing = billing + string.Format("<input type=\"checkbox\" name=\"billing\" value=\"{0}\" {3} /> {1} {2}", (int)item, Enum.GetName(typeof(Shop_Define.eBillingType), item), (billingCount % 5) == 0 ? "<br />" : "", isCheck);
                    billingCount++;
                }
                span_billing.InnerHtml = billing;


                idx.Value = index2.ToString();
                edit.Value = checkEdit ? "1" : "0";
                payValue1.SelectedValue = dataInfo1.Buy_PriceValue > (int)GMData_Define.eShopProduct.darkblaze_price_1100 ? dataInfo1.Buy_PriceValue.ToString() : GMDataManager.GetSystemProductBYRealPrice(ref TB, dataInfo1.Buy_PriceValue).PriceValue.ToString();
                payValue2.SelectedValue = dataInfo2.Buy_PriceValue > (int)GMData_Define.eShopProduct.darkblaze_price_1100 ? dataInfo2.Buy_PriceValue.ToString() : GMDataManager.GetSystemProductBYRealPrice(ref TB, dataInfo2.Buy_PriceValue).PriceValue.ToString(); 
                vip_point1.Text = dataInfo1.VIP_Point.ToString();
                vip_point2.Text = dataInfo2.VIP_Point.ToString();
                maxCnt.Text = dataInfo1.Max_Buy.ToString();
                maxCnt2.Text = dataInfo2.Max_Buy.ToString();
                title1.Text = dataInfo1.NameCN1;
                title2.Text = dataInfo2.NameCN1;
                startDay.Text = dataInfo1.SaleStartTime.ToString("yyyy-MM-dd");
                startHour.SelectedValue = dataInfo1.SaleStartTime.ToString("HH");
                startMin.SelectedValue = dataInfo1.SaleStartTime.ToString("mm");
                endDay.Text = dataInfo1.SaleEndTime.ToString("yyyy-MM-dd");
                endHour.SelectedValue = dataInfo1.SaleEndTime.ToString("HH");
                endMin.SelectedValue = dataInfo1.SaleEndTime.ToString("mm");
                loopType.SelectedValue = dataInfo1.LoopType.ToString();
                List<System_Package_RewardBox> rewardInfo1 = ShopManager.GetShop_System_Package_Cheap_RewardBox(ref TB, dataInfo1.Reward_Box1ID, true, dbkey);
                List<System_Package_RewardBox> rewardInfo2 = ShopManager.GetShop_System_Package_Cheap_RewardBox(ref TB, dataInfo2.Reward_Box1ID, true, dbkey);

                if (rewardInfo2.Count > 0)
                {
                    foreach (System_Package_RewardBox item in rewardInfo2)
                    {
                        if (item.ItemIndex == 1)
                        {
                            reward2_1.SelectedValue = item.Item_ID.ToString();
                            grade2_1.Text = item.Item_Grade.ToString();
                            rewardcnt2_1.Text = item.Item_Num.ToString();
                        }
                        else if (item.ItemIndex == 2)
                        {
                            reward2_2.SelectedValue = item.Item_ID.ToString();
                            grade2_2.Text = item.Item_Grade.ToString();
                            rewardcnt2_2.Text = item.Item_Num.ToString();
                        }
                        else if (item.ItemIndex == 3)
                        {
                            reward2_3.SelectedValue = item.Item_ID.ToString();
                            grade2_3.Text = item.Item_Grade.ToString();
                            rewardcnt2_3.Text = item.Item_Num.ToString();
                        }
                        else if (item.ItemIndex == 4)
                        {
                            reward2_4.SelectedValue = item.Item_ID.ToString();
                            grade2_4.Text = item.Item_Grade.ToString();
                            rewardcnt2_4.Text = item.Item_Num.ToString();
                        }
                        else if (item.ItemIndex == 5)
                        {
                            reward2_5.SelectedValue = item.Item_ID.ToString();
                            grade2_5.Text = item.Item_Grade.ToString();
                            rewardcnt2_5.Text = item.Item_Num.ToString();
                        }
                    }
                }
                if (rewardInfo1.Count > 0)
                {
                    foreach (System_Package_RewardBox item in rewardInfo1)
                    {
                        if (item.ItemIndex == 1)
                        {
                            reward1_1.SelectedValue = item.Item_ID.ToString();
                            grade1_1.Text = item.Item_Grade.ToString();
                            rewardcnt1_1.Text = item.Item_Num.ToString();
                        }
                        else if (item.ItemIndex == 2)
                        {
                            reward1_2.SelectedValue = item.Item_ID.ToString();
                            grade1_2.Text = item.Item_Grade.ToString();
                            rewardcnt1_2.Text = item.Item_Num.ToString();
                        }
                        else if (item.ItemIndex == 3)
                        {
                            reward1_3.SelectedValue = item.Item_ID.ToString();
                            grade1_3.Text = item.Item_Grade.ToString();
                            rewardcnt1_3.Text = item.Item_Num.ToString();
                        }
                        else if (item.ItemIndex == 4)
                        {
                            reward1_4.SelectedValue = item.Item_ID.ToString();
                            grade1_4.Text = item.Item_Grade.ToString();
                            rewardcnt1_4.Text = item.Item_Num.ToString();
                        }
                        else if (item.ItemIndex == 5)
                        {
                            reward1_5.SelectedValue = item.Item_ID.ToString();
                            grade1_5.Text = item.Item_Grade.ToString();
                            rewardcnt1_5.Text = item.Item_Num.ToString();
                        }
                    }
                }
            }
            else
            {
                foreach (var item in Enum.GetValues(typeof(Shop_Define.eBillingType)))
                {
                    billing = billing + string.Format("<input type=\"checkbox\" name=\"billing\" value=\"{0}\" /> {1} {2}", (int)item, Enum.GetName(typeof(Shop_Define.eBillingType), item), (billingCount % 5) == 0 ? "<br />" : "");
                    billingCount++;
                }
                span_billing.InnerHtml = billing;
            }
        }

        protected void SetFrom(ref TxnBlock TB, DateTime SaleStartTime, DateTime SaleEndTime, List<System_Package_List> packageList, List<System_Package_RewardBox> rewardList1, List<System_Package_RewardBox> rewardList2)
        {
            System_Package_List dataInfo1 = packageList.Find(item=>item.Buy_PriceValue==(long)GMData_Define.eShopProduct.darkblaze_price_3300);
            System_Package_List dataInfo2 = packageList.Find(item => item.Buy_PriceValue == (long)GMData_Define.eShopProduct.darkblaze_price_1100);
                
            idx2.Value = dataInfo2.Package_ID.ToString();
            maxCnt2.Text = dataInfo2.Max_Buy.ToString();
            maxCnt.Text = dataInfo1.Max_Buy.ToString();
            startDay.Text = SaleStartTime.ToString("yyyy-MM-dd");
            startHour.SelectedValue = SaleStartTime.ToString("HH");
            startMin.SelectedValue = SaleStartTime.ToString("mm");
            endDay.Text = SaleEndTime.ToString("yyyy-MM-dd");
            endHour.SelectedValue = SaleEndTime.ToString("HH");
            endMin.SelectedValue = SaleEndTime.ToString("mm");

            if (rewardList2.Count > 0)
            {
                foreach (System_Package_RewardBox item in rewardList2)
                {
                    if (item.ItemIndex == 1)
                    {
                        reward1_1.SelectedValue = item.Item_ID.ToString();
                        grade1_1.Text = item.Item_Grade.ToString();
                        rewardcnt1_1.Text = item.Item_Num.ToString();
                    }
                    else if (item.ItemIndex == 2)
                    {
                        reward1_2.SelectedValue = item.Item_ID.ToString();
                        grade1_2.Text = item.Item_Grade.ToString();
                        rewardcnt1_2.Text = item.Item_Num.ToString();
                    }
                    else if (item.ItemIndex == 3)
                    {
                        reward1_3.SelectedValue = item.Item_ID.ToString();
                        grade1_3.Text = item.Item_Grade.ToString();
                        rewardcnt1_3.Text = item.Item_Num.ToString();
                    }
                    else if (item.ItemIndex == 4)
                    {
                        reward1_4.SelectedValue = item.Item_ID.ToString();
                        grade1_4.Text = item.Item_Grade.ToString();
                        rewardcnt1_4.Text = item.Item_Num.ToString();
                    }
                    else if (item.ItemIndex == 5)
                    {
                        reward1_5.SelectedValue = item.Item_ID.ToString();
                        grade1_5.Text = item.Item_Grade.ToString();
                        rewardcnt1_5.Text = item.Item_Num.ToString();
                    }
                }
            }
            if (rewardList1.Count > 0)
            {
                foreach (System_Package_RewardBox item in rewardList1)
                {
                    if (item.ItemIndex == 1)
                    {
                        reward2_1.SelectedValue = item.Item_ID.ToString();
                        grade2_1.Text = item.Item_Grade.ToString();
                        rewardcnt2_1.Text = item.Item_Num.ToString();
                    }
                    else if (item.ItemIndex == 2)
                    {
                        reward2_2.SelectedValue = item.Item_ID.ToString();
                        grade2_2.Text = item.Item_Grade.ToString();
                        rewardcnt2_2.Text = item.Item_Num.ToString();
                    }
                    else if (item.ItemIndex == 3)
                    {
                        reward2_3.SelectedValue = item.Item_ID.ToString();
                        grade2_3.Text = item.Item_Grade.ToString();
                        rewardcnt2_3.Text = item.Item_Num.ToString();
                    }
                    else if (item.ItemIndex == 4)
                    {
                        reward2_4.SelectedValue = item.Item_ID.ToString();
                        grade2_4.Text = item.Item_Grade.ToString();
                        rewardcnt2_4.Text = item.Item_Num.ToString();
                    }
                    else if (item.ItemIndex == 5)
                    {
                        reward2_5.SelectedValue = item.Item_ID.ToString();
                        grade2_5.Text = item.Item_Grade.ToString();
                        rewardcnt2_5.Text = item.Item_Num.ToString();
                    }
                }
            }
        }
    }
}