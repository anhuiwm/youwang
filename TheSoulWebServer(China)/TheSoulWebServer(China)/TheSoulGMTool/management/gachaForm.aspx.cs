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


namespace TheSoulGMTool.management
{
    public partial class gachaForm : System.Web.UI.Page
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
                string serverlist = GMDataManager.GetServerCheckList(ref tb, serverID);
                change_server.InnerHtml = serverlist;
                idx.Value = reqidx.ToString();

                Result_Define.eResult retError = Result_Define.eResult.DB_ERROR;
                if (!Page.IsPostBack)
                    pageInit(ref tb, reqidx);
                else
                {
                    if (!string.IsNullOrEmpty(reqServer))
                    {
                        short reqMode = System.Convert.ToInt16(queryFetcher.QueryParam_Fetch(mode.UniqueID, "0"));
                        string sdate = queryFetcher.QueryParam_Fetch(startDay.UniqueID, DateTime.Today.ToString("yyyy-MM-dd"));
                        string edate = queryFetcher.QueryParam_Fetch(endDay.UniqueID, DateTime.Today.AddDays(1).ToString("yyyy-MM-dd"));
                        string shour = queryFetcher.QueryParam_Fetch(startHour.UniqueID, "00");
                        string ehour = queryFetcher.QueryParam_Fetch(endHour.UniqueID, "00");
                        string smin = queryFetcher.QueryParam_Fetch(startMin.UniqueID, "00");
                        string emin = queryFetcher.QueryParam_Fetch(endMin.UniqueID, "00");
                        long mainSoul = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(mainSoulID.UniqueID, "0"));
                        long subSoul1 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(subSoulID1.UniqueID, "0"));
                        long subSoul2 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(subSoulID2.UniqueID, "0"));
                        long subSoul3 = System.Convert.ToInt64(queryFetcher.QueryParam_Fetch(subSoulID3.UniqueID, "0"));
                        string rewardSoul = queryFetcher.QueryParam_Fetch(rewardID.UniqueID, "");
                        string rewardMax = queryFetcher.QueryParam_Fetch(maxCnt.UniqueID, "");
                        string rewardProb = queryFetcher.QueryParam_Fetch(prob.UniqueID, "");
                        int gachacash = System.Convert.ToInt32(queryFetcher.QueryParam_Fetch(gachaCash.UniqueID, "0"));

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
                        System_Gacha_Best setData = new System_Gacha_Best();
                        setData.GachaIndex = reqidx;
                        setData.StartDate = System.Convert.ToDateTime(startDate);
                        setData.EndDate = System.Convert.ToDateTime(endDate);
                        setData.Main_SoulItemID = mainSoul;
                        setData.Sub_SoulItemID_1 = subSoul1;
                        setData.Sub_SoulItemID_2 = subSoul2;
                        setData.Sub_SoulItemID_3 = subSoul3;
                        setData.Gacha_Cash = gachacash;

                        int itemIndex = 1;
                        List<System_Gacha_Best_DropGrop> rewardList = new List<System_Gacha_Best_DropGrop>();
                        string[] reward = System.Text.RegularExpressions.Regex.Split(rewardSoul, ",");
                        string[] rewardmax = System.Text.RegularExpressions.Regex.Split(rewardMax, ",");
                        string[] rewardprob = System.Text.RegularExpressions.Regex.Split(rewardProb, ",");

                        for (int i = 0; i < reward.Length; i++)
                        {
                            long itemid = System.Convert.ToInt64(reward[i]);
                            if (itemid > 0)
                            {
                                int itemMax = System.Convert.ToInt32(rewardmax[i]);
                                int itemProb = System.Convert.ToInt32(rewardprob[i]);
                                if (itemid > 0 && itemMax > 0)
                                {

                                    System_Gacha_Best_DropGrop item = new System_Gacha_Best_DropGrop();
                                    item.DropGroupID = reqidx;
                                    item.DropIndex = itemIndex;
                                    item.DropItemID = itemid;
                                    item.DropTargetType = "Item";
                                    item.DropItemLevel = 0;
                                    item.DropItemGrade = 1;
                                    item.DropMinNum = itemMax;
                                    item.DropMaxNum = itemMax;
                                    item.DropProb = itemProb;
                                    rewardList.Add(item);
                                    itemIndex++;
                                }
                            }
                        }
                        if (reqMode <= 1)
                        {
                            bool checkDate = true;
                            List<string> resultServer = GMDataManager.GetBestGachaMaxDateServerList(ref TxnBlackServer, reqidx, startDate);
                            if (resultServer.Count == 0)
                            {
                                resultServer = GMDataManager.GetBestGachaMaxDateServerList(ref TxnBlackServer, reqidx, endDate);
                                if (resultServer.Count > 0)
                                    checkDate = false;
                            }
                            else
                                checkDate = false;
                            if (checkDate)
                            {
                                if (reqidx > 0)
                                {
                                    retError = GMDataManager.UpdateBestGacha(ref TxnBlackServer, setData, rewardList);
                                    if (retError == Result_Define.eResult.SUCCESS)
                                        retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, reqidx, "", GMResult_Define.ControlType.BEST_GACHA_EDIT, queryFetcher.GetReqParams(), reqServer);
                                }
                                else
                                {
                                    retError = GMDataManager.InsertBestGacha(ref TxnBlackServer, setData, rewardList);
                                    if (retError == Result_Define.eResult.SUCCESS)
                                        retError = GMDataManager.InsertGMControlLog(ref tb, GMResult_Define.TargetType.GAME_SYSTEM, reqidx, "", GMResult_Define.ControlType.BLACKMARKET_ADD, queryFetcher.GetReqParams(), reqServer);
                                }
                                if (retError == Result_Define.eResult.SUCCESS)
                                    result = true;
                                retJson = queryFetcher.GM_Render("", retError);
                            }
                            else
                            {
                                string msg = "alert('" + Resources.languageResource.lang_msg_gachaDate + "');";
                                Page.ClientScript.RegisterStartupScript(GetType(), "alert", msg, true);
                                SetFrom(ref tb, setData, rewardList);
                            }
                        }
                        else
                        {
                            retError = GMDataManager.DeleteBestGacha(ref TxnBlackServer, reqidx);

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
                Response.Redirect("gachaList.aspx?ca2=" + queryFetcher.QueryParam_Fetch_Request("ca2", "1") + "&select_server=" + serverID);
        }

        protected void pageInit(ref TxnBlock TB, long idx)
        {
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

            ListItem selectItem = new ListItem("select", "-1");
            List<Admin_System_Item> allList = GMDataManager.GetItemClassItemList(ref TB, "Soul_Parts");
            mainSoulID.DataSource = allList;
            mainSoulID.DataTextField = "Description";
            mainSoulID.DataValueField = "Item_IndexID";
            mainSoulID.DataBind();
            mainSoulID.Items.Insert(0, selectItem);

            subSoulID1.DataSource = allList;
            subSoulID1.DataTextField = "Description";
            subSoulID1.DataValueField = "Item_IndexID";
            subSoulID1.DataBind();
            subSoulID1.Items.Insert(0, selectItem);

            subSoulID2.DataSource = allList;
            subSoulID2.DataTextField = "Description";
            subSoulID2.DataValueField = "Item_IndexID";
            subSoulID2.DataBind();
            subSoulID2.Items.Insert(0, selectItem);

            subSoulID3.DataSource = allList;
            subSoulID3.DataTextField = "Description";
            subSoulID3.DataValueField = "Item_IndexID";
            subSoulID3.DataBind();
            subSoulID3.Items.Insert(0, selectItem);

            rewardID.DataSource = allList;
            rewardID.DataTextField = "Description";
            rewardID.DataValueField = "Item_IndexID";
            rewardID.DataBind();
            rewardID.Items.Insert(0, selectItem);

            if (idx > 0)
            {
                System_Gacha_Best getData = GMDataManager.GetBestGachaInfo(ref TB, idx);
                List<System_Gacha_Best_DropGrop> itemList = GMDataManager.GetBestGachaDropGroupList(ref TB, idx);
                SetFrom(ref TB, getData, itemList);
            }
            else
            {
                int defaultCash = SystemData.GetConstValueInt(ref TB, "DEF_GACHA_SOUL_PRICE");
                gachaCash.Text = defaultCash.ToString();
            }
        }

        protected void SetFrom(ref TxnBlock TB, System_Gacha_Best getData, List<System_Gacha_Best_DropGrop> itemList)
        {
            ListItem selectItem = new ListItem("select", "-1");
            List<Admin_System_Item> allList = GMDataManager.GetItemClassItemList(ref TB, "Soul_Parts");

            startDay.Text = getData.StartDate.ToString("yyyy-MM-dd");
            startHour.SelectedValue = getData.StartDate.ToString("HH");
            startMin.SelectedValue = getData.StartDate.ToString("mm");
            endDay.Text = getData.EndDate.ToString("yyyy-MM-dd");
            endHour.SelectedValue = getData.EndDate.ToString("HH");
            endMin.SelectedValue = getData.EndDate.ToString("mm");
            mainSoulID.SelectedIndex = mainSoulID.Items.IndexOf(mainSoulID.Items.FindByValue(getData.Main_SoulItemID.ToString()));
            subSoulID1.SelectedIndex = subSoulID1.Items.IndexOf(subSoulID1.Items.FindByValue(getData.Sub_SoulItemID_1.ToString()));
            subSoulID2.SelectedIndex = subSoulID2.Items.IndexOf(subSoulID2.Items.FindByValue(getData.Sub_SoulItemID_2.ToString()));
            subSoulID3.SelectedIndex = subSoulID3.Items.IndexOf(subSoulID3.Items.FindByValue(getData.Sub_SoulItemID_3.ToString()));
            gachaCash.Text = getData.Gacha_Cash.ToString();

            if (itemList.Count > 0)
            {
                foreach (System_Gacha_Best_DropGrop item in itemList)
                {
                    if (item.DropIndex == 1 || item.DropItemID == itemList.First().DropItemID)
                    {
                        rewardID.SelectedIndex = rewardID.Items.IndexOf(rewardID.Items.FindByValue(item.DropItemID.ToString()));
                        maxCnt.Text = item.DropMaxNum.ToString();
                        prob.Text = item.DropProb.ToString();
                    }
                    else
                    {
                        HtmlTableRow addtr = new HtmlTableRow();
                        HtmlTableCell addtd1 = new HtmlTableCell();
                        HtmlTableCell addtd3 = new HtmlTableCell();
                        HtmlTableCell addtd4 = new HtmlTableCell();
                        HtmlSelect itemlist = new HtmlSelect();
                        HtmlInputText itemMax = new HtmlInputText();
                        HtmlInputText itemProb = new HtmlInputText();
                        itemMax.ID = "maxCnt";
                        itemMax.Value = item.DropMaxNum.ToString();
                        itemProb.ID = "prob";
                        itemProb.Value = item.DropProb.ToString();
                        itemlist.ID = "rewardID";
                        itemlist.DataSource = allList;
                        itemlist.DataTextField = "Description";
                        itemlist.DataValueField = "Item_IndexID";
                        itemlist.DataBind();
                        itemlist.Items.Insert(0, selectItem);
                        itemlist.SelectedIndex = itemlist.Items.IndexOf(itemlist.Items.FindByValue(item.DropItemID.ToString()));
                        addtd1.Controls.Add(itemlist);
                        addtd3.Controls.Add(itemMax);
                        addtd4.Controls.Add(itemProb);
                        addtr.Cells.Add(addtd1);
                        addtr.Cells.Add(addtd3);
                        addtr.Cells.Add(addtd4);
                        rewardTable.Rows.Add(addtr);
                    }
                }
            }
        }
    }
}