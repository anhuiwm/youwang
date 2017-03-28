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
    public partial class package_Form : System.Web.UI.Page
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

                idx.Value = reqidx.ToString();
                List<server_group_config> serverGourpList = GlobalManager.GetServerGroupList(ref tb);
                serverGourpList.RemoveAll(item => item.server_group_id == 0);
                string serverlist = GMDataManager.GetServerCheckList(ref tb, serverID);
                change_server.InnerHtml = serverlist;

                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                if (serverID > -1)
                {

                    pageInit(ref tb);

                    if (reqidx > 0)
                    {
                        packageView(ref tb, reqidx);

                    }
                    if (!string.IsNullOrEmpty(reqServer))
                    {
                        string reqName = queryFetcher.QueryParam_Fetch(packageName.UniqueID, "package");
                        string reqPackType = queryFetcher.QueryParam_Fetch(payType_0.UniqueID.Substring(0, payType_0.UniqueID.Length - 2), "PriceType_PayCash");
                        int reqPackPay = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(payValue.UniqueID, "0"));
                        int reqPackPay2 = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(payValue2.UniqueID, "0"));
                        short reqVIPLevel = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch(vipLevel.UniqueID, "0"));
                        int reqVIPpoint = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(vipPoint.UniqueID, "0"));
                        byte reqGrade = System.Convert.ToByte(queryFetcher.QueryParam_Fetch(grade.UniqueID, "0"));
                        byte reqActive = System.Convert.ToByte(queryFetcher.QueryParam_Fetch(active.UniqueID, "0"));
                        string reqTooltip = queryFetcher.QueryParam_Fetch(descCN.UniqueID, "");
                        string reqDetail = queryFetcher.QueryParam_Fetch(detailCN.UniqueID, "");
                        string sdate = queryFetcher.QueryParam_Fetch(startDay.UniqueID, DateTime.Today.ToString("yyyy-MM-dd"));
                        string edate = queryFetcher.QueryParam_Fetch(endDay.UniqueID, DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"));
                        string shour = queryFetcher.QueryParam_Fetch(startHour.UniqueID, "00");
                        string ehour = queryFetcher.QueryParam_Fetch(endHour.UniqueID, "00");
                        string smin = queryFetcher.QueryParam_Fetch(startMin.UniqueID, "00");
                        string emin = queryFetcher.QueryParam_Fetch(endMin.UniqueID, "00");
                        byte reqMaxBuy = queryFetcher.QueryParam_FetchByte(maxCnt.UniqueID);
                        string reqReward1 = queryFetcher.QueryParam_Fetch(all_item.UniqueID, "");
                        string reqReward1cnt = queryFetcher.QueryParam_Fetch(all_itemcnt.UniqueID, "");
                        string reqReward1grade = queryFetcher.QueryParam_Fetch(all_itemgrade.UniqueID, "");
                        string reqReward2 = queryFetcher.QueryParam_Fetch(warrior_item.UniqueID, "");
                        string reqReward2level = queryFetcher.QueryParam_Fetch(warriorlevel.UniqueID, "");
                        string reqReward2grade = queryFetcher.QueryParam_Fetch(warriorgrade.UniqueID, "");
                        string reqReward3 = queryFetcher.QueryParam_Fetch(sword_item.UniqueID, "");
                        string reqReward3level = queryFetcher.QueryParam_Fetch(swordlevel.UniqueID, "");
                        string reqReward3grade = queryFetcher.QueryParam_Fetch(swordgrade.UniqueID, "");
                        string reqReward4 = queryFetcher.QueryParam_Fetch(taoist_item.UniqueID, "");
                        string reqReward4level = queryFetcher.QueryParam_Fetch(taoistlevel.UniqueID, "");
                        string reqReward4grade = queryFetcher.QueryParam_Fetch(taoistgrade.UniqueID, "");
                        byte looptype = queryFetcher.QueryParam_FetchByte(loopType.UniqueID);
                        string billingType = queryFetcher.QueryParam_Fetch("billing");

                        if (reqPackType == "PriceType_PayReal")
                        {
                            reqPackPay = reqPackPay2;
                        }
                        else
                            billingType = "0";
                        if (shour.Equals("-1"))
                            shour = "00";
                        if (smin.Equals("-1"))
                            smin = "00";
                        if (ehour.Equals("-1"))
                            ehour = "00";
                        if (emin.Equals("-1"))
                            emin = "00";

                        string startDate = string.Format("{0} {1}:{2}:00", sdate, shour, smin);
                        string endDate = string.Format("{0} {1}:{2}:59", edate, ehour, emin);
                        
                        List<int> billingList = billingType.Split(',').Select(Int32.Parse).ToList();

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
                        System_Package_List packageItem = new System_Package_List();
                        reqName = reqName.Replace("'", "''");
                        reqTooltip = reqTooltip.Replace("'", "''");
                        reqDetail = reqDetail.Replace("'", "''");
                        packageItem.ActiveType = reqActive;
                        packageItem.Buy_PriceType = reqPackType;
                        packageItem.Buy_PriceValue = reqPackPay;
                        packageItem.DetailCN = reqDetail;
                        packageItem.Grade = reqGrade;
                        packageItem.Max_Buy = reqMaxBuy;
                        packageItem.NameCN1 = reqName;
                        packageItem.NameCN2 = "xxx";
                        packageItem.ToolTipCN = reqTooltip;
                        packageItem.VIP_Level = (byte)reqVIPLevel;
                        packageItem.VIP_Point = reqVIPpoint;
                        packageItem.LoopType = looptype;
                        if (reqidx > 0)
                        {
                            GM_System_Package_List dataInfo = GMDataManager.GetGM_Package_Data(ref tb, reqidx);
                            packageItem.Package_ID = reqidx;
                            packageItem.Reward_Box1ID = dataInfo.Reward_Box1ID;
                            packageItem.Reward_Box2ID = dataInfo.Reward_Box2ID;
                            packageItem.Reward_Box3ID = dataInfo.Reward_Box3ID;
                            packageItem.Reward_Box4ID = dataInfo.Reward_Box4ID;
                        }

                        // reward setting
                        int itemIndex = 1;
                        List<System_Package_RewardBox> rewardList1 = new List<System_Package_RewardBox>();
                        List<System_Package_RewardBox> rewardList2 = new List<System_Package_RewardBox>();
                        List<System_Package_RewardBox> rewardList3 = new List<System_Package_RewardBox>();
                        List<System_Package_RewardBox> rewardList4 = new List<System_Package_RewardBox>();

                        string[] reward1 = System.Text.RegularExpressions.Regex.Split(reqReward1, ",");
                        string[] reward1cnt = System.Text.RegularExpressions.Regex.Split(reqReward1cnt, ",");
                        string[] reward1grade = System.Text.RegularExpressions.Regex.Split(reqReward1grade, ",");

                        for (int i = 0; i < reward1.Length; i++)
                        {
                            long itemid = System.Convert.ToInt64(reward1[i]);
                            if (itemid > 0)
                            {
                                int itemNum = System.Convert.ToInt32(reward1cnt[i]);
                                byte itemgrade = 1;
                                if (!string.IsNullOrEmpty(reward1grade[i]))
                                    itemgrade = System.Convert.ToByte(reward1grade[i]);
                                if (itemid > 0 && itemNum > 0)
                                {

                                    System_Package_RewardBox item = new System_Package_RewardBox();
                                    item.Item_ID = itemid;
                                    item.Item_Num = itemNum;
                                    item.Item_Grade = itemgrade;
                                    item.ItemIndex = (byte)itemIndex;
                                    string itemType = Enum.GetName(typeof(GMData_Define.eRewardTye), itemid);
                                    if (string.IsNullOrEmpty(itemType))
                                    {
                                        itemType = "Item";
                                    }
                                    item.Item_TargetType = itemType;
                                    rewardList1.Add(item);
                                    itemIndex++;
                                }
                            }
                        }
                        itemIndex = 1;
                        string[] reward2 = System.Text.RegularExpressions.Regex.Split(reqReward2, ",");
                        string[] reward2level = System.Text.RegularExpressions.Regex.Split(reqReward2level, ",");
                        string[] reward2grade = System.Text.RegularExpressions.Regex.Split(reqReward2grade, ",");
                        for (int i = 0; i < reward2.Length; i++)
                        {
                            long itemid = System.Convert.ToInt64(reward2[i]);
                            if (itemid > 0)
                            {
                                if (string.IsNullOrEmpty(reward2level[i]))
                                    reward2level[i] = "0";
                                if (string.IsNullOrEmpty(reward2grade[i]))
                                    reward2grade[i] = "0";
                                short itemlevel = System.Convert.ToInt16(reward2level[i]);
                                short itemgrade = System.Convert.ToInt16(reward2grade[i]);
                                if (itemgrade > 0)
                                {
                                    System_Package_RewardBox item = new System_Package_RewardBox();
                                    item.Item_ID = itemid;
                                    item.Item_Num = 1;
                                    item.ItemIndex = (byte)itemIndex;
                                    item.Item_TargetType = "Item";
                                    item.Item_Level = (byte)itemlevel;
                                    item.Item_Grade = (byte)itemgrade;
                                    rewardList2.Add(item);
                                    itemIndex++;
                                }
                            }
                        }

                        itemIndex = 1;
                        string[] reward3 = System.Text.RegularExpressions.Regex.Split(reqReward3, ",");
                        string[] reward3level = System.Text.RegularExpressions.Regex.Split(reqReward3level, ",");
                        string[] reward3grade = System.Text.RegularExpressions.Regex.Split(reqReward3grade, ",");
                        for (int i = 0; i < reward3.Length; i++)
                        {
                            long itemid = System.Convert.ToInt64(reward3[i]);
                            if (itemid > 0)
                            {
                                if (string.IsNullOrEmpty(reward3level[i]))
                                    reward3level[i] = "0";
                                if (string.IsNullOrEmpty(reward3grade[i]))
                                    reward3grade[i] = "0";
                                short itemlevel = System.Convert.ToInt16(reward3level[i]);
                                short itemgrade = System.Convert.ToInt16(reward3grade[i]);
                                if (itemgrade > 0)
                                {
                                    System_Package_RewardBox item = new System_Package_RewardBox();
                                    item.Item_ID = itemid;
                                    item.Item_Num = 1;
                                    item.ItemIndex = (byte)itemIndex;
                                    item.Item_TargetType = "Item";
                                    item.Item_Level = (byte)itemlevel;
                                    item.Item_Grade = (byte)itemgrade;
                                    rewardList3.Add(item);
                                    itemIndex++;
                                }
                            }
                        }

                        string[] reward4 = System.Text.RegularExpressions.Regex.Split(reqReward4, ",");
                        string[] reward4level = System.Text.RegularExpressions.Regex.Split(reqReward4level, ",");
                        string[] reward4grade = System.Text.RegularExpressions.Regex.Split(reqReward4grade, ",");
                        for (int i = 0; i < reward4.Length; i++)
                        {
                            long itemid = System.Convert.ToInt64(reward4[i]);
                            if (itemid > 0)
                            {
                                if (string.IsNullOrEmpty(reward4level[i]))
                                    reward4level[i] = "0";
                                if (string.IsNullOrEmpty(reward4grade[i]))
                                    reward4grade[i] = "0";
                                short itemlevel = System.Convert.ToInt16(reward4level[i]);
                                short itemgrade = System.Convert.ToInt16(reward4grade[i]);
                                if (itemgrade > 0)
                                {
                                    System_Package_RewardBox item = new System_Package_RewardBox();
                                    item.Item_ID = itemid;
                                    item.Item_Num = 1;
                                    item.ItemIndex = (byte)itemIndex;
                                    item.Item_TargetType = "Item";
                                    item.Item_Level = (byte)itemlevel;
                                    item.Item_Grade = (byte)itemgrade;
                                    rewardList4.Add(item);
                                    itemIndex++;
                                }
                            }
                        }
                        if (reqidx > 0)
                        {
                            retError = GMDataManager.UpdatePackageList(ref TxnBlackServer, startDate, endDate, packageItem, rewardList1, rewardList2, rewardList3, rewardList4, billingList);
                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, reqidx, "", GMResult_Define.ControlType.PACKAGE_EDIT, queryFetcher.GetReqParams(), reqServer);
                        }
                        else
                        {
                            retError = GMDataManager.InsertPackageList(ref TxnBlackServer, startDate, endDate, packageItem, rewardList1, rewardList2, rewardList3, rewardList4, billingList);
                            if (retError == Result_Define.eResult.SUCCESS)
                                retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, 0, "", GMResult_Define.ControlType.PACKAGE_ADD, queryFetcher.GetReqParams(), reqServer);
                        }
                        retJson = queryFetcher.GM_Render(retError);
                        if (retError == Result_Define.eResult.SUCCESS)
                            result = true;
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
                Response.Redirect("pagckList.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID);
        }

        protected void packageView(ref TxnBlock TB, long idx)
        {
            GM_System_Package_List dataInfo = GMDataManager.GetGM_Package_Data(ref TB, idx);
            List<System_Package_RewardBox> rewardInfo1 = ShopManager.GetShop_System_Package_RewardBox(ref TB, dataInfo.Reward_Box1ID, true);
            List<System_Package_RewardBox> rewardInfo2 = ShopManager.GetShop_System_Package_RewardBox(ref TB, dataInfo.Reward_Box2ID, true);
            List<System_Package_RewardBox> rewardInfo3 = ShopManager.GetShop_System_Package_RewardBox(ref TB, dataInfo.Reward_Box3ID, true);
            List<System_Package_RewardBox> rewardInfo4 = ShopManager.GetShop_System_Package_RewardBox(ref TB, dataInfo.Reward_Box4ID, true);

            string billing = "";
            int billingCount = 0;
            List<GM_Number> checkbilling = GMDataManager.GetShopGoodsCodeList(ref TB, dataInfo.Package_ID);
            foreach (var item in Enum.GetValues(typeof(Shop_Define.eBillingType)))
            {
                string isCheck = checkbilling.Find(checkItem => checkItem.number == (int)item) == null ? "" : "checked";
                billing = billing + string.Format("<input type=\"checkbox\" name=\"billing\" value=\"{0}\" {3} /> {1} {2}", (int)item, Enum.GetName(typeof(Shop_Define.eBillingType), item), (billingCount % 5) == 0 ? "<br />" : "", isCheck);
                billingCount++;
            }
            span_billing.InnerHtml = billing;

            ListItem selectItem = new ListItem("select", "-1");
            packageName.Text = dataInfo.NameCN1;
            if (dataInfo.Buy_PriceType == "PriceType_PayReal")
                payType_1.Checked = true;
            else
                payType_0.Checked = true;
            String csname = "payTypeScript";
            Type cstype = this.GetType();
            ClientScriptManager cs = Page.ClientScript;
            string scriptString = "<script type='text/javascript'>payTypeChage(\"" + dataInfo.Buy_PriceType + "\");</script>";
            cs.RegisterStartupScript(cstype, csname, scriptString, false);
            
            payValue.Text = dataInfo.Buy_PriceValue.ToString();
            if (dataInfo.Buy_PriceType == "PriceType_PayReal")
            {
                payValue2.SelectedValue = dataInfo.Buy_PriceValue > (int)GMData_Define.eShopProduct.darkblaze_price_1100 ? dataInfo.Buy_PriceValue.ToString() : GMDataManager.GetSystemProductBYRealPrice(ref TB, dataInfo.Buy_PriceValue).PriceValue.ToString();
            }
            loopType.SelectedValue = dataInfo.LoopType.ToString();
            vipLevel.SelectedValue = dataInfo.VIP_Level > GMDataManager.GetSystemVIPLevelList(ref TB).Max(Item => Item.VIP_Level) ? "0" : dataInfo.VIP_Level.ToString();
            vipPoint.Text = dataInfo.VIP_Point.ToString();
            grade.SelectedValue = dataInfo.Grade.ToString();
            active.SelectedValue = dataInfo.ActiveType.ToString();
            descCN.Text = dataInfo.ToolTipCN;
            detailCN.Text = dataInfo.DetailCN;
            startDay.Text = dataInfo.SaleStartTime.ToString("yyyy-MM-dd");
            startHour.SelectedValue = dataInfo.SaleStartTime.ToString("HH");
            startMin.SelectedValue = dataInfo.SaleStartTime.ToString("mm");
            endDay.Text = dataInfo.SaleEndTime.ToString("yyyy-MM-dd");
            endHour.SelectedValue = dataInfo.SaleEndTime.ToString("HH");
            endMin.SelectedValue = dataInfo.SaleEndTime.ToString("mm");
            maxCnt.Text = dataInfo.Max_Buy.ToString();
            if (rewardInfo1.Count > 0)
            {
                //Response.Write(rewardInfo1.Count);
                //for (int i = 0; i > rewardInfo1.Count - 1; i++)
                //{
                //    string tableAdd = "tableAdd('allitem');";
                //    Page.ClientScript.RegisterStartupScript(GetType(), "tableAdd", tableAdd, true);
                //}
                foreach (System_Package_RewardBox item in rewardInfo1)
                {
                    if (item.ItemIndex == 1)
                    {
                        all_item.SelectedIndex = all_item.Items.IndexOf(all_item.Items.FindByValue(item.Item_ID.ToString()));
                        all_itemgrade.Value = item.Item_Grade.ToString();
                        all_itemcnt.Value = item.Item_Num.ToString();
                    }
                    else
                    {
                        HtmlTableRow addtr = new HtmlTableRow();
                        HtmlTableCell addtd1 = new HtmlTableCell();
                        HtmlTableCell addtd2 = new HtmlTableCell();
                        HtmlTableCell addtd3 = new HtmlTableCell();
                        HtmlSelect itemlist = new HtmlSelect();
                        HtmlInputText itemCount = new HtmlInputText();
                        HtmlInputText itemGrade = new HtmlInputText();
                        itemCount.ID = "all_itemcnt";
                        itemCount.Value = item.Item_Num.ToString();
                        itemGrade.ID = "all_itemgrade";
                        itemGrade.Value = item.Item_Grade.ToString();
                        itemlist.ID = "all_item";
                        itemlist.DataSource = all_item.DataSource;
                        itemlist.DataTextField = "Description";
                        itemlist.DataValueField = "Item_IndexID";
                        itemlist.DataBind();
                        itemlist.Items.Insert(0, selectItem);
                        itemlist.SelectedIndex = itemlist.Items.IndexOf(itemlist.Items.FindByValue(item.Item_ID.ToString()));
                        addtd1.Controls.Add(itemlist);
                        addtd2.Controls.Add(itemGrade);
                        addtd3.Controls.Add(itemCount);
                        addtr.Cells.Add(addtd1);
                        addtr.Cells.Add(addtd2);
                        addtr.Cells.Add(addtd3);
                        allitem.Rows.Add(addtr);
                    }
                }
            }

            if (rewardInfo2.Count > 0)
            {
                foreach (System_Package_RewardBox item in rewardInfo2)
                {
                    if (item.ItemIndex == 1)
                    {
                        warrior_item.SelectedIndex = warrior_item.Items.IndexOf(warrior_item.Items.FindByValue(item.Item_ID.ToString()));
                        warriorgrade.Value = item.Item_Grade.ToString();
                        warriorlevel.Value = item.Item_Level.ToString();
                    }
                    else
                    {
                        HtmlTableRow addtr = new HtmlTableRow();
                        HtmlTableCell addtd1 = new HtmlTableCell();
                        HtmlTableCell addtd2 = new HtmlTableCell();
                        HtmlTableCell addtd3 = new HtmlTableCell();
                        HtmlSelect itemlist = new HtmlSelect();
                        HtmlInputText itemlevel = new HtmlInputText();
                        HtmlInputText itemgrade = new HtmlInputText();
                        itemlevel.ID = "warriorlevel";
                        itemlevel.Value = item.Item_Level.ToString();
                        itemgrade.ID = "warriorgrade";
                        itemgrade.Value = item.Item_Grade.ToString();
                        itemlist.ID = "warrior_item";
                        itemlist.DataSource = warrior_item.DataSource;
                        itemlist.DataTextField = "Description";
                        itemlist.DataValueField = "Item_IndexID";
                        itemlist.DataBind();
                        itemlist.Items.Insert(0, selectItem);
                        itemlist.SelectedIndex = itemlist.Items.IndexOf(itemlist.Items.FindByValue(item.Item_ID.ToString()));
                        addtd1.Controls.Add(itemlist);
                        addtd2.Controls.Add(itemlevel);
                        addtd3.Controls.Add(itemgrade);
                        addtr.Cells.Add(addtd1);
                        addtr.Cells.Add(addtd2);
                        addtr.Cells.Add(addtd3);
                        warrior_itemtable.Rows.Add(addtr);
                    }
                }
            }
            if (rewardInfo3.Count > 0)
            {
                foreach (System_Package_RewardBox item in rewardInfo3)
                {
                    if (item.ItemIndex == 1)
                    {
                        sword_item.SelectedIndex = sword_item.Items.IndexOf(sword_item.Items.FindByValue(item.Item_ID.ToString()));
                        swordgrade.Value = item.Item_Grade.ToString();
                        swordlevel.Value = item.Item_Level.ToString();
                    }
                    else
                    {
                        HtmlTableRow addtr = new HtmlTableRow();
                        HtmlTableCell addtd1 = new HtmlTableCell();
                        HtmlTableCell addtd2 = new HtmlTableCell();
                        HtmlTableCell addtd3 = new HtmlTableCell();
                        HtmlSelect itemlist = new HtmlSelect();
                        HtmlInputText itemlevel = new HtmlInputText();
                        HtmlInputText itemgrade = new HtmlInputText();
                        itemlevel.ID = "swordlevel";
                        itemlevel.Value = item.Item_Level.ToString();
                        itemgrade.ID = "swordgrade";
                        itemgrade.Value = item.Item_Grade.ToString();
                        itemlist.ID = "sword_item";
                        itemlist.DataSource = sword_item.DataSource;
                        itemlist.DataTextField = "Description";
                        itemlist.DataValueField = "Item_IndexID";
                        itemlist.DataBind();
                        itemlist.Items.Insert(0, selectItem);
                        itemlist.SelectedIndex = itemlist.Items.IndexOf(itemlist.Items.FindByValue(item.Item_ID.ToString()));
                        addtd1.Controls.Add(itemlist);
                        addtd2.Controls.Add(itemlevel);
                        addtd3.Controls.Add(itemgrade);
                        addtr.Cells.Add(addtd1);
                        addtr.Cells.Add(addtd2);
                        addtr.Cells.Add(addtd3);
                        sword_itemtable.Rows.Add(addtr);
                    }
                }
            }

            if (rewardInfo4.Count > 0)
            {
                foreach (System_Package_RewardBox item in rewardInfo4)
                {
                    if (item.ItemIndex == 1)
                    {
                        taoist_item.SelectedIndex = taoist_item.Items.IndexOf(taoist_item.Items.FindByValue(item.Item_ID.ToString()));
                        taoistgrade.Value = item.Item_Grade.ToString();
                        taoistlevel.Value = item.Item_Level.ToString();
                    }
                    else
                    {
                        HtmlTableRow addtr = new HtmlTableRow();
                        HtmlTableCell addtd1 = new HtmlTableCell();
                        HtmlTableCell addtd2 = new HtmlTableCell();
                        HtmlTableCell addtd3 = new HtmlTableCell();
                        HtmlSelect itemlist = new HtmlSelect();
                        HtmlInputText itemlevel = new HtmlInputText();
                        HtmlInputText itemgrade = new HtmlInputText();
                        itemlevel.ID = "taoistlevel";
                        itemlevel.Value = item.Item_Level.ToString();
                        itemgrade.ID = "taoistgrade";
                        itemgrade.Value = item.Item_Grade.ToString();
                        itemlist.ID = "taoist_item";
                        itemlist.DataSource = taoist_item.DataSource;
                        itemlist.DataTextField = "Description";
                        itemlist.DataValueField = "Item_IndexID";
                        itemlist.DataBind();
                        itemlist.Items.Insert(0, selectItem);
                        itemlist.SelectedIndex = itemlist.Items.IndexOf(itemlist.Items.FindByValue(item.Item_ID.ToString()));
                        addtd1.Controls.Add(itemlist);
                        addtd2.Controls.Add(itemlevel);
                        addtd3.Controls.Add(itemgrade);
                        addtr.Cells.Add(addtd1);
                        addtr.Cells.Add(addtd2);
                        addtr.Cells.Add(addtd3);
                        taoist_itemtable.Rows.Add(addtr);
                    }
                }
            }
        }

        protected void pageInit(ref TxnBlock TB)
        {
            string billing = "";
            int billingCount = 0;
            foreach (var item in Enum.GetValues(typeof(Shop_Define.eBillingType)))
            {
                billing = billing + string.Format("<input type=\"checkbox\" name=\"billing\" value=\"{0}\" /> {1} {2}", (int)item, Enum.GetName(typeof(Shop_Define.eBillingType), item), (billingCount % 5) == 0 ? "<br />" : "");
                billingCount++;
            }
            span_billing.InnerHtml = billing;

            payValue2.DataSource = (GMDataManager.GetGmToolLanguage() == "kr") ? Enum.GetNames(typeof(GMData_Define.eShopProduct)).Select(o => new { Text = (int)(Enum.Parse(typeof(GMData_Define.eShopProduct), o)), Value = (int)(Enum.Parse(typeof(GMData_Define.eShopProduct), o)) })
                                                                                : Enum.GetNames(typeof(GMData_Define.eChinaShopProduct)).Select(o => new { Text = (int)(Enum.Parse(typeof(GMData_Define.eChinaShopProduct), o)), Value = (int)(Enum.Parse(typeof(GMData_Define.eChinaShopProduct), o)) });
            payValue2.DataTextField = "Text";
            payValue2.DataValueField = "Value";
            payValue2.DataBind();

            List<System_VIP_Level> vipLevelList = GMDataManager.GetSystemVIPLevelList(ref TB);

            string[] rewadValues = Enum.GetNames(typeof(GMData_Define.eRewardTye));
            List<ListItem> rewardList = new List<ListItem>();
            foreach (string item in rewadValues)
            {
                long itemid = (long)Enum.Parse(typeof(GMData_Define.eRewardTye), item);
                rewardList.Add(new ListItem(item, itemid.ToString()));
            }

            payType_0.Checked = true;
            ListItem selectItem = new ListItem("select", "-1");
            vipLevel.DataSource = vipLevelList;
            vipLevel.DataTextField = "VIP_Level";
            vipLevel.DataValueField = "VIP_Level";
            vipLevel.DataBind();
            vipLevel.Items.Insert(0, selectItem);

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

            List<Admin_System_Item> allList = GMDataManager.GetNonEquip_Accessory_ItemList(ref TB);
            List<Admin_System_Item> warriorList = GMDataManager.GetEquipItemList(ref TB, "Warrior");
            List<Admin_System_Item> swordList = GMDataManager.GetEquipItemList(ref TB, "Swordmaster");
            List<Admin_System_Item> taoistList = GMDataManager.GetEquipItemList(ref TB, "Taoist");
            all_item.DataSource = allList;
            all_item.DataTextField = "Description";
            all_item.DataValueField = "Item_IndexID";
            all_item.DataBind();
            all_item.Items.Insert(0, selectItem);

            warrior_item.DataSource = warriorList;
            warrior_item.DataTextField = "Description";
            warrior_item.DataValueField = "Item_IndexID";
            warrior_item.DataBind();
            warrior_item.Items.Insert(0, selectItem);

            sword_item.DataSource = swordList;
            sword_item.DataTextField = "Description";
            sword_item.DataValueField = "Item_IndexID";
            sword_item.DataBind();
            sword_item.Items.Insert(0, selectItem);

            taoist_item.DataSource = taoistList;
            taoist_item.DataTextField = "Description";
            taoist_item.DataValueField = "Item_IndexID";
            taoist_item.DataBind();
            taoist_item.Items.Insert(0, selectItem);
        }
    }
}